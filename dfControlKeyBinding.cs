using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Data Binding/Key Binding")]
[Serializable]
public class dfControlKeyBinding : MonoBehaviour, IDataBindingComponent
{
	[SerializeField]
	protected dfControl control;

	[SerializeField]
	protected UnityEngine.KeyCode keyCode;

	[SerializeField]
	protected bool shiftPressed;

	[SerializeField]
	protected bool altPressed;

	[SerializeField]
	protected bool controlPressed;

	[SerializeField]
	protected dfComponentMemberInfo target;

	private bool isBound;

	public bool AltPressed
	{
		get
		{
			return this.altPressed;
		}
		set
		{
			this.altPressed = value;
		}
	}

	public dfControl Control
	{
		get
		{
			return this.control;
		}
		set
		{
			if (this.isBound)
			{
				this.Unbind();
			}
			this.control = value;
		}
	}

	public bool ControlPressed
	{
		get
		{
			return this.controlPressed;
		}
		set
		{
			this.controlPressed = value;
		}
	}

	public UnityEngine.KeyCode KeyCode
	{
		get
		{
			return this.keyCode;
		}
		set
		{
			this.keyCode = value;
		}
	}

	public bool ShiftPressed
	{
		get
		{
			return this.shiftPressed;
		}
		set
		{
			this.shiftPressed = value;
		}
	}

	public dfComponentMemberInfo Target
	{
		get
		{
			return this.target;
		}
		set
		{
			if (this.isBound)
			{
				this.Unbind();
			}
			this.target = value;
		}
	}

	public dfControlKeyBinding()
	{
	}

	public void Awake()
	{
	}

	public void Bind()
	{
		if (this.isBound)
		{
			this.Unbind();
		}
		if (this.control != null)
		{
			this.control.KeyDown += new KeyPressHandler(this.eventSource_KeyDown);
		}
		this.isBound = true;
	}

	private void eventSource_KeyDown(dfControl control, dfKeyEventArgs args)
	{
		if (args.KeyCode != this.keyCode)
		{
			return;
		}
		if ((args.Shift != this.shiftPressed || args.Control != this.controlPressed ? true : args.Alt != this.altPressed))
		{
			return;
		}
		MethodInfo method = this.target.GetMethod();
		method.Invoke(this.target.Component, null);
	}

	public void OnEnable()
	{
	}

	public void Start()
	{
		if (this.control != null && this.target.IsValid)
		{
			this.Bind();
		}
	}

	public void Unbind()
	{
		if (this.control != null)
		{
			this.control.KeyDown -= new KeyPressHandler(this.eventSource_KeyDown);
		}
		this.isBound = false;
	}
}