using System;
using UnityForms;

public class CGuildOpen : CNpcUI
{
	public CGuildOpen()
	{
		this.m_kMenu[0] = new NPC_UI();
		this.m_kMenu[1] = new NPC_UI();
		this.m_kMenu[2] = new NPC_UI();
		this.m_kMenu[3] = new NPC_UI();
		this.m_kMenu[4] = new NPC_UI();
		this.m_kMenu[5] = new NPC_UI();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1824");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[1].strMenu = string.Empty;
		this.m_kMenu[2].strMenu = string.Empty;
		this.m_kMenu[3].strMenu = string.Empty;
		this.m_kMenu[4].strMenu = string.Empty;
		this.m_kMenu[5].strMenu = string.Empty;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[2].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[3].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[4].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[5].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[0].strIconPath = string.Empty;
		this.m_kMenu[1].strIconPath = string.Empty;
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
		base.SetUIID(0, G_ID.GUILD_LIST_DLG);
	}

	public void SetData()
	{
	}

	public override void OpenUI(int nSelect)
	{
		NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
		if (npcTalkUI_DLG != null)
		{
			npcTalkUI_DLG.Clear();
			npcTalkUI_DLG.Close();
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUILD_LIST_DLG);
	}
}
