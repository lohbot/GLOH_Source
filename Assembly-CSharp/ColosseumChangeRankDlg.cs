using System;
using UnityEngine;
using UnityForms;

public class ColosseumChangeRankDlg : Form
{
	private DrawTexture m_dtRankChange;

	private Label m_laGradeRank;

	private Label m_laCharName;

	private Label m_laGradePoint;

	private int m_OldGradeRank;

	private float m_fStartTime;

	private float m_ShowDlgTime = 6f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_Rank_Change", G_ID.COLOSSEUM_CHANGERANK_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_dtRankChange = (base.GetControl("DT_RankText") as DrawTexture);
		this.m_laGradeRank = (base.GetControl("Label_Rank2") as Label);
		this.m_laCharName = (base.GetControl("Label_Charname2") as Label);
		this.m_laGradePoint = (base.GetControl("Label_Matchpoint2") as Label);
		base.SetScreenCenter();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		if (Time.realtimeSinceStartup - this.m_fStartTime > this.m_ShowDlgTime)
		{
			this.Close();
		}
		base.Update();
	}

	public override void OnClose()
	{
	}

	public void ShowChangeRank(int colosseum_grade_oldrank)
	{
		this.m_OldGradeRank = colosseum_grade_oldrank;
		this.m_fStartTime = Time.realtimeSinceStartup;
		this.SetInfo();
		this.Show();
	}

	public void SetInfo()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		this.m_laGradeRank.Text = myCharInfo.GetColosseumMyGradeRank().ToString();
		this.m_laCharName.Text = charPersonInfo.GetCharName();
		int num = 1000 + myCharInfo.ColosseumGradePoint;
		this.m_laGradePoint.Text = num.ToString();
		if (this.m_OldGradeRank <= 0 || this.m_OldGradeRank > myCharInfo.GetColosseumMyGradeRank())
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_RANKUP", this.m_dtRankChange, this.m_dtRankChange.GetSize());
			NrSound.ImmedatePlay("UI_SFX", "COLOSSEUM", "RANKUP");
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_RANKDOWN", this.m_dtRankChange, this.m_dtRankChange.GetSize());
			NrSound.ImmedatePlay("UI_SFX", "COLOSSEUM", "RANKDOWN");
		}
	}
}
