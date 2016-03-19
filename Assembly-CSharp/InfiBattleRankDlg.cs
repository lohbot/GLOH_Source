using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class InfiBattleRankDlg : Form
{
	private Toggle[] m_tgRank = new Toggle[2];

	private Label m_lbdayCount;

	private NewListBox m_nlbRankList;

	private Label m_lbMyRank;

	private Label m_lbMyCharName;

	private Label m_lbMyLevel;

	private Label m_lbMyWinnig;

	private Button m_btPractice;

	private Button m_btPrev;

	private Button m_btPageSet;

	private Label m_lbPage;

	private Button m_btNext;

	private Button m_btPage;

	private Button m_btClose;

	private List<INFIBATTLE_RANK_INFO> m_RankInfoList = new List<INFIBATTLE_RANK_INFO>();

	private string m_strRank = string.Empty;

	private string m_strCharName = string.Empty;

	private string m_strLevel = string.Empty;

	private string m_strWinStreak = string.Empty;

	public InfiBattleDefine.eINFIBATTLE_RANKMODE m_eRankMode;

	public int m_iCurrentPage = 1;

	public int m_iMaxPage = 1;

	private long m_GetRankTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Plunder/InfiBattle/dlg_infinite_rank", G_ID.INFIBATTLE_RANK_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 2; i++)
		{
			int num = i + 1;
			this.m_tgRank[i] = (base.GetControl("Toggle_tab" + num) as Toggle);
			this.m_tgRank[i].data = i;
			this.m_tgRank[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickTabControl));
		}
		this.m_lbdayCount = (base.GetControl("Label_Label29") as Label);
		this.m_nlbRankList = (base.GetControl("NewListBox_infibattle_rank_list") as NewListBox);
		this.m_nlbRankList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRankList));
		this.m_nlbRankList.Reserve = false;
		this.m_btPractice = (base.GetControl("Button_practice") as Button);
		this.m_btPractice.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPractice));
		this.m_btPrev = (base.GetControl("BT_Back") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_btPage = (base.GetControl("Button_page") as Button);
		this.m_btPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPage));
		this.m_lbPage = (base.GetControl("Label_page2") as Label);
		this.m_lbPage.SetText(string.Empty);
		this.m_btNext = (base.GetControl("BT_Next") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_btClose = (base.GetControl("Button_close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.m_btPageSet = (base.GetControl("BT_Set") as Button);
		this.m_btPageSet.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickInputRank));
		this.m_lbMyRank = (base.GetControl("Label_Rank") as Label);
		this.m_lbMyCharName = (base.GetControl("Label_charname") as Label);
		this.m_lbMyLevel = (base.GetControl("Label_level") as Label);
		this.m_lbMyWinnig = (base.GetControl("Label_winning_streak") as Label);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	private void ClickTabControl(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		if (null == toggle)
		{
			return;
		}
		long curTime = PublicMethod.GetCurTime();
		byte b = (byte)((int)toggle.data);
		if (curTime > this.m_GetRankTime + 1L)
		{
			if (toggle.GetToggleState())
			{
				if (0 > b || 1 < b)
				{
					return;
				}
				this.GetRank(b, 1);
			}
		}
		else
		{
			toggle.Value = false;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	private void ClickRankList(IUIObject obj)
	{
		if (null == this.m_nlbRankList || null == this.m_nlbRankList.SelectedItem)
		{
			return;
		}
		if (this.m_nlbRankList.SelectedItem.Data != null)
		{
			long num = (long)this.m_nlbRankList.SelectedItem.Data;
			TsLog.LogWarning("!!!!!!! {0}", new object[]
			{
				num
			});
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (num != 0L && charPersonInfo != null)
			{
				for (int i = 0; i < this.m_RankInfoList.Count; i++)
				{
					if (this.m_RankInfoList[i].i64PersonID == num)
					{
						charPersonInfo.InfiBattlePersonID = this.m_RankInfoList[i].i64PersonID;
						charPersonInfo.InfiBattleRank = this.m_RankInfoList[i].i32Rank;
						return;
					}
				}
			}
		}
	}

	private void ClickPractice(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo.InfiBattlePersonID != myCharInfo.m_PersonID && charPersonInfo.InfiBattlePersonID > 0L)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "REMIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE;
				FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
			}
		}
	}

	private void ClickPrev(IUIObject obj)
	{
		if (this.m_iCurrentPage > 1)
		{
			int num = (this.m_iCurrentPage - 2) * 10 + 1;
			TsLog.LogWarning(" ClickPrev {0}", new object[]
			{
				num
			});
			this.GetRank((byte)this.m_eRankMode, num);
		}
	}

	private void ClickPage(IUIObject obj)
	{
		int num = int.Parse(this.m_btPageSet.GetText());
		if (num == this.m_iCurrentPage)
		{
			return;
		}
		int num2 = (num - 1) * 10 + 1;
		TsLog.LogWarning(" Click Page {0}", new object[]
		{
			num2
		});
		this.GetRank((byte)this.m_eRankMode, num2);
	}

	private void ClickNext(IUIObject obj)
	{
		if (this.m_iMaxPage > this.m_iCurrentPage)
		{
			int num = this.m_iCurrentPage * 10 + 1;
			TsLog.LogWarning(" ClickNext {0}", new object[]
			{
				num
			});
			this.GetRank((byte)this.m_eRankMode, num);
		}
	}

	private void ClickClose(IUIObject obj)
	{
		base.CloseNow();
	}

	public void AddRankInfo(INFIBATTLE_RANK_INFO RankInfo)
	{
		this.m_RankInfoList.Add(RankInfo);
	}

	public void InitRankInfo()
	{
		this.m_RankInfoList.Clear();
	}

	public void ShowRank(GS_INFIBATTLE_RANK_ACK ACK)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			charPersonInfo.InfiBattlePersonID = 0L;
			charPersonInfo.InfiBattleRank = 0;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		this.m_eRankMode = (InfiBattleDefine.eINFIBATTLE_RANKMODE)ACK.ui8Type;
		this.m_nlbRankList.Clear();
		for (int i = 0; i < this.m_RankInfoList.Count; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbRankList.ColumnNum, true);
			this.m_strCharName = TKString.NEWString(this.m_RankInfoList[i].strName);
			this.m_strLevel = this.m_RankInfoList[i].i16Level.ToString();
			this.m_strWinStreak = this.m_RankInfoList[i].i32StraightWin.ToString();
			newListItem.SetListItemData(0, this.m_strCharName, null, null, null);
			newListItem.SetListItemData(1, this.m_strLevel, null, null, null);
			if (this.m_RankInfoList[i].i32Rank != 0)
			{
				this.m_strRank = this.m_RankInfoList[i].i32Rank.ToString();
			}
			else
			{
				this.m_strRank = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("48");
			}
			newListItem.SetListItemData(2, this.m_strRank, null, null, null);
			newListItem.SetListItemData(3, this.m_strWinStreak, null, null, null);
			newListItem.Data = this.m_RankInfoList[i].i64PersonID;
			this.m_nlbRankList.Add(newListItem);
		}
		this.m_nlbRankList.RepositionItems();
		if (ACK.ui8Type == 1)
		{
			int index = this.m_RankInfoList.Count;
			NewListItem newListItem2 = new NewListItem(this.m_nlbRankList.ColumnNum, true);
			if (charPersonInfo != null)
			{
				this.m_strCharName = charPersonInfo.GetCharName();
				this.m_strLevel = myCharInfo.GetLevel().ToString();
				this.m_strWinStreak = myCharInfo.InfiBattleStraightWin.ToString();
				this.m_strRank = myCharInfo.InfinityBattle_Rank.ToString();
				if (myCharInfo.InfinityBattle_Rank > 0)
				{
					for (int i = 0; i < this.m_RankInfoList.Count; i++)
					{
						if (this.m_RankInfoList[i].i32Rank > myCharInfo.InfinityBattle_Rank)
						{
							index = i;
							break;
						}
						if (this.m_RankInfoList[i].i32Rank == 0)
						{
							index = i;
							break;
						}
					}
				}
				else
				{
					this.m_strRank = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("48");
					for (int i = 0; i < this.m_RankInfoList.Count; i++)
					{
						if (this.m_RankInfoList[i].i32Rank == myCharInfo.InfinityBattle_Rank)
						{
							index = i;
							break;
						}
					}
				}
			}
			else
			{
				this.m_strCharName = string.Empty;
				this.m_strLevel = "0";
				this.m_strWinStreak = "0";
				this.m_strRank = "0";
			}
			newListItem2.SetListItemData(0, this.m_strCharName, null, null, null);
			newListItem2.SetListItemData(1, this.m_strLevel, null, null, null);
			newListItem2.SetListItemData(2, this.m_strRank, null, null, null);
			newListItem2.SetListItemData(3, this.m_strWinStreak, null, null, null);
			newListItem2.Data = charPersonInfo.GetPersonID();
			this.m_nlbRankList.InsertAdd(index, newListItem2);
			this.m_nlbRankList.RepositionItems();
		}
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2226");
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
			num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCount;
			num3 = num - (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(21);
			if (num3 < 0)
			{
				num3 = 0;
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			num3,
			"maxcount",
			num
		});
		this.m_lbdayCount.SetText(text2);
		if (num2 < myCharInfo.InfinityBattle_Rank || 0 >= myCharInfo.InfinityBattle_Rank)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				myCharInfo.InfinityBattle_Rank
			});
		}
		this.m_lbMyRank.SetText(text2);
		if (charPersonInfo != null)
		{
			this.m_lbMyCharName.SetText(charPersonInfo.GetCharName());
		}
		this.m_iCurrentPage = ACK.i32StartRank / 10 + 1;
		this.m_iMaxPage = ACK.i32MaxRankCount / 10 + 1;
		this.m_lbMyLevel.SetText(myCharInfo.GetLevel().ToString());
		this.m_lbMyWinnig.SetText(myCharInfo.InfiBattleStraightWin.ToString());
		string text3 = "/ " + this.m_iMaxPage.ToString();
		this.m_lbPage.SetText(text3);
		this.m_btPageSet.SetText(this.m_iCurrentPage.ToString());
	}

	public void BtClickInputRank(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_Input_MinLevel), null, new Action<InputNumberDlg, object>(this.On_Close_InputNumber), null);
		inputNumberDlg.SetMinMax(1L, (long)this.m_iMaxPage);
		inputNumberDlg.SetNum((long)this.m_iCurrentPage);
	}

	private void On_Input_MinLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		if (num < 1)
		{
			num = 1;
		}
		this.m_btPageSet.SetText(num.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void On_Close_InputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public override void Show()
	{
		base.Show();
	}

	public void GetRank(byte bType, int i32StartRank)
	{
		GS_INFIBATTLE_RANK_REQ gS_INFIBATTLE_RANK_REQ = new GS_INFIBATTLE_RANK_REQ();
		gS_INFIBATTLE_RANK_REQ.i8Type = bType;
		gS_INFIBATTLE_RANK_REQ.i32StartRank = i32StartRank;
		gS_INFIBATTLE_RANK_REQ.i32RankCount = 10;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_RANK_REQ, gS_INFIBATTLE_RANK_REQ);
		this.m_GetRankTime = PublicMethod.GetCurTime();
	}
}
