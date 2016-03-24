using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_BOSS_JOININFO_NFY
	{
		public int i32Result;

		public long i64PersonID;

		public long i64GuildID;

		public short[] i16Floor = new short[16];

		public long[] i64PersonContribute = new long[16];

		public byte[] ui8IsClear = new byte[16];
	}
}
