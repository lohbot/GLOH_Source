using System;
using System.Collections.Generic;
using TsLibs;

public class TableData_GameWebCallURLInfo : NrTableBase
{
	public static Dictionary<eGameWebCallURL, GameWebCallURLInfo> m_dicGameWebCallURLInfo = new Dictionary<eGameWebCallURL, GameWebCallURLInfo>();

	public TableData_GameWebCallURLInfo() : base(CDefinePath.GameWebCallURLData_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			GameWebCallURLInfo gameWebCallURLInfo = new GameWebCallURLInfo();
			gameWebCallURLInfo.SetData(data);
			this.AddData(gameWebCallURLInfo);
		}
		return true;
	}

	public void AddData(GameWebCallURLInfo info)
	{
		if (info == null)
		{
			return;
		}
		if (TableData_GameWebCallURLInfo.m_dicGameWebCallURLInfo == null)
		{
			return;
		}
		if (!TableData_GameWebCallURLInfo.m_dicGameWebCallURLInfo.ContainsKey(info.m_eType))
		{
			TableData_GameWebCallURLInfo.m_dicGameWebCallURLInfo.Add(info.m_eType, info);
		}
	}

	public static string GetWeCallURL(eGameWebCallURL eType)
	{
		if (TableData_GameWebCallURLInfo.m_dicGameWebCallURLInfo.ContainsKey(eType))
		{
			GameWebCallURLInfo gameWebCallURLInfo = TableData_GameWebCallURLInfo.m_dicGameWebCallURLInfo[eType];
			if (gameWebCallURLInfo != null)
			{
				return gameWebCallURLInfo.m_stWebcallUrl;
			}
		}
		return string.Empty;
	}
}
