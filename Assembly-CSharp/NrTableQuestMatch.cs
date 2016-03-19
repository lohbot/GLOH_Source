using System;
using TsLibs;
using UnityEngine;

public class NrTableQuestMatch : NrTableBase
{
	public NrTableQuestMatch(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NPC_QUEST_MATCH nPC_QUEST_MATCH = new NPC_QUEST_MATCH();
			nPC_QUEST_MATCH.SetData(data);
			nPC_QUEST_MATCH.NNPCID = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(nPC_QUEST_MATCH.CharCode);
			if (!NrTSingleton<NkQuestManager>.Instance.AddNpcQuestMatchTable(nPC_QUEST_MATCH))
			{
				Debug.LogError("XML Parsing Error! - " + this.m_strFilePath);
			}
		}
		return true;
	}
}
