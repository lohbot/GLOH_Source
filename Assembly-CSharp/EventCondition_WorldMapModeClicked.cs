using System;
using UnityForms;

public class EventCondition_WorldMapModeClicked : EventTriggerItem_EventCondition
{
	public bool m_bIsMorning;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.WorldMapModeClick.callback += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.WorldMapModeClick.callback -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		WorldMapDlg worldMapDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WORLD_MAP) as WorldMapDlg;
		if (worldMapDlg == null)
		{
			base.Verify = false;
			return;
		}
		if (!worldMapDlg.NowNightMode)
		{
			base.Verify = false;
			return;
		}
		base.Verify = true;
	}

	public override string GetComment()
	{
		string str = string.Empty;
		if (this.m_bIsMorning)
		{
			str = "아침";
		}
		else
		{
			str = "저녁";
		}
		return "월드맵 " + str + " 버튼 클릭 시";
	}
}
