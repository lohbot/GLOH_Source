using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Agit_MerchantDlg : Form
{
	public const float DELAY_TIME = 1f;

	private ItemTexture m_itSelectItem;

	private Label m_lbText;

	private Label m_lbEquip;

	private Label m_lbItemName;

	private Label m_lbItemNum;

	private Button m_btConfirm;

	private NewListBox m_nlbSellList;

	private Label m_lbTime;

	private Label m_SoldOut;

	private List<AGIT_MERCHANT_SUB_INFO> m_MerchantItemList = new List<AGIT_MERCHANT_SUB_INFO>();

	private string m_strText = string.Empty;

	private AGIT_MERCHANT_SUB_INFO m_SelectInfo;

	private float m_fDelayTime;

	private string m_strTime = string.Empty;

	private short m_LastBuy_SellTypeNul;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/dlg_Agit_Merchant", G_ID.AGIT_MERCHANT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_itSelectItem = (base.GetControl("ImageView_equip") as ItemTexture);
		this.m_lbText = (base.GetControl("Label_text") as Label);
		this.m_lbEquip = (base.GetControl("Label_equip") as Label);
		this.m_lbItemName = (base.GetControl("Label_itemname") as Label);
		this.m_lbItemNum = (base.GetControl("Label_itemnum") as Label);
		this.m_btConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickBuyItem));
		this.m_nlbSellList = (base.GetControl("NLB_Selllist") as NewListBox);
		this.m_nlbSellList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSellList));
		this.m_lbTime = (base.GetControl("Label_time") as Label);
		this.m_SoldOut = (base.GetControl("Label_soldout") as Label);
		this.m_SoldOut.Visible = false;
		this.InitControl();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClearMerchantItem()
	{
		this.m_MerchantItemList.Clear();
	}

	public void AddMerchantItem(AGIT_MERCHANT_SUB_INFO Data)
	{
		this.m_MerchantItemList.Add(Data);
	}

	public void InitControl()
	{
		this.m_lbText.SetText(string.Empty);
		this.m_lbEquip.SetText(string.Empty);
		this.m_lbItemName.SetText(string.Empty);
		this.m_lbItemNum.SetText(string.Empty);
		this.m_itSelectItem.ClearData();
	}

	public void RefreshInfo()
	{
		this.InitControl();
		this.m_nlbSellList.Clear();
		long num = 1L;
		int num2 = 0;
		for (int i = 0; i < this.m_MerchantItemList.Count; i++)
		{
			long iAtbFlag = num << (int)this.m_MerchantItemList[i].i16SellType;
			if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsAtbAgitMerchantBuyItemFlag(iAtbFlag))
			{
				this.MakeMerchantItem(this.m_MerchantItemList[i]);
				num2++;
			}
		}
		this.m_nlbSellList.RepositionItems();
		if (num2 == 0)
		{
			this.m_SoldOut.Visible = true;
		}
		else
		{
			this.m_SoldOut.Visible = false;
		}
		this.m_SelectInfo = null;
	}

	public void MakeMerchantItem(AGIT_MERCHANT_SUB_INFO Data)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbSellList.ColumnNum, true);
		newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(Data.i32ItemUnique), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Data.i32ItemUnique),
			"count",
			Data.i32ItemNum
		});
		newListItem.SetListItemData(2, this.m_strText, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("411"),
			"count",
			Data.i32PriceHearts
		});
		newListItem.SetListItemData(3, Data.i32PriceHearts.ToString(), null, null, null);
		newListItem.Data = Data;
		this.m_nlbSellList.Add(newListItem);
	}

	public void ClickSellList(IUIObject obj)
	{
		if (this.m_nlbSellList.GetSelectItem() == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("227"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		UIListItemContainer uIListItemContainer = this.m_nlbSellList.GetSelectItem() as UIListItemContainer;
		if (uIListItemContainer == null)
		{
			return;
		}
		if (uIListItemContainer.Data == null)
		{
			return;
		}
		AGIT_MERCHANT_SUB_INFO aGIT_MERCHANT_SUB_INFO = (AGIT_MERCHANT_SUB_INFO)uIListItemContainer.Data;
		if (aGIT_MERCHANT_SUB_INFO == null)
		{
			return;
		}
		this.SelectMerchantItem(aGIT_MERCHANT_SUB_INFO);
	}

	public void SelectMerchantItem(AGIT_MERCHANT_SUB_INFO MerchantInfo)
	{
		if (MerchantInfo == null)
		{
			return;
		}
		this.m_SelectInfo = MerchantInfo;
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = MerchantInfo.i32ItemUnique;
		iTEM.m_nItemNum = MerchantInfo.i32ItemNum;
		this.m_itSelectItem.SetItemTexture(iTEM);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2544"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(MerchantInfo.i32ItemUnique),
			"count",
			MerchantInfo.i32ItemNum
		});
		this.m_lbEquip.SetText(this.m_strText);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1803"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70000)
		});
		this.m_lbItemName.SetText(this.m_strText);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2272"),
			"count1",
			MerchantInfo.i32PriceHearts,
			"count2",
			NkUserInventory.GetInstance().Get_First_ItemCnt(70000)
		});
		this.m_lbItemNum.SetText(this.m_strText);
	}

	public void ClickBuyItem(IUIObject obj)
	{
		if (this.m_SelectInfo == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("236"),
			"itemname1",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70000),
			"count1",
			this.m_SelectInfo.i32PriceHearts,
			"itemname2",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_SelectInfo.i32ItemUnique),
			"count2",
			this.m_SelectInfo.i32ItemNum
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MsgOKBuyItem), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), this.m_strText, eMsgType.MB_OK_CANCEL);
	}

	public void MsgOKBuyItem(object a_oObject)
	{
		if (this.m_SelectInfo == null)
		{
			return;
		}
		long iAtbFlag = 1L << (int)this.m_SelectInfo.i16SellType;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsAtbAgitMerchantBuyItemFlag(iAtbFlag))
		{
			NrTSingleton<NewGuildManager>.Instance.Set_GS_NEWGUILD_AGIT_NPC_USE_ACK(2);
			return;
		}
		if (NkUserInventory.GetInstance().Get_First_ItemCnt(70000) < this.m_SelectInfo.i32PriceHearts)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_NPC_USE_REQ(1, this.m_SelectInfo.i16SellType);
		this.m_LastBuy_SellTypeNul = this.m_SelectInfo.i16SellType;
		this.SetControlEnable(false);
	}

	public override void Update()
	{
		if (this.m_fDelayTime > 0f && this.m_fDelayTime < Time.time)
		{
			this.m_fDelayTime = 0f;
			this.SetControlEnable(true);
		}
		this.UpdateTime();
	}

	public void SetControlEnable(bool bEnable)
	{
		if (!bEnable)
		{
			this.m_fDelayTime = Time.time + 1f;
		}
		this.m_btConfirm.controlIsEnabled = bEnable;
	}

	public void UpdateTime()
	{
		AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(1);
		if (agitNPCSubDataFromNPCType != null)
		{
			long i64Time = agitNPCSubDataFromNPCType.i64NPCEndTime - PublicMethod.GetCurTime();
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strTime, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
				"timestring",
				this.GetTimeToString(i64Time)
			});
			this.m_lbTime.SetText(this.m_strTime);
		}
	}

	public string GetTimeToString(long i64Time)
	{
		this.m_strText = string.Empty;
		if (i64Time > 0L)
		{
			long totalHourFromSec = PublicMethod.GetTotalHourFromSec(i64Time);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(i64Time);
			long num = i64Time % 60L;
			this.m_strText = string.Format("{0}:{1}:{2}", totalHourFromSec.ToString("00"), minuteFromSec.ToString("00"), num.ToString("00"));
		}
		return this.m_strText;
	}
}
