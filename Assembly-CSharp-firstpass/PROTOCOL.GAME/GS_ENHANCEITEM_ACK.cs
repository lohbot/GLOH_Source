using System;

namespace PROTOCOL.GAME
{
	public class GS_ENHANCEITEM_ACK
	{
		public int Result;

		public int nItemPos;

		public int nItemType;

		public int nLastRank;

		public int nCurRank;

		public int[] i32ITEMOPTION = new int[10];

		public int[] i32ITEMUPGRADE = new int[10];

		public long LeftMoney;

		public long i64RemoveItemSolID;

		public byte i8ReduceSuccess;

		public byte i8AddSkill;

		public byte nAddSkillSuccess;

		public long i64SolID;

		public long i64ItemID;

		public byte i8ItemEnchantSuccess;

		public byte UpgradeType;
	}
}
