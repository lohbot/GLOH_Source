using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MsgBoxUI : Form
{
	private YesDelegate m_YesDelegate;

	private NoDelegate m_NoDelegate;

	private object m_oYesObject;

	private object m_oNoObject;

	private Box m_boxBG;

	private DrawTexture m_DrawTextureBG;

	private Label m_LbTitle;

	private Label m_LbNote;

	private Button m_btnOK;

	private Button m_btnCancel;

	private Button m_btnCancel2;

	private CheckBox m_CheckBox;

	private Label m_LbCheckBox;

	public int m_iParam;

	private string strOKText = string.Empty;

	private string strCancelText = string.Empty;

	private float m_fAutoCloseTime;

	private bool m_bOkEventImmediatelyClose = true;

	public float AutoCloseTime
	{
		get
		{
			return this.m_fAutoCloseTime;
		}
		set
		{
			this.m_fAutoCloseTime = value;
		}
	}

	public bool OkEventImmediatelyClose
	{
		set
		{
			this.m_bOkEventImmediatelyClose = value;
		}
	}

	public override void InitializeComponent()
	{
		this.m_YesDelegate = null;
		this.m_oYesObject = null;
		this.m_YesDelegate = null;
		this.m_oNoObject = null;
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFile(ref form, "Message/DLG_MsgBox", G_ID.MSGBOX_DLG, true);
		instance.CreateControl(ref this.m_boxBG, "MessageBg");
		instance.CreateControl(ref this.m_DrawTextureBG, "DrawTexture_DrawTexture6");
		instance.CreateControl(ref this.m_LbTitle, "Label_title");
		instance.CreateControl(ref this.m_LbNote, "Label_Note");
		instance.CreateControl(ref this.m_btnOK, "Button_ok");
		instance.CreateControl(ref this.m_btnCancel, "Button_cancel");
		instance.CreateControl(ref this.m_btnCancel2, "Button_cancel2");
		instance.CreateControl(ref this.m_CheckBox, "CheckBox_C");
		instance.CreateControl(ref this.m_LbCheckBox, "Label_C");
		Button expr_E7 = this.m_btnOK;
		expr_E7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E7.Click, new EZValueChangedDelegate(this.BtnMsg_OK));
		this.m_btnOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
		Button expr_128 = this.m_btnCancel;
		expr_128.Click = (EZValueChangedDelegate)Delegate.Combine(expr_128.Click, new EZValueChangedDelegate(this.BtnMsg_Cancel));
		this.m_btnCancel.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9"));
		Button expr_169 = this.m_btnCancel2;
		expr_169.Click = (EZValueChangedDelegate)Delegate.Combine(expr_169.Click, new EZValueChangedDelegate(this.BtnMsg_Cancel2));
		this.m_btnCancel2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8"));
		this.m_CheckBox.SetToggleState(1);
		this.Initialize();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		base.DonotDepthChange(90f);
		base.SetScreenCenter();
		this.Hide();
	}

	public void Initialize()
	{
		this.m_btnOK.Visible = false;
		this.m_btnCancel.Visible = false;
		this.m_btnCancel2.Visible = false;
		this.strOKText = this.m_btnOK.GetText();
		this.strCancelText = this.m_btnCancel.GetText();
		this.InitData();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void InitData()
	{
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, NoDelegate b_deNo, object b_oObject, string title, string message, string checkboxText, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		if (b_deNo != null)
		{
			this.m_NoDelegate = (NoDelegate)Delegate.Combine(this.m_NoDelegate, new NoDelegate(b_deNo.Invoke));
		}
		if (b_oObject != null)
		{
			this.m_oNoObject = b_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.m_LbCheckBox.SetText(checkboxText);
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, NoDelegate b_deNo, object b_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		if (b_deNo != null)
		{
			this.m_NoDelegate = (NoDelegate)Delegate.Combine(this.m_NoDelegate, new NoDelegate(b_deNo.Invoke));
		}
		if (b_oObject != null)
		{
			this.m_oNoObject = b_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, string title, string message, string checkboxText, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.m_LbCheckBox.SetText(checkboxText);
		this.Show();
	}

	public void SetMsg(YesDelegate a_deYes, object a_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_YesDelegate = (YesDelegate)Delegate.Combine(this.m_YesDelegate, new YesDelegate(a_deYes.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		this.SetMsgType(type);
		this.m_LbTitle.Text = title;
		this.m_LbNote.SetText(message);
		this.Show();
	}

	public object GetYesObject()
	{
		return this.m_oYesObject;
	}

	public void SetMsgType(eMsgType type)
	{
		bool visible = false;
		bool visible2 = false;
		bool visible3 = false;
		if (type == eMsgType.MB_OK)
		{
			visible = true;
		}
		else if (type == eMsgType.MB_OK_CANCEL)
		{
			visible2 = true;
		}
		else if (type == eMsgType.MB_CHECK_OK)
		{
			visible = true;
			visible3 = true;
		}
		else if (type == eMsgType.MB_CHECK_OK_CANCEL)
		{
			visible2 = true;
			visible3 = true;
		}
		this.m_btnOK.Visible = visible2;
		this.m_btnCancel.Visible = visible2;
		this.m_btnCancel2.Visible = visible;
		this.m_CheckBox.Visible = visible3;
		this.m_LbCheckBox.Visible = visible3;
	}

	private void BtnMsg_OK(IUIObject obj)
	{
		if (this.m_YesDelegate != null)
		{
			this.m_YesDelegate(this.m_oYesObject);
		}
		if (this.m_bOkEventImmediatelyClose)
		{
			this.Close();
		}
	}

	private void BtnMsg_Cancel(IUIObject obj)
	{
		if (this.m_NoDelegate != null)
		{
			this.m_NoDelegate(this.m_oNoObject);
		}
		this.Close();
	}

	private void BtnMsg_Cancel2(IUIObject obj)
	{
		if (this.m_YesDelegate != null)
		{
			this.m_YesDelegate(this.m_oYesObject);
		}
		this.Close();
	}

	public void SetButtonOKText(string strText)
	{
		this.m_btnOK.SetText(strText);
	}

	public void SetButtonCancelText(string strText)
	{
		this.m_btnCancel.SetText(strText);
	}

	public override void OnClose()
	{
		this.m_btnOK.SetText(this.strOKText);
		this.m_btnCancel.SetText(this.strCancelText);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public bool IsChecked()
	{
		return this.m_CheckBox.IsChecked();
	}

	public void SetMessageAnchor(SpriteText.Anchor_Pos _Anchor)
	{
		this.m_LbNote.SetAnchorText(_Anchor);
	}

	public void AddBlackBgClickCloseForm()
	{
		base.BLACK_BG.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public override void Update()
	{
		if (0f < this.m_fAutoCloseTime && this.m_fAutoCloseTime <= Time.time)
		{
			this.m_fAutoCloseTime = 0f;
			this.BtnMsg_Cancel(null);
		}
	}

	public void SetCheckBoxState(bool bCheck)
	{
		if (bCheck)
		{
			this.m_CheckBox.SetToggleState(1);
		}
		else
		{
			this.m_CheckBox.SetToggleState(0);
		}
	}
}
