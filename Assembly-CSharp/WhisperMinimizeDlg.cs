using System;
using UnityEngine;
using UnityForms;

public class WhisperMinimizeDlg : Form
{
	private DrawTexture _dtShow;

	private static float time = 0f;

	private static float startTime = Time.realtimeSinceStartup;

	private bool m_bShow;

	private bool bCheckMinimizeIcon;

	public bool CheckMSGMinimize
	{
		get
		{
			return this.bCheckMinimizeIcon;
		}
		set
		{
			this.bCheckMinimizeIcon = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Whisper/DLG_mainWhisperMinimize", G_ID.WHISPER_MINIMIZE_DLG, true);
		base.RegisterLeftIcon(G_ID.WHISPER_DLG);
	}

	public override void SetComponent()
	{
		this._dtShow = (base.GetControl("Button") as DrawTexture);
		this._dtShow.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickShow));
		this._dtShow.data = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("51");
		base.ChangeSceneDestory = false;
	}

	private void OnClickShow(IUIObject obj)
	{
		NrTSingleton<WhisperManager>.Instance.ShowWhisperDlg();
	}

	public override void Update()
	{
		WhisperMinimizeDlg.time = Time.realtimeSinceStartup;
		if (WhisperMinimizeDlg.time - WhisperMinimizeDlg.startTime > 0.5f)
		{
			if (!NrTSingleton<WhisperManager>.Instance.IsCheckMSG())
			{
				if (this.m_bShow)
				{
					this._dtShow.SetAlpha(0.5f);
				}
				else
				{
					this._dtShow.SetAlpha(1f);
				}
			}
			else
			{
				this._dtShow.SetAlpha(1f);
			}
			this.m_bShow = !this.m_bShow;
			WhisperMinimizeDlg.startTime = WhisperMinimizeDlg.time;
		}
		base.Update();
	}
}
