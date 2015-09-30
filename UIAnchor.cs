using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	public Camera uiCamera;

	public UIAnchor.Side side = UIAnchor.Side.Center;

	public bool halfPixelOffset = true;

	public bool otherThingsMightMoveThis;

	public float depthOffset;

	public Vector2 relativeOffset = Vector2.zero;

	[NonSerialized]
	private Transform __mTrans;

	[NonSerialized]
	private bool mTransGot;

	[NonSerialized]
	private bool mOnce;

	[NonSerialized]
	private Vector3 mLastPosition;

	protected Transform mTrans
	{
		get
		{
			if (!this.mTransGot)
			{
				this.__mTrans = base.transform;
				this.mTransGot = true;
			}
			return this.__mTrans;
		}
	}

	public UIAnchor()
	{
	}

	private void OnEnable()
	{
		if (!this.uiCamera)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
	}

	public static void ScreenOrigin(UIAnchor.Side side, float xMin, float xMax, float yMin, float yMax, out float x, out float y)
	{
		switch (side)
		{
			case UIAnchor.Side.BottomLeft:
			{
				x = xMin;
				y = yMin;
				break;
			}
			case UIAnchor.Side.Left:
			{
				x = xMin;
				y = (yMin + yMax) / 2f;
				break;
			}
			case UIAnchor.Side.TopLeft:
			{
				x = xMin;
				y = yMax;
				break;
			}
			case UIAnchor.Side.Top:
			{
				x = (xMin + xMax) / 2f;
				y = yMax;
				break;
			}
			case UIAnchor.Side.TopRight:
			{
				x = xMax;
				y = yMax;
				break;
			}
			case UIAnchor.Side.Right:
			{
				x = xMax;
				y = (yMin + yMax) / 2f;
				break;
			}
			case UIAnchor.Side.BottomRight:
			{
				x = xMax;
				y = yMin;
				break;
			}
			case UIAnchor.Side.Bottom:
			{
				x = (xMin + xMax) / 2f;
				y = yMin;
				break;
			}
			case UIAnchor.Side.Center:
			{
				x = (xMin + xMax) / 2f;
				y = (yMin + yMax) / 2f;
				break;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public static void ScreenOrigin(UIAnchor.Side side, float xMin, float xMax, float yMin, float yMax, float relativeOffsetX, float relativeOffsetY, out float x, out float y)
	{
		float single;
		float single1;
		UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, out single, out single1);
		x = single + relativeOffsetX * (xMax - xMin);
		y = single1 + relativeOffsetY * (yMax - yMin);
	}

	public static void ScreenOrigin(UIAnchor.Side side, float xMin, float xMax, float yMin, float yMax, float relativeOffsetX, float relativeOffsetY, UIAnchor.Flags flags, out float x, out float y)
	{
		float single;
		float single1;
		float single2;
		float single3;
		switch ((byte)(flags & (UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset)))
		{
			case 1:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, relativeOffsetX, relativeOffsetY, out single, out single1);
				x = Mathf.Round(single);
				y = Mathf.Round(single1);
				break;
			}
			case 2:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, relativeOffsetX, relativeOffsetY, out x, out y);
				break;
			}
			case 3:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, relativeOffsetX, relativeOffsetY, out single2, out single3);
				x = Mathf.Round(single2) - 0.5f;
				y = Mathf.Round(single3) + 0.5f;
				break;
			}
			default:
			{
				goto case 2;
			}
		}
	}

	public static void ScreenOrigin(UIAnchor.Side side, float xMin, float xMax, float yMin, float yMax, UIAnchor.Flags flags, out float x, out float y)
	{
		float single;
		float single1;
		float single2;
		float single3;
		switch ((byte)(flags & (!UIAnchor.Info.isWindows ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset)))
		{
			case 1:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, out single, out single1);
				x = Mathf.Round(single);
				y = Mathf.Round(single1);
				break;
			}
			case 2:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, out x, out y);
				break;
			}
			case 3:
			{
				UIAnchor.ScreenOrigin(side, xMin, xMax, yMin, yMax, out single2, out single3);
				x = Mathf.Round(single2) - 0.5f;
				y = Mathf.Round(single3) + 0.5f;
				break;
			}
			default:
			{
				goto case 2;
			}
		}
	}

	protected void SetPosition(ref Vector3 newPosition)
	{
		Transform transforms = this.mTrans;
		if (this.otherThingsMightMoveThis || !this.mOnce)
		{
			this.mLastPosition = transforms.position;
			this.mOnce = true;
		}
		if (newPosition.x != this.mLastPosition.x || newPosition.y != this.mLastPosition.y || newPosition.z != this.mLastPosition.z)
		{
			transforms.position = newPosition;
		}
	}

	protected void Update()
	{
		if (this.uiCamera)
		{
			Vector3 vector3 = UIAnchor.WorldOrigin(this.uiCamera, this.side, this.depthOffset, this.relativeOffset.x, this.relativeOffset.y, this.halfPixelOffset);
			this.SetPosition(ref vector3);
		}
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, float depthOffset, float relativeOffsetX, float relativeOffsetY, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = depthOffset;
		Rect rect = camera.pixelRect;
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		float single8 = relativeOffsetX;
		float single9 = relativeOffsetY;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, single8, single9, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, float relativeOffsetX, float relativeOffsetY, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = 0f;
		Rect rect = camera.pixelRect;
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		float single8 = relativeOffsetX;
		float single9 = relativeOffsetY;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, single8, single9, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, float depthOffset, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = depthOffset;
		Rect rect = camera.pixelRect;
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = 0f;
		Rect rect = camera.pixelRect;
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, RectOffset offset, float depthOffset, float relativeOffsetX, float relativeOffsetY, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = depthOffset;
		Rect rect = offset.Add(camera.pixelRect);
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		float single8 = relativeOffsetX;
		float single9 = relativeOffsetY;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, single8, single9, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, RectOffset offset, float relativeOffsetX, float relativeOffsetY, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = 0f;
		Rect rect = offset.Add(camera.pixelRect);
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		float single8 = relativeOffsetX;
		float single9 = relativeOffsetY;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, single8, single9, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, RectOffset offset, float depthOffset, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = depthOffset;
		Rect rect = offset.Add(camera.pixelRect);
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	public static Vector3 WorldOrigin(Camera camera, UIAnchor.Side side, RectOffset offset, bool halfPixel)
	{
		Vector3 vector3 = new Vector3();
		UIAnchor.Flags flag;
		vector3.z = 0f;
		Rect rect = offset.Add(camera.pixelRect);
		float single = rect.xMin;
		float single1 = rect.xMax;
		float single2 = rect.yMin;
		float single3 = rect.yMax;
		UIAnchor.Side side1 = side;
		float single4 = single;
		float single5 = single1;
		float single6 = single2;
		float single7 = single3;
		if (camera.isOrthoGraphic)
		{
			flag = (!halfPixel ? UIAnchor.Flags.CameraIsOrthographic : UIAnchor.Flags.CameraIsOrthographic | UIAnchor.Flags.HalfPixelOffset);
		}
		else if (!halfPixel)
		{
			flag = (UIAnchor.Flags)0;
		}
		else
		{
			flag = UIAnchor.Flags.HalfPixelOffset;
		}
		UIAnchor.ScreenOrigin(side1, single4, single5, single6, single7, flag, out vector3.x, out vector3.y);
		return camera.ScreenToWorldPoint(vector3);
	}

	[Flags]
	public enum Flags : byte
	{
		CameraIsOrthographic = 1,
		HalfPixelOffset = 2
	}

	protected static class Info
	{
		public readonly static bool isWindows;

		static Info()
		{
			RuntimePlatform runtimePlatform = Application.platform;
			UIAnchor.Info.isWindows = (runtimePlatform == RuntimePlatform.WindowsPlayer || runtimePlatform == RuntimePlatform.WindowsWebPlayer ? true : runtimePlatform == RuntimePlatform.WindowsEditor);
		}
	}

	public enum Side
	{
		BottomLeft,
		Left,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		Center
	}
}