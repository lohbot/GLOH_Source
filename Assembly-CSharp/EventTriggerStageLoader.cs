using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

public class EventTriggerStageLoader
{
	public delegate void LoadComplete(GameObject[] goTriggers);

	private static EventTriggerStageLoader.LoadComplete _LoadComplete;

	private static string _Path;

	private static ItemType _ItemType;

	public static bool HasLoadItem
	{
		get;
		private set;
	}

	static EventTriggerStageLoader()
	{
		EventTriggerStageLoader._Path = null;
		EventTriggerStageLoader._LoadComplete = null;
		EventTriggerStageLoader._ItemType = ItemType.USER_ASSETB;
		EventTriggerStageLoader.HasLoadItem = false;
	}

	[DebuggerHidden]
	public static IEnumerator LoadEventTrigger()
	{
		return new EventTriggerStageLoader.<LoadEventTrigger>c__Iterator4();
	}

	public static void SetLoadingInfo(string path, EventTriggerStageLoader.LoadComplete LoadCompleteFunc)
	{
		string text = string.Empty;
		if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			text = string.Format("{0}{1}.xml", CDefinePath.XMLPath(), path);
			EventTriggerStageLoader._ItemType = ItemType.USER_STRING;
			path = text;
		}
		else
		{
			if (TsPlatform.IsMobile)
			{
				text = string.Format("{0}{1}_mobile{2}", CDefinePath.XMLBundlePath(), path, Option.extAsset);
			}
			else
			{
				text = string.Format("{0}{1}{2}", CDefinePath.XMLBundlePath(), path, Option.extAsset);
			}
			EventTriggerStageLoader._ItemType = ItemType.USER_ASSETB;
			path = text;
		}
		EventTriggerStageLoader._Path = path;
		EventTriggerStageLoader._LoadComplete = LoadCompleteFunc;
		EventTriggerStageLoader.HasLoadItem = true;
		TsLog.Log("EventTriggerStageLoader.SetLoadingInfo({0}...)", new object[]
		{
			EventTriggerStageLoader._Path
		});
	}

	public static void ReadXML(IDownloadedItem wItem, object kParmObj)
	{
		GameObject[] goTriggers = null;
		EventTriggerXMLMaker eventTriggerXMLMaker = new EventTriggerXMLMaker();
		if (wItem.canAccessString)
		{
			goTriggers = eventTriggerXMLMaker.ReadXML(wItem.safeString);
		}
		else if (wItem.canAccessAssetBundle)
		{
			goTriggers = eventTriggerXMLMaker.ReadXML(wItem, null);
		}
		if (EventTriggerStageLoader._LoadComplete != null)
		{
			EventTriggerStageLoader._LoadComplete(goTriggers);
		}
	}
}
