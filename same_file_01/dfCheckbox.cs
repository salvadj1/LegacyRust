using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Checkbox")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfCheckbox : dfControl
{
	[SerializeField]
	protected bool isChecked;

	[SerializeField]
	protected dfControl checkIcon;

	[SerializeField]
	protected dfLabel label;

	[SerializeField]
	protected dfControl @group;

	private PropertyChangedEventHandler<bool> CheckChanged;

	public override bool CanFocus
	{
		get
		{
			return (!base.IsEnabled ? false : base.IsVisible);
		}
	}

	public dfControl CheckIcon
	{
		get
		{
			return this.checkIcon;
		}
		set
		{
			if (value != this.checkIcon)
			{
				this.checkIcon = value;
				this.Invalidate();
			}
		}
	}

	public dfControl GroupContainer
	{
		get
		{
			return this.@group;
		}
		set
		{
			if (value != this.@group)
			{
				this.@group = value;
				this.Invalidate();
			}
		}
	}

	public bool IsChecked
	{
		get
		{
			return this.isChecked;
		}
		set
		{
			if (value != this.isChecked)
			{
				this.isChecked = value;
				this.OnCheckChanged();
			}
		}
	}

	public dfLabel Label
	{
		get
		{
			return this.label;
		}
		set
		{
			if (value != this.label)
			{
				this.label = value;
				this.Invalidate();
			}
		}
	}

	public string Text
	{
		get
		{
			if (this.label == null)
			{
				return "[LABEL NOT SET]";
			}
			return this.label.Text;
		}
		set
		{
			if (this.label != null)
			{
				this.label.Text = value;
			}
		}
	}

	public dfCheckbox()
	{
	}

	protected internal void OnCheckChanged()
	{
		base.SignalHierarchy("OnCheckChanged", new object[] { this.isChecked });
		if (this.CheckChanged != null)
		{
			this.CheckChanged(this, this.isChecked);
		}
		if (this.checkIcon != null)
		{
			if (this.IsChecked)
			{
				this.checkIcon.BringToFront();
			}
			this.checkIcon.IsVisible = this.IsChecked;
		}
	}

	protected internal override void OnClick(dfMouseEventArgs args)
	{
		if (this.@group != null)
		{
			dfCheckbox[] componentsInChildren = base.transform.parent.GetComponentsInChildren<dfCheckbox>();
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				dfCheckbox _dfCheckbox = componentsInChildren[i];
				if (_dfCheckbox != this && _dfCheckbox.GroupContainer == this.GroupContainer && _dfCheckbox.IsChecked)
				{
					_dfCheckbox.IsChecked = false;
				}
			}
			this.IsChecked = true;
		}
		else
		{
			this.IsChecked = !this.IsChecked;
		}
		args.Use();
		base.OnClick(args);
	}

	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (args.KeyCode != KeyCode.Space)
		{
			base.OnKeyPress(args);
			return;
		}
		Ray ray = new Ray();
		this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, ray, Vector2.zero, 0f));
	}

	public override void Start()
	{
		base.Start();
		if (this.checkIcon != null)
		{
			this.checkIcon.BringToFront();
			this.checkIcon.IsVisible = this.IsChecked;
		}
	}

	public event PropertyChangedEventHandler<bool> CheckChanged
	{
		add
		{
			this.CheckChanged += value;
		}
		remove
		{
			this.CheckChanged -= value;
		}
	}
}