using System;
using UnityForms;

public class StopAutoMove : Form
{
	private Label m_TitleLabel;

	private DrawTexture m_DrawTexture_background;

	private DrawTexture m_DrawTexture_DrawTexture8;

	private Label m_Label_Label9;

	private Button m_Button_Button1;

	private Button m_Button_Button2;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFile(ref form, "DLG_StopAutoMove", G_ID.DLG_STOPAUTOMOVE, true);
		instance.CreateControl(ref this.m_TitleLabel, "Label_title");
		instance.CreateControl(ref this.m_DrawTexture_background, "DrawTexture_background");
		instance.CreateControl(ref this.m_DrawTexture_DrawTexture8, "DrawTexture_DrawTexture8");
		instance.CreateControl(ref this.m_Label_Label9, "Label_text1");
		instance.CreateControl(ref this.m_Button_Button1, "Button_1");
		instance.CreateControl(ref this.m_Button_Button2, "Button_2");
		Button expr_95 = this.m_Button_Button1;
		expr_95.Click = (EZValueChangedDelegate)Delegate.Combine(expr_95.Click, new EZValueChangedDelegate(this.BtnClickButton1));
		Button expr_BC = this.m_Button_Button2;
		expr_BC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_BC.Click, new EZValueChangedDelegate(this.BtnClickButton2));
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)@char;
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("119");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromMessageBox,
				"username",
				nrCharUser.GetFollowCharName()
			});
			this.m_Label_Label9.SetText(empty);
		}
		base.SetScreenCenter();
	}

	private void BtnClickButton1(IUIObject obj)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)@char;
			nrCharUser.SetFollowCharPersonID(0L, string.Empty);
			nrCharUser.m_kCharMove.MoveStop(true, false);
		}
		this.Close();
	}

	private void BtnClickButton2(IUIObject obj)
	{
		this.Close();
	}
}
