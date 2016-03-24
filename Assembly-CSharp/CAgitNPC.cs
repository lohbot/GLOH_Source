using GAME;
using System;

public class CAgitNPC : CNpcUI
{
	public CAgitNPC(int i32CharKind)
	{
		this.m_i32CharKind = i32CharKind;
		string strMenu = string.Empty;
		NewGuildDefine.eNEWGUILD_NPC_TYPE agitNPCTypeFromCharKind = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCTypeFromCharKind(this.m_i32CharKind);
		switch (agitNPCTypeFromCharKind)
		{
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_MERCHANT:
			strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2723");
			this.m_ExceptionTalkTextKey = "20450";
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_REFORGE:
			strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("98");
			this.m_ExceptionTalkTextKey = "20451";
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_REDUCE:
			strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("984");
			this.m_ExceptionTalkTextKey = "20452";
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_ITEMSKILL:
			strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("993");
			this.m_ExceptionTalkTextKey = "20453";
			if (NrTSingleton<ContentsLimitManager>.Instance.IsItemNormalSkillBlock())
			{
				return;
			}
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_GOLDENEGG:
			strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2756");
			this.m_ExceptionTalkTextKey = "20454";
			break;
		}
		this.m_kMenu[0].strMenu = strMenu;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = string.Empty;
		switch (agitNPCTypeFromCharKind)
		{
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_MERCHANT:
			base.SetUIID(0, G_ID.AGIT_MERCHANT_DLG);
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_REFORGE:
			base.SetUIID(0, G_ID.REFORGEMAIN_DLG);
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_REDUCE:
			base.SetUIID(0, G_ID.REDUCEMAIN_DLG);
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_ITEMSKILL:
			base.SetUIID(0, G_ID.ITEMSKILL_DLG);
			break;
		case NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_GOLDENEGG:
			base.SetUIID(0, G_ID.AGIT_GOLDENEGG_DLG);
			break;
		}
	}

	public void SetData()
	{
	}

	public override void OpenUI(int nSelect)
	{
		if (0 > nSelect || 6 <= nSelect)
		{
			return;
		}
		if (this.m_i32CharKind == NrTSingleton<NewGuildManager>.Instance.GetAgitNPCCharKindFromNPCType(1))
		{
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_MERCHANT_INFO_REQ();
		}
		base.OpenUI(nSelect);
	}
}
