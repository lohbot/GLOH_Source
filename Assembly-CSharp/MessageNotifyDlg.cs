using System;
using UnityForms;

public class MessageNotifyDlg : Form
{
	private Label Label_notify;

	private int openTime;

	private int closeDelay = 3000;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Message/DLG_messagenotify", G_ID.MESSAGE_NOTIFY_DLG, false);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.Label_notify = (base.GetControl("Label_notify") as Label);
		this.openTime = Environment.TickCount;
	}

	public void SetMessage(string message)
	{
		this.Label_notify.SetText(message);
	}

	public override void Update()
	{
		if (this.openTime + this.closeDelay < Environment.TickCount)
		{
			this.Close();
		}
	}
}
