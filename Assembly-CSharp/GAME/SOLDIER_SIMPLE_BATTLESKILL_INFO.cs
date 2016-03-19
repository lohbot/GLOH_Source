using PROTOCOL.GAME;
using System;

namespace GAME
{
	public class SOLDIER_SIMPLE_BATTLESKILL_INFO
	{
		public long SolID;

		public byte SkillIndex;

		public BATTLESKILL_DATA BattleSkillData = new BATTLESKILL_DATA();
	}
}
