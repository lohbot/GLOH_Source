using System;

namespace PROTOCOL
{
	public class BATTLE_SOLDIER_SUBDATA_INFO
	{
		public long SolID;

		public long[] SolSubData;

		public BATTLE_SOLDIER_SUBDATA_INFO()
		{
			this.SolSubData = new long[16];
		}
	}
}
