using System;

namespace PROTOCOL.GAME
{
	public class GS_NEWGUILD_INFO_ACK
	{
		public NEWGUILD_INFO GuildInfo = new NEWGUILD_INFO();

		public short i16NumGuildMembers;

		public short i16NumGuildApplicants;

		public long i64CreateMoney;

		public short i16LevelForCreate;

		public int i32MaxGuildNum;

		public int i32MaxMemberNum;

		public int i32MaxApplicantsNum;

		public byte ui8NewMasterCheckDay;

		public long i64NewbieLimitTime;

		public long i64PostText;

		public short i16BossBattleOpenLimitStart;

		public short i16BossBattleOpenLimitEnd;

		public short i16BossBattlePlayLimitStart;

		public short i16BossBattlePlayLimitEnd;

		public short i16GuildWarJoinCount;

		public short i16GuildWarRound;

		public long i64GuildWarGuildID;

		public long i64GuildWarStartTime;

		public char[] strGuildWarGuildName = new char[11];

		public short i16GuildWarApplyLevel;

		public short i16GuildWarApplySolLevel;

		public short i16AgitLevel;

		public long i64AgitExp;

		public int i32FundExchangeRate;

		public int i32FundDonation;

		public short i16LoadInfoType;
	}
}
