using System;

public class CItemSkill : CNpcUI
{
	public CItemSkill()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsItemNormalSkillBlock())
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("993");
			this.m_kMenu[0].strMenu = textFromInterface;
			this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
			this.m_kMenu[0].strIconPath = string.Empty;
			base.SetUIID(0, G_ID.ITEMSKILL_DLG);
		}
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2972");
		this.m_kMenu[1].strMenu = textFromInterface2;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
		base.SetUIID(1, G_ID.ITEMSKILL_DLG);
	}

	public void SetData()
	{
	}
}
