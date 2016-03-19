using System;
using UnityEngine;
using UnityForms;

public class ChallengePopupDlg : Form
{
	private Label m_kTitle;

	private Label m_kMessage;

	private DrawTexture m_kIcon;

	private Button m_kOk;

	private float m_fStartTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "QuestList/DLG_ChallengePopup", G_ID.CHALLENGEPOPUP_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_kTitle = (base.GetControl("Label_title") as Label);
		this.m_kMessage = (base.GetControl("Label_text") as Label);
		this.m_kIcon = (base.GetControl("DrawTexture_icon") as DrawTexture);
		this.m_kOk = (base.GetControl("Button_ok") as Button);
		this.m_kOk.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOk));
		this.m_fStartTime = Time.realtimeSinceStartup;
		base.SetScreenCenter();
	}

	public void ClickOk(IUIObject obj)
	{
		ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
		if (challengeDlg != null)
		{
			challengeDlg.ChangeTab();
		}
		this.Close();
	}

	public void SetPopupInfo(short unique, short index)
	{
		ChallengeTable challengeTable = NrTSingleton<ChallengeManager>.Instance.GetChallengeTable(unique);
		if (challengeTable == null)
		{
			return;
		}
		ChallengeTable.RewardInfo rewardInfo = challengeTable.m_kRewardInfo[(int)index];
		if (rewardInfo == null)
		{
			return;
		}
		this.m_kTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(challengeTable.m_szTitleTextKey);
		string text = string.Empty;
		string textFromChallenge = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(rewardInfo.m_szConditionTextKey);
		if (textFromChallenge.Contains("count"))
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromChallenge,
				"count",
				challengeTable.m_kRewardInfo[(int)index].m_nConditionCount,
				"count1",
				rewardInfo.m_nConditionCount,
				"count2",
				rewardInfo.m_nConditionCount
			});
		}
		else
		{
			text = textFromChallenge;
		}
		this.m_kMessage.Text = text;
		this.m_kIcon.SetTexture(NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(challengeTable.m_szIconKey));
	}

	public override void Update()
	{
		if (0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime >= 5f)
		{
			this.Close();
		}
	}
}
