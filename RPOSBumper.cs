using System;
using System.Collections.Generic;
using UnityEngine;

public class RPOSBumper : MonoBehaviour
{
	public UISlicedSprite background;

	public UIButton buttonPrefab;

	private HashSet<RPOSBumper.Instance> instances;

	public RPOSBumper()
	{
	}

	private void Clear()
	{
		if (this.instances != null)
		{
			foreach (RPOSBumper.Instance instance in this.instances)
			{
				if (!instance.window)
				{
					continue;
				}
				instance.window.RemoveBumper(instance);
			}
			this.instances.Clear();
		}
	}

	private void OnDestroy()
	{
		this.Clear();
	}

	public void Populate()
	{
		this.Clear();
		List<RPOSWindow> rPOSWindows = new List<RPOSWindow>(RPOS.GetBumperWindowList());
		int count = rPOSWindows.Count;
		for (int i = 0; i < count; i++)
		{
			if (!rPOSWindows[i] || string.IsNullOrEmpty(rPOSWindows[i].title))
			{
				int num = i;
				i = num - 1;
				rPOSWindows.RemoveAt(num);
				count--;
			}
			else
			{
				rPOSWindows[i].EnsureAwake<RPOSWindow>();
			}
		}
		Vector3 vector3 = this.buttonPrefab.gameObject.transform.localScale;
		float single = 75f * vector3.x;
		float single1 = 5f;
		float single2 = (float)count * single * -0.5f;
		int num1 = 0;
		if (this.instances == null)
		{
			this.instances = new HashSet<RPOSBumper.Instance>();
		}
		foreach (RPOSWindow rPOSWindow in rPOSWindows)
		{
			RPOSBumper.Instance instance = new RPOSBumper.Instance()
			{
				window = rPOSWindow
			};
			Vector3 vector31 = this.buttonPrefab.gameObject.transform.localScale;
			GameObject gameObject = NGUITools.AddChild(base.gameObject, this.buttonPrefab.gameObject);
			instance.label = gameObject.gameObject.GetComponentInChildren<UILabel>();
			instance.label.name = string.Concat(rPOSWindow.title, "BumperButton");
			Vector3 vector32 = gameObject.transform.localPosition;
			vector32.x = single2 + (single + single1) * (float)num1;
			gameObject.transform.localPosition = vector32;
			gameObject.transform.localScale = vector31;
			instance.button = gameObject.GetComponentInChildren<UIButton>();
			instance.bumper = this;
			rPOSWindow.AddBumper(instance);
			this.instances.Add(instance);
			num1++;
		}
		Vector3 vector33 = this.background.transform.localScale;
		vector33.x = (float)count * single + ((float)count - 1f * single1);
		this.background.gameObject.transform.localScale = vector33;
		Transform transforms = this.background.gameObject.transform;
		Vector3 vector34 = base.transform.localPosition;
		transforms.localPosition = new Vector3(vector33.x * -0.5f, vector34.y, 0f);
	}

	public class Instance
	{
		public RPOSBumper bumper;

		public UIButton button;

		public UILabel label;

		public RPOSWindow window;

		private UIEventListener _listener;

		private bool onceGetListener;

		public UIEventListener listener
		{
			get
			{
				if (!this.onceGetListener)
				{
					this.onceGetListener = true;
					if (this.button)
					{
						this._listener = UIEventListener.Get(this.button.gameObject);
					}
				}
				return this._listener;
			}
		}

		public Instance()
		{
		}
	}
}