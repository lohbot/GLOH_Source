using System;
using TsLibs;

public class NrTable_BABELTOWER_DATA : NrTableBase
{
	public NrTable_BABELTOWER_DATA() : base(CDefinePath.BABELTOWER_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BABELTOWER_DATA bABELTOWER_DATA = new BABELTOWER_DATA();
			bABELTOWER_DATA.SetData(data);
			NrTSingleton<BabelTowerManager>.Instance.AddBabelTowerData(bABELTOWER_DATA);
		}
		return true;
	}
}
