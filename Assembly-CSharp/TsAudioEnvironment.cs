using System;

[Serializable]
public class TsAudioEnvironment : TsAudio
{
	public TsAudioEnvironment(EAudioType audioType) : base(audioType)
	{
	}

	protected override void _OnAwake()
	{
		if (base.RefAudioSource == null)
		{
			TsLog.LogError("AudioSource Must need~!", new object[0]);
		}
	}
}
