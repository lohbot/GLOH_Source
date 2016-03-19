using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_BF_MOVE_POS_LIST_NFY
	{
		public sbyte iMovePosCount;

		public int Result;

		public sbyte iBFOrderType;

		public sbyte iBFTurnState;

		public int iFromBFCharUnique;

		public int iToBFCharUnique;

		public float fSendTime;

		public POS3D Pos = new POS3D();

		public POS3D TargetPos = new POS3D();

		public short nAttackPower;

		public short nTurnState;

		public int[] iPara = new int[5];

		public CBATTLEOrderUnique cBFOrderUnique = new CBATTLEOrderUnique();
	}
}
