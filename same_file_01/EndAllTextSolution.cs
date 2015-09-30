using System;
using System.Reflection;
using UnityEngine;

public class EndAllTextSolution : MonoBehaviour
{
	public GUIContent content = new GUIContent();

	[SerializeField]
	private string styleName = "textfield";

	[SerializeField]
	private bool multiLine;

	[SerializeField]
	private int maxLength;

	private static bool changed
	{
		get
		{
			return GUI.changed;
		}
		set
		{
			GUI.changed = value;
		}
	}

	private static GUISkin skin
	{
		get
		{
			return GUI.skin;
		}
	}

	public EndAllTextSolution()
	{
	}

	private static void DoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style)
	{
		if (maxLength >= 0 && content.text.Length > maxLength)
		{
			content.text = content.text.Substring(0, maxLength);
		}
		EndAllTextSolution.GUI2.CheckOnGUI();
		TextEditor stateObject = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), id);
		stateObject.content.text = content.text;
		stateObject.SaveBackup();
		stateObject.position = position;
		stateObject.style = style;
		stateObject.multiline = multiline;
		stateObject.controlID = id;
		stateObject.ClampPos();
		Event @event = Event.current;
		bool flag = false;
		switch (@event.type)
		{
			case EventType.MouseDown:
			{
				if (position.Contains(@event.mousePosition))
				{
					GUIUtility.hotControl = id;
					GUIUtility.keyboardControl = id;
					stateObject.MoveCursorToPosition(Event.current.mousePosition);
					if (Event.current.clickCount == 2 && EndAllTextSolution.skin.settings.doubleClickSelectsWord)
					{
						stateObject.SelectCurrentWord();
						stateObject.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
						stateObject.MouseDragSelectsWholeWords(true);
					}
					if (Event.current.clickCount == 3 && EndAllTextSolution.skin.settings.tripleClickSelectsLine)
					{
						stateObject.SelectCurrentParagraph();
						stateObject.MouseDragSelectsWholeWords(true);
						stateObject.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
					}
					@event.Use();
				}
				break;
			}
			case EventType.MouseUp:
			{
				if (GUIUtility.hotControl == id)
				{
					stateObject.MouseDragSelectsWholeWords(false);
					GUIUtility.hotControl = 0;
					@event.Use();
				}
				break;
			}
			case EventType.MouseMove:
			case EventType.KeyUp:
			case EventType.ScrollWheel:
			{
				break;
			}
			case EventType.MouseDrag:
			{
				if (GUIUtility.hotControl == id)
				{
					if (@event.shift)
					{
						stateObject.MoveCursorToPosition(Event.current.mousePosition);
					}
					else
					{
						stateObject.SelectToPosition(Event.current.mousePosition);
					}
					@event.Use();
					break;
				}
				else
				{
					break;
				}
			}
			case EventType.KeyDown:
			{
				if (GUIUtility.keyboardControl != id)
				{
					return;
				}
				if (!stateObject.HandleKeyEvent(@event))
				{
					if (@event.keyCode == KeyCode.Tab || @event.character == '\t')
					{
						return;
					}
					char chr = @event.character;
					if (chr == '\n' && !multiline && !@event.alt)
					{
						return;
					}
					Font font = style.font;
					if (font == null)
					{
						font = EndAllTextSolution.skin.font;
					}
					if (font.HasCharacter(chr) || chr == '\n')
					{
						stateObject.Insert(chr);
						flag = true;
					}
					else if (chr == 0)
					{
						if (Input.compositionString.Length > 0)
						{
							stateObject.ReplaceSelection(string.Empty);
							flag = true;
						}
						@event.Use();
					}
				}
				else
				{
					@event.Use();
					flag = true;
					content.text = stateObject.content.text;
				}
				break;
			}
			case EventType.Repaint:
			{
				if (GUIUtility.keyboardControl != id)
				{
					style.Draw(position, content, id, false);
				}
				else
				{
					stateObject.DrawCursor(content.text);
				}
				break;
			}
			default:
			{
				goto case EventType.ScrollWheel;
			}
		}
		if (GUIUtility.keyboardControl == id)
		{
			EndAllTextSolution.GUI2.textFieldInput = true;
		}
		if (flag)
		{
			EndAllTextSolution.changed = true;
			content.text = stateObject.content.text;
			if (maxLength >= 0 && content.text.Length > maxLength)
			{
				content.text = content.text.Substring(0, maxLength);
			}
			@event.Use();
		}
	}

	private void OnGUI()
	{
		int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
		EndAllTextSolution.DoTextField(new Rect(0f, 0f, (float)Screen.width, 30f), controlID, this.content, this.multiLine, this.maxLength, this.styleName);
	}

	private static class GUI2
	{
		public readonly static EndAllTextSolution.VoidCall CheckOnGUI;

		private readonly static PropertyInfo textFieldInputProperty;

		private readonly static object boxed_true;

		private readonly static object boxed_false;

		public static bool textFieldInput
		{
			get
			{
				return (bool)EndAllTextSolution.GUI2.textFieldInputProperty.GetValue(null, null);
			}
			set
			{
				EndAllTextSolution.GUI2.textFieldInputProperty.SetValue(null, (!value ? EndAllTextSolution.GUI2.boxed_false : EndAllTextSolution.GUI2.boxed_true), null);
			}
		}

		static GUI2()
		{
			EndAllTextSolution.GUI2.boxed_true = true;
			EndAllTextSolution.GUI2.boxed_false = false;
			MethodInfo method = typeof(GUIUtility).GetMethod("CheckOnGUI", BindingFlags.Static | BindingFlags.NonPublic);
			EndAllTextSolution.GUI2.CheckOnGUI = (EndAllTextSolution.VoidCall)Delegate.CreateDelegate(typeof(EndAllTextSolution.VoidCall), method);
			EndAllTextSolution.GUI2.textFieldInputProperty = typeof(GUIUtility).GetProperty("textFieldInput", BindingFlags.Static | BindingFlags.NonPublic);
		}
	}

	private delegate void VoidCall();
}