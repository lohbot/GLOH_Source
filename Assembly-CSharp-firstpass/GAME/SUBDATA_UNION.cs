using System;
using System.Runtime.InteropServices;

namespace GAME
{
	[StructLayout(LayoutKind.Explicit)]
	public struct SUBDATA_UNION
	{
		[FieldOffset(0)]
		public long nSubData;

		[FieldOffset(0)]
		public sbyte n8SubData_0;

		[FieldOffset(1)]
		public sbyte n8SubData_1;

		[FieldOffset(2)]
		public sbyte n8SubData_2;

		[FieldOffset(3)]
		public sbyte n8SubData_3;

		[FieldOffset(4)]
		public sbyte n8SubData_4;

		[FieldOffset(5)]
		public sbyte n8SubData_5;

		[FieldOffset(6)]
		public sbyte n8SubData_6;

		[FieldOffset(7)]
		public sbyte n8SubData_7;

		[FieldOffset(0)]
		public short n16SubData_0;

		[FieldOffset(2)]
		public short n16SubData_1;

		[FieldOffset(4)]
		public short n16SubData_2;

		[FieldOffset(6)]
		public short n16SubData_3;

		[FieldOffset(0)]
		public int n32SubData_0;

		[FieldOffset(4)]
		public int n32SubData_1;
	}
}
