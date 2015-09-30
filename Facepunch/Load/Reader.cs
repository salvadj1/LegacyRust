using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Facepunch.Load
{
	public sealed class Reader : Facepunch.Load.Stream
	{
		private JsonReader json;

		private bool insideOrderList;

		private bool insideRandomList;

		private Facepunch.Load.Token token;

		private bool disposed;

		private readonly bool disposesTextReader;

		private readonly string prefix;

		private Facepunch.Load.Item item;

		public Facepunch.Load.Item Item
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("Reader");
				}
				if (this.token != Facepunch.Load.Token.BundleListing)
				{
					throw new InvalidOperationException("You may only retreive Item when Token is Token.BundleListing!");
				}
				return this.item;
			}
		}

		public Facepunch.Load.Token Token
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("Reader");
				}
				return this.token;
			}
		}

		private Reader(JsonReader json, string bundlePath, bool createdForThisInstance)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			this.json = json;
			this.disposesTextReader = createdForThisInstance;
			this.prefix = bundlePath;
			if (!string.IsNullOrEmpty(this.prefix))
			{
				char chr = this.prefix[this.prefix.Length - 1];
				if (chr != '/' && chr != '\\')
				{
					this.prefix = string.Concat(this.prefix, "/");
				}
			}
			else
			{
				this.prefix = string.Empty;
			}
		}

		private Reader(JsonReader json, string bundlePath) : this(json, bundlePath, false)
		{
		}

		private Reader(TextReader reader, string bundlePath) : this(new JsonReader(reader), bundlePath, false)
		{
		}

		private Reader(string text, string bundlePath) : this(new JsonReader(text), bundlePath, true)
		{
		}

		public static Reader CreateFromFile(string openFilePath, string bundlePath)
		{
			return new Reader(new JsonReader(File.OpenText(openFilePath)), bundlePath, true);
		}

		public static Reader CreateFromFile(string openFilePath)
		{
			return Reader.CreateFromFile(openFilePath, Path.GetDirectoryName(openFilePath));
		}

		public static Reader CreateFromReader(TextReader textReader, string bundlePath)
		{
			return new Reader(textReader, bundlePath);
		}

		public static Reader CreateFromReader(JsonReader textReader, string bundlePath)
		{
			return new Reader(textReader, bundlePath);
		}

		public static Reader CreateFromText(string jsonText, string bundlePath)
		{
			return new Reader(jsonText, bundlePath);
		}

		public override void Dispose()
		{
			if (!this.disposed)
			{
				if (this.disposesTextReader)
				{
					try
					{
						this.json.Dispose();
					}
					catch (ObjectDisposedException objectDisposedException)
					{
					}
				}
				else
				{
					while (this.token != Facepunch.Load.Token.End && this.token != Facepunch.Load.Token.DownloadQueueEnd)
					{
						try
						{
							this.Read();
						}
						catch (JsonException jsonException)
						{
							this.token = Facepunch.Load.Token.End;
						}
					}
				}
				this.json = null;
				this.disposed = true;
			}
		}

		private static Type ParseType(string str)
		{
			Type type = Type.GetType(str, false, true);
			if (type != null)
			{
				return type;
			}
			string str1 = string.Concat("Facepunch.MeshBatch.", str);
			type = Type.GetType(str1, false, true);
			if (type != null)
			{
				return type;
			}
			return Type.GetType(str, true, true);
		}

		private string PathToBundle(string incomingPathFromJson)
		{
			if (incomingPathFromJson.Contains("//") || incomingPathFromJson.Contains(":/") || incomingPathFromJson.Contains(":\\") || Path.IsPathRooted(incomingPathFromJson))
			{
				return incomingPathFromJson;
			}
			return string.Concat(this.prefix, incomingPathFromJson);
		}

		public bool Read()
		{
			JsonToken token;
			if (this.disposed)
			{
				throw new ObjectDisposedException("Reader");
			}
			this.item = new Facepunch.Load.Item();
			if (!this.json.Read())
			{
				this.token = Facepunch.Load.Token.End;
				return false;
			}
			if (!this.insideOrderList)
			{
				token = this.json.Token;
				if (token == JsonToken.None)
				{
					this.token = Facepunch.Load.Token.End;
					return false;
				}
				if (token == JsonToken.ArrayStart)
				{
					this.token = Facepunch.Load.Token.DownloadQueueBegin;
					this.insideOrderList = true;
					return true;
				}
			}
			else if (!this.insideRandomList)
			{
				token = this.json.Token;
				if (token == JsonToken.ObjectStart)
				{
					this.token = Facepunch.Load.Token.RandomLoadOrderAreaBegin;
					this.insideRandomList = true;
					return true;
				}
				if (token == JsonToken.ArrayEnd)
				{
					this.token = Facepunch.Load.Token.DownloadQueueEnd;
					this.insideOrderList = false;
					return true;
				}
			}
			else
			{
				token = this.json.Token;
				if (token == JsonToken.PropertyName)
				{
					this.token = Facepunch.Load.Token.BundleListing;
					this.ReadBundleListing(this.json.Value.AsString);
					return true;
				}
				if (token == JsonToken.ObjectEnd)
				{
					this.token = Facepunch.Load.Token.RandomLoadOrderAreaEnd;
					this.insideRandomList = false;
					return true;
				}
			}
			throw new JsonException("Bad json state");
		}

		private void ReadBundleListing(string nameOfBundle)
		{
			int num;
			if (!this.json.Read())
			{
				throw new JsonException("End of stream unexpected");
			}
			if (this.json.Token != JsonToken.ObjectStart)
			{
				throw new JsonException(string.Concat("Expected object start for bundle name (property) ", nameOfBundle));
			}
			this.item.Name = nameOfBundle;
			this.item.ByteLength = -1;
			while (true)
			{
				if (!this.json.Read())
				{
					throw new JsonException("Unexpected end of stream");
				}
				if (this.json.Token == JsonToken.ObjectEnd)
				{
					if (string.IsNullOrEmpty(this.item.Path))
					{
						throw new JsonException(string.Concat("Path to bundle not defined for bundle listing ", nameOfBundle));
					}
					if (this.item.ByteLength == -1)
					{
						throw new JsonException(string.Concat("There was no size property for bundle listing ", nameOfBundle));
					}
					ContentType contentType = this.item.ContentType;
					if (contentType != ContentType.Assets)
					{
						if (contentType != ContentType.Scenes)
						{
							throw new JsonException(string.Concat(new object[] { "The content ", this.item.ContentType, " was not handled for bundle listing ", nameOfBundle }));
						}
						if (this.item.TypeOfAssets != null)
						{
							throw new JsonException(string.Concat("There should not have been a type property for scene bundle listing ", nameOfBundle));
						}
					}
					else if (this.item.TypeOfAssets == null)
					{
						throw new JsonException(string.Concat("There was no valid type property for asset bundle listing ", nameOfBundle));
					}
					return;
				}
				if (this.json.Token != JsonToken.PropertyName)
				{
					throw new JsonException(string.Concat("Unexpected token in json : JsonToken.", this.json.Token));
				}
				bool flag = false;
				string asString = this.json.Value.AsString;
				if (asString == null)
				{
					break;
				}
				if (Reader.<>f__switch$map4 == null)
				{
					Dictionary<string, int> strs = new Dictionary<string, int>(5)
					{
						{ "type", 0 },
						{ "size", 1 },
						{ "content", 2 },
						{ "filename", 3 },
						{ "url", 4 }
					};
					Reader.<>f__switch$map4 = strs;
				}
				if (!Reader.<>f__switch$map4.TryGetValue(asString, out num))
				{
					break;
				}
				switch (num)
				{
					case 0:
					{
						if (!this.json.Read())
						{
							throw new JsonException("Unexpected end of stream at type");
						}
						switch (this.json.Token)
						{
							case JsonToken.String:
							{
								try
								{
									this.item.TypeOfAssets = Reader.ParseType(this.json.Value.AsString);
								}
								catch (TypeLoadException typeLoadException)
								{
									throw new JsonException(this.json.Value.AsString, typeLoadException);
								}
								break;
							}
							case JsonToken.Boolean:
							{
								throw new JsonException(string.Concat("the type property expects only null or string. got : ", this.json.Token));
							}
							case JsonToken.Null:
							{
								this.item.TypeOfAssets = null;
								break;
							}
							default:
							{
								throw new JsonException(string.Concat("the type property expects only null or string. got : ", this.json.Token));
							}
						}
						break;
					}
					case 1:
					{
						if (!this.json.Read())
						{
							throw new JsonException("Unexpected end of stream at size");
						}
						switch (this.json.Token)
						{
							case JsonToken.Int:
							case JsonToken.Float:
							{
								this.item.ByteLength = this.json.Value.AsInt;
								break;
							}
							case JsonToken.Reserved:
							{
								throw new JsonException(string.Concat("the size property expects a number. got : ", this.json.Token));
							}
							default:
							{
								throw new JsonException(string.Concat("the size property expects a number. got : ", this.json.Token));
							}
						}
						break;
					}
					case 2:
					{
						if (!this.json.Read())
						{
							throw new JsonException("Unexpected end of stream at content");
						}
						switch (this.json.Token)
						{
							case JsonToken.Int:
							{
								this.item.ContentType = (ContentType)((byte)this.json.Value.AsInt);
								break;
							}
							case JsonToken.Reserved:
							case JsonToken.Float:
							{
								throw new JsonException(string.Concat("the content property expects a string or int. got : ", this.json.Token));
							}
							case JsonToken.String:
							{
								try
								{
									this.item.ContentType = (ContentType)((byte)Enum.Parse(typeof(ContentType), this.json.Value.AsString, true));
								}
								catch (ArgumentException argumentException)
								{
									throw new JsonException(this.json.Value.AsString, argumentException);
								}
								catch (OverflowException overflowException)
								{
									throw new JsonException(this.json.Value.AsString, overflowException);
								}
								break;
							}
							default:
							{
								throw new JsonException(string.Concat("the content property expects a string or int. got : ", this.json.Token));
							}
						}
						break;
					}
					case 3:
					{
						if (!this.json.Read())
						{
							throw new JsonException("Unexpected end of stream at filename");
						}
						if (this.json.Token != JsonToken.String)
						{
							throw new JsonException(string.Concat("the filename property expects a string. got : ", this.json.Token));
						}
						if (!flag)
						{
							try
							{
								this.item.Path = this.PathToBundle(this.json.Value.AsString);
							}
							catch (Exception exception)
							{
								throw new JsonException(this.json.Value.AsString, exception);
							}
							break;
						}
						else
						{
							break;
						}
					}
					case 4:
					{
						if (!this.json.Read())
						{
							throw new JsonException("Unexpected end of stream at url");
						}
						if (this.json.Token != JsonToken.String)
						{
							throw new JsonException(string.Concat("the url property expects a string. got : ", this.json.Token));
						}
						try
						{
							this.item.Path = this.json.Value.AsString;
						}
						catch (Exception exception1)
						{
							throw new JsonException(this.json.Value.AsString, exception1);
						}
						flag = true;
						break;
					}
					default:
					{
						throw new JsonException(string.Concat("Unhandled property named ", this.json.Value.AsString));
					}
				}
			}
			throw new JsonException(string.Concat("Unhandled property named ", this.json.Value.AsString));
		}
	}
}