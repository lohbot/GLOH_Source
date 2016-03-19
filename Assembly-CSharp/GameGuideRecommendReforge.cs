using System;
using UnityEngine;
using UnityForms;

public class GameGuideRecommendReforge : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.REFORGEMAIN_DLG);
		}
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		if (10000L <= NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
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
							NrEquipItemInfo equipItemInfo = soldierInfo.GetEquipItemInfo();
							if (equipItemInfo != null)
							{
								for (int j = 0; j < 6; j++)
								{
									if (0 < equipItemInfo.GetItemUnique(j))
									{
										int num = equipItemInfo.m_kItem[j].GetItem().m_nOption[2];
										if (num >= 3)
										{
											return false;
										}
									}
								}
							}
						}
					}
				}
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
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
