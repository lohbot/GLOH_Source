using GAME;
using Global;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CommunityUI_DLG : Form
{
	private enum eLAYER
	{
		eLAYER_NONE,
		eLAYER_INPUT_HELPSOL,
		eLAYER_FRIENDBATTLE,
		eLAYER_BASE_BUTTON,
		eLAYER_BAND_BUTTON,
		eLAYER_KAKAO_BUTTON,
		eLAYER_LINE,
		eLAYER_MAX
	}

	private enum E_LEAVE_TYPE
	{
		DELETE_GUILD,
		SELF_LEAVE,
		BAN_LEAVE
	}

	private enum eSORT
	{
		eSORT_NAME,
		eSORT_CONNECTTIME,
		eSORT_FRIENDSOL,
		eSORT_SUPPORTSOL,
		eSORT_REWARD,
		eSORT_MAX
	}

	private Button m_btEffectShowHelp;

	private Button m_btFriendMenu;

	private Label m_laFriendNum;

	private DrawTexture m_dtFriendNum;

	private Label m_laHelpSolGiveCount;

	private NewListBox[] m_LBList = new NewListBox[2];

	private Button m_btNameSort;

	private Button m_btConnectTimeSort;

	private Button m_btFriendSol;

	private Button m_btSupportSol;

	private Button m_btRewardSort;

	private DrawTexture[] m_dtSort = new DrawTexture[5];

	private Button m_btSelectBattleSol;

	private eCOMMUNITYDLG_SHOWTYPE m_eCurShowType;

	private List<COMMUNITY_USER_INFO> m_CommunityUserList = new List<COMMUNITY_USER_INFO>();

	private List<COMMUNITY_USER_INFO> m_CommunitySortList = new List<COMMUNITY_USER_INFO>();

	private List<COMMUNITY_USER_INFO> m_CommunityFirstList = new List<COMMUNITY_USER_INFO>();

	private bool m_bListBtnCheck;

	private COMMUNITY_USER_INFO m_cur_Comunity_User_Info = new COMMUNITY_USER_INFO();

	private int m_RoomUnique;

	private bool m_bNameSort;

	private bool m_bConnectTimeSort;

	private bool m_bFriendSolSort = true;

	private bool m_bSupportSolSort;

	private bool m_bRewardSort;

	private CommunityUI_DLG.eSORT m_eSort = CommunityUI_DLG.eSORT.eSORT_FRIENDSOL;

	private float m_fSortDelayTime;

	private Toggle m_GameFriend;

	private Toggle m_LineFriend;

	private Label m_LineLabel;

	public eCOMMUNITYDLG_SHOWTYPE CurShowType
	{
		get
		{
			return this.m_eCurShowType;
		}
		set
		{
			this.m_eCurShowType = value;
		}
	}

	public int RoomUnique
	{
		get
		{
			return this.m_RoomUnique;
		}
		set
		{
			this.m_RoomUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "Community/DLG_Community", G_ID.COMMUNITY_DLG, true);
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
			if (directionDLG != null)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo.GetLevel() > 20)
				{
					directionDLG.SetDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY);
				}
				directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY, 0);
			}
		}
	}

	public override void SetComponent()
	{
		this.m_btEffectShowHelp = (base.GetControl("Help_Button") as Button);
		this.m_btEffectShowHelp.Click = new EZValueChangedDelegate(this.BtnShowEffectHelpSol);
		this.m_laFriendNum = (base.GetControl("Label_friendnum") as Label);
		this.m_dtFriendNum = (base.GetControl("DrawTexture_friendnum") as DrawTexture);
		this.m_laHelpSolGiveCount = (base.GetControl("Label_HelpCount") as Label);
		this.m_LBList[0] = (base.GetControl("NLB_Friend") as NewListBox);
		this.m_LBList[1] = (base.GetControl("NLB_BattleSol") as NewListBox);
		for (int i = 0; i < 2; i++)
		{
			if (TsPlatform.IsWeb)
			{
				this.m_LBList[i].AddRightMouseDelegate(new EZValueChangedDelegate(this.BtClickListBox));
			}
			else
			{
				this.m_LBList[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickListBox));
			}
		}
		this.m_GameFriend = (base.GetControl("Toggle_GameFriend") as Toggle);
		this.m_GameFriend.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGameFriend));
		this.m_LineFriend = (base.GetControl("Toggle_LineFriend") as Toggle);
		this.m_LineLabel = (base.GetControl("Label_LineFriend") as Label);
		this.m_GameFriend.Visible = true;
		this.m_LineFriend.Visible = false;
		this.m_LineLabel.Visible = false;
		this.m_btFriendMenu = (base.GetControl("Button_Find") as Button);
		this.m_btFriendMenu.Click = new EZValueChangedDelegate(this.BtnClickFindMenu);
		base.SetShowLayer(3, true);
		this.m_btSelectBattleSol = (base.GetControl("BT_Help") as Button);
		this.m_btSelectBattleSol.Click = new EZValueChangedDelegate(this.BtnClickSelectBattleSol);
		this.m_btNameSort = (base.GetControl("BT_NameSort") as Button);
		this.m_btNameSort.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNameSort));
		this.m_btNameSort.EffectAni = false;
		this.m_btConnectTimeSort = (base.GetControl("BT_TimeSort") as Button);
		this.m_btConnectTimeSort.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConnectTimeSort));
		this.m_btConnectTimeSort.EffectAni = false;
		this.m_btFriendSol = (base.GetControl("BT_Support01") as Button);
		this.m_btFriendSol.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickFriendSolSort));
		this.m_btFriendSol.EffectAni = false;
		this.m_btSupportSol = (base.GetControl("BT_Support02") as Button);
		this.m_btSupportSol.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSupportSolSort));
		this.m_btSupportSol.EffectAni = false;
		this.m_btRewardSort = (base.GetControl("BT_RewardSort") as Button);
		this.m_btRewardSort.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickRewardSort));
		this.m_btRewardSort.EffectAni = false;
		this.m_dtSort[0] = (base.GetControl("DT_Arrow_Name") as DrawTexture);
		this.m_dtSort[1] = (base.GetControl("DT_Arrow_Time") as DrawTexture);
		this.m_dtSort[2] = (base.GetControl("DT_Arrow_Support01") as DrawTexture);
		this.m_dtSort[3] = (base.GetControl("DT_Arrow_Support01_C") as DrawTexture);
		this.m_dtSort[4] = (base.GetControl("DT_Arrow_Reward") as DrawTexture);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.Hide();
		TsLog.Log("(Get CommunityEffect {0}", new object[]
		{
			PlayerPrefs.GetInt("Community DLG Effect")
		});
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("frienddlg_open");
	}

	private void ClickGameFriend(IUIObject obj)
	{
		Toggle toggle = (Toggle)obj;
		if (null == toggle)
		{
			return;
		}
		if (toggle.Value)
		{
			this.ChangeMode(this.m_eCurShowType);
		}
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetScreenCenter();
	}

	public override void OnLoad()
	{
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITYMSG_DLG);
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		base.OnClose();
	}

	public override void Update()
	{
		if (0f < this.m_fSortDelayTime && this.m_fSortDelayTime <= Time.time)
		{
			this.m_fSortDelayTime = 0f;
		}
	}

	public void Initialize()
	{
		this.m_CommunityUserList.Clear();
	}

	public void UpdatePage()
	{
		this.ChangeMode(this.m_eCurShowType);
	}

	public void UpdateFriend(long _friend_personid)
	{
		int num = this.ListBox_FriendIndex(_friend_personid);
		if (num < 0)
		{
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = this.UpdateCommunityInfo(_friend_personid);
		if (cOMMUNITY_USER_INFO != null)
		{
			NewListItem newListItem = this.SetListItem_Colum(cOMMUNITY_USER_INFO);
			if (newListItem != null)
			{
				this.m_LBList[(int)this.m_eCurShowType].RemoveAdd(num, newListItem);
			}
		}
		this.m_LBList[(int)this.m_eCurShowType].RepositionItems();
		this.ShowFriendCountInfo();
		this.ShowFriendHelpSolCountInfo();
	}

	public void UpdateCommunity_Friend(COMMUNITY_USER_INFO info)
	{
		if (info != null)
		{
			int num = this.ListBox_FriendIndex(info.i64PersonID);
			if (num < 0)
			{
				return;
			}
			NewListItem newListItem = this.SetListItem_Colum(info);
			if (newListItem != null)
			{
				this.m_LBList[(int)this.m_eCurShowType].RemoveAdd(num, newListItem);
			}
		}
		this.m_LBList[(int)this.m_eCurShowType].RepositionItems();
	}

	public void Show(byte _show_type)
	{
		this.m_eCurShowType = (eCOMMUNITYDLG_SHOWTYPE)_show_type;
		this.ChangeMode(this.m_eCurShowType);
		base.Show();
	}

	public void SetWhisperRoomUnique(int RoomUnique)
	{
		this.m_RoomUnique = RoomUnique;
	}

	public static void CurrentLocationName(COMMUNITY_USER_INFO _userInfo, ref string strName, ref string ColorNum)
	{
		byte b = _userInfo.byLocation;
		int i32MapUnique = _userInfo.i32MapUnique;
		if (0 >= b)
		{
			string text = string.Empty;
			long iSec = PublicMethod.GetCurTime() - _userInfo.i64LogoutTime;
			long totalDayFromSec = PublicMethod.GetTotalDayFromSec(iSec);
			long hourFromSec = PublicMethod.GetHourFromSec(iSec);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(iSec);
			if (totalDayFromSec > 0L)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2173");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
				{
					text,
					"count",
					totalDayFromSec
				});
			}
			else if (hourFromSec > 0L)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2172");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
				{
					text,
					"count",
					hourFromSec
				});
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2171");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strName, new object[]
				{
					text,
					"count",
					minuteFromSec
				});
			}
		}
		else
		{
			string mapName = NrTSingleton<MapManager>.Instance.GetMapName(i32MapUnique);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1777");
			byte b2 = 200;
			b -= b2;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				textFromInterface,
				"count",
				b
			});
			strName = string.Format("{0}({1})", mapName, textFromInterface);
			ColorNum = "1104";
		}
	}

	public void SetSelectHelpSol(NkSoldierInfo _solider_Info)
	{
		if (_solider_Info == null)
		{
			return;
		}
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		if (this.m_cur_Comunity_User_Info != null)
		{
			GS_FRIEND_HELPSOL_SET_REQ gS_FRIEND_HELPSOL_SET_REQ = new GS_FRIEND_HELPSOL_SET_REQ();
			gS_FRIEND_HELPSOL_SET_REQ.i64FriendPersonID = this.m_cur_Comunity_User_Info.i64PersonID;
			gS_FRIEND_HELPSOL_SET_REQ.i64SolID = _solider_Info.GetSolID();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_SET_REQ, gS_FRIEND_HELPSOL_SET_REQ);
			this.m_cur_Comunity_User_Info = null;
		}
	}

	private void ChangeMode(eCOMMUNITYDLG_SHOWTYPE _show_type)
	{
		this.m_CommunityUserList.Clear();
		this.m_eCurShowType = _show_type;
		CommunityUI_DLG.eLAYER layer = CommunityUI_DLG.eLAYER.eLAYER_NONE;
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			layer = CommunityUI_DLG.eLAYER.eLAYER_INPUT_HELPSOL;
		}
		else if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
		{
			layer = CommunityUI_DLG.eLAYER.eLAYER_FRIENDBATTLE;
			this.m_btEffectShowHelp.Hide(true);
		}
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
			{
				if (uSER_FRIEND_INFO.ui8HelpUse < 1 && uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolKind > 0)
				{
					COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = new COMMUNITY_USER_INFO();
					cOMMUNITY_USER_INFO.Set(uSER_FRIEND_INFO);
					this.m_CommunityUserList.Add(cOMMUNITY_USER_INFO);
				}
			}
			else if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
			{
				if (uSER_FRIEND_INFO.nPersonID >= 11L)
				{
					COMMUNITY_USER_INFO cOMMUNITY_USER_INFO2 = new COMMUNITY_USER_INFO();
					cOMMUNITY_USER_INFO2.Set(uSER_FRIEND_INFO);
					this.m_CommunityUserList.Add(cOMMUNITY_USER_INFO2);
				}
			}
		}
		this.ShowInfo(layer);
	}

	private void ShowInfo(CommunityUI_DLG.eLAYER _layer)
	{
		this.ShowLayer(_layer);
		this.m_LBList[(int)this.m_eCurShowType].Clear();
		this.ShowFriendCountInfo();
		this.ShowFriendHelpSolCountInfo();
		if (this.m_CommunityUserList.Count <= 0)
		{
			return;
		}
		this.SortCommunity();
		for (int i = 0; i < this.m_CommunityUserList.Count; i++)
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = this.m_CommunityUserList[i];
			if (cOMMUNITY_USER_INFO != null)
			{
				NewListItem newListItem = this.SetListItem_Colum(cOMMUNITY_USER_INFO);
				if (newListItem != null)
				{
					this.m_LBList[(int)this.m_eCurShowType].Add(newListItem);
				}
			}
		}
		this.m_LBList[(int)this.m_eCurShowType].RepositionItems();
	}

	private void ShowFriendCountInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
		{
			this.m_laFriendNum.Hide(true);
			this.m_dtFriendNum.Hide(true);
		}
		else if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			this.m_laFriendNum.Hide(false);
			this.m_dtFriendNum.Hide(false);
			int num = 0;
			if (this.m_CommunityUserList.Count > 0)
			{
				num = this.m_CommunityUserList.Count;
			}
			int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1334");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				num,
				"count2",
				limitFriendCount
			});
			this.m_laFriendNum.SetText(empty);
		}
	}

	public void ShowFriendHelpSolCountInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
		{
			this.m_laHelpSolGiveCount.Hide(true);
		}
		else if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			this.m_laFriendNum.Hide(false);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1941");
			long charDetail = kMyCharInfo.GetCharDetail(9);
			int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				charDetail,
				"count2",
				limitFriendCount
			});
			this.m_laHelpSolGiveCount.Hide(false);
			this.m_laHelpSolGiveCount.SetText(empty);
		}
	}

	private COMMUNITY_USER_INFO UpdateCommunityInfo(long _friend_personid)
	{
		foreach (COMMUNITY_USER_INFO current in this.m_CommunityUserList)
		{
			if (current.i64PersonID == _friend_personid)
			{
				USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(_friend_personid);
				if (friend != null)
				{
					current.Set(friend);
					return current;
				}
			}
		}
		return null;
	}

	public COMMUNITY_USER_INFO GetCommunity_User(long _PersonID)
	{
		COMMUNITY_USER_INFO result = null;
		foreach (COMMUNITY_USER_INFO current in this.m_CommunityUserList)
		{
			if (current.i64PersonID == _PersonID)
			{
				result = current;
			}
		}
		return result;
	}

	private void ShowLayer(CommunityUI_DLG.eLAYER _layer)
	{
		for (int i = 0; i < 7; i++)
		{
			base.SetShowLayer(i, false);
		}
		base.SetShowLayer(0, true);
		base.SetShowLayer((int)_layer, true);
		if (_layer == CommunityUI_DLG.eLAYER.eLAYER_INPUT_HELPSOL)
		{
			base.SetShowLayer(3, true);
		}
		this.m_GameFriend.Visible = true;
		this.m_LineFriend.Visible = false;
		this.m_LineLabel.Visible = false;
	}

	private NewListItem SetListItem_Colum(COMMUNITY_USER_INFO _community_user_info)
	{
		NewListItem newListItem = new NewListItem(this.m_LBList[(int)this.m_eCurShowType].ColumnNum, true);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_community_user_info.i32FaceCharKind);
		if (charKindInfo == null)
		{
			return null;
		}
		newListItem.SetListItemData(0, true);
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = _community_user_info.i32FaceCharKind;
		nkListSolInfo.SolGrade = -1;
		nkListSolInfo.SolLevel = _community_user_info.i16Level;
		EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(charKindInfo.GetCharKind());
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE && _community_user_info.i64PersonID < 11L)
		{
			GMHELP_INFO gMHelpKindInfo = NrTSingleton<NrBaseTableManager>.Instance.GetGMHelpKindInfo(_community_user_info.Friend_HelpSolInfo.i64HelpSolID.ToString());
			newListItem.SetListItemData(12, false);
			if (gMHelpKindInfo == null)
			{
				nkListSolInfo.ShowLevel = true;
				newListItem.SetListItemData(1, nkListSolInfo, null, null, null);
			}
			else if (gMHelpKindInfo.m_strGMPortraitFile != string.Empty)
			{
				string text = string.Format("{0}", "UI/GMHELP/" + gMHelpKindInfo.m_strGMPortraitFile);
				newListItem.SetListItemData(1, text, true, null, null);
			}
		}
		else if (_community_user_info.UserPortrait == null)
		{
			nkListSolInfo.ShowLevel = true;
			newListItem.SetListItemData(1, nkListSolInfo, null, null, null);
			if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
			{
				if (eventHeroCharFriendCode != null)
				{
					newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
					newListItem.SetListItemData(12, true);
				}
				else
				{
					newListItem.SetListItemData(12, false);
				}
			}
			else if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
			{
				if (eventHeroCharFriendCode != null)
				{
					newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
					newListItem.SetListItemData(18, true);
				}
				else
				{
					newListItem.SetListItemData(18, false);
				}
			}
		}
		else
		{
			if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
			{
				newListItem.SetListItemData(12, false);
			}
			else
			{
				newListItem.SetListItemData(18, false);
			}
			newListItem.SetListItemData(1, _community_user_info.UserPortrait, null, _community_user_info.i16Level, null, null);
		}
		newListItem.SetListItemData(2, _community_user_info.strName, null, null, null);
		if (_community_user_info.strGuildName != string.Empty)
		{
			newListItem.SetListItemData(3, _community_user_info.strGuildName, null, null, null);
		}
		string gradeTexture = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTexture(_community_user_info.i16ColosseumGrade);
		if (gradeTexture != string.Empty)
		{
			newListItem.SetListItemData(4, gradeTexture, null, null, null);
		}
		newListItem.SetListItemData(9, string.Empty, _community_user_info, new EZValueChangedDelegate(this.BtClickRightMenu), null);
		if (_community_user_info.strPlatformName.Length > 0)
		{
			newListItem.SetListItemData(17, true);
		}
		else
		{
			newListItem.SetListItemData(17, false);
		}
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			if (_community_user_info.strGuildName == string.Empty)
			{
				newListItem.SetListItemData(2, false);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(17, true);
				newListItem.SetListItemData(17, _community_user_info.strName, null, null, null);
			}
			newListItem.SetListItemData(19, false);
		}
		if (_community_user_info.Friend_HelpSolInfo.i32SolKind <= 0)
		{
			if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
			{
				newListItem.SetListItemData(13, false);
			}
			newListItem.SetListItemData(5, false);
			newListItem.SetListItemData(7, true);
			newListItem.SetListItemData(8, false);
		}
		else
		{
			if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
			{
				if (_community_user_info.strGuildName == string.Empty)
				{
					newListItem.SetListItemData(2, false);
					newListItem.SetListItemData(3, false);
					newListItem.SetListItemData(14, _community_user_info.strName, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(3, _community_user_info.strGuildName, null, null, null);
					newListItem.SetListItemData(14, false);
				}
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(13, false);
			}
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(8, _community_user_info.GetHelpSolUse());
			NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_community_user_info.Friend_HelpSolInfo.i32SolKind);
			if (charKindInfo2 != null)
			{
				NkListSolInfo nkListSolInfo2 = new NkListSolInfo();
				nkListSolInfo2.SolCharKind = _community_user_info.Friend_HelpSolInfo.i32SolKind;
				nkListSolInfo2.SolGrade = (int)_community_user_info.Friend_HelpSolInfo.bySolGrade;
				nkListSolInfo2.SolLevel = _community_user_info.Friend_HelpSolInfo.iSolLevel;
				nkListSolInfo2.FightPower = (long)_community_user_info.Friend_HelpSolInfo.i32SolFightPower;
				UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo2.SolCharKind, nkListSolInfo2.SolGrade);
				if (legendFrame != null)
				{
					newListItem.SetListItemData(5, legendFrame, null, null, null);
				}
				newListItem.SetListItemData(5, true);
				if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
				{
					NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkListSolInfo2.SolCharKind);
					if (charKindInfo3 == null)
					{
						return null;
					}
					short gradeMaxLevel = charKindInfo3.GetGradeMaxLevel((short)((byte)nkListSolInfo2.SolGrade));
					if (nkListSolInfo2.SolLevel >= gradeMaxLevel)
					{
						nkListSolInfo2.ShowCombat = true;
						nkListSolInfo2.ShowLevel = false;
					}
					else
					{
						nkListSolInfo2.ShowCombat = false;
						nkListSolInfo2.ShowLevel = false;
					}
					nkListSolInfo2.ShowLevel = false;
					newListItem.SetListItemData(6, nkListSolInfo2, null, null, null);
					string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(nkListSolInfo2.SolCharKind, nkListSolInfo2.SolGrade, charKindInfo2.GetName());
					newListItem.SetListItemData(10, legendName, null, null, null);
					newListItem.SetListItemData(11, NrTSingleton<UIDataManager>.Instance.GetString("Lv ", _community_user_info.Friend_HelpSolInfo.iSolLevel.ToString()), null, null, null);
					UIBaseInfoLoader legendFrame2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo2.SolCharKind, nkListSolInfo2.SolGrade);
					if (legendFrame != null)
					{
						newListItem.SetListItemData(5, legendFrame2, null, null, null);
					}
				}
				else
				{
					UIBaseInfoLoader legendFrame3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo2.SolCharKind, nkListSolInfo2.SolGrade);
					if (legendFrame != null)
					{
						newListItem.SetListItemData(5, legendFrame3, null, null, null);
					}
					nkListSolInfo2.ShowLevel = true;
					newListItem.SetListItemData(6, nkListSolInfo2, null, null, null);
				}
			}
		}
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (_community_user_info.i16BattleMatch >= 10000)
			{
				if (_community_user_info.i16BattleMatch >= 21000)
				{
					newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2320"), null, null, null);
					newListItem.SetListItemData(16, false);
				}
				else if (_community_user_info.i16BattleMatch >= 10000 && _community_user_info.i16BattleMatch < 20000)
				{
					CommunityUI_DLG.CurrentLocationName(_community_user_info, ref empty, ref empty2);
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("649");
					short num = _community_user_info.i16BattleMatch - 10000;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromInterface,
						"floor",
						num
					});
					newListItem.SetListItemData(10, NrTSingleton<CTextParser>.Instance.GetTextColor(empty2) + empty, null, null, null);
					newListItem.SetListItemData(16, false);
				}
				else
				{
					string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("649");
					short num2 = _community_user_info.i16BattleMatch - 20000;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromInterface2,
						"floor",
						num2
					});
					newListItem.SetListItemData(16, empty, _community_user_info, new EZValueChangedDelegate(this.BtnClickBabelTower), null);
				}
			}
			else
			{
				CommunityUI_DLG.CurrentLocationName(_community_user_info, ref empty, ref empty2);
				newListItem.SetListItemData(10, NrTSingleton<CTextParser>.Instance.GetTextColor(empty2) + empty, null, null, null);
				newListItem.SetListItemData(16, false);
			}
			NkSoldierInfo nkSoldierInfo = _community_user_info.MyHelpSol();
			newListItem.SetListItemData(20, false);
			newListItem.SetListItemData(21, false);
			if (nkSoldierInfo != null)
			{
				NrCharKindInfo charKindInfo4 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
				if (charKindInfo4 != null)
				{
					NkListSolInfo nkListSolInfo3 = new NkListSolInfo();
					nkListSolInfo3.SolCharKind = nkSoldierInfo.GetCharKind();
					nkListSolInfo3.SolGrade = (int)nkSoldierInfo.GetGrade();
					nkListSolInfo3.SolLevel = nkSoldierInfo.GetLevel();
					nkListSolInfo3.ShowLevel = true;
					UIBaseInfoLoader legendFrame4 = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo3.SolCharKind, nkListSolInfo3.SolGrade);
					if (legendFrame4 != null)
					{
						newListItem.SetListItemData(11, legendFrame4, null, null, null);
					}
					newListItem.SetListItemData(11, true);
					newListItem.SetListItemData(12, nkListSolInfo3, null, null, null);
					newListItem.SetListItemData(13, false);
					if (nkSoldierInfo.AddHelpExp <= 0L)
					{
						newListItem.SetListItemEnable(14, false);
					}
					else
					{
						newListItem.SetListItemEnable(14, true);
						newListItem.SetListItemData(14, string.Empty, _community_user_info, new EZValueChangedDelegate(this.BtnClickHelpSol_ExpGive), null);
					}
					if (nkSoldierInfo.HelpSolUse)
					{
						newListItem.SetListItemData(21, true);
					}
					else
					{
						newListItem.SetListItemData(21, false);
					}
					newListItem.SetListItemData(15, true);
					newListItem.SetListItemData(15, string.Empty, _community_user_info, new EZValueChangedDelegate(this.BtnClickHelpSolUnSet), null);
				}
			}
			else
			{
				newListItem.SetListItemData(11, false);
				newListItem.SetListItemData(12, false);
				newListItem.SetListItemData(13, string.Empty, _community_user_info, new EZValueChangedDelegate(this.BtnClickSelectHelpSol), null);
				newListItem.SetListItemEnable(14, false);
				newListItem.SetListItemData(15, false);
			}
		}
		newListItem.Data = _community_user_info;
		return newListItem;
	}

	private int ListBox_FriendIndex(long _friend_personid)
	{
		for (int i = 0; i < this.m_LBList[(int)this.m_eCurShowType].Count; i++)
		{
			IUIListObject item = this.m_LBList[(int)this.m_eCurShowType].GetItem(i);
			if (item != null)
			{
				COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)item.Data;
				if (cOMMUNITY_USER_INFO != null && cOMMUNITY_USER_INFO.i64PersonID == _friend_personid)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private COMMUNITY_USER_INFO GetSelectMember()
	{
		IUIListObject selectItem = this.m_LBList[(int)this.m_eCurShowType].GetSelectItem();
		if (selectItem == null)
		{
			return null;
		}
		return selectItem.Data as COMMUNITY_USER_INFO;
	}

	public static string CommunityIcon(COMMUNITY_USER_INFO _userInfo)
	{
		if (!_userInfo.bConnect)
		{
			return "Win_I_Comm02";
		}
		if (_userInfo.byUserPlayState <= 0)
		{
			return "Win_I_Comm03";
		}
		return "Win_I_Comm01";
	}

	private static int CompareName(COMMUNITY_USER_INFO x, COMMUNITY_USER_INFO y)
	{
		int num = CommunityUI_DLG.CompareLogin(x, y);
		if (num != 0)
		{
			return num;
		}
		return string.Compare(x.strName, y.strName);
	}

	private static int CompareLogin(COMMUNITY_USER_INFO x, COMMUNITY_USER_INFO y)
	{
		if (x.bConnect && y.bConnect)
		{
			return 0;
		}
		if (x.bConnect)
		{
			return -1;
		}
		return 1;
	}

	private static int CompareHelpSolLevel(COMMUNITY_USER_INFO x, COMMUNITY_USER_INFO y)
	{
		if (11L > x.Friend_HelpSolInfo.i64HelpSolID && 0L < x.Friend_HelpSolInfo.i64HelpSolID)
		{
			return -1;
		}
		if (x.Friend_HelpSolInfo.iSolLevel > y.Friend_HelpSolInfo.iSolLevel)
		{
			return -1;
		}
		if (x.Friend_HelpSolInfo.iSolLevel == y.Friend_HelpSolInfo.iSolLevel)
		{
			return CommunityUI_DLG.CompareLevel(x, y);
		}
		return 1;
	}

	private static int CompareLevel(COMMUNITY_USER_INFO x, COMMUNITY_USER_INFO y)
	{
		if (x.i16Level > y.i16Level)
		{
			return -1;
		}
		if (x.i16Level == y.i16Level)
		{
			return 0;
		}
		return 1;
	}

	private void BtnClickBabelTower(IUIObject obj)
	{
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)obj.Data;
		bool bMoveWorld = false;
		if (Client.m_MyWS != (long)cOMMUNITY_USER_INFO.i32WorldID_Connect)
		{
			bMoveWorld = true;
		}
		bool flag = NrTSingleton<BabelTowerManager>.Instance.IsBabelStart();
		if (flag)
		{
			GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ = new GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ();
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.nInvite = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bAccept = true;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bMoveWorld = bMoveWorld;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.WorldID = cOMMUNITY_USER_INFO.i32WorldID_Connect;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.ChannelID = cOMMUNITY_USER_INFO.byLocation;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.PersonID = cOMMUNITY_USER_INFO.i64PersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ, gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ);
		}
	}

	private void BtnShowEffectHelpSol(IUIObject obj)
	{
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			directionDLG.ReviewDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY);
		}
	}

	private void BtnClickFindMenu(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COMMUNITY_FRIENDMENU_DLG);
	}

	private void BtClickListBox(IUIObject obj)
	{
		if (NrTSingleton<CRightClickMenu>.Instance.IsOpen())
		{
			TsLog.Log("CloseUI(CRightClickMenu.CLOSEOPTION.CLICK", new object[0]);
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		}
	}

	private void BtClickRightMenu(IUIObject obj)
	{
		if (this.m_eCurShowType != eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			return;
		}
		if (this.m_bListBtnCheck)
		{
			this.m_bListBtnCheck = false;
			return;
		}
		int eCurShowType = (int)this.m_eCurShowType;
		if (TsPlatform.IsWeb)
		{
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		}
		else if (NrTSingleton<CRightClickMenu>.Instance.IsOpen())
		{
			TsLog.Log("CloseUI(CRightClickMenu.CLOSEOPTION.CLICK", new object[0]);
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = obj.Data as COMMUNITY_USER_INFO;
		if (cOMMUNITY_USER_INFO == null)
		{
			return;
		}
		bool flag;
		if (cOMMUNITY_USER_INFO.byLocation <= 0 || !cOMMUNITY_USER_INFO.bConnect)
		{
			flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.COMMUNITY_FRIEND_LOGOFF, CRightClickMenu.TYPE.SIMPLE_SECTION_1);
		}
		else if (Client.m_MyWS != (long)cOMMUNITY_USER_INFO.i32WorldID || Client.m_MyCH != cOMMUNITY_USER_INFO.byLocation)
		{
			Debug.Log(string.Concat(new object[]
			{
				Client.m_MyWS,
				"!=",
				cOMMUNITY_USER_INFO.i32WorldID,
				",",
				Client.m_MyCH,
				"!=",
				cOMMUNITY_USER_INFO.byLocation
			}));
			flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.COMMUNITY_FRIEND_DIFF_SV_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_3);
		}
		else
		{
			flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.COMMUNITY_FRIEND_SAME_SV_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_3);
		}
		Button button = obj as Button;
		if (button != null && flag)
		{
			float x = this.m_LBList[eCurShowType].GetSize().x;
			float height = 28f;
			float left = base.GetLocation().x + this.m_LBList[eCurShowType].GetLocation().x + button.gameObject.transform.localPosition.x;
			float top = base.GetLocationY() + this.m_LBList[eCurShowType].GetLocationY() + -button.gameObject.transform.localPosition.y;
			Rect windowRect = new Rect(left, top, x, height);
			NrTSingleton<CRightClickMenu>.Instance.SetWindowRect(windowRect);
		}
	}

	private void BtnClickDelFriend(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		IUIListObject selectItem = this.m_LBList[(int)this.m_eCurShowType].GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)selectItem.Data;
		if (cOMMUNITY_USER_INFO != null && msgBoxUI != null)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("8");
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox,
				"Charname",
				cOMMUNITY_USER_INFO.strName
			});
			msgBoxUI.SetMsg(new YesDelegate(this.FriendDelYes), cOMMUNITY_USER_INFO, textFromInterface, empty, eMsgType.MB_OK_CANCEL);
		}
	}

	private void BtnClickSelectHelpSol(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		COMMUNITY_USER_INFO cur_Comunity_User_Info = (COMMUNITY_USER_INFO)obj.Data;
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.SetLocationByForm(this);
			solMilitarySelectDlg.SortwithoutHelpsol = true;
			solMilitarySelectDlg.SetFocus();
			solMilitarySelectDlg.Refresh();
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		this.m_bListBtnCheck = true;
		this.m_cur_Comunity_User_Info = cur_Comunity_User_Info;
	}

	private void BtnClickHelpSolUnSet(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)obj.Data;
		NkSoldierInfo nkSoldierInfo = cOMMUNITY_USER_INFO.MyHelpSol();
		if (nkSoldierInfo == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = cOMMUNITY_USER_INFO.i64PersonID;
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = nkSoldierInfo.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = nkSoldierInfo.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
		this.m_bListBtnCheck = true;
	}

	private void BtnClickHelpSol_ExpGive(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)obj.Data;
		NkSoldierInfo nkSoldierInfo = cOMMUNITY_USER_INFO.MyHelpSol();
		if (nkSoldierInfo == null)
		{
			return;
		}
		long charDetail = kMyCharInfo.GetCharDetail(9);
		int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
		if (charDetail >= (long)limitFriendCount)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("502");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_FRIEND_HELPSOL_EXP_GIVE_REQ gS_FRIEND_HELPSOL_EXP_GIVE_REQ = new GS_FRIEND_HELPSOL_EXP_GIVE_REQ();
		gS_FRIEND_HELPSOL_EXP_GIVE_REQ.i64SolID = nkSoldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_EXP_GIVE_REQ, gS_FRIEND_HELPSOL_EXP_GIVE_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "REWARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_bListBtnCheck = true;
	}

	private void BtnClickSelectBattleSol(IUIObject obj)
	{
		IUIListObject selectItem = this.m_LBList[(int)this.m_eCurShowType].GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)selectItem.Data;
		if (cOMMUNITY_USER_INFO.GetHelpSolUse())
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("59"),
				"charname",
				cOMMUNITY_USER_INFO.strName
			});
			if (empty != string.Empty)
			{
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			return;
		}
		if (cOMMUNITY_USER_INFO.Friend_HelpSolInfo.i64HelpSolID > 0L)
		{
			GS_BATTLE_FRIEND_HELP_REQ gS_BATTLE_FRIEND_HELP_REQ = new GS_BATTLE_FRIEND_HELP_REQ();
			gS_BATTLE_FRIEND_HELP_REQ.nFriendPersonID = cOMMUNITY_USER_INFO.i64PersonID;
			gS_BATTLE_FRIEND_HELP_REQ.nFriendSolID = cOMMUNITY_USER_INFO.Friend_HelpSolInfo.i64HelpSolID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_FRIEND_HELP_REQ, gS_BATTLE_FRIEND_HELP_REQ);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
			NrTSingleton<FiveRocksEventManager>.Instance.FriendHelpHero();
			return;
		}
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("74"),
			"charname",
			cOMMUNITY_USER_INFO.strName
		});
		if (empty2 != string.Empty)
		{
			Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	private void BtnClickRecommend(IUIObject obj)
	{
	}

	public void BtClickPlatformFriend(IUIObject obj)
	{
	}

	private void FriendDelYes(object a_oObject)
	{
		GS_DEL_FRIEND_REQ gS_DEL_FRIEND_REQ = new GS_DEL_FRIEND_REQ();
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)a_oObject;
		gS_DEL_FRIEND_REQ.i64FriendPersonID = cOMMUNITY_USER_INFO.i64PersonID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_DEL_FRIEND_REQ, gS_DEL_FRIEND_REQ);
	}

	public void MsgBoxCancelEvent(MsgBoxUI BoxUI, object EventObject)
	{
		Debug.LogError("MsgBoxOKEvent");
	}

	public List<COMMUNITY_USER_INFO> GetCommunity_User()
	{
		return this.m_CommunityUserList;
	}

	public void RequestCommunityData(eCOMMUNITYDLG_SHOWTYPE _show_type)
	{
		if (_show_type == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_SELECTBATTLE)
		{
			this.Show((byte)_show_type);
		}
		else
		{
			GS_FRIEND_HELPSOLINFO_REQ gS_FRIEND_HELPSOLINFO_REQ = new GS_FRIEND_HELPSOLINFO_REQ();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			gS_FRIEND_HELPSOLINFO_REQ.nPersonID = charPersonInfo.GetPersonID();
			gS_FRIEND_HELPSOLINFO_REQ.type = (byte)_show_type;
			SendPacket.GetInstance().SendObject(916, gS_FRIEND_HELPSOLINFO_REQ);
		}
	}

	public void OnClickNameSort(IUIObject obj)
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_eSort = CommunityUI_DLG.eSORT.eSORT_NAME;
		this.m_bNameSort = !this.m_bNameSort;
		this.ChangeMode(this.m_eCurShowType);
		this.SetSortDelayTime();
	}

	public void OnClickConnectTimeSort(IUIObject obj)
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_eSort = CommunityUI_DLG.eSORT.eSORT_CONNECTTIME;
		this.m_bConnectTimeSort = !this.m_bConnectTimeSort;
		this.ChangeMode(this.m_eCurShowType);
		this.SetSortDelayTime();
	}

	public void OnClickFriendSolSort(IUIObject obj)
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_eSort = CommunityUI_DLG.eSORT.eSORT_FRIENDSOL;
		this.m_bFriendSolSort = !this.m_bFriendSolSort;
		this.ChangeMode(this.m_eCurShowType);
		this.SetSortDelayTime();
	}

	public void OnClickSupportSolSort(IUIObject obj)
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_eSort = CommunityUI_DLG.eSORT.eSORT_SUPPORTSOL;
		this.m_bSupportSolSort = !this.m_bSupportSolSort;
		this.ChangeMode(this.m_eCurShowType);
		this.SetSortDelayTime();
	}

	public void OnClickRewardSort(IUIObject obj)
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_eSort = CommunityUI_DLG.eSORT.eSORT_REWARD;
		this.m_bRewardSort = !this.m_bRewardSort;
		this.ChangeMode(this.m_eCurShowType);
		this.SetSortDelayTime();
	}

	public void SortCommunity()
	{
		this.m_CommunitySortList.Clear();
		this.m_CommunityFirstList.Clear();
		if (this.m_eCurShowType == eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET)
		{
			if (this.m_eSort == CommunityUI_DLG.eSORT.eSORT_CONNECTTIME)
			{
				this.SortCommunity_ConnectTime();
			}
			else if (this.m_eSort == CommunityUI_DLG.eSORT.eSORT_FRIENDSOL)
			{
				this.SortCommunity_FriendSol();
			}
			else if (this.m_eSort == CommunityUI_DLG.eSORT.eSORT_SUPPORTSOL)
			{
				this.SortCommunity_SupportSol();
			}
			else
			{
				this.SortCommunitySub(this.m_CommunityUserList);
			}
		}
		else
		{
			this.SortCommunity_GMSol();
		}
	}

	public void SortCommunity_ConnectTime()
	{
		for (int i = 0; i < this.m_CommunityUserList.Count; i++)
		{
			if (this.m_CommunityUserList[i].bConnect)
			{
				this.m_CommunityFirstList.Add(this.m_CommunityUserList[i]);
			}
			else
			{
				this.m_CommunitySortList.Add(this.m_CommunityUserList[i]);
			}
		}
		this.SortCommunitySub(this.m_CommunitySortList);
		this.m_CommunityUserList.Clear();
		for (int j = 0; j < this.m_CommunityFirstList.Count; j++)
		{
			this.m_CommunityUserList.Add(this.m_CommunityFirstList[j]);
		}
		for (int k = 0; k < this.m_CommunitySortList.Count; k++)
		{
			this.m_CommunityUserList.Add(this.m_CommunitySortList[k]);
		}
	}

	public void SortCommunity_FriendSol()
	{
		for (int i = 0; i < this.m_CommunityUserList.Count; i++)
		{
			if (0 >= this.m_CommunityUserList[i].Friend_HelpSolInfo.i32SolKind)
			{
				this.m_CommunityFirstList.Add(this.m_CommunityUserList[i]);
			}
			else
			{
				this.m_CommunitySortList.Add(this.m_CommunityUserList[i]);
			}
		}
		this.SortCommunitySub(this.m_CommunitySortList);
		this.m_CommunityUserList.Clear();
		if (this.m_bFriendSolSort)
		{
			for (int j = 0; j < this.m_CommunitySortList.Count; j++)
			{
				this.m_CommunityUserList.Add(this.m_CommunitySortList[j]);
			}
			for (int k = 0; k < this.m_CommunityFirstList.Count; k++)
			{
				this.m_CommunityUserList.Add(this.m_CommunityFirstList[k]);
			}
		}
		else
		{
			for (int l = 0; l < this.m_CommunityFirstList.Count; l++)
			{
				this.m_CommunityUserList.Add(this.m_CommunityFirstList[l]);
			}
			for (int m = 0; m < this.m_CommunitySortList.Count; m++)
			{
				this.m_CommunityUserList.Add(this.m_CommunitySortList[m]);
			}
		}
	}

	public void SortCommunity_SupportSol()
	{
		for (int i = 0; i < this.m_CommunityUserList.Count; i++)
		{
			if (this.m_CommunityUserList[i].MyHelpSol() == null)
			{
				this.m_CommunityFirstList.Add(this.m_CommunityUserList[i]);
			}
			else
			{
				this.m_CommunitySortList.Add(this.m_CommunityUserList[i]);
			}
		}
		this.SortCommunitySub(this.m_CommunitySortList);
		this.m_CommunityUserList.Clear();
		if (this.m_bSupportSolSort)
		{
			for (int j = 0; j < this.m_CommunitySortList.Count; j++)
			{
				this.m_CommunityUserList.Add(this.m_CommunitySortList[j]);
			}
			for (int k = 0; k < this.m_CommunityFirstList.Count; k++)
			{
				this.m_CommunityUserList.Add(this.m_CommunityFirstList[k]);
			}
		}
		else
		{
			for (int l = 0; l < this.m_CommunityFirstList.Count; l++)
			{
				this.m_CommunityUserList.Add(this.m_CommunityFirstList[l]);
			}
			for (int m = 0; m < this.m_CommunitySortList.Count; m++)
			{
				this.m_CommunityUserList.Add(this.m_CommunitySortList[m]);
			}
		}
	}

	public void SortCommunity_GMSol()
	{
		this.m_CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(CommunityUI_DLG.CompareHelpSolLevel));
	}

	public void SortCommunitySub(List<COMMUNITY_USER_INFO> CommunityUserList)
	{
		switch (this.m_eSort)
		{
		case CommunityUI_DLG.eSORT.eSORT_NAME:
			this.ChangeSortTexture(this.m_bNameSort);
			if (this.m_bNameSort)
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareNameDESC));
			}
			else
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareNameASC));
			}
			break;
		case CommunityUI_DLG.eSORT.eSORT_CONNECTTIME:
			this.ChangeSortTexture(this.m_bConnectTimeSort);
			if (this.m_bConnectTimeSort)
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareConnectTimeDESC));
			}
			else
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareConnectTimeASC));
			}
			break;
		case CommunityUI_DLG.eSORT.eSORT_FRIENDSOL:
			this.ChangeSortTexture(this.m_bFriendSolSort);
			if (this.m_bFriendSolSort)
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareFriendSolDESC));
			}
			else
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareFriendSolASC));
			}
			break;
		case CommunityUI_DLG.eSORT.eSORT_SUPPORTSOL:
			this.ChangeSortTexture(this.m_bSupportSolSort);
			if (this.m_bSupportSolSort)
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareSupportSolDESC));
			}
			else
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareSupportSolASC));
			}
			break;
		case CommunityUI_DLG.eSORT.eSORT_REWARD:
			this.ChangeSortTexture(this.m_bRewardSort);
			if (this.m_bRewardSort)
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareRewardDESC));
			}
			else
			{
				CommunityUserList.Sort(new Comparison<COMMUNITY_USER_INFO>(this.CompareRewardASC));
			}
			break;
		}
	}

	private int CompareNameDESC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		if (b.strName.Equals(a.strName))
		{
			return b.i16Level.CompareTo(a.i16Level);
		}
		return b.strName.CompareTo(a.strName);
	}

	private int CompareNameASC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		if (a.strName.Equals(b.strName))
		{
			return a.i16Level.CompareTo(b.i16Level);
		}
		return a.strName.CompareTo(b.strName);
	}

	private int CompareConnectTimeDESC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		if (a.bConnect || b.bConnect)
		{
			return 0;
		}
		return b.i64LogoutTime.CompareTo(a.i64LogoutTime);
	}

	private int CompareConnectTimeASC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		if (a.bConnect || b.bConnect)
		{
			return 0;
		}
		return a.i64LogoutTime.CompareTo(b.i64LogoutTime);
	}

	private int CompareFriendSolDESC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		return b.Friend_HelpSolInfo.iSolLevel.CompareTo(a.Friend_HelpSolInfo.iSolLevel);
	}

	private int CompareFriendSolASC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		return a.Friend_HelpSolInfo.iSolLevel.CompareTo(b.Friend_HelpSolInfo.iSolLevel);
	}

	private int CompareSupportSolDESC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		NkSoldierInfo nkSoldierInfo = a.MyHelpSol();
		NkSoldierInfo nkSoldierInfo2 = b.MyHelpSol();
		if (nkSoldierInfo == null || nkSoldierInfo2 == null)
		{
			return 0;
		}
		return nkSoldierInfo2.GetLevel().CompareTo(nkSoldierInfo.GetLevel());
	}

	private int CompareSupportSolASC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		NkSoldierInfo nkSoldierInfo = a.MyHelpSol();
		NkSoldierInfo nkSoldierInfo2 = b.MyHelpSol();
		if (nkSoldierInfo == null || nkSoldierInfo2 == null)
		{
			return 0;
		}
		return nkSoldierInfo.GetLevel().CompareTo(nkSoldierInfo2.GetLevel());
	}

	private int CompareRewardDESC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		return b.GetHelpSolExp().CompareTo(a.GetHelpSolExp());
	}

	private int CompareRewardASC(COMMUNITY_USER_INFO a, COMMUNITY_USER_INFO b)
	{
		return a.GetHelpSolExp().CompareTo(b.GetHelpSolExp());
	}

	private void SetSortDelayTime()
	{
		if (0f < this.m_fSortDelayTime)
		{
			return;
		}
		this.m_fSortDelayTime = Time.time + 1f;
	}

	public void ChangeSortTexture(bool bSort)
	{
		for (int i = 0; i < 5; i++)
		{
			if (!(null == this.m_dtSort[i]))
			{
				if (this.m_eSort == (CommunityUI_DLG.eSORT)i)
				{
					this.m_dtSort[i].Visible = true;
				}
				else
				{
					this.m_dtSort[i].Visible = false;
				}
			}
		}
		if (bSort)
		{
			if (null != this.m_dtSort[(int)this.m_eSort])
			{
				this.m_dtSort[(int)this.m_eSort].SetTextureKey("Win_I_ArrowDown");
			}
		}
		else if (null != this.m_dtSort[(int)this.m_eSort])
		{
			this.m_dtSort[(int)this.m_eSort].SetTextureKey("Win_I_ArrowUp");
		}
	}

	public void HideSort()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_dtSort[i].Visible = false;
		}
		this.m_btNameSort.controlIsEnabled = false;
		this.m_btConnectTimeSort.controlIsEnabled = false;
		this.m_btRewardSort.controlIsEnabled = false;
	}
}
