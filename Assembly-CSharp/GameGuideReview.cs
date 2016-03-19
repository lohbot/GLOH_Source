using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class GameGuideReview : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		GS_CHAR_CHALLENGE_REQ gS_CHAR_CHALLENGE_REQ = new GS_CHAR_CHALLENGE_REQ();
		gS_CHAR_CHALLENGE_REQ.i16ChallengeUnique = 3140;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_CHALLENGE_REQ, gS_CHAR_CHALLENGE_REQ);
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest("10108_040"))
		{
			return false;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		UserChallengeInfo userChallengeInfo = myCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo != null)
		{
			Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(3140);
			if (userChallengeInfo2 != null && 0L < userChallengeInfo2.m_nValue)
			{
				return false;
			}
		}
		return true;
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
		string empty = string.Empty;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return empty;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey),
			"username",
			charPersonInfo.GetCharName()
		});
		return empty;
	}
}
