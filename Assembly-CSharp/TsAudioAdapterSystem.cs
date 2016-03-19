using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - System")]
public class TsAudioAdapterSystem : TsAudioAdapter
{
	[SerializeField]
	private TsAudioSystem _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioSystem(EAudioType.SYSTEM);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.SYSTEM)
		{
			return false;
		}
		this._audioEx = (TsAudioSystem)audioEx;
		return true;
	}
}
