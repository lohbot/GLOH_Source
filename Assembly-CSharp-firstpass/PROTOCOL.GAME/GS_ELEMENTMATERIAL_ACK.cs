using System;

namespace PROTOCOL.GAME
{
	public class GS_ELEMENTMATERIAL_ACK
	{
		public int i32CharKind;

		public byte i8Grade;

		public int[] i32MaterialCharKind = new int[5];

		public byte[] i8MaterialGrade = new byte[5];

		public long i64Money;

		public byte i8LegendGrade;

		public int[] i32LegendMaterialCharKind = new int[5];

		public byte[] i8LegendMaterialGrade = new byte[5];

		public long i64LegendMoney;

		public int i32LegendItemUnique;

		public int i32LegendNeedEssence;

		public int i32Result;
	}
}
