using GAME;
using System;

namespace PROTOCOL
{
	public class GS_ITEMMALL_BOX_TRADE_ACK
	{
		public int i32Result;

		public long i64ItemMallIndex;

		public int i32BuyCount;

		public int i32Unique;

		public int i32PosType;

		public int i32PosItem;

		public int i32ItemNum;

		public int[] i32arrAddItemNum = new int[12];

		public ITEM[] pAddItem = new ITEM[12];

		public GS_ITEMMALL_BOX_TRADE_ACK()
		{
			for (int i = 0; i < 12; i++)
			{
				this.pAddItem[i] = new ITEM();
			}
		}
	}
}
