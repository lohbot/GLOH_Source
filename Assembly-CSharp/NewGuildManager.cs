using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NewGuildManager : NrTSingleton<NewGuildManager>
{
	private long m_lCreateMoney;

	private short m_iLevelForCreate;

	private int m_iMaxGuildNum;

	private int m_iMaxMemberNum;

	private int m_iMaxApplicantsNum;

	private byte m_byNewMasterCheckDay;

	private long m_i64NewbieLimitTime;

	private long m_i64PostText;

	private int m_i32FundExchangeRate;

	private int m_i32FundDonation;

	private NewGuild m_NewGuild = new NewGuild();

	private UIBaseInfoLoader m_DefualtTexture;

	private int m_iReadyApplicantCount;

	private bool m_bCanGetGoldenEggReward;

	private NewGuildAgit m_NewGuildAgit = new NewGuildAgit();

	private NewGuildManager()
	{
		this.m_lCreateMoney = 0L;
		this.m_iLevelForCreate = 0;
		this.m_iMaxGuildNum = 0;
		this.m_iMaxMemberNum = 0;
		this.m_iMaxApplicantsNum = 0;
		this.m_byNewMasterCheckDay = 0;
		this.m_i64NewbieLimitTime = 0L;
		this.m_i64PostText = 0L;
		this.m_i32FundExchangeRate = 0;
		this.m_i32FundDonation = 0;
		this.m_iReadyApplicantCount = 0;
	}

	public void Clear()
	{
		this.m_NewGuild.Clear();
	}

	public void ClearDlg()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_MEMBER_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_LIST_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_MAIN_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_INVITE_MENU_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_INVITE_INPUT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_ADMINMENU_DLG);
	}

	public void SetGuildInfo(GS_NEWGUILD_INFO_ACK ACK)
	{
		this.m_lCreateMoney = ACK.i64CreateMoney;
		this.m_iLevelForCreate = ACK.i16LevelForCreate;
		this.m_iMaxGuildNum = ACK.i32MaxGuildNum;
		this.m_iMaxMemberNum = ACK.i32MaxMemberNum;
		this.m_iMaxApplicantsNum = ACK.i32MaxApplicantsNum;
		this.m_byNewMasterCheckDay = ACK.ui8NewMasterCheckDay;
		this.m_i64NewbieLimitTime = ACK.i64NewbieLimitTime;
		this.m_i64PostText = ACK.i64PostText;
		this.m_NewGuildAgit.AgitLevel = ACK.i16AgitLevel;
		this.m_NewGuildAgit.AgitExp = ACK.i64AgitExp;
		this.m_i32FundExchangeRate = ACK.i32FundExchangeRate;
		this.m_i32FundDonation = ACK.i32FundDonation;
		this.m_NewGuild.SetGuildInfo(ACK.GuildInfo);
	}

	public long GetCreateMoney()
	{
		return this.m_lCreateMoney;
	}

	public short GetLevelForCreate()
	{
		return this.m_iLevelForCreate;
	}

	public int GetMaxGuildNum()
	{
		return this.m_iMaxGuildNum;
	}

	public int GetMaxMemberNum()
	{
		return this.m_iMaxMemberNum;
	}

	public int GetMaxApplicantsNum()
	{
		return this.m_iMaxApplicantsNum;
	}

	public byte GetNewMasterCheckDay()
	{
		return this.m_byNewMasterCheckDay;
	}

	public long GetNewbieLimitTime()
	{
		return this.m_i64NewbieLimitTime;
	}

	public long GetNewbieLimitTimeHour()
	{
		return this.m_i64NewbieLimitTime / 3600L;
	}

	public long GetPostTex()
	{
		return this.m_i64PostText;
	}

	public short GetAgitLevel()
	{
		return this.m_NewGuildAgit.AgitLevel;
	}

	public long GetAgitExp()
	{
		return this.m_NewGuildAgit.AgitExp;
	}

	public int GetFundExchangeRate()
	{
		return this.m_i32FundExchangeRate;
	}

	public int GetFundDonation()
	{
		return this.m_i32FundDonation;
	}

	public int GetGoldenEggGetCount()
	{
		return this.m_NewGuildAgit.GetGoldenEggGetCount();
	}

	public void SetGoldenEggGetCount(int GoldenEggGetCount)
	{
		this.m_NewGuildAgit.SetGoldenEggGetCount(GoldenEggGetCount);
	}

	public List<AGIT_GOLDENEGG_INFO_SUB_DATA> GetRewardPersonInfoList()
	{
		return this.m_NewGuildAgit.GetRewardPersonInfoList();
	}

	public long GetGoldenEggGetLastPerson()
	{
		return this.m_NewGuildAgit.GetGoldenEggGetLastPerson();
	}

	public bool CanGetGoldenEggReward()
	{
		return this.m_bCanGetGoldenEggReward;
	}

	public void SetCanGoldenEggReward(bool bIsGetReward)
	{
		this.m_bCanGetGoldenEggReward = bIsGetReward;
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
		}
		GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
		if (guildCollect_DLG != null)
		{
			guildCollect_DLG.Update_Notice();
		}
	}

	public void AddMemberInfo(NEWGUILDMEMBER_INFO NewGuildMemberInfo)
	{
		this.m_NewGuild.AddMemberInfo(NewGuildMemberInfo);
	}

	public void AddApplicantInfo(NEWGUILDMEMBER_APPLICANT_INFO NewGuildApplicantInfo)
	{
		this.m_NewGuild.AddApplicantInfo(NewGuildApplicantInfo);
		this.SetReadyApplicantCount(this.m_NewGuild.GetApplicantCount());
		if (0 < this.m_NewGuild.GetApplicantCount() && this.IsApplicantMemberNum(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
			}
			GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
			if (guildCollect_DLG != null)
			{
				guildCollect_DLG.Update_Notice();
			}
		}
	}

	public long GetGuildID()
	{
		return this.m_NewGuild.GetGuildID();
	}

	public short GetLevel()
	{
		return this.m_NewGuild.GetLevel();
	}

	public long GetExp()
	{
		return this.m_NewGuild.GetExp();
	}

	public string GetGuildName()
	{
		return this.m_NewGuild.GetGuildName();
	}

	public long GetCreateDate()
	{
		return this.m_NewGuild.GetCreateDate();
	}

	public byte GetSetImage()
	{
		return this.m_NewGuild.GetSetImage();
	}

	public string GetGuildMessage()
	{
		return this.m_NewGuild.GetGuildMessage();
	}

	public short GetRank()
	{
		return this.m_NewGuild.GetRank();
	}

	public long GetFund()
	{
		return this.m_NewGuild.GetFund();
	}

	public string GetGuildNotice()
	{
		return this.m_NewGuild.GetGuildNotice();
	}

	public bool IsMaster(long lPersonID)
	{
		return this.m_NewGuild.IsMaster(lPersonID);
	}

	public bool IsSubMaster(long lPersonID)
	{
		return this.m_NewGuild.IsSubMaster(lPersonID);
	}

	public bool IsOfficer(long lPersonID)
	{
		return this.m_NewGuild.IsOfficer(lPersonID);
	}

	public bool IsRankChange(long lPersonID)
	{
		return this.m_NewGuild.IsRankChange(lPersonID);
	}

	public bool IsDischargeMember(long lPersonID)
	{
		return this.m_NewGuild.IsDischargeMember(lPersonID);
	}

	public bool IsInviteMember(long lPersonID)
	{
		return this.m_NewGuild.IsInviteMember(lPersonID);
	}

	public bool IsApplicantMemberNum(long lPersonID)
	{
		return this.m_NewGuild.IsApplicantMemberNum(lPersonID);
	}

	public int GetMemberCount()
	{
		return this.m_NewGuild.GetMemberCount();
	}

	public NewGuildMember GetMemberInfoFromIndex(int iIndex)
	{
		return this.m_NewGuild.GetMemberInfoFromIndex(iIndex);
	}

	public NewGuildMember GetMemberInfoFromPersonID(long lPersonID)
	{
		return this.m_NewGuild.GetMemberInfoFromPersonID(lPersonID);
	}

	public int GetApplicantCount()
	{
		return this.m_NewGuild.GetApplicantCount();
	}

	public NewGuildApplicant GetApplicantInfoFromIndex(int iIndex)
	{
		return this.m_NewGuild.GetApplicantInfoFromIndex(iIndex);
	}

	public NewGuildApplicant GetApplicantInfoFromPersonID(long lPersonID)
	{
		return this.m_NewGuild.GetApplicantInfoFromPersonID(lPersonID);
	}

	public bool IsGuildWar()
	{
		return this.m_NewGuild.IsGuildWar();
	}

	public void SetExitAgit(bool isExitAgit)
	{
		this.m_NewGuild.SetExitAgit(isExitAgit);
	}

	public bool IsExitAgit()
	{
		return this.m_NewGuild.IsExitAgit();
	}

	public void RemoveApplicant(long lPersonID)
	{
		this.m_NewGuild.RemoveApplicant(lPersonID);
		this.SetReadyApplicantCount(this.m_NewGuild.GetApplicantCount());
		if (this.IsApplicantMemberNum(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
			}
			GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
			if (guildCollect_DLG != null)
			{
				guildCollect_DLG.Update_Notice();
			}
		}
	}

	public void RemoveGuildMember(long lPersonID)
	{
		this.m_NewGuild.RemoveGuildMember(lPersonID);
	}

	public void SetImage(byte bySetImage)
	{
		this.m_NewGuild.SetImage(bySetImage);
	}

	public void SetReadyApplicantCount(int iReadyApplicantCount)
	{
		this.m_iReadyApplicantCount = iReadyApplicantCount;
	}

	public int GetReadyApplicantCount()
	{
		if (this.IsApplicantMemberNum(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			return this.m_iReadyApplicantCount;
		}
		return 0;
	}

	public string GetRankText(NewGuildDefine.eNEWGUILD_MEMBER_RANK eRank)
	{
		switch (eRank)
		{
		case NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("623");
		case NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_REGULAR:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("624");
		case NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("625");
		case NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_SUB_MASTER:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1850");
		case NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_MASTER:
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1849");
		default:
			return string.Empty;
		}
	}

	public void ChangeGuildMessage(string strGuildMessage)
	{
		this.m_NewGuild.ChangeGuildMessage(strGuildMessage);
	}

	public string GetMemberRankText(long lPersonID)
	{
		NewGuildDefine.eNEWGUILD_MEMBER_RANK memberRank = this.m_NewGuild.GetMemberRank(lPersonID);
		return this.GetRankText(memberRank);
	}

	public void ChangeMemberRank(long lPersonID, NewGuildDefine.eNEWGUILD_MEMBER_RANK eRank)
	{
		this.m_NewGuild.ChangeMemberRank(lPersonID, eRank);
	}

	public bool IsAddMember()
	{
		return this.GetMaxMemberNum() > this.m_NewGuild.GetMemberCount();
	}

	public UIBaseInfoLoader GetGuildDefualtTexture()
	{
		if (this.m_DefualtTexture == null)
		{
			this.m_DefualtTexture = new UIBaseInfoLoader();
			this.m_DefualtTexture.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
			this.m_DefualtTexture.Material = string.Format("Mobile/Material/Battleskill_Icon/BattleSkill001_mobile", new object[0]);
			float left = 255f;
			float top = 204f;
			this.m_DefualtTexture.UVs = new Rect(left, top, 50f, 50f);
		}
		return this.m_DefualtTexture;
	}

	public void AddJoin()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsMineApply((short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
		{
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
			}
			GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
			if (guildCollect_DLG != null)
			{
				guildCollect_DLG.Set_GuildButtons();
				guildCollect_DLG.Update_Notice();
			}
		}
	}

	public void Leave()
	{
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
		}
		GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
		if (guildCollect_DLG != null)
		{
			guildCollect_DLG.Set_GuildButtons();
			guildCollect_DLG.Update_Notice();
		}
	}

	public bool IsGuildPost(long lPersonID)
	{
		return this.m_NewGuild.IsGuildPost(lPersonID);
	}

	public bool IsChangeGuildName(string strGuildName)
	{
		return strGuildName.Contains("*");
	}

	public void ChangeGuildName(string strGuildName)
	{
		this.m_NewGuild.ChangeGuildName(strGuildName);
	}

	public int GetGuildMemberIndex(long lPersonID)
	{
		return this.m_NewGuild.GetGuildMemberIndex(lPersonID);
	}

	public void Send_GS_NEWGUILD_INFO_REQ(short iLoadType)
	{
		GS_NEWGUILD_INFO_REQ gS_NEWGUILD_INFO_REQ = new GS_NEWGUILD_INFO_REQ();
		gS_NEWGUILD_INFO_REQ.i16LoadInfoType = iLoadType;
		SendPacket.GetInstance().SendObject(1821, gS_NEWGUILD_INFO_REQ);
	}

	public void ChangeGuildNotice(string strGuildNotice)
	{
		this.m_NewGuild.ChangeGuildNotice(strGuildNotice);
	}

	public void Send_GS_NEWGUILD_AGIT_CREATE_REQ()
	{
		GS_NEWGUILD_AGIT_CREATE_REQ obj = new GS_NEWGUILD_AGIT_CREATE_REQ();
		SendPacket.GetInstance().SendObject(2302, obj);
	}

	public void SetFund(long i64Fund)
	{
		this.m_NewGuild.SetFund(i64Fund);
	}

	public void SetAgitLevel(short i16AgitLevel)
	{
		this.m_NewGuildAgit.AgitLevel = i16AgitLevel;
	}

	public void SetAgitExp(long i64AgitExp)
	{
		this.m_NewGuildAgit.AgitExp = i64AgitExp;
	}

	public void Send_GS_NEWGUILD_AGIT_LEVEL_REQ()
	{
		GS_NEWGUILD_AGIT_LEVEL_REQ obj = new GS_NEWGUILD_AGIT_LEVEL_REQ();
		SendPacket.GetInstance().SendObject(2306, obj);
	}

	public void Send_GS_NEWGUILD_AGIT_ENTER_REQ()
	{
		GS_NEWGUILD_AGIT_ENTER_REQ obj = new GS_NEWGUILD_AGIT_ENTER_REQ();
		SendPacket.GetInstance().SendObject(2308, obj);
	}

	public void Send_GS_NEWGUILD_DONATION_FUND_REQ()
	{
		GS_NEWGUILD_DONATION_FUND_REQ obj = new GS_NEWGUILD_DONATION_FUND_REQ();
		SendPacket.GetInstance().SendObject(2310, obj);
	}

	public void Send_GS_NEWGUILD_FUND_USE_HISTORY_GET_REQ()
	{
		GS_NEWGUILD_FUND_USE_HISTORY_GET_REQ obj = new GS_NEWGUILD_FUND_USE_HISTORY_GET_REQ();
		SendPacket.GetInstance().SendObject(2314, obj);
	}

	public string GetStringFromFundUseType(NewGuildDefine.eNEWGUILD_FUND_USE_TYPE eNewGuildFundUseType)
	{
		string result = string.Empty;
		switch (eNewGuildFundUseType)
		{
		case NewGuildDefine.eNEWGUILD_FUND_USE_TYPE.eNEWGUILD_FUND_USE_TYPE_AGIT_CREATE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2703");
			break;
		case NewGuildDefine.eNEWGUILD_FUND_USE_TYPE.eNEWGUILD_FUND_USE_TYPE_AGIT_LEVELUP:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2704");
			break;
		case NewGuildDefine.eNEWGUILD_FUND_USE_TYPE.eNEWGUILD_FUND_USE_TYPE_AGIT_BUY_NPC:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2705");
			break;
		case NewGuildDefine.eNEWGUILD_FUND_USE_TYPE.eNEWGUILD_FUND_USE_TYPE_AGIT_GUARDIAN:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2706");
			break;
		}
		return result;
	}

	public void Set_GS_NEWGUILD_AGIT_ADD_NPC_ACK(GS_NEWGUILD_AGIT_ADD_NPC_ACK ACK)
	{
		this.SetFund(ACK.i64AfterGuildFund);
		this.AddAgitNPCData(new AGIT_NPC_SUB_DATA
		{
			ui8NPCType = ACK.ui8NPCType,
			i16NPCLevel = ACK.i16NPCLevel,
			i64NPCEndTime = ACK.i64NPCEndTime
		});
		NewGuildDefine.eNEWGUILD_NPC_TYPE ui8NPCType = (NewGuildDefine.eNEWGUILD_NPC_TYPE)ACK.ui8NPCType;
		if (ui8NPCType == NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_MERCHANT)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex != 12 && Scene.CurScene != Scene.Type.BATTLE)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.MsgOKMoveAgit), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("252"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("253"), eMsgType.MB_OK_CANCEL, 2);
			}
		}
	}

	public void GS_NEWGUILD_AGIT_GOLDENEGG_REWARD_NFY()
	{
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("868"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex != 12 && Scene.CurScene != Scene.Type.BATTLE && Scene.CurScene != Scene.Type.PREPAREGAME)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.MsgOKMoveAgit), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("357"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("358"), eMsgType.MB_OK_CANCEL, 2);
		}
	}

	public void MsgOKMoveAgit(object a_oObject)
	{
		this.Send_GS_NEWGUILD_AGIT_ENTER_REQ();
	}

	public void AddAgitNPCData(AGIT_NPC_SUB_DATA Data)
	{
		this.m_NewGuildAgit.AddAgitNPCData(Data);
	}

	public int GetAgitNPCSubDataCount()
	{
		return this.m_NewGuildAgit.GetAgitNPCSubDataCount();
	}

	public AGIT_NPC_SUB_DATA GetAgitNPCSubData(int iIndex)
	{
		return this.m_NewGuildAgit.GetAgitNPCSubData(iIndex);
	}

	public AGIT_NPC_SUB_DATA GetAgitNPCSubDataFromNPCType(byte iNPCType)
	{
		return this.m_NewGuildAgit.GetAgitNPCSubDataFromNPCType(iNPCType);
	}

	public void DelAgitNPC(byte ui8NPCType)
	{
		this.m_NewGuildAgit.DelAgitNPC(ui8NPCType);
	}

	public void Send_GS_NEWGUILD_AGIT_MERCHANT_INFO_REQ()
	{
		GS_NEWGUILD_AGIT_MERCHANT_INFO_REQ obj = new GS_NEWGUILD_AGIT_MERCHANT_INFO_REQ();
		SendPacket.GetInstance().SendObject(2316, obj);
	}

	public void UpdateAgitNPC(int iMapIdx)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsAgitLimit())
		{
			return;
		}
		if (iMapIdx != 12)
		{
			return;
		}
		this.m_NewGuildAgit.ClearNPCInfo();
		for (byte b = 1; b < 6; b += 1)
		{
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(b.ToString());
			if (agitNPCData != null)
			{
				NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
				if (charKindInfoFromCode != null)
				{
					NrCharBase charByCharKind = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(charKindInfoFromCode.GetCharKind());
					if (charByCharKind != null)
					{
						NrTSingleton<NkCharManager>.Instance.DeleteChar(charByCharKind.GetID());
					}
				}
			}
		}
		for (int i = 0; i < this.m_NewGuildAgit.GetAgitNPCSubDataCount(); i++)
		{
			AGIT_NPC_SUB_DATA agitNPCSubData = this.m_NewGuildAgit.GetAgitNPCSubData(i);
			if (agitNPCSubData != null)
			{
				this.MakeAgitNPC(agitNPCSubData.ui8NPCType);
			}
		}
		this.ShowAgitInfoDLG();
	}

	public void ShowAgitInfoDLG()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsAgitLimit())
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.MapIndex != 12)
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_INFO_DLG);
	}

	public void MakeAgitNPC(byte iNPCType)
	{
		AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(iNPCType.ToString());
		if (agitNPCData == null)
		{
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(charKindInfoFromCode.GetCharKind()) != null)
		{
			return;
		}
		NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
		nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfoFromCode.GetName());
		nEW_MAKECHAR_INFO.CharPos.x = agitNPCData.fPosX;
		nEW_MAKECHAR_INFO.CharPos.y = 0f;
		nEW_MAKECHAR_INFO.CharPos.z = agitNPCData.fPosY;
		float f = agitNPCData.fDirection * 0.0174532924f;
		nEW_MAKECHAR_INFO.Direction.x = 1f * Mathf.Sin(f);
		nEW_MAKECHAR_INFO.Direction.y = 0f;
		nEW_MAKECHAR_INFO.Direction.z = 1f * Mathf.Cos(f);
		nEW_MAKECHAR_INFO.CharKind = charKindInfoFromCode.GetCharKind();
		nEW_MAKECHAR_INFO.CharKindType = 3;
		nEW_MAKECHAR_INFO.CharUnique = NrTSingleton<NkCharManager>.Instance.GetClientNpcUnique();
		if (nEW_MAKECHAR_INFO.CharUnique == 0)
		{
		}
		int num = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, false);
		TsLog.LogOnlyEditor(string.Concat(new object[]
		{
			"AgitNPC : ",
			agitNPCData.ui8NPCType,
			" : ",
			num
		}));
		AgitNPCInfo agitNPCInfo = new AgitNPCInfo();
		agitNPCInfo.SetCharID(num);
		agitNPCInfo.SetNPCType(agitNPCData.ui8NPCType);
		this.m_NewGuildAgit.AddNPCInfo(agitNPCInfo);
	}

	public int GetAgitNPCCharKindFromNPCType(byte iNPCType)
	{
		AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(iNPCType.ToString());
		if (agitNPCData == null)
		{
			return 0;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
		if (charKindInfoFromCode == null)
		{
			return 0;
		}
		return charKindInfoFromCode.GetCharKind();
	}

	public NewGuildDefine.eNEWGUILD_NPC_TYPE GetAgitNPCTypeFromCharKind(int i32CharKind)
	{
		for (int i = 1; i < 6; i++)
		{
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(i.ToString());
			if (agitNPCData != null)
			{
				NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(agitNPCData.strCharCode);
				if (charKindInfoFromCode != null)
				{
					if (charKindInfoFromCode.GetCharKind() == i32CharKind)
					{
						return (NewGuildDefine.eNEWGUILD_NPC_TYPE)i;
					}
				}
			}
		}
		return NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_NONE;
	}

	public void Send_GS_NEWGUILD_AGIT_NPC_USE_REQ(byte iNPCType, short i16SellType)
	{
		GS_NEWGUILD_AGIT_NPC_USE_REQ gS_NEWGUILD_AGIT_NPC_USE_REQ = new GS_NEWGUILD_AGIT_NPC_USE_REQ();
		gS_NEWGUILD_AGIT_NPC_USE_REQ.ui8NPCType = iNPCType;
		gS_NEWGUILD_AGIT_NPC_USE_REQ.i16SellType = i16SellType;
		SendPacket.GetInstance().SendObject(2318, gS_NEWGUILD_AGIT_NPC_USE_REQ);
	}

	public void Set_GS_NEWGUILD_AGIT_NPC_USE_ACK(int i32Result)
	{
		if (i32Result != 1)
		{
			if (i32Result == 2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("795"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				Agit_MerchantDlg agit_MerchantDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MERCHANT_DLG) as Agit_MerchantDlg;
				if (agit_MerchantDlg != null)
				{
					agit_MerchantDlg.RefreshInfo();
				}
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void Send_GS_NEWGUILD_AGIT_DEL_NPC_REQ(byte ui8NPCType)
	{
		GS_NEWGUILD_AGIT_DEL_NPC_REQ gS_NEWGUILD_AGIT_DEL_NPC_REQ = new GS_NEWGUILD_AGIT_DEL_NPC_REQ();
		gS_NEWGUILD_AGIT_DEL_NPC_REQ.ui8NPCType = ui8NPCType;
		SendPacket.GetInstance().SendObject(2304, gS_NEWGUILD_AGIT_DEL_NPC_REQ);
	}

	public bool IsAgitNPC(byte i8NPCType)
	{
		return this.m_NewGuildAgit.IsAgitNPC(i8NPCType);
	}

	public void Send_GS_NEWGUILD_AGIT_ADD_NPC_REQ(byte ui8NPCType, short i16NPCLevel)
	{
		GS_NEWGUILD_AGIT_ADD_NPC_REQ gS_NEWGUILD_AGIT_ADD_NPC_REQ = new GS_NEWGUILD_AGIT_ADD_NPC_REQ();
		gS_NEWGUILD_AGIT_ADD_NPC_REQ.ui8NPCType = ui8NPCType;
		gS_NEWGUILD_AGIT_ADD_NPC_REQ.i16NPCLevel = i16NPCLevel;
		SendPacket.GetInstance().SendObject(2300, gS_NEWGUILD_AGIT_ADD_NPC_REQ);
	}

	public void Set_Agit_Resut(int i32Result)
	{
		switch (i32Result)
		{
		case 9401:
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("754"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			break;
		case 9404:
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("771"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			break;
		}
		if (i32Result == 2)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void ClearAgit()
	{
		this.m_NewGuildAgit.Clear();
	}

	public bool CanDeclareWarSet()
	{
		return !NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit();
	}
}
