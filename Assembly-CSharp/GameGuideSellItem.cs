using System;
using UnityEngine;
using UnityForms;

public class GameGuideSellItem : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INVENTORY_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.INVENTORY_DLG);
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
			for (int i = 1; i <= 4; i++)
			{
				if (25 <= NkUserInventory.GetInstance().Get_Tab_List_Count(i))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
