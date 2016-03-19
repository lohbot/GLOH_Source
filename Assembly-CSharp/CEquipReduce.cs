using System;
using UnityForms;

public class CEquipReduce : CNpcUI
{
	public CEquipReduce()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("984");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = string.Empty;
		base.SetUIID(0, G_ID.REDUCEMAIN_DLG);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsTradeCaralyst())
		{
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1970");
			this.m_kMenu[1].strMenu = textFromInterface2;
			this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
			this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
			base.SetUIID(1, G_ID.ITEMCOMPOSE_DLG);
		}
	}

	public override void InitData()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsTradeCaralyst())
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1970");
			this.m_kMenu[1].strMenu = textFromInterface;
			this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
			this.m_kMenu[1].strIconPath = "NPC_I_QuestI11";
			base.SetUIID(1, G_ID.ITEMCOMPOSE_DLG);
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
			if (nSelect == 1)
			{
				form.InitData();
				form.Show();
			}
			else
			{
				form.Show();
			}
		}
	}
}
