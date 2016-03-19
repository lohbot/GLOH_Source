using System;
using TsLibs;

public class NkTableCharKindClassInfo : NrTableBase
{
	public NkTableCharKindClassInfo() : base(CDefinePath.CHARKIND_CLASSINFO_URL, true)
	{
		NrTSingleton<NrCharKindInfoManager>.Instance.SetClassTypeCodeInfo("CLS_ALL", 9223372036854775807L);
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<CHARKIND_CLASSINFO>(dr);
	}
}
