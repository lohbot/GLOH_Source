using System;
using UnityForms;

public class LoginDlg : Form
{
	private TextField pkTextFieldUserID;

	private TextField pkTextFieldPassWD;

	private Button pkButtonLogin;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Login/DLG_Login", G_ID.LOGIN_DLG, false);
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
		NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTSERVER_DLG).Show();
		this.Hide();
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
