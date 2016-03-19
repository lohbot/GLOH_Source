using System;
using TsBundle;
using UnityForms;

public class CBattleCustodian : CNpcUI
{
	public CBattleCustodian()
	{
		this.m_kMenu[0] = new NPC_UI();
		this.m_kMenu[1] = new NPC_UI();
		this.m_kMenu[2] = new NPC_UI();
		this.m_kMenu[3] = new NPC_UI();
		this.m_kMenu[4] = new NPC_UI();
		this.m_kMenu[5] = new NPC_UI();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("607");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("614");
		this.m_kMenu[0].strMenu = textFromInterface;
		this.m_kMenu[1].strMenu = textFromInterface2;
		this.m_kMenu[2].strMenu = string.Empty;
		this.m_kMenu[3].strMenu = string.Empty;
		this.m_kMenu[4].strMenu = string.Empty;
		this.m_kMenu[5].strMenu = string.Empty;
		this.m_kMenu[0].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_SCENARIOBATTLE;
		this.m_kMenu[1].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_SCENARIOBATTLE;
		this.m_kMenu[2].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[3].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[4].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[5].byMenuType = NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_NONE;
		this.m_kMenu[0].strIconPath = "Bat_I_TacticListI";
		this.m_kMenu[1].strIconPath = "Bat_I_TacticEnter";
		this.m_kMenu[2].strIconPath = string.Empty;
		this.m_kMenu[3].strIconPath = string.Empty;
		this.m_kMenu[4].strIconPath = string.Empty;
		this.m_kMenu[5].strIconPath = string.Empty;
	}

	public override void OpenUI(int nSelect)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG) && this.m_kMenu[nSelect].byMenuType == NPC_UI.E_NPC_UI_TYPE.E_NPC_UI_TYPE_SCENARIOBATTLE)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SB", "MENU_CLICK", true, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			base.OpenUI(nSelect);
			return;
		}
	}
}
