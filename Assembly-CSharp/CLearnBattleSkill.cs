using System;

public class CLearnBattleSkill : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.GetSolID() != 0L)
			{
				for (int j = 0; j < 6; j++)
				{
					int battleSkillUnique = soldierInfo.GetBattleSkillUnique(j);
					if (0 < battleSkillUnique)
					{
						int battleSkillLevel = soldierInfo.GetBattleSkillLevel(battleSkillUnique);
						if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(battleSkillUnique, battleSkillLevel) != null)
						{
							if (0 < battleSkillLevel)
							{
								num++;
							}
							if (base.GetParamVal() <= (long)num)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return empty;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.GetSolID() != 0L)
			{
				for (int j = 0; j < 6; j++)
				{
					int battleSkillUnique = soldierInfo.GetBattleSkillUnique(j);
					if (0 < battleSkillUnique)
					{
						int battleSkillLevel = soldierInfo.GetBattleSkillLevel(battleSkillUnique);
						if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(battleSkillUnique, battleSkillLevel) != null)
						{
							if (0 < battleSkillLevel)
							{
								num++;
							}
							if (base.GetParamVal() <= (long)num)
							{
								num = (int)base.GetParamVal();
								break;
							}
						}
					}
				}
			}
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			num,
			"count2",
			base.GetParamVal()
		});
		return empty;
	}
}
