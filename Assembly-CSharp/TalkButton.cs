using System;
using UnityForms;

public class TalkButton
{
	public Button m_NPCTalk_menutitle;

	public Label m_Label_menutitle_font;

	public DrawTexture m_NPCTalk_menuicon;

	public void Show(bool bShow)
	{
		this.m_NPCTalk_menutitle.Visible = bShow;
		this.m_Label_menutitle_font.Visible = bShow;
		this.m_NPCTalk_menuicon.Visible = bShow;
	}
}
