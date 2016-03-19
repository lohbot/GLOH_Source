using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_EVENT_ACTION_EFFECT_NFY
	{
		public int nAlly;

		public int nStartPos;

		public int nCharKind;

		public float fPosX;

		public float fPosZ;

		public char[] szEffectCode = new char[51];

		public char[] szEffectName = new char[51];

		public byte nType;
	}
}
