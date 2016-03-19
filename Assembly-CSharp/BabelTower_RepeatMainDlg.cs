using System;
using UnityForms;

public class BabelTower_RepeatMainDlg : Form
{
	private Button m_btRepeatStop;

	private DrawTexture m_dtBack;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "BabelTower/DLG_babel_repeat_bg", G_ID.BABELTOWER_REPEAT_MAIN_DLG, false);
		base.ShowBlackBG(1f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btRepeatStop = (base.GetControl("Button_RepeatStop") as Button);
		Button expr_1C = this.m_btRepeatStop;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnStopBabelRepeat));
		this.m_dtBack = (base.GetControl("Main_BG") as DrawTexture);
		UIDataManager.MuteSound(true);
		this.m_dtBack.SetTextureFromBundle("UI/Loading/chaostower");
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(0f, 0f);
		if (this.m_dtBack != null)
		{
			this.m_dtBack.SetSize(GUICamera.width, GUICamera.height);
			this.m_btRepeatStop.SetSize(GUICamera.width, GUICamera.height);
		}
	}

	public override void OnClose()
	{
		UIDataManager.MuteSound(false);
	}

	public void OnStopBabelRepeat(IUIObject obj)
	{
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			if (NrTSingleton<NkBabelMacroManager>.Instance.Status < eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_START)
			{
				NrTSingleton<NkBabelMacroManager>.Instance.SetStop(true);
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.RequestBabelMacroStopAndAutoBattle), null, new NoDelegate(this.RequestBableMacroStopCancle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("187"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("188"), eMsgType.MB_OK_CANCEL);
			}
			return;
		}
	}

	public void RequestBabelMacroStopAndAutoBattle(object a_oObject)
	{
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
			UIDataManager.MuteSound(false);
			NrTSingleton<NkBabelMacroManager>.Instance.SetStop(false);
		}
	}

	public void RequestBableMacroStopCancle(object a_oObject)
	{
		NrTSingleton<NkBabelMacroManager>.Instance.SetStop(false);
	}
}
