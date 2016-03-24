using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class ColosseumRankInfoDlg : Form
{
	private enum eTAB
	{
		eTAB_MYLEAGUERANK,
		eTAB_TOPLEAGUERANK,
		eTAB_SECONDLEAGUERANK,
		eTAB_MAX
	}

	private Toggle[] m_tgRankList = new Toggle[3];

	private DrawTexture m_dtMyrankBG;

	private DrawTexture m_dtMyRankBG1;

	private DrawTexture m_dtMyRankBG2;

	private DrawTexture m_dtBronzeExplain;

	private Button m_btClose;

	private Label m_laBronzeExplain1;

	private Label m_laBronzeExplain2;

	private Label m_laBronzeWinCount;

	private Label m_laTargetRank;

	private Label m_laTargetCharName;

	private Label m_laMatchPoint;

	private NewListBox m_lbMyRankList;

	private NewListBox m_lbTopRankList;

	private Button m_bReward;

	private Button m_btRewardInfo;

	private Button m_btRecord;

	private List<COLOSSEUM_RANKINFO> ListRankInfo = new List<COLOSSEUM_RANKINFO>();

	private long[] m_i64TopGuildID = new long[3];

	private eCOLOSSEUMRANK_SHOWTYPE show_type;

	private bool[] bReqInfo = new bool[3];

	private Button m_bJoin;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_Rank", G_ID.COLOSSEUMRANKINFO_DLG, true);
		base.SetScreenCenter();
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_dtMyrankBG = (base.GetControl("DrawTexture_DrawTexture24") as DrawTexture);
		this.m_dtMyRankBG1 = (base.GetControl("DrawTexture_DrawTexture21") as DrawTexture);
		this.m_dtMyRankBG2 = (base.GetControl("DrawTexture_DrawTexture23") as DrawTexture);
		this.m_dtBronzeExplain = (base.GetControl("DT_Black") as DrawTexture);
		this.m_dtBronzeExplain.Visible = false;
		this.m_laTargetRank = (base.GetControl("Label_Rank") as Label);
		this.m_laTargetCharName = (base.GetControl("Label_charname") as Label);
		this.m_laMatchPoint = (base.GetControl("Label_MatchPoint") as Label);
		this.m_btClose = (base.GetControl("Button_close") as Button);
		Button expr_C2 = this.m_btClose;
		expr_C2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C2.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_laBronzeExplain1 = (base.GetControl("LB_Bronze1") as Label);
		this.m_laBronzeExplain1.Visible = false;
		this.m_laBronzeExplain2 = (base.GetControl("LB_Bronze2") as Label);
		this.m_laBronzeExplain2.Visible = false;
		this.m_laBronzeWinCount = (base.GetControl("LB_WinCount") as Label);
		this.m_laBronzeWinCount.Visible = false;
		this.m_btRecord = (base.GetControl("Button_Record") as Button);
		Button expr_165 = this.m_btRecord;
		expr_165.Click = (EZValueChangedDelegate)Delegate.Combine(expr_165.Click, new EZValueChangedDelegate(this.OnClickShowRecord));
		this.m_lbMyRankList = (base.GetControl("NewListBox_rank_list") as NewListBox);
		this.m_lbTopRankList = (base.GetControl("NewListBox_rank_list2") as NewListBox);
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			this.m_tgRankList[i] = (base.GetControl("Toggle_tab" + num) as Toggle);
			this.m_tgRankList[i].data = i;
			this.m_tgRankList[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
			this.bReqInfo[i] = false;
		}
		this.m_bReward = (base.GetControl("Button_RankReward") as Button);
		Button expr_23C = this.m_bReward;
		expr_23C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_23C.Click, new EZValueChangedDelegate(this.OnClickReward));
		this.m_btRewardInfo = (base.GetControl("BT_rewardinfo") as Button);
		this.m_btRewardInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickRewardinfo));
		this.m_bJoin = (base.GetControl("Button_entry") as Button);
		Button expr_2A6 = this.m_bJoin;
		expr_2A6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2A6.Click, new EZValueChangedDelegate(this.OnClickJoin));
		this.m_bJoin.Visible = false;
		if (COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TOURNAMENT_JOIN) == 1)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo.ColosseumGrade == 5)
			{
				this.m_bJoin.Visible = true;
				DateTime nowTime = PublicMethod.GetNowTime();
				if (nowTime.DayOfWeek == DayOfWeek.Monday || nowTime.DayOfWeek == DayOfWeek.Tuesday || nowTime.DayOfWeek == DayOfWeek.Wednesday)
				{
					this.m_bJoin.Visible = false;
				}
				if (nowTime.DayOfWeek == DayOfWeek.Thursday && nowTime.Hour < 7)
				{
					this.m_bJoin.Visible = false;
				}
			}
			else
			{
				this.m_bJoin.Visible = false;
			}
		}
		else
		{
			this.m_bJoin.Visible = false;
		}
	}

	public void OnClickJoin(IUIObject obj)
	{
		GS_TOURNAMENT_JOIN_REQ gS_TOURNAMENT_JOIN_REQ = new GS_TOURNAMENT_JOIN_REQ();
		gS_TOURNAMENT_JOIN_REQ.nType = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_JOIN_REQ, gS_TOURNAMENT_JOIN_REQ);
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2653"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2654"), eMsgType.MB_OK, 2);
		}
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void AddRankInfo(COLOSSEUM_RANKINFO info)
	{
		this.ListRankInfo.Add(info);
	}

	public void SetTopRankGuild(long[] info)
	{
		this.m_i64TopGuildID = info;
	}

	public void InitUIControl()
	{
		this.m_dtBronzeExplain.Visible = false;
		this.m_laBronzeExplain1.Visible = false;
		this.m_laBronzeExplain2.Visible = false;
		this.m_laBronzeWinCount.Visible = false;
	}

	public void ShowList()
	{
		this.InitUIControl();
		if (this.show_type == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_MYLEAGUERANK)
		{
			this.ShowMyGradeList();
		}
		else
		{
			this.ShowTopGradeList();
		}
	}

	public void ShowMyGradeList()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		this.m_lbMyRankList.Clear();
		if (kMyCharInfo.ColosseumGrade == 0)
		{
			this.m_dtBronzeExplain.Visible = true;
			this.m_laBronzeExplain1.Visible = true;
			this.m_laBronzeExplain2.Visible = true;
			this.m_laBronzeWinCount.Visible = true;
			string text = string.Empty;
			string empty = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2208");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count1",
				kMyCharInfo.ColosseumWinCount,
				"count2",
				COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_BRONZE_UPGRADE_WINCOUNT)
			});
			this.m_laBronzeWinCount.SetText(empty);
		}
		else
		{
			List<COLOSSEUM_MYGRADE_USERINFO> list = kMyCharInfo.GeColosseumMyGradeUserList();
			if (list == null)
			{
				return;
			}
			int num = 1;
			foreach (COLOSSEUM_MYGRADE_USERINFO current in list)
			{
				NewListItem item;
				if (kMyCharInfo.GetColosseumMyGradeRank() == num)
				{
					item = this.SetColosseumListInfo(new COLOSSEUM_MYGRADE_USERINFO
					{
						i64PersonID = charPersonInfo.GetPersonID(),
						szCharName = TKString.StringChar(charPersonInfo.GetCharName()),
						i32ColosseumGradePoint = kMyCharInfo.ColosseumGradePoint
					}, num);
				}
				else
				{
					item = this.SetColosseumListInfo(current, num);
				}
				this.m_lbMyRankList.Add(item);
				num++;
			}
			this.m_lbMyRankList.RepositionItems();
		}
	}

	public NewListItem SetColosseumListInfo(COLOSSEUM_MYGRADE_USERINFO info, int Rank)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		List<COLOSSEUM_MYGRADE_USERINFO> list = kMyCharInfo.GeColosseumMyGradeUserList();
		NewListItem newListItem = new NewListItem(this.m_lbMyRankList.ColumnNum, true, string.Empty);
		string text3 = TKString.NEWString(info.szCharName);
		newListItem.SetListItemData(0, text3, null, null, null);
		text2 = Rank.ToString();
		newListItem.SetListItemData(2, text2, null, null, null);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"ratingpoint",
			1000 + info.i32ColosseumGradePoint
		});
		newListItem.SetListItemData(3, text2, null, null, null);
		int value = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_UPGRADE_RATE);
		int value2 = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_DOWNGRADE_RATE);
		int num = 0;
		bool flag = true;
		int num2 = 0;
		bool flag2 = true;
		if (list.Count > 0)
		{
			num = list.Count * value / 100;
			num2 = list.Count - list.Count * value2 / 100;
		}
		if (kMyCharInfo.ColosseumGrade == 5)
		{
			flag = false;
			flag2 = false;
		}
		else if (kMyCharInfo.ColosseumGrade == 4)
		{
			flag2 = false;
		}
		string text4 = string.Empty;
		if (num >= Rank && flag)
		{
			text4 = "Win_I_ArrowUp";
		}
		else if (num2 < Rank && flag2)
		{
			text4 = "Win_I_ArrowDown";
		}
		if (text4 != string.Empty)
		{
			newListItem.SetListItemData(4, text4, null, null, null);
		}
		return newListItem;
	}

	public void ShowTopGradeList()
	{
		this.SetListTotalRank();
		this.m_lbTopRankList.Clear();
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 1;
		foreach (COLOSSEUM_RANKINFO current in this.ListRankInfo)
		{
			NewListItem newListItem = new NewListItem(this.m_lbTopRankList.ColumnNum, true, string.Empty);
			string text3 = TKString.NEWString(current.szCharName);
			newListItem.SetListItemData(0, text3, null, null, null);
			text2 = num.ToString();
			newListItem.SetListItemData(2, text2, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"ratingpoint",
				1000L + current.i64MatchPoint
			});
			newListItem.SetListItemData(3, text2, null, null, null);
			string text4 = string.Empty;
			string[] array = TKString.NEWString(current.szGuildName).Split(new char[]
			{
				'_'
			});
			text4 = array[0];
			if (array.Length > 1)
			{
				text4 = NrTSingleton<CTextParser>.Instance.GetTextColor("1401") + text4;
			}
			newListItem.SetListItemData(5, text4, null, null, null);
			if (text4 != string.Empty)
			{
				string topGuildRank = this.GetTopGuildRank(current.i64GuildID);
				if (topGuildRank == string.Empty)
				{
					newListItem.SetListItemData(6, false);
				}
				else
				{
					newListItem.SetListItemData(6, true);
					newListItem.SetListItemData(6, topGuildRank, null, null, null);
				}
			}
			this.m_lbTopRankList.Add(newListItem);
			num++;
		}
		this.m_lbTopRankList.RepositionItems();
	}

	public void ShowInfo(eCOLOSSEUMRANK_SHOWTYPE rank_showtype, int page)
	{
		this.SetListInfo(rank_showtype);
		this.ShowMyinfo();
		this.ShowList();
		if (!base.Visible)
		{
			this.Show();
		}
	}

	public void SetListInfo(eCOLOSSEUMRANK_SHOWTYPE rank_showtype)
	{
		this.show_type = rank_showtype;
		if (rank_showtype == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_MYLEAGUERANK)
		{
			base.ShowLayer(1, 2);
			base.SetShowLayer(4, false);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			int colosseumOldRank = kMyCharInfo.ColosseumOldRank;
			if (colosseumOldRank <= 0)
			{
				base.SetShowLayer(3, false);
			}
			else
			{
				base.SetShowLayer(3, true);
			}
		}
		else if (rank_showtype == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_ROTALLEAGUERANK || rank_showtype == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_TOPSCORERANK)
		{
			base.ShowLayer(4);
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, false);
		}
	}

	public void ShowMyinfo()
	{
		string text = string.Empty;
		string empty = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (this.show_type == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_ROTALLEAGUERANK || this.show_type == eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_TOPSCORERANK)
		{
			this.m_laTargetRank.Text = " ";
			this.m_laTargetCharName.Text = " ";
			this.m_laMatchPoint.Text = " ";
			this.m_dtMyrankBG.Visible = false;
			this.m_dtMyRankBG1.Visible = false;
			this.m_dtMyRankBG2.Visible = false;
		}
		else if (kMyCharInfo.ColosseumGrade == 0)
		{
			this.m_laTargetRank.Text = " ";
			this.m_laTargetCharName.Text = " ";
			this.m_laMatchPoint.Text = " ";
			this.m_dtMyrankBG.Visible = false;
			this.m_dtMyRankBG1.Visible = false;
			this.m_dtMyRankBG2.Visible = false;
		}
		else
		{
			this.m_dtMyrankBG.Visible = true;
			this.m_dtMyRankBG1.Visible = true;
			this.m_dtMyRankBG2.Visible = true;
			this.m_laTargetRank.Text = kMyCharInfo.GetColosseumMyGradeRank().ToString();
			this.m_laTargetCharName.Text = charPersonInfo.GetCharName();
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"ratingpoint",
				1000 + kMyCharInfo.ColosseumGradePoint
			});
			this.m_laMatchPoint.Text = empty;
		}
	}

	public void SetListTotalRank()
	{
		this.ListRankInfo.Sort(new Comparison<COLOSSEUM_RANKINFO>(ColosseumRankInfoDlg.CompareGradePoint));
	}

	private static int CompareGradePoint(COLOSSEUM_RANKINFO x, COLOSSEUM_RANKINFO y)
	{
		if (x.i64MatchPoint > y.i64MatchPoint)
		{
			return -1;
		}
		return 1;
	}

	public void InitColosseumRankData()
	{
		this.ListRankInfo.Clear();
		this.m_lbMyRankList.Clear();
	}

	private void OnClickTabControl(IUIObject obj)
	{
		if (this.m_tgRankList[0].GetToggleState())
		{
			if (this.bReqInfo[0])
			{
				return;
			}
			this.bReqInfo[0] = true;
			this.bReqInfo[1] = false;
			this.bReqInfo[2] = false;
		}
		else if (this.m_tgRankList[1].GetToggleState())
		{
			if (this.bReqInfo[1])
			{
				return;
			}
			this.bReqInfo[0] = false;
			this.bReqInfo[1] = true;
			this.bReqInfo[2] = false;
		}
		else if (this.m_tgRankList[2].GetToggleState())
		{
			if (this.bReqInfo[2])
			{
				return;
			}
			this.bReqInfo[0] = false;
			this.bReqInfo[1] = false;
			this.bReqInfo[2] = true;
		}
		if (this.bReqInfo[0])
		{
			this.ShowInfo(eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_MYLEAGUERANK, 1);
		}
		else if (this.bReqInfo[1])
		{
			base.SetShowLayer(3, false);
			GS_COLOSSEUM_RANKINFO_GET_REQ gS_COLOSSEUM_RANKINFO_GET_REQ = new GS_COLOSSEUM_RANKINFO_GET_REQ();
			gS_COLOSSEUM_RANKINFO_GET_REQ.ui8Rank_GetType = 1;
			gS_COLOSSEUM_RANKINFO_GET_REQ.i32Page = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_RANKINFO_GET_REQ, gS_COLOSSEUM_RANKINFO_GET_REQ);
			this.InitColosseumRankData();
		}
		else if (this.bReqInfo[2])
		{
			base.SetShowLayer(3, false);
			GS_COLOSSEUM_RANKINFO_GET_REQ gS_COLOSSEUM_RANKINFO_GET_REQ2 = new GS_COLOSSEUM_RANKINFO_GET_REQ();
			gS_COLOSSEUM_RANKINFO_GET_REQ2.ui8Rank_GetType = 2;
			gS_COLOSSEUM_RANKINFO_GET_REQ2.i32Page = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_RANKINFO_GET_REQ, gS_COLOSSEUM_RANKINFO_GET_REQ2);
			this.InitColosseumRankData();
		}
	}

	private void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	private void OnClickReward(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int colosseumOldRank = kMyCharInfo.ColosseumOldRank;
		if (colosseumOldRank <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("214"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		ColosseumRewardDlg colosseumRewardDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMREWARD_DLG) as ColosseumRewardDlg;
		if (colosseumRewardDlg != null)
		{
			colosseumRewardDlg.SetColosseumRewardInfo();
		}
	}

	private void OnClickRewardinfo(IUIObject obj)
	{
		ColosseumRewardExplainDlg colosseumRewardExplainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMREWARD_EXPLAIN_DLG) as ColosseumRewardExplainDlg;
		if (colosseumRewardExplainDlg != null)
		{
			colosseumRewardExplainDlg.ShowColosseumRewardExplain();
		}
		this.Close();
	}

	public void OnClickShowRecord(IUIObject obj)
	{
		if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_BATTLE_RECORD_DLG) is ColosseumBattleRecordDlg))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_BATTLE_RECORD_DLG);
		}
	}

	public string GetTopGuildRank(long guild_id)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_i64TopGuildID[i] == guild_id)
			{
				switch (i)
				{
				case 0:
					return "Win_I_Rank03";
				case 1:
					return "Win_I_Rank02";
				case 2:
					return "Win_I_Rank01";
				}
			}
		}
		return string.Empty;
	}
}
