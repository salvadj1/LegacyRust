using System;
using UnityEngine;

public class ServerCategory : MonoBehaviour
{
	public dfLabel serverCount;

	public int categoryId;

	public bool activeCategory;

	public ServerCategory()
	{
	}

	public void CategoryChanged(int iCategory)
	{
		if (iCategory != this.categoryId)
		{
			base.GetComponent<dfControl>().Opacity = 0.5f;
		}
		else
		{
			base.GetComponent<dfControl>().Opacity = 1f;
		}
	}

	public void OnSelected()
	{
		UnityEngine.Object.FindObjectOfType<ServerBrowser>().SwitchCategory(this.categoryId);
	}

	public void UpdateServerCount(int iCount)
	{
		if (iCount != 0)
		{
			this.serverCount.Show();
		}
		else
		{
			this.serverCount.Hide();
		}
		this.serverCount.Text = iCount.ToString("#,##0");
	}
}