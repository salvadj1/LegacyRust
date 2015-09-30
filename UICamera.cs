using NGUI.MessageUtil;
using NGUIHack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	private const int kMouseButton0Flag = 1;

	private const int kMouseButton1Flag = 2;

	private const int kMouseButton2Flag = 4;

	private const int kMouseButton3Flag = 8;

	private const int kMouseButton4Flag = 16;

	private const int kMouseButtonCount = 3;

	private static UIPanel popupPanel;

	public static UICamera.BackwardsCompatabilitySupport currentTouch;

	public static bool SwallowScroll;

	public bool useMouse = true;

	public bool useTouch = true;

	public bool allowMultiTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public LayerMask eventReceiverMask = -1;

	public float tooltipDelay = 1f;

	public bool stickyTooltip = true;

	public float mouseClickThreshold = 10f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public string verticalAxisName = "Vertical";

	public string horizontalAxisName = "Horizontal";

	public KeyCode submitKey0 = KeyCode.Return;

	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	public KeyCode cancelKey0 = KeyCode.Escape;

	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	public UIInputMode mouseMode = UIInputMode.UseEvents;

	public UIInputMode keyboardMode = UIInputMode.UseInputAndEvents;

	public UIInputMode scrollWheelMode = UIInputMode.UseEvents;

	public bool onlyHotSpots;

	public static Vector2 lastTouchPosition;

	public static Vector2 lastMousePosition;

	public static UIHotSpot.Hit lastHit;

	public static Camera currentCamera;

	public static int currentTouchID;

	public static bool inputHasFocus;

	public static GameObject fallThrough;

	private static UICamera[] mList;

	private static byte[] mListSort;

	private static int mListCount;

	private static Dictionary<int, UICamera> mMouseCamera;

	private static Dictionary<KeyCode, UICamera> mKeyCamera;

	private static List<UICamera.Highlighted> mHighlighted;

	private static GameObject mSel;

	private static UIInput mSelInput;

	private static UIInput mPressInput;

	private static GameObject mHover;

	private static float mNextEvent;

	private GameObject mTooltip;

	private Camera mCam;

	private LayerMask mLayerMask;

	private float mTooltipTime;

	private bool mIsEditor;

	private int lastBoundLayerIndex = -1;

	private static bool inSelectionCallback;

	private readonly static UICamera.CamSorter sorter;

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	public static UICamera.CursorSampler Cursor
	{
		get
		{
			return UICamera.LateLoadCursor.Sampler;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			return UICamera.mList[UICamera.mListSort[0]];
		}
	}

	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			return UICamera.mHover;
		}
	}

	public static bool IsPressing
	{
		get
		{
			bool pressed;
			if (!UICamera.Cursor.Buttons.LeftValue.Held)
			{
				pressed = false;
			}
			else
			{
				pressed = UICamera.Cursor.Buttons.LeftValue.Pressed;
			}
			return pressed;
		}
	}

	[Obsolete("Use UICamera.currentCamera instead")]
	public static Camera lastCamera
	{
		get
		{
			return UICamera.currentCamera;
		}
	}

	[Obsolete("Use UICamera.currentTouchID instead")]
	public static int lastTouchID
	{
		get
		{
			return UICamera.currentTouchID;
		}
	}

	public static Camera mainCamera
	{
		get
		{
			Camera camera;
			UICamera uICamera = UICamera.eventHandler;
			if (uICamera == null)
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

	public static GameObject selectedObject
	{
		get
		{
			return UICamera.mSel;
		}
		set
		{
			if (!UICamera.SetSelectedObject(value))
			{
				throw new InvalidOperationException("Do not set selectedObject within a OnSelect message.");
			}
		}
	}

	public bool usesAnyEvents
	{
		get
		{
			return ((this.mouseMode | this.keyboardMode | this.scrollWheelMode) & UIInputMode.UseEvents) == UIInputMode.UseEvents;
		}
	}

	static UICamera()
	{
		UICamera.lastTouchPosition = Vector2.zero;
		UICamera.lastMousePosition = Vector2.zero;
		UICamera.currentCamera = null;
		UICamera.currentTouchID = -1;
		UICamera.inputHasFocus = false;
		UICamera.mList = new UICamera[32];
		UICamera.mListSort = new byte[32];
		UICamera.mListCount = 0;
		UICamera.mMouseCamera = new Dictionary<int, UICamera>();
		UICamera.mKeyCamera = new Dictionary<KeyCode, UICamera>();
		UICamera.mHighlighted = new List<UICamera.Highlighted>();
		UICamera.mSel = null;
		UICamera.mSelInput = null;
		UICamera.mPressInput = null;
		UICamera.mNextEvent = 0f;
		UICamera.sorter = new UICamera.CamSorter();
	}

	public UICamera()
	{
	}

	private void AddToList()
	{
		bool flag;
		int num = base.gameObject.layer;
		if (num != this.lastBoundLayerIndex)
		{
			if (this.lastBoundLayerIndex == -1 || !(UICamera.mList[this.lastBoundLayerIndex] == this))
			{
				byte[] numArray = UICamera.mListSort;
				int num1 = UICamera.mListCount;
				UICamera.mListCount = num1 + 1;
				numArray[num1] = (byte)num;
				flag = true;
			}
			else
			{
				UICamera.mList[this.lastBoundLayerIndex] = null;
				for (int i = 0; i < UICamera.mListCount; i++)
				{
					if (UICamera.mListSort[i] == this.lastBoundLayerIndex)
					{
						UICamera.mListSort[i] = (byte)num;
					}
				}
				flag = false;
			}
			UICamera.mList[num] = this;
			this.lastBoundLayerIndex = num;
			if (flag)
			{
				Array.Sort<byte>(UICamera.mListSort, 0, UICamera.mListCount, UICamera.sorter);
			}
		}
	}

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.useMouse = false;
			this.useTouch = true;
			this.useKeyboard = false;
			this.useController = false;
		}
		else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360)
		{
			this.useMouse = false;
			this.useTouch = false;
			this.useKeyboard = false;
			this.useController = true;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			this.mIsEditor = true;
		}
		this.AddToList();
		if (this.eventReceiverMask == -1)
		{
			this.eventReceiverMask = base.camera.cullingMask;
		}
		if (this.usesAnyEvents && Application.isPlaying)
		{
			UIUnityEvents.CameraCreated(this);
		}
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

	private static int CompareFunc(UICamera a, UICamera b)
	{
		return b.cachedCamera.depth.CompareTo(a.cachedCamera.depth);
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		return UICamera.mList[layer];
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		bool keyDown = Input.GetKeyDown(up);
		if (keyDown == Input.GetKeyDown(down))
		{
			return 0;
		}
		if (keyDown)
		{
			return (!Input.GetKey(down) ? 1 : 0);
		}
		return (!Input.GetKey(up) ? -1 : 0);
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		bool keyDown = Input.GetKeyDown(up0) | Input.GetKeyDown(up1);
		if (keyDown == (Input.GetKeyDown(down0) | Input.GetKeyDown(down1)))
		{
			return 0;
		}
		if (keyDown)
		{
			return (Input.GetKey(down0) || Input.GetKey(down1) ? 0 : 1);
		}
		return (Input.GetKey(up0) || Input.GetKey(up1) ? 0 : -1);
	}

	private static int GetDirection(string axis)
	{
		float single = Time.realtimeSinceStartup;
		if (UICamera.mNextEvent < single)
		{
			float single1 = Input.GetAxis(axis);
			if (single1 > 0.75f)
			{
				UICamera.mNextEvent = single + 0.25f;
				return 1;
			}
			if (single1 < -0.75f)
			{
				UICamera.mNextEvent = single + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static void HandleEvent(NGUIHack.Event @event, EventType type)
	{
		switch (type)
		{
			case EventType.MouseDown:
			{
				UICamera.Mouse.Button.ButtonPressEventHandler buttonPressEventHandler = new UICamera.Mouse.Button.ButtonPressEventHandler(@event);
				try
				{
					UICamera.IssueEvent(@event, EventType.MouseDown);
				}
				finally
				{
					((IDisposable)(object)buttonPressEventHandler).Dispose();
				}
				return;
			}
			case EventType.MouseUp:
			{
				UICamera.Mouse.Button.ButtonReleaseEventHandler buttonReleaseEventHandler = new UICamera.Mouse.Button.ButtonReleaseEventHandler(@event);
				try
				{
					UICamera.IssueEvent(@event, EventType.MouseUp);
				}
				finally
				{
					((IDisposable)(object)buttonReleaseEventHandler).Dispose();
				}
				return;
			}
			case EventType.MouseMove:
			{
				if (!UICamera.Mouse.Button.AllowMove)
				{
					return;
				}
				break;
			}
			case EventType.MouseDrag:
			{
				if (!UICamera.Mouse.Button.AllowDrag)
				{
					return;
				}
				break;
			}
			case EventType.KeyDown:
			case EventType.KeyUp:
			case EventType.ScrollWheel:
			case EventType.DragUpdated:
			case EventType.DragPerform:
			{
				break;
			}
			case EventType.Repaint:
			case EventType.Layout:
			case EventType.Ignore:
			case EventType.Used:
			{
				return;
			}
			default:
			{
				goto case EventType.DragPerform;
			}
		}
		UICamera.IssueEvent(@event, type);
		if (type == EventType.MouseMove && @event.type == EventType.Used)
		{
			UnityEngine.Debug.LogWarning("Mouse move was used.");
		}
	}

	private static void Highlight(GameObject go, bool highlighted)
	{
		if (go != null)
		{
			int count = UICamera.mHighlighted.Count;
			while (count > 0)
			{
				int num = count - 1;
				count = num;
				UICamera.Highlighted item = UICamera.mHighlighted[num];
				if (item == null || item.go == null)
				{
					UICamera.mHighlighted.RemoveAt(count);
				}
				else
				{
					if (item.go != go)
					{
						continue;
					}
					if (!highlighted)
					{
						UICamera.Highlighted highlighted1 = item;
						int num1 = highlighted1.counter - 1;
						int num2 = num1;
						highlighted1.counter = num1;
						if (num2 < 1)
						{
							UICamera.mHighlighted.Remove(item);
							go.Hover(false);
						}
					}
					else
					{
						UICamera.Highlighted highlighted2 = item;
						highlighted2.counter = highlighted2.counter + 1;
					}
					return;
				}
			}
			if (highlighted)
			{
				UICamera.Highlighted highlighted3 = new UICamera.Highlighted()
				{
					go = go,
					counter = 1
				};
				UICamera.mHighlighted.Add(highlighted3);
				go.Hover(true);
			}
		}
	}

	public static bool IsHighlighted(GameObject go)
	{
		int count = UICamera.mHighlighted.Count;
		while (count > 0)
		{
			int num = count - 1;
			count = num;
			if (UICamera.mHighlighted[num].go != go)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private static void IssueEvent(NGUIHack.Event @event, EventType type)
	{
		int num = @event.button;
		KeyCode keyCode = @event.keyCode;
		UICamera uICamera = null;
		EventType eventType = type;
		switch (eventType)
		{
			case EventType.MouseDown:
			{
				if (num != 0 && UICamera.mMouseCamera.TryGetValue(0, out uICamera) && uICamera)
				{
					uICamera.OnEvent(@event, type);
					if (@event.type != EventType.MouseDown)
					{
						UICamera.mMouseCamera[num] = uICamera;
						return;
					}
				}
				break;
			}
			case EventType.MouseUp:
			{
				if (UICamera.mMouseCamera.TryGetValue(num, out uICamera))
				{
					if (!uICamera)
					{
						@event.Use();
					}
					else
					{
						uICamera.OnEvent(@event, type);
						if (@event.type == EventType.MouseUp)
						{
							@event.Use();
						}
					}
					UICamera.mMouseCamera.Remove(num);
				}
				return;
			}
			case EventType.MouseMove:
			{
				break;
			}
			case EventType.MouseDrag:
			{
				if (UICamera.mMouseCamera.TryGetValue(0, out uICamera))
				{
					if (!uICamera)
					{
						@event.Use();
					}
					else
					{
						uICamera.OnEvent(@event, type);
					}
				}
				return;
			}
			case EventType.KeyDown:
			{
				break;
			}
			case EventType.KeyUp:
			{
				if (UICamera.mKeyCamera.TryGetValue(keyCode, out uICamera))
				{
					if (!uICamera)
					{
						@event.Use();
					}
					else
					{
						uICamera.OnEvent(@event, type);
						if (@event.type == EventType.KeyUp)
						{
							@event.Use();
						}
					}
					UICamera.mKeyCamera.Remove(keyCode);
				}
				return;
			}
			default:
			{
				goto case EventType.KeyDown;
			}
		}
		for (int i = 0; i < UICamera.mListCount; i++)
		{
			UICamera uICamera1 = UICamera.mList[UICamera.mListSort[i]];
			if (uICamera1 != uICamera)
			{
				if (uICamera1.usesAnyEvents)
				{
					uICamera1.OnEvent(@event, type);
					if (@event.type != type)
					{
						eventType = type;
						if (eventType == EventType.MouseDown)
						{
							UICamera.mMouseCamera[num] = uICamera1;
						}
						else if (eventType == EventType.KeyDown)
						{
							UICamera.mKeyCamera[keyCode] = uICamera1;
						}
						return;
					}
				}
			}
		}
	}

	private void OnApplicationQuit()
	{
		UICamera.mHighlighted.Clear();
	}

	private void OnCancelEvent(NGUIHack.Event @event, EventType type)
	{
		if (type == EventType.KeyDown)
		{
			UICamera.mSel.SendMessage("OnKey", KeyCode.Escape, SendMessageOptions.DontRequireReceiver);
			@event.Use();
		}
	}

	private void OnDestroy()
	{
		this.RemoveFromList();
	}

	private void OnDirectionEvent(NGUIHack.Event @event, int x, int y, EventType type)
	{
		bool flag = false;
		if (type == EventType.KeyDown)
		{
			if (x != 0)
			{
				UICamera.mSel.SendMessage("OnKey", (x >= 0 ? KeyCode.RightArrow : KeyCode.LeftArrow), SendMessageOptions.DontRequireReceiver);
				flag = true;
			}
			if (y != 0)
			{
				UICamera.mSel.SendMessage("OnKey", (y >= 0 ? KeyCode.UpArrow : KeyCode.DownArrow), SendMessageOptions.DontRequireReceiver);
				flag = true;
			}
		}
		if (flag)
		{
			@event.Use();
		}
	}

	private void OnEvent(NGUIHack.Event @event, EventType type)
	{
		Camera camera = UICamera.currentCamera;
		try
		{
			UICamera.currentCamera = this.cachedCamera;
			switch (type)
			{
				case EventType.MouseDown:
				case EventType.MouseUp:
				case EventType.MouseMove:
				case EventType.MouseDrag:
				{
					if ((this.mouseMode & UIInputMode.UseEvents) == UIInputMode.UseEvents)
					{
						this.OnMouseEvent(@event, type);
					}
					break;
				}
				case EventType.KeyDown:
				case EventType.KeyUp:
				{
					if ((this.keyboardMode & UIInputMode.UseEvents) == UIInputMode.UseEvents)
					{
						this.OnKeyboardEvent(@event, type);
					}
					break;
				}
				case EventType.ScrollWheel:
				{
					if ((this.scrollWheelMode & UIInputMode.UseEvents) == UIInputMode.UseEvents)
					{
						this.OnScrollWheelEvent(@event, type);
					}
					break;
				}
			}
		}
		finally
		{
			UICamera.currentCamera = camera;
		}
	}

	private bool OnEventShared(NGUIHack.Event @event, EventType type)
	{
		return false;
	}

	private void OnKeyboardEvent(NGUIHack.Event @event, EventType type)
	{
		if (this.OnEventShared(@event, type))
		{
			return;
		}
		char chr = @event.character;
		KeyCode keyCode = @event.keyCode;
		bool flag = UICamera.mSelInput;
		if (flag)
		{
			UICamera.mSelInput.OnEvent(this, @event, type);
		}
		if (UICamera.mSel != null)
		{
			KeyCode keyCode1 = keyCode;
			if (keyCode1 == KeyCode.Tab)
			{
				if (type == EventType.KeyDown)
				{
					UICamera.mSel.Key(KeyCode.Tab);
				}
			}
			else if (keyCode1 != KeyCode.Delete)
			{
				if (type == EventType.KeyDown && chr != 0)
				{
					UICamera.mSel.Input(chr.ToString());
				}
				if (keyCode == this.submitKey0 || keyCode == this.submitKey1)
				{
					if (!flag || @event.type == type)
					{
						this.OnSubmitEvent(@event, type);
					}
				}
				else if (keyCode == this.cancelKey0 || keyCode == this.cancelKey1)
				{
					if (!flag || @event.type == type)
					{
						this.OnCancelEvent(@event, type);
					}
				}
				else if (UICamera.inputHasFocus)
				{
					if (!flag || @event.type == type)
					{
						if (keyCode == KeyCode.UpArrow)
						{
							this.OnDirectionEvent(@event, 0, 1, type);
						}
						else if (keyCode == KeyCode.DownArrow)
						{
							this.OnDirectionEvent(@event, 0, -1, type);
						}
						else if (keyCode == KeyCode.LeftArrow)
						{
							this.OnDirectionEvent(@event, -1, 0, type);
						}
						else if (keyCode == KeyCode.RightArrow)
						{
							this.OnDirectionEvent(@event, 1, 0, type);
						}
					}
				}
				else if (!flag || @event.type == type)
				{
					if (keyCode == KeyCode.UpArrow || keyCode == KeyCode.W)
					{
						this.OnDirectionEvent(@event, 0, 1, type);
					}
					else if (keyCode == KeyCode.DownArrow || keyCode == KeyCode.S)
					{
						this.OnDirectionEvent(@event, 0, -1, type);
					}
					else if (keyCode == KeyCode.LeftArrow || keyCode == KeyCode.A)
					{
						this.OnDirectionEvent(@event, -1, 0, type);
					}
					else if (keyCode == KeyCode.RightArrow || keyCode == KeyCode.D)
					{
						this.OnDirectionEvent(@event, 1, 0, type);
					}
				}
			}
			else if (type == EventType.KeyDown)
			{
				UICamera.mSel.Input("\b");
			}
		}
	}

	private void OnMouseEvent(NGUIHack.Event @event, EventType type)
	{
		if (this.OnEventShared(@event, type))
		{
			return;
		}
		UICamera.Cursor.MouseEvent(@event, type);
	}

	private void OnScrollWheelEvent(NGUIHack.Event @event, EventType type)
	{
		if (UICamera.mHover != null)
		{
			Vector2 vector2 = @event.delta;
			bool swallowScroll = false;
			bool flag = false;
			if (vector2.y != 0f)
			{
				UICamera.SwallowScroll = false;
				UICamera.mHover.Scroll(vector2.y);
				flag = !UICamera.SwallowScroll;
			}
			if (vector2.x != 0f)
			{
				UICamera.SwallowScroll = false;
				UICamera.mHover.ScrollX(vector2.x);
				swallowScroll = !UICamera.SwallowScroll;
			}
			if (flag || swallowScroll)
			{
				UIPanel uIPanel = UIPanel.Find(UICamera.mHover.transform);
				if (uIPanel)
				{
					if (flag)
					{
						uIPanel.gameObject.NGUIMessage<float>("OnHoverScroll", vector2.y);
					}
					if (swallowScroll)
					{
						uIPanel.gameObject.NGUIMessage<float>("OnHoverScrollX", vector2.x);
					}
				}
			}
			@event.Use();
		}
	}

	private void OnSubmitEvent(NGUIHack.Event @event, EventType type)
	{
	}

	public static bool PopupPanel(UIPanel panel)
	{
		if (UICamera.popupPanel == panel)
		{
			return false;
		}
		if (UICamera.popupPanel)
		{
			UICamera.popupPanel.gameObject.NGUIMessage("PopupEnd");
			UICamera.popupPanel = null;
		}
		if (panel)
		{
			UICamera.popupPanel = panel;
			UICamera.popupPanel.gameObject.NGUIMessage("PopupStart");
		}
		return true;
	}

	private void ProcessOthers()
	{
		int direction = 0;
		int num = 0;
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				direction = direction + UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num = num + UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (direction != 0)
		{
			UICamera.mSel.SendMessage("OnKey", (direction <= 0 ? KeyCode.DownArrow : KeyCode.UpArrow), SendMessageOptions.DontRequireReceiver);
		}
		if (num != 0)
		{
			UICamera.mSel.SendMessage("OnKey", (num <= 0 ? KeyCode.LeftArrow : KeyCode.RightArrow), SendMessageOptions.DontRequireReceiver);
		}
	}

	private static bool Raycast(Vector3 inPos, ref UIHotSpot.Hit hit, out UICamera cam)
	{
		// 
		// Current member / type: System.Boolean UICamera::Raycast(UnityEngine.Vector3,UIHotSpot/Hit&,UICamera&)
		// File path: G:\Games\SteamLibrary\SteamApps\common\rust\legacy\rust_Data\Managed\Assembly-CSharp.dll
		// 
		// Product version: 2014.1.225.0
		// Exception in: System.Boolean Raycast(UnityEngine.Vector3,UIHotSpot/Hit&,UICamera&)
		// 
		// Not supported type UnityEngine.LayerMask.
		//    at ..(TypeDefinition ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 579
		//    at ..(TypeDefinition , TypeDefinition ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 519
		//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 408
		//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 220
		//    at ..set_Right( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 246
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 530
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 113
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 109
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 530
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 181
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 99
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 109
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 385
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 71
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 61
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 363
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 67
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 61
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 363
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 67
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 61
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 398
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 75
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 61
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 363
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 67
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 42
		//    at ..(MethodBody , ILanguage ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 83
		//    at ..( , ILanguage , MethodBody , & ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\Extensions.cs:line 99
		//    at ..(MethodBody , ILanguage , & ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\Extensions.cs:line 62
		//    at ..(ILanguage , MethodDefinition ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 116
		// 
		// mailto: JustDecompilePublicFeedback@telerik.com

	}

	private static bool Raycast(UICamera cam, Vector3 inPos, ref RaycastHit hit)
	{
		// 
		// Current member / type: System.Boolean UICamera::Raycast(UICamera,UnityEngine.Vector3,UnityEngine.RaycastHit&)
		// File path: G:\Games\SteamLibrary\SteamApps\common\rust\legacy\rust_Data\Managed\Assembly-CSharp.dll
		// 
		// Product version: 2014.1.225.0
		// Exception in: System.Boolean Raycast(UICamera,UnityEngine.Vector3,UnityEngine.RaycastHit&)
		// 
		// Not supported type UnityEngine.LayerMask.
		//    at ..(TypeDefinition ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 579
		//    at ..(TypeDefinition , TypeDefinition ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 519
		//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 408
		//    at ..() in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 220
		//    at ..set_Right( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\Expressions\BinaryExpression.cs:line 246
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 530
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 113
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 109
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 530
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 181
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 99
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 109
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 385
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 71
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 278
		//    at ..Visit[,]( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 288
		//    at ..Visit( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 319
		//    at ..( ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 339
		//    at ..( ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Steps\CombinedTransformerStep.cs:line 42
		//    at ..(MethodBody , ILanguage ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 83
		//    at ..( , ILanguage , MethodBody , & ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\Extensions.cs:line 99
		//    at ..(MethodBody , ILanguage , & ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\Extensions.cs:line 62
		//    at ..(ILanguage , MethodDefinition ,  ) in c:\Builds\245\Behemoth\ReleaseBranch Production Build\Sources\Decompiler\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 116
		// 
		// mailto: JustDecompilePublicFeedback@telerik.com

	}

	public UITextPosition RaycastText(Vector3 inPos, UILabel label)
	{
		float single;
		if (!base.enabled || !base.camera.enabled || !base.camera.pixelRect.Contains(inPos) || !label)
		{
			UnityEngine.Debug.Log("No Sir");
			return new UITextPosition();
		}
		Ray ray = base.camera.ScreenPointToRay(inPos);
		Vector3 vector3 = label.transform.forward;
		if (Vector3.Dot(ray.direction, vector3) <= 0f)
		{
			UnityEngine.Debug.Log("Bad Dir");
			return new UITextPosition();
		}
		Plane plane = new Plane(vector3, label.transform.position);
		if (!plane.Raycast(ray, out single))
		{
			UnityEngine.Debug.Log("Paralell");
			return new UITextPosition();
		}
		Vector3 point = ray.GetPoint(single);
		Vector3[] vector3Array = new Vector3[] { label.transform.InverseTransformPoint(point) };
		UITextPosition[] uITextPositionArray = new UITextPosition[1];
		UITextPosition uITextPosition = new UITextPosition();
		uITextPositionArray[0] = uITextPosition;
		UITextPosition[] uITextPositionArray1 = uITextPositionArray;
		if (label.CalculateTextPosition(Space.Self, vector3Array, uITextPositionArray1) == 0)
		{
			UnityEngine.Debug.Log("Zero");
		}
		return uITextPositionArray1[0];
	}

	private void RemoveFromList()
	{
		if (this.lastBoundLayerIndex != -1)
		{
			UICamera.mList[this.lastBoundLayerIndex] = null;
			int num = 0;
			for (int i = 0; i < UICamera.mListCount; i++)
			{
				if (UICamera.mListSort[i] != this.lastBoundLayerIndex)
				{
					int num1 = num;
					num = num1 + 1;
					UICamera.mListSort[num1] = UICamera.mListSort[i];
				}
			}
			UICamera.mListCount = num;
			this.lastBoundLayerIndex = -1;
		}
	}

	public static void Render()
	{
		for (int i = 0; i < UICamera.mListCount; i++)
		{
			if (UICamera.mList[i] && UICamera.mList[i].enabled && UICamera.mList[i].camera && !UICamera.mList[i].camera.enabled)
			{
				UICamera.mList[i].camera.Render();
			}
		}
	}

	internal bool SetKeyboardFocus(UIInput input)
	{
		if (UICamera.mSelInput == input)
		{
			return true;
		}
		if (UICamera.mSelInput)
		{
			return false;
		}
		if (!input)
		{
			return false;
		}
		return UICamera.SetSelectedObject(input.gameObject);
	}

	public static bool SetSelectedObject(GameObject value)
	{
		UIInput component;
		if (UICamera.mSel != value)
		{
			if (UICamera.inSelectionCallback)
			{
				return false;
			}
			if (!value)
			{
				component = null;
			}
			else
			{
				component = value.GetComponent<UIInput>();
			}
			UIInput uIInput = component;
			if (UICamera.mSelInput != uIInput)
			{
				if (UICamera.mSelInput)
				{
					UICamera.mSelInput.LoseFocus();
				}
				UICamera.mSelInput = uIInput;
				if (uIInput && UICamera.mPressInput != uIInput)
				{
					uIInput.GainFocus();
				}
			}
			if (UICamera.mSel != null)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(UICamera.mSel.layer);
				if (uICamera != null)
				{
					Camera camera = UICamera.currentCamera;
					try
					{
						UICamera.currentCamera = uICamera.mCam;
						UICamera.inSelectionCallback = true;
						UICamera.mSel.Select(false);
						if (uICamera.useController || uICamera.useKeyboard)
						{
							UICamera.Highlight(UICamera.mSel, false);
						}
					}
					finally
					{
						UICamera.currentCamera = camera;
						UICamera.inSelectionCallback = false;
					}
				}
			}
			UICamera.mSel = value;
			if (UICamera.mSel != null)
			{
				UICamera uICamera1 = UICamera.FindCameraForLayer(UICamera.mSel.layer);
				if (uICamera1 != null)
				{
					UICamera.currentCamera = uICamera1.mCam;
					if (uICamera1.useController || uICamera1.useKeyboard)
					{
						UICamera.Highlight(UICamera.mSel, true);
					}
					UICamera.mSel.Select(true);
				}
			}
		}
		return true;
	}

	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		if (this.mTooltip != null)
		{
			this.mTooltip.Tooltip(val);
		}
		if (!val)
		{
			this.mTooltip = null;
		}
	}

	public static bool UnPopupPanel(UIPanel panel)
	{
		if (!(UICamera.popupPanel == panel) || !panel)
		{
			return false;
		}
		UICamera.popupPanel.gameObject.NGUIMessage("PopupEnd");
		UICamera.popupPanel = null;
		return true;
	}

	private void Update()
	{
		if (!Application.isPlaying || !this.handlesEvents)
		{
			return;
		}
		if (UICamera.mSel == null)
		{
			UICamera.inputHasFocus = false;
		}
		else
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			if ((this.mouseMode & UIInputMode.UseInput) == UIInputMode.UseInput)
			{
				float axis = Input.GetAxis(this.scrollAxisName);
				if (axis != 0f)
				{
					UICamera.mHover.Scroll(axis);
				}
			}
			if (this.mTooltipTime != 0f && this.mTooltipTime < Time.realtimeSinceStartup)
			{
				this.mTooltip = UICamera.mHover;
				this.ShowTooltip(true);
			}
		}
	}

	public struct BackwardsCompatabilitySupport
	{
		public UICamera.ClickNotification clickNotification
		{
			get
			{
				return UICamera.Cursor.Buttons.LeftValue.ClickNotification;
			}
			set
			{
				UICamera.Cursor.Buttons.LeftValue.ClickNotification = value;
			}
		}

		public Vector2 delta
		{
			get
			{
				return UICamera.Cursor.FrameDelta;
			}
		}

		public Vector2 pos
		{
			get
			{
				return (UICamera.Cursor.CurrentButton != null ? UICamera.Cursor.CurrentButton.Point + UICamera.Cursor.CurrentButton.TotalDelta : UICamera.Cursor.Current.Mouse.Point);
			}
		}

		public Vector2 totalDelta
		{
			get
			{
				return UICamera.Cursor.Buttons.LeftValue.TotalDelta;
			}
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return -1;
		}

		public static bool operator ==(UICamera.BackwardsCompatabilitySupport b, bool? s)
		{
			return UICamera.Cursor.Current.Valid == s.HasValue;
		}

		public static bool operator !=(UICamera.BackwardsCompatabilitySupport b, bool? s)
		{
			return UICamera.Cursor.Current.Valid != s.HasValue;
		}

		public override string ToString()
		{
			return string.Format("[BackwardsCompatabilitySupport: clickNotification={0}, pos={1}, delta={2}, totalDelta={3}]", new object[] { this.clickNotification, this.pos, this.delta, this.totalDelta });
		}
	}

	private class CamSorter : Comparer<byte>
	{
		public CamSorter()
		{
		}

		public override int Compare(byte a, byte b)
		{
			float single = UICamera.mList[b].cachedCamera.depth;
			return single.CompareTo(UICamera.mList[a].cachedCamera.depth);
		}
	}

	public enum ClickNotification
	{
		None,
		Always,
		BasedOnDelta
	}

	public sealed class CursorSampler
	{
		private const float kDoubleClickLimit = 0.25f;

		public UICamera.Mouse.Button.ValCollection<UICamera.Mouse.Button.Sampler> Buttons;

		public DropNotificationFlags DropNotification;

		public bool Dragging;

		public UICamera.CursorSampler.Sample Current;

		public UICamera.CursorSampler.Sample Last;

		public float LastClickTime;

		public bool IsFirst;

		public bool IsLast;

		public bool IsCurrent;

		public UICamera.Mouse.Button.Sampler CurrentButton;

		private DropNotificationFlags LastHoverDropNotification;

		private DropNotificationFlags PressDropNotification;

		private GameObject DragHover;

		private UIPanel Panel;

		public Vector2 FrameDelta
		{
			get
			{
				return this.Current.Mouse.Delta;
			}
		}

		public Vector2 Point
		{
			get
			{
				return this.Current.Mouse.Point;
			}
		}

		public CursorSampler()
		{
			this.Buttons.LeftValue = new UICamera.Mouse.Button.Sampler(UICamera.Mouse.Button.Flags.Left, this);
			this.Buttons.RightValue = new UICamera.Mouse.Button.Sampler(UICamera.Mouse.Button.Flags.Right, this);
			this.Buttons.MiddleValue = new UICamera.Mouse.Button.Sampler(UICamera.Mouse.Button.Flags.Middle, this);
		}

		private void CheckDragHover(bool HasCurrent, GameObject Current, GameObject Pressed)
		{
			if (!HasCurrent)
			{
				this.ClearDragHover(Pressed);
			}
			else
			{
				if (this.DragHover == Current)
				{
					return;
				}
				if (this.DragHover && this.DragHover != Pressed)
				{
					UICamera.CursorSampler.ExitDragHover(Pressed, this.DragHover, this.LastHoverDropNotification);
				}
				this.DragHover = Current;
				if (Current != Pressed)
				{
					this.LastHoverDropNotification = this.DropNotification;
					UICamera.CursorSampler.EnterDragHover(Pressed, this.DragHover, this.LastHoverDropNotification);
				}
			}
		}

		private void ClearDragHover(GameObject Pressed)
		{
			if (this.DragHover)
			{
				if (this.DragHover != Pressed)
				{
					UICamera.CursorSampler.ExitDragHover(Pressed, this.DragHover, this.LastHoverDropNotification);
				}
				this.DragHover = null;
			}
		}

		private static void EnterDragHover(GameObject lander, GameObject drop, DropNotificationFlags flags)
		{
			if ((flags & DropNotificationFlags.ReverseHover) != DropNotificationFlags.ReverseHover)
			{
				if ((flags & DropNotificationFlags.DragHover) == DropNotificationFlags.DragHover)
				{
					drop.NGUIMessage("OnDragHoverEnter", lander);
				}
				if ((flags & DropNotificationFlags.LandHover) == DropNotificationFlags.LandHover)
				{
					lander.NGUIMessage("OnLandHoverEnter", drop);
				}
			}
			else
			{
				if ((flags & DropNotificationFlags.LandHover) == DropNotificationFlags.LandHover)
				{
					lander.NGUIMessage("OnLandHoverEnter", drop);
				}
				if ((flags & DropNotificationFlags.DragHover) == DropNotificationFlags.DragHover)
				{
					drop.NGUIMessage("OnDragHoverEnter", lander);
				}
			}
		}

		private static void ExitDragHover(GameObject lander, GameObject drop, DropNotificationFlags flags)
		{
			if ((flags & DropNotificationFlags.ReverseHover) != DropNotificationFlags.ReverseHover)
			{
				if ((flags & DropNotificationFlags.LandHover) == DropNotificationFlags.LandHover)
				{
					lander.NGUIMessage("OnLandHoverExit", drop);
				}
				if ((flags & DropNotificationFlags.DragHover) == DropNotificationFlags.DragHover)
				{
					drop.NGUIMessage("OnDragHoverExit", lander);
				}
			}
			else
			{
				if ((flags & DropNotificationFlags.DragHover) == DropNotificationFlags.DragHover)
				{
					drop.NGUIMessage("OnDragHoverExit", lander);
				}
				if ((flags & DropNotificationFlags.LandHover) == DropNotificationFlags.LandHover)
				{
					lander.NGUIMessage("OnLandHoverExit", drop);
				}
			}
		}

		internal void MouseEvent(NGUIHack.Event @event, EventType type)
		{
			UICamera.CursorSampler.Sample held = new UICamera.CursorSampler.Sample();
			float releaseTime;
			float uICamera;
			bool flag;
			float single;
			bool hasUnder;
			Vector2 vector2 = new Vector2();
			held.Mouse.Scroll = vector2;
			held.Mouse.Buttons.Pressed = UICamera.Mouse.Button.Held | UICamera.Mouse.Button.NewlyPressed;
			held.Mouse.Point = @event.mousePosition;
			if (!this.Current.Valid)
			{
				held.IsFirst = true;
				held.DidMove = false;
				float single1 = 0f;
				single = single1;
				held.Mouse.Delta.y = single1;
				held.Mouse.Delta.x = single;
			}
			else
			{
				held.IsFirst = false;
				if (this.Current.Mouse.Point.x != held.Mouse.Point.x)
				{
					held.Mouse.Delta.x = held.Mouse.Point.x - this.Current.Mouse.Point.x;
					if (this.Current.Mouse.Point.y == held.Mouse.Point.y)
					{
						held.Mouse.Delta.y = 0f;
					}
					else
					{
						held.Mouse.Delta.y = held.Mouse.Point.y - this.Current.Mouse.Point.y;
					}
					held.DidMove = true;
				}
				else if (this.Current.Mouse.Point.y == held.Mouse.Point.y)
				{
					held.DidMove = false;
					float single2 = 0f;
					single = single2;
					held.Mouse.Delta.y = single2;
					held.Mouse.Delta.x = single;
				}
				else
				{
					held.Mouse.Delta.x = 0f;
					held.Mouse.Delta.y = held.Mouse.Point.y - this.Current.Mouse.Point.y;
					held.DidMove = true;
				}
			}
			held.Hit = UIHotSpot.Hit.invalid;
			bool flag1 = UICamera.Raycast(held.Mouse.Point, ref held.Hit, out held.UICamera);
			bool flag2 = flag1;
			held.DidHit = flag1;
			if (flag2)
			{
				UICamera.lastHit = held.Hit;
				held.Under = held.Hit.gameObject;
				held.HasUnder = true;
			}
			else if (!UICamera.fallThrough)
			{
				held.Under = null;
				held.HasUnder = false;
				held.UICamera = (held.IsFirst || !this.Current.UICamera ? UICamera.mList[UICamera.mListSort[0]] : this.Current.UICamera);
			}
			else
			{
				held.Under = UICamera.fallThrough;
				held.HasUnder = true;
				held.UICamera = UICamera.FindCameraForLayer(UICamera.fallThrough.layer);
				if (!held.UICamera)
				{
					held.UICamera = (held.IsFirst || !this.Current.UICamera ? UICamera.mList[UICamera.mListSort[0]] : this.Current.UICamera);
				}
			}
			if (held.IsFirst)
			{
				hasUnder = true;
			}
			else if (!held.HasUnder)
			{
				hasUnder = this.Current.HasUnder;
			}
			else
			{
				hasUnder = (!this.Current.HasUnder ? true : this.Current.Under != held.Under);
			}
			held.UnderChange = hasUnder;
			held.HoverChange = (!held.UnderChange ? false : held.Under != UICamera.mHover);
			held.ButtonChange = UICamera.Mouse.Button.AnyNewlyPressedOrReleased;
			bool flag3 = false;
			if (!held.ButtonChange || !UICamera.Mouse.Button.AnyNewlyPressedThatCancelTooltips)
			{
				if (held.DidMove && (held.HoverChange || !held.UICamera.stickyTooltip))
				{
					if (held.UICamera.mTooltipTime != 0f)
					{
						held.UICamera.mTooltipTime = Time.realtimeSinceStartup + held.UICamera.tooltipDelay;
					}
					else if (held.UICamera.mTooltip != null)
					{
						flag3 = true;
						held.UICamera.ShowTooltip(false);
					}
				}
				if (held.HoverChange && UICamera.mHover)
				{
					if (held.UICamera.mTooltip != null)
					{
						held.UICamera.ShowTooltip(false);
					}
					UICamera.Highlight(UICamera.mHover, false);
					UICamera.mHover = null;
				}
			}
			else
			{
				held.UICamera.mTooltipTime = 0f;
			}
			held.Time = Time.realtimeSinceStartup;
			held.ButtonsPressed = UICamera.Mouse.Button.NewlyPressed;
			held.ButtonsReleased = UICamera.Mouse.Button.NewlyReleased;
			if (!flag3 && (int)held.ButtonsPressed != 0 && held.UICamera.mTooltip)
			{
				held.UICamera.ShowTooltip(false);
				flag3 = true;
			}
			for (UICamera.Mouse.Button.Flags i = UICamera.Mouse.Button.Flags.Left; i < (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle); i = (UICamera.Mouse.Button.Flags)((int)i << (int)UICamera.Mouse.Button.Flags.Left))
			{
				UICamera.Mouse.Button.Sampler item = this.Buttons[i];
				try
				{
					this.CurrentButton = item;
					int num = 0;
					flag2 = (bool)num;
					item.ReleasedNow = (bool)num;
					item.PressedNow = flag2;
					if ((held.ButtonsPressed & i) == i)
					{
						if (!item.Once)
						{
							float time = held.Time - 120f;
							single = time;
							item.ReleaseTime = time;
							releaseTime = single;
							item.Once = true;
						}
						else
						{
							releaseTime = item.ReleaseTime;
						}
						item.PressTime = held.Time;
						item.Pressed = held.Under;
						item.DidHit = held.DidHit;
						item.PressedNow = true;
						item.Hit = held.Hit;
						item.ReleasedNow = false;
						item.Held = true;
						item.Point = held.Mouse.Point;
						float single3 = 0f;
						single = single3;
						item.TotalDelta.y = single3;
						item.TotalDelta.x = single;
						item.ClickNotification = UICamera.ClickNotification.Always;
						if (i == UICamera.Mouse.Button.Flags.Left)
						{
							this.Dragging = false;
							this.DropNotification = DropNotificationFlags.DragDrop;
							item.DragClick = false;
							item.DragClickNumber = (ulong)0;
						}
						else if (!this.Dragging)
						{
							item.DragClick = false;
							item.DragClickNumber = (ulong)0;
						}
						else
						{
							item.DragClick = true;
							item.DragClickNumber = this.Buttons.LeftValue.ClickCount;
						}
						if (held.DidHit)
						{
							if (i != UICamera.Mouse.Button.Flags.Left)
							{
								if (UICamera.mSelInput)
								{
									UICamera.mSelInput.OnEvent(held.UICamera, @event, type);
								}
								if (!item.DragClick)
								{
									if (i == UICamera.Mouse.Button.Flags.Right)
									{
										UIPanel uIPanel = UIPanel.FindRoot(held.Under.transform);
										if (UICamera.popupPanel && UICamera.popupPanel != uIPanel)
										{
											UICamera.PopupPanel(null);
										}
										held.Under.AltPress(true);
									}
									else if (i == UICamera.Mouse.Button.Flags.Middle)
									{
										held.Under.MidPress(true);
									}
								}
							}
							else
							{
								UICamera.mPressInput = held.Under.GetComponent<UIInput>();
								if (!UICamera.mSelInput)
								{
									if (UICamera.mPressInput)
									{
										UICamera.mPressInput.GainFocus();
										UICamera.mPressInput.OnEvent(held.UICamera, @event, type);
									}
								}
								else if (!UICamera.mPressInput)
								{
									UICamera.mSelInput.LoseFocus();
									UICamera.mSelInput = null;
								}
								else if (UICamera.mSelInput != UICamera.mPressInput)
								{
									UICamera.mSelInput.LoseFocus();
									UICamera.mSelInput = null;
									UICamera.mPressInput.GainFocus();
									UICamera.mPressInput.OnEvent(held.UICamera, @event, type);
								}
								else
								{
									UICamera.mSelInput.OnEvent(held.UICamera, @event, type);
								}
								if (UICamera.mSel && UICamera.mSel != held.Under)
								{
									if (!flag3 && held.UICamera.mTooltip)
									{
										held.UICamera.ShowTooltip(false);
									}
									UICamera.SetSelectedObject(null);
								}
								this.Panel = UIPanel.FindRoot(held.Under.transform);
								if (!this.Panel)
								{
									if (UICamera.popupPanel)
									{
										UICamera.PopupPanel(null);
									}
									held.Under.Press(true);
								}
								else
								{
									if (this.Panel != UICamera.popupPanel && UICamera.popupPanel)
									{
										UICamera.PopupPanel(null);
									}
									held.Under.Press(true);
									this.Panel.gameObject.NGUIMessage("OnChildPress", true);
								}
								this.PressDropNotification = this.DropNotification;
							}
							@event.Use();
						}
						else if (i == UICamera.Mouse.Button.Flags.Left)
						{
							if (UICamera.popupPanel)
							{
								UICamera.PopupPanel(null);
							}
							UICamera.mPressInput = null;
							if (UICamera.mSelInput)
							{
								UICamera.mSelInput.LoseFocus();
								UICamera.mSelInput = null;
							}
							if (UICamera.mSel)
							{
								if (!flag3 && held.UICamera.mTooltip)
								{
									held.UICamera.ShowTooltip(false);
								}
								UICamera.SetSelectedObject(null);
							}
						}
					}
					else if (item.Held && item.DidHit)
					{
						if (type == EventType.MouseDrag && i == UICamera.Mouse.Button.Flags.Left)
						{
							if (UICamera.mPressInput)
							{
								UICamera.mPressInput.OnEvent(held.UICamera, @event, type);
							}
							@event.Use();
						}
						if (held.DidMove)
						{
							if (!flag3 && held.UICamera.mTooltip)
							{
								held.UICamera.ShowTooltip(false);
							}
							item.TotalDelta.x = item.TotalDelta.x + held.Mouse.Delta.x;
							item.TotalDelta.y = item.TotalDelta.y + held.Mouse.Delta.y;
							bool clickNotification = item.ClickNotification == UICamera.ClickNotification.None;
							if (i == UICamera.Mouse.Button.Flags.Left && !item.DragClick && (int)(this.PressDropNotification & (DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand)) != 0)
							{
								if (!this.Dragging)
								{
									item.Pressed.DragState(true);
									this.Dragging = true;
								}
								item.Pressed.Drag(held.Mouse.Delta);
								this.CheckDragHover(held.DidHit, held.Under, item.Pressed);
							}
							if (clickNotification)
							{
								item.ClickNotification = UICamera.ClickNotification.None;
							}
							else if (item.ClickNotification == UICamera.ClickNotification.BasedOnDelta)
							{
								if (i != UICamera.Mouse.Button.Flags.Left)
								{
									uICamera = (float)Screen.height * 0.1f;
									if (uICamera < held.UICamera.touchClickThreshold)
									{
										uICamera = held.UICamera.touchClickThreshold;
									}
								}
								else
								{
									uICamera = held.UICamera.mouseClickThreshold;
								}
								if (item.TotalDelta.x * item.TotalDelta.x + item.TotalDelta.y * item.TotalDelta.y > uICamera * uICamera)
								{
									item.ClickNotification = UICamera.ClickNotification.None;
								}
							}
						}
					}
				}
				finally
				{
					this.CurrentButton = null;
				}
			}
			for (UICamera.Mouse.Button.Flags j = UICamera.Mouse.Button.Flags.Middle; (int)j != 0; j = (UICamera.Mouse.Button.Flags)((int)j >> (int)UICamera.Mouse.Button.Flags.Left))
			{
				UICamera.Mouse.Button.Sampler sampler = this.Buttons[j];
				try
				{
					this.CurrentButton = sampler;
					if ((held.ButtonsReleased & j) == j)
					{
						sampler.ReleasedNow = true;
						if (sampler.DidHit)
						{
							if (j == UICamera.Mouse.Button.Flags.Left)
							{
								if ((type == EventType.MouseUp || type == EventType.KeyUp) && UICamera.mPressInput && sampler.Pressed == UICamera.mPressInput.gameObject)
								{
									UICamera.mPressInput.OnEvent(held.UICamera, @event, type);
									UICamera.mSelInput = UICamera.mPressInput;
								}
								UICamera.mPressInput = null;
								if (held.HasUnder)
								{
									if (sampler.Pressed != held.Under)
									{
										if (this.Dragging && !sampler.DragClick && (int)(this.PressDropNotification & (DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand)) != 0)
										{
											DropNotification.DropMessage(ref this.DropNotification, DragEventKind.Drag, sampler.Pressed, held.Under);
											this.ClearDragHover(sampler.Pressed);
											sampler.Pressed.DragState(false);
										}
										if (this.Panel)
										{
											this.Panel.gameObject.NGUIMessage("OnChildPress", false);
										}
										sampler.Pressed.Press(false);
										if (sampler.Pressed == UICamera.mHover)
										{
											sampler.Pressed.Hover(true);
										}
									}
									else
									{
										if (this.Dragging && (int)(this.PressDropNotification & (DropNotificationFlags.DragDrop | DropNotificationFlags.DragLand | DropNotificationFlags.AltDrop | DropNotificationFlags.AltLand | DropNotificationFlags.MidDrop | DropNotificationFlags.MidLand)) != 0)
										{
											this.ClearDragHover(sampler.Pressed);
											if (!sampler.DragClick)
											{
												sampler.Pressed.DragState(false);
											}
										}
										if (this.Panel)
										{
											this.Panel.gameObject.NGUIMessage("OnChildPress", false);
										}
										sampler.Pressed.Press(false);
										if (sampler.Pressed == UICamera.mHover)
										{
											sampler.Pressed.Hover(true);
										}
										if (sampler.Pressed == UICamera.mSel)
										{
											UICamera.mSel = sampler.Pressed;
										}
										else
										{
											UICamera.mSel = sampler.Pressed;
											sampler.Pressed.Select(true);
										}
										if (!sampler.DragClick && sampler.ClickNotification != UICamera.ClickNotification.None)
										{
											if (this.Panel)
											{
												this.Panel.gameObject.NGUIMessage("OnChildClick", sampler.Pressed);
											}
											if (sampler.ClickNotification != UICamera.ClickNotification.None)
											{
												sampler.Pressed.Click();
												if (sampler.ClickNotification != UICamera.ClickNotification.None && sampler.ReleaseTime + 0.25f > held.Time)
												{
													sampler.Pressed.DoubleClick();
												}
											}
										}
										else if (this.Panel)
										{
											this.Panel.gameObject.NGUIMessage("OnChildClickCanceled", sampler.Pressed);
										}
									}
								}
								else if (this.Dragging)
								{
									this.ClearDragHover(sampler.Pressed);
									if (!sampler.DragClick)
									{
										DropNotification.DropMessage(ref this.DropNotification, DragEventKind.Drag, sampler.Pressed, held.Under);
										sampler.Pressed.DragState(false);
									}
									if (this.Panel)
									{
										this.Panel.gameObject.NGUIMessage("OnChildPress", false);
									}
									sampler.Pressed.Press(false);
									if (sampler.Pressed == UICamera.mHover)
									{
										sampler.Pressed.Hover(true);
									}
									this.Dragging = false;
								}
							}
							else if (sampler.DragClick)
							{
								if (!this.Buttons.LeftValue.DragClick && this.Buttons.LeftValue.ClickCount == sampler.DragClickNumber)
								{
									if (j != UICamera.Mouse.Button.Flags.Right)
									{
										flag = (j != UICamera.Mouse.Button.Flags.Middle ? false : DropNotification.DropMessage(ref this.DropNotification, DragEventKind.Mid, this.Buttons.LeftValue.Pressed, sampler.Pressed));
									}
									else
									{
										flag = DropNotification.DropMessage(ref this.DropNotification, DragEventKind.Alt, this.Buttons.LeftValue.Pressed, sampler.Pressed);
									}
									if (flag)
									{
										this.Buttons.LeftValue.DragClick = true;
										this.ClearDragHover(this.Buttons.LeftValue.Pressed);
										sampler.Pressed.DragState(false);
									}
								}
							}
							else if (j == UICamera.Mouse.Button.Flags.Right)
							{
								sampler.Pressed.AltPress(false);
								if (held.HasUnder && sampler.Pressed == held.Under && sampler.ClickNotification != UICamera.ClickNotification.None)
								{
									sampler.Pressed.AltClick();
									if (sampler.ClickNotification != UICamera.ClickNotification.None && sampler.ReleaseTime + 0.25f > held.Time)
									{
										sampler.Pressed.AltDoubleClick();
									}
								}
							}
							else if (j == UICamera.Mouse.Button.Flags.Middle)
							{
								sampler.Pressed.MidPress(false);
								if (held.HasUnder && sampler.Pressed == held.Under && sampler.ClickNotification != UICamera.ClickNotification.None)
								{
									sampler.Pressed.MidClick();
									if (sampler.ClickNotification != UICamera.ClickNotification.None && sampler.ReleaseTime + 0.25f > held.Time)
									{
										sampler.Pressed.MidDoubleClick();
									}
								}
							}
						}
						sampler.ReleasedNow = true;
						sampler.ClickNotification = UICamera.ClickNotification.None;
						sampler.ReleaseTime = held.Time;
						sampler.Held = false;
						UICamera.Mouse.Button.Sampler clickCount = sampler;
						clickCount.ClickCount = clickCount.ClickCount + (long)1;
						sampler.DragClick = false;
						sampler.DragClickNumber = (ulong)0;
						if (j == UICamera.Mouse.Button.Flags.Left)
						{
							this.Dragging = false;
							this.Panel = null;
						}
						if (@event.type == EventType.MouseUp || @event.type == EventType.KeyUp)
						{
							@event.Use();
						}
					}
				}
				finally
				{
					this.CurrentButton = null;
				}
			}
			UICamera.lastMousePosition = (!held.IsFirst ? this.Current.Mouse.Point : held.Mouse.Point);
			if (held.HasUnder && (held.Mouse.Buttons.NonePressed || this.Dragging && (this.DropNotification & DropNotificationFlags.RegularHover) == DropNotificationFlags.RegularHover) && UICamera.mHover != held.Under)
			{
				held.UICamera.mTooltipTime = held.Time + held.UICamera.tooltipDelay;
				UICamera.mHover = held.Under;
				UICamera.Highlight(UICamera.mHover, true);
			}
			held.Valid = true;
			this.Last = this.Current;
			this.Current = held;
		}

		public struct Sample
		{
			public GameObject Under;

			public UICamera UICamera;

			public UICamera.Mouse.State Mouse;

			public UIHotSpot.Hit Hit;

			public float Time;

			public bool DidHit;

			public bool HasUnder;

			public bool Valid;

			public bool DidMove;

			public bool IsFirst;

			public bool ButtonChange;

			public bool UnderChange;

			public bool HoverChange;

			public UICamera.Mouse.Button.Flags ButtonsPressed;

			public UICamera.Mouse.Button.Flags ButtonsReleased;

			public Camera Camera
			{
				get
				{
					Camera uICamera;
					if (!this.UICamera)
					{
						uICamera = null;
					}
					else
					{
						uICamera = this.UICamera.cachedCamera;
					}
					return uICamera;
				}
			}

			public static bool operator @false(UICamera.CursorSampler.Sample sample)
			{
				return !sample.Valid;
			}

			public static bool operator @true(UICamera.CursorSampler.Sample sample)
			{
				return sample.Valid;
			}
		}
	}

	private class Highlighted
	{
		public GameObject go;

		public int counter;

		public Highlighted()
		{
		}
	}

	private static class LateLoadCursor
	{
		public readonly static UICamera.CursorSampler Sampler;

		static LateLoadCursor()
		{
			UICamera.LateLoadCursor.Sampler = new UICamera.CursorSampler();
		}
	}

	public static class Mouse
	{
		public static class Button
		{
			private const UICamera.Mouse.Button.Flags kCancelsTooltips = UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle;

			public const UICamera.Mouse.Button.Flags Left = UICamera.Mouse.Button.Flags.Left;

			public const UICamera.Mouse.Button.Flags Right = UICamera.Mouse.Button.Flags.Right;

			public const UICamera.Mouse.Button.Flags Middle = UICamera.Mouse.Button.Flags.Middle;

			public const UICamera.Mouse.Button.Flags Mouse0 = UICamera.Mouse.Button.Flags.Left;

			public const UICamera.Mouse.Button.Flags Mouse1 = UICamera.Mouse.Button.Flags.Right;

			public const UICamera.Mouse.Button.Flags Mouse2 = UICamera.Mouse.Button.Flags.Middle;

			public const UICamera.Mouse.Button.Flags None = 0;

			public const UICamera.Mouse.Button.Flags All = UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle;

			public const int Count = 3;

			private static UICamera.Mouse.Button.Flags pressed;

			private static UICamera.Mouse.Button.Flags released;

			private static UICamera.Mouse.Button.Flags held;

			private static int indexPressed;

			private static int indexReleased;

			public static bool AllowDrag
			{
				get
				{
					return (int)UICamera.Mouse.Button.held != 0;
				}
			}

			public static bool AllowMove
			{
				get
				{
					return (int)(UICamera.Mouse.Button.held | UICamera.Mouse.Button.released | UICamera.Mouse.Button.pressed) == 0;
				}
			}

			internal static bool AnyNewlyPressed
			{
				get
				{
					return (int)UICamera.Mouse.Button.pressed != 0;
				}
			}

			internal static bool AnyNewlyPressedOrReleased
			{
				get
				{
					return (int)(UICamera.Mouse.Button.pressed | UICamera.Mouse.Button.released) != 0;
				}
			}

			internal static bool AnyNewlyPressedThatCancelTooltips
			{
				get
				{
					return (int)(UICamera.Mouse.Button.pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle)) != 0;
				}
			}

			internal static bool AnyNewlyReleased
			{
				get
				{
					return (int)UICamera.Mouse.Button.released != 0;
				}
			}

			internal static UICamera.Mouse.Button.Flags Held
			{
				get
				{
					return UICamera.Mouse.Button.held;
				}
			}

			internal static UICamera.Mouse.Button.Flags NewlyPressed
			{
				get
				{
					return UICamera.Mouse.Button.pressed;
				}
			}

			internal static UICamera.Mouse.Button.Flags NewlyReleased
			{
				get
				{
					return UICamera.Mouse.Button.released;
				}
			}

			static Button()
			{
				UICamera.Mouse.Button.indexPressed = -1;
				UICamera.Mouse.Button.indexReleased = -1;
			}

			public static UICamera.Mouse.Button.Flags Index(int index)
			{
				if (index < 0 || index >= 3)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return (UICamera.Mouse.Button.Flags)(1 << (index & 31));
			}

			internal static bool IsNewlyPressed(UICamera.Mouse.Button.Flags flag)
			{
				return (UICamera.Mouse.Button.pressed & flag) == flag;
			}

			internal static bool IsNewlyReleased(UICamera.Mouse.Button.Flags flag)
			{
				return (UICamera.Mouse.Button.released & flag) == flag;
			}

			public struct ButtonPressEventHandler : IDisposable
			{
				private NGUIHack.Event @event;

				public ButtonPressEventHandler(NGUIHack.Event @event)
				{
					this.@event = @event;
					UICamera.Mouse.Button.pressed = UICamera.Mouse.Button.Index(@event.button);
					UICamera.Mouse.Button.indexPressed = @event.button;
				}

				public void Dispose()
				{
					if (UICamera.Mouse.Button.indexPressed != -1)
					{
						if (this.@event.type == EventType.Used)
						{
							UICamera.Mouse.Button.held = UICamera.Mouse.Button.held | UICamera.Mouse.Button.pressed;
						}
						UICamera.Mouse.Button.indexPressed = -1;
						UICamera.Mouse.Button.pressed = (UICamera.Mouse.Button.Flags)0;
					}
				}
			}

			public struct ButtonReleaseEventHandler : IDisposable
			{
				private NGUIHack.Event @event;

				public ButtonReleaseEventHandler(NGUIHack.Event @event)
				{
					this.@event = @event;
					UICamera.Mouse.Button.released = UICamera.Mouse.Button.Index(@event.button);
					UICamera.Mouse.Button.indexReleased = @event.button;
				}

				public void Dispose()
				{
					if (UICamera.Mouse.Button.indexReleased != -1)
					{
						if (this.@event.type == EventType.Used)
						{
							UICamera.Mouse.Button.held = UICamera.Mouse.Button.held & ~UICamera.Mouse.Button.released;
						}
						UICamera.Mouse.Button.indexReleased = -1;
						UICamera.Mouse.Button.released = (UICamera.Mouse.Button.Flags)0;
					}
				}
			}

			[Flags]
			public enum Flags
			{
				Left = 1,
				Right = 2,
				Middle = 4
			}

			public struct Pair<T>
			{
				public readonly UICamera.Mouse.Button.Flags Button;

				public readonly T Value;

				public Pair(UICamera.Mouse.Button.Flags Button, T Value)
				{
					this.Button = Button;
					this.Value = Value;
				}

				public Pair(UICamera.Mouse.Button.Flags Button, ref T Value)
				{
					this.Button = Button;
					this.Value = Value;
				}

				public Pair(UICamera.Mouse.Button.Flags Button) : this(Button, default(T))
				{
				}
			}

			public struct PressState : IEnumerable, IEnumerable<UICamera.Mouse.Button.Flags>
			{
				public UICamera.Mouse.Button.Flags Pressed;

				public bool AllPressed
				{
					get
					{
						return (this.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle)) == (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
					}
				}

				public bool AllReleased
				{
					get
					{
						return !this.AnyPressed;
					}
				}

				public bool AnyPressed
				{
					get
					{
						return (int)(this.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle)) != 0;
					}
				}

				public bool AnyReleased
				{
					get
					{
						return !this.AllPressed;
					}
				}

				public bool this[int index]
				{
					get
					{
						UICamera.Mouse.Button.Flags flag = UICamera.Mouse.Button.Index(index);
						return (this.Pressed & flag) == flag;
					}
					set
					{
						UICamera.Mouse.Button.Flags flag = UICamera.Mouse.Button.Index(index);
						if (!value)
						{
							UICamera.Mouse.Button.PressState pressed = this;
							pressed.Pressed = pressed.Pressed & ~flag;
						}
						else
						{
							UICamera.Mouse.Button.PressState pressStates = this;
							pressStates.Pressed = pressStates.Pressed | flag;
						}
					}
				}

				public bool LeftPressed
				{
					get
					{
						return (this.Pressed & UICamera.Mouse.Button.Flags.Left) == UICamera.Mouse.Button.Flags.Left;
					}
					set
					{
						if (!value)
						{
							UICamera.Mouse.Button.PressState pressed = this;
							pressed.Pressed = pressed.Pressed & (UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
						}
						else
						{
							UICamera.Mouse.Button.PressState pressStates = this;
							pressStates.Pressed = pressStates.Pressed | UICamera.Mouse.Button.Flags.Left;
						}
					}
				}

				public bool LeftReleased
				{
					get
					{
						return !this.LeftPressed;
					}
					set
					{
						this.LeftPressed = !value;
					}
				}

				public bool MiddlePressed
				{
					get
					{
						return (this.Pressed & UICamera.Mouse.Button.Flags.Middle) == UICamera.Mouse.Button.Flags.Middle;
					}
					set
					{
						if (!value)
						{
							UICamera.Mouse.Button.PressState pressed = this;
							pressed.Pressed = pressed.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right);
						}
						else
						{
							UICamera.Mouse.Button.PressState pressStates = this;
							pressStates.Pressed = pressStates.Pressed | UICamera.Mouse.Button.Flags.Middle;
						}
					}
				}

				public bool MiddleReleased
				{
					get
					{
						return !this.MiddlePressed;
					}
					set
					{
						this.MiddlePressed = !value;
					}
				}

				public bool NonePressed
				{
					get
					{
						return (int)(this.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle)) == 0;
					}
				}

				public bool NoneReleased
				{
					get
					{
						return !this.AllPressed;
					}
				}

				public int PressedCount
				{
					get
					{
						int num = 0;
						uint pressed = (uint)(this.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle));
						while (pressed != 0)
						{
							pressed = pressed & pressed - 1;
							num++;
						}
						return num;
					}
				}

				public UICamera.Mouse.Button.Flags Released
				{
					get
					{
						return ~this.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
					}
					set
					{
						this.Pressed = ~value & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
					}
				}

				public bool RightPressed
				{
					get
					{
						return (this.Pressed & UICamera.Mouse.Button.Flags.Right) == UICamera.Mouse.Button.Flags.Right;
					}
					set
					{
						if (!value)
						{
							UICamera.Mouse.Button.PressState pressed = this;
							pressed.Pressed = pressed.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Middle);
						}
						else
						{
							UICamera.Mouse.Button.PressState pressStates = this;
							pressStates.Pressed = pressStates.Pressed | UICamera.Mouse.Button.Flags.Right;
						}
					}
				}

				public bool RightReleased
				{
					get
					{
						return !this.RightPressed;
					}
					set
					{
						this.RightPressed = !value;
					}
				}

				public void Clear()
				{
					UICamera.Mouse.Button.PressState pressed = this;
					pressed.Pressed = (UICamera.Mouse.Button.Flags)((int)pressed.Pressed & -8);
				}

				public UICamera.Mouse.Button.PressState.Enumerator GetEnumerator()
				{
					return UICamera.Mouse.Button.PressState.Enumerator.Enumerate(this.Pressed);
				}

				public static UICamera.Mouse.Button.PressState operator +(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed | r.Pressed;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator +(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.Flags r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed | r;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator +(UICamera.Mouse.Button.Flags l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressStates = new UICamera.Mouse.Button.PressState();
					pressStates.Pressed = l | r.Pressed;
					return pressStates;
				}

				public static UICamera.Mouse.Button.PressState operator /(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed ^ r.Pressed;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator /(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.Flags r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed ^ r;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator /(UICamera.Mouse.Button.Flags l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressStates = new UICamera.Mouse.Button.PressState();
					pressStates.Pressed = l ^ r.Pressed;
					return pressStates;
				}

				public static bool operator @false(UICamera.Mouse.Button.PressState state)
				{
					return state.NonePressed;
				}

				public static implicit operator Flags(UICamera.Mouse.Button.PressState state)
				{
					return state.Pressed & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
				}

				public static implicit operator PressState(UICamera.Mouse.Button.Flags buttons)
				{
					UICamera.Mouse.Button.PressState pressStates = new UICamera.Mouse.Button.PressState();
					pressStates.Pressed = buttons & (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle);
					return pressStates;
				}

				public static UICamera.Mouse.Button.PressState operator *(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed & r.Pressed;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator *(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.Flags r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed & r;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator *(UICamera.Mouse.Button.Flags l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressStates = new UICamera.Mouse.Button.PressState();
					pressStates.Pressed = l & r.Pressed;
					return pressStates;
				}

				public static UICamera.Mouse.Button.PressState operator -(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed & ~r.Pressed;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator -(UICamera.Mouse.Button.PressState l, UICamera.Mouse.Button.Flags r)
				{
					UICamera.Mouse.Button.PressState pressed = new UICamera.Mouse.Button.PressState();
					pressed.Pressed = l.Pressed & ~r;
					return pressed;
				}

				public static UICamera.Mouse.Button.PressState operator -(UICamera.Mouse.Button.Flags l, UICamera.Mouse.Button.PressState r)
				{
					UICamera.Mouse.Button.PressState pressStates = new UICamera.Mouse.Button.PressState();
					pressStates.Pressed = l & ~r.Pressed;
					return pressStates;
				}

				public static bool operator @true(UICamera.Mouse.Button.PressState state)
				{
					return state.AnyPressed;
				}

				public static UICamera.Mouse.Button.PressState operator -(UICamera.Mouse.Button.PressState s)
				{
					UICamera.Mouse.Button.PressState released = new UICamera.Mouse.Button.PressState();
					released.Pressed = s.Released;
					return released;
				}

				IEnumerator<UICamera.Mouse.Button.Flags> System.Collections.Generic.IEnumerable<UICamera.Mouse.Button.Flags>.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				public class Enumerator : IDisposable, IEnumerator, IEnumerator<UICamera.Mouse.Button.Flags>
				{
					private readonly static UICamera.Mouse.Button.Flags[][] combos;

					private UICamera.Mouse.Button.Flags[] flags;

					private UICamera.Mouse.Button.Flags @value;

					private int pos;

					private UICamera.Mouse.Button.PressState.Enumerator nextDump;

					private bool inDump;

					private static UICamera.Mouse.Button.PressState.Enumerator dump;

					private static uint dumpCount;

					public UICamera.Mouse.Button.Flags Current
					{
						get
						{
							return this.flags[this.pos];
						}
					}

					object System.Collections.IEnumerator.Current
					{
						get
						{
							return this.flags[this.pos];
						}
					}

					static Enumerator()
					{
						UICamera.Mouse.Button.PressState.Enumerator.combos = new UICamera.Mouse.Button.Flags[8][];
						for (UICamera.Mouse.Button.Flags i = (UICamera.Mouse.Button.Flags)0; i <= (UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right | UICamera.Mouse.Button.Flags.Middle); i = (UICamera.Mouse.Button.Flags)((int)i + (int)UICamera.Mouse.Button.Flags.Left))
						{
							int num = 0;
							uint num1 = (uint)i;
							while (num1 != 0)
							{
								num1 = num1 & num1 - 1;
								num++;
							}
							UICamera.Mouse.Button.Flags[] flagsArray = new UICamera.Mouse.Button.Flags[num];
							int num2 = 0;
							for (int j = 0; j < 3 && num2 < num; j++)
							{
								if (((int)i & 1 << (j & 31)) == 1 << (j & 31))
								{
									int num3 = num2;
									num2 = num3 + 1;
									flagsArray[num3] = (UICamera.Mouse.Button.Flags)(1 << (j & 31));
								}
							}
							UICamera.Mouse.Button.PressState.Enumerator.combos[(int)i] = flagsArray;
						}
					}

					private Enumerator()
					{
					}

					public void Dispose()
					{
						if (!this.inDump)
						{
							this.nextDump = UICamera.Mouse.Button.PressState.Enumerator.dump;
							this.inDump = true;
							UICamera.Mouse.Button.PressState.Enumerator.dump = this;
							UICamera.Mouse.Button.PressState.Enumerator.dumpCount = UICamera.Mouse.Button.PressState.Enumerator.dumpCount + 1;
						}
					}

					public static UICamera.Mouse.Button.PressState.Enumerator Enumerate(UICamera.Mouse.Button.Flags flags)
					{
						UICamera.Mouse.Button.PressState.Enumerator enumerator;
						if (UICamera.Mouse.Button.PressState.Enumerator.dumpCount != 0)
						{
							enumerator = UICamera.Mouse.Button.PressState.Enumerator.dump;
							UICamera.Mouse.Button.PressState.Enumerator.dump = enumerator.nextDump;
							UICamera.Mouse.Button.PressState.Enumerator.dumpCount = UICamera.Mouse.Button.PressState.Enumerator.dumpCount - 1;
							enumerator.nextDump = null;
						}
						else
						{
							enumerator = new UICamera.Mouse.Button.PressState.Enumerator();
						}
						enumerator.pos = -1;
						enumerator.@value = flags;
						enumerator.inDump = false;
						enumerator.flags = UICamera.Mouse.Button.PressState.Enumerator.combos[(int)flags];
						return enumerator;
					}

					public bool MoveNext()
					{
						UICamera.Mouse.Button.PressState.Enumerator enumerator = this;
						int num = enumerator.pos + 1;
						int num1 = num;
						enumerator.pos = num;
						return num1 < (int)this.flags.Length;
					}

					public void Reset()
					{
						this.pos = -1;
					}
				}
			}

			public struct RefCollection<T> : IEnumerable, IEnumerable<UICamera.Mouse.Button.Pair<T>>
			{
				public T LeftValue;

				public T RightValue;

				public T MiddleValue;

				public T this[UICamera.Mouse.Button.Flags button]
				{
					get
					{
						switch (button)
						{
							case UICamera.Mouse.Button.Flags.Left:
							{
								return this.LeftValue;
							}
							case UICamera.Mouse.Button.Flags.Right:
							{
								return this.RightValue;
							}
							case UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
							case UICamera.Mouse.Button.Flags.Middle:
							{
								return this.MiddleValue;
							}
							default:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
						}
					}
					set
					{
						switch (button)
						{
							case UICamera.Mouse.Button.Flags.Left:
							{
								this.LeftValue = value;
								break;
							}
							case UICamera.Mouse.Button.Flags.Right:
							{
								this.RightValue = value;
								break;
							}
							case UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
							case UICamera.Mouse.Button.Flags.Middle:
							{
								this.MiddleValue = value;
								break;
							}
							default:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
						}
					}
				}

				public T this[int i]
				{
					get
					{
						return this[UICamera.Mouse.Button.Flags.Left];
					}
					set
					{
						this[UICamera.Mouse.Button.Flags.Left] = value;
					}
				}

				public IEnumerable<UICamera.Mouse.Button.Pair<T>> this[UICamera.Mouse.Button.PressState state]
				{
					get
					{
						UICamera.Mouse.Button.RefCollection<T>.<>c__Iterator4B variable = null;
						return variable;
					}
				}

				[DebuggerHidden]
				public IEnumerator<UICamera.Mouse.Button.Pair<T>> GetEnumerator()
				{
					UICamera.Mouse.Button.RefCollection<T>.<GetEnumerator>c__Iterator4C variable = null;
					return variable;
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
			}

			public sealed class Sampler
			{
				public readonly UICamera.Mouse.Button.Flags Button;

				public readonly UICamera.CursorSampler Cursor;

				public GameObject Pressed;

				public UIHotSpot.Hit Hit;

				public Vector2 Point;

				public Vector2 TotalDelta;

				public ulong ClickCount;

				public ulong DragClickNumber;

				public float PressTime;

				public float ReleaseTime;

				public UICamera.ClickNotification ClickNotification;

				public bool PressedNow;

				public bool Held;

				public bool ReleasedNow;

				public bool DidHit;

				public bool Once;

				public bool DragClick;

				public Sampler(UICamera.Mouse.Button.Flags Button, UICamera.CursorSampler Cursor)
				{
					this.Button = Button;
					this.Cursor = Cursor;
				}
			}

			public struct ValCollection<T> : IEnumerable, IEnumerable<UICamera.Mouse.Button.Pair<T>>
			{
				public T LeftValue;

				public T RightValue;

				public T MiddleValue;

				public T this[UICamera.Mouse.Button.Flags button]
				{
					get
					{
						switch (button)
						{
							case UICamera.Mouse.Button.Flags.Left:
							{
								return this.LeftValue;
							}
							case UICamera.Mouse.Button.Flags.Right:
							{
								return this.RightValue;
							}
							case UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
							case UICamera.Mouse.Button.Flags.Middle:
							{
								return this.MiddleValue;
							}
							default:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
						}
					}
					set
					{
						switch (button)
						{
							case UICamera.Mouse.Button.Flags.Left:
							{
								this.LeftValue = value;
								break;
							}
							case UICamera.Mouse.Button.Flags.Right:
							{
								this.RightValue = value;
								break;
							}
							case UICamera.Mouse.Button.Flags.Left | UICamera.Mouse.Button.Flags.Right:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
							case UICamera.Mouse.Button.Flags.Middle:
							{
								this.MiddleValue = value;
								break;
							}
							default:
							{
								throw new ArgumentOutOfRangeException("button", "button should not be None or a Combination of multiple buttons");
							}
						}
					}
				}

				public T this[int i]
				{
					get
					{
						return this[UICamera.Mouse.Button.Flags.Left];
					}
					set
					{
						this[UICamera.Mouse.Button.Flags.Left] = value;
					}
				}

				public IEnumerable<UICamera.Mouse.Button.Pair<T>> this[UICamera.Mouse.Button.PressState state]
				{
					get
					{
						UICamera.Mouse.Button.ValCollection<T>.<>c__Iterator49 variable = null;
						return variable;
					}
				}

				[DebuggerHidden]
				public IEnumerator<UICamera.Mouse.Button.Pair<T>> GetEnumerator()
				{
					UICamera.Mouse.Button.ValCollection<T>.<GetEnumerator>c__Iterator4A variable = null;
					return variable;
				}

				IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
			}
		}

		public struct State
		{
			public Vector2 Point;

			public Vector2 Delta;

			public Vector2 Scroll;

			public UICamera.Mouse.Button.PressState Buttons;
		}
	}

	private struct RaycastCheckWork
	{
		public Ray ray;

		public RaycastHit hit;

		public float dist;

		public int mask;

		public bool Check()
		{
			bool flag;
			UIPanel uIPanel = UIPanel.Find(this.hit.collider.transform, false);
			if (!uIPanel)
			{
				return true;
			}
			if (uIPanel.enabled && (uIPanel.clipping == UIDrawCall.Clipping.None || UICamera.CheckRayEnterClippingRect(this.ray, uIPanel.transform, uIPanel.clipRange)))
			{
				return true;
			}
			Collider collider = this.hit.collider;
			try
			{
				collider.enabled = false;
				flag = (!Physics.Raycast(this.ray, out this.hit, this.dist, this.mask) ? false : this.Check());
			}
			finally
			{
				collider.enabled = true;
			}
			return flag;
		}
	}
}