using System;
using UnityEngine;
using UnityForms;

public class ChatOptionDlg : Form
{
	private const int TOOLBAR_NUM = 2;

	private Toolbar m_TB;

	private TextField[] _tfChatMacro = new TextField[8];

	private Button _bMacroSetButton;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Chat/DLG_ChatOption", G_ID.CHAT_OPTION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_TB = (base.GetControl("ToolBar") as Toolbar);
		for (int i = 0; i < 2; i++)
		{
			UIPanelTab expr_2A = this.m_TB.Control_Tab[i];
			expr_2A.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_2A.ButtonClick, new EZValueChangedDelegate(this.OnClickToolBar));
		}
		this.m_TB.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("526");
		this.m_TB.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("527");
		this.m_TB.FirstSetting();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		this._tfChatMacro[0] = (base.GetControl("TextField_F1TextField") as TextField);
		Debug.Log("_tfChatMacro >> " + kMyCharInfo.m_ChatMacroInfo.GetChatMacro(0));
		this._tfChatMacro[0].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(0);
		this._tfChatMacro[1] = (base.GetControl("TextField_F2TextField") as TextField);
		this._tfChatMacro[1].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(1);
		this._tfChatMacro[2] = (base.GetControl("TextField_F3TextField") as TextField);
		this._tfChatMacro[2].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(2);
		this._tfChatMacro[3] = (base.GetControl("TextField_F4TextField") as TextField);
		this._tfChatMacro[3].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(3);
		this._tfChatMacro[4] = (base.GetControl("TextField_F5TextField") as TextField);
		this._tfChatMacro[4].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(4);
		this._tfChatMacro[5] = (base.GetControl("TextField_F6TextField") as TextField);
		this._tfChatMacro[5].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(5);
		this._tfChatMacro[6] = (base.GetControl("TextField_F7TextField") as TextField);
		this._tfChatMacro[6].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(6);
		this._tfChatMacro[7] = (base.GetControl("TextField_F8TextField") as TextField);
		this._tfChatMacro[7].Text = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(7);
		this._bMacroSetButton = (base.GetControl("Button0") as Button);
		Button expr_26D = this._bMacroSetButton;
		expr_26D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_26D.Click, new EZValueChangedDelegate(this.BtnClickMacroSet));
		this.ShowTab(0);
		base.SetScreenCenter();
	}

	private void ShowTab(int tabindex)
	{
		base.ShowLayer(tabindex + 2);
	}

	private void OnClickToolBar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.ShowTab(uIPanelTab.panel.index);
	}

	public void BtnClickMacroSet(IUIObject obj)
	{
	}
}
