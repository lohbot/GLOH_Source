using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TsLibs
{
	public class TsDataReader : IDisposable, IEnumerable
	{
		public interface IBinding
		{
			bool ReadFrom(TsDataReader dr);
		}

		public interface IBindingRow
		{
			bool ReadFrom(TsDataReader.Row tsRow);
		}

		public class Row : IDisposable
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

			protected List<string> values = new List<string>();

			public TsDataReader.Row.TYPE LineType
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

			public string GetToken(int index)
			{
				return this.GetColumn(index);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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

			public bool GetColumn(int index, out bool output)
			{
				try
				{
					if (this.values != null && index < this.values.Count)
					{
						int num = 0;
						int.TryParse(this.values[index], out num);
						output = (num >= 1);
						return true;
					}
				}
				catch (Exception o)
				{
					this._OutputDebug(o);
				}
				output = false;
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
				catch (Exception o)
				{
					this._OutputDebug(o);
				}
				output = 0;
				return false;
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
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
				catch (Exception o)
				{
					this._OutputDebug(o);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(int index, out sbyte output)
			{
				try
				{
					if (this.values != null && index < this.values.Count && sbyte.TryParse(this.values[index], out output))
					{
						return true;
					}
				}
				catch (Exception o)
				{
					this._OutputDebug(o);
				}
				output = 0;
				return false;
			}

			public bool GetColumn(ref object[] args)
			{
				bool result;
				try
				{
					for (int i = 0; i < args.Length; i++)
					{
						string text = this.values[i];
						if (args[i] is string)
						{
							args[i] = text;
						}
						else if (args[i] is int)
						{
							args[i] = int.Parse(text);
						}
						else if (args[i] is double)
						{
							args[i] = double.Parse(text);
						}
						else if (args[i] is float)
						{
							args[i] = float.Parse(text);
						}
						else if (args[i] is uint)
						{
							args[i] = uint.Parse(text);
						}
						else if (args[i] is long)
						{
							args[i] = long.Parse(text);
						}
						else if (args[i] is ulong)
						{
							args[i] = ulong.Parse(text);
						}
						else if (args[i] is short)
						{
							args[i] = short.Parse(text);
						}
						else if (args[i] is ushort)
						{
							args[i] = ushort.Parse(text);
						}
						else
						{
							if (!(args[i] is bool))
							{
								result = false;
								return result;
							}
							args[i] = bool.Parse(text);
						}
					}
					result = true;
				}
				catch (Exception o)
				{
					this._OutputDebug(o);
					result = false;
				}
				return result;
			}

			public void Dispose()
			{
				this.Clear();
			}

			public void Clear()
			{
				this.LineType = TsDataReader.Row.TYPE.LINE_NONE;
				this.values.Clear();
			}

			private void _OutputDebug(object o)
			{
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
					if (c != ' ')
					{
						break;
					}
					i++;
				}
				if (c == '\0')
				{
					this.LineType = TsDataReader.Row.TYPE.LINE_BLANK;
					this.Values.Add(string.Empty);
					return;
				}
				if ((c == '/' && line[i + 1] == '/') || c == '#')
				{
					this.LineType = TsDataReader.Row.TYPE.LINE_COMMENT;
				}
				else if (c == '[')
				{
					this.LineType = TsDataReader.Row.TYPE.LINE_SECTION;
				}
				else if (c == '<')
				{
					this.LineType = TsDataReader.Row.TYPE.LINE_SUBSECTION;
				}
				else
				{
					this.LineType = TsDataReader.Row.TYPE.LINE_DATA;
				}
				int num2 = 0;
				int num3 = i;
				for (int j = i; j < num; j++)
				{
					num2++;
					if (seperator == line[j])
					{
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
					this.Values.Add(line.Substring(num3, num2));
				}
				if (this.LineType == TsDataReader.Row.TYPE.LINE_SECTION || this.LineType == TsDataReader.Row.TYPE.LINE_SUBSECTION)
				{
					this.Values[0] = this.Values[0].ToLower();
				}
			}
		}

		protected struct _RowEnum : IEnumerator
		{
			public TsDataReader _dr;

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			public TsDataReader.Row Current
			{
				get
				{
					TsDataReader.Row currentRow;
					try
					{
						currentRow = this._dr.GetCurrentRow();
					}
					catch (Exception var_0_16)
					{
						throw new InvalidOperationException();
					}
					return currentRow;
				}
			}

			public _RowEnum(TsDataReader dr)
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
				this._dr.MoveCurrentSectionFirst();
			}
		}

		protected List<TsDataReader.Row> m_kRowString = new List<TsDataReader.Row>();

		protected int m_nCurrentRow;

		protected int m_nCurSectionRow;

		protected static string[] LineSeperators = new string[]
		{
			"\r\n"
		};

		protected static bool ms_bOptimize = true;

		public static TsDataReader.Row EMPTY_ROW = new TsDataReader.Row();

		public static bool UseOptimize
		{
			get
			{
				return TsDataReader.ms_bOptimize;
			}
			set
			{
				TsDataReader.ms_bOptimize = value;
			}
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

		public TsDataReader()
		{
			this.ColumnSeperators = new char[]
			{
				'\t'
			};
			this.ColumnSeperator = '\t';
			this.m_nCurrentRow = -1;
			this.m_nCurSectionRow = -1;
		}

		public bool LoadFrom(string strContext)
		{
			this._clear();
			if (strContext == null)
			{
				return false;
			}
			if (TsDataReader.UseOptimize)
			{
				return this.__parse_line_optimize(strContext);
			}
			return this.__parse_line(strContext);
		}

		public bool LoadFrom(string strContext, TsDataReader.IBinding bindingObject)
		{
			this._clear();
			return strContext != null && this.LoadFrom(strContext) && bindingObject.ReadFrom(this);
		}

		public bool LoadFrom(string strContext, string strSection, TsDataReader.IBindingRow bindingObject)
		{
			this._clear();
			return strContext != null && this.__parse_line_optimize(strContext, strSection, bindingObject);
		}

		private bool __parse_line(string strContext)
		{
			string[] array = strContext.Split(TsDataReader.LineSeperators, StringSplitOptions.RemoveEmptyEntries);
			this.m_kRowString.Capacity = 2000;
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

		private bool __parse_line_optimize(string strContext, string strSection, TsDataReader.IBindingRow bindingRow)
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
							this.__parse_row_optimize(strContext, i, num, bindingRow);
						}
					}
					else
					{
						this.__parse_row_optimize(strContext, i, num, bindingRow);
					}
					i = ++j + 1;
					num = 0;
				}
				else
				{
					num++;
				}
			}
			this.__parse_row_optimize(strContext, i, num, bindingRow);
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

		public bool LoadFrom(byte[] bytes, TsDataReader.IBinding bindingObject)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.LoadFrom(memoryStream, bindingObject);
			}
			return result;
		}

		public bool LoadFrom(byte[] bytes, string strSection, TsDataReader.IBindingRow bindingObject)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = this.LoadFrom(memoryStream, strSection, bindingObject);
			}
			return result;
		}

		public bool LoadFrom(Stream stream)
		{
			this._clear();
			try
			{
				using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true))
				{
					this.m_kRowString.Capacity = 2000;
					if (TsDataReader.UseOptimize)
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
					streamReader.Close();
				}
				return true;
			}
			catch (Exception var_2_91)
			{
			}
			return false;
		}

		public bool LoadFrom(Stream stream, TsDataReader.IBinding bindingObject)
		{
			this._clear();
			try
			{
				using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true))
				{
					string strContext = streamReader.ReadToEnd();
					streamReader.Close();
					return this.LoadFrom(strContext, bindingObject);
				}
			}
			catch (Exception var_2_45)
			{
			}
			return false;
		}

		public bool LoadFrom(Stream stream, string strSection, TsDataReader.IBindingRow bindingObject)
		{
			this._clear();
			try
			{
				using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true))
				{
					string strContext = streamReader.ReadToEnd();
					streamReader.Close();
					return this.LoadFrom(strContext, strSection, bindingObject);
				}
			}
			catch (Exception var_2_46)
			{
			}
			return false;
		}

		public bool Load(string strFileName)
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
			using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					this.m_kRowString.Capacity = 2000;
					if (TsDataReader.UseOptimize)
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
					streamReader.Close();
					result = true;
				}
			}
			return result;
		}

		public bool Load(string strFileName, string strSection, TsDataReader.IBindingRow bindingObject)
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
			using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					string strContext = streamReader.ReadToEnd();
					streamReader.Close();
					result = this.LoadFrom(strContext, strSection, bindingObject);
				}
			}
			return result;
		}

		private void _clear()
		{
			foreach (TsDataReader.Row current in this.m_kRowString)
			{
				current.Clear();
			}
			this.m_kRowString.Clear();
			this.m_nCurrentRow = -1;
			this.m_nCurSectionRow = -1;
		}

		public void Dispose()
		{
			this._clear();
		}

		private bool __parse_row(string line)
		{
			if (line == null)
			{
				return false;
			}
			TsDataReader.Row row = new TsDataReader.Row();
			string[] array = line.Split(this.ColumnSeperators);
			for (int i = 0; i < array.Length; i++)
			{
				string item = array[i];
				row.Values.Add(item);
			}
			string column = row.GetColumn(0);
			if (column == null)
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_BLANK;
			}
			else if (column == string.Empty || column[0] == ' ')
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_BLANK;
			}
			else if ((column[0] == '/' && column[1] == '/') || column[0] == '#')
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_COMMENT;
			}
			else if (column[0] == '[')
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_SECTION;
				row.Values[0] = row.Values[0].ToLower();
			}
			else if (column[0] == '<')
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_SUBSECTION;
				row.Values[0] = row.Values[0].ToLower();
			}
			else
			{
				row.LineType = TsDataReader.Row.TYPE.LINE_DATA;
			}
			this.m_kRowString.Add(row);
			if (this.m_kRowString.Count == this.m_kRowString.Capacity)
			{
				this.m_kRowString.Capacity += 500;
			}
			return true;
		}

		private bool __parse_row_optimize(string line)
		{
			return this.__parse_row_optimize(line, 0, 0, null);
		}

		private bool __parse_row_optimize(string line, int nStartLine, int nLineLength, TsDataReader.IBindingRow bindingRow)
		{
			if (nLineLength == 0 && nStartLine == 0)
			{
				nLineLength = line.Length;
			}
			TsDataReader.Row row = new TsDataReader.Row();
			row.__parse_from(line, nStartLine, nLineLength, this.ColumnSeperator);
			if (bindingRow == null)
			{
				this.m_kRowString.Add(row);
				if (this.m_kRowString.Count == this.m_kRowString.Capacity)
				{
					this.m_kRowString.Capacity += 500;
				}
			}
			else if (row.LineType == TsDataReader.Row.TYPE.LINE_DATA)
			{
				bool flag = bindingRow.ReadFrom(row);
				if (!flag)
				{
				}
				return flag;
			}
			return true;
		}

		public bool BeginSection(string strSection)
		{
			int num = 0;
			foreach (TsDataReader.Row current in this.m_kRowString)
			{
				if (current.LineType == TsDataReader.Row.TYPE.LINE_SECTION && current.GetColumn(0).ToLower() == strSection.ToLower())
				{
					this.m_nCurrentRow = num + 1;
					this.m_nCurSectionRow = num;
					return true;
				}
				num++;
			}
			return false;
		}

		public bool ReadKeyData(string keyName, out int value)
		{
			value = 0;
			string s;
			return this.ReadKeyData(keyName, out s) && int.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out uint value)
		{
			value = 0u;
			string s;
			return this.ReadKeyData(keyName, out s) && uint.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out short value)
		{
			value = 0;
			string s;
			return this.ReadKeyData(keyName, out s) && short.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out ushort value)
		{
			value = 0;
			string s;
			return this.ReadKeyData(keyName, out s) && ushort.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out long value)
		{
			value = 0L;
			string s;
			return this.ReadKeyData(keyName, out s) && long.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out ulong value)
		{
			value = 0uL;
			string s;
			return this.ReadKeyData(keyName, out s) && ulong.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out float value)
		{
			value = 0f;
			string s;
			return this.ReadKeyData(keyName, out s) && float.TryParse(s, out value);
		}

		public bool ReadKeyData(string keyName, out double value)
		{
			value = 0.0;
			string empty = string.Empty;
			return this.ReadKeyData(keyName, out empty) && double.TryParse(empty, out value);
		}

		public bool ReadKeyData(string keyName, out bool value)
		{
			value = false;
			string text;
			if (this.ReadKeyData(keyName, out text))
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

		public bool ReadKeyData(string keyName, out string value)
		{
			value = string.Empty;
			try
			{
				keyName.ToLower();
				foreach (TsDataReader.Row row in this)
				{
					char[] separator = new char[]
					{
						'='
					};
					string[] array = row.GetToken(0).Split(separator, 2);
					array[0].ToLower();
					if (array[0].StartsWith(keyName))
					{
						value = array[1];
						return true;
					}
				}
			}
			catch (Exception var_4_8F)
			{
			}
			return false;
		}

		public bool ReadKeyData<T>(string sectionName, string keyName, out T output_value)
		{
			output_value = default(T);
			if (output_value == null)
			{
				output_value = (T)((object)string.Empty);
			}
			if (this.BeginSection(sectionName))
			{
				string text;
				bool flag = this.ReadKeyData(keyName, out text);
				if (flag)
				{
					if (output_value is string)
					{
						output_value = (T)((object)text);
					}
					else if (output_value is int)
					{
						output_value = (T)((object)int.Parse(text));
					}
					else if (output_value is double)
					{
						output_value = (T)((object)double.Parse(text));
					}
					else if (output_value is float)
					{
						output_value = (T)((object)float.Parse(text));
					}
					else if (output_value is uint)
					{
						output_value = (T)((object)uint.Parse(text));
					}
					else if (output_value is long)
					{
						output_value = (T)((object)long.Parse(text));
					}
					else if (output_value is ulong)
					{
						output_value = (T)((object)ulong.Parse(text));
					}
					else if (output_value is short)
					{
						output_value = (T)((object)short.Parse(text));
					}
					else
					{
						if (!(output_value is ushort))
						{
							return false;
						}
						output_value = (T)((object)ushort.Parse(text));
					}
				}
			}
			return true;
		}

		public bool ReadTo(TsDataReader.IBinding bindingObject)
		{
			return bindingObject.ReadFrom(this);
		}

		public bool ReadToCurrentRow(TsDataReader.IBindingRow bindingObject)
		{
			return bindingObject.ReadFrom(this.GetCurrentRow());
		}

		public TsDataReader.Row GetCurrentRow()
		{
			TsDataReader.Row result;
			try
			{
				if (0 <= this.m_nCurrentRow && this.m_nCurrentRow < this.m_kRowString.Count)
				{
					result = this.m_kRowString[this.m_nCurrentRow];
				}
				else
				{
					result = TsDataReader.EMPTY_ROW;
				}
			}
			catch (Exception var_0_49)
			{
				result = TsDataReader.EMPTY_ROW;
			}
			return result;
		}

		public int GetRowCountAt(string strSectionName, string strSubSectionName)
		{
			try
			{
				int num = 0;
				if (this.BeginSection(strSectionName))
				{
					while (!this.IsEndOfSection())
					{
						TsDataReader.Row currentRow = this.GetCurrentRow();
						if (currentRow.LineType == TsDataReader.Row.TYPE.LINE_DATA)
						{
							num++;
						}
						this.NextLine();
					}
					return num;
				}
			}
			catch (Exception var_2_47)
			{
			}
			return 0;
		}

		public void MoveCurrentSectionFirst()
		{
			this.m_nCurrentRow = this.m_nCurSectionRow;
		}

		public void FirstLine()
		{
			this.m_nCurrentRow = 0;
			this.m_nCurSectionRow = 0;
		}

		public void NextLine()
		{
			this.m_nCurrentRow++;
		}

		public bool IsEOF()
		{
			return this.m_kRowString.Count < this.m_nCurrentRow;
		}

		public bool IsEndOfSection()
		{
			return this.IsEOF() || (this.GetCurrentRow().LineType == TsDataReader.Row.TYPE.LINE_SECTION && this.m_nCurrentRow != this.m_nCurSectionRow && this.m_nCurSectionRow != -1);
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			TsDataReader.<GetEnumerator>c__Iterator24 <GetEnumerator>c__Iterator = new TsDataReader.<GetEnumerator>c__Iterator24();
			<GetEnumerator>c__Iterator.<>f__this = this;
			return <GetEnumerator>c__Iterator;
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
			encoding = Encoding.ASCII;
			return 0;
		}

		private void _OutputDebug(object o)
		{
		}
	}
}
