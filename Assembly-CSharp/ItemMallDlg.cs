using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class ItemMallDlg : Form
{
	public enum eMODE
	{
		eMODE_NORMAL,
		eMODE_VOUCHER,
		eMODE_VOUCHER_HERO
	}

	private const ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.ITEMMALL;

	private const float DELAY_TIME = 1f;

	private NewListBox m_ListTab;

	private NewListBox m_ListBox;

	private DrawTexture m_DT_Innerbg;

	private DrawTexture m_dtVIP;

	private Label m_lbGold;

	private Label m_lbHearts;

	private Label m_lbSoulGems;

	private Label m_lbMythElxires;

	private Button m_btGoldState1;

	private Button m_btGoldState2;

	private Button m_btHeartsState1;

	private Button m_btHeartsState2;

	private Button m_btSoulGemState;

	private Button m_btSoulGemAdd;

	private Button m_btMythElxirState;

	private Button m_btMythElxirAdd;

	protected Toolbar m_tbTab;

	private Box m_BoxVoucherNotice;

	private Box m_BoxVoucherNotice2;

	private bool m_bVoucherNotice;

	private bool m_bVoucherNotice2;

	private Label m_lbVoucherInfo;

	private NewListBox m_nlbVoucher;

	protected NewListBox m_nlbVoucherPremium;

	private Label m_lbvip_level;

	private Label m_lbvip_exp;

	private DrawTexture m_DT_VipDrawTextureBg1;

	private DrawTexture m_DT_VipDrawTextureBg2;

	private Button m_BT_VIP_info;

	private int m_HeartsCount;

	private int m_SoulGemsCount;

	private int m_MythElxiresCount;

	private long m_Money;

	private UIButton _Touch;

	private int guideitemmallID = -1;

	private int guideWinID = -1;

	private bool m_bCloseEnable = true;

	private bool m_bTrading;

	private static ITEM_MALL_ITEM m_SelectItem;

	private NrMyCharInfo m_pkMychar;

	public bool m_bVipLevelUp;

	private long m_VipLevelUpdateTime;

	private byte m_i8CurrVipLevel;

	private eITEMMALL_TYPE m_ShowTab = eITEMMALL_TYPE.END;

	private static eITEMMALL_TYPE m_FirstTab = eITEMMALL_TYPE.END;

	private string m_strText = string.Empty;

	private string m_strTime = string.Empty;

	protected ItemMallDlg.eMODE m_eMode;

	private float m_fDelayTime;

	private long m_UpdateTime;

	public bool CloseEnable
	{
		get
		{
			return this.m_bCloseEnable;
		}
		set
		{
			this.m_bCloseEnable = value;
		}
	}

	public bool Trading
	{
		get
		{
			return this.m_bTrading;
		}
		set
		{
			this.m_bTrading = value;
		}
	}

	public static eITEMMALL_TYPE FirstTab
	{
		get
		{
			if (ItemMallDlg.m_FirstTab != eITEMMALL_TYPE.END)
			{
				return ItemMallDlg.m_FirstTab;
			}
			for (int i = 1; i < 10; i++)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab(i))
				{
					ItemMallDlg.m_FirstTab = (eITEMMALL_TYPE)i;
					break;
				}
			}
			return ItemMallDlg.m_FirstTab;
		}
	}

	public ItemMallDlg.eMODE MODE
	{
		get
		{
			return this.m_eMode;
		}
		private set
		{
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_itemmall", G_ID.ITEMMALL_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbGold = (base.GetControl("Label_gold") as Label);
		this.m_lbHearts = (base.GetControl("Label_hearts") as Label);
		this.m_lbSoulGems = (base.GetControl("Label_SoulGem") as Label);
		this.m_lbMythElxires = (base.GetControl("Label_MythElixir") as Label);
		this.m_btGoldState1 = (base.GetControl("Button_goldState1") as Button);
		this.m_btGoldState2 = (base.GetControl("Button_goldState2") as Button);
		this.m_btHeartsState1 = (base.GetControl("Button_heartsState1") as Button);
		this.m_btHeartsState2 = (base.GetControl("Button_heartsState2") as Button);
		this.m_btSoulGemState = (base.GetControl("Button_SoulGemState1") as Button);
		this.m_btSoulGemAdd = (base.GetControl("Button_SoulGem2") as Button);
		this.m_btMythElxirState = (base.GetControl("Button_MythElixirState1") as Button);
		this.m_btMythElxirAdd = (base.GetControl("Button_MythElixir2") as Button);
		this.m_btGoldState1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtGoldState));
		this.m_btGoldState2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtGoldState));
		this.m_btHeartsState1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btHeartsState2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btSoulGemState.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btSoulGemAdd.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btMythElxirState.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btMythElxirAdd.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_ListTab = (base.GetControl("NLB_ItemMallTab") as NewListBox);
		this.m_ListTab.Reserve = false;
		this.m_BoxVoucherNotice = (base.GetControl("Box_Notice") as Box);
		this.m_BoxVoucherNotice.SetLocation(this.m_BoxVoucherNotice.GetLocationX(), this.m_BoxVoucherNotice.GetLocationY(), -20f);
		this.m_BoxVoucherNotice.Visible = false;
		this.m_BoxVoucherNotice2 = (base.GetControl("Box_Notice2") as Box);
		this.m_BoxVoucherNotice2.SetLocation(this.m_BoxVoucherNotice2.GetLocationX(), this.m_BoxVoucherNotice2.GetLocationY(), -20f);
		this.m_BoxVoucherNotice2.Visible = false;
		int num = 0;
		for (int i = 1; i < 10; i++)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab(i))
			{
				num++;
			}
		}
		int num2 = (int)this.m_ListTab.GetSize().x;
		int num3 = (int)this.m_ListTab.itemSpacing;
		int num4 = (num2 - (num - 1) * num3 - 4) / num;
		int num5 = 0;
		if (num4 != (int)this.m_ListTab.LineHeight)
		{
			num5 = (num4 - (int)this.m_ListTab.LineHeight) / 2;
		}
		this.m_ListTab.LineHeight = (float)num4;
		this.ShowHLTap();
		for (int j = 0; j < this.m_ListTab.Count; j++)
		{
			UIListItemContainer item = this.m_ListTab.GetItem(j);
			UIRadioBtn componentInChildren = item.GetComponentInChildren<UIRadioBtn>();
			componentInChildren.SetSize((float)num4, componentInChildren.GetSize().y);
			Label[] componentsInChildren = item.GetComponentsInChildren<Label>();
			int num6 = 0;
			while (j < componentsInChildren.Length)
			{
				componentsInChildren[num6].SetLocation(componentsInChildren[num6].GetLocationX() + (float)num5, componentsInChildren[num6].GetLocationY());
				j++;
			}
			DrawTexture[] componentsInChildren2 = item.GetComponentsInChildren<DrawTexture>();
			int num7 = 0;
			while (j < componentsInChildren.Length)
			{
				componentsInChildren2[num7].SetLocation(componentsInChildren2[num7].GetLocationX() + (float)num5, componentsInChildren2[num7].GetLocationY());
				j++;
			}
		}
		this.m_ListTab.RepositionItems();
		this.m_HeartsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		this.m_lbHearts.SetText(ANNUALIZED.Convert(this.m_HeartsCount));
		this.m_Money = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
		this.m_lbGold.SetText(ANNUALIZED.Convert(this.m_Money));
		this.m_SoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
		this.m_lbSoulGems.SetText(ANNUALIZED.Convert(this.m_SoulGemsCount));
		this.m_MythElxiresCount = NkUserInventory.GetInstance().Get_First_ItemCnt(50311);
		this.m_lbMythElxires.SetText(ANNUALIZED.Convert(this.m_MythElxiresCount));
		this.m_ListBox = (base.GetControl("NLB_Shop") as NewListBox);
		this.m_ListBox.Reserve = false;
		this.m_tbTab = (base.GetControl("ToolBar_ItemMall") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450");
		UIPanelTab expr_508 = this.m_tbTab.Control_Tab[0];
		expr_508.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_508.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2726");
		UIPanelTab expr_558 = this.m_tbTab.Control_Tab[1];
		expr_558.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_558.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
		this.m_tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2781");
		UIPanelTab expr_5A8 = this.m_tbTab.Control_Tab[2];
		expr_5A8.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_5A8.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVoucherLimit())
		{
			this.m_tbTab.Visible = false;
			this.m_tbTab.Control_Tab[0].Visible = false;
			this.m_tbTab.Control_Tab[1].Visible = false;
			this.m_tbTab.Control_Tab[2].Visible = false;
		}
		this.m_lbVoucherInfo = (base.GetControl("Label_Text01") as Label);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2728"),
			"hour",
			NrTSingleton<ItemMallItemManager>.Instance.VoucherRefillTime
		});
		this.m_lbVoucherInfo.SetText(this.m_strText);
		this.m_nlbVoucher = (base.GetControl("NewListBox_Voucher") as NewListBox);
		this.m_nlbVoucher.Reserve = false;
		this.m_nlbVoucherPremium = (base.GetControl("NewListBox_Premium") as NewListBox);
		this.m_nlbVoucherPremium.Reserve = false;
		this.m_lbvip_level = (base.GetControl("Label_Label_vip_level") as Label);
		this.m_lbvip_exp = (base.GetControl("Label_Label_vip_exp") as Label);
		this.m_DT_VipDrawTextureBg1 = (base.GetControl("vip_DrawTexture_bg01") as DrawTexture);
		this.m_DT_VipDrawTextureBg2 = (base.GetControl("vip_DrawTexture_bg2") as DrawTexture);
		this.m_BT_VIP_info = (base.GetControl("Button_VIP_info") as Button);
		this.m_BT_VIP_info.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickVipInfo));
		this.m_dtVIP = (base.GetControl("DrawTexture_VIPMark") as DrawTexture);
		this.SetShowData();
		if (!ItemMallDlg.CheckNetwork())
		{
			return;
		}
		this.m_pkMychar = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (this.m_pkMychar != null && this.m_pkMychar.m_SN == 0L)
		{
			GS_USER_SN_GET_REQ gS_USER_SN_GET_REQ = new GS_USER_SN_GET_REQ();
			gS_USER_SN_GET_REQ.i64PersonID = this.m_pkMychar.m_PersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_USER_SN_GET_REQ, gS_USER_SN_GET_REQ);
		}
		NrTSingleton<FiveRocksEventManager>.Instance.ItemMallOpen();
		this.m_DT_Innerbg = (base.GetControl("DT_Innerbg") as DrawTexture);
		this.m_DT_Innerbg.SetTextureFromBundle("ui/ItemShop/itemshopbg");
		this.SetShowDataVoucher();
		base.SetShowLayer(4, true);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			base.SetShowLayer(4, false);
		}
		ITEMMALL_POPUPSHOP atbToIDX = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetAtbToIDX(ItemMallPoPupShopManager.ePoPupShop_Type.ShopOpen);
		if (atbToIDX != null)
		{
			GS_ITEMSHOP_ITEMPOPUP_INFO_REQ gS_ITEMSHOP_ITEMPOPUP_INFO_REQ = new GS_ITEMSHOP_ITEMPOPUP_INFO_REQ();
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64PersonID = this.m_pkMychar.m_PersonID;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64Idx = (long)atbToIDX.m_Idx;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i32ATB = 2;
			SendPacket.GetInstance().SendObject(2536, gS_ITEMSHOP_ITEMPOPUP_INFO_REQ);
		}
		base.SetScreenCenter();
	}

	public void SetShowType(eITEMMALL_TYPE _ItemType)
	{
		if (_ItemType == eITEMMALL_TYPE.NONE)
		{
			_ItemType = ItemMallDlg.m_FirstTab;
		}
		if (this.m_ShowTab == _ItemType)
		{
			return;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab((int)_ItemType))
		{
			this.m_ShowTab = _ItemType;
		}
		else
		{
			this.m_ShowTab = ItemMallDlg.FirstTab;
		}
		for (int i = 0; i < this.m_ListTab.Count; i++)
		{
			UIListItemContainer item = this.m_ListTab.GetItem(i);
			if ((int)item.Data == (int)this.m_ShowTab)
			{
				UIRadioBtn componentInChildren = item.GetComponentInChildren<UIRadioBtn>();
				componentInChildren.Value = true;
			}
		}
		this.SetShowData();
	}

	public virtual void SetShowMode(ItemMallDlg.eMODE _ItemMode)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVoucherLimit())
		{
			this.SetShowType(eITEMMALL_TYPE.BUY_RECOMMEND);
		}
		else
		{
			if (this.m_eMode == _ItemMode)
			{
				return;
			}
			this.m_eMode = _ItemMode;
			this.m_tbTab.SetSelectTabIndex((int)this.m_eMode);
			if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab(1))
			{
				this.m_ShowTab = eITEMMALL_TYPE.BUY_RECOMMEND;
			}
			else
			{
				this.m_ShowTab = ItemMallDlg.FirstTab;
			}
			this.SetShowData();
		}
	}

	public void SetShowData()
	{
		switch (this.m_eMode)
		{
		case ItemMallDlg.eMODE.eMODE_NORMAL:
			base.ShowLayer(1);
			this.SetShowDataNormal();
			base.SetShowLayer(4, true);
			this.SetVipData();
			break;
		case ItemMallDlg.eMODE.eMODE_VOUCHER:
			this.SetShowDataVoucher();
			base.ShowLayer(2);
			base.SetShowLayer(4, true);
			this.SetVipData();
			break;
		case ItemMallDlg.eMODE.eMODE_VOUCHER_HERO:
			base.ShowLayer(3);
			base.SetShowLayer(4, true);
			this.SetShowDataVoucherHero();
			this.SetVipData();
			break;
		}
	}

	public void SetShowDataNormal()
	{
		this.m_ListBox.Clear();
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup((int)this.m_ShowTab);
		if (group == null)
		{
			return;
		}
		for (int i = 0; i < group.Count; i++)
		{
			if (this.IsAddItemList(group[i]))
			{
				NewListItem newListItem = new NewListItem(this.m_ListBox.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(group[i].m_strTextKey), null, null, null);
				newListItem.SetListItemData(3, group[i].m_strIconPath, true, null, null);
				string empty = string.Empty;
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)group[i].m_nMoneyType));
				if (group[i].m_nGroup == 8)
				{
					COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
					if (instance == null)
					{
						goto IL_391;
					}
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(group[i].m_strTextKey),
						"count",
						group[i].m_nItemNum
					});
					newListItem.SetListItemData(1, empty2, null, null, null);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("80502"),
						"count1",
						group[i].m_nItemNum,
						"count2",
						instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EXP_BOOSTER1)
					});
					NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
					long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_EXPBOOSTER);
					string empty3 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1198"),
						"count",
						charSubData.ToString()
					});
					newListItem.SetListItemData(8, true);
					newListItem.SetListItemData(9, true);
					newListItem.SetListItemData(9, empty3, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(8, false);
					newListItem.SetListItemData(9, false);
				}
				newListItem.SetListItemData(6, loader, null, null, null);
				newListItem.SetListItemData(5, string.Empty, group[i], new EZValueChangedDelegate(this.OnClickButton), null);
				bool flag = false;
				ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(group[i].m_Idx);
				if (itemVoucherDataFromItemID != null)
				{
					long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime((eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
					if (voucherRemainTime > 0L)
					{
						newListItem.SetListItemData(16, this.GetTimeToString(voucherRemainTime), null, null, null);
						newListItem.SetListItemEnable(5, false);
						newListItem.SetListItemData(6, false);
						flag = true;
					}
				}
				if (!flag)
				{
					newListItem.SetListItemData(7, ItemMallItemManager.GetCashPrice(group[i]), null, null, null);
				}
				newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(group[i].m_strItemTextKey), null, null, null);
				newListItem.SetListItemData(10, string.Empty, group[i], new EZValueChangedDelegate(this.OnClickTooltip), null);
				newListItem.SetListItemData(11, this.GetEventMark(group[i].m_isEvent));
				newListItem.SetListItemData(14, this.GetNewMark(group[i].m_isEvent));
				if (group[i].m_nVipExp != 0)
				{
					newListItem.SetListItemData(15, true);
				}
				else
				{
					newListItem.SetListItemData(15, false);
				}
				newListItem.Data = group[i].m_Idx;
				this.m_ListBox.Add(newListItem);
			}
			IL_391:;
		}
		this.m_ListBox.RepositionItems();
	}

	public void SetShowDataVoucherItem(eVOUCHER_TYPE eVoucherType)
	{
		if (eVoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_GIVE_ITEM || eVoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_PREMIUM_RECRUIT || eVoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_HEARTS_RECRUIT)
		{
			this.SetShowDataVoucher();
		}
	}

	public void SetShowDataVoucher()
	{
		this.m_nlbVoucher.Clear();
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup(50);
		if (group == null)
		{
			return;
		}
		for (int i = 0; i < group.Count; i++)
		{
			if (this.IsAddItemList(group[i]))
			{
				this.MakeVoucherItem(group[i]);
			}
		}
		this.m_nlbVoucher.RepositionItems();
	}

	public void SetShowDataVoucherHero()
	{
		this.m_nlbVoucherPremium.Clear();
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup(51);
		if (group == null)
		{
			return;
		}
		for (int i = 0; i < group.Count; i++)
		{
			if (this.IsAddItemList(group[i]))
			{
				this.MakeVoucherHero(group[i]);
			}
		}
		this.m_nlbVoucherPremium.RepositionItems();
	}

	public void SetVipData()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			base.SetShowLayer(4, false);
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2790"),
				"level",
				levelExp
			});
			this.m_lbvip_level.SetText(empty);
			this.m_dtVIP.SetTextureFromBundle(string.Format("UI/etc/{0}", NrTSingleton<NrVipSubInfoManager>.Instance.GetIconPath(levelExp)));
			long num = NrTSingleton<NrTableVipManager>.Instance.GetLevelMaxExp(levelExp);
			long nextLevelMaxExp = NrTSingleton<NrTableVipManager>.Instance.GetNextLevelMaxExp(levelExp);
			string text = string.Empty;
			if (nextLevelMaxExp == 0L)
			{
				num = NrTSingleton<NrTableVipManager>.Instance.GetBeforLevelMaxExp(levelExp);
				text = num.ToString() + " / " + num.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize(this.m_DT_VipDrawTextureBg1.width, this.m_DT_VipDrawTextureBg2.GetSize().y);
			}
			else
			{
				long num2 = charSubData - num;
				long num3 = nextLevelMaxExp - num;
				float num4 = (float)num2 / (float)num3;
				text = num2.ToString() + " / " + num3.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize((this.m_DT_VipDrawTextureBg1.GetSize().x - 2f) * num4, this.m_DT_VipDrawTextureBg1.GetSize().y);
			}
			this.m_lbvip_exp.SetText(text);
		}
		else
		{
			this.m_lbvip_level.SetText("0");
			this.m_lbvip_level.SetText("0");
			this.m_DT_VipDrawTextureBg2.Visible = false;
		}
	}

	public override void Update()
	{
		if (this.m_HeartsCount != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_HeartsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			this.m_lbHearts.SetText(ANNUALIZED.Convert(this.m_HeartsCount));
		}
		if (this.m_Money != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			this.m_Money = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
			this.m_lbGold.SetText(ANNUALIZED.Convert(this.m_Money));
		}
		if (this.m_SoulGemsCount != NkUserInventory.GetInstance().Get_First_ItemCnt(70002))
		{
			this.m_SoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
			this.m_lbSoulGems.SetText(ANNUALIZED.Convert(this.m_SoulGemsCount));
		}
		if (this.m_MythElxiresCount != NkUserInventory.GetInstance().Get_First_ItemCnt(50311))
		{
			this.m_MythElxiresCount = NkUserInventory.GetInstance().Get_First_ItemCnt(50311);
			this.m_lbMythElxires.SetText(ANNUALIZED.Convert(this.m_MythElxiresCount));
		}
		this.UpdateTimeNormal();
		this.UpdateTimeVoucher();
		this.UpdateTimeFree();
		if (this.m_bVoucherNotice)
		{
			this.m_BoxVoucherNotice.Visible = true;
		}
		else
		{
			this.m_BoxVoucherNotice.Visible = false;
		}
		if (this.m_bVoucherNotice2)
		{
			this.m_BoxVoucherNotice2.Visible = true;
		}
		else
		{
			this.m_BoxVoucherNotice2.Visible = false;
		}
		if (this.m_bVipLevelUp)
		{
			if (this.m_VipLevelUpdateTime == 0L)
			{
				this.m_VipLevelUpdateTime = PublicMethod.GetCurTime();
			}
			else if (PublicMethod.GetCurTime() > this.m_VipLevelUpdateTime + 2L)
			{
				VipInfoDlg vipInfoDlg = (VipInfoDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.VIPINFO_DLG);
				if (vipInfoDlg != null && this.m_i8CurrVipLevel != 0)
				{
					vipInfoDlg.SetLevel(this.m_i8CurrVipLevel, true);
					this.m_i8CurrVipLevel = 0;
					this.m_VipLevelUpdateTime = 0L;
					this.m_bVipLevelUp = false;
				}
				return;
			}
		}
	}

	public void OnClickVipInfo(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			byte b = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
			if (b < 0)
			{
				b = 0;
			}
			this.SetVipInfoShow(b, false);
		}
	}

	public void SetVipInfoShow(byte i8Level, bool VipLevelUpCheck)
	{
		this.SetVipData();
		if (VipLevelUpCheck)
		{
			this.m_DT_Innerbg.Visible = true;
			this.m_bVipLevelUp = true;
			this.m_i8CurrVipLevel = i8Level;
			NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_vip_levelup01_mobile", this.m_DT_Innerbg, this.m_DT_Innerbg.GetSize());
		}
		else
		{
			VipInfoDlg vipInfoDlg = (VipInfoDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.VIPINFO_DLG);
			if (vipInfoDlg != null)
			{
				vipInfoDlg.SetLevel(i8Level, false);
			}
		}
	}

	public virtual void OnClickBtHeartsState(IUIObject obj)
	{
		this.SetShowType(eITEMMALL_TYPE.BUY_HEARTS);
	}

	public virtual void OnClickBtGoldState(IUIObject obj)
	{
		this.SetShowType(eITEMMALL_TYPE.BUY_GOLD);
	}

	public void OnClickTab(IUIObject obj)
	{
		UIRadioBtn uIRadioBtn = obj as UIRadioBtn;
		if (!uIRadioBtn.Value)
		{
			return;
		}
		eITEMMALL_TYPE eITEMMALL_TYPE = (eITEMMALL_TYPE)((int)uIRadioBtn.Data);
		if (this.m_ShowTab == eITEMMALL_TYPE)
		{
			return;
		}
		this.m_ShowTab = eITEMMALL_TYPE;
		this.SetShowData();
	}

	public void OnClickButton(IUIObject obj)
	{
		ITEM_MALL_ITEM itemMall = obj.Data as ITEM_MALL_ITEM;
		ItemMallDlg.BuyItem(itemMall);
	}

	public static void BuyItem(ITEM_MALL_ITEM _ItemMall)
	{
		string empty = string.Empty;
		ItemMallDlg.m_SelectItem = null;
		if (_ItemMall != null)
		{
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(_ItemMall.m_Idx) || !NrTSingleton<ItemMallItemManager>.Instance.BuyItem(_ItemMall.m_Idx))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("658"));
				return;
			}
			if (_ItemMall.m_nMoneyType == 1)
			{
				if (!ItemMallDlg.CheckNetwork())
				{
					return;
				}
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo.m_SN == 0L)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"));
					return;
				}
			}
			VOUCHER_DATA voucherData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherData(eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO, _ItemMall.m_Idx);
			if (voucherData != null)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				if (readySolList == null || readySolList.GetCount() >= 100)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
			}
			long voucherRemainTimeFromItemID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTimeFromItemID(_ItemMall.m_Idx);
			if (voucherRemainTimeFromItemID > 0L && voucherRemainTimeFromItemID > (long)NrTSingleton<ItemMallItemManager>.Instance.VoucherRefillTime * 3600L)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("755"));
				return;
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_ItemMall.m_strTextKey)
				});
				if (_ItemMall.m_nGroup == 8)
				{
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_ItemMall.m_strTextKey),
						"count",
						_ItemMall.m_nItemNum
					});
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
						"targetname",
						empty2
					});
				}
				msgBoxUI.OkEventImmediatelyClose = false;
				NrTSingleton<ItemMallItemManager>.Instance.CheckMsgBox = msgBoxUI;
				ItemMallDlg.m_SelectItem = _ItemMall;
				bool flag = false;
				if (_ItemMall.m_nGift == 1)
				{
					if (TsPlatform.IsEditor || TsPlatform.IsAndroid)
					{
						flag = true;
					}
					ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(_ItemMall.m_Idx);
					if (itemVoucherDataFromItemID != null && itemVoucherDataFromItemID.ui8VoucherType == 9)
					{
						flag = false;
					}
				}
				if (flag)
				{
					msgBoxUI.OkEventImmediatelyClose = true;
					msgBoxUI.SetMsg(new YesDelegate(ItemMallDlg.MsgBoxOKEvent), msgBoxUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2578"), eMsgType.MB_CHECK_OK_CANCEL);
					msgBoxUI.SetCheckBoxState(false);
					if (_ItemMall.m_nGroup == 101 && 0 < _ItemMall.m_nGift)
					{
						msgBoxUI.SetCheckBoxState(true);
						msgBoxUI.OnOKByScript();
					}
				}
				else
				{
					msgBoxUI.SetMsg(new YesDelegate(ItemMallDlg.MsgBoxOKEvent), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
				}
				msgBoxUI.ShowItemMallPolicy(true);
				if (_ItemMall.m_nGroup == 51 || _ItemMall.m_nGroup == 50)
				{
					msgBoxUI.SetPolicy(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3328"));
				}
			}
		}
	}

	public virtual void OnClickTooltip(IUIObject obj)
	{
		if (obj.Data == null)
		{
			return;
		}
		ITEM_MALL_ITEM iTEM_MALL_ITEM = obj.Data as ITEM_MALL_ITEM;
		if (iTEM_MALL_ITEM != null)
		{
			ItemMallProductDetailDlg itemMallProductDetailDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_PRODUCTDETAIL_DLG) as ItemMallProductDetailDlg;
			if (itemMallProductDetailDlg == null)
			{
				return;
			}
			itemMallProductDetailDlg.SetItem(iTEM_MALL_ITEM);
			eITEMMALL_TYPE nGroup = (eITEMMALL_TYPE)iTEM_MALL_ITEM.m_nGroup;
			if (nGroup == eITEMMALL_TYPE.BUY_HERO || nGroup == eITEMMALL_TYPE.BUY_ITEMBOX)
			{
				itemMallProductDetailDlg.SetEffect();
			}
			itemMallProductDetailDlg.AddBlackBgClickCloseForm();
			ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
			if (itemVoucherDataFromItemID != null)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
				if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
				{
					long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
					if (PublicMethod.GetCurTime() > charSubData)
					{
						itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.ClickFreeVoucher));
					}
					else
					{
						itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.ClickHearts));
					}
				}
				else if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
				{
					long charSubData2 = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
					if (PublicMethod.GetCurTime() > charSubData2)
					{
						itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.ClickFreeVoucher));
					}
					else
					{
						itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.OnClickButton));
					}
				}
				else if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_ELEVEN)
				{
					itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.ClickHearts));
				}
				else
				{
					itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.OnClickButton));
				}
			}
			else
			{
				itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.OnClickButton));
			}
		}
	}

	public static void MsgBoxOKEvent(object EventObject)
	{
		ItemMallDlg_ChallengeQuest itemMallDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg_ChallengeQuest;
		if (itemMallDlg_ChallengeQuest != null)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.Close();
			}
			itemMallDlg_ChallengeQuest.MsgBoxOKEvent();
			return;
		}
		if (NrTSingleton<ItemMallItemManager>.Instance.Trading)
		{
			MsgBoxUI msgBoxUI2 = EventObject as MsgBoxUI;
			if (msgBoxUI2 != null)
			{
				msgBoxUI2.Close();
			}
			return;
		}
		if (!NrTSingleton<ItemMallItemManager>.Instance.CanBuyItemByHeartsOrGold_Notify(ItemMallDlg.m_SelectItem))
		{
			MsgBoxUI msgBoxUI3 = EventObject as MsgBoxUI;
			if (msgBoxUI3 != null)
			{
				msgBoxUI3.Close();
			}
			return;
		}
		if (TsPlatform.IsEditor || TsPlatform.IsAndroid)
		{
			MsgBoxUI msgBoxUI4 = EventObject as MsgBoxUI;
			if (msgBoxUI4 != null && msgBoxUI4.IsChecked())
			{
				ItemGiftTargetDlg itemGiftTargetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMGIFTTARGET_DLG) as ItemGiftTargetDlg;
				if (itemGiftTargetDlg != null)
				{
					itemGiftTargetDlg.SetTradeItem(ItemMallDlg.m_SelectItem, ItemMallItemManager.eItemMall_SellType.ITEMMALL);
				}
				return;
			}
		}
		NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(ItemMallDlg.m_SelectItem, ItemMallItemManager.eItemMall_SellType.ITEMMALL);
		GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
		gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = ItemMallDlg.m_SelectItem.m_Idx;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
		NrTSingleton<ItemMallItemManager>.Instance.Trading = true;
	}

	public void RefreshData()
	{
		this.SetShowData();
	}

	public override void OnClose()
	{
		this.DestroyTouch((long)this.guideitemmallID);
		if (NrTSingleton<FormsManager>.Instance.IsPopUPDlgNotExist(base.WindowID))
		{
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture();
		}
		base.OnClose();
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
		{
			BillingManager_Google component = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
			if (component != null)
			{
				component.CheckRestoreItem();
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
		{
			BillingManager_TStore component2 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
			if (component2 != null)
			{
				component2.CheckRestoreItem();
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
		{
			BillingManager_NStore component3 = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
			if (component3 != null)
			{
				component3.CheckRestoreItem();
			}
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_EXPBOOSTER);
		if (charSubData > 0L)
		{
			ExpBoosterDlg expBoosterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_EXPBOOSTER_DLG) as ExpBoosterDlg;
			if (expBoosterDlg == null)
			{
				expBoosterDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_EXPBOOSTER_DLG) as ExpBoosterDlg);
			}
			if (expBoosterDlg != null)
			{
				expBoosterDlg.Show();
			}
		}
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("store_out");
		kMyCharInfo.m_SN = 0L;
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg != null && costumeRoom_Dlg._costumeViewerSetter != null && costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter != null)
		{
			costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter.VisibleChar(true);
		}
		int num = 1;
		PlayerPrefs.SetString(NrPrefsKey.ITEMMALL_VIEW, num.ToString());
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
	}

	private static bool CheckNetwork()
	{
		if (!BaseNet_Game.GetInstance().IsSocketConnected())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("382"));
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMMALL_DLG);
			return false;
		}
		return true;
	}

	public bool IsAddItemList(ITEM_MALL_ITEM ItemMallItem)
	{
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(ItemMallItem.m_Idx);
		if (itemVoucherDataFromItemID != null)
		{
			eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
			long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, ItemMallItem.m_Idx);
			if (voucherRemainTime > 0L)
			{
				return true;
			}
		}
		return NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(ItemMallItem.m_Idx) && NrTSingleton<ItemMallItemManager>.Instance.BuyItem(ItemMallItem.m_Idx) && NrTSingleton<ItemMallItemManager>.Instance.IsBuyCountLimit(ItemMallItem);
	}

	public string GetTimeToString(long i64Time)
	{
		this.m_strText = string.Empty;
		if (i64Time > 0L)
		{
			long totalDayFromSec = PublicMethod.GetTotalDayFromSec(i64Time);
			long hourFromSec = PublicMethod.GetHourFromSec(i64Time);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(i64Time);
			if (totalDayFromSec == 0L)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1092"),
					"hour",
					hourFromSec,
					"min",
					minuteFromSec
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3521"),
					"day",
					totalDayFromSec
				});
			}
		}
		return this.m_strText;
	}

	public string GetTimeToString(long i64Time, string strTextKey)
	{
		this.m_strText = string.Empty;
		if (i64Time > 0L)
		{
			DateTime dueDate = PublicMethod.GetDueDate(i64Time);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey),
				"year",
				dueDate.Year,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day
			});
		}
		return this.m_strText;
	}

	public void UpdateTimeNormal()
	{
		for (int i = 0; i < this.m_ListBox.Count; i++)
		{
			UIListItemContainer item = this.m_ListBox.GetItem(i);
			if (!(item == null))
			{
				long index = (long)item.Data;
				ITEM_MALL_ITEM item2 = NrTSingleton<ItemMallItemManager>.Instance.GetItem(index);
				if (item2 != null)
				{
					if (NrTSingleton<ItemMallItemManager>.Instance.IsItemVoucherType(item2))
					{
						Label label = item.GetElement(16) as Label;
						if (!(label == null))
						{
							long voucherRemainTimeFromItemID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTimeFromItemID(item2.m_Idx);
							if (voucherRemainTimeFromItemID > 0L)
							{
								label.SetText(this.GetTimeToString(voucherRemainTimeFromItemID));
								item.GetElement(5).controlIsEnabled = false;
								item.GetElement(6).Visible = false;
								item.GetElement(7).Visible = false;
							}
							else
							{
								label.SetText(string.Empty);
								item.GetElement(5).controlIsEnabled = true;
								item.GetElement(6).Visible = true;
								label = (item.GetElement(7) as Label);
								label.SetText(ItemMallItemManager.GetCashPrice(item2));
								item.GetElement(7).Visible = true;
							}
						}
					}
				}
			}
		}
	}

	public void MakeVoucherItem(ITEM_MALL_ITEM Item)
	{
		if (Item == null)
		{
			return;
		}
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(Item.m_Idx);
		if (itemVoucherDataFromItemID == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbVoucher.ColumnNum, true, string.Empty);
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
		newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey), null, null, null);
		newListItem.SetListItemData(3, Item.m_strIconPath, true, null, null);
		this.m_strText = string.Empty;
		switch (ui8VoucherType)
		{
		case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_GIVE_ITEM:
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2738");
			break;
		case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_PREMIUM_RECRUIT:
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
			break;
		case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_HEARTS_RECRUIT:
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
			break;
		}
		long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
		if (voucherRemainTime > 0L)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsUseVoucher(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID))
			{
				newListItem.SetListItemData(4, this.m_strText, Item, new EZValueChangedDelegate(this.ClickVoucherUse), null);
			}
			else
			{
				long nextUseVoucherTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetNextUseVoucherTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
				this.m_strTime = this.GetTimeToString(nextUseVoucherTime);
				newListItem.SetListItemData(4, this.m_strTime, Item, new EZValueChangedDelegate(this.ClickVoucherUse), null);
				newListItem.SetListItemEnable(4, false);
			}
			long voucherEndTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherEndTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
			newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2740"), Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
			newListItem.SetListItemData(9, this.GetTimeToString(voucherEndTime, "2730"), null, null, null);
			newListItem.SetListItemData(6, false);
			newListItem.SetListItemData(7, false);
			long voucherStartTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherStartTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
			newListItem.SetListItemData(8, this.GetTimeToString(voucherStartTime, "2729"), null, null, null);
		}
		else
		{
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(5, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
			if (uIBaseInfoLoader != null)
			{
				newListItem.SetListItemData(6, uIBaseInfoLoader, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(6, false);
			}
			newListItem.SetListItemData(7, ItemMallItemManager.GetCashPrice(Item), null, null, null);
			newListItem.SetListItemData(8, false);
			newListItem.SetListItemData(9, false);
		}
		newListItem.SetListItemData(10, string.Empty, Item, new EZValueChangedDelegate(this.OnClickTooltip), null);
		newListItem.SetListItemData(11, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTextKey), null, null, null);
		newListItem.SetListItemData(12, this.GetEventMark(Item.m_isEvent));
		newListItem.SetListItemData(15, this.GetNewMark(Item.m_isEvent));
		newListItem.Data = Item;
		this.m_nlbVoucher.Add(newListItem);
	}

	public void MakeVoucherHero(ITEM_MALL_ITEM Item)
	{
		if (Item == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(Item.m_Idx);
		if (itemVoucherDataFromItemID == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbVoucherPremium.ColumnNum, true, string.Empty);
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
		newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey), null, null, null);
		newListItem.SetListItemData(3, Item.m_strIconPath, true, null, null);
		this.m_strText = string.Empty;
		eVOUCHER_TYPE eVOUCHER_TYPE = ui8VoucherType;
		if (eVOUCHER_TYPE != eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
		{
			if (eVOUCHER_TYPE == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO_ELEVEN)
			{
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
			}
		}
		else
		{
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2738");
		}
		if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO || ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
		{
			long charSubData;
			if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
			{
				charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
			}
			else
			{
				charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
			}
			string empty = string.Empty;
			if (PublicMethod.GetCurTime() > charSubData)
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickFreeVoucher), null);
				newListItem.SetListItemData(5, false);
				newListItem.SetListItemData(13, false);
				newListItem.SetListItemData(6, false);
				newListItem.SetListItemData(14, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3344"), null, null, null);
			}
			else
			{
				if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
				{
					newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickHearts), null);
				}
				else
				{
					newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
				}
				UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
				if (uIBaseInfoLoader != null)
				{
					newListItem.SetListItemData(5, uIBaseInfoLoader, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(5, false);
				}
				newListItem.SetListItemData(6, ItemMallItemManager.GetCashPrice(Item), null, null, null);
				DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
				DateTime dueDate2 = PublicMethod.GetDueDate(charSubData);
				TimeSpan timeSpan = dueDate2 - dueDate;
				string empty2 = string.Empty;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("849");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromInterface,
					"day",
					timeSpan.Days,
					"hour",
					timeSpan.Hours,
					"min",
					timeSpan.Minutes
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3343"),
					"timestring",
					empty2
				});
				newListItem.SetListItemData(13, empty, null, null, null);
				newListItem.SetListItemData(14, false);
			}
		}
		else
		{
			if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_ELEVEN)
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickHearts), null);
			}
			else
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
			}
			UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
			if (uIBaseInfoLoader2 != null)
			{
				newListItem.SetListItemData(5, uIBaseInfoLoader2, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(5, false);
			}
			newListItem.SetListItemData(6, ItemMallItemManager.GetCashPrice(Item), null, null, null);
			newListItem.SetListItemData(13, false);
			newListItem.SetListItemData(14, false);
		}
		newListItem.SetListItemData(7, string.Empty, Item, new EZValueChangedDelegate(this.OnClickTooltip), null);
		newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTextKey), null, null, null);
		newListItem.SetListItemData(9, this.GetEventMark(Item.m_isEvent));
		newListItem.SetListItemData(12, this.GetNewMark(Item.m_isEvent));
		newListItem.Data = Item;
		this.m_nlbVoucherPremium.Add(newListItem);
	}

	public void ReMakeVoucherItem(int i, ITEM_MALL_ITEM Item)
	{
		if (Item == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(Item.m_Idx);
		if (itemVoucherDataFromItemID == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbVoucherPremium.ColumnNum, true, string.Empty);
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
		newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey), null, null, null);
		newListItem.SetListItemData(3, Item.m_strIconPath, true, null, null);
		this.m_strText = string.Empty;
		eVOUCHER_TYPE eVOUCHER_TYPE = ui8VoucherType;
		if (eVOUCHER_TYPE != eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
		{
			if (eVOUCHER_TYPE == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO_ELEVEN)
			{
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
			}
		}
		else
		{
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2738");
		}
		if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO || ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
		{
			long charSubData;
			if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
			{
				charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
			}
			else
			{
				charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
			}
			string empty = string.Empty;
			if (PublicMethod.GetCurTime() > charSubData)
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickFreeVoucher), null);
				newListItem.SetListItemData(5, false);
				newListItem.SetListItemData(13, false);
				newListItem.SetListItemData(6, false);
				newListItem.SetListItemData(14, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3344"), null, null, null);
			}
			else
			{
				if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
				{
					newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickHearts), null);
				}
				else
				{
					newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
				}
				UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
				if (uIBaseInfoLoader != null)
				{
					newListItem.SetListItemData(5, uIBaseInfoLoader, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(5, false);
				}
				newListItem.SetListItemData(6, ItemMallItemManager.GetCashPrice(Item), null, null, null);
				DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
				DateTime dueDate2 = PublicMethod.GetDueDate(charSubData);
				TimeSpan timeSpan = dueDate2 - dueDate;
				string empty2 = string.Empty;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("849");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromInterface,
					"day",
					timeSpan.Days,
					"hour",
					timeSpan.Hours,
					"min",
					timeSpan.Minutes
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3343"),
					"timestring",
					empty2
				});
				newListItem.SetListItemData(13, empty, null, null, null);
				newListItem.SetListItemData(14, false);
			}
		}
		else
		{
			if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_ELEVEN)
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickHearts), null);
			}
			else
			{
				newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
			}
			UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
			if (uIBaseInfoLoader2 != null)
			{
				newListItem.SetListItemData(5, uIBaseInfoLoader2, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(5, false);
			}
			newListItem.SetListItemData(6, ItemMallItemManager.GetCashPrice(Item), null, null, null);
			newListItem.SetListItemData(13, false);
			newListItem.SetListItemData(14, false);
		}
		newListItem.SetListItemData(7, string.Empty, Item, new EZValueChangedDelegate(this.OnClickTooltip), null);
		newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTextKey), null, null, null);
		newListItem.SetListItemData(9, this.GetEventMark(Item.m_isEvent));
		newListItem.SetListItemData(12, this.GetNewMark(Item.m_isEvent));
		newListItem.Data = Item;
		this.m_nlbVoucherPremium.UpdateContents(i, newListItem);
	}

	private bool GetEventMark(byte byEvent)
	{
		return byEvent == 1 || byEvent == 3;
	}

	private bool GetNewMark(byte byEvent)
	{
		return byEvent == 2 || byEvent == 3;
	}

	protected virtual void ClickVoucherUse(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		ITEM_MALL_ITEM iTEM_MALL_ITEM = obj.Data as ITEM_MALL_ITEM;
		if (iTEM_MALL_ITEM == null)
		{
			return;
		}
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
		if (itemVoucherDataFromItemID == null)
		{
			return;
		}
		this.Send_GS_CHARACTER_VOUCHER_USE_REQ((eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
	}

	protected virtual void ClickBuyVoucher(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		ITEM_MALL_ITEM iTEM_MALL_ITEM = obj.Data as ITEM_MALL_ITEM;
		if (iTEM_MALL_ITEM == null)
		{
			return;
		}
		ItemMallDlg.BuyItem(iTEM_MALL_ITEM);
		this.DestroyTouch(iTEM_MALL_ITEM.m_Idx);
	}

	protected virtual void ClickFreeVoucher(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		ITEM_MALL_ITEM iTEM_MALL_ITEM = obj.Data as ITEM_MALL_ITEM;
		if (iTEM_MALL_ITEM == null)
		{
			return;
		}
		ItemMallDlg.FreeItem(iTEM_MALL_ITEM);
		this.DestroyTouch(iTEM_MALL_ITEM.m_Idx);
	}

	protected virtual void ClickHearts(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		ITEM_MALL_ITEM iTEM_MALL_ITEM = obj.Data as ITEM_MALL_ITEM;
		if (iTEM_MALL_ITEM == null)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.GetCanOpenTicket())
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1699");
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("127");
			ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
			if (itemVoucherDataFromItemID == null)
			{
				return;
			}
			int value;
			if (itemVoucherDataFromItemID.ui8VoucherType == 6)
			{
				value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORONE);
			}
			else
			{
				if (itemVoucherDataFromItemID.ui8VoucherType != 7)
				{
					return;
				}
				value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORTEN);
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
			{
				textFromMessageBox,
				"count",
				value
			});
			msgBoxUI.SetMsg(new YesDelegate(ItemMallDlg.MsgBoxOKHeartEvent), iTEM_MALL_ITEM.m_Idx, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
		}
	}

	public static void MsgBoxOKHeartEvent(object EventObject)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		long i64ItemID = (long)EventObject;
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(i64ItemID);
		if (itemVoucherDataFromItemID == null)
		{
			return;
		}
		eSolRecruitType eSolRecruitType;
		int num;
		if (itemVoucherDataFromItemID.ui8VoucherType == 6)
		{
			eSolRecruitType = eSolRecruitType.SOLRECRUIT_CASH_ONE;
			num = 1;
		}
		else
		{
			if (itemVoucherDataFromItemID.ui8VoucherType != 7)
			{
				return;
			}
			eSolRecruitType = eSolRecruitType.SOLRECRUIT_CASH_TEN;
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOLCOUNT_FORTEN);
		}
		int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASH_ITEMUNIQUE);
		bool flag = false;
		if (eSolRecruitType == eSolRecruitType.SOLRECRUIT_CASH_TEN)
		{
			flag = true;
		}
		if (!flag)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() + num - 1 >= 100)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		if (NkUserInventory.GetInstance().GetFirstItemByUnique(value) == null)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(false);
		GS_SOLDIER_RECRUIT_REQ gS_SOLDIER_RECRUIT_REQ = default(GS_SOLDIER_RECRUIT_REQ);
		gS_SOLDIER_RECRUIT_REQ.ItemUnique = value;
		gS_SOLDIER_RECRUIT_REQ.RecruitType = (int)eSolRecruitType;
		gS_SOLDIER_RECRUIT_REQ.SubData = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_RECRUIT_REQ, gS_SOLDIER_RECRUIT_REQ);
	}

	private void Send_GS_CHARACTER_VOUCHER_USE_REQ(eVOUCHER_TYPE eVoucherType, long i64ItemMallID)
	{
		if (this.m_fDelayTime > 0f && this.m_fDelayTime > Time.time)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		switch (eVoucherType)
		{
		case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_PREMIUM_RECRUIT:
		case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_HEARTS_RECRUIT:
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() >= 100)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			break;
		}
		}
		GS_CHARACTER_VOUCHER_USE_REQ gS_CHARACTER_VOUCHER_USE_REQ = new GS_CHARACTER_VOUCHER_USE_REQ();
		gS_CHARACTER_VOUCHER_USE_REQ.i8VoucherType = (byte)eVoucherType;
		gS_CHARACTER_VOUCHER_USE_REQ.i64ItemMallID = i64ItemMallID;
		SendPacket.GetInstance().SendObject(2401, gS_CHARACTER_VOUCHER_USE_REQ);
		this.m_fDelayTime = Time.time + 1f;
	}

	public virtual void OnClickMode(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eMode = (ItemMallDlg.eMODE)uIPanelTab.panel.index;
		this.SetShowData();
	}

	private void UpdateTimeFree()
	{
		if (this.m_UpdateTime == 0L)
		{
			this.m_UpdateTime = PublicMethod.GetCurTime();
		}
		else
		{
			if (PublicMethod.GetCurTime() < this.m_UpdateTime + 20L)
			{
				return;
			}
			this.m_UpdateTime = PublicMethod.GetCurTime();
		}
		this.m_bVoucherNotice2 = false;
		bool flag = false;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
		if (PublicMethod.GetCurTime() < charSubData)
		{
			if (this.m_eMode == ItemMallDlg.eMODE.eMODE_VOUCHER_HERO)
			{
				for (int i = 0; i < this.m_nlbVoucherPremium.Count; i++)
				{
					IUIListObject item = this.m_nlbVoucherPremium.GetItem(i);
					if (item != null)
					{
						ITEM_MALL_ITEM iTEM_MALL_ITEM = (ITEM_MALL_ITEM)item.Data;
						if (iTEM_MALL_ITEM != null)
						{
							ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
							if (itemVoucherDataFromItemID == null)
							{
								return;
							}
							if (itemVoucherDataFromItemID.ui8VoucherType == 3)
							{
								this.ReMakeVoucherItem(i, iTEM_MALL_ITEM);
							}
						}
					}
				}
			}
		}
		else
		{
			flag = true;
		}
		charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
		if (PublicMethod.GetCurTime() < charSubData)
		{
			if (this.m_eMode == ItemMallDlg.eMODE.eMODE_VOUCHER_HERO)
			{
				for (int j = 0; j < this.m_nlbVoucherPremium.Count; j++)
				{
					IUIListObject item2 = this.m_nlbVoucherPremium.GetItem(j);
					if (item2 != null)
					{
						ITEM_MALL_ITEM iTEM_MALL_ITEM2 = (ITEM_MALL_ITEM)item2.Data;
						if (iTEM_MALL_ITEM2 != null)
						{
							ITEM_VOUCHER_DATA itemVoucherDataFromItemID2 = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM2.m_Idx);
							if (itemVoucherDataFromItemID2 == null)
							{
								return;
							}
							if (itemVoucherDataFromItemID2.ui8VoucherType == 6)
							{
								this.ReMakeVoucherItem(j, iTEM_MALL_ITEM2);
							}
						}
					}
				}
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			this.m_bVoucherNotice2 = true;
		}
	}

	private void UpdateTimeVoucher()
	{
		for (int i = 0; i < this.m_nlbVoucher.Count; i++)
		{
			UIListItemContainer item = this.m_nlbVoucher.GetItem(i);
			if (!(item == null))
			{
				if (item.Data != null)
				{
					ITEM_MALL_ITEM iTEM_MALL_ITEM = item.Data as ITEM_MALL_ITEM;
					if (iTEM_MALL_ITEM != null)
					{
						ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
						if (itemVoucherDataFromItemID != null)
						{
							this.UpdateTimeVoucherControl(item, itemVoucherDataFromItemID, iTEM_MALL_ITEM);
						}
					}
				}
			}
		}
		this.m_bVoucherNotice = false;
		if (this.CheckVoucherNotice())
		{
			this.m_bVoucherNotice = true;
		}
	}

	private bool CheckVoucherNotice()
	{
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup(50);
		if (group == null)
		{
			return false;
		}
		for (int i = 0; i < group.Count; i++)
		{
			ITEM_MALL_ITEM iTEM_MALL_ITEM = group[i];
			if (iTEM_MALL_ITEM != null)
			{
				ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
				if (itemVoucherDataFromItemID != null)
				{
					eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
					long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
					if (voucherRemainTime > 0L && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsUseVoucher(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void UpdateTimeVoucherControl(UIListItemContainer pkListItem, ITEM_VOUCHER_DATA ItemVoucherData, ITEM_MALL_ITEM Item)
	{
		if (pkListItem == null || ItemVoucherData == null || Item == null)
		{
			return;
		}
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)ItemVoucherData.ui8VoucherType;
		UIButton uIButton = pkListItem.GetElement(3) as UIButton;
		UIButton uIButton2 = pkListItem.GetElement(4) as UIButton;
		Label label = pkListItem.GetElement(7) as Label;
		Label label2 = pkListItem.GetElement(8) as Label;
		if (uIButton == null || uIButton2 == null || label == null || label2 == null)
		{
			return;
		}
		long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, ItemVoucherData.i64ItemMallID);
		if (voucherRemainTime > 0L)
		{
			if (!uIButton.Visible)
			{
				uIButton.Visible = true;
			}
			switch (ui8VoucherType)
			{
			case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_GIVE_ITEM:
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2738");
				goto IL_12E;
			case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_PREMIUM_RECRUIT:
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
				goto IL_12E;
			case eVOUCHER_TYPE.eVOUCHER_TYPE_FREE_HEARTS_RECRUIT:
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2739");
				goto IL_12E;
			}
			return;
			IL_12E:
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsUseVoucher(ui8VoucherType, ItemVoucherData.i64ItemMallID))
			{
				if (!uIButton.controlIsEnabled)
				{
					uIButton.controlIsEnabled = true;
				}
				uIButton.SetText(this.m_strText);
			}
			else
			{
				long nextUseVoucherTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetNextUseVoucherTime(ui8VoucherType, ItemVoucherData.i64ItemMallID);
				if (uIButton.controlIsEnabled)
				{
					uIButton.controlIsEnabled = false;
				}
				uIButton.SetText(this.GetTimeToString(nextUseVoucherTime));
			}
			long voucherEndTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherEndTime(ui8VoucherType, ItemVoucherData.i64ItemMallID);
			uIButton2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2740"));
			uIButton2.controlIsEnabled = false;
			label2.SetText(this.GetTimeToString(voucherEndTime, "2730"));
			long voucherStartTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherStartTime(ui8VoucherType, ItemVoucherData.i64ItemMallID);
			label.SetText(this.GetTimeToString(voucherStartTime, "2729"));
			if (!label.Visible)
			{
				label.Visible = true;
			}
			if (!label2.Visible)
			{
				label2.Visible = true;
			}
			DrawTexture drawTexture = pkListItem.GetElement(5) as DrawTexture;
			if (drawTexture != null && drawTexture.Visible)
			{
				drawTexture.Visible = false;
			}
			Label label3 = pkListItem.GetElement(6) as Label;
			if (label3 != null && label3.Visible)
			{
				drawTexture.Visible = false;
			}
		}
		else
		{
			uIButton.Visible = false;
			uIButton2.SetText(string.Empty);
			DrawTexture drawTexture2 = pkListItem.GetElement(5) as DrawTexture;
			if (drawTexture2 != null && !drawTexture2.Visible)
			{
				drawTexture2.Visible = true;
				UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
				if (uIBaseInfoLoader != null)
				{
					drawTexture2.SetTexture(uIBaseInfoLoader);
				}
			}
			Label label4 = pkListItem.GetElement(6) as Label;
			if (label4 != null && !label4.Visible)
			{
				label4.Visible = true;
				label4.SetText(ItemMallItemManager.GetCashPrice(Item));
			}
			if (label.Visible)
			{
				label.Visible = false;
			}
			if (label2.Visible)
			{
				label2.Visible = false;
			}
		}
	}

	public override void CloseForm(IUIObject obj)
	{
		if (this.m_bCloseEnable)
		{
			base.CloseForm(obj);
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.AutoCloseTime = Time.time + 30f;
		msgBoxUI.SetMsg(null, null, new NoDelegate(ItemMallDlg.OnItemMallWaiteCancel), null, string.Empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("274"), eMsgType.MB_OK);
		msgBoxUI.OkEventImmediatelyClose = false;
		msgBoxUI.Show();
	}

	public static void OnItemMallWaiteCancel(object EventObject)
	{
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			itemMallDlg.CloseEnable = true;
		}
	}

	private void ShowHLTap()
	{
		this.m_ListTab.Clear();
		for (int i = 1; i < 10; i++)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab(i))
			{
				NewListItem newListItem = new NewListItem(this.m_ListTab.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(0, string.Empty, (eITEMMALL_TYPE)i, new EZValueChangedDelegate(this.OnClickTab), null);
				newListItem.Data = (eITEMMALL_TYPE)i;
				string text = string.Empty;
				string text2 = string.Empty;
				switch (i)
				{
				case 1:
					text = "[#fff00000]" + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("834");
					text2 = "Win_B_ItemShop01";
					break;
				case 2:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1490");
					text2 = "Win_B_ItemShop04";
					break;
				case 3:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3311");
					text2 = "Win_B_ItemShop02";
					break;
				case 4:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676");
					text2 = "Win_B_ItemShop03";
					break;
				case 5:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("451");
					text2 = "Win_B_ItemShop06";
					break;
				case 6:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("192");
					text2 = "Win_B_ItemShop05";
					break;
				case 7:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1558");
					break;
				case 8:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("832");
					break;
				case 9:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3332");
					text2 = "Win_B_ItemShop07";
					break;
				}
				if (text != string.Empty)
				{
					if (i == (int)this.m_ShowTab)
					{
						newListItem.SetListItemData(4, text, null, null, null);
					}
					else
					{
						newListItem.SetListItemData(2, text, null, null, null);
					}
				}
				if (text2 != string.Empty)
				{
					if (i == 1)
					{
						newListItem.SetListItemData(1, string.Format("{0}_1", text2), null, null, null);
					}
					else if (i == (int)this.m_ShowTab)
					{
						newListItem.SetListItemData(3, string.Format("{0}_1", text2), null, null, null);
					}
					else
					{
						newListItem.SetListItemData(1, text2, null, null, null);
					}
				}
				this.m_ListTab.Add(newListItem);
			}
		}
		this.m_ListTab.RepositionItems();
	}

	public static void FreeItem(ITEM_MALL_ITEM _ItemMall)
	{
		string empty = string.Empty;
		ItemMallDlg.m_SelectItem = null;
		if (_ItemMall != null)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() >= 100)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_ItemMall.m_strTextKey)
				});
				msgBoxUI.OkEventImmediatelyClose = false;
				NrTSingleton<ItemMallItemManager>.Instance.CheckMsgBox = msgBoxUI;
				ItemMallDlg.m_SelectItem = _ItemMall;
				msgBoxUI.SetMsg(new YesDelegate(ItemMallDlg.MsgBoxFreeEvent), msgBoxUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
				msgBoxUI.ShowItemMallPolicy(false);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"));
			}
		}
	}

	public static void MsgBoxFreeEvent(object EventObject)
	{
		ItemMallDlg_ChallengeQuest itemMallDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg_ChallengeQuest;
		if (itemMallDlg_ChallengeQuest != null)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.Close();
			}
			itemMallDlg_ChallengeQuest.MsgBoxOKEvent();
			return;
		}
		MsgBoxUI msgBoxUI2 = EventObject as MsgBoxUI;
		if (msgBoxUI2 != null)
		{
			msgBoxUI2.Close();
		}
		if (NrTSingleton<ItemMallItemManager>.Instance.Trading)
		{
			return;
		}
		NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(ItemMallDlg.m_SelectItem, ItemMallItemManager.eItemMall_SellType.ITEMMALL);
		GS_ITEMMALL_FREE_TRADE_REQ gS_ITEMMALL_FREE_TRADE_REQ = new GS_ITEMMALL_FREE_TRADE_REQ();
		gS_ITEMMALL_FREE_TRADE_REQ.ItemID = ItemMallDlg.m_SelectItem.m_Idx;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_FREE_TRADE_REQ, gS_ITEMMALL_FREE_TRADE_REQ);
		NrTSingleton<ItemMallItemManager>.Instance.Trading = true;
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		string[] array = param1.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 2)
		{
			return;
		}
		this.guideWinID = winID;
		this.guideitemmallID = int.Parse(array[0]);
		int value = int.Parse(array[1]);
		UIListItemContainer guideItemProcess = this.GetGuideItemProcess(this.guideitemmallID);
		if (guideItemProcess == null)
		{
			return;
		}
		if (this._Touch != null)
		{
			return;
		}
		this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
		this._Touch.PlayAni(true);
		AutoSpriteControlBase element = guideItemProcess.GetElement(value);
		if (element == null)
		{
			return;
		}
		this._Touch.gameObject.transform.parent = element.gameObject.transform;
		this._Touch.transform.position = new Vector3(element.transform.position.x, element.transform.position.y, element.transform.position.z - 3f);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected void DestroyTouch(long touchItmIdx)
	{
		if (this._Touch == null)
		{
			return;
		}
		if (touchItmIdx != (long)this.guideitemmallID)
		{
			return;
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.guideWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		UnityEngine.Object.Destroy(this._Touch.gameObject);
		this._Touch = null;
	}

	private UIListItemContainer GetGuideItemProcess(int wantItemMallID)
	{
		int listItemIdx = -1;
		UIListItemContainer guideItem = this.GetGuideItem(wantItemMallID, ref listItemIdx, ref this.m_nlbVoucherPremium);
		if (guideItem != null)
		{
			this.ResetPosition(listItemIdx, ref this.m_nlbVoucherPremium);
			return guideItem;
		}
		guideItem = this.GetGuideItem(wantItemMallID, ref listItemIdx, ref this.m_nlbVoucher);
		if (guideItem != null)
		{
			this.ResetPosition(listItemIdx, ref this.m_nlbVoucher);
			return guideItem;
		}
		guideItem = this.GetGuideItem(wantItemMallID, ref listItemIdx, ref this.m_ListBox);
		if (guideItem != null)
		{
			this.ResetPosition(listItemIdx, ref this.m_ListBox);
			return guideItem;
		}
		return null;
	}

	private UIListItemContainer GetGuideItem(int wantItemMallID, ref int listBoxitemIdx, ref NewListBox checkListBox)
	{
		if (checkListBox == null)
		{
			return null;
		}
		for (int i = 0; i < checkListBox.Count; i++)
		{
			UIListItemContainer item = checkListBox.GetItem(i);
			if (!(item == null) && item.Data != null)
			{
				ITEM_MALL_ITEM iTEM_MALL_ITEM = item.Data as ITEM_MALL_ITEM;
				if (iTEM_MALL_ITEM != null)
				{
					if ((long)wantItemMallID == iTEM_MALL_ITEM.m_Idx)
					{
						listBoxitemIdx = i;
						return item;
					}
				}
			}
		}
		return null;
	}

	private void ResetPosition(int listItemIdx, ref NewListBox listbox)
	{
		if (listbox == null)
		{
			return;
		}
		listbox.ScrollPosition = (float)listItemIdx / (float)listbox.Count;
	}
}
