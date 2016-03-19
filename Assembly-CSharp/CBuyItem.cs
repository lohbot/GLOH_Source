using GAME;
using System;

public class CBuyItem : CNpcUI
{
	public CBuyItem()
	{
		this.m_kMenu[0] = new NPC_UI();
		this.m_kMenu[1] = new NPC_UI();
		this.m_kMenu[2] = new NPC_UI();
		this.m_kMenu[3] = new NPC_UI();
		this.m_kMenu[4] = new NPC_UI();
		this.m_kMenu[5] = new NPC_UI();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2251");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2257");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[1].strMenu = textFromInterface2;
		this.m_kMenu[2].strMenu = string.Empty;
		this.m_kMenu[3].strMenu = string.Empty;
		this.m_kMenu[4].strMenu = string.Empty;
		this.m_kMenu[5].strMenu = string.Empty;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[2].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[3].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[4].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[5].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[0].strIconPath = "NPC_I_QuestI11";
		this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_POINT_EXCHANGE_LIMIT);
			this.m_kMenu[0].nLimintLevel = (short)value;
			this.m_kMenu[1].nLimintLevel = (short)value;
			this.m_kMenu[2].nLimintLevel = (short)value;
			this.m_kMenu[3].nLimintLevel = (short)value;
			this.m_kMenu[4].nLimintLevel = (short)value;
			this.m_kMenu[5].nLimintLevel = (short)value;
			base.SetUIID(0, G_ID.EXCHANGE_POINT_DLG);
			base.SetUIID(1, G_ID.EXCHANGE_ITEM_DLG);
		}
	}
}
