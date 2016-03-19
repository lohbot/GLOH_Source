using PROTOCOL.GAME;
using System;
using UnityEngine;
using UnityForms;

public class GameGuideRecommendCompose : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLCOMPOSE_MAIN_DLG);
		}
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		if (this.m_nMinLevel > myCharInfo.GetLevel())
		{
			return false;
		}
		UserChallengeInfo userChallengeInfo = myCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return false;
		}
		Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(3050);
		if (userChallengeInfo2 == null)
		{
			return false;
		}
		if (0L < userChallengeInfo2.m_nValue)
		{
			return false;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		return readySolList != null && 5 <= readySolList.GetCount() && 10000L <= myCharInfo.m_Money;
	}

	public override bool CheckGameGuide()
	{
		if (Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			this.m_nCheckTime = Time.realtimeSinceStartup;
			return this.CheckGameGuideOnce();
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
