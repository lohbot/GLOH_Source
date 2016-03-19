using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NkBabelMacroManager : NrTSingleton<NkBabelMacroManager>
{
	private eBABEL_MACRO_STATUS m_eStatus;

	private MACRO_SOLDIERBATCH[] m_BatchUserSol;

	private MACRO_SOLDIERBATCH[] m_FriendBatch;

	private List<USER_FRIEND_INFO> m_kFriendSolList = new List<USER_FRIEND_INFO>();

	private int MYSOL_NUM_MAX;

	private int m_nMyBatchSolNum;

	private float m_fUpdateTime;

	private int m_nMacroCount;

	private float m_fSelectTime;

	private short m_nSelectStage;

	private short m_nSubFloor;

	private short m_nFloorType;

	private bool m_bStop;

	private bool m_bAutoRevive = true;

	private bool m_bBattleSpeedCheck;

	public int MacroCount
	{
		get
		{
			return this.m_nMacroCount;
		}
		set
		{
			this.m_nMacroCount = value;
		}
	}

	public eBABEL_MACRO_STATUS Status
	{
		get
		{
			return this.m_eStatus;
		}
	}

	private NkBabelMacroManager()
	{
	}

	public void Init()
	{
		this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE;
		this.MYSOL_NUM_MAX = 9;
		this.m_BatchUserSol = new MACRO_SOLDIERBATCH[this.MYSOL_NUM_MAX];
		for (int i = 0; i < this.MYSOL_NUM_MAX; i++)
		{
			this.m_BatchUserSol[i] = new MACRO_SOLDIERBATCH();
		}
		this.m_FriendBatch = new MACRO_SOLDIERBATCH[3];
		for (int i = 0; i < 3; i++)
		{
			this.m_FriendBatch[i] = new MACRO_SOLDIERBATCH();
		}
		this.m_nMyBatchSolNum = 0;
		BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
		if (babelTowerMainDlg != null)
		{
			this.m_nFloorType = babelTowerMainDlg.FloorType;
		}
		if (this.m_nFloorType == 0 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_DLG))
		{
			this.m_nFloorType = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELTYPE, 1);
		}
		if (this.m_nFloorType == 2)
		{
			this.m_nSelectStage = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 0);
			this.m_nSubFloor = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, -1);
		}
		else
		{
			this.m_nSelectStage = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 0);
			this.m_nSubFloor = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, -1);
		}
		this.m_bStop = false;
	}

	public bool IsMacro()
	{
		return this.m_eStatus != eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE;
	}

	public bool IsAutoRevive()
	{
		return this.m_bAutoRevive;
	}

	public void SetStatus(eBABEL_MACRO_STATUS eStatus, float fTime)
	{
		this.m_eStatus = eStatus;
		this.m_fSelectTime = fTime;
		if (this.m_eStatus == eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWER_REPEAT_MAIN_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWER_REPEAT_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_RESULT_DLG);
		}
	}

	public void Start(bool bAutoReviveCheck, bool bBattleSpeedCheck)
	{
		if (this.m_eStatus == eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE)
		{
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_INIT;
			this.m_nMacroCount = 0;
			this.m_bAutoRevive = bAutoReviveCheck;
			this.m_bBattleSpeedCheck = bBattleSpeedCheck;
		}
	}

	public void SetStop(bool bStop)
	{
		this.m_bStop = bStop;
	}

	public void Update()
	{
		if (this.m_bStop)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this.m_fUpdateTime < 0.4f)
		{
			return;
		}
		this.m_fUpdateTime = Time.realtimeSinceStartup;
		switch (this.m_eStatus)
		{
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_INIT:
			this.Init();
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_BATTLEPOS;
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_BATTLEPOS:
			if (!this.CheckBattlePos())
			{
				this.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
				return;
			}
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_INJURY;
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_INJURY:
			if (!this.InjuryCureComplete())
			{
				this.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
				return;
			}
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_WAIT_CURE;
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_WAIT_CURE:
			if (!this.WaitCure())
			{
				return;
			}
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_START;
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_START:
			if (!this.StartBabelMacroBattle())
			{
				this.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
				return;
			}
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_ING;
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_SELECT_SPECIAL_RESULT:
			if (Time.realtimeSinceStartup - this.m_fSelectTime > 0.3f && this.m_fSelectTime != 0f)
			{
				Battle_ResultDlg_Content battle_ResultDlg_Content = (Battle_ResultDlg_Content)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG);
				if (battle_ResultDlg_Content != null)
				{
					int iSelectIndex = UnityEngine.Random.Range(0, 4);
					battle_ResultDlg_Content.ClickRewardCardButton(iSelectIndex);
					this.m_fSelectTime = 0f;
				}
			}
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_SELECT_SPECIAL_COMPLETE:
			if (Time.realtimeSinceStartup - this.m_fSelectTime > 1f && this.m_fSelectTime != 0f)
			{
				Battle_ResultDlg_Content battle_ResultDlg_Content2 = (Battle_ResultDlg_Content)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG);
				if (battle_ResultDlg_Content2 != null)
				{
					battle_ResultDlg_Content2.ClickRewardOKButton(null);
					this.m_fSelectTime = 0f;
				}
			}
			break;
		case eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_END:
		{
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			int num = 0;
			if (instance != null)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
				{
					num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT);
				}
				else
				{
					int vipLevelAddBattleRepeat = (int)NrTSingleton<NrTableVipManager>.Instance.GetVipLevelAddBattleRepeat();
					num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT) + vipLevelAddBattleRepeat;
				}
			}
			this.m_nMacroCount++;
			if (this.m_nMacroCount < num)
			{
				this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_BATTLEPOS;
			}
			else
			{
				this.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
			}
			break;
		}
		}
	}

	private bool CheckBattlePos()
	{
		if (this.m_nSelectStage <= 0 || this.m_nSubFloor < 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("614");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		BABELTOWER_DATA babelTowerData = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerData(this.m_nSelectStage, this.m_nSubFloor, this.m_nFloorType);
		if (babelTowerData == null)
		{
			TsLog.LogError("BABELTOWER_DATA == NULL  FloorType ={0} Floor={1} SubFloor={2}", new object[]
			{
				this.m_nFloorType,
				this.m_nSelectStage,
				this.m_nSubFloor
			});
			return false;
		}
		if (!kMyCharInfo.IsEnableBattleUseActivityPoint(babelTowerData.m_nWillSpend))
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		this.m_nMyBatchSolNum = 0;
		byte b;
		if (this.m_BatchUserSol != null)
		{
			b = 0;
			while ((int)b < this.MYSOL_NUM_MAX)
			{
				this.m_BatchUserSol[(int)b].Init();
				b += 1;
			}
		}
		if (this.m_FriendBatch != null)
		{
			for (b = 0; b < 3; b += 1)
			{
				this.m_FriendBatch[(int)b].Init();
			}
		}
		for (b = 1; b <= 20; b += 1)
		{
			if (this.m_nMyBatchSolNum >= this.MYSOL_NUM_MAX)
			{
				break;
			}
			string str = "Babel Solpos";
			if (b >= 17)
			{
				string value = "0";
				PlayerPrefs.SetString(str + b.ToString(), value);
			}
			else
			{
				string @string = PlayerPrefs.GetString(str + b.ToString());
				if (@string != string.Empty)
				{
					long num = long.Parse(@string);
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(num);
					byte b2 = 0;
					byte nGridPos = 0;
					SoldierBatch.GetCalcBattlePos((long)b, ref b2, ref nGridPos);
					if (soldierInfoFromSolID != null)
					{
						if (soldierInfoFromSolID.GetSolPosType() == 1 || soldierInfoFromSolID.GetSolPosType() == 0 || soldierInfoFromSolID.GetSolPosType() == 2 || soldierInfoFromSolID.GetSolPosType() == 6)
						{
							this.m_BatchUserSol[this.m_nMyBatchSolNum].m_nSolID = num;
							this.m_BatchUserSol[this.m_nMyBatchSolNum].m_nGridPos = nGridPos;
							this.m_BatchUserSol[this.m_nMyBatchSolNum].m_bInjury = soldierInfoFromSolID.IsInjuryStatus();
							this.m_nMyBatchSolNum++;
						}
					}
				}
			}
		}
		if (this.m_nMyBatchSolNum < this.MYSOL_NUM_MAX)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("610"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		List<byte> list = new List<byte>();
		for (b = 0; b < 16; b += 1)
		{
			list.Add(b);
		}
		b = 0;
		while ((int)b < this.m_nMyBatchSolNum)
		{
			list.Remove(this.m_BatchUserSol[(int)b].m_nGridPos);
			b += 1;
		}
		this.m_kFriendSolList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID > 0L)
			{
				if (uSER_FRIEND_INFO.ui8HelpUse < 1)
				{
					this.m_kFriendSolList.Add(uSER_FRIEND_INFO);
				}
			}
		}
		this.m_kFriendSolList.Sort(new Comparison<USER_FRIEND_INFO>(this.CompareFriendSolLevel));
		int num2 = 0;
		for (int i = 0; i < this.m_kFriendSolList.Count; i++)
		{
			if (num2 >= 3)
			{
				break;
			}
			USER_FRIEND_INFO uSER_FRIEND_INFO2 = this.m_kFriendSolList[i];
			int index = UnityEngine.Random.Range(0, list.Count);
			byte b3 = list[index];
			this.m_FriendBatch[num2].m_nPersonID = uSER_FRIEND_INFO2.nPersonID;
			this.m_FriendBatch[num2].m_nSolID = uSER_FRIEND_INFO2.FriendHelpSolInfo.i64HelpSolID;
			this.m_FriendBatch[num2].m_nGridPos = b3;
			this.m_FriendBatch[num2].m_bInjury = false;
			list.Remove(b3);
			num2++;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_REPEAT_MAIN_DLG) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_REPEAT_MAIN_DLG);
		}
		return true;
	}

	private int CompareFriendSolLevel(USER_FRIEND_INFO a, USER_FRIEND_INFO b)
	{
		return b.FriendHelpSolInfo.iSolLevel.CompareTo(a.FriendHelpSolInfo.iSolLevel);
	}

	public bool InjuryCureComplete()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		for (int i = 0; i < this.m_nMyBatchSolNum; i++)
		{
			if (this.m_BatchUserSol[i].m_bInjury)
			{
				long nSolID = this.m_BatchUserSol[i].m_nSolID;
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
				if (soldierInfoFromSolID != null)
				{
					if (!soldierInfoFromSolID.IsInjuryStatus())
					{
						this.m_BatchUserSol[i].m_bInjury = false;
					}
					else if (this.m_BatchUserSol[i].m_eRequestInjury == INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE)
					{
						if (soldierInfoFromSolID.InjuryCureByItem())
						{
							this.m_BatchUserSol[i].m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_ITEM;
							return true;
						}
						if (soldierInfoFromSolID.InjuryCureByMoney())
						{
							this.m_BatchUserSol[i].m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_MONEY;
							return true;
						}
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("611"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return false;
					}
				}
			}
		}
		return true;
	}

	public void SetRequestInjury(long nSolID)
	{
		if (this.m_BatchUserSol == null)
		{
			return;
		}
		if (this.m_eStatus == eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		for (int i = 0; i < this.m_nMyBatchSolNum; i++)
		{
			if (this.m_BatchUserSol[i].m_nSolID == nSolID)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
				if (soldierInfoFromSolID != null)
				{
					if (!soldierInfoFromSolID.IsInjuryStatus())
					{
						this.m_BatchUserSol[i].m_bInjury = false;
						this.m_BatchUserSol[i].m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE;
					}
					else if (this.m_BatchUserSol[i].m_eRequestInjury == INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_ITEM)
					{
						this.m_BatchUserSol[i].m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE;
					}
				}
			}
		}
	}

	private bool WaitCure()
	{
		if (this.m_BatchUserSol == null)
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_nMyBatchSolNum; i++)
		{
			if (this.m_BatchUserSol[i].m_nSolID > 0L && this.m_BatchUserSol[i].m_bInjury)
			{
				if (this.m_BatchUserSol[i].m_eRequestInjury != INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (num > 0)
		{
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_WAIT_CURE;
			return false;
		}
		if (num2 > 0)
		{
			this.m_eStatus = eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_CHECK_INJURY;
			return false;
		}
		return true;
	}

	public bool StartBabelMacroBattle()
	{
		GS_BABELTOWER_MACRO_BATTLE_START_REQ gS_BABELTOWER_MACRO_BATTLE_START_REQ = new GS_BABELTOWER_MACRO_BATTLE_START_REQ();
		gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFloorType = this.m_nFloorType;
		gS_BABELTOWER_MACRO_BATTLE_START_REQ.nBabelStage = this.m_nSelectStage;
		gS_BABELTOWER_MACRO_BATTLE_START_REQ.nSubFloor = this.m_nSubFloor;
		gS_BABELTOWER_MACRO_BATTLE_START_REQ.nAutoRevive = ((!this.m_bAutoRevive) ? 0 : 1);
		gS_BABELTOWER_MACRO_BATTLE_START_REQ.nBattleSpeedCheck = ((!this.m_bBattleSpeedCheck) ? 0 : 1);
		for (int i = 0; i < this.MYSOL_NUM_MAX; i++)
		{
			if (this.m_BatchUserSol[i].m_nSolID > 0L)
			{
				if (this.m_BatchUserSol[i].m_bInjury)
				{
					return false;
				}
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nSolID[i] = this.m_BatchUserSol[i].m_nSolID;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nBattlePos[i] = this.m_BatchUserSol[i].m_nGridPos;
				GS_BABELTOWER_MACRO_BATTLE_START_REQ expr_B4 = gS_BABELTOWER_MACRO_BATTLE_START_REQ;
				expr_B4.nMySolCount += 1;
			}
			else
			{
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nSolID[i] = 0L;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nBattlePos[i] = 0;
			}
		}
		for (int i = 0; i < 3; i++)
		{
			if (this.m_FriendBatch[i].m_nSolID > 0L)
			{
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendPersonID[i] = this.m_FriendBatch[i].m_nPersonID;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendSolID[i] = this.m_FriendBatch[i].m_nSolID;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendBattlePos[i] = this.m_FriendBatch[i].m_nGridPos;
				GS_BABELTOWER_MACRO_BATTLE_START_REQ expr_145 = gS_BABELTOWER_MACRO_BATTLE_START_REQ;
				expr_145.nFriendSolCount += 1;
			}
			else
			{
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendPersonID[i] = 0L;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendSolID[i] = 0L;
				gS_BABELTOWER_MACRO_BATTLE_START_REQ.nFriendBattlePos[i] = 0;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_MACRO_BATTLE_START_REQ, gS_BABELTOWER_MACRO_BATTLE_START_REQ);
		Battle.BabelPartyCount = 1;
		return true;
	}
}
