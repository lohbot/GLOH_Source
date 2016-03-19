using System;

namespace PROTOCOL
{
	public class MINE_INFO
	{
		public long i64MineID;

		public short i16MineDataID;

		public byte ui8MineState;

		public int i32LeftItemNum;

		public long i64Time;

		public void Init()
		{
			this.i64MineID = 0L;
			this.i16MineDataID = 0;
			this.ui8MineState = 0;
			this.i32LeftItemNum = 0;
			this.i64Time = 0L;
		}

		public void Set(MINE_INFO info)
		{
			this.i64MineID = info.i64MineID;
			this.i16MineDataID = info.i16MineDataID;
			this.ui8MineState = info.ui8MineState;
			this.i32LeftItemNum = info.i32LeftItemNum;
			this.i64Time = info.i64Time;
		}
	}
}
