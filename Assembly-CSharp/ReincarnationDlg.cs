using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ReincarnationDlg : Form
{
	private DrawTexture m_dtCurRank;

	private DrawTexture m_dtCurPortrait;

	private Label m_lbCurSeason;

	private DrawTexture m_dtAfterRank;

	private DrawTexture m_dtAfterPortrait;

	private Label m_lbAfterSeason;

	private Label m_lbNeedMoney;

	private Label m_lbMyMoney;

	private Button m_btStart;

	private long m_lNeedMoney;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Char/dlg_reincarnation_check", G_ID.REINCARNATION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtCurRank = (base.GetControl("DrawTexture_Rank1") as DrawTexture);
		this.m_dtCurPortrait = (base.GetControl("DT_SolImg1") as DrawTexture);
		this.m_lbCurSeason = (base.GetControl("Label_Season1") as Label);
		this.m_dtAfterRank = (base.GetControl("DrawTexture_Rank2") as DrawTexture);
		this.m_dtAfterPortrait = (base.GetControl("DT_SolImg2") as DrawTexture);
		this.m_lbAfterSeason = (base.GetControl("Label_Season2") as Label);
		this.m_lbNeedMoney = (base.GetControl("Label_MoneyText") as Label);
		this.m_lbMyMoney = (base.GetControl("Label_Gold") as Label);
		this.m_btStart = (base.GetControl("BT_Start") as Button);
		this.m_btStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickReincarnation));
		this.ShowInfo();
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ShowInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
		if (leaderSoldierInfo == null)
		{
			return;
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_REINCARNATION_COUNT);
		int num2 = num + 1;
		ReincarnationInfo reincarnation = NrTSingleton<NrBaseTableManager>.Instance.GetReincarnation(num.ToString());
		if (reincarnation == null)
		{
			return;
		}
		ReincarnationInfo reincarnation2 = NrTSingleton<NrBaseTableManager>.Instance.GetReincarnation(num2.ToString());
		if (reincarnation2 == null)
		{
			return;
		}
		UIBaseInfoLoader solGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolGradeImg(leaderSoldierInfo.GetCharKind(), (int)leaderSoldierInfo.GetGrade());
		if (solGradeImg != null)
		{
			this.m_dtCurRank.Visible = true;
			this.m_dtCurRank.SetTexture(solGradeImg);
		}
		else
		{
			this.m_dtCurRank.Visible = false;
		}
		this.m_dtCurPortrait.SetTexture(eCharImageType.LARGE, leaderSoldierInfo.GetCharKind(), (int)leaderSoldierInfo.GetGrade());
		int num3 = leaderSoldierInfo.GetSeason() + 1;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			num3.ToString()
		});
		this.m_lbCurSeason.SetText(this.m_strText);
		solGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolGradeImg(leaderSoldierInfo.GetCharKind(), 0);
		if (solGradeImg != null)
		{
			this.m_dtAfterRank.Visible = true;
			this.m_dtAfterRank.SetTexture(solGradeImg);
		}
		else
		{
			this.m_dtAfterRank.Visible = false;
		}
		int reincarnationCharKind = reincarnation2.GetReincarnationCharKind(reincarnation, leaderSoldierInfo.GetCharKind());
		this.m_dtAfterPortrait.SetTexture(eCharImageType.LARGE, reincarnationCharKind, 0);
		num3 = 2;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(reincarnationCharKind);
		if (charKindInfo != null)
		{
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO(0);
			if (cHARKIND_SOLGRADEINFO != null)
			{
				num3 = cHARKIND_SOLGRADEINFO.SolSeason + 1;
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			num3.ToString()
		});
		this.m_lbAfterSeason.SetText(this.m_strText);
		this.m_lNeedMoney = reincarnation2.lNeedMoney;
		this.m_lbNeedMoney.SetText(ANNUALIZED.Convert(reincarnation2.lNeedMoney));
		this.m_lbMyMoney.SetText(ANNUALIZED.Convert(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money));
	}

	private void OnClickReincarnation(IUIObject obj)
	{
		if (!this.IsReincarnation())
		{
			return;
		}
		GS_SOLDIER_REINCARNATION_SET_REQ obj2 = new GS_SOLDIER_REINCARNATION_SET_REQ();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_REINCARNATION_SET_REQ, obj2);
		base.CloseNow();
	}

	private bool IsReincarnation()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money < this.m_lNeedMoney)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		return true;
	}
}
