using System;
using UnityForms;

public class BABELTOWER_USER_CHATINFO
{
	public ItemTexture m_itUserImage0;

	public Label m_lCharName0;

	public int Userkind = -1;

	public void Show(bool show)
	{
		if (!show)
		{
			this.m_lCharName0.Visible = false;
			this.m_itUserImage0.Visible = false;
			this.m_lCharName0.Text = string.Empty;
		}
	}

	public void Init()
	{
		this.m_itUserImage0.Visible = false;
		this.m_lCharName0.Text = string.Empty;
		this.m_lCharName0.Visible = false;
	}
}
