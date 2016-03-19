using System;

namespace PROTOCOL.GAME
{
	public class GS_BF_CHARINFO_NFY
	{
		public float fServerTime;

		public sbyte iCharInfoType;

		public int iBFCharUnique;

		public int[] iPara = new int[10];

		public CBATTLEOrderUnique cBFOrderUnique = new CBATTLEOrderUnique();
	}
}
