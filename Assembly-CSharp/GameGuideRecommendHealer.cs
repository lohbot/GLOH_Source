using System;
using UnityEngine;
using UnityForms;

public class GameGuideRecommendHealer : GameGuideInfo
{
	private int m_nCharKind;

	public override void Init()
	{
		base.Init();
		this.m_nCharKind = 0;
	}

	public override void ExcuteGameGuide()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLMILITARYGROUP_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
		}
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (soldierInfo != null)
				{
					if (soldierInfo.GetSolID() != 0L)
					{
						if (soldierInfo.GetCharKindInfo() != null)
						{
							if ("2070" == soldierInfo.GetCharKindInfo().GetCHARKIND_INFO().SoldierSpec1)
							{
								return false;
							}
							if (soldierInfo.GetCharKind() == 6)
							{
								return false;
							}
						}
					}
				}
			}
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return false;
			}
			int num = -1;
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current != null)
				{
					if (current.GetSolID() != 0L)
					{
						if ("2070" == current.GetCharKindInfo().GetCHARKIND_INFO().SoldierSpec1 && num < (int)current.GetGrade())
						{
							num = (int)current.GetGrade();
							this.m_nCharKind = current.GetCharKind();
						}
					}
				}
			}
			if (0 < this.m_nCharKind)
			{
				return true;
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
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey),
			"solname",
			NrTSingleton<NrCharKindInfoManager>.Instance.GetName(this.m_nCharKind)
		});
		return empty;
	}
}
