using System;
using TsLibs;

public class NrTable_ItemMall_PoPupShop : NrTableBase
{
	public NrTable_ItemMall_PoPupShop() : base(CDefinePath.s_strPoPupShop_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEMMALL_POPUPSHOP iTEMMALL_POPUPSHOP = new ITEMMALL_POPUPSHOP();
			iTEMMALL_POPUPSHOP.SetData(data);
			NrTSingleton<ItemMallPoPupShopManager>.Instance.Set_Value(iTEMMALL_POPUPSHOP);
		}
		return true;
	}
}
