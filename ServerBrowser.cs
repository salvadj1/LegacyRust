using POSIX;
using Rust.Steam;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServerBrowser : MonoBehaviour
{
	public const int ServerItemHeight = 34;

	public const int SERVERTYPE_OFFICIAL = 0;

	public const int SERVERTYPE_COMMUNITY = 1;

	public const int SERVERTYPE_MODDED = 2;

	public const int SERVERTYPE_WHITELIST = 3;

	public const int SERVERTYPE_HISTORY = 4;

	public const int SERVERTYPE_FRIENDS = 5;

	public GameObject serverItem;

	public ServerCategory[] categoryButtons;

	public dfPanel serverContainer;

	public Pagination pagination;

	public dfControl refreshButton;

	public dfRichTextLabel detailsLabel;

	public string currentServerChecksum;

	[NonSerialized]
	public List<ServerBrowser.Server>[] servers = new List<ServerBrowser.Server>[6];

	[NonSerialized]
	public Queue<GameObject> pooledServerItems = new Queue<GameObject>();

	[NonSerialized]
	public int serverType;

	private ServerBrowser.funcServerAdd AddServerCallback;

	private GCHandle AddServerGC;

	private ServerBrowser.funcServerFinish FinServerCallback;

	private GCHandle RefreshFinishedGC;

	private IntPtr serverRefresh;

	private bool firstOpened;

	private bool needsServerListUpdate;

	private int playerCount;

	private int serverCount;

	private int slotCount;

	private int orderType = 2;

	private int pageNumber;

	public ServerBrowser()
	{
	}

	private void Add_Server(int iMaxPlayers, int iCurrentPlayers, int iPing, uint iLastPlayed, [In] string strHostname, [In] string strAddress, int iPort, int iQueryPort, [In] string tags, bool bPassworded, int iType)
	{
		ulong num;
		string str = string.Concat(strAddress, ":", iPort.ToString());
		ServerBrowser.Server server = new ServerBrowser.Server()
		{
			name = strHostname,
			address = strAddress,
			maxplayers = iMaxPlayers,
			currentplayers = iCurrentPlayers,
			ping = iPing,
			lastplayed = iLastPlayed,
			port = iPort,
			queryport = iQueryPort,
			fave = FavouriteList.Contains(str)
		};
		if (server.name.Length > 64)
		{
			server.name = server.name.Substring(0, 64);
		}
		if (this.ShouldIgnoreServer(server))
		{
			return;
		}
		ServerBrowser serverBrowser = this;
		serverBrowser.playerCount = serverBrowser.playerCount + iCurrentPlayers;
		ServerBrowser serverBrowser1 = this;
		serverBrowser1.serverCount = serverBrowser1.serverCount + 1;
		ServerBrowser serverBrowser2 = this;
		serverBrowser2.slotCount = serverBrowser2.slotCount + iMaxPlayers;
		this.needsServerListUpdate = true;
		int num1 = (int)((float)this.playerCount / (float)this.slotCount * 100f);
		this.detailsLabel.Text = string.Concat(new string[] { "Found ", this.playerCount.ToString(), " players on ", this.serverCount.ToString(), " servers. We are at ", num1.ToString(), "% capacity." });
		if (iType == 3)
		{
			this.servers[5].Add(server);
			this.categoryButtons[5].UpdateServerCount(this.servers[5].Count);
			return;
		}
		if (iType == 4)
		{
			int num2 = (int)POSIX.Time.ElapsedSecondsSince((int)server.lastplayed);
			string empty = string.Empty;
			if (num2 < 60)
			{
				empty = string.Concat(num2.ToString(), " seconds ago");
			}
			else if (num2 < 3600)
			{
				int num3 = num2 / 60;
				empty = string.Concat(num3.ToString(), " minutes ago");
			}
			else if (num2 >= 172800)
			{
				int num4 = num2 / 60 / 60 / 24;
				empty = string.Concat(num4.ToString(), " days ago");
			}
			else
			{
				int num5 = num2 / 60 / 60;
				empty = string.Concat(num5.ToString(), " hours ago");
			}
			ServerBrowser.Server server1 = server;
			server1.name = string.Concat(server1.name, " (", empty, ")");
			this.servers[4].Add(server);
			this.categoryButtons[4].UpdateServerCount(this.servers[4].Count);
			return;
		}
		if (tags.Contains("official"))
		{
			this.servers[0].Add(server);
			this.categoryButtons[0].UpdateServerCount(this.servers[0].Count);
			return;
		}
		string[] strArrays = tags.Split(new char[] { ',' });
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str1 = strArrays[i];
			if (!str1.StartsWith("mp"))
			{
				if (!str1.StartsWith("cp"))
				{
					if (str1.StartsWith("sg:") && ulong.TryParse(str1.Substring(3), NumberStyles.HexNumber, null, out num))
					{
						if (!SteamGroups.MemberOf(num))
						{
							return;
						}
						this.servers[3].Add(server);
						this.categoryButtons[3].UpdateServerCount(this.servers[3].Count);
						return;
					}
				}
			}
		}
		if (tags.Contains("modded"))
		{
			this.servers[2].Add(server);
			this.categoryButtons[2].UpdateServerCount(this.servers[2].Count);
			return;
		}
		if (strHostname.Contains("oxide", true))
		{
			return;
		}
		if (strHostname.Contains("rust++", true))
		{
			return;
		}
		this.servers[1].Add(server);
		this.categoryButtons[1].UpdateServerCount(this.servers[1].Count);
	}

	public void ChangeOrder(int iType)
	{
		this.orderType = iType;
		this.UpdateServerList();
	}

	public void ClearList()
	{
		this.pageNumber = 0;
		List<ServerBrowser.Server>[] listArrays = this.servers;
		for (int i = 0; i < (int)listArrays.Length; i++)
		{
			listArrays[i].Clear();
		}
		ServerCategory[] serverCategoryArray = this.categoryButtons;
		for (int j = 0; j < (int)serverCategoryArray.Length; j++)
		{
			ServerCategory serverCategory = serverCategoryArray[j];
			if (serverCategory)
			{
				serverCategory.UpdateServerCount(0);
			}
		}
		this.ClearServers();
		this.playerCount = 0;
		this.serverCount = 0;
		this.slotCount = 0;
		this.detailsLabel.Text = "...";
	}

	public void ClearServers()
	{
		ServerItem[] componentsInChildren = this.serverContainer.GetComponentsInChildren<ServerItem>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			ServerItem serverItem = componentsInChildren[i];
			serverItem.gameObject.GetComponent<dfControl>().Hide();
			this.pooledServerItems.Enqueue(serverItem.gameObject);
		}
	}

	private int GetMaxServers()
	{
		return (int)this.serverContainer.Height / 34;
	}

	private GameObject NewServerItem()
	{
		if (this.pooledServerItems.Count > 0)
		{
			return this.pooledServerItems.Dequeue();
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.serverItem);
		dfControl component = gameObject.GetComponent<dfControl>();
		this.serverContainer.AddControl(component);
		return gameObject;
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.ServerListUpdater());
	}

	public void OnFirstOpen()
	{
		if (this.firstOpened)
		{
			return;
		}
		if (!base.GetComponent<dfPanel>().IsVisible)
		{
			return;
		}
		this.firstOpened = true;
		this.RefreshServerList();
	}

	public void OnPageSwitched(int iNewPage)
	{
		this.pageNumber = iNewPage;
		this.UpdateServerList();
	}

	public void OrderByName()
	{
		this.ChangeOrder(0);
	}

	public void OrderByPing()
	{
		this.ChangeOrder(2);
	}

	public void OrderByPlayers()
	{
		this.ChangeOrder(1);
	}

	private void RefreshFinished()
	{
		this.refreshButton.IsEnabled = true;
		this.refreshButton.Opacity = 1f;
	}

	public void RefreshServerList()
	{
		this.refreshButton.IsEnabled = false;
		this.refreshButton.Opacity = 0.2f;
		SteamClient.Needed();
		this.ClearList();
		this.detailsLabel.Text = "Updating..";
		this.serverRefresh = ServerBrowser.SteamServers_Fetch(1069, this.AddServerCallback, this.FinServerCallback);
		if (this.serverRefresh == IntPtr.Zero)
		{
			UnityEngine.Debug.Log("Error! Couldn't refresh servers!!");
		}
	}

	[DebuggerHidden]
	private IEnumerator ServerListUpdater()
	{
		ServerBrowser.<ServerListUpdater>c__Iterator33 variable = null;
		return variable;
	}

	private bool ShouldIgnoreServer(ServerBrowser.Server item)
	{
		string lower = item.name.ToLower();
		if (lower.Contains("[color"))
		{
			return true;
		}
		if (lower.Contains("[sprite"))
		{
			return true;
		}
		if (lower.Contains("--"))
		{
			return true;
		}
		if (lower.Contains("%%"))
		{
			return true;
		}
		if (!char.IsLetterOrDigit(lower[0]))
		{
			return true;
		}
		if (!char.IsLetterOrDigit(lower[lower.Length - 1]))
		{
			return true;
		}
		string str = lower;
		for (int i = 0; i < str.Length; i++)
		{
			char chr = str[i];
			if (!char.IsLetterOrDigit(chr))
			{
				if (chr != '\'')
				{
					if (chr != '[')
					{
						if (chr != ']')
						{
							if (chr != '|')
							{
								if (chr != ' ')
								{
									if (chr != '-')
									{
										if (chr != '(')
										{
											if (chr != '%')
											{
												if (chr != ')')
												{
													if (chr != '\u005F')
													{
														if (chr != '@')
														{
															if (chr != '+')
															{
																if (chr != '&')
																{
																	if (chr != ':')
																	{
																		if (chr != '/')
																		{
																			if (chr != '.')
																			{
																				if (chr != '?')
																				{
																					if (chr != '#')
																					{
																						if (chr != '!')
																						{
																							if (chr != ',')
																							{
																								return true;
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (item.currentplayers > item.maxplayers)
		{
			return true;
		}
		if (item.currentplayers > 500)
		{
			return true;
		}
		return false;
	}

	private void Start()
	{
		for (int i = 0; i < (int)this.servers.Length; i++)
		{
			this.servers[i] = new List<ServerBrowser.Server>();
		}
		this.AddServerCallback = new ServerBrowser.funcServerAdd(this.Add_Server);
		this.AddServerGC = GCHandle.Alloc(this.AddServerCallback);
		this.FinServerCallback = new ServerBrowser.funcServerFinish(this.RefreshFinished);
		this.RefreshFinishedGC = GCHandle.Alloc(this.FinServerCallback);
		base.BroadcastMessage("CategoryChanged", this.serverType);
		this.pagination.OnPageSwitch += new Pagination.SwitchToPage(this.OnPageSwitched);
		for (int j = 0; j < 50; j++)
		{
			this.NewServerItem();
		}
		this.ClearServers();
	}

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void SteamServers_Destroy(IntPtr ptr);

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern IntPtr SteamServers_Fetch(int serverVersion, ServerBrowser.funcServerAdd fnc, ServerBrowser.funcServerFinish fnsh);

	public void SwitchCategory(int catID)
	{
		if (this.serverType == catID)
		{
			return;
		}
		this.pageNumber = 0;
		this.currentServerChecksum = string.Empty;
		this.serverType = catID;
		base.BroadcastMessage("CategoryChanged", this.serverType);
		this.ClearServers();
		this.UpdateServerList();
	}

	public void UpdateServerList()
	{
		this.needsServerListUpdate = false;
		int maxServers = this.GetMaxServers();
		maxServers = Math.Min(this.servers[this.serverType].Count, maxServers);
		int num = this.pageNumber * maxServers;
		if (this.servers[this.serverType].Count == 0)
		{
			return;
		}
		if (num < 0)
		{
			return;
		}
		if (num > this.servers[this.serverType].Count)
		{
			return;
		}
		int num1 = (int)Mathf.Ceil((float)this.servers[this.serverType].Count / (float)maxServers);
		if (this.serverType != 4)
		{
			if (this.orderType == 0)
			{
				this.servers[this.serverType].Sort((ServerBrowser.Server x, ServerBrowser.Server y) => (x.fave == y.fave ? string.Compare(x.name, y.name) : y.fave.CompareTo(x.fave)));
			}
			if (this.orderType == 1)
			{
				this.servers[this.serverType].Sort((ServerBrowser.Server x, ServerBrowser.Server y) => (x.fave == y.fave ? (x.currentplayers == y.currentplayers ? string.Compare(x.name, y.name) : y.currentplayers.CompareTo(x.currentplayers)) : y.fave.CompareTo(x.fave)));
			}
			if (this.orderType == 2)
			{
				this.servers[this.serverType].Sort((ServerBrowser.Server x, ServerBrowser.Server y) => (x.fave == y.fave ? (x.ping == y.ping ? string.Compare(x.name, y.name) : x.ping.CompareTo(y.ping)) : y.fave.CompareTo(x.fave)));
			}
		}
		else
		{
			this.servers[this.serverType].Sort((ServerBrowser.Server x, ServerBrowser.Server y) => (x.lastplayed == y.lastplayed ? string.Compare(x.name, y.name) : y.lastplayed.CompareTo(x.lastplayed)));
		}
		if (num + maxServers > this.servers[this.serverType].Count)
		{
			maxServers = this.servers[this.serverType].Count - num;
		}
		List<ServerBrowser.Server> range = this.servers[this.serverType].GetRange(num, maxServers);
		this.pagination.Setup(num1, this.pageNumber);
		string empty = string.Empty;
		foreach (ServerBrowser.Server server in range)
		{
			empty = string.Concat(empty, server.address);
		}
		if (empty == this.currentServerChecksum)
		{
			return;
		}
		this.ClearServers();
		Vector3 vector3 = new Vector3(0f, 0f, 0f);
		this.currentServerChecksum = empty;
		bool flag = false;
		foreach (ServerBrowser.Server server1 in range)
		{
			ServerBrowser.Server server2 = server1;
			if (flag && !server1.fave)
			{
				vector3.y = vector3.y - 2f;
			}
			flag = server1.fave;
			GameObject gameObject = this.NewServerItem();
			gameObject.GetComponent<ServerItem>().Init(ref server2);
			dfControl component = gameObject.GetComponent<dfControl>();
			component.Width = this.serverContainer.Width;
			component.Position = vector3;
			component.Show();
			vector3.y = vector3.y - 34f;
		}
		this.serverContainer.Invalidate();
	}

	public delegate void funcServerAdd(int iMaxPlayers, int iCurrentPlayers, int iPing, uint iLastPlayed, [In] string strHostname, [In] string strAddress, int iPort, int iQueryPort, [In] string tags, bool bPassworded, int iType);

	public delegate void funcServerFinish();

	public class Server
	{
		public bool passworded;

		public string name;

		public string address;

		public int maxplayers;

		public int currentplayers;

		public int ping;

		public uint lastplayed;

		public int port;

		public int queryport;

		public bool fave;

		public Server()
		{
		}
	}
}