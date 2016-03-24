using System;
using TsLibs;

public class NrTable_ITEM_MAKERANK : NrTableBase
{
	public NrTable_ITEM_MAKERANK() : base(CDefinePath.s_strItemMakeRankURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MAKE_RANK mAKE_RANK = new MAKE_RANK();
			mAKE_RANK.SetData(data);
			mAKE_RANK.m_byRank = (byte)NrTSingleton<ITEM_SELL_Manager>.Instance.GetRankType(mAKE_RANK.stRank);
			NrTSingleton<Item_Makerank_Manager>.Instance.SetItemMakeRankData(mAKE_RANK);
		}
		return true;
	}
}
