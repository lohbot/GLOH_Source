using System;
using UnityEngine;
using UnityForms;

internal class Wait_Dlg : Form
{
	private DrawTexture m_waitImg;

	private bool m_bAutoMode;

	private float m_fTime;

	public bool AutoMode
	{
		set
		{
			this.m_bAutoMode = true;
			this.m_fTime = Time.realtimeSinceStartup;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "System/dlg_wait", G_ID.WAIT_DLG, false, true);
		base.SetSize(GUICamera.width, GUICamera.height);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_waitImg = (base.GetControl("DrawTexture_Loading") as DrawTexture);
		this.m_waitImg.SetLocation(GUICamera.width / 2f - this.m_waitImg.width / 2f, GUICamera.height / 2f - this.m_waitImg.height / 2f);
	}

	public override void Update()
	{
		this.m_waitImg.Rotate(5f);
		if (this.m_bAutoMode && Time.realtimeSinceStartup - this.m_fTime >= 5f)
		{
			this.Close();
		}
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			base.SetLocation(base.GetLocation().x, base.GetLocationY(), itemMallDlg.GetLocation().z - 4f);
		}
	}
}
