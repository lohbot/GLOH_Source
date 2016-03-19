using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NLibCs
{
	public class NTextGroup
	{
		protected List<string> m_LoadedFiles = new List<string>();

		protected NTextManager m_owner;

		protected string m_strName = string.Empty;

		protected Dictionary<string, string> m_TextTable = new Dictionary<string, string>();

		protected string m_strFilePath = string.Empty;

		protected bool m_UseFileNameEncryption;

		private StringBuilder m_sbWarning = new StringBuilder(1024);

		public int Count
		{
			get
			{
				return this.m_TextTable.Count;
			}
		}

		public Dictionary<string, string> Dic
		{
			get
			{
				return this.m_TextTable;
			}
		}

		public int LoadedFileCount
		{
			get
			{
				return (this.m_LoadedFiles == null) ? 0 : this.m_LoadedFiles.Count;
			}
		}

		public string Name
		{
			get
			{
				return this.m_strName;
			}
		}

		public StringBuilder Warning
		{
			get
			{
				return this.m_sbWarning;
			}
		}

		public string this[string textKey]
		{
			get
			{
				return this.GetText(textKey);
			}
		}

		public NTextGroup(NTextManager owner, string groupName)
		{
			this.m_owner = owner;
			this.m_strName = groupName;
		}

		public NTextGroup(NTextManager owner, string groupName, bool useFileNameEncryption)
		{
			this.m_owner = owner;
			this.m_strName = groupName;
			this.m_UseFileNameEncryption = useFileNameEncryption;
		}

		public virtual string GetText(string strTextKey)
		{
			return this.GetText(strTextKey, string.Empty);
		}

		public virtual string GetText(string strTextKey, string srcText)
		{
			try
			{
				string text = string.Empty;
				string result;
				if (this.m_TextTable.TryGetValue(strTextKey, out text))
				{
					if (this.m_owner.CRLFReplaceTime == NTextManager.CRLFReplaceTimeType.GETTING)
					{
						text = text.Replace("{\\r\\n}", "\r\n").Replace("{\\t}", "\\t");
					}
					result = text;
					return result;
				}
				string text2 = strTextKey.Replace("\r", "\\r");
				text2 = text2.Replace("\r\n", "◀┘");
				text2 = text2.Replace("\n", "\\n");
				text2 = text2.Replace("\t", "\\t");
				string text3 = this.m_strName;
				text3 = text3.Replace("\r", "\\r");
				text3 = text3.Replace("\r\n", "◀┘");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("\t", "\\t");
				if (NTextManager._onOutputline != null)
				{
					string msg = string.Format("(notext! TextKey: \"{0}\" - (Group: {1} (texts:{2}))", text2, text3, this.Count);
					NTextManager._onOutputline(msg);
				}
				this._OutputDebugLine(string.Format("(notext! TextKey: \"{0}\" - (Group: {1} ({2}))", text2, text3, srcText));
				result = string.Empty;
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return "(Exception)";
		}

		public bool LoadFromContext(string context)
		{
			return this.LoadFromContext(context, "[Table]");
		}

		public bool LoadFromContext(string context, string sectionName)
		{
			bool bAutoReplace = this.m_owner.CRLFReplaceTime == NTextManager.CRLFReplaceTimeType.LOADING;
			try
			{
				NDataReader nDataReader = NDataReader.FromContext(context);
				NDataSection nDataSection = nDataReader[sectionName];
				foreach (NDataReader.Row row in nDataSection)
				{
					int textKeyStartColumnIndex = NTextManager.TextKeyStartColumnIndex;
					string column = row.GetColumn(textKeyStartColumnIndex, bAutoReplace);
					string column2 = row.GetColumn(textKeyStartColumnIndex + 1, bAutoReplace);
					this.SetText(column, column2);
				}
				return true;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public bool LoadFromFile(string path, string _filepath, string sectionName = "[Table]")
		{
			bool result = false;
			try
			{
				bool flag = false;
				foreach (string current in this.m_LoadedFiles)
				{
					if (current == path)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.m_LoadedFiles.Add(path);
				}
				this.m_strFilePath = _filepath;
				path = string.Format("{0}{1}", _filepath, path);
				result = this.LoadFromDataReader(path, sectionName);
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return result;
		}

		public bool LoadFromDataReader(string path, string sectionName)
		{
			bool bAutoReplace = this.m_owner.CRLFReplaceTime == NTextManager.CRLFReplaceTimeType.LOADING;
			try
			{
				NDataReader nDataReader = new NDataReader();
				nDataReader.UseFileNameEncryption = this.m_UseFileNameEncryption;
				nDataReader.Load(path);
				NDataSection nDataSection = nDataReader[sectionName];
				foreach (NDataReader.Row row in nDataSection)
				{
					int textKeyStartColumnIndex = NTextManager.TextKeyStartColumnIndex;
					string column = row.GetColumn(textKeyStartColumnIndex, bAutoReplace);
					string column2 = row.GetColumn(textKeyStartColumnIndex + 1, bAutoReplace);
					this.SetText(column, column2);
				}
				return true;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public bool Reload()
		{
			try
			{
				this.m_TextTable.Clear();
				bool result;
				foreach (string current in this.m_LoadedFiles)
				{
					if (!this.LoadFromFile(current, this.m_strFilePath, "[Table]"))
					{
						result = false;
						return result;
					}
				}
				result = true;
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public virtual bool SetText(string strTextKey, string strText)
		{
			try
			{
				if (this.m_TextTable.ContainsKey(strTextKey))
				{
					this.m_TextTable.Remove(strTextKey);
				}
				this.m_TextTable.Add(strTextKey, strText);
				return true;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format(" {0,12} - Text수:{1,4} 로딩파일수:{2,4}{3}", new object[]
			{
				this.Name,
				this.Count,
				this.LoadedFileCount,
				Environment.NewLine
			});
		}

		private void _OutDebugLineGUI(string strout)
		{
			NTextManager._OutPutDelegate(strout);
		}

		private void _OutputDebugLine(object output)
		{
			Debug.LogWarning(output);
		}

		public string GetTextAll()
		{
			StringBuilder stringBuilder = new StringBuilder(10240);
			stringBuilder.AppendLine("---------------------------------------------");
			foreach (KeyValuePair<string, string> current in this.m_TextTable)
			{
				stringBuilder.AppendFormat("{0}\t\t{1}", current.Key, current.Value);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("---------------------------------------------");
			return stringBuilder.ToString();
		}

		public string[] GetKeyList()
		{
			string[] array = new string[this.Count];
			int num = 0;
			foreach (KeyValuePair<string, string> current in this.m_TextTable)
			{
				array[num] = current.Key;
				num++;
			}
			return array;
		}
	}
}
