using System;

namespace PROTOCOL.GAME
{
	public class BATTLESKILL_DATA
	{
		public int BattleSkillUnique;

		public int BattleSkillLevel;

		public void Set(ref BATTLESKILL_DATA pkSkillData)
		{
			this.BattleSkillUnique = pkSkillData.BattleSkillUnique;
			this.BattleSkillLevel = pkSkillData.BattleSkillLevel;
		}

		public void Init()
		{
			this.BattleSkillUnique = 0;
			this.BattleSkillLevel = 0;
		}
	}
}
