using System;

namespace PROTOCOL
{
	public class CONTENTSLIMIT_DATA
	{
		public int i32ContentsLimitType;

		public long[] i64Param = new long[10];

		private void Set(CONTENTSLIMIT_DATA Data)
		{
			this.i32ContentsLimitType = Data.i32ContentsLimitType;
			for (int i = 0; i < 10; i++)
			{
				this.i64Param[i] = Data.i64Param[i];
			}
		}
	}
}
