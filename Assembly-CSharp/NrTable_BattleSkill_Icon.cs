using System;
using TsLibs;

public class NrTable_BattleSkill_Icon : NrTableBase
{
	public NrTable_BattleSkill_Icon() : base(CDefinePath.s_strBattleSkillIconURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLESKILL_ICON bATTLESKILL_ICON = new BATTLESKILL_ICON();
			bATTLESKILL_ICON.SetData(data);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBattleSkillIcon(bATTLESKILL_ICON);
		}
		return true;
	}
}
