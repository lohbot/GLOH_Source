using System;

namespace PROTOCOL.GAME
{
	public class MAILBOXHISTORY_INFO
	{
		public long i64MailID;

		public int i32MailType;

		public long i64DateVary_Send;

		public byte ui8IsRead;

		public long i64DateVary_Read;

		public char[] szCharName_Send = new char[21];

		public long nMoney;

		public int nItemUnique;

		public int nSolKind;
	}
}
