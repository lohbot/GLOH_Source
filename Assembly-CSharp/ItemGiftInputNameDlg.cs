using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ItemGiftInputNameDlg : Form
{
	private TextField m_tfCharName;

	private Button m_btOK;

	private Button m_btCancel;

	private ItemMallItemManager.eItemMall_SellType m_eSellType = ItemMallItemManager.eItemMall_SellType.ITEMMALL;

	private ITEM_MALL_ITEM m_SelectItem;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Item/dlg_gift_input_name", G_ID.ITEMGIFTINPUTNAME_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_tfCharName = (base.GetControl("TextField_charname") as TextField);
		this.m_tfCharName.ClearText();
		this.m_btOK = (base.GetControl("Button_ok") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	private void ClickOK(IUIObject obj)
	{
		string text = this.m_tfCharName.GetText();
		if (!this.IsCheckGitf(text))
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("327"),
				"Product",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(this.m_SelectItem.m_strTextKey),
				"targetname",
				text
			});
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), text, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL);
		}
	}

	public void MsgBoxOKEvent(object EventObject)
	{
		string text = (string)EventObject;
		if (text != null)
		{
			if (!this.IsCheckGitf(text))
			{
				return;
			}
			NrTSingleton<ItemMallItemManager>.Instance.SetTradeItem(this.m_SelectItem, this.m_eSellType);
			GS_ITEMMALL_CHECK_CAN_TRADE_REQ gS_ITEMMALL_CHECK_CAN_TRADE_REQ = new GS_ITEMMALL_CHECK_CAN_TRADE_REQ();
			gS_ITEMMALL_CHECK_CAN_TRADE_REQ.MallIndex = this.m_SelectItem.m_Idx;
			TKString.StringChar(text, ref gS_ITEMMALL_CHECK_CAN_TRADE_REQ.strGiftUserName);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEMMALL_CHECK_CAN_TRADE_REQ, gS_ITEMMALL_CHECK_CAN_TRADE_REQ);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMGIFTINPUTNAME_DLG);
		}
	}

	private void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMGIFTINPUTNAME_DLG);
	}

	public bool IsCheckGitf(string strCharName)
	{
		if (strCharName.Length == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("51"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if (strCharName.Length > 21)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("126"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo.GetCharName().Equals(strCharName))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("712"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if (UIDataManager.IsFilterSpecialCharacters(strCharName, NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea()))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		return true;
	}

	public void SetTradeItem(ITEM_MALL_ITEM SelectItem, ItemMallItemManager.eItemMall_SellType eSellType)
	{
		this.m_SelectItem = SelectItem;
		this.m_eSellType = eSellType;
	}
}
