using System;

namespace PROTOCOL.GAME
{
	public class GS_EXPEDITION_DETAILINFO_ACK
	{
		public int i32Result;

		public long i64ExpeditionID;

		public byte ui8ExpeditionMilitaryUniq;

		public short i16ExpeditionCreateDataID;

		public int i32ExpeditionTotalItemNum;

		public short i16ExpeditionMonLevel;

		public long i64BattleTime;

		public long i64CheckBattleTime;

		public byte ui8ExpeditionState;

		public int i32ExpeditionRewardItemNum;

		public int i32ExpeditionMonPlunderItemNum;

		public bool bUserInfo;

		public void init()
		{
			this.i32Result = 0;
			this.i64ExpeditionID = 0L;
			this.ui8ExpeditionMilitaryUniq = 0;
			this.i16ExpeditionCreateDataID = 0;
			this.i32ExpeditionTotalItemNum = 0;
			this.i16ExpeditionMonLevel = 0;
			this.i64BattleTime = 0L;
			this.i64CheckBattleTime = 0L;
			this.ui8ExpeditionState = 0;
			this.i32ExpeditionRewardItemNum = 0;
			this.i32ExpeditionMonPlunderItemNum = 0;
			this.bUserInfo = false;
		}
	}
}
