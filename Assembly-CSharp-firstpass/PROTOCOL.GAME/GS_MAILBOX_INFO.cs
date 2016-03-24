using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_INFO
	{
		public long i64MailID;

		public int i32MailType;

		public long i64DateVary_Send;

		public char[] szCharName_Send = new char[21];

		public long i64CharMoney;

		public long i64ItemID;

		public long i64DateVary_End;

		public int i32ItemUnique;

		public int i32ItemNum;

		public long i64SolID;

		public int i32CharKind;

		public byte i8Grade;

		public short i16Level;
	}
}
