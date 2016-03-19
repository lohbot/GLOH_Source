using System;
using UnityForms;

public class IndunEnterRaid_DLG : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_indunenter_raid", G_ID.INDUN_ENTER_RAID_DLG, false);
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
