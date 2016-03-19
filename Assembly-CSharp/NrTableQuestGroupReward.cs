using System;
using TsLibs;

public class NrTableQuestGroupReward : NrTableBase
{
	public NrTableQuestGroupReward(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			QUEST_GROUP_REWARD_TEMP qUEST_GROUP_REWARD_TEMP = new QUEST_GROUP_REWARD_TEMP();
			qUEST_GROUP_REWARD_TEMP.SetData(data);
			if (!NrTSingleton<NkQuestManager>.Instance.AddQuestGroupReward(qUEST_GROUP_REWARD_TEMP))
			{
				string msg = "QUEST_GROUP_REWARD_TEMP! - kData.i32GroupUnique = " + qUEST_GROUP_REWARD_TEMP.i32GroupUnique;
				NrTSingleton<NrMainSystem>.Instance.Alert(msg);
			}
		}
		return true;
	}
}
