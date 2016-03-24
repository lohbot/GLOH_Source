using System;

namespace PROTOCOL
{
	public class MINE_INFO
	{
		public long i64MineID;

		public short i16MineDataID;

		public byte ui8MineState;

		public int i32LeftItemNum;

		public int i32PlunderItemNum;

		public long i64Time;

		public long i64CreateTime;

		public void Init()
		{
			this.i64MineID = 0L;
			this.i16MineDataID = 0;
			this.ui8MineState = 0;
			this.i32LeftItemNum = 0;
			this.i32PlunderItemNum = 0;
			this.i64Time = 0L;
			this.i64CreateTime = 0L;
		}

		public void Set(MINE_INFO info)
		{
			this.i64MineID = info.i64MineID;
			this.i16MineDataID = info.i16MineDataID;
			this.ui8MineState = info.ui8MineState;
			this.i32LeftItemNum = info.i32LeftItemNum;
			this.i32PlunderItemNum = info.i32PlunderItemNum;
			this.i64Time = info.i64Time;
			this.i64CreateTime = info.i64CreateTime;
		}
	}
}
