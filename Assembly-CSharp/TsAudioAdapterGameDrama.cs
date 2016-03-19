using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - GameDrama")]
public class TsAudioAdapterGameDrama : TsAudioAdapter
{
	[SerializeField]
	private TsAudioGameDrama _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioGameDrama(EAudioType.GAME_DRAMA);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.GAME_DRAMA)
		{
			return false;
		}
		this._audioEx = (TsAudioGameDrama)audioEx;
		return true;
	}
}
