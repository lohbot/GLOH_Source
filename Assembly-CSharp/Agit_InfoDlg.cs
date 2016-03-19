using System;
using UnityForms;

public class Agit_InfoDlg : Form
{
	private Button m_btAgitInfo;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/dlg_agit_info", G_ID.AGIT_INFO_DLG, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btAgitInfo = (base.GetControl("Button_agit_info") as Button);
		this.m_btAgitInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAgitMain));
	}

	public void ClickAgitMain(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_MAIN_DLG);
	}
}
