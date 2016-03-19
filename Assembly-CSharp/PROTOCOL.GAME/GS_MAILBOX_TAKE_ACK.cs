using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_TAKE_ACK
	{
		public int result;

		public long money;

		public int PosType;

		public int ItemPos;

		public ITEM nItem = new ITEM();

		public long nIdx;

		public SOLDIER_INFO SoldierInfo = new SOLDIER_INFO();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public long[] SolSubData = new long[14];

		public GS_MAILBOX_TAKE_ACK()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
		}
	}
}
