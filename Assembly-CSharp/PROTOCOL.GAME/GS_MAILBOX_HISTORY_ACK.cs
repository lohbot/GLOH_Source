using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_HISTORY_ACK
	{
		public long i64MailID;

		public int Result;

		public char[] szrCharName_Send = new char[21];

		public char[] szrCharName_Recv = new char[21];

		public bool bDidISend;

		public long m_i64rCharMoney;

		public int m_i32rMailType;

		public char[] m_szrMSG = new char[257];

		public bool bHaveBinaryData;

		public ITEM UserItem = new ITEM();

		public long i64SolID;

		public int i32CharKind;

		public byte ui8Grade;

		public short i16Level;

		public byte[] byBinaryData = new byte[200];
	}
}
