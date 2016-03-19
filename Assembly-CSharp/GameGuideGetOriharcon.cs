using PROTOCOL.GAME;
using System;
using UnityEngine;

public class GameGuideGetOriharcon : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(125);
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
		UserChallengeInfo userChallengeInfo = myCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return false;
		}
		Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(3120);
		if (userChallengeInfo2 != null)
		{
			if (0L < userChallengeInfo2.m_nValue)
			{
				return false;
			}
		}
		return 0 < NkUserInventory.instance.Get_First_ItemCnt(50305);
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
