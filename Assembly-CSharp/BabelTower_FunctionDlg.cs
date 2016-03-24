using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class BabelTower_FunctionDlg : Form
{
	private int m_nInjurySoldirtCount;

	public Button m_btInvite;

	public Button m_btChat;

	public Button m_btInitiative;

	public Box m_bChatNew;

	public Label m_lbChat;

	public Label m_lbInvite;

	public Label m_lbInitiative;

	public TextField tf_chat;

	public Button m_btAuto;

	public DrawTexture m_dtAuto;

	private bool isClickEnter;

	private bool isAutoOn;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_char_function", G_ID.BABELTOWER_FUNCTION_DLG, false, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btInvite = (base.GetControl("BT_InviteAlly") as Button);
		this.m_btInvite.Click = new EZValueChangedDelegate(this.OnClickInviteFriend);
		this.m_btInvite.Hide(true);
		this.m_btInvite.EffectAni = false;
		this.m_btInitiative = (base.GetControl("BT_Initiative") as Button);
		Button expr_61 = this.m_btInitiative;
		expr_61.Click = (EZValueChangedDelegate)Delegate.Combine(expr_61.Click, new EZValueChangedDelegate(this.OnClickSetInitiative));
		this.m_btInitiative.EffectAni = false;
		this.m_btChat = (base.GetControl("BT_Chat") as Button);
		this.m_btChat.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1922");
		Button expr_C4 = this.m_btChat;
		expr_C4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C4.Click, new EZValueChangedDelegate(this.ChatDlg));
		this.m_bChatNew = (base.GetControl("Box_New") as Box);
		this.m_bChatNew.Visible = false;
		this.m_lbChat = (base.GetControl("Label_Chat") as Label);
		this.m_lbInvite = (base.GetControl("Label_InviteAlly") as Label);
		this.m_lbInitiative = (base.GetControl("Label_Initiative") as Label);
		this.tf_chat = (base.GetControl("TF_Chat") as TextField);
		this.tf_chat.SetCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		this.tf_chat.Visible = false;
		this.m_btAuto = (base.GetControl("BT_AUTO") as Button);
		this.m_btAuto.Click = new EZValueChangedDelegate(this.ClickAutoBattleCheck);
		this.m_dtAuto = (base.GetControl("DT_AUTO") as DrawTexture);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			this.tf_chat.Visible = true;
			MYTHRAID_PERSON mythRaidLeaderInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidLeaderInfo();
			if (mythRaidLeaderInfo != null)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char.GetPersonID() == mythRaidLeaderInfo.nPartyPersonID)
				{
					this.m_btInvite.Click = new EZValueChangedDelegate(this.OnClickInviteFriend_MythRaid);
					this.m_btInvite.Hide(false);
				}
				else
				{
					this.m_btInvite.Hide(false);
					this.m_btInvite.SetEnabled(false);
				}
			}
		}
		else
		{
			BABELTOWER_PERSON babelLeaderInfo = SoldierBatch.BABELTOWER_INFO.GetBabelLeaderInfo();
			if (babelLeaderInfo != null)
			{
				NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (char2.GetPersonID() == babelLeaderInfo.nPartyPersonID)
				{
					this.m_btInvite.Hide(false);
				}
				else
				{
					this.m_btInvite.Hide(false);
					this.m_btInvite.SetEnabled(false);
				}
			}
		}
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), -(GUICamera.height - base.GetSizeY()), base.GetLocation().z);
		}
		base.DonotDepthChange(1005f);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			this.m_btChat.Visible = false;
			this.m_lbChat.Visible = false;
			this.m_btInvite.Visible = false;
			this.m_lbInvite.Visible = false;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
		{
			this.isAutoOn = true;
		}
		else
		{
			this.isAutoOn = false;
		}
		this.SetAutoBattleTexture(this.isAutoOn);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), -(GUICamera.height - base.GetSizeY()), base.GetLocation().z);
		}
	}

	public void OnClickInviteFriend(IUIObject obj)
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(i);
			if (babelPersonInfo.nPartyPersonID <= 0L && babelPersonInfo.nPartyPersonID != SoldierBatch.BABELTOWER_INFO.m_nLeaderPersonID)
			{
				flag = true;
				if (babelPersonInfo.nSlotType == 0)
				{
					BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
					if (babelLobbyUserListDlg != null)
					{
						babelLobbyUserListDlg.SetSlotIndex(i, 1);
						GS_BABELTOWER_CHANGE_SLOTTYPE_REQ gS_BABELTOWER_CHANGE_SLOTTYPE_REQ = new GS_BABELTOWER_CHANGE_SLOTTYPE_REQ();
						gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.pos = i;
						gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
						gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.change_type = 1;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_CHANGE_SLOTTYPE_REQ, gS_BABELTOWER_CHANGE_SLOTTYPE_REQ);
					}
				}
				break;
			}
		}
		if (flag)
		{
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("306"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void OnClickInviteFriend_MythRaid(IUIObject obj)
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i);
			if (mythRaidPersonInfo.nPartyPersonID <= 0L && mythRaidPersonInfo.nPartyPersonID != SoldierBatch.MYTHRAID_INFO.m_nLeaderPersonID)
			{
				flag = true;
				if (mythRaidPersonInfo.nSlotType == 0)
				{
					MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
					if (mythRaidLobbyUserListDlg != null)
					{
						mythRaidLobbyUserListDlg.SetSlotIndex(i, 1);
						GS_MYTHRAID_CHANGE_SLOTTYPE_REQ gS_MYTHRAID_CHANGE_SLOTTYPE_REQ = new GS_MYTHRAID_CHANGE_SLOTTYPE_REQ();
						gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.pos = i;
						gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
						gS_MYTHRAID_CHANGE_SLOTTYPE_REQ.change_type = 1;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_CHANGE_SLOTTYPE_REQ, gS_MYTHRAID_CHANGE_SLOTTYPE_REQ);
					}
				}
				break;
			}
		}
		if (flag)
		{
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("306"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void OnClickSetInitiative(IUIObject obj)
	{
		int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
		if (solBatchNum <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("740"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InitiativeSetDlg initiativeSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INITIATIVE_SET_DLG) as InitiativeSetDlg;
		if (initiativeSetDlg != null)
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
			{
				initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS);
			}
			else
			{
				initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER);
			}
		}
	}

	public void ChatDlg(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWER_CHAT))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABELTOWER_CHAT);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EMOTICON_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWER_CHAT);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EMOTICON_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
		}
	}

	private void OnClickSolAllCure(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("230"),
				"count",
				this.m_nInjurySoldirtCount
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnClickSolAllCureOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("229"), empty, eMsgType.MB_OK_CANCEL, 2);
		}
	}

	private void OnClickSolAllCureOK(object _Object)
	{
		NrTSingleton<NkCharManager>.Instance.IsInjuryCureAllChar = true;
	}

	public void CheckInjurySoldier()
	{
		this.m_nInjurySoldirtCount = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolID() > 0L)
			{
				if (nkSoldierInfo.IsInjuryStatus())
				{
					this.m_nInjurySoldirtCount++;
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolID() > 0L)
			{
				if (current.IsInjuryStatus())
				{
					this.m_nInjurySoldirtCount++;
				}
			}
		}
	}

	private void OnInputText(IKeyFocusable obj)
	{
		Batch_Chat_DLG batch_Chat_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATCH_CAHT_DLG) as Batch_Chat_DLG;
		if (batch_Chat_DLG.GetChatType() == CHAT_TYPE.GUILD)
		{
			string chatNameStr = ChatManager.GetChatNameStr(NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1).GetCharName(), NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().ColosseumGrade, true);
			NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.GUILD, this.tf_chat.Text, false, null, short.Parse(NrTSingleton<WhisperManager>.Instance.ChatColor), this.GetLeaderPersonID(), 0);
			batch_Chat_DLG.PushMsg(chatNameStr, this.tf_chat.Text, CHAT_TYPE.GUILD);
		}
		else if (this.IsMythRaid())
		{
			NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.MYTHRAID, this.tf_chat.Text, false, null, short.Parse(NrTSingleton<WhisperManager>.Instance.ChatColor), this.GetLeaderPersonID(), 0);
		}
		else
		{
			NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.BABELPARTY, this.tf_chat.Text, false, null, short.Parse(NrTSingleton<WhisperManager>.Instance.ChatColor), this.GetLeaderPersonID(), 0);
		}
		this.tf_chat.ClearText();
	}

	private long GetLeaderPersonID()
	{
		if (this.IsMythRaid())
		{
			return SoldierBatch.MYTHRAID_INFO.m_nLeaderPersonID;
		}
		return SoldierBatch.BABELTOWER_INFO.m_nLeaderPersonID;
	}

	private bool IsMythRaid()
	{
		return SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID;
	}

	public override void Update()
	{
		if (this.isClickEnter)
		{
			this.tf_chat.ClearText();
			this.tf_chat.LostFocus();
			this.isClickEnter = false;
		}
		base.Update();
	}

	public void SetAutoBattle(bool _isAutoOn)
	{
		string message = string.Empty;
		if (_isAutoOn)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("523");
		}
		else
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("524");
		}
		Main_UI_SystemMessage.ADDMessage(message);
		this.isAutoOn = _isAutoOn;
		this.SetAutoBattleTexture(this.isAutoOn);
	}

	public void ClickAutoBattleCheck(IUIObject _obj)
	{
		Battle.Send_GS_BATTLE_AUTO_REQ();
	}

	private void SetAutoBattleTexture(bool _isAutoOn)
	{
		if (_isAutoOn)
		{
			this.m_dtAuto.SetTexture("Win_B_Aouto1");
		}
		else
		{
			this.m_dtAuto.SetTexture("Win_B_Aouto2");
		}
	}
}
