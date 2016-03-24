using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTH_EVOLUTION_SOL_ACK
	{
		public int i32Result;

		public long i64SolID;

		public byte i8Grade;

		public int[] i32BattleSkillUnique = new int[2];

		public int[] i32BattleSkillLevel = new int[2];

		public int[] i32BattleSkillIndex = new int[2];

		public long[] i64Delete_ItemID = new long[2];

		public int[] i32Delete_ItemUnique = new int[2];

		public int[] i32Delete_ItemNum = new int[2];
	}
}
