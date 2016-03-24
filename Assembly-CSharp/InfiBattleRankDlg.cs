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
	private Toggle[] m_tgRank = new Toggle[3];

	private NewListBox m_nlbRankList;

	private DrawTexture m_DrawTextureNotice;

	private DrawTexture m_DTMyRankIcon;

	private Label m_lbMyRank;

	private Label m_lbMyCharName;

	private Label m_lbMyLevel;

	private Label m_lbMyWinnig;

	private Label m_lbMyWinningStreak;

	private Button m_btPractice;

	private Button m_btPrev;

	private Button m_btPageSet;

	private Label m_lbPage;

	private Button m_btNext;

	private Button m_btClose;

	private Button m_btRewardGet;

	private Button m_btRewardInfo;

	private List<INFIBATTLE_RANK_INFO> m_RankInfoList = new List<INFIBATTLE_RANK_INFO>();

	private string m_strRank = string.Empty;

	private string m_strCharName = string.Empty;

	private string m_strLevel = string.Empty;

	public InfiBattleDefine.eINFIBATTLE_RANKMODE m_eRankMode = InfiBattleDefine.eINFIBATTLE_RANKMODE.eINFIBATTLE_RANKMODE_TOTAL;

	public int m_iCurrentPage = 1;

	public int m_iMaxPage = 1;

	private long m_GetRankTime;

	private InfiBattleDefine.eINFIBATTLE_RANKMODE m_ShowType;

	private NrMyCharInfo m_pkMyCharInfo;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Plunder/InfiBattle/dlg_infinite_rank", G_ID.INFIBATTLE_RANK_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			this.m_tgRank[i] = (base.GetControl("Toggle_tab" + num) as Toggle);
			this.m_tgRank[i].data = i;
			this.m_tgRank[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickTabControl));
		}
		this.m_pkMyCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		this.m_nlbRankList = (base.GetControl("NewListBox_infibattle_rank_list") as NewListBox);
		this.m_nlbRankList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRankList));
		this.m_nlbRankList.Reserve = false;
		this.m_btPractice = (base.GetControl("Button_practice") as Button);
		this.m_btPractice.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPractice));
		this.m_btPractice.SetEnabled(false);
		this.m_btPrev = (base.GetControl("BT_Back") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
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
		this.m_lbMyWinningStreak = (base.GetControl("Label_winning_streak") as Label);
		this.m_DrawTextureNotice = (base.GetControl("DT_Notice") as DrawTexture);
		this.m_DrawTextureNotice.Visible = false;
		this.m_btRewardGet = (base.GetControl("BT_RewardGet") as Button);
		this.m_btRewardGet.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickRewardGet));
		this.m_btRewardInfo = (base.GetControl("BT_rewardinfo") as Button);
		this.m_btRewardInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRewardInfo));
		this.m_DTMyRankIcon = (base.GetControl("DT_RankIcon") as DrawTexture);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.SelectTab();
		this.SetRewardTexutre();
	}

	private void ClickTabControl(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		if (null == toggle)
		{
			return;
		}
		if (!toggle.Value)
		{
			return;
		}
		long curTime = PublicMethod.GetCurTime();
		if (curTime > this.m_GetRankTime + 1L)
		{
			if (toggle.GetToggleState())
			{
				this.m_ShowType = (InfiBattleDefine.eINFIBATTLE_RANKMODE)((int)toggle.data);
				this.SelectTab();
			}
		}
		else
		{
			toggle.Value = false;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void SelectTab()
	{
		InfiBattleDefine.eINFIBATTLE_RANKMODE showType = this.m_ShowType;
		if (showType != InfiBattleDefine.eINFIBATTLE_RANKMODE.eINFIBATTLE_RANKMODE_LASTWEEK)
		{
			this.GetRank((byte)this.m_ShowType - 1, 1);
		}
		else
		{
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
				{
					this.m_tgRank[i].SetToggleState(true);
				}
				else
				{
					this.m_tgRank[i].SetToggleState(false);
				}
			}
			GS_INFIBATTLE_REWARDINFO_REQ gS_INFIBATTLE_REWARDINFO_REQ = new GS_INFIBATTLE_REWARDINFO_REQ();
			gS_INFIBATTLE_REWARDINFO_REQ.i64PersonID = this.m_pkMyCharInfo.m_PersonID;
			SendPacket.GetInstance().SendObject(2011, gS_INFIBATTLE_REWARDINFO_REQ);
			this.m_GetRankTime = PublicMethod.GetCurTime();
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
				this.m_btPractice.SetEnabled(true);
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
		if (this.m_ShowType == InfiBattleDefine.eINFIBATTLE_RANKMODE.eINFIBATTLE_RANKMODE_LASTWEEK)
		{
			return;
		}
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

	private void ClickRewardInfo(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			InfiBattleReward infiBattleReward = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward;
			if (infiBattleReward != null)
			{
				GS_INFIBATTLE_GET_REWARDINFO_REQ gS_INFIBATTLE_GET_REWARDINFO_REQ = new GS_INFIBATTLE_GET_REWARDINFO_REQ();
				gS_INFIBATTLE_GET_REWARDINFO_REQ.i64PersonID = myCharInfo.m_PersonID;
				SendPacket.GetInstance().SendObject(2013, gS_INFIBATTLE_GET_REWARDINFO_REQ);
				this.Close();
			}
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
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < this.m_RankInfoList.Count; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbRankList.ColumnNum, true, string.Empty);
			this.m_strCharName = TKString.NEWString(this.m_RankInfoList[i].strName);
			this.m_strLevel = this.m_RankInfoList[i].i16Level.ToString();
			newListItem.SetListItemData(0, this.m_strCharName, null, null, null);
			newListItem.SetListItemData(1, this.m_strLevel, null, null, null);
			if (this.m_RankInfoList[i].i32Rank != 0)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3202");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strRank, new object[]
				{
					text,
					"rank",
					this.m_RankInfoList[i].i32Rank
				});
				if (this.m_RankInfoList[i].i32Rank == 1)
				{
					this.m_strRank = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + this.m_strRank;
				}
				else if (this.m_RankInfoList[i].i32Rank <= 10)
				{
					this.m_strRank = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + this.m_strRank;
				}
				newListItem.SetListItemData(5, this.GetRankImg(this.m_RankInfoList[i].i32Rank), null, null, null);
			}
			else
			{
				this.m_strRank = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("48");
			}
			newListItem.SetListItemData(3, this.m_strRank, null, null, null);
			if (this.m_RankInfoList[i].i32BattleTotalCount > 0)
			{
				float num = (float)this.m_RankInfoList[i].i32BattleWinCount / (float)this.m_RankInfoList[i].i32BattleTotalCount;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"Count",
					(int)(num * 100f)
				});
			}
			else
			{
				text2 = string.Empty;
			}
			newListItem.SetListItemData(4, text2, null, null, null);
			newListItem.Data = this.m_RankInfoList[i].i64PersonID;
			this.m_nlbRankList.Add(newListItem);
		}
		this.m_nlbRankList.RepositionItems();
		if (ACK.ui8Type == 2)
		{
			int index = this.m_RankInfoList.Count;
			NewListItem newListItem2 = new NewListItem(this.m_nlbRankList.ColumnNum, true, string.Empty);
			if (charPersonInfo != null)
			{
				this.m_strCharName = charPersonInfo.GetCharName();
				this.m_strLevel = myCharInfo.GetLevel().ToString();
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
						newListItem2.SetListItemData(5, this.GetRankImg(this.m_RankInfoList[i].i32Rank), null, null, null);
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
				this.m_strRank = "0";
			}
			newListItem2.SetListItemData(0, this.m_strCharName, null, null, null);
			newListItem2.SetListItemData(1, this.m_strLevel, null, null, null);
			newListItem2.SetListItemData(3, this.m_strRank, null, null, null);
			if (myCharInfo.InifBattle_TotalCount > 0)
			{
				float num = (float)myCharInfo.InifBattle_WinCount / (float)myCharInfo.InifBattle_TotalCount;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"Count",
					(int)(num * 100f)
				});
			}
			else
			{
				text2 = string.Empty;
			}
			newListItem2.SetListItemData(4, text2, null, null, null);
			newListItem2.Data = charPersonInfo.GetPersonID();
			this.m_nlbRankList.InsertAdd(index, newListItem2);
			this.m_nlbRankList.RepositionItems();
		}
		int num2 = 0;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
		}
		if (num2 < myCharInfo.InfinityBattle_Rank || 0 >= myCharInfo.InfinityBattle_Rank)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3202");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				myCharInfo.InfinityBattle_Rank
			});
			if (myCharInfo.InfinityBattle_Rank == 1)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text2;
			}
			else if (myCharInfo.InfinityBattle_Rank <= 10)
			{
				text2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text2;
			}
		}
		this.m_DTMyRankIcon.SetTexture(this.GetRankImg(myCharInfo.InfinityBattle_Rank));
		this.m_lbMyRank.SetText(text2);
		if (charPersonInfo != null)
		{
			this.m_lbMyCharName.SetText(charPersonInfo.GetCharName());
		}
		this.m_iCurrentPage = ACK.i32StartRank / 10 + 1;
		this.m_iMaxPage = ACK.i32MaxRankCount / 10 + 1;
		this.m_lbMyLevel.SetText(myCharInfo.GetLevel().ToString());
		this.m_lbMyWinnig.SetText(myCharInfo.InfiBattleStraightWin.ToString());
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			"0"
		});
		if (myCharInfo.InifBattle_TotalCount > 0)
		{
			float num = (float)myCharInfo.InifBattle_WinCount / (float)myCharInfo.InifBattle_TotalCount;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"Count",
				(int)(num * 100f)
			});
		}
		else
		{
			text2 = string.Empty;
		}
		this.m_lbMyWinningStreak.SetText(text2);
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
		int num2 = int.Parse(this.m_btPageSet.GetText());
		if (num2 == this.m_iCurrentPage)
		{
			return;
		}
		int num3 = (num2 - 1) * 10 + 1;
		TsLog.LogWarning(" Click Page {0}", new object[]
		{
			num3
		});
		this.GetRank((byte)this.m_eRankMode, num3);
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
		this.m_btPractice.Visible = true;
		this.m_btPractice.SetEnabled(false);
		GS_INFIBATTLE_RANK_REQ gS_INFIBATTLE_RANK_REQ = new GS_INFIBATTLE_RANK_REQ();
		gS_INFIBATTLE_RANK_REQ.i8Type = bType;
		gS_INFIBATTLE_RANK_REQ.i32StartRank = i32StartRank;
		gS_INFIBATTLE_RANK_REQ.i32RankCount = 10;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_RANK_REQ, gS_INFIBATTLE_RANK_REQ);
		this.m_GetRankTime = PublicMethod.GetCurTime();
	}

	public UIBaseInfoLoader GetRankImg(int i32Rank)
	{
		string key = string.Empty;
		if (i32Rank == 1)
		{
			key = "Win_I_Rank03";
		}
		else if (i32Rank == 2)
		{
			key = "Win_I_Rank02";
		}
		else if (i32Rank == 3)
		{
			key = "Win_I_Rank01";
		}
		else
		{
			key = string.Empty;
		}
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public void SetTopRankStart()
	{
		this.m_nlbRankList.Clear();
	}

	public void SetTopRankEnd()
	{
		this.m_nlbRankList.RepositionItems();
		string text = "/ " + 1;
		this.m_lbPage.SetText(text);
		this.m_btPageSet.SetText(1.ToString());
	}

	public void SetTopRank(int iCount, int i32Rank, string szCharName, int i32CharLevel, int i32BattleCnt, int i32WinCnt)
	{
		string text = string.Empty;
		string empty = string.Empty;
		NewListItem newListItem = new NewListItem(this.m_nlbRankList.ColumnNum, true, string.Empty);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
			"charname",
			szCharName
		});
		newListItem.SetListItemData(0, text, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			i32CharLevel.ToString()
		});
		newListItem.SetListItemData(1, text, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3202"),
			"rank",
			i32Rank
		});
		if (i32Rank == 1)
		{
			text = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text;
		}
		else if (i32Rank <= 10)
		{
			text = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text;
		}
		newListItem.SetListItemData(3, text, null, null, null);
		if (i32BattleCnt > 0)
		{
			float num = (float)i32WinCnt / (float)i32BattleCnt;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"Count",
				(int)(num * 100f)
			});
		}
		else
		{
			empty = string.Empty;
		}
		newListItem.SetListItemData(4, empty, null, null, null);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count",
			"0"
		});
		if (i32Rank > 0 && i32Rank < 4)
		{
			newListItem.SetListItemData(5, this.GetRankImg(i32Rank), null, null, null);
		}
		this.m_nlbRankList.Add(newListItem);
	}

	public void SetMyOldRankInfo(int i32Rank, string szCharName, int i32CharLevel, int i32BattleCnt, int i32WinCnt)
	{
		int num = 0;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (num < i32Rank || 0 >= i32Rank)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		else
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3202");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text2,
				"rank",
				i32Rank
			});
			if (i32Rank == 1)
			{
				text = NrTSingleton<CTextParser>.Instance.GetTextColor("1403") + text;
			}
			else if (i32Rank <= 10)
			{
				text = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + text;
			}
		}
		this.m_lbMyRank.SetText(text);
		if (szCharName.Length > 0)
		{
			this.m_lbMyCharName.SetText(szCharName);
		}
		else
		{
			this.m_lbMyCharName.SetText(string.Empty);
		}
		if (i32CharLevel > 0)
		{
			this.m_lbMyLevel.SetText(i32CharLevel.ToString());
		}
		else
		{
			this.m_lbMyLevel.SetText(string.Empty);
		}
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			text2,
			"count",
			"0"
		});
		if (i32Rank > 0 && i32Rank < 4)
		{
			this.m_DTMyRankIcon.SetTexture(this.GetRankImg(i32Rank));
		}
		else
		{
			this.m_DTMyRankIcon.SetTexture(string.Empty);
		}
		if (i32BattleCnt > 0)
		{
			float num2 = (float)i32WinCnt / (float)i32BattleCnt;
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text2,
				"Count",
				(int)(num2 * 100f)
			});
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		this.m_lbMyWinningStreak.SetText(text);
	}

	public void On_ClickRewardGet(IUIObject a_cObject)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			GS_INFIBATTLE_GETREWARD_REQ gS_INFIBATTLE_GETREWARD_REQ = new GS_INFIBATTLE_GETREWARD_REQ();
			gS_INFIBATTLE_GETREWARD_REQ.i64PersonID = myCharInfo.m_PersonID;
			SendPacket.GetInstance().SendObject(2015, gS_INFIBATTLE_GETREWARD_REQ);
		}
	}

	public void SetRewardTexutre()
	{
		bool flag = false;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleReward == 0)
		{
			flag = true;
		}
		this.m_btRewardGet.SetEnabled(flag);
		this.m_DrawTextureNotice.Visible = flag;
	}
}
