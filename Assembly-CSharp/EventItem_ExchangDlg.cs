using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class EventItem_ExchangDlg : Form
{
	private ItemTexture m_itItemEquip;

	private ItemTexture[] m_itHaveItem;

	private Label[] m_lbHaveItemName;

	private Label[] m_lbHaveItemCount;

	private DrawTexture[] m_dtHaveItemBG;

	private Label m_lbGuideText;

	private NewListBox m_ListBox;

	private Button m_btnClick;

	private EventExchangeTable m_SelectData;

	private Dictionary<int, ITEM_EXCHANGE_LIMIT> m_Exchange_Limit = new Dictionary<int, ITEM_EXCHANGE_LIMIT>();

	private int m_SelectIndex;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Event/DLG_EVENT_EXCHANGE", G_ID.EXCHANGE_EVENTITEM_DLG, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_itItemEquip = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_itItemEquip.AddValueChangedDelegate(new EZValueChangedDelegate(this.ShowToolTip));
		this.m_lbGuideText = (base.GetControl("LB_TextBG") as Label);
		this.m_itHaveItem = new ItemTexture[3];
		this.m_lbHaveItemName = new Label[3];
		this.m_lbHaveItemCount = new Label[3];
		this.m_dtHaveItemBG = new DrawTexture[3];
		for (int i = 0; i < 3; i++)
		{
			this.m_itHaveItem[i] = (base.GetControl(string.Format("IT_ITEMICON{0}", (i + 1).ToString())) as ItemTexture);
			this.m_itHaveItem[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ShowToolTip));
			this.m_lbHaveItemName[i] = (base.GetControl(string.Format("LB_Item{0}", (i + 1).ToString())) as Label);
			this.m_lbHaveItemName[i].SetText(string.Empty);
			this.m_lbHaveItemCount[i] = (base.GetControl(string.Format("LB_ItemHave{0}", (i + 1).ToString())) as Label);
			this.m_lbHaveItemCount[i].SetText(string.Empty);
			this.m_dtHaveItemBG[i] = (base.GetControl(string.Format("DT_ITEM{0}BG", (i + 1).ToString())) as DrawTexture);
			this.m_dtHaveItemBG[i].Visible = false;
		}
		this.m_ListBox = (base.GetControl("NLB_eventsellist") as NewListBox);
		this.m_ListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemClick));
		this.m_btnClick = (base.GetControl("BT_Confirm") as Button);
		this.m_btnClick.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnConfirm));
		GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ gS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ = new GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ();
		if (gS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ != null)
		{
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ, gS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_REQ);
		}
	}

	public void ShowUI()
	{
		this.SetData();
		this.Show();
	}

	private void ShowToolTip(IUIObject pObj)
	{
		ItemTexture itemTexture = (ItemTexture)pObj;
		if (itemTexture == null)
		{
			return;
		}
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (itemTooltipDlg != null)
		{
			if (itemTexture.Data is int)
			{
				itemTooltipDlg.Set_Tooltip(base.Orignal_ID, (int)itemTexture.Data);
			}
			else if (itemTexture.Data is ITEM)
			{
				itemTooltipDlg.Set_Tooltip(base.Orignal_ID, itemTexture.Data as ITEM, null, false);
			}
			else
			{
				itemTooltipDlg.Close();
			}
		}
	}

	private void ShowHaveItem()
	{
		if (this.m_SelectData == null)
		{
			return;
		}
		this.m_itItemEquip.SetItemTexture(this.m_SelectData.m_nItemUnique);
		this.m_itItemEquip.Data = this.m_SelectData.m_nItemUnique;
		string text = string.Empty;
		bool flag = true;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_SelectData.m_nNeedItemUnique[i] > 0)
			{
				this.MateralDataShow(i, true);
				int num = NkUserInventory.instance.Get_First_ItemCnt(this.m_SelectData.m_nNeedItemUnique[i]);
				this.m_itHaveItem[i].SetItemTexture(this.m_SelectData.m_nNeedItemUnique[i]);
				this.m_itHaveItem[i].Data = this.m_SelectData.m_nNeedItemUnique[i];
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3468"),
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_SelectData.m_nNeedItemUnique[i])
				});
				this.m_lbHaveItemName[i].SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2158"),
					"count",
					num
				});
				if (num < this.m_SelectData.m_nNeedItemCount[i])
				{
					text = NrTSingleton<CTextParser>.Instance.GetTextColor("1305") + text;
					if (flag)
					{
						flag = false;
					}
				}
				this.m_lbHaveItemCount[i].SetText(text);
			}
			else
			{
				this.MateralDataShow(i, false);
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3476"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_SelectData.m_nItemUnique),
			"count",
			this.m_SelectData.m_nItemNum
		});
		this.m_lbGuideText.Text = text;
		if (!flag)
		{
			this.m_btnClick.controlIsEnabled = false;
		}
		else
		{
			this.m_btnClick.controlIsEnabled = true;
		}
	}

	private void MateralDataShow(int index, bool bShow)
	{
		this.m_itHaveItem[index].Visible = bShow;
		this.m_lbHaveItemName[index].Visible = bShow;
		this.m_lbHaveItemCount[index].Visible = bShow;
		this.m_dtHaveItemBG[index].Visible = bShow;
	}

	private void SetData()
	{
		Dictionary<int, EventExchangeTable> totalEventExchangeTable = NrTSingleton<PointManager>.Instance.GetTotalEventExchangeTable();
		foreach (EventExchangeTable current in totalEventExchangeTable.Values)
		{
			NewListItem item = this.SetListItemData(current);
			this.m_ListBox.Add(item);
		}
		this.m_ListBox.RepositionItems();
	}

	private NewListItem SetListItemData(EventExchangeTable pData)
	{
		string text = string.Empty;
		NewListItem newListItem = new NewListItem(this.m_ListBox.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(2, new ITEM
		{
			m_nItemUnique = pData.m_nItemUnique,
			m_nItemNum = pData.m_nItemNum
		}, null, null, null);
		newListItem.SetListItemData(3, NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(pData.m_nItemUnique), null, null, null);
		if (pData.m_nExchangeLimit != -1)
		{
			int num = 0;
			if (this.m_Exchange_Limit.ContainsKey(pData.m_nItemUnique))
			{
				ITEM_EXCHANGE_LIMIT iTEM_EXCHANGE_LIMIT = this.m_Exchange_Limit[pData.m_nItemUnique];
				if (iTEM_EXCHANGE_LIMIT != null)
				{
					num = pData.m_nExchangeLimit - iTEM_EXCHANGE_LIMIT.i32ExchangeLimit;
				}
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("568"),
				"Count",
				num,
				"Count2",
				pData.m_nExchangeLimit
			});
			if (num == pData.m_nExchangeLimit)
			{
				text = NrTSingleton<CTextParser>.Instance.GetTextColor("1305") + text;
			}
			newListItem.SetListItemData(10, text, null, null, null);
		}
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			if (pData.m_nNeedItemUnique[i] > 0)
			{
				ITEM iTEM = new ITEM();
				iTEM.m_nItemUnique = pData.m_nNeedItemUnique[i];
				iTEM.m_nItemNum = pData.m_nNeedItemCount[i];
				newListItem.SetListItemData(4 + num2, iTEM, null, null, null);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3468"),
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iTEM.m_nItemUnique)
				});
				newListItem.SetListItemData(7 + num2, text, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(4 + num2, false);
				newListItem.SetListItemData(7 + num2, false);
			}
			num2++;
		}
		newListItem.Data = pData;
		return newListItem;
	}

	private void OnItemClick(IUIObject pObj)
	{
		this.m_SelectData = (EventExchangeTable)this.m_ListBox.SelectedItem.Data;
		this.m_SelectIndex = this.m_ListBox.SelectedItem.index;
		this.ShowHaveItem();
	}

	private void OnConfirm(IUIObject pObj)
	{
		if (this.m_SelectData != null)
		{
			if (this.m_Exchange_Limit.ContainsKey(this.m_SelectData.m_nItemUnique) && this.m_Exchange_Limit[this.m_SelectData.m_nItemUnique].i32ExchangeLimit == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("842"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				return;
			}
			if (this.m_SelectData.m_nExchangeLimit != -1)
			{
				if (this.m_Exchange_Limit.ContainsKey(this.m_SelectData.m_nItemUnique))
				{
					if (1 > this.m_Exchange_Limit[this.m_SelectData.m_nItemUnique].i32ExchangeLimit)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("841"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
						return;
					}
				}
				else if (1 > this.m_SelectData.m_nExchangeLimit)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("841"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					return;
				}
			}
			GS_EXCHANGE_EVENT_ITEM_REQ gS_EXCHANGE_EVENT_ITEM_REQ = new GS_EXCHANGE_EVENT_ITEM_REQ();
			gS_EXCHANGE_EVENT_ITEM_REQ.nIDX = this.m_SelectData.m_nIDX;
			gS_EXCHANGE_EVENT_ITEM_REQ.nSelectNum = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXCHANGE_EVENT_ITEM_REQ, gS_EXCHANGE_EVENT_ITEM_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2591"));
		}
	}

	public void AddLimitData(ITEM_EXCHANGE_LIMIT pData)
	{
		if (!this.m_Exchange_Limit.ContainsKey(pData.i32ItemUnique))
		{
			this.m_Exchange_Limit.Add(pData.i32ItemUnique, pData);
		}
	}

	private void AddUseData(int i32ItemUnique, int i32ItemNum)
	{
		if (this.m_Exchange_Limit.ContainsKey(i32ItemUnique))
		{
			this.m_Exchange_Limit[i32ItemUnique].i32ExchangeLimit--;
		}
		else if (this.m_SelectData != null)
		{
			ITEM_EXCHANGE_LIMIT iTEM_EXCHANGE_LIMIT = new ITEM_EXCHANGE_LIMIT();
			iTEM_EXCHANGE_LIMIT.i32ItemUnique = i32ItemUnique;
			iTEM_EXCHANGE_LIMIT.i32ExchangeLimit = this.m_SelectData.m_nExchangeLimit - 1;
			this.m_Exchange_Limit.Add(i32ItemUnique, iTEM_EXCHANGE_LIMIT);
		}
	}

	public void ReflashData(int i32ItemUnique, int i32ItemNum)
	{
		this.AddUseData(i32ItemUnique, i32ItemNum);
		this.ShowHaveItem();
		this.m_ListBox.RemoveAddNew(this.m_SelectIndex, this.SetListItemData(this.m_SelectData));
		this.m_ListBox.RepositionItems();
	}
}
