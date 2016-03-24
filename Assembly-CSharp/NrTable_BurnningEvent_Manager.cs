using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrTable_BurnningEvent_Manager : NrTSingleton<NrTable_BurnningEvent_Manager>
{
	private SortedDictionary<eBUNNING_EVENT, BUNNING_EVENT_REFLASH_INFO> m_sdCollection;

	private BUNNING_EVENT_INFO[] m_BunningEvent = new BUNNING_EVENT_INFO[40];

	private NkValueParse<eBUNNING_EVENT> m_kBunningEventCode;

	private int m_nEventDay;

	private int m_nEventWeek;

	private EVENT_INFO[] m_EventInfo = new EVENT_INFO[7];

	public sbyte m_nDifficult = -1;

	private NrTable_BurnningEvent_Manager()
	{
		this.m_sdCollection = new SortedDictionary<eBUNNING_EVENT, BUNNING_EVENT_REFLASH_INFO>();
		this.m_kBunningEventCode = new NkValueParse<eBUNNING_EVENT>();
		for (int i = 0; i < 7; i++)
		{
			this.m_EventInfo[i] = new EVENT_INFO();
		}
		this.SetBurnningEventCode();
	}

	public SortedDictionary<eBUNNING_EVENT, BUNNING_EVENT_REFLASH_INFO> Get_Collection()
	{
		return this.m_sdCollection;
	}

	public int GetEventDay()
	{
		return this.m_nEventDay;
	}

	public int GetEventWeek()
	{
		return this.m_nEventWeek;
	}

	public int Get_Count()
	{
		return this.m_sdCollection.Count;
	}

	public BUNNING_EVENT_REFLASH_INFO Get_Value(eBUNNING_EVENT eEventType)
	{
		if (this.m_sdCollection.ContainsKey(eEventType))
		{
			return this.m_sdCollection[eEventType];
		}
		return null;
	}

	public void InitEventReflash()
	{
		this.m_sdCollection.Clear();
	}

	public void SetEventReflash(BUNNING_EVENT_REFLASH_INFO a_cValue)
	{
		if (this.m_sdCollection.ContainsKey(a_cValue.m_eEventType))
		{
			this.m_sdCollection[a_cValue.m_eEventType] = a_cValue;
		}
		else
		{
			this.m_sdCollection.Add(a_cValue.m_eEventType, a_cValue);
		}
	}

	public void InitEventInfo()
	{
		for (int i = 0; i < 7; i++)
		{
			this.m_EventInfo[i].Init();
		}
	}

	public int CurrentEventCount()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return 0;
		}
		int level = kMyCharInfo.GetLevel();
		int num = 0;
		for (int i = 0; i < 7; i++)
		{
			EVENT_INFO eventInfo = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventInfo(i);
			if (eventInfo != null)
			{
				if (eventInfo.m_nEventType != 0)
				{
					if (NrTSingleton<ContentsLimitManager>.Instance.IsChallenge() || eventInfo.m_nEventType != 36)
					{
						BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
						if (value != null)
						{
							if (level <= value.m_nLimitLevel)
							{
								num++;
							}
						}
					}
				}
			}
		}
		if (this.SetBasicData())
		{
			num++;
		}
		int num2 = (int)kMyCharInfo.GetCharDetail(5);
		if (0 < num2 && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num2))
		{
			num++;
		}
		int num3 = 0;
		for (int j = 0; j < 7; j++)
		{
			if (this.m_EventInfo[j].m_nEventType > 0)
			{
				num3++;
				if (this.m_EventInfo[j].m_nEventType == 1 || this.m_EventInfo[j].m_nEventType == 14 || this.m_EventInfo[j].m_nEventType == 18)
				{
					num3--;
				}
			}
		}
		return num3 - num;
	}

	public void InitCurrentEventInfo()
	{
		for (int i = 0; i < 7; i++)
		{
			this.m_EventInfo[i].Init();
		}
	}

	public void AddEvent(sbyte nEventInfoWeek, int nEventType, int nStartTime, int nTime, int nMaxLimitCount, long nLeftEventTime, int nDay, int nWeek, int nTitleText, int nExplain)
	{
		if (nEventType <= 0 || nEventType >= 40)
		{
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			if (this.m_EventInfo[i].m_nEventType == nEventType && (int)this.m_EventInfo[i].m_nEventInfoWeek == (int)nEventInfoWeek)
			{
				this.m_EventInfo[i].SetEventInfo(nEventInfoWeek, nEventType, nStartTime, nTime, nMaxLimitCount, nLeftEventTime, nTitleText, nExplain);
				return;
			}
		}
		for (int j = 0; j < 7; j++)
		{
			if (this.m_EventInfo[j].m_nEventType <= 0)
			{
				this.m_EventInfo[j].SetEventInfo(nEventInfoWeek, nEventType, nStartTime, nTime, nMaxLimitCount, nLeftEventTime, nTitleText, nExplain);
				break;
			}
		}
		this.m_nEventDay = nDay;
		this.m_nEventWeek = nWeek;
	}

	public EVENT_INFO GetEventInfo(int iIndex)
	{
		if (0 > iIndex || iIndex > 7)
		{
			return null;
		}
		return this.m_EventInfo[iIndex];
	}

	public EVENT_INFO GetEventInfoFromType(int nEventType)
	{
		if (nEventType <= 0 || nEventType >= 40)
		{
			return null;
		}
		for (int i = 0; i < 40; i++)
		{
			if (this.m_EventInfo[i].m_nEventType == nEventType)
			{
				return this.m_EventInfo[i];
			}
		}
		return null;
	}

	public void DeleteEvent(sbyte nEventInfoWeek, int nEventType)
	{
		if (nEventType <= 0 || nEventType >= 40)
		{
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			if (this.m_EventInfo[i].m_nEventType == nEventType)
			{
				if ((int)this.m_EventInfo[i].m_nEventInfoWeek < 0 || (int)this.m_EventInfo[i].m_nEventInfoWeek == (int)nEventInfoWeek)
				{
					this.m_EventInfo[i].Init();
					break;
				}
			}
		}
	}

	public void SetBurnningEventCode()
	{
		this.m_kBunningEventCode.InsertCodeValue("BABELPARTY", eBUNNING_EVENT.eBUNNING_EVENT_BABELPARTY);
		this.m_kBunningEventCode.InsertCodeValue("COLOSSEUM", eBUNNING_EVENT.eBUNNING_EVENT_COLOSSEUM);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE1", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE1);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE2", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE2);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE3", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE3);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE4", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE4);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE5", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE5);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE6", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE6);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE7", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE7);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE8", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE8);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE9", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE9);
		this.m_kBunningEventCode.InsertCodeValue("DAILYDUNGEON", eBUNNING_EVENT.eBUNNING_EVENT_DAILYDUNGEON);
		this.m_kBunningEventCode.InsertCodeValue("DAILYQUEST", eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST);
		this.m_kBunningEventCode.InsertCodeValue("DAILYQUEST1", eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST1);
		this.m_kBunningEventCode.InsertCodeValue("DAILYQUEST2", eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST2);
		this.m_kBunningEventCode.InsertCodeValue("BOUNTYHUNT", eBUNNING_EVENT.eBUNNING_EVENT_BOUNTYHUNT);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF1", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF1);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF2", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF2);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF3", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF3);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF4", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF4);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF5", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF5);
		this.m_kBunningEventCode.InsertCodeValue("GMBUFF6", eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF6);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE10", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE10);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE11", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE11);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE12", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE12);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE13", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE13);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE14", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE14);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE15", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE15);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE16", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE16);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE17", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE17);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE18", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE18);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE19", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE19);
		this.m_kBunningEventCode.InsertCodeValue("BUFFDAMAGE20", eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE20);
		this.m_kBunningEventCode.InsertCodeValue("CHALLENGE", eBUNNING_EVENT.eBUNNING_CHALLENGE);
		this.m_kBunningEventCode.InsertCodeValue("EXPEVENT", eBUNNING_EVENT.eBUNNING_EVENT_EXPEVENT);
		this.m_kBunningEventCode.InsertCodeValue("GOLDEVENT", eBUNNING_EVENT.eBUNNING_EVENT_GOLDEVENT);
		this.m_kBunningEventCode.InsertCodeValue("ITEMEVENT", eBUNNING_EVENT.eBUNNING_EVENT_ITEMEVENT);
		this.m_kBunningEventCode.InsertCodeValue("SHOWEVENT", eBUNNING_EVENT.eBUNNING_EVENT_SHOWEVENT);
	}

	public eBUNNING_EVENT GetBurnningEventCode(string strBurnningEventCode)
	{
		return this.m_kBunningEventCode.GetValue(strBurnningEventCode);
	}

	public void Set_Value(eBUNNING_EVENT eBurnningEvent, BUNNING_EVENT_INFO value)
	{
		if (eBurnningEvent <= eBUNNING_EVENT.eBUNNING_EVENT_NONE || eBurnningEvent >= eBUNNING_EVENT.eBUNNING_EVENT_MAX)
		{
			return;
		}
		this.m_BunningEvent[(int)eBurnningEvent] = value;
	}

	public int GetEventCount()
	{
		int num = 0;
		for (int i = 0; i < 40; i++)
		{
			if (this.m_BunningEvent[i] != null)
			{
				if (this.m_BunningEvent[i].m_eEventType > eBUNNING_EVENT.eBUNNING_EVENT_NONE)
				{
					num++;
				}
			}
		}
		return num;
	}

	public BUNNING_EVENT_INFO GetValue(eBUNNING_EVENT eBurnningEvent)
	{
		if (eBurnningEvent <= eBUNNING_EVENT.eBUNNING_EVENT_NONE || eBurnningEvent >= eBUNNING_EVENT.eBUNNING_EVENT_MAX)
		{
			return null;
		}
		return this.m_BunningEvent[(int)eBurnningEvent];
	}

	public bool IsHaveReward(eBUNNING_EVENT eBurnningEvent)
	{
		if (eBurnningEvent <= eBUNNING_EVENT.eBUNNING_EVENT_NONE || eBurnningEvent >= eBUNNING_EVENT.eBUNNING_EVENT_MAX)
		{
			return false;
		}
		bool flag = false;
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		sUBDATA_UNION.nSubData = kMyCharInfo.GetCharDetail(15);
		int num = -1;
		if (eBurnningEvent != eBUNNING_EVENT.eBUNNING_EVENT_BABELPARTY)
		{
			if (eBurnningEvent == eBUNNING_EVENT.eBUNNING_EVENT_COLOSSEUM)
			{
				int num2 = (int)sUBDATA_UNION.n8SubData_1;
				num = num2 % 2;
				flag = (num > 0);
			}
		}
		else
		{
			int num2 = (int)sUBDATA_UNION.n8SubData_0;
			num = num2 % 2;
			flag = (num > 0);
		}
		return num >= 0 && flag;
	}

	public int GetLimitCount(eBUNNING_EVENT eBurnningEvent)
	{
		int result = -1;
		if (eBurnningEvent <= eBUNNING_EVENT.eBUNNING_EVENT_NONE || eBurnningEvent >= eBUNNING_EVENT.eBUNNING_EVENT_MAX)
		{
			return result;
		}
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		sUBDATA_UNION.nSubData = kMyCharInfo.GetCharDetail(15);
		EVENT_INFO eventInfoFromType = this.GetEventInfoFromType((int)eBurnningEvent);
		if (eventInfoFromType == null)
		{
			return result;
		}
		int nMaxLimitCount = eventInfoFromType.m_nMaxLimitCount;
		int num = nMaxLimitCount * 2;
		if (eBurnningEvent != eBUNNING_EVENT.eBUNNING_EVENT_BABELPARTY)
		{
			if (eBurnningEvent == eBUNNING_EVENT.eBUNNING_EVENT_COLOSSEUM)
			{
				int num2 = (int)sUBDATA_UNION.n8SubData_1;
				result = (num - num2 + 1) / 2;
			}
		}
		else
		{
			int num2 = (int)sUBDATA_UNION.n8SubData_0;
			result = (num - num2 + 1) / 2;
		}
		return result;
	}

	public bool SetBasicData()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return false;
		}
		sbyte dayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		DAILYDUNGEON_INFO dailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)dayOfWeek);
		if (dailyDungeonInfo == null)
		{
			return false;
		}
		if ((int)this.m_nDifficult < 0)
		{
			this.m_nDifficult = dailyDungeonInfo.m_i8Diff;
		}
		if ((int)this.m_nDifficult == 0)
		{
			this.m_nDifficult = 1;
		}
		return EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(this.m_nDifficult, dayOfWeek) != null && (dailyDungeonInfo.m_i32IsClear > 1 || (int)dailyDungeonInfo.m_i8IsReward == 0);
	}

	public bool IsGet_DailyDungeonReward()
	{
		bool result = false;
		sbyte dayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		DAILYDUNGEON_INFO dailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)dayOfWeek);
		if (dailyDungeonInfo != null)
		{
			sbyte b = dailyDungeonInfo.m_i8Diff;
			if ((int)b == 0)
			{
				b = 1;
			}
			if (EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(b, dayOfWeek) == null)
			{
				return false;
			}
			if (dailyDungeonInfo.m_i32IsClear <= 1)
			{
				result = (dailyDungeonInfo.m_i32IsClear == 1);
				if ((int)dailyDungeonInfo.m_i8IsReward == 1)
				{
					result = false;
				}
			}
		}
		else
		{
			result = false;
		}
		return result;
	}
}
