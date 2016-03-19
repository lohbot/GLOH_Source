using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_BF_ORDER_REQ
	{
		public short iCharUnique;

		public sbyte iBFOrderType;

		public sbyte iBFNextOrderType;

		public int iFromBFCharUnique;

		public int iToBFCharUnique;

		public float fSendTime;

		public POS3D Pos = new POS3D();

		public POS3D TargetPos = new POS3D();

		public int[] iPara = new int[5];

		public CBATTLEOrderUnique cBFOrderUnique = new CBATTLEOrderUnique();
	}
}
