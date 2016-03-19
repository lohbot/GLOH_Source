using System;
using TsLibs;

public class NkTableCharKindSolGradeInfo : NrTableBase
{
	public NkTableCharKindSolGradeInfo() : base(CDefinePath.CHARKIND_SOLGRADEINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<BASE_SOLGRADEINFO>(dr);
	}
}
