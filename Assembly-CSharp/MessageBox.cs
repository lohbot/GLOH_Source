using System;
using UnityForms;

internal class MessageBox
{
	public enum Type
	{
		OK,
		OKCANCEL
	}

	public enum Result
	{
		NONE = -2,
		IDLE,
		CANCEL,
		OK
	}

	private MessageBox.Result _Result = MessageBox.Result.NONE;

	public string Title
	{
		get;
		set;
	}

	public string Message
	{
		get;
		set;
	}

	public string OK
	{
		get;
		set;
	}

	public string CANCEL
	{
		get;
		set;
	}

	public MessageBox.Type MBType
	{
		get;
		set;
	}

	public MessageBox()
	{
		this.Title = "Set Title!";
		this.Message = "Set Message!";
		this.OK = "OK";
		this.CANCEL = "CANCEL";
		this.MBType = MessageBox.Type.OK;
	}

	public static void Show<T>() where T : MessageBox, new()
	{
		T t = Activator.CreateInstance<T>();
		t.Show();
	}

	public MessageBox.Result Show(string msg, string title, MessageBox.Type eType)
	{
		this.Message = msg;
		this.Title = title;
		this.MBType = eType;
		return this.Show();
	}

	public virtual MessageBox.Result Show()
	{
		IntroMsgBoxDlg introMsgBoxDlg = (IntroMsgBoxDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INTROMSGBOX_DLG);
		if (introMsgBoxDlg != null)
		{
			introMsgBoxDlg.SetClose(false);
			this._Result = MessageBox.Result.IDLE;
			if (this.MBType == MessageBox.Type.OK)
			{
				introMsgBoxDlg.SetBtnChangeName(this.OK);
				introMsgBoxDlg.SetMsg(new Action<IntroMsgBoxDlg, object>(this._onOK), this, this.Title, this.Message, eMsgType.MB_OK);
			}
			else if (this.MBType == MessageBox.Type.OKCANCEL)
			{
				introMsgBoxDlg.SetBtnChangeName(this.OK, this.CANCEL);
				introMsgBoxDlg.SetMsg(new Action<IntroMsgBoxDlg, object>(this._onOK), this, new Action<IntroMsgBoxDlg, object>(this._onCancel), this, this.Title, this.Message, eMsgType.MB_OK_CANCEL);
			}
			int num = (int)(GUICamera.width / 2f - introMsgBoxDlg.GetSize().x / 2f);
			int num2 = (int)(GUICamera.height / 2f - introMsgBoxDlg.GetSize().y / 2f);
			introMsgBoxDlg.SetLocation((float)num, (float)num2, 50f);
		}
		return this._Result;
	}

	private void _onOK(IntroMsgBoxDlg msgBox, object arg)
	{
		this._Result = MessageBox.Result.OK;
		this.OnOK();
	}

	private void _onCancel(IntroMsgBoxDlg msgBox, object arg)
	{
		this._Result = MessageBox.Result.CANCEL;
		this.OnCancel();
	}

	public virtual void OnOK()
	{
	}

	public virtual void OnCancel()
	{
	}
}
