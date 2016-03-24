using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrTableTimeShopManager : NrTSingleton<NrTableTimeShopManager>
{
	private Dictionary<byte, Dictionary<long, TIMESHOP_DATA>> m_dicTimeshopData = new Dictionary<byte, Dictionary<long, TIMESHOP_DATA>>();

	private List<TIMESHOP_REFRESHDATA> m_lstTimeShopRefreshData = new List<TIMESHOP_REFRESHDATA>();

	private string m_strTime = string.Empty;

	private ChallengeManager.eCHALLENGECODE[] m_earrTimeShopChallegeCode = new ChallengeManager.eCHALLENGECODE[]
	{
		ChallengeManager.eCHALLENGECODE.CHALLENGECODE_DAY_TIMESHOP_REFRESH10,
		ChallengeManager.eCHALLENGECODE.CHALLENGECODE_DAY_TIMESHOP_REFRESH20,
		ChallengeManager.eCHALLENGECODE.CHALLENGECODE_DAY_TIMESHOP_REFRESH30
	};

	private NrTableTimeShopManager()
	{
	}

	public void Set_DataValue(TIMESHOP_DATA _pData)
	{
		if (_pData == null)
		{
			return;
		}
		if (this.m_dicTimeshopData.ContainsKey(_pData.m_byType))
		{
			if (!this.m_dicTimeshopData[_pData.m_byType].ContainsKey(_pData.m_lIdx))
			{
				this.m_dicTimeshopData[_pData.m_byType].Add(_pData.m_lIdx, _pData);
			}
		}
		else
		{
			this.m_dicTimeshopData.Add(_pData.m_byType, new Dictionary<long, TIMESHOP_DATA>());
			this.m_dicTimeshopData[_pData.m_byType].Add(_pData.m_lIdx, _pData);
		}
	}

	public void Load_ServerValue(TIMESHOP_SERVERDATA _pData)
	{
		if (_pData == null)
		{
			return;
		}
		if (_pData.i8Type > 1)
		{
			return;
		}
		if (!this.m_dicTimeshopData.ContainsKey(_pData.i8Type))
		{
			return;
		}
		if (this.m_dicTimeshopData[_pData.i8Type].ContainsKey(_pData.i64Idx))
		{
			this.m_dicTimeshopData[_pData.i8Type][_pData.i64Idx].m_nMoneyType = _pData.i32MoneyType;
			this.m_dicTimeshopData[_pData.i8Type][_pData.i64Idx].m_fRate = (float)_pData.i32Rate;
			this.m_dicTimeshopData[_pData.i8Type][_pData.i64Idx].m_lPrice = _pData.i64Price;
			this.m_dicTimeshopData[_pData.i8Type][_pData.i64Idx].m_nItemUnique = _pData.i32ItemUnique;
			this.m_dicTimeshopData[_pData.i8Type][_pData.i64Idx].m_lItemNum = _pData.i64ItemNum;
		}
		else
		{
			TIMESHOP_DATA tIMESHOP_DATA = new TIMESHOP_DATA();
			tIMESHOP_DATA.m_lIdx = _pData.i64Idx;
			tIMESHOP_DATA.m_byType = _pData.i8Type;
			tIMESHOP_DATA.m_nMoneyType = _pData.i32MoneyType;
			tIMESHOP_DATA.m_fRate = (float)_pData.i32Rate;
			tIMESHOP_DATA.m_lPrice = _pData.i64Price;
			tIMESHOP_DATA.m_nItemUnique = _pData.i32ItemUnique;
			tIMESHOP_DATA.m_lItemNum = _pData.i64ItemNum;
			this.m_dicTimeshopData[_pData.i8Type].Add(_pData.i64Idx, tIMESHOP_DATA);
		}
	}

	public void Clear_DataValue()
	{
		this.m_dicTimeshopData.Clear();
	}

	public void Set_RefreshDataValue(TIMESHOP_REFRESHDATA _pData)
	{
		if (_pData == null)
		{
			return;
		}
		if (!this.m_lstTimeShopRefreshData.Contains(_pData))
		{
			this.m_lstTimeShopRefreshData.Add(_pData);
		}
	}

	public void Clear_RefreshDataValue()
	{
		this.m_lstTimeShopRefreshData.Clear();
	}

	public TIMESHOP_DATA Get_TimeShopDataByIDX(byte _byType, long _i64Idx)
	{
		if (_i64Idx <= 0L)
		{
			return null;
		}
		if (!this.m_dicTimeshopData.ContainsKey(_byType))
		{
			return null;
		}
		for (int i = 0; i < this.m_dicTimeshopData[_byType].Count; i++)
		{
			if (this.m_dicTimeshopData[_byType].ContainsKey(_i64Idx))
			{
				return this.m_dicTimeshopData[_byType][_i64Idx];
			}
		}
		return null;
	}

	public TIMESHOP_REFRESHDATA Get_TimeShopRefreshData(short _i16Count)
	{
		if (_i16Count < 0)
		{
			return null;
		}
		TIMESHOP_REFRESHDATA result = null;
		for (int i = 0; i < this.m_lstTimeShopRefreshData.Count; i++)
		{
			if (this.m_lstTimeShopRefreshData[i].m_i16RefreshCount <= _i16Count)
			{
				result = this.m_lstTimeShopRefreshData[i];
			}
		}
		return result;
	}

	public ChallengeManager.eCHALLENGECODE[] Get_TimeShopChallengeCode()
	{
		return this.m_earrTimeShopChallegeCode;
	}

	public string Get_MoneyTypeTextureName(eTIMESHOP_MONEYTYPE _type)
	{
		switch (_type)
		{
		case eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_HEARTS:
			return "Win_I_Hearts";
		case eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_GOLD:
			return "Com_I_MoneyIcon";
		case eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_SOULJAM:
			return "WIN_I_SoulGem";
		case eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_MYTHELXIR:
			return "Win_I_MythElixir";
		default:
			return string.Empty;
		}
	}

	public string GetTimeToString(long _i64Time)
	{
		this.m_strTime = string.Empty;
		if (_i64Time > 0L)
		{
			long totalHourFromSec = PublicMethod.GetTotalHourFromSec(_i64Time);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(_i64Time);
			long num = _i64Time % 60L;
			this.m_strTime = string.Format("{0}:{1}:{2}", totalHourFromSec.ToString("00"), minuteFromSec.ToString("00"), num.ToString("00"));
		}
		return this.m_strTime;
	}

	public bool IsRecommend(byte _byType, long _i64IDX)
	{
		if (!this.m_dicTimeshopData.ContainsKey(_byType))
		{
			return false;
		}
		for (int i = 0; i < this.m_dicTimeshopData[_byType].Count; i++)
		{
			if (this.m_dicTimeshopData[_byType].ContainsKey(_i64IDX))
			{
				return this.m_dicTimeshopData[_byType][_i64IDX].m_byRecommend == 1;
			}
		}
		return false;
	}

	public eTIMESHOP_TYPE GetType_ByIDX(long _i64IDX)
	{
		if (_i64IDX < 0L)
		{
			return eTIMESHOP_TYPE.eTIMESHOP_TYPE_NORMAL;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			return eTIMESHOP_TYPE.eTIMESHOP_TYPE_NORMAL;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return eTIMESHOP_TYPE.eTIMESHOP_TYPE_NORMAL;
		}
		int num;
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TIMESHOP_MIN_SLOTCOUNT);
		}
		else
		{
			num = (int)NrTSingleton<NrTableVipManager>.Instance.GetTimeShopCountByVipLevel(0);
		}
		int index_byTimeShopIDX = kMyCharInfo.GetIndex_byTimeShopIDX(_i64IDX);
		if (index_byTimeShopIDX < 0)
		{
			return eTIMESHOP_TYPE.eTIMESHOP_TYPE_NORMAL;
		}
		if (index_byTimeShopIDX < num)
		{
			return eTIMESHOP_TYPE.eTIMESHOP_TYPE_NORMAL;
		}
		return eTIMESHOP_TYPE.eTIMESHOP_TYPE_VIP;
	}

	public bool Is_GetRefreshReward()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		List<ChallengeTable> list = new List<ChallengeTable>();
		for (int i = 0; i < this.m_earrTimeShopChallegeCode.Length; i++)
		{
			ChallengeTable challengeTable = NrTSingleton<ChallengeManager>.Instance.GetChallengeTable((short)this.m_earrTimeShopChallegeCode[i]);
			if (challengeTable != null)
			{
				list.Add(challengeTable);
			}
		}
		int num = -1;
		bool result = false;
		bool flag = false;
		for (int j = 0; j < list.Count; j++)
		{
			if ((int)list[j].m_nLevel <= kMyCharInfo.GetLevel())
			{
				for (int k = 0; k < list[j].m_kRewardInfo.Count; k++)
				{
					if (kMyCharInfo.GetLevel() < list[j].m_kRewardInfo[k].m_nConditionLevel)
					{
						num = k;
						break;
					}
				}
				if (num != -1)
				{
					long charDetail = kMyCharInfo.GetCharDetail(12);
					if (1L <= (charDetail & list[j].m_nCheckRewardValue))
					{
						if (j < list.Count - 1)
						{
							goto IL_15D;
						}
						flag = true;
					}
					long num2 = (long)kMyCharInfo.GetDayCharDetail((eCHAR_DAY_COUNT)list[j].m_nDetailInfoIndex);
					if (num2 >= (long)list[j].m_kRewardInfo[num].m_nConditionCount && !flag)
					{
						result = true;
					}
				}
			}
			IL_15D:;
		}
		return result;
	}
}
