using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExchangeEvolutionDlg : Form
{
	public readonly int FIRST_ELEMENT = 50417;

	private ItemTexture m_itSelectItem;

	private NewListBox m_nlbList;

	private Toggle m_tItem;

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

	private Label m_lbExtraLabel;

	private TextField m_tfSellNum;

	private int m_nSelectItemIDX;

	private int m_nSelectItemUnique;

	private int m_nSelectItemNum;

	private int m_nGetItemNum;

	private int m_nTicketNum;

	private int m_nUseTicketNum;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Point/DLG_Evolutionexchange", G_ID.EXCHANGE_EVOLUTION_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_nlbList = (base.GetControl("newlistbox_exchangeevolution") as NewListBox);
		this.m_nlbList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_itSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_itSelectItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelectItem));
		this.m_tItem = (base.GetControl("CheckBox_Item") as Toggle);
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
		this.InitSelectItem();
		this.Show_ExchangeList();
		base.SetShowLayer(2, false);
		base.SetScreenCenter();
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(this.FIRST_ELEMENT);
		this.Set_UserItemNum();
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
		this.m_tItem.Value = true;
	}

	public void Update_List()
	{
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(this.FIRST_ELEMENT);
		this.InitSelectItem();
		this.Set_UserItemNum();
		this.Show_ExchangeList();
		this.ClickList(null);
	}

	private void Set_UserItemNum()
	{
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		this.m_lbName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(exchangeEvolutionTable.m_nNeedItemUnique.ToString());
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
		Dictionary<int, ExchangeEvolutionTable> totalExchangeEvolutionTable = NrTSingleton<PointManager>.Instance.GetTotalExchangeEvolutionTable();
		foreach (ExchangeEvolutionTable current in totalExchangeEvolutionTable.Values)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbList.ColumnNum, true, string.Empty);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nItemUnique);
			ITEM iTEM = new ITEM();
			iTEM.m_nItemUnique = current.m_nItemUnique;
			iTEM.m_nOption[2] = NrTSingleton<ItemManager>.Instance.GetItemInfo(current.m_nItemUnique).m_nQualityLevel;
			newListItem.SetListItemData(1, iTEM, "Material", null, null);
			newListItem.SetListItemData(2, itemNameByItemUnique, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_nNeedItemUnique), null, null, null);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("632"),
				"count",
				NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + current.m_nNeedCount
			});
			newListItem.SetListItemData(4, empty, null, null, null);
			newListItem.SetListItemData(5, false);
			newListItem.Data = current;
			this.m_nlbList.Add(newListItem);
		}
		this.m_nlbList.RepositionItems();
		this.m_nlbList.SetSelectedItem(0);
	}

	private void ClickList(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.m_nlbList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)selectedItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		this.m_nTicketNum = NkUserInventory.GetInstance().Get_First_ItemCnt(exchangeEvolutionTable.m_nNeedItemUnique);
		this.m_itSelectItem.SetItemTexture(exchangeEvolutionTable.m_nItemUnique, 0, false, 1f);
		this.m_itSelectItem.Data = exchangeEvolutionTable;
		this.m_nSelectItemUnique = exchangeEvolutionTable.m_nItemUnique;
		this.m_nSelectItemIDX = exchangeEvolutionTable.m_nIDX;
		this.m_nSelectItemNum = 1;
		this.m_nGetItemNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nItemCount;
		this.m_nUseTicketNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nNeedCount;
		this.m_tfSellNum.MaxValue = (long)(this.m_nTicketNum / exchangeEvolutionTable.m_nNeedCount * exchangeEvolutionTable.m_nItemCount);
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
		this.m_lbSelectItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(exchangeEvolutionTable.m_nItemUnique);
		this.Set_UserItemNum();
	}

	private void ClickSelectItem(IUIObject obj)
	{
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (this.m_itSelectItem.Data is ITEM)
		{
			ITEM pkItem = (ITEM)this.m_itSelectItem.Data;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, pkItem, null, false);
		}
		else if (this.m_itSelectItem.Data is PointTable)
		{
			PointTable pointTable = (PointTable)this.m_itSelectItem.Data;
			ITEM iTEM = new ITEM();
			iTEM.Init();
			iTEM.m_nItemUnique = pointTable.m_nItemUnique;
			iTEM.m_nItemNum = 1;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
		else if (this.m_itSelectItem.Data is ExchangeEvolutionTable)
		{
			ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
			ITEM iTEM2 = new ITEM();
			iTEM2.Init();
			iTEM2.m_nItemUnique = exchangeEvolutionTable.m_nItemUnique;
			iTEM2.m_nItemNum = 1 * exchangeEvolutionTable.m_nItemCount;
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM2, null, false);
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
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		long num = a_cForm.GetNum();
		this.m_nSelectItemNum = (int)num;
		this.m_nGetItemNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nItemCount;
		this.m_nUseTicketNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nNeedCount;
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
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		this.m_nSelectItemNum--;
		if (1 >= this.m_nSelectItemNum)
		{
			this.m_nSelectItemNum = 1;
		}
		this.m_nUseTicketNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nItemCount;
		this.m_nGetItemNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nNeedCount;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.Set_UserItemNum();
	}

	private void ClickPlus(IUIObject obj)
	{
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		this.m_nSelectItemNum++;
		int num = this.m_nTicketNum / exchangeEvolutionTable.m_nNeedCount;
		if (this.m_nSelectItemNum > num)
		{
			this.m_nSelectItemNum = num;
		}
		this.m_nUseTicketNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nNeedCount;
		this.m_nGetItemNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nItemCount;
		this.m_tfSellNum.Text = this.m_nGetItemNum.ToString();
		this.Set_UserItemNum();
	}

	private void ClickAll(IUIObject obj)
	{
		ExchangeEvolutionTable exchangeEvolutionTable = (ExchangeEvolutionTable)this.m_itSelectItem.Data;
		if (exchangeEvolutionTable == null)
		{
			return;
		}
		this.m_nSelectItemNum = this.m_nTicketNum / exchangeEvolutionTable.m_nNeedCount;
		this.m_nGetItemNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nItemCount;
		this.m_nUseTicketNum = this.m_nSelectItemNum * exchangeEvolutionTable.m_nNeedCount;
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
		GS_EXCHANGE_EVOLUTION_REQ gS_EXCHANGE_EVOLUTION_REQ = new GS_EXCHANGE_EVOLUTION_REQ();
		gS_EXCHANGE_EVOLUTION_REQ.nIDX = this.m_nSelectItemIDX;
		gS_EXCHANGE_EVOLUTION_REQ.nItemUnique = this.m_nSelectItemUnique;
		gS_EXCHANGE_EVOLUTION_REQ.nItemNum = this.m_nGetItemNum;
		gS_EXCHANGE_EVOLUTION_REQ.nSelectNum = this.m_nSelectItemNum;
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
			msgBoxUI.SetMsg(new YesDelegate(this.BuyItem), gS_EXCHANGE_EVOLUTION_REQ, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
		}
	}

	private void BuyItem(object obj)
	{
		GS_EXCHANGE_EVOLUTION_REQ gS_EXCHANGE_EVOLUTION_REQ = (GS_EXCHANGE_EVOLUTION_REQ)obj;
		if (gS_EXCHANGE_EVOLUTION_REQ == null)
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_EVOLUTION_REQ, gS_EXCHANGE_EVOLUTION_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Gear_Evolve.ToString());
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
