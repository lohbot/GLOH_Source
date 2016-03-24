using System;
using TsLibs;

public class NrTable_Item_Mall_Item : NrTableBase
{
	public NrTable_Item_Mall_Item() : base(CDefinePath.s_strItemMallURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_MALL_ITEM iTEM_MALL_ITEM = new ITEM_MALL_ITEM();
			iTEM_MALL_ITEM.SetData(data);
			NrTSingleton<ItemMallItemManager>.Instance.Set_Value(iTEM_MALL_ITEM, -1);
		}
		return true;
	}
}
