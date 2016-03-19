using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class AuctionTenderCheckDlg : Form
{
	private DrawTexture m_dtItemBG;

	private ItemTexture m_itItem;

	private Label m_lbItemName;

	private Label m_lbTradeCount;

	private Button m_btDetailInfo;

	private Label m_lbCost;

	private Button m_btMyCost;

	private Label m_lbMyCost;

	private DrawTexture m_dtCostIcon1;

	private DrawTexture m_dtCostIcon2;

	private Button m_btOK;

	private Button m_btCancel;

	private AUCTION_REGISTER_ITEM m_RegisterItem;

	private AUCTION_REGISTER_SOL_TOTAL m_RegisterSol;

	private string m_strText = string.Empty;

	private long m_lCostMoney;

	private int m_iCostHearts;

	private long m_lCost;

	private long m_lTenderCost;

	private long m_lAuctionID;

	private long m_lPersonID;

	private long m_lCurCostMoney;

	private long m_lCurDirectCostMoney;

	private int m_iCurCostHearts;

	private int m_iCurDirectCostHearts;

	private AuctionDefine.ePAYTYPE m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionTenderCheck", G_ID.AUCTION_TENDERCHECK_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtItemBG = (base.GetControl("DrawTexture_ItemBG") as DrawTexture);
		this.m_itItem = (base.GetControl("ItemTexture_ItemIcon") as ItemTexture);
		this.m_lbItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_lbTradeCount = (base.GetControl("Label_TradeCount") as Label);
		this.m_btDetailInfo = (base.GetControl("Button_DetailInfo") as Button);
		this.m_btDetailInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDetailInfo));
		this.m_lbCost = (base.GetControl("Label_Cost01") as Label);
		this.m_btMyCost = (base.GetControl("Button_Cost02") as Button);
		this.m_btMyCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMyCost));
		this.m_lbMyCost = (base.GetControl("Label_Cost02") as Label);
		this.m_dtCostIcon1 = (base.GetControl("Icon_Money01") as DrawTexture);
		this.m_dtCostIcon2 = (base.GetControl("Icon_Money02") as DrawTexture);
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_NO") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickDetailInfo(IUIObject obj)
	{
		if (this.m_RegisterItem != null)
		{
			AuctionMainDlg.ShowItemDetailInfo(this.m_RegisterItem.Item, (G_ID)base.WindowID);
		}
		else if (this.m_RegisterSol != null)
		{
			AuctionMainDlg.ShowSolDetailInfo(this.m_RegisterSol, this);
		}
	}

	public void ClickOK(IUIObject obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID() == this.m_lPersonID)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("300"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (0L < this.m_lCostMoney)
		{
			if (this.m_lTenderCost < this.m_lCostMoney)
			{
				return;
			}
			if (this.m_lTenderCost > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		if (0 < this.m_iCostHearts)
		{
			if (this.m_lTenderCost < (long)this.m_iCostHearts)
			{
				return;
			}
			if (this.m_lTenderCost > (long)NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		if (0L >= this.m_lAuctionID)
		{
			return;
		}
		GS_AUCTION_TENDER_REQ gS_AUCTION_TENDER_REQ = new GS_AUCTION_TENDER_REQ();
		gS_AUCTION_TENDER_REQ.i64AuctionID = this.m_lAuctionID;
		if (this.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			gS_AUCTION_TENDER_REQ.i64CostMoney = this.m_lTenderCost;
		}
		else if (this.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			gS_AUCTION_TENDER_REQ.i32CostHearts = (int)this.m_lTenderCost;
		}
		gS_AUCTION_TENDER_REQ.i64CurCostMoney = this.m_lCurCostMoney;
		gS_AUCTION_TENDER_REQ.i64CurDirectCostMoney = this.m_lCurDirectCostMoney;
		gS_AUCTION_TENDER_REQ.i32CurCostHearts = this.m_iCurCostHearts;
		gS_AUCTION_TENDER_REQ.i32CurDirectCostHearts = this.m_iCurDirectCostHearts;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_TENDER_REQ, gS_AUCTION_TENDER_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_TENDERCHECK_DLG);
	}

	public void ClickMyCost(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputMyCost), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		long costMax = AuctionMainDlg.GetCostMax(this.m_ePayType);
		long tenderCostMin = this.GetTenderCostMin(this.m_lCost);
		inputNumberDlg.SetMinMax(tenderCostMin, costMax);
		inputNumberDlg.SetNum(this.m_lTenderCost);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_TENDERCHECK_DLG);
	}

	public void SetSelectInfo()
	{
	}

	public void SetTenderItem(AUCTION_REGISTER_ITEM RegisterItem)
	{
		this.m_RegisterItem = RegisterItem;
		this.m_itItem.SetItemTexture(RegisterItem.Item);
		int rank = RegisterItem.Item.m_nOption[2];
		string str = ItemManager.RankTextColor(rank);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(RegisterItem.Item);
		this.m_lbItemName.SetText(str + rankColorName);
		this.m_dtItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
		int num = RegisterItem.Item.m_nOption[7];
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			num
		});
		this.m_lbTradeCount.SetText(this.m_strText);
		this.SetTenderInfo(RegisterItem.RegisterInfo);
	}

	public void SetTenderSol(AUCTION_REGISTER_SOL_TOTAL RegisterSol)
	{
		this.m_RegisterSol = RegisterSol;
		this.m_itItem.SetSolImageTexure(eCharImageType.SMALL, this.m_RegisterSol.SoldierInfo.CharKind, (int)this.m_RegisterSol.SoldierInfo.Grade, (int)this.m_RegisterSol.SoldierInfo.Level);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_RegisterSol.SoldierInfo.CharKind);
		if (charKindInfo != null)
		{
			this.m_lbItemName.SetText(charKindInfo.GetName());
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			RegisterSol.SolSubData[5]
		});
		this.m_lbTradeCount.SetText(this.m_strText);
		this.SetTenderInfo(RegisterSol.RegisterInfo);
	}

	public void SetTenderInfo(AUCTION_REGISTER_INFO RegisterInfo)
	{
		this.m_lAuctionID = RegisterInfo.i64AuctionID;
		this.m_lPersonID = RegisterInfo.i64PersonID;
		this.m_lCurCostMoney = RegisterInfo.i64CostMoney;
		this.m_lCurDirectCostMoney = RegisterInfo.i64DirectCostMoney;
		this.m_iCurCostHearts = RegisterInfo.i32CostHearts;
		this.m_iCurDirectCostHearts = RegisterInfo.i32DirectCostHearts;
		if (0L < RegisterInfo.i64CostMoney)
		{
			this.m_lCostMoney = RegisterInfo.i64CostMoney;
			this.m_lCost = RegisterInfo.i64CostMoney;
			this.m_lbCost.SetText(ANNUALIZED.Convert(RegisterInfo.i64CostMoney));
			this.m_lTenderCost = this.GetTenderCostMin(RegisterInfo.i64CostMoney);
			this.m_lbMyCost.SetText(ANNUALIZED.Convert(this.m_lTenderCost));
			this.m_dtCostIcon1.SetTexture("Com_I_MoneyIconM");
			this.m_dtCostIcon2.SetTexture("Com_I_MoneyIconM");
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
		}
		else if (0 < RegisterInfo.i32CostHearts)
		{
			this.m_iCostHearts = RegisterInfo.i32CostHearts;
			this.m_lCost = (long)RegisterInfo.i32CostHearts;
			this.m_lbCost.SetText(ANNUALIZED.Convert(RegisterInfo.i32CostHearts));
			this.m_lTenderCost = this.GetTenderCostMin((long)RegisterInfo.i32CostHearts);
			this.m_lbMyCost.SetText(ANNUALIZED.Convert(this.m_lTenderCost));
			this.m_dtCostIcon1.SetTexture("Win_I_Hearts");
			this.m_dtCostIcon2.SetTexture("Win_I_Hearts");
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
		}
	}

	public long GetTenderCostMin(long lCurTenderCost)
	{
		double num = (double)AuctionMainDlg.GetAuctionTenderRate();
		long num2 = (long)((double)lCurTenderCost * num);
		if (0L >= num2)
		{
			num2 = 1L;
		}
		return lCurTenderCost + num2;
	}

	public void OnInputMyCost(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		if (AuctionMainDlg.GetCostMax(this.m_ePayType) < num)
		{
			num = AuctionMainDlg.GetCostMax(this.m_ePayType);
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string empty = string.Empty;
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("258"),
			"targetname",
			this.m_lbItemName.GetText() + textColor,
			"count",
			num
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MessageBoxMyCost), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1104"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.Show();
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void MessageBoxMyCost(object a_oObject)
	{
		long num = (long)a_oObject;
		if (num > 0L)
		{
			this.m_lTenderCost = (long)a_oObject;
			this.m_lbMyCost.SetText(ANNUALIZED.Convert(this.m_lTenderCost));
			this.ClickOK(null);
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}
}
