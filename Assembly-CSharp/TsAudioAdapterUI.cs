using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - UI")]
public class TsAudioAdapterUI : TsAudioAdapter
{
	[SerializeField]
	private TsAudioUI _audioEx;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioUI(EAudioType.UI);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.UI)
		{
			return false;
		}
		this._audioEx = (TsAudioUI)audioEx;
		return true;
	}
}
