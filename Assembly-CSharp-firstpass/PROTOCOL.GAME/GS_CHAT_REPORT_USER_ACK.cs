using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAT_REPORT_USER_ACK
	{
		public int i32Result;

		public int i32TargetReportCount;

		public char[] szTargetCharName = new char[21];
	}
}
