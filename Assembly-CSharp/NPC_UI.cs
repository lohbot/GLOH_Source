using System;

public class NPC_UI
{
	public enum E_NPC_UI_TYPE
	{
		E_NPC_UI_TYPE_NONE,
		E_NPC_UI_TYPE_COMMON,
		E_NPC_UI_TYPE_NONE_POPUP,
		E_NPC_UI_TYPE_QUEST,
		E_NPC_UI_TYPE_QUEST_VICTORYSCENARIOBATTLE,
		E_NPC_UI_TYPE_QUEST_SUB,
		E_NPC_UI_TYPE_SCENARIOBATTLE
	}

	public string strMenu = string.Empty;

	public NPC_UI.E_NPC_UI_TYPE byMenuType;

	public string strIconPath = string.Empty;

	public byte bNpcTalkType;

	public short nLimintLevel;
}
