using System;
using UnityEngine;
using UnityForms;

public class ToastMsgDlg : Form
{
	private Label m_LbNote;

	private float m_fCloseTime;

	private float m_fStartTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.TopMost = true;
		Form form = this;
		instance.LoadFile(ref form, "Main/DLG_ToastMsg", G_ID.TOASTMSG_DLG, false);
		instance.CreateControl(ref this.m_LbNote, "Label_Note");
		base.Draggable = false;
		base.SetScreenCenter();
		base.SetLocation(base.GetLocation().x, GUICamera.height - base.GetSize().y - 20f);
		this.m_LbNote.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("23"));
		this.m_fCloseTime = Time.time + 2f;
		this.m_fStartTime = Time.time + 0.5f;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void Update()
	{
		if (Time.time > this.m_fCloseTime)
		{
			this.m_fCloseTime = 0f;
			this.Close();
		}
	}

	public void SetMessage(string Message)
	{
		this.m_LbNote.SetText(Message);
	}

	public bool IsQuitGame()
	{
		return this.m_fStartTime < Time.time;
	}
}
