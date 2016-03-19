using System;
using System.Collections;
using UnityForms;

public class CIndunOpen : CNpcUI
{
	public int[] m_nIndunIDX = new int[6];

	public CIndunOpen()
	{
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
		this.m_kMenu[0].strIconPath = string.Empty;
		this.m_kMenu[1].strIconPath = string.Empty;
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
		this.m_nIndunIDX[0] = -1;
		this.m_nIndunIDX[1] = -1;
		this.m_nIndunIDX[2] = -1;
		this.m_nIndunIDX[3] = -1;
		this.m_nIndunIDX[4] = -1;
		this.m_nIndunIDX[5] = -1;
	}

	public void SetData()
	{
		int num = 0;
		ICollection indunInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetIndunInfo_Col();
		foreach (INDUN_INFO iNDUN_INFO in indunInfo_Col)
		{
			if (iNDUN_INFO.m_nNpcCode == this.m_i32CharKind)
			{
				this.m_kMenu[num].strMenu = NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(iNDUN_INFO.szTextKey);
				this.m_nIndunIDX[num] = iNDUN_INFO.m_nIndunIDX;
				this.m_kMenu[num].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
				this.m_kMenu[0].strIconPath = "Main_I_ForceIcon";
				num++;
				if (num >= 6)
				{
					break;
				}
			}
		}
	}

	public override void OpenUI(int nSelect)
	{
		short nNpcCharUnique = 0;
		NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
		if (npcTalkUI_DLG != null)
		{
			nNpcCharUnique = npcTalkUI_DLG.NPCCharUnique;
			npcTalkUI_DLG.Clear();
			npcTalkUI_DLG.Close();
		}
		IndunEnterScenario_DLG indunEnterScenario_DLG = (IndunEnterScenario_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INDUN_ENTER_SCENARIO_DLG);
		indunEnterScenario_DLG.Set(this.m_nIndunIDX[nSelect], nNpcCharUnique);
	}
}
