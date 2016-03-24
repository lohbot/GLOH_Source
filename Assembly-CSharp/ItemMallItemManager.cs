using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityForms;

public class ItemMallItemManager : NrTSingleton<ItemMallItemManager>
{
	public enum eItemMall_SellType
	{
		NONE,
		ITEMMALL,
		FIVEROCKSEVENT
	}

	private SortedDictionary<int, List<ITEM_MALL_ITEM>> m_OriginalCollection;

	private SortedDictionary<int, List<ITEM_MALL_ITEM>> m_sdCollection;

	private List<ITEMMALL_ITEM_BASE_DATA> m_ItemMallBaseData = new List<ITEMMALL_ITEM_BASE_DATA>();

	private List<ITEMMALL_DATA> m_ItemMallDataList = new List<ITEMMALL_DATA>();

	private List<ITEMMALLBUYCOUNT_INFO> m_ItemMallBuyCountList = new List<ITEMMALLBUYCOUNT_INFO>();

	private List<ITEM_VOUCHER_DATA> m_ItemVoucherDataList = new List<ITEM_VOUCHER_DATA>();

	private bool m_bTrading;

	private int m_iVoucherRefillTime;

	private ItemMallItemManager.eItemMall_SellType m_curSellType;

	private ITEM_MALL_ITEM m_mallItem;

	private MsgBoxUI m_checkMsgBox;

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

	public MsgBoxUI CheckMsgBox
	{
		set
		{
			this.m_checkMsgBox = value;
		}
	}

	public int VoucherRefillTime
	{
		get
		{
			return this.m_iVoucherRefillTime;
		}
		set
		{
			this.m_iVoucherRefillTime = value;
		}
	}

	private ItemMallItemManager()
	{
		this.m_sdCollection = new SortedDictionary<int, List<ITEM_MALL_ITEM>>();
		this.m_OriginalCollection = new SortedDictionary<int, List<ITEM_MALL_ITEM>>();
	}

	public SortedDictionary<int, List<ITEM_MALL_ITEM>> Get_Collection()
	{
		return this.m_sdCollection;
	}

	public int Get_Count()
	{
		return this.m_sdCollection.Count;
	}

	public void Set_Value(ITEM_MALL_ITEM a_cValue, int groupNum = -1)
	{
		if (groupNum < 0)
		{
			groupNum = a_cValue.m_nGroup;
		}
		if (this.m_sdCollection.ContainsKey(groupNum))
		{
			this.m_OriginalCollection[groupNum].Add(a_cValue);
			this.m_sdCollection[groupNum].Add(a_cValue);
		}
		else
		{
			this.m_OriginalCollection.Add(groupNum, new List<ITEM_MALL_ITEM>());
			this.m_OriginalCollection[groupNum].Add(a_cValue);
			this.m_sdCollection.Add(groupNum, new List<ITEM_MALL_ITEM>());
			this.m_sdCollection[groupNum].Add(a_cValue);
		}
		if (a_cValue.m_isRecommend && groupNum != 1)
		{
			this.Set_Value(a_cValue, 1);
		}
	}

	public List<ITEM_MALL_ITEM> GetGroup(int nGroup)
	{
		if (this.m_sdCollection.ContainsKey(nGroup))
		{
			return this.m_sdCollection[nGroup];
		}
		return null;
	}

	public ITEM_MALL_ITEM GetItem(long Index)
	{
		foreach (List<ITEM_MALL_ITEM> current in this.m_sdCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if (current[i].m_Idx == Index)
				{
					return current[i];
				}
			}
		}
		return null;
	}

	public ITEM_MALL_ITEM GetOriginalItem(long Index)
	{
		foreach (List<ITEM_MALL_ITEM> current in this.m_OriginalCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if (current[i].m_Idx == Index)
				{
					return current[i];
				}
			}
		}
		return null;
	}

	public string GetProductID(int index)
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		foreach (List<ITEM_MALL_ITEM> current in this.m_sdCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if (current[i].m_Idx == (long)index)
				{
					eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
					if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_ANDROID_USQA)
					{
						string result = current[i].m_strGoogleQA;
						return result;
					}
					if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
					{
						string result = current[i].m_strGoogle;
						return result;
					}
					if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_IOS_USIOS)
					{
						string result = current[i].m_strApple;
						return result;
					}
				}
			}
		}
		return string.Empty;
	}

	public string[] GetItems()
	{
		List<string> list = new List<string>();
		string text = string.Empty;
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		foreach (List<ITEM_MALL_ITEM> current in this.m_sdCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				text = string.Empty;
				eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
				if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_ANDROID_USQA)
				{
					if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
					{
						if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_IOS_USIOS)
						{
							text = current[i].m_strApple;
						}
					}
					else
					{
						text = current[i].m_strGoogle;
					}
				}
				else
				{
					text = current[i].m_strGoogleQA;
				}
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
		}
		return list.ToArray();
	}

	public long GetItemIndex(string ProductID)
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		foreach (List<ITEM_MALL_ITEM> current in this.m_sdCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				eSERVICE_AREA eSERVICE_AREA = currentServiceArea;
				if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_ANDROID_USQA)
				{
					if (eSERVICE_AREA != eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE)
					{
						if (eSERVICE_AREA == eSERVICE_AREA.SERVICE_IOS_USIOS)
						{
							if (!string.IsNullOrEmpty(current[i].m_strApple) && string.Equals(current[i].m_strApple, ProductID))
							{
								long idx = current[i].m_Idx;
								return idx;
							}
						}
					}
					else if (!string.IsNullOrEmpty(current[i].m_strGoogle) && string.Equals(current[i].m_strGoogle, ProductID))
					{
						long idx = current[i].m_Idx;
						return idx;
					}
				}
				else if (!string.IsNullOrEmpty(current[i].m_strGoogleQA) && string.Equals(current[i].m_strGoogleQA, ProductID))
				{
					long idx = current[i].m_Idx;
					return idx;
				}
			}
		}
		return 0L;
	}

	public void RecodeErrorMessage(ref GS_BILLING_ITEM_RECODE_REQ req, string error)
	{
		if (error.Length <= 0)
		{
			return;
		}
		if (error.Length > 0)
		{
			this.SubstringMessage(error, 0, ref req.Message1);
		}
		int num = 50;
		if (error.Length > num)
		{
			this.SubstringMessage(error, num, ref req.Message2);
		}
		num = 100;
		if (error.Length > num)
		{
			this.SubstringMessage(error, num, ref req.Message3);
		}
		num = 150;
		if (error.Length > num)
		{
			this.SubstringMessage(error, num, ref req.Message4);
		}
	}

	private void SubstringMessage(string error, int startlength, ref char[] retMessage)
	{
		if (error.Length > startlength && error.Length <= startlength + 50)
		{
			TKString.StringChar(error, ref retMessage, startlength, error.Length - startlength);
		}
		else
		{
			if (error.Length <= startlength)
			{
				return;
			}
			TKString.StringChar(error, ref retMessage, startlength, startlength + 50);
		}
	}

	public void SetTradeItem(ITEM_MALL_ITEM mallItem, ItemMallItemManager.eItemMall_SellType eSellStyle)
	{
		this.m_mallItem = mallItem;
		this.m_curSellType = eSellStyle;
	}

	public ITEM_MALL_ITEM GetTradeItem()
	{
		return this.m_mallItem;
	}

	public void TradeCheckFail(int iResult, string strGiftUserName)
	{
		if (this.m_checkMsgBox != null)
		{
			this.m_checkMsgBox.Close();
			this.m_checkMsgBox = null;
		}
		string message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("212");
		if (strGiftUserName != string.Empty && iResult == -10)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("36");
		}
		if (iResult == -5)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("718");
		}
		if (iResult == 9200)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("755");
		}
		MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG);
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), message, eMsgType.MB_OK, 2);
		}
		NrTSingleton<ItemMallItemManager>.Instance.Trading = false;
	}

	public void TradeItem()
	{
		if (this.m_checkMsgBox != null)
		{
			this.m_checkMsgBox.Close();
			this.m_checkMsgBox = null;
		}
		if (this.m_mallItem.m_nMoneyType == 1)
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE)
			{
				ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
				if (itemMallDlg != null)
				{
					itemMallDlg.CloseEnable = false;
				}
			}
			BillingManager.PurchaseItem(this.m_mallItem, this.m_curSellType == ItemMallItemManager.eItemMall_SellType.ITEMMALL);
			NrTSingleton<ItemMallItemManager>.Instance.Trading = false;
			this.m_mallItem = null;
		}
		else if (this.m_mallItem.m_nMoneyType != 4)
		{
			if (this.CanBuyItemByHeartsOrGold_Notify(this.m_mallItem))
			{
				GS_ITEMMALL_TRADE_REQ gS_ITEMMALL_TRADE_REQ = new GS_ITEMMALL_TRADE_REQ();
				gS_ITEMMALL_TRADE_REQ.MallIndex = this.m_mallItem.m_Idx;
				gS_ITEMMALL_TRADE_REQ.SolID = 0L;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_TRADE_REQ, gS_ITEMMALL_TRADE_REQ);
				this.m_mallItem = null;
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.Trading = false;
				this.m_mallItem = null;
			}
		}
	}

	public bool CanBuyItemByHeartsOrGold_Notify(ITEM_MALL_ITEM mallItem)
	{
		string empty = string.Empty;
		string text = string.Empty;
		if (mallItem.m_nMoneyType == 2)
		{
			if ((long)NkUserInventory.GetInstance().Get_First_ItemCnt(70000) < mallItem.m_nPrice)
			{
				text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70000);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				return false;
			}
		}
		else if (mallItem.m_nMoneyType == 3)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money < mallItem.m_nPrice)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676")
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				return false;
			}
		}
		else if (mallItem.m_nMoneyType == 5)
		{
			if ((long)NkUserInventory.GetInstance().Get_First_ItemCnt(70002) < mallItem.m_nPrice)
			{
				text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70002);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				return false;
			}
		}
		else if (mallItem.m_nMoneyType == 6 && (long)NkUserInventory.GetInstance().Get_First_ItemCnt(50311) < mallItem.m_nPrice)
		{
			text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(50311);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
				"targetname",
				text
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return false;
		}
		return true;
	}

	public void ClearItemMallData()
	{
		this.m_ItemMallDataList.Clear();
	}

	public void AddItemMallData(ITEMMALL_DATA Data)
	{
		for (int i = 0; i < this.m_ItemMallDataList.Count; i++)
		{
			if (this.m_ItemMallDataList[i].i64ItemMallIDX == Data.i64ItemMallIDX)
			{
				return;
			}
		}
		this.m_ItemMallDataList.Add(Data);
	}

	public bool BuyItem(long i64ItemMallIDX)
	{
		for (int i = 0; i < this.m_ItemMallDataList.Count; i++)
		{
			if (this.m_ItemMallDataList[i].i64ItemMallIDX == i64ItemMallIDX)
			{
				return false;
			}
		}
		return true;
	}

	public void Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE eItemMallType, bool bShowDlg)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMMALL_DLG);
		GS_ITEMMALL_INFO_REQ gS_ITEMMALL_INFO_REQ = new GS_ITEMMALL_INFO_REQ();
		gS_ITEMMALL_INFO_REQ.i32ItemMallType = (int)eItemMallType;
		gS_ITEMMALL_INFO_REQ.i32ItemMallMode = 0;
		gS_ITEMMALL_INFO_REQ.bShowDLG = bShowDlg;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_INFO_REQ, gS_ITEMMALL_INFO_REQ);
	}

	public void Send_GS_ITEMMALL_INFO_REQ(ItemMallDlg.eMODE IeItemMallMode)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMMALL_DLG);
		GS_ITEMMALL_INFO_REQ gS_ITEMMALL_INFO_REQ = new GS_ITEMMALL_INFO_REQ();
		gS_ITEMMALL_INFO_REQ.i32ItemMallType = 0;
		gS_ITEMMALL_INFO_REQ.i32ItemMallMode = (int)IeItemMallMode;
		gS_ITEMMALL_INFO_REQ.bShowDLG = true;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_INFO_REQ, gS_ITEMMALL_INFO_REQ);
	}

	public void ClearItemMallBuyCount()
	{
		this.m_ItemMallBuyCountList.Clear();
	}

	public void AddItemMallBuyCountInfo(long i64ItemMallID, int i32BuyCount)
	{
		for (int i = 0; i < this.m_ItemMallBuyCountList.Count; i++)
		{
			if (this.m_ItemMallBuyCountList[i].i64ItemMallID == i64ItemMallID)
			{
				this.m_ItemMallBuyCountList[i].i32BuyCount = i32BuyCount;
				return;
			}
		}
		ITEMMALLBUYCOUNT_INFO iTEMMALLBUYCOUNT_INFO = new ITEMMALLBUYCOUNT_INFO();
		iTEMMALLBUYCOUNT_INFO.i64ItemMallID = i64ItemMallID;
		iTEMMALLBUYCOUNT_INFO.i32BuyCount = i32BuyCount;
		this.m_ItemMallBuyCountList.Add(iTEMMALLBUYCOUNT_INFO);
	}

	public bool IsBuyCountLimit(ITEM_MALL_ITEM ItemMallItem)
	{
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = this.GetItemVoucherDataFromItemID(ItemMallItem.m_Idx);
		if (itemVoucherDataFromItemID != null)
		{
			long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime((eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
			TsLog.LogError("i64RemainTime ={0}", new object[]
			{
				voucherRemainTime
			});
			if (voucherRemainTime > 0L)
			{
				return true;
			}
		}
		if (ItemMallItem.m_nSaleNum == 0)
		{
			return true;
		}
		for (int i = 0; i < this.m_ItemMallBuyCountList.Count; i++)
		{
			if (this.m_ItemMallBuyCountList[i].i64ItemMallID == ItemMallItem.m_Idx)
			{
				return ItemMallItem.m_nSaleNum > this.m_ItemMallBuyCountList[i].i32BuyCount;
			}
		}
		return true;
	}

	public int GetBuyCount(long lItemMallID)
	{
		for (int i = 0; i < this.m_ItemMallBuyCountList.Count; i++)
		{
			if (this.m_ItemMallBuyCountList[i].i64ItemMallID == lItemMallID)
			{
				return this.m_ItemMallBuyCountList[i].i32BuyCount;
			}
		}
		return 0;
	}

	public void ClearItemMallItemBaseData()
	{
		this.m_ItemMallBaseData.Clear();
	}

	public void AddItemMallItemBaseData(ITEMMALL_ITEM_BASE_DATA BaseData)
	{
		for (int i = 0; i < this.m_ItemMallBaseData.Count; i++)
		{
			if (this.m_ItemMallBaseData[i].i64Idx == BaseData.i64Idx)
			{
				return;
			}
		}
		this.m_ItemMallBaseData.Add(BaseData);
	}

	public void RefreshItemMallData()
	{
		this.m_sdCollection.Clear();
		for (int i = 0; i < this.m_ItemMallBaseData.Count; i++)
		{
			ITEM_MALL_ITEM originalItem = this.GetOriginalItem(this.m_ItemMallBaseData[i].i64Idx);
			if (originalItem != null)
			{
				ITEM_MALL_ITEM iTEM_MALL_ITEM = new ITEM_MALL_ITEM();
				iTEM_MALL_ITEM.Set(originalItem);
				iTEM_MALL_ITEM.m_nGroup = (int)this.m_ItemMallBaseData[i].ui8Group;
				iTEM_MALL_ITEM.m_isRecommend = this.m_ItemMallBaseData[i].isRecommend;
				iTEM_MALL_ITEM.m_strTextKey = this.m_ItemMallBaseData[i].i32ProductTextKey.ToString();
				iTEM_MALL_ITEM.m_nPrice = (long)this.m_ItemMallBaseData[i].i32Price;
				iTEM_MALL_ITEM.m_fPrice = this.m_ItemMallBaseData[i].fPrice;
				iTEM_MALL_ITEM.m_nItemUnique = (long)this.m_ItemMallBaseData[i].i32ItemUnique;
				iTEM_MALL_ITEM.m_nItemNum = (long)this.m_ItemMallBaseData[i].i32ItemNum;
				iTEM_MALL_ITEM.m_nGetMoney = (long)this.m_ItemMallBaseData[i].i32GetMoney;
				iTEM_MALL_ITEM.m_nSaleNum = (int)this.m_ItemMallBaseData[i].ui8SaleNum;
				iTEM_MALL_ITEM.m_nGift = this.m_ItemMallBaseData[i].i8Gift;
				iTEM_MALL_ITEM.m_strItemTextKey = this.m_ItemMallBaseData[i].i32ItemTextKey.ToString();
				iTEM_MALL_ITEM.m_strItemTooltip = this.m_ItemMallBaseData[i].i32ItemToolTipKey.ToString();
				iTEM_MALL_ITEM.m_isEvent = this.m_ItemMallBaseData[i].i8Event;
				this.AddData(iTEM_MALL_ITEM, -1);
			}
		}
	}

	public void AddData(ITEM_MALL_ITEM Data, int groupNum = -1)
	{
		if (groupNum < 0)
		{
			groupNum = Data.m_nGroup;
		}
		if (this.m_sdCollection.ContainsKey(groupNum))
		{
			this.m_sdCollection[groupNum].Add(Data);
		}
		else
		{
			this.m_sdCollection.Add(groupNum, new List<ITEM_MALL_ITEM>());
			this.m_sdCollection[groupNum].Add(Data);
		}
		if (Data.m_isRecommend && groupNum != 1)
		{
			this.AddData(Data, 1);
		}
	}

	public static string GetCashTextureName(eITEMMALL_MONEY_TYPE type)
	{
		switch (type)
		{
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_CASH:
			if (TsPlatform.IsIPhone || NrGlobalReference.strLangType.Equals("eng"))
			{
				return "Win_I_Dolla";
			}
			return "Win_I_Won";
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_HEARTS:
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_BOX_HEARTS:
			return "Win_I_Hearts";
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_GOLD:
			return "Com_I_MoneyIcon";
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_SOULGEM:
			return "WIN_I_SoulGem";
		case eITEMMALL_MONEY_TYPE.MONEY_TYPE_MYTHELXIR:
			return "Win_I_MythElixir";
		default:
			return string.Empty;
		}
	}

	public static string GetCashPrice(ITEM_MALL_ITEM item)
	{
		if (item.m_nMoneyType == 1 && (TsPlatform.IsIPhone || NrGlobalReference.strLangType.Equals("eng")))
		{
			return Protocol_Item.Money_Format(item.m_fPrice);
		}
		return Protocol_Item.Money_Format(item.m_nPrice);
	}

	public ITEM_MALL_ITEM GetItemData(int ItemUnique)
	{
		if (ItemUnique <= 0)
		{
			return null;
		}
		foreach (List<ITEM_MALL_ITEM> current in this.m_sdCollection.Values)
		{
			for (int i = 0; i < current.Count; i++)
			{
				if (current[i].m_nItemUnique == (long)ItemUnique)
				{
					return current[i];
				}
			}
		}
		return null;
	}

	public bool IsItemVoucherType(ITEM_MALL_ITEM Item)
	{
		return Item != null && this.IsItemVoucherTypeFromItemID(Item.m_Idx);
	}

	public bool IsItemVoucherType(eVOUCHER_TYPE eVoucherType)
	{
		return eVoucherType > eVOUCHER_TYPE.eVOUCHER_TYPE_NONE && eVoucherType < eVOUCHER_TYPE.eVOUCHER_TYPE_MAX;
	}

	public bool IsItemVoucherTypeFromItemID(long i64ItemID)
	{
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = this.GetItemVoucherDataFromItemID(i64ItemID);
		return itemVoucherDataFromItemID != null && this.IsItemVoucherType((eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType);
	}

	public void ClearItemVoucherData()
	{
		this.m_ItemVoucherDataList.Clear();
	}

	public void AddItemVoucherData(ITEM_VOUCHER_DATA ItemVoucherData)
	{
		this.m_ItemVoucherDataList.Add(ItemVoucherData);
	}

	public int GetItemVoucherDataSize()
	{
		return this.m_ItemVoucherDataList.Count;
	}

	public ITEM_VOUCHER_DATA GetItemVoucherDataFromItemID(long i64ItemID)
	{
		for (int i = 0; i < this.m_ItemVoucherDataList.Count; i++)
		{
			if (this.m_ItemVoucherDataList[i].i64ItemMallID == i64ItemID)
			{
				return this.m_ItemVoucherDataList[i];
			}
		}
		return null;
	}

	public byte IsEventItem(int shopIdx)
	{
		ITEM_MALL_ITEM itemMall = this.GetItemMall(shopIdx);
		if (itemMall == null)
		{
			return 0;
		}
		return itemMall.m_isEvent;
	}

	public eITEMMALL_MONEY_TYPE GetMoneyType(int shopIdx)
	{
		ITEM_MALL_ITEM itemMall = this.GetItemMall(shopIdx);
		if (itemMall == null)
		{
			return eITEMMALL_MONEY_TYPE.END;
		}
		return (eITEMMALL_MONEY_TYPE)itemMall.m_nMoneyType;
	}

	public string GetCashPrice(int shopIdx)
	{
		ITEM_MALL_ITEM itemMall = this.GetItemMall(shopIdx);
		if (itemMall == null)
		{
			return string.Empty;
		}
		return ItemMallItemManager.GetCashPrice(itemMall);
	}

	public ITEM_MALL_ITEM GetItemMall(int shopIdx)
	{
		if (this.m_OriginalCollection == null)
		{
			return null;
		}
		foreach (List<ITEM_MALL_ITEM> current in this.m_OriginalCollection.Values)
		{
			if (current != null)
			{
				foreach (ITEM_MALL_ITEM current2 in current)
				{
					if (current2 != null)
					{
						if (current2.m_Idx == (long)shopIdx)
						{
							return current2;
						}
					}
				}
			}
		}
		return null;
	}
}
