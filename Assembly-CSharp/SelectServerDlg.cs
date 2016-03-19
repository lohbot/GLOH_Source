using System;
using UnityForms;

public class SelectServerDlg : Form
{
	public ListBox pkListBox;

	public Button requestLogin;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Login/DLG_SelectServer", G_ID.SELECTSERVER_DLG, false);
	}

	public override void SetComponent()
	{
		this.pkListBox = (base.GetControl("ListBox_ListBox12") as ListBox);
		this.pkListBox.LineHeight = 50f;
		this.requestLogin = (base.GetControl("Button_Button10_C") as Button);
	}

	public void ClickList(IUIObject obj)
	{
	}

	public override void Show()
	{
		base.Show();
		base.SetScreenCenter();
	}
}
