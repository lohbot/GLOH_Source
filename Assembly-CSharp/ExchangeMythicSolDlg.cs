using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ExchangeMythicSolDlg : Form
{
	private enum TYPE
	{
		TYPE_EXCHANGE_TICKET = 1,
		TYPE_EXCHANGE_ITEM
	}

	private ExchangeMythicSolDlg.TYPE m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET;

	private Toggle m_kTicket;

	private Toggle m_kEquipItem;

	private ItemTexture m_kSelectItem;

	private NewListBox m_kList;

	private Button m_kSell;

	private Button m_kMinus;

	private Button m_kPlus;

	private Button m_kAll;

	private Label m_kSelectItemName;

	private Label m_kName;

	private Label m_kLimitTicketNum;

	private Label m_kName2;

	private Label m_kLimitTicketNum2;

	private TextField m_kSellNum;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private int m_nTicketNum;

	private long m_nSelectItemID;

	private Button m_kInputBox;

	private int m_nUseTicketNum;

	private int m_nResultItemUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Point/dlg_mythicexchange", G_ID.EXCHANGE_MYTHICSOL_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_kList = (base.GetControl("newlistbox_mythicexchange") as NewListBox);
		this.m_kList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
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
		this.m_kName = (base.GetControl("Label_text1") as Label);
		this.m_kLimitTicketNum = (base.GetControl("Label_text2") as Label);
		this.m_kName2 = (base.GetControl("Label_text03") as Label);
		this.m_kLimitTicketNum2 = (base.GetControl("Label_text04") as Label);
		this.m_kSelectItemName = (base.GetControl("Label_text") as Label);
		this.m_kSellNum = (base.GetControl("TextField_Count") as TextField);
		this.m_kSellNum.NumberMode = true;
		this.m_kInputBox = (base.GetControl("Button_NumPad") as Button);
		this.m_kInputBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputBox));
		this.InitSelectItem();
		this.SetPoint();
		this.ShowTicketList();
		base.SetShowLayer(2, false);
		base.SetScreenCenter();
		this.m_nTicketNum = 0;
		this.SetUsetTicketNum();
	}

	private void ClickSelectItem(IUIObject obj)
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
			if (iTEM == null)
			{
				return;
			}
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
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
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_kSelectItem.Data;
		if (mythicSolTable == null)
		{
			return;
		}
		long num = a_cForm.GetNum();
		this.m_nSelectItemNum = (int)num;
		this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
		this.m_kSellNum.Text = num.ToString();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
		this.SetUsetTicketNum();
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void SetPoint()
	{
	}

	private void ClickMinus(IUIObject obj)
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			MythicSolTable mythicSolTable = (MythicSolTable)this.m_kSelectItem.Data;
			if (mythicSolTable == null)
			{
				return;
			}
			this.m_nSelectItemNum--;
			if (1 >= this.m_nSelectItemNum)
			{
				this.m_nSelectItemNum = 1;
			}
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.SetUsetTicketNum();
		}
	}

	private void ClickPlus(IUIObject obj)
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			MythicSolTable mythicSolTable = (MythicSolTable)this.m_kSelectItem.Data;
			if (mythicSolTable == null)
			{
				return;
			}
			this.m_nSelectItemNum++;
			int num = this.m_nTicketNum / mythicSolTable.m_nNeedNum;
			if (this.m_nSelectItemNum > num)
			{
				this.m_nSelectItemNum = num;
			}
			this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.SetUsetTicketNum();
		}
	}

	private void ClickAll(IUIObject obj)
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			MythicSolTable mythicSolTable = (MythicSolTable)this.m_kSelectItem.Data;
			if (mythicSolTable == null)
			{
				return;
			}
			this.m_nSelectItemNum = this.m_nTicketNum / mythicSolTable.m_nNeedNum;
			this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.SetUsetTicketNum();
		}
	}

	private void SetUsetTicketNum()
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			MythicSolTable mythicSolTable = (MythicSolTable)this.m_kSelectItem.Data;
			if (mythicSolTable == null)
			{
				return;
			}
			this.m_kName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(mythicSolTable.m_nNeedItemUnique.ToString());
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2272"),
				"count1",
				this.m_nUseTicketNum,
				"count2",
				this.m_nTicketNum
			});
			this.m_kLimitTicketNum.Text = empty;
		}
		else
		{
			ITEM iTEM = (ITEM)this.m_kSelectItem.Data;
			if (iTEM == null)
			{
				return;
			}
			MythicSolTable mythicSolTable2 = NrTSingleton<PointManager>.Instance.GetMythicSolTable(iTEM.m_nItemUnique);
			if (mythicSolTable2 == null)
			{
				return;
			}
			this.m_kName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(mythicSolTable2.m_nNeedItemUnique.ToString());
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("916"),
				"Count",
				mythicSolTable2.m_nExchangeNum
			});
			this.m_kLimitTicketNum2.Text = empty2;
		}
	}

	private void ClickSell(IUIObject obj)
	{
		if (this.m_kSelectItem != null)
		{
			ITEM iTEM = this.m_kSelectItem.Data as ITEM;
			if (iTEM != null && iTEM.IsLock())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
		}
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			GS_EXCHANGE_MYTHICSOL_REQ gS_EXCHANGE_MYTHICSOL_REQ = new GS_EXCHANGE_MYTHICSOL_REQ();
			gS_EXCHANGE_MYTHICSOL_REQ.nType = (int)this.m_eType;
			gS_EXCHANGE_MYTHICSOL_REQ.nItemUnique = this.m_nSelectItemUnique;
			gS_EXCHANGE_MYTHICSOL_REQ.nItemNum = this.m_nSelectItemNum;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2257");
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("204");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
			{
				textFromMessageBox,
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nSelectItemUnique),
				"count",
				this.m_nSelectItemNum
			});
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_MYTHICSOL_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
			}
		}
		else if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			MythicSolTable mythicSolTable = NrTSingleton<PointManager>.Instance.GetMythicSolTable(this.m_nSelectItemUnique);
			if (mythicSolTable == null)
			{
				return;
			}
			GS_EXCHANGE_MYTHICSOL_REQ gS_EXCHANGE_MYTHICSOL_REQ2 = new GS_EXCHANGE_MYTHICSOL_REQ();
			gS_EXCHANGE_MYTHICSOL_REQ2.nType = (int)this.m_eType;
			gS_EXCHANGE_MYTHICSOL_REQ2.nItemUnique = this.m_nSelectItemUnique;
			gS_EXCHANGE_MYTHICSOL_REQ2.nItemNum = 1;
			gS_EXCHANGE_MYTHICSOL_REQ2.nItemID = this.m_nSelectItemID;
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2257");
			string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("223");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox2, new object[]
			{
				textFromMessageBox2,
				"targetname1",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nSelectItemUnique),
				"targetname2",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable.m_nNeedItemUnique),
				"count",
				mythicSolTable.m_nExchangeNum
			});
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI2 != null)
			{
				msgBoxUI2.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_MYTHICSOL_REQ2, null, null, textFromInterface2, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
			}
		}
	}

	private void BuyItem(object obj)
	{
		GS_EXCHANGE_MYTHICSOL_REQ gS_EXCHANGE_MYTHICSOL_REQ = (GS_EXCHANGE_MYTHICSOL_REQ)obj;
		if (gS_EXCHANGE_MYTHICSOL_REQ == null)
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_MYTHICSOL_REQ, gS_EXCHANGE_MYTHICSOL_REQ);
		this.m_nResultItemUnique = gS_EXCHANGE_MYTHICSOL_REQ.nItemUnique;
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickList(IUIObject obj)
	{
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			UIListItemContainer selectedItem = this.m_kList.SelectedItem;
			if (null == selectedItem)
			{
				return;
			}
			MythicSolTable mythicSolTable = (MythicSolTable)selectedItem.Data;
			if (mythicSolTable == null)
			{
				return;
			}
			this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(mythicSolTable.m_nNeedItemUnique);
			this.m_kSelectItem.SetItemTexture(mythicSolTable.m_nItemUnique);
			this.m_kSelectItem.Data = mythicSolTable;
			this.m_nSelectItemUnique = mythicSolTable.m_nItemUnique;
			this.m_nSelectItemNum = 1;
			this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
			this.m_kSellNum.MaxValue = (long)(this.m_nTicketNum / mythicSolTable.m_nNeedNum);
			if (0L >= this.m_kSellNum.MaxValue)
			{
				this.m_kSell.controlIsEnabled = false;
				this.m_nSelectItemNum = 0;
			}
			else
			{
				this.m_kSell.controlIsEnabled = true;
			}
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable.m_nItemUnique);
		}
		else if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			UIListItemContainer selectedItem2 = this.m_kList.SelectedItem;
			if (null == selectedItem2)
			{
				return;
			}
			ITEM iTEM = (ITEM)selectedItem2.Data;
			if (iTEM == null)
			{
				return;
			}
			MythicSolTable mythicSolTable2 = NrTSingleton<PointManager>.Instance.GetMythicSolTable(iTEM.m_nItemUnique);
			if (mythicSolTable2 == null)
			{
				return;
			}
			this.m_kSell.controlIsEnabled = true;
			this.m_kSelectItem.SetItemTexture(mythicSolTable2.m_nItemUnique);
			this.m_kSelectItem.Data = iTEM;
			this.m_nSelectItemUnique = mythicSolTable2.m_nItemUnique;
			this.m_nSelectItemID = iTEM.m_nItemID;
			this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable2.m_nItemUnique);
		}
		this.SetPoint();
		this.SetUsetTicketNum();
	}

	private void ShowTicketList()
	{
		this.m_nUseTicketNum = 0;
		this.m_kList.Clear();
		Dictionary<int, MythicSolTable> totalMythicSolTable = NrTSingleton<PointManager>.Instance.GetTotalMythicSolTable();
		foreach (MythicSolTable current in totalMythicSolTable.Values)
		{
			if (current.m_nType != 2)
			{
				NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true);
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nItemUnique);
				newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_nItemUnique), null, null, null);
				newListItem.SetListItemData(2, itemNameByItemUnique, null, null, null);
				newListItem.SetListItemData(3, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nNeedItemUnique), null, null, null);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("632"),
					"count",
					NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + current.m_nNeedNum
				});
				newListItem.SetListItemData(4, empty, null, null, null);
				newListItem.SetListItemData(5, false);
				newListItem.Data = current;
				this.m_kList.Add(newListItem);
			}
		}
		this.m_kList.RepositionItems();
		this.m_kList.SetSelectedItem(0);
	}

	private void ClickTicket(IUIObject obj)
	{
		if (!this.m_kTicket.Value)
		{
			return;
		}
		this.InitSelectItem();
		this.m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET;
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		this.ShowTicketList();
	}

	private int CompareItemLevel(ITEM a, ITEM b)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a.m_nItemUnique);
		ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(b.m_nItemUnique);
		if (itemInfo.m_nQualityLevel != itemInfo2.m_nQualityLevel)
		{
			return -itemInfo.m_nQualityLevel.CompareTo(itemInfo2.m_nQualityLevel);
		}
		if (a.GetRank() != b.GetRank())
		{
			return a.GetRank().CompareTo(b.GetRank());
		}
		int useMinLevel = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(a);
		int useMinLevel2 = NrTSingleton<ItemManager>.Instance.GetUseMinLevel(b);
		if (useMinLevel == useMinLevel2)
		{
			return -a.m_nItemUnique.CompareTo(b.m_nItemUnique);
		}
		return -useMinLevel.CompareTo(useMinLevel2);
	}

	private void ShowEquipList()
	{
		this.m_nUseTicketNum = 0;
		this.m_kList.Clear();
		Dictionary<int, MythicSolTable> totalMythicSolTable = NrTSingleton<PointManager>.Instance.GetTotalMythicSolTable();
		List<ITEM> itemFromATB = NkUserInventory.GetInstance().GetItemFromATB(131072L);
		List<ITEM> typeItemsByInvenType = NkUserInventory.GetInstance().GetTypeItemsByInvenType(0, eITEM_TYPE.ITEMTYPE_RING);
		foreach (ITEM current in typeItemsByInvenType)
		{
			itemFromATB.Add(current);
		}
		foreach (ITEM current2 in itemFromATB)
		{
			foreach (MythicSolTable current3 in totalMythicSolTable.Values)
			{
				if (current3.m_nType != 1)
				{
					if (current3.m_nItemUnique == current2.m_nItemUnique)
					{
						NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true);
						string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current3.m_nItemUnique);
						newListItem.SetListItemData(1, current2, null, null, null);
						newListItem.SetListItemData(2, itemNameByItemUnique, null, null, null);
						newListItem.SetListItemData(3, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current3.m_nNeedItemUnique), null, null, null);
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("632"),
							"count",
							NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + current3.m_nExchangeNum.ToString()
						});
						newListItem.SetListItemData(4, empty, null, null, null);
						newListItem.SetListItemData(5, current2.IsLock());
						newListItem.Data = current2;
						this.m_kList.Add(newListItem);
					}
				}
			}
		}
		this.m_kList.RepositionItems();
		this.m_kList.SetSelectedItem(0);
	}

	private void ClickEquipItem(IUIObject obj)
	{
		if (!this.m_kEquipItem.Value)
		{
			return;
		}
		this.InitSelectItem();
		this.m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_ITEM;
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		this.ShowEquipList();
	}

	public void InitSelectItem()
	{
		this.m_kSelectItem.ClearData();
		this.m_nSelectItemNum = 0;
		this.m_kSellNum.Text = string.Empty;
		this.m_nUseTicketNum = 0;
		this.m_nSelectItemID = -1L;
		this.m_nResultItemUnique = 0;
		this.m_kSelectItemName.Text = string.Empty;
		this.m_kName.Text = string.Empty;
		this.m_kName2.Text = string.Empty;
		this.m_kLimitTicketNum.Text = string.Empty;
		this.m_kLimitTicketNum2.Text = string.Empty;
		this.m_kSell.controlIsEnabled = false;
	}

	public void UpdateUI()
	{
		this.InitSelectItem();
		this.SetPoint();
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			this.ShowTicketList();
		}
		else
		{
			this.ShowEquipList();
		}
	}

	public void ResultMessage()
	{
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(PointManager.NECTAR_TICKET);
		MythicSolTable mythicSolTable = NrTSingleton<PointManager>.Instance.GetMythicSolTable(this.m_nResultItemUnique);
		if (this.m_eType == ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable.m_nItemUnique),
				"count",
				this.m_nSelectItemNum
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			this.m_nUseTicketNum = 0;
			this.SetUsetTicketNum();
		}
		else
		{
			if (NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(this.m_nResultItemUnique) == null)
			{
				return;
			}
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("647"),
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable.m_nNeedItemUnique),
				"count",
				mythicSolTable.m_nExchangeNum
			});
			Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		}
		this.m_nResultItemUnique = 0;
	}
}
