using System;

public class GameGuideInviteFriend : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		return this.m_eCheck == GameGuideCheck.LOGIN;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
