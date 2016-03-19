using System;
using TsLibs;

public class NrTable_BattleSkill_TrainingInclude : NrTableBase
{
	public NrTable_BattleSkill_TrainingInclude() : base(CDefinePath.s_strBattleSkillTrainingIncludeURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_SKILL_TRAININGINCLUDE bATTLE_SKILL_TRAININGINCLUDE = new BATTLE_SKILL_TRAININGINCLUDE();
			bATTLE_SKILL_TRAININGINCLUDE.SetData(data);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillTrainingInclude(bATTLE_SKILL_TRAININGINCLUDE);
		}
		return true;
	}
}
