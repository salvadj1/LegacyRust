using System;
using UnityEngine;

[Serializable]
public class ControllerClassesConfigurations
{
	[SerializeField]
	public string localPlayer;

	[SerializeField]
	public string remotePlayer;

	[SerializeField]
	public string localAI;

	[SerializeField]
	public string remoteAI;

	[SerializeField]
	public string cl_unassigned;

	[SerializeField]
	public string sv_unassigned;

	internal string unassignedClassName
	{
		get
		{
			string str;
			string clUnassigned = this.cl_unassigned;
			string svUnassigned = this.sv_unassigned;
			if (!string.IsNullOrEmpty(clUnassigned))
			{
				str = clUnassigned;
			}
			else if (!string.IsNullOrEmpty(svUnassigned))
			{
				str = svUnassigned;
			}
			else
			{
				str = null;
			}
			return str;
		}
	}

	public ControllerClassesConfigurations()
	{
	}

	internal string GetClassName(bool player, bool local)
	{
		string str;
		string str1;
		string str2;
		string str3;
		if (player)
		{
			if (local)
			{
				if (!string.IsNullOrEmpty(this.localPlayer))
				{
					str3 = this.localPlayer;
				}
				else
				{
					str3 = null;
				}
				return str3;
			}
			if (!string.IsNullOrEmpty(this.remotePlayer))
			{
				str2 = this.remotePlayer;
			}
			else
			{
				str2 = null;
			}
			return str2;
		}
		if (local)
		{
			if (!string.IsNullOrEmpty(this.localAI))
			{
				str1 = this.localAI;
			}
			else
			{
				str1 = null;
			}
			return str1;
		}
		if (!string.IsNullOrEmpty(this.remoteAI))
		{
			str = this.remoteAI;
		}
		else
		{
			str = null;
		}
		return str;
	}
}