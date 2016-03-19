using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ColosseumRewardDlg : Form
{
	private DrawTexture m_dRankImg;

	private Label m_lRank;

	private Button m_bOK;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_Reward", G_ID.COLOSSEUMREWARD_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_dRankImg = (base.GetControl("DT_LeagueIcon") as DrawTexture);
		this.m_lRank = (base.GetControl("LB_RankNum01") as Label);
		this.m_bOK = (base.GetControl("BT_GetRank_Reward") as Button);
		Button expr_48 = this.m_bOK;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.OnClickOK));
		base.SetScreenCenter();
	}

	public void SetColosseumRewardInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		short colosseumOldGrade = kMyCharInfo.ColosseumOldGrade;
		int colosseumOldRank = kMyCharInfo.ColosseumOldRank;
		string gradeTexture = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTexture(colosseumOldGrade);
		if (gradeTexture != string.Empty)
		{
			this.m_dRankImg.SetTexture(gradeTexture);
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2509");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"rank",
			colosseumOldRank.ToString()
		});
		this.m_lRank.Text = empty;
	}

	public void OnClickOK(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int colosseumOldRank = kMyCharInfo.ColosseumOldRank;
		if (colosseumOldRank <= 0)
		{
			return;
		}
		GS_COLOSSEUM_REWARD_REQ gS_COLOSSEUM_REWARD_REQ = new GS_COLOSSEUM_REWARD_REQ();
		gS_COLOSSEUM_REWARD_REQ.rank = colosseumOldRank;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_REWARD_REQ, gS_COLOSSEUM_REWARD_REQ);
	}
}
