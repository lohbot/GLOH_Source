using System;

namespace PROTOCOL.GAME
{
	public class GS_BF_ORDER_ACK
	{
		public float fServerTime;

		public sbyte iBFOrderType;

		public sbyte iBFTurnState;

		public int iFromBFCharUnique;

		public int iToBFCharUnique;

		public short nTemp;

		public short nTurnState;

		public int[] iPara = new int[5];

		public CBATTLEOrderUnique cBFOrderUnique = new CBATTLEOrderUnique();
	}
}
