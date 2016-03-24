using System;
using UnityForms;

public class MobileLoginDlg : Form
{
	private TextField pkTextFieldUserID;

	private TextField pkTextFieldPassWD;

	private Button pkButtonLogin;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Login/DLG_MobileLogin", G_ID.MOBILELOGIN_DLG, false);
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.LOGINRATING_DLG);
	}

	public void AAA(IUIObject ojb)
	{
		this.pkTextFieldUserID.Text = string.Empty;
	}

	public override void SetComponent()
	{
		this.pkTextFieldUserID = (base.GetControl("TextField_TextField2") as TextField);
		this.pkTextFieldPassWD = (base.GetControl("TextField_TextField3") as TextField);
		this.pkTextFieldPassWD.Password = true;
		this.pkButtonLogin = (base.GetControl("Button_Button4") as Button);
		Button expr_54 = this.pkButtonLogin;
		expr_54.Click = (EZValueChangedDelegate)Delegate.Combine(expr_54.Click, new EZValueChangedDelegate(this.LoginButton_Click));
		base.SetScreenCenter();
	}

	private void LoginButton_Click(IUIObject obj)
	{
		this.Hide();
		NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTSERVER_DLG).Show();
	}

	private void test(EZAnimation ainm)
	{
	}

	public void SetUserID(string userid)
	{
		this.pkTextFieldUserID.Text = userid;
	}

	public string GetUserID()
	{
		return this.pkTextFieldUserID.Text.Trim();
	}

	public void SetPassWD(string passwd)
	{
		this.pkTextFieldPassWD.Text = passwd;
	}

	public string GetPassWD()
	{
		return this.pkTextFieldPassWD.Text;
	}
}
