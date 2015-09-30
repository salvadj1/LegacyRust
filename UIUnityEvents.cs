using NGUIHack;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("")]
public class UIUnityEvents : MonoBehaviour
{
	private const int idLoop = 300;

	private const int controlIDHint = 320323492;

	private const string kControlName = "ngui-unityevents";

	private const int kGUIDepth = 49;

	public static bool forbidHandlingNewEvents;

	private UIInput mInput;

	private UICamera mCamera;

	private static bool madeSingleton;

	private readonly static Rect idRect;

	private static int controlID;

	private static UIInput lastInput;

	private static UILabel lastLabel;

	private static UICamera lastInputCamera;

	private static bool submit;

	private static GUIContent textInputContent;

	private static Vector2 lastCursorPosition;

	private static UITextPosition lastTextPosition;

	private static bool requiresBinding;

	private static bool focusSetInOnGUI;

	private static Vector2 lastMousePosition;

	private static int blankID;

	private static bool inOnGUI;

	public static bool shouldBlockButtonInput
	{
		get
		{
			return UIUnityEvents.lastInput;
		}
	}

	private static GUIStyle textStyle
	{
		get
		{
			return GUI.skin.textField;
		}
	}

	static UIUnityEvents()
	{
		UIUnityEvents.idRect = new Rect(0f, 0f, 69999f, 69999f);
		UIUnityEvents.textInputContent = null;
		UIUnityEvents.lastMousePosition = new Vector2(-100f, -100f);
	}

	public UIUnityEvents()
	{
	}

	private void Awake()
	{
		base.useGUILayout = false;
	}

	private static void Bind()
	{
		if (UIUnityEvents.requiresBinding && UIUnityEvents.lastInput && UIUnityEvents.lastInputCamera)
		{
			UIUnityEvents.SetKeyboardControl();
			UIUnityEvents.requiresBinding = false;
			UIUnityEvents.focusSetInOnGUI = true;
		}
	}

	public static void CameraCreated(UICamera camera)
	{
		if (Application.isPlaying && !UIUnityEvents.LateLoaded.singleton)
		{
			Debug.Log("singleton check failed.");
		}
	}

	private static void ChangeFocus(UICamera camera, UIInput input, UILabel label)
	{
		if (UIUnityEvents.lastInput != input)
		{
			UIUnityEvents.lastInput = input;
			UIUnityEvents.textInputContent = null;
			UIUnityEvents.requiresBinding = input;
			UIUnityEvents.focusSetInOnGUI = UIUnityEvents.inOnGUI;
		}
		UIUnityEvents.lastInputCamera = camera;
		UIUnityEvents.lastLabel = label;
	}

	private static bool GetKeyboardControl()
	{
		if (GUIUtility.keyboardControl == UIUnityEvents.controlID)
		{
			return true;
		}
		return false;
	}

	private static bool GetTextEditor(out TextEditor te)
	{
		UIUnityEvents.submit = false;
		if (!UIUnityEvents.focusSetInOnGUI && UIUnityEvents.requiresBinding && UIUnityEvents.lastInput && UIUnityEvents.lastInputCamera)
		{
			GUI.FocusControl("ngui-unityevents");
		}
		UIUnityEvents.Bind();
		te = GUIUtility.GetStateObject(typeof(TextEditor), UIUnityEvents.controlID) as TextEditor;
		if (!UIUnityEvents.lastInput)
		{
			te = null;
			return false;
		}
		GUIContent gUIContent = UIUnityEvents.textInputContent;
		if (gUIContent == null)
		{
			gUIContent = new GUIContent();
			UIUnityEvents.textInputContent = gUIContent;
		}
		gUIContent.text = UIUnityEvents.lastInput.inputText;
		te.content.text = UIUnityEvents.textInputContent.text;
		te.SaveBackup();
		te.position = UIUnityEvents.idRect;
		te.style = UIUnityEvents.textStyle;
		te.multiline = UIUnityEvents.lastInput.inputMultiline;
		te.controlID = UIUnityEvents.controlID;
		te.ClampPos();
		return true;
	}

	private static bool MoveTextPosition(UnityEngine.Event @event, TextEditor te, ref UITextPosition res)
	{
		UIUnityEvents.lastTextPosition = res;
		if (!res.valid)
		{
			return false;
		}
		te.pos = res.uniformPosition;
		if (!@event.shift)
		{
			te.selectPos = te.pos;
		}
		return true;
	}

	private void OnDestroy()
	{
		if (UIUnityEvents.madeSingleton && UIUnityEvents.LateLoaded.singleton == this)
		{
			UIUnityEvents.LateLoaded.singleton = null;
		}
	}

	private void OnGUI()
	{
		try
		{
			UIUnityEvents.inOnGUI = true;
			GUI.depth = 49;
			UIUnityEvents.blankID = GUIUtility.GetControlID(FocusType.Keyboard);
			GUI.SetNextControlName("ngui-unityevents");
			UIUnityEvents.controlID = GUIUtility.GetControlID(FocusType.Keyboard);
			GUI.color = Color.clear;
			UnityEngine.Event @event = UnityEngine.Event.current;
			EventType eventType = @event.type;
			if (eventType == EventType.MouseMove)
			{
				Debug.Log("Mouse Move");
			}
			switch (eventType)
			{
				case EventType.MouseDown:
				{
					if (!UIUnityEvents.forbidHandlingNewEvents)
					{
						bool flag = @event.button == 0;
						using (NGUIHack.Event event1 = new NGUIHack.Event(@event))
						{
							UICamera.HandleEvent(event1, eventType);
						}
						if (flag && @event.type == EventType.Used && GUIUtility.hotControl == 0)
						{
							GUIUtility.hotControl = UIUnityEvents.blankID;
						}
					}
					goto case EventType.DragPerform;
				}
				case EventType.MouseUp:
				{
					bool flag1 = @event.button == 0;
					using (NGUIHack.Event event2 = new NGUIHack.Event(@event))
					{
						UICamera.HandleEvent(event2, eventType);
					}
					if (flag1 && GUIUtility.hotControl == UIUnityEvents.blankID)
					{
						GUIUtility.hotControl = 0;
					}
					goto case EventType.DragPerform;
				}
				case EventType.MouseMove:
				case EventType.MouseDrag:
				case EventType.KeyUp:
				case EventType.ScrollWheel:
				{
					using (NGUIHack.Event event3 = new NGUIHack.Event(@event))
					{
						UICamera.HandleEvent(event3, eventType);
					}
					goto case EventType.DragPerform;
				}
				case EventType.KeyDown:
				{
					if (!UIUnityEvents.forbidHandlingNewEvents)
					{
						using (NGUIHack.Event event4 = new NGUIHack.Event(@event))
						{
							UICamera.HandleEvent(event4, eventType);
						}
					}
					goto case EventType.DragPerform;
				}
				case EventType.Repaint:
				{
					if (!UIUnityEvents.forbidHandlingNewEvents && UIUnityEvents.lastMousePosition != @event.mousePosition)
					{
						UIUnityEvents.lastMousePosition = @event.mousePosition;
						using (NGUIHack.Event event5 = new NGUIHack.Event(@event, EventType.MouseMove))
						{
							UICamera.HandleEvent(event5, EventType.MouseMove);
						}
					}
					goto case EventType.DragPerform;
				}
				case EventType.DragUpdated:
				case EventType.DragPerform:
				{
				Label0:
					eventType != EventType.Repaint;
					break;
				}
				case EventType.Used:
				{
					Debug.Log("Used");
					break;
				}
				default:
				{
					goto case EventType.DragPerform;
				}
			}
		}
		finally
		{
			UIUnityEvents.inOnGUI = false;
		}
	}

	private static bool Perform(TextEditor te, UIUnityEvents.TextEditOp operation)
	{
		return UIUnityEvents.PerformOperation(te, operation);
	}

	private static bool PerformOperation(TextEditor te, UIUnityEvents.TextEditOp operation)
	{
		switch (operation)
		{
			case UIUnityEvents.TextEditOp.MoveLeft:
			{
				te.MoveLeft();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveRight:
			{
				te.MoveRight();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveUp:
			{
				te.MoveUp();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveDown:
			{
				te.MoveDown();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveLineStart:
			{
				te.MoveLineStart();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveLineEnd:
			{
				te.MoveLineEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveTextStart:
			{
				te.MoveTextStart();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveTextEnd:
			{
				te.MoveTextEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.MovePageUp:
			case UIUnityEvents.TextEditOp.MovePageDown:
			case UIUnityEvents.TextEditOp.SelectPageUp:
			case UIUnityEvents.TextEditOp.SelectPageDown:
			{
				Debug.Log(string.Concat("Unimplemented: ", operation));
				break;
			}
			case UIUnityEvents.TextEditOp.MoveGraphicalLineStart:
			{
				te.MoveGraphicalLineStart();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveGraphicalLineEnd:
			{
				te.MoveGraphicalLineEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveWordLeft:
			{
				te.MoveWordLeft();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveWordRight:
			{
				te.MoveWordRight();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveParagraphForward:
			{
				te.MoveParagraphForward();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveParagraphBackward:
			{
				te.MoveParagraphBackward();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveToStartOfNextWord:
			{
				te.MoveToStartOfNextWord();
				break;
			}
			case UIUnityEvents.TextEditOp.MoveToEndOfPreviousWord:
			{
				te.MoveToEndOfPreviousWord();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectLeft:
			{
				te.SelectLeft();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectRight:
			{
				te.SelectRight();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectUp:
			{
				te.SelectUp();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectDown:
			{
				te.SelectDown();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectTextStart:
			{
				te.SelectTextStart();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectTextEnd:
			{
				te.SelectTextEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineStart:
			{
				te.ExpandSelectGraphicalLineStart();
				break;
			}
			case UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineEnd:
			{
				te.ExpandSelectGraphicalLineEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectGraphicalLineStart:
			{
				te.SelectGraphicalLineStart();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectGraphicalLineEnd:
			{
				te.SelectGraphicalLineEnd();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectWordLeft:
			{
				te.SelectWordLeft();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectWordRight:
			{
				te.SelectWordRight();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectToEndOfPreviousWord:
			{
				te.SelectToEndOfPreviousWord();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectToStartOfNextWord:
			{
				te.SelectToStartOfNextWord();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectParagraphBackward:
			{
				te.SelectParagraphBackward();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectParagraphForward:
			{
				te.SelectParagraphForward();
				break;
			}
			case UIUnityEvents.TextEditOp.Delete:
			{
				return te.Delete();
			}
			case UIUnityEvents.TextEditOp.Backspace:
			{
				return te.Backspace();
			}
			case UIUnityEvents.TextEditOp.DeleteWordBack:
			{
				return te.DeleteWordBack();
			}
			case UIUnityEvents.TextEditOp.DeleteWordForward:
			{
				return te.DeleteWordForward();
			}
			case UIUnityEvents.TextEditOp.Cut:
			{
				return te.Cut();
			}
			case UIUnityEvents.TextEditOp.Copy:
			{
				te.Copy();
				break;
			}
			case UIUnityEvents.TextEditOp.Paste:
			{
				return te.Paste();
			}
			case UIUnityEvents.TextEditOp.SelectAll:
			{
				te.SelectAll();
				break;
			}
			case UIUnityEvents.TextEditOp.SelectNone:
			{
				te.SelectNone();
				break;
			}
			default:
			{
				goto case UIUnityEvents.TextEditOp.SelectPageDown;
			}
		}
		return false;
	}

	internal static bool RequestKeyboardFocus(UIInput input)
	{
		if (input == UIUnityEvents.lastInput)
		{
			return true;
		}
		if (UIUnityEvents.lastInput)
		{
			return false;
		}
		if (!input.label || !input.label.enabled)
		{
			return false;
		}
		UICamera uICamera = UICamera.FindCameraForLayer(input.label.gameObject.layer);
		if (!uICamera)
		{
			return false;
		}
		if (!uICamera.SetKeyboardFocus(input))
		{
			return false;
		}
		UIUnityEvents.ChangeFocus(uICamera, input, input.label);
		return true;
	}

	private static bool SelectTextPosition(UnityEngine.Event @event, TextEditor te, ref UITextPosition res)
	{
		UIUnityEvents.lastTextPosition = res;
		if (!res.valid)
		{
			return false;
		}
		UIUnityEvents.lastCursorPosition = UIUnityEvents.textStyle.GetCursorPixelPosition(UIUnityEvents.idRect, UIUnityEvents.textInputContent, res.uniformPosition);
		te.SelectToPosition(UIUnityEvents.lastCursorPosition);
		return true;
	}

	private static bool SetKeyboardControl()
	{
		GUIUtility.keyboardControl = UIUnityEvents.controlID;
		return GUIUtility.keyboardControl == UIUnityEvents.controlID;
	}

	internal static void TextClickDown(UICamera camera, UIInput input, NGUIHack.Event @event, UILabel label)
	{
		UIUnityEvents.TextClickDown(camera, input, @event.real, label);
	}

	private static void TextClickDown(UICamera camera, UIInput input, UnityEngine.Event @event, UILabel label)
	{
		UITextPosition uITextPosition = (!@event.shift ? camera.RaycastText(Input.mousePosition, label) : new UITextPosition());
		TextEditor textEditor = null;
		UIUnityEvents.ChangeFocus(camera, input, label);
		if (UIUnityEvents.GetTextEditor(out textEditor))
		{
			GUIUtility.hotControl = UIUnityEvents.controlID;
			UIUnityEvents.SetKeyboardControl();
			UIUnityEvents.MoveTextPosition(@event, textEditor, ref uITextPosition);
			int num = @event.clickCount;
			if (num == 2)
			{
				textEditor.SelectCurrentWord();
				textEditor.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
				textEditor.MouseDragSelectsWholeWords(true);
			}
			else if (num == 3)
			{
				if (input.trippleClickSelect)
				{
					textEditor.SelectCurrentParagraph();
					textEditor.MouseDragSelectsWholeWords(true);
					textEditor.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
				}
			}
			@event.Use();
		}
		else
		{
			Debug.LogError("Null Text Editor");
		}
		UIUnityEvents.TextSharedEnd(false, textEditor, @event);
	}

	internal static void TextClickUp(UICamera camera, UIInput input, NGUIHack.Event @event, UILabel label)
	{
		UIUnityEvents.TextClickUp(camera, input, @event.real, label);
	}

	private static void TextClickUp(UICamera camera, UIInput input, UnityEngine.Event @event, UILabel label)
	{
		if (input == UIUnityEvents.lastInput && camera == UIUnityEvents.lastInputCamera)
		{
			UIUnityEvents.lastLabel = label;
			TextEditor textEditor = null;
			if (!UIUnityEvents.GetTextEditor(out textEditor))
			{
				return;
			}
			if (UIUnityEvents.controlID != GUIUtility.hotControl)
			{
				Debug.Log(string.Concat(new object[] { "Did not match ", UIUnityEvents.controlID, " ", GUIUtility.hotControl }));
			}
			else
			{
				textEditor.MouseDragSelectsWholeWords(false);
				GUIUtility.hotControl = 0;
				@event.Use();
				UIUnityEvents.SetKeyboardControl();
			}
			UIUnityEvents.TextSharedEnd(false, textEditor, @event);
		}
	}

	internal static void TextDrag(UICamera camera, UIInput input, NGUIHack.Event @event, UILabel label)
	{
		UIUnityEvents.TextDrag(camera, input, @event.real, label);
	}

	private static void TextDrag(UICamera camera, UIInput input, UnityEngine.Event @event, UILabel label)
	{
		if (input == UIUnityEvents.lastInput && camera == UIUnityEvents.lastInputCamera)
		{
			UIUnityEvents.lastLabel = label;
			TextEditor textEditor = null;
			if (!UIUnityEvents.GetTextEditor(out textEditor))
			{
				return;
			}
			if (UIUnityEvents.controlID == GUIUtility.hotControl)
			{
				UITextPosition uITextPosition = camera.RaycastText(Input.mousePosition, label);
				if (@event.shift)
				{
					UIUnityEvents.MoveTextPosition(@event, textEditor, ref uITextPosition);
				}
				else
				{
					UIUnityEvents.SelectTextPosition(@event, textEditor, ref uITextPosition);
				}
				@event.Use();
			}
			UIUnityEvents.TextSharedEnd(false, textEditor, @event);
		}
	}

	private static bool TextEditorHandleEvent(UnityEngine.Event e, TextEditor te)
	{
		bool flag;
		EventModifiers eventModifier = e.modifiers;
		if ((eventModifier & EventModifiers.CapsLock) != EventModifiers.CapsLock)
		{
			return UIUnityEvents.TextEditorHandleEvent2(e, te);
		}
		try
		{
			e.modifiers = eventModifier & (EventModifiers.Shift | EventModifiers.Control | EventModifiers.Alt | EventModifiers.Command | EventModifiers.Numeric | EventModifiers.FunctionKey);
			flag = UIUnityEvents.TextEditorHandleEvent2(e, te);
		}
		finally
		{
			e.modifiers = eventModifier;
		}
		return flag;
	}

	private static bool TextEditorHandleEvent2(UnityEngine.Event e, TextEditor te)
	{
		if (!UIUnityEvents.LateLoaded.Keyactions.Contains(e))
		{
			return false;
		}
		UIUnityEvents.Perform(te, (UIUnityEvents.TextEditOp)Convert.ToInt32(UIUnityEvents.LateLoaded.Keyactions[e]));
		return true;
	}

	internal static void TextGainFocus(UIInput input)
	{
	}

	internal static void TextKeyDown(UICamera camera, UIInput input, NGUIHack.Event @event, UILabel label)
	{
		UIUnityEvents.TextKeyDown(camera, input, @event.real, label);
	}

	private static void TextKeyDown(UICamera camera, UIInput input, UnityEngine.Event @event, UILabel label)
	{
		if (input == UIUnityEvents.lastInput && camera == UIUnityEvents.lastInputCamera)
		{
			UIUnityEvents.lastLabel = label;
			TextEditor textEditor = null;
			if (!UIUnityEvents.GetTextEditor(out textEditor))
			{
				return;
			}
			if (!UIUnityEvents.GetKeyboardControl())
			{
				Debug.Log(string.Concat("Did not ", @event));
				return;
			}
			bool flag = false;
			if (!UIUnityEvents.TextEditorHandleEvent(@event, textEditor))
			{
				KeyCode keyCode = @event.keyCode;
				if (keyCode == KeyCode.Tab)
				{
					return;
				}
				if (keyCode == KeyCode.None)
				{
					char chr = @event.character;
					if (chr == '\t')
					{
						return;
					}
					bool flag1 = false;
					flag1 = chr == '\n';
					if (flag1 && !input.inputMultiline && !@event.alt)
					{
						UIUnityEvents.submit = true;
					}
					else if (label.font)
					{
						BMFont bMFont = label.font.bmFont;
						BMFont bMFont1 = bMFont;
						if (bMFont != null)
						{
							if (flag1 || chr != 0 && bMFont1.ContainsGlyph(chr))
							{
								textEditor.Insert(chr);
								flag = true;
							}
							else if (chr == 0)
							{
								if (Input.compositionString.Length > 0)
								{
									textEditor.ReplaceSelection(string.Empty);
									flag = true;
								}
								@event.Use();
							}
						}
					}
				}
			}
			else
			{
				@event.Use();
				flag = true;
			}
			UIUnityEvents.TextSharedEnd(flag, textEditor, @event);
		}
	}

	internal static void TextKeyUp(UICamera camera, UIInput input, NGUIHack.Event @event, UILabel label)
	{
		UIUnityEvents.TextKeyUp(camera, input, @event.real, label);
	}

	private static void TextKeyUp(UICamera camera, UIInput input, UnityEngine.Event @event, UILabel label)
	{
		if (input == UIUnityEvents.lastInput && camera == UIUnityEvents.lastInputCamera)
		{
			UIUnityEvents.lastLabel = label;
			TextEditor textEditor = null;
			if (!UIUnityEvents.GetTextEditor(out textEditor))
			{
				return;
			}
			UIUnityEvents.TextSharedEnd(false, textEditor, @event);
		}
	}

	internal static void TextLostFocus(UIInput input)
	{
		if (input == UIUnityEvents.lastInput)
		{
			if (UIUnityEvents.lastInputCamera && UICamera.selectedObject == input)
			{
				UICamera.selectedObject = null;
			}
			UIUnityEvents.lastInput = null;
			UIUnityEvents.lastInputCamera = null;
			UIUnityEvents.lastLabel = null;
		}
	}

	private static void TextSharedEnd(bool changed, TextEditor te, UnityEngine.Event @event)
	{
		if (UIUnityEvents.GetKeyboardControl())
		{
			UIUnityEvents.LateLoaded.textFieldInput = true;
		}
		if (changed || @event.type == EventType.Used)
		{
			if (UIUnityEvents.lastInput)
			{
				UIUnityEvents.textInputContent.text = te.content.text;
			}
			if (!changed)
			{
				UIUnityEvents.lastInput.CheckPositioning(te.pos, te.selectPos);
			}
			else
			{
				GUI.changed = true;
				UIUnityEvents.lastInput.CheckChanges(UIUnityEvents.textInputContent.text);
				UIUnityEvents.lastInput.CheckPositioning(te.pos, te.selectPos);
				@event.Use();
			}
		}
		if (UIUnityEvents.submit)
		{
			UIUnityEvents.submit = false;
			if (UIUnityEvents.lastInput.SendSubmitMessage())
			{
				@event.Use();
			}
		}
	}

	private static class LateLoaded
	{
		public readonly static GUIStyle mTextBlockStyle;

		private readonly static PropertyInfo _textFieldInput;

		public static UIUnityEvents singleton;

		public static Hashtable Keyactions;

		private static bool failedInvokeTextFieldInputGet;

		private static bool failedInvokeTextFieldInputSet;

		public static bool textFieldInput
		{
			get
			{
				bool value;
				if (!UIUnityEvents.LateLoaded.failedInvokeTextFieldInputGet)
				{
					try
					{
						value = (bool)UIUnityEvents.LateLoaded._textFieldInput.GetValue(null, null);
					}
					catch (MethodAccessException methodAccessException1)
					{
						MethodAccessException methodAccessException = methodAccessException1;
						UIUnityEvents.LateLoaded.failedInvokeTextFieldInputGet = true;
						Debug.Log(string.Concat("Can not get GUIUtility.textFieldInput\r\n", methodAccessException));
						return false;
					}
					return value;
				}
				return false;
			}
			set
			{
				if (!UIUnityEvents.LateLoaded.failedInvokeTextFieldInputSet)
				{
					try
					{
						UIUnityEvents.LateLoaded._textFieldInput.SetValue(null, value, null);
					}
					catch (MethodAccessException methodAccessException1)
					{
						MethodAccessException methodAccessException = methodAccessException1;
						UIUnityEvents.LateLoaded.failedInvokeTextFieldInputSet = true;
						Debug.Log(string.Concat("Can not set GUIUtility.textFieldInput\r\n", methodAccessException));
					}
				}
			}
		}

		static LateLoaded()
		{
			UIUnityEvents.LateLoaded.mTextBlockStyle = new GUIStyle()
			{
				alignment = TextAnchor.UpperLeft,
				border = new RectOffset(0, 0, 0, 0),
				clipping = TextClipping.Overflow,
				contentOffset = new Vector2(),
				fixedWidth = -1f,
				fixedHeight = -1f,
				imagePosition = ImagePosition.TextOnly,
				margin = new RectOffset(0, 0, 0, 0),
				name = "BLOCK STYLE",
				overflow = new RectOffset(0, 0, 0, 0),
				padding = new RectOffset(0, 0, 0, 0),
				stretchHeight = false,
				stretchWidth = false,
				wordWrap = false
			};
			GUIStyleState gUIStyleState = new GUIStyleState()
			{
				background = null,
				textColor = Color.clear
			};
			UIUnityEvents.LateLoaded.mTextBlockStyle.active = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.focused = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.hover = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.normal = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.onActive = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.onFocused = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.onHover = gUIStyleState;
			UIUnityEvents.LateLoaded.mTextBlockStyle.onNormal = gUIStyleState;
			UIUnityEvents.LateLoaded._textFieldInput = typeof(GUIUtility).GetProperty("textFieldInput", BindingFlags.Static | BindingFlags.NonPublic);
			if (UIUnityEvents.LateLoaded._textFieldInput == null)
			{
				UIUnityEvents.LateLoaded.failedInvokeTextFieldInputGet = true;
				UIUnityEvents.LateLoaded.failedInvokeTextFieldInputSet = true;
				Debug.LogError("Unity has changed. no bool property textFieldInput in GUIUtility");
			}
			GameObject gameObject = new GameObject("__UIUnityEvents", new Type[] { typeof(UIUnityEvents) });
			UIUnityEvents.LateLoaded.singleton = gameObject.GetComponent<UIUnityEvents>();
			UIUnityEvents.madeSingleton = true;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			if (null != null)
			{
				Debug.Log("Thats imposible.");
			}
			try
			{
				MethodInfo method = typeof(TextEditor).GetMethod("InitKeyActions", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method == null)
				{
					throw new MethodAccessException("Unity has changed. no InitKeyActions member in TextEditor");
				}
				method.Invoke(new TextEditor(), new object[0]);
				object obj = typeof(TextEditor).InvokeMember("s_Keyactions", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField, null, null, new object[0]);
				if (!(obj is Hashtable))
				{
					if (!(obj is IDictionary))
					{
						throw new MethodAccessException("Unity has changed. no s_Keyactions member in TextEditor");
					}
					UIUnityEvents.LateLoaded.Keyactions = new Hashtable(obj as IDictionary);
				}
				else
				{
					UIUnityEvents.LateLoaded.Keyactions = (Hashtable)obj;
				}
			}
			catch (MethodAccessException methodAccessException)
			{
				Debug.Log(string.Concat("Caught exception \r\n", methodAccessException, "\r\nManually building keyactions."));
				UIUnityEvents.LateLoaded.Keyactions = new Hashtable();
				UIUnityEvents.LateLoaded.MapKey("left", UIUnityEvents.TextEditOp.MoveLeft);
				UIUnityEvents.LateLoaded.MapKey("right", UIUnityEvents.TextEditOp.MoveRight);
				UIUnityEvents.LateLoaded.MapKey("up", UIUnityEvents.TextEditOp.MoveUp);
				UIUnityEvents.LateLoaded.MapKey("down", UIUnityEvents.TextEditOp.MoveDown);
				UIUnityEvents.LateLoaded.MapKey("#left", UIUnityEvents.TextEditOp.SelectLeft);
				UIUnityEvents.LateLoaded.MapKey("#right", UIUnityEvents.TextEditOp.SelectRight);
				UIUnityEvents.LateLoaded.MapKey("#up", UIUnityEvents.TextEditOp.SelectUp);
				UIUnityEvents.LateLoaded.MapKey("#down", UIUnityEvents.TextEditOp.SelectDown);
				UIUnityEvents.LateLoaded.MapKey("delete", UIUnityEvents.TextEditOp.Delete);
				UIUnityEvents.LateLoaded.MapKey("backspace", UIUnityEvents.TextEditOp.Backspace);
				UIUnityEvents.LateLoaded.MapKey("#backspace", UIUnityEvents.TextEditOp.Backspace);
				if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor)
				{
					UIUnityEvents.LateLoaded.MapKey("home", UIUnityEvents.TextEditOp.MoveGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("end", UIUnityEvents.TextEditOp.MoveGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("%left", UIUnityEvents.TextEditOp.MoveWordLeft);
					UIUnityEvents.LateLoaded.MapKey("%right", UIUnityEvents.TextEditOp.MoveWordRight);
					UIUnityEvents.LateLoaded.MapKey("%up", UIUnityEvents.TextEditOp.MoveParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("%down", UIUnityEvents.TextEditOp.MoveParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("^left", UIUnityEvents.TextEditOp.MoveToEndOfPreviousWord);
					UIUnityEvents.LateLoaded.MapKey("^right", UIUnityEvents.TextEditOp.MoveToStartOfNextWord);
					UIUnityEvents.LateLoaded.MapKey("^up", UIUnityEvents.TextEditOp.MoveParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("^down", UIUnityEvents.TextEditOp.MoveParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("#^left", UIUnityEvents.TextEditOp.SelectToEndOfPreviousWord);
					UIUnityEvents.LateLoaded.MapKey("#^right", UIUnityEvents.TextEditOp.SelectToStartOfNextWord);
					UIUnityEvents.LateLoaded.MapKey("#^up", UIUnityEvents.TextEditOp.SelectParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("#^down", UIUnityEvents.TextEditOp.SelectParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("#home", UIUnityEvents.TextEditOp.SelectGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("#end", UIUnityEvents.TextEditOp.SelectGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("^delete", UIUnityEvents.TextEditOp.DeleteWordForward);
					UIUnityEvents.LateLoaded.MapKey("^backspace", UIUnityEvents.TextEditOp.DeleteWordBack);
					UIUnityEvents.LateLoaded.MapKey("^a", UIUnityEvents.TextEditOp.SelectAll);
					UIUnityEvents.LateLoaded.MapKey("^x", UIUnityEvents.TextEditOp.Cut);
					UIUnityEvents.LateLoaded.MapKey("^c", UIUnityEvents.TextEditOp.Copy);
					UIUnityEvents.LateLoaded.MapKey("^v", UIUnityEvents.TextEditOp.Paste);
					UIUnityEvents.LateLoaded.MapKey("#delete", UIUnityEvents.TextEditOp.Cut);
					UIUnityEvents.LateLoaded.MapKey("^insert", UIUnityEvents.TextEditOp.Copy);
					UIUnityEvents.LateLoaded.MapKey("#insert", UIUnityEvents.TextEditOp.Paste);
				}
				else
				{
					UIUnityEvents.LateLoaded.MapKey("^left", UIUnityEvents.TextEditOp.MoveGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("^right", UIUnityEvents.TextEditOp.MoveGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("&left", UIUnityEvents.TextEditOp.MoveWordLeft);
					UIUnityEvents.LateLoaded.MapKey("&right", UIUnityEvents.TextEditOp.MoveWordRight);
					UIUnityEvents.LateLoaded.MapKey("&up", UIUnityEvents.TextEditOp.MoveParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("&down", UIUnityEvents.TextEditOp.MoveParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("%left", UIUnityEvents.TextEditOp.MoveGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("%right", UIUnityEvents.TextEditOp.MoveGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("%up", UIUnityEvents.TextEditOp.MoveTextStart);
					UIUnityEvents.LateLoaded.MapKey("%down", UIUnityEvents.TextEditOp.MoveTextEnd);
					UIUnityEvents.LateLoaded.MapKey("#home", UIUnityEvents.TextEditOp.SelectTextStart);
					UIUnityEvents.LateLoaded.MapKey("#end", UIUnityEvents.TextEditOp.SelectTextEnd);
					UIUnityEvents.LateLoaded.MapKey("#^left", UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("#^right", UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("#^up", UIUnityEvents.TextEditOp.SelectParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("#^down", UIUnityEvents.TextEditOp.SelectParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("#&left", UIUnityEvents.TextEditOp.SelectWordLeft);
					UIUnityEvents.LateLoaded.MapKey("#&right", UIUnityEvents.TextEditOp.SelectWordRight);
					UIUnityEvents.LateLoaded.MapKey("#&up", UIUnityEvents.TextEditOp.SelectParagraphBackward);
					UIUnityEvents.LateLoaded.MapKey("#&down", UIUnityEvents.TextEditOp.SelectParagraphForward);
					UIUnityEvents.LateLoaded.MapKey("#%left", UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineStart);
					UIUnityEvents.LateLoaded.MapKey("#%right", UIUnityEvents.TextEditOp.ExpandSelectGraphicalLineEnd);
					UIUnityEvents.LateLoaded.MapKey("#%up", UIUnityEvents.TextEditOp.SelectTextStart);
					UIUnityEvents.LateLoaded.MapKey("#%down", UIUnityEvents.TextEditOp.SelectTextEnd);
					UIUnityEvents.LateLoaded.MapKey("%a", UIUnityEvents.TextEditOp.SelectAll);
					UIUnityEvents.LateLoaded.MapKey("%x", UIUnityEvents.TextEditOp.Cut);
					UIUnityEvents.LateLoaded.MapKey("%c", UIUnityEvents.TextEditOp.Copy);
					UIUnityEvents.LateLoaded.MapKey("%v", UIUnityEvents.TextEditOp.Paste);
					UIUnityEvents.LateLoaded.MapKey("^d", UIUnityEvents.TextEditOp.Delete);
					UIUnityEvents.LateLoaded.MapKey("^h", UIUnityEvents.TextEditOp.Backspace);
					UIUnityEvents.LateLoaded.MapKey("^b", UIUnityEvents.TextEditOp.MoveLeft);
					UIUnityEvents.LateLoaded.MapKey("^f", UIUnityEvents.TextEditOp.MoveRight);
					UIUnityEvents.LateLoaded.MapKey("^a", UIUnityEvents.TextEditOp.MoveLineStart);
					UIUnityEvents.LateLoaded.MapKey("^e", UIUnityEvents.TextEditOp.MoveLineEnd);
					UIUnityEvents.LateLoaded.MapKey("&delete", UIUnityEvents.TextEditOp.DeleteWordForward);
					UIUnityEvents.LateLoaded.MapKey("&backspace", UIUnityEvents.TextEditOp.DeleteWordBack);
				}
			}
		}

		private static void MapKey(string key, UIUnityEvents.TextEditOp action)
		{
			UIUnityEvents.LateLoaded.Keyactions[UnityEngine.Event.KeyboardEvent(key)] = action;
		}
	}

	private enum TextEditOp
	{
		MoveLeft,
		MoveRight,
		MoveUp,
		MoveDown,
		MoveLineStart,
		MoveLineEnd,
		MoveTextStart,
		MoveTextEnd,
		MovePageUp,
		MovePageDown,
		MoveGraphicalLineStart,
		MoveGraphicalLineEnd,
		MoveWordLeft,
		MoveWordRight,
		MoveParagraphForward,
		MoveParagraphBackward,
		MoveToStartOfNextWord,
		MoveToEndOfPreviousWord,
		SelectLeft,
		SelectRight,
		SelectUp,
		SelectDown,
		SelectTextStart,
		SelectTextEnd,
		SelectPageUp,
		SelectPageDown,
		ExpandSelectGraphicalLineStart,
		ExpandSelectGraphicalLineEnd,
		SelectGraphicalLineStart,
		SelectGraphicalLineEnd,
		SelectWordLeft,
		SelectWordRight,
		SelectToEndOfPreviousWord,
		SelectToStartOfNextWord,
		SelectParagraphBackward,
		SelectParagraphForward,
		Delete,
		Backspace,
		DeleteWordBack,
		DeleteWordForward,
		Cut,
		Copy,
		Paste,
		SelectAll,
		SelectNone,
		ScrollStart,
		ScrollEnd,
		ScrollPageUp,
		ScrollPageDown
	}
}