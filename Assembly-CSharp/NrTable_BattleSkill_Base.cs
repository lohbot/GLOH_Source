using System;
using TsLibs;

public class NrTable_BattleSkill_Base : NrTableBase
{
	public NrTable_BattleSkill_Base() : base(CDefinePath.s_strBattleSkillBaseURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLESKILL_BASE bATTLESKILL_BASE = new BATTLESKILL_BASE();
			bATTLESKILL_BASE.SetData(data);
			bATTLESKILL_BASE.m_nSkilNeedWeapon = NrTSingleton<BattleSkill_Manager>.Instance.GetNeedWeaponType(bATTLESKILL_BASE.m_strParserWeaphoneType);
			bATTLESKILL_BASE.m_nSkillAniSequenceCode = (int)NrTSingleton<BattleSkill_Manager>.Instance.GetCharAniType(bATTLESKILL_BASE.m_strParserCharAniType);
			bATTLESKILL_BASE.m_nSkillBuffType = (int)NrTSingleton<BattleSkill_Manager>.Instance.GetBuffType(bATTLESKILL_BASE.m_strParserBuffType);
			bATTLESKILL_BASE.m_nSkillAiType = (int)NrTSingleton<BattleSkill_Manager>.Instance.GetAiType(bATTLESKILL_BASE.m_strParserSkillAniType);
			bATTLESKILL_BASE.m_nSkillTargetWeaponType = NrTSingleton<NkATB_Manager>.Instance.ParseTargetWeaponTypeATB(bATTLESKILL_BASE.m_strParserTargetWeaponType);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillBase(bATTLESKILL_BASE);
		}
		return true;
	}
}
