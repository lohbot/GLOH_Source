using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BabelLobbyUserListDlg : Form
{
	private const int MAX_USER_COUNT = 4;

	private const int LEADER_INDEX = 0;

	public DropDownList[] m_ddlSlotType = new DropDownList[4];

	public Label[] m_laSlotState = new Label[4];

	public Button m_btStart;

	public Button m_btReady;

	private GameObject m_StartEffect;

	private BABEL_USER_CONTROL_INFO[] user_info = new BABEL_USER_CONTROL_INFO[4];

	private int m_nSelectIndex;

	private float m_fRoateVal = 5f;

	private int nInjurySoldierCount;

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
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_char_solnum", G_ID.BABELTOWERUSERLIST_DLG, false, true);
		float x = GUICamera.width - base.GetSizeX();
		float y = 0f;
		base.SetLocation(x, y);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(1005f);
	}

	public override void SetComponent()
	{
		this.m_btStart = (base.GetControl("Button_Start") as Button);
		Button expr_1C = this.m_btStart;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickStartBabel));
		this.m_btStart.EffectAni = false;
		this.m_btReady = (base.GetControl("Button_Ready") as Button);
		Button expr_65 = this.m_btReady;
		expr_65.Click = (EZValueChangedDelegate)Delegate.Combine(expr_65.Click, new EZValueChangedDelegate(this.OnClickReady));
		this.m_btReady.EffectAni = false;
		for (int i = 0; i < 4; i++)
		{
			this.user_info[i] = new BABEL_USER_CONTROL_INFO();
			this.user_info[i].m_laUserName = (base.GetControl("Label_User" + i.ToString()) as Label);
			this.user_info[i].m_laUserSolNum = (base.GetControl("Label_Solnum" + i.ToString()) as Label);
			if (i != 0)
			{
				this.user_info[i].m_btKickUser = (base.GetControl("Button_Kick" + i.ToString()) as Button);
				this.user_info[i].m_dtReady = (base.GetControl("DT_Ready" + i.ToString()) as DrawTexture);
				this.user_info[i].m_dtReady.SetLocation(this.user_info[i].m_dtReady.GetLocationX(), this.user_info[i].m_dtReady.GetLocationY(), this.user_info[i].m_dtReady.GetLocation().z - 0.1f);
				this.user_info[i].m_laReady = (base.GetControl("LB_Ready" + i.ToString()) as Label);
				this.user_info[i].m_laReady.SetLocation(this.user_info[i].m_laReady.GetLocationX(), this.user_info[i].m_laReady.GetLocationY(), this.user_info[i].m_laReady.GetLocation().z - 0.1f);
				this.user_info[i].m_laSlotState = (base.GetControl("LB_Status" + i.ToString()) as Label);
				this.user_info[i].m_dLoadingImg = (base.GetControl("DrawTexture_Loading" + i.ToString()) as DrawTexture);
				this.user_info[i].InitLoadingState();
				this.user_info[i].Show(false);
				this.m_ddlSlotType[i] = (base.GetControl("DropDownList_" + i.ToString()) as DropDownList);
				if (this.m_ddlSlotType[i] != null)
				{
					this.m_ddlSlotType[i].SetViewArea(0);
					this.m_ddlSlotType[i].Clear();
					string text = string.Empty;
					for (int j = 0; j < 3; j++)
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
						if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique || j != 2)
						{
							this.m_ddlSlotType[i].AddItem(text, j);
						}
					}
					this.m_ddlSlotType[i].Data = i;
					this.m_ddlSlotType[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.DropDownChangeSlotType));
					this.m_ddlSlotType[i].RepositionItems();
					this.m_ddlSlotType[i].SetFirstItem();
				}
			}
		}
		base.SetShowLayer(5, false);
		base.SetShowLayer(6, false);
		base.SetShowLayer(7, false);
		base.SetShowLayer(8, false);
		this.Hide();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != 0)
			{
				if (SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(i) != null)
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
		for (int i = 0; i < 5; i++)
		{
			base.SetShowLayer(i, false);
		}
		base.SetShowLayer(0, true);
		BABELTOWER_PERSON babelLeaderInfo = SoldierBatch.BABELTOWER_INFO.GetBabelLeaderInfo();
		if (babelLeaderInfo != null)
		{
			if (@char.GetPersonID() == babelLeaderInfo.nPartyPersonID)
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
		for (int j = 0; j < 4; j++)
		{
			if (j != 0)
			{
				BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(j);
				if (babelPersonInfo.nPartyPersonID > 0L)
				{
					base.SetShowLayer(j + 1, true);
				}
				else
				{
					base.SetShowLayer(j + 1, false);
				}
			}
		}
	}

	public void RefreshPossibleLevel()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		BABELTOWER_PERSON babelLeaderInfo = SoldierBatch.BABELTOWER_INFO.GetBabelLeaderInfo();
		if (babelLeaderInfo == null || @char.GetPersonID() == babelLeaderInfo.nPartyPersonID)
		{
		}
	}

	public void SetWaitingLock(bool bShow)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != 0)
			{
				BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(i);
				if (babelPersonInfo != null)
				{
					if (babelPersonInfo.nPartyPersonID > 0L)
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
		if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique)
		{
			if (SoldierBatch.BABELTOWER_INFO.m_nBabelFloorType == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2787");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("833");
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"floor",
				SoldierBatch.BABELTOWER_INFO.m_nBabelFloor,
				"subfloor",
				(int)(SoldierBatch.BABELTOWER_INFO.m_nBabelSubFloor + 1)
			});
		}
		else
		{
			BountyInfoData bountyInfoDataFromUnique = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataFromUnique(SoldierBatch.BABELTOWER_INFO.BountHuntUnique);
			if (bountyInfoDataFromUnique != null)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bountyInfoDataFromUnique.i32WeekTitleKey.ToString());
			}
		}
		PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
		if (plunderSolNumDlg != null)
		{
			plunderSolNumDlg.SetTitle(text2);
		}
		BABELTOWER_PERSON babelLeaderInfo = SoldierBatch.BABELTOWER_INFO.GetBabelLeaderInfo();
		if (babelLeaderInfo != null)
		{
			this.user_info[0].Show(true);
			this.user_info[0].PersonID = babelLeaderInfo.nPartyPersonID;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"count",
				babelLeaderInfo.nLevel.ToString(),
				"targetname",
				babelLeaderInfo.strCharName
			});
			this.user_info[0].m_laUserName.SetText(text2);
			for (int j = 0; j < 4; j++)
			{
				if (j != 0)
				{
					BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(j);
					if (babelPersonInfo != null)
					{
						if (babelPersonInfo.nPartyPersonID > 0L)
						{
							this.m_ddlSlotType[j].SetVisible(false);
							this.user_info[j].Show(true);
							this.user_info[j].PersonID = babelPersonInfo.nPartyPersonID;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								text,
								"count",
								babelPersonInfo.nLevel.ToString(),
								"targetname",
								babelPersonInfo.strCharName
							});
							this.user_info[j].m_laUserName.SetText(text2);
							this.user_info[j].m_dtReady.Visible = babelPersonInfo.bReady;
							if (babelPersonInfo.bReady)
							{
								this.user_info[j].m_laReady.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("662");
							}
							else
							{
								this.user_info[j].m_laReady.Text = string.Empty;
							}
							if (@char.GetPersonID() == babelLeaderInfo.nPartyPersonID || @char.GetPersonID() == babelPersonInfo.nPartyPersonID)
							{
								this.user_info[j].m_btKickUser.Data = j;
								this.user_info[j].m_btKickUser.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickKickUser));
							}
							else
							{
								this.user_info[j].m_btKickUser.Visible = false;
							}
						}
						else
						{
							this.user_info[j].Show(false);
							if (@char.GetPersonID() == babelLeaderInfo.nPartyPersonID)
							{
								this.user_info[j].SetSlotState(false, 0);
								this.m_ddlSlotType[j].SetVisible(true);
							}
							else
							{
								this.user_info[j].SetSlotState(true, (int)babelPersonInfo.nSlotType);
								this.m_ddlSlotType[j].SetVisible(false);
							}
						}
					}
				}
			}
		}
		if (@char.GetPersonID() == babelLeaderInfo.nPartyPersonID)
		{
			if (SoldierBatch.BABELTOWER_INFO.IsCanBattle())
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
		BABELTOWER_PERSON babelLeaderInfo = SoldierBatch.BABELTOWER_INFO.GetBabelLeaderInfo();
		int num = (int)SoldierBatch.BABELTOWER_INFO.Count;
		if (num <= 0)
		{
			num = 1;
		}
		int num2;
		if (num == 1)
		{
			num2 = 12;
		}
		else
		{
			num2 = 12 / num + 1;
		}
		for (int i = 0; i < 4; i++)
		{
			if (this.user_info[i].PersonID > 0L)
			{
				this.user_info[i].m_laUserSolNum.SetText(SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(this.user_info[i].PersonID).ToString() + "/" + num2.ToString());
			}
		}
		if (charPersonInfo.GetPersonID() != babelLeaderInfo.nPartyPersonID)
		{
			if (!SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()) && SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(charPersonInfo.GetPersonID()) == num2)
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

	public void UpdateBabelReadyState()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			return;
		}
		bool flag = SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID());
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

	public bool SetBabelReadyState()
	{
		bool flag = false;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return flag;
		}
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			return flag;
		}
		if (!SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
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
			SoldierBatch.BABELTOWER_INFO.Init();
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			this.OnKickOutUser(null);
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("72");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("82");
		string empty = string.Empty;
		BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(num);
		if (babelPersonInfo != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox2,
				"targetname",
				babelPersonInfo.strCharName
			});
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnKickOutUser), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
	}

	private void OnKickOutUser(object a_oObject)
	{
		GS_BABELTOWER_LEAVE_REQ gS_BABELTOWER_LEAVE_REQ = new GS_BABELTOWER_LEAVE_REQ();
		if (this.m_nSelectIndex < 0)
		{
			return;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.GetPersonID() == this.user_info[this.m_nSelectIndex].PersonID)
		{
			gS_BABELTOWER_LEAVE_REQ.mode = 1;
		}
		else
		{
			gS_BABELTOWER_LEAVE_REQ.mode = 2;
		}
		gS_BABELTOWER_LEAVE_REQ.nLeavePersonID = this.user_info[this.m_nSelectIndex].PersonID;
		gS_BABELTOWER_LEAVE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_LEAVE_REQ, gS_BABELTOWER_LEAVE_REQ);
	}

	public void SetUserSlotType(int pos, byte type)
	{
		this.m_ddlSlotType[pos].SetIndex((int)type);
	}

	public void OnClickStartBabel(IUIObject obj)
	{
		byte partyCount = SoldierBatch.BABELTOWER_INFO.GetPartyCount();
		byte readyPersonCount = SoldierBatch.BABELTOWER_INFO.GetReadyPersonCount();
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
		if (SoldierBatch.BABELTOWER_INFO.Count <= 0)
		{
			SoldierBatch.BABELTOWER_INFO.Count = 1;
		}
		byte count = SoldierBatch.BABELTOWER_INFO.Count;
		int num = (int)(12 / count);
		if (count == 1)
		{
			if (!this.IsEnableUseFriend())
			{
				num = 9;
			}
		}
		else
		{
			num = (int)(12 / count + 1);
		}
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
			msgBoxUI.SetMsg(new YesDelegate(this.OnStartBabel), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL, 2);
			return;
		}
		if (solBatchNum2 > num)
		{
			return;
		}
		this.OnStartBabel(null);
	}

	private void OnStartBabel(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		GS_BABELTOWER_START_REQ gS_BABELTOWER_START_REQ = new GS_BABELTOWER_START_REQ();
		gS_BABELTOWER_START_REQ.nCombinationUnique = NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
		gS_BABELTOWER_START_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		gS_BABELTOWER_START_REQ.nPersonID = charPersonInfo.GetPersonID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_START_REQ, gS_BABELTOWER_START_REQ);
		NrTSingleton<FiveRocksEventManager>.Instance.BabelTowerParty((int)SoldierBatch.BABELTOWER_INFO.GetPartyCount());
		if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique)
		{
			if (SoldierBatch.BABELTOWER_INFO.m_nBabelFloorType == 2)
			{
				PlayerPrefs.SetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, (int)SoldierBatch.BABELTOWER_INFO.m_nBabelFloor);
				PlayerPrefs.SetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, (int)SoldierBatch.BABELTOWER_INFO.m_nBabelSubFloor);
			}
			else
			{
				PlayerPrefs.SetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, (int)SoldierBatch.BABELTOWER_INFO.m_nBabelFloor);
				PlayerPrefs.SetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, (int)SoldierBatch.BABELTOWER_INFO.m_nBabelSubFloor);
			}
			PlayerPrefs.SetInt(NrPrefsKey.LASTPLAY_BABELTYPE, (int)SoldierBatch.BABELTOWER_INFO.m_nBabelFloorType);
		}
		for (int i = 0; i < 4; i++)
		{
			BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(i);
			if (babelPersonInfo == null || (babelPersonInfo.nPartyPersonID != charPersonInfo.GetPersonID() && babelPersonInfo.nPartyPersonID > 0L))
			{
				PlayerPrefs.SetString("Babel JoinPlayer" + i, babelPersonInfo.nPartyPersonID.ToString());
				if (0 < SoldierBatch.BABELTOWER_INFO.BountHuntUnique)
				{
					PlayerPrefs.SetString("BountyHunt JoinPlayer" + i, babelPersonInfo.nPartyPersonID.ToString());
				}
			}
		}
	}

	public void OnClickReady(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (!SoldierBatch.BABELTOWER_INFO.IsReadyBattle(charPersonInfo.GetPersonID()))
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
			if (SoldierBatch.BABELTOWER_INFO.Count <= 0)
			{
				SoldierBatch.BABELTOWER_INFO.Count = 1;
			}
			byte count = SoldierBatch.BABELTOWER_INFO.Count;
			int num;
			if (count == 1)
			{
				num = (int)(12 / count);
			}
			else
			{
				num = (int)(12 / count + 1);
			}
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
				msgBoxUI.SetMsg(new YesDelegate(this.OnReadyBabel), null, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL, 2);
				return;
			}
		}
		this.OnReadyBabel(null);
	}

	private void OnReadyBabel(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		bool bReady = this.SetBabelReadyState();
		GS_BABELTOWER_READY_REQ gS_BABELTOWER_READY_REQ = new GS_BABELTOWER_READY_REQ();
		gS_BABELTOWER_READY_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		gS_BABELTOWER_READY_REQ.nPersonID = charPersonInfo.GetPersonID();
		gS_BABELTOWER_READY_REQ.bReady = bReady;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_READY_REQ, gS_BABELTOWER_READY_REQ);
	}

	public void OnClickCancelBabel(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		byte mode;
		if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
		{
			mode = 0;
		}
		else
		{
			mode = 1;
		}
		GS_BABELTOWER_LEAVE_REQ gS_BABELTOWER_LEAVE_REQ = new GS_BABELTOWER_LEAVE_REQ();
		gS_BABELTOWER_LEAVE_REQ.mode = mode;
		gS_BABELTOWER_LEAVE_REQ.nLeavePersonID = charPersonInfo.GetPersonID();
		gS_BABELTOWER_LEAVE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_LEAVE_REQ, gS_BABELTOWER_LEAVE_REQ);
		SoldierBatch.BABELTOWER_INFO.Init();
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void DropDownChangeSlotType(IUIObject obj)
	{
		int num = (int)obj.Data;
		ListItem listItem = this.m_ddlSlotType[num].SelectedItem.Data as ListItem;
		if (listItem != null)
		{
			int num2 = (int)listItem.Key;
			GS_BABELTOWER_CHANGE_SLOTTYPE_REQ gS_BABELTOWER_CHANGE_SLOTTYPE_REQ = new GS_BABELTOWER_CHANGE_SLOTTYPE_REQ();
			gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.pos = num;
			gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
			gS_BABELTOWER_CHANGE_SLOTTYPE_REQ.change_type = (byte)num2;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_CHANGE_SLOTTYPE_REQ, gS_BABELTOWER_CHANGE_SLOTTYPE_REQ);
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

	public bool IsEnableUseFriend()
	{
		int num = 0;
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID > 0L)
			{
				if (uSER_FRIEND_INFO.ui8HelpUse < 1)
				{
					num++;
				}
			}
		}
		return num > 0;
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
}
