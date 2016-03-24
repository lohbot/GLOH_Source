using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExchangePointDlg : Form
{
	private enum TYPE
	{
		TYPE_TICKET,
		TYPE_EQUIPITEM
	}

	private ExchangePointDlg.TYPE m_eType;

	private Toggle m_kTicket;

	private Toggle m_kEquipItem;

	private Label m_kHeroPoint;

	private Label m_kEquipPoint;

	private ItemTexture m_kSelectItem;

	private NewListBox m_kList;

	private Button m_kSell;

	private Button m_kMinus;

	private Button m_kPlus;

	private Button m_kAll;

	private Label m_kPoint;

	private Label m_kSelectItemName;

	private TextField m_kSellNum;

	private Label m_kEquipCount;

	private Label m_kTotalEquipPoint;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private long[] m_nRemoveItemID = new long[10];

	private Label m_kEquipPointName;

	private Label m_kHeroPointName;

	private DrawTexture m_kTextNumBack;

	private Button m_kInputBox;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Point/dlg_pointexchangemain", G_ID.EXCHANGE_POINT_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	private void InitRemoveItemID()
	{
		for (int i = 0; i < 10; i++)
		{
			this.m_nRemoveItemID[i] = 0L;
		}
	}

	private void SetPoint()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041"),
			"count",
			ANNUALIZED.Convert(myCharInfo.GetHeroPoint())
		});
		this.m_kHeroPoint.Text = empty;
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041"),
			"count",
			ANNUALIZED.Convert(myCharInfo.GetEquipPoint())
		});
		this.m_kEquipPoint.Text = empty;
	}

	public override void SetComponent()
	{
		this.m_kHeroPoint = (base.GetControl("Label_HeroPointCount") as Label);
		this.m_kEquipPoint = (base.GetControl("Label_ItemPointCount") as Label);
		this.m_kList = (base.GetControl("NewListBox_TicketExchange") as NewListBox);
		this.m_kList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_kList.MaxMultiSelectNum = 10;
		this.m_kTicket = (base.GetControl("CheckBox_Ticket") as Toggle);
		this.m_kTicket.Value = true;
		this.m_kTicket.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickTicket));
		this.m_kEquipItem = (base.GetControl("CheckBox_Item") as Toggle);
		this.m_kEquipItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEquipItem));
		this.m_kSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_kSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectItem));
		this.m_kSell = (base.GetControl("Button_confirm") as Button);
		this.m_kSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSell));
		this.m_kMinus = (base.GetControl("Button_CountDown") as Button);
		this.m_kMinus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMinus));
		this.m_kPlus = (base.GetControl("Button_CountUp") as Button);
		this.m_kPlus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPlus));
		this.m_kAll = (base.GetControl("Button_MaxCountUp") as Button);
		this.m_kAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAll));
		this.m_kMinus.Visible = false;
		this.m_kPlus.Visible = false;
		this.m_kAll.Visible = false;
		this.m_kPoint = (base.GetControl("Label_text8") as Label);
		this.m_kSelectItemName = (base.GetControl("Label_text") as Label);
		this.m_kSellNum = (base.GetControl("TextField_Count") as TextField);
		this.m_kSellNum.NumberMode = true;
		this.m_kSellNum.Visible = false;
		this.m_kEquipCount = (base.GetControl("Label_ItemCount") as Label);
		this.m_kTotalEquipPoint = (base.GetControl("Label_Itemtext2") as Label);
		this.m_kEquipPointName = (base.GetControl("Label_Itemtext") as Label);
		this.m_kEquipPointName.Visible = false;
		this.m_kHeroPointName = (base.GetControl("Label_text2") as Label);
		this.m_kHeroPointName.Visible = false;
		this.m_kTextNumBack = (base.GetControl("DrawTexture_DrawTexture22_C") as DrawTexture);
		this.m_kTextNumBack.Visible = false;
		this.m_kInputBox = (base.GetControl("Button_NumPad") as Button);
		this.m_kInputBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputBox));
		this.SetPoint();
		this.ShowTicketList();
		base.SetShowLayer(2, false);
		base.SetScreenCenter();
	}

	private void ClickInputBox(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		if (inputNumberDlg == null)
		{
			return;
		}
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.OnInputNumber), null, new Action<InputNumberDlg, object>(this.OnCloseInputNumber), null);
		long maxValue = this.m_kSellNum.MaxValue;
		long i64Min = 1L;
		inputNumberDlg.SetMinMax(i64Min, maxValue);
		inputNumberDlg.SetNum(0L);
		inputNumberDlg.SetInputNum(0L);
	}

	public void OnInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		long num = a_cForm.GetNum();
		this.m_nSelectItemNum = (int)num;
		this.m_kSellNum.Text = num.ToString();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
		ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
		if (iTEM == null)
		{
			return;
		}
		this.GetPoint(iTEM);
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void GetPoint(ITEM item)
	{
		if (this.m_eType == ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			PointTable pointTable = NrTSingleton<PointManager>.Instance.GetPointTable(item.m_nItemUnique);
			if (pointTable == null)
			{
				return;
			}
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2256"),
				"count",
				this.m_nSelectItemNum * pointTable.m_nGetPoint
			});
			this.m_kPoint.Text = empty;
		}
	}

	private void ClickMinus(IUIObject obj)
	{
		if (this.m_eType != ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			return;
		}
		ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
		if (iTEM == null)
		{
			return;
		}
		this.m_nSelectItemNum--;
		if (1 > this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = 1;
		}
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.GetPoint(iTEM);
	}

	private void ClickPlus(IUIObject obj)
	{
		if (this.m_eType != ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			return;
		}
		ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
		if (iTEM == null)
		{
			return;
		}
		this.m_nSelectItemNum++;
		if (iTEM.m_nItemNum < this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = iTEM.m_nItemNum;
		}
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.GetPoint(iTEM);
	}

	private void ClickAll(IUIObject obj)
	{
		if (this.m_eType != ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			return;
		}
		ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
		if (iTEM == null)
		{
			return;
		}
		this.m_nSelectItemNum = iTEM.m_nItemNum;
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.GetPoint(iTEM);
	}

	private void ClickSell(IUIObject obj)
	{
		GS_POINT_BUY_REQ gS_POINT_BUY_REQ = new GS_POINT_BUY_REQ();
		if (this.m_eType == ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			gS_POINT_BUY_REQ.nType = 0;
			gS_POINT_BUY_REQ.nItemUnique = this.m_nSelectItemUnique;
			gS_POINT_BUY_REQ.nItemNum = (long)this.m_nSelectItemNum;
		}
		else if (this.m_eType == ExchangePointDlg.TYPE.TYPE_EQUIPITEM)
		{
			bool flag = false;
			gS_POINT_BUY_REQ.nType = 1;
			for (int i = 0; i < 10; i++)
			{
				if (0L < this.m_nRemoveItemID[i])
				{
					gS_POINT_BUY_REQ.nItemID[i] = this.m_nRemoveItemID[i];
					ITEM itemFromItemID = NkUserInventory.instance.GetItemFromItemID(this.m_nRemoveItemID[i]);
					if (itemFromItemID != null)
					{
						if (itemFromItemID.IsLock())
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
						}
						else
						{
							int num = itemFromItemID.m_nOption[2];
							if (num >= 4)
							{
								flag = true;
							}
						}
					}
				}
				else
				{
					gS_POINT_BUY_REQ.nItemID[i] = 0L;
				}
			}
			if (flag)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2251");
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("198");
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), gS_POINT_BUY_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
					return;
				}
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_POINT_BUY_REQ, gS_POINT_BUY_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void MsgBoxOKEvent(object obj)
	{
		if (obj == null)
		{
			return;
		}
		GS_POINT_BUY_REQ obj2 = (GS_POINT_BUY_REQ)obj;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_POINT_BUY_REQ, obj2);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickList(IUIObject obj)
	{
		if (this.m_eType == ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			UIListItemContainer selectedItem = this.m_kList.SelectedItem;
			if (null == selectedItem)
			{
				return;
			}
			ITEM iTEM = (ITEM)selectedItem.Data;
			if (iTEM == null)
			{
				return;
			}
			this.m_kSelectItem.SetItemTexture(iTEM);
			this.m_kSelectItem.Data = iTEM;
			this.m_nSelectItemUnique = iTEM.m_nItemUnique;
			this.m_nSelectItemNum = 1;
			this.m_kSellNum.MaxValue = (long)iTEM.m_nItemNum;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iTEM.m_nItemUnique);
			this.GetPoint(iTEM);
			this.SetVisible(true);
		}
		else if (this.m_eType == ExchangePointDlg.TYPE.TYPE_EQUIPITEM)
		{
			for (int i = 0; i < this.m_kList.Count; i++)
			{
				UIListItemContainer item = this.m_kList.GetItem(i);
				if (!(null == item))
				{
					if (item.IsSelected())
					{
						item.GetElement(5).Visible = true;
					}
					else
					{
						item.GetElement(5).Visible = false;
					}
				}
			}
			this.InitRemoveItemID();
			int num = this.m_kList.SelectedItems.Count;
			if (num == 0)
			{
				this.InitSelectItemInfo();
				return;
			}
			if (num > 10)
			{
				return;
			}
			num = 0;
			long num2 = 0L;
			bool flag = false;
			foreach (IUIListObject current in this.m_kList.SelectedItems.Values)
			{
				IUIListObject iUIListObject = current;
				if (iUIListObject != null)
				{
					if (num > 10)
					{
						break;
					}
					ITEM iTEM2 = (ITEM)iUIListObject.Data;
					if (iTEM2 != null)
					{
						ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM2.m_nItemUnique);
						if (itemInfo != null)
						{
							int itemMakeRank = iTEM2.m_nOption[2];
							ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, itemMakeRank);
							if (itemSellData != null)
							{
								long num3 = (long)(itemSellData.nItemSellMoney / NrTSingleton<PointManager>.Instance.GetItemBuyRate());
								if (0L < num3)
								{
									if (!flag)
									{
										this.m_kSelectItem.SetItemTexture(iTEM2);
										this.m_kSelectItem.Data = iTEM2;
										this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iTEM2.m_nItemUnique);
										flag = true;
									}
									this.m_nRemoveItemID[num++] = iTEM2.m_nItemID;
									num2 += num3;
								}
							}
						}
					}
				}
			}
			string empty = string.Empty;
			if (1 < this.m_kList.SelectedItems.Count)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("295"),
					"count",
					this.m_kList.SelectedItems.Count - 1
				});
				this.m_kEquipCount.Text = empty;
			}
			else
			{
				this.m_kEquipCount.Text = string.Empty;
			}
			this.m_kEquipPointName.Visible = true;
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2256"),
				"count",
				num2
			});
			this.m_kTotalEquipPoint.Text = empty;
		}
		this.SetPoint();
	}

	private void ShowTicketList()
	{
		this.m_kList.SetMultiSelectMode(false);
		this.m_kList.Clear();
		List<ITEM> functionItemsByInvenType = NkUserInventory.instance.GetFunctionItemsByInvenType(6, eITEM_SUPPLY_FUNCTION.SUPPLY_GETSOLDIER);
		foreach (ITEM current in functionItemsByInvenType)
		{
			if (!NrTSingleton<ItemManager>.Instance.IsItemATB(current.m_nItemUnique, 2048L))
			{
				PointTable pointTable = NrTSingleton<PointManager>.Instance.GetPointTable(current.m_nItemUnique);
				if (pointTable != null)
				{
					if (0 < pointTable.m_nGetPoint)
					{
						NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true, string.Empty);
						string name = NrTSingleton<ItemManager>.Instance.GetName(current);
						newListItem.SetListItemData(1, current, null, null, null);
						newListItem.SetListItemData(2, name, null, null, null);
						newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2254"), null, null, null);
						newListItem.SetListItemData(4, NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + pointTable.m_nGetPoint.ToString(), null, null, null);
						newListItem.SetListItemData(5, false);
						newListItem.SetListItemData(6, false);
						newListItem.Data = current;
						this.m_kList.Add(newListItem);
					}
				}
			}
		}
		this.m_kList.RepositionItems();
		functionItemsByInvenType.Clear();
	}

	private void ClickTicket(IUIObject obj)
	{
		if (!this.m_kTicket.Value)
		{
			return;
		}
		this.m_eType = ExchangePointDlg.TYPE.TYPE_TICKET;
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		this.InitSelectItemInfo();
		this.ShowTicketList();
		this.SetVisible(false);
	}

	private int CompareItemLevel(ITEM a, ITEM b)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a.m_nItemUnique);
		if (itemInfo == null)
		{
			return 0;
		}
		ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(b.m_nItemUnique);
		if (itemInfo2 == null)
		{
			return 0;
		}
		int itemMakeRank = a.m_nOption[2];
		int itemMakeRank2 = b.m_nOption[2];
		ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, itemMakeRank);
		if (itemSellData == null)
		{
			return 0;
		}
		ITEM_SELL itemSellData2 = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo2.m_nQualityLevel, itemMakeRank2);
		if (itemSellData2 == null)
		{
			return 0;
		}
		long num = (long)(itemSellData.nItemSellMoney / NrTSingleton<PointManager>.Instance.GetItemBuyRate());
		if (0L >= num)
		{
			return 0;
		}
		long num2 = (long)(itemSellData2.nItemSellMoney / NrTSingleton<PointManager>.Instance.GetItemBuyRate());
		if (0L >= num2)
		{
			return 0;
		}
		return num.CompareTo(num2);
	}

	private void ShowEquipList()
	{
		this.m_kList.SetMultiSelectMode(true);
		this.m_kList.Clear();
		List<ITEM> list = new List<ITEM>();
		for (int i = 1; i <= 4; i++)
		{
			for (int j = 0; j < ItemDefine.INVENTORY_ITEMSLOT_MAX; j++)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(i, j);
				if (item != null)
				{
					ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
					if (itemInfo != null)
					{
						int itemMakeRank = item.m_nOption[2];
						ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, itemMakeRank);
						if (itemSellData != null)
						{
							long num = (long)(itemSellData.nItemSellMoney / NrTSingleton<PointManager>.Instance.GetItemBuyRate());
							if (0L < num)
							{
								list.Add(item);
							}
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			list.Sort(new Comparison<ITEM>(this.CompareItemLevel));
			for (int k = 0; k < list.Count; k++)
			{
				ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(list[k].m_nItemUnique);
				if (itemInfo2 != null)
				{
					int itemMakeRank2 = list[k].m_nOption[2];
					ITEM_SELL itemSellData2 = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo2.m_nQualityLevel, itemMakeRank2);
					if (itemSellData2 != null)
					{
						long num2 = (long)(itemSellData2.nItemSellMoney / NrTSingleton<PointManager>.Instance.GetItemBuyRate());
						if (0L < num2)
						{
							NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true, string.Empty);
							newListItem.SetListItemData(1, list[k], null, null, null);
							string name = NrTSingleton<ItemManager>.Instance.GetName(list[k]);
							newListItem.SetListItemData(2, name, null, null, null);
							newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2255"), null, null, null);
							newListItem.SetListItemData(4, NrTSingleton<CTextParser>.Instance.GetTextColor("1301") + num2.ToString(), null, null, null);
							newListItem.SetListItemData(5, false);
							newListItem.SetListItemData(6, list[k].IsLock());
							newListItem.Data = list[k];
							this.m_kList.Add(newListItem);
						}
					}
				}
			}
		}
		this.m_kList.RepositionItems();
		list.Clear();
	}

	private void ClickEquipItem(IUIObject obj)
	{
		if (!this.m_kEquipItem.Value)
		{
			return;
		}
		this.m_eType = ExchangePointDlg.TYPE.TYPE_EQUIPITEM;
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		this.ShowEquipList();
		this.InitSelectItemInfo();
		this.m_kEquipPointName.Visible = false;
	}

	private void SetVisible(bool flag)
	{
		this.m_kMinus.Visible = flag;
		this.m_kPlus.Visible = flag;
		this.m_kAll.Visible = flag;
		this.m_kSellNum.Visible = flag;
		this.m_kHeroPointName.Visible = flag;
		this.m_kTextNumBack.Visible = flag;
	}

	private void InitSelectItemInfo()
	{
		this.m_kSelectItem.ClearData();
		this.m_kSelectItem.Data = null;
		this.m_kSelectItemName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2263");
		this.m_nSelectItemNum = 0;
		this.m_kSellNum.Text = string.Empty;
		this.m_kPoint.Text = string.Empty;
		this.m_kEquipCount.Text = string.Empty;
		this.m_kTotalEquipPoint.Text = string.Empty;
	}

	public void UpdateUI()
	{
		this.InitSelectItemInfo();
		this.InitRemoveItemID();
		this.SetPoint();
		if (this.m_eType == ExchangePointDlg.TYPE.TYPE_TICKET)
		{
			this.ShowTicketList();
			this.SetVisible(false);
		}
		else
		{
			this.ShowEquipList();
			this.m_kEquipPointName.Visible = false;
		}
	}

	private void ClickSelectItem(IUIObject obj)
	{
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (this.m_kSelectItem.Data is ITEM)
		{
			ITEM pkItem = (ITEM)this.m_kSelectItem.Data;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, pkItem, null, false);
		}
		else
		{
			Debug.LogError("Can't Find Obj type");
			itemTooltipDlg.Close();
		}
	}
}
