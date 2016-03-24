using System;
using TsLibs;

public class NrTableCharKindGuideInfo : NrTableBase
{
	public NrTableCharKindGuideInfo() : base(CDefinePath.CHARKIND_SOLINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			CHARKIND_SOLDIERINFO cHARKIND_SOLDIERINFO = new CHARKIND_SOLDIERINFO();
			cHARKIND_SOLDIERINFO.SetData(data);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(cHARKIND_SOLDIERINFO);
		}
		return true;
	}
}
