using System;

namespace PROTOCOL.GAME
{
	public class GS_FRIEND_APPLY_ACK
	{
		public int Result;

		public short i32SrcWorldID;

		public long i64SrcPersonID;

		public int i32SrcLevel;

		public short i32TargetWorldID;

		public long i64TargetPersonID;

		public int i32TargetLevel;

		public char[] Name = new char[21];
	}
}
