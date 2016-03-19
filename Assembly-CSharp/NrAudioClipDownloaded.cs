using System;
using TsBundle;
using UnityEngine;

public class NrAudioClipDownloaded
{
	private static string m_strGameObjectNameForSystem = "One shot audio System";

	public static void OnEventAudioClipDownloadedImmedatePlay(IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (wItem.canAccessAssetBundle)
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null)
			{
				if (wItem.mainAsset == null)
				{
					TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
					{
						wItem.assetPath
					});
				}
				else
				{
					tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
					wItem.unloadImmediate = true;
				}
				tsAudio.PlayClipAtPoint(Vector3.zero);
			}
		}
	}

	public static void OnEventForcedAudioStop()
	{
	}

	public static void OnEventAudioClipDownloadedForSystem(IDownloadedItem wItem, object obj)
	{
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
		else
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null)
			{
				tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
				wItem.unloadImmediate = true;
				TsAudio.PlayPointInfo playClipAtPoint_Info = new TsAudio.PlayPointInfo(TsAudio.PlayPointInfo.EType.Skip_if_SameName, Vector3.zero, NrAudioClipDownloaded.m_strGameObjectNameForSystem);
				tsAudio.PlayClipAtPoint(playClipAtPoint_Info);
			}
		}
	}
}
