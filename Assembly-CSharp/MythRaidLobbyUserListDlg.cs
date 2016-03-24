using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MythRaidLobbyUserListDlg : Form
{
	private const int MAX_USER_COUNT = 4;

	private const int LEADER_INDEX = 0;

	public DropDownList[] m_ddlSlotType = new DropDownList[4];

	public Label[] m_laSlotState = new Label[4];

	public Button m_btStart;

	public Button m_btReady;

	private GameObject m_StartEffect;

	public MYTHRAID_USER_CONTROL_INFO[] user_info = new MYTHRAID_USER_CONTROL_INFO[4];

	private int m_nSelectIndex;

	private float m_fRoateVal = 5f;

	private int nInjurySoldierCount;

	public DrawTexture[] dt_UserBG = new DrawTexture[4];

	public Label lb_Solnum0;

	public DropDownList[] m_ddlSlotType_Unvisible = new DropDownList[4];

	public Label[] m_laSlotState_Unvisible = new Label[4];

	public DrawTexture dt_LeaderReady0;

	private int nInjuryBattleSoldierCount;

	private int nInjuryReadySoldierCount;

	public int InjurySoldierCount
	{
		get
		{
			return this.nInjurySoldierCount;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_char_solnum", G_ID.MYTHRAID_USERLIST_DLG, false, true);
		float x = GUICamera.width - base.GetSizeX();
		float y = 0f;
		base.SetLocation(x, y);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(1005f);
	}

	public override void SetComponent()
	{
		this.m_btStart = (base.GetControl("Button_Start") as Button);
		this.m_btStart.Click = new EZValueChangedDelegate(this.OnClickStartMythRaid);
		this.m_btStart.EffectAni = false;
		this.m_btReady = (base.GetControl("Button_Ready") as Button);
		this.m_btReady.Click = new EZValueChangedDelegate(this.OnClickReady);
		this.m_btReady.EffectAni = false;
		for (int i = 0; i < 4; i++)
		{
			this.user_info[i] = new MYTHRAID_USER_CONTROL_INFO();
			this.user_info[i].m_laUserName = (base.GetControl("Label_MythRaid_User" + i.ToString()) as Label);
			this.user_info[i].m_laUserSolNum = (base.GetControl("Label_MythRaid_Solnum" + i.ToString()) as Label);
			this.user_info[i].btn_Guardian_Select = (base.GetControl("BTN_Guardian_Select" + i.ToString()) as Button);
			this.user_info[i].btn_Guardian_Reselect = (base.GetControl("BTN_Guardian_Reselect" + i.ToString()) as Button);
			this.user_info[i].dt_GuardianImg = (base.GetControl("DT_GuardianImg" + i.ToString()) as DrawTexture);
			this.user_info[i].dt_GuardianImg_Frame = (base.GetControl("DT_GuardianImg_Frame" + i.ToString()) as DrawTexture);
			if (i != 0)
			{
				this.user_info[i].m_btKickUser = (base.GetControl("Button_MythRaid_Kick" + i.ToString()) as Button);
				this.user_info[i].m_dtReady = (base.GetControl("DT_MythRaid_Ready" + i.ToString()) as DrawTexture);
				this.user_info[i].m_dtReady.SetLocation(this.user_info[i].m_dtReady.GetLocationX(), this.user_info[i].m_dtReady.GetLocationY(), this.user_info[i].m_dtReady.GetLocation().z - 0.1f);
				this.user_info[i].m_laReady = (base.GetControl("LB_MythRaid_Ready" + i.ToString()) as Label);
				this.user_info[i].m_laReady.SetLocation(this.user_info[i].m_laReady.GetLocationX(), this.user_info[i].m_laReady.GetLocationY(), this.user_info[i].m_laReady.GetLocation().z - 0.1f);
				this.user_info[i].m_laSlotState = (base.GetControl("LB_MythRaid_Status" + i.ToString()) as Label);
				this.user_info[i].m_dLoadingImg = (base.GetControl("DrawTexture_MythRaid_Loading" + i.ToString()) as DrawTexture);
				this.user_info[i].m_dLoadingImg = (base.GetControl("DrawTexture_MythRaid_Loading" + i.ToString()) as DrawTexture);
				this.user_info[i].InitLoadingState();
				this.user_info[i].Show(false);
				this.m_ddlSlotType[i] = (base.GetControl("DropDownList_MythRaid_" + i.ToString()) as DropDownList);
				if (this.m_ddlSlotType[i] != null)
				{
					this.m_ddlSlotType[i].SetViewArea(0);
					this.m_ddlSlotType[i].Clear();
					string text = string.Empty;
					for (int j = 0; j <= 2; j++)
					{
						if (j == 1)
						{
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("899");
						}
						else if (j == 2)
						{
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("907");
						}
						else if (j == 0)
						{
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("908");
						}
						this.m_ddlSlotType[i].AddItem(text, j);
					}
					this.m_ddlSlotType[i].Data = i;
					this.m_ddlSlotType[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.DropDownChangeSlotType));
					this.m_ddlSlotType[i].RepositionItems();
					this.m_ddlSlotType[i].SetFirstItem();
				}
			}
		}
		this.HideForMythraid();
		this.Hide();
	}

	private void HideForMythraid()
	{
		MYTHRAID_USER_CONTROL_INFO[] array = new MYTHRAID_USER_CONTROL_INFO[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = new MYTHRAID_USER_CONTROL_INFO();
			array[i].m_laUserName = (base.GetControl("Label_User" + i.ToString()) as Label);
			array[i].m_laUserSolNum = (base.GetControl("Label_Solnum" + i.ToString()) as Label);
			array[i].m_laUserName.Visible = false;
			array[i].m_laUserSolNum.Visible = false;
			if (i != 0)
			{
				array[i].m_btKickUser = (base.GetControl("Button_Kick" + i.ToString()) as Button);
				array[i].m_dtReady = (base.GetControl("DT_Ready" + i.ToString()) as DrawTexture);
				array[i].m_dtReady.SetLocation(array[i].m_dtReady.GetLocationX(), array[i].m_dtReady.GetLocationY(), array[i].m_dtReady.GetLocation().z - 0.1f);
				array[i].m_laReady = (base.GetControl("LB_Ready" + i.ToString()) as Label);
				array[i].m_laReady.SetLocation(array[i].m_laReady.GetLocationX(), array[i].m_laReady.GetLocationY(), array[i].m_laReady.GetLocation().z - 0.1f);
				array[i].m_laSlotState = (base.GetControl("LB_Status_" + i.ToString()) as Label);
				array[i].m_dLoadingImg = (base.GetControl("DrawTexture_Loading" + i.ToString()) as DrawTexture);
				array[i].InitLoadingState();
				array[i].Show(false);
			}
		}
		this.dt_UserBG[0] = (base.GetControl("DT_UserBG01") as DrawTexture);
		this.dt_UserBG[1] = (base.GetControl("DT_UserBG02") as DrawTexture);
		this.dt_UserBG[2] = (base.GetControl("DT_UserBG03") as DrawTexture);
		this.dt_UserBG[3] = (base.GetControl("DT_UserBG04") as DrawTexture);
		this.dt_UserBG[0].Visible = false;
		this.dt_UserBG[1].Visible = false;
		this.dt_UserBG[2].Visible = false;
		this.dt_UserBG[3].Visible = false;
		this.lb_Solnum0 = (base.GetControl("Label_Solnum0") as Label);
		this.lb_Solnum0.Visible = false;
		this.m_ddlSlotType_Unvisible[0] = (base.GetControl("DropDownList_1") as DropDownList);
		this.m_ddlSlotType_Unvisible[1] = (base.GetControl("DropDownList_2") as DropDownList);
		this.m_ddlSlotType_Unvisible[2] = (base.GetControl("DropDownList_3") as DropDownList);
		this.m_ddlSlotType_Unvisible[0].Visible = false;
		this.m_ddlSlotType_Unvisible[1].Visible = false;
		this.m_ddlSlotType_Unvisible[2].Visible = false;
		this.m_laSlotState_Unvisible[0] = (base.GetControl("LB_Status1") as Label);
		this.m_laSlotState_Unvisible[1] = (base.GetControl("LB_Status2") as Label);
		this.m_laSlotState_Unvisible[2] = (base.GetControl("LB_Status3") as Label);
		this.m_laSlotState_Unvisible[0].Visible = false;
		this.m_laSlotState_Unvisible[1].Visible = false;
		this.m_laSlotState_Unvisible[2].Visible = false;
		this.dt_LeaderReady0 = (base.GetControl("DT_LeaderReady0") as DrawTexture);
		this.dt_LeaderReady0.Visible = false;
	}

	public override void Close()
	{
		base.Close();
	}

	public override void Show()
	{
		this.OnClickGuardAngelSelect(null);
		base.Show();
	}

	public override void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != 0)
			{
				if (SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i) != null)
				{
					if (this.user_info[i].m_dLoadingImg.Visible)
					{
						this.user_info[i].m_dLoadingImg.Rotate(this.m_fRoateVal);
					}
				}
			}
		}
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		if (this.m_StartEffect != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_StartEffect);
		}
		base.OnClose();
	}

	public void SetLayer()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		for (int i = 6; i < 9; i++)
		{
			base.SetShowLayer(i, false);
		}
		MYTHRAID_PERSON mythRaidLeaderInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidLeaderInfo();
		if (mythRaidLeaderInfo != null)
		{
			if (@char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID)
			{
				this.m_btStart.Hide(false);
				this.m_btReady.Hide(true);
			}
			else
			{
				this.m_btStart.Hide(true);
				this.m_btReady.Hide(false);
			}
		}
		for (int j = 5; j < 9; j++)
		{
			if (j - 5 != 0)
			{
				MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(j - 5);
				if (mythRaidPersonInfo.nPartyPersonID > 0L)
				{
					base.SetShowLayer(j, true);
				}
				else
				{
					base.SetShowLayer(j, false);
				}
			}
		}
	}

	public void RefreshPossibleLevel()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		MYTHRAID_PERSON mythRaidLeaderInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidLeaderInfo();
		if (mythRaidLeaderInfo == null || @char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID)
		{
		}
	}

	public void SetWaitingLock(bool bShow)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != 0)
			{
				MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i);
				if (mythRaidPersonInfo != null)
				{
					if (mythRaidPersonInfo.nPartyPersonID > 0L)
					{
						this.user_info[i].SetLoadingState(bShow);
					}
				}
			}
		}
	}

	public void RefreshSolInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			this.user_info[i].Init();
		}
		this.SetLayer();
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		string text = string.Empty;
		string text2 = string.Empty;
		switch (SoldierBatch.MYTHRAID_INFO.m_nDifficulty)
		{
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3243");
			break;
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3244");
			break;
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3245");
			break;
		}
		text2 = text;
		PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
		if (plunderSolNumDlg != null)
		{
			plunderSolNumDlg.SetTitle(text2);
		}
		MYTHRAID_PERSON mythRaidLeaderInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidLeaderInfo();
		if (mythRaidLeaderInfo != null)
		{
			this.user_info[0].Show(true);
			this.user_info[0].PersonID = mythRaidLeaderInfo.nPartyPersonID;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3254");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"targetname",
				mythRaidLeaderInfo.strCharName
			});
			this.user_info[0].m_laUserName.SetText(text2);
			for (int j = 0; j < 4; j++)
			{
				if (j != 0)
				{
					MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(j);
					if (mythRaidPersonInfo != null)
					{
						if (mythRaidPersonInfo.nPartyPersonID > 0L)
						{
							this.m_ddlSlotType[j].SetVisible(false);
							this.user_info[j].Show(true);
							this.user_info[j].PersonID = mythRaidPersonInfo.nPartyPersonID;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								text,
								"count",
								mythRaidPersonInfo.nLevel.ToString(),
								"targetname",
								mythRaidPersonInfo.strCharName
							});
							this.user_info[j].m_laUserName.SetText(text2);
							this.user_info[j].m_dtReady.Visible = mythRaidPersonInfo.bReady;
							if (mythRaidPersonInfo.bReady)
							{
								this.user_info[j].m_laReady.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
							}
							else
							{
								this.user_info[j].m_laReady.Text = string.Empty;
							}
							if (@char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID || @char.GetPersonID() == mythRaidPersonInfo.nPartyPersonID)
							{
								this.user_info[j].m_btKickUser.Data = j;
								this.user_info[j].m_btKickUser.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnClickKickUser));
							}
							else
							{
								this.user_info[j].m_btKickUser.Visible = false;
							}
						}
						else
						{
							this.user_info[j].Show(false);
							if (@char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID)
							{
								this.user_info[j].SetSlotState(false, 0);
								this.m_ddlSlotType[j].SetVisible(true);
							}
							else
							{
								this.user_info[j].SetSlotState(true, (int)mythRaidPersonInfo.nSlotType);
								this.m_ddlSlotType[j].SetVisible(false);
							}
						}
					}
				}
			}
		}
		if (@char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID)
		{
			if (SoldierBatch.MYTHRAID_INFO.IsCanBattle())
			{
				if (this.m_StartEffect == null)
				{
				}
			}
			else if (this.m_StartEffect != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_StartEffect);
			}
		}
		this.RefreshSolCount();
		this.RefreshPossibleLevel();
		this.SetGuardAngelSkillButton();
		if (!base.Visible)
		{
			this.Show();
		}
	}

	public void RefreshSolCount()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		MYTHRAID_PERSON mythRaidLeaderInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidLeaderInfo();
		int num = (int)SoldierBatch.MYTHRAID_INFO.Count;
		if (num <= 0)
		{
			num = 1;
		}
		int num2 = 12 / num;
		for (int i = 0; i < 4; i++)
		{
			if (this.user_info[i].PersonID > 0L)
			{
				this.user_info[i].m_laUserSolNum.SetText(SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(this.user_info[i].PersonID).ToString() + "/" + num2.ToString());
			}
		}
		if (charPersonInfo.GetPersonID() != mythRaidLeaderInfo.nPartyPersonID)
		{
			if (!SoldierBatch.MYTHRAID_INFO.IsReadyBattle(charPersonInfo.GetPersonID()) && SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(charPersonInfo.GetPersonID()) == num2)
			{
				if (this.m_StartEffect == null)
				{
				}
			}
			else if (this.m_StartEffect != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_StartEffect);
			}
		}
	}

	public void UpdateMythRaidReadyState()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(charPersonInfo.GetPersonID()))
		{
			return;
		}
		bool flag = SoldierBatch.MYTHRAID_INFO.IsReadyBattle(charPersonInfo.GetPersonID());
		string text = string.Empty;
		if (flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("685");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
		}
		this.m_btReady.SetText(text);
	}

	public bool SetMythRaidReadyState()
	{
		bool flag = false;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return flag;
		}
		if (SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(charPersonInfo.GetPersonID()))
		{
			return flag;
		}
		if (!SoldierBatch.MYTHRAID_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			flag = true;
		}
		string text = string.Empty;
		if (flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("685");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
		}
		this.m_btReady.SetText(text);
		return flag;
	}

	public void OnClickKickUser(IUIObject obj)
	{
		int num = (int)obj.Data;
		if (num >= 4)
		{
			return;
		}
		this.m_nSelectIndex = num;
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.GetPersonID() == this.user_info[num].PersonID)
		{
			SoldierBatch.MYTHRAID_INFO.Init();
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			this.OnKickOutUser(null);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("72");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("82");
		string empty = string.Empty;
		MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(num);
		if (mythRaidPersonInfo != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox2,
				"targetname",
				mythRaidPersonInfo.strCharName
			});
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnKickOutUser), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
	}

	private void OnKickOutUser(object a_oObject)
	{
		GS_MYTHRAID_LEAVE_REQ gS_MYTHRAID_LEAVE_REQ = new GS_MYTHRAID_LEAVE_REQ();
		if (this.m_nSelectIndex < 0)
		{
			return;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.GetPersonID() == this.user_info[this.m_nSelectIndex].PersonID)
		{
			gS_MYTHRAID_LEAVE_REQ.mode = 1;
		}
		else
		{
			gS_MYTHRAID_LEAVE_REQ.mode = 2;
		}
		gS_MYTHRAID_LEAVE_REQ.nLeavePersonID = this.user_info[this.m_nSelectIndex].PersonID;
		gS_MYTHRAID_LEAVE_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_LEAVE_REQ, gS_MYTHRAID_LEAVE_REQ);
	}

	public void SetUserSlotType(int pos, byte type)
	{
		this.m_ddlSlotType[pos].SetIndex((int)type);
	}

	public void OnClickStartMythRaid(IUIObject obj)
	{
		byte partyCount = SoldierBatch.MYTHRAID_INFO.GetPartyCount();
		byte readyPersonCount = SoldierBatch.MYTHRAID_INFO.GetReadyPersonCount();
		if (!SoldierBatch.SOLDIERBATCH.IsHeroBabelBatch())
		{
			string empty = string.Empty;
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
				"charname",
				@char.GetCharName()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (partyCount != readyPersonCount)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("182"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		if (solBatchNum <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("181"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int solBatchNum2 = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		if (SoldierBatch.MYTHRAID_INFO.Count <= 0)
		{
			SoldierBatch.MYTHRAID_INFO.Count = 1;
		}
		byte count = SoldierBatch.MYTHRAID_INFO.Count;
		int num = (int)(12 / count);
		if (solBatchNum2 < num)
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("74");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromMessageBox2,
				"currentnum",
				solBatchNum2,
				"maxnum",
				num
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.OnStartMythRaid), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL, 2);
			return;
		}
		if (solBatchNum2 > num)
		{
			Debug.LogError("최대 배치 수 이상 배치되어 있음");
			return;
		}
		this.OnStartMythRaid(null);
	}

	private void OnStartMythRaid(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		GS_MYTHRAID_START_REQ gS_MYTHRAID_START_REQ = new GS_MYTHRAID_START_REQ();
		gS_MYTHRAID_START_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
		gS_MYTHRAID_START_REQ.nPersonID = charPersonInfo.GetPersonID();
		gS_MYTHRAID_START_REQ.nCombinationUnique = NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_START_REQ, gS_MYTHRAID_START_REQ);
	}

	public void OnClickReady(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (!SoldierBatch.MYTHRAID_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
		{
			if (!SoldierBatch.SOLDIERBATCH.IsHeroBabelBatch())
			{
				string empty = string.Empty;
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
					"charname",
					@char.GetCharName()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
			if (SoldierBatch.MYTHRAID_INFO.Count <= 0)
			{
				SoldierBatch.MYTHRAID_INFO.Count = 1;
			}
			byte count = SoldierBatch.MYTHRAID_INFO.Count;
			int num = (int)(12 / count);
			if (solBatchNum < num)
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("74");
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromMessageBox2,
					"currentnum",
					solBatchNum,
					"maxnum",
					num
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.OnReadyMythRaid), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL, 2);
				return;
			}
		}
		this.OnReadyMythRaid(null);
	}

	private void OnReadyMythRaid(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		bool bReady = this.SetMythRaidReadyState();
		GS_MYTHRAID_READY_REQ gS_MYTHRAID_READY_REQ = new GS_MYTHRAID_READY_REQ();
		gS_MYTHRAID_READY_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
		gS_MYTHRAID_READY_REQ.nPersonID = charPersonInfo.GetPersonID();
		gS_MYTHRAID_READY_REQ.bReady = bReady;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_READY_REQ, gS_MYTHRAID_READY_REQ);
	}

	public void OnClickCancelMythRaid(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		byte mode;
		if (SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(charPersonInfo.GetPersonID()))
		{
			mode = 0;
		}
		else
		{
			mode = 1;
		}
		GS_MYTHRAID_LEAVE_REQ gS_MYTHRAID_LEAVE_REQ = new GS_MYTHRAID_LEAVE_REQ();
		gS_MYTHRAID_LEAVE_REQ.mode = mode;
		gS_MYTHRAID_LEAVE_REQ.nLeavePersonID = charPersonInfo.GetPersonID();
		gS_MYTHRAID_LEAVE_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_LEAVE_REQ, gS_MYTHRAID_LEAVE_REQ);
		SoldierBatch.MYTHRAID_INFO.Init();
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void DropDownChangeSlotType(IUIObject obj)
	{
		int num = (int)obj.Data;
		ListItem listItem = this.m_ddlSlotType[num].SelectedItem.Data as ListItem;
		if (listItem != null)
		{
			int num2 = (int)listItem.Key;
			GS_MYTHRAID_CHANGE_SLOTTYPE_REQ gS_MYTHRAID_CHANGE_SLOTTYPE_REQ = new GS_MYTHRAID_CHANGE_SLOTTYPE_REQ();
			gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.pos = num;
			gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
			gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.change_type = (byte)num2;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_CHANGE_SLOTTYPE_REQ, gS_MYTHRAID_CHANGE_SLOTTYPE_REQ);
		}
	}

	public void effectDelete(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_StartEffect = obj;
	}

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType, List<NkSoldierInfo> list)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (eAddPosType == eSOL_POSTYPE.SOLPOS_BATTLE)
		{
			if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
			{
				return;
			}
		}
		else if (pkSolinfo.GetSolPosType() != 0 && pkSolinfo.GetSolPosType() != 2 && pkSolinfo.GetSolPosType() != 6)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(pkSolinfo);
		list.Add(nkSoldierInfo);
	}

	public void CheckInjurySoldierList()
	{
		List<NkSoldierInfo> list = new List<NkSoldierInfo>();
		List<NkSoldierInfo> list2 = new List<NkSoldierInfo>();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		list.Clear();
		list2.Clear();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
			this.AddSolList(soldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE, list);
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY, list2);
		}
		int num = 0;
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] != null)
			{
				if (list[j].IsSolStatus(2))
				{
					num++;
				}
			}
		}
		this.nInjuryBattleSoldierCount = num;
		num = 0;
		for (int k = 0; k < list2.Count; k++)
		{
			if (list2[k] != null)
			{
				if (list2[k].IsSolStatus(2))
				{
					if (list2[k].IsInjuryStatus())
					{
						num++;
					}
				}
			}
		}
		this.nInjuryReadySoldierCount = num;
		this.nInjurySoldierCount = this.nInjuryBattleSoldierCount + this.nInjuryReadySoldierCount;
	}

	private int CompareSolPosIndex(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetSolPosIndex().CompareTo(b.GetSolPosIndex());
	}

	public void SetSlotIndex(int index, byte type)
	{
		this.m_ddlSlotType[index].SetIndex((int)type);
	}

	private void SetGuardAngelSkillButton()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		for (int i = 0; i < 4; i++)
		{
			MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i);
			if (mythRaidPersonInfo != null)
			{
				if (@char.GetPersonID() == mythRaidPersonInfo.nPartyPersonID)
				{
					this.user_info[i].btn_Guardian_Select.Click = new EZValueChangedDelegate(this.OnClickGuardAngelSelect);
					this.user_info[i].btn_Guardian_Reselect.Click = new EZValueChangedDelegate(this.OnClickGuardAngelSelect);
				}
				if (mythRaidPersonInfo.selectedGuardianUnique >= 0)
				{
					this.user_info[i].btn_Guardian_Select.Visible = false;
					this.user_info[i].btn_Guardian_Reselect.Visible = true;
					this.user_info[i].dt_GuardianImg.Visible = true;
					this.user_info[i].dt_GuardianImg_Frame.Visible = true;
					MYTHRAID_GUARDIANANGEL_INFO mythRaidGuardAngelInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidGuardAngelInfo(mythRaidPersonInfo.selectedGuardianUnique);
					this.user_info[i].dt_GuardianImg.SetTextureFromUISoldierBundle(eCharImageType.SMALL, mythRaidGuardAngelInfo.IMAGE);
				}
				else if (@char.GetPersonID() == mythRaidPersonInfo.nPartyPersonID)
				{
					this.user_info[i].btn_Guardian_Select.Visible = true;
					this.user_info[i].btn_Guardian_Reselect.Visible = false;
					this.user_info[i].dt_GuardianImg.Visible = false;
					this.user_info[i].dt_GuardianImg_Frame.Visible = false;
				}
				else if (mythRaidPersonInfo.nPartyPersonID > 0L)
				{
					this.user_info[i].btn_Guardian_Select.Visible = false;
					this.user_info[i].btn_Guardian_Reselect.Visible = false;
					this.user_info[i].dt_GuardianImg.Visible = true;
					this.user_info[i].dt_GuardianImg_Frame.Visible = false;
					this.user_info[i].dt_GuardianImg.SetTextureKey("Win_I_NotSelSol");
				}
				else
				{
					this.user_info[i].btn_Guardian_Select.Visible = false;
					this.user_info[i].btn_Guardian_Reselect.Visible = false;
					this.user_info[i].dt_GuardianImg.Visible = false;
					this.user_info[i].dt_GuardianImg_Frame.Visible = false;
				}
			}
		}
	}

	private void OnClickGuardAngelSelect(IUIObject obj)
	{
		GS_MYTHRAID_GUARDIANSELECT_REQ gS_MYTHRAID_GUARDIANSELECT_REQ = new GS_MYTHRAID_GUARDIANSELECT_REQ();
		gS_MYTHRAID_GUARDIANSELECT_REQ.i16SelectedGuardianUnique = -1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_GUARDIANSELECT_REQ, gS_MYTHRAID_GUARDIANSELECT_REQ);
	}

	public void SetGuardianInfo(long _personID, int _guardianUnique)
	{
		for (int i = 0; i < 4; i++)
		{
			MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i);
			if (mythRaidPersonInfo.nPartyPersonID == _personID)
			{
				mythRaidPersonInfo.selectedGuardianUnique = _guardianUnique;
			}
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (_personID == @char.GetPersonID())
		{
			if (_guardianUnique == -1)
			{
				MythRaid_GuardianSelect_DLG mythRaid_GuardianSelect_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_GUARDIANSELECT_DLG) as MythRaid_GuardianSelect_DLG;
				if (mythRaid_GuardianSelect_DLG != null)
				{
					mythRaid_GuardianSelect_DLG.Show();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MYTHRAID_GUARDIANSELECT_DLG);
			}
		}
		else
		{
			MythRaid_GuardianSelect_DLG mythRaid_GuardianSelect_DLG2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_GUARDIANSELECT_DLG) as MythRaid_GuardianSelect_DLG;
			if (mythRaid_GuardianSelect_DLG2 != null)
			{
				mythRaid_GuardianSelect_DLG2.Show();
			}
		}
		this.SetGuardAngelSkillButton();
	}
}
