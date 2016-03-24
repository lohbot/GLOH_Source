using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_TAKE_GETMAILALL_REQ
	{
		public byte i8MailCount;

		public long[] i64Idx = new long[5];
	}
}
