using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class HUDIndicator : MonoBehaviour
{
	private static double _stepTime;

	protected static Matrix4x4 worldToCameraLocalMatrix;

	private static double _lastTime;

	private UIAnchor anchor;

	private int listIndex = -1;

	private static HUDIndicator.Target CenterFixed3000Tall;

	private static HUDIndicator.Target CenterAuto;

	protected static double stepTime
	{
		get
		{
			return HUDIndicator._stepTime;
		}
	}

	protected static double time
	{
		get
		{
			double num;
			if (!DebugInput.GetKey(KeyCode.Period))
			{
				num = NetCull.time;
				HUDIndicator._lastTime = num;
			}
			else
			{
				num = HUDIndicator._lastTime;
			}
			return num;
		}
	}

	static HUDIndicator()
	{
		HUDIndicator.worldToCameraLocalMatrix = Matrix4x4.identity;
		HUDIndicator.CenterFixed3000Tall = new HUDIndicator.Target("HUD_INDICATOR_CENTER_3000", 3000, UIAnchor.Side.Center);
		HUDIndicator.CenterAuto = new HUDIndicator.Target("HUD_INDICATOR_CENTER_AUTO", UIAnchor.Side.Center);
	}

	protected HUDIndicator()
	{
	}

	protected abstract bool Continue();

	protected Vector3 GetPoint(HUDIndicator.PlacementSpace space, Vector3 input)
	{
		switch (space)
		{
			case HUDIndicator.PlacementSpace.World:
			{
				Camera camera = HUDIndicator.Target.camera;
				Vector3? nullable = CameraFX.World2Screen(input);
				input = camera.ScreenToWorldPoint((!nullable.HasValue ? Vector3.zero : nullable.Value));
				break;
			}
			case HUDIndicator.PlacementSpace.Screen:
			{
				input = HUDIndicator.Target.camera.ScreenToWorldPoint(input);
				break;
			}
			case HUDIndicator.PlacementSpace.Viewport:
			{
				input = HUDIndicator.Target.camera.ViewportToWorldPoint(input);
				break;
			}
			case HUDIndicator.PlacementSpace.Anchor:
			{
				input = this.anchor.transform.TransformPoint(input);
				break;
			}
		}
		return input;
	}

	private static HUDIndicator InstantiateIndicator(ref HUDIndicator.Target target, HUDIndicator prefab, HUDIndicator.PlacementSpace space, Vector3 position, float rotation)
	{
		UIAnchor uIAnchor = target.anchor;
		Quaternion quaternion = Quaternion.AngleAxis(rotation, Vector3.back);
		switch (space)
		{
			case HUDIndicator.PlacementSpace.World:
			{
				Camera camera = HUDIndicator.Target.camera;
				Vector3? nullable = CameraFX.World2Screen(position);
				position = camera.ScreenToWorldPoint((!nullable.HasValue ? Vector3.zero : nullable.Value));
				break;
			}
			case HUDIndicator.PlacementSpace.Screen:
			{
				position = HUDIndicator.Target.camera.ScreenToWorldPoint(position);
				break;
			}
			case HUDIndicator.PlacementSpace.Viewport:
			{
				position = HUDIndicator.Target.camera.ViewportToWorldPoint(position);
				break;
			}
			case HUDIndicator.PlacementSpace.Anchor:
			{
				position = uIAnchor.transform.TransformPoint(position);
				quaternion = uIAnchor.transform.rotation * quaternion;
				break;
			}
		}
		position.z = uIAnchor.transform.position.z;
		HUDIndicator hUDIndicator = (HUDIndicator)UnityEngine.Object.Instantiate(prefab, position, quaternion);
		hUDIndicator.transform.parent = uIAnchor.transform;
		hUDIndicator.transform.localScale = Vector3.one;
		hUDIndicator.anchor = target.anchor;
		return hUDIndicator;
	}

	protected static HUDIndicator InstantiateIndicator(HUDIndicator.ScratchTarget target, HUDIndicator prefab, HUDIndicator.PlacementSpace space, Vector3 position, float rotation)
	{
		HUDIndicator.ScratchTarget scratchTarget = target;
		if (scratchTarget == HUDIndicator.ScratchTarget.CenteredAuto)
		{
			return HUDIndicator.InstantiateIndicator(ref HUDIndicator.CenterAuto, prefab, space, position, rotation);
		}
		if (scratchTarget != HUDIndicator.ScratchTarget.CenteredFixed3000Tall)
		{
			throw new ArgumentOutOfRangeException("target");
		}
		return HUDIndicator.InstantiateIndicator(ref HUDIndicator.CenterFixed3000Tall, prefab, space, position, rotation);
	}

	protected static HUDIndicator InstantiateIndicator(HUDIndicator.ScratchTarget target, HUDIndicator prefab, HUDIndicator.PlacementSpace space, Vector3 position)
	{
		return HUDIndicator.InstantiateIndicator(target, prefab, space, position, 0f);
	}

	protected static HUDIndicator InstantiateIndicator(HUDIndicator.ScratchTarget target, HUDIndicator prefab)
	{
		return HUDIndicator.InstantiateIndicator(target, prefab, HUDIndicator.PlacementSpace.Anchor, Vector3.zero, 0f);
	}

	protected void OnDestroy()
	{
		if (this.listIndex != -1)
		{
			HUDIndicator.INDICATOR.Remove(this);
		}
	}

	protected void Start()
	{
		if (this.Continue())
		{
			HUDIndicator.INDICATOR.Add(this);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	internal static void Step()
	{
		HUDIndicator._stepTime = HUDIndicator.time;
		Camera camera = Camera.main;
		if (camera)
		{
			HUDIndicator.worldToCameraLocalMatrix = Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * camera.worldToCameraMatrix;
		}
		int count = HUDIndicator.INDICATOR.activeIndicators.Count;
		int num = count - 1;
		while (num >= 0)
		{
			if (HUDIndicator.INDICATOR.activeIndicators[num].Continue())
			{
				num--;
			}
			else
			{
				int num1 = num;
				do
				{
					HUDIndicator item = HUDIndicator.INDICATOR.activeIndicators[num];
					HUDIndicator.INDICATOR.activeIndicators.RemoveAt(num);
					item.listIndex = -1;
					UnityEngine.Object.Destroy(item.gameObject);
					count--;
					int num2 = num - 1;
					num = num2;
					if (num2 < 0)
					{
						continue;
					}
					while (HUDIndicator.INDICATOR.activeIndicators[num].Continue())
					{
						int num3 = num - 1;
						num = num3;
						if (num3 < 0)
						{
							goto Label0;
						}
					}
					num1 = num;
				Label0:
				}
				while (num1 == num);
				while (num1 < count)
				{
					HUDIndicator.INDICATOR.activeIndicators[num1].listIndex = num1;
					num1++;
				}
				break;
			}
		}
	}

	private static class INDICATOR
	{
		public static List<HUDIndicator> activeIndicators;

		static INDICATOR()
		{
			HUDIndicator.INDICATOR.activeIndicators = new List<HUDIndicator>();
		}

		public static void Add(HUDIndicator hud)
		{
			if (hud.listIndex != -1)
			{
				return;
			}
			hud.listIndex = HUDIndicator.INDICATOR.activeIndicators.Count;
			HUDIndicator.INDICATOR.activeIndicators.Add(hud);
		}

		public static void Remove(HUDIndicator hud)
		{
			if (hud.listIndex == -1)
			{
				return;
			}
			try
			{
				HUDIndicator.INDICATOR.activeIndicators.RemoveAt(hud.listIndex);
				int num = hud.listIndex;
				int count = HUDIndicator.INDICATOR.activeIndicators.Count;
				while (num < count)
				{
					HUDIndicator.INDICATOR.activeIndicators[num].listIndex = num;
					num++;
				}
			}
			finally
			{
				hud.listIndex = -1;
			}
		}
	}

	protected enum PlacementSpace
	{
		World,
		Screen,
		Viewport,
		Anchor,
		DoNotModify
	}

	protected enum ScratchTarget
	{
		CenteredAuto,
		CenteredFixed3000Tall
	}

	private struct Target
	{
		private const int kDefaultManualSize = 1000;

		private const UIAnchor.Side kDefaultSide = UIAnchor.Side.Center;

		private UIRoot _root;

		private UIAnchor _anchor;

		public readonly string name;

		public readonly bool automatic;

		public readonly int manualSize;

		public readonly UIAnchor.Side side;

		private static UICamera _uiCamera;

		public UIAnchor anchor
		{
			get
			{
				if (!this._anchor)
				{
					UIRoot uIRoot = this.root;
					GameObject gameObject = new GameObject("ANCHOR", new Type[] { typeof(UIAnchor) })
					{
						layer = HUDIndicator.Target.g.layer
					};
					this._anchor = gameObject.GetComponent<UIAnchor>();
					this._anchor.transform.parent = uIRoot.transform;
					this._anchor.side = this.side;
					this._anchor.uiCamera = HUDIndicator.Target.camera;
				}
				return this._anchor;
			}
		}

		public static Camera camera
		{
			get
			{
				Camera camera;
				UICamera uICamera = HUDIndicator.Target.uiCamera;
				if (!uICamera)
				{
					camera = null;
				}
				else
				{
					camera = uICamera.cachedCamera;
				}
				return camera;
			}
		}

		public UIRoot root
		{
			get
			{
				if (!this._root)
				{
					GameObject gameObject = new GameObject(this.name, new Type[] { typeof(UIRoot) })
					{
						layer = HUDIndicator.Target.g.layer
					};
					this._root = gameObject.GetComponent<UIRoot>();
					this._root.automatic = this.automatic;
					this._root.manualHeight = this.manualSize;
				}
				return this._root;
			}
		}

		public static UICamera uiCamera
		{
			get
			{
				if (!HUDIndicator.Target._uiCamera)
				{
					HUDIndicator.Target._uiCamera = UICamera.FindCameraForLayer(HUDIndicator.Target.g.layer);
				}
				return HUDIndicator.Target._uiCamera;
			}
		}

		public Target(string name) : this(name, true, 1000, UIAnchor.Side.Center)
		{
		}

		public Target(string name, int manualSize) : this(name, false, manualSize, UIAnchor.Side.Center)
		{
		}

		public Target(string name, UIAnchor.Side side) : this(name, true, 1000, side)
		{
		}

		public Target(string name, int manualSize, UIAnchor.Side side) : this(name, false, manualSize, side)
		{
		}

		private Target(string name, bool automatic, int manualSize, UIAnchor.Side side)
		{
			this.automatic = automatic;
			this.manualSize = manualSize;
			this.name = name;
			this.side = side;
			this._root = null;
			this._anchor = null;
		}

		private static class g
		{
			public readonly static int layer;

			static g()
			{
				HUDIndicator.Target.g.layer = LayerMask.NameToLayer("NGUILayer2D");
			}
		}
	}
}