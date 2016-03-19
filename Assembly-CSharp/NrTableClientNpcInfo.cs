using System;
using TsLibs;
using UnityEngine;

public class NrTableClientNpcInfo : NrTableBase
{
	public NrTableClientNpcInfo(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NrClientNpcInfo nrClientNpcInfo = new NrClientNpcInfo();
			nrClientNpcInfo.SetData(data);
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(nrClientNpcInfo.kStartCon.strQuestUnique) == null)
			{
				string msg = string.Concat(new string[]
				{
					"캐릭터 코드 : ",
					nrClientNpcInfo.strCharCode,
					" 퀘스트",
					nrClientNpcInfo.kStartCon.strQuestUnique,
					"클라이언트 npc 시작 퀘스트 정보가 없습니다"
				});
				NrTSingleton<NrMainSystem>.Instance.Alert(msg);
			}
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(nrClientNpcInfo.kEndCon.strQuestUnique) == null)
			{
				string msg2 = string.Concat(new string[]
				{
					"캐릭터 코드 : ",
					nrClientNpcInfo.strCharCode,
					" 퀘스트",
					nrClientNpcInfo.kEndCon.strQuestUnique,
					"클라이언트 npc 마지막 퀘스트 정보가 없습니다"
				});
				NrTSingleton<NrMainSystem>.Instance.Alert(msg2);
			}
			NrTSingleton<NkQuestManager>.Instance.AddQuestAutoPath(nrClientNpcInfo);
			if (!NrTSingleton<NkQuestManager>.Instance.AddQuestNpcPos(nrClientNpcInfo))
			{
				Debug.LogError("XML Parsing Error! - " + this.m_strFilePath);
			}
		}
		return true;
	}
}
