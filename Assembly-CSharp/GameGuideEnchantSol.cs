using System;
using UnityEngine;
using UnityForms;

public class GameGuideEnchantSol : GameGuideInfo
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

	public override bool CheckGameGuide()
	{
		if (Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			this.m_nCheckTime = Time.realtimeSinceStartup;
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList() != null && 15 <= NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetCount())
			{
				return true;
			}
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
