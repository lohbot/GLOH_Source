using System;
using TsLibs;

public class NrTable_BattleSkill_Training : NrTableBase
{
	public NrTable_BattleSkill_Training() : base(CDefinePath.s_strBattleSkillTrainingURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLESKILL_TRAINING bATTLESKILL_TRAINING = new BATTLESKILL_TRAINING();
			bATTLESKILL_TRAINING.SetData(data);
			if (bATTLESKILL_TRAINING.m_nSkillLevel == 1)
			{
				NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillTraining(bATTLESKILL_TRAINING);
				BATTLE_SKILL_TRAININGINCLUDE bATTLE_SKILL_TRAININGINCLUDE = null;
				for (int i = 1; i < 101; i++)
				{
					BATTLE_SKILL_TRAININGINCLUDE battleSkillTrainingIncludeData = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTrainingIncludeData(bATTLESKILL_TRAINING.m_nSkillUnique, i);
					if (battleSkillTrainingIncludeData != null)
					{
						bATTLE_SKILL_TRAININGINCLUDE = battleSkillTrainingIncludeData;
					}
					if (bATTLE_SKILL_TRAININGINCLUDE != null)
					{
						if (i != 1)
						{
							BATTLESKILL_TRAINING trainingDataNext = new BATTLESKILL_TRAINING();
							if (!NrTSingleton<BattleSkill_Manager>.Instance.AddMakeBattleSkillTraining(i, bATTLE_SKILL_TRAININGINCLUDE, trainingDataNext))
							{
							}
						}
					}
				}
			}
		}
		return true;
	}
}
