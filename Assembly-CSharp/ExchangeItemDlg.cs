using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ExchangeItemDlg : Form
{
	private enum TYPE
	{
		TYPE_EXCHANGE_TICKET,
		TYPE_EXCHANGE_ITEM
	}

	private ExchangeItemDlg.TYPE m_eType;

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

	private Label m_kSelectItemName;

	private Label m_kName;

	private Label m_kLimitTicketNum;

	private Label m_kName2;

	private Label m_kLimitTicketNum2;

	private TextField m_kSellNum;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private int m_nTicketNum;

	private Button m_kInputBox;

	private Button m_kHelp;

	private bool bShowHelp;

	private int m_nHeroLimitTicketNum;

	private int m_nEquipLimitTicketNum;

	private int m_nUseTicketNum;

	private int m_nResultItemUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Point/dlg_itemexchangemain", G_ID.EXCHANGE_ITEM_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_kEquipItem = (base.GetControl("CheckBox_Item") as Toggle);
		this.m_kSell = (base.GetControl("Button_confirm") as Button);
		this.m_kSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSell));
		this.m_kEquipItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEquipItem));
		this.m_kHeroPoint = (base.GetControl("Label_HeroPointCount") as Label);
		this.m_kEquipPoint = (base.GetControl("Label_ItemPointCount") as Label);
		this.m_kTicket = (base.GetControl("CheckBox_Ticket") as Toggle);
		this.m_kTicket.Value = true;
		this.m_kTicket.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickTicket));
		this.m_kList = (base.GetControl("newlistbox_itemexchange") as NewListBox);
		this.m_kList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_kSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_kMinus = (base.GetControl("Button_CountDown") as Button);
		this.m_kMinus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMinus));
		this.m_kPlus = (base.GetControl("Button_CountUp") as Button);
		this.m_kPlus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPlus));
		this.m_kAll = (base.GetControl("Button_MaxCountUp") as Button);
		this.m_kAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAll));
		this.m_kName = (base.GetControl("Label_text1") as Label);
		this.m_kLimitTicketNum = (base.GetControl("Label_text2") as Label);
		this.m_kName2 = (base.GetControl("Label_text3") as Label);
		this.m_kLimitTicketNum2 = (base.GetControl("Label_text4") as Label);
		this.m_kSelectItemName = (base.GetControl("Label_text") as Label);
		this.m_kSellNum = (base.GetControl("TextField_Count") as TextField);
		this.m_kSellNum.NumberMode = true;
		this.m_kInputBox = (base.GetControl("Button_NumPad") as Button);
		this.m_kInputBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputBox));
		this.m_kHelp = (base.GetControl("BT_Help01") as Button);
		this.m_kHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.SetPoint();
		this.ShowTicketList();
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		base.SetScreenCenter();
	}

	private void ClickHelp(IUIObject obj)
	{
		this.bShowHelp = !this.bShowHelp;
		base.SetShowLayer(3, this.bShowHelp);
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
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
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
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			this.m_kName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2260");
			PointLimitTable pointLimitTable = NrTSingleton<PointManager>.Instance.GetPointLimitTable(myCharInfo.GetLevel());
			if (pointLimitTable == null)
			{
				return;
			}
			PointTable pointTable = (PointTable)this.m_kSelectItem.Data;
			if (pointTable == null)
			{
				return;
			}
			if (pointTable.m_nItemUnique == PointManager.HERO_TICKET)
			{
				this.m_nHeroLimitTicketNum = pointLimitTable.m_nHeroTicketNum - (int)myCharInfo.GetCharDetail(6);
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2261"),
					"count",
					this.m_nHeroLimitTicketNum
				});
				this.m_kLimitTicketNum.Text = empty;
			}
			else
			{
				this.m_nEquipLimitTicketNum = pointLimitTable.m_nEquipTicketNum - (int)myCharInfo.GetCharDetail(7);
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2261"),
					"count",
					this.m_nEquipLimitTicketNum
				});
				this.m_kLimitTicketNum.Text = empty;
			}
			this.m_kHelp.Visible = true;
		}
		else
		{
			PointTable pointTable2 = (PointTable)this.m_kSelectItem.Data;
			if (pointTable2 == null)
			{
				return;
			}
			this.m_kName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(pointTable2.m_nNeedItemUnique.ToString());
		}
	}

	private void ClickMinus(IUIObject obj)
	{
		this.m_nSelectItemNum--;
		if (1 >= this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = 1;
		}
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			PointTable pointTable = (PointTable)this.m_kSelectItem.Data;
			if (pointTable == null)
			{
				return;
			}
			this.m_nUseTicketNum = this.m_nSelectItemNum * pointTable.m_nExchangePoint;
			this.SetUsetTicketNum();
		}
	}

	private void ClickPlus(IUIObject obj)
	{
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			PointTable pointTable = (PointTable)this.m_kSelectItem.Data;
			if (pointTable == null)
			{
				return;
			}
			this.m_nSelectItemNum++;
			if (pointTable.m_nItemUnique == PointManager.HERO_TICKET)
			{
				if (this.m_nSelectItemNum > this.m_nHeroLimitTicketNum)
				{
					this.m_nSelectItemNum = this.m_nHeroLimitTicketNum;
				}
			}
			else if (this.m_nSelectItemNum > this.m_nEquipLimitTicketNum)
			{
				this.m_nSelectItemNum = this.m_nEquipLimitTicketNum;
			}
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		}
		else
		{
			PointTable pointTable2 = (PointTable)this.m_kSelectItem.Data;
			if (pointTable2 == null)
			{
				return;
			}
			this.m_nSelectItemNum++;
			int num = NkUserInventory.instance.Get_First_ItemCnt(pointTable2.m_nNeedItemUnique);
			if (this.m_nSelectItemNum * pointTable2.m_nExchangePoint > num)
			{
				this.m_nSelectItemNum--;
			}
			this.m_nUseTicketNum = this.m_nSelectItemNum * pointTable2.m_nExchangePoint;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.SetUsetTicketNum();
		}
	}

	private void ClickAll(IUIObject obj)
	{
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			PointTable pointTable = (PointTable)this.m_kSelectItem.Data;
			if (pointTable == null)
			{
				return;
			}
			if (pointTable.m_nItemUnique == PointManager.HERO_TICKET)
			{
				this.m_nSelectItemNum = this.m_nHeroLimitTicketNum;
			}
			else
			{
				this.m_nSelectItemNum = this.m_nEquipLimitTicketNum;
			}
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		}
		else
		{
			PointTable pointTable2 = (PointTable)this.m_kSelectItem.Data;
			if (pointTable2 == null)
			{
				return;
			}
			int num = NkUserInventory.instance.Get_First_ItemCnt(pointTable2.m_nNeedItemUnique);
			this.m_nSelectItemNum = num / pointTable2.m_nExchangePoint;
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.m_nUseTicketNum = this.m_nSelectItemNum * pointTable2.m_nExchangePoint;
			this.SetUsetTicketNum();
		}
	}

	private void SetUsetTicketNum()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2272"),
			"count1",
			this.m_nUseTicketNum,
			"count2",
			this.m_nTicketNum
		});
		this.m_kLimitTicketNum2.Text = empty;
	}

	private void ClickSell(IUIObject obj)
	{
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			GS_POINT_BUY_REQ gS_POINT_BUY_REQ = new GS_POINT_BUY_REQ();
			gS_POINT_BUY_REQ.nAddPointType = 1;
			if (PointManager.HERO_TICKET == this.m_nSelectItemUnique)
			{
				gS_POINT_BUY_REQ.nType = 0;
			}
			else
			{
				gS_POINT_BUY_REQ.nType = 1;
			}
			gS_POINT_BUY_REQ.nItemUnique = this.m_nSelectItemUnique;
			gS_POINT_BUY_REQ.nItemNum = (long)this.m_nSelectItemNum;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_POINT_BUY_REQ, gS_POINT_BUY_REQ);
		}
		else if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			GS_EXCHANGE_ITEM_REQ gS_EXCHANGE_ITEM_REQ = new GS_EXCHANGE_ITEM_REQ();
			gS_EXCHANGE_ITEM_REQ.nItemUnique = this.m_nSelectItemUnique;
			gS_EXCHANGE_ITEM_REQ.nItemNum = (long)this.m_nSelectItemNum;
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
				msgBoxUI.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_ITEM_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
			}
		}
	}

	private void BuyItem(object obj)
	{
		GS_EXCHANGE_ITEM_REQ gS_EXCHANGE_ITEM_REQ = (GS_EXCHANGE_ITEM_REQ)obj;
		if (gS_EXCHANGE_ITEM_REQ == null)
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_ITEM_REQ, gS_EXCHANGE_ITEM_REQ);
		this.m_nResultItemUnique = gS_EXCHANGE_ITEM_REQ.nItemUnique;
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickList(IUIObject obj)
	{
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			this.m_kHelp.Visible = false;
			UIListItemContainer selectedItem = this.m_kList.SelectedItem;
			if (null == selectedItem)
			{
				return;
			}
			PointTable pointTable = (PointTable)selectedItem.Data;
			if (pointTable == null)
			{
				return;
			}
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			this.m_kSelectItem.SetItemTexture(pointTable.m_nItemUnique);
			this.m_kSelectItem.Data = pointTable;
			this.m_nSelectItemUnique = pointTable.m_nItemUnique;
			this.m_nSelectItemNum = 1;
			this.SetPoint();
			if (PointManager.HERO_TICKET == pointTable.m_nItemUnique)
			{
				if (0 < pointTable.m_nBuyPoint)
				{
					this.m_nHeroLimitTicketNum = (int)Math.Min((long)this.m_nHeroLimitTicketNum, myCharInfo.GetHeroPoint() / (long)pointTable.m_nBuyPoint);
					this.m_kSellNum.MaxValue = (long)this.m_nHeroLimitTicketNum;
				}
			}
			else if (0 < pointTable.m_nBuyPoint)
			{
				this.m_nEquipLimitTicketNum = (int)Math.Min((long)this.m_nEquipLimitTicketNum, myCharInfo.GetEquipPoint() / (long)pointTable.m_nBuyPoint);
				this.m_kSellNum.MaxValue = (long)this.m_nEquipLimitTicketNum;
			}
			if (0L >= this.m_kSellNum.MaxValue)
			{
				this.m_kSell.controlIsEnabled = false;
			}
			else
			{
				this.m_kSell.controlIsEnabled = true;
			}
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(pointTable.m_nItemUnique);
		}
		else if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_ITEM)
		{
			UIListItemContainer selectedItem2 = this.m_kList.SelectedItem;
			if (null == selectedItem2)
			{
				return;
			}
			PointTable pointTable2 = (PointTable)selectedItem2.Data;
			if (pointTable2 == null)
			{
				return;
			}
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			this.m_kSelectItem.SetItemTexture(pointTable2.m_nItemUnique);
			this.m_kSelectItem.Data = pointTable2;
			this.m_nSelectItemUnique = pointTable2.m_nItemUnique;
			this.SetPoint();
			this.m_nTicketNum = NkUserInventory.instance.Get_First_ItemCnt(pointTable2.m_nNeedItemUnique);
			if (pointTable2.m_nExchangePoint <= this.m_nTicketNum)
			{
				this.m_nSelectItemNum = 1;
			}
			else
			{
				this.m_nSelectItemNum = 0;
			}
			this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(pointTable2.m_nItemUnique);
			this.m_kSellNum.MaxValue = (long)(this.m_nTicketNum / pointTable2.m_nExchangePoint);
			this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
			if (0L >= this.m_kSellNum.MaxValue)
			{
				this.m_kSell.controlIsEnabled = false;
			}
			else
			{
				this.m_kSell.controlIsEnabled = true;
			}
			this.m_nUseTicketNum = this.m_nSelectItemNum * pointTable2.m_nExchangePoint;
			this.SetUsetTicketNum();
		}
	}

	private void ShowTicketList()
	{
		this.m_nUseTicketNum = 0;
		this.m_kList.Clear();
		Dictionary<int, PointTable> totalPointTable = NrTSingleton<PointManager>.Instance.GetTotalPointTable();
		foreach (PointTable current in totalPointTable.Values)
		{
			if (0 < current.m_nBuyPoint)
			{
				NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true);
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nItemUnique);
				newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_nItemUnique), null, null, null);
				newListItem.SetListItemData(2, itemNameByItemUnique, null, null, null);
				if (current.m_nItemUnique == PointManager.HERO_TICKET)
				{
					newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2254"), null, null, null);
				}
				else
				{
					newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2255"), null, null, null);
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041"),
					"count",
					NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + current.m_nBuyPoint
				});
				newListItem.SetListItemData(4, empty, null, null, null);
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
		this.InitPoint();
		this.m_eType = ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET;
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		this.ShowTicketList();
	}

	public void InitPoint()
	{
		this.m_kHelp.Visible = false;
		this.m_kName2.Text = string.Empty;
		this.m_kLimitTicketNum.Text = string.Empty;
		this.m_kLimitTicketNum2.Text = string.Empty;
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
		Dictionary<int, PointTable> totalPointTable = NrTSingleton<PointManager>.Instance.GetTotalPointTable();
		foreach (PointTable current in totalPointTable.Values)
		{
			if (0 < current.m_nExchangePoint)
			{
				NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true);
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nItemUnique);
				newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_nItemUnique), null, null, null);
				newListItem.SetListItemData(2, itemNameByItemUnique, null, null, null);
				newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(current.m_nNeedItemUnique.ToString()), null, null, null);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2261"),
					"count",
					NrTSingleton<CTextParser>.Instance.GetTextColor("1301") + current.m_nExchangePoint
				});
				newListItem.SetListItemData(4, empty, null, null, null);
				newListItem.Data = current;
				this.m_kList.Add(newListItem);
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
		this.InitPoint();
		this.m_eType = ExchangeItemDlg.TYPE.TYPE_EXCHANGE_ITEM;
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		this.ShowEquipList();
	}

	public void InitSelectItem()
	{
		this.m_kSelectItem.ClearData();
		this.m_nSelectItemNum = 0;
		this.m_kSellNum.Text = string.Empty;
		this.m_nUseTicketNum = 0;
	}

	public void UpdateUI()
	{
		this.InitSelectItem();
		this.SetPoint();
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
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
		if (this.m_eType == ExchangeItemDlg.TYPE.TYPE_EXCHANGE_TICKET)
		{
			string empty = string.Empty;
			if (PointManager.HERO_TICKET == this.m_nSelectItemUnique)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(PointManager.HERO_TICKET),
					"count",
					this.m_nSelectItemNum
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(PointManager.EQUIP_TICKET),
					"count",
					this.m_nSelectItemNum
				});
			}
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
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
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nResultItemUnique),
				"count",
				this.m_nSelectItemNum
			});
			Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		}
		this.m_nResultItemUnique = 0;
	}
}
