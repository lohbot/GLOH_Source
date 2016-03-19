using System;
using UnityForms;

public class CNpcUI
{
	protected G_ID[] m_nUI = new G_ID[6];

	public NPC_UI[] m_kMenu = new NPC_UI[6];

	protected string m_ExceptionTalkTextKey = string.Empty;

	public string m_strDuty = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("417");

	public int m_i32CharKind;

	public bool m_bOpenUI;

	public CNpcUI()
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_nUI[i] = G_ID.NONE;
		}
		this.m_kMenu[0] = new NPC_UI();
		this.m_kMenu[1] = new NPC_UI();
		this.m_kMenu[2] = new NPC_UI();
		this.m_kMenu[3] = new NPC_UI();
		this.m_kMenu[4] = new NPC_UI();
		this.m_kMenu[5] = new NPC_UI();
		this.m_kMenu[0].strMenu = string.Empty;
		this.m_kMenu[1].strMenu = string.Empty;
		this.m_kMenu[2].strMenu = string.Empty;
		this.m_kMenu[3].strMenu = string.Empty;
		this.m_kMenu[4].strMenu = string.Empty;
		this.m_kMenu[5].strMenu = string.Empty;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[2].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[3].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[4].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[5].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[0].strIconPath = "IMG001_004";
		this.m_kMenu[1].strIconPath = string.Empty;
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
	}

	public virtual void InitData()
	{
	}

	public string GetExceptionTalkTextKey()
	{
		return this.m_ExceptionTalkTextKey;
	}

	public void SetUIID(int nNum, G_ID eUI_ID)
	{
		this.m_nUI[nNum] = eUI_ID;
	}

	public G_ID GetUID(int nNum)
	{
		if (0 > nNum || 6 <= nNum)
		{
			return G_ID.NONE;
		}
		return this.m_nUI[nNum];
	}

	public virtual void OpenUI(int nSelect)
	{
		if (0 > nSelect || 6 <= nSelect)
		{
			return;
		}
		if (G_ID.NONE < this.m_nUI[nSelect])
		{
			NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
			if (npcTalkUI_DLG != null)
			{
				npcTalkUI_DLG.Clear();
				npcTalkUI_DLG.Close();
			}
			Form form = NrTSingleton<FormsManager>.Instance.LoadForm(this.m_nUI[nSelect]);
			form.p_nCharKind = this.m_i32CharKind;
			form.Show();
		}
	}

	public virtual void CloseUIALL()
	{
		for (byte b = 0; b < 6; b += 1)
		{
			if (G_ID.NONE < this.m_nUI[(int)b] && NrTSingleton<FormsManager>.Instance.IsForm(this.m_nUI[(int)b]))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(this.m_nUI[(int)b]);
			}
		}
		this.m_bOpenUI = false;
	}

	public virtual bool NpcDo()
	{
		return false;
	}

	public virtual bool MenuCheck(byte bMenuNum)
	{
		return true;
	}
}
