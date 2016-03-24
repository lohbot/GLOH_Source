using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NrMyCharInfo
{
	public NkCoinInfo m_kCoinInfo;

	public long m_PersonID;

	public long m_TID;

	public long m_SN;

	private int m_customerAnswerCount;

	private GameObject m_pkMyCharObject;

	public bool m_bBackupPerson;

	public NrPersonInfoUser m_kBackupPersonInfo;

	public string m_szIntroMsg = string.Empty;

	private long m_nHeroPoint;

	private long m_nEquipPoint;

	private bool m_bGetPortrait;

	private Texture2D m_UserPortrait;

	private bool m_bColosseumMatching;

	private bool m_bTournament;

	private byte[] m_byPushBlock = new byte[3];

	private byte m_i8ConsecutivelyattendanceTotalNum;

	private byte m_i8ConsecutivelyattendanceCurrentNum;

	private byte m_i8ConsecutivelyattendanceRewardType;

	private bool m_bConsecutivelyattendanceReward;

	public NrCharMapInfo m_kCharMapInfo;

	public long DepolyCombatPower;

	private long[] m_nCharSubData = new long[51];

	private long[] m_nCharDetail = new long[31];

	private long[] m_nCharMonthData = new long[1];

	private long[] m_nCharWeekData = new long[2];

	private long[] m_nCharSolGuide = new long[9];

	private long m_lMoney;

	public NrFriendInfo m_kFriendInfo;

	public NrChatMacroInfo m_ChatMacroInfo;

	public bool m_bGameConnected;

	public long m_nActivityPoint;

	public long m_nMaxActivityPoint;

	public float m_fCurrentActivityTime;

	public int m_nVipActivityAddTime;

	public long m_i64LastLoginTime = -1L;

	public long m_i64CreateDate;

	public long m_i64TotalPlayTime;

	public long m_StartTick = (long)Environment.TickCount;

	private E_BF_AUTO_TYPE m_eAutoType;

	private E_BATTLE_CONTINUE_TYPE m_eBattleContinueType = E_BATTLE_CONTINUE_TYPE.CONTINUE;

	public byte m_byUserPlayState = 1;

	private NkReadySolList m_ReadySolList = new NkReadySolList();

	public long m_nFollowCharPersonID;

	public string m_FollowCharName;

	public bool m_bRequestFollowCharPos;

	public int m_nIndunUnique = -1;

	private NkMilitaryList m_kMilitaryList;

	private UserChallengeInfo m_kUserChallengeInfo;

	private NkBabelClearInfo m_kBabelClearInfo;

	private long m_nEquipSellMoney;

	private int m_nPlunderRank;

	private short m_nColosseumGrade;

	private int m_nColosseumGradePoint;

	private short m_nColosseumOldGrade;

	private int m_nColosseumOldRank;

	private int m_nColosseumWinCount;

	private int m_nChatChannel;

	private byte m_bRecommend_RecvMaxCount;

	private byte m_bRecommend_RecvCurrnetCount;

	private byte m_bRecommend_SendMaxCount;

	private byte m_bRecommend_SendCurrentCount;

	private int m_i32InfinityBattle_Rank;

	private int m_i32InfinityBattle_OldRank;

	private int m_i32InfiBattleStraightWin;

	private int m_iInfiBattleCharLevel;

	private byte m_bInfiBattleReward = 1;

	private int m_i32InfinityBattle_TotalCount;

	private int m_i32InfinityBattle_WinCount;

	private long m_nPlunderMoney;

	private string m_szPlunderCharName = string.Empty;

	private int m_nPlunderCharLevel;

	private NrSolWarehouse m_SolWarehouse;

	private NrColosseum_MyGrade_UserIList m_Colosseum_GradeUserList;

	private NrGuildBoss_MyRoomInfoList m_GuildBoss_MyRoomInfo;

	private List<int> m_TreasureMap = new List<int>();

	private int m_i32HP_Auth;

	private byte m_byHP_AuthRequest;

	private bool m_bEquipMoneyAttackPlunder;

	private List<BOUNTYHUNT_CLEARINFO> m_BountyHuntClearInfo = new List<BOUNTYHUNT_CLEARINFO>();

	private short m_iBountyHuntUnique;

	private List<COLOSSEUM_SUPPORTSOLDIER> m_ColosseumSupportSoldier = new List<COLOSSEUM_SUPPORTSOLDIER>();

	private List<int> m_ColosseumEnableBatchSoldierKind = new List<int>();

	public int m_nColosseumBatchKindTotal;

	private List<VOUCHER_DATA> m_VoucharData = new List<VOUCHER_DATA>();

	private NrTimeShopInfo m_kTimeShopInfo;

	public string m_szServerName
	{
		get;
		set;
	}

	public string m_szChannel
	{
		get;
		set;
	}

	public string m_szWorldType
	{
		get;
		set;
	}

	public bool m_bNoMove
	{
		get;
		set;
	}

	public bool UserPortrait
	{
		get
		{
			return this.m_bGetPortrait;
		}
		set
		{
			this.m_bGetPortrait = value;
		}
	}

	public string IntroMsg
	{
		get
		{
			return this.m_szIntroMsg;
		}
		set
		{
			this.m_szIntroMsg = value;
		}
	}

	public bool ColosseumMatching
	{
		get
		{
			return this.m_bColosseumMatching;
		}
		set
		{
			this.m_bColosseumMatching = value;
			if (!value)
			{
				this.m_bTournament = false;
			}
		}
	}

	public bool Tournament
	{
		get
		{
			return this.m_bTournament;
		}
		set
		{
			this.m_bTournament = value;
		}
	}

	public byte[] PushBlock
	{
		get
		{
			return this.m_byPushBlock;
		}
		set
		{
			this.m_byPushBlock = value;
		}
	}

	public byte ConsecutivelyattendanceTotalNum
	{
		get
		{
			return this.m_i8ConsecutivelyattendanceTotalNum;
		}
		set
		{
			this.m_i8ConsecutivelyattendanceTotalNum = value;
		}
	}

	public byte ConsecutivelyattendanceCurrentNum
	{
		get
		{
			return this.m_i8ConsecutivelyattendanceCurrentNum;
		}
		set
		{
			this.m_i8ConsecutivelyattendanceCurrentNum = value;
		}
	}

	public bool ConsecutivelyattendanceReward
	{
		get
		{
			return this.m_bConsecutivelyattendanceReward;
		}
		set
		{
			this.m_bConsecutivelyattendanceReward = value;
		}
	}

	public byte ConsecutivelyattendanceRewardType
	{
		get
		{
			return this.m_i8ConsecutivelyattendanceRewardType;
		}
		set
		{
			this.m_i8ConsecutivelyattendanceRewardType = value;
		}
	}

	public Texture2D UserPortraitTexture
	{
		get
		{
			return this.m_UserPortrait;
		}
		set
		{
			this.m_UserPortrait = value;
		}
	}

	public int CustomerAnswerCount
	{
		get
		{
			return this.m_customerAnswerCount;
		}
		set
		{
			this.m_customerAnswerCount = value;
		}
	}

	public long m_Money
	{
		get
		{
			return this.m_lMoney;
		}
		set
		{
			this.m_lMoney = value;
			if (Protocol_Item.s_deMoneyDelegate != null)
			{
				Protocol_Item.s_deMoneyDelegate();
			}
		}
	}

	public byte VipLevel
	{
		get
		{
			long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			return NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		}
	}

	public byte Recommend_RecvMaxCount
	{
		get
		{
			return this.m_bRecommend_RecvMaxCount;
		}
	}

	public byte Recommend_RecvCurrnetCount
	{
		get
		{
			return this.m_bRecommend_RecvCurrnetCount;
		}
	}

	public byte Recommend_SendMaxCount
	{
		get
		{
			return this.m_bRecommend_SendMaxCount;
		}
	}

	public byte Recommend_SendCurrentCount
	{
		get
		{
			return this.m_bRecommend_SendCurrentCount;
		}
	}

	public int PlunderRank
	{
		get
		{
			return this.m_nPlunderRank;
		}
		set
		{
			this.m_nPlunderRank = value;
		}
	}

	public short ColosseumGrade
	{
		get
		{
			return this.m_nColosseumGrade;
		}
		set
		{
			this.m_nColosseumGrade = value;
		}
	}

	public int ColosseumGradePoint
	{
		get
		{
			return this.m_nColosseumGradePoint;
		}
		set
		{
			this.m_nColosseumGradePoint = value;
		}
	}

	public short ColosseumOldGrade
	{
		get
		{
			return this.m_nColosseumOldGrade;
		}
		set
		{
			this.m_nColosseumOldGrade = value;
		}
	}

	public int ColosseumOldRank
	{
		get
		{
			return this.m_nColosseumOldRank;
		}
		set
		{
			this.m_nColosseumOldRank = value;
		}
	}

	public int ColosseumWinCount
	{
		get
		{
			return this.m_nColosseumWinCount;
		}
		set
		{
			this.m_nColosseumWinCount = value;
		}
	}

	public long PlunderMoney
	{
		get
		{
			return this.m_nPlunderMoney;
		}
		set
		{
			this.m_nPlunderMoney = value;
		}
	}

	public string PlunderCharName
	{
		get
		{
			return this.m_szPlunderCharName;
		}
		set
		{
			this.m_szPlunderCharName = value;
		}
	}

	public int PlunderCharLevel
	{
		get
		{
			return this.m_nPlunderCharLevel;
		}
		set
		{
			this.m_nPlunderCharLevel = value;
		}
	}

	public int ChatChannel
	{
		get
		{
			return this.m_nChatChannel;
		}
		set
		{
			this.m_nChatChannel = value;
		}
	}

	public int InfinityBattle_Rank
	{
		get
		{
			return this.m_i32InfinityBattle_Rank;
		}
		set
		{
			this.m_i32InfinityBattle_Rank = value;
		}
	}

	public int InfinityBattle_OldRank
	{
		get
		{
			return this.m_i32InfinityBattle_OldRank;
		}
		set
		{
			this.m_i32InfinityBattle_OldRank = value;
		}
	}

	public int InfiBattleStraightWin
	{
		get
		{
			return this.m_i32InfiBattleStraightWin;
		}
		set
		{
			this.m_i32InfiBattleStraightWin = value;
		}
	}

	public byte InfiBattleReward
	{
		get
		{
			return this.m_bInfiBattleReward;
		}
		set
		{
			this.m_bInfiBattleReward = value;
		}
	}

	public int InifBattle_TotalCount
	{
		get
		{
			return this.m_i32InfinityBattle_TotalCount;
		}
		set
		{
			this.m_i32InfinityBattle_TotalCount = value;
		}
	}

	public int InifBattle_WinCount
	{
		get
		{
			return this.m_i32InfinityBattle_WinCount;
		}
		set
		{
			this.m_i32InfinityBattle_WinCount = value;
		}
	}

	public int InfiBattleCharLevel
	{
		get
		{
			return this.m_iInfiBattleCharLevel;
		}
		set
		{
			this.m_iInfiBattleCharLevel = value;
		}
	}

	public int HP_Auth
	{
		get
		{
			return this.m_i32HP_Auth;
		}
		set
		{
			this.m_i32HP_Auth = value;
		}
	}

	public byte HP_AuthRequest
	{
		get
		{
			return this.m_byHP_AuthRequest;
		}
		set
		{
			this.m_byHP_AuthRequest = value;
		}
	}

	public bool EquipMoneyAttackPlunder
	{
		get
		{
			return this.m_bEquipMoneyAttackPlunder;
		}
		set
		{
			this.m_bEquipMoneyAttackPlunder = value;
		}
	}

	public short BountyHuntUnique
	{
		get
		{
			return this.m_iBountyHuntUnique;
		}
		set
		{
			this.m_iBountyHuntUnique = value;
		}
	}

	public long NextRefreshTime
	{
		get
		{
			return this.m_kTimeShopInfo.RefreshTime;
		}
	}

	public short RefreshCount
	{
		get
		{
			return this.m_kTimeShopInfo.RefreshCount;
		}
	}

	public NrMyCharInfo()
	{
		this.m_kCoinInfo = new NkCoinInfo();
		this.m_kFriendInfo = new NrFriendInfo();
		this.m_kCharMapInfo = new NrCharMapInfo();
		this.m_ChatMacroInfo = new NrChatMacroInfo();
		this.m_kMilitaryList = new NkMilitaryList();
		this.m_kUserChallengeInfo = new UserChallengeInfo();
		this.m_kBabelClearInfo = new NkBabelClearInfo();
		this.m_SolWarehouse = new NrSolWarehouse();
		this.m_Colosseum_GradeUserList = new NrColosseum_MyGrade_UserIList();
		this.m_GuildBoss_MyRoomInfo = new NrGuildBoss_MyRoomInfoList();
		this.m_TreasureMap = new List<int>();
		this.m_kTimeShopInfo = new NrTimeShopInfo();
		this.Init();
	}

	public void SetHeroPoint(long point)
	{
		this.m_nHeroPoint = point;
	}

	public long GetHeroPoint()
	{
		return this.m_nHeroPoint;
	}

	public void SetEquipPoint(long point)
	{
		this.m_nEquipPoint = point;
	}

	public long GetEquipPoint()
	{
		return this.m_nEquipPoint;
	}

	public void GetUserPortrait(bool bRefresh)
	{
		string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(this.m_PersonID);
		if (bRefresh)
		{
			WebFileCache.RemoveEventItem(userPortraitURL);
		}
		WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserImageCallback), bRefresh);
	}

	private void ReqWebUserImageCallback(Texture2D txtr, object _param)
	{
		bool flag = (bool)_param;
		if (txtr != null)
		{
			this.m_UserPortrait = txtr;
		}
		else
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
			if (leaderSoldierInfo == null)
			{
				return;
			}
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(leaderSoldierInfo.GetCharKind());
			if (charKindInfo == null)
			{
				return;
			}
			string imageKey = charKindInfo.GetPortraitFile1((int)leaderSoldierInfo.GetGrade(), string.Empty) + "_64";
			this.m_UserPortrait = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey);
		}
		if (flag)
		{
			SolMilitaryGroupDlg solMilitaryGroupDlg = (SolMilitaryGroupDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG);
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.SetPortraitRefresh();
			}
			StoryChatDlg storyChatDlg = (StoryChatDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG);
			if (storyChatDlg != null)
			{
				storyChatDlg.UpdateUserPersonID(this.m_PersonID);
			}
		}
	}

	public int GetLevel()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			return charPersonInfo.GetLevel(0L);
		}
		return 0;
	}

	public long GetExp()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			return (long)charPersonInfo.GetLevel(0L);
		}
		return 0L;
	}

	public Texture2D GetFriendTexture(long i64PersonID)
	{
		Texture2D result = null;
		if (this.m_kFriendInfo != null)
		{
			result = this.m_kFriendInfo.GetPersonIDTexture(i64PersonID);
		}
		return result;
	}

	public void Init()
	{
		this.m_PersonID = 0L;
		this.m_TID = 0L;
		this.m_pkMyCharObject = null;
		this.m_bNoMove = false;
		this.m_bBackupPerson = false;
		this.m_kBackupPersonInfo = null;
		this.m_kFriendInfo.Init();
		this.m_kCharMapInfo.Init();
		this.m_ChatMacroInfo.Init();
		this.m_kMilitaryList.Init();
		this.m_kUserChallengeInfo.Init();
		this.m_kBabelClearInfo.Init();
		this.m_ReadySolList.Init();
		this.m_SolWarehouse.Clear();
		this.m_kTimeShopInfo.Init();
		this.m_Money = 0L;
		this.m_nActivityPoint = 0L;
		this.m_nVipActivityAddTime = 0;
		this.m_nFollowCharPersonID = 0L;
		this.m_FollowCharName = string.Empty;
		this.m_bRequestFollowCharPos = false;
		this.DepolyCombatPower = 0L;
		this.m_nIndunUnique = -1;
		this.m_nEquipSellMoney = 0L;
		this.m_bColosseumMatching = false;
		this.m_nChatChannel = 0;
		this.m_eAutoType = E_BF_AUTO_TYPE.MANUAL;
		this.InitCharSubData();
		this.InitCharDetail();
	}

	public void SetMyCharObject(GameObject objUser)
	{
		this.m_pkMyCharObject = objUser;
		NrTSingleton<NrMainSystem>.Instance.m_bSendPing = true;
	}

	public GameObject GetMyCharObject()
	{
		return this.m_pkMyCharObject;
	}

	public void InitCharSubData()
	{
		for (int i = 0; i < 51; i++)
		{
			this.m_nCharSubData[i] = 0L;
		}
		this.InitCharSolGuide();
	}

	public long GetCharSolGuide(int type)
	{
		long result = 0L;
		int num = 9;
		if (0 <= type && type < num)
		{
			result = this.m_nCharSolGuide[type];
		}
		return result;
	}

	public void InitCharSolGuide()
	{
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			this.m_nCharSolGuide[i] = 0L;
		}
	}

	public void SetCharSolGuide(int i32KindChar)
	{
		ICollection solGuide_Col = NrTSingleton<NrBaseTableManager>.Instance.GetSolGuide_Col();
		if (solGuide_Col == null)
		{
			return;
		}
		foreach (SOL_GUIDE sOL_GUIDE in solGuide_Col)
		{
			if (sOL_GUIDE.m_i32CharKind == i32KindChar)
			{
				int type = (int)(8 + sOL_GUIDE.m_bFlagSet - 1);
				if ((this.GetCharSubData(type) & 1L << (int)sOL_GUIDE.m_bFlagSetCount) == 0L)
				{
					this.m_nCharSolGuide[(int)(sOL_GUIDE.m_bFlagSet - 1)] |= 1L << (int)sOL_GUIDE.m_bFlagSetCount;
				}
			}
		}
	}

	public void SetCharSubData(int type, long value)
	{
		if (type < 0 || type >= 51)
		{
			return;
		}
		this.m_nCharSubData[type] = value;
		if (type == 0)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				int faceCharKind = this.GetFaceCharKind();
				byte faceSolGrade = this.GetFaceSolGrade();
				long faceSolID = this.GetFaceSolID();
				int faceCostumeUnique = this.GetFaceCostumeUnique();
				nrCharUser.ChangeCharModel(faceCharKind, faceSolGrade, faceSolID, faceCostumeUnique);
			}
		}
		if (type == 6 && value > 0L)
		{
			Social.ReportScore(value, "CgkIzeOZyPQCEAIQIw", delegate(bool success)
			{
			});
		}
	}

	public long GetCharSubData(eCHAR_SUBDATA eType)
	{
		return this.GetCharSubData((int)eType);
	}

	public long GetCharSubData(int type)
	{
		if (type < 0 || type >= 51)
		{
			return 0L;
		}
		return this.m_nCharSubData[type];
	}

	public void ResultCharSubData(int datatype, long datavalue, long i64Befordatavalue)
	{
		if (datatype == 1)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle() && this.GetCharSubData(1) != 0L)
			{
				NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.LOGIN, GameGuideType.SUPPORT_GOLD);
			}
		}
		else if (datatype == 0)
		{
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.ActionChangeFaceChar(datavalue);
				solMilitaryGroupDlg.RefreshSolList();
			}
		}
		else if (datatype == 2)
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPush(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME, 0L);
		}
		else if (datatype >= 8 && datatype < 17)
		{
			this.SetCharSubData(datatype, datavalue);
		}
		else if (datatype == 19)
		{
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ITEMMALL_DLG))
			{
				ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
				if (itemMallDlg != null)
				{
					itemMallDlg.RefreshData();
				}
			}
			if (datavalue > 0L)
			{
				ExpBoosterDlg expBoosterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_EXPBOOSTER_DLG) as ExpBoosterDlg;
				if (expBoosterDlg != null)
				{
					expBoosterDlg.RefreshData();
				}
			}
		}
		else if (datatype == 28)
		{
			this.SetSubData_Waring(datatype, datavalue);
		}
		else if (datatype == 37)
		{
			this.SetActivityTime(PublicMethod.GetCurTime());
			byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)i64Befordatavalue));
			byte levelExp2 = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)datavalue));
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.SOLRECRUITSUCCESS_DLG) && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend() && levelExp < levelExp2)
			{
				SolRecruitSuccessDlg solRecruitSuccessDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUITSUCCESS_DLG) as SolRecruitSuccessDlg;
				if (solRecruitSuccessDlg != null)
				{
					solRecruitSuccessDlg.SetVipTextShow();
					this.SetActivityMax();
					return;
				}
			}
			ItemMallDlg itemMallDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
			if (itemMallDlg2 != null)
			{
				if (levelExp < levelExp2)
				{
					itemMallDlg2.SetVipInfoShow(levelExp2, true);
				}
				else
				{
					itemMallDlg2.SetVipData();
				}
			}
			this.SetActivityMax();
		}
		else if (datatype == 50)
		{
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetFloorList();
			}
		}
	}

	public int GetFaceCharKind()
	{
		long solID = this.m_nCharSubData[0];
		NkSoldierInfo leaderSolInfo = NrTSingleton<NkClientLogic>.Instance.GetLeaderSolInfo(solID);
		if (leaderSolInfo == null || leaderSolInfo.IsLeader() || !leaderSolInfo.IsValid())
		{
			return 0;
		}
		return leaderSolInfo.GetCharKind();
	}

	public byte GetFaceSolGrade()
	{
		long solID = this.m_nCharSubData[0];
		NkSoldierInfo leaderSolInfo = NrTSingleton<NkClientLogic>.Instance.GetLeaderSolInfo(solID);
		if (leaderSolInfo == null || !leaderSolInfo.IsValid())
		{
			return 0;
		}
		return leaderSolInfo.GetGrade();
	}

	public long GetFaceSolID()
	{
		long solID = this.m_nCharSubData[0];
		NkSoldierInfo leaderSolInfo = NrTSingleton<NkClientLogic>.Instance.GetLeaderSolInfo(solID);
		if (leaderSolInfo == null || !leaderSolInfo.IsValid())
		{
			return 0L;
		}
		return leaderSolInfo.GetSolID();
	}

	public int GetFaceCostumeUnique()
	{
		long solID = this.m_nCharSubData[0];
		NkSoldierInfo leaderSolInfo = NrTSingleton<NkClientLogic>.Instance.GetLeaderSolInfo(solID);
		if (leaderSolInfo == null || !leaderSolInfo.IsValid())
		{
			return 0;
		}
		return (int)leaderSolInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
	}

	public int GetImgFaceCharKind()
	{
		long solID = this.m_nCharSubData[0];
		NkSoldierInfo leaderSolInfo = NrTSingleton<NkClientLogic>.Instance.GetLeaderSolInfo(solID);
		if (leaderSolInfo == null || !leaderSolInfo.IsValid())
		{
			return 0;
		}
		return leaderSolInfo.GetCharKind();
	}

	public void InitCharDetail()
	{
		for (int i = 0; i < 31; i++)
		{
			this.m_nCharDetail[i] = 0L;
		}
	}

	public void SetCharDetail(int type, long value)
	{
		if (type < 0 || type >= 31)
		{
			return;
		}
		this.m_nCharDetail[type] = value;
	}

	public long GetCharDetail(int type)
	{
		if (type < 0 || type >= 31)
		{
			return 0L;
		}
		return this.m_nCharDetail[type];
	}

	public sbyte GetDayCharDetail(eCHAR_DAY_COUNT type)
	{
		if (type <= eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_INVITE_KAKAO_COUNT)
		{
			SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
			sUBDATA_UNION.nSubData = this.m_nCharDetail[17];
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_JOIN_EXIEDITION_COUNT)
			{
				return sUBDATA_UNION.n8SubData_0;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_JOIN_COLOSSEUM_COUNT)
			{
				return sUBDATA_UNION.n8SubData_1;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_USE_WILL_COUNT)
			{
				return sUBDATA_UNION.n8SubData_2;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_REQUEST_FRIENDSOL_COUNT)
			{
				return sUBDATA_UNION.n8SubData_3;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_REPLAY_BATTLE_COUNT)
			{
				return sUBDATA_UNION.n8SubData_4;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_JOIN_MINE_COUNT)
			{
				return sUBDATA_UNION.n8SubData_5;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_JOIN_GUILDBOSS_COUNT)
			{
				return sUBDATA_UNION.n8SubData_6;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_INVITE_KAKAO_COUNT)
			{
				return sUBDATA_UNION.n8SubData_7;
			}
		}
		else if (type <= eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_HEROWAR_COUNT)
		{
			SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
			sUBDATA_UNION2.nSubData = this.m_nCharDetail[18];
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_BABELTOWER_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_0;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_BABELTOWER3_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_1;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_BABELTOWER4_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_2;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_BABELTOWER5_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_3;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_BABELTOWER6_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_4;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_CLEAR_DAILYDUNGEON)
			{
				return sUBDATA_UNION2.n8SubData_5;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_COLOSSEUM_WITHPLAYER_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_6;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_WIN_HEROWAR_COUNT)
			{
				return sUBDATA_UNION2.n8SubData_7;
			}
		}
		else
		{
			SUBDATA_UNION sUBDATA_UNION3 = default(SUBDATA_UNION);
			sUBDATA_UNION3.nSubData = this.m_nCharDetail[25];
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_ENJOY_INFIBATTLE)
			{
				return sUBDATA_UNION3.n8SubData_0;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_ENJOY_BOUNT_HUNT)
			{
				return sUBDATA_UNION3.n8SubData_1;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_ENJOY_BABELTOWER_COUNT)
			{
				return sUBDATA_UNION3.n8SubData_2;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_ENJOY_MYTHRAID_COUNT)
			{
				return sUBDATA_UNION3.n8SubData_3;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_COUNT_TIMESHOP_REFRESH_COUNT)
			{
				return sUBDATA_UNION3.n8SubData_4;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_DAILYDUNGEON)
			{
				return sUBDATA_UNION3.n8SubData_5;
			}
			if (type == eCHAR_DAY_COUNT.eCHAR_DAY_NEWEXPLORATION)
			{
				return sUBDATA_UNION3.n8SubData_6;
			}
		}
		return 0;
	}

	public short GetCharDetailFromUnion(eCHAR_DETAIL_INFO eDetailType, int eType)
	{
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.m_nCharDetail[(int)eDetailType];
		if (eType == 0)
		{
			return sUBDATA_UNION.n16SubData_0;
		}
		if (eType == 1)
		{
			return sUBDATA_UNION.n16SubData_1;
		}
		if (eType == 2)
		{
			return sUBDATA_UNION.n16SubData_2;
		}
		if (eType == 3)
		{
			return sUBDATA_UNION.n16SubData_3;
		}
		return 0;
	}

	public void ResultCharDetail(int datatype, long datavalue)
	{
		switch (datatype)
		{
		case 22:
		{
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.SetDailyMailCount();
			}
			return;
		}
		case 23:
		{
			MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
			if (myCharInfoDlg != null)
			{
				myCharInfoDlg.Attend_Notice_Show();
			}
			return;
		}
		case 24:
		{
			IL_1D:
			if (datatype == 5)
			{
				if (0L < datavalue)
				{
					MyCharInfoDlg myCharInfoDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
					if (myCharInfoDlg2 != null)
					{
						myCharInfoDlg2.UpdateNoticeInfo();
					}
				}
				return;
			}
			if (datatype == 12)
			{
				NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
				ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
				if (challengeDlg != null)
				{
					challengeDlg.SetChallengeInfo();
				}
				return;
			}
			if (datatype != 30)
			{
				return;
			}
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetInfo();
			}
			return;
		}
		case 25:
		{
			NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
			TimeShop_DLG timeShop_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TIMESHOP_DLG) as TimeShop_DLG;
			if (timeShop_DLG != null)
			{
				timeShop_DLG.Set_RewardButton();
			}
			return;
		}
		}
		goto IL_1D;
	}

	public void InitCharWeekData()
	{
		for (int i = 0; i < 2; i++)
		{
			this.m_nCharWeekData[i] = 0L;
		}
	}

	public void SetCharWeekData(int type, long value)
	{
		if (type < 0 || type >= 2)
		{
			return;
		}
		this.m_nCharWeekData[type] = value;
	}

	public long GetCharWeekData(int type)
	{
		if (type < 0 || type >= 2)
		{
			return 0L;
		}
		return this.m_nCharWeekData[type];
	}

	public void InitCharMonthData()
	{
		for (int i = 0; i < 1; i++)
		{
			this.m_nCharMonthData[i] = 0L;
		}
	}

	public void SetCharMonthData(int type, long value)
	{
		if (type < 0 || type >= 1)
		{
			return;
		}
		this.m_nCharMonthData[type] = value;
	}

	public long GetCharMonthData(int type)
	{
		if (type < 0 || type >= 1)
		{
			return 0L;
		}
		return this.m_nCharMonthData[type];
	}

	public long ChangeMonthDataToDay(long DayData)
	{
		if (DayData <= 0L || DayData > 30L)
		{
			return 0L;
		}
		int num = 1 << (int)DayData - 1;
		return (long)num;
	}

	public UserChallengeInfo GetUserChallengeInfo()
	{
		return this.m_kUserChallengeInfo;
	}

	public void BackupPersonInfo(NrPersonInfoUser pkPersonInfo)
	{
		if (this.m_kBackupPersonInfo == null)
		{
			this.m_kBackupPersonInfo = new NrPersonInfoUser();
		}
		this.m_kBackupPersonInfo.SetPersonInfo(pkPersonInfo);
		this.m_bBackupPerson = true;
	}

	public void SetBackupPersonInfo(bool BackupPerson)
	{
		this.m_bBackupPerson = BackupPerson;
	}

	public bool IsBackupPersonInfo()
	{
		return this.m_kBackupPersonInfo != null && this.m_bBackupPerson;
	}

	public int GetWeaponType()
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return 0;
		}
		return nrCharUser.GetWeaponType();
	}

	public int GetLevelForSolPosIndex(int solindex)
	{
		int result = 1;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			switch (solindex)
			{
			case 1:
				result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEVEL_FORSOLINDEX1);
				break;
			case 2:
				result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEVEL_FORSOLINDEX2);
				break;
			case 3:
				result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEVEL_FORSOLINDEX3);
				break;
			case 4:
				result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEVEL_FORSOLINDEX4);
				break;
			case 5:
				result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEVEL_FORSOLINDEX5);
				break;
			}
		}
		return result;
	}

	public bool IsAddBattleSoldier(int solindex)
	{
		if (COMMON_CONSTANT_Manager.GetInstance() == null)
		{
			return false;
		}
		int level = this.GetLevel();
		int levelForSolPosIndex = this.GetLevelForSolPosIndex(solindex);
		return level >= levelForSolPosIndex;
	}

	public void SetActivityPoint(long ActivityPoint)
	{
		this.m_nActivityPoint = ActivityPoint;
	}

	public void AddActivityPoint(long AddPoint)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		this.m_nActivityPoint += AddPoint;
		if (this.m_nActivityPoint < 0L)
		{
			this.m_nActivityPoint = 0L;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (this.m_nActivityPoint > num)
		{
			this.m_nActivityPoint = num;
		}
	}

	public void SetEquipSellMoney(long nEquipSellMoney, bool bAttackPlunder)
	{
		this.m_nEquipSellMoney = nEquipSellMoney;
		this.EquipMoneyAttackPlunder = bAttackPlunder;
		if (0L < this.m_nEquipSellMoney)
		{
			NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_SELL);
		}
	}

	public long GetEquipSellMoney()
	{
		return this.m_nEquipSellMoney;
	}

	public void SetActivityPointMax(long ActivityPoint, long MaxActivityPoint)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		if (this.m_nActivityPoint != 0L && this.m_nActivityPoint > ActivityPoint)
		{
			NrTSingleton<FiveRocksEventManager>.Instance.Placement("activity_spend");
		}
		this.m_nActivityPoint = ActivityPoint;
		this.m_nMaxActivityPoint = MaxActivityPoint;
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		this.m_nActivityPoint = ((this.m_nActivityPoint <= num) ? this.m_nActivityPoint : num);
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg != null)
		{
			myCharInfoDlg.Update();
		}
		NrTSingleton<NkLocalPushManager>.Instance.SetPush(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME, 0L);
	}

	public void SetActivityTime(long ServerTime)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		float num = 600f;
		short vipLevelActivityTime = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelActivityTime();
		if (instance != null)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
			{
				num = (float)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) * 60f;
			}
			else
			{
				num = (float)(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) - (int)vipLevelActivityTime) * 60f;
			}
		}
		if (ServerTime != 0L)
		{
			this.m_fCurrentActivityTime = Time.realtimeSinceStartup + num;
			long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
			if (levelExp <= 0 || NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
			{
				long minuteFromSec = PublicMethod.GetMinuteFromSec(ServerTime);
				long num2 = ServerTime % 60L;
				this.m_fCurrentActivityTime -= (float)minuteFromSec % (float)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_UPDATE_ACTIVITY_MINUTE) * 60f + (float)num2;
			}
			else
			{
				this.m_fCurrentActivityTime -= (float)this.m_nVipActivityAddTime;
			}
			NrTSingleton<NkLocalPushManager>.Instance.SetPush(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME, 0L);
		}
	}

	public void SetActivityMax()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
			{
				this.m_nMaxActivityPoint = (long)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BASE_ACTIVITY);
			}
			else
			{
				byte vipLevelActivityPointMax = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelActivityPointMax();
				this.m_nMaxActivityPoint = (long)(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BASE_ACTIVITY) + (int)vipLevelActivityPointMax);
			}
		}
		else
		{
			this.m_nMaxActivityPoint = (long)(this.GetLevel() / 2 + 20);
		}
	}

	public void SetVipActivityAddTime(int addtime)
	{
		this.m_nVipActivityAddTime = addtime;
		this.SetActivityTime(PublicMethod.GetCurTime());
	}

	public void RefreshVipActivityAddTime()
	{
		this.SetVipActivityAddTime(this.m_nVipActivityAddTime);
	}

	public bool IsEnableBattleUseActivityPoint(short WillSpend = 1)
	{
		return !NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend() || (this.m_nActivityPoint > 0L && this.m_nActivityPoint >= this.GetActivityPointUseBattle() * (long)WillSpend);
	}

	public long GetActivityPointUseBattle()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			return (long)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_USE_BATTLE_ACTIVITY);
		}
		return 1L;
	}

	public NkReadySolList GetReadySolList()
	{
		return this.m_ReadySolList;
	}

	public NkSoldierInfo GetReadySoldierInfoBySolID(long i64SolID)
	{
		return this.m_ReadySolList.GetSolInfo(i64SolID);
	}

	public List<int> GetReadySolKindList()
	{
		if (this.m_ReadySolList == null)
		{
			return null;
		}
		return this.m_ReadySolList.GetReadySolKindList();
	}

	public void UpdateReadySoldierInfo()
	{
		this.m_ReadySolList.UpdateSoldierInfo();
	}

	public void SetBattleContinueType(E_BATTLE_CONTINUE_TYPE eType)
	{
		this.m_eBattleContinueType = eType;
	}

	public E_BATTLE_CONTINUE_TYPE GetBattleContinueType()
	{
		return this.m_eBattleContinueType;
	}

	public void SetAutoBattle(E_BF_AUTO_TYPE eType)
	{
		this.m_eAutoType = eType;
	}

	public E_BF_AUTO_TYPE GetAutoBattle()
	{
		return this.m_eAutoType;
	}

	public NkMilitaryList GetMilitaryList()
	{
		return this.m_kMilitaryList;
	}

	public bool IsMineMilitaryAction()
	{
		NkReadySolList readySolList = this.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolPosType() == 2)
			{
				return true;
			}
		}
		return false;
	}

	public void OnFightOK(object a_oObject)
	{
		GS_BATTLE_OPEN_FIGHT_REQ gS_BATTLE_OPEN_FIGHT_REQ = a_oObject as GS_BATTLE_OPEN_FIGHT_REQ;
		if (gS_BATTLE_OPEN_FIGHT_REQ != null)
		{
			gS_BATTLE_OPEN_FIGHT_REQ.nAllow = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_OPEN_FIGHT_REQ, gS_BATTLE_OPEN_FIGHT_REQ);
		}
	}

	public void OnFightCancle(object a_oObject)
	{
		GS_BATTLE_OPEN_FIGHT_REQ gS_BATTLE_OPEN_FIGHT_REQ = a_oObject as GS_BATTLE_OPEN_FIGHT_REQ;
		if (gS_BATTLE_OPEN_FIGHT_REQ != null)
		{
			gS_BATTLE_OPEN_FIGHT_REQ.nAllow = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_OPEN_FIGHT_REQ, gS_BATTLE_OPEN_FIGHT_REQ);
		}
	}

	public void OnFightPatchCancle(object a_oObject)
	{
		GS_BATTLE_FIGHT_ALLOW_ACK gS_BATTLE_FIGHT_ALLOW_ACK = (GS_BATTLE_FIGHT_ALLOW_ACK)a_oObject;
		GS_BATTLE_OPEN_FIGHT_REQ gS_BATTLE_OPEN_FIGHT_REQ = new GS_BATTLE_OPEN_FIGHT_REQ();
		gS_BATTLE_OPEN_FIGHT_REQ.nCharUnique = gS_BATTLE_FIGHT_ALLOW_ACK.nCharUnique;
		gS_BATTLE_OPEN_FIGHT_REQ.nPersonID = gS_BATTLE_FIGHT_ALLOW_ACK.nPersonID;
		gS_BATTLE_OPEN_FIGHT_REQ.nAllow = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_OPEN_FIGHT_REQ, gS_BATTLE_OPEN_FIGHT_REQ);
	}

	public void AddBabelClearInfo(BABEL_CLEARINFO info)
	{
		this.m_kBabelClearInfo.AddBabelClearInfo(info);
	}

	public void SetBabelClearInfo(byte column, long clearinfo, short floortype)
	{
		this.m_kBabelClearInfo.SetBabelClearInfo(column, clearinfo, floortype);
	}

	public void AddBabelSubFloorRankInfo(BABEL_SUBFLOOR_RANKINFO info)
	{
		this.m_kBabelClearInfo.AddBabelSubFloorRankInfo(info);
	}

	public void SetBabelSubFloorRankInfo(short floor, byte subfloor, byte rank, bool bTreasure, short floortype)
	{
		this.m_kBabelClearInfo.SetBabelSubFloorRankInfo(floor, subfloor, rank, bTreasure, floortype);
	}

	public byte GetBabelFloorRankInfo(short floor, short floortype)
	{
		return this.m_kBabelClearInfo.GetBabelFloorRankInfo(floor, floortype);
	}

	public byte GetBabelSubFloorRankInfo(short floor, byte subfoor, short floortype)
	{
		return this.m_kBabelClearInfo.GetBabelSubFloorRankInfo(floor, subfoor, floortype);
	}

	public bool IsBabelClear(short _floor, short floortype)
	{
		return this.m_kBabelClearInfo.IsBabelClear(_floor, floortype);
	}

	public bool IsBabelClear(short _floor, short _sub_floor, short floortype)
	{
		return this.m_kBabelClearInfo.IsBabelClear(_floor, _sub_floor, floortype);
	}

	public bool IsBabelTreasure(short _floor, short floortype)
	{
		return this.m_kBabelClearInfo.IsBabelTreasure(_floor, floortype);
	}

	public bool IsBabelTreasure(short _floor, short _sub_floor, short floortype)
	{
		return this.m_kBabelClearInfo.IsBabelTreasure(_floor, _sub_floor, floortype);
	}

	public bool IsColosseumChallengeClear(int index)
	{
		long charSubData = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUM_CHALLENGE);
		return charSubData > (long)index;
	}

	public void SetRecommend_Recv(byte RecvCur)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			this.m_bRecommend_RecvMaxCount = (byte)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_RECOMMEND_MAX_ACCEPT);
			this.m_bRecommend_RecvCurrnetCount = RecvCur;
		}
		else
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!! NULL m_bRecommend_RecvCurrnetCount : {0} , m_bRecommend_RecvMaxCount {1} .", new object[]
			{
				RecvCur,
				this.m_bRecommend_RecvMaxCount
			});
		}
	}

	public void SetRecommend_Send(byte SendCur)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			this.m_bRecommend_SendMaxCount = (byte)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_RECOMMEND_MAX);
			this.m_bRecommend_SendCurrentCount = SendCur;
		}
		else
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!! NULL m_bRecommend_SendCurrentCount : {0} , m_bRecommend_SendMaxCount {1} .", new object[]
			{
				SendCur,
				this.m_bRecommend_SendMaxCount
			});
		}
	}

	public bool IsSamePersonID(long lPersonID)
	{
		return lPersonID == this.m_PersonID;
	}

	public void ClearSolWarehouseInfo()
	{
		this.m_SolWarehouse.Clear();
	}

	public void AddSolWarehouseInfo(GS_SOLDIER_WAREHOUSE_MOVE_ACK ACK)
	{
		this.m_SolWarehouse.AddSolWarehouseInfo(ACK);
	}

	public void AddSolWarehouseInfo(GS_SOLDIER_LOAD_GET_ACK ACK)
	{
		this.m_SolWarehouse.AddSolWarehouseInfo(ACK);
	}

	public void AddSolWarehouseInfo(NkSoldierInfo pkSolinfo)
	{
		this.m_SolWarehouse.AddSolWarehouseInfo(pkSolinfo);
	}

	public NrSolWarehouse GetWarehouseSolList()
	{
		return this.m_SolWarehouse;
	}

	public NkSoldierInfo GetSolWarehouse(long lSolID)
	{
		return this.m_SolWarehouse.GetSolWarehouse(lSolID);
	}

	public List<NkSoldierInfo> GetSolWarehouseList()
	{
		return this.m_SolWarehouse.GetSolWarehouseList();
	}

	public List<int> GetWarehouseSolKindList()
	{
		if (this.m_SolWarehouse == null)
		{
			return null;
		}
		return this.m_SolWarehouse.GetWarehouseSolKindList();
	}

	public void SetLoadServerData(bool bLoadServerData)
	{
		this.m_SolWarehouse.SetLoadServerData(bLoadServerData);
	}

	public bool IsSolWarehouseMove()
	{
		long num = (long)this.GetWarehouseCount();
		List<NkSoldierInfo> solWarehouseList = this.m_SolWarehouse.GetSolWarehouseList();
		return solWarehouseList != null && num > (long)solWarehouseList.Count;
	}

	public void RemoveSolWarehouse(long lSolID)
	{
		this.m_SolWarehouse.RemoveSolWarehouse(lSolID);
	}

	public int GetSolWarehouseCount()
	{
		return this.m_SolWarehouse.GetSolWarehouseCount();
	}

	public int GetWarehouseCount()
	{
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_SOL_WAREHOUSE_MAX);
		return num + COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FREE_HERO_WAREHOUSE);
	}

	public void Colosseum_MyGrade_Userinfo_Init()
	{
		this.m_Colosseum_GradeUserList.Init();
	}

	public void AddColosseum_MyGrade_UserInfo(COLOSSEUM_MYGRADE_USERINFO info)
	{
		this.m_Colosseum_GradeUserList.AddMyGrade_UserInfo(info);
	}

	public void Colosseum_MyGrade_Sort()
	{
		this.m_Colosseum_GradeUserList.SortList();
	}

	public int GetColosseumMyGradeRank()
	{
		return this.m_Colosseum_GradeUserList.GetMyGradeRank(this.ColosseumGradePoint);
	}

	public List<COLOSSEUM_MYGRADE_USERINFO> GeColosseumMyGradeUserList()
	{
		return this.m_Colosseum_GradeUserList.GetList();
	}

	public void UpdateColosseumGradePoint(int gradepoint)
	{
		this.ColosseumGradePoint = gradepoint;
		this.m_Colosseum_GradeUserList.UpdateMyInfo(this.ColosseumGradePoint);
		this.m_Colosseum_GradeUserList.SortList();
	}

	public void InitGuildBossMyRoomInfo()
	{
		this.m_GuildBoss_MyRoomInfo.Init();
	}

	public void InitGuildBossRoomStateInfo()
	{
		this.m_GuildBoss_MyRoomInfo.InitData();
	}

	public int GetGuildBossRoomInfoCount()
	{
		return this.m_GuildBoss_MyRoomInfo.GetCount();
	}

	public bool GetGuildBossCheck()
	{
		return this.m_GuildBoss_MyRoomInfo.GuildBossCheck();
	}

	public void AddGuildBossMyRoomInfo(NEWGUILD_MY_BOSS_ROOMINFO info)
	{
		NEWGUILD_MY_BOSS_ROOMINFO info2 = this.m_GuildBoss_MyRoomInfo.GetInfo(info.i16Floor);
		if (info2 != null)
		{
			info2.byRoomState = info.byRoomState;
			info2.ui8PlayState = info.ui8PlayState;
			info2.i64PlayPersonID = info.i64PlayPersonID;
		}
		else
		{
			this.m_GuildBoss_MyRoomInfo.AddInfo(info);
		}
	}

	public NEWGUILD_MY_BOSS_ROOMINFO GetGuildBossMyRoomInfo(short floor)
	{
		return this.m_GuildBoss_MyRoomInfo.GetInfo(floor);
	}

	public void DelGuildBossMyRoomInfo(short floor)
	{
		this.m_GuildBoss_MyRoomInfo.DelInfo(floor);
	}

	public void AddGuildBossRoomStateInfo(short GuildBossFloor)
	{
		this.m_GuildBoss_MyRoomInfo.AddGuildBossRoomStateInfo(GuildBossFloor);
	}

	public void RemoveGuildBossRoomStateInfo(short GuildBossFloor)
	{
		this.m_GuildBoss_MyRoomInfo.RemoveGuildBossRoomStateInfo(GuildBossFloor);
	}

	public bool GetGuildBossRoomStateInfo(short floor)
	{
		return this.m_GuildBoss_MyRoomInfo.GetGuildBossRoomStateInfo(floor);
	}

	public void AddGuildBossRewardInfo(bool bGuildBossRewardInfo)
	{
		this.m_GuildBoss_MyRoomInfo.AddGuildBossRewardInfo(bGuildBossRewardInfo);
	}

	public bool GetGuildBossRewardInfo()
	{
		return this.m_GuildBoss_MyRoomInfo.GetGuildBossRewardInfo();
	}

	public void SetSubData_Waring(int nSubDataType, long nSubDataValue)
	{
		this.SetCharSubData(nSubDataType, nSubDataValue);
		this.ShowSubDataWaring();
	}

	public void ShowSubDataWaring()
	{
		if (CommonTasks.IsEndOfPrework)
		{
			long charSubData = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_WARING_MSG);
			if ((charSubData & 1L) != 0L)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1968");
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("190");
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKSubDataWaringTypeA), charSubData, textFromInterface, textFromMessageBox, eMsgType.MB_OK, 2);
			}
			else if ((charSubData & 2L) != 0L)
			{
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1969");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("191");
				MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI2.SetMsg(new YesDelegate(this.MsgBoxOKSubDataWaringTypeB), charSubData, textFromInterface2, textFromMessageBox2, eMsgType.MB_OK, 2);
			}
		}
	}

	public void MsgBoxOKSubDataWaringTypeA(object a_oObject)
	{
		long num = (long)a_oObject;
		num ^= 1L;
		GS_CHARACTER_SUBDATA_REQ gS_CHARACTER_SUBDATA_REQ = new GS_CHARACTER_SUBDATA_REQ();
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataType = 28;
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataValue = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_SUBDATA_REQ, gS_CHARACTER_SUBDATA_REQ);
	}

	public void MsgBoxOKSubDataWaringTypeB(object a_oObject)
	{
		long num = (long)a_oObject;
		num ^= 2L;
		GS_CHARACTER_SUBDATA_REQ gS_CHARACTER_SUBDATA_REQ = new GS_CHARACTER_SUBDATA_REQ();
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataType = 28;
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataValue = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_SUBDATA_REQ, gS_CHARACTER_SUBDATA_REQ);
	}

	public long GetWillChargeGold()
	{
		short charDetailFromUnion = this.GetCharDetailFromUnion(eCHAR_DETAIL_INFO.eCHAR_DETAIL_INFO_LIMIT_COUNT, 1);
		long result = 200L;
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			if ((int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT1))
			{
				result = charSpend.iCharWillChargeGold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT1) && (int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT2))
			{
				result = charSpend.iCharWillChargeLimit1Gold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT2) && (int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT3))
			{
				result = charSpend.iCharWillChargeLimit2Gold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT3) && (int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT4))
			{
				result = charSpend.iCharWillChargeLimit3Gold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT4) && (int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT5))
			{
				result = charSpend.iCharWillChargeLimit4Gold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT5) && (int)charDetailFromUnion < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT6))
			{
				result = charSpend.iCharWillChargeLimit5Gold;
			}
			else if ((int)charDetailFromUnion >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT6))
			{
				result = charSpend.iCharWillChargeLimit6Gold;
			}
		}
		return result;
	}

	public long GetMaxWillChargeGold(long iWillCount)
	{
		if (iWillCount <= 0L)
		{
			return 0L;
		}
		short num = this.GetCharDetailFromUnion(eCHAR_DETAIL_INFO.eCHAR_DETAIL_INFO_LIMIT_COUNT, 1);
		long num2 = 0L;
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			int num3 = 0;
			while ((long)num3 < iWillCount)
			{
				if ((int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT1))
				{
					num2 += charSpend.iCharWillChargeGold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT1) && (int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT2))
				{
					num2 += charSpend.iCharWillChargeLimit1Gold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT2) && (int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT3))
				{
					num2 += charSpend.iCharWillChargeLimit2Gold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT3) && (int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT4))
				{
					num2 += charSpend.iCharWillChargeLimit3Gold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT4) && (int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT5))
				{
					num2 += charSpend.iCharWillChargeLimit4Gold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT5) && (int)num < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT6))
				{
					num2 += charSpend.iCharWillChargeLimit5Gold;
				}
				else if ((int)num >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_LIMIT6))
				{
					num2 += charSpend.iCharWillChargeLimit6Gold;
				}
				num += 1;
				num3++;
			}
		}
		return num2;
	}

	public void ClearTreasureMapData()
	{
		this.m_TreasureMap.Clear();
	}

	public void AddTreasureMapData(int i32MapData)
	{
		this.m_TreasureMap.Add(i32MapData);
	}

	public List<int> GetTreasureMapDate()
	{
		return this.m_TreasureMap;
	}

	public void ClearBountyHuntClearInfo()
	{
		this.m_BountyHuntClearInfo.Clear();
	}

	public int GetBountyHuntClearInfoSize()
	{
		return this.m_BountyHuntClearInfo.Count;
	}

	public void AddBountyHuntClearInfo(BOUNTYHUNT_CLEARINFO ClearInfo)
	{
		for (int i = 0; i < this.m_BountyHuntClearInfo.Count; i++)
		{
			if (ClearInfo.i16BountyHuntUnique == this.m_BountyHuntClearInfo[i].i16BountyHuntUnique)
			{
				this.m_BountyHuntClearInfo[i] = ClearInfo;
				return;
			}
		}
		this.m_BountyHuntClearInfo.Add(ClearInfo);
	}

	public void AddBountyHuntClearInfo(short iClearBountyHuntUnique, byte byClearRank)
	{
		for (int i = 0; i < this.m_BountyHuntClearInfo.Count; i++)
		{
			if (iClearBountyHuntUnique == this.m_BountyHuntClearInfo[i].i16BountyHuntUnique)
			{
				if (this.m_BountyHuntClearInfo[i].i8ClearRank < byClearRank)
				{
					this.m_BountyHuntClearInfo[i].i8ClearRank = byClearRank;
				}
				return;
			}
		}
		BOUNTYHUNT_CLEARINFO bOUNTYHUNT_CLEARINFO = new BOUNTYHUNT_CLEARINFO();
		bOUNTYHUNT_CLEARINFO.i16BountyHuntUnique = iClearBountyHuntUnique;
		bOUNTYHUNT_CLEARINFO.i8ClearRank = byClearRank;
		this.m_BountyHuntClearInfo.Add(bOUNTYHUNT_CLEARINFO);
	}

	public BOUNTYHUNT_CLEARINFO GetBountyHuntClearInfo(int iBountyHuntUnique)
	{
		for (int i = 0; i < this.m_BountyHuntClearInfo.Count; i++)
		{
			if (iBountyHuntUnique == (int)this.m_BountyHuntClearInfo[i].i16BountyHuntUnique)
			{
				return this.m_BountyHuntClearInfo[i];
			}
		}
		return null;
	}

	public BOUNTYHUNT_CLEARINFO GetBountyHuntClearInfoFromIndex(int iIndex)
	{
		if (0 > iIndex || this.m_BountyHuntClearInfo.Count <= iIndex)
		{
			return null;
		}
		return this.m_BountyHuntClearInfo[iIndex];
	}

	public eBOUNTYHUNTCLEAR_STATE GetBountyHuntClearState(short iBountyHuntUnique)
	{
		BOUNTYHUNT_CLEARINFO bountyHuntClearInfo = this.GetBountyHuntClearInfo((int)iBountyHuntUnique);
		if (bountyHuntClearInfo != null && 0 < bountyHuntClearInfo.i8ClearRank)
		{
			return eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_CLEAR;
		}
		if (iBountyHuntUnique == this.BountyHuntUnique)
		{
			return eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_ACCEPT;
		}
		return eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_NONE;
	}

	public byte GetBountyHuntClearRank(short iBountyHuntUnique)
	{
		BOUNTYHUNT_CLEARINFO bountyHuntClearInfo = this.GetBountyHuntClearInfo((int)iBountyHuntUnique);
		if (bountyHuntClearInfo != null)
		{
			return bountyHuntClearInfo.i8ClearRank;
		}
		return 0;
	}

	public bool IsBountyHuntClearUnique(short iBountyHuntUnique)
	{
		BOUNTYHUNT_CLEARINFO bountyHuntClearInfo = this.GetBountyHuntClearInfo((int)iBountyHuntUnique);
		return bountyHuntClearInfo != null && 0 < bountyHuntClearInfo.i8ClearRank;
	}

	public bool IsBountyHunt()
	{
		if (0 >= this.BountyHuntUnique)
		{
			return false;
		}
		BountyInfoData bountyInfoDataFromUnique = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataFromUnique(this.BountyHuntUnique);
		return bountyInfoDataFromUnique != null && NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataTime(bountyInfoDataFromUnique.i16Unique);
	}

	public void ClearColoseumSupportSoldier()
	{
		this.m_ColosseumSupportSoldier.Clear();
	}

	public int GetColoseumSupportSoldierCount()
	{
		return this.m_ColosseumSupportSoldier.Count;
	}

	public void AddColoseumSupportSoldier(COLOSSEUM_SUPPORTSOLDIER Data)
	{
		for (int i = 0; i < this.m_ColosseumSupportSoldier.Count; i++)
		{
			if (Data.i32CharKind == this.m_ColosseumSupportSoldier[i].i32CharKind)
			{
				this.m_ColosseumSupportSoldier[i] = Data;
				return;
			}
		}
		this.m_ColosseumSupportSoldier.Add(Data);
	}

	public int GetColoseumSupportSoldier(int iIndex)
	{
		if (0 > iIndex || this.m_ColosseumSupportSoldier.Count <= iIndex)
		{
			return 0;
		}
		return this.m_ColosseumSupportSoldier[iIndex].i32CharKind;
	}

	public COLOSSEUM_SUPPORTSOLDIER GetColosseumSupportSoldierdata(int iIndex)
	{
		if (0 > iIndex || this.m_ColosseumSupportSoldier.Count <= iIndex)
		{
			return null;
		}
		return this.m_ColosseumSupportSoldier[iIndex];
	}

	public void ClearColosseumEnableBatchSoldierKind()
	{
		this.m_ColosseumEnableBatchSoldierKind.Clear();
		this.m_nColosseumBatchKindTotal = 0;
	}

	public int GetColosseumEnableBatchSoldierKindCount()
	{
		return this.m_ColosseumEnableBatchSoldierKind.Count;
	}

	public void AddColosseumEnableBatchsoldierKind(int nCharKind)
	{
		if (!this.m_ColosseumEnableBatchSoldierKind.Contains(nCharKind))
		{
			this.m_ColosseumEnableBatchSoldierKind.Add(nCharKind);
			this.m_nColosseumBatchKindTotal += nCharKind;
		}
	}

	public int GetColosseumEnableBatchSoldierKind(int iIndex)
	{
		if (0 > iIndex || this.m_ColosseumEnableBatchSoldierKind.Count <= iIndex)
		{
			return 0;
		}
		return this.m_ColosseumEnableBatchSoldierKind[iIndex];
	}

	public bool IsEnableBatchColosseumSoldier(int nCharKind)
	{
		return this.m_ColosseumEnableBatchSoldierKind.Contains(nCharKind);
	}

	public bool IsAtbCommonFlag(long iAtbCommonFlag)
	{
		long charSubData = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COMMONFLAG);
		return (charSubData & iAtbCommonFlag) != 0L;
	}

	public void SetAtbCommonFlag(long iAtbCommonFlag)
	{
		long num = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COMMONFLAG);
		num |= iAtbCommonFlag;
		GS_UPDATE_CHARSUBDATA_REQ gS_UPDATE_CHARSUBDATA_REQ = new GS_UPDATE_CHARSUBDATA_REQ();
		gS_UPDATE_CHARSUBDATA_REQ.i32CharSubDataType = 30;
		gS_UPDATE_CHARSUBDATA_REQ.i64CharSubDataValue = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_UPDATE_CHARSUBDATA_REQ, gS_UPDATE_CHARSUBDATA_REQ);
	}

	public void DelAtbCommonFlag(long iAtbCommonFlag)
	{
		long num = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COMMONFLAG);
		num &= ~iAtbCommonFlag;
		GS_UPDATE_CHARSUBDATA_REQ gS_UPDATE_CHARSUBDATA_REQ = new GS_UPDATE_CHARSUBDATA_REQ();
		gS_UPDATE_CHARSUBDATA_REQ.i32CharSubDataType = 30;
		gS_UPDATE_CHARSUBDATA_REQ.i64CharSubDataValue = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_UPDATE_CHARSUBDATA_REQ, gS_UPDATE_CHARSUBDATA_REQ);
	}

	public NkSoldierInfo GetSoldierInfoBySolID(long i64SolID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			return charPersonInfo.GetSoldierInfoFromSolID(i64SolID);
		}
		return this.GetReadySoldierInfoBySolID(i64SolID);
	}

	public int GetSameSolNumFromSolPosType(eSOL_POSTYPE eSolPosType)
	{
		return this.m_ReadySolList.GetSameSolNumFromSolPosType(eSolPosType);
	}

	public int GetReadySolCount()
	{
		return this.m_ReadySolList.ReadySoliderCount();
	}

	public void ClearVoucherData()
	{
		this.m_VoucharData.Clear();
	}

	public void AddVoucherData(VOUCHER_DATA VoucherData)
	{
		if (VoucherData.ui8VoucherType <= 0)
		{
			return;
		}
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == VoucherData.ui8VoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == VoucherData.i64ItemMallID)
				{
					this.m_VoucharData[i] = VoucherData;
					return;
				}
			}
		}
		this.m_VoucharData.Add(VoucherData);
	}

	public VOUCHER_DATA GetVoucherData(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i];
				}
			}
		}
		return null;
	}

	public long GetVoucherRemainTime(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i].i64EndTime - PublicMethod.GetCurTime();
				}
			}
		}
		return 0L;
	}

	public long GetVoucherRemainTimeFromItemID(long i64ItemID)
	{
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(i64ItemID);
		if (itemVoucherDataFromItemID == null)
		{
			return 0L;
		}
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == itemVoucherDataFromItemID.ui8VoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == itemVoucherDataFromItemID.i64ItemMallID)
				{
					return this.m_VoucharData[i].i64EndTime - PublicMethod.GetCurTime();
				}
			}
		}
		return 0L;
	}

	public void SetVoucherRefreshTime(eVOUCHER_TYPE eVoucherType, long i64ItemMallID, long i64RefreshTime)
	{
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					this.m_VoucharData[i].i64RefreshTime = i64RefreshTime;
					return;
				}
			}
		}
	}

	public bool IsUseVoucher(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		if (this.GetVoucherRemainTime(eVoucherType, i64ItemMallID) <= 0L)
		{
			return false;
		}
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i].i64RefreshTime <= PublicMethod.GetCurTime();
				}
			}
		}
		return false;
	}

	public long GetNextUseVoucherTime(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		if (this.GetVoucherRemainTime(eVoucherType, i64ItemMallID) <= 0L)
		{
			return 0L;
		}
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i].i64RefreshTime - PublicMethod.GetCurTime();
				}
			}
		}
		return 0L;
	}

	public long GetVoucherStartTime(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i].i64StartTime;
				}
			}
		}
		return 0L;
	}

	public long GetVoucherEndTime(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		for (int i = 0; i < this.m_VoucharData.Count; i++)
		{
			if (this.m_VoucharData[i].ui8VoucherType == (byte)eVoucherType)
			{
				if (this.m_VoucharData[i].i64ItemMallID == i64ItemMallID)
				{
					return this.m_VoucharData[i].i64EndTime;
				}
			}
		}
		return 0L;
	}

	public bool IsAtbAgitMerchantBuyItemFlag(long iAtbFlag)
	{
		long charSubData = this.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_AGITMERCHANTBUYITEM);
		return (charSubData & iAtbFlag) != 0L;
	}

	public bool IsGuildAgit()
	{
		return this.m_kCharMapInfo.MapIndex == 12;
	}

	public void SetCoinInfo(long realHearts, long freeHearts)
	{
		this.m_kCoinInfo.SetCoinInfo(realHearts, freeHearts);
	}

	public bool IsMySolKindExist(int solKind)
	{
		List<int> ownAllSolKindList = this.GetOwnAllSolKindList();
		return ownAllSolKindList != null && ownAllSolKindList.Count != 0 && ownAllSolKindList.Contains(solKind);
	}

	public List<int> GetOwnAllSolKindList()
	{
		List<int> list = new List<int>();
		List<int> battleReadySolKindList = this.GetBattleReadySolKindList();
		if (battleReadySolKindList != null)
		{
			list.AddRange(battleReadySolKindList);
		}
		List<int> readySolKindList = this.GetReadySolKindList();
		if (readySolKindList != null)
		{
			list.AddRange(readySolKindList);
		}
		List<int> warehouseSolKindList = this.GetWarehouseSolKindList();
		if (warehouseSolKindList != null)
		{
			list.AddRange(warehouseSolKindList);
		}
		return list;
	}

	public List<NkSoldierInfo> GetAllSolList()
	{
		List<NkSoldierInfo> list = new List<NkSoldierInfo>();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			if (soldierList != null && soldierList.GetSoldierList() != null)
			{
				list.AddRange(soldierList.GetSoldierList());
			}
		}
		if (this.m_ReadySolList != null && this.m_ReadySolList.GetReadyAllSolList() != null)
		{
			list.AddRange(this.m_ReadySolList.GetReadyAllSolList().Values);
		}
		if (this.m_SolWarehouse != null && this.m_SolWarehouse.GetSolWarehouseList() != null)
		{
			list.AddRange(this.m_SolWarehouse.GetSolWarehouseList());
		}
		return list;
	}

	public List<int> GetOwnBattleReadyAndReadySolKindList()
	{
		List<int> list = new List<int>();
		List<int> battleReadySolKindList = this.GetBattleReadySolKindList();
		if (battleReadySolKindList != null)
		{
			list.AddRange(battleReadySolKindList);
		}
		List<int> readySolKindList = this.GetReadySolKindList();
		if (readySolKindList != null)
		{
			list.AddRange(readySolKindList);
		}
		return list;
	}

	public List<int> GetOwnBattleMinePossibleKindList()
	{
		if (this.m_ReadySolList == null)
		{
			return null;
		}
		return this.m_ReadySolList.GetMineBattlePossibleKindList();
	}

	public List<int> GetOwnReadySolKindList()
	{
		return this.GetReadySolKindList();
	}

	private List<int> GetBattleReadySolKindList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return null;
		}
		return charPersonInfo.GetSolKindList();
	}

	public long GetBestPowerSoldierID_InBattleReadyAndReadySol(int charKind)
	{
		NkSoldierInfo nkSoldierInfo = null;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			NkSoldierInfo soldierInfoByKind = soldierList.GetSoldierInfoByKind(charKind);
			nkSoldierInfo = soldierInfoByKind;
		}
		if (this.m_ReadySolList != null)
		{
			List<NkSoldierInfo> solInfoListByKind = this.m_ReadySolList.GetSolInfoListByKind(charKind);
			foreach (NkSoldierInfo current in solInfoListByKind)
			{
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = current;
				}
				else if (current.GetFightPower() >= nkSoldierInfo.GetFightPower())
				{
					nkSoldierInfo = current;
				}
			}
		}
		if (nkSoldierInfo == null)
		{
			return 0L;
		}
		return nkSoldierInfo.GetSolID();
	}

	public long GetBestPowerSoldierID_InMineBattlePossibleSol(int charKind)
	{
		if (this.m_ReadySolList == null)
		{
			return 0L;
		}
		List<NkSoldierInfo> mineBattlePossibleSolInfoList = this.m_ReadySolList.GetMineBattlePossibleSolInfoList();
		if (mineBattlePossibleSolInfoList == null || mineBattlePossibleSolInfoList.Count == 0)
		{
			return 0L;
		}
		NkSoldierInfo nkSoldierInfo = null;
		foreach (NkSoldierInfo current in mineBattlePossibleSolInfoList)
		{
			if (current.GetCharKind() == charKind)
			{
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = current;
				}
				else if (current.GetFightPower() >= nkSoldierInfo.GetFightPower())
				{
					nkSoldierInfo = current;
				}
			}
		}
		if (nkSoldierInfo == null)
		{
			return 0L;
		}
		return nkSoldierInfo.GetSolID();
	}

	public void Add_UserTimeShopItemList(TIMESHOP_ITEMINFO _pItemInfo)
	{
		this.m_kTimeShopInfo.Add_UserTimeShopItemList(_pItemInfo);
	}

	public void Clear_UserTimeShopItemList()
	{
		this.m_kTimeShopInfo.Clear_UserTimeShopItemList();
	}

	public List<TIMESHOP_ITEMINFO> Get_UserTimeShopItemList()
	{
		return this.m_kTimeShopInfo.Get_UserTimeShopItemList();
	}

	public int Get_UserTimeShopItemListCount()
	{
		return this.m_kTimeShopInfo.Get_UserTimeShopItemListCount();
	}

	public void Set_UserTimeShopInfo(short _i16RefreshCount, long _i64RefreshTime)
	{
		this.m_kTimeShopInfo.Set_UserTimeShopInfo(_i16RefreshCount, _i64RefreshTime);
	}

	public bool IsBuy_TimeShopItemByIDX(long _i64IDX)
	{
		return this.m_kTimeShopInfo.Get_UserTimeShopItmeIsBuy(_i64IDX);
	}

	public int GetIndex_byTimeShopIDX(long _i64IDX)
	{
		return this.m_kTimeShopInfo.GetIndex_byTimeShopIDX(_i64IDX);
	}

	public void Set_UserTimeShopItemBuy(long _i64IDX, byte _i8IsBuy)
	{
		this.m_kTimeShopInfo.Set_UserTimeShopItemBuy(_i64IDX, _i8IsBuy);
	}
}
