using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_BATTLE_RESULT_REWARD_GET_REQ
	{
		public long i64MailID;

		public long i64LegionActionID;

		public int i32ItemUnique;

		public int i32ItemNum;

		public long[] i64SolID = new long[15];
	}
}
