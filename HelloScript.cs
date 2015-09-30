using System;
using UnityEngine;

public class HelloScript : MonoBehaviour
{
	public string helloString;

	public HelloScript()
	{
	}

	private void Start()
	{
		Debug.Log(string.Concat("HELLO!:", this.helloString, "from object: ", base.gameObject.name));
	}
}