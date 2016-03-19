using System;
using UnityForms;

internal class Wait_Dlg : Form
{
	private DrawTexture m_waitImg;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "System/dlg_wait", G_ID.WAIT_DLG, false);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_waitImg = (base.GetControl("DrawTexture_Loading") as DrawTexture);
	}

	public override void Update()
	{
		this.m_waitImg.Rotate(5f);
	}
}
