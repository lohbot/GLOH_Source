using System;

namespace PROTOCOL.GAME
{
	public class GS_CHANGE_CLASS_ACK
	{
		public int i32Result;

		public int i32CharKind;

		public long i64Money;

		public long i64SolID;

		public int[] i32SkillUnique = new int[6];

		public short[] i16SkillLevel = new short[6];
	}
}
