using System;
using UnityEngine;

public class ServerItem : MonoBehaviour
{
	public ServerItem selectedItem;

	public dfButton textLabel;

	public dfLabel textPlayers;

	public dfLabel textPing;

	public dfButton btnFave;

	public ServerBrowser.Server server;

	public ServerItem()
	{
	}

	public void Connect()
	{
		Debug.Log(string.Concat("> net.connect ", this.server.address, ":", this.server.port.ToString()));
		ConsoleSystem.Run(string.Concat("net.connect ", this.server.address, ":", this.server.port.ToString()), false);
	}

	public void Init(ref ServerBrowser.Server s)
	{
		this.server = s;
		this.textLabel.Text = this.server.name;
		this.textPlayers.Text = string.Concat(this.server.currentplayers.ToString(), " / ", this.server.maxplayers.ToString());
		this.textPing.Text = this.server.ping.ToString();
		dfScrollPanel component = base.transform.parent.GetComponent<dfScrollPanel>();
		if (component)
		{
			base.GetComponent<dfControl>().Width = component.Width;
			base.GetComponent<dfControl>().ResetLayout(true, false);
		}
		this.UpdateColours();
	}

	public void OnClickFave()
	{
		this.server.fave = !this.server.fave;
		this.UpdateColours();
		base.SendMessageUpwards("UpdateServerList");
		if (!this.server.fave)
		{
			ConsoleSystem.Run(string.Concat("serverfavourite.remove ", this.server.address, ":", this.server.port.ToString()), false);
		}
		else
		{
			ConsoleSystem.Run(string.Concat("serverfavourite.add ", this.server.address, ":", this.server.port.ToString()), false);
		}
		ConsoleSystem.Run("serverfavourite.save", false);
	}

	public void SelectThis()
	{
		this.selectedItem = this;
	}

	protected void UpdateColours()
	{
		if (!this.server.fave)
		{
			this.btnFave.Opacity = 0.2f;
		}
		else
		{
			this.btnFave.Opacity = 1f;
		}
	}
}