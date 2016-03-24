using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class ItemGiftTargetDlg : Form
{
	public class TARGET_INFO
	{
		private string m_strName = string.Empty;

		private long m_lPersonID;

		private short m_iLevel;

		private int m_iCharKind;

		public string Name
		{
			get
			{
				return this.m_strName;
			}
			set
			{
				this.m_strName = value;
			}
		}

		public long PersonID
		{
			get
			{
				return this.m_lPersonID;
			}
			set
			{
				this.m_lPersonID = value;
			}
		}

		public short Level
		{
			get
			{
				return this.m_iLevel;
			}
			set
			{
				this.m_iLevel = value;
			}
		}

		public int CharKind
		{
			get
			{
				return this.m_iCharKind;
			}
			set
			{
				this.m_iCharKind = value;
			}
		}

		public void SetInfo(string strName, long lPersonID, short iLevel, int iCharKind)
		{
			this.Name = strName;
			this.PersonID = lPersonID;
			this.Level = iLevel;
			this.CharKind = iCharKind;
		}
	}

	public enum eTAB
	{
		eTAB_FRIEND,
		eTAB_GUILD,
		eTAB_INPUT,
		eTAB_MAX
	}

	private const float DELAY_TIME = 2f;

	private Toolbar m_tbTab;

	private NewListBox m_nlbGiftTarget;

	private DrawTexture m_dtNPCFace;

	private Button m_btClose;

	private ItemGiftTargetDlg.eTAB m_eTab;

	private List<ItemGiftTargetDlg.TARGET_INFO> m_TargetInfoList = new List<ItemGiftTargetDlg.TARGET_INFO>();

	private ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.ITEMMALL;

	private ITEM_MALL_ITEM m_SelectItem;

	private float m_fCheckTime;

	private CharCostumeInfo_Data m_giftCostumeData;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_gift_target", G_ID.ITEMGIFTTARGET_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tbTab = (base.GetControl("ToolBar") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("326");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1912");
		this.m_tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1100");
		UIPanelTab expr_86 = this.m_tbTab.Control_Tab[0];
		expr_86.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_86.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_B4 = this.m_tbTab.Control_Tab[1];
		expr_B4.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_B4.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_E2 = this.m_tbTab.Control_Tab[2];
		expr_E2.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_E2.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_nlbGiftTarget = (base.GetControl("NLB_Gift_Target") as NewListBox);
		this.m_nlbGiftTarget.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickGiftTargetList));
		this.m_dtNPCFace = (base.GetControl("DT_NPCIMG") as DrawTexture);
		this.m_dtNPCFace.SetTextureFromUISoldierBundle(eCharImageType.LARGE, "mine");
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.SelectTab(this.m_eTab);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void Update()
	{
		if (0f < this.m_fCheckTime && this.m_fCheckTime <= Time.time)
		{
			this.m_fCheckTime = 0f;
			this.SetEnable(true);
		}
	}

	public override void OnClose()
	{
		this.CostumeCharSetting();
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eTab = (ItemGiftTargetDlg.eTAB)uIPanelTab.panel.index;
		this.SelectTab(this.m_eTab);
	}

	public void SelectTab(ItemGiftTargetDlg.eTAB eTab)
	{
		switch (eTab)
		{
		case ItemGiftTargetDlg.eTAB.eTAB_FRIEND:
			this.AddFriendList();
			break;
		case ItemGiftTargetDlg.eTAB.eTAB_GUILD:
			this.AddGuildList();
			break;
		case ItemGiftTargetDlg.eTAB.eTAB_INPUT:
			this.AddInputList();
			break;
		}
	}

	public void AddFriendList()
	{
		this.m_nlbGiftTarget.Clear();
		this.m_TargetInfoList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (uSER_FRIEND_INFO != null)
			{
				if (uSER_FRIEND_INFO.nPersonID >= 11L)
				{
					ItemGiftTargetDlg.TARGET_INFO tARGET_INFO = new ItemGiftTargetDlg.TARGET_INFO();
					tARGET_INFO.SetInfo(TKString.NEWString(uSER_FRIEND_INFO.szName), uSER_FRIEND_INFO.nPersonID, uSER_FRIEND_INFO.i16Level, uSER_FRIEND_INFO.i32FaceCharKind);
					this.m_TargetInfoList.Add(tARGET_INFO);
				}
			}
		}
		if (0 < this.m_TargetInfoList.Count)
		{
			this.m_TargetInfoList.Sort(new Comparison<ItemGiftTargetDlg.TARGET_INFO>(this.CompareNameDESC));
			for (int i = 0; i < this.m_TargetInfoList.Count; i++)
			{
				NewListItem newListItem = this.GetNewListItem(this.m_TargetInfoList[i]);
				if (newListItem != null)
				{
					this.m_nlbGiftTarget.Add(newListItem);
				}
			}
		}
		this.m_nlbGiftTarget.RepositionItems();
	}

	public void AddGuildList()
	{
		this.m_nlbGiftTarget.Clear();
		this.m_TargetInfoList.Clear();
		for (int i = 0; i < NrTSingleton<NewGuildManager>.Instance.GetMemberCount(); i++)
		{
			NewGuildMember memberInfoFromIndex = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromIndex(i);
			if (memberInfoFromIndex != null)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID != memberInfoFromIndex.GetPersonID())
				{
					ItemGiftTargetDlg.TARGET_INFO tARGET_INFO = new ItemGiftTargetDlg.TARGET_INFO();
					tARGET_INFO.SetInfo(memberInfoFromIndex.GetCharName(), memberInfoFromIndex.GetPersonID(), memberInfoFromIndex.GetLevel(), memberInfoFromIndex.GetFaceCharKind());
					this.m_TargetInfoList.Add(tARGET_INFO);
				}
			}
		}
		if (0 < this.m_TargetInfoList.Count)
		{
			this.m_TargetInfoList.Sort(new Comparison<ItemGiftTargetDlg.TARGET_INFO>(this.CompareNameDESC));
			for (int i = 0; i < this.m_TargetInfoList.Count; i++)
			{
				NewListItem newListItem = this.GetNewListItem(this.m_TargetInfoList[i]);
				if (newListItem != null)
				{
					this.m_nlbGiftTarget.Add(newListItem);
				}
			}
		}
		this.m_nlbGiftTarget.RepositionItems();
	}

	public void AddInputList()
	{
		this.m_nlbGiftTarget.Clear();
		this.m_TargetInfoList.Clear();
		this.m_nlbGiftTarget.RepositionItems();
		ItemGiftInputNameDlg itemGiftInputNameDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMGIFTINPUTNAME_DLG) as ItemGiftInputNameDlg;
		if (itemGiftInputNameDlg != null)
		{
			itemGiftInputNameDlg.SetTradeItem(this.m_SelectItem, this.m_eSellType);
			itemGiftInputNameDlg.SetGiftCostumeData(this.m_giftCostumeData);
		}
	}

	public NewListItem GetNewListItem(ItemGiftTargetDlg.TARGET_INFO Info)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbGiftTarget.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, true);
		newListItem.SetListItemData(1, Info.CharKind, null, null, null);
		newListItem.SetListItemData(2, Info.Name, null, null, null);
		newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1723"), Info.PersonID, new EZValueChangedDelegate(this.ClickTargetUser), null);
		newListItem.Data = Info.PersonID;
		return newListItem;
	}

	private void OnClickGiftTargetList(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.m_nlbGiftTarget.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		long num = (long)selectedItem.data;
		if (0L < num)
		{
		}
	}

	private int CompareNameDESC(ItemGiftTargetDlg.TARGET_INFO a, ItemGiftTargetDlg.TARGET_INFO b)
	{
		if (b.Name.Equals(a.Name))
		{
			return b.Level.CompareTo(a.Level);
		}
		return b.Name.CompareTo(a.Name);
	}

	private void ClickTargetUser(IUIObject obj)
	{
		long num = (long)obj.Data;
		for (int i = 0; i < this.m_TargetInfoList.Count; i++)
		{
			if (this.m_TargetInfoList[i].PersonID == num)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					string empty = string.Empty;
					if (this.m_giftCostumeData != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("327"),
							"Product",
							NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(this.m_giftCostumeData.m_costumeUnique),
							"targetname",
							this.m_TargetInfoList[i].Name
						});
						msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKCostume), this.m_TargetInfoList[i].Name, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("327"),
							"Product",
							NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(this.m_SelectItem.m_strTextKey),
							"targetname",
							this.m_TargetInfoList[i].Name
						});
						msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), this.m_TargetInfoList[i].Name, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
					}
				}
				return;
			}
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		string text = (string)EventObject;
		if (text != null)
		{
			NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(this.m_SelectItem, this.m_eSellType);
			GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
			gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = this.m_SelectItem.m_Idx;
			TKString.StringChar(text, ref gS_ITEMMALL_CHECK_CAN_TRADE_REQ.strGiftUserName);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
			this.m_fCheckTime = Time.time + 2f;
			this.SetEnable(false);
		}
	}

	public void MsgBoxOKCostume(object eventObject)
	{
		string text = (string)eventObject;
		if (string.IsNullOrEmpty(text) || this.m_giftCostumeData == null)
		{
			return;
		}
		NrTSingleton<CostumeBuyManager>.Instance.BuyCostume(this.m_giftCostumeData, text);
	}

	public void SetTradeItem(ITEM_MALL_ITEM SelectItem, ItemMallItemManager.eItemMall_SellType eSellType)
	{
		this.m_SelectItem = SelectItem;
		this.m_eSellType = eSellType;
	}

	public void SetEnable(bool bEnable)
	{
		this.m_nlbGiftTarget.controlIsEnabled = bEnable;
	}

	public void SetGiftCostumeData(CharCostumeInfo_Data costumeData)
	{
		this.m_giftCostumeData = costumeData;
	}

	private void CostumeCharSetting()
	{
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null)
		{
			return;
		}
		if (costumeRoom_Dlg._costumeViewerSetter == null || costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter == null)
		{
			return;
		}
		costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter.VisibleChar(true);
	}
}
