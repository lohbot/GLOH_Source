using System;
using UnityForms;

public class ReCommendDlg : Form
{
	private Label m_Label_NoteRecommend1;

	private Label m_Label_NoteRecommend2;

	private TextField m_TextField_CharName;

	private Button m_Button_Add;

	private Button m_Button_Cancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Community/DLG_Community_Recommend", G_ID.RECOMMEND_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_Label_NoteRecommend1 = (base.GetControl("Label_note2") as Label);
		this.m_Label_NoteRecommend2 = (base.GetControl("Label_note3") as Label);
		this.m_TextField_CharName = (base.GetControl("TextField_charname") as TextField);
		this.m_TextField_CharName.Text = string.Empty;
		this.m_Button_Add = (base.GetControl("Button_add") as Button);
		this.m_Button_Cancel = (base.GetControl("Button_cancel") as Button);
		this.m_TextField_CharName.AddValueChangedDelegate(new EZValueChangedDelegate(this.TextFieldChange));
		this.m_Button_Add.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickAdd));
		this.m_Button_Cancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickCancel));
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.ReCommendDlgGuiSet();
	}

	private void ReCommendDlgGuiSet()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2163");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			myCharInfo.Recommend_RecvCurrnetCount.ToString(),
			"count2",
			myCharInfo.Recommend_RecvMaxCount.ToString()
		});
		this.m_Label_NoteRecommend1.SetText(textFromInterface);
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2164");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			myCharInfo.Recommend_SendCurrentCount.ToString(),
			"count2",
			myCharInfo.Recommend_SendMaxCount.ToString()
		});
		this.m_Label_NoteRecommend2.SetText(textFromInterface);
	}

	public void RefreshDlgGuiSet()
	{
		this.ReCommendDlgGuiSet();
	}

	private void TextFieldChange(IUIObject obj)
	{
	}

	public void On_ClickAdd(IUIObject a_cObject)
	{
	}

	public void On_ClickCancel(IUIObject a_cObject)
	{
		if (a_cObject == null)
		{
			return;
		}
		base.CloseForm(a_cObject);
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
	}
}
