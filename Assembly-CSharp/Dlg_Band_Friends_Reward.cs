using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class Dlg_Band_Friends_Reward : Form
{
	public enum SHOW_TYPE
	{
		EVENT,
		CHALLENGE
	}

	private const int MAX_REWARD_COUNT = 5;

	private DrawTexture m_txRewardTexture;

	private DrawTexture[] m_txRewardCompleteBG;

	private DrawTexture[] m_txRewardComplete;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/DLG_BandReward", G_ID.BAND_FRIENDS_REWARD_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_txRewardTexture = (base.GetControl("DrawTexture_ImageBG01") as DrawTexture);
		this.m_txRewardCompleteBG = new DrawTexture[4];
		this.m_txRewardComplete = new DrawTexture[4];
		for (int i = 0; i < 4; i++)
		{
			this.m_txRewardCompleteBG[i] = (base.GetControl("DrawTexture_Reward" + (i + 1).ToString()) as DrawTexture);
			this.m_txRewardCompleteBG[i].Visible = false;
			this.m_txRewardComplete[i] = (base.GetControl("DrawTexture_Complete" + (i + 1).ToString()) as DrawTexture);
			this.m_txRewardComplete[i].Visible = false;
		}
		base.SetShowLayer(2, false);
	}

	public void SetType(Dlg_Band_Friends_Reward.SHOW_TYPE _ShowType)
	{
		if (_ShowType == Dlg_Band_Friends_Reward.SHOW_TYPE.EVENT)
		{
			this.m_txRewardTexture.SetTextureFromBundle("UI/Etc/Invite_Reward02");
		}
		else
		{
			this.m_txRewardTexture.SetTextureFromBundle("UI/Etc/Invite_Reward");
			this.SetData();
		}
	}

	public void SetData()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return;
		}
		Dictionary<short, ChallengeTable> challengeType = NrTSingleton<ChallengeManager>.Instance.GetChallengeType(ChallengeManager.TYPE.BAND_INVITE_FRIENDS);
		if (challengeType == null)
		{
			return;
		}
		foreach (ChallengeTable current in challengeType.Values)
		{
			if ((int)current.m_nLevel <= kMyCharInfo.GetLevel())
			{
				Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(current.m_nUnique);
				int i = 0;
				int num = 64;
				while (i < current.m_kRewardInfo.Count)
				{
					if (current.m_kRewardInfo[i].m_nConditionCount != 0 && current.m_kRewardInfo[i].m_nItemUnique != 0)
					{
						bool visible = false;
						if (userChallengeInfo2 != null)
						{
							if (i < num)
							{
								long num2 = 1L << (i & 31);
								if (1L <= (userChallengeInfo2.m_bGetReward1 & num2))
								{
									visible = true;
								}
							}
							else
							{
								long num3 = 1L << (i - num & 31);
								if (1L <= (userChallengeInfo2.m_bGetReward1 & num3))
								{
									visible = true;
								}
							}
						}
						if (i < 5 && i > 0)
						{
							this.m_txRewardCompleteBG[i - 1].Visible = visible;
							this.m_txRewardComplete[i - 1].Visible = visible;
						}
					}
					i++;
				}
			}
		}
	}
}
