using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIER_REINCARNATION_SET_ACK
	{
		public int i32Result = -1;

		public long i64PersonID;

		public long i64SolID;

		public int[] i32SkillUnique = new int[6];

		public short[] i16SkillLevel = new short[6];

		public long i64CurMoney;

		public long i64UseMoney;

		public int i32CharSubDataType;

		public long i64CharSubDataValue;

		public byte i8Grade;

		public short i16Level;

		public long i64Exp;

		public int i32SolSubDataType1;

		public long i64SolSubDataValue1;

		public int i32CharKind;

		public byte i8ResultEffect;
	}
}
