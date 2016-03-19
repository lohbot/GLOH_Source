using System;

namespace PROTOCOL
{
	public class GS_ITEMMALL_TRADE_ACK
	{
		public int m_nResult;

		public long m_MallIndex;

		public long SolID;

		public long i64Money;

		public char[] strGiftCharName = new char[21];

		public int i32BuyCount;

		public VOUCHER_DATA VoucherData = new VOUCHER_DATA();
	}
}
