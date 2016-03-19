using System;

namespace PROTOCOL
{
	public class GS_ITEM_UPDATE_NFY
	{
		public sbyte m_nCreateType;

		public short m_nCount;

		public int m_nItemCount;

		public sbyte m_ParamVar;

		public int m_nCharDetailType = -1;

		public long m_nDetailValue;

		public byte ui8VoucherType;

		public long i64ItemMallID;

		public long i64VoucherRefreshTime;
	}
}
