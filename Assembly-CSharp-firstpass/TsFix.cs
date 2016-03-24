using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class TsFix
{
	private static AudioClip DummyAudioClip = AudioClip.Create("dummy_audio_clip", 44100, 1, 44100, false, false);

	[DebuggerHidden]
	private static IEnumerable<AudioSource> _FindAudioSources(Transform target)
	{
		TsFix.<_FindAudioSources>c__Iterator27 <_FindAudioSources>c__Iterator = new TsFix.<_FindAudioSources>c__Iterator27();
		<_FindAudioSources>c__Iterator.target = target;
		<_FindAudioSources>c__Iterator.<$>target = target;
		TsFix.<_FindAudioSources>c__Iterator27 expr_15 = <_FindAudioSources>c__Iterator;
		expr_15.$PC = -2;
		return expr_15;
	}

	public static void AudioSourceWarning(GameObject target)
	{
		if (target == null)
		{
			return;
		}
		foreach (AudioSource current in TsFix._FindAudioSources(target.transform))
		{
			if (current.clip == null)
			{
				current.clip = TsFix.DummyAudioClip;
			}
		}
	}
}
