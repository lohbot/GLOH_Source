using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class ChatNoticeDlg : Form
{
	private float m_fStartTime;

	private float m_fCloseUITime;

	private Label m_kMapName;

	private Queue<NOTIFY_MESSAGE> m_MessageQue = new Queue<NOTIFY_MESSAGE>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Chat/Dlg_BGMinfo", G_ID.CHAT_NOTICE_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
	}

	public override void SetComponent()
	{
		this.m_kMapName = (base.GetControl("LB_MapName") as Label);
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
		if (form != null)
		{
			base.SetLocation(base.GetLocation().x, form.GetLocationY() - 30f);
		}
	}

	public void AddText(string message)
	{
		NOTIFY_MESSAGE nOTIFY_MESSAGE = new NOTIFY_MESSAGE();
		nOTIFY_MESSAGE.message = message;
		this.m_MessageQue.Enqueue(nOTIFY_MESSAGE);
	}

	public void DequeueInfo()
	{
		NOTIFY_MESSAGE nOTIFY_MESSAGE = this.m_MessageQue.Dequeue();
		string message = nOTIFY_MESSAGE.message;
		this.m_kMapName.SetText(message);
		this.m_fStartTime = Time.realtimeSinceStartup;
	}

	public override void Update()
	{
		if (this.m_MessageQue.Count > 0)
		{
			if (this.m_fStartTime <= 0f)
			{
				this.DequeueInfo();
			}
			else if (0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime >= 8f)
			{
				this.DequeueInfo();
			}
		}
		else
		{
			if (0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime >= 8f)
			{
				base.AlphaAni(1f, 0f, 1f);
				this.m_fStartTime = 0f;
				this.m_fCloseUITime = Time.realtimeSinceStartup;
			}
			if (0f < this.m_fCloseUITime && Time.realtimeSinceStartup - this.m_fCloseUITime >= 1f)
			{
				this.Close();
				this.m_fCloseUITime = 0f;
			}
		}
	}
}
