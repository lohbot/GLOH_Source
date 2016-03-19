using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	[StructLayout(LayoutKind.Sequential)]
	public class GS_DECLAREWAR_SET_TARGET_ACK
	{
		public int i32Result;

		public long i64GuildID;

		public long i64TargetGuildId;

		public char[] strEnemyGuildName = new char[11];
	}
}
