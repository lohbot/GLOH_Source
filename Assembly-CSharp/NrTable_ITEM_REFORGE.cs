using System;
using TsLibs;

public class NrTable_ITEM_REFORGE : NrTableBase
{
	public NrTable_ITEM_REFORGE() : base(CDefinePath.s_strItemReforgeURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_REFORGE iTEM_REFORGE = new ITEM_REFORGE();
			iTEM_REFORGE.SetData(data);
			iTEM_REFORGE.nItemMakeRank = NrTSingleton<ITEM_REFORGE_Manager>.Instance.GetRankType(iTEM_REFORGE.stRank);
			NrTSingleton<ITEM_REFORGE_Manager>.Instance.SetItemReforgeData(iTEM_REFORGE);
		}
		return true;
	}
}
