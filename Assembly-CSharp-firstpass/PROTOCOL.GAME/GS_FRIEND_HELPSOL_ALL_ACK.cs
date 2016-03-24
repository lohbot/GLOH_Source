using System;

namespace PROTOCOL.GAME
{
	public class GS_FRIEND_HELPSOL_ALL_ACK
	{
		public int i32Result;

		public int i32ItemUnique;

		public int i32ItemNum;

		public int i32VipItemUnique;

		public int i32VipItemNum;

		public long[] i64SolID = new long[30];

		public long i64CharDetailCount_HelpSolGiveItem;

		public long i64GiveMoney;

		public long i64TotalMoney;
	}
}
