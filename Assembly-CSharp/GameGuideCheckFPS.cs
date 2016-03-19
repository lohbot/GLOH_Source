using System;
using UnityEngine;

public class GameGuideCheckFPS : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void InitData()
	{
		PlayerPrefs.SetInt("CheckFps", 1);
	}

	public override void ExcuteGameGuide()
	{
		TsQualityManager.Level level = TsQualityManager.Instance.CurrLevel;
		if (level == TsQualityManager.Level.HIGHEST)
		{
			level = TsQualityManager.Level.MEDIUM;
		}
		else if (level == TsQualityManager.Level.MEDIUM)
		{
			level = TsQualityManager.Level.LOWEST;
		}
		CustomQuality.GetInstance().SetQualitySettings(level);
		System_Option_Dlg.CallTsQualityManagerRefresh();
		TsQualityManager.Instance.SaveCustomSettings();
		PlayerPrefs.SetInt("CheckFps", 1);
		Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("166"));
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		return false;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
