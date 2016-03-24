using System;
using UnityForms;

public class MythRaid_Room_DLG : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "mythraid/dlg_room", G_ID.MYTHRAID_USERLIST_DLG, false);
	}

	public override void OnClose()
	{
	}

	public override void SetComponent()
	{
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
	}
}
