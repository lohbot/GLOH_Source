using System;
using UnityForms;

public class Main_UI_FollowChar : Form
{
	private Label m_Label_Text;

	private DrawTexture m_DrawTexture_Bg;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFile(ref form, "DLG_FollowChar", G_ID.DLG_FOLLOWCHAR, false);
		instance.CreateControl(ref this.m_DrawTexture_Bg, "DrawTexture_Bg");
		instance.CreateControl(ref this.m_Label_Text, "Label_Text");
		this.m_Label_Text.Text = string.Empty;
		this.m_DrawTexture_Bg.SetAlpha(0.5f);
		this.ChangeSize(GUICamera.width, GUICamera.height);
		base.Draggable = false;
		base.CheckMouseEvent = false;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1540");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"username",
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_FollowCharName
		});
		this.SetMessage(empty);
	}

	public void ChangeSize(float screenwidth, float screenheight)
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
		if (form != null)
		{
			base.SetLocation(form.GetLocation().x, form.GetLocationY() - form.GetSizeY() + base.GetSize().y);
		}
	}

	public void SetMessage(string msg)
	{
		this.m_Label_Text.Text = msg;
	}
}
