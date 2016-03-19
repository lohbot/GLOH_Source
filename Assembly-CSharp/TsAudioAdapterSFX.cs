using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - SFX")]
public class TsAudioAdapterSFX : TsAudioAdapter
{
	[SerializeField]
	private TsAudioSFX _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioSFX(EAudioType.SFX);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.SFX)
		{
			return false;
		}
		this._audioEx = (TsAudioSFX)audioEx;
		return true;
	}
}
