using System;
using UnityForms;

public class IndunRecommendList_DLG : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_indunrecommendlist", G_ID.INDUN_RECOMMENDLIST_DLG, false);
		base.ChangeSceneDestory = false;
		this.Hide();
	}

	public override void SetComponent()
	{
	}

	public override void OnClose()
	{
	}

	public override void Update()
	{
	}
}
