using System;
using TsLibs;

public class NrTableQuestReward : NrTableBase
{
	public NrTableQuestReward(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			QEUST_REWARD_ITEM qEUST_REWARD_ITEM = new QEUST_REWARD_ITEM();
			qEUST_REWARD_ITEM.SetData(data);
			if (!NrTSingleton<NkQuestManager>.Instance.AddQuestReward(qEUST_REWARD_ITEM))
			{
				string msg = "QEUST_REWARD_ITEM! - kData.strQuestUnique = " + qEUST_REWARD_ITEM.strQuestUnique;
				NrTSingleton<NrMainSystem>.Instance.Alert(msg);
			}
		}
		return true;
	}
}
