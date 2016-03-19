using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_BOSS_AGGRO_NFY
	{
		public short i16BUID;

		public short[] i16AggroTargetBUID = new short[3];

		public byte[] i8AggroValue = new byte[3];

		public GS_BATTLE_BOSS_AGGRO_NFY()
		{
			for (int i = 0; i < 3; i++)
			{
				this.i16AggroTargetBUID[i] = -1;
				this.i8AggroValue[i] = 0;
			}
		}
	}
}
