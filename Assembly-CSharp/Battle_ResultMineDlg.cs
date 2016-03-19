using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Battle_ResultMineDlg : Form
{
	private Label m_lbResult;

	private Label m_lbItem;

	private DrawTexture m_dtTotalBG;

	private DrawTexture m_dtWinLose;

	private NewListBox m_lbSolList;

	private NewListBox m_lbItemList;

	private NewListBox m_lbContRankList;

	private Button m_btTakeMail;

	private Button m_btReplay;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private bool m_bWin;

	private long m_i64MailID;

	private List<GS_BATTLE_RESULT_SOLDIER> m_SolInfoList = new List<GS_BATTLE_RESULT_SOLDIER>();

	private List<ITEM> m_ItemList = new List<ITEM>();

	private GS_BATTLE_RESULT_MINE m_BasicInfo = new GS_BATTLE_RESULT_MINE();

	private List<MINE_REPORT_CONTRANK_USER_INFO> m_ContributionRankList = new List<MINE_REPORT_CONTRANK_USER_INFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "MINE/dlg_minebattle_result", G_ID.BATTLE_RESULT_MINE_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void Show()
	{
		this.ResizeDlg();
		this.LinkData();
		base.Show();
	}

	public override void SetComponent()
	{
		this.m_lbResult = (base.GetControl("Label_Result") as Label);
		this.m_lbResult.SetCharacterSize(45f);
		this.m_lbItem = (base.GetControl("Label_RootItem") as Label);
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_dtWinLose = (base.GetControl("DrawTexture_DrawTexture1_C") as DrawTexture);
		string path = string.Format("{0}Texture/Loading/0{1}", NrTSingleton<UIDataManager>.Instance.FilePath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		Texture2D texture = (Texture2D)CResources.Load(path);
		this.m_dtTotalBG.SetTexture(texture);
		this.m_lbSolList = (base.GetControl("NLB_SolGetExp") as NewListBox);
		this.m_lbSolList.Reserve = false;
		this.m_lbItemList = (base.GetControl("NLB_MineItemList") as NewListBox);
		this.m_lbItemList.Reserve = false;
		this.m_btTakeMail = (base.GetControl("Button_ok") as Button);
		Button expr_FF = this.m_btTakeMail;
		expr_FF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_FF.Click, new EZValueChangedDelegate(this.OnClickTakMail));
		this.m_lbContRankList = (base.GetControl("NLB_MineRank") as NewListBox);
		this.m_lbContRankList.Reserve = false;
		this.m_btReplay = (base.GetControl("Button_Replay") as Button);
		Button expr_15E = this.m_btReplay;
		expr_15E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_15E.Click, new EZValueChangedDelegate(this.OnClickReplay));
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
		this.m_ScreenWidth = GUICamera.width;
		this.m_ScreenHeight = GUICamera.height;
		this.m_dtTotalBG.SetSize(this.m_ScreenWidth, this.m_ScreenHeight);
		this.m_lbResult.SetLocation(350, 13);
		this.m_lbResult.SetSize(300f, 76f);
		this.m_dtWinLose.SetLocation(0f, 0f);
		this.m_dtWinLose.SetSize(350f, 96f);
	}

	public void SetMailID(long mailid)
	{
		this.m_i64MailID = mailid;
	}

	public void AddSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_SolInfoList.Add(solinfo);
	}

	public void SetBasicData(GS_BATTLE_RESULT_MINE info)
	{
		this.m_BasicInfo = info;
	}

	public void AddItemData(ITEM item_info)
	{
		this.m_ItemList.Add(item_info);
	}

	public void AddContributionRankInfo(MINE_REPORT_CONTRANK_USER_INFO info)
	{
		this.m_ContributionRankList.Add(info);
		this.m_ContributionRankList.Sort(new Comparison<MINE_REPORT_CONTRANK_USER_INFO>(this.OnSortRank));
	}

	public void ClearSolData()
	{
		this.m_SolInfoList.Clear();
	}

	public void LinkData()
	{
		this._LinkBasicData();
		this._LinkSolData();
		this._LinkItemData();
		this._LinkContRankData();
	}

	public void _LinkBasicData()
	{
		string empty = string.Empty;
		ushort num = (ushort)(this.m_BasicInfo.i64BattleTime / 3600L);
		ushort num2 = (ushort)((this.m_BasicInfo.i64BattleTime - (long)(num * 3600)) / 60L);
		ushort num3 = (ushort)(this.m_BasicInfo.i64BattleTime - (long)(num * 3600) - (long)(num2 * 60));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("976"),
			"hour",
			string.Format("{0:D2}", num),
			"min",
			string.Format("{0:D2}", num2),
			"sec",
			string.Format("{0:D2}", num3)
		});
		this.m_bWin = (this.m_BasicInfo.i64WinGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID());
		if (this.m_bWin)
		{
			this.m_dtWinLose.SetTexture("Bat_I_ResultWin");
			this.m_lbResult.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("855"));
		}
		else
		{
			this.m_dtWinLose.SetTexture("Bat_I_ResultLose");
			this.m_lbResult.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("856"));
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1365");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1641");
		this.m_lbItem.Text = ((this.m_BasicInfo.i64AttackGuildID != NrTSingleton<NewGuildManager>.Instance.GetGuildID()) ? textFromInterface2 : textFromInterface);
	}

	private void _LinkSolData()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		int num = 0;
		this.m_lbSolList.Clear();
		foreach (GS_BATTLE_RESULT_SOLDIER current in this.m_SolInfoList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbSolList.ColumnNum, true);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
			if (charKindInfo != null)
			{
				newListItem.SetListItemData(1, new NkListSolInfo
				{
					SolCharKind = current.CharKind,
					SolGrade = current.SolGrade,
					SolInjuryStatus = current.bInjury,
					SolLevel = current.i16Level,
					ShowLevel = true
				}, null, null, null);
				string text = string.Empty;
				if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(current.CharKind))
				{
					text = TKString.NEWString(this.m_BasicInfo.szAttackGuildName);
				}
				else
				{
					text = charKindInfo.GetName();
				}
				string text2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471"),
					"targetname",
					text,
					"count",
					current.i16Level
				});
				newListItem.SetListItemData(2, text2, null, null, null);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(current.SolID);
				short gradeMaxLevel = charKindInfo.GetGradeMaxLevel((short)soldierInfoFromSolID.GetGrade());
				if (soldierInfoFromSolID != null)
				{
					float num2 = soldierInfoFromSolID.GetExpPercent();
					string empty = string.Empty;
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
						"Count",
						((int)(num2 * 100f)).ToString()
					});
					newListItem.SetListItemData(4, "Com_T_GauWaPr4", 400f * num2, null, null);
					newListItem.SetListItemData(5, empty, null, null, null);
				}
				if (gradeMaxLevel == current.i16Level)
				{
					newListItem.SetListItemData(6, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286"), null, null, null);
				}
				else
				{
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1802"),
						"exp",
						current.i32AddExp.ToString()
					});
					text2 += "\r\n";
					text2 += empty2;
					newListItem.SetListItemData(6, empty2, null, null, null);
				}
				this.m_lbSolList.Add(newListItem);
				num++;
			}
		}
		this.m_lbSolList.RepositionItems();
	}

	private void _LinkItemData()
	{
		foreach (ITEM current in this.m_ItemList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbItemList.ColumnNum, true);
			UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_nItemUnique);
			newListItem.SetListItemData(0, itemTexture, null, null, null);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nItemUnique);
			newListItem.SetListItemData(1, itemNameByItemUnique, null, null, null);
			int nItemNum = current.m_nItemNum;
			string text = Protocol_Item.Money_Format((long)nItemNum) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442");
			newListItem.SetListItemData(2, text, null, null, null);
			this.m_lbItemList.Add(newListItem);
		}
		this.m_lbItemList.RepositionItems();
	}

	private void _LinkContRankData()
	{
		byte b = 0;
		string text = string.Empty;
		string empty = string.Empty;
		foreach (MINE_REPORT_CONTRANK_USER_INFO current in this.m_ContributionRankList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbContRankList.ColumnNum, true);
			if (current.ContributionRank == 0)
			{
				b += 1;
			}
			else
			{
				b = current.ContributionRank;
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1413");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"rank",
				b,
				"targetname",
				TKString.NEWString(current.szCharName)
			});
			newListItem.SetListItemData(0, empty, null, null, null);
			this.m_lbContRankList.Add(newListItem);
		}
		this.m_lbContRankList.RepositionItems();
	}

	public void OnClickTakMail(IUIObject obj)
	{
		GS_MAILBOX_TAKE_REPORT_REQ gS_MAILBOX_TAKE_REPORT_REQ = new GS_MAILBOX_TAKE_REPORT_REQ();
		gS_MAILBOX_TAKE_REPORT_REQ.ui8TakeReportType = 0;
		gS_MAILBOX_TAKE_REPORT_REQ.i64MailID = this.m_i64MailID;
		gS_MAILBOX_TAKE_REPORT_REQ.i64LegionActionID = this.m_BasicInfo.i64LegionActionID;
		for (int i = 0; i < 5; i++)
		{
			if (this.m_ItemList.Count <= i)
			{
				gS_MAILBOX_TAKE_REPORT_REQ.i32ItemUnique[i] = 0;
				gS_MAILBOX_TAKE_REPORT_REQ.i32ItemNum[i] = 0;
			}
			else
			{
				gS_MAILBOX_TAKE_REPORT_REQ.i32ItemUnique[i] = this.m_ItemList[i].m_nItemUnique;
				gS_MAILBOX_TAKE_REPORT_REQ.i32ItemNum[i] = this.m_ItemList[i].m_nItemNum;
			}
		}
		for (int j = 0; j < this.m_SolInfoList.Count; j++)
		{
			if (j >= 5)
			{
				break;
			}
			if (this.m_SolInfoList[j].SolID > 0L)
			{
				gS_MAILBOX_TAKE_REPORT_REQ.i64SolID[j] = this.m_SolInfoList[j].SolID;
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_TAKE_REPORT_REQ, gS_MAILBOX_TAKE_REPORT_REQ);
		this.Close();
	}

	public void OnClickReplay(IUIObject obj)
	{
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay && this.m_BasicInfo.i64LegionActionID > 0L)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayMineHttp(this.m_BasicInfo.i64LegionActionID);
		}
	}

	public int OnSortRank(MINE_REPORT_CONTRANK_USER_INFO a, MINE_REPORT_CONTRANK_USER_INFO b)
	{
		if (b.ContributionRank <= 0)
		{
			return -1;
		}
		if (a.ContributionRank > b.ContributionRank)
		{
			return 1;
		}
		return -1;
	}
}
