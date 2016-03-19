using System;
using TsLibs;
using UnityEngine;

public class NrTableQuestDropItem : NrTableBase
{
	public NrTableQuestDropItem(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			QUEST_DROP_ITEM qUEST_DROP_ITEM = new QUEST_DROP_ITEM();
			qUEST_DROP_ITEM.SetData(data);
			if (!NrTSingleton<NkQuestManager>.Instance.AddQuestDropItem(qUEST_DROP_ITEM))
			{
				Debug.LogError("XML Parsing Error! - " + this.m_strFilePath);
			}
		}
		return true;
	}
}
