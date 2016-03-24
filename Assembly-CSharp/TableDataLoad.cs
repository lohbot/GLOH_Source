using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using TsLibs;
using UnityEngine;

public static class TableDataLoad
{
	[DebuggerHidden]
	public static IEnumerator Load()
	{
		return new TableDataLoad.<Load>c__Iterator26();
	}

	public static bool LoadTableData(NrTableBase dataTable)
	{
		if (dataTable == null)
		{
			UnityEngine.Debug.LogError("ERROR, TableDataLoad.cs, LoadTableData(), dataTable is Null ");
			return false;
		}
		using (TsDataReader tsDataReader = new TsDataReader())
		{
			tsDataReader.UseMD5 = true;
			string text = Option.GetProtocolRootPath(Protocol.FILE);
			text = text.Substring("file:///".Length, text.Length - "file:///".Length);
			dataTable.m_strFilePath = string.Format("{0}{1}", text, dataTable.m_strFilePath);
			dataTable.m_strFilePath = dataTable.m_strFilePath.ToLower();
			if (dataTable.m_bReadImmediate)
			{
				bool flag = tsDataReader.LoadImmediate(dataTable.m_strFilePath, "[Table]", new TsDataReader.RowDataCallback(dataTable.ParseDataFromNDTImmediate));
				bool result = flag;
				return result;
			}
			if (tsDataReader.Load(dataTable.m_strFilePath))
			{
				if (tsDataReader.BeginSection("[Table]"))
				{
					dataTable.ParseDataFromNDT(tsDataReader);
					bool result = true;
					return result;
				}
			}
			else
			{
				UnityEngine.Debug.LogError("failed : " + dataTable.m_strFilePath);
			}
		}
		return false;
	}
}
