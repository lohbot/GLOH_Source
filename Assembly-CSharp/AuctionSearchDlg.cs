using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class AuctionSearchDlg : Form
{
	private Toolbar m_tbTab;

	private Button m_btCost;

	private Label m_lbCost;

	private Button m_btDirectCost;

	private Label m_lbDirectCost;

	private CheckBox m_cbHearts;

	private CheckBox m_cbMoney;

	private DrawTexture m_dtCostIconHearts;

	private DrawTexture m_dtCostIconMoney;

	private Button m_btSearch;

	private Label m_lbCostKind;

	private DrawTexture m_dtCostIcon1;

	private DrawTexture m_dtCostIcon2;

	private DropDownList m_dlItemType;

	private Button m_btUseMinLevel;

	private Label m_lbUseMinLevel;

	private Button m_btUseMaxLevel;

	private Label m_lbUseMaxLevel;

	private DropDownList m_dlItemOption;

	private Button m_btItemSkillLevel;

	private Label m_lbItemSkillLevel;

	private Button m_btItemTradeCount;

	private Label m_lbItemTradeCount;

	private DropDownList m_dlSolSeason;

	private Button m_btSolLevel;

	private Label m_lbSolLevel;

	private TextField m_tfSolName;

	private Button m_btSolTradeCount;

	private Label m_lbSolTradeCount;

	private AuctionSearchOption m_SearchOption = new AuctionSearchOption();

	private string m_strText = string.Empty;

	private List<byte> m_SeasonData = new List<byte>();

	private Dictionary<int, ITEMTYPE_INFO> m_ItemTypeInfo = new Dictionary<int, ITEMTYPE_INFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionSearch", G_ID.AUCTION_SEARCH_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1368");
		string text = string.Empty;
		this.m_dtCostIcon1 = (base.GetControl("Icn_Money01") as DrawTexture);
		this.m_dtCostIcon2 = (base.GetControl("Icn_Money02") as DrawTexture);
		this.m_tbTab = (base.GetControl("ToolBar") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("451");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
		UIPanelTab expr_AE = this.m_tbTab.Control_Tab[0];
		expr_AE.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_AE.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_DC = this.m_tbTab.Control_Tab[1];
		expr_DC.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_DC.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_btCost = (base.GetControl("Button_AuctionCost") as Button);
		this.m_btCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCost));
		this.m_lbCost = (base.GetControl("Label_AuctionCost") as Label);
		this.m_btDirectCost = (base.GetControl("Button_DirectCost") as Button);
		this.m_btDirectCost.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDirectCost));
		this.m_lbDirectCost = (base.GetControl("Label_DirectCost") as Label);
		this.m_cbHearts = (base.GetControl("Toggle_MoneyKind01") as CheckBox);
		this.m_cbHearts.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickPayTypeHearts));
		this.m_cbHearts.SetCheckState(1);
		this.m_cbMoney = (base.GetControl("Toggle_MoneyKind02") as CheckBox);
		this.m_cbMoney.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickPayTypeMoney));
		this.m_cbMoney.SetCheckState(1);
		this.m_btSearch = (base.GetControl("Button_OK") as Button);
		this.m_btSearch.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSearch));
		this.m_dtCostIconHearts = (base.GetControl("Icon_MoneyKindHearts") as DrawTexture);
		this.m_dtCostIconMoney = (base.GetControl("Icon_MoneyKind_Gold") as DrawTexture);
		this.m_lbCostKind = (base.GetControl("Label_MoneyKind") as Label);
		this.m_dlItemType = (base.GetControl("DropDownList_Base01") as DropDownList);
		this.m_dlItemType.AddItem(this.m_strText, eITEM_TYPE.ITEMTYPE_NONE);
		this.m_ItemTypeInfo.Clear();
		for (int i = 1; i <= 20; i++)
		{
			ITEMTYPE_INFO itemTypeInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(i.ToString());
			if (itemTypeInfo != null)
			{
				if (0 < itemTypeInfo.AuctionSearch)
				{
					this.m_ItemTypeInfo.Add(itemTypeInfo.AuctionSearch, itemTypeInfo);
				}
			}
		}
		foreach (ITEMTYPE_INFO current in this.m_ItemTypeInfo.Values)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(current.TEXTKEY);
			if (!(string.Empty == text))
			{
				this.m_dlItemType.AddItem(text, current);
			}
		}
		this.m_dlItemType.SetViewArea(this.m_dlItemType.Count);
		this.m_dlItemType.RepositionItems();
		this.m_dlItemType.SetFirstItem();
		this.m_dlItemType.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeItemType));
		this.m_btUseMinLevel = (base.GetControl("Button_EquipLv01") as Button);
		this.m_btUseMinLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseMinLevel));
		this.m_lbUseMinLevel = (base.GetControl("Label_EquipLv01") as Label);
		this.m_btUseMaxLevel = (base.GetControl("Button_EquipLv02") as Button);
		this.m_btUseMaxLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseMaxLevel));
		this.m_lbUseMaxLevel = (base.GetControl("Label_EquipLv02") as Label);
		this.m_dlItemOption = (base.GetControl("DropDownList_Option01") as DropDownList);
		this.m_dlItemOption.SetViewArea(this.m_dlItemOption.Count);
		this.m_dlItemOption.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeItemOption));
		this.m_btItemSkillLevel = (base.GetControl("Button_Option01") as Button);
		this.m_btItemSkillLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemSkillLevel));
		this.m_lbItemSkillLevel = (base.GetControl("Label_Option01") as Label);
		this.m_btItemTradeCount = (base.GetControl("Button_TradeCount") as Button);
		this.m_btItemTradeCount.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickItemTradeCount));
		this.m_lbItemTradeCount = (base.GetControl("Label_TradeCount") as Label);
		this.m_dlSolSeason = (base.GetControl("DropDownList_Base02") as DropDownList);
		this.m_dlSolSeason.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1317"), 0);
		this.m_SeasonData.Add(0);
		List<SOL_GUIDE> valueAllSeason = NrTSingleton<NrTableSolGuideManager>.Instance.GetValueAllSeason();
		for (int i = 0; i < valueAllSeason.Count; i++)
		{
			if (0 < valueAllSeason[i].m_bSeason)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
					"count",
					valueAllSeason[i].m_bSeason
				});
				this.m_dlSolSeason.AddItem(text, valueAllSeason[i].m_bSeason);
				this.m_SeasonData.Add(valueAllSeason[i].m_bSeason);
			}
		}
		this.m_dlSolSeason.SetViewArea(this.m_dlSolSeason.Count);
		this.m_dlSolSeason.RepositionItems();
		this.m_dlSolSeason.SetFirstItem();
		this.m_dlSolSeason.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSolSeason));
		this.m_btSolLevel = (base.GetControl("Button_EquipLv03") as Button);
		this.m_btSolLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolLevel));
		this.m_lbSolLevel = (base.GetControl("Label_EquipLv03") as Label);
		this.m_tfSolName = (base.GetControl("TextField_SearchName") as TextField);
		this.m_btSolTradeCount = (base.GetControl("Button_TradeCount01") as Button);
		this.m_btSolTradeCount.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolTradeCount));
		this.m_lbSolTradeCount = (base.GetControl("Label_TradeCount1") as Label);
		if (!AuctionMainDlg.IsPayTypeHearts())
		{
			this.m_cbHearts.Hide(true);
			this.m_dtCostIconHearts.Hide(true);
			this.m_cbMoney.SetCheckState(1);
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD);
		}
		if (!AuctionMainDlg.IsPayTypeMoney())
		{
			this.m_cbMoney.Hide(true);
			this.m_dtCostIconMoney.Hide(true);
			this.m_cbHearts.SetCheckState(1);
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS);
		}
		if (!AuctionMainDlg.IsPayTypeHearts() || !AuctionMainDlg.IsPayTypeMoney())
		{
			this.m_lbCostKind.Hide(true);
			this.m_cbHearts.Hide(true);
			this.m_cbMoney.Hide(true);
			this.m_dtCostIconHearts.Hide(true);
			this.m_dtCostIconMoney.Hide(true);
		}
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickPayTypeHearts(IUIObject obj)
	{
		if (this.m_SearchOption == null)
		{
			return;
		}
		this.ClickPayType();
	}

	public void ClickPayTypeMoney(IUIObject obj)
	{
		if (this.m_SearchOption == null)
		{
			return;
		}
		this.ClickPayType();
	}

	public void ClickPayType()
	{
		if (null == this.m_cbHearts || null == this.m_cbMoney)
		{
			return;
		}
		if (this.m_cbHearts.IsChecked())
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
			this.m_SearchOption.m_lCostMoney = 0L;
			this.m_SearchOption.m_lDirectCostMoney = 0L;
			this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_iCostHearts));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_iCostDirectHearts));
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS);
		}
		else if (this.m_cbMoney.IsChecked())
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
			this.m_SearchOption.m_iCostHearts = 0;
			this.m_SearchOption.m_iCostDirectHearts = 0;
			this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lCostMoney));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lDirectCostMoney));
			AuctionMainDlg.SetChangePayTexture(this.m_dtCostIcon1, this.m_dtCostIcon2, AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD);
		}
		bool flag = false;
		if (this.m_cbHearts.IsChecked() && this.m_cbMoney.IsChecked())
		{
			flag = true;
		}
		if (!this.m_cbHearts.IsChecked() && !this.m_cbMoney.IsChecked())
		{
			flag = true;
		}
		if (flag)
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_ALL;
			this.m_SearchOption.m_lCostMoney = 0L;
			this.m_SearchOption.m_lDirectCostMoney = 0L;
			this.m_SearchOption.m_iCostHearts = 0;
			this.m_SearchOption.m_iCostDirectHearts = 0;
			this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lCostMoney));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lDirectCostMoney));
		}
	}

	public void ClickUseMinLevel(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputUseMinLevel), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 200L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iUseMinLevel);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickUseMaxLevel(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputUseMaxLevel), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 200L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iUseMaxLevel);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickItemSkillLevel(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputItemSkillLevel), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 200L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iItemSkillLevel);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickItemTradeCount(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputItemTradeCount), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 10000L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iItemTradeCount);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickCost(IUIObject obj)
	{
		if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputCostMoney), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
			inputNumberDlg.SetMinMax(1L, AuctionMainDlg.GetCostMax(this.m_SearchOption.m_ePayType));
			inputNumberDlg.SetNum(this.m_SearchOption.m_lCostMoney);
			inputNumberDlg.SetInputNum(0L);
		}
		else if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			InputNumberDlg inputNumberDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			inputNumberDlg2.SetCallback(new Action<InputNumberDlg, object>(this.OnInputCostHearts), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
			inputNumberDlg2.SetMinMax(1L, AuctionMainDlg.GetCostMax(this.m_SearchOption.m_ePayType));
			inputNumberDlg2.SetNum((long)this.m_SearchOption.m_iCostHearts);
			inputNumberDlg2.SetInputNum(0L);
		}
		else if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_ALL)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("307"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
	}

	public void ClickDirectCost(IUIObject obj)
	{
		if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputDirectCostMoney), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
			inputNumberDlg.SetMinMax(1L, AuctionMainDlg.GetCostMax(this.m_SearchOption.m_ePayType));
			inputNumberDlg.SetNum(this.m_SearchOption.m_lCostMoney);
			inputNumberDlg.SetInputNum(0L);
		}
		else if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			InputNumberDlg inputNumberDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			inputNumberDlg2.SetCallback(new Action<InputNumberDlg, object>(this.OnInputDirectCostHearts), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
			inputNumberDlg2.SetMinMax(1L, AuctionMainDlg.GetCostMax(this.m_SearchOption.m_ePayType));
			inputNumberDlg2.SetNum(this.m_SearchOption.m_lDirectCostMoney);
			inputNumberDlg2.SetInputNum(0L);
		}
		else if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_ALL)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("307"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
	}

	public void ClickSearch(IUIObject obj)
	{
		if (string.Empty != this.m_tfSolName.Text)
		{
		}
		this.m_SearchOption.m_strSolName = this.m_tfSolName.Text;
		if (this.m_cbHearts.IsChecked() && this.m_cbMoney.IsChecked())
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_ALL;
		}
		else if (this.m_cbHearts.IsChecked())
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
		}
		else if (this.m_cbMoney.IsChecked())
		{
			this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
		}
		AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
		if (auctionMainDlg != null)
		{
			auctionMainDlg.SetSearchOption(this.m_SearchOption);
		}
		AuctionMainDlg.Send_PurchaseList(0, this.m_SearchOption, AuctionDefine.eSORT_TYPE.eSORT_TYPE_NONE, true);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SEARCH_DLG);
	}

	public void ClickSolLevel(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputSolLevel), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 200L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iSolLevel);
		inputNumberDlg.SetInputNum(0L);
	}

	public void ClickSolTradeCount(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputSolTradeCout), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		inputNumberDlg.SetMinMax(1L, 1000L);
		inputNumberDlg.SetNum((long)this.m_SearchOption.m_iSolTradeCount);
		inputNumberDlg.SetInputNum(0L);
	}

	public void InitControl()
	{
	}

	public void OnChangeItemType(IUIObject obj)
	{
		this.m_SearchOption.m_eItemType = eITEM_TYPE.ITEMTYPE_NONE;
		ITEMTYPE_INFO iTEMTYPE_INFO = null;
		if (this.m_dlItemType.Count > 0 && this.m_dlItemType.SelectedItem != null)
		{
			ListItem listItem = this.m_dlItemType.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				iTEMTYPE_INFO = (listItem.Key as ITEMTYPE_INFO);
				if (iTEMTYPE_INFO != null)
				{
					this.m_SearchOption.m_eItemType = (eITEM_TYPE)iTEMTYPE_INFO.ITEMTYPE;
				}
			}
		}
		this.SetItemOption(iTEMTYPE_INFO, 0);
	}

	public void SetItemOption(ITEMTYPE_INFO ItemTypeInfo, int iSelectBattleSkillUnique)
	{
		this.m_dlItemOption.Clear();
		this.m_dlItemOption.AddItem(this.m_strText, 0);
		int index = 0;
		List<ITEMSKILL_INFO> list;
		if (ItemTypeInfo != null)
		{
			list = NrTSingleton<NrItemSkillInfoManager>.Instance.GetValueFromItemType((eITEM_TYPE)ItemTypeInfo.ITEMTYPE);
		}
		else
		{
			list = NrTSingleton<NrItemSkillInfoManager>.Instance.GetValueFromAll();
		}
		string text = string.Empty;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].iAuctionSearch == 1)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(list[i].SkillUnique);
				if (battleSkillBase != null)
				{
					if (ItemTypeInfo != null)
					{
						text = battleSkillBase.m_waSkillName;
					}
					else
					{
						int eItemType = (int)list[i].m_eItemType;
						ITEMTYPE_INFO itemTypeInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(eItemType.ToString());
						if (itemTypeInfo == null)
						{
							goto IL_149;
						}
						text = string.Format("{0} ({1})", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey), NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemTypeInfo.TEXTKEY));
					}
					this.m_dlItemOption.AddItem(text, list[i].SkillUnique);
					if (0 < iSelectBattleSkillUnique && iSelectBattleSkillUnique == list[i].SkillUnique)
					{
						index = i + 1;
					}
				}
			}
			IL_149:;
		}
		this.m_dlItemOption.SetViewArea(this.m_dlItemOption.Count);
		this.m_dlItemOption.RepositionItems();
		this.m_dlItemOption.SetIndex(index);
	}

	public void OnChangeItemOption(IUIObject obj)
	{
		this.m_SearchOption.m_iItemSkillUnique = 0;
		if (this.m_dlItemOption.Count > 0 && this.m_dlItemOption.SelectedItem != null)
		{
			ListItem listItem = this.m_dlItemOption.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_SearchOption.m_iItemSkillUnique = (int)listItem.Key;
			}
		}
	}

	public void OnChangeSolSeason(IUIObject obj)
	{
		if (this.m_dlSolSeason.Count > 0 && this.m_dlSolSeason.SelectedItem != null)
		{
			ListItem listItem = this.m_dlSolSeason.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_SearchOption.m_bySolSeason = (byte)listItem.Key;
			}
		}
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		if (this.m_SearchOption == null)
		{
			return;
		}
		this.m_SearchOption.m_eAuctionRegisterType = (AuctionDefine.eAUCTIONREGISTERTYPE)uIPanelTab.panel.index;
		this.SelectTab();
	}

	public void SelectTab()
	{
		this.ChangeTab();
		AuctionDefine.eAUCTIONREGISTERTYPE eAuctionRegisterType = this.m_SearchOption.m_eAuctionRegisterType;
		if (eAuctionRegisterType != AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ITEM)
		{
			if (eAuctionRegisterType == AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_SOL)
			{
				this.ShowSolInfo();
			}
		}
		else
		{
			this.ShowItemInfo();
		}
	}

	public void ChangeTab()
	{
	}

	public void ShowItemInfo()
	{
		base.ShowLayer(1);
		this.m_tbTab.SetSelectTabIndex(0);
		this.m_dlItemType.SetVisible(true);
		this.m_dlItemOption.SetVisible(true);
		this.m_dlSolSeason.SetVisible(false);
	}

	public void ShowSolInfo()
	{
		base.ShowLayer(2);
		this.m_tbTab.SetSelectTabIndex(1);
		this.m_dlItemType.SetVisible(false);
		this.m_dlItemOption.SetVisible(false);
		this.m_dlSolSeason.SetVisible(true);
	}

	public void SetSearchOption(AuctionSearchOption SearchOption)
	{
		this.m_SearchOption.Set(SearchOption);
		this.m_lbUseMinLevel.SetText(this.m_SearchOption.m_iUseMinLevel.ToString());
		this.m_lbUseMaxLevel.SetText(this.m_SearchOption.m_iUseMaxLevel.ToString());
		this.m_lbItemSkillLevel.SetText(this.m_SearchOption.m_iItemSkillLevel.ToString());
		this.m_lbItemTradeCount.SetText(this.m_SearchOption.m_iItemTradeCount.ToString());
		this.m_lbSolLevel.SetText(this.m_SearchOption.m_iSolLevel.ToString());
		this.m_tfSolName.SetText(this.m_SearchOption.m_strSolName);
		this.m_lbSolTradeCount.SetText(this.m_SearchOption.m_iSolTradeCount.ToString());
		if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lCostMoney));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_lDirectCostMoney));
			this.m_cbMoney.SetCheckState(1);
			this.m_cbHearts.SetCheckState(0);
		}
		else if (this.m_SearchOption.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_iCostHearts));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_SearchOption.m_iCostDirectHearts));
			this.m_cbMoney.SetCheckState(0);
			this.m_cbHearts.SetCheckState(1);
		}
		else
		{
			long num = 0L;
			long num2 = 0L;
			if (AuctionMainDlg.IsPayTypeMoney())
			{
				this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;
				num = this.m_SearchOption.m_lCostMoney;
				num2 = this.m_SearchOption.m_lDirectCostMoney;
			}
			else if (AuctionMainDlg.IsPayTypeHearts())
			{
				this.m_SearchOption.m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS;
				num = (long)this.m_SearchOption.m_iCostHearts;
				num2 = (long)this.m_SearchOption.m_iCostDirectHearts;
			}
			this.m_lbCost.SetText(ANNUALIZED.Convert(num));
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(num2));
			this.m_cbMoney.SetCheckState(1);
			this.m_cbHearts.SetCheckState(1);
		}
		int num3 = 0;
		if (eITEM_TYPE.ITEMTYPE_NONE < this.m_SearchOption.m_eItemType)
		{
			num3 = (int)this.m_SearchOption.m_eItemType;
			this.m_dlItemType.SetIndex(num3);
		}
		ITEMTYPE_INFO itemTypeInfo = null;
		if (0 < num3)
		{
			itemTypeInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(num3.ToString());
		}
		this.SetItemOption(itemTypeInfo, this.m_SearchOption.m_iItemSkillUnique);
		for (int i = 0; i < this.m_SeasonData.Count; i++)
		{
			if (this.m_SeasonData[i] == this.m_SearchOption.m_bySolSeason)
			{
				this.m_dlSolSeason.SetIndex(i);
				break;
			}
		}
		this.SelectTab();
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputUseMinLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		short num = (short)a_cForm.GetNum();
		if (this.m_SearchOption.m_iUseMaxLevel < num)
		{
			this.m_SearchOption.m_iUseMaxLevel = num;
			this.m_lbUseMaxLevel.SetText(num.ToString());
		}
		this.m_SearchOption.m_iUseMinLevel = num;
		this.m_lbUseMinLevel.SetText(num.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputUseMaxLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		short num = (short)a_cForm.GetNum();
		if (num < this.m_SearchOption.m_iUseMinLevel)
		{
			this.m_SearchOption.m_iUseMinLevel = num;
			this.m_lbUseMinLevel.SetText(num.ToString());
		}
		this.m_SearchOption.m_iUseMaxLevel = num;
		this.m_lbUseMaxLevel.SetText(num.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputItemSkillLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		short iItemSkillLevel = (short)a_cForm.GetNum();
		this.m_SearchOption.m_iItemSkillLevel = iItemSkillLevel;
		this.m_lbItemSkillLevel.SetText(iItemSkillLevel.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputItemTradeCount(InputNumberDlg a_cForm, object a_oObject)
	{
		short iItemTradeCount = (short)a_cForm.GetNum();
		this.m_SearchOption.m_iItemTradeCount = iItemTradeCount;
		this.m_lbItemTradeCount.SetText(iItemTradeCount.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputCostMoney(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		this.m_SearchOption.m_lCostMoney = num;
		this.m_lbCost.SetText(ANNUALIZED.Convert(num));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputCostHearts(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		this.m_SearchOption.m_iCostHearts = num;
		this.m_lbCost.SetText(ANNUALIZED.Convert(num));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputDirectCostMoney(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		this.m_SearchOption.m_lDirectCostMoney = num;
		this.m_lbDirectCost.SetText(ANNUALIZED.Convert(num));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputDirectCostHearts(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		this.m_SearchOption.m_iCostDirectHearts = num;
		this.m_lbDirectCost.SetText(ANNUALIZED.Convert(num));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputSolLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		short iSolLevel = (short)a_cForm.GetNum();
		this.m_SearchOption.m_iSolLevel = iSolLevel;
		this.m_lbSolLevel.SetText(iSolLevel.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnInputSolTradeCout(InputNumberDlg a_cForm, object a_oObject)
	{
		short iSolTradeCount = (short)a_cForm.GetNum();
		this.m_SearchOption.m_iSolTradeCount = iSolTradeCount;
		this.m_lbSolTradeCount.SetText(iSolTradeCount.ToString());
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}
}
