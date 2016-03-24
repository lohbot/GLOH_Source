using System;

namespace PROTOCOL.GAME
{
	public class GS_BABELTOWER_MACRO_BATTLE_START_REQ
	{
		public short nBabelStage;

		public short nSubFloor;

		public short nFloorType;

		public short nMySolCount;

		public short nFriendSolCount;

		public byte nAutoRevive;

		public byte nBattleSpeedCheck;

		public long[] nSolID;

		public byte[] nBattlePos;

		public long[] nFriendPersonID;

		public long[] nFriendSolID;

		public byte[] nFriendBattlePos;

		public int nCombinationUnique;

		public bool bOpenMacro;

		public GS_BABELTOWER_MACRO_BATTLE_START_REQ()
		{
			int num = 9;
			this.nSolID = new long[num];
			this.nBattlePos = new byte[num];
			this.nFriendPersonID = new long[3];
			this.nFriendSolID = new long[3];
			this.nFriendBattlePos = new byte[3];
			this.bOpenMacro = false;
		}
	}
}
