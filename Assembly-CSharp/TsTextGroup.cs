using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TsLibs;
using UnityEngine;

public class TsTextGroup
{
	protected Dictionary<string, string> m_TextTable;

	protected List<string> m_LoadedFiles;

	protected string m_strName;

	public string Name
	{
		get
		{
			return this.m_strName;
		}
	}

	public int Count
	{
		get
		{
			return this.m_TextTable.Count;
		}
	}

	public int LoadedFileCount
	{
		get
		{
			return (this.m_LoadedFiles == null) ? 0 : this.m_LoadedFiles.Count;
		}
	}

	public Dictionary<string, string> Dic
	{
		get
		{
			return this.m_TextTable;
		}
	}

	public TsTextGroup(string groupName)
	{
		this.m_TextTable = new Dictionary<string, string>();
		this.m_LoadedFiles = new List<string>();
		this.m_strName = groupName;
	}

	public virtual string GetText(string strTextKey)
	{
		try
		{
			string empty = string.Empty;
			string result;
			if (this.m_TextTable.TryGetValue(strTextKey, out empty))
			{
				result = empty;
				return result;
			}
			string text = strTextKey.Replace("\r\n", "◀┘");
			text = text.Replace("\n", "◀┘");
			this._OutputDebugLine(string.Format("(notext! TextKey: \"{0}\" - (Group: {1} (texts:{2}))", text, this.m_strName, this.Count));
			result = string.Empty;
			return result;
		}
		catch (Exception output)
		{
			this._OutputDebugLine(output);
		}
		return "(Exception)";
	}

	public virtual bool SetText(string strTextKey, string strText)
	{
		try
		{
			if (this.m_TextTable.ContainsKey(strTextKey))
			{
				this.m_TextTable.Remove(strTextKey);
				this._OutputDebugLine(string.Format("##TsTextManager.SetText: 지우고 다시Set! - {0} : {1}", this.m_strName, strTextKey));
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

	public bool LoadFromFile(string filename)
	{
		bool result = false;
		try
		{
			bool flag = false;
			foreach (string current in this.m_LoadedFiles)
			{
				if (current == filename)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.m_LoadedFiles.Add(filename);
			}
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
				{
					string context = streamReader.ReadToEnd();
					result = this.LoadFromContext(context);
				}
			}
		}
		catch (Exception output)
		{
			this._OutputDebugLine(output);
		}
		return result;
	}

	public bool LoadFromContext(string context)
	{
		try
		{
			bool result;
			using (TsDataReader tsDataReader = new TsDataReader())
			{
				if (tsDataReader.LoadFrom(context) && tsDataReader.BeginSection("[data]"))
				{
					foreach (TsDataReader.Row row in tsDataReader)
					{
						this.SetText(row.GetColumn(0), row.GetColumn(1));
					}
					result = true;
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

	public bool Reload()
	{
		try
		{
			this.m_TextTable.Clear();
			foreach (string current in this.m_LoadedFiles)
			{
				if (!this.LoadFromFile(current))
				{
				}
			}
			return true;
		}
		catch (Exception output)
		{
			this._OutputDebugLine(output);
		}
		return false;
	}

	public bool Clear()
	{
		try
		{
			this.m_TextTable.Clear();
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

	private void _OutputDebugLine(object output)
	{
		Debug.LogWarning(output);
	}
}
