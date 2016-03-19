using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NLibCs
{
	public class NTextManager
	{
		public enum CRLFReplaceTimeType
		{
			LOADING,
			GETTING
		}

		private static NTextManager _instance;

		public static delegateOutput _onOutputline;

		public IContextLoader _contextLoader = new ContextLoader.AtUnityResource();

		protected Dictionary<string, NTextGroup> m_TextGroups = new Dictionary<string, NTextGroup>();

		private static int m_nStartColumnindex;

		public static NTextManager Instance
		{
			get
			{
				if (NTextManager._instance == null)
				{
					NTextManager._instance = new NTextManager();
				}
				return NTextManager._instance;
			}
		}

		public static int TextKeyStartColumnIndex
		{
			get
			{
				return NTextManager.m_nStartColumnindex;
			}
			set
			{
				NTextManager.m_nStartColumnindex = value;
			}
		}

		public NTextManager.CRLFReplaceTimeType CRLFReplaceTime
		{
			get;
			set;
		}

		public int GroupCount
		{
			get
			{
				return this.m_TextGroups.Count;
			}
		}

		public int TextCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<string, NTextGroup> current in this.m_TextGroups)
				{
					NTextGroup value = current.Value;
					num += value.Count;
				}
				return num;
			}
		}

		public NTextGroup this[string groupKey]
		{
			get
			{
				return this.GetTextGroup(groupKey);
			}
		}

		public string this[string groupKey, string textKey]
		{
			get
			{
				return this.GetText(groupKey, textKey, string.Empty);
			}
		}

		public NTextManager()
		{
			NTextManager.TextKeyStartColumnIndex = 0;
			this.CRLFReplaceTime = NTextManager.CRLFReplaceTimeType.GETTING;
		}

		public NTextManager(IContextLoader contextLoader)
		{
			NTextManager.TextKeyStartColumnIndex = 0;
			this._contextLoader = contextLoader;
			this.CRLFReplaceTime = NTextManager.CRLFReplaceTimeType.GETTING;
		}

		public NTextManager(NTextManager.CRLFReplaceTimeType eType)
		{
			NTextManager.TextKeyStartColumnIndex = 0;
			this.CRLFReplaceTime = eType;
		}

		public static void _OutPutDelegate(string stroutput)
		{
			if (NTextManager._onOutputline != null)
			{
				NTextManager._onOutputline(stroutput);
			}
		}

		public bool ClearTextGroup(string strGroupKey)
		{
			try
			{
				return this.m_TextGroups.Remove(strGroupKey);
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public string GetText(string groupKey, string textKey, string srcText = "")
		{
			try
			{
				string key = groupKey.ToLower();
				NTextGroup nTextGroup = null;
				string result;
				if (this.m_TextGroups.TryGetValue(key, out nTextGroup))
				{
					result = nTextGroup.GetText(textKey);
					return result;
				}
				string text = textKey.Replace("\r", "\\r");
				text = text.Replace("\r\n", "◀┘");
				text = text.Replace("\n", "\\n");
				text = text.Replace("\t", "\\t");
				string text2 = groupKey.Replace("\r", "\\r");
				text2 = text2.Replace("\r\n", "◀┘");
				text2 = text2.Replace("\n", "\\n");
				text2 = text2.Replace("\t", "\\t");
				if (srcText == string.Empty)
				{
					result = string.Format("(noTextGroup:{0},{1})", text2, text);
					return result;
				}
				result = string.Format("(noTextGroup:{0},{1},{2})", text2, text, srcText);
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return string.Format("(Exception:{0}, {1})", groupKey, textKey);
		}

		public string GetText(string strGroupKey, int nTextKey)
		{
			return this.GetText(strGroupKey, nTextKey.ToString(), string.Empty);
		}

		public string GetText(string textOneKey)
		{
			string groupKey = string.Empty;
			string textKey = string.Empty;
			if (textOneKey == null)
			{
				return textOneKey;
			}
			if (textOneKey.Length < 3)
			{
				return textOneKey;
			}
			string text = textOneKey.Trim();
			if (text[0] == '[' && text[1] == '[')
			{
				string[] array = text.Substring(2).Replace("]]", string.Empty).Split(new char[]
				{
					':'
				});
				groupKey = array[0];
				if (array.Length > 1)
				{
					textKey = array[1];
				}
				return this.GetText(groupKey, textKey, string.Empty);
			}
			return string.Format("(keyError:{0})", textOneKey);
		}

		public NTextGroup GetTextGroup(string strGroupKey)
		{
			try
			{
				NTextGroup result;
				if (this.m_TextGroups.TryGetValue(strGroupKey.ToLower(), out result))
				{
					return result;
				}
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return null;
		}

		public bool IsTextOneKey(string keyString)
		{
			if (keyString == null)
			{
				return false;
			}
			if (keyString.Length < 3)
			{
				return false;
			}
			string text = keyString.Trim();
			return text[0] == '[' && text[1] == '[';
		}

		public bool LoadFromGroupList(string groupListPath, bool useFileNameEncryption = false, string sectinName = "[TextFiles]")
		{
			try
			{
				bool result;
				if (groupListPath == null)
				{
					result = false;
					return result;
				}
				string strFilePath = groupListPath.Substring(0, groupListPath.ToLower().IndexOf("/ndt/") + 1);
				NDataReader nDataReader = new NDataReader();
				nDataReader.UseFileNameEncryption = useFileNameEncryption;
				if (nDataReader.Load(groupListPath))
				{
					if (nDataReader.BeginSection(sectinName))
					{
						foreach (NDataReader.Row row in nDataReader)
						{
							string str = row[0].str;
							string str2 = row[1].str;
							if (!this.LoadTextGroup(str, str2, strFilePath, useFileNameEncryption))
							{
								result = false;
								return result;
							}
						}
					}
					result = true;
					return result;
				}
				result = false;
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public bool LoadFromGroupListContext(string strContext, string strFilePath, string sectinName = "[TextFiles]")
		{
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.LoadFrom(strContext))
				{
					bool result;
					if (nDataReader.BeginSection(sectinName))
					{
						foreach (NDataReader.Row row in nDataReader)
						{
							string str = row[0].str;
							string str2 = row[1].str;
							if (!this.LoadTextGroup(str, str2, strFilePath, false))
							{
								result = false;
								return result;
							}
						}
					}
					result = true;
					return result;
				}
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public bool LoadTextGroup(string strTextGroupKey, string strFileName, string strFilePath, bool useFileNameEncryption = false)
		{
			string key = strTextGroupKey.ToLower();
			try
			{
				NTextGroup nTextGroup = null;
				bool result;
				if (!this.m_TextGroups.TryGetValue(key, out nTextGroup))
				{
					if (!this.RegistTextGroup(strTextGroupKey, useFileNameEncryption))
					{
						result = false;
						return result;
					}
					this.m_TextGroups.TryGetValue(key, out nTextGroup);
				}
				result = nTextGroup.LoadFromFile(strFileName, strFilePath, "[Table]");
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public bool LoadTextGroupFromContext(string strTextGroupKey, string strContext)
		{
			string key = strTextGroupKey.ToLower();
			try
			{
				NTextGroup nTextGroup = null;
				bool result;
				if (this.m_TextGroups.TryGetValue(key, out nTextGroup))
				{
					result = nTextGroup.LoadFromContext(strContext);
					return result;
				}
				result = false;
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public void OutputdelegateConnect(delegateOutput func)
		{
			NTextManager._onOutputline = func;
		}

		public bool RegistTextGroup(string strTextGroupKey, bool useFileNameEncryption)
		{
			try
			{
				string key = strTextGroupKey.ToLower();
				bool result;
				if (!this.m_TextGroups.ContainsKey(key))
				{
					this.m_TextGroups.Add(key, new NTextGroup(this, strTextGroupKey, useFileNameEncryption));
					result = true;
					return result;
				}
				result = false;
				return result;
			}
			catch (Exception output)
			{
				this._OutputDebugLine(output);
			}
			return false;
		}

		public void Reload()
		{
			foreach (KeyValuePair<string, NTextGroup> current in this.m_TextGroups)
			{
				if (current.Value.Reload())
				{
				}
			}
		}

		public bool SetText(string strGroupKey, string strTextKey, string strText)
		{
			NTextGroup textGroup = this.GetTextGroup(strGroupKey);
			return textGroup != null && textGroup.SetText(strTextKey, strText);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendLine(":NTextManager -------------------------------");
			stringBuilder.AppendFormat(" 그룹 수: {0}", this.GroupCount);
			stringBuilder.AppendLine();
			foreach (KeyValuePair<string, NTextGroup> current in this.m_TextGroups)
			{
				NTextGroup value = current.Value;
				stringBuilder.Append(value.ToString());
			}
			stringBuilder.AppendLine("---------------------------------------------");
			stringBuilder.AppendFormat("Total Text Count : {0}", this.TextCount);
			return stringBuilder.ToString();
		}

		private void _OutputDebugLine(object output)
		{
			Debug.LogWarning(output);
		}

		public bool SetGroupText(string _GroupText, bool useFileNameEncryption)
		{
			NTextGroup nTextGroup = null;
			string key = _GroupText.ToLower();
			if (!this.m_TextGroups.TryGetValue(key, out nTextGroup))
			{
				if (!this.RegistTextGroup(_GroupText, useFileNameEncryption))
				{
					return false;
				}
				this.m_TextGroups.TryGetValue(key, out nTextGroup);
			}
			return true;
		}

		public string[] GetGroupList()
		{
			string[] array = new string[this.GroupCount];
			int num = 0;
			foreach (KeyValuePair<string, NTextGroup> current in this.m_TextGroups)
			{
				NTextGroup value = current.Value;
				array[num] = value.Name;
				num++;
			}
			return array;
		}
	}
}
