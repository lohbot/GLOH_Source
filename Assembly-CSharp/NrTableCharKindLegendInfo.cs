using System;
using TsLibs;

public class NrTableCharKindLegendInfo : NrTableBase
{
	public NrTableCharKindLegendInfo() : base(CDefinePath.CHARKIND_LEGENDINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			CHARKIND_LEGENDINFO cHARKIND_LEGENDINFO = new CHARKIND_LEGENDINFO();
			cHARKIND_LEGENDINFO.SetData(data);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(cHARKIND_LEGENDINFO);
		}
		return true;
	}
}
