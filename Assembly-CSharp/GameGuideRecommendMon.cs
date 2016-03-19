using System;
using UnityEngine;
using UnityForms;

public class GameGuideRecommendMon : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABELTOWERMAIN_DLG);
		}
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		return myCharInfo != null && NrTSingleton<GameGuideManager>.Instance.MonsterLevel >= myCharInfo.GetLevel() + 3;
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
