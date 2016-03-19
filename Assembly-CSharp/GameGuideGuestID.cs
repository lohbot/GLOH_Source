using System;

public class GameGuideGuestID : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void InitData()
	{
	}

	public override void ExcuteGameGuide()
	{
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		return myCharInfo != null && 10 <= myCharInfo.GetLevel();
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
