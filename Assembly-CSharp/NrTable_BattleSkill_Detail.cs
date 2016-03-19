using System;
using TsLibs;

public class NrTable_BattleSkill_Detail : NrTableBase
{
	public NrTable_BattleSkill_Detail() : base(CDefinePath.s_strBattleSkillDetailURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLESKILL_DETAIL bATTLESKILL_DETAIL = new BATTLESKILL_DETAIL();
			bATTLESKILL_DETAIL.SetData(data);
			bATTLESKILL_DETAIL.m_nSkillBuffTarget = (int)NrTSingleton<BattleSkill_Manager>.Instance.GetBuffType(bATTLESKILL_DETAIL.m_strParserBuffType);
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(bATTLESKILL_DETAIL.m_nSkillUnique);
			if (battleSkillBase != null)
			{
				if (battleSkillBase.m_nSkillUseDetailInclude == 1)
				{
					if (bATTLESKILL_DETAIL.m_nSkillLevel == 1)
					{
						bATTLESKILL_DETAIL.SetSkillDetailATB();
						NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillDetail(bATTLESKILL_DETAIL);
						BATTLESKILL_DETAILINCLUDE bATTLESKILL_DETAILINCLUDE = null;
						for (int i = 1; i < 101; i++)
						{
							BATTLESKILL_DETAILINCLUDE battleSkillDetailIncludeData = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetailIncludeData(bATTLESKILL_DETAIL.m_nSkillUnique, i);
							if (battleSkillDetailIncludeData != null)
							{
								bATTLESKILL_DETAILINCLUDE = battleSkillDetailIncludeData;
							}
							if (bATTLESKILL_DETAILINCLUDE != null)
							{
								if (i != 1)
								{
									BATTLESKILL_DETAIL detailDataNext = new BATTLESKILL_DETAIL();
									if (!NrTSingleton<BattleSkill_Manager>.Instance.AddMakeBattleSkillDetail(i, bATTLESKILL_DETAILINCLUDE, detailDataNext))
									{
									}
								}
							}
						}
					}
				}
				else
				{
					bATTLESKILL_DETAIL.SetSkillDetailATB();
					NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillDetail(bATTLESKILL_DETAIL);
				}
			}
		}
		return true;
	}
}
