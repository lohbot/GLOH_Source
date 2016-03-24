using System;
using TsBundle;
using UnityForms;

public class ColosseumHelpDlg : Form
{
	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_Help", G_ID.COLOSSEUM_HELP, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_btClose = (base.GetControl("Close_Button") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
		this.Hide();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
