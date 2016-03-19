using System;

namespace PROTOCOL
{
	public class MINE_MILITARY_USER_SOLINFO
	{
		public long i64PersonID;

		public byte ui8MilitaryUnique;

		public short i16CharLevel;

		public char[] szCharname = new char[21];

		public byte ui8BatchIndex;

		public int i32GetItemNum;

		public byte nLeaderMilitary;

		public byte nMilitaryState;

		public MINE_BATCH_SOLINFO[] mine_solinfo = new MINE_BATCH_SOLINFO[5];

		public MINE_MILITARY_USER_SOLINFO()
		{
			for (int i = 0; i < 5; i++)
			{
				this.mine_solinfo[i] = new MINE_BATCH_SOLINFO();
			}
		}
	}
}
