using PROTOCOL.GAME;
using System;

namespace GAME
{
	public class SOLDIER_BATTLESKILL_INFO
	{
		public long SolID;

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public SOLDIER_BATTLESKILL_INFO()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
			this.Init();
		}

		public void Init()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i].Init();
			}
		}

		public void Set(SOLDIER_BATTLESKILL_INFO pkBattleSkill)
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i].Set(ref pkBattleSkill.BattleSkillData[i]);
			}
		}
	}
}
