using System;
using UnityForms;

public class StoryChatListDlg : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "StoryChat/DLG_StoryChatList", G_ID.STORYCHATLIST_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
	}
}
