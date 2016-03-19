using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_HISTORY_LIST_REQ
	{
		public int i32Page;

		public int i32PageSize;

		public int i32MailType_Begin;

		public int i32MailType_End;

		public byte m_ui8FilterType;

		public long nFirstMailID;

		public long nLastMailID;

		public bool bNextRequest;
	}
}
