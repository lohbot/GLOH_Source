using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class PlunderProtectDlg : Form
{
	private Button m_bAgree_1;

	private Button m_bAgree_2;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_protect", G_ID.PLUNDER_PROTECT_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_bAgree_1 = (base.GetControl("Button_use1") as Button);
		Button expr_1C = this.m_bAgree_1;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnAgree1));
		this.m_bAgree_2 = (base.GetControl("Button_use2") as Button);
		Button expr_59 = this.m_bAgree_2;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnAgree2));
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public void OnAgree1(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string empty = string.Empty;
		int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_BUY1) / 60;
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_PRICE1);
		if (value > NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		byte b = 0;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("151"),
			"timestring",
			num.ToString(),
			"num",
			value.ToString()
		});
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("150");
		msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKAgree), b, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
	}

	public void OnAgree2(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string empty = string.Empty;
		int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_BUY2) / 60;
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_PRICE2);
		if (value > NkUserInventory.GetInstance().Get_First_ItemCnt(70000))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		byte b = 1;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("151"),
			"timestring",
			num.ToString(),
			"num",
			value.ToString()
		});
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("150");
		msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKAgree), b, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
	}

	public void MsgBoxOKAgree(object a_oObject)
	{
		byte nMode = (byte)a_oObject;
		GS_PLUNDER_PROTECT_REQ gS_PLUNDER_PROTECT_REQ = new GS_PLUNDER_PROTECT_REQ();
		gS_PLUNDER_PROTECT_REQ.m_nMode = nMode;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_PROTECT_REQ, gS_PLUNDER_PROTECT_REQ);
		this.Close();
	}
}
