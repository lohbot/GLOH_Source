using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class AuctionMainDlg : Form
{
	public enum eTAB
	{
		eTAB_PURCHASE,
		eTAB_SELL,
		eTAB_TENDER,
		eTAB_MAX
	}

	private Toolbar m_tbTab;

	private Label m_lbHeartsNum;

	private DrawTexture m_dtHeartsIcon;

	private Label m_lbMoney;

	private DrawTexture m_dtMoneyIcon;

	private Button m_btRefresh;

	private Button m_btBack;

	private Button m_btOption;

	private Button m_btCost;

	private Button m_btDirectCost;

	private Button m_btRemainingTime;

	private NewListBox m_nlbPurchaseList;

	private Button m_btSerchCondition;

	private Button m_btPrev;

	private Label m_lbPage;

	private Button m_btNext;

	private Button m_btTender;

	private Button m_btPurchase;

	private Button m_btSelectItem;

	private ItemTexture m_itSellItem;

	private Label m_lbSellItemName;

	private Label m_lbSellTradeCount;

	private Label m_lbSellMoneyKind;

	private Toggle[] m_tgPayType = new Toggle[3];

	private DrawTexture m_dtSellHeartsIcon;

	private DrawTexture m_dtsellMoneyIcon;

	private DrawTexture m_dtCostIcon1;

	private DrawTexture m_dtCostIcon2;

	private Button m_btSellCost;

	private Label m_lbSellCost;

	private Button m_btSellDirectCost;

	private Label m_lbSellDirectCoust;

	private Label m_lbCommission;

	private Button m_btSellRegister;

	private Label m_lbSellListItemNum;

	private NewListBox m_nlbSellList;

	private AuctionMainDlg.eTAB m_eTab;

	private AuctionDefine.ePAYTYPE m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;

	private long m_lCurGold = -1L;

	private int m_iCurHearts = -1;

	private ITEM m_SelectItem;

	private NkSoldierInfo m_SelectSol;

	private string m_strText = string.Empty;

	private long m_lSellCost = 1L;

	private long m_lSellDirectCost;

	private int m_iCurPageNum;

	private int m_iMaxPageNum;

	private string m_strPageNum = string.Empty;

	private AuctionSearchOption m_SearchOption = new AuctionSearchOption();

	private List<AUCTION_REGISTER_ITEM> m_RegisterItem = new List<AUCTION_REGISTER_ITEM>();

	private List<AUCTION_REGISTER_SOL_TOTAL> m_RegisterSol = new List<AUCTION_REGISTER_SOL_TOTAL>();

	private string m_strExpireTime = string.Empty;

	private List<long> m_Register = new List<long>();

	private List<RegisterSort> m_RegisterSort = new List<RegisterSort>();

	private static int m_i32AuctionHeartsUse;

	private static int m_i32AuctionGoldUse;

	private static float m_fAuctionCommission;

	private static int m_i32AuctionSellMaxNum;

	private static long m_lAuctionSellPrice;

	private static long m_lAuctionDuration;

	private static long m_lAuctionDurationExtend;

	private static int m_i32AuctionTenderMaxNum;

	private static int m_i32AuctionTenderShowNum;

	private static float m_fAuctionTenderRate;

	private static float m_fAuctionSellPriceRate;

	private static int m_i32HeartsValue;

	private static int m_i32DailySellLimit;

	private static int m_i32DailyBuyLimit;

	private static short m_i16SolLevelLimit;

	private static short m_i16SolSkillLevelLimit;

	private AuctionDefine.eSORT_TYPE m_eSortType;

	private AUCTION_TABLE_INFO m_AuctionTableInfo = new AUCTION_TABLE_INFO();

	private bool m_bSendAuctionTableInfo;

	private bool m_bInitAuctionStateControl = true;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionMain", G_ID.AUCTION_MAIN_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tbTab = (base.GetControl("TabButton") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1022");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1023");
		this.m_tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1411");
		UIPanelTab expr_86 = this.m_tbTab.Control_Tab[0];
		expr_86.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_86.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_B4 = this.m_tbTab.Control_Tab[1];
		expr_B4.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_B4.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_E2 = this.m_tbTab.Control_Tab[2];
		expr_E2.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_E2.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_lbHeartsNum = (base.GetControl("Label_HeartNum") as Label);
		this.m_dtHeartsIcon = (base.GetControl("Icon_Hearts01") as DrawTexture);
		this.m_lbMoney = (base.GetControl("Label_GoldNum") as Label);
		this.m_dtMoneyIcon = (base.GetControl("Icon_Gold01") as DrawTexture);
		this.m_btRefresh = (base.GetControl("Button_Refresh") as Button);
		this.m_btRefresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRefresh));
		this.m_btOption = (base.GetControl("Button_Buy_Option") as Button);
		this.m_btOption.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOptionSort));
		this.m_btCost = (base.GetControl("Button_Buy_AuctionCost") as Button);
		this.m_btCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCostSort));
		this.m_btDirectCost = (base.GetControl("Button_Buy_DirectCost") as Button);
		this.m_btDirectCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDirectCostSort));
		this.m_btRemainingTime = (base.GetControl("Button_Buy_LeftTime") as Button);
		this.m_btRemainingTime.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRemainingTimeSort));
		this.m_nlbPurchaseList = (base.GetControl("NewListBox_AuctionBuyList") as NewListBox);
		this.m_nlbPurchaseList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPurchasekList));
		this.m_nlbPurchaseList.touchScroll = false;
		this.m_btSerchCondition = (base.GetControl("Button_Buy_Search") as Button);
		this.m_btSerchCondition.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSerchCondition));
		this.m_btPrev = (base.GetControl("Button_Buy_PrePage") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_lbPage = (base.GetControl("Label_Buy_PageNum") as Label);
		this.m_btNext = (base.GetControl("Button_Buy_NextPage") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_btTender = (base.GetControl("Button_Buy_Tender") as Button);
		this.m_btTender.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickTender));
		this.m_btPurchase = (base.GetControl("Button_Buy_Purchase") as Button);
		this.m_btPurchase.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDirectPurchase));
		this.m_btSelectItem = (base.GetControl("Button_SelectItem") as Button);
		this.m_btSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectItem));
		this.m_itSellItem = (base.GetControl("Icon_Sell_ItemImg") as ItemTexture);
		this.m_lbSellItemName = (base.GetControl("Label_Sell_ItemName") as Label);
		this.m_lbSellTradeCount = (base.GetControl("Label_Sell_TradeCount") as Label);
		this.m_lbSellMoneyKind = (base.GetControl("Label_Sell_MoneyKind") as Label);
		this.m_tgPayType[0] = (base.GetControl("Toggle_MoneyKind01") as Toggle);
		this.m_tgPayType[0].SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickPayType));
		this.m_tgPayType[0].Data = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
		this.m_tgPayType[1] = (base.GetControl("Toggle_MoneyKind02") as Toggle);
		this.m_tgPayType[1].SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickPayType));
		this.m_tgPayType[1].Data = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
		this.m_dtSellHeartsIcon = (base.GetControl("Icon_MoneyKindHearts") as DrawTexture);
		this.m_dtsellMoneyIcon = (base.GetControl("Icon_MoneyKind_Gold") as DrawTexture);
		this.m_dtCostIcon1 = (base.GetControl("Icon_MoneyKind01") as DrawTexture);
		this.m_dtCostIcon2 = (base.GetControl("Icon_MoneyKind02") as DrawTexture);
		this.m_btSellCost = (base.GetControl("Button_Sell_AuctionCost") as Button);
		this.m_btSellCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSellCost));
		this.m_lbSellCost = (base.GetControl("Label_Sell_AuctionCost") as Label);
		this.m_btSellDirectCost = (base.GetControl("Button_Sell_DirectCost") as Button);
		this.m_btSellDirectCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSellDirectCost));
		this.m_lbSellDirectCoust = (base.GetControl("Label_Sell_DirectCost") as Label);
		this.m_lbCommission = (base.GetControl("Label_Commission") as Label);
		this.m_btSellRegister = (base.GetControl("Button_Sell") as Button);
		this.m_btSellRegister.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSellRegister));
		this.m_lbSellListItemNum = (base.GetControl("Label_SellList_ItemNum") as Label);
		this.m_nlbSellList = (base.GetControl("NewListBox_AuctionSellList") as NewListBox);
		this.m_nlbSellList.touchScroll = false;
		this.m_btBack = (base.GetControl("BT_back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBack));
		this.InitControl();
		this.SelectTab(this.m_eTab);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void Update()
	{
		if (this.m_lCurGold != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			this.m_lCurGold = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
			this.m_lbMoney.SetText(ANNUALIZED.Convert(this.m_lCurGold));
		}
		if (this.m_iCurHearts != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_iCurHearts = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			string text = ANNUALIZED.Convert(this.m_iCurHearts);
			this.m_lbHeartsNum.SetText(text);
			TsLog.LogOnlyEditor("Hearts : " + text);
		}
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eTab = (AuctionMainDlg.eTAB)uIPanelTab.panel.index;
		this.SelectTab(this.m_eTab);
	}

	public void SelectTab(AuctionMainDlg.eTAB eTab)
	{
		this.ChangeTab();
		this.SetEnableControl(false);
		switch (eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
			this.ShowPurchasetList();
			break;
		case AuctionMainDlg.eTAB.eTAB_SELL:
			this.ShowSellList();
			break;
		case AuctionMainDlg.eTAB.eTAB_TENDER:
			this.ShowTenderList();
			break;
		}
	}

	public void ChangeTab()
	{
		this.m_nlbPurchaseList.Clear();
		this.m_nlbSellList.Clear();
		this.m_btPurchase.SetEnabled(true);
	}

	public void ShowPurchasetList()
	{
		base.ShowLayer(1);
		this.m_btSerchCondition.Hide(false);
		AuctionMainDlg.Send_PurchaseList(0, this.m_SearchOption, AuctionDefine.eSORT_TYPE.eSORT_TYPE_NONE, false);
	}

	public void ShowSellList()
	{
		base.ShowLayer(2);
		this.InitControl(AuctionMainDlg.eTAB.eTAB_SELL);
		AuctionMainDlg.Send_SellList(0);
		this.CheckHeartsAnGoldUse();
	}

	public void ShowTenderList()
	{
		base.ShowLayer(1);
		this.m_btSerchCondition.Hide(true);
		AuctionMainDlg.Send_TenderList(0);
	}

	public void ClickRefresh(IUIObject obj)
	{
		switch (this.m_eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
			AuctionMainDlg.Send_PurchaseList(this.m_iCurPageNum, this.m_SearchOption, AuctionDefine.eSORT_TYPE.eSORT_TYPE_NONE, false);
			break;
		case AuctionMainDlg.eTAB.eTAB_SELL:
			AuctionMainDlg.Send_SellList(this.m_iCurPageNum);
			break;
		case AuctionMainDlg.eTAB.eTAB_TENDER:
			AuctionMainDlg.Send_TenderList(this.m_iCurPageNum);
			break;
		}
	}

	public void ClickOptionSort(IUIObject obj)
	{
	}

	public void ClickCostSort(IUIObject obj)
	{
	}

	public void ClickDirectCostSort(IUIObject obj)
	{
	}

	public void ClickRemainingTimeSort(IUIObject obj)
	{
	}

	public void ClickSerchCondition(IUIObject obj)
	{
		AuctionSearchDlg auctionSearchDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_SEARCH_DLG) as AuctionSearchDlg;
		if (auctionSearchDlg != null)
		{
			if (this.m_SearchOption.m_eAuctionRegisterType == AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ALL)
			{
				this.m_SearchOption.m_eAuctionRegisterType = AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ITEM;
			}
			auctionSearchDlg.SetSearchOption(this.m_SearchOption);
		}
	}

	public void ClickPrev(IUIObject obj)
	{
		int num = this.m_iCurPageNum;
		if (-1 <= num - 1)
		{
			num--;
		}
		switch (this.m_eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
			AuctionMainDlg.Send_PurchaseList(num, this.m_SearchOption, this.m_eSortType, false);
			break;
		case AuctionMainDlg.eTAB.eTAB_SELL:
			AuctionMainDlg.Send_SellList(num);
			break;
		case AuctionMainDlg.eTAB.eTAB_TENDER:
			AuctionMainDlg.Send_TenderList(num);
			break;
		}
	}

	public void ClickNext(IUIObject obj)
	{
		int num = this.m_iCurPageNum;
		num++;
		switch (this.m_eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
			AuctionMainDlg.Send_PurchaseList(num, this.m_SearchOption, this.m_eSortType, false);
			break;
		case AuctionMainDlg.eTAB.eTAB_SELL:
			AuctionMainDlg.Send_SellList(num);
			break;
		case AuctionMainDlg.eTAB.eTAB_TENDER:
			AuctionMainDlg.Send_TenderList(num);
			break;
		}
	}

	public void ClickTender(IUIObject obj)
	{
		if (null == this.m_nlbPurchaseList.SelectedItem)
		{
			return;
		}
		UIListItemContainer selectedItem = this.m_nlbPurchaseList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		if (selectedItem.Data == null)
		{
			return;
		}
		switch (this.m_eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
		{
			AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectedItem.Data as AUCTION_REGISTER_ITEM;
			if (aUCTION_REGISTER_ITEM != null)
			{
				this.SelectPurchasekListItem(aUCTION_REGISTER_ITEM);
			}
			else
			{
				AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectedItem.Data as AUCTION_REGISTER_SOL_TOTAL;
				if (aUCTION_REGISTER_SOL_TOTAL != null)
				{
					this.SelectPurchasekListSol(aUCTION_REGISTER_SOL_TOTAL);
				}
			}
			break;
		}
		case AuctionMainDlg.eTAB.eTAB_TENDER:
		{
			AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM2 = selectedItem.Data as AUCTION_REGISTER_ITEM;
			if (aUCTION_REGISTER_ITEM2 != null)
			{
				this.SelectPurchasekListItem(aUCTION_REGISTER_ITEM2);
			}
			else
			{
				AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL2 = selectedItem.Data as AUCTION_REGISTER_SOL_TOTAL;
				if (aUCTION_REGISTER_SOL_TOTAL2 != null)
				{
					this.SelectPurchasekListSol(aUCTION_REGISTER_SOL_TOTAL2);
				}
			}
			break;
		}
		}
	}

	public void ClickDirectPurchase(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbPurchaseList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(20);
		if (AuctionMainDlg.GetDailyBuyLimit() <= num)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("651"),
				"count",
				num
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		switch (this.m_eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_PURCHASE:
		{
			AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectItem.Data as AUCTION_REGISTER_ITEM;
			if (aUCTION_REGISTER_ITEM != null)
			{
				this.SelectDirectPurchasekListItem(aUCTION_REGISTER_ITEM);
			}
			else
			{
				AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectItem.Data as AUCTION_REGISTER_SOL_TOTAL;
				if (aUCTION_REGISTER_SOL_TOTAL != null)
				{
					this.SelectDirectPurchasekListSol(aUCTION_REGISTER_SOL_TOTAL);
				}
			}
			break;
		}
		case AuctionMainDlg.eTAB.eTAB_TENDER:
		{
			AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM2 = selectItem.Data as AUCTION_REGISTER_ITEM;
			if (aUCTION_REGISTER_ITEM2 != null)
			{
				this.SelectDirectPurchasekListItem(aUCTION_REGISTER_ITEM2);
			}
			else
			{
				AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL2 = selectItem.Data as AUCTION_REGISTER_SOL_TOTAL;
				if (aUCTION_REGISTER_SOL_TOTAL2 != null)
				{
					this.SelectDirectPurchasekListSol(aUCTION_REGISTER_SOL_TOTAL2);
				}
			}
			break;
		}
		}
	}

	public void ClickSelectItem(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_ITEMSELECT_DLG);
	}

	public void ClickPayType(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		if (null == toggle)
		{
			return;
		}
		this.m_ePayType = (AuctionDefine.ePAYTYPE)((int)toggle.data);
		AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, this.m_ePayType);
	}

	public static void SetChangePayTexture(DrawTexture dt1, DrawTexture dt2, AuctionDefine.ePAYTYPE ePayType)
	{
		if (ePayType != AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
			{
				dt1.SetTextureKey("Com_I_MoneyIconM");
				dt2.SetTextureKey("Com_I_MoneyIconM");
			}
		}
		else
		{
			dt1.SetTextureKey("Win_I_Hearts");
			dt2.SetTextureKey("Win_I_Hearts");
		}
	}

	public void ClickSellCost(IUIObject obj)
	{
		if (this.m_bSendAuctionTableInfo)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputSellCost), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		long i64Min = 1L;
		long costMax = AuctionMainDlg.GetCostMax(this.m_ePayType);
		inputNumberDlg.SetMinMax(i64Min, costMax);
		inputNumberDlg.SetNum(this.m_lSellCost);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickSellDirectCost(IUIObject obj)
	{
		if (this.m_bSendAuctionTableInfo)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputSellDirectCost), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		long num = AuctionMainDlg.GetDirectCostMin(this.m_ePayType, this.m_lSellCost);
		long num2 = AuctionMainDlg.GetCostMax(this.m_ePayType);
		string empty = string.Empty;
		if (0 < this.m_AuctionTableInfo.i32Unique)
		{
			if (this.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
			{
				num = this.m_AuctionTableInfo.i64MinGold;
				if (0L < this.m_AuctionTableInfo.i64MaxGold)
				{
					num2 = this.m_AuctionTableInfo.i64MaxGold;
				}
			}
			else
			{
				num = this.m_AuctionTableInfo.i64MinHearts;
				if (0L < this.m_AuctionTableInfo.i64MaxHearts)
				{
					num2 = this.m_AuctionTableInfo.i64MaxHearts;
				}
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2128"),
				"count1",
				num,
				"count2",
				num2
			});
		}
		if (string.Empty != empty)
		{
			inputNumberDlg.SetTitleText(empty);
		}
		inputNumberDlg.SetMinMax(num, num2);
		inputNumberDlg.SetNum(this.m_lSellDirectCost);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickSellRegister(IUIObject obj)
	{
		if (this.m_SelectItem != null)
		{
			AuctionSellCheckDlg auctionSellCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_SELLCHECK_DLG) as AuctionSellCheckDlg;
			if (auctionSellCheckDlg != null)
			{
				auctionSellCheckDlg.SetItemInfo(this.m_SelectItem, this.m_lSellCost, this.m_lSellDirectCost, this.m_ePayType);
				this.SetEnableControl(false);
			}
		}
		if (this.m_SelectSol != null)
		{
			if (0L < this.m_SelectSol.GetFriendPersonID())
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKUnsetSolHelp), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("156"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("155"), eMsgType.MB_OK_CANCEL, 2);
				msgBoxUI.Show();
				return;
			}
			else
			{
				AuctionSellCheckDlg auctionSellCheckDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_SELLCHECK_DLG) as AuctionSellCheckDlg;
				if (auctionSellCheckDlg2 != null)
				{
					auctionSellCheckDlg2.SetSolInfo(this.m_SelectSol, this.m_lSellCost, this.m_lSellDirectCost, this.m_ePayType);
					this.SetEnableControl(false);
				}
			}
		}
		if (this.m_SelectItem == null && this.m_SelectSol == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_ITEMSELECT_DLG);
		}
	}

	public void SetSelectItem(ITEM Item)
	{
		if (Item == null)
		{
			return;
		}
		this.m_SelectItem = Item;
		this.m_SelectSol = null;
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(this.m_SelectItem);
		this.m_itSellItem.SetItemTexture(this.m_SelectItem);
		this.m_lbSellItemName.SetText(rankColorName);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			this.m_SelectItem.m_nOption[7]
		});
		this.m_lbSellTradeCount.SetText(this.m_strText);
		this.m_lSellCost = 1L;
		this.m_lbSellCost.SetText(ANNUALIZED.Convert(this.m_lSellCost));
		this.SetRegisterCheckReq(AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ITEM, this.m_SelectItem.m_nItemUnique);
	}

	public void SetSelectSol(NkSoldierInfo Sol)
	{
		if (Sol == null)
		{
			return;
		}
		this.m_SelectItem = null;
		this.m_SelectSol = Sol;
		this.m_itSellItem.SetSolImageTexure(eCharImageType.SMALL, Sol.GetListSolInfo(true));
		this.m_lbSellItemName.SetText(Sol.GetName());
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			Sol.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT)
		});
		this.m_lbSellTradeCount.SetText(this.m_strText);
		this.m_lSellCost = 1L;
		this.m_lbSellCost.SetText(ANNUALIZED.Convert(this.m_lSellCost));
		this.SetRegisterCheckReq(AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_SOL, this.m_SelectSol.GetCharKind());
	}

	public void InitControl()
	{
		this.m_lbSellItemName.SetText(string.Empty);
		this.m_lbSellTradeCount.SetText(string.Empty);
		AuctionDefine.ePAYTYPE ePayType = this.m_ePayType;
		if (ePayType != AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
			{
				if (AuctionMainDlg.IsPayTypeMoney())
				{
					this.m_tgPayType[(int)this.m_ePayType].Value = true;
				}
			}
		}
		else if (AuctionMainDlg.IsPayTypeHearts())
		{
			this.m_tgPayType[(int)this.m_ePayType].Value = true;
		}
		this.m_lbSellCost.SetText(this.m_lSellCost.ToString());
		this.m_lbSellDirectCoust.SetText(this.m_lSellDirectCost.ToString());
		this.m_lbSellListItemNum.SetText(string.Empty);
		this.SetCommissionText();
		AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, this.m_ePayType);
	}

	public void OnInputSellCost(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		if (AuctionMainDlg.GetCostMax(this.m_ePayType) < num)
		{
			num = AuctionMainDlg.GetCostMax(this.m_ePayType);
		}
		this.m_lSellCost = num;
		this.m_lbSellCost.SetText(ANNUALIZED.Convert(this.m_lSellCost));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
		this.m_lSellDirectCost = 0L;
		this.m_lbSellDirectCoust.SetText(string.Empty);
	}

	public void OnInputSellDirectCost(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		if (AuctionMainDlg.GetCostMax(this.m_ePayType) < num)
		{
			num = AuctionMainDlg.GetCostMax(this.m_ePayType);
		}
		this.m_lSellDirectCost = num;
		this.m_lbSellDirectCoust.SetText(ANNUALIZED.Convert(this.m_lSellDirectCost));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public static long GetCostMax(AuctionDefine.ePAYTYPE ePayType)
	{
		long result = 1L;
		if (ePayType != AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
			{
				result = 99999999999L;
			}
		}
		else
		{
			result = 99999999999L;
		}
		return result;
	}

	public static long GetDirectCostMin(AuctionDefine.ePAYTYPE ePayType, long lSellCost)
	{
		long num = lSellCost;
		if (0L >= num)
		{
			num = 1L;
		}
		long costMax = AuctionMainDlg.GetCostMax(ePayType);
		if (num > costMax)
		{
			num = costMax;
		}
		return num;
	}

	public void SetEnableControl(bool bEnable)
	{
		this.m_nlbPurchaseList.controlIsEnabled = bEnable;
		this.m_nlbSellList.controlIsEnabled = bEnable;
		this.m_tbTab.controlIsEnabled = bEnable;
		this.m_btRefresh.controlIsEnabled = bEnable;
		this.m_btSerchCondition.controlIsEnabled = bEnable;
		this.m_btPrev.controlIsEnabled = bEnable;
		this.m_btNext.controlIsEnabled = bEnable;
		this.m_btTender.controlIsEnabled = bEnable;
		this.m_btPurchase.controlIsEnabled = bEnable;
		this.m_btSelectItem.controlIsEnabled = bEnable;
		this.m_tgPayType[0].controlIsEnabled = bEnable;
		this.m_tgPayType[1].controlIsEnabled = bEnable;
		this.m_btSellCost.controlIsEnabled = bEnable;
		this.m_btSellDirectCost.controlIsEnabled = bEnable;
		this.m_btSellRegister.controlIsEnabled = bEnable;
		if (!bEnable)
		{
			CallBackScheduler.Instance.RegFunc(600L, new Action(AuctionMainDlg.AutoEnableControl));
		}
	}

	public void SlideBPercentage()
	{
		TsLog.LogOnlyEditor("SlideBPercentage : " + Time.time);
	}

	public void SetPurchaseListAll(GS_AUCTION_PURCHASELIST_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbPurchaseList.Clear();
		this.m_RegisterItem.Clear();
		this.m_RegisterSol.Clear();
		this.m_RegisterSort.Clear();
		for (int i = 0; i < (int)ACK.i16ItemNum; i++)
		{
			GS_AUCTION_PURCHASELIST_ITEM packet = kDeserializePacket.GetPacket<GS_AUCTION_PURCHASELIST_ITEM>();
			if (0 < packet.RegisterItem.Item.m_nItemUnique)
			{
				this.m_RegisterItem.Add(packet.RegisterItem);
				RegisterSort registerSort = new RegisterSort();
				registerSort.m_iRank = packet.i16Rank;
				registerSort.m_lAuctionID = packet.RegisterItem.RegisterInfo.i64AuctionID;
				this.m_RegisterSort.Add(registerSort);
			}
		}
		for (int i = 0; i < (int)ACK.i16SolNum; i++)
		{
			GS_AUCTION_PURCHASELIST_SOL packet2 = kDeserializePacket.GetPacket<GS_AUCTION_PURCHASELIST_SOL>();
			if (0L < packet2.RegisterSol.SoldierInfo.SolID)
			{
				this.m_RegisterSol.Add(packet2.RegisterSol);
				RegisterSort registerSort2 = new RegisterSort();
				registerSort2.m_iRank = packet2.i16Rank;
				registerSort2.m_lAuctionID = packet2.RegisterSol.RegisterInfo.i64AuctionID;
				this.m_RegisterSort.Add(registerSort2);
			}
		}
		this.m_RegisterSort.Sort(new Comparison<RegisterSort>(this.CompareRank));
		for (int i = 0; i < this.m_RegisterSort.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.m_RegisterItem.Count; j++)
			{
				if (this.m_RegisterSort[i].m_lAuctionID == this.m_RegisterItem[j].RegisterInfo.i64AuctionID)
				{
					this.SetAddListItem(this.m_nlbPurchaseList, this.m_RegisterItem[j]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this.m_RegisterSol.Count; j++)
				{
					if (this.m_RegisterSort[i].m_lAuctionID == this.m_RegisterSol[j].RegisterInfo.i64AuctionID)
					{
						this.SetAddListSol(this.m_nlbPurchaseList, this.m_RegisterSol[j]);
						break;
					}
				}
			}
		}
		this.m_nlbPurchaseList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageNum();
		AuctionMainDlg.m_i32AuctionHeartsUse = ACK.i32AuctionHeartsUse;
		AuctionMainDlg.m_i32AuctionGoldUse = ACK.i32AuctionGoldUse;
		AuctionMainDlg.m_fAuctionCommission = ACK.fAuctionCommission;
		AuctionMainDlg.m_i32AuctionSellMaxNum = ACK.i32AuctionSellMaxNum;
		AuctionMainDlg.m_lAuctionSellPrice = ACK.lAuctionSellPrice;
		AuctionMainDlg.m_lAuctionDuration = ACK.lAuctionDuration;
		AuctionMainDlg.m_lAuctionDurationExtend = ACK.lAuctionDurationExtend;
		AuctionMainDlg.m_i32AuctionTenderMaxNum = ACK.i32AuctionTenderMaxNum;
		AuctionMainDlg.m_i32AuctionTenderShowNum = ACK.i32AuctionTenderShowNum;
		AuctionMainDlg.m_fAuctionTenderRate = ACK.fAuctionTenderRate;
		AuctionMainDlg.m_fAuctionSellPriceRate = ACK.fAuctionSellPriceRate;
		AuctionMainDlg.m_i32HeartsValue = ACK.i32HeartsValue;
		AuctionMainDlg.m_i32DailySellLimit = ACK.i32DailySellLimit;
		AuctionMainDlg.m_i32DailyBuyLimit = ACK.i32DailyBuyLimit;
		AuctionMainDlg.m_i16SolLevelLimit = ACK.i16SolLevelLimit;
		AuctionMainDlg.m_i16SolSkillLevelLimit = ACK.i16SolSkillLevelLimit;
		this.SetCommissionText();
		if (ACK.bSearchButton && ACK.i16ItemNum == 0 && ACK.i16SolNum == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("529"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		this.InitAuctionStateControl();
	}

	public void SetPurchaseListItem(GS_AUCTION_PURCHASELIST_ITEM_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbPurchaseList.Clear();
		this.m_RegisterItem.Clear();
		for (int i = 0; i < (int)ACK.i16ItemNum; i++)
		{
			AUCTION_REGISTER_ITEM packet = kDeserializePacket.GetPacket<AUCTION_REGISTER_ITEM>();
			if (0 < packet.Item.m_nItemUnique)
			{
				this.SetAddListItem(this.m_nlbPurchaseList, packet);
				this.m_RegisterItem.Add(packet);
			}
		}
		this.m_nlbPurchaseList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageNum();
		if (ACK.bSearchButton && ACK.i16ItemNum == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("529"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
	}

	public void SetAddListItem(NewListBox nlb, AUCTION_REGISTER_ITEM RegisterItem)
	{
		NewListItem newListItem = new NewListItem(nlb.ColumnNum, true, string.Empty);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(RegisterItem.Item);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, RegisterItem.Item, null, null, null);
		newListItem.SetListItemData(2, rankColorName, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			RegisterItem.Item.m_nOption[7]
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		int skillUnique = RegisterItem.Item.m_nOption[4];
		int num = RegisterItem.Item.m_nOption[5];
		this.m_strText = string.Empty;
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		if (battleSkillBase != null)
		{
			newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey), null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1142"),
				"count",
				num
			});
			newListItem.SetListItemData(5, this.m_strText, null, null, null);
		}
		else
		{
			newListItem.SetListItemData(4, this.m_strText, null, null, null);
			newListItem.SetListItemData(5, this.m_strText, null, null, null);
		}
		this.SetPyrchaseListRegisterInfo(newListItem, RegisterItem.RegisterInfo);
		newListItem.Data = RegisterItem;
		nlb.Add(newListItem);
	}

	public void SetPyrchaseListRegisterInfo(NewListItem NListItem, AUCTION_REGISTER_INFO RegisterInfo)
	{
		DateTime dueDate = PublicMethod.GetDueDate(RegisterInfo.tmExpireTime);
		DateTime dueDate2 = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
		int num = dueDate2.CompareTo(dueDate);
		if (0 > num)
		{
			TimeSpan timeSpan = dueDate - dueDate2;
			if (0 < timeSpan.Days)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strExpireTime, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1134"),
					"day",
					timeSpan.Days,
					"hour",
					timeSpan.Hours,
					"min",
					timeSpan.Minutes
				});
			}
			else if (0 < timeSpan.Hours)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strExpireTime, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1135"),
					"hour",
					timeSpan.Hours,
					"min",
					timeSpan.Minutes
				});
			}
			else if (0 < timeSpan.Minutes)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strExpireTime, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1136"),
					"min",
					timeSpan.Minutes
				});
			}
		}
		else
		{
			this.m_strExpireTime = string.Empty;
		}
		NListItem.SetListItemData(6, this.m_strExpireTime, null, null, null);
		if (0L < RegisterInfo.i64CostMoney)
		{
			UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_MoneyIconM");
			NListItem.SetListItemData(7, loader, null, null, null);
			NListItem.SetListItemData(9, ANNUALIZED.Convert(RegisterInfo.i64CostMoney), null, null, null);
		}
		else if (0 < RegisterInfo.i32CostHearts)
		{
			UIBaseInfoLoader loader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Hearts");
			NListItem.SetListItemData(7, loader2, null, null, null);
			NListItem.SetListItemData(9, ANNUALIZED.Convert(RegisterInfo.i32CostHearts), null, null, null);
		}
		else
		{
			NListItem.SetListItemData(7, false);
			NListItem.SetListItemData(9, false);
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsSamePersonID(RegisterInfo.i64TenderPersonID))
		{
			NListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1133"), null, null, null);
		}
		else
		{
			NListItem.SetListItemData(10, false);
		}
		if (0L < RegisterInfo.i64DirectCostMoney)
		{
			UIBaseInfoLoader loader3 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_MoneyIconM");
			NListItem.SetListItemData(8, loader3, null, null, null);
			NListItem.SetListItemData(11, ANNUALIZED.Convert(RegisterInfo.i64DirectCostMoney), null, null, null);
		}
		else if (0 < RegisterInfo.i32DirectCostHearts)
		{
			UIBaseInfoLoader loader4 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Hearts");
			NListItem.SetListItemData(8, loader4, null, null, null);
			NListItem.SetListItemData(11, ANNUALIZED.Convert(RegisterInfo.i32DirectCostHearts), null, null, null);
		}
		else
		{
			NListItem.SetListItemData(8, false);
			NListItem.SetListItemData(11, false);
		}
		NListItem.SetListItemData(12, string.Empty, null, new EZValueChangedDelegate(this.ClickDetailInfo), null);
	}

	public void SetPurchaseListSol(GS_AUCTION_PURCHASELIST_SOL_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbPurchaseList.Clear();
		this.m_RegisterSol.Clear();
		for (int i = 0; i < (int)ACK.i16SolNum; i++)
		{
			AUCTION_REGISTER_SOL_TOTAL packet = kDeserializePacket.GetPacket<AUCTION_REGISTER_SOL_TOTAL>();
			if (0L < packet.SoldierInfo.SolID)
			{
				this.SetAddListSol(this.m_nlbPurchaseList, packet);
				this.m_RegisterSol.Add(packet);
			}
		}
		this.m_nlbPurchaseList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageNum();
		if (ACK.bSearchButton && ACK.i16SolNum == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("529"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
	}

	public void SetAddListSol(NewListBox nlb, AUCTION_REGISTER_SOL_TOTAL RegisterSol)
	{
		int skillUnique = 0;
		int num = 0;
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(RegisterSol.SoldierInfo);
		for (int i = 0; i < 6; i++)
		{
			if (num < RegisterSol.BattleSkillData[i].BattleSkillLevel)
			{
				skillUnique = RegisterSol.BattleSkillData[i].BattleSkillUnique;
				num = RegisterSol.BattleSkillData[i].BattleSkillLevel;
			}
			nkSoldierInfo.m_kBattleSkill.BattleSkillData[i] = RegisterSol.BattleSkillData[i];
		}
		for (int i = 0; i < 16; i++)
		{
			nkSoldierInfo.m_nSolSubData[i] = RegisterSol.SolSubData[i];
		}
		NewListItem newListItem = new NewListItem(nlb.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, nkSoldierInfo.GetListSolInfo(true), null, null, null);
		string text = nkSoldierInfo.GetName();
		if (text == null)
		{
			text = string.Empty;
		}
		newListItem.SetListItemData(2, text, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT)
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		if (battleSkillBase != null)
		{
			newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey), null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1142"),
				"count",
				num
			});
			newListItem.SetListItemData(5, this.m_strText, null, null, null);
		}
		else
		{
			newListItem.SetListItemData(4, string.Empty, null, null, null);
			newListItem.SetListItemData(5, string.Empty, null, null, null);
		}
		this.SetPyrchaseListRegisterInfo(newListItem, RegisterSol.RegisterInfo);
		newListItem.Data = RegisterSol;
		nlb.Add(newListItem);
	}

	public void SetPageNum()
	{
		this.m_strPageNum = string.Format("{0}/{1}", this.m_iCurPageNum + 1, this.m_iMaxPageNum);
		this.m_lbPage.SetText(this.m_strPageNum);
	}

	public static void Send_PurchaseList(int iCurPageNum, AuctionDefine.eAUCTIONREGISTERTYPE eAuctionRegisterType)
	{
		GS_AUCTION_PURCHASELIST_REQ gS_AUCTION_PURCHASELIST_REQ = new GS_AUCTION_PURCHASELIST_REQ();
		gS_AUCTION_PURCHASELIST_REQ.i16PageNum = (short)iCurPageNum;
		gS_AUCTION_PURCHASELIST_REQ.i8RegisterType = (byte)eAuctionRegisterType;
		gS_AUCTION_PURCHASELIST_REQ.i8PayType = 2;
		gS_AUCTION_PURCHASELIST_REQ.i8SortType = 0;
		gS_AUCTION_PURCHASELIST_REQ.bSearchButton = false;
		TKString.StringChar(string.Empty, ref gS_AUCTION_PURCHASELIST_REQ.strSolName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_PURCHASELIST_REQ, gS_AUCTION_PURCHASELIST_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
	}

	public static void Send_PurchaseList(int iCurPageNum, AuctionSearchOption SearchOption, AuctionDefine.eSORT_TYPE eSortType, bool bSearchButton)
	{
		GS_AUCTION_PURCHASELIST_REQ gS_AUCTION_PURCHASELIST_REQ = new GS_AUCTION_PURCHASELIST_REQ();
		gS_AUCTION_PURCHASELIST_REQ.i16PageNum = (short)iCurPageNum;
		gS_AUCTION_PURCHASELIST_REQ.i8RegisterType = (byte)SearchOption.m_eAuctionRegisterType;
		gS_AUCTION_PURCHASELIST_REQ.i8ItemType = (byte)SearchOption.m_eItemType;
		gS_AUCTION_PURCHASELIST_REQ.i16UseMinLevel = SearchOption.m_iUseMinLevel;
		gS_AUCTION_PURCHASELIST_REQ.i16UseMaxLevel = SearchOption.m_iUseMaxLevel;
		gS_AUCTION_PURCHASELIST_REQ.i32ItemSkillUnique = SearchOption.m_iItemSkillUnique;
		gS_AUCTION_PURCHASELIST_REQ.i16ItemSkillLevel = SearchOption.m_iItemSkillLevel;
		gS_AUCTION_PURCHASELIST_REQ.i16ItemTradeCount = SearchOption.m_iItemTradeCount;
		gS_AUCTION_PURCHASELIST_REQ.i8SolSeason = SearchOption.m_bySolSeason;
		gS_AUCTION_PURCHASELIST_REQ.i16SolLevel = SearchOption.m_iSolLevel;
		TKString.StringChar(SearchOption.m_strSolName, ref gS_AUCTION_PURCHASELIST_REQ.strSolName);
		gS_AUCTION_PURCHASELIST_REQ.SolTradeCount = SearchOption.m_iSolTradeCount;
		gS_AUCTION_PURCHASELIST_REQ.i64CostMoney = SearchOption.m_lCostMoney;
		gS_AUCTION_PURCHASELIST_REQ.i64DirectCostMoney = SearchOption.m_lDirectCostMoney;
		gS_AUCTION_PURCHASELIST_REQ.i32CostHearts = SearchOption.m_iCostHearts;
		gS_AUCTION_PURCHASELIST_REQ.i32DirectCostHearts = SearchOption.m_iCostDirectHearts;
		gS_AUCTION_PURCHASELIST_REQ.i8PayType = (byte)SearchOption.m_ePayType;
		gS_AUCTION_PURCHASELIST_REQ.i8SortType = (byte)eSortType;
		gS_AUCTION_PURCHASELIST_REQ.bSearchButton = bSearchButton;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_PURCHASELIST_REQ, gS_AUCTION_PURCHASELIST_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
			auctionMainDlg.SetSortType(eSortType);
		}
	}

	public static void Send_SellList(int iCurPageNum)
	{
		GS_AUCTION_SELLLIST_REQ gS_AUCTION_SELLLIST_REQ = new GS_AUCTION_SELLLIST_REQ();
		gS_AUCTION_SELLLIST_REQ.i16PageNum = (short)iCurPageNum;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_SELLLIST_REQ, gS_AUCTION_SELLLIST_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
	}

	public static void Send_TenderList(int iCurPageNum)
	{
		GS_AUCTION_TENDERLIST_REQ gS_AUCTION_TENDERLIST_REQ = new GS_AUCTION_TENDERLIST_REQ();
		gS_AUCTION_TENDERLIST_REQ.i16PageNum = (short)iCurPageNum;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_TENDERLIST_REQ, gS_AUCTION_TENDERLIST_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
	}

	public static void Send_RegisterCancel(long lAuctionID)
	{
		GS_AUCTION_REGISTER_CANCEL_REQ gS_AUCTION_REGISTER_CANCEL_REQ = new GS_AUCTION_REGISTER_CANCEL_REQ();
		gS_AUCTION_REGISTER_CANCEL_REQ.i64AuctionID = lAuctionID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_REGISTER_CANCEL_REQ, gS_AUCTION_REGISTER_CANCEL_REQ);
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(false);
		}
	}

	public static void AutoEnableControl()
	{
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetEnableControl(true);
		}
	}

	public void SetPurchaseMaxPageNum(int iPurchaseMaxPageNum)
	{
		this.m_iMaxPageNum = iPurchaseMaxPageNum;
	}

	public void ClickPurchasekList(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbPurchaseList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectItem.Data as AUCTION_REGISTER_ITEM;
		if (aUCTION_REGISTER_ITEM != null)
		{
			if (0L >= aUCTION_REGISTER_ITEM.RegisterInfo.i64DirectCostMoney && 0 >= aUCTION_REGISTER_ITEM.RegisterInfo.i32DirectCostHearts)
			{
				this.m_btPurchase.SetEnabled(false);
			}
			else
			{
				this.m_btPurchase.SetEnabled(true);
			}
		}
		else
		{
			AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectItem.Data as AUCTION_REGISTER_SOL_TOTAL;
			if (aUCTION_REGISTER_SOL_TOTAL != null)
			{
				if (0L >= aUCTION_REGISTER_SOL_TOTAL.RegisterInfo.i64DirectCostMoney && 0 >= aUCTION_REGISTER_SOL_TOTAL.RegisterInfo.i32DirectCostHearts)
				{
					this.m_btPurchase.SetEnabled(false);
				}
				else
				{
					this.m_btPurchase.SetEnabled(true);
				}
			}
		}
	}

	public void InitControl(AuctionMainDlg.eTAB eTab)
	{
		switch (eTab)
		{
		case AuctionMainDlg.eTAB.eTAB_SELL:
			this.m_SelectItem = null;
			this.m_SelectSol = null;
			this.m_itSellItem.ClearData();
			this.m_itSellItem.Hide(true);
			this.m_lbSellItemName.SetText(string.Empty);
			this.m_lbSellTradeCount.SetText(string.Empty);
			this.m_lbSellCost.SetText(string.Empty);
			this.m_lbSellDirectCoust.SetText(string.Empty);
			this.m_lSellCost = 1L;
			this.m_lSellDirectCost = 0L;
			break;
		}
	}

	public void SelectPurchasekListItem(AUCTION_REGISTER_ITEM RegisterItem)
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (0 >= auctionUseLevel)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
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
			return;
		}
		if (RegisterItem == null)
		{
			return;
		}
		this.SetEnableControl(false);
		AuctionTenderCheckDlg auctionTenderCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_TENDERCHECK_DLG) as AuctionTenderCheckDlg;
		if (auctionTenderCheckDlg != null)
		{
			auctionTenderCheckDlg.SetTenderItem(RegisterItem);
		}
	}

	public void SelectPurchasekListSol(AUCTION_REGISTER_SOL_TOTAL RegisterSol)
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (0 >= auctionUseLevel)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
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
			return;
		}
		AuctionTenderCheckDlg auctionTenderCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_TENDERCHECK_DLG) as AuctionTenderCheckDlg;
		if (auctionTenderCheckDlg != null)
		{
			auctionTenderCheckDlg.SetTenderSol(RegisterSol);
		}
	}

	public void SetTenderList(GS_AUCTION_TENDERLIST_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbPurchaseList.Clear();
		this.m_RegisterItem.Clear();
		this.m_RegisterSol.Clear();
		this.m_Register.Clear();
		for (int i = 0; i < (int)ACK.i16ItemNum; i++)
		{
			AUCTION_REGISTER_ITEM packet = kDeserializePacket.GetPacket<AUCTION_REGISTER_ITEM>();
			if (0 < packet.Item.m_nItemUnique)
			{
				this.m_RegisterItem.Add(packet);
				this.m_Register.Add(packet.RegisterInfo.i64AuctionID);
			}
		}
		for (int i = 0; i < (int)ACK.i16SolNum; i++)
		{
			AUCTION_REGISTER_SOL_TOTAL packet2 = kDeserializePacket.GetPacket<AUCTION_REGISTER_SOL_TOTAL>();
			if (0L < packet2.SoldierInfo.SolID)
			{
				this.m_RegisterSol.Add(packet2);
				this.m_Register.Add(packet2.RegisterInfo.i64AuctionID);
			}
		}
		this.m_Register.Sort(new Comparison<long>(this.CompareAuctionID));
		for (int i = 0; i < this.m_Register.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.m_RegisterItem.Count; j++)
			{
				if (this.m_Register[i] == this.m_RegisterItem[j].RegisterInfo.i64AuctionID)
				{
					this.SetAddListItem(this.m_nlbPurchaseList, this.m_RegisterItem[j]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this.m_RegisterSol.Count; j++)
				{
					if (this.m_Register[i] == this.m_RegisterSol[j].RegisterInfo.i64AuctionID)
					{
						this.SetAddListSol(this.m_nlbPurchaseList, this.m_RegisterSol[j]);
						break;
					}
				}
			}
		}
		this.m_nlbPurchaseList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageNum();
	}

	public void SetSearchOption(AuctionSearchOption SearchOption)
	{
		this.m_SearchOption.Set(SearchOption);
	}

	public void SelectDirectPurchasekListItem(AUCTION_REGISTER_ITEM RegisterItem)
	{
		if (RegisterItem == null)
		{
			return;
		}
		if (0L >= RegisterItem.RegisterInfo.i64DirectCostMoney && 0 >= RegisterItem.RegisterInfo.i32DirectCostHearts)
		{
			return;
		}
		this.SetEnableControl(false);
		AuctionPurchaseCheckDlg auctionPurchaseCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_PURCHASECHECK_DLG) as AuctionPurchaseCheckDlg;
		if (auctionPurchaseCheckDlg != null)
		{
			auctionPurchaseCheckDlg.SetSelectInfoItem(RegisterItem);
		}
	}

	public void SelectDirectPurchasekListSol(AUCTION_REGISTER_SOL_TOTAL RegisterSol)
	{
		if (RegisterSol == null)
		{
			return;
		}
		if (0L >= RegisterSol.RegisterInfo.i64DirectCostMoney && 0 >= RegisterSol.RegisterInfo.i32DirectCostHearts)
		{
			return;
		}
		this.SetEnableControl(false);
		AuctionPurchaseCheckDlg auctionPurchaseCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_PURCHASECHECK_DLG) as AuctionPurchaseCheckDlg;
		if (auctionPurchaseCheckDlg != null)
		{
			auctionPurchaseCheckDlg.SetSelectInfoSol(RegisterSol);
		}
	}

	public void SetSellList(GS_AUCTION_SELLLIST_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbSellList.Clear();
		this.m_RegisterItem.Clear();
		this.m_RegisterSol.Clear();
		this.m_Register.Clear();
		for (int i = 0; i < (int)ACK.i16ItemNum; i++)
		{
			AUCTION_REGISTER_ITEM packet = kDeserializePacket.GetPacket<AUCTION_REGISTER_ITEM>();
			if (0 < packet.Item.m_nItemUnique)
			{
				if (0L < packet.Item.m_nItemID)
				{
					this.m_RegisterItem.Add(packet);
					this.m_Register.Add(packet.RegisterInfo.i64AuctionID);
				}
			}
		}
		for (int i = 0; i < (int)ACK.i16SolNum; i++)
		{
			AUCTION_REGISTER_SOL_TOTAL packet2 = kDeserializePacket.GetPacket<AUCTION_REGISTER_SOL_TOTAL>();
			if (0L < packet2.SoldierInfo.SolID)
			{
				this.m_RegisterSol.Add(packet2);
				this.m_Register.Add(packet2.RegisterInfo.i64AuctionID);
			}
		}
		this.m_Register.Sort(new Comparison<long>(this.CompareAuctionID));
		for (int i = 0; i < this.m_Register.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.m_RegisterItem.Count; j++)
			{
				if (this.m_Register[i] == this.m_RegisterItem[j].RegisterInfo.i64AuctionID)
				{
					this.SetAddSellListItem(this.m_nlbSellList, this.m_RegisterItem[j]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this.m_RegisterSol.Count; j++)
				{
					if (this.m_Register[i] == this.m_RegisterSol[j].RegisterInfo.i64AuctionID)
					{
						this.SetAddSellListSol(this.m_nlbSellList, this.m_RegisterSol[j]);
						break;
					}
				}
			}
		}
		this.m_nlbSellList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageNum();
	}

	public void SetAddSellListItem(NewListBox nlb, AUCTION_REGISTER_ITEM RegisterItem)
	{
		NewListItem newListItem = new NewListItem(nlb.ColumnNum, true, string.Empty);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(RegisterItem.Item);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, RegisterItem.Item, null, null, null);
		newListItem.SetListItemData(2, rankColorName, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			RegisterItem.Item.m_nOption[7]
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		this.SetAddSellSubInfo(newListItem, RegisterItem.RegisterInfo);
		newListItem.Data = RegisterItem;
		nlb.Add(newListItem);
	}

	public void SetAddSellListSol(NewListBox nlb, AUCTION_REGISTER_SOL_TOTAL RegisterSol)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(RegisterSol.SoldierInfo);
		for (int i = 0; i < 6; i++)
		{
			nkSoldierInfo.m_kBattleSkill.BattleSkillData[i] = RegisterSol.BattleSkillData[i];
		}
		for (int i = 0; i < 16; i++)
		{
			nkSoldierInfo.m_nSolSubData[i] = RegisterSol.SolSubData[i];
		}
		NewListItem newListItem = new NewListItem(nlb.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, nkSoldierInfo.GetListSolInfo(true), null, null, null);
		string text = nkSoldierInfo.GetName();
		if (text == null)
		{
			text = string.Empty;
		}
		newListItem.SetListItemData(2, text, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT)
		});
		newListItem.SetListItemData(3, this.m_strText, null, null, null);
		this.SetAddSellSubInfo(newListItem, RegisterSol.RegisterInfo);
		newListItem.Data = RegisterSol;
		nlb.Add(newListItem);
	}

	public void SetAddSellSubInfo(NewListItem NListItem, AUCTION_REGISTER_INFO RegisterInfo)
	{
		if (0L < RegisterInfo.i64CostMoney)
		{
			UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_MoneyIconM");
			NListItem.SetListItemData(4, ANNUALIZED.Convert(RegisterInfo.i64CostMoney), null, null, null);
			NListItem.SetListItemData(5, loader, null, null, null);
		}
		else if (0 < RegisterInfo.i32CostHearts)
		{
			UIBaseInfoLoader loader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Hearts");
			NListItem.SetListItemData(4, ANNUALIZED.Convert(RegisterInfo.i32CostHearts), null, null, null);
			NListItem.SetListItemData(5, loader2, null, null, null);
		}
		else
		{
			NListItem.SetListItemData(4, false);
			NListItem.SetListItemData(5, false);
		}
		if (0L < RegisterInfo.i64DirectCostMoney)
		{
			UIBaseInfoLoader loader3 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_MoneyIconM");
			NListItem.SetListItemData(6, ANNUALIZED.Convert(RegisterInfo.i64DirectCostMoney), null, null, null);
			NListItem.SetListItemData(7, loader3, null, null, null);
		}
		else if (0 < RegisterInfo.i32DirectCostHearts)
		{
			UIBaseInfoLoader loader4 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Hearts");
			NListItem.SetListItemData(6, ANNUALIZED.Convert(RegisterInfo.i32DirectCostHearts), null, null, null);
			NListItem.SetListItemData(7, loader4, null, null, null);
		}
		else
		{
			NListItem.SetListItemData(6, false);
			NListItem.SetListItemData(7, false);
		}
		NListItem.SetListItemData(8, string.Empty, null, new EZValueChangedDelegate(this.ClickRegisterCancel), null);
		NListItem.SetListItemData(9, string.Empty, null, new EZValueChangedDelegate(this.ClickDetailInfoSellList), null);
	}

	public void ClickRegisterCancel(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbSellList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		long num = 0L;
		AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectItem.Data as AUCTION_REGISTER_ITEM;
		if (aUCTION_REGISTER_ITEM != null)
		{
			num = aUCTION_REGISTER_ITEM.RegisterInfo.i64AuctionID;
			if (0L < aUCTION_REGISTER_ITEM.RegisterInfo.i64TenderPersonID)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("303"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
		}
		else
		{
			AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectItem.Data as AUCTION_REGISTER_SOL_TOTAL;
			if (aUCTION_REGISTER_SOL_TOTAL != null)
			{
				num = aUCTION_REGISTER_SOL_TOTAL.RegisterInfo.i64AuctionID;
				if (0L < aUCTION_REGISTER_SOL_TOTAL.RegisterInfo.i64TenderPersonID)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("303"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					return;
				}
			}
		}
		if (0L >= num)
		{
			return;
		}
		AuctionMainDlg.Send_RegisterCancel(num);
	}

	public void SetTenderSuccess()
	{
		if (this.m_eTab == AuctionMainDlg.eTAB.eTAB_PURCHASE)
		{
			AuctionMainDlg.Send_PurchaseList(0, this.m_SearchOption, AuctionDefine.eSORT_TYPE.eSORT_TYPE_NONE, false);
		}
		else if (this.m_eTab == AuctionMainDlg.eTAB.eTAB_TENDER)
		{
			AuctionMainDlg.Send_TenderList(0);
		}
	}

	public static bool IsPayTypeMoney()
	{
		return 0 < AuctionMainDlg.GetAuctionGoldUse();
	}

	public static bool IsPayTypeHearts()
	{
		return 0 < AuctionMainDlg.GetAuctionHeartsUse();
	}

	public override void OnClose()
	{
		base.OnClose();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_ITEMSELECT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_PURCHASECHECK_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SEARCH_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SELLCHECK_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_TENDERCHECK_DLG);
	}

	public static void ShowItemDetailInfo(ITEM Item, G_ID eGID)
	{
		ITEM iTEM = new ITEM();
		iTEM.Set(Item);
		if (iTEM != null && iTEM.m_nItemUnique > 0)
		{
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.Set_Tooltip(eGID, iTEM, null, false);
		}
	}

	public static void ShowSolDetailInfo(AUCTION_REGISTER_SOL_TOTAL Sol, Form Dlg)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(Sol);
		AuctionMainDlg.ShowSolDetailInfo(nkSoldierInfo, Dlg);
	}

	public static void ShowSolDetailInfo(NkSoldierInfo Sol, Form Dlg)
	{
		if (!Sol.IsItemReceiveData())
		{
			Sol.SetReceivedEquipItem(true);
			Sol.UpdateSoldierStatInfo();
		}
		SolDetailinfoDlg solDetailinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAILINFO_DLG) as SolDetailinfoDlg;
		if (solDetailinfoDlg != null)
		{
			solDetailinfoDlg.SetDataNotMySol(ref Sol);
			solDetailinfoDlg.SetLocationByForm(Dlg);
			solDetailinfoDlg.SetFocus();
		}
	}

	public void ClickDetailInfo(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbPurchaseList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectItem.data as AUCTION_REGISTER_ITEM;
		if (aUCTION_REGISTER_ITEM != null)
		{
			AuctionMainDlg.ShowItemDetailInfo(aUCTION_REGISTER_ITEM.Item, (G_ID)base.WindowID);
		}
		else
		{
			AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectItem.data as AUCTION_REGISTER_SOL_TOTAL;
			if (aUCTION_REGISTER_SOL_TOTAL != null)
			{
				AuctionMainDlg.ShowSolDetailInfo(aUCTION_REGISTER_SOL_TOTAL, this);
			}
		}
	}

	public void ClickDetailInfoSellList(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_nlbSellList.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		AUCTION_REGISTER_ITEM aUCTION_REGISTER_ITEM = selectItem.data as AUCTION_REGISTER_ITEM;
		if (aUCTION_REGISTER_ITEM != null)
		{
			AuctionMainDlg.ShowItemDetailInfo(aUCTION_REGISTER_ITEM.Item, (G_ID)base.WindowID);
		}
		else
		{
			AUCTION_REGISTER_SOL_TOTAL aUCTION_REGISTER_SOL_TOTAL = selectItem.data as AUCTION_REGISTER_SOL_TOTAL;
			if (aUCTION_REGISTER_SOL_TOTAL != null)
			{
				AuctionMainDlg.ShowSolDetailInfo(aUCTION_REGISTER_SOL_TOTAL, this);
			}
		}
	}

	private int CompareAuctionID(long a, long b)
	{
		return b.CompareTo(a);
	}

	private int CompareRank(RegisterSort a, RegisterSort b)
	{
		return a.m_iRank.CompareTo(b.m_iRank);
	}

	public void MsgBoxOKUnsetSolHelp(object a_oObject)
	{
		if (this.m_SelectSol == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = this.m_SelectSol.GetFriendPersonID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = this.m_SelectSol.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = this.m_SelectSol.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
	}

	public void CheckHeartsAnGoldUse()
	{
		if (!AuctionMainDlg.IsPayTypeHearts() || !AuctionMainDlg.IsPayTypeMoney())
		{
			this.m_lbSellMoneyKind.Hide(true);
			this.m_tgPayType[0].Hide(true);
			this.m_tgPayType[1].Hide(true);
			this.m_dtSellHeartsIcon.Hide(true);
			this.m_dtsellMoneyIcon.Hide(true);
		}
	}

	public static int GetAuctionHeartsUse()
	{
		return AuctionMainDlg.m_i32AuctionHeartsUse;
	}

	public static int GetAuctionGoldUse()
	{
		return AuctionMainDlg.m_i32AuctionGoldUse;
	}

	public static float GetAuctionCommission()
	{
		return AuctionMainDlg.m_fAuctionCommission;
	}

	public static int GetAuctionSellMaxNum()
	{
		return AuctionMainDlg.m_i32AuctionSellMaxNum;
	}

	public static long GetAuctionSellPrice()
	{
		return AuctionMainDlg.m_lAuctionSellPrice;
	}

	public static long GetAuctionDuration()
	{
		return AuctionMainDlg.m_lAuctionDuration;
	}

	public static long GetAuctionDuractionExtend()
	{
		return AuctionMainDlg.m_lAuctionDurationExtend;
	}

	public static int GetAuctionTenderMaxNum()
	{
		return AuctionMainDlg.m_i32AuctionTenderMaxNum;
	}

	public static int GetAuctionTenderShowNum()
	{
		return AuctionMainDlg.m_i32AuctionTenderShowNum;
	}

	public static float GetAuctionTenderRate()
	{
		return AuctionMainDlg.m_fAuctionTenderRate;
	}

	public static float GetAuctionSellPriceRate()
	{
		return AuctionMainDlg.m_fAuctionSellPriceRate;
	}

	public static int GetHeartsValue()
	{
		return AuctionMainDlg.m_i32HeartsValue;
	}

	public static int GetDailySellLimit()
	{
		return AuctionMainDlg.m_i32DailySellLimit;
	}

	public static int GetDailyBuyLimit()
	{
		return AuctionMainDlg.m_i32DailyBuyLimit;
	}

	public static short GetSolLevelLimit()
	{
		return AuctionMainDlg.m_i16SolLevelLimit;
	}

	public static short GetSolSkillLevelLimit()
	{
		return AuctionMainDlg.m_i16SolSkillLevelLimit;
	}

	public void SetSortType(AuctionDefine.eSORT_TYPE eSortType)
	{
		this.m_eSortType = eSortType;
		if (this.m_eSortType == AuctionDefine.eSORT_TYPE.eSORT_TYPE_NONE)
		{
		}
	}

	public void SetCommissionText()
	{
		float num = AuctionMainDlg.GetAuctionCommission();
		num *= 100f;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1070"),
			"count",
			num
		});
		this.m_lbCommission.SetText(this.m_strText);
	}

	public void SetRegisterCheckReq(AuctionDefine.eAUCTIONREGISTERTYPE RegisterType, int iUnique)
	{
		GS_AUCTION_REGISTERCHECK_REQ gS_AUCTION_REGISTERCHECK_REQ = new GS_AUCTION_REGISTERCHECK_REQ();
		gS_AUCTION_REGISTERCHECK_REQ.i8Type = (byte)RegisterType;
		gS_AUCTION_REGISTERCHECK_REQ.i32Unique = iUnique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_REGISTERCHECK_REQ, gS_AUCTION_REGISTERCHECK_REQ);
		this.m_bSendAuctionTableInfo = true;
	}

	public void SetRegisterCheckACK(GS_AUCTION_REGISTERCHECK_ACK ACK)
	{
		this.m_AuctionTableInfo = ACK.AuctionTableInfo;
		this.m_bSendAuctionTableInfo = false;
	}

	public void SetHP_AuthMessageBox()
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2264");
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2265");
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromInterface, textFromInterface2, eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2269"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("415"));
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
	}

	public void InitAuctionStateControl()
	{
		if (!this.m_bInitAuctionStateControl)
		{
			return;
		}
		this.m_bInitAuctionStateControl = false;
		if (!AuctionMainDlg.IsPayTypeHearts())
		{
			this.m_lbHeartsNum.Visible = false;
			this.m_dtHeartsIcon.Visible = false;
			this.m_tgPayType[0].Visible = false;
			this.m_tgPayType[1].SetToggleState(true);
			this.m_dtSellHeartsIcon.Visible = false;
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, this.m_ePayType);
		}
		if (!AuctionMainDlg.IsPayTypeMoney())
		{
			this.m_lbMoney.Visible = false;
			this.m_dtMoneyIcon.Visible = false;
			this.m_tgPayType[1].Visible = false;
			this.m_tgPayType[0].SetToggleState(true);
			this.m_dtsellMoneyIcon.Visible = false;
			this.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, this.m_ePayType);
		}
		this.CheckHeartsAnGoldUse();
	}

	public void OnClickBack(object a_oObject)
	{
		this.Close();
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.MAINMENU_DLG);
	}
}
