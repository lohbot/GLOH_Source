using System;
using TsLibs;

public class NrTable_AgitMerchant : NrTableBase
{
	public NrTable_AgitMerchant() : base(CDefinePath.AGIT_MERCHANT_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<AgitMerchantData>(dr);
	}
}
