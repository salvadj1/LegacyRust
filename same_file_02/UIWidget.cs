using NGUI.Meshing;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Color mColor = Color.white;

	[HideInInspector]
	[SerializeField]
	private UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	[HideInInspector]
	[SerializeField]
	private int mDepth;

	[HideInInspector]
	[SerializeField]
	private bool mAlphaUnchecked;

	[NonSerialized]
	private bool mForcedChanged;

	private Transform mTrans;

	private Texture mTex;

	private UIPanel mPanel;

	private bool mChangedCall = true;

	protected bool mPlayMode = true;

	private bool gotCachedTransform;

	[NonSerialized]
	protected readonly UIWidget.WidgetFlags widgetFlags;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private int mVisibleFlag = -1;

	private int globalIndex = -1;

	private UIGeometry __mGeom;

	private static Vector2[] tempVector2s;

	private static UIWidget.WidgetFlags[] tempWidgetFlags;

	public float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			Color color = this.mColor;
			color.a = value;
			this.color = color;
		}
	}

	public bool alphaUnchecked
	{
		get
		{
			return this.mAlphaUnchecked;
		}
		set
		{
			if (value)
			{
				if (!this.mAlphaUnchecked)
				{
					this.mAlphaUnchecked = true;
					if (NGUITools.ZeroAlpha(this.mColor.a))
					{
						this.mChangedCall = true;
					}
				}
			}
			else if (this.mAlphaUnchecked)
			{
				this.mAlphaUnchecked = false;
				if (NGUITools.ZeroAlpha(this.mColor.a))
				{
					this.mChangedCall = true;
				}
			}
		}
	}

	protected UIMaterial baseMaterial
	{
		get
		{
			return (UIMaterial)this.mMat;
		}
		set
		{
			UIMaterial uIMaterial = (UIMaterial)this.mMat;
			if (uIMaterial != value)
			{
				if (uIMaterial != null && this.mPanel != null)
				{
					this.mPanel.RemoveWidget(this);
				}
				this.mPanel = null;
				this.mMat = (Material)value;
				this.mTex = null;
				if (this.mMat != null)
				{
					this.CreatePanel();
				}
			}
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (!this.gotCachedTransform)
			{
				this.mTrans = base.transform;
				this.gotCachedTransform = true;
			}
			return this.mTrans;
		}
	}

	public bool changesQueued
	{
		get
		{
			return (this.mChangedCall ? true : this.mForcedChanged);
		}
	}

	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				this.mColor = value;
				this.mChangedCall = true;
			}
		}
	}

	protected virtual UIMaterial customMaterial
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				if (this.mPanel != null)
				{
					this.mPanel.MarkMaterialAsChanged(this.material, true);
				}
			}
		}
	}

	public bool keepMaterial
	{
		get
		{
			return (byte)(this.widgetFlags & UIWidget.WidgetFlags.KeepsMaterial) == 64;
		}
	}

	public Texture mainTexture
	{
		get
		{
			if (!this.mTex)
			{
				UIMaterial uIMaterial = this.material;
				if (uIMaterial != null)
				{
					this.mTex = uIMaterial.mainTexture;
				}
			}
			return this.mTex;
		}
	}

	public UIMaterial material
	{
		get
		{
			if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.CustomMaterialGet) == 4)
			{
				return this.customMaterial;
			}
			return this.baseMaterial;
		}
		set
		{
			if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.CustomMaterialSet) != 8)
			{
				this.baseMaterial = value;
			}
			else
			{
				this.customMaterial = value;
			}
		}
	}

	private UIGeometry mGeom
	{
		get
		{
			UIGeometry _MGeom = this.__mGeom;
			if (_MGeom == null)
			{
				UIGeometry uIGeometry = new UIGeometry();
				UIGeometry uIGeometry1 = uIGeometry;
				this.__mGeom = uIGeometry;
				_MGeom = uIGeometry1;
			}
			return _MGeom;
		}
	}

	public UIPanel panel
	{
		get
		{
			if (!this.mPanel)
			{
				this.CreatePanel();
			}
			return this.mPanel;
		}
		set
		{
			this.mPanel = value;
		}
	}

	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				this.mChangedCall = true;
			}
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.CustomPivotOffset) != 1)
			{
				return UIWidget.DefaultPivot(this.mPivot);
			}
			UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomPivotOffset;
			this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
			return UIWidget.tempVector2s[0];
		}
	}

	public Vector2 relativeSize
	{
		get
		{
			if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.CustomRelativeSize) != 2)
			{
				return Vector2.one;
			}
			UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomRelativeSize;
			this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
			return UIWidget.tempVector2s[0];
		}
	}

	public int visibleFlag
	{
		get
		{
			return this.mVisibleFlag;
		}
		set
		{
			this.mVisibleFlag = value;
		}
	}

	[Obsolete("Use 'relativeSize' instead")]
	public Vector2 visibleSize
	{
		get
		{
			return this.relativeSize;
		}
	}

	static UIWidget()
	{
		Vector2[] vector2Array = new Vector2[2];
		Vector2 vector2 = new Vector2();
		vector2Array[0] = vector2;
		Vector2 vector21 = new Vector2();
		vector2Array[1] = vector21;
		UIWidget.tempVector2s = vector2Array;
		UIWidget.tempWidgetFlags = new UIWidget.WidgetFlags[2];
	}

	protected UIWidget(UIWidget.WidgetFlags flags)
	{
		this.widgetFlags = flags;
	}

	protected void Awake()
	{
		this.mPlayMode = Application.isPlaying;
		UIWidget.Global.RegisterWidget(this);
	}

	protected void ChangedAuto()
	{
		this.mChangedCall = true;
	}

	private void CheckLayer()
	{
		if (this.mPanel != null && this.mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.mPanel.gameObject.layer;
		}
	}

	private void CheckParent()
	{
		if (this.mPanel != null)
		{
			bool flag = true;
			Transform transforms = this.cachedTransform.parent;
			while (transforms != null)
			{
				if (transforms == this.mPanel.cachedTransform)
				{
					break;
				}
				else if (this.mPanel.WatchesTransform(transforms))
				{
					transforms = transforms.parent;
				}
				else
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.KeepsMaterial) != 64)
				{
					this.material = null;
				}
				this.mPanel = null;
				this.CreatePanel();
			}
		}
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	private void CreatePanel()
	{
		if (!this.mPanel && base.enabled && base.gameObject.activeInHierarchy && this.material != null)
		{
			this.mPanel = UIPanel.Find(this.cachedTransform);
			if (this.mPanel != null)
			{
				this.CheckLayer();
				this.mPanel.AddWidget(this);
				this.mChangedCall = true;
			}
		}
	}

	protected static Vector2 DefaultPivot(UIWidget.Pivot pivot)
	{
		Vector2 vector2 = new Vector2();
		switch (pivot)
		{
			case UIWidget.Pivot.TopLeft:
			{
				vector2.x = 0f;
				vector2.y = 0f;
				break;
			}
			case UIWidget.Pivot.Top:
			{
				vector2.y = -0.5f;
				vector2.x = -1f;
				break;
			}
			case UIWidget.Pivot.TopRight:
			{
				vector2.y = 0f;
				vector2.x = -1f;
				break;
			}
			case UIWidget.Pivot.Left:
			{
				vector2.x = 0f;
				vector2.y = 0.5f;
				break;
			}
			case UIWidget.Pivot.Center:
			{
				vector2.x = -0.5f;
				vector2.y = 0.5f;
				break;
			}
			case UIWidget.Pivot.Right:
			{
				vector2.x = -1f;
				vector2.y = 0.5f;
				break;
			}
			case UIWidget.Pivot.BottomLeft:
			{
				vector2.x = 0f;
				vector2.y = 1f;
				break;
			}
			case UIWidget.Pivot.Bottom:
			{
				vector2.x = -0.5f;
				vector2.y = 1f;
				break;
			}
			case UIWidget.Pivot.BottomRight:
			{
				vector2.x = -1f;
				vector2.y = 1f;
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		return vector2;
	}

	private void DefaultUpdate()
	{
		if (!this.mPanel)
		{
			this.CreatePanel();
		}
		Vector3 vector3 = this.cachedTransform.localScale;
		if (vector3.z != 1f)
		{
			vector3.z = 1f;
			this.mTrans.localScale = vector3;
		}
	}

	public void ForceReloadMaterial()
	{
		if (this.mMat)
		{
			if (this.mPanel)
			{
				this.mPanel.RemoveWidget(this);
			}
			this.mPanel = null;
			this.mTex = null;
			if (this.mMat)
			{
				this.CreatePanel();
			}
		}
	}

	protected virtual void GetCustomVector2s(int start, int end, UIWidget.WidgetFlags[] flags, Vector2[] v)
	{
		throw new NotSupportedException("Only call base.GetCustomVector2s when its something not supported by your implementation, otherwise the custructor for your class is incorrect in its usage.");
	}

	public void GetPivotOffsetAndRelativeSize(out Vector2 pivotOffset, out Vector2 relativeSize)
	{
		relativeSize = new Vector2();
		float single;
		switch ((byte)(this.widgetFlags & (UIWidget.WidgetFlags.CustomPivotOffset | UIWidget.WidgetFlags.CustomRelativeSize)))
		{
			case 1:
			{
				UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomPivotOffset;
				this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
				pivotOffset = UIWidget.tempVector2s[0];
				float single1 = 1f;
				single = single1;
				relativeSize.y = single1;
				relativeSize.x = single;
				break;
			}
			case 2:
			{
				UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomRelativeSize;
				this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
				relativeSize = UIWidget.tempVector2s[0];
				pivotOffset = UIWidget.DefaultPivot(this.mPivot);
				break;
			}
			case 3:
			{
				UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomPivotOffset;
				UIWidget.tempWidgetFlags[1] = UIWidget.WidgetFlags.CustomRelativeSize;
				this.GetCustomVector2s(0, 2, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
				pivotOffset = UIWidget.tempVector2s[0];
				relativeSize = UIWidget.tempVector2s[1];
				break;
			}
			default:
			{
				pivotOffset = UIWidget.DefaultPivot(this.mPivot);
				float single2 = 1f;
				single = single2;
				relativeSize.y = single2;
				relativeSize.x = single;
				break;
			}
		}
	}

	public static void GlobalUpdate()
	{
		UIWidget.Global.WidgetUpdate();
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 vector3 = this.cachedTransform.localScale;
		int num = Mathf.RoundToInt(vector3.x);
		int num1 = Mathf.RoundToInt(vector3.y);
		vector3.x = (float)num;
		vector3.y = (float)num1;
		vector3.z = 1f;
		Vector3 vector31 = this.cachedTransform.localPosition;
		vector31.z = (float)Mathf.RoundToInt(vector31.z);
		if (num % 2 != 1 || this.pivot != UIWidget.Pivot.Top && this.pivot != UIWidget.Pivot.Center && this.pivot != UIWidget.Pivot.Bottom)
		{
			vector31.x = Mathf.Round(vector31.x);
		}
		else
		{
			vector31.x = Mathf.Floor(vector31.x) + 0.5f;
		}
		if (num1 % 2 != 1 || this.pivot != UIWidget.Pivot.Left && this.pivot != UIWidget.Pivot.Center && this.pivot != UIWidget.Pivot.Right)
		{
			vector31.y = Mathf.Round(vector31.y);
		}
		else
		{
			vector31.y = Mathf.Ceil(vector31.y) - 0.5f;
		}
		this.cachedTransform.localPosition = vector31;
		this.cachedTransform.localScale = vector3;
	}

	public virtual void MarkAsChanged()
	{
		this.mChangedCall = true;
		if (this.mPanel != null && base.enabled && base.gameObject.activeInHierarchy && !Application.isPlaying && this.material != null)
		{
			this.mPanel.AddWidget(this);
			this.CheckLayer();
		}
	}

	public void MarkAsChangedForced()
	{
		this.MarkAsChanged();
		this.mForcedChanged = true;
	}

	private void OnDestroy()
	{
		UIWidget.Global.UnregisterWidget(this);
		if (this.mPanel != null)
		{
			this.mPanel.RemoveWidget(this);
			this.mPanel = null;
		}
		this.__mGeom = null;
	}

	private void OnDisable()
	{
		UIWidget.Global.WidgetDisabled(this);
		if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.KeepsMaterial) != 64)
		{
			this.material = null;
		}
		else if (this.mPanel != null)
		{
			this.mPanel.RemoveWidget(this);
		}
		this.mPanel = null;
	}

	private void OnEnable()
	{
		UIWidget.Global.WidgetEnabled(this);
		this.mChangedCall = true;
		if ((byte)(this.widgetFlags & UIWidget.WidgetFlags.KeepsMaterial) != 64)
		{
			this.mMat = null;
			this.mTex = null;
		}
		if (this.mPanel != null && this.material != null)
		{
			this.mPanel.MarkMaterialAsChanged(this.material, false);
		}
	}

	public abstract void OnFill(MeshBuffer m);

	protected virtual void OnStart()
	{
	}

	public virtual bool OnUpdate()
	{
		return false;
	}

	private void Start()
	{
		this.OnStart();
		this.CreatePanel();
	}

	public bool UpdateGeometry(ref Matrix4x4 worldToPanel, bool parentMoved, bool generateNormals)
	{
		Vector3 vector3 = new Vector3();
		Vector2 vector2;
		Vector2 vector21 = new Vector2();
		float single;
		if (!this.material)
		{
			return false;
		}
		UIGeometry uIGeometry = this.mGeom;
		if (!this.OnUpdate() && !this.mChangedCall && !this.mForcedChanged)
		{
			if (uIGeometry.hasVertices && parentMoved)
			{
				Matrix4x4 matrix4x4 = worldToPanel * this.cachedTransform.localToWorldMatrix;
				uIGeometry.Apply(ref matrix4x4);
			}
			return false;
		}
		this.mChangedCall = false;
		this.mForcedChanged = false;
		uIGeometry.Clear();
		if (this.mAlphaUnchecked || !NGUITools.ZeroAlpha(this.mColor.a))
		{
			this.OnFill(uIGeometry.meshBuffer);
		}
		if (uIGeometry.hasVertices)
		{
			switch ((byte)(this.widgetFlags & (UIWidget.WidgetFlags.CustomPivotOffset | UIWidget.WidgetFlags.CustomRelativeSize)))
			{
				case 1:
				{
					UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomPivotOffset;
					this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
					vector2 = UIWidget.tempVector2s[0];
					float single1 = 1f;
					single = single1;
					vector21.y = single1;
					vector21.x = single;
					break;
				}
				case 2:
				{
					UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomRelativeSize;
					this.GetCustomVector2s(0, 1, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
					vector21 = UIWidget.tempVector2s[0];
					vector2 = UIWidget.DefaultPivot(this.mPivot);
					break;
				}
				case 3:
				{
					UIWidget.tempWidgetFlags[0] = UIWidget.WidgetFlags.CustomPivotOffset;
					UIWidget.tempWidgetFlags[1] = UIWidget.WidgetFlags.CustomRelativeSize;
					this.GetCustomVector2s(0, 2, UIWidget.tempWidgetFlags, UIWidget.tempVector2s);
					vector2 = UIWidget.tempVector2s[0];
					vector21 = UIWidget.tempVector2s[1];
					break;
				}
				default:
				{
					vector2 = UIWidget.DefaultPivot(this.mPivot);
					float single2 = 1f;
					single = single2;
					vector21.y = single2;
					vector21.x = single;
					break;
				}
			}
			vector3.x = vector2.x * vector21.x;
			vector3.y = vector2.y * vector21.y;
			vector3.z = 0f;
			Matrix4x4 matrix4x41 = worldToPanel * this.cachedTransform.localToWorldMatrix;
			uIGeometry.Apply(ref vector3, ref matrix4x41);
		}
		return true;
	}

	public void WriteToBuffers(MeshBuffer m)
	{
		this.mGeom.WriteToBuffers(m);
	}

	private static class Global
	{
		private static bool noGlobal
		{
			get
			{
				return !Application.isPlaying;
			}
		}

		public static void RegisterWidget(UIWidget widget)
		{
			if (UIWidget.Global.noGlobal)
			{
				return;
			}
			UIGlobal.EnsureGlobal();
			if (widget.globalIndex == -1)
			{
				widget.globalIndex = UIWidget.Global.g.allWidgets.Count;
				UIWidget.Global.g.allWidgets.Add(widget);
			}
		}

		public static void UnregisterWidget(UIWidget widget)
		{
			if (UIWidget.Global.noGlobal)
			{
				return;
			}
			if (widget.globalIndex != -1)
			{
				UIWidget.Global.g.allWidgets.RemoveAt(widget.globalIndex);
				int num = widget.globalIndex;
				int count = UIWidget.Global.g.allWidgets.Count;
				while (num < count)
				{
					UIWidget.Global.g.allWidgets[num].globalIndex = num;
					num++;
				}
				widget.globalIndex = -1;
			}
		}

		public static void WidgetDisabled(UIWidget widget)
		{
			if (UIWidget.Global.noGlobal)
			{
				return;
			}
			UIWidget.Global.g.enabledWidgets.Remove(widget);
		}

		public static void WidgetEnabled(UIWidget widget)
		{
			if (UIWidget.Global.noGlobal)
			{
				return;
			}
			UIWidget.Global.g.enabledWidgets.Add(widget);
		}

		public static void WidgetUpdate()
		{
			if (UIWidget.Global.noGlobal)
			{
				return;
			}
			try
			{
				UIWidget.Global.g.enableWidgetsSwap.AddRange(UIWidget.Global.g.enabledWidgets);
				foreach (UIWidget uIWidget in UIWidget.Global.g.enableWidgetsSwap)
				{
					if (!uIWidget || !uIWidget.enabled)
					{
						continue;
					}
					uIWidget.DefaultUpdate();
				}
			}
			finally
			{
				UIWidget.Global.g.enableWidgetsSwap.Clear();
			}
		}

		public static class g
		{
			public static List<UIWidget> allWidgets;

			public static HashSet<UIWidget> enabledWidgets;

			public static List<UIWidget> enableWidgetsSwap;

			static g()
			{
				UIWidget.Global.g.allWidgets = new List<UIWidget>();
				UIWidget.Global.g.enabledWidgets = new HashSet<UIWidget>();
				UIWidget.Global.g.enableWidgetsSwap = new List<UIWidget>();
				UIGlobal.EnsureGlobal();
			}
		}
	}

	public enum Pivot
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	[Flags]
	protected enum WidgetFlags : byte
	{
		CustomPivotOffset = 1,
		CustomRelativeSize = 2,
		CustomMaterialGet = 4,
		CustomMaterialSet = 8,
		CustomBorder = 16,
		KeepsMaterial = 64,
		Reserved = 128
	}
}