using System;
using UnityForms;

public class VipInfoDlg : Form
{
	private const int MIN_SLOT_COUNT = 0;

	private const int MAX_SLOT_COUNT = 5;

	private Label m_LabelTitle;

	private Label m_LabelNote;

	private Button m_ButtonLeft;

	private Button m_ButtonRight;

	private Button m_ButtonCancel;

	private byte m_nTextIndex;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Message/DLG_VIPinfo", G_ID.VIPINFO_DLG, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_LabelTitle = (base.GetControl("Label_title") as Label);
		this.m_LabelNote = (base.GetControl("Label_Note") as Label);
		this.m_ButtonLeft = (base.GetControl("Button_left") as Button);
		this.m_ButtonRight = (base.GetControl("Button_right") as Button);
		this.m_ButtonCancel = (base.GetControl("Button_cancel") as Button);
		this.m_ButtonLeft.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtLeft));
		this.m_ButtonRight.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtRight));
		this.m_ButtonCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtCancel));
		string text = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2849");
		this.m_LabelTitle.SetText(text);
		base.SetScreenCenter();
	}

	public void OnClickBtLeft(IUIObject obj)
	{
		if (this.m_nTextIndex <= 0)
		{
			return;
		}
		this.m_nTextIndex -= 1;
		this.Show();
	}

	public void OnClickBtRight(IUIObject obj)
	{
		if (this.m_nTextIndex >= 5)
		{
			return;
		}
		this.m_nTextIndex += 1;
		this.Show();
	}

	public void OnClickBtCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.VIPINFO_DLG);
	}

	public void SetLevel(byte i8Level)
	{
		if (i8Level < 0 || i8Level > 5)
		{
			this.m_nTextIndex = 0;
		}
		else
		{
			this.m_nTextIndex = i8Level;
		}
		this.Show();
	}

	public override void Show()
	{
		base.Show();
		if (this.m_nTextIndex < 0)
		{
			return;
		}
		string text = string.Empty;
		switch (this.m_nTextIndex)
		{
		case 0:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("266");
			break;
		case 1:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("267");
			break;
		case 2:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("268");
			break;
		case 3:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("269");
			break;
		case 4:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("270");
			break;
		case 5:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("271");
			break;
		}
		this.m_LabelNote.SetText(text);
	}
}
