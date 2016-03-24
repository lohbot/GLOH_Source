using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class TimeShop_DLG : Form
{
	private enum eTIMESHOP_LAYER
	{
		LAYER_BASIC,
		LAYER_MONEY,
		LAYER_VIP = 4,
		MAX
	}

	private int m_nHeartsCount;

	private int m_nSoulGemsCount;

	private byte m_i8VipLevel;

	private byte m_i8SlotCount;

	private long m_i64RemainTime;

	private Dictionary<int, TIMESHOP_DATA> m_dicTimeShopItem = new Dictionary<int, TIMESHOP_DATA>();

	private Label m_lbNowHearts;

	private Label m_lbVIPLevel;

	private Label m_lbVIPExp;

	private Label m_lbRemainTime;

	private Label m_lbRewardHelp;

	private Label m_lbSoulGem;

	private DrawTexture m_dtVIPExpBarBG;

	private DrawTexture m_dtVIPExpBar;

	private DrawTexture m_dtVIPMark;

	private DrawTexture m_dtBanner;

	private Button m_btnHeartStateBG;

	private Button m_btnHeartState;

	private Button m_btnVIPInfo;

	private Button m_btnRefresh;

	private Button m_btnSoulGemState;

	private Button m_btnHelp;

	private Button m_btnRewardHelp;

	private Button m_btnBanner;

	private Button m_btnSoulGem;

	private Button m_btnRefreshReward;

	private NewListBox m_nlbItemList;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_timeshop", G_ID.TIMESHOP_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbNowHearts = (base.GetControl("Label_hearts") as Label);
		this.m_lbVIPLevel = (base.GetControl("Label_Label_vip_level") as Label);
		this.m_lbVIPExp = (base.GetControl("Label_Label_vip_exp") as Label);
		this.m_lbRemainTime = (base.GetControl("LB_RemainTime") as Label);
		this.m_lbRewardHelp = (base.GetControl("LB_Refresh_Reward") as Label);
		this.m_lbSoulGem = (base.GetControl("Label_SoulGem") as Label);
		this.m_dtVIPExpBarBG = (base.GetControl("vip_DrawTexture_bg01") as DrawTexture);
		this.m_dtVIPExpBar = (base.GetControl("vip_DrawTexture_bg2") as DrawTexture);
		this.m_dtVIPMark = (base.GetControl("DrawTexture_VIPMark") as DrawTexture);
		this.m_dtBanner = (base.GetControl("DT_Banner") as DrawTexture);
		this.m_dtBanner.SetTextureFromBundle("ui/itemshop/timeshop_banner");
		this.m_btnHeartStateBG = (base.GetControl("Button_heartsState1") as Button);
		this.m_btnHeartStateBG.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeartState));
		this.m_btnHeartState = (base.GetControl("Button_heartsState2") as Button);
		this.m_btnHeartState.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeartState));
		this.m_btnVIPInfo = (base.GetControl("Button_VIP_info") as Button);
		this.m_btnVIPInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_VIPInfo));
		this.m_btnRefresh = (base.GetControl("BT_Refresh") as Button);
		this.m_btnRefresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Refresh));
		this.m_btnSoulGemState = (base.GetControl("Button_SoulGemState1") as Button);
		this.m_btnSoulGemState.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeartState));
		this.m_btnSoulGem = (base.GetControl("Button_SoulGem") as Button);
		this.m_btnSoulGem.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeartState));
		this.m_btnHelp = (base.GetControl("BT_Help") as Button);
		this.m_btnHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Help));
		this.m_btnRewardHelp = (base.GetControl("BT_RewardHelp") as Button);
		this.m_btnRewardHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_RewardHelp));
		this.m_btnBanner = (base.GetControl("BT_Banner") as Button);
		this.m_btnBanner.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Help));
		this.m_btnRefreshReward = (base.GetControl("BT_Refresh_RewardGet") as Button);
		this.m_btnRefreshReward.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_RewardHelp));
		this.m_nlbItemList = (base.GetControl("NLB_Timeshop") as NewListBox);
		this.Init_Data();
		this.Set_UserInfo();
	}

	public override void Update()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		if (this.m_nHeartsCount != NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			this.m_nHeartsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
			this.m_lbNowHearts.SetText(ANNUALIZED.Convert(this.m_nHeartsCount));
		}
		if (this.m_nSoulGemsCount != NkUserInventory.GetInstance().Get_First_ItemCnt(70002))
		{
			this.m_nSoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
			this.m_lbSoulGem.SetText(ANNUALIZED.Convert(this.m_nSoulGemsCount));
		}
		this.Update_RemainTime();
	}

	private void Init_Data()
	{
		this.m_dicTimeShopItem.Clear();
	}

	public void Set_UserInfo()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Get_UserTimeShopItemListCount() <= 0)
		{
			this.Request_TimeShopInfo();
			return;
		}
		this.m_nHeartsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70000);
		this.m_lbNowHearts.SetText(ANNUALIZED.Convert(this.m_nHeartsCount));
		this.m_nSoulGemsCount = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
		this.m_lbSoulGem.SetText(ANNUALIZED.Convert(this.m_nSoulGemsCount));
		this.Set_RewardButton();
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			base.SetShowLayer(4, false);
			this.Set_ItemList();
			return;
		}
		base.SetShowLayer(4, true);
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
		this.m_i8VipLevel = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2790"),
			"level",
			this.m_i8VipLevel
		});
		this.m_lbVIPLevel.SetText(empty);
		this.m_dtVIPMark.SetTextureFromBundle(string.Format("UI/etc/{0}", NrTSingleton<NrVipSubInfoManager>.Instance.GetIconPath(this.m_i8VipLevel)));
		long num = NrTSingleton<NrTableVipManager>.Instance.GetLevelMaxExp(this.m_i8VipLevel);
		long nextLevelMaxExp = NrTSingleton<NrTableVipManager>.Instance.GetNextLevelMaxExp(this.m_i8VipLevel);
		string text = string.Empty;
		if (nextLevelMaxExp == 0L)
		{
			num = NrTSingleton<NrTableVipManager>.Instance.GetBeforLevelMaxExp(this.m_i8VipLevel);
			text = num.ToString() + " / " + num.ToString();
			this.m_dtVIPExpBar.SetSize(this.m_dtVIPExpBarBG.width, this.m_dtVIPExpBar.GetSize().y);
		}
		else
		{
			long num2 = charSubData - num;
			long num3 = nextLevelMaxExp - num;
			float num4 = (float)num2 / (float)num3;
			text = num2.ToString() + " / " + num3.ToString();
			this.m_dtVIPExpBar.SetSize((this.m_dtVIPExpBarBG.GetSize().x - 2f) * num4, this.m_dtVIPExpBarBG.GetSize().y);
		}
		this.m_lbVIPExp.SetText(text);
		this.Set_ItemList();
	}

	public void Set_ItemList()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		List<TIMESHOP_ITEMINFO> userTimeShopItemList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Get_UserTimeShopItemList();
		if (userTimeShopItemList == null)
		{
			return;
		}
		int value;
		if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			this.m_i8SlotCount = (byte)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TIMESHOP_MIN_SLOTCOUNT);
			value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TIMESHOP_MIN_SLOTCOUNT);
		}
		else
		{
			this.m_i8SlotCount = NrTSingleton<NrTableVipManager>.Instance.GetTimeShopCountByVipLevel(this.m_i8VipLevel);
			value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TIMESHOP_TYPE_SLOTNUM);
		}
		this.m_nlbItemList.Clear();
		this.m_dicTimeShopItem.Clear();
		for (int i = 0; i < userTimeShopItemList.Count; i++)
		{
			byte byType;
			if (i < value - 1)
			{
				byType = 0;
			}
			else
			{
				byType = 1;
			}
			TIMESHOP_DATA tIMESHOP_DATA = NrTSingleton<NrTableTimeShopManager>.Instance.Get_TimeShopDataByIDX(byType, userTimeShopItemList[i].i64IDX);
			if (tIMESHOP_DATA != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlbItemList.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(tIMESHOP_DATA.m_strProductTextKey), null, null, null);
				newListItem.SetListItemData(3, tIMESHOP_DATA.m_strIconPath, true, null, null);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3335"),
					"itemname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(tIMESHOP_DATA.m_strProductTextKey),
					"itemnum",
					tIMESHOP_DATA.m_lItemNum
				});
				newListItem.SetListItemData(4, empty, null, null, null);
				newListItem.SetListItemData(5, string.Empty, tIMESHOP_DATA, new EZValueChangedDelegate(this.Click_Buy), null);
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(NrTSingleton<NrTableTimeShopManager>.Instance.Get_MoneyTypeTextureName((eTIMESHOP_MONEYTYPE)tIMESHOP_DATA.m_nMoneyType));
				newListItem.SetListItemData(6, loader, null, null, null);
				newListItem.SetListItemData(7, tIMESHOP_DATA.m_lPrice.ToString(), null, null, null);
				newListItem.SetListItemData(9, string.Empty, tIMESHOP_DATA, new EZValueChangedDelegate(this.Click_ToolTip), null);
				newListItem.SetListItemData(14, false);
				newListItem.SetListItemData(15, NrTSingleton<NrTableTimeShopManager>.Instance.IsRecommend(byType, tIMESHOP_DATA.m_lIdx));
				newListItem.SetListItemData(16, false);
				newListItem.SetListItemData(17, false);
				newListItem.SetListItemData(19, false);
				if (tIMESHOP_DATA.m_lDisplayPrice > 0L)
				{
					newListItem.SetListItemData(7, false);
					newListItem.SetListItemData(12, tIMESHOP_DATA.m_lDisplayPrice.ToString(), null, null, null);
					newListItem.SetListItemData(13, tIMESHOP_DATA.m_lPrice.ToString(), null, null, null);
					newListItem.SetListItemData(14, true);
				}
				if (i <= (int)(this.m_i8SlotCount - 1))
				{
					if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBuy_TimeShopItemByIDX(tIMESHOP_DATA.m_lIdx))
					{
						newListItem.SetListItemEnable(5, false);
						newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3341"), null, null, null);
						newListItem.SetListItemData(6, false);
						newListItem.SetListItemData(7, false);
						newListItem.SetListItemData(12, false);
						newListItem.SetListItemData(13, false);
						newListItem.SetListItemData(14, false);
						newListItem.SetListItemData(19, true);
					}
				}
				else
				{
					if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
					{
						goto IL_3E0;
					}
					newListItem.SetListItemData(16, true);
					newListItem.SetListItemData(17, true);
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3304"),
						"level",
						NrTSingleton<NrTableVipManager>.Instance.GetVipLevelByTimeShopCount((byte)(i + 1)).ToString()
					});
					newListItem.SetListItemData(18, empty2, null, null, null);
				}
				newListItem.Data = i;
				this.m_nlbItemList.Add(newListItem);
				this.m_dicTimeShopItem.Add(i, tIMESHOP_DATA);
			}
			IL_3E0:;
		}
		this.m_nlbItemList.RepositionItems();
	}

	private void Update_RemainTime()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		this.m_i64RemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.NextRefreshTime - PublicMethod.GetCurTime();
		this.m_lbRemainTime.SetText(NrTSingleton<NrTableTimeShopManager>.Instance.GetTimeToString(this.m_i64RemainTime));
	}

	public void Set_RewardButton()
	{
		bool flag = NrTSingleton<NrTableTimeShopManager>.Instance.Is_GetRefreshReward();
		this.m_btnRefreshReward.Visible = flag;
		this.m_btnRewardHelp.Visible = !flag;
		this.m_lbRewardHelp.Visible = !flag;
	}

	private void Click_HeartState(IUIObject _obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}

	private void Click_VIPInfo(IUIObject _obj)
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
			VipInfoDlg vipInfoDlg = (VipInfoDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.VIPINFO_DLG);
			if (vipInfoDlg != null)
			{
				vipInfoDlg.SetLevel(b, false);
			}
		}
	}

	private void Click_Refresh(IUIObject _obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		TIMESHOP_REFRESHDATA tIMESHOP_REFRESHDATA = NrTSingleton<NrTableTimeShopManager>.Instance.Get_TimeShopRefreshData(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.RefreshCount);
		if (tIMESHOP_REFRESHDATA == null)
		{
			return;
		}
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(tIMESHOP_REFRESHDATA.m_i32ItemUnique);
		string text = string.Empty;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
		{
			byte timeShopFreeCountByVipLevel = NrTSingleton<NrTableVipManager>.Instance.GetTimeShopFreeCountByVipLevel(this.m_i8VipLevel);
			byte b = (byte)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(29);
			if (b < timeShopFreeCountByVipLevel)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("445");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					text,
					"freecount",
					(int)(timeShopFreeCountByVipLevel - b)
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					msgBoxUI.SetMsg(new YesDelegate(this.Request_Refresh), tIMESHOP_REFRESHDATA, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), text, eMsgType.MB_OK_CANCEL, 2);
				}
				return;
			}
		}
		long num = (long)NkUserInventory.GetInstance().Get_First_ItemCnt(tIMESHOP_REFRESHDATA.m_i32ItemUnique) - tIMESHOP_REFRESHDATA.m_i64ItemNum;
		if (num < 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("442");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"targetitem3",
				itemNameByItemUnique,
				"targetitem3num",
				Mathf.Abs((float)num)
			});
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI2 != null)
			{
				msgBoxUI2.SetMsg(new YesDelegate(this.Open_ItemMall), tIMESHOP_REFRESHDATA, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), text, eMsgType.MB_OK_CANCEL, 2);
			}
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("441");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"itemname",
				itemNameByItemUnique,
				"count",
				tIMESHOP_REFRESHDATA.m_i64ItemNum
			});
			MsgBoxUI msgBoxUI3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI3 != null)
			{
				msgBoxUI3.SetMsg(new YesDelegate(this.Request_Refresh), tIMESHOP_REFRESHDATA, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), text, eMsgType.MB_OK_CANCEL, 2);
			}
		}
	}

	private void Click_Buy(IUIObject _obj)
	{
		TIMESHOP_DATA tIMESHOP_DATA = _obj.Data as TIMESHOP_DATA;
		if (tIMESHOP_DATA == null)
		{
			return;
		}
		int num = this.Get_ItemIndex(tIMESHOP_DATA.m_lIdx);
		if (num < 0)
		{
			return;
		}
		if (num > (int)(this.m_i8SlotCount - 1))
		{
			byte vipLevelByTimeShopCount = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelByTimeShopCount((byte)(num + 1));
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null)
			{
				lackGold_dlg.SetDataTimeShop((int)vipLevelByTimeShopCount);
			}
			return;
		}
		if (!this.CanBuyItemByMoneyType((eTIMESHOP_MONEYTYPE)this.m_dicTimeShopItem[num].m_nMoneyType, this.m_dicTimeShopItem[num].m_lPrice))
		{
			return;
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
			"targetname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(tIMESHOP_DATA.m_strProductTextKey)
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.Request_Buy), num, string.Empty, textFromMessageBox, eMsgType.MB_OK_CANCEL, 2);
		}
	}

	private void Click_ToolTip(IUIObject _obj)
	{
		TIMESHOP_DATA tIMESHOP_DATA = _obj.Data as TIMESHOP_DATA;
		if (tIMESHOP_DATA == null)
		{
			return;
		}
		int num = this.Get_ItemIndex(tIMESHOP_DATA.m_lIdx);
		if (num < 0)
		{
			return;
		}
		ItemMallProductDetailDlg itemMallProductDetailDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_PRODUCTDETAIL_DLG) as ItemMallProductDetailDlg;
		if (itemMallProductDetailDlg == null)
		{
			return;
		}
		itemMallProductDetailDlg.SetTimeShopItem(tIMESHOP_DATA, num);
		itemMallProductDetailDlg.SetButtonEvent(new FunDelegate(this.Click_Buy));
		itemMallProductDetailDlg.AddBlackBgClickCloseForm();
	}

	private void Click_Help(IUIObject _obj)
	{
		TimeShopInfo_DLG timeShopInfo_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TIMESHOP_INFO_DLG) as TimeShopInfo_DLG;
		if (timeShopInfo_DLG != null)
		{
			timeShopInfo_DLG.SetShowLayer(3, false);
		}
	}

	private void Click_RewardHelp(IUIObject _obj)
	{
		TimeShopInfo_DLG timeShopInfo_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TIMESHOP_INFO_DLG) as TimeShopInfo_DLG;
		if (timeShopInfo_DLG != null)
		{
			timeShopInfo_DLG.SetShowLayer(2, false);
		}
	}

	private void Request_TimeShopInfo()
	{
		GS_TIMESHOP_ITEMLIST_INFO_REQ obj = new GS_TIMESHOP_ITEMLIST_INFO_REQ();
		SendPacket.GetInstance().SendObject(2526, obj);
	}

	private void Request_Refresh(object _obj)
	{
		if (!(_obj is TIMESHOP_REFRESHDATA))
		{
			return;
		}
		GS_TIMESHOP_ITEMLIST_REFRESH_REQ gS_TIMESHOP_ITEMLIST_REFRESH_REQ = new GS_TIMESHOP_ITEMLIST_REFRESH_REQ();
		gS_TIMESHOP_ITEMLIST_REFRESH_REQ.bIsCompul = true;
		SendPacket.GetInstance().SendObject(2528, gS_TIMESHOP_ITEMLIST_REFRESH_REQ);
	}

	private void Request_Buy(object _obj)
	{
		int num = (int)_obj;
		if (num < 0)
		{
			return;
		}
		GS_TIMESHOP_ITEM_BUY_REQ gS_TIMESHOP_ITEM_BUY_REQ = new GS_TIMESHOP_ITEM_BUY_REQ();
		gS_TIMESHOP_ITEM_BUY_REQ.i8ItemIndex = (byte)num;
		gS_TIMESHOP_ITEM_BUY_REQ.i64IDX = this.m_dicTimeShopItem[num].m_lIdx;
		gS_TIMESHOP_ITEM_BUY_REQ.i32MoneyType = this.m_dicTimeShopItem[num].m_nMoneyType;
		gS_TIMESHOP_ITEM_BUY_REQ.i64Price = this.m_dicTimeShopItem[num].m_lPrice;
		gS_TIMESHOP_ITEM_BUY_REQ.i32ItemUnique = this.m_dicTimeShopItem[num].m_nItemUnique;
		gS_TIMESHOP_ITEM_BUY_REQ.i64ItemNum = this.m_dicTimeShopItem[num].m_lItemNum;
		SendPacket.GetInstance().SendObject(2530, gS_TIMESHOP_ITEM_BUY_REQ);
	}

	private void Open_ItemMall(object _obj)
	{
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			itemMallDlg.SetShowMode(ItemMallDlg.eMODE.eMODE_NORMAL);
			itemMallDlg.SetShowType(eITEMMALL_TYPE.BUY_HEARTS);
		}
	}

	public bool CanBuyItemByMoneyType(eTIMESHOP_MONEYTYPE _eMoneyType, long _i64Price)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		bool flag = true;
		if (_eMoneyType == eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_HEARTS)
		{
			if ((long)NkUserInventory.GetInstance().Get_First_ItemCnt(70000) < _i64Price)
			{
				text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70000);
				flag = false;
			}
		}
		else if (_eMoneyType == eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_GOLD)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money < _i64Price)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676");
				flag = false;
			}
		}
		else if (_eMoneyType == eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_SOULJAM)
		{
			if ((long)NkUserInventory.GetInstance().Get_First_ItemCnt(70002) < _i64Price)
			{
				text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70002);
				flag = false;
			}
		}
		else if (_eMoneyType == eTIMESHOP_MONEYTYPE.eTIMESHOP_MONEYTYPE_MYTHELXIR && (long)NkUserInventory.GetInstance().Get_First_ItemCnt(50311) < _i64Price)
		{
			text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(50311);
			flag = false;
		}
		if (!flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("443");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"targetitem4",
				text2
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this.Open_ItemMall), _eMoneyType, string.Empty, text, eMsgType.MB_OK_CANCEL, 2);
			}
		}
		return flag;
	}

	private int Get_ItemIndex(long _idx)
	{
		int result = -1;
		for (int i = 0; i < this.m_dicTimeShopItem.Count; i++)
		{
			if (this.m_dicTimeShopItem[i].m_lIdx == _idx)
			{
				return i;
			}
		}
		return result;
	}
}
