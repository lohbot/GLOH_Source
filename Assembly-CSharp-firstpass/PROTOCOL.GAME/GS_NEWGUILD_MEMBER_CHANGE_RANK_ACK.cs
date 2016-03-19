using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_MEMBER_CHANGE_RANK_ACK
	{
		public int i32Result = -1;

		public long i64PersonID_ChangeRank;

		public byte i8NewRank;

		public char[] strCharNameChangeRank = new char[11];

		public long i64PersonID_Swap;

		public byte ui8MemberRank_Swap;

		public char[] strCharNameSwap = new char[11];
	}
}
