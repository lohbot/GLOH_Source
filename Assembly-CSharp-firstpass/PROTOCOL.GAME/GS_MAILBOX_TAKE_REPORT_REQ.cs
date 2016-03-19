using System;

namespace PROTOCOL.GAME
{
	public class GS_MAILBOX_TAKE_REPORT_REQ
	{
		public byte ui8TakeReportType;

		public long i64MailID;

		public long i64LegionActionID;

		public int[] i32ItemUnique = new int[5];

		public int[] i32ItemNum = new int[5];

		public long[] i64SolID = new long[15];
	}
}
