using System;
using UnityEngine;

[Serializable]
public class TsAudioSFX : TsAudio
{
	private float _lastUpdateTime;

	private float _updateIntervalTime = 3f;

	private float _fTime;

	public static bool UseDistanceCulling
	{
		get;
		set;
	}

	public TsAudioSFX(EAudioType audioType) : base(audioType)
	{
	}

	protected override void _OnUpdate()
	{
		if (!TsAudioSFX.UseDistanceCulling)
		{
			return;
		}
		if (Time.time - this._lastUpdateTime < this._updateIntervalTime)
		{
			return;
		}
		this._lastUpdateTime = Time.time;
		float num = TsAudioManager.Instance.CalcDistanceToCurrentAudioListener(base.RefAudioSource.transform.position);
		if (num > base.RefAudioSource.maxDistance && base.RefAudioSource.isPlaying && !base.IsForceStop)
		{
			base.IsForceStop = true;
			this._fTime = base.RefAudioSource.time;
			base.RefAudioSource.Stop();
		}
		if (num <= base.RefAudioSource.maxDistance && !base.RefAudioSource.isPlaying && base.IsForceStop)
		{
			base.IsForceStop = false;
			base.RefAudioSource.time = this._fTime;
			base.RefAudioSource.Play();
		}
	}
}
