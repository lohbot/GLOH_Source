using System;
using UnityForms;

public class AutoMove_DLG : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/DLG_Automove", G_ID.MAIN_UI_AUTO_MOVE, false);
		this.SetDlgPos();
	}

	public override void SetComponent()
	{
	}

	public override void InitData()
	{
	}

	public void SetDlgPos()
	{
		float x = GUICamera.width / 2f - base.GetSize().x / 2f;
		float y = GUICamera.height / 2f * 0.3f;
		base.SetLocation(x, y);
	}
}
