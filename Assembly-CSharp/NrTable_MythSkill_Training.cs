using System;
using TsLibs;

public class NrTable_MythSkill_Training : NrTableBase
{
	public NrTable_MythSkill_Training() : base(CDefinePath.s_strMythSkillTrainingURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MYTHSKILL_TRAINING mYTHSKILL_TRAINING = new MYTHSKILL_TRAINING();
			mYTHSKILL_TRAINING.SetData(data);
			NrTSingleton<BattleSkill_Manager>.Instance.SetMythSkillTraining(mYTHSKILL_TRAINING);
		}
		return true;
	}
}
