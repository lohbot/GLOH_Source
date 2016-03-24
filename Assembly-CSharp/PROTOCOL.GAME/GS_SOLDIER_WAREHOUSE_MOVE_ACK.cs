using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_WAREHOUSE_MOVE_ACK
	{
		public int i32Result;

		public byte ui8SolPosType;

		public SOLDIER_INFO SoldierInfo = new SOLDIER_INFO();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public long[] SolSubData = new long[16];

		public ITEM[] EquipItem = new ITEM[6];

		public GS_SOLDIER_WAREHOUSE_MOVE_ACK()
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
