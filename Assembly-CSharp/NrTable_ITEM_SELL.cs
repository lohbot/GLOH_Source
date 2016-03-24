using System;
using TsLibs;

public class NrTable_ITEM_SELL : NrTableBase
{
	public NrTable_ITEM_SELL() : base(CDefinePath.s_strItemSellURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_SELL iTEM_SELL = new ITEM_SELL();
			iTEM_SELL.SetData(data);
			iTEM_SELL.nItemMakeRank = NrTSingleton<ITEM_SELL_Manager>.Instance.GetRankType(iTEM_SELL.stRank);
			NrTSingleton<ITEM_SELL_Manager>.Instance.SetItemSellData(iTEM_SELL);
		}
		return true;
	}
}
