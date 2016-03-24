using SERVICE;
using System;
using UnityEngine;

public class GameGuideCertifyEMAIL : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_EMAIL);
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		if (0 < NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nConfirmCheck)
		{
			return false;
		}
		if (PlayerPrefs.HasKey(NrPrefsKey.EMAIL) && PlayerPrefs.GetInt(NrPrefsKey.EMAIL) == 1)
		{
			return false;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		if (1L < kMyCharInfo.m_nActivityPoint && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return false;
		}
		if (string.Empty != NmFacebookManager.instance.UserData.m_ID)
		{
			return false;
		}
		if (0 < NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nGetConfirmItem)
		{
			return false;
		}
		if (!PlayerPrefs.HasKey(NrPrefsKey.EMAIL))
		{
			PlayerPrefs.SetInt(NrPrefsKey.EMAIL, 1);
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.EMAIL, 1);
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
