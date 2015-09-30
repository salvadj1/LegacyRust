using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	private static Localization mInst;

	public string startingLanguage;

	public TextAsset[] languages;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private string mLanguage;

	public string currentLanguage
	{
		get
		{
			if (string.IsNullOrEmpty(this.mLanguage))
			{
				this.currentLanguage = PlayerPrefs.GetString("Language");
				if (string.IsNullOrEmpty(this.mLanguage))
				{
					this.currentLanguage = this.startingLanguage;
					if (string.IsNullOrEmpty(this.mLanguage) && this.languages != null && (int)this.languages.Length > 0)
					{
						this.currentLanguage = this.languages[0].name;
					}
				}
			}
			return this.mLanguage;
		}
		set
		{
			if (this.mLanguage != value)
			{
				this.startingLanguage = value;
				if (!string.IsNullOrEmpty(value))
				{
					if (this.languages != null)
					{
						int num = 0;
						int length = (int)this.languages.Length;
						while (num < length)
						{
							TextAsset textAsset = this.languages[num];
							if (textAsset != null && textAsset.name == value)
							{
								this.Load(textAsset);
								return;
							}
							num++;
						}
					}
					TextAsset textAsset1 = UnityEngine.Resources.Load(value, typeof(TextAsset)) as TextAsset;
					if (textAsset1 != null)
					{
						this.Load(textAsset1);
						return;
					}
				}
				this.mDictionary.Clear();
				PlayerPrefs.DeleteKey("Language");
			}
		}
	}

	public static Localization instance
	{
		get
		{
			if (Localization.mInst == null)
			{
				Localization.mInst = UnityEngine.Object.FindObjectOfType(typeof(Localization)) as Localization;
				if (Localization.mInst == null)
				{
					GameObject gameObject = new GameObject("_Localization");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					Localization.mInst = gameObject.AddComponent<Localization>();
				}
			}
			return Localization.mInst;
		}
	}

	public Localization()
	{
	}

	private void Awake()
	{
		if (Localization.mInst != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Localization.mInst = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public string Get(string key)
	{
		string str;
		return (!this.mDictionary.TryGetValue(key, out str) ? key : str);
	}

	private void Load(TextAsset asset)
	{
		this.mLanguage = asset.name;
		PlayerPrefs.SetString("Language", this.mLanguage);
		this.mDictionary = (new ByteReader(asset)).ReadDictionary();
		UIRoot.Broadcast("OnLocalize", this);
	}

	private void OnDestroy()
	{
		if (Localization.mInst == this)
		{
			Localization.mInst = null;
		}
	}

	private void OnEnable()
	{
		if (Localization.mInst == null)
		{
			Localization.mInst = this;
		}
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(this.startingLanguage))
		{
			this.currentLanguage = this.startingLanguage;
		}
	}
}