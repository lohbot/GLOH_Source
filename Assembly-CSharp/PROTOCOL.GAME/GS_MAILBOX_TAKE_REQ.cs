using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_TAKE_REQ
	{
		public long nIdx;

		public long nMoney;

		public ITEM nItem;

		public short i16IconType;

		public long i64SolID_Trade;

		public byte i8Accept;

		public int i32RecvMailType;

		public GS_MAILBOX_TAKE_REQ()
		{
			this.nItem = new ITEM();
		}
	}
}
