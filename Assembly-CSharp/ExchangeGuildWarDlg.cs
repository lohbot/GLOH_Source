using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExchangeGuildWarDlg : Form
{
	private ItemTexture m_kSelectItem;

	private NewListBox m_kList;

	private Button m_kSell;

	private Button m_kMinus;

	private Button m_kPlus;

	private Button m_kAll;

	private Label m_kSelectItemName;

	private Label m_kName;

	private Label m_kLimitTicketNum;

	private Label m_kText;

	private Label m_kExchangeLimit;

	private TextField m_kSellNum;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private int m_nTicketNum;

	private Button m_kInputBox;

	private Button m_kHelp;

	private DrawTexture m_kHelpTail;

	private DrawTexture m_kHelpBG;

	private Label m_kHelpText;

	private Dictionary<int, GUILDWAR_EXCHANGE> m_GuildWar_Exchange_Limit = new Dictionary<int, GUILDWAR_EXCHANGE>();

	private int m_nUseTicketNum;

	private int m_nResultItemUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/dlg_GuildWar_Exchange", G_ID.EXCHANGE_GUILDWAR_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_kList = (base.GetControl("NLB_GuildWarSelllist") as NewListBox);
		this.m_kList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_kSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_kSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectItem));
		this.m_kSell = (base.GetControl("BT_Confirm") as Button);
		this.m_kSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSell));
		this.m_kName = (base.GetControl("LB_Itemname2") as Label);
		this.m_kLimitTicketNum = (base.GetControl("LB_itemnum") as Label);
		this.m_kText = (base.GetControl("LB_TextBG") as Label);
		this.m_kExchangeLimit = (base.GetControl("LB_LimitExchange") as Label);
		this.m_kExchangeLimit.Visible = false;
		this.m_kInputBox = (base.GetControl("BT_NumPad") as Button);
		this.m_kInputBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputBox));
		this.m_kMinus = (base.GetControl("BT_CountDown") as Button);
		this.m_kMinus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMinus));
		this.m_kPlus = (base.GetControl("BT_CountUp") as Button);
		this.m_kPlus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPlus));
		this.m_kAll = (base.GetControl("BT_MaxCountUp") as Button);
		this.m_kAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAll));
		this.m_kSellNum = (base.GetControl("TextField_Count") as TextField);
		this.m_kSellNum.NumberMode = true;
		this.m_kSelectItemName = (base.GetControl("LB_ItemName1") as Label);
		this.m_kHelp = (base.GetControl("BT_Help01") as Button);
		this.m_kHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_kHelp.Visible = false;
		this.m_kHelpTail = (base.GetControl("DT_HelpTail01") as DrawTexture);
		this.m_kHelpTail.SetLocationZ(this.m_kHelpTail.GetLocation().z - 1f);
		this.m_kHelpTail.Visible = false;
		this.m_kHelpBG = (base.GetControl("DT_HelpBG01") as DrawTexture);
		this.m_kHelpBG.SetLocationZ(this.m_kHelpBG.GetLocation().z - 1f);
		this.m_kHelpBG.Visible = false;
		this.m_kHelpText = (base.GetControl("LB_HelpText01") as Label);
		this.m_kHelpText.SetLocationZ(this.m_kHelpText.GetLocation().z - 1f);
		this.m_kHelpText.Visible = false;
		this.m_GuildWar_Exchange_Limit.Clear();
		GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ gS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ = new GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ();
		if (gS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ != null)
		{
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ, gS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_REQ);
		}
		this.InitSelectItem();
		this.SetPoint();
		this.ShowTicketList();
		this.SetUsetTicketNum();
		base.SetScreenCenter();
	}

	public void ShowUI()
	{
		this.InitSelectItem();
		this.SetPoint();
		this.ShowTicketList();
		base.SetScreenCenter();
		this.m_nTicketNum = 0;
		this.SetUsetTicketNum();
		this.Show();
	}

	private void ClickSelectItem(IUIObject obj)
	{
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (this.m_kSelectItem.Data is ITEM)
		{
			ITEM pkItem = (ITEM)this.m_kSelectItem.Data;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, pkItem, null, false);
		}
		else if (this.m_kSelectItem.Data is GuildWarExchangeTable)
		{
			GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
			ITEM iTEM = new ITEM();
			iTEM.Init();
			iTEM.m_nItemUnique = guildWarExchangeTable.m_nItemUnique;
			iTEM.m_nItemNum = 1;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
		else
		{
			Debug.LogError("Can't Find Obj type");
			itemTooltipDlg.Close();
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
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		long num = a_cForm.GetNum();
		this.m_nSelectItemNum = (int)num;
		this.m_nUseTicketNum = this.m_nSelectItemNum * guildWarExchangeTable.m_nNeedNum;
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
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		this.m_nSelectItemNum--;
		if (1 >= this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = 1;
		}
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.m_nUseTicketNum = this.m_nSelectItemNum * guildWarExchangeTable.m_nNeedNum;
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.SetUsetTicketNum();
	}

	private void ClickPlus(IUIObject obj)
	{
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		this.m_nSelectItemNum++;
		int num = this.m_nTicketNum / guildWarExchangeTable.m_nNeedNum;
		if (this.m_nSelectItemNum > num)
		{
			this.m_nSelectItemNum = num;
		}
		this.m_nUseTicketNum = this.m_nSelectItemNum * guildWarExchangeTable.m_nNeedNum;
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.SetUsetTicketNum();
	}

	private void ClickAll(IUIObject obj)
	{
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		this.m_nSelectItemNum = this.m_nTicketNum / guildWarExchangeTable.m_nNeedNum;
		this.m_nUseTicketNum = this.m_nSelectItemNum * guildWarExchangeTable.m_nNeedNum;
		this.m_kSellNum.Text = this.m_nSelectItemNum.ToString();
		this.SetUsetTicketNum();
	}

	private void SetUsetTicketNum()
	{
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		this.m_kName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(guildWarExchangeTable.m_nNeedItemUnique.ToString());
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
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)this.m_kSelectItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		if (this.m_GuildWar_Exchange_Limit.ContainsKey(this.m_nSelectItemUnique) && this.m_GuildWar_Exchange_Limit[this.m_nSelectItemUnique].i32ExchangeLimit == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("842"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			return;
		}
		if (guildWarExchangeTable.m_nExchangeLimit != -1)
		{
			if (this.m_GuildWar_Exchange_Limit.ContainsKey(this.m_nSelectItemUnique))
			{
				if (this.m_nSelectItemNum > this.m_GuildWar_Exchange_Limit[this.m_nSelectItemUnique].i32ExchangeLimit)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("841"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					return;
				}
			}
			else if (this.m_nSelectItemNum > guildWarExchangeTable.m_nExchangeLimit)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("841"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
		}
		GS_EXCHANGE_GUILDWAR_CHECK_REQ gS_EXCHANGE_GUILDWAR_CHECK_REQ = new GS_EXCHANGE_GUILDWAR_CHECK_REQ();
		gS_EXCHANGE_GUILDWAR_CHECK_REQ.nItemUnique = this.m_nSelectItemUnique;
		gS_EXCHANGE_GUILDWAR_CHECK_REQ.nItemNum = this.m_nSelectItemNum;
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
			msgBoxUI.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_GUILDWAR_CHECK_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
		}
	}

	private void BuyItem(object obj)
	{
		GS_EXCHANGE_GUILDWAR_CHECK_REQ gS_EXCHANGE_GUILDWAR_CHECK_REQ = (GS_EXCHANGE_GUILDWAR_CHECK_REQ)obj;
		if (gS_EXCHANGE_GUILDWAR_CHECK_REQ == null)
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_GUILDWAR_CHECK_REQ, gS_EXCHANGE_GUILDWAR_CHECK_REQ);
		this.m_nResultItemUnique = gS_EXCHANGE_GUILDWAR_CHECK_REQ.nItemUnique;
		this.m_kExchangeLimit.Visible = false;
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickList(IUIObject obj)
	{
		this.m_kHelpTail.Visible = false;
		this.m_kHelpBG.Visible = false;
		this.m_kHelpText.Visible = false;
		UIListItemContainer selectedItem = this.m_kList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		GuildWarExchangeTable guildWarExchangeTable = (GuildWarExchangeTable)selectedItem.Data;
		if (guildWarExchangeTable == null)
		{
			return;
		}
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(guildWarExchangeTable.m_nNeedItemUnique);
		this.m_kSelectItem.SetItemTexture(guildWarExchangeTable.m_nItemUnique);
		this.m_kSelectItem.Data = guildWarExchangeTable;
		this.m_nSelectItemUnique = guildWarExchangeTable.m_nItemUnique;
		this.m_nSelectItemNum = 1;
		this.m_nUseTicketNum = this.m_nSelectItemNum * guildWarExchangeTable.m_nNeedNum;
		this.m_kSellNum.MaxValue = (long)(this.m_nTicketNum / guildWarExchangeTable.m_nNeedNum);
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
		this.m_kSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(guildWarExchangeTable.m_nItemUnique);
		if (guildWarExchangeTable.m_nExchangeLimit != -1)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3036");
			if (this.m_GuildWar_Exchange_Limit.ContainsKey(guildWarExchangeTable.m_nItemUnique))
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
				{
					textFromInterface,
					"count",
					this.m_GuildWar_Exchange_Limit[guildWarExchangeTable.m_nItemUnique].i32ExchangeLimit
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
				{
					textFromInterface,
					"count",
					guildWarExchangeTable.m_nExchangeLimit
				});
			}
			this.m_kExchangeLimit.Text = textFromInterface;
			this.m_kExchangeLimit.Visible = true;
			this.m_kHelp.Visible = true;
		}
		else
		{
			this.m_kExchangeLimit.Visible = false;
			this.m_kHelp.Visible = false;
		}
		this.m_kText.Visible = false;
		this.SetPoint();
		this.SetUsetTicketNum();
	}

	private void ShowTicketList()
	{
		this.m_nUseTicketNum = 0;
		this.m_kList.Clear();
		Dictionary<int, GuildWarExchangeTable> totalGuildWarExchangeTable = NrTSingleton<PointManager>.Instance.GetTotalGuildWarExchangeTable();
		foreach (GuildWarExchangeTable current in totalGuildWarExchangeTable.Values)
		{
			NewListItem newListItem = new NewListItem(this.m_kList.ColumnNum, true, string.Empty);
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
			newListItem.Data = current;
			this.m_kList.Add(newListItem);
		}
		this.m_kList.RepositionItems();
		this.m_kList.SetSelectedItem(0);
	}

	private void ClickTicket(IUIObject obj)
	{
		this.InitSelectItem();
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

	public void InitSelectItem()
	{
		this.m_kSelectItem.ClearData();
		this.m_nSelectItemNum = 0;
		this.m_kSellNum.Text = string.Empty;
		this.m_nUseTicketNum = 0;
		this.m_nResultItemUnique = 0;
		this.m_kSelectItemName.Text = string.Empty;
		this.m_kName.Text = string.Empty;
		this.m_kLimitTicketNum.Text = string.Empty;
		this.m_kSell.controlIsEnabled = false;
	}

	public void UpdateUI()
	{
		this.InitSelectItem();
		this.SetPoint();
		this.ShowTicketList();
	}

	public void ResultMessage()
	{
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(PointManager.NECTAR_TICKET);
		GuildWarExchangeTable guildWarExchangeTable = NrTSingleton<PointManager>.Instance.GetGuildWarExchangeTable(this.m_nResultItemUnique);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
			"targetname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(guildWarExchangeTable.m_nItemUnique),
			"count",
			this.m_nSelectItemNum
		});
		Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		this.m_nUseTicketNum = 0;
		this.SetUsetTicketNum();
		this.m_nResultItemUnique = 0;
	}

	public void AddExchangeLimitUpdate(int ItemUnique, int GuildWar_ExchangeLimit)
	{
		if (ItemUnique != 0)
		{
			GUILDWAR_EXCHANGE gUILDWAR_EXCHANGE = new GUILDWAR_EXCHANGE();
			gUILDWAR_EXCHANGE.i32ItemUnique = ItemUnique;
			gUILDWAR_EXCHANGE.i32ExchangeLimit = GuildWar_ExchangeLimit;
			if (this.m_GuildWar_Exchange_Limit.ContainsKey(gUILDWAR_EXCHANGE.i32ItemUnique))
			{
				this.m_GuildWar_Exchange_Limit[gUILDWAR_EXCHANGE.i32ItemUnique].i32ExchangeLimit = gUILDWAR_EXCHANGE.i32ExchangeLimit;
			}
			else
			{
				this.m_GuildWar_Exchange_Limit.Add(gUILDWAR_EXCHANGE.i32ItemUnique, gUILDWAR_EXCHANGE);
			}
		}
	}

	private void ClickHelp(IUIObject obj)
	{
		if (!this.m_kHelpTail.Visible)
		{
			this.m_kHelpTail.Visible = true;
			this.m_kHelpBG.Visible = true;
			this.m_kHelpText.Visible = true;
		}
		else
		{
			this.m_kHelpTail.Visible = false;
			this.m_kHelpBG.Visible = false;
			this.m_kHelpText.Visible = false;
		}
	}
}
