using System;
using UnityForms;

public class CItemRepair : CNpcUI
{
	public CItemRepair()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("421");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_COMMON;
		this.m_kMenu[0].strIconPath = string.Empty;
		base.SetUIID(0, G_ID.REDUCEMAIN_DLG);
	}

	public override void InitData()
	{
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
			form.p_nSelectIndex = 100;
			form.Show();
		}
	}
}
