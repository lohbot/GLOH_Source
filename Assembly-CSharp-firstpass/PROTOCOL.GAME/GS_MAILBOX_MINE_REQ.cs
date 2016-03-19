using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_MINE_REQ
	{
		public int i32MailType_Begin;

		public int i32MailType_End;

		public int i32Page;

		public int i32PageSize;

		public long nFirstMailID;

		public long nLastMailID;

		public bool bNextRequest;
	}
}
