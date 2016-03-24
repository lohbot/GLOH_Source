using System;
using UnityEngine;

public class GameGuideChallenge : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void InitData()
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
	}

	public override void ExcuteGameGuide()
	{
		this.InitData();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		return true;
	}

	public override bool CheckGameGuide()
	{
		return this.CheckGameGuideOnce();
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
