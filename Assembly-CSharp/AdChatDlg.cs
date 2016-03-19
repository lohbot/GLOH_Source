using System;
using UnityForms;

public class AdChatDlg : Form
{
	private const int MAX_LENGTH = 50;

	private TextArea _taAD;

	private Label _lbLength;

	private Button _btOK;

	private Button _btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Chat/DLG_ADChat", G_ID.CHAT_AD_DLG, true);
	}

	public override void SetComponent()
	{
		this._taAD = (base.GetControl("TextArea_TextArea31") as TextArea);
		this._taAD.maxLength = 50;
		TextArea expr_29 = this._taAD;
		expr_29.TextChanged = (EZValueChangedDelegate)Delegate.Combine(expr_29.TextChanged, new EZValueChangedDelegate(this.OnTextChange));
		this._lbLength = (base.GetControl("Label_Label7") as Label);
		this._lbLength.Text = "0/" + 50.ToString();
		this._btOK = (base.GetControl("Button_Button1") as Button);
		this._btOK.Click = new EZValueChangedDelegate(this.OnClickOK);
		this._btCancel = (base.GetControl("Button_Button2") as Button);
		Button expr_C8 = this._btCancel;
		expr_C8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C8.Click, new EZValueChangedDelegate(this.OnClickCancel));
		base.SetScreenCenter();
	}

	private void OnTextChange(IUIObject obj)
	{
		this._lbLength.Text = this._taAD.Text.Length.ToString() + "/" + 50.ToString();
	}

	private void OnClickOK(IUIObject obj)
	{
	}

	private void OnClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
