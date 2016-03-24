using System;
using System.Collections.Generic;

public class NrGameHelpInfoManager : NrTSingleton<NrGameHelpInfoManager>
{
	private Dictionary<short, GameHelpInfo_Data> m_HelpInfoList;

	private NrGameHelpInfoManager()
	{
		this.m_HelpInfoList = new Dictionary<short, GameHelpInfo_Data>();
	}

	public void SetData(GameHelpInfo_Data info)
	{
		if (info != null)
		{
			this.m_HelpInfoList.Add(info.m_nSort, info);
		}
	}

	public GameHelpInfo_Data GetData(short index)
	{
		if (this.m_HelpInfoList.ContainsKey(index))
		{
			return this.m_HelpInfoList[index];
		}
		return null;
	}

	public GameHelpInfo_Data GetData(string ListName)
	{
		foreach (GameHelpInfo_Data current in this.m_HelpInfoList.Values)
		{
			if (string.Equals(current.m_HelpListName, ListName))
			{
				return current;
			}
		}
		return null;
	}

	public short GetCount()
	{
		return (short)this.m_HelpInfoList.Count;
	}
}
