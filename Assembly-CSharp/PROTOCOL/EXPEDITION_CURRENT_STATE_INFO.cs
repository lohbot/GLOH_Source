using System;

namespace PROTOCOL
{
	public class EXPEDITION_CURRENT_STATE_INFO
	{
		public long i64ExpeditionID;

		public byte ui8ExpeditionMilitaryUniq;

		public short i16ExpeditionCreateDataID;

		public byte ui8ExpeditionState;

		public long i64Time;

		public long i64CheckBattleTime;

		public short i16MonLevel;
	}
}
