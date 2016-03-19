using System;
using UnityForms;

public class IntroMsgBoxDlg : Form
{
	private Action<IntroMsgBoxDlg, object> m_deYes;

	private Action<IntroMsgBoxDlg, object> m_deNo;

	private object m_oYesObject;

	private object m_oNoObject;

	private Label m_Label_title;

	private Label m_Label_Note;

	private Button m_Button_ok;

	private Button m_Button_cancel;

	private Button m_Button_ok1;

	private bool m_bClose;

	public void SetClose(bool bShow)
	{
		this.m_bClose = bShow;
	}

	public bool GetClose()
	{
		return this.m_bClose;
	}

	public override void InitializeComponent()
	{
		TsLog.Assert(TsPlatform.IsMobile, "TsPlatform.IsMobile Only!", new object[0]);
		this.m_deYes = null;
		this.m_deNo = null;
		this.m_oYesObject = null;
		this.m_oNoObject = null;
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFile(ref form, "Message/DLG_IntroMsgBox", G_ID.INTROMSGBOX_DLG, true);
		instance.CreateControl(ref this.m_Label_title, "Label_title");
		instance.CreateControl(ref this.m_Label_Note, "Label_Note");
		instance.CreateControl(ref this.m_Button_ok, "Button_ok");
		instance.CreateControl(ref this.m_Button_ok1, "Button_ok1");
		instance.CreateControl(ref this.m_Button_cancel, "Button_cancel");
		this.m_Button_ok.Click = new EZValueChangedDelegate(this.BtnMsg_OK);
		this.m_Button_cancel.Click = new EZValueChangedDelegate(this.BtnMsg_Cancel);
		this.m_Button_ok1.Click = new EZValueChangedDelegate(this.BtnMsg_OK2);
		base.ChangeSceneDestory = false;
		base.SetLocation(0f, 0f);
		base.Draggable = false;
		base.SetScreenCenter();
	}

	public override void Update()
	{
		if (this.m_bClose)
		{
			this.Hide();
		}
	}

	public void SetMsg(Action<IntroMsgBoxDlg, object> a_deYes, object a_oObject, Action<IntroMsgBoxDlg, object> b_deNo, object b_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_deYes = new Action<IntroMsgBoxDlg, object>(a_deYes.Invoke);
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		if (b_deNo != null)
		{
			this.m_deNo = new Action<IntroMsgBoxDlg, object>(b_deNo.Invoke);
		}
		if (b_oObject != null)
		{
			this.m_oNoObject = b_oObject;
		}
		this.SetMsgType(type);
		this.m_Label_title.Text = title;
		this.m_Label_Note.Text = message;
		this.Show();
	}

	public void SetMsg(Action<IntroMsgBoxDlg, object> a_deYes, object a_oObject, string title, string message, eMsgType type)
	{
		if (a_deYes != null)
		{
			this.m_deYes = new Action<IntroMsgBoxDlg, object>(a_deYes.Invoke);
		}
		if (a_oObject != null)
		{
			this.m_oYesObject = a_oObject;
		}
		this.SetMsgType(type);
		this.m_Label_title.Text = title;
		this.m_Label_Note.Text = message;
		this.Show();
	}

	public void SetMsgType(eMsgType type)
	{
		bool visible = false;
		bool visible2 = false;
		if (type == eMsgType.MB_OK)
		{
			visible = true;
		}
		else if (type == eMsgType.MB_OK_CANCEL)
		{
			visible2 = true;
		}
		else
		{
			visible = true;
		}
		this.m_Button_ok.Visible = visible2;
		this.m_Button_cancel.Visible = visible2;
		this.m_Button_ok1.Visible = visible;
	}

	private void BtnMsg_OK(IUIObject obj)
	{
		this.m_bClose = true;
		if (this.m_deYes != null)
		{
			this.m_deYes(this, this.m_oYesObject);
		}
	}

	private void BtnMsg_Cancel(IUIObject obj)
	{
		this.m_bClose = true;
		if (this.m_deNo != null)
		{
			this.m_deNo(this, this.m_oNoObject);
		}
	}

	private void BtnMsg_OK2(IUIObject obj)
	{
		this.m_bClose = true;
		if (this.m_deYes != null)
		{
			this.m_deYes(this, this.m_oYesObject);
		}
	}

	public void SetBtnChangeName(string ok, string cancel)
	{
		this.m_Button_ok.Text = ok;
		this.m_Button_cancel.Text = cancel;
	}

	public void SetBtnChangeName(string ok)
	{
		this.m_Button_ok1.Text = ok;
	}
}
