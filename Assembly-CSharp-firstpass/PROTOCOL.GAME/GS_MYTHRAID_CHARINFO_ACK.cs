using System;

namespace PROTOCOL.GAME
{
	public class GS_MYTHRAID_CHARINFO_ACK
	{
		public byte nRaidSeason;

		public byte nRaidType;

		public int clearRound;

		public long soloDamage;

		public long partyDamage;

		public int soloRank;

		public int partyRank;

		public long upRankDamage;
	}
}
