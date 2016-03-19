using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - Environment")]
public class TsAudioAdapterEnvironment : TsAudioAdapter
{
	[SerializeField]
	private TsAudioEnvironment _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioEnvironment(EAudioType.ENVIRONMENT);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.ENVIRONMENT)
		{
			return false;
		}
		this._audioEx = (TsAudioEnvironment)audioEx;
		return true;
	}
}
