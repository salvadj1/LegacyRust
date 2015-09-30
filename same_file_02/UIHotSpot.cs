using System;
using UnityEngine;

public class UIHotSpot : MonoBehaviour
{
	private const UIHotSpot.Kind kKindFlag_2D = UIHotSpot.Kind.Circle;

	private const UIHotSpot.Kind kKindFlag_3D = UIHotSpot.Kind.Sphere;

	private const UIHotSpot.Kind kKindFlag_Radial = UIHotSpot.Kind.Circle;

	private const UIHotSpot.Kind kKindFlag_Axial = UIHotSpot.Kind.Rect;

	private const UIHotSpot.Kind kKindFlag_Convex = UIHotSpot.Kind.Convex;

	private const float kCos45 = 0.707106769f;

	private const float k2Cos45 = 1.41421354f;

	public readonly UIHotSpot.Kind kind;

	private UIPanel panel;

	private Matrix4x4 toWorld;

	private Matrix4x4 toLocal;

	private Matrix4x4 lastWorld;

	private Matrix4x4 lastLocal;

	private Bounds _bounds;

	private Bounds _lastBoundsEntered;

	private bool once;

	private bool justAdded;

	private int index = -1;

	private readonly bool configuredInLocalSpace;

	protected readonly static Plane localPlane;

	public UIBoxHotSpot asBox
	{
		get
		{
			UIBoxHotSpot uIBoxHotSpot;
			if (this.kind != UIHotSpot.Kind.Box)
			{
				uIBoxHotSpot = null;
			}
			else
			{
				uIBoxHotSpot = (UIBoxHotSpot)this;
			}
			return uIBoxHotSpot;
		}
	}

	public UIBrushHotSpot asBrush
	{
		get
		{
			UIBrushHotSpot uIBrushHotSpot;
			if (this.kind != UIHotSpot.Kind.Brush)
			{
				uIBrushHotSpot = null;
			}
			else
			{
				uIBrushHotSpot = (UIBrushHotSpot)this;
			}
			return uIBrushHotSpot;
		}
	}

	public UICircleHotSpot asCircle
	{
		get
		{
			UICircleHotSpot uICircleHotSpot;
			if (this.kind != UIHotSpot.Kind.Circle)
			{
				uICircleHotSpot = null;
			}
			else
			{
				uICircleHotSpot = (UICircleHotSpot)this;
			}
			return uICircleHotSpot;
		}
	}

	public UIConvexHotSpot asConvex
	{
		get
		{
			UIConvexHotSpot uIConvexHotSpot;
			if (this.kind != UIHotSpot.Kind.Convex)
			{
				uIConvexHotSpot = null;
			}
			else
			{
				uIConvexHotSpot = (UIConvexHotSpot)this;
			}
			return uIConvexHotSpot;
		}
	}

	public UIRectHotSpot asRect
	{
		get
		{
			UIRectHotSpot uIRectHotSpot;
			if (this.kind != UIHotSpot.Kind.Rect)
			{
				uIRectHotSpot = null;
			}
			else
			{
				uIRectHotSpot = (UIRectHotSpot)this;
			}
			return uIRectHotSpot;
		}
	}

	public UISphereHotSpot asSphere
	{
		get
		{
			UISphereHotSpot uISphereHotSpot;
			if (this.kind != UIHotSpot.Kind.Sphere)
			{
				uISphereHotSpot = null;
			}
			else
			{
				uISphereHotSpot = (UISphereHotSpot)this;
			}
			return uISphereHotSpot;
		}
	}

	protected static Vector3 backward
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = 0f;
			vector3.y = 0f;
			vector3.z = 1f;
			return vector3;
		}
	}

	private UIBoxHotSpot boxUS
	{
		get
		{
			return (UIBoxHotSpot)this;
		}
	}

	private UIBrushHotSpot brushUS
	{
		get
		{
			return (UIBrushHotSpot)this;
		}
	}

	public Vector3 center
	{
		get
		{
			UIHotSpot.Kind kind = this.kind;
			if (kind == UIHotSpot.Kind.Circle)
			{
				return ((UICircleHotSpot)this).center;
			}
			if (kind == UIHotSpot.Kind.Rect)
			{
				return ((UIRectHotSpot)this).center;
			}
			if (kind == UIHotSpot.Kind.Sphere)
			{
				return ((UISphereHotSpot)this).center;
			}
			if (kind == UIHotSpot.Kind.Box)
			{
				return ((UIBoxHotSpot)this).center;
			}
			return new Vector3();
		}
		set
		{
			UIHotSpot.Kind kind = this.kind;
			if (kind == UIHotSpot.Kind.Circle)
			{
				((UICircleHotSpot)this).center = value;
			}
			else if (kind == UIHotSpot.Kind.Rect)
			{
				((UIRectHotSpot)this).center = value;
			}
			else if (kind == UIHotSpot.Kind.Sphere)
			{
				((UISphereHotSpot)this).center = value;
			}
			else if (kind == UIHotSpot.Kind.Box)
			{
				((UIBoxHotSpot)this).center = value;
			}
		}
	}

	private UICircleHotSpot circleUS
	{
		get
		{
			return (UICircleHotSpot)this;
		}
	}

	private UIConvexHotSpot convexUS
	{
		get
		{
			return (UIConvexHotSpot)this;
		}
	}

	protected static Vector3 forward
	{
		get
		{
			Vector3 vector3 = new Vector3();
			vector3.x = 0f;
			vector3.y = 0f;
			vector3.z = -1f;
			return vector3;
		}
	}

	protected Color gizmoColor
	{
		get
		{
			return Color.green;
		}
	}

	protected Matrix4x4 gizmoMatrix
	{
		get
		{
			if (this.index != -1)
			{
				return (!this.configuredInLocalSpace ? Matrix4x4.identity : this.toWorld);
			}
			return (!this.configuredInLocalSpace ? Matrix4x4.identity : base.transform.localToWorldMatrix);
		}
	}

	public bool isBox
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Box;
		}
	}

	public bool isBrush
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Brush;
		}
	}

	public bool isCircle
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Circle;
		}
	}

	public bool isConvex
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Convex;
		}
	}

	public bool isRect
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Rect;
		}
	}

	public bool isSphere
	{
		get
		{
			return this.kind == UIHotSpot.Kind.Sphere;
		}
	}

	private UIRectHotSpot rectUS
	{
		get
		{
			return (UIRectHotSpot)this;
		}
	}

	public Vector3 size
	{
		get
		{
			Vector3 vector3 = new Vector3();
			UIHotSpot.Kind kind = this.kind;
			if (kind == UIHotSpot.Kind.Circle)
			{
				vector3.x = ((UICircleHotSpot)this).radius * 2f;
				vector3.y = vector3.x;
				vector3.z = 0f;
				return vector3;
			}
			if (kind == UIHotSpot.Kind.Rect)
			{
				return ((UIRectHotSpot)this).size;
			}
			if (kind != UIHotSpot.Kind.Sphere)
			{
				if (kind == UIHotSpot.Kind.Box)
				{
					return ((UIBoxHotSpot)this).size;
				}
				return new Vector3();
			}
			vector3.x = ((UICircleHotSpot)this).radius * 1.41421354f;
			float single = vector3.x;
			float single1 = single;
			vector3.z = single;
			vector3.y = single1;
			return vector3;
		}
		set
		{
			UIHotSpot.Kind kind = this.kind;
			if (kind == UIHotSpot.Kind.Circle)
			{
				value.y = value.y * 0.707106769f;
				value.x = value.x * 0.707106769f;
				((UICircleHotSpot)this).radius = Mathf.Sqrt(value.x * value.x + value.y * value.y) / 2f;
			}
			else if (kind == UIHotSpot.Kind.Rect)
			{
				((UIRectHotSpot)this).size = new Vector2(value.x, value.y);
			}
			else if (kind == UIHotSpot.Kind.Sphere)
			{
				value.z = value.z * 0.707106769f;
				value.y = value.y * 0.707106769f;
				value.x = value.x * 0.707106769f;
				((UISphereHotSpot)this).radius = Mathf.Sqrt(value.x * value.x + value.y * value.y + value.z * value.z) / 2f;
			}
			else if (kind == UIHotSpot.Kind.Box)
			{
				((UIBoxHotSpot)this).size = value;
			}
		}
	}

	private UISphereHotSpot sphereUS
	{
		get
		{
			return (UISphereHotSpot)this;
		}
	}

	public UIPanel uipanel
	{
		get
		{
			return this.panel;
		}
	}

	public Vector3 worldCenter
	{
		get
		{
			Vector3 vector3;
			UIHotSpot.Kind kind = this.kind;
			if (kind == UIHotSpot.Kind.Circle)
			{
				vector3 = ((UICircleHotSpot)this).center;
			}
			else if (kind == UIHotSpot.Kind.Rect)
			{
				vector3 = ((UIRectHotSpot)this).center;
			}
			else if (kind == UIHotSpot.Kind.Sphere)
			{
				vector3 = ((UISphereHotSpot)this).center;
			}
			else
			{
				if (kind != UIHotSpot.Kind.Box)
				{
					throw new NotImplementedException();
				}
				vector3 = ((UIBoxHotSpot)this).center;
			}
			return base.transform.TransformPoint(vector3);
		}
	}

	static UIHotSpot()
	{
		UIHotSpot.localPlane = new Plane(Vector3.back, Vector3.zero);
	}

	protected UIHotSpot(UIHotSpot.Kind kind, bool configuredInLocalSpace)
	{
		this.kind = kind;
		this.configuredInLocalSpace = configuredInLocalSpace;
	}

	public bool As(out UICircleHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Circle)
		{
			cast = null;
			return false;
		}
		cast = (UICircleHotSpot)this;
		return true;
	}

	public bool As(out UIRectHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Rect)
		{
			cast = null;
			return false;
		}
		cast = (UIRectHotSpot)this;
		return true;
	}

	public bool As(out UIConvexHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Convex)
		{
			cast = null;
			return false;
		}
		cast = (UIConvexHotSpot)this;
		return true;
	}

	public bool As(out UISphereHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Sphere)
		{
			cast = null;
			return false;
		}
		cast = (UISphereHotSpot)this;
		return true;
	}

	public bool As(out UIBoxHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Box)
		{
			cast = null;
			return false;
		}
		cast = (UIBoxHotSpot)this;
		return true;
	}

	public bool As(out UIBrushHotSpot cast)
	{
		if (this.kind != UIHotSpot.Kind.Brush)
		{
			cast = null;
			return false;
		}
		cast = (UIBrushHotSpot)this;
		return true;
	}

	public bool ClosestRaycast(Ray ray, ref UIHotSpot.Hit hit)
	{
		UIHotSpot.Hit hit1;
		if (!this.Raycast(ray, out hit1) || hit1.distance >= hit.distance)
		{
			return false;
		}
		hit = hit1;
		return true;
	}

	public static void ConvertRaycastHit(ref Ray ray, ref RaycastHit raycastHit, out UIHotSpot.Hit hit)
	{
		hit = new UIHotSpot.Hit();
		hit.collider = raycastHit.collider;
		hit.hotSpot = hit.collider.GetComponent<UIHotSpot>();
		hit.isCollider = !hit.hotSpot;
		if (!hit.isCollider)
		{
			hit.panel = (!hit.hotSpot.panel ? UIPanel.Find(hit.collider.transform) : hit.hotSpot.panel);
		}
		else
		{
			hit.panel = UIPanel.Find(hit.collider.transform);
		}
		hit.ray = ray;
		hit.distance = raycastHit.distance;
		hit.point = raycastHit.point;
		hit.normal = raycastHit.normal;
	}

	private bool DisableHotSpot()
	{
		return UIHotSpot.Global.Remove(this);
	}

	private bool DoRaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		UIHotSpot.Kind kind = this.kind;
		switch (kind)
		{
			case UIHotSpot.Kind.Circle:
			{
				return ((UICircleHotSpot)this).Internal_RaycastRef(ray, ref hit);
			}
			case UIHotSpot.Kind.Rect:
			{
				return ((UIRectHotSpot)this).Internal_RaycastRef(ray, ref hit);
			}
			case UIHotSpot.Kind.Convex:
			{
				return ((UIConvexHotSpot)this).Internal_RaycastRef(ray, ref hit);
			}
			default:
			{
				switch (kind)
				{
					case UIHotSpot.Kind.Sphere:
					{
						return ((UISphereHotSpot)this).Internal_RaycastRef(ray, ref hit);
					}
					case UIHotSpot.Kind.Box:
					{
						return ((UIBoxHotSpot)this).Internal_RaycastRef(ray, ref hit);
					}
					case UIHotSpot.Kind.Brush:
					{
						return ((UIBrushHotSpot)this).Internal_RaycastRef(ray, ref hit);
					}
				}
				break;
			}
		}
		throw new NotImplementedException();
	}

	private bool EnableHotSpot()
	{
		return UIHotSpot.Global.Add(this);
	}

	protected virtual void HotSpotInit()
	{
	}

	private bool LocalRaycastRef(Ray worldRay, ref UIHotSpot.Hit hit)
	{
		Matrix4x4 matrix4x4 = base.transform.worldToLocalMatrix;
		Ray ray = new Ray(matrix4x4.MultiplyPoint(worldRay.origin), matrix4x4.MultiplyVector(worldRay.direction));
		UIHotSpot.Hit hit1 = UIHotSpot.Hit.invalid;
		if (!this.DoRaycastRef(ray, ref hit1))
		{
			return false;
		}
		matrix4x4 = base.transform.localToWorldMatrix;
		hit.point = matrix4x4.MultiplyPoint(hit1.point);
		hit.normal = matrix4x4.MultiplyVector(hit1.normal);
		hit.ray = worldRay;
		hit.distance = Vector3.Dot(worldRay.direction, hit.point - worldRay.origin);
		hit.hotSpot = this;
		hit.panel = this.panel;
		return true;
	}

	private void OnDestroy()
	{
		if (this.panel)
		{
			UIPanel uIPanel = this.panel;
			this.panel = null;
			UIPanel.UnregisterHotSpot(uIPanel, this);
		}
	}

	protected void OnDisable()
	{
		if (this.once)
		{
			this.DisableHotSpot();
		}
	}

	protected void OnEnable()
	{
		if (this.panel && this.panel.enabled)
		{
			this.EnableHotSpot();
		}
	}

	internal void OnPanelDestroy()
	{
		UIPanel uIPanel = this.panel;
		this.panel = null;
		if (base.enabled && uIPanel && uIPanel.enabled)
		{
			this.OnPanelDisable();
		}
	}

	internal void OnPanelDisable()
	{
		if (base.enabled)
		{
			this.DisableHotSpot();
		}
	}

	internal void OnPanelEnable()
	{
		if (base.enabled)
		{
			this.EnableHotSpot();
		}
	}

	public bool Raycast(Ray ray, out UIHotSpot.Hit hit)
	{
		hit = UIHotSpot.Hit.invalid;
		return this.RaycastRef(ray, ref hit);
	}

	public static bool Raycast(Ray ray, out UIHotSpot.Hit hit, float distance)
	{
		return UIHotSpot.Global.Raycast(ray, out hit, distance);
	}

	public bool RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		if (this.configuredInLocalSpace)
		{
			return this.LocalRaycastRef(ray, ref hit);
		}
		return this.DoRaycastRef(ray, ref hit);
	}

	private void SetBounds(bool moved, Bounds bounds, bool worldEquals)
	{
		if (!this.configuredInLocalSpace)
		{
			this._lastBoundsEntered = bounds;
			this._bounds = bounds;
		}
		else
		{
			if (this._lastBoundsEntered == bounds && worldEquals)
			{
				return;
			}
			this._lastBoundsEntered = bounds;
			AABBox.Transform3x4(ref bounds, ref this.toWorld, out this._bounds);
		}
	}

	private void Start()
	{
		this.panel = UIPanel.Find(base.transform);
		if (!this.panel)
		{
			Debug.LogWarning("Did not find panel!", this);
		}
		else
		{
			UIPanel.RegisterHotSpot(this.panel, this);
		}
	}

	private static class Global
	{
		private static int allCount;

		private static bool allAny;

		private static Bounds allBounds;

		private static bool validBounds;

		private static bool anyAddedRecently;

		private static bool anyRemovedRecently;

		private static UIHotSpot.Global.List<UICircleHotSpot> Circle;

		private static UIHotSpot.Global.List<UIRectHotSpot> Rect;

		private static UIHotSpot.Global.List<UIConvexHotSpot> Convex;

		private static UIHotSpot.Global.List<UISphereHotSpot> Sphere;

		private static UIHotSpot.Global.List<UIBoxHotSpot> Box;

		private static UIHotSpot.Global.List<UIBrushHotSpot> Brush;

		private static int lastStepFrame;

		static Global()
		{
			UIHotSpot.Global.lastStepFrame = -2147483648;
		}

		public static bool Add(UIHotSpot hotSpot)
		{
			if (hotSpot.index != -1)
			{
				return false;
			}
			UIHotSpot.Kind kind = hotSpot.kind;
			switch (kind)
			{
				case UIHotSpot.Kind.Circle:
				{
					UIHotSpot.Global.Circle.Add((UICircleHotSpot)hotSpot);
					break;
				}
				case UIHotSpot.Kind.Rect:
				{
					UIHotSpot.Global.Rect.Add((UIRectHotSpot)hotSpot);
					break;
				}
				case UIHotSpot.Kind.Convex:
				{
					UIHotSpot.Global.Convex.Add((UIConvexHotSpot)hotSpot);
					break;
				}
				default:
				{
					switch (kind)
					{
						case UIHotSpot.Kind.Sphere:
						{
							UIHotSpot.Global.Sphere.Add((UISphereHotSpot)hotSpot);
							hotSpot.justAdded = true;
							if (!hotSpot.once)
							{
								hotSpot.HotSpotInit();
								hotSpot.once = true;
							}
							return true;
						}
						case UIHotSpot.Kind.Box:
						{
							UIHotSpot.Global.Box.Add((UIBoxHotSpot)hotSpot);
							hotSpot.justAdded = true;
							if (!hotSpot.once)
							{
								hotSpot.HotSpotInit();
								hotSpot.once = true;
							}
							return true;
						}
						case UIHotSpot.Kind.Brush:
						{
							UIHotSpot.Global.Brush.Add((UIBrushHotSpot)hotSpot);
							hotSpot.justAdded = true;
							if (!hotSpot.once)
							{
								hotSpot.HotSpotInit();
								hotSpot.once = true;
							}
							return true;
						}
					}
					throw new NotImplementedException();
				}
			}
			hotSpot.justAdded = true;
			if (!hotSpot.once)
			{
				hotSpot.HotSpotInit();
				hotSpot.once = true;
			}
			return true;
		}

		private static bool DoRaycast(Ray ray, out UIHotSpot.Hit hit, float dist)
		{
			float single;
			hit = UIHotSpot.Hit.invalid;
			UIHotSpot.Hit hit1 = UIHotSpot.Hit.invalid;
			bool flag = true;
			Vector3 vector3 = ray.origin;
			int num = UIHotSpot.Global.allCount;
			if (UIHotSpot.Global.Circle.any)
			{
				for (int i = 0; i < UIHotSpot.Global.Circle.count; i++)
				{
					UICircleHotSpot circle = UIHotSpot.Global.Circle.array[i];
					if ((circle._bounds.Contains(vector3) || circle._bounds.IntersectRay(ray, out single) && single < dist) && circle.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && circle.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num1 = num - 1;
						num = num1;
						if (num1 == 0)
						{
							return true;
						}
					}
					else
					{
						int num2 = num - 1;
						num = num2;
						if (num2 == 0)
						{
							return !flag;
						}
					}
				}
			}
			if (UIHotSpot.Global.Rect.any)
			{
				for (int j = 0; j < UIHotSpot.Global.Rect.count; j++)
				{
					UIRectHotSpot rect = UIHotSpot.Global.Rect.array[j];
					if ((rect._bounds.Contains(vector3) || rect._bounds.IntersectRay(ray, out single) && single < dist) && rect.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && rect.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num3 = num - 1;
						num = num3;
						if (num3 == 0)
						{
							return true;
						}
					}
					else
					{
						int num4 = num - 1;
						num = num4;
						if (num4 == 0)
						{
							return !flag;
						}
					}
				}
			}
			if (UIHotSpot.Global.Convex.any)
			{
				for (int k = 0; k < UIHotSpot.Global.Convex.count; k++)
				{
					UIConvexHotSpot convex = UIHotSpot.Global.Convex.array[k];
					if ((convex._bounds.Contains(vector3) || convex._bounds.IntersectRay(ray, out single) && single < dist) && convex.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && convex.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num5 = num - 1;
						num = num5;
						if (num5 == 0)
						{
							return true;
						}
					}
					else
					{
						int num6 = num - 1;
						num = num6;
						if (num6 == 0)
						{
							return !flag;
						}
					}
				}
			}
			if (UIHotSpot.Global.Sphere.any)
			{
				for (int l = 0; l < UIHotSpot.Global.Sphere.count; l++)
				{
					UISphereHotSpot sphere = UIHotSpot.Global.Sphere.array[l];
					if ((sphere._bounds.Contains(vector3) || sphere._bounds.IntersectRay(ray, out single) && single < dist) && sphere.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && sphere.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num7 = num - 1;
						num = num7;
						if (num7 == 0)
						{
							return true;
						}
					}
					else
					{
						int num8 = num - 1;
						num = num8;
						if (num8 == 0)
						{
							return !flag;
						}
					}
				}
			}
			if (UIHotSpot.Global.Box.any)
			{
				for (int m = 0; m < UIHotSpot.Global.Box.count; m++)
				{
					UIBoxHotSpot box = UIHotSpot.Global.Box.array[m];
					if ((box._bounds.Contains(vector3) || box._bounds.IntersectRay(ray, out single) && single < dist) && box.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && box.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num9 = num - 1;
						num = num9;
						if (num9 == 0)
						{
							return true;
						}
					}
					else
					{
						int num10 = num - 1;
						num = num10;
						if (num10 == 0)
						{
							return !flag;
						}
					}
				}
			}
			if (UIHotSpot.Global.Brush.any)
			{
				for (int n = 0; n < UIHotSpot.Global.Brush.count; n++)
				{
					UIBrushHotSpot brush = UIHotSpot.Global.Brush.array[n];
					if ((brush._bounds.Contains(vector3) || brush._bounds.IntersectRay(ray, out single) && single < dist) && brush.panel.InsideClippingRect(ray, UIHotSpot.Global.lastStepFrame) && brush.RaycastRef(ray, ref hit1) && hit1.distance < dist)
					{
						if (flag)
						{
							flag = false;
						}
						dist = hit1.distance;
						hit = hit1;
						int num11 = num - 1;
						num = num11;
						if (num11 == 0)
						{
							return true;
						}
					}
					else
					{
						int num12 = num - 1;
						num = num12;
						if (num12 == 0)
						{
							return !flag;
						}
					}
				}
			}
			throw new InvalidOperationException("Something is messed up. this line should never execute.");
		}

		private static Bounds? DoStep()
		{
			bool flag;
			bool flag1;
			Transform transforms;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			bool flag6;
			bool flag7;
			bool flag8;
			bool flag9;
			bool flag10;
			bool flag11;
			bool flag12;
			bool flag13;
			bool flag14;
			Bounds bound = new Bounds();
			bool flag15 = true;
			int num = UIHotSpot.Global.allCount;
			if (UIHotSpot.Global.Circle.any)
			{
				for (int i = 0; i < UIHotSpot.Global.Circle.count; i++)
				{
					UICircleHotSpot circle = UIHotSpot.Global.Circle.array[i];
					transforms = circle.transform;
					circle.lastWorld = circle.toWorld;
					circle.toWorld = transforms.localToWorldMatrix;
					circle.lastLocal = circle.toLocal;
					circle.toLocal = transforms.worldToLocalMatrix;
					bool flag16 = !circle.justAdded;
					flag2 = flag16;
					if (!flag16)
					{
						flag13 = true;
					}
					else
					{
						bool flag17 = UIHotSpot.Global.MatrixEquals(ref circle.toWorld, ref circle.lastWorld);
						flag2 = flag17;
						flag13 = !flag17;
					}
					flag1 = flag13;
					if (circle.justAdded)
					{
						flag14 = true;
					}
					else
					{
						flag14 = (!circle.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref circle.toLocal, ref circle.lastLocal));
					}
					flag = flag14;
					UICircleHotSpot uICircleHotSpot = circle;
					bool flag18 = flag1;
					Bounds? nullable = circle.Internal_CalculateBounds(flag);
					uICircleHotSpot.SetBounds(flag18, (!nullable.HasValue ? circle._bounds : nullable.Value), flag2);
					circle.justAdded = false;
					if (circle._bounds.size == Vector3.zero)
					{
						int num1 = num - 1;
						num = num1;
						if (num1 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(circle._bounds);
						int num2 = num - 1;
						num = num2;
						if (num2 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num3 = num - 1;
						num = num3;
						if (num3 == 0)
						{
							return new Bounds?(circle._bounds);
						}
						flag15 = false;
						bound = circle._bounds;
					}
				}
			}
			if (UIHotSpot.Global.Rect.any)
			{
				for (int j = 0; j < UIHotSpot.Global.Rect.count; j++)
				{
					UIRectHotSpot rect = UIHotSpot.Global.Rect.array[j];
					transforms = rect.transform;
					rect.lastWorld = rect.toWorld;
					rect.toWorld = transforms.localToWorldMatrix;
					rect.lastLocal = rect.toLocal;
					rect.toLocal = transforms.worldToLocalMatrix;
					bool flag19 = !rect.justAdded;
					flag2 = flag19;
					if (!flag19)
					{
						flag11 = true;
					}
					else
					{
						bool flag20 = UIHotSpot.Global.MatrixEquals(ref rect.toWorld, ref rect.lastWorld);
						flag2 = flag20;
						flag11 = !flag20;
					}
					flag1 = flag11;
					if (rect.justAdded)
					{
						flag12 = true;
					}
					else
					{
						flag12 = (!rect.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref rect.toLocal, ref rect.lastLocal));
					}
					flag = flag12;
					UIRectHotSpot uIRectHotSpot = rect;
					bool flag21 = flag1;
					Bounds? nullable1 = rect.Internal_CalculateBounds(flag);
					uIRectHotSpot.SetBounds(flag21, (!nullable1.HasValue ? rect._bounds : nullable1.Value), flag2);
					rect.justAdded = false;
					if (rect._bounds.size == Vector3.zero)
					{
						int num4 = num - 1;
						num = num4;
						if (num4 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(rect._bounds);
						int num5 = num - 1;
						num = num5;
						if (num5 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num6 = num - 1;
						num = num6;
						if (num6 == 0)
						{
							return new Bounds?(rect._bounds);
						}
						flag15 = false;
						bound = rect._bounds;
					}
				}
			}
			if (UIHotSpot.Global.Convex.any)
			{
				for (int k = 0; k < UIHotSpot.Global.Convex.count; k++)
				{
					UIConvexHotSpot convex = UIHotSpot.Global.Convex.array[k];
					transforms = convex.transform;
					convex.lastWorld = convex.toWorld;
					convex.toWorld = transforms.localToWorldMatrix;
					convex.lastLocal = convex.toLocal;
					convex.toLocal = transforms.worldToLocalMatrix;
					bool flag22 = !convex.justAdded;
					flag2 = flag22;
					if (!flag22)
					{
						flag9 = true;
					}
					else
					{
						bool flag23 = UIHotSpot.Global.MatrixEquals(ref convex.toWorld, ref convex.lastWorld);
						flag2 = flag23;
						flag9 = !flag23;
					}
					flag1 = flag9;
					if (convex.justAdded)
					{
						flag10 = true;
					}
					else
					{
						flag10 = (!convex.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref convex.toLocal, ref convex.lastLocal));
					}
					flag = flag10;
					UIConvexHotSpot uIConvexHotSpot = convex;
					bool flag24 = flag1;
					Bounds? nullable2 = convex.Internal_CalculateBounds(flag);
					uIConvexHotSpot.SetBounds(flag24, (!nullable2.HasValue ? convex._bounds : nullable2.Value), flag2);
					convex.justAdded = false;
					if (convex._bounds.size == Vector3.zero)
					{
						int num7 = num - 1;
						num = num7;
						if (num7 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(convex._bounds);
						int num8 = num - 1;
						num = num8;
						if (num8 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num9 = num - 1;
						num = num9;
						if (num9 == 0)
						{
							return new Bounds?(convex._bounds);
						}
						flag15 = false;
						bound = convex._bounds;
					}
				}
			}
			if (UIHotSpot.Global.Sphere.any)
			{
				for (int l = 0; l < UIHotSpot.Global.Sphere.count; l++)
				{
					UISphereHotSpot sphere = UIHotSpot.Global.Sphere.array[l];
					transforms = sphere.transform;
					sphere.lastWorld = sphere.toWorld;
					sphere.toWorld = transforms.localToWorldMatrix;
					sphere.lastLocal = sphere.toLocal;
					sphere.toLocal = transforms.worldToLocalMatrix;
					bool flag25 = !sphere.justAdded;
					flag2 = flag25;
					if (!flag25)
					{
						flag7 = true;
					}
					else
					{
						bool flag26 = UIHotSpot.Global.MatrixEquals(ref sphere.toWorld, ref sphere.lastWorld);
						flag2 = flag26;
						flag7 = !flag26;
					}
					flag1 = flag7;
					if (sphere.justAdded)
					{
						flag8 = true;
					}
					else
					{
						flag8 = (!sphere.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref sphere.toLocal, ref sphere.lastLocal));
					}
					flag = flag8;
					UISphereHotSpot uISphereHotSpot = sphere;
					bool flag27 = flag1;
					Bounds? nullable3 = sphere.Internal_CalculateBounds(flag);
					uISphereHotSpot.SetBounds(flag27, (!nullable3.HasValue ? sphere._bounds : nullable3.Value), flag2);
					sphere.justAdded = false;
					if (sphere._bounds.size == Vector3.zero)
					{
						int num10 = num - 1;
						num = num10;
						if (num10 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(sphere._bounds);
						int num11 = num - 1;
						num = num11;
						if (num11 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num12 = num - 1;
						num = num12;
						if (num12 == 0)
						{
							return new Bounds?(sphere._bounds);
						}
						flag15 = false;
						bound = sphere._bounds;
					}
				}
			}
			if (UIHotSpot.Global.Box.any)
			{
				for (int m = 0; m < UIHotSpot.Global.Box.count; m++)
				{
					UIBoxHotSpot box = UIHotSpot.Global.Box.array[m];
					transforms = box.transform;
					box.lastWorld = box.toWorld;
					box.toWorld = transforms.localToWorldMatrix;
					box.lastLocal = box.toLocal;
					box.toLocal = transforms.worldToLocalMatrix;
					bool flag28 = !box.justAdded;
					flag2 = flag28;
					if (!flag28)
					{
						flag5 = true;
					}
					else
					{
						bool flag29 = UIHotSpot.Global.MatrixEquals(ref box.toWorld, ref box.lastWorld);
						flag2 = flag29;
						flag5 = !flag29;
					}
					flag1 = flag5;
					if (box.justAdded)
					{
						flag6 = true;
					}
					else
					{
						flag6 = (!box.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref box.toLocal, ref box.lastLocal));
					}
					flag = flag6;
					UIBoxHotSpot uIBoxHotSpot = box;
					bool flag30 = flag1;
					Bounds? nullable4 = box.Internal_CalculateBounds(flag);
					uIBoxHotSpot.SetBounds(flag30, (!nullable4.HasValue ? box._bounds : nullable4.Value), flag2);
					box.justAdded = false;
					if (box._bounds.size == Vector3.zero)
					{
						int num13 = num - 1;
						num = num13;
						if (num13 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(box._bounds);
						int num14 = num - 1;
						num = num14;
						if (num14 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num15 = num - 1;
						num = num15;
						if (num15 == 0)
						{
							return new Bounds?(box._bounds);
						}
						flag15 = false;
						bound = box._bounds;
					}
				}
			}
			if (UIHotSpot.Global.Brush.any)
			{
				for (int n = 0; n < UIHotSpot.Global.Brush.count; n++)
				{
					UIBrushHotSpot brush = UIHotSpot.Global.Brush.array[n];
					transforms = brush.transform;
					brush.lastWorld = brush.toWorld;
					brush.toWorld = transforms.localToWorldMatrix;
					brush.lastLocal = brush.toLocal;
					brush.toLocal = transforms.worldToLocalMatrix;
					bool flag31 = !brush.justAdded;
					flag2 = flag31;
					if (!flag31)
					{
						flag3 = true;
					}
					else
					{
						bool flag32 = UIHotSpot.Global.MatrixEquals(ref brush.toWorld, ref brush.lastWorld);
						flag2 = flag32;
						flag3 = !flag32;
					}
					flag1 = flag3;
					if (brush.justAdded)
					{
						flag4 = true;
					}
					else
					{
						flag4 = (!brush.configuredInLocalSpace ? flag1 : !UIHotSpot.Global.MatrixEquals(ref brush.toLocal, ref brush.lastLocal));
					}
					flag = flag4;
					UIBrushHotSpot uIBrushHotSpot = brush;
					bool flag33 = flag1;
					Bounds? nullable5 = brush.Internal_CalculateBounds(flag);
					uIBrushHotSpot.SetBounds(flag33, (!nullable5.HasValue ? brush._bounds : nullable5.Value), flag2);
					brush.justAdded = false;
					if (brush._bounds.size == Vector3.zero)
					{
						int num16 = num - 1;
						num = num16;
						if (num16 == 0)
						{
							return null;
						}
					}
					else if (!flag15)
					{
						bound.Encapsulate(brush._bounds);
						int num17 = num - 1;
						num = num17;
						if (num17 == 0)
						{
							return new Bounds?(bound);
						}
					}
					else
					{
						int num18 = num - 1;
						num = num18;
						if (num18 == 0)
						{
							return new Bounds?(brush._bounds);
						}
						flag15 = false;
						bound = brush._bounds;
					}
				}
			}
			throw new InvalidOperationException("Something is messed up. this line should never execute.");
		}

		private static bool MatrixEquals(ref Matrix4x4 a, ref Matrix4x4 b)
		{
			return (a.m03 != b.m03 || a.m12 != b.m13 || a.m23 != b.m23 || a.m00 != b.m00 || a.m11 != b.m11 || a.m22 != b.m22 || a.m01 != b.m01 || a.m12 != b.m12 || a.m20 != b.m20 || a.m02 != b.m02 || a.m10 != b.m10 || a.m21 != b.m21 || a.m30 != b.m30 || a.m31 != b.m31 || a.m32 != b.m32 ? false : a.m33 == b.m33);
		}

		public static bool Raycast(Ray ray, out UIHotSpot.Hit hit, float distance)
		{
			float single;
			if (!UIHotSpot.Global.allAny)
			{
				hit = UIHotSpot.Hit.invalid;
				return false;
			}
			int num = Time.frameCount;
			if (UIHotSpot.Global.lastStepFrame != num || UIHotSpot.Global.anyRemovedRecently || UIHotSpot.Global.anyAddedRecently)
			{
				UIHotSpot.Global.Step();
				int num1 = 0;
				UIHotSpot.Global.anyAddedRecently = (bool)num1;
				UIHotSpot.Global.anyRemovedRecently = (bool)num1;
			}
			UIHotSpot.Global.lastStepFrame = num;
			if (!UIHotSpot.Global.validBounds)
			{
				hit = UIHotSpot.Hit.invalid;
				return false;
			}
			if (!UIHotSpot.Global.allBounds.Contains(ray.origin))
			{
				if (!UIHotSpot.Global.allBounds.IntersectRay(ray, out single) || single > distance)
				{
					hit = UIHotSpot.Hit.invalid;
					return false;
				}
				if (single != 0f)
				{
					ray.origin = ray.GetPoint(single - 0.001f);
					single = 0f;
				}
			}
			else
			{
				single = 0f;
			}
			return UIHotSpot.Global.DoRaycast(ray, out hit, distance);
		}

		public static bool Remove(UIHotSpot hotSpot)
		{
			if (hotSpot.index == -1)
			{
				return false;
			}
			UIHotSpot.Kind kind = hotSpot.kind;
			switch (kind)
			{
				case UIHotSpot.Kind.Circle:
				{
					UIHotSpot.Global.Circle.Erase((UICircleHotSpot)hotSpot);
					break;
				}
				case UIHotSpot.Kind.Rect:
				{
					UIHotSpot.Global.Rect.Erase((UIRectHotSpot)hotSpot);
					break;
				}
				case UIHotSpot.Kind.Convex:
				{
					UIHotSpot.Global.Convex.Erase((UIConvexHotSpot)hotSpot);
					break;
				}
				default:
				{
					switch (kind)
					{
						case UIHotSpot.Kind.Sphere:
						{
							UIHotSpot.Global.Sphere.Erase((UISphereHotSpot)hotSpot);
							return true;
						}
						case UIHotSpot.Kind.Box:
						{
							UIHotSpot.Global.Box.Erase((UIBoxHotSpot)hotSpot);
							return true;
						}
						case UIHotSpot.Kind.Brush:
						{
							UIHotSpot.Global.Brush.Erase((UIBrushHotSpot)hotSpot);
							return true;
						}
					}
					throw new NotImplementedException();
				}
			}
			return true;
		}

		public static void Step()
		{
			if (!UIHotSpot.Global.allAny)
			{
				UIHotSpot.Global.validBounds = false;
			}
			else
			{
				Bounds? nullable = UIHotSpot.Global.DoStep();
				UIHotSpot.Global.validBounds = nullable.HasValue;
				if (UIHotSpot.Global.validBounds)
				{
					UIHotSpot.Global.allBounds = nullable.Value;
				}
			}
		}

		private struct List<THotSpot>
		where THotSpot : UIHotSpot
		{
			public THotSpot[] array;

			public int count;

			public int capacity;

			public bool any;

			public void Add(THotSpot hotSpot)
			{
				object obj = hotSpot;
				UIHotSpot.Global.List<THotSpot> list = this;
				int num = list.count;
				int num1 = num;
				list.count = num + 1;
				obj.index = num1;
				if (hotSpot.index == this.capacity)
				{
					UIHotSpot.Global.List<THotSpot> list1 = this;
					list1.capacity = list1.capacity + 8;
					Array.Resize<THotSpot>(ref this.array, this.capacity);
				}
				this.array[hotSpot.index] = hotSpot;
				this.any = true;
				int num2 = UIHotSpot.Global.allCount;
				UIHotSpot.Global.allCount = num2 + 1;
				if (num2 == 0)
				{
					UIHotSpot.Global.allAny = true;
				}
				UIHotSpot.Global.anyAddedRecently = true;
			}

			public void Erase(THotSpot hotSpot)
			{
				UIHotSpot.Global.allCount = UIHotSpot.Global.allCount - 1;
				UIHotSpot.Global.List<THotSpot> list = this;
				int num = list.count - 1;
				int num1 = num;
				list.count = num;
				if (num1 != hotSpot.index)
				{
					THotSpot[] tHotSpotArray = this.array;
					int num2 = hotSpot.index;
					THotSpot tHotSpot = this.array[this.count];
					THotSpot tHotSpot1 = tHotSpot;
					tHotSpotArray[num2] = tHotSpot;
					tHotSpot1.index = hotSpot.index;
					this.array[this.count] = (THotSpot)null;
				}
				else
				{
					this.array[hotSpot.index] = (THotSpot)null;
					this.any = this.count > 0;
					if (!this.any)
					{
						UIHotSpot.Global.allAny = UIHotSpot.Global.allCount > 0;
					}
				}
				hotSpot.index = -1;
				UIHotSpot.Global.anyRemovedRecently = true;
			}
		}
	}

	public struct Hit
	{
		public UIHotSpot hotSpot;

		public Collider collider;

		public UIPanel panel;

		public Vector3 point;

		public Vector3 normal;

		public Ray ray;

		public float distance;

		public bool isCollider;

		public readonly static UIHotSpot.Hit invalid;

		public Component component
		{
			get
			{
				Component component;
				if (!this.isCollider)
				{
					component = this.hotSpot;
				}
				else
				{
					component = this.collider;
				}
				return component;
			}
		}

		public GameObject gameObject
		{
			get
			{
				GameObject gameObject;
				if (this.isCollider)
				{
					gameObject = this.collider.gameObject;
				}
				else if (!this.hotSpot)
				{
					gameObject = null;
				}
				else
				{
					gameObject = this.hotSpot.gameObject;
				}
				return gameObject;
			}
		}

		public Transform transform
		{
			get
			{
				Transform transforms;
				if (this.isCollider)
				{
					transforms = this.collider.transform;
				}
				else if (!this.hotSpot)
				{
					transforms = null;
				}
				else
				{
					transforms = this.hotSpot.transform;
				}
				return transforms;
			}
		}

		static Hit()
		{
			UIHotSpot.Hit hit = new UIHotSpot.Hit()
			{
				distance = Single.PositiveInfinity,
				ray = new Ray(),
				point = new Vector3(),
				normal = new Vector3()
			};
			UIHotSpot.Hit.invalid = hit;
		}
	}

	public enum Kind
	{
		Circle = 0,
		Rect = 1,
		Convex = 2,
		Sphere = 128,
		Box = 129,
		Brush = 130
	}
}