using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/GUI Manager")]
[ExecuteInEditMode]
[RequireComponent(typeof(dfInputManager))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[Serializable]
public class dfGUIManager : MonoBehaviour
{
	[SerializeField]
	protected float uiScale = 1f;

	[SerializeField]
	protected dfInputManager inputManager;

	[SerializeField]
	protected int fixedWidth = -1;

	[SerializeField]
	protected int fixedHeight = 600;

	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected dfFont defaultFont;

	[SerializeField]
	protected bool mergeMaterials;

	[SerializeField]
	protected bool pixelPerfectMode = true;

	[SerializeField]
	protected Camera renderCamera;

	[SerializeField]
	protected bool generateNormals;

	[SerializeField]
	protected bool consumeMouseEvents = true;

	[SerializeField]
	protected bool overrideCamera;

	[SerializeField]
	protected int renderQueueBase = 3000;

	private static dfControl activeControl;

	private static Stack<dfGUIManager.ModalControlReference> modalControlStack;

	private dfGUICamera guiCamera;

	private Mesh[] renderMesh;

	private MeshFilter renderFilter;

	private MeshRenderer meshRenderer;

	private int activeRenderMesh;

	private int cachedChildCount;

	private bool isDirty;

	private Vector2 cachedScreenSize;

	private Vector3[] corners = new Vector3[4];

	private dfList<Rect> occluders = new dfList<Rect>(256);

	private Stack<dfGUIManager.ClipRegion> clipStack = new Stack<dfGUIManager.ClipRegion>();

	private static dfRenderData masterBuffer;

	private dfList<dfRenderData> drawCallBuffers = new dfList<dfRenderData>();

	private List<int> submeshes = new List<int>();

	private int drawCallCount;

	private Vector2 uiOffset = Vector2.zero;

	private bool? applyHalfPixelOffset;

	public static dfControl ActiveControl
	{
		get
		{
			return dfGUIManager.activeControl;
		}
	}

	public bool ConsumeMouseEvents
	{
		get
		{
			return this.consumeMouseEvents;
		}
		set
		{
			this.consumeMouseEvents = value;
		}
	}

	public int ControlsRendered
	{
		get;
		private set;
	}

	public dfAtlas DefaultAtlas
	{
		get
		{
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.invalidateAllControls();
			}
		}
	}

	public dfFont DefaultFont
	{
		get
		{
			return this.defaultFont;
		}
		set
		{
			if (value != this.defaultFont)
			{
				this.defaultFont = value;
				this.invalidateAllControls();
			}
		}
	}

	public int FixedHeight
	{
		get
		{
			return this.fixedHeight;
		}
		set
		{
			if (value != this.fixedHeight)
			{
				int num = this.fixedHeight;
				this.fixedHeight = value;
				this.onResolutionChanged(num, value);
			}
		}
	}

	public int FixedWidth
	{
		get
		{
			return this.fixedWidth;
		}
		set
		{
			if (value != this.fixedWidth)
			{
				this.fixedWidth = value;
				this.onResolutionChanged();
			}
		}
	}

	public int FramesRendered
	{
		get;
		private set;
	}

	public bool GenerateNormals
	{
		get
		{
			return this.generateNormals;
		}
		set
		{
			if (value != this.generateNormals)
			{
				this.generateNormals = value;
				if (this.renderMesh != null)
				{
					this.renderMesh[0].Clear();
					this.renderMesh[1].Clear();
				}
				dfRenderData.FlushObjectPool();
				this.invalidateAllControls();
			}
		}
	}

	public bool MergeMaterials
	{
		get
		{
			return this.mergeMaterials;
		}
		set
		{
			if (value != this.mergeMaterials)
			{
				this.mergeMaterials = value;
				this.invalidateAllControls();
			}
		}
	}

	public bool OverrideCamera
	{
		get
		{
			return this.overrideCamera;
		}
		set
		{
			this.overrideCamera = value;
		}
	}

	public bool PixelPerfectMode
	{
		get
		{
			return this.pixelPerfectMode;
		}
		set
		{
			if (value != this.pixelPerfectMode)
			{
				this.pixelPerfectMode = value;
				this.onResolutionChanged();
				this.Invalidate();
			}
		}
	}

	public Camera RenderCamera
	{
		get
		{
			return this.renderCamera;
		}
		set
		{
			if (!object.ReferenceEquals(this.renderCamera, value))
			{
				this.renderCamera = value;
				this.Invalidate();
				if (value != null && value.gameObject.GetComponent<dfGUICamera>() == null)
				{
					value.gameObject.AddComponent<dfGUICamera>();
				}
				if (this.inputManager != null)
				{
					this.inputManager.RenderCamera = value;
				}
			}
		}
	}

	public int TotalDrawCalls
	{
		get;
		private set;
	}

	public int TotalTriangles
	{
		get;
		private set;
	}

	public Vector2 UIOffset
	{
		get
		{
			return this.uiOffset;
		}
		set
		{
			if (!object.Equals(this.uiOffset, value))
			{
				this.uiOffset = value;
				this.Invalidate();
			}
		}
	}

	public float UIScale
	{
		get
		{
			return this.uiScale;
		}
		set
		{
			if (!Mathf.Approximately(value, this.uiScale))
			{
				this.uiScale = value;
				this.onResolutionChanged();
			}
		}
	}

	static dfGUIManager()
	{
		dfGUIManager.activeControl = null;
		dfGUIManager.modalControlStack = new Stack<dfGUIManager.ModalControlReference>();
		dfGUIManager.masterBuffer = new dfRenderData(4096);
	}

	public dfGUIManager()
	{
	}

	public T AddControl<T>()
	where T : dfControl
	{
		return (T)this.AddControl(typeof(T));
	}

	public dfControl AddControl(Type type)
	{
		if (!typeof(dfControl).IsAssignableFrom(type))
		{
			throw new InvalidCastException();
		}
		GameObject gameObject = new GameObject(type.Name, new Type[] { type });
		gameObject.transform.parent = base.transform;
		gameObject.layer = base.gameObject.layer;
		dfControl component = gameObject.GetComponent(type) as dfControl;
		component.ZOrder = this.getMaxZOrder() + 1;
		return component;
	}

	public virtual void Awake()
	{
		dfRenderData.FlushObjectPool();
	}

	public void BringToFront(dfControl control)
	{
		if (control.Parent != null)
		{
			control = control.GetRootContainer();
		}
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			int num = 0;
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				dfControl item = topLevelControls[i];
				if (item != control)
				{
					int num1 = num;
					num = num1 + 1;
					item.ZOrder = num1;
				}
			}
			control.ZOrder = num;
			this.Invalidate();
		}
	}

	private dfRenderData compileMasterBuffer()
	{
		this.submeshes.Clear();
		dfGUIManager.masterBuffer.Clear();
		for (int i = 0; i < this.drawCallCount; i++)
		{
			this.submeshes.Add(dfGUIManager.masterBuffer.Triangles.Count);
			dfRenderData item = this.drawCallBuffers[i];
			if (this.generateNormals && item.Normals.Count == 0)
			{
				this.generateNormalsAndTangents(item);
			}
			dfGUIManager.masterBuffer.Merge(item, false);
		}
		dfGUIManager.masterBuffer.ApplyTransform(base.transform.worldToLocalMatrix);
		return dfGUIManager.masterBuffer;
	}

	public static bool ContainsFocus(dfControl control)
	{
		if (dfGUIManager.activeControl == control)
		{
			return true;
		}
		if (dfGUIManager.activeControl == null || control == null)
		{
			return false;
		}
		return dfGUIManager.activeControl.transform.IsChildOf(control.transform);
	}

	private dfGUICamera findCameraComponent()
	{
		if (this.guiCamera != null)
		{
			return this.guiCamera;
		}
		if (this.renderCamera == null)
		{
			return null;
		}
		this.guiCamera = this.renderCamera.GetComponent<dfGUICamera>();
		if (this.guiCamera == null)
		{
			this.guiCamera = this.renderCamera.gameObject.AddComponent<dfGUICamera>();
			this.guiCamera.transform.position = base.transform.position;
		}
		return this.guiCamera;
	}

	private dfRenderData findDrawCallBufferByMaterial(Material material)
	{
		for (int i = 0; i < this.drawCallCount; i++)
		{
			if (this.drawCallBuffers[i].Material == material)
			{
				return this.drawCallBuffers[i];
			}
		}
		return null;
	}

	private Material[] gatherMaterials()
	{
		int num = this.renderQueueBase;
		dfGUIManager.MaterialCache.Reset();
		int num1 = this.drawCallBuffers.Matching((dfRenderData buff) => (buff == null ? false : buff.Material != null));
		int num2 = 0;
		Material[] materialArray = new Material[num1];
		for (int i = 0; i < this.drawCallBuffers.Count; i++)
		{
			if (this.drawCallBuffers[i].Material != null)
			{
				Material material = dfGUIManager.MaterialCache.Lookup(this.drawCallBuffers[i].Material);
				int num3 = num;
				num = num3 + 1;
				material.renderQueue = num3;
				int num4 = num2;
				num2 = num4 + 1;
				materialArray[num4] = material;
			}
		}
		return materialArray;
	}

	private void generateNormalsAndTangents(dfRenderData buffer)
	{
		Vector3 vector3 = buffer.Transform.MultiplyVector(Vector3.back).normalized;
		Vector4 vector4 = buffer.Transform.MultiplyVector(Vector3.right).normalized;
		vector4.w = -1f;
		for (int i = 0; i < buffer.Vertices.Count; i++)
		{
			buffer.Normals.Add(vector3);
			buffer.Tangents.Add(vector4);
		}
	}

	public virtual Plane[] GetClippingPlanes()
	{
		Vector3[] corners = this.GetCorners();
		Vector3 vector3 = base.transform.TransformDirection(Vector3.right);
		Vector3 vector31 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector32 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector33 = base.transform.TransformDirection(Vector3.down);
		return new Plane[] { new Plane(vector3, corners[0]), new Plane(vector31, corners[1]), new Plane(vector32, corners[2]), new Plane(vector33, corners[0]) };
	}

	private Rect getControlOccluder(dfControl control)
	{
		Rect screenRect = control.GetScreenRect();
		float hotZoneScale = screenRect.width * control.HotZoneScale.x;
		float single = screenRect.height;
		Vector2 vector2 = control.HotZoneScale;
		Vector2 vector21 = new Vector2(hotZoneScale, single * vector2.y);
		Vector2 vector22 = new Vector2(vector21.x - screenRect.width, vector21.y - screenRect.height) * 0.5f;
		return new Rect(screenRect.x - vector22.x, screenRect.y - vector22.y, vector21.x, vector21.y);
	}

	public Vector3[] GetCorners()
	{
		float units = this.PixelsToUnits();
		Vector2 screenSize = this.GetScreenSize() * units;
		float single = screenSize.x;
		float single1 = screenSize.y;
		Vector3 vector3 = new Vector3(-single * 0.5f, single1 * 0.5f);
		Vector3 vector31 = vector3 + new Vector3(single, 0f);
		Vector3 vector32 = vector3 + new Vector3(0f, -single1);
		Vector3 vector33 = vector31 + new Vector3(0f, -single1);
		Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
		this.corners[0] = matrix4x4.MultiplyPoint(vector3);
		this.corners[1] = matrix4x4.MultiplyPoint(vector31);
		this.corners[2] = matrix4x4.MultiplyPoint(vector33);
		this.corners[3] = matrix4x4.MultiplyPoint(vector32);
		return this.corners;
	}

	private dfRenderData getDrawCallBuffer(Material material)
	{
		dfRenderData dfRenderDatum = null;
		if (this.MergeMaterials && material != null)
		{
			dfRenderDatum = this.findDrawCallBufferByMaterial(material);
			if (dfRenderDatum != null)
			{
				return dfRenderDatum;
			}
		}
		dfRenderDatum = dfRenderData.Obtain();
		dfRenderDatum.Material = material;
		this.drawCallBuffers.Add(dfRenderDatum);
		dfGUIManager _dfGUIManager = this;
		_dfGUIManager.drawCallCount = _dfGUIManager.drawCallCount + 1;
		return dfRenderDatum;
	}

	public dfRenderData GetDrawCallBuffer(int drawCallNumber)
	{
		return this.drawCallBuffers[drawCallNumber];
	}

	private int getMaxZOrder()
	{
		int num = -1;
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				num = Mathf.Max(num, topLevelControls[i].ZOrder);
			}
		}
		return num;
	}

	public static dfControl GetModalControl()
	{
		dfControl _dfControl;
		if (dfGUIManager.modalControlStack.Count <= 0)
		{
			_dfControl = null;
		}
		else
		{
			_dfControl = dfGUIManager.modalControlStack.Peek().control;
		}
		return _dfControl;
	}

	private Mesh getRenderMesh()
	{
		this.activeRenderMesh = (this.activeRenderMesh != 1 ? 1 : 0);
		return this.renderMesh[this.activeRenderMesh];
	}

	public Vector2 GetScreenSize()
	{
		Camera renderCamera = this.RenderCamera;
		if ((!Application.isPlaying ? true : renderCamera == null))
		{
			return new Vector2((float)this.FixedWidth, (float)this.FixedHeight);
		}
		float single = (!this.PixelPerfectMode ? renderCamera.pixelHeight / (float)this.fixedHeight * this.uiScale : 1f);
		return (new Vector2(renderCamera.pixelWidth, renderCamera.pixelHeight) / single).CeilToInt();
	}

	private dfList<dfControl> getTopLevelControls()
	{
		int num = base.transform.childCount;
		dfList<dfControl> dfControls = dfList<dfControl>.Obtain(num);
		for (int i = 0; i < num; i++)
		{
			dfControl component = base.transform.GetChild(i).GetComponent<dfControl>();
			if (component != null)
			{
				dfControls.Add(component);
			}
		}
		dfControls.Sort();
		return dfControls;
	}

	public static bool HasFocus(dfControl control)
	{
		if (control == null)
		{
			return false;
		}
		return dfGUIManager.activeControl == control;
	}

	public dfControl HitTest(Vector2 screenPosition)
	{
		Ray ray = this.renderCamera.ScreenPointToRay(screenPosition);
		float single = this.renderCamera.farClipPlane - this.renderCamera.nearClipPlane;
		RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, single, this.renderCamera.cullingMask);
		Array.Sort<RaycastHit>(raycastHitArray, new Comparison<RaycastHit>(dfInputManager.raycastHitSorter));
		return this.inputManager.clipCast(raycastHitArray);
	}

	private void initialize()
	{
		if (this.renderCamera == null)
		{
			Debug.LogError("No camera is assigned to the GUIManager");
			return;
		}
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.hideFlags = HideFlags.HideInInspector;
		this.renderFilter = base.GetComponent<MeshFilter>();
		this.renderFilter.hideFlags = HideFlags.HideInInspector;
		Mesh[] meshArray = new Mesh[2];
		Mesh mesh = new Mesh()
		{
			hideFlags = HideFlags.DontSave
		};
		meshArray[0] = mesh;
		mesh = new Mesh()
		{
			hideFlags = HideFlags.DontSave
		};
		meshArray[1] = mesh;
		this.renderMesh = meshArray;
		this.renderMesh[0].MarkDynamic();
		this.renderMesh[1].MarkDynamic();
		if (this.fixedWidth < 0)
		{
			this.fixedWidth = Mathf.RoundToInt((float)this.fixedHeight * 1.33333f);
			base.GetComponentsInChildren<dfControl>().ToList<dfControl>().ForEach((dfControl x) => x.ResetLayout(false, false));
		}
	}

	public void Invalidate()
	{
		if (this.isDirty)
		{
			return;
		}
		this.isDirty = true;
		this.updateRenderSettings();
	}

	private void invalidateAllControls()
	{
		dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Invalidate();
		}
		this.updateRenderOrder(null);
	}

	public virtual void LateUpdate()
	{
		if (this.renderMesh == null || (int)this.renderMesh.Length == 0)
		{
			this.initialize();
		}
		if (!Application.isPlaying)
		{
			BoxCollider boxCollider = base.collider as BoxCollider;
			if (boxCollider != null)
			{
				Vector2 screenSize = this.GetScreenSize() * this.PixelsToUnits();
				boxCollider.center = Vector3.zero;
				boxCollider.size = screenSize;
			}
		}
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Render();
		}
	}

	private bool needHalfPixelOffset()
	{
		if (this.applyHalfPixelOffset.HasValue)
		{
			return this.applyHalfPixelOffset.Value;
		}
		RuntimePlatform runtimePlatform = Application.platform;
		bool flag = (!this.pixelPerfectMode || runtimePlatform != RuntimePlatform.WindowsPlayer && runtimePlatform != RuntimePlatform.WindowsWebPlayer && runtimePlatform != RuntimePlatform.WindowsEditor ? false : SystemInfo.graphicsShaderLevel < 40);
		this.applyHalfPixelOffset = new bool?((Application.isEditor ? true : flag));
		return flag;
	}

	public virtual void OnDestroy()
	{
		if (this.meshRenderer != null)
		{
			this.renderFilter.sharedMesh = null;
			UnityEngine.Object.DestroyImmediate(this.renderMesh[0]);
			UnityEngine.Object.DestroyImmediate(this.renderMesh[1]);
			this.renderMesh = null;
		}
	}

	public virtual void OnDisable()
	{
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = false;
		}
	}

	public virtual void OnEnable()
	{
		this.FramesRendered = 0;
		this.TotalDrawCalls = 0;
		this.TotalTriangles = 0;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = true;
		}
		if (Application.isPlaying)
		{
			this.onResolutionChanged();
		}
	}

	public void OnGUI()
	{
		if (this.overrideCamera || !this.consumeMouseEvents || !Application.isPlaying || this.occluders == null)
		{
			return;
		}
		Vector3 vector3 = Input.mousePosition;
		vector3.y = (float)Screen.height - vector3.y;
		if (dfGUIManager.modalControlStack.Count > 0)
		{
			GUI.Box(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), GUIContent.none, GUIStyle.none);
		}
		int num = 0;
		while (num < this.occluders.Count)
		{
			if (!this.occluders[num].Contains(vector3))
			{
				num++;
			}
			else
			{
				GUI.Box(this.occluders[num], GUIContent.none, GUIStyle.none);
				break;
			}
		}
		if (Event.current.isMouse && Input.touchCount > 0)
		{
			int num1 = Input.touchCount;
			for (int i = 0; i < num1; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					Vector2 vector2 = touch.position;
					vector2.y = (float)Screen.height - vector2.y;
					if (this.occluders.Any((Rect x) => x.Contains(vector2)))
					{
						Event.current.Use();
						break;
					}
				}
			}
		}
	}

	private void onResolutionChanged()
	{
		int num;
		num = (!Application.isPlaying ? this.FixedHeight : (int)this.renderCamera.pixelHeight);
		this.onResolutionChanged(this.FixedHeight, num);
	}

	private void onResolutionChanged(int oldSize, int currentSize)
	{
		float renderCamera = this.RenderCamera.aspect;
		float single = (float)oldSize * renderCamera;
		float single1 = (float)currentSize * renderCamera;
		Vector2 vector2 = new Vector2(single, (float)oldSize);
		this.onResolutionChanged(vector2, new Vector2(single1, (float)currentSize));
	}

	private void onResolutionChanged(Vector2 oldSize, Vector2 currentSize)
	{
		this.cachedScreenSize = currentSize;
		this.applyHalfPixelOffset = null;
		float renderCamera = this.RenderCamera.aspect;
		float single = oldSize.y * renderCamera;
		float single1 = currentSize.y * renderCamera;
		Vector2 vector2 = new Vector2(single, oldSize.y);
		Vector2 vector21 = new Vector2(single1, currentSize.y);
		dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
		Array.Sort<dfControl>(componentsInChildren, new Comparison<dfControl>(this.renderSortFunc));
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			if (this.pixelPerfectMode && componentsInChildren[i].Parent == null)
			{
				componentsInChildren[i].MakePixelPerfect(true);
			}
			componentsInChildren[i].OnResolutionChanged(vector2, vector21);
		}
		for (int j = 0; j < (int)componentsInChildren.Length; j++)
		{
			componentsInChildren[j].PerformLayout();
		}
		for (int k = 0; k < (int)componentsInChildren.Length && this.pixelPerfectMode; k++)
		{
			if (componentsInChildren[k].Parent == null)
			{
				componentsInChildren[k].MakePixelPerfect(true);
			}
		}
	}

	public float PixelsToUnits()
	{
		return 2f / (float)this.FixedHeight * this.UIScale;
	}

	public static void PopModal()
	{
		if (dfGUIManager.modalControlStack.Count == 0)
		{
			throw new InvalidOperationException("Modal stack is empty");
		}
		dfGUIManager.ModalControlReference modalControlReference = dfGUIManager.modalControlStack.Pop();
		if (modalControlReference.callback != null)
		{
			modalControlReference.callback(modalControlReference.control);
		}
	}

	private bool processRenderData(ref dfRenderData buffer, dfRenderData controlData, Bounds bounds, uint checksum, dfGUIManager.ClipRegion clipInfo)
	{
		bool flag;
		if (buffer == null)
		{
			flag = true;
		}
		else if (!controlData.IsValid())
		{
			flag = false;
		}
		else if (!object.Equals(buffer.Shader, controlData.Shader))
		{
			flag = true;
		}
		else
		{
			flag = (controlData.Material == null ? false : !controlData.Material.Equals(buffer.Material));
		}
		if (flag && controlData.IsValid())
		{
			buffer = this.getDrawCallBuffer(controlData.Material);
		}
		if (controlData != null && controlData.IsValid() && clipInfo.PerformClipping(buffer, bounds, checksum, controlData))
		{
			return true;
		}
		return false;
	}

	public static void PushModal(dfControl control, dfGUIManager.ModalPoppedCallback callback = null)
	{
		if (control == null)
		{
			throw new NullReferenceException("Cannot call PushModal() with a null reference");
		}
		Stack<dfGUIManager.ModalControlReference> modalControlReferences = dfGUIManager.modalControlStack;
		dfGUIManager.ModalControlReference modalControlReference = new dfGUIManager.ModalControlReference()
		{
			control = control,
			callback = callback
		};
		modalControlReferences.Push(modalControlReference);
	}

	public static void RefreshAll(bool force = false)
	{
		dfGUIManager[] dfGUIManagerArray = UnityEngine.Object.FindObjectsOfType(typeof(dfGUIManager)) as dfGUIManager[];
		for (int i = 0; i < (int)dfGUIManagerArray.Length; i++)
		{
			dfGUIManagerArray[i].invalidateAllControls();
			if (force || !Application.isPlaying)
			{
				dfGUIManagerArray[i].Render();
			}
		}
	}

	public void Render()
	{
		dfGUIManager framesRendered = this;
		framesRendered.FramesRendered = framesRendered.FramesRendered + 1;
		if (dfGUIManager.BeforeRender != null)
		{
			dfGUIManager.BeforeRender(this);
		}
		try
		{
			try
			{
				this.updateRenderSettings();
				this.ControlsRendered = 0;
				this.occluders.Clear();
				this.TotalDrawCalls = 0;
				this.TotalTriangles = 0;
				if (this.RenderCamera != null && base.enabled)
				{
					if (this.meshRenderer != null && !this.meshRenderer.enabled)
					{
						this.meshRenderer.enabled = true;
					}
					if (this.renderMesh == null || (int)this.renderMesh.Length == 0)
					{
						Debug.LogError("GUI Manager not initialized before Render() called");
					}
					else
					{
						this.resetDrawCalls();
						dfRenderData dfRenderDatum = null;
						this.clipStack.Clear();
						this.clipStack.Push(dfGUIManager.ClipRegion.Obtain());
						uint sTARTVALUE = dfChecksumUtil.START_VALUE;
						using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
						{
							this.updateRenderOrder(topLevelControls);
							for (int i = 0; i < topLevelControls.Count; i++)
							{
								dfControl item = topLevelControls[i];
								this.renderControl(ref dfRenderDatum, item, sTARTVALUE, 1f);
							}
						}
						this.drawCallBuffers.RemoveAll((dfRenderData x) => x.Vertices.Count == 0);
						this.drawCallCount = this.drawCallBuffers.Count;
						this.TotalDrawCalls = this.drawCallCount;
						if (this.drawCallBuffers.Count != 0)
						{
							this.meshRenderer.sharedMaterials = this.gatherMaterials();
							dfRenderData dfRenderDatum1 = this.compileMasterBuffer();
							this.TotalTriangles = dfRenderDatum1.Triangles.Count / 3;
							Mesh renderMesh = this.getRenderMesh();
							this.renderFilter.sharedMesh = renderMesh;
							Mesh items = renderMesh;
							items.Clear();
							items.vertices = dfRenderDatum1.Vertices.Items;
							items.uv = dfRenderDatum1.UV.Items;
							items.colors32 = dfRenderDatum1.Colors.Items;
							if (this.generateNormals && (int)dfRenderDatum1.Normals.Items.Length == (int)dfRenderDatum1.Vertices.Items.Length)
							{
								items.normals = dfRenderDatum1.Normals.Items;
								items.tangents = dfRenderDatum1.Tangents.Items;
							}
							items.subMeshCount = this.submeshes.Count;
							for (int j = 0; j < this.submeshes.Count; j++)
							{
								int num = this.submeshes[j];
								int count = dfRenderDatum1.Triangles.Count - num;
								if (j < this.submeshes.Count - 1)
								{
									count = this.submeshes[j + 1] - num;
								}
								int[] numArray = new int[count];
								dfRenderDatum1.Triangles.CopyTo(num, numArray, 0, count);
								items.SetTriangles(numArray, j);
							}
							if (this.clipStack.Count != 1)
							{
								Debug.LogError("Clip stack not properly maintained");
							}
							this.clipStack.Pop().Release();
							this.clipStack.Clear();
						}
						else if (this.renderFilter.sharedMesh != null)
						{
							this.renderFilter.sharedMesh.Clear();
						}
					}
				}
				else if (this.meshRenderer != null)
				{
					this.meshRenderer.enabled = false;
				}
			}
			catch (dfAbortRenderingException _dfAbortRenderingException)
			{
				this.isDirty = true;
			}
		}
		finally
		{
			if (dfGUIManager.AfterRender != null)
			{
				dfGUIManager.AfterRender(this);
			}
		}
	}

	private void renderControl(ref dfRenderData buffer, dfControl control, uint checksum, float opacity)
	{
		if (!control.GetIsVisibleRaw())
		{
			return;
		}
		float single = opacity * control.Opacity;
		if (opacity <= 0.005f)
		{
			return;
		}
		dfGUIManager.ClipRegion clipRegion = this.clipStack.Peek();
		checksum = dfChecksumUtil.Calculate(checksum, control.Version);
		Bounds bounds = control.GetBounds();
		bool flag = false;
		if (control is IDFMultiRender)
		{
			dfList<dfRenderData> dfRenderDatas = ((IDFMultiRender)control).RenderMultiple();
			if (dfRenderDatas != null)
			{
				for (int i = 0; i < dfRenderDatas.Count; i++)
				{
					if (this.processRenderData(ref buffer, dfRenderDatas[i], bounds, checksum, clipRegion))
					{
						flag = true;
					}
				}
			}
		}
		else
		{
			dfRenderData dfRenderDatum = control.Render();
			if (dfRenderDatum == null)
			{
				return;
			}
			if (this.processRenderData(ref buffer, dfRenderDatum, bounds, checksum, clipRegion))
			{
				flag = true;
			}
		}
		if (flag)
		{
			dfGUIManager controlsRendered = this;
			controlsRendered.ControlsRendered = controlsRendered.ControlsRendered + 1;
			this.occluders.Add(this.getControlOccluder(control));
		}
		if (control.ClipChildren)
		{
			clipRegion = dfGUIManager.ClipRegion.Obtain(clipRegion, control);
			this.clipStack.Push(clipRegion);
		}
		for (int j = 0; j < control.Controls.Count; j++)
		{
			dfControl item = control.Controls[j];
			this.renderControl(ref buffer, item, checksum, single);
		}
		if (control.ClipChildren)
		{
			this.clipStack.Pop().Release();
		}
	}

	private int renderSortFunc(dfControl lhs, dfControl rhs)
	{
		return lhs.RenderOrder.CompareTo(rhs.RenderOrder);
	}

	private void resetDrawCalls()
	{
		this.drawCallCount = 0;
		for (int i = 0; i < this.drawCallBuffers.Count; i++)
		{
			this.drawCallBuffers[i].Release();
		}
		this.drawCallBuffers.Clear();
	}

	public Vector2 ScreenToGui(Vector2 position)
	{
		Vector2 screenSize = this.GetScreenSize();
		position.y = screenSize.y - position.y;
		return position;
	}

	public void SendToBack(dfControl control)
	{
		if (control.Parent != null)
		{
			control = control.GetRootContainer();
		}
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			int num = 1;
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				dfControl item = topLevelControls[i];
				if (item != control)
				{
					int num1 = num;
					num = num1 + 1;
					item.ZOrder = num1;
				}
			}
			control.ZOrder = 0;
			this.Invalidate();
		}
	}

	public static void SetFocus(dfControl control)
	{
		if (dfGUIManager.activeControl == control)
		{
			return;
		}
		dfControl _dfControl = dfGUIManager.activeControl;
		dfGUIManager.activeControl = control;
		dfFocusEventArgs dfFocusEventArg = new dfFocusEventArgs(control, _dfControl);
		dfList<dfControl> dfControls = dfList<dfControl>.Obtain();
		if (_dfControl != null)
		{
			for (dfControl i = _dfControl; i != null; i = i.Parent)
			{
				dfControls.Add(i);
			}
		}
		dfList<dfControl> dfControls1 = dfList<dfControl>.Obtain();
		if (control != null)
		{
			for (dfControl j = control; j != null; j = j.Parent)
			{
				dfControls1.Add(j);
			}
		}
		if (_dfControl != null)
		{
			dfControls.ForEach((dfControl c) => {
				if (!dfControls1.Contains(c))
				{
					c.OnLeaveFocus(dfFocusEventArg);
				}
			});
			_dfControl.OnLostFocus(dfFocusEventArg);
		}
		if (control != null)
		{
			dfControls1.ForEach((dfControl c) => {
				if (!dfControls.Contains(c))
				{
					c.OnEnterFocus(dfFocusEventArg);
				}
			});
			control.OnGotFocus(dfFocusEventArg);
		}
		dfControls1.Release();
		dfControls.Release();
	}

	public virtual void Start()
	{
		Camera[] cameraArray = UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
		for (int i = 0; i < (int)cameraArray.Length; i++)
		{
			Camera camera = cameraArray[i];
			camera.eventMask = camera.eventMask & ~(1 << (base.gameObject.layer & 31 & 31));
		}
		this.inputManager = base.GetComponent<dfInputManager>() ?? base.gameObject.AddComponent<dfInputManager>();
		this.inputManager.RenderCamera = this.RenderCamera;
		this.FramesRendered = 0;
		this.invalidateAllControls();
		this.updateRenderOrder(null);
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = true;
		}
	}

	public virtual void Update()
	{
		if (this.renderCamera == null || !base.enabled)
		{
			if (this.meshRenderer != null)
			{
				this.meshRenderer.enabled = false;
			}
			return;
		}
		if (this.renderMesh == null || (int)this.renderMesh.Length == 0)
		{
			this.initialize();
			if (Application.isEditor && !Application.isPlaying)
			{
				this.Render();
			}
		}
		if (this.cachedChildCount != base.transform.childCount)
		{
			this.cachedChildCount = base.transform.childCount;
			this.Invalidate();
		}
		Vector2 screenSize = this.GetScreenSize();
		if ((screenSize - this.cachedScreenSize).sqrMagnitude > 1.401298E-45f)
		{
			this.onResolutionChanged(this.cachedScreenSize, screenSize);
			this.cachedScreenSize = screenSize;
		}
	}

	private void updateRenderCamera(Camera camera)
	{
		if (!Application.isPlaying || !(camera.targetTexture != null))
		{
			camera.clearFlags = CameraClearFlags.Depth;
		}
		else
		{
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.clear;
		}
		Vector3 vector3 = (!Application.isPlaying ? Vector3.zero : -this.uiOffset * this.PixelsToUnits());
		if (!camera.isOrthoGraphic)
		{
			float single = camera.fieldOfView * 0.0174532924f;
			Vector3[] corners = this.GetCorners();
			float single1 = Vector3.Distance(corners[3], corners[0]);
			float single2 = single1 / (2f * Mathf.Tan(single / 2f));
			Vector3 vector31 = base.transform.TransformDirection(Vector3.back) * single2;
			camera.farClipPlane = Mathf.Max(single2 * 2f, camera.farClipPlane);
			vector3 = vector3 + vector31;
		}
		else
		{
			camera.nearClipPlane = Mathf.Min(camera.nearClipPlane, -1f);
			camera.farClipPlane = Mathf.Max(camera.farClipPlane, 1f);
		}
		if (Application.isPlaying && this.needHalfPixelOffset())
		{
			float single3 = camera.pixelHeight;
			float fixedHeight = 2f / single3 * ((float)single3 / (float)this.FixedHeight);
			Vector3 vector32 = new Vector3(fixedHeight * 0.5f, fixedHeight * -0.5f, 0f);
			vector3 = vector3 + vector32;
		}
		if (!this.overrideCamera)
		{
			if (Vector3.SqrMagnitude(camera.transform.localPosition - vector3) > 1.401298E-45f)
			{
				camera.transform.localPosition = vector3;
			}
			camera.transform.hasChanged = false;
		}
	}

	private void updateRenderOrder(dfList<dfControl> list = null)
	{
		dfList<dfControl> dfControls = (list == null ? this.getTopLevelControls() : list);
		dfControls.Sort();
		int num = 0;
		for (int i = 0; i < dfControls.Count; i++)
		{
			dfControl item = dfControls[i];
			if (item.Parent == null)
			{
				item.setRenderOrder(ref num);
			}
		}
	}

	private void updateRenderSettings()
	{
		Camera renderCamera = this.RenderCamera;
		if (renderCamera == null)
		{
			return;
		}
		if (!this.overrideCamera)
		{
			this.updateRenderCamera(renderCamera);
		}
		if (base.transform.hasChanged)
		{
			Vector3 vector3 = base.transform.localScale;
			if ((vector3.x < 1.401298E-45f || !Mathf.Approximately(vector3.x, vector3.y) ? true : !Mathf.Approximately(vector3.x, vector3.z)))
			{
				float single = Mathf.Max(vector3.x, 0.001f);
				float single1 = single;
				vector3.x = single;
				float single2 = single1;
				single1 = single2;
				vector3.z = single2;
				vector3.y = single1;
				base.transform.localScale = vector3;
			}
		}
		if (!this.overrideCamera)
		{
			if (!Application.isPlaying || !this.PixelPerfectMode)
			{
				renderCamera.orthographicSize = 1f;
				renderCamera.fieldOfView = 60f;
			}
			else
			{
				float single3 = renderCamera.pixelHeight / (float)this.fixedHeight;
				renderCamera.orthographicSize = single3;
				renderCamera.fieldOfView = 60f * single3;
			}
		}
		renderCamera.transparencySortMode = TransparencySortMode.Orthographic;
		if (this.cachedScreenSize.sqrMagnitude <= 1.401298E-45f)
		{
			this.cachedScreenSize = new Vector2((float)this.FixedWidth, (float)this.FixedHeight);
		}
		base.transform.hasChanged = false;
	}

	public Vector2 WorldPointToGUI(Vector3 worldPoint)
	{
		Vector2 screenSize = this.GetScreenSize();
		Camera camera = Camera.main;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
		screenPoint.x = screenSize.x * (screenPoint.x / camera.pixelWidth);
		screenPoint.y = screenSize.y * (screenPoint.y / camera.pixelHeight);
		return this.ScreenToGui(screenPoint);
	}

	public static event dfGUIManager.RenderCallback AfterRender;

	public static event dfGUIManager.RenderCallback BeforeRender;

	private class ClipRegion
	{
		private static Queue<dfGUIManager.ClipRegion> pool;

		private dfList<Plane> planes;

		static ClipRegion()
		{
			dfGUIManager.ClipRegion.pool = new Queue<dfGUIManager.ClipRegion>();
		}

		private ClipRegion()
		{
			this.planes = new dfList<Plane>();
		}

		public void clipToPlanes(dfList<Plane> planes, dfRenderData data, dfRenderData dest, uint controlChecksum)
		{
			if (data == null || data.Vertices.Count == 0)
			{
				return;
			}
			if (planes != null && planes.Count != 0)
			{
				dfClippingUtil.Clip(planes, data, dest);
				return;
			}
			dest.Merge(data, true);
		}

		public static dfGUIManager.ClipRegion Obtain()
		{
			return (dfGUIManager.ClipRegion.pool.Count <= 0 ? new dfGUIManager.ClipRegion() : dfGUIManager.ClipRegion.pool.Dequeue());
		}

		public static dfGUIManager.ClipRegion Obtain(dfGUIManager.ClipRegion parent, dfControl control)
		{
			dfGUIManager.ClipRegion clipRegion = (dfGUIManager.ClipRegion.pool.Count <= 0 ? new dfGUIManager.ClipRegion() : dfGUIManager.ClipRegion.pool.Dequeue());
			clipRegion.planes.AddRange(control.GetClippingPlanes());
			if (parent != null)
			{
				clipRegion.planes.AddRange(parent.planes);
			}
			return clipRegion;
		}

		public bool PerformClipping(dfRenderData dest, Bounds bounds, uint checksum, dfRenderData controlData)
		{
			dfIntersectionType _dfIntersectionType;
			if (controlData.Checksum == checksum)
			{
				if (controlData.Intersection == dfIntersectionType.Inside)
				{
					dest.Merge(controlData, true);
					return true;
				}
				if (controlData.Intersection == dfIntersectionType.None)
				{
					return false;
				}
			}
			bool flag = false;
			using (dfList<Plane> planes = this.TestIntersection(bounds, out _dfIntersectionType))
			{
				if (_dfIntersectionType == dfIntersectionType.Inside)
				{
					dest.Merge(controlData, true);
					flag = true;
				}
				else if (_dfIntersectionType == dfIntersectionType.Intersecting)
				{
					this.clipToPlanes(planes, controlData, dest, checksum);
					flag = true;
				}
				controlData.Checksum = checksum;
				controlData.Intersection = _dfIntersectionType;
			}
			return flag;
		}

		public void Release()
		{
			this.planes.Clear();
			dfGUIManager.ClipRegion.pool.Enqueue(this);
		}

		private static int sortClipPlanes(Plane lhs, Plane rhs)
		{
			return lhs.distance.CompareTo(rhs.distance);
		}

		public dfList<Plane> TestIntersection(Bounds bounds, out dfIntersectionType type)
		{
			if (this.planes == null || this.planes.Count == 0)
			{
				type = dfIntersectionType.Inside;
				return null;
			}
			dfList<Plane> planes = dfList<Plane>.Obtain(this.planes.Count);
			Vector3 vector3 = bounds.center;
			Vector3 vector31 = bounds.extents;
			bool flag = false;
			for (int i = 0; i < this.planes.Count; i++)
			{
				Plane item = this.planes[i];
				Vector3 vector32 = item.normal;
				float single = item.distance;
				float single1 = vector31.x * Mathf.Abs(vector32.x) + vector31.y * Mathf.Abs(vector32.y) + vector31.z * Mathf.Abs(vector32.z);
				float single2 = Vector3.Dot(vector32, vector3) + single;
				if (Mathf.Abs(single2) <= single1)
				{
					flag = true;
					planes.Add(item);
				}
				else if (single2 < -single1)
				{
					type = dfIntersectionType.None;
					planes.Release();
					return null;
				}
			}
			if (flag)
			{
				type = dfIntersectionType.Intersecting;
				return planes;
			}
			type = dfIntersectionType.Inside;
			planes.Release();
			return null;
		}
	}

	private class MaterialCache
	{
		private static Dictionary<Material, dfGUIManager.MaterialCache.Cache> cache;

		static MaterialCache()
		{
			dfGUIManager.MaterialCache.cache = new Dictionary<Material, dfGUIManager.MaterialCache.Cache>();
		}

		public MaterialCache()
		{
		}

		public static Material Lookup(Material BaseMaterial)
		{
			if (BaseMaterial == null)
			{
				Debug.LogError("Cache lookup on null material");
				return null;
			}
			dfGUIManager.MaterialCache.Cache cache = null;
			if (dfGUIManager.MaterialCache.cache.TryGetValue(BaseMaterial, out cache))
			{
				return cache.Obtain();
			}
			dfGUIManager.MaterialCache.Cache cache1 = new dfGUIManager.MaterialCache.Cache(BaseMaterial);
			dfGUIManager.MaterialCache.cache[BaseMaterial] = cache1;
			cache = cache1;
			return cache.Obtain();
		}

		public static void Reset()
		{
			dfGUIManager.MaterialCache.Cache.ResetAll();
		}

		private class Cache
		{
			private static List<dfGUIManager.MaterialCache.Cache> cacheInstances;

			private Material baseMaterial;

			private List<Material> instances;

			private int currentIndex;

			static Cache()
			{
				dfGUIManager.MaterialCache.Cache.cacheInstances = new List<dfGUIManager.MaterialCache.Cache>();
			}

			private Cache()
			{
				throw new NotImplementedException();
			}

			public Cache(Material BaseMaterial)
			{
				this.baseMaterial = BaseMaterial;
				this.instances.Add(BaseMaterial);
				dfGUIManager.MaterialCache.Cache.cacheInstances.Add(this);
			}

			public Material Obtain()
			{
				if (this.currentIndex < this.instances.Count)
				{
					List<Material> materials = this.instances;
					dfGUIManager.MaterialCache.Cache cache = this;
					int num = cache.currentIndex;
					int num1 = num;
					cache.currentIndex = num + 1;
					return materials[num1];
				}
				dfGUIManager.MaterialCache.Cache cache1 = this;
				cache1.currentIndex = cache1.currentIndex + 1;
				Material material = new Material(this.baseMaterial)
				{
					hideFlags = HideFlags.DontSave,
					name = string.Format("{0} (Copy {1})", this.baseMaterial.name, this.currentIndex)
				};
				Material material1 = material;
				this.instances.Add(material1);
				return material1;
			}

			public void Reset()
			{
				this.currentIndex = 0;
			}

			public static void ResetAll()
			{
				for (int i = 0; i < dfGUIManager.MaterialCache.Cache.cacheInstances.Count; i++)
				{
					dfGUIManager.MaterialCache.Cache.cacheInstances[i].Reset();
				}
			}
		}
	}

	private struct ModalControlReference
	{
		public dfControl control;

		public dfGUIManager.ModalPoppedCallback callback;
	}

	[dfEventCategory("Modal Dialog")]
	public delegate void ModalPoppedCallback(dfControl control);

	[dfEventCategory("Global Callbacks")]
	public delegate void RenderCallback(dfGUIManager manager);
}