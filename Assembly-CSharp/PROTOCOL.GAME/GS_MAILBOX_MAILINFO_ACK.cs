using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_MAILINFO_ACK
	{
		public int nResult;

		public long nIdx;

		public int i32MailType;

		public char[] szSendName = new char[21];

		public long nDate;

		public long nMoney;

		public ITEM UserItem = new ITEM();

		public char[] szMsg = new char[257];

		public long[] nParam = new long[2];

		public byte[] byrBinaryData = new byte[200];

		public long i64SolID;

		public int i32CharKind;

		public byte ui8Grade;

		public short i16Level;
	}
}
