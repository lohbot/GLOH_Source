using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_LOAD_GET_ACK
	{
		public SOLDIER_INFO SoldierInfo = new SOLDIER_INFO();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public long[] SolSubData = new long[14];

		public ITEM[] EquipItem = new ITEM[6];

		public GS_SOLDIER_LOAD_GET_ACK()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
			for (int i = 0; i < 6; i++)
			{
				this.EquipItem[i] = new ITEM();
			}
		}
	}
}
