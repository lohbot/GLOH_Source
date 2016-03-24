using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class PlunderRankInfoDlg : Form
{
	private enum eTAB
	{
		eTAB_TOTALRANK,
		eTAB_FRIENDRANK,
		eTAB_MAX
	}

	private Toggle[] m_tgRankList = new Toggle[2];

	private Button m_btMyRank;

	private Button m_btSearch;

	private Button m_btClose;

	private Label m_laTargetRank;

	private Label m_laTargetCharName;

	private Label m_laTargetLevel;

	private Label m_laMatchPoint;

	private Label m_laMatchingNoticeMsg;

	private DrawTexture m_dtRankUpDown;

	private NewListBox m_lbRankList;

	private TextField m_tfSearchName;

	private List<UI_RANKINFO> ListRankInfo = new List<UI_RANKINFO>();

	private UI_RANKINFO TargetInfo;

	private int i32OldRank;

	private int i32NewRank;

	private eRANK_SHOWTYPE show_type;

	private bool[] bReqInfo = new bool[2];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_plunder_rank", G_ID.PLUNDER_RANKINFO_DLG, true);
		base.SetScreenCenter();
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_laTargetRank = (base.GetControl("Label_Rank") as Label);
		this.m_laTargetCharName = (base.GetControl("Label_charname") as Label);
		this.m_laTargetLevel = (base.GetControl("Label_level") as Label);
		this.m_laMatchPoint = (base.GetControl("Label_MatchPoint") as Label);
		this.m_dtRankUpDown = (base.GetControl("DrawTexture_DrawTexture28") as DrawTexture);
		this.m_laMatchingNoticeMsg = (base.GetControl("Label_Label29") as Label);
		this.m_btMyRank = (base.GetControl("Button_MyRank") as Button);
		Button expr_A0 = this.m_btMyRank;
		expr_A0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A0.Click, new EZValueChangedDelegate(this.OnClickMyInfo));
		this.m_btSearch = (base.GetControl("Button_search") as Button);
		Button expr_DD = this.m_btSearch;
		expr_DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DD.Click, new EZValueChangedDelegate(this.OnClickSearchFriend));
		this.m_btClose = (base.GetControl("Button_close") as Button);
		Button expr_11A = this.m_btClose;
		expr_11A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_11A.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_tfSearchName = (base.GetControl("TextField_TextField11") as TextField);
		this.m_lbRankList = (base.GetControl("NewListBox_rank_list") as NewListBox);
		for (int i = 0; i < 2; i++)
		{
			int num = i + 1;
			this.m_tgRankList[i] = (base.GetControl("Toggle_tab" + num) as Toggle);
			this.m_tgRankList[i].data = i;
			this.m_tgRankList[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
			this.bReqInfo[i] = false;
		}
		this.i32OldRank = PlayerPrefs.GetInt("Plunder Rank");
		base.SetShowLayer(3, false);
	}

	public override void OnClose()
	{
		if (this.i32NewRank != 0 || this.i32OldRank == 0)
		{
			PlayerPrefs.SetInt("Plunder Rank", this.i32NewRank);
		}
		base.OnClose();
	}

	public void AddRankInfo(PLUNDER_RANKINFO info)
	{
		UI_RANKINFO item = new UI_RANKINFO(info);
		this.ListRankInfo.Add(item);
	}

	public void AddFriendRankInfo(PLUNDER_FRIEND_RANKINFO info)
	{
		UI_RANKINFO item = new UI_RANKINFO(info);
		this.ListRankInfo.Add(item);
	}

	public void SetTargetInfo(PLUNDER_RANKINFO info)
	{
		this.TargetInfo = new UI_RANKINFO(info);
	}

	public void ShowList()
	{
		this.m_lbRankList.Clear();
		string text = string.Empty;
		string empty = string.Empty;
		foreach (UI_RANKINFO current in this.ListRankInfo)
		{
			NewListItem newListItem = new NewListItem(this.m_lbRankList.ColumnNum, true, string.Empty);
			newListItem.SetListItemData(0, current.Charname, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count",
				current.iCharLevel
			});
			newListItem.SetListItemData(1, empty, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"rank",
				current.i32Rank
			});
			newListItem.SetListItemData(2, empty, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"ratingpoint",
				1000L + current.i64MatchPoint
			});
			newListItem.SetListItemData(3, empty, null, null, null);
			newListItem.SetListItemData(4, false);
			this.m_lbRankList.Add(newListItem);
		}
	}

	public void ShowTargetInfo()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.TargetInfo == null)
		{
			return;
		}
		if (this.TargetInfo.i32Rank > 0)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2093");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"rank",
				this.TargetInfo.i32Rank
			});
		}
		else
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("292");
		}
		this.m_laTargetRank.Text = text2;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"charname",
			this.TargetInfo.Charname
		});
		this.m_laTargetCharName.Text = text2;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			this.TargetInfo.iCharLevel
		});
		this.m_laTargetLevel.Text = text2;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2091");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"ratingpoint",
			1000L + this.TargetInfo.i64MatchPoint
		});
		this.m_laMatchPoint.Text = text2;
		if (this.show_type == eRANK_SHOWTYPE.eRANK_SHOWTYPE_FRIENDRANK)
		{
			this.m_dtRankUpDown.Visible = true;
			this.m_laMatchingNoticeMsg.SetText(" ");
			if (this.i32NewRank == this.i32OldRank)
			{
				this.m_dtRankUpDown.SetTexture("Win_I_ArrowMaintain");
			}
			else if (this.i32NewRank > this.i32OldRank)
			{
				this.m_dtRankUpDown.SetTexture("Win_I_ArrowUP");
			}
			else if (this.i32NewRank < this.i32OldRank)
			{
				this.m_dtRankUpDown.SetTexture("Win_I_ArrowDown");
			}
		}
		else
		{
			this.m_laMatchingNoticeMsg.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("441"));
			this.m_dtRankUpDown.Visible = false;
		}
	}

	public void ShowInfo(eRANK_SHOWTYPE rank_showtype)
	{
		this.SetListInfo(rank_showtype);
		this.ShowList();
		this.ShowTargetInfo();
		this.Show();
	}

	public void SetListInfo(eRANK_SHOWTYPE rank_showtype)
	{
		base.SetShowLayer(2, false);
		this.show_type = rank_showtype;
		if (rank_showtype == eRANK_SHOWTYPE.eRANK_SHOWTYPE_TOTALRANK)
		{
			base.SetShowLayer(1, true);
			this.SetListTotalRank();
		}
		else if (rank_showtype == eRANK_SHOWTYPE.eRANK_SHOWTYPE_FRIENDRANK)
		{
			base.SetShowLayer(1, false);
			this.SetListFriendRank();
		}
	}

	public void SetListTotalRank()
	{
		this.ListRankInfo.Sort(new Comparison<UI_RANKINFO>(PlunderRankInfoDlg.CompareLevel));
	}

	public void SetListFriendRank()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MATCHPOINT);
		UI_RANKINFO item = new UI_RANKINFO(charPersonInfo.GetCharName(), (byte)charPersonInfo.GetLevel(0L), charSubData);
		this.ListRankInfo.Add(item);
		this.ListRankInfo.Sort(new Comparison<UI_RANKINFO>(PlunderRankInfoDlg.CompareMatchPoint));
		int num = 1;
		foreach (UI_RANKINFO current in this.ListRankInfo)
		{
			current.i32Rank = num;
			num++;
		}
		foreach (UI_RANKINFO current2 in this.ListRankInfo)
		{
			if (current2.Charname == charPersonInfo.GetCharName())
			{
				this.TargetInfo = current2;
				this.i32NewRank = this.TargetInfo.i32Rank;
				break;
			}
		}
	}

	private static int CompareLevel(UI_RANKINFO x, UI_RANKINFO y)
	{
		if (x.i32Rank > y.i32Rank)
		{
			return 1;
		}
		return -1;
	}

	private static int CompareMatchPoint(UI_RANKINFO x, UI_RANKINFO y)
	{
		if (x.i64MatchPoint < y.i64MatchPoint)
		{
			return 1;
		}
		if (x.i64MatchPoint == y.i64MatchPoint)
		{
			return 0;
		}
		return -1;
	}

	public void InitPlunderRankData()
	{
		this.ListRankInfo.Clear();
		this.m_lbRankList.Clear();
	}

	private void OnClickMyInfo(IUIObject obj)
	{
		GS_PLUNDER_RANKINFO_GET_REQ gS_PLUNDER_RANKINFO_GET_REQ = new GS_PLUNDER_RANKINFO_GET_REQ();
		gS_PLUNDER_RANKINFO_GET_REQ.ui8Rank_GetType = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RANKINFO_GET_REQ, gS_PLUNDER_RANKINFO_GET_REQ);
	}

	private void OnClickSearchFriend(IUIObject obj)
	{
		string text = this.m_tfSearchName.Text;
		if (text == string.Empty)
		{
			return;
		}
		GS_PLUNDER_RANKINFO_GET_REQ gS_PLUNDER_RANKINFO_GET_REQ = new GS_PLUNDER_RANKINFO_GET_REQ();
		gS_PLUNDER_RANKINFO_GET_REQ.ui8Rank_GetType = 1;
		TKString.StringChar(text, ref gS_PLUNDER_RANKINFO_GET_REQ.szSearchName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RANKINFO_GET_REQ, gS_PLUNDER_RANKINFO_GET_REQ);
		this.m_tfSearchName.Text = string.Empty;
	}

	private void OnClickTabControl(IUIObject obj)
	{
		this.InitPlunderRankData();
		if (this.m_tgRankList[0].GetToggleState())
		{
			if (this.bReqInfo[0])
			{
				return;
			}
			this.bReqInfo[0] = true;
			this.bReqInfo[1] = false;
		}
		else if (this.m_tgRankList[1].GetToggleState())
		{
			if (this.bReqInfo[1])
			{
				return;
			}
			this.bReqInfo[0] = false;
			this.bReqInfo[1] = true;
		}
		if (this.bReqInfo[0])
		{
			GS_PLUNDER_RANKINFO_GET_REQ gS_PLUNDER_RANKINFO_GET_REQ = new GS_PLUNDER_RANKINFO_GET_REQ();
			gS_PLUNDER_RANKINFO_GET_REQ.ui8Rank_GetType = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RANKINFO_GET_REQ, gS_PLUNDER_RANKINFO_GET_REQ);
		}
		else if (this.bReqInfo[1])
		{
			GS_PLUNDER_RANKINFO_GET_REQ gS_PLUNDER_RANKINFO_GET_REQ2 = new GS_PLUNDER_RANKINFO_GET_REQ();
			gS_PLUNDER_RANKINFO_GET_REQ2.ui8Rank_GetType = 3;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RANKINFO_GET_REQ, gS_PLUNDER_RANKINFO_GET_REQ2);
		}
	}

	private void OnClickClose(IUIObject obj)
	{
		this.Close();
	}
}
