using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - Voice")]
public class TsAudioAdapterVoice : TsAudioAdapter
{
	[SerializeField]
	private TsAudioVoice _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioVoice(EAudioType.VOICE);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.VOICE)
		{
			return false;
		}
		this._audioEx = (TsAudioVoice)audioEx;
		return true;
	}
}
