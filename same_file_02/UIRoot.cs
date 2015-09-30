using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	private static List<UIRoot> mRoots;

	public bool automatic = true;

	public int manualHeight = 800;

	private Transform mTrans;

	static UIRoot()
	{
		UIRoot.mRoots = new List<UIRoot>();
	}

	public UIRoot()
	{
	}

	private void Awake()
	{
		UIRoot.mRoots.Add(this);
	}

	public static void Broadcast(string funcName)
	{
		int num = 0;
		int count = UIRoot.mRoots.Count;
		while (num < count)
		{
			UIRoot item = UIRoot.mRoots[num];
			if (item != null)
			{
				item.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			num++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param != null)
		{
			int num = 0;
			int count = UIRoot.mRoots.Count;
			while (num < count)
			{
				UIRoot item = UIRoot.mRoots[num];
				if (item != null)
				{
					item.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
				}
				num++;
			}
		}
		else
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
	}

	private void OnDestroy()
	{
		UIRoot.mRoots.Remove(this);
	}

	private void Start()
	{
		this.mTrans = base.transform;
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
	}

	private void Update()
	{
		this.manualHeight = Mathf.Max(2, (!this.automatic ? this.manualHeight : Screen.height));
		float single = 2f / (float)this.manualHeight;
		Vector3 vector3 = this.mTrans.localScale;
		if (Mathf.Abs(vector3.x - single) > 1.401298E-45f || Mathf.Abs(vector3.y - single) > 1.401298E-45f || Mathf.Abs(vector3.z - single) > 1.401298E-45f)
		{
			this.mTrans.localScale = new Vector3(single, single, single);
		}
	}
}