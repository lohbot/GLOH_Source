using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class NewExplorationManager : NrTSingleton<NewExplorationManager>
{
	[StructLayout(LayoutKind.Explicit)]
	public struct SUBDATA_NEWEXPLORATION
	{
		[FieldOffset(0)]
		public long nSubData;

		[FieldOffset(0)]
		public sbyte bLastClearFloor;

		[FieldOffset(1)]
		public sbyte bLastClearSubFloor;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CHAR_WEEK_DATA_NEWEXPLORATION
	{
		[FieldOffset(0)]
		public long nSubData;

		[FieldOffset(0)]
		public short i16LastTresureIndex;

		[FieldOffset(2)]
		public sbyte bRestartFloor;

		[FieldOffset(3)]
		public sbyte bBestClearFloor;

		[FieldOffset(4)]
		public sbyte bBestClearSubFloor;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CHAR_DETAIL_NEWEXPLORATION
	{
		[FieldOffset(0)]
		public long nSubData;

		[FieldOffset(0)]
		public sbyte bCurFloor;

		[FieldOffset(1)]
		public sbyte bCurSubFloor;

		[FieldOffset(2)]
		public sbyte bPlayState;

		[FieldOffset(3)]
		public sbyte bResetCount;

		[FieldOffset(4)]
		public int i32BossDamage;
	}

	private Dictionary<short, NEWEXPLORATION_DATA> m_listData = new Dictionary<short, NEWEXPLORATION_DATA>();

	private Dictionary<int, BATTLE_NEWEXPLORATION_SREWARD> m_listSRewardData = new Dictionary<int, BATTLE_NEWEXPLORATION_SREWARD>();

	private Dictionary<short, NEWEXPLORATION_TREASURE> m_listTreasureData = new Dictionary<short, NEWEXPLORATION_TREASURE>();

	private Dictionary<short, NEWEXPLORATION_RESET_INFO> m_listResetInfoData = new Dictionary<short, NEWEXPLORATION_RESET_INFO>();

	private List<NEWEXPLORATION_RANK_REWARD> m_listRankRewardData = new List<NEWEXPLORATION_RANK_REWARD>();

	private clTempBattlePos[] m_BattlePos;

	private bool m_bAutoBattle;

	private bool m_bBattleSpeedCheck;

	private bool m_bIfDie_StopAutoBattle;

	public bool AutoBattle
	{
		get
		{
			return this.m_bAutoBattle;
		}
	}

	public bool IfDie_StopAutoBattle
	{
		get
		{
			return this.m_bIfDie_StopAutoBattle;
		}
	}

	private NewExplorationManager()
	{
		this.BattleInit();
	}

	private void BattleInit()
	{
		this.SetAutoBattle(false, false, false);
	}

	public void AddData(NEWEXPLORATION_DATA data)
	{
		this.m_listData[data.i16Index] = data;
	}

	public void AddSRewardData(BATTLE_NEWEXPLORATION_SREWARD data)
	{
		this.m_listSRewardData[data.m_nRewardUnique] = data;
	}

	public void AddTreasureData(NEWEXPLORATION_TREASURE data)
	{
		this.m_listTreasureData[data.i16Index] = data;
	}

	public void AddResetInfoData(NEWEXPLORATION_RESET_INFO data)
	{
		this.m_listResetInfoData[data.i16CountIndex] = data;
	}

	public void AddRankRewardData(NEWEXPLORATION_RANK_REWARD data)
	{
		this.m_listRankRewardData.Add(data);
	}

	public void SortRankRewardData()
	{
		NEWEXPLORATION_RANK_REWARD_COMPARE @object = new NEWEXPLORATION_RANK_REWARD_COMPARE();
		this.m_listRankRewardData.Sort(new Comparison<NEWEXPLORATION_RANK_REWARD>(@object.Compare));
	}

	public short GetFloorIndex(sbyte floor, sbyte subFloor)
	{
		return (short)((int)floor * 100 + (int)subFloor);
	}

	public NewExplorationManager.SUBDATA_NEWEXPLORATION GetCharSubData()
	{
		NewExplorationManager.SUBDATA_NEWEXPLORATION result = default(NewExplorationManager.SUBDATA_NEWEXPLORATION);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return result;
		}
		result.nSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_NEWEXPLORATION_INFO);
		return result;
	}

	public NewExplorationManager.CHAR_DETAIL_NEWEXPLORATION GetCharDetatilData()
	{
		NewExplorationManager.CHAR_DETAIL_NEWEXPLORATION result = default(NewExplorationManager.CHAR_DETAIL_NEWEXPLORATION);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return result;
		}
		result.nSubData = kMyCharInfo.GetCharDetail(30);
		return result;
	}

	public NewExplorationManager.CHAR_WEEK_DATA_NEWEXPLORATION GetCharWeekData()
	{
		NewExplorationManager.CHAR_WEEK_DATA_NEWEXPLORATION result = default(NewExplorationManager.CHAR_WEEK_DATA_NEWEXPLORATION);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return result;
		}
		result.nSubData = kMyCharInfo.GetCharWeekData(1);
		return result;
	}

	public sbyte GetFloor()
	{
		sbyte b = this.GetCharDetatilData().bCurFloor;
		if ((int)b == 0)
		{
			b = this.GetCharWeekData().bRestartFloor;
		}
		return ((int)b > 0) ? b : 1;
	}

	public sbyte GetSubFloor()
	{
		NewExplorationManager.CHAR_DETAIL_NEWEXPLORATION charDetatilData = this.GetCharDetatilData();
		return ((int)charDetatilData.bCurSubFloor > 0) ? charDetatilData.bCurSubFloor : 1;
	}

	public eNEWEXPLORATION_PLAYSTATE GetPlayState()
	{
		return (eNEWEXPLORATION_PLAYSTATE)this.GetCharDetatilData().bPlayState;
	}

	public short GetLastTreasureIndex()
	{
		return this.GetCharWeekData().i16LastTresureIndex;
	}

	public int GetBossDamage()
	{
		return this.GetCharDetatilData().i32BossDamage;
	}

	public NEWEXPLORATION_DATA GetData(sbyte floor, sbyte subFloor)
	{
		short floorIndex = this.GetFloorIndex(floor, subFloor);
		if (!this.m_listData.ContainsKey(floorIndex))
		{
			return null;
		}
		return this.m_listData[floorIndex];
	}

	public Dictionary<short, NEWEXPLORATION_DATA> GetDataList()
	{
		return this.m_listData;
	}

	public bool CanGetEndReward()
	{
		NewExplorationManager.SUBDATA_NEWEXPLORATION charSubData = this.GetCharSubData();
		return this.GetData(charSubData.bLastClearFloor, charSubData.bLastClearSubFloor) != null;
	}

	public bool IsClear(sbyte bFloor, sbyte bSubFloor)
	{
		if ((int)this.GetFloor() < (int)bFloor)
		{
			return false;
		}
		if ((int)this.GetFloor() == (int)bFloor && (int)this.GetSubFloor() < (int)bSubFloor)
		{
			return false;
		}
		if ((int)this.GetFloor() == (int)bFloor && (int)this.GetSubFloor() == (int)bSubFloor)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return false;
			}
			NEWEXPLORATION_DATA data = this.GetData(bFloor, bSubFloor);
			if (data == null)
			{
				return false;
			}
			if (this.GetBossDamage() < data.i32BossHp)
			{
				return false;
			}
		}
		return true;
	}

	public NEWEXPLORATION_DATA GetEndRewardData()
	{
		NewExplorationManager.SUBDATA_NEWEXPLORATION charSubData = this.GetCharSubData();
		return this.GetData(charSubData.bLastClearFloor, charSubData.bLastClearSubFloor);
	}

	public bool CanPlay(sbyte bFloor, sbyte bSubFloor)
	{
		return this.GetPlayState() != eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END && (int)bFloor == (int)this.GetFloor() && (int)bSubFloor == (int)this.GetSubFloor() && !this.IsClear(bFloor, bSubFloor);
	}

	public bool DoNotPlay()
	{
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_NONE)
		{
			return true;
		}
		if (this.GetBossDamage() > 0)
		{
			return false;
		}
		NewExplorationManager.CHAR_DETAIL_NEWEXPLORATION charDetatilData = this.GetCharDetatilData();
		if ((int)charDetatilData.bCurFloor > 0 || (int)charDetatilData.bCurSubFloor > 0)
		{
			return false;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return false;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.IsAtbCommonFlag(16L))
			{
				return false;
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.IsAtbCommonFlag(16L))
			{
				return false;
			}
		}
		return true;
	}

	public NEWEXPLORATION_TREASURE GetTreasureData(sbyte bFloor, sbyte bSubFloor)
	{
		foreach (KeyValuePair<short, NEWEXPLORATION_TREASURE> current in this.m_listTreasureData)
		{
			if ((int)current.Value.bFloor == (int)bFloor && (int)current.Value.bSubFloor == (int)bSubFloor)
			{
				return current.Value;
			}
		}
		return null;
	}

	public NEWEXPLORATION_TREASURE GetTreasureData(short Index)
	{
		if (!this.m_listTreasureData.ContainsKey(Index))
		{
			return null;
		}
		return this.m_listTreasureData[Index];
	}

	public NEWEXPLORATION_TREASURE GetLastTreasureData()
	{
		return this.GetTreasureData(this.GetLastTreasureIndex());
	}

	public NEWEXPLORATION_TREASURE GetPrevTreasureData(sbyte bFloor, sbyte bSubFloor)
	{
		NEWEXPLORATION_TREASURE treasureData = this.GetTreasureData(bFloor, bSubFloor);
		return this.GetTreasureData(treasureData.i16Index - 1);
	}

	public bool IsOpenTreasureBox(sbyte bFloor, sbyte bSubFloor)
	{
		NEWEXPLORATION_TREASURE lastTreasureData = this.GetLastTreasureData();
		return lastTreasureData != null && ((int)lastTreasureData.bFloor > (int)bFloor || ((int)lastTreasureData.bFloor == (int)bFloor && (int)lastTreasureData.bSubFloor >= (int)bSubFloor));
	}

	public bool CanGetTreasure(sbyte bFloor, sbyte bSubFloor)
	{
		NEWEXPLORATION_DATA nEWEXPLORATION_DATA = this.CanGetTreasureData();
		return nEWEXPLORATION_DATA != null && (int)nEWEXPLORATION_DATA.bFloor == (int)bFloor && (int)nEWEXPLORATION_DATA.bSubFloor == (int)bSubFloor;
	}

	public NEWEXPLORATION_DATA CanGetTreasureData()
	{
		short num = this.GetLastTreasureIndex();
		num += 1;
		NEWEXPLORATION_TREASURE treasureData = this.GetTreasureData(num);
		if (treasureData == null)
		{
			return null;
		}
		NewExplorationManager.CHAR_WEEK_DATA_NEWEXPLORATION charWeekData = this.GetCharWeekData();
		if ((int)treasureData.bFloor > (int)charWeekData.bBestClearFloor)
		{
			return null;
		}
		if ((int)treasureData.bFloor == (int)charWeekData.bBestClearFloor && (int)treasureData.bSubFloor > (int)charWeekData.bBestClearSubFloor)
		{
			return null;
		}
		return this.GetData(treasureData.bFloor, treasureData.bSubFloor);
	}

	public BATTLE_NEWEXPLORATION_SREWARD GetSReward(int rewardUnique)
	{
		if (!this.m_listSRewardData.ContainsKey(rewardUnique))
		{
			return null;
		}
		return this.m_listSRewardData[rewardUnique];
	}

	public NEWEXPLORATION_RESET_INFO GetResetInfoData(sbyte countIndex)
	{
		if (!this.m_listResetInfoData.ContainsKey((short)countIndex))
		{
			return null;
		}
		return this.m_listResetInfoData[(short)countIndex];
	}

	public sbyte GetMaxResetCount()
	{
		byte vipLevel = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.VipLevel;
		return (sbyte)NrTSingleton<NrTableVipManager>.Instance.GetNewExplorationResetCountByVipLevel(vipLevel);
	}

	public sbyte GetResetCount()
	{
		return this.GetCharDetatilData().bResetCount;
	}

	public void SetAutoBattle(bool bAutoBattle, bool bIfDie_StopAutoBattle, bool bBattleSpeedCheck)
	{
		this.m_bAutoBattle = bAutoBattle;
		this.m_bIfDie_StopAutoBattle = bIfDie_StopAutoBattle;
		this.m_bBattleSpeedCheck = bBattleSpeedCheck;
	}

	public void SetBattlePos(clTempBattlePos[] battlePos)
	{
		this.m_BattlePos = battlePos;
		this.SaveSoldierBatch(this.m_BattlePos);
	}

	public void AutoSellItem()
	{
		if (!this.m_bBattleSpeedCheck)
		{
			return;
		}
		int @int = PlayerPrefs.GetInt(NrPrefsKey.AUTOSELLGRADE, 0);
		int int2 = PlayerPrefs.GetInt(NrPrefsKey.AUTOSELLRANK, 0);
		if (@int > 0 || int2 > 0)
		{
			GS_ITEM_SELL_AUTO_BABEL_REQ gS_ITEM_SELL_AUTO_BABEL_REQ = new GS_ITEM_SELL_AUTO_BABEL_REQ();
			gS_ITEM_SELL_AUTO_BABEL_REQ.i16BabelAutoSellGrade = (short)@int;
			gS_ITEM_SELL_AUTO_BABEL_REQ.i16BabelAutoSellRank = (short)int2;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SELL_AUTO_BABEL_REQ, gS_ITEM_SELL_AUTO_BABEL_REQ);
		}
	}

	public void SaveSoldierBatch(clTempBattlePos[] battlePos)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Length = 0;
		bool flag = true;
		for (int i = 0; i < 5; i++)
		{
			if (this.IsValidBattlePos(battlePos[i]))
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				flag = false;
				stringBuilder.Append(string.Format("{0},{1},{2}", battlePos[i].m_nSolID, battlePos[i].m_nBattlePos, battlePos[i].m_nCharKind));
			}
		}
		PlayerPrefs.SetString(NrPrefsKey.NEWEXPLORATION_BATTLE_POS, stringBuilder.ToString());
	}

	public clTempBattlePos[] LoadSoldierBatch()
	{
		clTempBattlePos[] array = new clTempBattlePos[5];
		string text = PlayerPrefs.GetString(NrPrefsKey.NEWEXPLORATION_BATTLE_POS);
		text = text.TrimEnd(new char[]
		{
			','
		});
		text = text.TrimStart(new char[]
		{
			','
		});
		string[] array2 = text.Split(new char[]
		{
			','
		});
		int num = 0;
		int num2 = 0;
		while (num2 + 3 <= array2.Length && num < 5)
		{
			clTempBattlePos clTempBattlePos = new clTempBattlePos();
			clTempBattlePos.m_nSolID = Convert.ToInt64(array2[num2++]);
			clTempBattlePos.m_nBattlePos = Convert.ToByte(array2[num2++]);
			clTempBattlePos.m_nCharKind = Convert.ToInt32(array2[num2++]);
			if (!this.IsValidBattlePos(clTempBattlePos))
			{
				array[num++] = new clTempBattlePos();
			}
			else
			{
				array[num++] = clTempBattlePos;
			}
		}
		return array;
	}

	public bool IsEmptyOrDieSol()
	{
		int num = 0;
		for (int i = 0; i < this.m_BattlePos.Length; i++)
		{
			if (this.m_BattlePos[i] != null && this.m_BattlePos[i].m_nSolID > 0L && this.m_BattlePos[i].m_nCharKind > 0 && this.m_BattlePos[i].m_nBattlePos > 0)
			{
				NkSoldierInfo soldierInfoBySolID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSoldierInfoBySolID(this.m_BattlePos[i].m_nSolID);
				if (SoldierBatch_SolList.IsNotExcludeSol(soldierInfoBySolID, eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION))
				{
					return false;
				}
				num++;
			}
		}
		return num > 0;
	}

	public bool IsValidBattlePos(clTempBattlePos pos)
	{
		if (pos == null || pos.m_nSolID <= 0L || pos.m_nCharKind <= 0 || pos.m_nBattlePos <= 0)
		{
			return false;
		}
		NkSoldierInfo soldierInfoBySolID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSoldierInfoBySolID(pos.m_nSolID);
		return !SoldierBatch_SolList.IsNotExcludeSol(soldierInfoBySolID, eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION);
	}

	public void ResetInvalidArray()
	{
		for (int i = 0; i < 5; i++)
		{
			if (!this.IsValidBattlePos(this.m_BattlePos[i]))
			{
				this.m_BattlePos[i] = new clTempBattlePos();
			}
		}
	}

	public bool SetAutoBatch()
	{
		this.m_BattlePos = this.LoadSoldierBatch();
		this.ResetInvalidArray();
		clTempBattlePos[] autoBatchPos = SoldierBatch_SolList.GetAutoBatchPos(5, eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION, this.m_BattlePos);
		if (autoBatchPos == null)
		{
			return false;
		}
		this.m_BattlePos = autoBatchPos;
		return true;
	}

	public void UpdateBattleSpeed()
	{
		if (this.m_bBattleSpeedCheck)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null && kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT) <= 0L)
			{
				this.m_bBattleSpeedCheck = false;
			}
		}
	}

	public bool Send_GS_NEWEXPLORATION_START_REQ()
	{
		if (!this.CanPlay(this.GetFloor(), this.GetSubFloor()))
		{
			return false;
		}
		if (!this.IsEmptyOrDieSol())
		{
			if (this.m_bIfDie_StopAutoBattle)
			{
				return false;
			}
			if (!this.SetAutoBatch())
			{
				return false;
			}
		}
		this.UpdateBattleSpeed();
		GS_NEWEXPLORATION_START_REQ gS_NEWEXPLORATION_START_REQ = new GS_NEWEXPLORATION_START_REQ();
		gS_NEWEXPLORATION_START_REQ.i8Floor = this.GetFloor();
		gS_NEWEXPLORATION_START_REQ.i8SubFloor = this.GetSubFloor();
		gS_NEWEXPLORATION_START_REQ.bOnBattleSpeed = this.m_bBattleSpeedCheck;
		gS_NEWEXPLORATION_START_REQ.i32CombinationUnique = NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
		for (int i = 0; i < 5; i++)
		{
			if (!this.IsValidBattlePos(this.m_BattlePos[i]))
			{
				gS_NEWEXPLORATION_START_REQ.i64SolID[i] = 0L;
			}
			else
			{
				gS_NEWEXPLORATION_START_REQ.i64SolID[i] = this.m_BattlePos[i].m_nSolID;
				byte b = 0;
				byte b2 = 0;
				SoldierBatch.GetCalcBattlePos((long)this.m_BattlePos[i].m_nBattlePos, ref b, ref b2);
				gS_NEWEXPLORATION_START_REQ.i8SolPosition[i] = b2;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_START_REQ, gS_NEWEXPLORATION_START_REQ);
		return true;
	}
}
