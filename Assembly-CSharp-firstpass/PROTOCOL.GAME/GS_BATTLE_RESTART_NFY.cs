using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_RESTART_NFY
	{
		public sbyte nEnemyAlly = -1;

		public sbyte nContinueCount;

		public bool bBoss;

		public int[] nMonsterKind = new int[6];
	}
}
