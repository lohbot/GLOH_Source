using System;
using System.IO;
using TsBundle;

public class EventTriggerMapInfoLoader
{
	private static EventTriggerMapInfoLoader _Instance = null;

	private static string _MapInfoFilePath = string.Empty;

	private static string _MapTextFilePath = string.Empty;

	public static EventTriggerMapInfoLoader Instance
	{
		get
		{
			return EventTriggerMapInfoLoader.GetInstance();
		}
	}

	public static string MapInfoFilePath
	{
		get
		{
			if (string.IsNullOrEmpty(EventTriggerMapInfoLoader._MapInfoFilePath))
			{
				EventTriggerMapInfoLoader._MapInfoFilePath = string.Format("{0}{1}{2}.ndt", Option.GetProtocolRootPath(Protocol.FILE), CDefinePath.NDTPath(), CDefinePath.MAP_INFO_URL);
				EventTriggerMapInfoLoader._MapInfoFilePath = EventTriggerMapInfoLoader._MapInfoFilePath.Substring("file:///".Length, EventTriggerMapInfoLoader._MapInfoFilePath.Length - "file:///".Length);
				EventTriggerMapInfoLoader._MapInfoFilePath = EventTriggerMapInfoLoader._MapInfoFilePath.Replace('\\', '/');
			}
			return EventTriggerMapInfoLoader._MapInfoFilePath;
		}
	}

	public static string MapTextFilePath
	{
		get
		{
			if (string.IsNullOrEmpty(EventTriggerMapInfoLoader._MapTextFilePath))
			{
				EventTriggerMapInfoLoader._MapTextFilePath = string.Format("{0}{1}textmanager/text_{2}.ndt", Option.GetProtocolRootPath(Protocol.FILE), CDefinePath.NDTPath(), "map");
				EventTriggerMapInfoLoader._MapTextFilePath = EventTriggerMapInfoLoader._MapTextFilePath.Substring("file:///".Length, EventTriggerMapInfoLoader._MapTextFilePath.Length - "file:///".Length);
				EventTriggerMapInfoLoader._MapTextFilePath = EventTriggerMapInfoLoader._MapTextFilePath.Replace('\\', '/');
			}
			return EventTriggerMapInfoLoader._MapTextFilePath;
		}
	}

	public EventTriggerMapInfoLoader()
	{
		this.LoadMapInfo();
	}

	private static EventTriggerMapInfoLoader GetInstance()
	{
		if (EventTriggerMapInfoLoader._Instance == null)
		{
			EventTriggerMapInfoLoader._Instance = new EventTriggerMapInfoLoader();
		}
		return EventTriggerMapInfoLoader._Instance;
	}

	public void LoadMapInfo()
	{
		NrTSingleton<NrBaseTableManager>.Instance.RemoveResourceInfo(NrTableData.eResourceType.eRT_MAP_INFO);
		using (StreamReader streamReader = new StreamReader(EventTriggerMapInfoLoader.MapInfoFilePath))
		{
			NrTableBase nrTableBase = new NkTableMapInfo();
			nrTableBase.LoadFromStream(streamReader.BaseStream);
		}
		NrTSingleton<NrTextMgr>.Instance.ClearTextGroup(NrTextMgr.eTEXTMGR.eTEXTMGR_START);
		using (StreamReader streamReader2 = new StreamReader(EventTriggerMapInfoLoader.MapTextFilePath))
		{
			NrTableBase nrTableBase2 = new NrTextTable_ForTsTxtMgr("Map", NrTextMgr.eTEXTMGR.eTEXTMGR_START, EventTriggerMapInfoLoader.MapTextFilePath);
			nrTableBase2.LoadFromStream(streamReader2.BaseStream);
		}
		EventTriggerMapManager.Instance.Claer();
		EventTriggerMapManager.Instance.MakeMapInfo();
	}
}
