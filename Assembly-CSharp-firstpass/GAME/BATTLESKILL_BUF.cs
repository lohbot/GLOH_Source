using System;

namespace GAME
{
	public struct BATTLESKILL_BUF
	{
		public int BSkillBufSkillUnique;

		public int BSkillBufLevel;

		public int BSkillBufLastKeepTurn;

		public int BSkillBufActionTurn;

		public bool BSkillDeBuff;

		public uint BSkillBufEffectCode;

		public int BSkillBufAddUseAngerlyPoint;

		public void init()
		{
			this.BSkillBufSkillUnique = 0;
			this.BSkillBufLevel = 0;
			this.BSkillBufLastKeepTurn = 0;
			this.BSkillBufActionTurn = 0;
			this.BSkillDeBuff = false;
			this.BSkillBufEffectCode = 0u;
			this.BSkillBufAddUseAngerlyPoint = 0;
		}
	}
}
