using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TsBundle;

public class EventTriggerGameDataInfoLoader
{
	private static EventTriggerGameDataInfoLoader _Instance = null;

	private static string _CharKindFilePath = string.Empty;

	private List<string> _CharCode = new List<string>();

	private static string _MiniDramaTextFilePath = string.Empty;

	public static EventTriggerGameDataInfoLoader Instance
	{
		get
		{
			return EventTriggerGameDataInfoLoader.GetInstance();
		}
	}

	public static string CharKindFilePath
	{
		get
		{
			if (string.IsNullOrEmpty(EventTriggerGameDataInfoLoader._CharKindFilePath))
			{
				EventTriggerGameDataInfoLoader._CharKindFilePath = string.Format("{0}{1}{2}.ndt", Option.GetProtocolRootPath(Protocol.FILE), CDefinePath.NDTPath(), CDefinePath.CHARKIND_INFO_URL);
				EventTriggerGameDataInfoLoader._CharKindFilePath = EventTriggerGameDataInfoLoader._CharKindFilePath.Substring("file:///".Length, EventTriggerGameDataInfoLoader._CharKindFilePath.Length - "file:///".Length);
				EventTriggerGameDataInfoLoader._CharKindFilePath = EventTriggerGameDataInfoLoader._CharKindFilePath.Replace('\\', '/');
			}
			return EventTriggerGameDataInfoLoader._CharKindFilePath;
		}
	}

	public static string MiniDramaTextFilePath
	{
		get
		{
			if (string.IsNullOrEmpty(EventTriggerGameDataInfoLoader._MiniDramaTextFilePath))
			{
				EventTriggerGameDataInfoLoader._MiniDramaTextFilePath = string.Format("{0}{1}textmanager/text_{2}.ndt", Option.GetProtocolRootPath(Protocol.FILE), CDefinePath.NDTPath(), "minidrama");
				EventTriggerGameDataInfoLoader._MiniDramaTextFilePath = EventTriggerGameDataInfoLoader._MiniDramaTextFilePath.Substring("file:///".Length, EventTriggerGameDataInfoLoader._MiniDramaTextFilePath.Length - "file:///".Length);
				EventTriggerGameDataInfoLoader._MiniDramaTextFilePath = EventTriggerGameDataInfoLoader._MiniDramaTextFilePath.Replace('\\', '/');
			}
			return EventTriggerGameDataInfoLoader._MiniDramaTextFilePath;
		}
	}

	private static EventTriggerGameDataInfoLoader GetInstance()
	{
		if (EventTriggerGameDataInfoLoader._Instance == null)
		{
			EventTriggerGameDataInfoLoader._Instance = new EventTriggerGameDataInfoLoader();
		}
		return EventTriggerGameDataInfoLoader._Instance;
	}

	public void LoadCharKindInfo()
	{
		this._CharCode.Clear();
		NrTSingleton<NrBaseTableManager>.Instance.RemoveResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_INFO);
		using (StreamReader streamReader = new StreamReader(EventTriggerGameDataInfoLoader.CharKindFilePath))
		{
			NrTableBase nrTableBase = new NrTableCharKindInfo();
			nrTableBase.LoadFromStream(streamReader.BaseStream);
		}
		ICollection charInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetCharInfo_Col();
		if (charInfo_Col == null)
		{
			return;
		}
		foreach (CHARKIND_INFO cHARKIND_INFO in charInfo_Col)
		{
			this._CharCode.Add(cHARKIND_INFO.CHARCODE);
		}
	}

	public List<string> GetCharCodes()
	{
		return this._CharCode;
	}

	public void LoadMiniTextInfo()
	{
		NrTSingleton<NrTextMgr>.Instance.ClearTextGroup(NrTextMgr.eTEXTMGR.eTEXTMGR_MINIDRAMA);
		using (StreamReader streamReader = new StreamReader(EventTriggerGameDataInfoLoader.MiniDramaTextFilePath))
		{
			NrTableBase nrTableBase = new NrTextTable_ForTsTxtMgr("minidrama", NrTextMgr.eTEXTMGR.eTEXTMGR_MINIDRAMA, EventTriggerGameDataInfoLoader.MiniDramaTextFilePath);
			nrTableBase.LoadFromStream(streamReader.BaseStream);
		}
	}

	public Dictionary<string, string> GetMiniText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextGroupFromMINIDRAMA();
	}
}
