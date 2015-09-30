using System;
using UnityEngine;

public interface IInputAdapter
{
	float GetAxis(string axisName);

	bool GetKeyDown(KeyCode key);

	bool GetKeyUp(KeyCode key);

	bool GetMouseButton(int button);

	bool GetMouseButtonDown(int button);

	bool GetMouseButtonUp(int button);

	Vector2 GetMousePosition();
}