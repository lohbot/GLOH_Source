using GameMessage.Private;
using Ndoors.Framework.Stage;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NewGuildWarDetailInfoDlg : Form
{
	public enum eSTATE
	{
		eSTATE_RAID_LEADER,
		eSTATE_RAID_MEMBER,
		eSTATE_ENEMY_INFO,
		eSTATE_MAX
	}

	public enum eMODE
	{
		eMODE_NORMAL,
		eMODE_CHANGE
	}

	public enum eEFFECT
	{
		eEFFECT_LEADER_LRAGE,
		eEFFECT_LEADER_SMALL,
		eEFFECT_MAX
	}

	private const int MAX_SOL_SLOT = 9;

	private const float DELAY_TIME = 2f;

	private DrawTexture m_dtMainBG;

	private Label m_lbUserName;

	private ItemTexture[] m_itSolInfo = new ItemTexture[9];

	private Label m_lbGuildWarInfo;

	private Label m_lbGuildName;

	private DrawTexture m_dtGuildMark;

	private DrawTexture[] m_dtEnemySelectImage = new DrawTexture[9];

	private ItemTexture[] m_itEnemySelectSolLarge = new ItemTexture[9];

	private Button[] m_btEnemySelectSolLarge = new Button[9];

	private ItemTexture[] m_itEnemySelectSolSmall = new ItemTexture[9];

	private Button[] m_btEnemySelectSolSmall = new Button[9];

	private DrawTexture[] m_dtEnemyLeader = new DrawTexture[9];

	private DrawTexture[] m_dtAllySelectImage = new DrawTexture[9];

	private ItemTexture[] m_itAllySelectSolLarge = new ItemTexture[9];

	private Button[] m_btAllySelectSolLarge = new Button[9];

	private DrawTexture[] m_dtAllyLeaderLarge = new DrawTexture[9];

	private ItemTexture[] m_itAllySelectSolSmall = new ItemTexture[9];

	private Button[] m_btAllySelectSolSmall = new Button[9];

	private DrawTexture[] m_dtAllyLeaderSmall = new DrawTexture[9];

	private Button m_btMemberStart;

	private Button m_btMemberCancel;

	private Button m_btMemberClose;

	private Button m_btLeaderChange;

	private Button m_btLeaderCancel;

	private Button m_btLeaderClose;

	private Button m_btEnemyClose;

	private string m_strText = string.Empty;

	private List<ApplyInfo> m_ApplyInfo = new List<ApplyInfo>();

	private NewGuildWarDetailInfoDlg.eSTATE m_eState;

	private long m_i64GuildID;

	private byte m_iRaidUnique;

	private int m_iCurSelectIndex = -1;

	private int m_iOldSelectIndex;

	private NewGuildWarDetailInfoDlg.eMODE m_eMode;

	private int m_iChangeCurSelectIndex = -1;

	private int m_iChangeOldSelectIndex = -1;

	private long[] m_i64ChangeMilitaryID = new long[9];

	private float m_fDelayTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/dlg_guildwardetailinfo", G_ID.GUILDWAR_DETAILINFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtMainBG = (base.GetControl("DT_ImageBG") as DrawTexture);
		this.m_lbUserName = (base.GetControl("Label_Label64") as Label);
		this.m_lbGuildWarInfo = (base.GetControl("LB_GuildWarInfo") as Label);
		this.m_lbGuildName = (base.GetControl("LB_GuildName") as Label);
		this.m_dtGuildMark = (base.GetControl("DT_GuildMark") as DrawTexture);
		for (int i = 0; i < 9; i++)
		{
			this.m_strText = string.Format("IT_SolInfo0{0}", i + 1);
			this.m_itSolInfo[i] = (base.GetControl(this.m_strText) as ItemTexture);
		}
		for (int i = 0; i < 9; i++)
		{
			this.m_strText = string.Format("DT_SelectImage0{0}", i + 1);
			this.m_dtEnemySelectImage[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("IT_SelectSolImage0{0}", i + 1);
			this.m_itEnemySelectSolLarge[i] = (base.GetControl(this.m_strText) as ItemTexture);
			this.m_strText = string.Format("Btn_SelectSol0{0}", i + 1);
			this.m_btEnemySelectSolLarge[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btEnemySelectSolLarge[i].TabIndex = i;
			this.m_btEnemySelectSolLarge[i].EffectAni = false;
			this.m_btEnemySelectSolLarge[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectEnemyInfoLarge));
			this.m_strText = string.Format("IT_SolImage0{0}", i + 1);
			this.m_itEnemySelectSolSmall[i] = (base.GetControl(this.m_strText) as ItemTexture);
			this.m_strText = string.Format("Btn_Sol0{0}", i + 1);
			this.m_btEnemySelectSolSmall[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btEnemySelectSolSmall[i].TabIndex = i;
			this.m_btEnemySelectSolSmall[i].EffectAni = false;
			this.m_btEnemySelectSolSmall[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectEnemyInfoSmall));
			this.m_strText = string.Format("DT_PersonImage0{0}", i + 1);
			this.m_dtEnemyLeader[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("DT_R_SelectSol0{0}", i + 1);
			this.m_dtAllySelectImage[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("IT_R_SelectSolImage0{0}", i + 1);
			this.m_itAllySelectSolLarge[i] = (base.GetControl(this.m_strText) as ItemTexture);
			this.m_strText = string.Format("Btn_R_SelectSol0{0}", i + 1);
			this.m_btAllySelectSolLarge[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btAllySelectSolLarge[i].TabIndex = i;
			this.m_btAllySelectSolLarge[i].EffectAni = false;
			this.m_btAllySelectSolLarge[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectAllyInfoLarge));
			this.m_strText = string.Format("DT_R_SelectImage0{0}", i + 1);
			this.m_dtAllyLeaderLarge[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("IT_R_SolImage0{0}", i + 1);
			this.m_itAllySelectSolSmall[i] = (base.GetControl(this.m_strText) as ItemTexture);
			this.m_strText = string.Format("Btn_R_Sol0{0}", i + 1);
			this.m_btAllySelectSolSmall[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btAllySelectSolSmall[i].TabIndex = i;
			this.m_btAllySelectSolSmall[i].EffectAni = false;
			this.m_btAllySelectSolSmall[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectAllyInfoSmall));
			this.m_strText = string.Format("DT_R_PersonImage0{0}", i + 1);
			this.m_dtAllyLeaderSmall[i] = (base.GetControl(this.m_strText) as DrawTexture);
		}
		this.m_btMemberStart = (base.GetControl("Btn_Start01") as Button);
		this.m_btMemberStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMemberStart));
		this.m_btMemberCancel = (base.GetControl("Btn_End01") as Button);
		this.m_btMemberCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMemberCancel));
		this.m_btMemberClose = (base.GetControl("Btn_Close01") as Button);
		this.m_btMemberClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMemberClose));
		this.m_btLeaderChange = (base.GetControl("Btn_Change01") as Button);
		this.m_btLeaderChange.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLeaderRaidBattlePosChange));
		this.m_btLeaderCancel = (base.GetControl("Btn_End02") as Button);
		this.m_btLeaderCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLeaderCancel));
		this.m_btLeaderClose = (base.GetControl("Btn_Close03") as Button);
		this.m_btLeaderClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLeaderClose));
		this.m_btEnemyClose = (base.GetControl("Btn_Close02") as Button);
		this.m_btEnemyClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEnemyClose));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.InitControl();
	}

	public void InitControl()
	{
		this.m_lbUserName.SetText(string.Empty);
		this.m_lbGuildWarInfo.SetText(string.Empty);
	}

	public void ClickMemberStart(IUIObject obj)
	{
		byte b;
		if (this.m_iCurSelectIndex < 0)
		{
			b = this.GetEmptyRaidBattlePos();
		}
		else
		{
			b = (byte)this.m_iCurSelectIndex;
		}
		ApplyInfo applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos(b);
		if (applyInfoFromRaidBattlePos != null)
		{
			b = this.GetEmptyRaidBattlePos();
		}
		if (b < 0 || b >= 9)
		{
			return;
		}
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP;
		SoldierBatch.GuildWarRaidUnique = this.m_iRaidUnique;
		SoldierBatch.GuildWarRaidBattlePos = b;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
	}

	public void ClickMemberCancel(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_CANCEL_REQ(this.m_iRaidUnique);
		this.SetControlEnable(false);
	}

	public void ClickMemberClose(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(0);
		this.SetControlEnable(false);
	}

	public void ClickLeaderRaidBattlePosChange(IUIObject obj)
	{
		bool flag = false;
		if (this.m_eMode == NewGuildWarDetailInfoDlg.eMODE.eMODE_NORMAL)
		{
			this.m_eMode = NewGuildWarDetailInfoDlg.eMODE.eMODE_CHANGE;
			this.m_btLeaderChange.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2514"));
		}
		else
		{
			this.m_eMode = NewGuildWarDetailInfoDlg.eMODE.eMODE_NORMAL;
			this.m_btLeaderChange.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2513"));
			flag = this.ShowChangeMessageBox();
		}
		if (!flag)
		{
			this.ChangeMode(this.m_eMode);
		}
	}

	public void ClickLeaderCancel(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_CANCEL_REQ(this.m_iRaidUnique);
	}

	public void ClickLeaderClose(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(0);
		this.SetControlEnable(false);
	}

	public void ClickEnemyClose(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(0);
		this.SetControlEnable(false);
	}

	public void RefeshDetailInfo(GS_GUILDWAR_APPLY_MILITARY_INFO_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_ApplyInfo.Clear();
		this.m_i64GuildID = ACK.i64GuildID;
		this.m_iRaidUnique = ACK.ui8RaidUnique;
		for (int i = 0; i < (int)ACK.i16UserInfoCount; i++)
		{
			GUILDWAR_APPLY_MILITARY_USER_INFO packet = kDeserializePacket.GetPacket<GUILDWAR_APPLY_MILITARY_USER_INFO>();
			ApplyInfo item = new ApplyInfo(packet);
			this.m_ApplyInfo.Add(item);
		}
		for (int i = 0; i < (int)ACK.i16DetailInfoCount; i++)
		{
			GUILDWAR_APPLY_MILITARY_DETAIL_INFO packet2 = kDeserializePacket.GetPacket<GUILDWAR_APPLY_MILITARY_DETAIL_INFO>();
			this.AddDetailInfo(packet2);
		}
		NewGuildWarDetailInfoDlg.eSTATE eSTATE;
		if (ACK.i64GuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			ApplyUserInfo applyUserInfo = this.GetApplyUserInfo(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
			if (applyUserInfo != null && applyUserInfo.GetLeader() == 1)
			{
				eSTATE = NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_LEADER;
			}
			else
			{
				eSTATE = NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_MEMBER;
			}
		}
		else
		{
			eSTATE = NewGuildWarDetailInfoDlg.eSTATE.eSTATE_ENEMY_INFO;
		}
		this.SetLayerInfo(eSTATE);
		this.SetDetailInfo(eSTATE);
	}

	public void AddDetailInfo(GUILDWAR_APPLY_MILITARY_DETAIL_INFO DetailInfo)
	{
		for (int i = 0; i < this.m_ApplyInfo.Count; i++)
		{
			if (this.m_ApplyInfo[i].GetPersonID() == DetailInfo.i64PersonID)
			{
				this.m_ApplyInfo[i].AddDetailInfo(DetailInfo);
			}
		}
	}

	public ApplyUserInfo GetApplyUserInfo(long i64PersonID)
	{
		for (int i = 0; i < this.m_ApplyInfo.Count; i++)
		{
			if (this.m_ApplyInfo[i].GetPersonID() == i64PersonID)
			{
				return this.m_ApplyInfo[i].GetUserInfo();
			}
		}
		return null;
	}

	public ApplyUserInfo GetApplyUserInfoLeader()
	{
		for (int i = 0; i < this.m_ApplyInfo.Count; i++)
		{
			if (this.m_ApplyInfo[i].GetLeader() == 1)
			{
				return this.m_ApplyInfo[i].GetUserInfo();
			}
		}
		return null;
	}

	public ApplyUserInfo GetUserInfoFromRaidBattlePos(byte iRaidBattlePos)
	{
		for (int i = 0; i < this.m_ApplyInfo.Count; i++)
		{
			if (this.m_ApplyInfo[i].GetRaidBattlePos() == iRaidBattlePos)
			{
				return this.m_ApplyInfo[i].GetUserInfo();
			}
		}
		return null;
	}

	public ApplyInfo GetApplyInfo(int iIndex)
	{
		if (iIndex < 0 || iIndex >= this.m_ApplyInfo.Count)
		{
			return null;
		}
		return this.m_ApplyInfo[iIndex];
	}

	public ApplyInfo GetApplyInfoFromRaidBattlePos(byte iRaidBattlePos)
	{
		foreach (ApplyInfo current in this.m_ApplyInfo)
		{
			if (current.GetRaidBattlePos() == iRaidBattlePos)
			{
				return current;
			}
		}
		return null;
	}

	public ApplyInfo GetApplyInfoFromMilitaryID(long i64MilitaryID)
	{
		foreach (ApplyInfo current in this.m_ApplyInfo)
		{
			if (current.GetMilitaryID() == i64MilitaryID)
			{
				return current;
			}
		}
		return null;
	}

	public void SetLayerInfo(NewGuildWarDetailInfoDlg.eSTATE eState)
	{
		this.m_eState = eState;
		this.m_strText = string.Format("UI/Mine/bg_mine_attack_detailInfo02", new object[0]);
		switch (this.m_eState)
		{
		case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_LEADER:
			base.ShowLayer(2, 6);
			break;
		case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_MEMBER:
		{
			base.ShowLayer(2, 5);
			byte iMilitaryUnique = this.m_iRaidUnique + 60;
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsGuildWarApply(iMilitaryUnique))
			{
				this.m_btMemberStart.Visible = false;
			}
			else
			{
				this.m_btMemberCancel.Visible = false;
			}
			break;
		}
		case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_ENEMY_INFO:
			base.ShowLayer(1, 9);
			this.m_strText = string.Format("UI/Mine/bg_mine_detailInfo02", new object[0]);
			break;
		}
		this.m_dtMainBG.SetTextureFromBundle(this.m_strText);
	}

	public void SetDetailInfo(NewGuildWarDetailInfoDlg.eSTATE eState)
	{
		this.InitControl();
		if (this.m_i64GuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			this.m_lbGuildName.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildName());
		}
		else
		{
			this.m_lbGuildName.SetText(NrTSingleton<GuildWarManager>.Instance.GuildWarGuildName);
		}
		if (0L < this.m_i64GuildID)
		{
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(this.m_i64GuildID);
			WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), null);
		}
		int num = 0;
		ApplyUserInfo applyUserInfo = this.GetApplyUserInfo(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
		if (applyUserInfo == null)
		{
			applyUserInfo = this.GetApplyUserInfoLeader();
		}
		if (applyUserInfo != null)
		{
			num = (int)applyUserInfo.GetRaidBattlePos();
		}
		this.m_iCurSelectIndex = num;
		this.m_iOldSelectIndex = num;
		this.LoadEffect();
		this.SetDetailInfoSub(num);
		if (this.m_i64GuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2595"),
				"targetname",
				NrTSingleton<NewGuildManager>.Instance.GetGuildName(),
				"count",
				(int)(this.m_iRaidUnique + 1)
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2595"),
				"targetname",
				NrTSingleton<GuildWarManager>.Instance.GuildWarGuildName,
				"count",
				(int)(this.m_iRaidUnique + 1)
			});
		}
		this.m_lbGuildWarInfo.SetText(this.m_strText);
	}

	public void SetDetailInfoSub(int iSelectIndex)
	{
		for (int i = 0; i < 9; i++)
		{
			ApplyInfo applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos((byte)i);
			switch (this.m_eState)
			{
			case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_LEADER:
				this.SetDetailInfoSub_RaidLeader(i, applyInfoFromRaidBattlePos, iSelectIndex);
				break;
			case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_MEMBER:
				this.SetDetailInfoSub_RaidLeader(i, applyInfoFromRaidBattlePos, iSelectIndex);
				break;
			case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_ENEMY_INFO:
				this.SetDetailInfoSub_EnemyInfo(applyInfoFromRaidBattlePos);
				break;
			}
		}
	}

	public void SelectAllyUserInfo(int iSelectIndex)
	{
		if (iSelectIndex < 0 || iSelectIndex >= 9)
		{
			return;
		}
		ApplyInfo applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos((byte)iSelectIndex);
		if (applyInfoFromRaidBattlePos == null)
		{
			return;
		}
		ApplyInfo applyInfoFromRaidBattlePos2 = this.GetApplyInfoFromRaidBattlePos((byte)this.m_iOldSelectIndex);
		this.m_iCurSelectIndex = iSelectIndex;
		this.m_itAllySelectSolLarge[this.m_iOldSelectIndex].Visible = false;
		this.m_btAllySelectSolLarge[this.m_iOldSelectIndex].Visible = false;
		this.m_dtAllyLeaderLarge[this.m_iOldSelectIndex].Visible = false;
		this.m_itAllySelectSolSmall[this.m_iOldSelectIndex].Visible = true;
		this.m_btAllySelectSolSmall[this.m_iOldSelectIndex].Visible = true;
		if (applyInfoFromRaidBattlePos2 != null && applyInfoFromRaidBattlePos2.GetLeader() == 1)
		{
			this.m_dtAllyLeaderSmall[this.m_iOldSelectIndex].Visible = true;
		}
		this.m_itAllySelectSolLarge[this.m_iCurSelectIndex].Visible = true;
		this.m_btAllySelectSolLarge[this.m_iCurSelectIndex].Visible = true;
		if (applyInfoFromRaidBattlePos != null && applyInfoFromRaidBattlePos.GetLeader() == 1)
		{
			this.m_dtAllyLeaderLarge[this.m_iCurSelectIndex].Visible = true;
		}
		this.m_itAllySelectSolSmall[this.m_iCurSelectIndex].Visible = false;
		this.m_btAllySelectSolSmall[this.m_iCurSelectIndex].Visible = false;
		this.m_dtAllyLeaderSmall[this.m_iCurSelectIndex].Visible = false;
		this.m_iOldSelectIndex = iSelectIndex;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
			"count",
			applyInfoFromRaidBattlePos.GetCharLevel(),
			"targetname",
			applyInfoFromRaidBattlePos.GetCharName()
		});
		this.m_lbUserName.SetText(this.m_strText);
		for (int i = 0; i < 9; i++)
		{
			GUILDWAR_APPLY_MILITARY_DETAIL_INFO detailInfoBattlePos = applyInfoFromRaidBattlePos.GetDetailInfoBattlePos(i);
			if (detailInfoBattlePos == null)
			{
				this.m_itSolInfo[i].ClearData();
				this.m_itSolInfo[i].Visible = false;
			}
			else if (detailInfoBattlePos.i16BattlePos >= 0 && detailInfoBattlePos.i16BattlePos < 9)
			{
				NkListSolInfo nkListSolInfo = new NkListSolInfo();
				nkListSolInfo.ShowCombat = true;
				nkListSolInfo.FightPower = detailInfoBattlePos.i64FightPower;
				nkListSolInfo.SolLevel = detailInfoBattlePos.i16level;
				nkListSolInfo.SolCharKind = detailInfoBattlePos.i32CharKind;
				nkListSolInfo.SolGrade = (int)detailInfoBattlePos.ui8grade;
				nkListSolInfo.ShowLevel = false;
				this.m_itSolInfo[i].Visible = true;
				this.m_itSolInfo[i].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
			}
		}
	}

	public void SetDetailInfoSub_RaidLeader(int iIndex, ApplyInfo Info, int iSelectIndex)
	{
		this.m_dtAllySelectImage[iIndex].Visible = false;
		this.m_itAllySelectSolLarge[iIndex].ClearData();
		this.m_itAllySelectSolLarge[iIndex].Visible = false;
		this.m_btAllySelectSolLarge[iIndex].Visible = false;
		this.m_dtAllyLeaderLarge[iIndex].Visible = false;
		this.m_dtAllyLeaderSmall[iIndex].Visible = false;
		this.m_itAllySelectSolSmall[iIndex].ClearData();
		if (Info == null)
		{
			return;
		}
		if (Info.GetLeader() == 1)
		{
			if (iIndex == iSelectIndex)
			{
				this.m_dtAllyLeaderLarge[iIndex].Visible = true;
			}
			else
			{
				this.m_dtAllyLeaderSmall[iIndex].Visible = true;
			}
		}
		GUILDWAR_APPLY_MILITARY_DETAIL_INFO gUILDWAR_APPLY_MILITARY_DETAIL_INFO = Info.GetDetailInfo(0);
		if (gUILDWAR_APPLY_MILITARY_DETAIL_INFO != null)
		{
			this.SetSolImage(gUILDWAR_APPLY_MILITARY_DETAIL_INFO, this.m_itAllySelectSolLarge[iIndex]);
			this.SetSolImage(gUILDWAR_APPLY_MILITARY_DETAIL_INFO, this.m_itAllySelectSolSmall[iIndex]);
			if (iIndex == iSelectIndex)
			{
				this.m_itAllySelectSolSmall[iIndex].Visible = false;
				this.m_btAllySelectSolSmall[iIndex].Visible = false;
				this.m_itAllySelectSolLarge[iIndex].Visible = true;
				this.m_btAllySelectSolLarge[iIndex].Visible = true;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
					"count",
					Info.GetCharLevel(),
					"targetname",
					Info.GetCharName()
				});
				this.m_lbUserName.SetText(this.m_strText);
			}
			else
			{
				this.m_itAllySelectSolSmall[iIndex].Visible = true;
				this.m_btAllySelectSolSmall[iIndex].Visible = true;
			}
		}
		if (iIndex == iSelectIndex)
		{
			for (int i = 0; i < 9; i++)
			{
				gUILDWAR_APPLY_MILITARY_DETAIL_INFO = Info.GetDetailInfoBattlePos(i);
				if (gUILDWAR_APPLY_MILITARY_DETAIL_INFO == null)
				{
					this.m_itSolInfo[i].Visible = false;
				}
				else if (gUILDWAR_APPLY_MILITARY_DETAIL_INFO.i16BattlePos >= 0 && gUILDWAR_APPLY_MILITARY_DETAIL_INFO.i16BattlePos < 9)
				{
					NkListSolInfo nkListSolInfo = new NkListSolInfo();
					nkListSolInfo.ShowCombat = true;
					nkListSolInfo.FightPower = gUILDWAR_APPLY_MILITARY_DETAIL_INFO.i64FightPower;
					nkListSolInfo.SolLevel = gUILDWAR_APPLY_MILITARY_DETAIL_INFO.i16level;
					nkListSolInfo.SolCharKind = gUILDWAR_APPLY_MILITARY_DETAIL_INFO.i32CharKind;
					nkListSolInfo.SolGrade = (int)gUILDWAR_APPLY_MILITARY_DETAIL_INFO.ui8grade;
					nkListSolInfo.ShowLevel = false;
					this.m_itSolInfo[i].Visible = true;
					this.m_itSolInfo[i].SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
				}
			}
		}
	}

	public void SetSolImage(GUILDWAR_APPLY_MILITARY_DETAIL_INFO DetailInfo, ItemTexture it)
	{
		if (DetailInfo == null || it == null)
		{
			return;
		}
		it.SetSolImageTexure(eCharImageType.SMALL, DetailInfo.i32CharKind, (int)DetailInfo.ui8grade);
	}

	public void SetDetailInfoSub_RaidMember(ApplyInfo Info)
	{
		if (Info == null)
		{
			return;
		}
	}

	public void SetDetailInfoSub_EnemyInfo(ApplyInfo Info)
	{
		if (Info == null)
		{
			return;
		}
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		if (this.m_dtGuildMark == null)
		{
			return;
		}
		if (txtr == null)
		{
			this.m_dtGuildMark.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			this.m_dtGuildMark.SetTexture(txtr);
		}
	}

	public byte GetEmptyRaidBattlePos()
	{
		byte[] array = new byte[9];
		for (int i = 0; i < 9; i++)
		{
			array[i] = (byte)i;
		}
		foreach (ApplyInfo current in this.m_ApplyInfo)
		{
			for (int i = 0; i < 9; i++)
			{
				if (array[i] == current.GetRaidBattlePos())
				{
					array[i] = 100;
				}
			}
		}
		for (int i = 0; i < 9; i++)
		{
			if (array[i] >= 0 && array[i] < 9)
			{
				return array[i];
			}
		}
		return 100;
	}

	public void ClickSelectEnemyInfoLarge(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		this.m_iCurSelectIndex = button.TabIndex;
	}

	public void ClickSelectEnemyInfoSmall(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		this.m_iCurSelectIndex = button.TabIndex;
	}

	public void ClickSelectAllyInfoLarge(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		this.m_iCurSelectIndex = button.TabIndex;
	}

	public void ClickSelectAllyInfoSmall(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		this.m_iCurSelectIndex = button.TabIndex;
		this.SelectAllyUserInfo(this.m_iCurSelectIndex);
	}

	public void ChangeMode(NewGuildWarDetailInfoDlg.eMODE eMode)
	{
		this.m_iChangeCurSelectIndex = -1;
		this.m_iChangeOldSelectIndex = -1;
		for (int i = 0; i < 9; i++)
		{
			this.m_i64ChangeMilitaryID[i] = 0L;
		}
		if (eMode == NewGuildWarDetailInfoDlg.eMODE.eMODE_NORMAL)
		{
			this.m_itAllySelectSolLarge[this.m_iCurSelectIndex].Visible = true;
			this.m_btAllySelectSolLarge[this.m_iCurSelectIndex].Visible = true;
			this.m_dtAllyLeaderLarge[this.m_iCurSelectIndex].Visible = true;
			this.m_itAllySelectSolSmall[this.m_iCurSelectIndex].Visible = false;
			this.m_btAllySelectSolSmall[this.m_iCurSelectIndex].Visible = false;
			this.m_dtAllyLeaderSmall[this.m_iCurSelectIndex].Visible = false;
			this.m_btLeaderCancel.Visible = true;
			for (int i = 0; i < 9; i++)
			{
				this.m_btAllySelectSolSmall[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectAllyInfoSmall));
				this.m_btAllySelectSolSmall[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickChangeRaidBattlePos));
			}
			this.SetDetailInfoSub(this.m_iCurSelectIndex);
		}
		else
		{
			this.m_dtAllySelectImage[this.m_iCurSelectIndex].Visible = false;
			this.m_itAllySelectSolLarge[this.m_iCurSelectIndex].Visible = false;
			this.m_btAllySelectSolLarge[this.m_iCurSelectIndex].Visible = false;
			this.m_dtAllyLeaderLarge[this.m_iCurSelectIndex].Visible = false;
			this.m_itAllySelectSolSmall[this.m_iCurSelectIndex].Visible = true;
			this.m_btAllySelectSolSmall[this.m_iCurSelectIndex].Visible = true;
			ApplyInfo applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos((byte)this.m_iCurSelectIndex);
			if (applyInfoFromRaidBattlePos != null && applyInfoFromRaidBattlePos.GetLeader() == 1)
			{
				this.m_dtAllyLeaderSmall[this.m_iCurSelectIndex].Visible = true;
			}
			this.m_btLeaderCancel.Visible = false;
			for (int i = 0; i < 9; i++)
			{
				this.m_btAllySelectSolSmall[i].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectAllyInfoSmall));
				this.m_btAllySelectSolSmall[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickChangeRaidBattlePos));
				applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos((byte)i);
				if (applyInfoFromRaidBattlePos != null)
				{
					this.m_i64ChangeMilitaryID[i] = applyInfoFromRaidBattlePos.GetMilitaryID();
				}
			}
		}
	}

	public void ClickChangeRaidBattlePos(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		if (this.m_iChangeCurSelectIndex < 0)
		{
			this.m_iChangeCurSelectIndex = button.TabIndex;
			this.m_dtAllySelectImage[this.m_iChangeCurSelectIndex].Visible = true;
		}
		else
		{
			if (this.m_iChangeCurSelectIndex == button.TabIndex)
			{
				return;
			}
			this.m_iChangeOldSelectIndex = button.TabIndex;
		}
		if (this.m_iChangeCurSelectIndex >= 0 && this.m_iChangeOldSelectIndex >= 0)
		{
			this.ChangeRaidBattlePos((byte)this.m_iChangeCurSelectIndex, (byte)this.m_iChangeOldSelectIndex);
			this.m_dtAllySelectImage[this.m_iChangeCurSelectIndex].Visible = false;
			this.m_iChangeCurSelectIndex = -1;
			this.m_iChangeOldSelectIndex = -1;
		}
	}

	public void ChangeRaidBattlePos(byte iChangeCurSelectIndex, byte iChangeOldSelectIndex)
	{
		ApplyInfo applyInfoFromMilitaryID = this.GetApplyInfoFromMilitaryID(this.m_i64ChangeMilitaryID[this.m_iChangeCurSelectIndex]);
		GUILDWAR_APPLY_MILITARY_DETAIL_INFO gUILDWAR_APPLY_MILITARY_DETAIL_INFO = null;
		ApplyInfo applyInfoFromMilitaryID2 = this.GetApplyInfoFromMilitaryID(this.m_i64ChangeMilitaryID[(int)iChangeOldSelectIndex]);
		GUILDWAR_APPLY_MILITARY_DETAIL_INFO gUILDWAR_APPLY_MILITARY_DETAIL_INFO2 = null;
		long num = this.m_i64ChangeMilitaryID[this.m_iChangeCurSelectIndex];
		this.m_i64ChangeMilitaryID[this.m_iChangeCurSelectIndex] = this.m_i64ChangeMilitaryID[(int)iChangeOldSelectIndex];
		this.m_i64ChangeMilitaryID[(int)iChangeOldSelectIndex] = num;
		if (applyInfoFromMilitaryID != null)
		{
			gUILDWAR_APPLY_MILITARY_DETAIL_INFO = applyInfoFromMilitaryID.GetDetailInfo(0);
			if (applyInfoFromMilitaryID.GetLeader() == 1)
			{
				this.m_dtAllyLeaderSmall[(int)iChangeOldSelectIndex].Visible = true;
			}
			else
			{
				this.m_dtAllyLeaderSmall[(int)iChangeOldSelectIndex].Visible = false;
			}
		}
		if (gUILDWAR_APPLY_MILITARY_DETAIL_INFO != null)
		{
			this.SetSolImage(gUILDWAR_APPLY_MILITARY_DETAIL_INFO, this.m_itAllySelectSolSmall[(int)iChangeOldSelectIndex]);
		}
		else
		{
			this.m_itAllySelectSolSmall[(int)iChangeOldSelectIndex].ClearData();
			this.m_dtAllyLeaderSmall[(int)iChangeOldSelectIndex].Visible = false;
		}
		if (applyInfoFromMilitaryID2 != null)
		{
			gUILDWAR_APPLY_MILITARY_DETAIL_INFO2 = applyInfoFromMilitaryID2.GetDetailInfo(0);
			if (applyInfoFromMilitaryID2.GetLeader() == 1)
			{
				this.m_dtAllyLeaderSmall[this.m_iChangeCurSelectIndex].Visible = true;
			}
			else
			{
				this.m_dtAllyLeaderSmall[this.m_iChangeCurSelectIndex].Visible = false;
			}
		}
		if (gUILDWAR_APPLY_MILITARY_DETAIL_INFO2 != null)
		{
			this.SetSolImage(gUILDWAR_APPLY_MILITARY_DETAIL_INFO2, this.m_itAllySelectSolSmall[this.m_iChangeCurSelectIndex]);
		}
		else
		{
			this.m_itAllySelectSolSmall[this.m_iChangeCurSelectIndex].ClearData();
			this.m_dtAllyLeaderSmall[this.m_iChangeCurSelectIndex].Visible = false;
		}
	}

	public void SetControlEnable(bool bEnable)
	{
		if (!bEnable)
		{
			this.m_fDelayTime = Time.time + 2f;
		}
		this.m_btMemberStart.controlIsEnabled = bEnable;
		this.m_btMemberCancel.controlIsEnabled = bEnable;
		this.m_btLeaderChange.controlIsEnabled = bEnable;
		this.m_btLeaderCancel.controlIsEnabled = bEnable;
		for (int i = 0; i < 9; i++)
		{
			this.m_btEnemySelectSolLarge[i].controlIsEnabled = bEnable;
			this.m_btEnemySelectSolSmall[i].controlIsEnabled = bEnable;
			this.m_btAllySelectSolLarge[i].controlIsEnabled = bEnable;
			this.m_btAllySelectSolSmall[i].controlIsEnabled = bEnable;
		}
	}

	public override void Update()
	{
		if (this.m_fDelayTime > 0f && this.m_fDelayTime < Time.time)
		{
			this.SetControlEnable(true);
			this.m_fDelayTime = 0f;
		}
	}

	public void LoadEffect()
	{
		switch (this.m_eState)
		{
		case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_LEADER:
		case NewGuildWarDetailInfoDlg.eSTATE.eSTATE_RAID_MEMBER:
			this.m_strText = string.Format("{0}", "UI/Mine/fx_ui_minecrown" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			for (int i = 0; i < 9; i++)
			{
				NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(this.m_strText, this.m_dtAllyLeaderLarge[i], this.m_dtAllyLeaderLarge[i].GetSize());
				this.m_dtAllyLeaderLarge[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureLoadEffectDelegate));
				NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(this.m_strText, this.m_dtAllyLeaderSmall[i], this.m_dtAllyLeaderSmall[i].GetSize());
				this.m_dtAllyLeaderSmall[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureLoadEffectDelegate));
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_EMERGENCY", this.m_dtAllySelectImage[i], this.m_dtAllySelectImage[i].GetSize());
			}
			break;
		}
	}

	public void DrawTextureLoadEffectDelegate(IUIObject control, GameObject obj)
	{
		for (int i = 0; i < 9; i++)
		{
			if (this.m_dtAllyLeaderLarge[i] == control)
			{
				obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
				obj.transform.localPosition = new Vector3(16.3f, 6f, -0.2f);
				break;
			}
			if (this.m_dtAllyLeaderSmall[i] == control)
			{
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.transform.localPosition = new Vector3(18.3f, 4f, -0.2f);
				break;
			}
		}
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public byte GetLeaderRaidBattlePos()
	{
		for (int i = 0; i < this.m_ApplyInfo.Count; i++)
		{
			if (this.m_ApplyInfo[i].GetLeader() == 1)
			{
				return this.m_ApplyInfo[i].GetRaidBattlePos();
			}
		}
		return 0;
	}

	public bool ShowChangeMessageBox()
	{
		if (this.IsChangeRaidBattlePos())
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2513");
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("165");
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.MsgOKChangeRaidBattlePos), null, new NoDelegate(this.MsgCancelChangeRaidBattlePos), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
			return true;
		}
		return false;
	}

	public void MsgOKChangeRaidBattlePos(object a_oObject)
	{
		GS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ gS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ = new GS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ();
		gS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ.ui8RaidUnique = this.m_iRaidUnique;
		for (int i = 0; i < 9; i++)
		{
			gS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ.i64MilitaryID[i] = this.m_i64ChangeMilitaryID[i];
			gS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ.ui8RaidBattlePos[i] = (byte)i;
		}
		SendPacket.GetInstance().SendObject(2215, gS_GUILDWAR_RAIDBATTLEPOS_CHANGE_REQ);
		this.SetControlEnable(false);
	}

	public void MsgCancelChangeRaidBattlePos(object a_oObject)
	{
		this.ChangeMode(this.m_eMode);
	}

	public bool IsChangeRaidBattlePos()
	{
		for (int i = 0; i < 9; i++)
		{
			ApplyInfo applyInfoFromRaidBattlePos = this.GetApplyInfoFromRaidBattlePos((byte)i);
			if (applyInfoFromRaidBattlePos == null && this.m_i64ChangeMilitaryID[i] > 0L)
			{
				return true;
			}
			if (applyInfoFromRaidBattlePos != null && applyInfoFromRaidBattlePos.GetMilitaryID() != this.m_i64ChangeMilitaryID[i])
			{
				return true;
			}
		}
		return false;
	}
}
