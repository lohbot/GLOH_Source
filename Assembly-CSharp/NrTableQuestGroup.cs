using System;
using TsLibs;
using UnityEngine;

public class NrTableQuestGroup : NrTableBase
{
	public NrTableQuestGroup(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			QUEST_GROUP_INFO qUEST_GROUP_INFO = new QUEST_GROUP_INFO();
			qUEST_GROUP_INFO.SetData(data);
			if (!NrTSingleton<NkQuestManager>.Instance.AddQuestGroupInfo(qUEST_GROUP_INFO))
			{
				Debug.LogWarning("QuestGroup Error! - kData.m_i32QuestGroupUniuque = " + qUEST_GROUP_INFO.m_i32QuestGroupUniuque);
			}
		}
		return true;
	}
}
