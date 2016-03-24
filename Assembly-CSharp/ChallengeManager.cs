using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class ChallengeManager : NrTSingleton<ChallengeManager>
{
	public enum TYPE
	{
		ONEDAY = 1,
		WEEK,
		CONTINUE,
		EVENT,
		HIDDEN,
		BAND_INVITE_FRIENDS = 10
	}

	public enum eCHALLENGECODE
	{
		CHALLENGECODE_DAY_LOGIN = 1000,
		CHALLENGECODE_DAY_JOIN_EXPEDITION = 1010,
		CHALLENGECODE_DAY_WIN_HEROWAR1,
		CHALLENGECODE_DAY_WIN_HEROWAR2,
		CHALLENGECODE_DAY_WIN_HEROWAR3,
		CHALLENGECODE_DAY_WIN_HEROWAR4,
		CHALLENGECODE_DAY_WIN_HEROWAR5,
		CHALLENGECODE_DAY_JOIN_COLOSSEUM = 1020,
		CHALLENGECODE_DAY_USE_WILL = 1030,
		CHALLENGECODE_DAY_REQUEST_FRIENDSOL = 1040,
		CHALLENGECODE_DAY_REPLAY_BATTLE = 1050,
		CHALLENGECODE_DAY_JOIN_MINE = 1060,
		CHALLENGECODE_DAY_WIN_COLOSSEUM2,
		CHALLENGECODE_DAY_WIN_COLOSSEUM3,
		CHALLENGECODE_DAY_WIN_COLOSSEUM4,
		CHALLENGECODE_DAY_PLAYER_WIN_COLOSSEUM1,
		CHALLENGECODE_DAY_PLAYER_WIN_COLOSSEUM2,
		CHALLENGECODE_DAY_PLAYER_WIN_COLOSSEUM3,
		CHALLENGECODE_DAY_JOIN_GUILDBOSS = 1070,
		CHALLENGECODE_DAY_INVITE_FACEBOOK2,
		CHALLENGECODE_DAY_INVITE_FACEBOOK3,
		CHALLENGECODE_DAY_INVITE_KAKAO1 = 1080,
		CHALLENGECODE_DAY_INVITE_KAKAO2,
		CHALLENGECODE_DAY_INVITE_KAKAO3,
		CHALLENGECODE_DAY_WIN_BABELTOWER1 = 1090,
		CHALLENGECODE_DAY_WIN_BABELTOWER2,
		CHALLENGECODE_DAY_WIN_BABELTOWER3,
		CHALLENGECODE_DAY_WIN_BABELTOWER31 = 1100,
		CHALLENGECODE_DAY_WIN_BABELTOWER32,
		CHALLENGECODE_DAY_WIN_BABELTOWER41 = 1110,
		CHALLENGECODE_DAY_WIN_BABELTOWER42,
		CHALLENGECODE_DAY_WIN_BABELTOWER51 = 1120,
		CHALLENGECODE_DAY_WIN_BABELTOWER52,
		CHALLENGECODE_DAY_WIN_BABELTOWER61 = 1130,
		CHALLENGECODE_DAY_WIN_BABELTOWER62,
		CHALLENGECODE_DAY_CLEAR_DAILYDUNGEON = 1201,
		CHALLENGECODE_DAY_USER_INFIBATTLE,
		CHALLENGECODE_DAY_USE_BOUNT_HUNT,
		CHALLENGECODE_DAY_USE_BABELTOWER,
		CHALLENGECODE_DAY_USE_MANYWILL,
		CHALLENGECODE_DAY_USER_MANYINFIBATTLE,
		CHALLENGECODE_DAY_USE_BABELTOWER15,
		CHALLENGECODE_DAY_USE_BABELTOWER5,
		CHALLENGECODE_DAY_USER_BOUNT_HUNT4,
		CHALLENGECODE_DAY_USER_MYTHRAID,
		CHALLENGECODE_DAY_USER_DAILYDUNGEON,
		CHALLENGECODE_DAY_USER_NEWEXPLORATION,
		CHALLENGECODE_DAY_TIMESHOP_REFRESH10 = 1301,
		CHALLENGECODE_DAY_TIMESHOP_REFRESH20,
		CHALLENGECODE_DAY_TIMESHOP_REFRESH30,
		CHALLENGECODE_RECOMMEND_START = 1450,
		CHALLENGECODE_COMPOSE = 1499,
		CHALLENGECODE_PREMIUM_RECRUIT,
		CHALLENGECODE_EXTRACT = 1502,
		CHALLENGECODE_LEGEND_ESSENCE_RECRUIT,
		CHALLENGECODE_LEGENDSOL,
		CHALLENGECODE_TRANSCENDENCE,
		CHALLENGECODE_TICKET_PREMIUM,
		CHALLENGECODE_RECOMMEND_END,
		CHALLENGECODE_WEEK_LOGIN = 2000,
		CHALLENGECODE_WEEK_JOIN_HEROWAR,
		CHALLENGECODE_WIN_BATTLE = 3000,
		CHALLENGECODE_ENHANCE_1LV = 3010,
		CHALLENGECODE_ENHANCE_10LV = 3020,
		CHALLENGECODE_ENHANCE_20LV = 3030,
		CHALLENGECODE_SELLITEM_MONEY = 3040,
		CHALLENGECODE_ENHANCE_HERO = 3050,
		CHALLENGECODE_RECRUIT_HERO = 3055,
		CHALLENGECODE_EVOLVE_HERO = 3060,
		CHALLENGECODE_HEROWAR_GAINMONEY = 3070,
		CHALLENGECODE_JOIN_COLOSSEUM = 3080,
		CHALLENGECODE_REPLAY_BATTLE = 3090,
		CHALLENGECODE_DOWNLEVEL_ENHANCE = 3120,
		CHALLENGECODE_SKILL_ENHANCE = 3130,
		CHALLENGECODE_REVIEW = 3140,
		CHALLENGECODE_LEVELUP = 3150,
		CHALLENGECODE_GET_MINE_ITEM = 3160,
		CHALLENGECODE_EQUIP_ITEM50 = 3170,
		CHALLENGECODE_EQUIP_ITEM60,
		CHALLENGECODE_EQUIP_ITEM70,
		CHALLENGECODE_EQUIP_ITEM80,
		CHALLENGECODE_EQUIP_ITEM90,
		CHALLENGECODE_EQUIP_LEGEND_EXCALIBUR = 3180,
		CHALLENGECODE_EQUIP_LEGEND_GUNGNIR,
		CHALLENGECODE_EQUIP_LEGEND_MJOLNIR,
		CHALLENGECODE_EQUIP_LEGEND_CUPIDBOW,
		CHALLENGECODE_EQUIP_LEGEND_MARYREAD,
		CHALLENGECODE_EQUIP_LEGEND_CROMWELL_CANNON,
		CHALLENGECODE_EQUIP_LEGEND_LIFETREE,
		CHALLENGECODE_EQUIP_LEGEND_DEAD_SEA_SCROLLS,
		CHALLENGECODE_EQUIP_LEGEND_IMMORTAL_ARMOR,
		CHALLENGECODE_EQUIP_LEGEND_CHIU_HELMET,
		CHALLENGECODE_EQUIP_LEGEND_ATLASS_HANDS,
		CHALLENGECODE_EQUIP_LEGEND_HERMES_SHOES,
		CHALLENGECODE_COMPLETE_QUEST = 3500,
		CHALLENGECODE_EVENT1 = 4000,
		CHALLENGECODE_GSTAR = 4010,
		CHALLENGECODE_CLEAR_STARTINDEX = 10000,
		CHALLENGECODE_CLEAR_ENDINDEX = 10100,
		CHALLENGECODE_RANK_CLEAR_STARTINDEX = 11000,
		CHALLENGECODE_RANK_CLEAR_ENDINDEX = 10100,
		CHALLENGECODE_TREASURE_STARTINDEX = 12000,
		CHALLENGECODE_TREASURE_ENDINDEX = 10100,
		CHALLENGECODE_PLATFORM_INVITE_FRIEND = 20000
	}

	public static long CHALLENGEREWARD_DAY_LOGIN = 1L;

	public static long CHALLENGEREWARD_DAY_JOIN_EXPEDITION = 2L;

	public static long CHALLENGEREWARD_DAY_JOIN_COLOSSEUM = 4L;

	public static long CHALLENGEREWARD_DAY_USE_WILL = 8L;

	public static long CHALLENGEREWARD_DAY_REQUEST_FRIENDSOL = 16L;

	public static long CHALLENGEREWARD_DAY_REPLAY_BATTLE = 32L;

	public static long CHALLENGEREWARD_DAY_JOIN_MINE = 64L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM2 = 128L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM3 = 256L;

	public static long CHALLENGEREWARD_DAY_JOIN_GUILDBOSS = 512L;

	public static long CHALLENGEREWARD_DAY_INVITE_FACEBOOK2 = 1024L;

	public static long CHALLENGEREWARD_DAY_INVITE_FACEBOOK3 = 2048L;

	public static long CHALLENGEREWARD_DAY_INVITE_KAKAO1 = 4096L;

	public static long CHALLENGEREWARD_DAY_INVITE_KAKAO2 = 8192L;

	public static long CHALLENGEREWARD_DAY_INVITE_KAKAO3 = 16384L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER1 = 32768L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER2 = 65536L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER3 = 131072L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER31 = 262144L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER32 = 524288L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER41 = 1048576L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER42 = 2097152L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER51 = 4194304L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER52 = 8388608L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER61 = 16777216L;

	public static long CHALLENGEREWARD_DAY_WIN_BABELTOWER62 = 33554432L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM4 = 67108864L;

	public static long CHALLENGEREWARD_DAY_CLEAR_DAILYDUNGEON = 134217728L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER1 = 268435456L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER2 = 536870912L;

	public static long CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER3 = 1073741824L;

	public static long CHALLENGEREWARD_DAY_WIN_HEROWAR1 = (long)((ulong)-2147483648);

	public static long CHALLENGEREWARD_DAY_WIN_HEROWAR2 = 4294967296L;

	public static long CHALLENGEREWARD_DAY_WIN_HEROWAR3 = 8589934592L;

	public static long CHALLENGEREWARD_DAY_WIN_HEROWAR4 = 17179869184L;

	public static long CHALLENGEREWARD_DAY_WIN_HEROWAR5 = 34359738368L;

	public static long CHALLENGEREWARD_DAY_USER_INFIBATTLE = 68719476736L;

	public static long CHALLENGEREWARD_DAY_USER_BOUNT_HUNT = 137438953472L;

	public static long CHALLENGEREWARD_DAY_BABELTOWER = 274877906944L;

	public static long CHALLENGEREWARD_DAY_USER_MANYWILL = 549755813888L;

	public static long CHALLENGEREWARD_DAY_USER_MANYINFIBATTLE = 1099511627776L;

	public static long CHALLENGEREWARD_DAY_BABELTOWER15 = 2199023255552L;

	public static long CHALLENGEREWARD_DAY_BABELTOWER5 = 4398046511104L;

	public static long CHALLENGEREWARD_DAY_USER_BOUNT_HUNT4 = 8796093022208L;

	public static long CHALLENGEREWARD_DAY_USER_MYTHRAID = 17592186044416L;

	public static long CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH10 = 35184372088832L;

	public static long CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH20 = 70368744177664L;

	public static long CHALLENGEREWARD_DAY_USER_TIMESHOP_REFRESH30 = 140737488355328L;

	public static long CHALLENGEREWARD_DAY_USER_DAILYDUNGEON = 281474976710656L;

	public static long CHALLENGEREWARD_DAY_USER_NEWEXPLORATION = 562949953421312L;

	private Dictionary<short, ChallengeTimeTable> m_kChallengeTime = new Dictionary<short, ChallengeTimeTable>();

	private Dictionary<short, Dictionary<short, ChallengeTable>> m_kChallenge = new Dictionary<short, Dictionary<short, ChallengeTable>>();

	private Dictionary<short, ChallengeEquipTable> m_kChallengeEquip = new Dictionary<short, ChallengeEquipTable>();

	private List<ChallengeTable> m_listChallengeData = new List<ChallengeTable>();

	private int m_nTotalRewardCount;

	private int m_nDayRewardNoticeCount;

	private int m_nContinueRewardNoticeCount;

	private int m_nEventRewardNoticeCount;

	private Dictionary<short, short> m_kNotice = new Dictionary<short, short>();

	private Dictionary<short, short> m_kOldNotice = new Dictionary<short, short>();

	private List<short> m_nChallengeEvent_RewardInfo = new List<short>();

	private bool m_bLoad;

	private ChallengeManager()
	{
	}

	public bool Initialize()
	{
		this.m_nTotalRewardCount = 0;
		this.m_nDayRewardNoticeCount = 0;
		this.m_nContinueRewardNoticeCount = 0;
		this.m_nChallengeEvent_RewardInfo.Clear();
		return true;
	}

	public void AddChallengeTimeTable(ChallengeTimeTable table)
	{
		if (this.m_kChallengeTime.ContainsKey(table.m_nUnique))
		{
			return;
		}
		this.m_kChallengeTime.Add(table.m_nUnique, table);
	}

	public ChallengeTimeTable GetChallengeTimeTable(short unique)
	{
		if (!this.m_kChallengeTime.ContainsKey(unique))
		{
			return null;
		}
		return this.m_kChallengeTime[unique];
	}

	public Dictionary<short, ChallengeTable> GetChallengeType(ChallengeManager.TYPE type)
	{
		short key = (short)type;
		if (!this.m_kChallenge.ContainsKey(key))
		{
			return null;
		}
		return this.m_kChallenge[key];
	}

	public void AddChallengeTable(ChallengeTable table)
	{
		if (this.m_bLoad)
		{
			return;
		}
		if (!this.m_kChallenge.ContainsKey(table.m_nType))
		{
			Dictionary<short, ChallengeTable> dictionary = new Dictionary<short, ChallengeTable>();
			table.m_kRewardInfo.Add(table.m_kInfo);
			dictionary.Add(table.m_nUnique, table);
			this.m_kChallenge.Add(table.m_nType, dictionary);
		}
		else if (this.m_kChallenge[table.m_nType].ContainsKey(table.m_nUnique))
		{
			Dictionary<short, ChallengeTable> dictionary2 = this.m_kChallenge[table.m_nType];
			dictionary2[table.m_nUnique].m_kRewardInfo.Add(table.m_kInfo);
		}
		else
		{
			Dictionary<short, ChallengeTable> dictionary3 = new Dictionary<short, ChallengeTable>();
			table.m_kRewardInfo.Add(table.m_kInfo);
			dictionary3.Add(table.m_nUnique, table);
			this.m_kChallenge[table.m_nType].Add(table.m_nUnique, table);
		}
	}

	public void AddChallengeEquipTable(ChallengeEquipTable data)
	{
		if (this.m_kChallengeEquip.ContainsKey(data.m_ChallengeIdx))
		{
			return;
		}
		this.m_kChallengeEquip.Add(data.m_ChallengeIdx, data);
	}

	public ChallengeTable GetChallengeTable(short unique)
	{
		foreach (Dictionary<short, ChallengeTable> current in this.m_kChallenge.Values)
		{
			if (current != null)
			{
				if (current.ContainsKey(unique))
				{
					return current[unique];
				}
			}
		}
		return null;
	}

	public void CalcTotalRewardCount()
	{
		this.m_bLoad = true;
		short key = 3;
		if (!this.m_kChallenge.ContainsKey(key))
		{
			return;
		}
		int num = 0;
		foreach (ChallengeTable current in this.m_kChallenge[key].Values)
		{
			if (current != null)
			{
				foreach (ChallengeTable.RewardInfo current2 in current.m_kRewardInfo)
				{
					if (0 < current2.m_nConditionCount)
					{
						num++;
					}
				}
			}
		}
		this.m_nTotalRewardCount = num;
	}

	public int CalcTotalPercent()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return 0;
		}
		UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return 0;
		}
		short key = 3;
		if (!this.m_kChallenge.ContainsKey(key))
		{
			return 0;
		}
		int num = 0;
		foreach (ChallengeTable current in this.m_kChallenge[key].Values)
		{
			if (current != null)
			{
				if ((int)current.m_nLevel <= kMyCharInfo.GetLevel())
				{
					Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(current.m_nUnique);
					if (userChallengeInfo2 != null)
					{
						foreach (ChallengeTable.RewardInfo current2 in current.m_kRewardInfo)
						{
							if (userChallengeInfo2.m_nValue >= (long)current2.m_nConditionCount)
							{
								num++;
							}
						}
					}
				}
			}
		}
		if (this.m_nTotalRewardCount == 0)
		{
			return 0;
		}
		return num * 100 / this.m_nTotalRewardCount;
	}

	public int GetDayRewardNoticeCount()
	{
		return this.m_nDayRewardNoticeCount;
	}

	public int GetContinueRewardNoticeCount()
	{
		return this.m_nContinueRewardNoticeCount;
	}

	public int GetEventRewardNoticeCount()
	{
		return this.m_nEventRewardNoticeCount;
	}

	public int GetRewardNoticeCount()
	{
		return this.m_nDayRewardNoticeCount + this.m_nContinueRewardNoticeCount + this.m_nEventRewardNoticeCount;
	}

	public int CalcDayRewardNoticeCount()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return 0;
		}
		short key = 1;
		if (!this.m_kChallenge.ContainsKey(key))
		{
			return 0;
		}
		int num = 0;
		int num2 = -1;
		foreach (ChallengeTable current in this.m_kChallenge[key].Values)
		{
			if ((int)current.m_nLevel <= kMyCharInfo.GetLevel())
			{
				for (int i = 0; i < current.m_kRewardInfo.Count; i++)
				{
					if (kMyCharInfo.GetLevel() < current.m_kRewardInfo[i].m_nConditionLevel)
					{
						num2 = i;
						break;
					}
				}
				if (num2 == -1)
				{
					return 0;
				}
				long charDetail = kMyCharInfo.GetCharDetail(12);
				long num3 = (long)current.m_kRewardInfo[num2].m_nConditionCount;
				if (1L > (charDetail & current.m_nCheckRewardValue))
				{
					if (current.m_nUnique == 1012)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR1) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1013)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR2) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1014)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR2) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR3) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1015)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR2) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR3) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR4) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1065)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER1) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1066)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER2) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1081)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO1) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1082)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO2) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1091)
					{
						if ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER1) == 0L)
						{
							continue;
						}
					}
					else if (current.m_nUnique == 1092 && ((charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER1) == 0L || (charDetail & ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER2) == 0L))
					{
						continue;
					}
					long num4 = (long)kMyCharInfo.GetDayCharDetail((eCHAR_DAY_COUNT)current.m_nDetailInfoIndex);
					if (num4 >= num3)
					{
						num++;
					}
				}
			}
		}
		this.m_nDayRewardNoticeCount = num;
		return num;
	}

	public int CalcContinueRewardNoticeCount()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return 0;
		}
		UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return 0;
		}
		short key = 3;
		if (!this.m_kChallenge.ContainsKey(key))
		{
			return 0;
		}
		int num = 0;
		int num2 = 64;
		foreach (ChallengeTable current in this.m_kChallenge[key].Values)
		{
			if (current != null)
			{
				if ((int)current.m_nLevel <= kMyCharInfo.GetLevel())
				{
					Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(current.m_nUnique);
					if (userChallengeInfo2 != null)
					{
						if (current.m_nType != 1 && current.m_nType != 2)
						{
							int num3 = 0;
							foreach (ChallengeTable.RewardInfo current2 in current.m_kRewardInfo)
							{
								bool flag = false;
								if (num3 < num2)
								{
									long num4 = 1L << (num3 & 31);
									if ((userChallengeInfo2.m_bGetReward1 & num4) == 0L)
									{
										flag = true;
									}
								}
								else
								{
									long num5 = 1L << (num3 - num2 & 31);
									if ((userChallengeInfo2.m_bGetReward1 & num5) == 0L)
									{
										flag = true;
									}
								}
								if (userChallengeInfo2.m_nValue >= (long)current2.m_nConditionCount && flag)
								{
									num++;
									break;
								}
								num3++;
							}
						}
					}
				}
			}
		}
		this.m_nContinueRewardNoticeCount = num;
		return this.m_nContinueRewardNoticeCount;
	}

	public int CalcEvnetRewardNoticeCount()
	{
		int num = 0;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return 0;
		}
		ITEM item = NkUserInventory.instance.GetItem(50902);
		if (item == null)
		{
			return 0;
		}
		if (item.m_nItemNum > 0)
		{
			Dictionary<short, ChallengeTable> challengeType = NrTSingleton<ChallengeManager>.Instance.GetChallengeType(ChallengeManager.TYPE.EVENT);
			if (challengeType == null)
			{
				return 0;
			}
			foreach (ChallengeTable current in challengeType.Values)
			{
				int index = 0;
				if (item.m_nItemNum >= current.m_kRewardInfo[index].m_nConditionCount && !NrTSingleton<ChallengeManager>.Instance.GetChallengeEventRewardInfo(current.m_nUnique))
				{
					num++;
				}
			}
		}
		this.m_nEventRewardNoticeCount = num;
		return this.m_nEventRewardNoticeCount = num;
	}

	public void SetEvnetRewardNoticeCount(int nCount)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		this.m_nEventRewardNoticeCount = nCount;
	}

	public void DeleteNotice(short unique)
	{
		if (!this.m_kNotice.ContainsKey(unique))
		{
			return;
		}
		this.m_kNotice.Remove(unique);
	}

	public void InsertNotice(short unique, short index)
	{
		if (this.m_kOldNotice.ContainsKey(unique))
		{
			return;
		}
		if (this.m_kNotice.ContainsKey(unique))
		{
			return;
		}
		this.m_kNotice.Add(unique, index);
		this.m_kOldNotice.Add(unique, index);
	}

	public void ShowNotice()
	{
		if (Scene.CurScene == Scene.Type.WORLD)
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) != null || NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) != null)
			{
				return;
			}
			if (0 < this.m_kNotice.Count && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHALLENGEPOPUP_DLG) == null)
			{
				short num = 0;
				short index = 0;
				using (Dictionary<short, short>.Enumerator enumerator = this.m_kNotice.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<short, short> current = enumerator.Current;
						num = current.Key;
						index = current.Value;
					}
				}
				if (0 < num)
				{
					ChallengePopupDlg challengePopupDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHALLENGEPOPUP_DLG) as ChallengePopupDlg;
					if (challengePopupDlg != null)
					{
						challengePopupDlg.SetPopupInfo(num, index);
					}
				}
				this.m_kNotice.Remove(num);
			}
		}
	}

	public void GetEquipChallengeStrKey(ChallengeManager.eCHALLENGECODE type, out string titleKey1, out string titleKey2)
	{
		titleKey1 = string.Empty;
		titleKey2 = string.Empty;
		if (this.m_kChallengeEquip.ContainsKey((short)type))
		{
			ChallengeEquipTable challengeEquipTable = this.m_kChallengeEquip[(short)type];
			titleKey1 = challengeEquipTable.m_LabelText1;
			titleKey2 = challengeEquipTable.m_LabelText2;
		}
	}

	public bool isEquipChallenge(short unique)
	{
		return this.m_kChallengeEquip.ContainsKey(unique);
	}

	public void SetChallengeEventRewardInfo(short ChallengeUnique)
	{
		if (!this.m_nChallengeEvent_RewardInfo.Contains(ChallengeUnique))
		{
			this.m_nChallengeEvent_RewardInfo.Add(ChallengeUnique);
		}
	}

	public bool GetChallengeEventRewardInfo(short ChallengeUnique)
	{
		return this.m_nChallengeEvent_RewardInfo.Contains(ChallengeUnique);
	}

	public int GetChallengeEventRewardInfoCount()
	{
		return this.m_nChallengeEvent_RewardInfo.Count;
	}

	public void ChallengeEventRewardInfoInit()
	{
		this.m_nChallengeEvent_RewardInfo.Clear();
	}

	public void AddSequenceChallengeTable(ChallengeTable table)
	{
		if (!this.m_listChallengeData.Contains(table))
		{
			this.m_listChallengeData.Add(table);
		}
		else
		{
			TsLog.LogWarning("m_listChallengeData Add Error", new object[]
			{
				table.m_nUnique
			});
		}
	}

	public ChallengeTable GetSequenceChallenge(short Sequence)
	{
		for (int i = 0; i < this.m_listChallengeData.Count; i++)
		{
			if (0 >= this.m_listChallengeData[i].m_nSequence)
			{
				if (this.m_listChallengeData[i].m_nSequence == Sequence)
				{
					return this.m_listChallengeData[i];
				}
			}
		}
		return null;
	}

	public ChallengeTable GetSequenceChallengeTable()
	{
		ChallengeTable challengeTable = null;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return null;
		}
		UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return null;
		}
		for (int i = 0; i < this.m_listChallengeData.Count; i++)
		{
			Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(this.m_listChallengeData[i].m_nUnique);
			if (userChallengeInfo2 == null)
			{
				if (challengeTable == null)
				{
					challengeTable = this.m_listChallengeData[i];
				}
				break;
			}
			int j = 0;
			int num = 64;
			while (j < this.m_listChallengeData[i].m_kRewardInfo.Count)
			{
				bool flag = false;
				if (j < num)
				{
					long num2 = 1L << (j & 31);
					if ((userChallengeInfo2.m_bGetReward1 & num2) == 0L)
					{
						flag = true;
					}
				}
				else
				{
					long num3 = 1L << (j - num & 31);
					if ((userChallengeInfo2.m_bGetReward1 & num3) == 0L)
					{
						flag = true;
					}
				}
				if (flag)
				{
					challengeTable = this.m_listChallengeData[i];
					break;
				}
				if (userChallengeInfo2.m_nValue >= (long)this.m_listChallengeData[i].m_kRewardInfo[j].m_nConditionCount)
				{
					challengeTable = null;
				}
				j++;
			}
		}
		return challengeTable;
	}

	public void GetChallengeOpenReward()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHALLENGE_DLG))
		{
			ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
			if (challengeDlg != null)
			{
				challengeDlg.ChangeTab();
			}
		}
	}

	public void GetChallengeOpenUi(string OpenUi, short unique)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string a = "e" + OpenUi;
		if (a == "0")
		{
			return;
		}
		if (a == eChallenge_OpenUi.ePLUNDERMAIN.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
			{
				GS_INFIBATTLE_RANK_GET_REQ gS_INFIBATTLE_RANK_GET_REQ = new GS_INFIBATTLE_RANK_GET_REQ();
				gS_INFIBATTLE_RANK_GET_REQ.i64PersonID = kMyCharInfo.m_PersonID;
				SendPacket.GetInstance().SendObject(2017, gS_INFIBATTLE_RANK_GET_REQ);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERMAIN_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eBOUNTYHUNTING.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BOUNTYHUNTING_DLG))
			{
				BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
				if (bountyHuntingDlg != null)
				{
					bountyHuntingDlg.SetData();
				}
			}
		}
		else if (a == eChallenge_OpenUi.eBABELTOWER_MODESELECT.ToString())
		{
			int level = kMyCharInfo.GetLevel();
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_LIMITLEVEL);
			if (level < value)
			{
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"level",
					value.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (kMyCharInfo.m_kFriendInfo.GetFriendCount() > 0 && kMyCharInfo.m_kFriendInfo.GetFriendsBaBelDataCount() == 0)
			{
				GS_FRIENDS_BABELTOWER_CLEARINFO_REQ obj = new GS_FRIENDS_BABELTOWER_CLEARINFO_REQ();
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIENDS_BABELTOWER_CLEARINFO_REQ, obj);
			}
			int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_HARD_LEVEL);
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG))
			{
				if (level < value2)
				{
					DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
					if (directionDLG != null)
					{
						directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL, 1);
					}
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABELTOWER_MODESELECT_DLG);
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERMAIN_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eMYTHRAID_MODESELECT.ToString())
		{
			if (kMyCharInfo != null)
			{
				int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTHRAID_LIMITLEVEL);
				if (num == 0)
				{
					num = 50;
				}
				if (kMyCharInfo.GetLevel() < num)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("908");
					if (string.IsNullOrEmpty(textFromNotify2))
					{
						return;
					}
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return;
				}
			}
			DirectionDLG directionDLG2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
			if (directionDLG2 != null)
			{
				directionDLG2.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MYTHRAID, 0);
			}
		}
		else if (a == eChallenge_OpenUi.eMINE_SEARCH.ToString())
		{
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID() || !NrTSingleton<ContentsLimitManager>.Instance.IsMineApply((short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
			{
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("763");
				Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MINE_TUTORIAL_STEP);
			if (charSubData == 1L)
			{
				MineTutorialStepDlg mineTutorialStepDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_TUTORIAL_STEP_DLG) as MineTutorialStepDlg;
				if (mineTutorialStepDlg != null)
				{
					mineTutorialStepDlg.SetStep(1L);
				}
			}
			else
			{
				NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
			}
		}
		else if (a == eChallenge_OpenUi.eCOLOSSEUMMAIN.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMMAIN_DLG))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMMAIN_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMMAIN_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eBABEL_GUILDBOSS_MAIN.ToString())
		{
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsGuildBoss())
			{
				return;
			}
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() <= 0L)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("545"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
			if (babelGuildBossDlg != null)
			{
				babelGuildBossDlg.Show();
			}
		}
		else if (a == eChallenge_OpenUi.eREFORGEMAIN.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGERESULT_DLG);
				ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
				reforgeMainDlg.Show();
			}
		}
		else if (a == eChallenge_OpenUi.eSOLEXTRACT_DUMMY.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG))
			{
				HeroCollect_DLG_challengequest heroCollect_DLG_challengequest = (HeroCollect_DLG_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
				if (heroCollect_DLG_challengequest != null)
				{
					heroCollect_DLG_challengequest._ChallengeQuestUnique = (int)unique;
					heroCollect_DLG_challengequest.SetDummyUI();
					heroCollect_DLG_challengequest.Show();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eSOLTRANSCENDENCE_DUMMY.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG))
			{
				HeroCollect_DLG_challengequest heroCollect_DLG_challengequest2 = (HeroCollect_DLG_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
				if (heroCollect_DLG_challengequest2 != null)
				{
					heroCollect_DLG_challengequest2._ChallengeQuestUnique = (int)unique;
					heroCollect_DLG_challengequest2.SetDummyUI();
					heroCollect_DLG_challengequest2.Show();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eSOLCOMPOSE_DUMMY.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG))
			{
				HeroCollect_DLG_challengequest heroCollect_DLG_challengequest3 = (HeroCollect_DLG_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
				if (heroCollect_DLG_challengequest3 != null)
				{
					heroCollect_DLG_challengequest3._ChallengeQuestUnique = (int)unique;
					heroCollect_DLG_challengequest3.SetDummyUI();
					heroCollect_DLG_challengequest3.Show();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eSOLRECRUIT_DUMMY.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLRECRUIT_CHALLENGEQUEST_DLG))
			{
				SolRecruitDlg_ChallengeQuest solRecruitDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUIT_CHALLENGEQUEST_DLG) as SolRecruitDlg_ChallengeQuest;
				solRecruitDlg_ChallengeQuest.SetTicketList();
			}
		}
		else if (a == eChallenge_OpenUi.eSOLRECRUIT.ToString())
		{
			GS_TICKET_SELL_INFO_REQ obj2 = new GS_TICKET_SELL_INFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_REQ, obj2);
		}
		else if (a == eChallenge_OpenUi.eSTORYCHAT.ToString())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.STORYCHAT_DLG))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.STORYCHAT_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.STORYCHAT_DLG);
			}
		}
		else if (a == eChallenge_OpenUi.eBABELTOWERMAIN.ToString())
		{
			int level2 = kMyCharInfo.GetLevel();
			int value3 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_LIMITLEVEL);
			if (level2 < value3)
			{
				string empty2 = string.Empty;
				string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromNotify4,
					"level",
					value3.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
			if (babelTowerMainDlg != null)
			{
				babelTowerMainDlg.Show();
			}
		}
		else if (a == eChallenge_OpenUi.eREDUCEMAIN.ToString())
		{
			NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(125);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
		}
		else if (a == eChallenge_OpenUi.eITEMSKILL.ToString())
		{
			NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(109);
		}
		else if (a == eChallenge_OpenUi.eSOLDETAIL.ToString())
		{
			GS_SOLGUIDE_INFO_REQ gS_SOLGUIDE_INFO_REQ = new GS_SOLGUIDE_INFO_REQ();
			gS_SOLGUIDE_INFO_REQ.bElementMark = false;
			gS_SOLGUIDE_INFO_REQ.i32CharKind = 1053;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLGUIDE_INFO_REQ, gS_SOLGUIDE_INFO_REQ);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
		}
		else if (a == eChallenge_OpenUi.eITEMMALL_DUMMY.ToString())
		{
			ItemMallDlg_ChallengeQuest itemMallDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg_ChallengeQuest;
			if (itemMallDlg_ChallengeQuest != null)
			{
				itemMallDlg_ChallengeQuest._ChallengeQuestUnique = (int)unique;
				itemMallDlg_ChallengeQuest.SetShowMode(ItemMallDlg.eMODE.eMODE_VOUCHER_HERO);
				itemMallDlg_ChallengeQuest.InitDummyUI();
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
		}
		else if (a == eChallenge_OpenUi.eSOLEVOLUTION_DUMMY.ToString())
		{
			Myth_Evolution_Main_DLG_ChallengeQuest myth_Evolution_Main_DLG_ChallengeQuest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_MAIN_CHALLENGEQUEST_DLG) as Myth_Evolution_Main_DLG_ChallengeQuest;
			if (myth_Evolution_Main_DLG_ChallengeQuest != null)
			{
				myth_Evolution_Main_DLG_ChallengeQuest.ChallengeQuestUnique = (int)unique;
				myth_Evolution_Main_DLG_ChallengeQuest.InitDummyUI();
			}
		}
		else if (a == eChallenge_OpenUi.eDAILYDUNGEON.ToString())
		{
			DailyDungeon_Select_Dlg dailyDungeon_Select_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_SELECT) as DailyDungeon_Select_Dlg;
			if (dailyDungeon_Select_Dlg != null)
			{
				dailyDungeon_Select_Dlg.SetData();
			}
		}
		else if (a == eChallenge_OpenUi.eNEWEXPLORATION.ToString())
		{
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetInfo();
			}
		}
		else if (a == eChallenge_OpenUi.eSOLCOMPOSE.ToString())
		{
			SolComposeMainDlg solComposeMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_MAIN_DLG) as SolComposeMainDlg;
		}
		if (NrTSingleton<EventConditionHandler>.Instance.OpenUIByChallenge != null)
		{
			NrTSingleton<EventConditionHandler>.Instance.OpenUIByChallenge.OnTrigger(unique);
		}
	}
}
