using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Chartboost
{
	public static class CBJSON
	{
		private sealed class Parser : IDisposable
		{
			private enum TOKEN
			{
				NONE,
				CURLY_OPEN,
				CURLY_CLOSE,
				SQUARED_OPEN,
				SQUARED_CLOSE,
				COLON,
				COMMA,
				STRING,
				NUMBER,
				TRUE,
				FALSE,
				NULL
			}

			private const string WORD_BREAK = "{}[],:\"";

			private StringReader json;

			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!CBJSON.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			private CBJSON.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return CBJSON.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					switch (peekChar)
					{
					case '"':
						return CBJSON.Parser.TOKEN.STRING;
					case '#':
					case '$':
					case '%':
					case '&':
					case '\'':
					case '(':
					case ')':
					case '*':
					case '+':
					case '.':
					case '/':
						IL_8D:
						switch (peekChar)
						{
						case '[':
							return CBJSON.Parser.TOKEN.SQUARED_OPEN;
						case '\\':
						{
							IL_A2:
							switch (peekChar)
							{
							case '{':
								return CBJSON.Parser.TOKEN.CURLY_OPEN;
							case '}':
								this.json.Read();
								return CBJSON.Parser.TOKEN.CURLY_CLOSE;
							}
							string nextWord = this.NextWord;
							switch (nextWord)
							{
							case "false":
								return CBJSON.Parser.TOKEN.FALSE;
							case "true":
								return CBJSON.Parser.TOKEN.TRUE;
							case "null":
								return CBJSON.Parser.TOKEN.NULL;
							}
							return CBJSON.Parser.TOKEN.NONE;
						}
						case ']':
							this.json.Read();
							return CBJSON.Parser.TOKEN.SQUARED_CLOSE;
						}
						goto IL_A2;
					case ',':
						this.json.Read();
						return CBJSON.Parser.TOKEN.COMMA;
					case '-':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return CBJSON.Parser.TOKEN.NUMBER;
					case ':':
						return CBJSON.Parser.TOKEN.COLON;
					}
					goto IL_8D;
				}
			}

			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			public static object Parse(string jsonString)
			{
				object result;
				using (CBJSON.Parser parser = new CBJSON.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			private Hashtable ParseObject()
			{
				Hashtable hashtable = new Hashtable();
				this.json.Read();
				while (true)
				{
					CBJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case CBJSON.Parser.TOKEN.NONE:
						goto IL_37;
					case CBJSON.Parser.TOKEN.CURLY_OPEN:
					{
						IL_2B:
						if (nextToken == CBJSON.Parser.TOKEN.COMMA)
						{
							continue;
						}
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_2;
						}
						if (this.NextToken != CBJSON.Parser.TOKEN.COLON)
						{
							goto Block_3;
						}
						this.json.Read();
						hashtable[text] = this.ParseValue();
						continue;
					}
					case CBJSON.Parser.TOKEN.CURLY_CLOSE:
						return hashtable;
					}
					goto IL_2B;
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			private ArrayList ParseArray()
			{
				ArrayList arrayList = new ArrayList();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					CBJSON.Parser.TOKEN nextToken = this.NextToken;
					CBJSON.Parser.TOKEN tOKEN = nextToken;
					switch (tOKEN)
					{
					case CBJSON.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						continue;
					case CBJSON.Parser.TOKEN.COLON:
						IL_38:
						if (tOKEN != CBJSON.Parser.TOKEN.NONE)
						{
							object value = this.ParseByToken(nextToken);
							arrayList.Add(value);
							continue;
						}
						return null;
					case CBJSON.Parser.TOKEN.COMMA:
						continue;
					}
					goto IL_38;
				}
				return arrayList;
			}

			private object ParseValue()
			{
				CBJSON.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			private object ParseByToken(CBJSON.Parser.TOKEN token)
			{
				switch (token)
				{
				case CBJSON.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case CBJSON.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case CBJSON.Parser.TOKEN.STRING:
					return this.ParseString();
				case CBJSON.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case CBJSON.Parser.TOKEN.TRUE:
					return true;
				case CBJSON.Parser.TOKEN.FALSE:
					return false;
				case CBJSON.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					char c = nextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else
						{
							if (this.json.Peek() != -1)
							{
								nextChar = this.NextChar;
								char c2 = nextChar;
								switch (c2)
								{
								case 'n':
									stringBuilder.Append('\n');
									continue;
								case 'o':
								case 'p':
								case 'q':
								case 's':
									IL_A5:
									if (c2 == '"' || c2 == '/' || c2 == '\\')
									{
										stringBuilder.Append(nextChar);
										continue;
									}
									if (c2 == 'b')
									{
										stringBuilder.Append('\b');
										continue;
									}
									if (c2 != 'f')
									{
										continue;
									}
									stringBuilder.Append('\f');
									continue;
								case 'r':
									stringBuilder.Append('\r');
									continue;
								case 't':
									stringBuilder.Append('\t');
									continue;
								case 'u':
								{
									char[] array = new char[4];
									for (int i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									continue;
								}
								}
								goto IL_A5;
							}
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}
		}

		private sealed class Serializer
		{
			private StringBuilder builder;

			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			public static string Serialize(object obj)
			{
				CBJSON.Serializer serializer = new CBJSON.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			private void SerializeValue(object val)
			{
				string str;
				ArrayList anArray;
				Hashtable obj;
				if (val == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (val as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (val is bool)
				{
					this.builder.Append((!(bool)val) ? "false" : "true");
				}
				else if ((anArray = (val as ArrayList)) != null)
				{
					this.SerializeArray(anArray);
				}
				else if ((obj = (val as Hashtable)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (val is char)
				{
					this.SerializeString(new string((char)val, 1));
				}
				else
				{
					this.SerializeOther(val);
				}
			}

			private void SerializeObject(Hashtable obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (DictionaryEntry dictionaryEntry in obj)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(dictionaryEntry.Key.ToString());
					this.builder.Append(':');
					this.SerializeValue(dictionaryEntry.Value);
					flag = false;
				}
				this.builder.Append('}');
			}

			private void SerializeArray(ArrayList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object val = anArray[i];
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(val);
					flag = false;
				}
				this.builder.Append(']');
			}

			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				for (int i = 0; i < array.Length; i++)
				{
					char c = array[i];
					char c2 = c;
					switch (c2)
					{
					case '\b':
						this.builder.Append("\\b");
						goto IL_14C;
					case '\t':
						this.builder.Append("\\t");
						goto IL_14C;
					case '\n':
						this.builder.Append("\\n");
						goto IL_14C;
					case '\v':
						IL_44:
						if (c2 == '"')
						{
							this.builder.Append("\\\"");
							goto IL_14C;
						}
						if (c2 != '\\')
						{
							int num = Convert.ToInt32(c);
							if (num >= 32 && num <= 126)
							{
								this.builder.Append(c);
							}
							else
							{
								this.builder.Append("\\u");
								this.builder.Append(num.ToString("x4"));
							}
							goto IL_14C;
						}
						this.builder.Append("\\\\");
						goto IL_14C;
					case '\f':
						this.builder.Append("\\f");
						goto IL_14C;
					case '\r':
						this.builder.Append("\\r");
						goto IL_14C;
					}
					goto IL_44;
					IL_14C:;
				}
				this.builder.Append('"');
			}

			private void SerializeOther(object val)
			{
				if (val is float)
				{
					this.builder.Append(((float)val).ToString("R"));
				}
				else if (val is int || val is uint || val is long || val is sbyte || val is byte || val is short || val is ushort || val is ulong)
				{
					this.builder.Append(val);
				}
				else if (val is double || val is decimal)
				{
					this.builder.Append(Convert.ToDouble(val).ToString("R"));
				}
				else
				{
					this.SerializeString(val.ToString());
				}
			}
		}

		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return CBJSON.Parser.Parse(json);
		}

		public static string Serialize(object obj)
		{
			return CBJSON.Serializer.Serialize(obj);
		}
	}
}
