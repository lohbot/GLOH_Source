using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class TournamentMasterDlg : Form
{
	private int STATE1 = 4;

	private int STATE2 = 5;

	private int BUTTON = 11;

	private int PLAYER1 = 12;

	private int PLAYER2 = 13;

	private int OBSERVER = 14;

	private int BUTTON_UPDATE = 15;

	private int MATCHINDEX = 16;

	private int TURN = 17;

	private int WINCOUNT1 = 21;

	private int WINCOUNT2 = 22;

	private int DELETEBUTTON = 23;

	private int LOBBY = 24;

	private int m_nLastIndex;

	private bool m_bStart;

	private Label m_lbTitle;

	private NewListBox m_lbBattleMatchInfo;

	private Button m_btDataLoad;

	private Button m_btSlotAdd;

	private Button m_btStart;

	private Button m_btUpdateTurn;

	private Button m_btAlly0;

	private Button m_btAlly1;

	private Button m_btAllyRandom;

	private Button m_btAddLobby;

	private Label m_lbTurn;

	private int m_nFirstTurn = -1;

	private List<TOURNAMENT_MATCH_LIST> m_liMatchList;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Tournament_Master", G_ID.TOURNAMENT_MASTER_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_Title") as Label);
		this.m_lbBattleMatchInfo = (base.GetControl("NewListBox_BattleSlot") as NewListBox);
		this.m_btDataLoad = (base.GetControl("Button_DataLoad") as Button);
		Button expr_48 = this.m_btDataLoad;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.OnClickDataLoad));
		this.m_btStart = (base.GetControl("Button_START") as Button);
		Button expr_85 = this.m_btStart;
		expr_85.Click = (EZValueChangedDelegate)Delegate.Combine(expr_85.Click, new EZValueChangedDelegate(this.OnClickStart));
		this.m_btSlotAdd = (base.GetControl("Button_SlotAdd") as Button);
		Button expr_C2 = this.m_btSlotAdd;
		expr_C2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C2.Click, new EZValueChangedDelegate(this.OnClickDataAdd));
		this.m_btAddLobby = (base.GetControl("Button_SlotAddLobby") as Button);
		Button expr_FF = this.m_btAddLobby;
		expr_FF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_FF.Click, new EZValueChangedDelegate(this.OnClickDataAddUseLobby));
		this.m_btUpdateTurn = (base.GetControl("Button_UpdateTurn") as Button);
		this.m_btUpdateTurn.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2426"));
		Button expr_156 = this.m_btUpdateTurn;
		expr_156.Click = (EZValueChangedDelegate)Delegate.Combine(expr_156.Click, new EZValueChangedDelegate(this.OnClickUpdateTurn));
		this.m_btAlly0 = (base.GetControl("Button_Ally0") as Button);
		this.m_btAlly0.SetText("1");
		Button expr_1A3 = this.m_btAlly0;
		expr_1A3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1A3.Click, new EZValueChangedDelegate(this.OnClickAlly));
		this.m_btAlly1 = (base.GetControl("Button_Ally1") as Button);
		this.m_btAlly1.SetText("2");
		Button expr_1F0 = this.m_btAlly1;
		expr_1F0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1F0.Click, new EZValueChangedDelegate(this.OnClickAlly));
		this.m_btAllyRandom = (base.GetControl("Button_Random") as Button);
		this.m_btAllyRandom.SetText("R");
		Button expr_23D = this.m_btAllyRandom;
		expr_23D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_23D.Click, new EZValueChangedDelegate(this.OnClickAlly));
		this.m_lbTurn = (base.GetControl("Label_TURN") as Label);
		this.m_lbTurn.SetText(string.Empty);
		base.SetScreenCenter();
		this.m_liMatchList = new List<TOURNAMENT_MATCH_LIST>();
		GS_TOURNAMENT_START_REQ gS_TOURNAMENT_START_REQ = new GS_TOURNAMENT_START_REQ();
		gS_TOURNAMENT_START_REQ.bSet = false;
		gS_TOURNAMENT_START_REQ.bStart = true;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_START_REQ, gS_TOURNAMENT_START_REQ);
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
		this.UpdateButtonState();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
	}

	public void OnClickStart(IUIObject obj)
	{
		if (!this.m_bStart)
		{
			GS_TOURNAMENT_START_REQ gS_TOURNAMENT_START_REQ = new GS_TOURNAMENT_START_REQ();
			gS_TOURNAMENT_START_REQ.bSet = true;
			gS_TOURNAMENT_START_REQ.bStart = true;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_START_REQ, gS_TOURNAMENT_START_REQ);
		}
		else
		{
			GS_TOURNAMENT_START_REQ gS_TOURNAMENT_START_REQ2 = new GS_TOURNAMENT_START_REQ();
			gS_TOURNAMENT_START_REQ2.bSet = true;
			gS_TOURNAMENT_START_REQ2.bStart = false;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_START_REQ, gS_TOURNAMENT_START_REQ2);
		}
	}

	public void OnClickDataLoad(IUIObject obj)
	{
		GS_TOURNAMENT_MATCHLIST_REQ gS_TOURNAMENT_MATCHLIST_REQ = new GS_TOURNAMENT_MATCHLIST_REQ();
		gS_TOURNAMENT_MATCHLIST_REQ.bReload = false;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_MATCHLIST_REQ, gS_TOURNAMENT_MATCHLIST_REQ);
		this.m_liMatchList.Clear();
		this.m_lbBattleMatchInfo.Clear();
		this.m_nLastIndex = 0;
	}

	public void OnClickDataAdd(IUIObject obj)
	{
		this.AddMatchInfo(false);
	}

	public void OnClickDataAddUseLobby(IUIObject obj)
	{
		this.AddMatchInfo(true);
	}

	public void OnClickUpdateTurn(IUIObject obj)
	{
		if (this.m_nFirstTurn < 0)
		{
			return;
		}
		foreach (TOURNAMENT_MATCH_LIST current in this.m_liMatchList)
		{
			if (current != null)
			{
				if (current.m_nStartTurnAlly != this.m_nFirstTurn)
				{
					current.m_nStartTurnAlly = this.m_nFirstTurn;
					this.ChangeTURNState(current);
				}
				this.UpdateTournamentMatchInfoFromListBox(current);
				this.ChangeTURNState(current);
				this.UpdateMatchInfoToServer(current, false);
			}
		}
	}

	public void OnClickAlly(IUIObject obj)
	{
		Button x = obj as Button;
		if (x == null)
		{
			return;
		}
		if (x == this.m_btAlly0)
		{
			this.m_nFirstTurn = 0;
			this.m_lbTurn.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2387"));
		}
		else if (x == this.m_btAlly1)
		{
			this.m_nFirstTurn = 1;
			this.m_lbTurn.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2388"));
		}
		else if (x == this.m_btAllyRandom)
		{
			this.m_nFirstTurn = 2;
			this.m_lbTurn.SetText("RANDOM");
		}
	}

	public void OnClickMatch(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null)
		{
			if (tOURNAMENT_MATCH_LIST.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH)
			{
				GS_TOURNAMENT_STATE_UPDATE_REQ gS_TOURNAMENT_STATE_UPDATE_REQ = new GS_TOURNAMENT_STATE_UPDATE_REQ();
				TKString.StringChar(tOURNAMENT_MATCH_LIST.m_szPlayer[0], ref gS_TOURNAMENT_STATE_UPDATE_REQ.szPlayerName);
				gS_TOURNAMENT_STATE_UPDATE_REQ.nMatchState = 2;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_STATE_UPDATE_REQ, gS_TOURNAMENT_STATE_UPDATE_REQ);
			}
			else if (tOURNAMENT_MATCH_LIST.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE)
			{
				GS_TOURNAMENT_STATE_UPDATE_REQ gS_TOURNAMENT_STATE_UPDATE_REQ2 = new GS_TOURNAMENT_STATE_UPDATE_REQ();
				TKString.StringChar(tOURNAMENT_MATCH_LIST.m_szPlayer[0], ref gS_TOURNAMENT_STATE_UPDATE_REQ2.szPlayerName);
				gS_TOURNAMENT_STATE_UPDATE_REQ2.nMatchState = 8;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_STATE_UPDATE_REQ, gS_TOURNAMENT_STATE_UPDATE_REQ2);
			}
			else if (tOURNAMENT_MATCH_LIST.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY)
			{
				GS_TOURNAMENT_STATE_UPDATE_REQ gS_TOURNAMENT_STATE_UPDATE_REQ3 = new GS_TOURNAMENT_STATE_UPDATE_REQ();
				TKString.StringChar(tOURNAMENT_MATCH_LIST.m_szPlayer[0], ref gS_TOURNAMENT_STATE_UPDATE_REQ3.szPlayerName);
				gS_TOURNAMENT_STATE_UPDATE_REQ3.nMatchState = 4;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_STATE_UPDATE_REQ, gS_TOURNAMENT_STATE_UPDATE_REQ3);
			}
		}
	}

	public void OnClickUpdateMatch(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null)
		{
			this.UpdateTournamentMatchInfoFromListBox(tOURNAMENT_MATCH_LIST);
			this.ChangeTURNState(tOURNAMENT_MATCH_LIST);
			this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, false);
		}
	}

	public void OnClickTurn1(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null && tOURNAMENT_MATCH_LIST.m_nStartTurnAlly != 0)
		{
			tOURNAMENT_MATCH_LIST.m_nStartTurnAlly = 0;
			this.ChangeTURNState(tOURNAMENT_MATCH_LIST);
			this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, false);
		}
	}

	public void OnClickTurn2(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null && tOURNAMENT_MATCH_LIST.m_nStartTurnAlly != 1)
		{
			tOURNAMENT_MATCH_LIST.m_nStartTurnAlly = 1;
			this.ChangeTURNState(tOURNAMENT_MATCH_LIST);
			this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, false);
		}
	}

	public void OnClickTurnR(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null && tOURNAMENT_MATCH_LIST.m_nStartTurnAlly != 2)
		{
			tOURNAMENT_MATCH_LIST.m_nStartTurnAlly = 2;
			this.ChangeTURNState(tOURNAMENT_MATCH_LIST);
			this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, false);
		}
	}

	public void OnClickDELETE(IUIObject obj)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = obj.Data as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST != null)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("216"),
				"charname1",
				tOURNAMENT_MATCH_LIST.m_szPlayer[0],
				"charname2",
				tOURNAMENT_MATCH_LIST.m_szPlayer[1]
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnDeleteMatchOK), tOURNAMENT_MATCH_LIST, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1795"), empty, eMsgType.MB_OK_CANCEL, 2);
		}
	}

	public void OnDeleteMatchOK(object a_oObject)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = a_oObject as TOURNAMENT_MATCH_LIST;
		if (tOURNAMENT_MATCH_LIST == null)
		{
			return;
		}
		this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, true);
	}

	public void SetShowList()
	{
		if (this.m_liMatchList == null)
		{
			return;
		}
		if (this.m_liMatchList.Count <= 0)
		{
			return;
		}
		this.m_lbBattleMatchInfo.Clear();
		foreach (TOURNAMENT_MATCH_LIST current in this.m_liMatchList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbBattleMatchInfo.ColumnNum, true, string.Empty);
			string tournamentStateToString = this.GetTournamentStateToString(current.ePlayerState[0]);
			newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2387"), null, null, null);
			newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2388"), null, null, null);
			newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2389"), null, null, null);
			newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2390"), null, null, null);
			newListItem.SetListItemData(this.STATE1, tournamentStateToString, null, null, null);
			tournamentStateToString = this.GetTournamentStateToString(current.ePlayerState[1]);
			newListItem.SetListItemData(this.STATE2, tournamentStateToString, null, null, null);
			newListItem.SetListItemData(this.BUTTON, string.Empty, current, new EZValueChangedDelegate(this.OnClickMatch), null);
			newListItem.SetListItemData(this.PLAYER1, current.m_szPlayer[0], null, null, null);
			newListItem.SetListItemData(this.PLAYER2, current.m_szPlayer[1], null, null, null);
			newListItem.SetListItemData(this.OBSERVER, current.m_szObserver, null, null, null);
			newListItem.SetListItemData(this.BUTTON_UPDATE, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2420"), current, new EZValueChangedDelegate(this.OnClickUpdateMatch), null);
			newListItem.SetListItemData(this.MATCHINDEX, current.nIndex.ToString(), null, null, null);
			if (current.m_nStartTurnAlly == 0)
			{
				newListItem.SetListItemData(this.TURN, current.m_szPlayer[0], null, null, null);
			}
			else if (current.m_nStartTurnAlly == 1)
			{
				newListItem.SetListItemData(this.TURN, current.m_szPlayer[1], null, null, null);
			}
			else
			{
				newListItem.SetListItemData(this.TURN, "RANDOM", null, null, null);
			}
			newListItem.SetListItemData(18, "1", current, new EZValueChangedDelegate(this.OnClickTurn1), null);
			newListItem.SetListItemData(19, "2", current, new EZValueChangedDelegate(this.OnClickTurn2), null);
			newListItem.SetListItemData(20, "R", current, new EZValueChangedDelegate(this.OnClickTurnR), null);
			newListItem.SetListItemData(this.WINCOUNT1, current.m_nWinCount[0].ToString(), null, null, null);
			newListItem.SetListItemData(this.WINCOUNT2, current.m_nWinCount[1].ToString(), null, null, null);
			newListItem.SetListItemData(this.DELETEBUTTON, string.Empty, current, new EZValueChangedDelegate(this.OnClickDELETE), null);
			if (current.bUseLoddy)
			{
				newListItem.SetListItemData(this.LOBBY, "LOBBY", null, null, null);
			}
			else
			{
				newListItem.SetListItemData(this.LOBBY, string.Empty, null, null, null);
			}
			newListItem.SetListItemEnable(this.LOBBY, current.bUseLoddy);
			this.m_lbBattleMatchInfo.Add(newListItem);
			this.ChangeButtonState(current);
		}
		this.m_lbBattleMatchInfo.RepositionItems();
	}

	public string GetTournamentStateToString(eTOURNAMENT_PLAYER_STATE eState)
	{
		int num = (int)(1101 + eState);
		string result = string.Empty;
		if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2421");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOGIN)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2423");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2425");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2424");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2392");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2762");
		}
		else if (eState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_END)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor(num.ToString()) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2763");
		}
		return result;
	}

	public void SetStart(bool bStart)
	{
		if (this.m_bStart != bStart)
		{
			this.m_bStart = bStart;
		}
		if (this.m_bStart)
		{
			this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2386") + "(" + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("359") + ")");
			this.m_btStart.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2419"));
		}
		else
		{
			this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2386"));
			this.m_btStart.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2418"));
		}
		if (!this.m_btStart.Visible)
		{
			this.m_btStart.Visible = true;
		}
	}

	public void SetMatchList(GS_TOURNAMENT_MATCHLIST_INFO pkInfo)
	{
		if (pkInfo == null)
		{
			return;
		}
		if (this.m_liMatchList == null)
		{
			return;
		}
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = new TOURNAMENT_MATCH_LIST();
		tOURNAMENT_MATCH_LIST.nIndex = pkInfo.nIndex;
		tOURNAMENT_MATCH_LIST.m_szPlayer[0] = TKString.NEWString(pkInfo.szCharName1);
		tOURNAMENT_MATCH_LIST.ePlayerState[0] = (eTOURNAMENT_PLAYER_STATE)pkInfo.nPlayerState1;
		tOURNAMENT_MATCH_LIST.m_szPlayer[1] = TKString.NEWString(pkInfo.szCharName2);
		tOURNAMENT_MATCH_LIST.ePlayerState[1] = (eTOURNAMENT_PLAYER_STATE)pkInfo.nPlayerState2;
		tOURNAMENT_MATCH_LIST.m_szObserver = TKString.NEWString(pkInfo.szObserver);
		tOURNAMENT_MATCH_LIST.m_nWinCount[0] = pkInfo.nWinCount1;
		tOURNAMENT_MATCH_LIST.m_nWinCount[1] = pkInfo.nWinCount2;
		tOURNAMENT_MATCH_LIST.bUseLoddy = pkInfo.bUseLobby;
		tOURNAMENT_MATCH_LIST.m_nStartTurnAlly = pkInfo.i32FirstTurn;
		if (this.m_nLastIndex < tOURNAMENT_MATCH_LIST.nIndex)
		{
			this.m_nLastIndex = tOURNAMENT_MATCH_LIST.nIndex;
		}
		this.m_liMatchList.Add(tOURNAMENT_MATCH_LIST);
	}

	public void ChangePlayerState(GS_TOURNAMENT_PLAYER_STATE_NFY _NFY)
	{
		string value = TKString.NEWString(_NFY.szPlayerName);
		bool flag = false;
		foreach (TOURNAMENT_MATCH_LIST current in this.m_liMatchList)
		{
			int i = 0;
			while (i < 2)
			{
				if (current.m_szPlayer[i].Equals(value))
				{
					if (_NFY.nPlayerState == 10)
					{
						this.m_liMatchList.Remove(current);
						flag = true;
						break;
					}
					if (_NFY.nPlayerState != 9)
					{
						current.ePlayerState[i] = (eTOURNAMENT_PLAYER_STATE)_NFY.nPlayerState;
					}
					string tournamentStateToString = this.GetTournamentStateToString(current.ePlayerState[i]);
					for (int j = 0; j < this.m_liMatchList.Count; j++)
					{
						UIListItemContainer item = this.m_lbBattleMatchInfo.GetItem(j);
						if (item != null)
						{
							TextField textField = item.GetElement(this.PLAYER1) as TextField;
							if (textField != null && textField.GetText().Equals(value))
							{
								if (_NFY.nPlayerState == 9)
								{
									Label label = item.GetElement(this.WINCOUNT1) as Label;
									if (label != null)
									{
										current.m_nWinCount[i] = current.m_nWinCount[i] + 1;
										label.SetText(current.m_nWinCount[i].ToString());
									}
								}
								else
								{
									Label label2 = item.GetElement(this.STATE1) as Label;
									if (label2 != null)
									{
										label2.SetText(tournamentStateToString);
										break;
									}
								}
							}
							textField = (item.GetElement(this.PLAYER2) as TextField);
							if (textField != null && textField.GetText().Equals(value))
							{
								if (_NFY.nPlayerState == 9)
								{
									Label label3 = item.GetElement(this.WINCOUNT2) as Label;
									if (label3 != null)
									{
										current.m_nWinCount[i] = current.m_nWinCount[i] + 1;
										label3.SetText(current.m_nWinCount[i].ToString());
									}
								}
								else
								{
									Label label4 = item.GetElement(this.STATE2) as Label;
									if (label4 != null)
									{
										label4.SetText(tournamentStateToString);
										break;
									}
								}
							}
						}
					}
					return;
				}
				else
				{
					i++;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			this.m_lbBattleMatchInfo.Clear();
			this.SetShowList();
		}
	}

	public void ChangeButtonState(TOURNAMENT_MATCH_LIST pkMatch)
	{
		for (int i = 0; i < this.m_liMatchList.Count; i++)
		{
			UIListItemContainer item = this.m_lbBattleMatchInfo.GetItem(i);
			if (item != null)
			{
				TextField textField = item.GetElement(this.PLAYER1) as TextField;
				if (textField != null && textField.GetText().Equals(pkMatch.m_szPlayer[0]))
				{
					UIButton uIButton = item.GetElement(this.BUTTON) as UIButton;
					if (uIButton != null)
					{
						if (pkMatch.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE)
						{
							if (pkMatch.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH)
							{
								uIButton.enabled = true;
								uIButton.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2391"));
							}
							else if (pkMatch.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY)
							{
								uIButton.enabled = false;
								uIButton.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2422"));
							}
							else if (pkMatch.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY)
							{
								uIButton.enabled = true;
								uIButton.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2765"));
							}
							else if (pkMatch.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE)
							{
								uIButton.enabled = true;
								uIButton.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2392"));
							}
							else if (pkMatch.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT)
							{
								uIButton.enabled = false;
								uIButton.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2766"));
							}
						}
						else
						{
							uIButton.enabled = false;
							uIButton.SetText(string.Empty);
						}
					}
				}
			}
		}
	}

	public void ChangeTURNState(TOURNAMENT_MATCH_LIST pkMatch)
	{
		for (int i = 0; i < this.m_liMatchList.Count; i++)
		{
			UIListItemContainer item = this.m_lbBattleMatchInfo.GetItem(i);
			if (item != null)
			{
				TextField textField = item.GetElement(this.PLAYER1) as TextField;
				if (textField != null && textField.GetText().Equals(pkMatch.m_szPlayer[0]))
				{
					Label label = item.GetElement(this.TURN) as Label;
					if (label != null)
					{
						if (pkMatch.m_nStartTurnAlly == 0)
						{
							label.SetText(pkMatch.m_szPlayer[0]);
						}
						else if (pkMatch.m_nStartTurnAlly == 1)
						{
							label.SetText(pkMatch.m_szPlayer[1]);
						}
						else
						{
							label.SetText("RANDOM");
						}
					}
				}
			}
		}
	}

	public void UpdateTournamentMatchInfoFromListBox(TOURNAMENT_MATCH_LIST pkMatch)
	{
		for (int i = 0; i < this.m_liMatchList.Count; i++)
		{
			UIListItemContainer item = this.m_lbBattleMatchInfo.GetItem(i);
			if (item != null)
			{
				Label label = item.GetElement(this.MATCHINDEX) as Label;
				if (label != null)
				{
					int num = int.Parse(label.GetText());
					if (num == pkMatch.nIndex)
					{
						TextField textField = item.GetElement(this.PLAYER1) as TextField;
						if (textField != null && !textField.GetText().Equals(pkMatch.m_szPlayer[0]))
						{
							pkMatch.m_szPlayer[0] = textField.GetText();
						}
						textField = (item.GetElement(this.PLAYER2) as TextField);
						if (textField != null && !textField.GetText().Equals(pkMatch.m_szPlayer[1]))
						{
							pkMatch.m_szPlayer[1] = textField.GetText();
						}
						textField = (item.GetElement(this.OBSERVER) as TextField);
						if (textField != null && !textField.GetText().Equals(pkMatch.m_szObserver))
						{
							pkMatch.m_szObserver = textField.GetText();
						}
					}
				}
			}
		}
	}

	public void UpdateButtonState()
	{
		foreach (TOURNAMENT_MATCH_LIST current in this.m_liMatchList)
		{
			if ((current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE || current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE) && current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE)
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE;
				this.ChangeButtonState(current);
			}
			if (current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOGIN && current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOGIN && current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH)
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH;
				this.ChangeButtonState(current);
			}
			if (current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH && current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_MATCH && current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY)
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY;
				this.ChangeButtonState(current);
			}
			if (current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY && current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY)
			{
				if (!current.bUseLoddy)
				{
					if (current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE)
					{
						current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE;
						this.ChangeButtonState(current);
					}
				}
				else if (current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY)
				{
					current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY;
					this.ChangeButtonState(current);
				}
			}
			if (current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT && current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT && current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT)
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_WAIT;
				this.ChangeButtonState(current);
			}
			if (current.ePlayerState[0] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_END && current.ePlayerState[1] == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_END && current.eMatchState != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE)
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE;
				this.ChangeButtonState(current);
			}
			if (current.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE && !current.bUseLoddy)
			{
				if (current.ePlayerState[0] != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY || current.ePlayerState[1] != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY)
				{
					current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY;
					this.ChangeButtonState(current);
				}
			}
			else if (current.eMatchState == eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_BATTLE && current.bUseLoddy && (current.ePlayerState[0] != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_END || current.ePlayerState[1] != eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_LOBBY_END))
			{
				current.eMatchState = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_READY;
				this.ChangeButtonState(current);
			}
		}
	}

	public void UpdateMatchInfoToServer(TOURNAMENT_MATCH_LIST pkMatchInfo, bool bDelete)
	{
		GS_TOURNAMENT_MATCH_UPDATE_REQ gS_TOURNAMENT_MATCH_UPDATE_REQ = new GS_TOURNAMENT_MATCH_UPDATE_REQ();
		gS_TOURNAMENT_MATCH_UPDATE_REQ.nIndex = pkMatchInfo.nIndex;
		TKString.StringChar(pkMatchInfo.m_szPlayer[0], ref gS_TOURNAMENT_MATCH_UPDATE_REQ.szCharName1);
		TKString.StringChar(pkMatchInfo.m_szPlayer[1], ref gS_TOURNAMENT_MATCH_UPDATE_REQ.szCharName2);
		TKString.StringChar(pkMatchInfo.m_szObserver, ref gS_TOURNAMENT_MATCH_UPDATE_REQ.szObserver);
		gS_TOURNAMENT_MATCH_UPDATE_REQ.i32FirstTurn = pkMatchInfo.m_nStartTurnAlly;
		gS_TOURNAMENT_MATCH_UPDATE_REQ.bUseLobby = pkMatchInfo.bUseLoddy;
		gS_TOURNAMENT_MATCH_UPDATE_REQ.bDelete = bDelete;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_MATCH_UPDATE_REQ, gS_TOURNAMENT_MATCH_UPDATE_REQ);
	}

	public void AddMatchInfo(bool bUseLobby)
	{
		TOURNAMENT_MATCH_LIST tOURNAMENT_MATCH_LIST = new TOURNAMENT_MATCH_LIST();
		tOURNAMENT_MATCH_LIST.nIndex = this.m_nLastIndex + 1;
		this.m_nLastIndex++;
		tOURNAMENT_MATCH_LIST.m_szPlayer[0] = "Player" + tOURNAMENT_MATCH_LIST.nIndex.ToString() + "_1";
		tOURNAMENT_MATCH_LIST.ePlayerState[0] = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE;
		tOURNAMENT_MATCH_LIST.m_szPlayer[1] = "Player" + tOURNAMENT_MATCH_LIST.nIndex.ToString() + "_2";
		tOURNAMENT_MATCH_LIST.ePlayerState[1] = eTOURNAMENT_PLAYER_STATE.eTOURNAMENT_PLAYER_STATE_NONE;
		tOURNAMENT_MATCH_LIST.m_szObserver = "OBSERVER" + tOURNAMENT_MATCH_LIST.nIndex.ToString();
		tOURNAMENT_MATCH_LIST.m_nWinCount[0] = 0;
		tOURNAMENT_MATCH_LIST.m_nWinCount[1] = 0;
		tOURNAMENT_MATCH_LIST.m_nStartTurnAlly = 0;
		tOURNAMENT_MATCH_LIST.bUseLoddy = bUseLobby;
		this.m_liMatchList.Add(tOURNAMENT_MATCH_LIST);
		NewListItem newListItem = new NewListItem(this.m_lbBattleMatchInfo.ColumnNum, true, string.Empty);
		string tournamentStateToString = this.GetTournamentStateToString(tOURNAMENT_MATCH_LIST.ePlayerState[0]);
		newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2387"), null, null, null);
		newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2388"), null, null, null);
		newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2389"), null, null, null);
		newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2390"), null, null, null);
		newListItem.SetListItemData(this.STATE1, tournamentStateToString, null, null, null);
		tournamentStateToString = this.GetTournamentStateToString(tOURNAMENT_MATCH_LIST.ePlayerState[1]);
		newListItem.SetListItemData(this.STATE2, tournamentStateToString, null, null, null);
		newListItem.SetListItemData(this.BUTTON, string.Empty, tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickMatch), null);
		newListItem.SetListItemData(this.PLAYER1, tOURNAMENT_MATCH_LIST.m_szPlayer[0], null, null, null);
		newListItem.SetListItemData(this.PLAYER2, tOURNAMENT_MATCH_LIST.m_szPlayer[1], null, null, null);
		newListItem.SetListItemData(this.OBSERVER, tOURNAMENT_MATCH_LIST.m_szObserver, null, null, null);
		newListItem.SetListItemData(this.BUTTON_UPDATE, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2420"), tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickUpdateMatch), null);
		newListItem.SetListItemData(this.MATCHINDEX, tOURNAMENT_MATCH_LIST.nIndex.ToString(), null, null, null);
		if (tOURNAMENT_MATCH_LIST.m_nStartTurnAlly == 0)
		{
			newListItem.SetListItemData(this.TURN, tOURNAMENT_MATCH_LIST.m_szPlayer[0], null, null, null);
		}
		else if (tOURNAMENT_MATCH_LIST.m_nStartTurnAlly == 1)
		{
			newListItem.SetListItemData(this.TURN, tOURNAMENT_MATCH_LIST.m_szPlayer[1], null, null, null);
		}
		else
		{
			newListItem.SetListItemData(this.TURN, "RANDOM", null, null, null);
		}
		newListItem.SetListItemData(18, "1", tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickTurn1), null);
		newListItem.SetListItemData(19, "2", tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickTurn2), null);
		newListItem.SetListItemData(20, "R", tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickTurnR), null);
		newListItem.SetListItemData(this.WINCOUNT1, tOURNAMENT_MATCH_LIST.m_nWinCount[0].ToString(), null, null, null);
		newListItem.SetListItemData(this.WINCOUNT2, tOURNAMENT_MATCH_LIST.m_nWinCount[1].ToString(), null, null, null);
		newListItem.SetListItemData(this.DELETEBUTTON, string.Empty, tOURNAMENT_MATCH_LIST, new EZValueChangedDelegate(this.OnClickDELETE), null);
		if (bUseLobby)
		{
			newListItem.SetListItemData(this.LOBBY, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2764"), null, null, null);
		}
		else
		{
			newListItem.SetListItemData(this.LOBBY, string.Empty, null, null, null);
		}
		newListItem.SetListItemEnable(this.LOBBY, bUseLobby);
		this.m_lbBattleMatchInfo.Add(newListItem);
		this.m_lbBattleMatchInfo.RepositionItems();
		this.UpdateTournamentMatchInfoFromListBox(tOURNAMENT_MATCH_LIST);
		this.ChangeTURNState(tOURNAMENT_MATCH_LIST);
		this.UpdateMatchInfoToServer(tOURNAMENT_MATCH_LIST, false);
	}
}
