using System;
using TsLibs;

public class BASE_EVENTTRIGGER_MANAGER : NrTableBase
{
	private static BASE_EVENTTRIGGER_MANAGER Instance;

	public BASE_EVENTTRIGGER_MANAGER(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EventTriggerInfo eventTriggerInfo = new EventTriggerInfo();
			eventTriggerInfo.SetData(data);
			NrTSingleton<NrEventTriggerInfoManager>.Instance.AddTriggerInfo(eventTriggerInfo);
		}
		return true;
	}

	public static BASE_EVENTTRIGGER_MANAGER GetInstance()
	{
		if (BASE_EVENTTRIGGER_MANAGER.Instance == null)
		{
			BASE_EVENTTRIGGER_MANAGER.Instance = new BASE_EVENTTRIGGER_MANAGER(CDefinePath.EventTriggerInfoURL);
		}
		return BASE_EVENTTRIGGER_MANAGER.Instance;
	}
}
