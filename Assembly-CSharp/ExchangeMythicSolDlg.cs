using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExchangeMythicSolDlg : Form
{
	private enum TYPE
	{
		TYPE_EXCHANGE_NEW = 1,
		TYPE_EXCHANGE_OLD
	}

	private Toggle m_tgNewItem;

	private Toggle m_tgOldItem;

	private ItemTexture m_itSelectItem;

	private NewListBox m_nlbList;

	private Button m_btnSell;

	private Button m_btnMinus;

	private Button m_btnPlus;

	private Button m_btnAll;

	private Button m_btnHelp;

	private Button m_btnInputBox;

	private Label m_lbSelectItemName;

	private Label m_lbName;

	private Label m_lbLimitTicketNum;

	private Label m_lbName2;

	private Label m_lbLimitTicketNum2;

	private TextField m_tfSellNum;

	private ExchangeMythicSolDlg.TYPE m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_NEW;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private int m_nSelectItemIDX;

	private int m_nGetItemNum;

	private int m_nTicketNum;

	private int m_nUseTicketNum;

	private bool m_bShow;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Point/dlg_mythicexchange", G_ID.EXCHANGE_MYTHICSOL_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_nlbList = (base.GetControl("newlistbox_mythicexchange") as NewListBox);
		this.m_nlbList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_tgNewItem = (base.GetControl("CheckBox_Ticket") as Toggle);
		this.m_tgNewItem.Value = true;
		this.m_tgNewItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_NewItemExchange));
		this.m_tgOldItem = (base.GetControl("CheckBox_Item") as Toggle);
		this.m_tgOldItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_OldItemExchange));
		this.m_itSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_itSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectItem));
		this.m_btnSell = (base.GetControl("Button_confirm") as Button);
		this.m_btnSell.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSell));
		this.m_btnMinus = (base.GetControl("Button_CountDown") as Button);
		this.m_btnMinus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMinus));
		this.m_btnPlus = (base.GetControl("Button_CountUp") as Button);
		this.m_btnPlus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPlus));
		this.m_btnAll = (base.GetControl("Button_MaxCountUp") as Button);
		this.m_btnAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAll));
		this.m_lbName = (base.GetControl("Label_text1") as Label);
		this.m_lbLimitTicketNum = (base.GetControl("Label_text2") as Label);
		this.m_lbName2 = (base.GetControl("Label_text03") as Label);
		this.m_lbLimitTicketNum2 = (base.GetControl("Label_text04") as Label);
		this.m_lbSelectItemName = (base.GetControl("Label_text") as Label);
		this.m_tfSellNum = (base.GetControl("TextField_Count") as TextField);
		this.m_tfSellNum.NumberMode = true;
		this.m_btnInputBox = (base.GetControl("Button_NumPad") as Button);
		this.m_btnInputBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickInputBox));
		this.m_btnHelp = (base.GetControl("Help_Button") as Button);
		this.m_btnHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		base.SetScreenCenter();
		GS_MYTHICSOLLIMIT_INFO_REQ gS_MYTHICSOLLIMIT_INFO_REQ = default(GS_MYTHICSOLLIMIT_INFO_REQ);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHICSOLLIMIT_INFO_REQ, gS_MYTHICSOLLIMIT_INFO_REQ);
	}

	public override void AfterShow()
	{
		if (!this.m_bShow)
		{
			this.Hide();
		}
	}

	public void InitSelectItem()
	{
		this.m_itSelectItem.ClearData();
		this.m_nSelectItemNum = 0;
		this.m_nGetItemNum = 0;
		this.m_tfSellNum.Text = string.Empty;
		this.m_nUseTicketNum = 0;
		this.m_lbSelectItemName.Text = string.Empty;
		this.m_lbName.Text = string.Empty;
		this.m_lbName2.Text = string.Empty;
		this.m_lbLimitTicketNum.Text = string.Empty;
		this.m_lbLimitTicketNum2.Text = string.Empty;
		this.m_btnSell.controlIsEnabled = false;
	}

	public void UpdateUI()
	{
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(PointManager.NECTAR_TICKET);
		this.Set_UserItemNum();
		this.InitSelectItem();
		this.Show_ExchangeList();
	}

	public void ShowUI()
	{
		this.m_bShow = true;
		this.InitSelectItem();
		this.Show_ExchangeList();
		base.SetShowLayer(2, false);
		base.SetScreenCenter();
		this.m_nTicketNum = 0;
		this.Set_UserItemNum();
		this.Show();
	}

	private void Set_UserItemNum()
	{
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
		if (mythicSolTable == null)
		{
			return;
		}
		this.m_lbName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(mythicSolTable.m_nNeedItemUnique.ToString());
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2272"),
			"count1",
			this.m_nUseTicketNum,
			"count2",
			this.m_nTicketNum
		});
		this.m_lbLimitTicketNum.Text = empty;
	}

	private void Show_ExchangeList()
	{
		this.m_nUseTicketNum = 0;
		this.m_nlbList.Clear();
		Dictionary<int, MythicSolTable> totalMythicSolTable = NrTSingleton<PointManager>.Instance.GetTotalMythicSolTable();
		foreach (MythicSolTable current in totalMythicSolTable.Values)
		{
			if (this.m_eType == (ExchangeMythicSolDlg.TYPE)current.m_nType)
			{
				if (!NrTSingleton<PointManager>.Instance.IsMythicSolLimit(current.m_nItemUnique))
				{
					NewListItem newListItem = new NewListItem(this.m_nlbList.ColumnNum, true, string.Empty);
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
					this.m_nlbList.Add(newListItem);
				}
			}
		}
		this.m_nlbList.RepositionItems();
		this.m_nlbList.SetSelectedItem(0);
	}

	private void Click_NewItemExchange(IUIObject obj)
	{
		if (!this.m_tgNewItem.Value)
		{
			return;
		}
		this.InitSelectItem();
		this.m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_NEW;
		this.Show_ExchangeList();
	}

	private void Click_OldItemExchange(IUIObject obj)
	{
		if (!this.m_tgOldItem.Value)
		{
			return;
		}
		this.InitSelectItem();
		this.m_eType = ExchangeMythicSolDlg.TYPE.TYPE_EXCHANGE_OLD;
		this.Show_ExchangeList();
	}

	private void ClickSelectItem(IUIObject obj)
	{
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (this.m_itSelectItem.Data is ITEM)
		{
			ITEM pkItem = (ITEM)this.m_itSelectItem.Data;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, pkItem, null, false);
		}
		else if (this.m_itSelectItem.Data is MythicSolTable)
		{
			MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
			ITEM iTEM = new ITEM();
			iTEM.Init();
			iTEM.m_nItemUnique = mythicSolTable.m_nItemUnique;
			iTEM.m_nItemNum = mythicSolTable.m_nExchangeNum;
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
		long maxValue = this.m_tfSellNum.MaxValue;
		long i64Min = 1L;
		inputNumberDlg.SetMinMax(i64Min, maxValue);
		inputNumberDlg.SetNum(0L);
		inputNumberDlg.SetInputNum(0L);
	}

	public void OnInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
		if (mythicSolTable == null)
		{
			return;
		}
		long num = a_cForm.GetNum();
		this.m_nSelectItemNum = (int)num;
		this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
		this.m_nGetItemNum = this.m_nSelectItemNum * mythicSolTable.m_nExchangeNum;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
		this.Set_UserItemNum();
	}

	public void OnCloseInputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void ClickMinus(IUIObject obj)
	{
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
		if (mythicSolTable == null)
		{
			return;
		}
		this.m_nSelectItemNum--;
		if (1 >= this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = 1;
		}
		this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
		this.m_nGetItemNum = this.m_nSelectItemNum * mythicSolTable.m_nExchangeNum;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.Set_UserItemNum();
	}

	private void ClickPlus(IUIObject obj)
	{
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
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
		this.m_nGetItemNum = this.m_nSelectItemNum * mythicSolTable.m_nExchangeNum;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.Set_UserItemNum();
	}

	private void ClickAll(IUIObject obj)
	{
		MythicSolTable mythicSolTable = (MythicSolTable)this.m_itSelectItem.Data;
		if (mythicSolTable == null)
		{
			return;
		}
		this.m_nSelectItemNum = this.m_nTicketNum / mythicSolTable.m_nNeedNum;
		this.m_nGetItemNum = this.m_nSelectItemNum * mythicSolTable.m_nExchangeNum;
		this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.Set_UserItemNum();
	}

	private void ClickSell(IUIObject obj)
	{
		if (this.m_itSelectItem != null)
		{
			ITEM iTEM = this.m_itSelectItem.Data as ITEM;
			if (iTEM != null && iTEM.IsLock())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
		}
		GS_EXCHANGE_MYTHICSOL_REQ gS_EXCHANGE_MYTHICSOL_REQ = new GS_EXCHANGE_MYTHICSOL_REQ();
		gS_EXCHANGE_MYTHICSOL_REQ.nType = (int)this.m_eType;
		gS_EXCHANGE_MYTHICSOL_REQ.nItemUnique = this.m_nSelectItemUnique;
		gS_EXCHANGE_MYTHICSOL_REQ.nItemNum = this.m_nGetItemNum;
		gS_EXCHANGE_MYTHICSOL_REQ.nIDX = this.m_nSelectItemIDX;
		gS_EXCHANGE_MYTHICSOL_REQ.nSelectNum = this.m_nSelectItemNum;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2257");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("204");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			textFromMessageBox,
			"targetname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nSelectItemUnique),
			"count",
			this.m_nGetItemNum
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_MYTHICSOL_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
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
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickList(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.m_nlbList.SelectedItem;
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
		this.m_itSelectItem.SetItemTexture(mythicSolTable.m_nItemUnique);
		this.m_itSelectItem.Data = mythicSolTable;
		this.m_nSelectItemUnique = mythicSolTable.m_nItemUnique;
		this.m_nSelectItemIDX = mythicSolTable.m_nIDX;
		this.m_nSelectItemNum = 1;
		this.m_nGetItemNum = this.m_nSelectItemNum * mythicSolTable.m_nExchangeNum;
		this.m_nUseTicketNum = this.m_nSelectItemNum * mythicSolTable.m_nNeedNum;
		this.m_tfSellNum.MaxValue = (long)(this.m_nTicketNum / mythicSolTable.m_nNeedNum * mythicSolTable.m_nExchangeNum);
		if (0L >= this.m_tfSellNum.MaxValue)
		{
			this.m_btnSell.controlIsEnabled = false;
			this.m_nSelectItemNum = 0;
		}
		else
		{
			this.m_btnSell.controlIsEnabled = true;
		}
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.m_lbSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mythicSolTable.m_nItemUnique);
		this.Set_UserItemNum();
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Myth.ToString());
		}
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
}
