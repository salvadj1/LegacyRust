using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Input Manager")]
[Serializable]
public class dfInputManager : MonoBehaviour
{
	private static KeyCode[] wasd;

	private static dfControl controlUnderMouse;

	[SerializeField]
	protected Camera renderCamera;

	[SerializeField]
	protected bool useTouch = true;

	[SerializeField]
	protected bool useJoystick;

	[SerializeField]
	protected KeyCode joystickClickButton = KeyCode.Joystick1Button1;

	[SerializeField]
	protected string horizontalAxis = "Horizontal";

	[SerializeField]
	protected string verticalAxis = "Vertical";

	[SerializeField]
	protected float axisPollingInterval = 0.15f;

	[SerializeField]
	protected bool retainFocus;

	[SerializeField]
	protected int touchClickRadius = 20;

	private dfControl buttonDownTarget;

	private dfInputManager.MouseInputManager mouseHandler;

	private IInputAdapter adapter;

	private float lastAxisCheck;

	public IInputAdapter Adapter
	{
		get
		{
			return this.adapter;
		}
		set
		{
			object defaultInput = value;
			if (defaultInput == null)
			{
				defaultInput = new dfInputManager.DefaultInput();
			}
			this.adapter = (IInputAdapter)defaultInput;
		}
	}

	public static dfControl ControlUnderMouse
	{
		get
		{
			return dfInputManager.controlUnderMouse;
		}
	}

	public string HorizontalAxis
	{
		get
		{
			return this.horizontalAxis;
		}
		set
		{
			this.horizontalAxis = value;
		}
	}

	public KeyCode JoystickClickButton
	{
		get
		{
			return this.joystickClickButton;
		}
		set
		{
			this.joystickClickButton = value;
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
			this.renderCamera = value;
		}
	}

	public bool RetainFocus
	{
		get
		{
			return this.retainFocus;
		}
		set
		{
			this.retainFocus = value;
		}
	}

	public int TouchClickRadius
	{
		get
		{
			return this.touchClickRadius;
		}
		set
		{
			this.touchClickRadius = Mathf.Max(0, value);
		}
	}

	public bool UseJoystick
	{
		get
		{
			return this.useJoystick;
		}
		set
		{
			this.useJoystick = value;
		}
	}

	public bool UseTouch
	{
		get
		{
			return this.useTouch;
		}
		set
		{
			this.useTouch = value;
		}
	}

	public string VerticalAxis
	{
		get
		{
			return this.verticalAxis;
		}
		set
		{
			this.verticalAxis = value;
		}
	}

	static dfInputManager()
	{
		dfInputManager.wasd = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow };
		dfInputManager.controlUnderMouse = null;
	}

	public dfInputManager()
	{
	}

	public void Awake()
	{
	}

	internal dfControl clipCast(RaycastHit[] hits)
	{
		if (hits == null || (int)hits.Length == 0)
		{
			return null;
		}
		dfControl _dfControl = null;
		dfControl modalControl = dfGUIManager.GetModalControl();
		for (int i = (int)hits.Length - 1; i >= 0; i--)
		{
			RaycastHit raycastHit = hits[i];
			dfControl component = raycastHit.transform.GetComponent<dfControl>();
			if ((component == null || modalControl != null && !component.transform.IsChildOf(modalControl.transform) || !component.enabled || dfInputManager.combinedOpacity(component) <= 0.01f || !component.IsEnabled || !component.IsVisible ? false : component.transform.IsChildOf(base.transform)))
			{
				if (dfInputManager.isInsideClippingRegion(raycastHit, component) && (_dfControl == null || component.RenderOrder > _dfControl.RenderOrder))
				{
					_dfControl = component;
				}
			}
		}
		return _dfControl;
	}

	private static float combinedOpacity(dfControl control)
	{
		float opacity = 1f;
		while (control != null)
		{
			opacity = opacity * control.Opacity;
			control = control.Parent;
		}
		return opacity;
	}

	internal static bool isInsideClippingRegion(RaycastHit hit, dfControl control)
	{
		Plane[] clippingPlanes;
		Vector3 vector3 = hit.point;
		while (control != null)
		{
			if (!control.ClipChildren)
			{
				clippingPlanes = null;
			}
			else
			{
				clippingPlanes = control.GetClippingPlanes();
			}
			Plane[] planeArray = clippingPlanes;
			if (planeArray != null && (int)planeArray.Length > 0)
			{
				for (int i = 0; i < (int)planeArray.Length; i++)
				{
					if (!planeArray[i].GetSide(vector3))
					{
						return false;
					}
				}
			}
			control = control.Parent;
		}
		return true;
	}

	public void OnEnable()
	{
		this.mouseHandler = new dfInputManager.MouseInputManager();
		if (this.adapter == null)
		{
			Component component = (
				from c in (IEnumerable<Component>)base.GetComponents(typeof(MonoBehaviour))
				where typeof(IInputAdapter).IsAssignableFrom(c.GetType())
				select c).FirstOrDefault<Component>();
			object defaultInput = (IInputAdapter)component;
			if (defaultInput == null)
			{
				defaultInput = new dfInputManager.DefaultInput();
			}
			this.adapter = (IInputAdapter)defaultInput;
		}
	}

	public void OnGUI()
	{
		Event @event = Event.current;
		if (@event == null)
		{
			return;
		}
		if (!@event.isKey || @event.keyCode == KeyCode.None)
		{
			return;
		}
		this.processKeyEvent(@event.type, @event.keyCode, @event.modifiers);
	}

	private void processJoystick()
	{
		try
		{
			dfControl activeControl = dfGUIManager.ActiveControl;
			if (!(activeControl == null) && activeControl.transform.IsChildOf(base.transform))
			{
				float axis = this.adapter.GetAxis(this.horizontalAxis);
				float single = this.adapter.GetAxis(this.verticalAxis);
				if (Mathf.Abs(axis) < 0.5f && Mathf.Abs(single) <= 0.5f)
				{
					this.lastAxisCheck = Time.deltaTime - this.axisPollingInterval;
				}
				if (Time.realtimeSinceStartup - this.lastAxisCheck > this.axisPollingInterval)
				{
					if (Mathf.Abs(axis) >= 0.5f)
					{
						this.lastAxisCheck = Time.realtimeSinceStartup;
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, (axis <= 0f ? KeyCode.LeftArrow : KeyCode.RightArrow), false, false, false));
					}
					if (Mathf.Abs(single) >= 0.5f)
					{
						this.lastAxisCheck = Time.realtimeSinceStartup;
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, (single <= 0f ? KeyCode.DownArrow : KeyCode.UpArrow), false, false, false));
					}
				}
				if (this.joystickClickButton != KeyCode.None)
				{
					if (this.adapter.GetKeyDown(this.joystickClickButton))
					{
						Vector3 center = activeControl.GetCenter();
						Camera camera = activeControl.GetCamera();
						Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(center));
						dfMouseEventArgs dfMouseEventArg = new dfMouseEventArgs(activeControl, dfMouseButtons.Left, 0, ray, center, 0f);
						activeControl.OnMouseDown(dfMouseEventArg);
						this.buttonDownTarget = activeControl;
					}
					if (this.adapter.GetKeyUp(this.joystickClickButton))
					{
						if (this.buttonDownTarget == activeControl)
						{
							activeControl.DoClick();
						}
						Vector3 vector3 = activeControl.GetCenter();
						Camera camera1 = activeControl.GetCamera();
						Ray ray1 = camera1.ScreenPointToRay(camera1.WorldToScreenPoint(vector3));
						dfMouseEventArgs dfMouseEventArg1 = new dfMouseEventArgs(activeControl, dfMouseButtons.Left, 0, ray1, vector3, 0f);
						activeControl.OnMouseUp(dfMouseEventArg1);
						this.buttonDownTarget = null;
					}
				}
				for (KeyCode i = KeyCode.Joystick1Button0; i <= KeyCode.Joystick1Button19; i++)
				{
					if (this.adapter.GetKeyDown(i))
					{
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, i, false, false, false));
					}
				}
			}
		}
		catch (UnityException unityException)
		{
			Debug.LogError(unityException.ToString(), this);
			this.useJoystick = false;
		}
	}

	private bool processKeyboard()
	{
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || string.IsNullOrEmpty(Input.inputString) || !activeControl.transform.IsChildOf(base.transform))
		{
			return false;
		}
		string str = Input.inputString;
		for (int i = 0; i < str.Length; i++)
		{
			char chr = str[i];
			if (chr != '\b' && chr != '\n')
			{
				dfKeyEventArgs dfKeyEventArg = new dfKeyEventArgs(activeControl, (KeyCode)chr, false, false, false)
				{
					Character = chr
				};
				activeControl.OnKeyPress(dfKeyEventArg);
			}
		}
		return true;
	}

	private void processKeyEvent(EventType eventType, KeyCode keyCode, EventModifiers modifiers)
	{
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || !activeControl.IsEnabled || !activeControl.transform.IsChildOf(base.transform))
		{
			return;
		}
		bool flag = (modifiers & EventModifiers.Control) == EventModifiers.Control;
		bool flag1 = (modifiers & EventModifiers.Shift) == EventModifiers.Shift;
		bool flag2 = (modifiers & EventModifiers.Alt) == EventModifiers.Alt;
		dfKeyEventArgs dfKeyEventArg = new dfKeyEventArgs(activeControl, keyCode, flag, flag1, flag2);
		if (keyCode >= KeyCode.Space && keyCode <= KeyCode.Z)
		{
			char chr = (char)keyCode;
			dfKeyEventArg.Character = (!flag1 ? char.ToLower(chr) : char.ToUpper(chr));
		}
		if (eventType == EventType.KeyDown)
		{
			activeControl.OnKeyDown(dfKeyEventArg);
		}
		else if (eventType == EventType.KeyUp)
		{
			activeControl.OnKeyUp(dfKeyEventArg);
		}
		if (!dfKeyEventArg.Used && eventType != EventType.KeyUp)
		{
			return;
		}
	}

	private void processMouseInput()
	{
		Vector2 mousePosition = this.adapter.GetMousePosition();
		Ray ray = this.renderCamera.ScreenPointToRay(mousePosition);
		float single = this.renderCamera.farClipPlane - this.renderCamera.nearClipPlane;
		RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, single, this.renderCamera.cullingMask);
		Array.Sort<RaycastHit>(raycastHitArray, new Comparison<RaycastHit>(dfInputManager.raycastHitSorter));
		dfInputManager.controlUnderMouse = this.clipCast(raycastHitArray);
		this.mouseHandler.ProcessInput(this.adapter, ray, dfInputManager.controlUnderMouse, this.retainFocus);
	}

	internal static int raycastHitSorter(RaycastHit lhs, RaycastHit rhs)
	{
		return lhs.distance.CompareTo(rhs.distance);
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		dfControl activeControl = dfGUIManager.ActiveControl;
		this.processMouseInput();
		if (activeControl == null)
		{
			return;
		}
		if (this.processKeyboard())
		{
			return;
		}
		if (this.useJoystick)
		{
			for (int i = 0; i < (int)dfInputManager.wasd.Length; i++)
			{
				if (Input.GetKey(dfInputManager.wasd[i]) || Input.GetKeyDown(dfInputManager.wasd[i]) || Input.GetKeyUp(dfInputManager.wasd[i]))
				{
					return;
				}
			}
			this.processJoystick();
		}
	}

	private class DefaultInput : IInputAdapter
	{
		public DefaultInput()
		{
		}

		public float GetAxis(string axisName)
		{
			return Input.GetAxis(axisName);
		}

		public bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		public bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUp(key);
		}

		public bool GetMouseButton(int button)
		{
			return Input.GetMouseButton(button);
		}

		public bool GetMouseButtonDown(int button)
		{
			return Input.GetMouseButtonDown(button);
		}

		public bool GetMouseButtonUp(int button)
		{
			return Input.GetMouseButtonUp(button);
		}

		public Vector2 GetMousePosition()
		{
			return Input.mousePosition;
		}
	}

	private class MouseInputManager
	{
		private const string scrollAxisName = "Mouse ScrollWheel";

		private const float DOUBLECLICK_TIME = 0.25f;

		private const int DRAG_START_DELTA = 2;

		private const float HOVER_NOTIFICATION_FREQUENCY = 0.1f;

		private const float HOVER_NOTIFICATION_BEGIN = 0.25f;

		private dfControl activeControl;

		private Vector2 lastPosition;

		private Vector2 mouseMoveDelta;

		private float lastClickTime;

		private float lastHoverTime;

		private dfDragDropState dragState;

		private object dragData;

		private dfControl lastDragControl;

		private dfMouseButtons buttonsDown;

		private dfMouseButtons buttonsReleased;

		private dfMouseButtons buttonsPressed;

		public MouseInputManager()
		{
		}

		private static void getMouseButtonInfo(IInputAdapter adapter, ref dfMouseButtons buttonsDown, ref dfMouseButtons buttonsReleased, ref dfMouseButtons buttonsPressed)
		{
			for (int i = 0; i < 3; i++)
			{
				if (adapter.GetMouseButton(i))
				{
					buttonsDown = (dfMouseButtons)((int)buttonsDown | 1 << (i & 31));
				}
				if (adapter.GetMouseButtonUp(i))
				{
					buttonsReleased = (dfMouseButtons)((int)buttonsReleased | 1 << (i & 31));
				}
				if (adapter.GetMouseButtonDown(i))
				{
					buttonsPressed = (dfMouseButtons)((int)buttonsPressed | 1 << (i & 31));
				}
			}
		}

		public void ProcessInput(IInputAdapter adapter, Ray ray, dfControl control, bool retainFocusSetting)
		{
			Vector2 mousePosition = adapter.GetMousePosition();
			this.buttonsDown = dfMouseButtons.None;
			this.buttonsReleased = dfMouseButtons.None;
			this.buttonsPressed = dfMouseButtons.None;
			dfInputManager.MouseInputManager.getMouseButtonInfo(adapter, ref this.buttonsDown, ref this.buttonsReleased, ref this.buttonsPressed);
			float axis = adapter.GetAxis("Mouse ScrollWheel");
			if (!Mathf.Approximately(axis, 0f))
			{
				axis = Mathf.Sign(axis) * Mathf.Max(1f, Mathf.Abs(axis));
			}
			this.mouseMoveDelta = mousePosition - this.lastPosition;
			this.lastPosition = mousePosition;
			if (this.dragState == dfDragDropState.Dragging)
			{
				if (this.buttonsReleased == dfMouseButtons.None)
				{
					if (control == this.activeControl)
					{
						return;
					}
					if (control == this.lastDragControl)
					{
						if (control != null && Vector2.Distance(mousePosition, this.lastPosition) > 1f)
						{
							dfDragEventArgs dfDragEventArg = new dfDragEventArgs(control, this.dragState, this.dragData, ray, mousePosition);
							control.OnDragOver(dfDragEventArg);
						}
						return;
					}
					if (this.lastDragControl != null)
					{
						dfDragEventArgs dfDragEventArg1 = new dfDragEventArgs(this.lastDragControl, this.dragState, this.dragData, ray, mousePosition);
						this.lastDragControl.OnDragLeave(dfDragEventArg1);
					}
					if (control != null)
					{
						dfDragEventArgs dfDragEventArg2 = new dfDragEventArgs(control, this.dragState, this.dragData, ray, mousePosition);
						control.OnDragEnter(dfDragEventArg2);
					}
					this.lastDragControl = control;
					return;
				}
				if (!(control != null) || !(control != this.activeControl))
				{
					dfDragDropState _dfDragDropState = (control != null ? dfDragDropState.Cancelled : dfDragDropState.CancelledNoTarget);
					dfDragEventArgs dfDragEventArg3 = new dfDragEventArgs(this.activeControl, _dfDragDropState, this.dragData, ray, mousePosition);
					this.activeControl.OnDragEnd(dfDragEventArg3);
				}
				else
				{
					dfDragEventArgs dfDragEventArg4 = new dfDragEventArgs(control, dfDragDropState.Dragging, this.dragData, ray, mousePosition);
					control.OnDragDrop(dfDragEventArg4);
					if (!dfDragEventArg4.Used || dfDragEventArg4.State == dfDragDropState.Dragging)
					{
						dfDragEventArg4.State = dfDragDropState.Cancelled;
					}
					dfDragEventArg4 = new dfDragEventArgs(this.activeControl, dfDragEventArg4.State, dfDragEventArg4.Data, ray, mousePosition)
					{
						Target = control
					};
					this.activeControl.OnDragEnd(dfDragEventArg4);
				}
				this.dragState = dfDragDropState.None;
				this.lastDragControl = null;
				this.activeControl = null;
				this.lastClickTime = 0f;
				this.lastHoverTime = 0f;
				this.lastPosition = mousePosition;
				return;
			}
			if (this.buttonsReleased != dfMouseButtons.None)
			{
				this.lastHoverTime = Time.realtimeSinceStartup + 0.25f;
				if (this.activeControl == null)
				{
					this.setActive(control, mousePosition, ray);
					return;
				}
				if (this.activeControl == control && this.buttonsDown == dfMouseButtons.None)
				{
					if (Time.realtimeSinceStartup - this.lastClickTime >= 0.25f)
					{
						this.lastClickTime = Time.realtimeSinceStartup;
						this.activeControl.OnClick(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 1, ray, mousePosition, axis));
					}
					else
					{
						this.lastClickTime = 0f;
						this.activeControl.OnDoubleClick(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 1, ray, mousePosition, axis));
					}
				}
				this.activeControl.OnMouseUp(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 0, ray, mousePosition, axis));
				if (this.buttonsDown == dfMouseButtons.None && this.activeControl != control)
				{
					this.setActive(null, mousePosition, ray);
				}
				return;
			}
			if (this.buttonsPressed != dfMouseButtons.None)
			{
				this.lastHoverTime = Time.realtimeSinceStartup + 0.25f;
				if (this.activeControl == null)
				{
					this.setActive(control, mousePosition, ray);
					if (control != null)
					{
						control.OnMouseDown(new dfMouseEventArgs(control, this.buttonsPressed, 0, ray, mousePosition, axis));
					}
					else if (!retainFocusSetting)
					{
						dfControl activeControl = dfGUIManager.ActiveControl;
						if (activeControl != null)
						{
							activeControl.Unfocus();
						}
					}
				}
				else
				{
					this.activeControl.OnMouseDown(new dfMouseEventArgs(this.activeControl, this.buttonsPressed, 0, ray, mousePosition, axis));
				}
				return;
			}
			if (this.activeControl != null && this.activeControl == control && this.mouseMoveDelta.magnitude == 0f && Time.realtimeSinceStartup - this.lastHoverTime > 0.1f)
			{
				this.activeControl.OnMouseHover(new dfMouseEventArgs(this.activeControl, this.buttonsDown, 0, ray, mousePosition, axis));
				this.lastHoverTime = Time.realtimeSinceStartup;
			}
			if (this.buttonsDown == dfMouseButtons.None)
			{
				if (axis != 0f && control != null)
				{
					this.setActive(control, mousePosition, ray);
					control.OnMouseWheel(new dfMouseEventArgs(control, this.buttonsDown, 0, ray, mousePosition, axis));
					return;
				}
				this.setActive(control, mousePosition, ray);
			}
			else if (this.activeControl != null)
			{
				if (control != null)
				{
					control.RenderOrder <= this.activeControl.RenderOrder;
				}
				if (this.mouseMoveDelta.magnitude >= 2f && (this.buttonsDown & (dfMouseButtons.Left | dfMouseButtons.Right)) != dfMouseButtons.None && this.dragState != dfDragDropState.Denied)
				{
					dfDragEventArgs dfDragEventArg5 = new dfDragEventArgs(this.activeControl)
					{
						Position = mousePosition
					};
					this.activeControl.OnDragStart(dfDragEventArg5);
					if (dfDragEventArg5.State == dfDragDropState.Dragging)
					{
						this.dragState = dfDragDropState.Dragging;
						this.dragData = dfDragEventArg5.Data;
						return;
					}
					this.dragState = dfDragDropState.Denied;
				}
			}
			if (this.activeControl != null && this.mouseMoveDelta.magnitude >= 1f)
			{
				dfMouseEventArgs dfMouseEventArg = new dfMouseEventArgs(this.activeControl, this.buttonsDown, 0, ray, mousePosition, axis)
				{
					MoveDelta = this.mouseMoveDelta
				};
				this.activeControl.OnMouseMove(dfMouseEventArg);
			}
		}

		private void setActive(dfControl control, Vector2 position, Ray ray)
		{
			dfMouseEventArgs dfMouseEventArg;
			if (this.activeControl != null && this.activeControl != control)
			{
				dfControl _dfControl = this.activeControl;
				dfMouseEventArg = new dfMouseEventArgs(this.activeControl)
				{
					Position = position,
					Ray = ray
				};
				_dfControl.OnMouseLeave(dfMouseEventArg);
			}
			if (control != null && control != this.activeControl)
			{
				this.lastClickTime = 0f;
				this.lastHoverTime = Time.realtimeSinceStartup + 0.25f;
				dfMouseEventArg = new dfMouseEventArgs(control)
				{
					Position = position,
					Ray = ray
				};
				control.OnMouseEnter(dfMouseEventArg);
			}
			this.activeControl = control;
			this.lastPosition = position;
			this.dragState = dfDragDropState.None;
		}
	}

	private class TouchInputManager
	{
		private List<dfInputManager.TouchInputManager.ControlTouchTracker> tracked;

		private List<int> untracked;

		private dfInputManager manager;

		private TouchInputManager()
		{
		}

		public TouchInputManager(dfInputManager manager)
		{
			this.manager = manager;
		}

		private dfControl clipCast(Transform transform, RaycastHit[] hits)
		{
			if (hits == null || (int)hits.Length == 0)
			{
				return null;
			}
			dfControl _dfControl = null;
			dfControl modalControl = dfGUIManager.GetModalControl();
			for (int i = (int)hits.Length - 1; i >= 0; i--)
			{
				RaycastHit raycastHit = hits[i];
				dfControl component = raycastHit.transform.GetComponent<dfControl>();
				if ((component == null || modalControl != null && !component.transform.IsChildOf(modalControl.transform) || !component.enabled || component.Opacity < 0.01f || !component.IsEnabled || !component.IsVisible ? false : component.transform.IsChildOf(transform)))
				{
					if (this.isInsideClippingRegion(raycastHit, component) && (_dfControl == null || component.RenderOrder > _dfControl.RenderOrder))
					{
						_dfControl = component;
					}
				}
			}
			return _dfControl;
		}

		private bool isInsideClippingRegion(RaycastHit hit, dfControl control)
		{
			Plane[] clippingPlanes;
			Vector3 vector3 = hit.point;
			while (control != null)
			{
				if (!control.ClipChildren)
				{
					clippingPlanes = null;
				}
				else
				{
					clippingPlanes = control.GetClippingPlanes();
				}
				Plane[] planeArray = clippingPlanes;
				if (planeArray != null && (int)planeArray.Length > 0)
				{
					for (int i = 0; i < (int)planeArray.Length; i++)
					{
						if (!planeArray[i].GetSide(vector3))
						{
							return false;
						}
					}
				}
				control = control.Parent;
			}
			return true;
		}

		internal void Process(Transform transform, Camera renderCamera, dfList<Touch> touches, bool retainFocus)
		{
			for (int i = 0; i < touches.Count; i++)
			{
				Touch item = touches[i];
				Ray ray = renderCamera.ScreenPointToRay(item.position);
				float single = renderCamera.farClipPlane - renderCamera.nearClipPlane;
				RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, single, renderCamera.cullingMask);
				dfInputManager.controlUnderMouse = this.clipCast(transform, raycastHitArray);
				if (dfInputManager.controlUnderMouse == null && item.phase == TouchPhase.Began)
				{
					this.untracked.Add(item.fingerId);
				}
				else if (!this.untracked.Contains(item.fingerId))
				{
					dfInputManager.TouchInputManager.TouchRaycast touchRaycast = new dfInputManager.TouchInputManager.TouchRaycast(dfInputManager.controlUnderMouse, item, ray);
					dfInputManager.TouchInputManager.ControlTouchTracker controlTouchTracker = this.tracked.FirstOrDefault<dfInputManager.TouchInputManager.ControlTouchTracker>((dfInputManager.TouchInputManager.ControlTouchTracker x) => x.IsTrackingFinger(touchRaycast.FingerID));
					if (controlTouchTracker == null)
					{
						bool flag = false;
						int num = 0;
						while (num < this.tracked.Count)
						{
							if (!this.tracked[num].Process(touchRaycast))
							{
								num++;
							}
							else
							{
								flag = true;
								break;
							}
						}
						if (!flag && dfInputManager.controlUnderMouse != null)
						{
							if (!this.tracked.Any<dfInputManager.TouchInputManager.ControlTouchTracker>((dfInputManager.TouchInputManager.ControlTouchTracker x) => x.control == dfInputManager.controlUnderMouse))
							{
								if (dfInputManager.controlUnderMouse == null)
								{
									Debug.Log(string.Concat("Tracking touch with no control: ", item.fingerId));
								}
								dfInputManager.TouchInputManager.ControlTouchTracker controlTouchTracker1 = new dfInputManager.TouchInputManager.ControlTouchTracker(this.manager, dfInputManager.controlUnderMouse);
								this.tracked.Add(controlTouchTracker1);
								controlTouchTracker1.Process(touchRaycast);
							}
						}
					}
					else
					{
						controlTouchTracker.Process(touchRaycast);
					}
				}
				else if (item.phase == TouchPhase.Ended)
				{
					this.untracked.Remove(item.fingerId);
				}
			}
		}

		private class ControlTouchTracker
		{
			public dfControl control;

			public Dictionary<int, dfInputManager.TouchInputManager.TouchRaycast> touches;

			public List<int> capture;

			private dfInputManager manager;

			private dfDragDropState dragState;

			private object dragData;

			public bool IsDragging
			{
				get
				{
					return this.dragState == dfDragDropState.Dragging;
				}
			}

			public int TouchCount
			{
				get
				{
					return this.touches.Count;
				}
			}

			public ControlTouchTracker(dfInputManager manager, dfControl control)
			{
				this.manager = manager;
				this.control = control;
			}

			private bool canFireClickEvent(dfInputManager.TouchInputManager.TouchRaycast info, dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				if (this.manager.TouchClickRadius <= 0)
				{
					return true;
				}
				float single = Vector2.Distance(info.position, touch.position);
				return single < (float)this.manager.TouchClickRadius;
			}

			private List<Touch> getActiveTouches()
			{
				Touch[] touchArray = Input.touches;
				List<Touch> list = (
					from x in this.touches
					select x.Value.touch).ToList<Touch>();
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = touchArray.First<Touch>((Touch x) => x.fingerId == list[i].fingerId);
				}
				return list;
			}

			public bool IsTrackingFinger(int fingerID)
			{
				return this.touches.ContainsKey(fingerID);
			}

			public bool Process(dfInputManager.TouchInputManager.TouchRaycast info)
			{
				if (this.IsDragging)
				{
					if (!this.capture.Contains(info.FingerID))
					{
						return false;
					}
					if (info.Phase == TouchPhase.Stationary)
					{
						return true;
					}
					if (info.Phase == TouchPhase.Canceled)
					{
						this.control.OnDragEnd(new dfDragEventArgs(this.control, dfDragDropState.Cancelled, this.dragData, info.ray, info.position));
						this.dragState = dfDragDropState.None;
						this.touches.Clear();
						this.capture.Clear();
						return true;
					}
					if (info.Phase != TouchPhase.Ended)
					{
						return true;
					}
					if (info.control == null || info.control == this.control)
					{
						this.control.OnDragEnd(new dfDragEventArgs(this.control, dfDragDropState.CancelledNoTarget, this.dragData, info.ray, info.position));
						this.dragState = dfDragDropState.None;
						this.touches.Clear();
						this.capture.Clear();
						return true;
					}
					dfDragEventArgs dfDragEventArg = new dfDragEventArgs(info.control, dfDragDropState.Dragging, this.dragData, info.ray, info.position);
					info.control.OnDragDrop(dfDragEventArg);
					if (!dfDragEventArg.Used || dfDragEventArg.State != dfDragDropState.Dropped)
					{
						dfDragEventArg.State = dfDragDropState.Cancelled;
					}
					dfDragEventArgs dfDragEventArg1 = new dfDragEventArgs(this.control, dfDragEventArg.State, this.dragData, info.ray, info.position)
					{
						Target = info.control
					};
					this.control.OnDragEnd(dfDragEventArg1);
					this.dragState = dfDragDropState.None;
					this.touches.Clear();
					this.capture.Clear();
					return true;
				}
				if (!this.touches.ContainsKey(info.FingerID))
				{
					if (info.control != this.control)
					{
						return false;
					}
					this.touches[info.FingerID] = info;
					if (this.touches.Count == 1)
					{
						this.control.OnMouseEnter(info);
						if (info.Phase == TouchPhase.Began)
						{
							this.capture.Add(info.FingerID);
							this.control.OnMouseDown(info);
						}
						return true;
					}
					if (info.Phase == TouchPhase.Began)
					{
						this.control.OnMouseUp(info);
						this.control.OnMouseLeave(info);
						List<Touch> activeTouches = this.getActiveTouches();
						dfTouchEventArgs dfTouchEventArg = new dfTouchEventArgs(this.control, activeTouches, info.ray);
						this.control.OnMultiTouch(dfTouchEventArg);
					}
					return true;
				}
				if (info.Phase == TouchPhase.Canceled || info.Phase == TouchPhase.Ended)
				{
					dfInputManager.TouchInputManager.TouchRaycast item = this.touches[info.FingerID];
					this.touches.Remove(info.FingerID);
					if (this.touches.Count == 0)
					{
						if (this.capture.Contains(info.FingerID))
						{
							if (this.canFireClickEvent(info, item) && info.control == this.control)
							{
								if (info.touch.tapCount <= 1)
								{
									this.control.OnClick(info);
								}
								else
								{
									this.control.OnDoubleClick(info);
								}
							}
							this.control.OnMouseUp(info);
						}
						this.control.OnMouseLeave(info);
						this.capture.Remove(info.FingerID);
						return true;
					}
					this.capture.Remove(info.FingerID);
					if (this.touches.Count == 1)
					{
						dfTouchEventArgs dfTouchEventArg1 = this.touches.Values.First<dfInputManager.TouchInputManager.TouchRaycast>();
						this.control.OnMouseEnter(dfTouchEventArg1);
						this.control.OnMouseDown(dfTouchEventArg1);
						return true;
					}
				}
				if (this.touches.Count > 1)
				{
					List<Touch> touches = this.getActiveTouches();
					dfTouchEventArgs dfTouchEventArg2 = new dfTouchEventArgs(this.control, touches, info.ray);
					this.control.OnMultiTouch(dfTouchEventArg2);
					return true;
				}
				if (!this.IsDragging && info.Phase == TouchPhase.Stationary)
				{
					if (info.control != this.control)
					{
						return false;
					}
					this.control.OnMouseHover(info);
					return true;
				}
				if ((!this.capture.Contains(info.FingerID) || this.dragState != dfDragDropState.None ? false : info.Phase == TouchPhase.Moved))
				{
					dfDragEventArgs dfDragEventArg2 = info;
					this.control.OnDragStart(dfDragEventArg2);
					if (dfDragEventArg2.State == dfDragDropState.Dragging && dfDragEventArg2.Used)
					{
						this.dragState = dfDragDropState.Dragging;
						this.dragData = dfDragEventArg2.Data;
						return true;
					}
					this.dragState = dfDragDropState.Denied;
				}
				if (!(info.control != this.control) || this.capture.Contains(info.FingerID))
				{
					this.control.OnMouseMove(info);
					return true;
				}
				this.control.OnMouseLeave(info);
				this.touches.Remove(info.FingerID);
				return true;
			}
		}

		private class TouchRaycast
		{
			public dfControl control;

			public Touch touch;

			public Ray ray;

			public Vector2 position;

			public int FingerID
			{
				get
				{
					return this.touch.fingerId;
				}
			}

			public TouchPhase Phase
			{
				get
				{
					return this.touch.phase;
				}
			}

			public TouchRaycast(dfControl control, Touch touch, Ray ray)
			{
				this.control = control;
				this.touch = touch;
				this.ray = ray;
				this.position = touch.position;
			}

			public static implicit operator dfTouchEventArgs(dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				return new dfTouchEventArgs(touch.control, touch.touch, touch.ray);
			}

			public static implicit operator dfDragEventArgs(dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				dfDragEventArgs dfDragEventArg = new dfDragEventArgs(touch.control, dfDragDropState.None, null, touch.ray, touch.position);
				return dfDragEventArg;
			}
		}
	}
}