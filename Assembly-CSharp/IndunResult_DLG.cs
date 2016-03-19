using System;
using UnityForms;

public class IndunResult_DLG : Form
{
	private enum eSHOWLAYER
	{
		eSHOWLAYER_NONE,
		eSHOWLAYER_WIN,
		eSHOWLAYER_LOSE
	}

	private IndunResult_DLG.eSHOWLAYER m_eShowLayer;

	private Label m_lbRewardGold;

	private Label m_lbLoseReason;

	private Button m_buClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_indunresult", G_ID.INDUN_RESULT_DLG, false);
		base.ChangeSceneDestory = false;
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_lbRewardGold = (base.GetControl("Label_RewardGold") as Label);
		this.m_lbLoseReason = (base.GetControl("Label_TitleInfo02") as Label);
		this.m_buClose = (base.GetControl("Button_Close") as Button);
		Button expr_48 = this.m_buClose;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.CloseForm));
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height / 2f - base.GetSize().y / 2f);
	}

	public void SetReult(bool bWin, eINDUN_CLOSE_REASON eReason, long nRewardGold)
	{
		if (bWin)
		{
			this.m_eShowLayer = IndunResult_DLG.eSHOWLAYER.eSHOWLAYER_WIN;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1583"),
				"rewardgold",
				nRewardGold.ToString()
			});
			this.m_lbRewardGold.SetText(empty);
		}
		else
		{
			this.m_eShowLayer = IndunResult_DLG.eSHOWLAYER.eSHOWLAYER_LOSE;
			string empty2 = string.Empty;
			switch (eReason)
			{
			case eINDUN_CLOSE_REASON.eINDUN_CLOSE_REASON_GIVEUP:
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1604"),
					"username",
					"NULL"
				});
				break;
			case eINDUN_CLOSE_REASON.eINDUN_CLOSE_REASON_TIMEOVER:
				this.m_lbLoseReason.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1605"));
				break;
			case eINDUN_CLOSE_REASON.eINDUN_CLOSE_REASON_FAILWAR:
				this.m_lbLoseReason.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1606"));
				break;
			}
		}
		this.Show();
		base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height / 2f - base.GetSize().y / 2f);
		base.ShowLayer((int)this.m_eShowLayer);
	}
}
