using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class CCharChange : CNpcUI
{
	public CCharChange()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("951");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = string.Empty;
		base.SetUIID(0, G_ID.CHARCHANGEMAIN_DLG);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsAwakeningUse())
		{
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2288");
			this.m_kMenu[1].strMenu = textFromInterface2;
			this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
			this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
			base.SetUIID(1, G_ID.SOLAWAKENING_DLG);
		}
	}

	public override void InitData()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsAwakeningUse())
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2288");
			this.m_kMenu[1].strMenu = textFromInterface;
			this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
			this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
			base.SetUIID(1, G_ID.SOLAWAKENING_DLG);
		}
		else
		{
			this.m_kMenu[1].strMenu = string.Empty;
			this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
			this.m_kMenu[1].strIconPath = string.Empty;
			base.SetUIID(1, G_ID.NONE);
		}
	}

	public void SetData()
	{
	}

	public override void OpenUI(int nSelect)
	{
		if (this.m_nUI[nSelect] == G_ID.SOLAWAKENING_DLG)
		{
			GS_SOLAWAKENING_INFO_REQ obj = new GS_SOLAWAKENING_INFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLAWAKENING_INFO_REQ, obj);
			NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
			if (npcTalkUI_DLG != null)
			{
				npcTalkUI_DLG.Clear();
				npcTalkUI_DLG.Close();
			}
		}
		else
		{
			base.OpenUI(nSelect);
		}
	}
}
