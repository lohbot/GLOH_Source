using System;
using TsLibs;

public class NrTable_BABELGUILDBOSS_DATA : NrTableBase
{
	public NrTable_BABELGUILDBOSS_DATA() : base(CDefinePath.BABEL_GUILDBOSS_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BABEL_GUILDBOSS bABEL_GUILDBOSS = new BABEL_GUILDBOSS();
			bABEL_GUILDBOSS.SetData(data);
			NrTSingleton<BabelTowerManager>.Instance.AddBabelGuildBossData(bABEL_GUILDBOSS);
		}
		return true;
	}
}
