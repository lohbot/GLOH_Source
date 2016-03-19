using System;
using TsBundle;
using UnityForms;

public class ExplainTooltipDlg : Form
{
	public enum eEXPLAIN_TYPE
	{
		eEXPLAIN_SOLDETAILINFO,
		eEXPLAIN_GUILDBOSS_BASICREWARD,
		eEXPLAIN_GUILDBOSS_RANKREWARD
	}

	private Label m_laTitle;

	private Label m_laExplain;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolDetailinfotooltip", G_ID.EXPLAIN_TOOLTIP_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_laTitle = (base.GetControl("Label_Title") as Label);
		this.m_laExplain = (base.GetControl("Label_TEXT") as Label);
		this.InitData();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INFORMATION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetExplainType(ExplainTooltipDlg.eEXPLAIN_TYPE type, Form pkTargetDlg)
	{
		switch (type)
		{
		case ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_SOLDETAILINFO:
			this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1627"));
			this.m_laExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1628"));
			break;
		case ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_GUILDBOSS_BASICREWARD:
			this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1913"));
			this.m_laExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1926"));
			break;
		case ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_GUILDBOSS_RANKREWARD:
			this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1914"));
			this.m_laExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1927"));
			break;
		}
		if (pkTargetDlg != null)
		{
			this.SetLocationByForm(pkTargetDlg);
		}
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		base.SetScreenCenter();
	}
}
