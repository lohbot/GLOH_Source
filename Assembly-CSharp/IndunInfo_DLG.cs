using System;
using UnityEngine;
using UnityForms;

public class IndunInfo_DLG : Form
{
	private Label m_lbTitle;

	private Label m_lbJoinPeopleNum;

	private Label m_lbLimitLineTime;

	private Label m_lbProgressTimeNum;

	private Label m_lbRewardGold;

	private DrawTexture m_dtBackTexture;

	private float m_fStartTime;

	private float m_fEndTime;

	private float m_fUpdateTime;

	private bool m_bSetBackTexture;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_induninfo", G_ID.INDUN_INFO_DLG, true);
		base.ChangeSceneDestory = false;
		this.m_bSetBackTexture = false;
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_ScenarioTitle") as Label);
		this.m_lbJoinPeopleNum = (base.GetControl("Label_JoinPeopleNUM") as Label);
		this.m_lbLimitLineTime = (base.GetControl("Label_LimitLineTime") as Label);
		this.m_lbProgressTimeNum = (base.GetControl("Label_ProgressTimeNUM") as Label);
		this.m_lbRewardGold = (base.GetControl("Label_RewardGold") as Label);
		this.m_dtBackTexture = (base.GetControl("DrawTexture_Img") as DrawTexture);
	}

	public override void OnClose()
	{
	}

	public override void Update()
	{
		if (Mathf.Abs(Time.time - this.m_fUpdateTime) < 1f)
		{
			return;
		}
		if (NrLoadPageScreen.IsShow())
		{
			base.AllHideLayer();
			this.m_fStartTime = Time.time;
			return;
		}
		if (!this.visible)
		{
			this.Show();
		}
		float num = this.m_fEndTime - Time.time;
		int num2 = (int)(num / 3600f);
		int num3 = (int)((num - (float)num2 * 3600f) / 60f);
		int num4 = (int)((num - (float)num2 * 3600f - (float)num3 * 60f) % 60f);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
			"hour",
			num2.ToString(),
			"min",
			num3.ToString(),
			"sec",
			num4.ToString()
		});
		this.m_lbProgressTimeNum.SetText(empty);
		this.m_fUpdateTime = Time.time;
		if (!this.m_bSetBackTexture && NrTSingleton<NkIndunManager>.Instance.LoadUIBackTexture)
		{
			this.m_dtBackTexture.SetTexture(NrTSingleton<NkIndunManager>.Instance.IndunUIBackTexture);
			this.m_bSetBackTexture = true;
		}
	}

	public void SetIndunInfo(float fStayTime, int nUserNum)
	{
		INDUN_INFO castedTarget = NrTSingleton<NkIndunManager>.Instance.IndunInfo.CastedTarget;
		if (castedTarget == null && !castedTarget.m_bShowUI)
		{
			this.Close();
			return;
		}
		this.m_bSetBackTexture = false;
		this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(castedTarget.szTextKey));
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1583"),
			"rewardgold",
			castedTarget.m_nRewardGold.ToString()
		});
		this.m_lbRewardGold.SetText(empty);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1600"),
			"usernum",
			nUserNum.ToString(),
			"maxuser",
			castedTarget.m_nMaxUser.ToString()
		});
		this.m_lbJoinPeopleNum.SetText(empty);
		int num = (int)(castedTarget.m_fPlayTime / 3600f);
		int num2 = (int)((castedTarget.m_fPlayTime - (float)num * 3600f) / 60f);
		int num3 = (int)((castedTarget.m_fPlayTime - (float)num * 3600f - (float)num2 * 60f) % 60f);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
			"hour",
			num.ToString(),
			"min",
			num2.ToString(),
			"sec",
			num3.ToString()
		});
		this.m_lbLimitLineTime.SetText(empty);
		this.m_fStartTime = Time.time;
		this.m_fEndTime = this.m_fStartTime + fStayTime;
		if (!this.m_bSetBackTexture && NrTSingleton<NkIndunManager>.Instance.LoadUIBackTexture)
		{
			this.m_dtBackTexture.SetTexture(NrTSingleton<NkIndunManager>.Instance.IndunUIBackTexture);
			this.m_bSetBackTexture = true;
		}
		if (!NrLoadPageScreen.IsShow())
		{
			this.Show();
		}
	}
}
