using System;
using UnityEngine;
using UnityForms;

public class GameGuideColosseum : GameGuideInfo
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
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int @int = PlayerPrefs.GetInt("Colosseum GradeRank", 0);
		ColosseumChangeRankDlg colosseumChangeRankDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_CHANGERANK_DLG) as ColosseumChangeRankDlg;
		if (colosseumChangeRankDlg != null)
		{
			colosseumChangeRankDlg.ShowChangeRank(@int);
			int colosseumMyGradeRank = myCharInfo.GetColosseumMyGradeRank();
			int colosseumGrade = (int)myCharInfo.ColosseumGrade;
			PlayerPrefs.SetInt("Colosseum GradeRank", colosseumMyGradeRank);
			PlayerPrefs.SetInt("Colosseum Grade", colosseumGrade);
		}
		this.InitData();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		int @int = PlayerPrefs.GetInt("Colosseum GradeRank", 0);
		int int2 = PlayerPrefs.GetInt("Colosseum Grade", 0);
		if (myCharInfo.ColosseumGrade > 0)
		{
			if ((int)myCharInfo.ColosseumGrade == int2)
			{
				if (@int != myCharInfo.GetColosseumMyGradeRank() || @int == 0)
				{
					return true;
				}
			}
			else
			{
				PlayerPrefs.SetInt("Colosseum GradeRank", myCharInfo.GetColosseumMyGradeRank());
				PlayerPrefs.SetInt("Colosseum Grade", (int)myCharInfo.ColosseumGrade);
			}
		}
		return false;
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
