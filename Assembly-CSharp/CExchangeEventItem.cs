using System;

public class CExchangeEventItem : CNpcUI
{
	public CExchangeEventItem()
	{
		this.m_kMenu[0] = new NPC_UI();
		this.m_kMenu[1] = new NPC_UI();
		this.m_kMenu[2] = new NPC_UI();
		this.m_kMenu[3] = new NPC_UI();
		this.m_kMenu[4] = new NPC_UI();
		this.m_kMenu[5] = new NPC_UI();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3479");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = "NPC_I_QuestI11";
		base.SetUIID(0, G_ID.EXCHANGE_EVENTITEM_DLG);
		this.m_kMenu[1].strMenu = string.Empty;
		this.m_kMenu[2].strMenu = string.Empty;
		this.m_kMenu[3].strMenu = string.Empty;
		this.m_kMenu[4].strMenu = string.Empty;
		this.m_kMenu[5].strMenu = string.Empty;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[2].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[3].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[4].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[5].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[1].strIconPath = string.Empty;
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
		int num = 0;
		this.m_kMenu[0].nLimintLevel = (short)num;
		this.m_kMenu[1].nLimintLevel = (short)num;
		this.m_kMenu[2].nLimintLevel = (short)num;
		this.m_kMenu[3].nLimintLevel = (short)num;
		this.m_kMenu[4].nLimintLevel = (short)num;
		this.m_kMenu[5].nLimintLevel = (short)num;
	}
}
