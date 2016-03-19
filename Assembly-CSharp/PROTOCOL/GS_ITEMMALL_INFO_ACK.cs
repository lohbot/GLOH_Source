using System;

namespace PROTOCOL
{
	public class GS_ITEMMALL_INFO_ACK
	{
		public int i32ItemMallType;

		public int i32ItemMallMode;

		public short i16BaseCount;

		public short i16Count;

		public short i16BuyCount;

		public byte i8Login;

		public bool bStart;

		public bool bEnd;

		public int i32VoucherRefillTime;

		public short i16ItemVoucherDataCount;

		public bool bShowDLG = true;
	}
}
