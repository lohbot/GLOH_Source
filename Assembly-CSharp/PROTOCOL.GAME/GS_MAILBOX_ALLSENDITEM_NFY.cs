using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_ALLSENDITEM_NFY
	{
		public int i32result;

		public byte i8MailReawrdType;

		public long i64Maxmoney;

		public long i64AddMoney;

		public ITEM item = new ITEM();

		public int i32AddItemNum;

		public SOLDIER_INFO SoldierInfo = new SOLDIER_INFO();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public long[] SolSubData = new long[16];

		public GS_MAILBOX_ALLSENDITEM_NFY()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
		}
	}
}
