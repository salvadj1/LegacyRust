using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class dfMarkupParser
{
	private static Regex TAG_PATTERN;

	private static Regex ATTR_PATTERN;

	private static Regex STYLE_PATTERN;

	private static Dictionary<string, Type> tagTypes;

	private static dfMarkupParser parserInstance;

	private dfRichTextLabel owner;

	static dfMarkupParser()
	{
		dfMarkupParser.TAG_PATTERN = null;
		dfMarkupParser.ATTR_PATTERN = null;
		dfMarkupParser.STYLE_PATTERN = null;
		dfMarkupParser.tagTypes = null;
		dfMarkupParser.parserInstance = new dfMarkupParser();
		RegexOptions regexOption = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant;
		dfMarkupParser.TAG_PATTERN = new Regex("(\\<\\/?)(?<tag>[a-zA-Z0-9$_]+)(\\s(?<attr>.+?))?([\\/]*\\>)", regexOption);
		dfMarkupParser.ATTR_PATTERN = new Regex("(?<key>[a-zA-Z0-9$_]+)=(?<value>(\"((\\\\\")|\\\\\\\\|[^\"\\n])*\")|('((\\\\')|\\\\\\\\|[^'\\n])*')|\\d+|\\w+)", regexOption);
		dfMarkupParser.STYLE_PATTERN = new Regex("(?<key>[a-zA-Z0-9\\-]+)(\\s*\\:\\s*)(?<value>[^;]+)", regexOption);
	}

	public dfMarkupParser()
	{
	}

	public static dfList<dfMarkupElement> Parse(dfRichTextLabel owner, string source)
	{
		dfList<dfMarkupElement> dfMarkupElements;
		try
		{
			dfMarkupParser.parserInstance.owner = owner;
			dfMarkupElements = dfMarkupParser.parserInstance.parseMarkup(source);
		}
		finally
		{
		}
		return dfMarkupElements;
	}

	private dfMarkupElement parseElement(Queue<dfMarkupElement> tokens)
	{
		dfMarkupElement _dfMarkupElement = tokens.Dequeue();
		if (_dfMarkupElement is dfMarkupString)
		{
			return ((dfMarkupString)_dfMarkupElement).SplitWords();
		}
		dfMarkupTag _dfMarkupTag = (dfMarkupTag)_dfMarkupElement;
		if (_dfMarkupTag.IsClosedTag || _dfMarkupTag.IsEndTag)
		{
			return this.refineTag(_dfMarkupTag);
		}
		while (tokens.Count > 0)
		{
			dfMarkupElement _dfMarkupElement1 = this.parseElement(tokens);
			if (_dfMarkupElement1 is dfMarkupTag)
			{
				dfMarkupTag _dfMarkupTag1 = (dfMarkupTag)_dfMarkupElement1;
				if (_dfMarkupTag1.IsEndTag)
				{
					if (_dfMarkupTag1.TagName != _dfMarkupTag.TagName)
					{
						return this.refineTag(_dfMarkupTag);
					}
					break;
				}
			}
			_dfMarkupTag.AddChildNode(_dfMarkupElement1);
		}
		return this.refineTag(_dfMarkupTag);
	}

	private dfList<dfMarkupElement> parseMarkup(string source)
	{
		Queue<dfMarkupElement> dfMarkupElements = new Queue<dfMarkupElement>();
		MatchCollection matchCollections = dfMarkupParser.TAG_PATTERN.Matches(source);
		int index = 0;
		for (int i = 0; i < matchCollections.Count; i++)
		{
			Match item = matchCollections[i];
			if (item.Index > index)
			{
				string str = source.Substring(index, item.Index - index);
				dfMarkupElements.Enqueue(new dfMarkupString(str));
			}
			index = item.Index + item.Length;
			dfMarkupElements.Enqueue(this.parseTag(item));
		}
		if (index < source.Length)
		{
			dfMarkupElements.Enqueue(new dfMarkupString(source.Substring(index)));
		}
		return this.processTokens(dfMarkupElements);
	}

	private void parseStyleAttribute(dfMarkupTag element, string text)
	{
		MatchCollection matchCollections = dfMarkupParser.STYLE_PATTERN.Matches(text);
		for (int i = 0; i < matchCollections.Count; i++)
		{
			Match item = matchCollections[i];
			string lowerInvariant = item.Groups["key"].Value.ToLowerInvariant();
			string value = item.Groups["value"].Value;
			element.Attributes.Add(new dfMarkupAttribute(lowerInvariant, value));
		}
	}

	private dfMarkupElement parseTag(Match tag)
	{
		string lowerInvariant = tag.Groups["tag"].Value.ToLowerInvariant();
		if (tag.Value.StartsWith("</"))
		{
			return new dfMarkupTag(lowerInvariant)
			{
				IsEndTag = true
			};
		}
		dfMarkupTag _dfMarkupTag = new dfMarkupTag(lowerInvariant);
		string value = tag.Groups["attr"].Value;
		MatchCollection matchCollections = dfMarkupParser.ATTR_PATTERN.Matches(value);
		for (int i = 0; i < matchCollections.Count; i++)
		{
			Match item = matchCollections[i];
			string str = item.Groups["key"].Value;
			string str1 = dfMarkupEntity.Replace(item.Groups["value"].Value);
			if (str1.StartsWith("\""))
			{
				str1 = str1.Trim(new char[] { '\"' });
			}
			else if (str1.StartsWith("'"))
			{
				str1 = str1.Trim(new char[] { '\'' });
			}
			if (!string.IsNullOrEmpty(str1))
			{
				if (str != "style")
				{
					_dfMarkupTag.Attributes.Add(new dfMarkupAttribute(str, str1));
				}
				else
				{
					this.parseStyleAttribute(_dfMarkupTag, str1);
				}
			}
		}
		if (tag.Value.EndsWith("/>") || lowerInvariant == "br" || lowerInvariant == "img")
		{
			_dfMarkupTag.IsClosedTag = true;
		}
		return _dfMarkupTag;
	}

	private dfList<dfMarkupElement> processTokens(Queue<dfMarkupElement> tokens)
	{
		dfList<dfMarkupElement> dfMarkupElements = dfList<dfMarkupElement>.Obtain();
		while (tokens.Count > 0)
		{
			dfMarkupElements.Add(this.parseElement(tokens));
		}
		for (int i = 0; i < dfMarkupElements.Count; i++)
		{
			if (dfMarkupElements[i] is dfMarkupTag)
			{
				((dfMarkupTag)dfMarkupElements[i]).Owner = this.owner;
			}
		}
		return dfMarkupElements;
	}

	private dfMarkupTag refineTag(dfMarkupTag original)
	{
		if (original.IsEndTag)
		{
			return original;
		}
		if (dfMarkupParser.tagTypes == null)
		{
			dfMarkupParser.tagTypes = new Dictionary<string, Type>();
			Type[] exportedTypes = Assembly.GetExecutingAssembly().GetExportedTypes();
			for (int i = 0; i < (int)exportedTypes.Length; i++)
			{
				Type type = exportedTypes[i];
				if (typeof(dfMarkupTag).IsAssignableFrom(type))
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(dfMarkupTagInfoAttribute), true);
					if (customAttributes != null && (int)customAttributes.Length != 0)
					{
						for (int j = 0; j < (int)customAttributes.Length; j++)
						{
							string tagName = ((dfMarkupTagInfoAttribute)customAttributes[j]).TagName;
							dfMarkupParser.tagTypes[tagName] = type;
						}
					}
				}
			}
		}
		if (!dfMarkupParser.tagTypes.ContainsKey(original.TagName))
		{
			return original;
		}
		Type item = dfMarkupParser.tagTypes[original.TagName];
		return (dfMarkupTag)Activator.CreateInstance(item, new object[] { original });
	}
}