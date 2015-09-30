using NGUI.Meshing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Panel")]
[ExecuteInEditMode]
public class UIPanel : MonoBehaviour
{
	[SerializeField]
	private UIPanel _rootPanel;

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool depthPass;

	public bool widgetsAreStatic;

	[HideInInspector]
	[SerializeField]
	private UIPanel.DebugInfo mDebugInfo = UIPanel.DebugInfo.Gizmos;

	[HideInInspector]
	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = Vector4.zero;

	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	[HideInInspector]
	[SerializeField]
	private bool manualPanelUpdate;

	private OrderedDictionary mChildren = new OrderedDictionary();

	private List<UIWidget> mWidgets = new List<UIWidget>();

	private HashSet<UIMaterial> mChanged = new HashSet<UIMaterial>();

	private UIDrawCall mDrawCalls;

	private int mDrawCallCount;

	private MeshBuffer mCacheBuffer = new MeshBuffer();

	private HashSet<UIHotSpot> mHotSpots;

	private Transform mTrans;

	private Camera mCam;

	private int mLayer = -1;

	private bool mDepthChanged;

	private bool mRebuildAll;

	private bool mChangedLastFrame;

	private float mMatrixTime;

	private Matrix4x4 mWorldToLocal = Matrix4x4.identity;

	private static float[] mTemp;

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private List<Transform> mRemoved = new List<Transform>();

	private bool mCheckVisibility;

	private float mCullTime;

	private bool mCulled;

	private int globalIndex = -1;

	private static List<UINode> mHierarchy;

	private int traceID;

	private Ray lastRayTrace;

	private bool lastRayTraceInside;

	[NonSerialized]
	private UIPanelMaterialPropertyBlock _propertyBlock;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public bool changedLastFrame
	{
		get
		{
			return this.mChangedLastFrame;
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mCheckVisibility = true;
				this.mClipping = value;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (this.mClipRange != value)
			{
				this.mCullTime = (this.mCullTime != 0f ? Time.realtimeSinceStartup + 0.15f : 0.001f);
				this.mCheckVisibility = true;
				this.mClipRange = value;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
				this.UpdateDrawcalls();
			}
		}
	}

	public UIPanel.DebugInfo debugInfo
	{
		get
		{
			return this.mDebugInfo;
		}
		set
		{
			if (this.mDebugInfo != value)
			{
				this.mDebugInfo = value;
				UIDrawCall.Iterator next = (UIDrawCall.Iterator)this.mDrawCalls;
				HideFlags hideFlag = (this.mDebugInfo != UIPanel.DebugInfo.Geometry ? HideFlags.HideAndDontSave : HideFlags.DontSave | HideFlags.NotEditable);
				while (next.Has)
				{
					GameObject current = next.Current.gameObject;
					next = next.Next;
					current.SetActive(false);
					current.hideFlags = hideFlag;
					current.SetActive(true);
				}
			}
		}
	}

	public int drawCallCount
	{
		get
		{
			return this.mDrawCallCount;
		}
	}

	public UIDrawCall.Iterator drawCalls
	{
		get
		{
			return (UIDrawCall.Iterator)this.mDrawCalls;
		}
	}

	public bool manUp
	{
		get
		{
			return this.manualPanelUpdate;
		}
	}

	public UIPanelMaterialPropertyBlock propertyBlock
	{
		get
		{
			return this._propertyBlock;
		}
		set
		{
			this._propertyBlock = value;
		}
	}

	public UIPanel RootPanel
	{
		get
		{
			return (!this._rootPanel ? this : this._rootPanel);
		}
		set
		{
			if (value != this)
			{
				this._rootPanel = value;
			}
			else
			{
				this._rootPanel = null;
			}
		}
	}

	public List<UIWidget> widgets
	{
		get
		{
			return this.mWidgets;
		}
	}

	static UIPanel()
	{
		UIPanel.mTemp = new float[4];
		UIPanel.mHierarchy = new List<UINode>();
	}

	public UIPanel()
	{
	}

	private UINode AddTransform(Transform t)
	{
		UINode uINode = null;
		UINode item = null;
		while (t != null && t != this.cachedTransform)
		{
			if (!this.mChildren.Contains(t))
			{
				uINode = new UINode(t);
				if (item == null)
				{
					item = uINode;
				}
				this.mChildren.Add(t, uINode);
				t = t.parent;
			}
			else
			{
				if (item == null)
				{
					item = (UINode)this.mChildren[t];
				}
				break;
			}
		}
		return item;
	}

	public void AddWidget(UIWidget w)
	{
		if (w != null)
		{
			UINode uINode = this.AddTransform(w.cachedTransform);
			if (uINode == null)
			{
				Debug.LogError(string.Concat("Unable to find an appropriate UIRoot for ", NGUITools.GetHierarchy(w.gameObject), "\nPlease make sure that there is at least one game object above this widget!"), w.gameObject);
			}
			else
			{
				uINode.widget = w;
				if (!this.mWidgets.Contains(w))
				{
					this.mWidgets.Add(w);
					if (!this.mChanged.Contains(w.material))
					{
						this.mChanged.Add(w.material);
						this.mChangedLastFrame = true;
					}
					this.mDepthChanged = true;
				}
			}
		}
	}

	protected void Awake()
	{
		UIPanel.Global.RegisterPanel(this);
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		float single = this.clipRange.z * 0.5f;
		float single1 = this.clipRange.w * 0.5f;
		Vector2 vector2 = new Vector2(min.x, min.y);
		Vector2 vector21 = new Vector2(max.x, max.y);
		Vector4 vector4 = this.clipRange;
		Vector4 vector41 = this.clipRange;
		Vector2 vector22 = new Vector2(vector4.x - single, vector41.y - single1);
		Vector4 vector42 = this.clipRange;
		Vector4 vector43 = this.clipRange;
		Vector2 vector23 = new Vector2(vector42.x + single, vector43.y + single1);
		if (this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			vector22.x = vector22.x + this.clipSoftness.x;
			vector22.y = vector22.y + this.clipSoftness.y;
			vector23.x = vector23.x - this.clipSoftness.x;
			vector23.y = vector23.y - this.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(vector2, vector21, vector22, vector23);
	}

	private static bool CheckRayEnterClippingRect(Ray ray, Transform transform, Vector4 clipRange)
	{
		float single;
		Plane plane = new Plane(transform.forward, transform.position);
		if (!plane.Raycast(ray, out single))
		{
			return false;
		}
		Vector3 vector3 = transform.InverseTransformPoint(ray.GetPoint(single));
		clipRange.z = Mathf.Abs(clipRange.z);
		clipRange.w = Mathf.Abs(clipRange.w);
		Rect rect = new Rect(clipRange.x - clipRange.z / 2f, clipRange.y - clipRange.w / 2f, clipRange.z, clipRange.w);
		return rect.Contains(vector3);
	}

	public bool ConstrainTargetToBounds(Transform target, ref AABBox targetBounds, bool immediate)
	{
		Vector3 vector3 = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (vector3.magnitude <= 0f)
		{
			return false;
		}
		if (!immediate)
		{
			SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector3, 13f);
			springPosition.ignoreTimeScale = true;
			springPosition.worldSpace = false;
		}
		else
		{
			Transform transforms = target;
			transforms.localPosition = transforms.localPosition + vector3;
			targetBounds.center = targetBounds.center + vector3;
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		return true;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(this.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref aABBox, immediate);
	}

	public bool Contains(UIDrawCall drawcall)
	{
		for (UIDrawCall.Iterator i = (UIDrawCall.Iterator)this.mDrawCalls; i.Has; i = i.Next)
		{
			if (object.ReferenceEquals(drawcall, i.Current))
			{
				return true;
			}
		}
		return false;
	}

	private void DefaultLateUpdate()
	{
		if (this.manualPanelUpdate)
		{
			this.FillUpdate();
		}
		else
		{
			this.PanelUpdate(true);
		}
	}

	private void Delete(ref UIDrawCall.Iterator iter)
	{
		if (iter.Has)
		{
			UIDrawCall current = iter.Current;
			if (object.ReferenceEquals(current, this.mDrawCalls))
			{
				this.mDrawCalls = iter.Next.Current;
			}
			iter = iter.Next;
			current.LinkedList__Remove();
			UIPanel uIPanel = this;
			uIPanel.mDrawCallCount = uIPanel.mDrawCallCount - 1;
			NGUITools.DestroyImmediate(current.gameObject);
		}
	}

	private void Fill(UIMaterial mat)
	{
		int count = this.mWidgets.Count;
		while (count > 0)
		{
			int num = count - 1;
			count = num;
			if (this.mWidgets[num] != null)
			{
				continue;
			}
			this.mWidgets.RemoveAt(count);
		}
		int num1 = 0;
		int count1 = this.mWidgets.Count;
		while (num1 < count1)
		{
			UIWidget item = this.mWidgets[num1];
			if (item.visibleFlag == 1 && item.material == mat)
			{
				if (this.GetNode(item.cachedTransform) == null)
				{
					Debug.LogError(string.Concat("No transform found for ", NGUITools.GetHierarchy(item.gameObject)), this);
				}
				else
				{
					item.WriteToBuffers(this.mCacheBuffer);
				}
			}
			num1++;
		}
		if (this.mCacheBuffer.vSize <= 0)
		{
			UIDrawCall.Iterator drawCall = this.GetDrawCall(mat, false);
			if (drawCall.Has)
			{
				this.Delete(ref drawCall);
			}
		}
		else
		{
			UIDrawCall current = this.GetDrawCall(mat, true).Current;
			current.depthPass = this.depthPass;
			current.panelPropertyBlock = this.propertyBlock;
			current.Set(this.mCacheBuffer);
		}
		this.mCacheBuffer.Clear();
	}

	private void FillUpdate()
	{
		foreach (UIMaterial uIMaterial in this.mChanged)
		{
			this.Fill(uIMaterial);
		}
		this.UpdateDrawcalls();
		this.mChanged.Clear();
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform transforms = trans;
		UIPanel component = null;
		while (component == null && trans != null)
		{
			component = trans.GetComponent<UIPanel>();
			if (component != null)
			{
				break;
			}
			else if (trans.parent != null)
			{
				trans = trans.parent;
			}
			else
			{
				break;
			}
		}
		if (createIfMissing && component == null && trans != transforms)
		{
			component = trans.gameObject.AddComponent<UIPanel>();
			UIPanel.SetChildLayer(component.cachedTransform, component.gameObject.layer);
		}
		return component;
	}

	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, true);
	}

	public static UIPanel FindRoot(Transform trans)
	{
		UIPanel rootPanel;
		UIPanel uIPanel = UIPanel.Find(trans);
		if (!uIPanel)
		{
			rootPanel = null;
		}
		else
		{
			rootPanel = uIPanel.RootPanel;
		}
		return rootPanel;
	}

	private int GetChangeFlag(UINode start)
	{
		int num = start.changeFlag;
		if (num == -1)
		{
			Transform transforms = start.trans.parent;
			while (true)
			{
				if (!this.mChildren.Contains(transforms))
				{
					num = 0;
					break;
				}
				else
				{
					UINode item = (UINode)this.mChildren[transforms];
					num = item.changeFlag;
					transforms = transforms.parent;
					if (num != -1)
					{
						break;
					}
					else
					{
						UIPanel.mHierarchy.Add(item);
					}
				}
			}
			int num1 = 0;
			int count = UIPanel.mHierarchy.Count;
			while (num1 < count)
			{
				UIPanel.mHierarchy[num1].changeFlag = num;
				num1++;
			}
			UIPanel.mHierarchy.Clear();
		}
		return num;
	}

	private UIDrawCall.Iterator GetDrawCall(UIMaterial mat, bool createIfMissing)
	{
		for (UIDrawCall.Iterator i = (UIDrawCall.Iterator)this.mDrawCalls; i.Has; i = i.Next)
		{
			if (i.Current.material == mat)
			{
				return i;
			}
		}
		UIDrawCall uIDrawCall = null;
		if (createIfMissing)
		{
			GameObject gameObject = new GameObject(string.Concat("_UIDrawCall [", mat.name, "]"))
			{
				hideFlags = HideFlags.DontSave,
				layer = base.gameObject.layer
			};
			uIDrawCall = gameObject.AddComponent<UIDrawCall>();
			uIDrawCall.material = mat;
			uIDrawCall.LinkedList__Insert(ref this.mDrawCalls);
			UIPanel uIPanel = this;
			uIPanel.mDrawCallCount = uIPanel.mDrawCallCount + 1;
		}
		return (UIDrawCall.Iterator)uIDrawCall;
	}

	private UINode GetNode(Transform t)
	{
		UINode item = null;
		if (t != null && this.mChildren.Contains(t))
		{
			item = (UINode)this.mChildren[t];
		}
		return item;
	}

	public static void GlobalUpdate()
	{
		UIPanel.Global.PanelUpdate();
	}

	internal bool InsideClippingRect(Ray ray, int traceID)
	{
		if (this.clipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		if (traceID != this.traceID || ray.origin != this.lastRayTrace.origin || ray.direction != this.lastRayTrace.direction)
		{
			this.traceID = traceID;
			this.lastRayTrace = ray;
			this.lastRayTraceInside = UIPanel.CheckRayEnterClippingRect(ray, base.transform, this.clipRange);
		}
		return this.lastRayTraceInside;
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.mWorldToLocal.MultiplyPoint3x4(a);
		b = this.mWorldToLocal.MultiplyPoint3x4(b);
		c = this.mWorldToLocal.MultiplyPoint3x4(c);
		d = this.mWorldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float single = Mathf.Min(UIPanel.mTemp);
		float single1 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float single2 = Mathf.Min(UIPanel.mTemp);
		float single3 = Mathf.Max(UIPanel.mTemp);
		if (single1 < this.mMin.x)
		{
			return false;
		}
		if (single3 < this.mMin.y)
		{
			return false;
		}
		if (single > this.mMax.x)
		{
			return false;
		}
		if (single2 > this.mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(UIWidget w)
	{
		if (!w.enabled || !w.gameObject.activeInHierarchy || w.color.a < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector2 vector2 = w.relativeSize;
		Vector2 vector21 = Vector2.Scale(w.pivotOffset, vector2);
		Vector2 vector22 = vector21;
		vector21.x = vector21.x + vector2.x;
		vector21.y = vector21.y - vector2.y;
		Transform transforms = w.cachedTransform;
		Vector3 vector3 = transforms.TransformPoint(vector21);
		Vector3 vector31 = transforms.TransformPoint(new Vector2(vector21.x, vector22.y));
		Vector3 vector32 = transforms.TransformPoint(new Vector2(vector22.x, vector21.y));
		Vector3 vector33 = transforms.TransformPoint(vector22);
		return this.IsVisible(vector3, vector31, vector32, vector33);
	}

	public bool ManualPanelUpdate()
	{
		if (!this.manualPanelUpdate || !base.enabled)
		{
			return false;
		}
		this.PanelUpdate(false);
		return true;
	}

	public void MarkMaterialAsChanged(UIMaterial mat, bool sort)
	{
		if (mat)
		{
			if (sort)
			{
				this.mDepthChanged = true;
			}
			if (this.mChanged.Add(mat))
			{
				this.mChangedLastFrame = true;
			}
		}
	}

	protected void OnDestroy()
	{
		UIPanel.Global.UnregisterPanel(this);
		if (this.mHotSpots != null)
		{
			HashSet<UIHotSpot> uIHotSpots = this.mHotSpots;
			this.mHotSpots = null;
			foreach (UIHotSpot uIHotSpot in uIHotSpots)
			{
				uIHotSpot.OnPanelDestroy();
			}
		}
	}

	protected void OnDisable()
	{
		UIPanel.Global.PanelDisabled(this);
		if (this.mHotSpots != null)
		{
			foreach (UIHotSpot mHotSpot in this.mHotSpots)
			{
				mHotSpot.OnPanelDisable();
			}
		}
		UIDrawCall.Iterator next = (UIDrawCall.Iterator)this.mDrawCalls;
		while (next.Has)
		{
			UIDrawCall current = next.Current;
			next = next.Next;
			NGUITools.DestroyImmediate(current.gameObject);
		}
		this.mDrawCalls = null;
		this.mChanged.Clear();
		this.mChildren.Clear();
	}

	protected void OnEnable()
	{
		UIPanel.Global.PanelEnabled(this);
		if (this.mHotSpots != null)
		{
			foreach (UIHotSpot mHotSpot in this.mHotSpots)
			{
				mHotSpot.OnPanelEnable();
			}
		}
		int num = 0;
		int count = this.mWidgets.Count;
		while (num < count)
		{
			this.AddWidget(this.mWidgets[num]);
			num++;
		}
		this.mRebuildAll = true;
	}

	private void PanelUpdate(bool letFill)
	{
		this.UpdateTransformMatrix();
		this.UpdateTransforms();
		if (this.mLayer != base.gameObject.layer)
		{
			this.mLayer = base.gameObject.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
			this.mCam = (uICamera == null ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
			UIPanel.SetChildLayer(this.cachedTransform, this.mLayer);
			for (UIDrawCall.Iterator i = (UIDrawCall.Iterator)this.mDrawCalls; i.Has; i = i.Next)
			{
				i.Current.gameObject.layer = this.mLayer;
			}
		}
		this.UpdateWidgets();
		if (this.mDepthChanged)
		{
			this.mDepthChanged = false;
			this.mWidgets.Sort(new Comparison<UIWidget>(UIWidget.CompareFunc));
		}
		if (!letFill)
		{
			this.UpdateDrawcalls();
		}
		else
		{
			this.FillUpdate();
		}
		this.mRebuildAll = false;
	}

	public void Refresh()
	{
		base.BroadcastMessage("Update", SendMessageOptions.DontRequireReceiver);
		this.DefaultLateUpdate();
	}

	internal static void RegisterHotSpot(UIPanel panel, UIHotSpot hotSpot)
	{
		if (panel.mHotSpots == null)
		{
			panel.mHotSpots = new HashSet<UIHotSpot>();
		}
		if (panel.mHotSpots.Add(hotSpot))
		{
			if (!panel.enabled)
			{
				hotSpot.OnPanelDisable();
			}
			else
			{
				hotSpot.OnPanelEnable();
			}
		}
	}

	private void RemoveTransform(Transform t)
	{
		if (t != null)
		{
			do
			{
				if (!this.mChildren.Contains(t))
				{
					return;
				}
				this.mChildren.Remove(t);
				t = t.parent;
			}
			while (!(t == null) && !(t == this.mTrans) && t.childCount <= 1);
		}
	}

	public void RemoveWidget(UIWidget w)
	{
		if (w != null)
		{
			UINode node = this.GetNode(w.cachedTransform);
			if (node != null)
			{
				if (node.visibleFlag == 1 && !this.mChanged.Contains(w.material))
				{
					this.mChanged.Add(w.material);
					this.mChangedLastFrame = true;
				}
				this.RemoveTransform(w.cachedTransform);
			}
			this.mWidgets.Remove(w);
		}
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.GetComponent<UIPanel>() == null)
			{
				child.gameObject.layer = layer;
				UIPanel.SetChildLayer(child, layer);
			}
		}
	}

	protected void Start()
	{
		this.mLayer = base.gameObject.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
		this.mCam = (uICamera == null ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
	}

	internal static void UnregisterHotSpot(UIPanel panel, UIHotSpot hotSpot)
	{
		if (panel.mHotSpots == null || !panel.mHotSpots.Remove(hotSpot))
		{
			return;
		}
		if (panel.enabled)
		{
			hotSpot.OnPanelDisable();
		}
	}

	public void UpdateDrawcalls()
	{
		Vector4 vector4 = Vector4.zero;
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			vector4 = new Vector4(this.mClipRange.x, this.mClipRange.y, this.mClipRange.z * 0.5f, this.mClipRange.w * 0.5f);
		}
		if (vector4.z == 0f)
		{
			vector4.z = (float)Screen.width * 0.5f;
		}
		if (vector4.w == 0f)
		{
			vector4.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform runtimePlatform = Application.platform;
		if (runtimePlatform == RuntimePlatform.WindowsPlayer || runtimePlatform == RuntimePlatform.WindowsWebPlayer || runtimePlatform == RuntimePlatform.WindowsEditor)
		{
			vector4.x = vector4.x - 0.5f;
			vector4.y = vector4.y + 0.5f;
		}
		Vector3 vector3 = this.cachedTransform.position;
		Quaternion quaternion = this.cachedTransform.rotation;
		Vector3 vector31 = this.cachedTransform.lossyScale;
		UIDrawCall.Iterator next = (UIDrawCall.Iterator)this.mDrawCalls;
		while (next.Has)
		{
			UIDrawCall current = next.Current;
			next = next.Next;
			current.clipping = this.mClipping;
			current.clipRange = vector4;
			current.clipSoftness = this.mClipSoftness;
			current.depthPass = this.depthPass;
			current.panelPropertyBlock = this.propertyBlock;
			Transform transforms = current.transform;
			transforms.position = vector3;
			transforms.rotation = quaternion;
			transforms.localScale = vector31;
		}
	}

	private void UpdateTransformMatrix()
	{
		float single = Time.realtimeSinceStartup;
		if (single == 0f || this.mMatrixTime != single)
		{
			this.mMatrixTime = single;
			this.mWorldToLocal = this.cachedTransform.worldToLocalMatrix;
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				Vector2 vector2 = new Vector2(this.mClipRange.z, this.mClipRange.w);
				if (vector2.x == 0f)
				{
					vector2.x = (this.mCam != null ? this.mCam.pixelWidth : (float)Screen.width);
				}
				if (vector2.y == 0f)
				{
					vector2.y = (this.mCam != null ? this.mCam.pixelHeight : (float)Screen.height);
				}
				vector2 = vector2 * 0.5f;
				this.mMin.x = this.mClipRange.x - vector2.x;
				this.mMin.y = this.mClipRange.y - vector2.y;
				this.mMax.x = this.mClipRange.x + vector2.x;
				this.mMax.y = this.mClipRange.y + vector2.y;
			}
		}
	}

	private void UpdateTransforms()
	{
		int num;
		this.mChangedLastFrame = false;
		bool flag = false;
		bool flag1 = Time.realtimeSinceStartup > this.mCullTime;
		if (!this.widgetsAreStatic || flag1 != this.mCulled)
		{
			int num1 = 0;
			int count = this.mChildren.Count;
			while (num1 < count)
			{
				UINode item = (UINode)this.mChildren[num1];
				if (item.trans == null)
				{
					this.mRemoved.Add(item.trans);
				}
				else if (!item.HasChanged())
				{
					item.changeFlag = -1;
				}
				else
				{
					item.changeFlag = 1;
					flag = true;
				}
				num1++;
			}
			int num2 = 0;
			int count1 = this.mRemoved.Count;
			while (num2 < count1)
			{
				this.mChildren.Remove(this.mRemoved[num2]);
				num2++;
			}
			this.mRemoved.Clear();
		}
		if (!this.mCulled && flag1)
		{
			this.mCheckVisibility = true;
		}
		if (this.mCheckVisibility || flag || this.mRebuildAll)
		{
			int num3 = 0;
			int count2 = this.mChildren.Count;
			while (num3 < count2)
			{
				UINode changeFlag = (UINode)this.mChildren[num3];
				if (changeFlag.widget != null)
				{
					int num4 = 1;
					if (flag1 || flag)
					{
						if (changeFlag.changeFlag == -1)
						{
							changeFlag.changeFlag = this.GetChangeFlag(changeFlag);
						}
						if (flag1)
						{
							if (this.mCheckVisibility || changeFlag.changeFlag == 1)
							{
								num = (!this.IsVisible(changeFlag.widget) ? 0 : 1);
							}
							else
							{
								num = changeFlag.visibleFlag;
							}
							num4 = num;
						}
					}
					if (changeFlag.visibleFlag != num4)
					{
						changeFlag.changeFlag = 1;
					}
					if (changeFlag.changeFlag == 1 && (num4 == 1 || changeFlag.visibleFlag != 0))
					{
						changeFlag.visibleFlag = num4;
						UIMaterial uIMaterial = changeFlag.widget.material;
						if (!this.mChanged.Contains(uIMaterial))
						{
							this.mChanged.Add(uIMaterial);
							this.mChangedLastFrame = true;
						}
					}
				}
				num3++;
			}
		}
		this.mCulled = flag1;
		this.mCheckVisibility = false;
	}

	private void UpdateWidgets()
	{
		int num = 0;
		int count = this.mChildren.Count;
		while (num < count)
		{
			UINode item = (UINode)this.mChildren[num];
			UIWidget uIWidget = item.widget;
			if (item.visibleFlag == 1 && uIWidget != null && uIWidget.UpdateGeometry(ref this.mWorldToLocal, item.changeFlag == 1, this.generateNormals) && !this.mChanged.Contains(uIWidget.material))
			{
				this.mChanged.Add(uIWidget.material);
				this.mChangedLastFrame = true;
			}
			item.changeFlag = 0;
			num++;
		}
	}

	public bool WatchesTransform(Transform t)
	{
		return (t == this.cachedTransform ? true : this.mChildren.Contains(t));
	}

	public enum DebugInfo
	{
		None,
		Gizmos,
		Geometry
	}

	private static class Global
	{
		public static bool noGlobal
		{
			get
			{
				return !Application.isPlaying;
			}
		}

		public static void PanelDisabled(UIPanel panel)
		{
			if (UIPanel.Global.noGlobal)
			{
				return;
			}
			UIPanel.Global.g.allEnabled.Remove(panel);
		}

		public static void PanelEnabled(UIPanel panel)
		{
			if (UIPanel.Global.noGlobal)
			{
				return;
			}
			UIPanel.Global.g.allEnabled.Add(panel);
		}

		public static void PanelUpdate()
		{
			if (UIPanel.Global.noGlobal)
			{
				return;
			}
			try
			{
				UIPanel.Global.g.allEnableSwap.AddRange(UIPanel.Global.g.allEnabled);
				foreach (UIPanel uIPanel in UIPanel.Global.g.allEnableSwap)
				{
					if (!uIPanel || !uIPanel.enabled)
					{
						continue;
					}
					uIPanel.DefaultLateUpdate();
				}
			}
			finally
			{
				UIPanel.Global.g.allEnableSwap.Clear();
			}
		}

		public static void RegisterPanel(UIPanel panel)
		{
			if (UIPanel.Global.noGlobal)
			{
				return;
			}
			UIGlobal.EnsureGlobal();
			if (panel.globalIndex == -1)
			{
				panel.globalIndex = UIPanel.Global.g.allPanels.Count;
				UIPanel.Global.g.allPanels.Add(panel);
			}
		}

		public static void UnregisterPanel(UIPanel panel)
		{
			if (UIPanel.Global.noGlobal)
			{
				return;
			}
			if (panel.globalIndex != -1)
			{
				UIPanel.Global.g.allPanels.RemoveAt(panel.globalIndex);
				int num = panel.globalIndex;
				int count = UIPanel.Global.g.allPanels.Count;
				while (num < count)
				{
					UIPanel.Global.g.allPanels[num].globalIndex = num;
					num++;
				}
				panel.globalIndex = -1;
			}
		}

		private static class g
		{
			public static HashSet<UIPanel> allEnabled;

			public static List<UIPanel> allEnableSwap;

			public static List<UIPanel> allPanels;

			static g()
			{
				UIPanel.Global.g.allEnabled = new HashSet<UIPanel>();
				UIPanel.Global.g.allEnableSwap = new List<UIPanel>();
				UIPanel.Global.g.allPanels = new List<UIPanel>();
				UIGlobal.EnsureGlobal();
			}
		}
	}
}