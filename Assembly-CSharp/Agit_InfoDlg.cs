using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Agit_InfoDlg : Form
{
	private Button m_btAgitInfo;

	private Button m_btAgitOut;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/dlg_agit_info", G_ID.AGIT_INFO_DLG, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH + 1f);
	}

	public override void SetComponent()
	{
		this.m_btAgitInfo = (base.GetControl("Button_agit_info") as Button);
		this.m_btAgitInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAgitMain));
		this.m_btAgitOut = (base.GetControl("Button_agit_exit") as Button);
		this.m_btAgitOut.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAgitOut));
	}

	public void ClickAgitMain(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_MAIN_DLG);
	}

	public void ClickAgitOut(IUIObject obj)
	{
		GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
		gS_WARP_REQ.nMode = 0;
		gS_WARP_REQ.nGateIndex = 0;
		gS_WARP_REQ.nWorldMapWarp_MapIDX = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
	}
}
