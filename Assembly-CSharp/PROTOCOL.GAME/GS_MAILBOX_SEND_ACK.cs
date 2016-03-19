using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_SEND_ACK
	{
		public int nResult;

		public long nMoney;

		public int nPosType;

		public int nItemPos;

		public ITEM nItem;

		public char[] szRecvName = new char[21];

		public GS_MAILBOX_SEND_ACK()
		{
			this.nItem = new ITEM();
		}
	}
}
