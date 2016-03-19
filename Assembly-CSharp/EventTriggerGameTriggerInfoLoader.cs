using System;
using System.IO;
using TsBundle;

public class EventTriggerGameTriggerInfoLoader
{
	private static EventTriggerGameTriggerInfoLoader _Instance = null;

	private static string _FilePath = string.Empty;

	public static EventTriggerGameTriggerInfoLoader Instance
	{
		get
		{
			return EventTriggerGameTriggerInfoLoader.GetInstance();
		}
	}

	public static string FilePath
	{
		get
		{
			if (string.IsNullOrEmpty(EventTriggerGameTriggerInfoLoader._FilePath))
			{
				EventTriggerGameTriggerInfoLoader._FilePath = string.Format("{0}{1}{2}.ndt", Option.GetProtocolRootPath(Protocol.FILE), CDefinePath.NDTPath(), CDefinePath.EventTriggerInfoURL);
				EventTriggerGameTriggerInfoLoader._FilePath = EventTriggerGameTriggerInfoLoader._FilePath.Substring("file:///".Length, EventTriggerGameTriggerInfoLoader._FilePath.Length - "file:///".Length);
				EventTriggerGameTriggerInfoLoader._FilePath = EventTriggerGameTriggerInfoLoader._FilePath.Replace('\\', '/');
			}
			return EventTriggerGameTriggerInfoLoader._FilePath;
		}
	}

	public EventTriggerGameTriggerInfoLoader()
	{
		this.LoadEventTriggerInfo();
	}

	private static EventTriggerGameTriggerInfoLoader GetInstance()
	{
		if (EventTriggerGameTriggerInfoLoader._Instance == null)
		{
			EventTriggerGameTriggerInfoLoader._Instance = new EventTriggerGameTriggerInfoLoader();
		}
		return EventTriggerGameTriggerInfoLoader._Instance;
	}

	public void LoadEventTriggerInfo()
	{
		EventTriggerGameTriggerInfo.Instance.Clear();
		using (StreamReader streamReader = new StreamReader(EventTriggerGameTriggerInfoLoader.FilePath))
		{
			NrTableBase nrTableBase = new BASE_EVENTTRIGGER_MANAGER(EventTriggerGameTriggerInfoLoader.FilePath);
			nrTableBase.LoadFromStream(streamReader.BaseStream);
			EventTriggerGameTriggerInfo.Instance.Load();
		}
	}
}
