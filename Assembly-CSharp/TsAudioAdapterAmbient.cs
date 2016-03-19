using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - Ambient")]
public class TsAudioAdapterAmbient : TsAudioAdapter
{
	[SerializeField]
	private TsAudioAmbient _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioAmbient(EAudioType.AMBIENT);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.AMBIENT)
		{
			return false;
		}
		this._audioEx = (TsAudioAmbient)audioEx;
		return true;
	}
}
