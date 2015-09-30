using System;
using System.Collections.Generic;
using System.Text;

public class dfMarkupEntity
{
	private static List<dfMarkupEntity> HTML_ENTITIES;

	private static StringBuilder buffer;

	public string EntityName;

	public string EntityChar;

	static dfMarkupEntity()
	{
		List<dfMarkupEntity> dfMarkupEntities = new List<dfMarkupEntity>()
		{
			new dfMarkupEntity("&nbsp;", " "),
			new dfMarkupEntity("&quot;", "\""),
			new dfMarkupEntity("&amp;", "&"),
			new dfMarkupEntity("&lt;", "<"),
			new dfMarkupEntity("&gt;", ">"),
			new dfMarkupEntity("&#39;", "'"),
			new dfMarkupEntity("&trade;", "™"),
			new dfMarkupEntity("&copy;", "©"),
			new dfMarkupEntity("\u00a0", " ")
		};
		dfMarkupEntity.HTML_ENTITIES = dfMarkupEntities;
		dfMarkupEntity.buffer = new StringBuilder();
	}

	public dfMarkupEntity(string entityName, string entityChar)
	{
		this.EntityName = entityName;
		this.EntityChar = entityChar;
	}

	public static string Replace(string text)
	{
		dfMarkupEntity.buffer.EnsureCapacity(text.Length);
		dfMarkupEntity.buffer.Length = 0;
		dfMarkupEntity.buffer.Append(text);
		for (int i = 0; i < dfMarkupEntity.HTML_ENTITIES.Count; i++)
		{
			dfMarkupEntity item = dfMarkupEntity.HTML_ENTITIES[i];
			dfMarkupEntity.buffer.Replace(item.EntityName, item.EntityChar);
		}
		return dfMarkupEntity.buffer.ToString();
	}
}