using System;
using TsLibs;

public class NrTableDailyGift : NrTableBase
{
	public NrTableDailyGift() : base(CDefinePath.DAILY_GIFT_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			DAILY_GIFT dAILY_GIFT = new DAILY_GIFT();
			dAILY_GIFT.SetData(data);
			NrTSingleton<NrDailyGiftManager>.Instance.AddData(dAILY_GIFT);
		}
		return true;
	}
}
