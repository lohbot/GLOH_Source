using System;
using TsBundle;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/Test_Module/Test AudioContainer Download"), RequireComponent(typeof(AudioSource))]
public class TsTestDownloadAtAudioContainer : MonoBehaviour
{
	public void Start()
	{
		base.audio.minDistance = 10f;
		base.audio.maxDistance = 1000f;
		base.audio.loop = true;
		base.audio.rolloffMode = AudioRolloffMode.Linear;
	}

	public void TestDownload(string domainKey, string categoryKey, string audioKey, string bundleKey)
	{
		TsAudioManager.Container.RequestAudioClip(domainKey, categoryKey, audioKey, bundleKey, new PostProcPerItem(this.OnEvent_Downloaded));
	}

	public void OnEvent_Downloaded(IDownloadedItem wItem, object obj)
	{
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
		TsAudio.RequestData requestData = obj as TsAudio.RequestData;
		TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
		if (tsAudio != null)
		{
			tsAudio.RefAudioSource = base.audio;
			tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
			tsAudio.Play();
		}
	}
}
