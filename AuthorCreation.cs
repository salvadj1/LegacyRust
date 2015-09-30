using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public abstract class AuthorCreation : AuthorShared
{
	[SerializeField]
	private UnityEngine.Object _output;

	[NonSerialized]
	public readonly Type outputType;

	protected int creationSeperatorHeight = 300;

	protected int sideBarWidth = 200;

	protected int palletLabelHeight = 48;

	protected int palletPanelWidth = 96;

	[SerializeField]
	private List<AuthorPeice> allPeices;

	[NonSerialized]
	private List<AuthorPeice> selected;

	protected readonly static AuthorPalletObject[] NoPalletObjects;

	protected readonly static AuthorPeice[] NoPeices;

	private readonly static AuthorShared.PeiceCommand[] NoCommand;

	protected UnityEngine.Object output
	{
		get
		{
			return this._output;
		}
	}

	public int palletContentHeight
	{
		get
		{
			return this.palletLabelHeight;
		}
	}

	public int palletWidth
	{
		get
		{
			return this.palletPanelWidth;
		}
	}

	public int rightPanelWidth
	{
		get
		{
			return this.sideBarWidth;
		}
	}

	public int settingsHeight
	{
		get
		{
			return this.creationSeperatorHeight;
		}
	}

	static AuthorCreation()
	{
		AuthorCreation.NoPalletObjects = new AuthorPalletObject[0];
		AuthorCreation.NoPeices = new AuthorPeice[0];
		AuthorCreation.NoCommand = new AuthorShared.PeiceCommand[0];
	}

	protected AuthorCreation(Type outputType) : this()
	{
		this.outputType = outputType;
	}

	private AuthorCreation()
	{
	}

	public bool Contains(string peiceID)
	{
		bool flag;
		if (this.allPeices != null)
		{
			List<AuthorPeice>.Enumerator enumerator = this.allPeices.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AuthorPeice current = enumerator.Current;
					if (!current || !(current.peiceID == peiceID))
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return flag;
		}
		return false;
	}

	public bool Contains(AuthorPeice comp)
	{
		bool flag;
		if (this.allPeices != null)
		{
			List<AuthorPeice>.Enumerator enumerator = this.allPeices.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AuthorPeice current = enumerator.Current;
					if (!current || !(current == comp))
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return flag;
		}
		return false;
	}

	public TPeice CreatePeice<TPeice>(string id, params Type[] additionalComponents)
	where TPeice : AuthorPeice
	{
		Type[] typeArray = new Type[(int)additionalComponents.Length + 1];
		Array.Copy(additionalComponents, 0, typeArray, 1, (int)additionalComponents.Length);
		typeArray[0] = typeof(TPeice);
		GameObject gameObject = new GameObject(id, typeArray);
		TPeice component = gameObject.GetComponent<TPeice>();
		if (!component || !this.RegisterPeice(component, id))
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
			component = (TPeice)null;
		}
		return component;
	}

	protected virtual bool DefaultApply()
	{
		return false;
	}

	[DebuggerHidden]
	private IEnumerable<AuthorPeice> DoGUIPeiceInspector(List<AuthorPeice> peices)
	{
		AuthorCreation.<DoGUIPeiceInspector>c__Iterator3 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerable<AuthorShared.PeiceCommand> DoGUIPeiceList(List<AuthorPeice> peices)
	{
		AuthorCreation.<DoGUIPeiceList>c__Iterator4 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public virtual IEnumerable<AuthorPeice> DoSceneView()
	{
		AuthorCreation.<DoSceneView>c__Iterator5 variable = null;
		return variable;
	}

	protected virtual IEnumerable<AuthorPalletObject> EnumeratePalletObjects()
	{
		return AuthorCreation.NoPalletObjects;
	}

	protected IEnumerable<AuthorPeice> EnumeratePeices()
	{
		IEnumerable<AuthorPeice> noPeices;
		if (this.allPeices == null || this.allPeices.Count == 0)
		{
			noPeices = AuthorCreation.NoPeices;
		}
		else
		{
			noPeices = new List<AuthorPeice>(this.allPeices);
		}
		return noPeices;
	}

	internal IEnumerable<AuthorPeice> EnumeratePeices(bool selectedOnly)
	{
		return (!selectedOnly ? this.EnumeratePeices() : this.EnumerateSelectedPeices());
	}

	protected IEnumerable<AuthorPeice> EnumerateSelectedPeices()
	{
		IEnumerable<AuthorPeice> noPeices;
		if (this.selected == null || this.selected.Count == 0)
		{
			noPeices = AuthorCreation.NoPeices;
		}
		else
		{
			noPeices = new List<AuthorPeice>(this.selected);
		}
		return noPeices;
	}

	public virtual void ExecuteCommand(AuthorShared.PeiceCommand cmd)
	{
		UnityEngine.Debug.Log(cmd.action, cmd.peice);
		switch (cmd.action)
		{
			case AuthorShared.PeiceAction.AddToSelection:
			{
				UnityEngine.Object obj = cmd.peice.selectReference;
				UnityEngine.Object[] allSelectedObjects = AuthorShared.GetAllSelectedObjects();
				Array.Resize<UnityEngine.Object>(ref allSelectedObjects, (int)allSelectedObjects.Length + 1);
				allSelectedObjects[(int)allSelectedObjects.Length - 1] = obj;
				AuthorShared.SetAllSelectedObjects(allSelectedObjects);
				break;
			}
			case AuthorShared.PeiceAction.RemoveFromSelection:
			{
				UnityEngine.Object obj1 = cmd.peice.selectReference;
				UnityEngine.Object[] objArray = AuthorShared.GetAllSelectedObjects();
				int num = 0;
				for (int i = 0; i < (int)objArray.Length; i++)
				{
					if (objArray[i] != obj1 && objArray[i] != cmd.peice)
					{
						int num1 = num;
						num = num1 + 1;
						objArray[num1] = objArray[i];
					}
				}
				if (num < (int)objArray.Length)
				{
					Array.Resize<UnityEngine.Object>(ref objArray, num);
					AuthorShared.SetAllSelectedObjects(objArray);
				}
				break;
			}
			case AuthorShared.PeiceAction.SelectSolo:
			{
				AuthorShared.SetAllSelectedObjects(new UnityEngine.Object[] { cmd.peice.selectReference });
				break;
			}
			case AuthorShared.PeiceAction.Delete:
			{
				bool? nullable = AuthorShared.Ask(string.Concat(new object[] { "You want to delete ", cmd.peice.peiceID, "? (", cmd.peice, ")" }));
				if ((!nullable.HasValue ? false : nullable.Value))
				{
					cmd.peice.Delete();
				}
				break;
			}
			case AuthorShared.PeiceAction.Dirty:
			{
				AuthorShared.SetDirty(cmd.peice);
				break;
			}
			case AuthorShared.PeiceAction.Ping:
			{
				AuthorShared.PingObject(cmd.peice);
				break;
			}
		}
	}

	protected Stream GetStream(bool write, string filepath, out AuthorCreationProject proj)
	{
		proj = AuthorCreationProject.current;
		if (!proj)
		{
			throw new InvalidOperationException("Theres no project loaded");
		}
		if (proj.FindAuthorCreationInScene() != this)
		{
			throw new InvalidOperationException("The current project is not for this creation");
		}
		return proj.GetStream(write, filepath);
	}

	public bool GUICreationSettings()
	{
		return this.OnGUICreationSettings();
	}

	public bool GUIPalletObjects(params GUILayoutOption[] options)
	{
		return this.GUIPalletObjects(GUI.skin.button, options);
	}

	public bool GUIPalletObjects(GUIStyle buttonStyle, params GUILayoutOption[] options)
	{
		AuthorPeice authorPeice;
		bool flag = GUI.enabled;
		bool flag1 = false;
		IEnumerator<AuthorPalletObject> enumerator = this.EnumeratePalletObjects().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AuthorPalletObject current = enumerator.Current;
				if (current.guiContent == null)
				{
					current.guiContent = new GUIContent(current.ToString());
				}
				GUI.enabled = (!flag ? false : current.Validate(this));
				if (!GUILayout.Button(current.guiContent, buttonStyle, options) || !current.Create(this, out authorPeice))
				{
					continue;
				}
				if (this.RegisterPeice(authorPeice))
				{
					flag1 = true;
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(authorPeice.gameObject);
				}
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		GUI.enabled = flag;
		return flag1;
	}

	public IEnumerable<AuthorPeice> GUIPeiceInspector()
	{
		if (this.selected == null || this.selected.Count == 0)
		{
			return AuthorCreation.NoPeices;
		}
		return this.DoGUIPeiceInspector(this.selected);
	}

	public IEnumerable<AuthorShared.PeiceCommand> GUIPeiceList()
	{
		if (this.allPeices == null || this.allPeices.Count == 0)
		{
			return AuthorCreation.NoCommand;
		}
		return this.DoGUIPeiceList(this.allPeices);
	}

	protected abstract void LoadSettings(JSONStream stream);

	protected bool LoadSettings()
	{
		AuthorCreationProject authorCreationProject;
		string str;
		int num;
		bool flag;
		Stream stream = this.GetStream(true, "dat.asc", out authorCreationProject);
		if (stream == null)
		{
			return false;
		}
		try
		{
			using (JSONStream jSONStream = JSONStream.CreateWriter(stream))
			{
				while (jSONStream.Read())
				{
					if (jSONStream.token == JSONToken.ObjectStart)
					{
						while (jSONStream.ReadNextProperty(out str))
						{
							string str1 = str;
							if (str1 == null)
							{
								continue;
							}
							if (AuthorCreation.<>f__switch$map0 == null)
							{
								Dictionary<string, int> strs = new Dictionary<string, int>(2)
								{
									{ "project", 0 },
									{ "settings", 1 }
								};
								AuthorCreation.<>f__switch$map0 = strs;
							}
							if (!AuthorCreation.<>f__switch$map0.TryGetValue(str1, out num))
							{
								continue;
							}
							if (num == 0)
							{
								jSONStream.ReadSkip();
							}
							else if (num == 1)
							{
								this.LoadSettings(jSONStream);
							}
						}
					}
				}
			}
			flag = true;
		}
		finally
		{
			stream.Dispose();
		}
		return flag;
	}

	protected virtual bool OnGUICreationSettings()
	{
		return false;
	}

	protected virtual void OnSelectionChange()
	{
	}

	protected virtual void OnUnregisteredPeice(AuthorPeice peice)
	{
	}

	protected virtual void OnWillUnregisterPeice(AuthorPeice peice)
	{
	}

	protected virtual bool RegisterPeice(AuthorPeice peice)
	{
		if (this.allPeices != null)
		{
			if (this.allPeices.Contains(peice))
			{
				return false;
			}
			this.allPeices.Add(peice);
		}
		else
		{
			this.allPeices = new List<AuthorPeice>()
			{
				peice
			};
		}
		peice.Registered(this);
		return true;
	}

	private bool RegisterPeice(AuthorPeice peice, string id)
	{
		peice.peiceID = id;
		return this.RegisterPeice(peice);
	}

	public virtual string RootBonePath(AuthorPeice callingPeice, Transform bone)
	{
		return AuthorShared.CalculatePath(bone, bone.root);
	}

	protected abstract void SaveSettings(JSONStream stream);

	protected bool SaveSettings()
	{
		AuthorCreationProject authorCreationProject;
		bool flag;
		Stream stream = this.GetStream(true, "dat.asc", out authorCreationProject);
		if (stream == null)
		{
			return false;
		}
		try
		{
			using (JSONStream jSONStream = JSONStream.CreateWriter(stream))
			{
				jSONStream.WriteObjectStart();
				jSONStream.WriteObjectStart("project");
				jSONStream.WriteText("guid", AuthorShared.PathToGUID(AuthorShared.GetAssetPath(authorCreationProject)));
				jSONStream.WriteText("name", authorCreationProject.project);
				jSONStream.WriteText("author", authorCreationProject.authorName);
				jSONStream.WriteText("scene", authorCreationProject.scene);
				jSONStream.WriteText("folder", authorCreationProject.folder);
				jSONStream.WriteObjectEnd();
				jSONStream.WriteProperty("settings");
				this.SaveSettings(jSONStream);
				jSONStream.WriteObjectEnd();
			}
			flag = true;
		}
		finally
		{
			stream.Dispose();
		}
		return flag;
	}

	public bool SetSelection(UnityEngine.Object[] objects)
	{
		List<AuthorPeice> authorPeices = null;
		UnityEngine.Object[] objArray = objects;
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			UnityEngine.Object obj = objArray[i];
			if (obj is AuthorPeice && obj)
			{
				if (authorPeices == null)
				{
					authorPeices = new List<AuthorPeice>()
					{
						(AuthorPeice)obj
					};
				}
				else if (!authorPeices.Contains((AuthorPeice)obj))
				{
					authorPeices.Add((AuthorPeice)obj);
				}
			}
		}
		bool count = false;
		try
		{
			if (authorPeices != null)
			{
				if (this.allPeices != null)
				{
					authorPeices.Sort((AuthorPeice x, AuthorPeice y) => this.allPeices.IndexOf(x).CompareTo(this.allPeices.IndexOf(y)));
				}
				if (this.selected == null || this.selected.Count != authorPeices.Count)
				{
					count = true;
				}
				else
				{
					List<AuthorPeice>.Enumerator enumerator = this.selected.GetEnumerator();
					try
					{
						List<AuthorPeice>.Enumerator enumerator1 = authorPeices.GetEnumerator();
						try
						{
							while (enumerator.MoveNext() && enumerator1.MoveNext())
							{
								if (enumerator.Current == enumerator1.Current)
								{
									continue;
								}
								count = true;
								break;
							}
						}
						finally
						{
							((IDisposable)(object)enumerator1).Dispose();
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
				}
			}
			else if (this.selected != null)
			{
				count = this.selected.Count > 0;
				this.selected.Clear();
			}
		}
		finally
		{
			if (count)
			{
				if (this.selected != null)
				{
					this.selected.Clear();
					if (authorPeices != null)
					{
						this.selected.AddRange(authorPeices);
					}
				}
				else if (authorPeices != null)
				{
					this.selected = authorPeices;
				}
				this.OnSelectionChange();
			}
		}
		return count;
	}

	internal void UnregisterPeice(AuthorPeice peice)
	{
		if (this.allPeices != null && this.allPeices.IndexOf(peice) != -1)
		{
			this.OnWillUnregisterPeice(peice);
			this.allPeices.Remove(peice);
			if (this.selected != null)
			{
				this.selected.Remove(peice);
			}
			this.OnUnregisteredPeice(peice);
			if (!Application.isPlaying)
			{
				AuthorShared.SetDirty(this);
			}
		}
	}
}