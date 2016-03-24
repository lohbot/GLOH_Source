using System;
using TsLibs;

public class NrTableSolCombinationSkill : NrTableBase
{
	public NrTableSolCombinationSkill(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		int num = 0;
		foreach (TsDataReader.Row dataRow in dr)
		{
			NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.SetData(num, dataRow);
			num++;
		}
		return true;
	}
}
