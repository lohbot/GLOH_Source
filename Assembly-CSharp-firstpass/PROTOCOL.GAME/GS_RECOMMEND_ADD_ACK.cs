using System;

namespace PROTOCOL.GAME
{
	public class GS_RECOMMEND_ADD_ACK
	{
		public int i32Result;

		public byte Recommend_Recv;

		public byte Recommend_Send;

		public char[] RecvName = new char[21];
	}
}
