using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - MOVIE")]
public class TsAudioAdapterMovie : TsAudioAdapter
{
	[SerializeField]
	private TsAudioMovie _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioMovie(EAudioType.MOVIE);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.MOVIE)
		{
			return false;
		}
		this._audioEx = (TsAudioMovie)audioEx;
		return true;
	}
}
