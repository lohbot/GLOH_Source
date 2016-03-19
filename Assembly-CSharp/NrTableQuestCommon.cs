using System;
using TsLibs;

public class NrTableQuestCommon : NrTableBase
{
	public NrTableQuestCommon(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			QUEST_COMMON qUEST_COMMON = new QUEST_COMMON();
			qUEST_COMMON.SetData(data);
			NrTSingleton<NkQuestManager>.Instance.AddQuest(qUEST_COMMON);
		}
		return true;
	}
}
