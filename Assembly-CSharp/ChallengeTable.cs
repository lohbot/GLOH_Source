using System;
using System.Collections.Generic;
using TsLibs;

public class ChallengeTable
{
	public class RewardInfo
	{
		public string m_szConditionTextKey = string.Empty;

		public int m_nConditionLevel;

		public int m_nConditionCount;

		public long m_nMoney;

		public int m_nItemUnique;

		public int m_nItemNum;
	}

	public short m_nType;

	public short m_nUnique;

	public string m_szIconKey = string.Empty;

	public short m_nLevel;

	public string m_szTitleTextKey = string.Empty;

	public ChallengeTable.RewardInfo m_kInfo = new ChallengeTable.RewardInfo();

	public List<ChallengeTable.RewardInfo> m_kRewardInfo = new List<ChallengeTable.RewardInfo>();

	public bool m_bAutoReward;

	public long m_nCheckRewardValue;

	public int m_nDetailInfoIndex;

	public short m_nSequence;

	public string m_szOpenUI = string.Empty;

	public ChallengeTable()
	{
		this.m_nType = 0;
		this.m_nUnique = 0;
		this.m_szIconKey = string.Empty;
		this.m_nLevel = 0;
		this.m_szTitleTextKey = string.Empty;
		this.m_kRewardInfo = new List<ChallengeTable.RewardInfo>();
		this.m_bAutoReward = false;
		this.m_nCheckRewardValue = 0L;
		this.m_nDetailInfoIndex = 0;
		this.m_nSequence = 0;
		this.m_szOpenUI = string.Empty;
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nUnique);
		row.GetColumn(num++, out this.m_nType);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_szIconKey);
		row.GetColumn(num++, out this.m_nLevel);
		row.GetColumn(num++, out this.m_szTitleTextKey);
		row.GetColumn(num++, out this.m_kInfo.m_szConditionTextKey);
		row.GetColumn(num++, out this.m_kInfo.m_nConditionLevel);
		row.GetColumn(num++, out this.m_kInfo.m_nConditionCount);
		row.GetColumn(num++, out this.m_kInfo.m_nMoney);
		row.GetColumn(num++, out this.m_kInfo.m_nItemUnique);
		row.GetColumn(num++, out this.m_kInfo.m_nItemNum);
		row.GetColumn(num++, out this.m_bAutoReward);
		row.GetColumn(num++, out this.m_szOpenUI);
		row.GetColumn(num++, out this.m_nSequence);
		if (this.m_nUnique == 1010)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_JOIN_EXPEDITION;
			this.m_nDetailInfoIndex = 0;
		}
		else if (this.m_nUnique == 1011)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR1;
			this.m_nDetailInfoIndex = 15;
		}
		else if (this.m_nUnique == 1012)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR2;
			this.m_nDetailInfoIndex = 15;
		}
		else if (this.m_nUnique == 1013)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR3;
			this.m_nDetailInfoIndex = 15;
		}
		else if (this.m_nUnique == 1014)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR4;
			this.m_nDetailInfoIndex = 15;
		}
		else if (this.m_nUnique == 1015)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR5;
			this.m_nDetailInfoIndex = 15;
		}
		else if (this.m_nUnique == 1020)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_JOIN_COLOSSEUM;
			this.m_nDetailInfoIndex = 1;
		}
		else if (this.m_nUnique == 1030)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USE_WILL;
			this.m_nDetailInfoIndex = 2;
		}
		else if (this.m_nUnique == 1040)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_REQUEST_FRIENDSOL;
			this.m_nDetailInfoIndex = 3;
		}
		else if (this.m_nUnique == 1050)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_REPLAY_BATTLE;
			this.m_nDetailInfoIndex = 4;
		}
		else if (this.m_nUnique == 1060)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_JOIN_MINE;
			this.m_nDetailInfoIndex = 5;
		}
		else if (this.m_nUnique == 1064)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER1;
			this.m_nDetailInfoIndex = 14;
		}
		else if (this.m_nUnique == 1065)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER2;
			this.m_nDetailInfoIndex = 14;
		}
		else if (this.m_nUnique == 1066)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER3;
			this.m_nDetailInfoIndex = 14;
		}
		else if (this.m_nUnique == 1070)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_JOIN_GUILDBOSS;
			this.m_nDetailInfoIndex = 6;
		}
		else if (this.m_nUnique == 1080)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO1;
			this.m_nDetailInfoIndex = 7;
		}
		else if (this.m_nUnique == 1081)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO2;
			this.m_nDetailInfoIndex = 7;
		}
		else if (this.m_nUnique == 1082)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO3;
			this.m_nDetailInfoIndex = 7;
		}
		else if (this.m_nUnique == 1090)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER1;
			this.m_nDetailInfoIndex = 8;
		}
		else if (this.m_nUnique == 1091)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER2;
			this.m_nDetailInfoIndex = 8;
		}
		else if (this.m_nUnique == 1092)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER3;
			this.m_nDetailInfoIndex = 8;
		}
		else if (this.m_nUnique == 1100)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER31;
			this.m_nDetailInfoIndex = 9;
		}
		else if (this.m_nUnique == 1101)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER32;
			this.m_nDetailInfoIndex = 9;
		}
		else if (this.m_nUnique == 1110)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER41;
			this.m_nDetailInfoIndex = 10;
		}
		else if (this.m_nUnique == 1111)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER42;
			this.m_nDetailInfoIndex = 10;
		}
		else if (this.m_nUnique == 1120)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER51;
			this.m_nDetailInfoIndex = 11;
		}
		else if (this.m_nUnique == 1121)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER52;
			this.m_nDetailInfoIndex = 11;
		}
		else if (this.m_nUnique == 1130)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER61;
			this.m_nDetailInfoIndex = 12;
		}
		else if (this.m_nUnique == 1131)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER62;
			this.m_nDetailInfoIndex = 12;
		}
		else if (this.m_nUnique == 1201)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_CLEAR_DAILYDUNGEON;
			this.m_nDetailInfoIndex = 13;
		}
		else if (this.m_nUnique == 1205)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_MANYWILL;
			this.m_nDetailInfoIndex = 2;
		}
		else if (this.m_nUnique == 1202)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_INFIBATTLE;
			this.m_nDetailInfoIndex = 16;
		}
		else if (this.m_nUnique == 1203)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_BOUNT_HUNT;
			this.m_nDetailInfoIndex = 17;
		}
		else if (this.m_nUnique == 1204)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_BABELTOWER;
			this.m_nDetailInfoIndex = 18;
		}
		else if (this.m_nUnique == 1206)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_MANYINFIBATTLE;
			this.m_nDetailInfoIndex = 16;
		}
		else if (this.m_nUnique == 1207)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_BABELTOWER15;
			this.m_nDetailInfoIndex = 18;
		}
		else if (this.m_nUnique == 1208)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_BABELTOWER5;
			this.m_nDetailInfoIndex = 18;
		}
		else if (this.m_nUnique == 1209)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_BOUNT_HUNT4;
			this.m_nDetailInfoIndex = 17;
		}
		else if (this.m_nUnique == 1210)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_MYTHRAID;
			this.m_nDetailInfoIndex = 19;
		}
		else if (this.m_nUnique == 1301)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH10;
			this.m_nDetailInfoIndex = 20;
		}
		else if (this.m_nUnique == 1302)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH20;
			this.m_nDetailInfoIndex = 20;
		}
		else if (this.m_nUnique == 1303)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH30;
			this.m_nDetailInfoIndex = 20;
		}
		else if (this.m_nUnique == 1211)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_DAILYDUNGEON;
			this.m_nDetailInfoIndex = 21;
		}
		else if (this.m_nUnique == 1212)
		{
			this.m_nCheckRewardValue = ChallengeManager.CHALLENGEREWARD_DAY_USER_NEWEXPLORATION;
			this.m_nDetailInfoIndex = 22;
		}
	}
}
