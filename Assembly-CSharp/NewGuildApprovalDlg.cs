using System;
using UnityForms;

public class NewGuildApprovalDlg : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_Approval", G_ID.NEWGUILD_APPROVAL_DLG, true);
	}

	public override void SetComponent()
	{
	}
}
