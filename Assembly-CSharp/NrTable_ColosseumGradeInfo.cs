using System;
using TsLibs;

public class NrTable_ColosseumGradeInfo : NrTableBase
{
	public NrTable_ColosseumGradeInfo() : base(CDefinePath.s_strColosseumGradeInfoURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			COLOSSEUM_GRADEINFO cOLOSSEUM_GRADEINFO = new COLOSSEUM_GRADEINFO();
			cOLOSSEUM_GRADEINFO.SetData(data);
			NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.AddGradeInfo(cOLOSSEUM_GRADEINFO);
		}
		return true;
	}
}
