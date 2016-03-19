using System;

namespace PROTOCOL.GAME
{
	public class GS_OTHERCHAR_INFO_PERMIT_ACK
	{
		public int Result;

		public long i64TargetPersonID;

		public char[] Name = new char[21];

		public char[] szIntroMsg = new char[41];
	}
}
