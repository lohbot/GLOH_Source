using System;
using UnityForms;

public class BABEL_USER_CONTROL_INFO
{
	public long PersonID;

	public bool bSlotClose;

	public Label m_laUserName;

	public Label m_laUserSolNum;

	public Button m_btKickUser;

	public DrawTexture m_dtReady;

	public Label m_laReady;

	public Label m_laSlotState;

	public DrawTexture m_dLoadingImg;

	public void Show(bool show)
	{
		if (!show)
		{
			this.PersonID = 0L;
			this.m_laUserName.Text = string.Empty;
			this.m_laUserSolNum.Text = string.Empty;
			this.m_laReady.Text = string.Empty;
		}
		if (this.m_btKickUser != null)
		{
			this.m_btKickUser.Visible = show;
		}
		if (this.m_dtReady != null)
		{
			this.m_dtReady.Visible = show;
		}
	}

	public void SetLoadingState(bool show)
	{
		if (this.m_dLoadingImg == null)
		{
			return;
		}
		if (!show)
		{
			this.m_dLoadingImg.Visible = false;
		}
		else
		{
			this.m_laSlotState.Visible = true;
		}
	}

	public void InitLoadingState()
	{
		this.m_dLoadingImg.Visible = false;
	}

	public void SetSlotState(bool show, int slot_type)
	{
		if (this.m_laSlotState == null)
		{
			return;
		}
		if (!show)
		{
			this.m_laSlotState.SetText(string.Empty);
			this.m_laSlotState.Visible = false;
		}
		else
		{
			this.m_laSlotState.Visible = true;
			string text = string.Empty;
			if (slot_type == 1)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("899");
			}
			else if (slot_type == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("907");
			}
			else if (slot_type == 0)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("908");
			}
			this.m_laSlotState.SetText(text);
		}
	}

	public void Init()
	{
		this.PersonID = 0L;
		this.bSlotClose = false;
		this.m_laUserName.Text = string.Empty;
		this.m_laUserSolNum.Text = string.Empty;
		if (this.m_laSlotState != null)
		{
			this.m_laSlotState.Text = string.Empty;
		}
		if (this.m_laReady != null)
		{
			this.m_laReady.Text = string.Empty;
		}
		if (this.m_btKickUser != null)
		{
			this.m_btKickUser.Visible = false;
		}
		if (this.m_dtReady != null)
		{
			this.m_dtReady.Visible = false;
		}
	}
}
