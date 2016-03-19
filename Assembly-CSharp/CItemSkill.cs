using System;

public class CItemSkill : CNpcUI
{
	public CItemSkill()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("993");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = string.Empty;
		base.SetUIID(0, G_ID.ITEMSKILL_DLG);
	}

	public void SetData()
	{
	}
}
