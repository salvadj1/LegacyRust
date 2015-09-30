using System;

public struct DeployedObjectInfo
{
	public bool valid;

	public ulong userID;

	public Character playerCharacter
	{
		get
		{
			if (!this.valid)
			{
				return null;
			}
			Controllable controllable = this.playerControllable;
			if (!controllable)
			{
				return null;
			}
			return controllable.idMain;
		}
	}

	public PlayerClient playerClient
	{
		get
		{
			PlayerClient playerClient;
			if (!this.valid)
			{
				return null;
			}
			PlayerClient.FindByUserID(this.userID, out playerClient);
			return playerClient;
		}
	}

	public Controllable playerControllable
	{
		get
		{
			if (!this.valid)
			{
				return null;
			}
			PlayerClient playerClient = this.playerClient;
			if (!playerClient)
			{
				return null;
			}
			return playerClient.controllable;
		}
	}
}