using System;
using UnityForms;

public class LoginBGDlg : Form
{
	public ListBox pkListBox;

	private Label pkText;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Login/DLG_LoginBG", G_ID.LOGINBG_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		this.pkListBox = (base.GetControl("ListBox_TEST") as ListBox);
		if (!TsPlatform.IsMobile)
		{
			this.pkListBox.LineHeight = 28f;
		}
		else
		{
			this.pkListBox.LineHeight = 38f;
		}
		this.pkListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.test));
		this.pkText = (base.GetControl("Label_Copyright") as Label);
		this.pkText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("37");
		this.pkText.SetSize(this.pkText.spriteText.TotalWidth, this.pkText.spriteText.TotalHeight);
		this.pkText.SetLocation((GUICamera.width - this.pkText.width) / 2f, GUICamera.height - 10f - this.pkText.height);
	}

	public void test(IUIObject obj)
	{
	}
}
