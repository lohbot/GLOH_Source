using System;
using UnityForms;

public class MYTHRAID_USER_CONTROL_INFO
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

	public Button btn_Guardian_Select;

	public Button btn_Guardian_Reselect;

	public DrawTexture dt_GuardianImg;

	public DrawTexture dt_GuardianImg_Frame;

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
		if (this.btn_Guardian_Select != null)
		{
			this.btn_Guardian_Select.Visible = show;
		}
		if (this.btn_Guardian_Reselect != null)
		{
			this.btn_Guardian_Reselect.Visible = show;
		}
		if (this.dt_GuardianImg != null)
		{
			this.dt_GuardianImg.Visible = show;
		}
		if (this.dt_GuardianImg_Frame != null)
		{
			this.dt_GuardianImg_Frame.Visible = show;
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
		if (this.btn_Guardian_Select != null)
		{
			this.btn_Guardian_Select.Visible = false;
		}
		if (this.btn_Guardian_Reselect != null)
		{
			this.btn_Guardian_Reselect.Visible = false;
		}
		if (this.dt_GuardianImg != null)
		{
			this.dt_GuardianImg.Visible = false;
		}
		if (this.dt_GuardianImg_Frame != null)
		{
			this.dt_GuardianImg_Frame.Visible = false;
		}
	}
}
