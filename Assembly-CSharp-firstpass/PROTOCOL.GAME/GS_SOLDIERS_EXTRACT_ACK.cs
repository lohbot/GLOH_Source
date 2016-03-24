using System;

namespace PROTOCOL.GAME
{
	public class GS_SOLDIERS_EXTRACT_ACK
	{
		public int i32Result;

		public long[] i64ExtractSolID = new long[10];

		public int[] ExtractItemNum = new int[10];

		public bool[] bGreat = new bool[10];

		public int ExtractItemUnique;

		public int ExtractCurrentTotalNum;

		public int ExtractAddTotalNum;
	}
}
