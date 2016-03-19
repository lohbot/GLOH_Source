using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace NLibCs
{
	public class NDataReader : IDisposable, IEnumerable
	{
		public interface IBindee
		{
			bool ReadFrom(NDataReader dr);
		}

		public interface IRowBindee
		{
			bool ReadFrom(NDataReader.Row tsRow);
		}

		public class Row
		{
			public enum TYPE
			{
				LINE_NONE,
				LINE_COMMENT,
				LINE_SECTION,
				LINE_SUBSECTION,
				LINE_BLANK,
				LINE_DATA
			}

			public static NDataReader.Row EMPTY = new NDataReader.Row(null);

			protected List<string> values = new List<string>();

			protected NDataReader _owner;

			public NDataReader.Row.TYPE LineType
			{
				get;
				set;
			}

			public List<string> Values
			{
				get
				{
					return this.values;
				}
			}

			public int ColumnCount
			{
				get
				{
					return this.values.Count;
				}
			}

			public NDataStr this[int columnInndex]
			{
				get
				{
					NDataStr result;
					this.GetColumn(columnInndex, out result.str, false);
					return result;
				}
			}

			public NDataStr this[string columnName]
			{
				get
				{
					int index = 0;
					if (this._owner != null)
					{
						this._owner.ReadFieldNames();
						index = this._owner.GetFieldIndex(columnName);
					}
					NDataStr result;
					this.GetColumn(index, out result.str, false);
					return result;
				}
			}

			public Row()
			{
			}

			internal Row(NDataReader owner)
			{
				this._owner = owner;
			}

			public bool GetColumn(int index, out uint output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && uint.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0u;
				return false;
			}

			public bool GetColumn(int index, out long output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && long.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0L;
				return false;
			}

			public bool GetColumn(int index, out ulong output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && ulong.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0uL;
				return false;
			}

			public bool GetColumn(int index, out short output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && short.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(int index, out ushort output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && ushort.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(int index, out double output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && double.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0.0;
				return false;
			}

			public bool GetColumn(int index, out float output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && float.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0f;
				return false;
			}

			public bool GetColumn(int index, out byte output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && byte.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(int index, out int output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && int.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(int index, out string output)
			{
				return this.GetColumn(index, out output, true);
			}

			public bool GetColumn(int index, out string output, bool bAutoReplace)
			{
				output = "(null)";
				try
				{
					if (this.values != null && index < this.values.Count)
					{
						if (bAutoReplace)
						{
							output = this.values[index].Replace("{\\r\\n}", "\r\n").Replace("{\\t}", "\\t");
						}
						else
						{
							output = this.values[index];
						}
						return true;
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				return false;
			}

			public string GetColumn(int index)
			{
				string result;
				this.GetColumn(index, out result, false);
				return result;
			}

			public string GetColumn(int index, bool bAutoReplace)
			{
				string result;
				this.GetColumn(index, out result, bAutoReplace);
				return result;
			}

			public bool GetColumn(ref object[] args)
			{
				bool result;
				try
				{
					NDataStr dstr = default(NDataStr);
					for (int i = 0; i < args.Length; i++)
					{
						dstr.str = this.values[i];
						if (args[i] is string)
						{
							args[i] = dstr.str;
						}
						else if (args[i] is int)
						{
							args[i] = dstr;
						}
						else if (args[i] is double)
						{
							args[i] = dstr;
						}
						else if (args[i] is float)
						{
							args[i] = dstr;
						}
						else if (args[i] is uint)
						{
							args[i] = dstr;
						}
						else if (args[i] is long)
						{
							args[i] = dstr;
						}
						else if (args[i] is ulong)
						{
							args[i] = dstr;
						}
						else if (args[i] is short)
						{
							args[i] = dstr;
						}
						else if (args[i] is ushort)
						{
							args[i] = dstr;
						}
						else if (args[i] is bool)
						{
							args[i] = dstr;
						}
						else if (args[i] is Vector2)
						{
							args[i] = dstr;
						}
						else if (args[i] is Vector3)
						{
							args[i] = dstr;
						}
						else
						{
							if (!(args[i] is Vector4))
							{
								result = false;
								return result;
							}
							args[i] = dstr;
						}
					}
					result = true;
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
					result = false;
				}
				return result;
			}

			public void Clear()
			{
				this.LineType = NDataReader.Row.TYPE.LINE_NONE;
				this.values.Clear();
			}

			private void _OutputDebug(object msg)
			{
				UnityEngine.Debug.LogWarning(msg);
			}

			public override string ToString()
			{
				if (1 < this.Values.Count)
				{
					int num = 0;
					foreach (string current in this.Values)
					{
						num += current.Length;
					}
					StringBuilder stringBuilder = new StringBuilder(num + 100);
					stringBuilder.AppendFormat("{0}:\t", this.LineType);
					foreach (string current2 in this.Values)
					{
						stringBuilder.AppendFormat("{0}|", current2);
					}
					return stringBuilder.ToString();
				}
				if (this.Values.Count == 1)
				{
					return string.Format("{0}:\t{1}", this.LineType, this.values[0]);
				}
				return string.Format("{0}: X", this.LineType);
			}

			public string ToDataString()
			{
				if (1 < this.Values.Count)
				{
					int num = 0;
					foreach (string current in this.Values)
					{
						num += current.Length;
					}
					int num2 = 0;
					StringBuilder stringBuilder = new StringBuilder(num + 100);
					foreach (string current2 in this.Values)
					{
						stringBuilder.AppendFormat("{0}", current2);
						if (num2 < this.Values.Count - 1)
						{
							stringBuilder.AppendFormat("\t", new object[0]);
						}
						num2++;
					}
					return stringBuilder.ToString();
				}
				if (this.Values.Count == 1)
				{
					return string.Format("{0}", this.values[0]);
				}
				return string.Empty;
			}

			public void __parse_from(string line)
			{
				this.__parse_from(line, 0, 0, '\t');
			}

			public void __parse_from(string line, int nStartLine, int nLineLength, char seperator)
			{
				if (nLineLength == 0 && nStartLine == 0)
				{
					nLineLength = line.Length;
				}
				int i = nStartLine;
				char c = '\0';
				int num = nStartLine + nLineLength;
				while (i < num)
				{
					c = line[i];
					if (c != ' ' && c != '﻿')
					{
						break;
					}
					i++;
				}
				if (c == '\0')
				{
					this.LineType = NDataReader.Row.TYPE.LINE_BLANK;
					this.Values.Add(string.Empty);
					return;
				}
				if ((c == '/' && line[i + 1] == '/') || c == '#' || c == ';')
				{
					this.LineType = NDataReader.Row.TYPE.LINE_COMMENT;
				}
				else if (c == '[')
				{
					this.LineType = NDataReader.Row.TYPE.LINE_SECTION;
				}
				else if (c == '<')
				{
					this.LineType = NDataReader.Row.TYPE.LINE_SUBSECTION;
				}
				else
				{
					this.LineType = NDataReader.Row.TYPE.LINE_DATA;
				}
				bool flag = c == '\t';
				int num2 = 0;
				int num3 = i;
				for (int j = i; j < num; j++)
				{
					num2++;
					if (seperator == line[j])
					{
						if (flag)
						{
							flag = (num2 - 1 == 0);
						}
						this.Values.Add(line.Substring(num3, num2 - 1));
						num3 = j + 1;
						num2 = 0;
					}
				}
				if (num3 == 0 && line.Length == nLineLength)
				{
					this.Values.Add(line);
				}
				else
				{
					string text = line.Substring(num3, num2);
					this.Values.Add(text);
					if (flag)
					{
						flag = string.IsNullOrEmpty(text);
					}
				}
				if (flag)
				{
					this.LineType = NDataReader.Row.TYPE.LINE_BLANK;
				}
			}

			public bool GetColumn(int index, out Vector2 output)
			{
				try
				{
					if (this.values != null && index < this.values.Count)
					{
						string[] array = NDataReader.__SplitCoord(this.values[index]);
						if (array.Length == 2)
						{
							float x = 0f;
							float y = 0f;
							if (float.TryParse(array[0], out x) && float.TryParse(array[1], out y))
							{
								Vector2 vector = new Vector2(x, y);
								output = vector;
								return true;
							}
						}
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = Vector2.zero;
				return false;
			}

			public bool GetColumn(int index, out Vector3 output)
			{
				try
				{
					if (this.values != null && index < this.values.Count)
					{
						string[] array = NDataReader.__SplitCoord(this.values[index]);
						if (array.Length == 3)
						{
							float x = 0f;
							float y = 0f;
							float z = 0f;
							if (float.TryParse(array[0], out x) && float.TryParse(array[1], out y) && float.TryParse(array[2], out z))
							{
								Vector3 vector = new Vector3(x, y, z);
								output = vector;
								return true;
							}
						}
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = Vector3.zero;
				return false;
			}

			public bool GetColumn(int index, out Vector4 output)
			{
				try
				{
					if (this.values != null && index < this.values.Count)
					{
						string[] array = NDataReader.__SplitCoord(this.values[index]);
						if (array.Length == 4)
						{
							float x = 0f;
							float y = 0f;
							float z = 0f;
							float w = 0f;
							if (float.TryParse(array[0], out x) && float.TryParse(array[1], out y) && float.TryParse(array[2], out z) && float.TryParse(array[3], out w))
							{
								Vector4 vector = new Vector4(x, y, z, w);
								output = vector;
								return true;
							}
						}
					}
				}
				catch (Exception msg)
				{
					this._OutputDebug(msg);
				}
				output = Vector4.zero;
				return false;
			}
		}

		protected class _RowEnum : IEnumerator
		{
			public NDataReader _dr;

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			public NDataReader.Row Current
			{
				get
				{
					NDataReader.Row currentRow;
					try
					{
						currentRow = this._dr.CurrentRow;
					}
					catch (Exception var_0_16)
					{
						throw new InvalidOperationException();
					}
					return currentRow;
				}
			}

			public _RowEnum(NDataReader dr)
			{
				this._dr = dr;
			}

			public bool MoveNext()
			{
				this._dr.NextLine();
				return !this._dr.IsEndOfSection();
			}

			public void Reset()
			{
				this._dr._curSection.FirstLine(0);
			}
		}

		protected static string[] LineSeperators = new string[]
		{
			"\r\n"
		};

		protected static bool ms_bOptimize = true;

		public static NDataReader.Row EMPTY_ROW = new NDataReader.Row(null);

		protected internal List<NDataReader.Row> m_list_Rows = new List<NDataReader.Row>();

		protected Dictionary<string, int> m_dic_FieldIndex = new Dictionary<string, int>();

		protected NDataSection _curSection;

		public static bool EditorMode;

		private Encoding encoding = Encoding.Default;

		public static bool UseOptimize
		{
			get
			{
				return NDataReader.ms_bOptimize;
			}
			set
			{
				NDataReader.ms_bOptimize = value;
			}
		}

		public NDataReader.Row CurrentRow
		{
			get
			{
				return this._curSection.CurrentRow;
			}
		}

		public NDataSection CurrentSection
		{
			get
			{
				return this._curSection;
			}
		}

		public bool UseSubSection
		{
			get;
			set;
		}

		public char[] ColumnSeperators
		{
			get;
			set;
		}

		public char ColumnSeperator
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public string EncryptedFileName
		{
			get;
			set;
		}

		public Dictionary<string, int> FieldIndex
		{
			get
			{
				return this.m_dic_FieldIndex;
			}
		}

		public bool UseFileNameEncryption
		{
			get;
			set;
		}

		public string GetName
		{
			get
			{
				if (this.UseFileNameEncryption)
				{
					return this.EncryptedFileName;
				}
				return this.FileName;
			}
		}

		public NDataSection this[string sectionName]
		{
			get
			{
				bool flag = true;
				if (this._curSection == null)
				{
					this._curSection = new NDataSection(this, sectionName);
				}
				else
				{
					flag = this._curSection.FindSection(sectionName);
				}
				if (flag)
				{
					return this._curSection;
				}
				return new NDataSection(new NDataReader(), sectionName);
			}
		}

		public NDataReader()
		{
			this.FileName = string.Empty;
			this.ColumnSeperators = new char[]
			{
				'\t'
			};
			this.ColumnSeperator = '\t';
			this._curSection = new NDataSection(this, string.Empty);
			this.UseSubSection = false;
			this.UseFileNameEncryption = false;
			this._clear();
		}

		public bool ReadTo(NDataReader.IBindee bindee)
		{
			return bindee.ReadFrom(this);
		}

		public bool ReadToCurrentRow(NDataReader.IRowBindee rowBindee)
		{
			return rowBindee.ReadFrom(this.CurrentRow);
		}

		public NDataStr ReadKeyData(string keyName)
		{
			NDataStr result = default(NDataStr);
			keyName = keyName.ToLower();
			this._curSection.FirstLine(1);
			string text = string.Empty;
			foreach (NDataReader.Row row in this)
			{
				char[] separator = new char[]
				{
					'='
				};
				string[] array = row.ToDataString().Split(separator, 2);
				text = array[0].ToLower().TrimStart(new char[0]);
				if (text.StartsWith(keyName))
				{
					result.str = array[1].Trim();
					return result;
				}
			}
			return NDataStr.Empty;
		}

		public bool ReadKeyData(string keyName, out string value, string default_value = "")
		{
			bool flag = this.ReadKeyDataNoException(keyName, out value, default_value);
			if (!flag)
			{
				throw new NoKeyDataException(this, this._curSection, keyName);
			}
			return flag;
		}

		public bool ReadKeyDataNoException(string keyName, out string value, string default_value = "")
		{
			value = default_value;
			keyName = keyName.ToLower();
			this._curSection.FirstLine(1);
			string text = string.Empty;
			foreach (NDataReader.Row row in this)
			{
				char[] separator = new char[]
				{
					'='
				};
				string[] array = row.ToDataString().Split(separator, 2);
				text = array[0].ToLower().TrimStart(new char[0]);
				if (text.StartsWith(keyName))
				{
					value = array[1].Trim();
					return true;
				}
			}
			return false;
		}

		public bool ReadKeyData(string keyName, out int value, int defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && int.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out uint value, uint defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && uint.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out short value, short defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && short.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out ushort value, ushort defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && ushort.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out long value, long defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && long.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out ulong value, ulong defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && ulong.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out float value, float defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyData(keyName, out s, string.Empty) && float.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out double value, double defalut_value)
		{
			value = defalut_value;
			string empty = string.Empty;
			return this.ReadKeyData(keyName, out empty, string.Empty) && double.TryParse(empty, out value);
		}

		public bool ReadKeyData(string keyName, out int value)
		{
			return this.ReadKeyData(keyName, out value, 0);
		}

		public bool ReadKeyData(string keyName, out uint value)
		{
			return this.ReadKeyData(keyName, out value, 0u);
		}

		public bool ReadKeyData(string keyName, out short value)
		{
			return this.ReadKeyData(keyName, out value, 0);
		}

		public bool ReadKeyData(string keyName, out ushort value)
		{
			return this.ReadKeyData(keyName, out value, 0);
		}

		public bool ReadKeyData(string keyName, out long value)
		{
			return this.ReadKeyData(keyName, out value, 0L);
		}

		public bool ReadKeyData(string keyName, out ulong value)
		{
			return this.ReadKeyData(keyName, out value, 0uL);
		}

		public bool ReadKeyData(string keyName, out float value)
		{
			return this.ReadKeyData(keyName, out value, 0f);
		}

		public bool ReadKeyData(string keyName, out double value)
		{
			return this.ReadKeyData(keyName, out value, 0.0);
		}

		public bool ReadKeyData(string keyName, out bool value)
		{
			return this.ReadKeyData(keyName, out value, false);
		}

		public bool ReadKeyData(string keyName, out bool value, bool defalut_value)
		{
			value = defalut_value;
			string text;
			if (this.ReadKeyData(keyName, out text, string.Empty))
			{
				text = text.ToLower();
				if (text.Contains("true"))
				{
					value = true;
				}
				else if (text.Contains("false"))
				{
					value = false;
				}
				else
				{
					int num;
					if (!int.TryParse(text, out num))
					{
						return false;
					}
					value = (num != 0);
				}
				return true;
			}
			return false;
		}

		public bool ReadKeyDataNoException(string keyName, out int value, int defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && int.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out uint value, uint defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && uint.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out short value, short defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && short.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out ushort value, ushort defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && ushort.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out long value, long defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && long.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out ulong value, ulong defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && ulong.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out float value, float defalut_value)
		{
			value = defalut_value;
			string s;
			return this.ReadKeyDataNoException(keyName, out s, string.Empty) && float.TryParse(s, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out double value, double defalut_value)
		{
			value = defalut_value;
			string empty = string.Empty;
			return this.ReadKeyDataNoException(keyName, out empty, string.Empty) && double.TryParse(empty, out value);
		}

		public bool ReadKeyDataNoException(string keyName, out int value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0);
		}

		public bool ReadKeyDataNoException(string keyName, out uint value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0u);
		}

		public bool ReadKeyDataNoException(string keyName, out short value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0);
		}

		public bool ReadKeyDataNoException(string keyName, out ushort value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0);
		}

		public bool ReadKeyDataNoException(string keyName, out long value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0L);
		}

		public bool ReadKeyDataNoException(string keyName, out ulong value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0uL);
		}

		public bool ReadKeyDataNoException(string keyName, out float value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0f);
		}

		public bool ReadKeyDataNoException(string keyName, out double value)
		{
			return this.ReadKeyDataNoException(keyName, out value, 0.0);
		}

		public bool ReadKeyDataNoException(string keyName, out bool value)
		{
			return this.ReadKeyDataNoException(keyName, out value, false);
		}

		public bool ReadKeyDataNoException(string keyName, out bool value, bool defalut_value)
		{
			value = defalut_value;
			string text;
			if (this.ReadKeyDataNoException(keyName, out text, string.Empty))
			{
				text = text.ToLower();
				if (text.Contains("true"))
				{
					value = true;
				}
				else if (text.Contains("false"))
				{
					value = false;
				}
				else
				{
					int num;
					if (!int.TryParse(text, out num))
					{
						return false;
					}
					value = (num != 0);
				}
				return true;
			}
			return false;
		}

		public static string GetMD5(string str)
		{
			if (str == string.Empty)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			str = str.ToLower();
			str = Path.GetFileName(str);
			byte[] bytes = Encoding.ASCII.GetBytes(str);
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString().ToLower();
		}

		public Stream GetFileStream(string filename)
		{
			FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			FileInfo fileInfo = new FileInfo(filename);
			NEncrypt.enc_type enc_type = NEncrypt.IsForwarded(fileStream);
			if (enc_type != NEncrypt.enc_type.ENC_NONE)
			{
				Stream stream = this.Decrypt(fileStream, Convert.ToUInt32(fileInfo.Length));
				stream.Position = 0L;
				return stream;
			}
			return fileStream;
		}

		public Stream GetFileStream(Stream stream)
		{
			NEncrypt.enc_type enc_type = NEncrypt.IsForwarded(stream);
			if (enc_type != NEncrypt.enc_type.ENC_NONE)
			{
				Stream stream2 = this.Decrypt(stream, Convert.ToUInt32(stream.Length));
				stream2.Position = 0L;
				return stream2;
			}
			return stream;
		}

		public static NDataReader FromFile(string filename, bool useFileNameEncryption)
		{
			NDataReader nDataReader = new NDataReader();
			nDataReader.UseFileNameEncryption = useFileNameEncryption;
			if (nDataReader.Load(filename))
			{
				return nDataReader;
			}
			return null;
		}

		public static NDataReader FromContext(string context)
		{
			NDataReader nDataReader = new NDataReader();
			if (nDataReader.LoadFrom(context))
			{
				return nDataReader;
			}
			return null;
		}

		private static string[] __SplitCoord(string data)
		{
			int num = 0;
			int num2 = data.Length;
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == '(')
				{
					num = i + 1;
				}
				else if (data[i] == ')')
				{
					num2 = i;
					break;
				}
			}
			data = data.Substring(num, num2 - num);
			return data.Split(new char[]
			{
				','
			});
		}

		private void Assert(bool p)
		{
			throw new NotImplementedException();
		}

		public bool ReadKeyData(string keyName, out Vector2 value)
		{
			return this.ReadKeyData(keyName, out value, Vector2.zero);
		}

		public bool ReadKeyData(string keyName, out Vector3 value)
		{
			return this.ReadKeyData(keyName, out value, Vector3.zero);
		}

		public bool ReadKeyData(string keyName, out Vector4 value)
		{
			return this.ReadKeyData(keyName, out value, Vector4.zero);
		}

		public bool ReadKeyData(string keyName, out Vector2 value, Vector2 defalut_value)
		{
			value = defalut_value;
			string empty = string.Empty;
			if (this.ReadKeyData(keyName, out empty, string.Empty))
			{
				string[] array = NDataReader.__SplitCoord(empty);
				if (array.Length == 2)
				{
					float new_x = 0f;
					float new_y = 0f;
					if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y))
					{
						value.Set(new_x, new_y);
						return true;
					}
				}
			}
			return false;
		}

		public bool ReadKeyData(string keyName, out Vector3 value, Vector3 defalut_value)
		{
			value = defalut_value;
			string empty = string.Empty;
			if (this.ReadKeyData(keyName, out empty, string.Empty))
			{
				string[] array = NDataReader.__SplitCoord(empty);
				if (array.Length == 3)
				{
					float new_x = 0f;
					float new_y = 0f;
					float new_z = 0f;
					if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y) && float.TryParse(array[2], out new_z))
					{
						value.Set(new_x, new_y, new_z);
						return true;
					}
				}
			}
			return false;
		}

		public bool ReadKeyData(string keyName, out Vector4 value, Vector4 defalut_value)
		{
			value = defalut_value;
			string empty = string.Empty;
			if (this.ReadKeyData(keyName, out empty, string.Empty))
			{
				string[] array = NDataReader.__SplitCoord(empty);
				if (array.Length == 4)
				{
					float new_x = 0f;
					float new_y = 0f;
					float new_z = 0f;
					float new_w = 0f;
					if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y) && float.TryParse(array[2], out new_z) && float.TryParse(array[3], out new_w))
					{
						value.Set(new_x, new_y, new_z, new_w);
						return true;
					}
				}
			}
			return false;
		}

		public bool LoadFrom(string strContext)
		{
			this._clear();
			if (strContext == null)
			{
				return false;
			}
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, this.encoding);
			streamWriter.Write(strContext);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			using (Stream fileStream = this.GetFileStream(memoryStream))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, this.encoding, true))
				{
					strContext = streamReader.ReadToEnd();
				}
			}
			streamWriter.Close();
			memoryStream.Close();
			if (NDataReader.UseOptimize)
			{
				return this.__parse_line_optimize(strContext);
			}
			return this.__parse_line(strContext);
		}

		public bool LoadFrom(string strContext, Encoding _encoding)
		{
			this.encoding = _encoding;
			return this.LoadFrom(strContext);
		}

		public bool LoadFrom(string strContext, NDataReader.IBindee bindee)
		{
			this._clear();
			if (strContext == null)
			{
				return false;
			}
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.Default);
			streamWriter.Write(strContext);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			using (Stream fileStream = this.GetFileStream(memoryStream))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					strContext = streamReader.ReadToEnd();
				}
			}
			streamWriter.Close();
			memoryStream.Close();
			return this.LoadFrom(strContext) && bindee.ReadFrom(this);
		}

		public bool LoadFrom(string strContext, string strSection, NDataReader.IRowBindee rowBindee)
		{
			this._clear();
			if (strContext == null)
			{
				return false;
			}
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.Default);
			streamWriter.Write(strContext);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			using (Stream fileStream = this.GetFileStream(memoryStream))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					strContext = streamReader.ReadToEnd();
				}
			}
			streamWriter.Close();
			memoryStream.Close();
			return this.__parse_line_optimize(strContext, strSection, rowBindee);
		}

		private bool __parse_line(string strContext)
		{
			string[] array = strContext.Split(NDataReader.LineSeperators, StringSplitOptions.RemoveEmptyEntries);
			this.m_list_Rows.Capacity = 2000;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				this.__parse_row(text.TrimStart(new char[0]));
			}
			return true;
		}

		private bool __parse_line_optimize(string strContext)
		{
			int nStartLine = 0;
			int num = 0;
			for (int i = 0; i < strContext.Length; i++)
			{
				if (strContext[i] == '\r' && strContext[i + 1] == '\n')
				{
					this.__parse_row_optimize(strContext, nStartLine, num, null);
					nStartLine = ++i + 1;
					num = 0;
				}
				else
				{
					num++;
				}
			}
			this.__parse_row_optimize(strContext, nStartLine, num, null);
			return true;
		}

		private bool __parse_line_optimize(string strContext, string strSection, NDataReader.IRowBindee rowBindee)
		{
			int i = 0;
			int num = 0;
			bool flag = strSection != null && strSection != string.Empty;
			bool flag2 = !flag;
			string value = (!flag) ? string.Empty : strSection.ToLower();
			for (int j = 0; j < strContext.Length; j++)
			{
				if (strContext[j] == '\r' && strContext[j + 1] == '\n')
				{
					int num2 = i + num - 1;
					while (i < num2)
					{
						if (strContext[i] != ' ')
						{
							break;
						}
						i++;
					}
					if (flag)
					{
						if (strContext[i] == '[')
						{
							string text = strContext.Substring(i, num).ToLower();
							if (flag2)
							{
								return true;
							}
							if (text.StartsWith(value))
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							this.__parse_row_optimize(strContext, i, num, rowBindee);
						}
					}
					else
					{
						this.__parse_row_optimize(strContext, i, num, rowBindee);
					}
					i = ++j + 1;
					num = 0;
				}
				else
				{
					num++;
				}
			}
			this.__parse_row_optimize(strContext, i, num, rowBindee);
			return true;
		}

		public bool LoadFrom(byte[] bytes)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.LoadFrom(memoryStream);
			}
			return result;
		}

		public bool LoadFrom(byte[] bytes, NDataReader.IBindee bindee)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.LoadFrom(memoryStream, bindee);
			}
			return result;
		}

		public bool LoadFrom(byte[] bytes, string strSection, NDataReader.IRowBindee bindee)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.LoadFrom(memoryStream, strSection, bindee);
			}
			return result;
		}

		public bool LoadFrom(Stream stream)
		{
			this._clear();
			Stream fileStream;
			stream = (fileStream = this.GetFileStream(stream));
			bool result;
			try
			{
				using (StreamReader streamReader = new StreamReader(stream, this.encoding, true))
				{
					this.m_list_Rows.Capacity = 2000;
					if (NDataReader.UseOptimize)
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							this.__parse_row_optimize(text);
						}
					}
					else
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							text = text.Trim();
							this.__parse_row(text);
						}
					}
					result = true;
				}
			}
			finally
			{
				if (fileStream != null)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
			return result;
		}

		public bool LoadFrom(Stream stream, Encoding _encoding)
		{
			this.encoding = _encoding;
			return this.LoadFrom(stream);
		}

		public bool LoadFrom(Stream stream, NDataReader.IBindee bindee)
		{
			this._clear();
			try
			{
				Stream fileStream;
				stream = (fileStream = this.GetFileStream(stream));
				try
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true))
					{
						string strContext = streamReader.ReadToEnd();
						streamReader.Close();
						return this.LoadFrom(strContext, bindee);
					}
				}
				finally
				{
					if (fileStream != null)
					{
						((IDisposable)fileStream).Dispose();
					}
				}
			}
			catch (Exception var_3_63)
			{
			}
			return false;
		}

		public bool LoadFrom(Stream stream, string strSection, NDataReader.IRowBindee rowBindee)
		{
			this._clear();
			Stream fileStream;
			stream = (fileStream = this.GetFileStream(stream));
			bool result;
			try
			{
				using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true))
				{
					string strContext = streamReader.ReadToEnd();
					streamReader.Close();
					result = this.LoadFrom(strContext, strSection, rowBindee);
				}
			}
			finally
			{
				if (fileStream != null)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
			return result;
		}

		public string GetFileString(string strFileName)
		{
			this._clear();
			string text = string.Empty;
			if (strFileName == null)
			{
				return string.Empty;
			}
			this.FileName = strFileName;
			this.EncryptedFileName = string.Empty;
			if (this.UseFileNameEncryption && !NDataReader.EditorMode)
			{
				if (!Path.IsPathRooted(strFileName))
				{
					strFileName = Path.GetFullPath(strFileName);
				}
				string fileName = Path.GetFileName(strFileName);
				string mD = NDataReader.GetMD5(fileName);
				FileInfo fileInfo = new FileInfo(strFileName);
				strFileName = Path.Combine(fileInfo.DirectoryName, mD);
				this.EncryptedFileName = strFileName;
			}
			if (!File.Exists(strFileName))
			{
				Console.WriteLine("{0} does not exist.", strFileName);
				return string.Empty;
			}
			string result;
			using (Stream fileStream = this.GetFileStream(strFileName))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					text = streamReader.ReadToEnd();
				}
				result = text;
			}
			return result;
		}

		public bool Load(string strFileName)
		{
			this._clear();
			if (strFileName == null)
			{
				return false;
			}
			this.FileName = strFileName;
			this.EncryptedFileName = string.Empty;
			if (this.UseFileNameEncryption && !NDataReader.EditorMode)
			{
				if (!Path.IsPathRooted(strFileName))
				{
					strFileName = Path.GetFullPath(strFileName);
				}
				string fileName = Path.GetFileName(strFileName);
				string mD = NDataReader.GetMD5(fileName);
				FileInfo fileInfo = new FileInfo(strFileName);
				strFileName = Path.Combine(fileInfo.DirectoryName, mD);
				this.EncryptedFileName = strFileName;
			}
			if (!File.Exists(strFileName))
			{
				Console.WriteLine("{0} does not exist.", strFileName);
				return false;
			}
			bool result;
			using (Stream fileStream = this.GetFileStream(strFileName))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					this.m_list_Rows.Capacity = 2000;
					if (NDataReader.UseOptimize)
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							this.__parse_row_optimize(text);
						}
					}
					else
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							text = text.TrimStart(new char[0]);
							this.__parse_row(text);
						}
					}
					result = true;
				}
			}
			return result;
		}

		public bool Load(string strFileName, string strSection, NDataReader.IRowBindee rowBindee)
		{
			this._clear();
			if (strFileName == null)
			{
				return false;
			}
			if (!File.Exists(strFileName))
			{
				Console.WriteLine("{0} does not exist.", strFileName);
				return false;
			}
			bool result;
			using (Stream fileStream = this.GetFileStream(strFileName))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					string strContext = streamReader.ReadToEnd();
					streamReader.Close();
					this.FileName = strFileName;
					result = this.LoadFrom(strContext, strSection, rowBindee);
				}
			}
			return result;
		}

		private void _clear()
		{
			this.m_list_Rows.Clear();
			this._curSection.Clear();
		}

		public void Dispose()
		{
			this._clear();
		}

		private MemoryStream Decrypt(Stream _stream, uint _uiFileSize)
		{
			byte[] array = new byte[_uiFileSize];
			NEncrypt.enc_args enc_args = new NEncrypt.enc_args(NEncrypt.enc_type.ENC_DEFAULT, array, (uint)array.Length, null, false);
			BinaryReader binaryReader = new BinaryReader(_stream);
			enc_args.pSrcBuf = binaryReader.ReadBytes(Convert.ToInt32(_uiFileSize));
			enc_args.nSrcSize = _uiFileSize;
			enc_args.uiSrcFileSize = _uiFileSize;
			bool flag = NEncrypt.Decrypt(enc_args, ' ');
			MemoryStream memoryStream = null;
			if (flag)
			{
				memoryStream = new MemoryStream();
				memoryStream.Write(enc_args.pOutBuf, 0, enc_args.pOutBuf.Length);
				memoryStream.Position = 0L;
			}
			return memoryStream;
		}

		private bool __parse_row(string line)
		{
			if (line == null)
			{
				return false;
			}
			NDataReader.Row row = new NDataReader.Row(this);
			string[] array = line.Split(this.ColumnSeperators);
			for (int i = 0; i < array.Length; i++)
			{
				string item = array[i];
				row.Values.Add(item);
			}
			string column = row.GetColumn(0);
			if (column == null)
			{
				row.LineType = NDataReader.Row.TYPE.LINE_BLANK;
			}
			else if (column == string.Empty || column[0] == ' ')
			{
				row.LineType = NDataReader.Row.TYPE.LINE_BLANK;
			}
			else if ((column[0] == '/' && column[1] == '/') || column[0] == '#')
			{
				row.LineType = NDataReader.Row.TYPE.LINE_COMMENT;
			}
			else if (column[0] == '[')
			{
				row.LineType = NDataReader.Row.TYPE.LINE_SECTION;
			}
			else if (column[0] == '<')
			{
				row.LineType = NDataReader.Row.TYPE.LINE_SUBSECTION;
			}
			else
			{
				row.LineType = NDataReader.Row.TYPE.LINE_DATA;
			}
			this.m_list_Rows.Add(row);
			if (this.m_list_Rows.Count == this.m_list_Rows.Capacity)
			{
				this.m_list_Rows.Capacity += 500;
			}
			return true;
		}

		private bool __parse_row_optimize(string line)
		{
			return this.__parse_row_optimize(line, 0, 0, null);
		}

		private bool __parse_row_optimize(string line, int nStartLine, int nLineLength, NDataReader.IRowBindee bindingRow)
		{
			if (nLineLength == 0 && nStartLine == 0)
			{
				nLineLength = line.Length;
			}
			NDataReader.Row row = new NDataReader.Row(this);
			row.__parse_from(line, nStartLine, nLineLength, this.ColumnSeperator);
			if (bindingRow == null)
			{
				this.m_list_Rows.Add(row);
				if (this.m_list_Rows.Count == this.m_list_Rows.Capacity)
				{
					this.m_list_Rows.Capacity += 500;
				}
			}
			else if (row.LineType == NDataReader.Row.TYPE.LINE_DATA)
			{
				bool flag = bindingRow.ReadFrom(row);
				if (!flag)
				{
				}
				return flag;
			}
			return true;
		}

		public bool BeginSection(string sectionName)
		{
			return this._curSection.FindSection(sectionName);
		}

		public bool ReadFieldNames()
		{
			bool flag = false;
			if (this.BeginSection("[FieldNames]"))
			{
				flag = true;
			}
			else if (this.BeginSection("[FiledNames]"))
			{
				flag = true;
			}
			if (flag)
			{
				IEnumerator enumerator = this.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						NDataReader.Row row = (NDataReader.Row)enumerator.Current;
						int num = 0;
						this.m_dic_FieldIndex.Clear();
						int num2 = 0;
						foreach (string current in row.Values)
						{
							int num3 = 1;
							string text = current;
							while (this.m_dic_FieldIndex.TryGetValue(text.ToLower(), out num))
							{
								text = string.Format("{0}{1}", current, num3++);
							}
							if (text != current)
							{
								this._OutputDebug(string.Format("중복된 필드명!! 수정필요!! {0} (임의변경:{1})", current, text));
							}
							this.m_dic_FieldIndex.Add(text.ToLower(), num2++);
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return this.m_dic_FieldIndex.Count != 0;
			}
			return false;
		}

		public int GetFieldIndex(string fieldName)
		{
			int result = -1;
			if (this.m_dic_FieldIndex.TryGetValue(fieldName.ToLower(), out result))
			{
				return result;
			}
			return -1;
		}

		public NDataSection GetFirstSection()
		{
			this._curSection.Clear();
			this.FirstLine();
			return this.GetNextSection();
		}

		public NDataSection GetNextSection()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			while (!this.IsEOF())
			{
				if (this.CurrentRow.LineType == NDataReader.Row.TYPE.LINE_SECTION)
				{
					stringBuilder.Length = 0;
					stringBuilder.Append(this.CurrentRow.ToDataString());
					stringBuilder = stringBuilder.Replace('[', ' ');
					stringBuilder = stringBuilder.Replace(']', ' ');
					stringBuilder = stringBuilder.Replace('<', ' ');
					stringBuilder = stringBuilder.Replace('>', ' ');
					this._curSection._beginRow = this._curSection._currentRow;
					this._curSection._beginRowSub = -1;
					this._curSection._sectionName = stringBuilder.ToString().Trim();
					this.NextDataLine(false);
					return this._curSection;
				}
				this._curSection.NextLine();
			}
			return null;
		}

		public bool ReadSection(string sectionName, out string output, string default_value = "")
		{
			output = default_value;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = this.ReadSection(sectionName, ref stringBuilder);
			if (flag)
			{
				output = stringBuilder.ToString();
			}
			return flag;
		}

		public bool ReadSection(string sectionName, ref StringBuilder output)
		{
			NDataSection nDataSection = this[sectionName];
			foreach (NDataReader.Row row in nDataSection)
			{
				output.AppendLine(row.ToDataString());
			}
			return true;
		}

		public NDataReader.Row GetCurrentRow()
		{
			return this.CurrentRow;
		}

		public int GetRowCountAt(string sectionName, string strSubSectionName)
		{
			NDataSection nDataSection = this[sectionName];
			return nDataSection.DataCount;
		}

		public void FirstLine()
		{
			this._curSection.FirstLine(0);
		}

		public int NextLine()
		{
			return this._curSection.NextLine();
		}

		public int NextDataLine(bool bSubSection = false)
		{
			return this._curSection.NextDataLine(bSubSection);
		}

		public bool IsEOF()
		{
			return this.m_list_Rows.Count < this._curSection._currentRow;
		}

		public bool IsEndOfSection()
		{
			return this._curSection.IsEndOfSection;
		}

		public bool IsEndOfSubSection()
		{
			return this._curSection.IsEndOfSubSection;
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			NDataReader.<GetEnumerator>c__IteratorA <GetEnumerator>c__IteratorA = new NDataReader.<GetEnumerator>c__IteratorA();
			<GetEnumerator>c__IteratorA.<>f__this = this;
			return <GetEnumerator>c__IteratorA;
		}

		public static int CheckBOM(ref byte[] bom, out Encoding encoding)
		{
			encoding = Encoding.Default;
			if (bom.Length < 4)
			{
				return 0;
			}
			if (bom[0] == 255 && bom[1] == 254)
			{
				encoding = Encoding.Unicode;
				return 2;
			}
			if (bom[0] == 254 && bom[1] == 255)
			{
				encoding = Encoding.BigEndianUnicode;
				return 2;
			}
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 255 && bom[3] == 254)
			{
				encoding = new UTF32Encoding(false, true);
				return 4;
			}
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 254 && bom[3] == 255)
			{
				encoding = new UTF32Encoding(true, true);
				return 4;
			}
			if (bom[0] == 239 && bom[1] == 187 && bom[2] == 191)
			{
				encoding = Encoding.UTF8;
				return 3;
			}
			if (bom[0] == 43 && bom[1] == 47 && bom[2] == 118)
			{
				encoding = Encoding.UTF7;
				return 3;
			}
			if (bom[0] == 221 && bom[1] == 115 && bom[2] == 102 && bom[2] == 115)
			{
				return 3;
			}
			if (bom[0] == 15 && bom[1] == 254 && bom[2] == 255)
			{
				return 3;
			}
			if (bom[0] == 251 && bom[1] == 238 && bom[2] == 40)
			{
				return 3;
			}
			encoding = Encoding.Default;
			return 0;
		}

		private void _OutputDebug(object o)
		{
		}
	}
}
