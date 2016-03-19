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

	private Label m_lbGold;

	private Label m_lbHearts;

	private Button m_btGoldState1;

	private Button m_btGoldState2;

	private Button m_btHeartsState1;

	private Button m_btHeartsState2;

	private Button m_btPolicy;

	private Toolbar m_tbTab;

	private Box m_BoxVoucherNotice;

	private bool m_bVoucherNotice;

	private Label m_lbVoucherInfo;

	private NewListBox m_nlbVoucher;

	private Label m_lbvip_level;

	private Label m_lbvip_exp;

	private DrawTexture m_DT_VipDrawTextureBg1;

	private DrawTexture m_DT_VipDrawTextureBg2;

	private Button m_BT_VIP_info;

	private int m_HeartsCount;

	private long m_Money;

	private ITEM_MALL_ITEM m_SelectItem;

	private NrMyCharInfo m_pkMychar;

	private Button m_bJpnPolicy1;

	private Button m_bJpnPolicy2;

	private eITEMMALL_TYPE m_ShowTab = eITEMMALL_TYPE.END;

	private static eITEMMALL_TYPE m_FirstTab = eITEMMALL_TYPE.END;

	private string m_strText = string.Empty;

	private string m_strTime = string.Empty;

	private ItemMallDlg.eMODE m_eMode;

	private float m_fDelayTime;

	public static eITEMMALL_TYPE FirstTab
	{
		get
		{
			if (ItemMallDlg.m_FirstTab != eITEMMALL_TYPE.END)
			{
				return ItemMallDlg.m_FirstTab;
			}
			for (int i = 1; i < 9; i++)
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
		this.m_btGoldState1 = (base.GetControl("Button_goldState1") as Button);
		this.m_btGoldState2 = (base.GetControl("Button_goldState2") as Button);
		this.m_btHeartsState1 = (base.GetControl("Button_heartsState1") as Button);
		this.m_btHeartsState2 = (base.GetControl("Button_heartsState2") as Button);
		this.m_btGoldState1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtGoldState));
		this.m_btGoldState2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtGoldState));
		this.m_btHeartsState1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btHeartsState2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtHeartsState));
		this.m_btPolicy = (base.GetControl("Button_policy") as Button);
		this.m_btPolicy.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPolicy));
		if (this.m_btPolicy != null)
		{
			this.m_btPolicy.Visible = false;
		}
		this.m_ListTab = (base.GetControl("NLB_ItemMallTab") as NewListBox);
		this.m_ListTab.Reserve = false;
		this.m_BoxVoucherNotice = (base.GetControl("Box_Notice") as Box);
		this.m_BoxVoucherNotice.SetLocation(this.m_BoxVoucherNotice.GetLocationX(), this.m_BoxVoucherNotice.GetLocationY(), -20f);
		this.m_BoxVoucherNotice.Visible = false;
		int num = 0;
		for (int i = 1; i < 9; i++)
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
		for (int j = 1; j < 9; j++)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsShopTab(j))
			{
				NewListItem newListItem = new NewListItem(this.m_ListTab.ColumnNum, true);
				newListItem.SetListItemData(0, string.Empty, (eITEMMALL_TYPE)j, new EZValueChangedDelegate(this.OnClickTab), null);
				newListItem.Data = (eITEMMALL_TYPE)j;
				string text = string.Empty;
				string text2 = string.Empty;
				switch (j)
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
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2007");
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
				}
				if (text != string.Empty)
				{
					newListItem.SetListItemData(2, text, null, null, null);
				}
				if (text2 != string.Empty)
				{
					newListItem.SetListItemData(1, text2, null, null, null);
				}
				this.m_ListTab.Add(newListItem);
			}
		}
		this.m_ListTab.RepositionItems();
		for (int k = 0; k < this.m_ListTab.Count; k++)
		{
			UIListItemContainer uIListItemContainer = this.m_ListTab.GetItem(k) as UIListItemContainer;
			UIRadioBtn componentInChildren = uIListItemContainer.GetComponentInChildren<UIRadioBtn>();
			componentInChildren.SetSize((float)num4, componentInChildren.GetSize().y);
			Label componentInChildren2 = uIListItemContainer.GetComponentInChildren<Label>();
			componentInChildren2.SetLocation(componentInChildren2.GetLocationX() + (float)num5, componentInChildren2.GetLocationY());
			DrawTexture componentInChildren3 = uIListItemContainer.GetComponentInChildren<DrawTexture>();
			componentInChildren3.SetLocation(componentInChildren3.GetLocationX() + (float)num5, componentInChildren3.GetLocationY());
		}
		this.m_ListTab.RepositionItems();
		this.m_HeartsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		this.m_lbHearts.SetText(ANNUALIZED.Convert(this.m_HeartsCount));
		this.m_Money = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
		this.m_lbGold.SetText(ANNUALIZED.Convert(this.m_Money));
		this.m_ListBox = (base.GetControl("NLB_Shop") as NewListBox);
		this.m_ListBox.Reserve = false;
		this.m_tbTab = (base.GetControl("ToolBar_ItemMall") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450");
		UIPanelTab expr_56E = this.m_tbTab.Control_Tab[0];
		expr_56E.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_56E.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2726");
		UIPanelTab expr_5BD = this.m_tbTab.Control_Tab[1];
		expr_5BD.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_5BD.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
		this.m_tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2781");
		UIPanelTab expr_60C = this.m_tbTab.Control_Tab[2];
		expr_60C.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_60C.ButtonClick, new EZValueChangedDelegate(this.OnClickMode));
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
		this.m_lbvip_level = (base.GetControl("Label_Label_vip_level") as Label);
		this.m_lbvip_exp = (base.GetControl("Label_Label_vip_exp") as Label);
		this.m_DT_VipDrawTextureBg1 = (base.GetControl("vip_DrawTexture_bg01") as DrawTexture);
		this.m_DT_VipDrawTextureBg2 = (base.GetControl("vip_DrawTexture_bg2") as DrawTexture);
		this.m_BT_VIP_info = (base.GetControl("Button_VIP_info") as Button);
		this.m_BT_VIP_info.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickVipInfo));
		this.SetShowData();
		if (!this.CheckNetwork())
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
		this.m_bJpnPolicy1 = (base.GetControl("Line_policy_1") as Button);
		this.m_bJpnPolicy2 = (base.GetControl("Line_policy_2") as Button);
		this.m_bJpnPolicy1.Visible = false;
		this.m_bJpnPolicy2.Visible = false;
		base.SetScreenCenter();
	}

	private void ClickPolicy1(IUIObject obj)
	{
	}

	private void ClickPolicy2(IUIObject obj)
	{
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
			UIListItemContainer uIListItemContainer = this.m_ListTab.GetItem(i) as UIListItemContainer;
			if ((int)uIListItemContainer.Data == (int)this.m_ShowTab)
			{
				UIRadioBtn componentInChildren = uIListItemContainer.GetComponentInChildren<UIRadioBtn>();
				componentInChildren.Value = true;
			}
		}
		this.SetShowData();
	}

	public void SetShowMode(ItemMallDlg.eMODE _ItemMode)
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
				NewListItem newListItem = new NewListItem(this.m_ListBox.ColumnNum, true);
				newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(group[i].m_strTextKey), null, null, null);
				newListItem.SetListItemData(3, group[i].m_strIconPath, true, null, null);
				string empty = string.Empty;
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)group[i].m_nMoneyType));
				if (group[i].m_nGroup == 8)
				{
					COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
					if (instance == null)
					{
						goto IL_33E;
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
				bool flag = false;
				ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(group[i].m_Idx);
				if (itemVoucherDataFromItemID != null)
				{
					long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime((eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
					if (voucherRemainTime > 0L)
					{
						newListItem.SetListItemData(4, this.GetTimeToString(voucherRemainTime), null, null, null);
						flag = true;
					}
				}
				if (!flag)
				{
					newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(group[i].m_strItemTextKey), null, null, null);
				}
				newListItem.SetListItemData(5, string.Empty, group[i], new EZValueChangedDelegate(this.OnClickButton), null);
				newListItem.SetListItemData(6, loader, null, null, null);
				newListItem.SetListItemData(7, ItemMallItemManager.GetCashPrice(group[i]), null, null, null);
				newListItem.SetListItemData(10, string.Empty, group[i], new EZValueChangedDelegate(this.OnClickTooltip), null);
				newListItem.SetListItemData(11, group[i].m_isEvent);
				newListItem.Data = group[i].m_Idx;
				this.m_ListBox.Add(newListItem);
			}
			IL_33E:;
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
		this.m_nlbVoucher.Clear();
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
		this.m_nlbVoucher.RepositionItems();
		this.m_nlbVoucher.Visible = true;
	}

	public void SetVipData()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((int)charSubData);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2790"),
				"level",
				levelExp
			});
			this.m_lbvip_level.SetText(empty);
			long num = (long)NrTSingleton<NrTableVipManager>.Instance.GetLevelMaxExp(levelExp);
			long num2 = (long)NrTSingleton<NrTableVipManager>.Instance.GetNextLevelMaxExp(levelExp);
			string text = string.Empty;
			if (num2 == 0L)
			{
				num = (long)NrTSingleton<NrTableVipManager>.Instance.GetBeforLevelMaxExp(levelExp);
				text = num.ToString() + " / " + num.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize(this.m_DT_VipDrawTextureBg1.width, this.m_DT_VipDrawTextureBg2.GetSize().y);
			}
			else
			{
				long num3 = charSubData - num;
				long num4 = num2 - num;
				float num5 = (float)num3 / (float)num4;
				text = num3.ToString() + " / " + num4.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize((this.m_DT_VipDrawTextureBg1.GetSize().x - 2f) * num5, this.m_DT_VipDrawTextureBg1.GetSize().y);
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
		this.UpdateTimeNormal();
		this.UpdateTimeVoucher();
		if (this.m_bVoucherNotice)
		{
			this.m_BoxVoucherNotice.Visible = true;
		}
		else
		{
			this.m_BoxVoucherNotice.Visible = false;
		}
	}

	public void OnClickVipInfo(IUIObject obj)
	{
		this.SetVipInfoShow(0);
	}

	public void SetVipInfoShow(byte i8Level)
	{
		this.SetVipData();
		VipInfoDlg vipInfoDlg = (VipInfoDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.VIPINFO_DLG);
		if (vipInfoDlg != null)
		{
			vipInfoDlg.SetLevel(i8Level);
		}
	}

	public void OnClickBtHeartsState(IUIObject obj)
	{
		this.SetShowType(eITEMMALL_TYPE.BUY_HEARTS);
	}

	public void OnClickBtGoldState(IUIObject obj)
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
		this.BuyItem(itemMall);
	}

	public void OnClickPolicy(IUIObject obj)
	{
		NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
		nrMobileNoticeWeb.OnPolicyView();
	}

	public void BuyItem(ITEM_MALL_ITEM _ItemMall)
	{
		string empty = string.Empty;
		this.m_SelectItem = null;
		if (_ItemMall != null)
		{
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(_ItemMall.m_Idx) || !NrTSingleton<ItemMallItemManager>.Instance.BuyItem(_ItemMall.m_Idx))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("221"));
				return;
			}
			if (_ItemMall.m_nMoneyType == 1)
			{
				if (!this.CheckNetwork())
				{
					return;
				}
				int posType = 6;
				switch (NrTSingleton<ItemManager>.Instance.GetItemTypeByItemUnique((int)_ItemMall.m_nItemUnique))
				{
				case eITEM_TYPE.ITEMTYPE_BOX:
				case eITEM_TYPE.ITEMTYPE_HEAL:
				case eITEM_TYPE.ITEMTYPE_SUPPLY:
					posType = 5;
					break;
				case eITEM_TYPE.ITEMTYPE_QUEST:
				case eITEM_TYPE.ITEMTYPE_MATERIAL:
					posType = 6;
					break;
				case eITEM_TYPE.ITEMTYPE_TICKET:
					posType = 7;
					break;
				}
				if (NkUserInventory.GetInstance().GetEmptySlot(posType) == -1)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
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
				if (readySolList == null || readySolList.GetCount() >= 50)
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
				if (_ItemMall.m_nGroup == 50 || _ItemMall.m_nGroup == 51)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("273"),
						"targetname",
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_ItemMall.m_strTextKey)
					});
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
						"targetname",
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_ItemMall.m_strTextKey)
					});
				}
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
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("273"),
						"targetname",
						empty2
					});
				}
				msgBoxUI.OkEventImmediatelyClose = false;
				NrTSingleton<ItemMallItemManager>.Instance.CheckMsgBox = msgBoxUI;
				this.m_SelectItem = _ItemMall;
				bool flag = false;
				if (_ItemMall.m_nGift == 1 && (TsPlatform.IsEditor || TsPlatform.IsAndroid))
				{
					flag = true;
				}
				if (flag)
				{
					msgBoxUI.OkEventImmediatelyClose = true;
					msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), msgBoxUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2578"), eMsgType.MB_CHECK_OK_CANCEL);
					msgBoxUI.SetCheckBoxState(false);
				}
				else
				{
					msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
				}
			}
		}
	}

	public void OnClickTooltip(IUIObject obj)
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
			itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.OnClickButton));
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		if (!NrTSingleton<ItemMallItemManager>.Instance.CanBuyItemByHeartsOrGold_Notify(this.m_SelectItem))
		{
			MsgBoxUI msgBoxUI = EventObject as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.Close();
			}
			return;
		}
		if (TsPlatform.IsEditor || TsPlatform.IsAndroid)
		{
			MsgBoxUI msgBoxUI2 = EventObject as MsgBoxUI;
			if (msgBoxUI2 != null && msgBoxUI2.IsChecked())
			{
				ItemGiftTargetDlg itemGiftTargetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMGIFTTARGET_DLG) as ItemGiftTargetDlg;
				if (itemGiftTargetDlg != null)
				{
					itemGiftTargetDlg.SetTradeItem(this.m_SelectItem, ItemMallItemManager.eItemMall_SellType.ITEMMALL);
				}
				return;
			}
		}
		NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(this.m_SelectItem, ItemMallItemManager.eItemMall_SellType.ITEMMALL);
		GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
		gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = this.m_SelectItem.m_Idx;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
		Debug.LogError("Check1");
	}

	public void RefreshData()
	{
		this.SetShowData();
	}

	public override void OnClose()
	{
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
	}

	private bool CheckNetwork()
	{
		if (!BaseNet_Game.GetInstance().IsSocketConnected())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("382"));
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMMALL_DLG);
			return false;
		}
		return true;
	}

	private void ClickView(IUIObject obj)
	{
		this.SetShowData();
	}

	public bool IsAddItemList(ITEM_MALL_ITEM ItemMallItem)
	{
		return NrTSingleton<ContentsLimitManager>.Instance.IsShopProduct(ItemMallItem.m_Idx) && NrTSingleton<ItemMallItemManager>.Instance.BuyItem(ItemMallItem.m_Idx);
	}

	public string GetTimeToString(long i64Time)
	{
		this.m_strText = string.Empty;
		if (i64Time > 0L)
		{
			long hourFromSec = PublicMethod.GetHourFromSec(i64Time);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(i64Time);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1092"),
				"hour",
				hourFromSec,
				"min",
				minuteFromSec
			});
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
			UIListItemContainer uIListItemContainer = this.m_ListBox.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				long index = (long)uIListItemContainer.Data;
				ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(index);
				if (item != null)
				{
					if (NrTSingleton<ItemMallItemManager>.Instance.IsItemVoucherType(item))
					{
						Label label = uIListItemContainer.GetElement(4) as Label;
						if (!(label == null))
						{
							long voucherRemainTimeFromItemID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTimeFromItemID(item.m_Idx);
							if (voucherRemainTimeFromItemID > 0L)
							{
								label.SetText(this.GetTimeToString(voucherRemainTimeFromItemID));
							}
							else
							{
								label.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(item.m_strItemTextKey));
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
		NewListItem newListItem = new NewListItem(this.m_nlbVoucher.ColumnNum, true);
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
		newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey), null, null, null);
		newListItem.SetListItemData(2, Item.m_strIconPath, true, null, null);
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
				newListItem.SetListItemData(3, this.m_strText, Item, new EZValueChangedDelegate(this.ClickVoucherUse), null);
			}
			else
			{
				long nextUseVoucherTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetNextUseVoucherTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
				this.m_strTime = this.GetTimeToString(nextUseVoucherTime);
				newListItem.SetListItemData(3, this.m_strTime, Item, new EZValueChangedDelegate(this.ClickVoucherUse), null);
			}
			long voucherEndTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherEndTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
			newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2740"), Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
			newListItem.SetListItemData(8, this.GetTimeToString(voucherEndTime, "2730"), null, null, null);
			newListItem.SetListItemData(5, false);
			newListItem.SetListItemData(6, false);
			long voucherStartTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherStartTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
			newListItem.SetListItemData(7, this.GetTimeToString(voucherStartTime, "2729"), null, null, null);
		}
		else
		{
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
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
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(8, false);
		}
		newListItem.SetListItemData(9, string.Empty, Item, new EZValueChangedDelegate(this.OnClickTooltip), null);
		newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTextKey), null, null, null);
		newListItem.SetListItemData(11, Item.m_isEvent);
		newListItem.Data = Item;
		this.m_nlbVoucher.Add(newListItem);
	}

	public void MakeVoucherHero(ITEM_MALL_ITEM Item)
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
		NewListItem newListItem = new NewListItem(this.m_nlbVoucher.ColumnNum, true);
		eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
		newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey), null, null, null);
		newListItem.SetListItemData(2, Item.m_strIconPath, true, null, null);
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
		newListItem.SetListItemData(3, false);
		newListItem.SetListItemData(4, string.Empty, Item, new EZValueChangedDelegate(this.ClickBuyVoucher), null);
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
		newListItem.SetListItemData(7, false);
		newListItem.SetListItemData(8, false);
		newListItem.SetListItemData(9, string.Empty, Item, new EZValueChangedDelegate(this.OnClickTooltip), null);
		newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTextKey), null, null, null);
		newListItem.SetListItemData(11, Item.m_isEvent);
		newListItem.Data = Item;
		this.m_nlbVoucher.Add(newListItem);
	}

	private void ClickVoucherUse(IUIObject obj)
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

	private void ClickBuyVoucher(IUIObject obj)
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
		this.BuyItem(iTEM_MALL_ITEM);
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
			if (readySolList == null || readySolList.GetCount() >= 50)
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

	private void OnClickMode(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eMode = (ItemMallDlg.eMODE)uIPanelTab.panel.index;
		this.SetShowData();
	}

	private void UpdateTimeVoucher()
	{
		for (int i = 0; i < this.m_nlbVoucher.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.m_nlbVoucher.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				if (uIListItemContainer.Data != null)
				{
					ITEM_MALL_ITEM iTEM_MALL_ITEM = uIListItemContainer.Data as ITEM_MALL_ITEM;
					if (iTEM_MALL_ITEM != null)
					{
						ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
						if (itemVoucherDataFromItemID != null)
						{
							this.UpdateTimeVoucherControl(uIListItemContainer, itemVoucherDataFromItemID, iTEM_MALL_ITEM);
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
}
