using System;

namespace PROTOCOL.GAME
{
	public class GS_MINE_BATTLE_RESULT_LIST_REQ
	{
		public short i16Page;

		public long i64FirstLegionActionID;

		public long i64LastLegionActionID;

		public bool bNextRequest;

		public bool bGiveComplete;
	}
}
