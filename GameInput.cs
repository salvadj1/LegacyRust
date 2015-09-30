using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
	public static GameInput.GameButton[] Buttons;

	public static Vector2 mouseDelta
	{
		get
		{
			Vector2 vector2 = new Vector2();
			vector2.x = GameInput.mouseDeltaX;
			vector2.y = GameInput.mouseDeltaY;
			return vector2;
		}
	}

	public static float mouseDeltaX
	{
		get
		{
			return Input.GetAxis("Mouse X") * GameInput.mouseSensitivity;
		}
	}

	public static float mouseDeltaY
	{
		get
		{
			return Input.GetAxis("Mouse Y") * GameInput.mouseSensitivity;
		}
	}

	public static float mouseSensitivity
	{
		get
		{
			return input.mousespeed;
		}
	}

	static GameInput()
	{
		GameInput.Buttons = new GameInput.GameButton[] { new GameInput.GameButton("Left"), new GameInput.GameButton("Right"), new GameInput.GameButton("Up"), new GameInput.GameButton("Down"), new GameInput.GameButton("Jump"), new GameInput.GameButton("Duck"), new GameInput.GameButton("Sprint"), new GameInput.GameButton("Fire"), new GameInput.GameButton("AltFire"), new GameInput.GameButton("Reload"), new GameInput.GameButton("Use"), new GameInput.GameButton("Inventory"), new GameInput.GameButton("Flashlight"), new GameInput.GameButton("Laser"), new GameInput.GameButton("Voice"), new GameInput.GameButton("Chat") };
	}

	public GameInput()
	{
	}

	public static GameInput.GameButton GetButton(string strName)
	{
		GameInput.GameButton[] buttons = GameInput.Buttons;
		for (int i = 0; i < (int)buttons.Length; i++)
		{
			GameInput.GameButton gameButton = buttons[i];
			if (gameButton.Name == strName)
			{
				return gameButton;
			}
		}
		return null;
	}

	public static string GetConfig()
	{
		string empty = string.Empty;
		GameInput.GameButton[] buttons = GameInput.Buttons;
		for (int i = 0; i < (int)buttons.Length; i++)
		{
			GameInput.GameButton gameButton = buttons[i];
			string str = empty;
			empty = string.Concat(new string[] { str, "input.bind ", gameButton.Name, " ", gameButton.bindingOne.ToString(), " ", gameButton.bindingTwo.ToString(), "\n" });
		}
		return empty;
	}

	public class GameButton
	{
		public readonly string Name;

		public KeyCode bindingOne;

		public KeyCode bindingTwo;

		internal GameButton(string NiceName)
		{
			this.Name = NiceName;
		}

		public void Bind(string A, string B)
		{
			GameInput.GameButton.SetKeyCode(ref this.bindingOne, A);
			GameInput.GameButton.SetKeyCode(ref this.bindingTwo, B);
		}

		public bool IsDown()
		{
			bool flag;
			if (GameInput.GameButton.IsKeyHeld(this.bindingOne))
			{
				flag = true;
			}
			else
			{
				flag = (this.bindingOne == this.bindingTwo ? false : GameInput.GameButton.IsKeyHeld(this.bindingTwo));
			}
			return flag;
		}

		private static bool IsKeyHeld(KeyCode key)
		{
			return (key == KeyCode.None ? false : Input.GetKey(key));
		}

		public bool IsPressed()
		{
			if (GameInput.GameButton.WasKeyPressed(this.bindingOne))
			{
				return (this.bindingTwo == this.bindingOne || GameInput.GameButton.WasKeyPressed(this.bindingTwo) ? true : !GameInput.GameButton.IsKeyHeld(this.bindingTwo));
			}
			if (this.bindingTwo == this.bindingOne || !GameInput.GameButton.WasKeyPressed(this.bindingTwo))
			{
				return false;
			}
			return !GameInput.GameButton.IsKeyHeld(this.bindingOne);
		}

		public bool IsReleased()
		{
			if (GameInput.GameButton.WasKeyReleased(this.bindingOne))
			{
				return (this.bindingTwo == this.bindingOne || GameInput.GameButton.WasKeyReleased(this.bindingTwo) ? true : !GameInput.GameButton.IsKeyHeld(this.bindingTwo));
			}
			if (this.bindingTwo == this.bindingOne || !GameInput.GameButton.WasKeyReleased(this.bindingTwo))
			{
				return false;
			}
			return !GameInput.GameButton.IsKeyHeld(this.bindingOne);
		}

		public static bool operator @false(GameInput.GameButton gameButton)
		{
			return (object.ReferenceEquals(gameButton, null) ? false : !gameButton.IsDown());
		}

		public static bool operator @true(GameInput.GameButton gameButton)
		{
			return (object.ReferenceEquals(gameButton, null) ? false : gameButton.IsDown());
		}

		private static KeyCode? ParseKeyCode(string name)
		{
			KeyCode? nullable;
			try
			{
				nullable = new KeyCode?((KeyCode)((int)Enum.Parse(typeof(KeyCode), name, true)));
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				nullable = null;
			}
			return nullable;
		}

		private static void SetKeyCode(ref KeyCode value, string name)
		{
			KeyCode keyCode;
			KeyCode? nullable = GameInput.GameButton.ParseKeyCode(name);
			if (!nullable.HasValue)
			{
				keyCode = (KeyCode)((int)value);
			}
			else
			{
				keyCode = nullable.Value;
			}
			value = keyCode;
		}

		public override string ToString()
		{
			return this.Name ?? string.Empty;
		}

		private static bool WasKeyPressed(KeyCode key)
		{
			return (key == KeyCode.None ? false : Input.GetKeyDown(key));
		}

		private static bool WasKeyReleased(KeyCode key)
		{
			return (key == KeyCode.None ? false : Input.GetKeyUp(key));
		}
	}
}