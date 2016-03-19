using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_SEND_REQ
	{
		public int nRecvType;

		public char[] szRecvCharName = new char[21];

		public long nMoney;

		public int nItemUnique;

		public int nPosType;

		public int sPosItem;

		public int Cur_ItemNum;

		public int Send_ItemNum;

		public long nFee;

		public char[] szComment = new char[257];

		public byte i8Push;

		public char[] strPushText = new char[256];
	}
}
