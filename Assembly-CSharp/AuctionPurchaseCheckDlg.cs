using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class AuctionPurchaseCheckDlg : Form
{
	private DrawTexture m_dtItemBG;

	private ItemTexture m_itItem;

	private Label m_lbItemName;

	private Label m_lbTradeCount;

	private Button m_btDetailInfo;

	private Label m_lbDirectCost;

	private DrawTexture m_dtCostIcon;

	private Button m_btOK;

	private Button m_btCancel;

	private AUCTION_REGISTER_ITEM m_RegisterItem;

	private AUCTION_REGISTER_SOL_TOTAL m_RegisterSol;

	private string m_strText = string.Empty;

	private long m_lDirectCostMoney;

	private int m_iDirectCostHearts;

	private long m_lAuctionID;

	private AuctionDefine.ePAYTYPE m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionPurchaseCheck", G_ID.AUCTION_PURCHASECHECK_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtItemBG = (base.GetControl("DrawTexture_ItemBG") as DrawTexture);
		this.m_itItem = (base.GetControl("ItemTexture_ItemIcon") as ItemTexture);
		this.m_lbItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_lbTradeCount = (base.GetControl("Label_TradeCount") as Label);
		this.m_btDetailInfo = (base.GetControl("Button_DetailInfo") as Button);
		this.m_btDetailInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDetailInfo));
		this.m_dtCostIcon = (base.GetControl("Icon_Money01") as DrawTexture);
		this.m_lbDirectCost = (base.GetControl("Label_Cost01") as Label);
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
		if (!this.IsDirectPurchase())
		{
			return;
		}
		GS_AUCTION_DIRECTPURCHASE_REQ gS_AUCTION_DIRECTPURCHASE_REQ = new GS_AUCTION_DIRECTPURCHASE_REQ();
		gS_AUCTION_DIRECTPURCHASE_REQ.i64AuctionID = this.m_lAuctionID;
		gS_AUCTION_DIRECTPURCHASE_REQ.i64DirectCostMoney = this.m_lDirectCostMoney;
		gS_AUCTION_DIRECTPURCHASE_REQ.i32DirectCostHearts = this.m_iDirectCostHearts;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_DIRECTPURCHASE_REQ, gS_AUCTION_DIRECTPURCHASE_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_PURCHASECHECK_DLG);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_PURCHASECHECK_DLG);
	}

	public void SetSelectInfo()
	{
	}

	public void SetSelectInfoItem(AUCTION_REGISTER_ITEM RegisterItem)
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

	public void SetSelectInfoSol(AUCTION_REGISTER_SOL_TOTAL RegisterSol)
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
		if (0L < RegisterInfo.i64DirectCostMoney)
		{
			this.m_lDirectCostMoney = RegisterInfo.i64DirectCostMoney;
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_lDirectCostMoney));
			this.m_dtCostIcon.SetTexture("Com_I_MoneyIconM");
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
		}
		else if (0 < RegisterInfo.i32DirectCostHearts)
		{
			this.m_iDirectCostHearts = RegisterInfo.i32DirectCostHearts;
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_iDirectCostHearts));
			this.m_dtCostIcon.SetTexture("Win_I_Hearts");
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
		}
	}

	public bool IsDirectPurchase()
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (0 >= auctionUseLevel)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (auctionUseLevel > (short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel())
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("628"),
				"count",
				auctionUseLevel
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (0L >= this.m_lAuctionID)
		{
			return false;
		}
		if (this.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			if (this.m_lDirectCostMoney > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
		}
		else
		{
			if (this.m_ePayType != AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
			{
				return false;
			}
			if (this.m_iDirectCostHearts > NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
		}
		return true;
	}
}
