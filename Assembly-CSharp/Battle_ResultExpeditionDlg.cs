using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_ResultExpeditionDlg : Form
{
	private Label m_lbResult;

	private DrawTexture m_dtTotalBG;

	private DrawTexture m_dtWinLose;

	private NewListBox m_lbSolList;

	private NewListBox m_lbItemList;

	private Button m_btConfirmMail;

	private Button m_btReplay;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private bool m_bWin;

	private long m_i64MailID;

	private List<GS_BATTLE_RESULT_SOLDIER> m_SolInfoList = new List<GS_BATTLE_RESULT_SOLDIER>();

	private List<ITEM> m_ItemList = new List<ITEM>();

	private GS_BATTLE_RESULT_EXPEDITION m_BasicInfo = new GS_BATTLE_RESULT_EXPEDITION();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "MINE/dlg_minebattle_result", G_ID.EXPEDITION_BATTLE_RESULT_DLG, false);
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
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_dtWinLose = (base.GetControl("DrawTexture_DrawTexture1_C") as DrawTexture);
		string path = string.Format("{0}Texture/Loading/0{1}", NrTSingleton<UIDataManager>.Instance.FilePath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		Texture2D texture = (Texture2D)CResources.Load(path);
		this.m_dtTotalBG.SetTexture(texture);
		this.m_lbSolList = (base.GetControl("NLB_SolGetExp") as NewListBox);
		this.m_lbSolList.Reserve = false;
		this.m_lbItemList = (base.GetControl("NLB_MineItemList") as NewListBox);
		this.m_lbItemList.Reserve = false;
		this.m_btConfirmMail = (base.GetControl("Button_ok") as Button);
		Button expr_E9 = this.m_btConfirmMail;
		expr_E9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E9.Click, new EZValueChangedDelegate(this.OnClickTakeMail));
		this.m_btReplay = (base.GetControl("Button_Replay") as Button);
		Button expr_126 = this.m_btReplay;
		expr_126.Click = (EZValueChangedDelegate)Delegate.Combine(expr_126.Click, new EZValueChangedDelegate(this.OnClickReplay));
	}

	public void SetBG(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.m_dtTotalBG.SetTexture(texture2D);
			}
		}
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

	public void SetBasicData(GS_BATTLE_RESULT_EXPEDITION info)
	{
		this.m_BasicInfo = info;
	}

	public void AddItemData(ITEM item_info)
	{
		this.m_ItemList.Add(item_info);
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
		this.m_bWin = (this.m_BasicInfo.byWin == 1);
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
			NewListItem newListItem = new NewListItem(this.m_lbSolList.ColumnNum, true, string.Empty);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
			if (charKindInfo != null)
			{
				newListItem.SetListItemData(1, new NkListSolInfo
				{
					SolCharKind = current.CharKind,
					SolGrade = current.SolGrade,
					SolInjuryStatus = current.bInjury,
					SolLevel = current.i16Level,
					ShowLevel = true,
					SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(current.i32CostumeUnique)
				}, null, null, null);
				string text = string.Empty;
				text = charKindInfo.GetName();
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
				short num2 = 0;
				if (soldierInfoFromSolID != null)
				{
					num2 = charKindInfo.GetGradeMaxLevel((short)soldierInfoFromSolID.GetGrade());
					float num3 = soldierInfoFromSolID.GetExpPercent();
					string empty = string.Empty;
					if (num3 < 0f)
					{
						num3 = 0f;
					}
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
						"Count",
						((int)(num3 * 100f)).ToString()
					});
					newListItem.SetListItemData(4, "Com_T_GauWaPr4", 400f * num3, null, null);
					newListItem.SetListItemData(5, empty, null, null, null);
				}
				if (num2 == current.i16Level)
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
						ANNUALIZED.Convert(current.i32AddExp)
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
			NewListItem newListItem = new NewListItem(this.m_lbItemList.ColumnNum, true, string.Empty);
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

	public void OnClickTakeMail(IUIObject obj)
	{
		GS_MAILBOX_TAKE_REPORT_REQ gS_MAILBOX_TAKE_REPORT_REQ = new GS_MAILBOX_TAKE_REPORT_REQ();
		gS_MAILBOX_TAKE_REPORT_REQ.ui8TakeReportType = 0;
		gS_MAILBOX_TAKE_REPORT_REQ.i64MailID = this.m_i64MailID;
		gS_MAILBOX_TAKE_REPORT_REQ.i64LegionActionID = this.m_BasicInfo.i64ExpeditionBattleUnique;
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
			if (j >= 15)
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
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay && this.m_BasicInfo.i64ExpeditionBattleUnique > 0L)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestExpeditionReplay(this.m_BasicInfo.i64ExpeditionBattleUnique);
		}
	}
}
