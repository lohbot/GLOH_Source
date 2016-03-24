using System;
using TsLibs;

public class NrTable_BattleSkill_DetailInclude : NrTableBase
{
	public NrTable_BattleSkill_DetailInclude() : base(CDefinePath.s_strBattleSkillDetailIncludeURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLESKILL_DETAILINCLUDE bATTLESKILL_DETAILINCLUDE = new BATTLESKILL_DETAILINCLUDE();
			bATTLESKILL_DETAILINCLUDE.SetData(data);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillDetailInclude(bATTLESKILL_DETAILINCLUDE);
		}
		return true;
	}
}
