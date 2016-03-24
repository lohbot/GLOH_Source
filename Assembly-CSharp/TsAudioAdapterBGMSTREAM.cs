using System;
using UnityEngine;

[AddComponentMenu("Audio/Ndoors/TsAudioAdapter - BGMSTREAM")]
public class TsAudioAdapterBGMSTREAM : TsAudioAdapter
{
	[SerializeField]
	private TsAudioBGMSTREAM _audioEx;

	private float m_fVolum;

	private bool m_bFadeOut;

	protected override TsAudio _GetAudioEx()
	{
		if (this._audioEx == null)
		{
			this._audioEx = new TsAudioBGMSTREAM(EAudioType.BGM_STREAM);
		}
		return this._audioEx;
	}

	public override bool _InitAudioEx(TsAudio audioEx)
	{
		if (audioEx == null || audioEx.baseData.AudioType != EAudioType.BGM_STREAM)
		{
			return false;
		}
		this._audioEx = (TsAudioBGMSTREAM)audioEx;
		return true;
	}

	public void FadeOut()
	{
		this.m_fVolum = base.GetComponent<AudioSource>().volume;
		this.m_bFadeOut = true;
	}

	public override void ChildUpdate()
	{
		if (this.m_bFadeOut)
		{
			if (this.m_fVolum > 0.1f)
			{
				this.m_fVolum -= 0.2f * Time.deltaTime;
				base.GetComponent<AudioSource>().volume = this.m_fVolum;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
