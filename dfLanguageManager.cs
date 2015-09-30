using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class dfLanguageManager : MonoBehaviour
{
	[SerializeField]
	private dfLanguageCode currentLanguage;

	[SerializeField]
	private TextAsset dataFile;

	private Dictionary<string, string> strings = new Dictionary<string, string>();

	public dfLanguageCode CurrentLanguage
	{
		get
		{
			return this.currentLanguage;
		}
	}

	public TextAsset DataFile
	{
		get
		{
			return this.dataFile;
		}
		set
		{
			if (value != this.dataFile)
			{
				this.dataFile = value;
				this.LoadLanguage(this.currentLanguage);
			}
		}
	}

	public dfLanguageManager()
	{
	}

	public string GetValue(string key)
	{
		string empty = string.Empty;
		if (this.strings.TryGetValue(key, out empty))
		{
			return empty;
		}
		return key;
	}

	public void LoadLanguage(dfLanguageCode language)
	{
		this.currentLanguage = language;
		this.strings.Clear();
		if (this.dataFile != null)
		{
			this.parseDataFile();
		}
		dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Localize();
		}
	}

	private void parseDataFile()
	{
		string str;
		string str1 = this.dataFile.text.Replace("\r\n", "\n").Trim();
		List<string> strs = new List<string>();
		int num = this.parseLine(str1, strs, 0);
		int num1 = strs.IndexOf(this.currentLanguage.ToString());
		if (num1 < 0)
		{
			return;
		}
		List<string> strs1 = new List<string>();
		while (num < str1.Length)
		{
			num = this.parseLine(str1, strs1, num);
			if (strs1.Count != 0)
			{
				string item = strs1[0];
				str = (num1 >= strs1.Count ? string.Empty : strs1[num1]);
				this.strings[item] = str;
			}
		}
	}

	private int parseLine(string data, List<string> values, int index)
	{
		values.Clear();
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(256);
		while (index < data.Length)
		{
			char chr = data[index];
			if (chr == '\"')
			{
				if (!flag)
				{
					flag = true;
				}
				else if (index + 1 >= data.Length || data[index + 1] != chr)
				{
					flag = false;
				}
				else
				{
					index++;
					stringBuilder.Append(chr);
				}
			}
			else if (chr == ',')
			{
				if (!flag)
				{
					values.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder.Append(chr);
				}
			}
			else if (chr != '\n')
			{
				stringBuilder.Append(chr);
			}
			else if (!flag)
			{
				index++;
				break;
			}
			else
			{
				stringBuilder.Append(chr);
			}
			index++;
		}
		if (stringBuilder.Length > 0)
		{
			values.Add(stringBuilder.ToString());
		}
		return index;
	}

	public void Start()
	{
		dfLanguageCode languageCode = this.currentLanguage;
		if (this.currentLanguage == dfLanguageCode.None)
		{
			languageCode = this.SystemLanguageToLanguageCode(Application.systemLanguage);
		}
		this.LoadLanguage(languageCode);
	}

	private dfLanguageCode SystemLanguageToLanguageCode(SystemLanguage language)
	{
		switch (language)
		{
			case SystemLanguage.Afrikaans:
			{
				return dfLanguageCode.AF;
			}
			case SystemLanguage.Arabic:
			{
				return dfLanguageCode.AR;
			}
			case SystemLanguage.Basque:
			{
				return dfLanguageCode.EU;
			}
			case SystemLanguage.Belarusian:
			{
				return dfLanguageCode.BE;
			}
			case SystemLanguage.Bulgarian:
			{
				return dfLanguageCode.BG;
			}
			case SystemLanguage.Catalan:
			{
				return dfLanguageCode.CA;
			}
			case SystemLanguage.Chinese:
			{
				return dfLanguageCode.ZH;
			}
			case SystemLanguage.Czech:
			{
				return dfLanguageCode.CS;
			}
			case SystemLanguage.Danish:
			{
				return dfLanguageCode.DA;
			}
			case SystemLanguage.Dutch:
			{
				return dfLanguageCode.NL;
			}
			case SystemLanguage.English:
			{
				return dfLanguageCode.EN;
			}
			case SystemLanguage.Estonian:
			{
				return dfLanguageCode.ES;
			}
			case SystemLanguage.Faroese:
			{
				return dfLanguageCode.FO;
			}
			case SystemLanguage.Finnish:
			{
				return dfLanguageCode.FI;
			}
			case SystemLanguage.French:
			{
				return dfLanguageCode.FR;
			}
			case SystemLanguage.German:
			{
				return dfLanguageCode.DE;
			}
			case SystemLanguage.Greek:
			{
				return dfLanguageCode.EL;
			}
			case SystemLanguage.Hebrew:
			{
				return dfLanguageCode.HE;
			}
			case SystemLanguage.Hugarian:
			{
				return dfLanguageCode.HU;
			}
			case SystemLanguage.Icelandic:
			{
				return dfLanguageCode.IS;
			}
			case SystemLanguage.Indonesian:
			{
				return dfLanguageCode.ID;
			}
			case SystemLanguage.Italian:
			{
				return dfLanguageCode.IT;
			}
			case SystemLanguage.Japanese:
			{
				return dfLanguageCode.JA;
			}
			case SystemLanguage.Korean:
			{
				return dfLanguageCode.KO;
			}
			case SystemLanguage.Latvian:
			{
				return dfLanguageCode.LV;
			}
			case SystemLanguage.Lithuanian:
			{
				return dfLanguageCode.LT;
			}
			case SystemLanguage.Norwegian:
			{
				return dfLanguageCode.NO;
			}
			case SystemLanguage.Polish:
			{
				return dfLanguageCode.PL;
			}
			case SystemLanguage.Portuguese:
			{
				return dfLanguageCode.PT;
			}
			case SystemLanguage.Romanian:
			{
				return dfLanguageCode.RO;
			}
			case SystemLanguage.Russian:
			{
				return dfLanguageCode.RU;
			}
			case SystemLanguage.SerboCroatian:
			{
				return dfLanguageCode.SH;
			}
			case SystemLanguage.Slovak:
			{
				return dfLanguageCode.SK;
			}
			case SystemLanguage.Slovenian:
			{
				return dfLanguageCode.SL;
			}
			case SystemLanguage.Spanish:
			{
				return dfLanguageCode.ES;
			}
			case SystemLanguage.Swedish:
			{
				return dfLanguageCode.SV;
			}
			case SystemLanguage.Thai:
			{
				return dfLanguageCode.TH;
			}
			case SystemLanguage.Turkish:
			{
				return dfLanguageCode.TR;
			}
			case SystemLanguage.Ukrainian:
			{
				return dfLanguageCode.UK;
			}
			case SystemLanguage.Vietnamese:
			{
				return dfLanguageCode.VI;
			}
			case SystemLanguage.Unknown:
			{
				return dfLanguageCode.EN;
			}
		}
		throw new ArgumentException(string.Concat("Unknown system language: ", language));
	}
}