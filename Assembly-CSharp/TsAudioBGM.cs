using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class TsAudioBGM : TsAudio
{
	private struct _Fade
	{
		public bool _isFading;

		public float _origVolume;

		public float _volume;

		public float _fadeStartTime;

		public float _fadeCurrentTime;

		public float _fadeEndTime;

		public float CurrentDeltaTime
		{
			get
			{
				return this._fadeCurrentTime - this._fadeStartTime;
			}
		}

		public float EndDeltaTime
		{
			get
			{
				return this._fadeEndTime - this._fadeStartTime;
			}
		}

		public void Init()
		{
			this._isFading = false;
			this._origVolume = 0f;
			this._volume = 0f;
			this._fadeStartTime = 0f;
			this._fadeCurrentTime = 0f;
			this._fadeEndTime = 0f;
		}
	}

	private class _PlayTimeInfo
	{
		public string goName;

		public float playTime;
	}

	private static TsAudioBGM s_currentBGM = null;

	private static TsAudioBGM s_nextBGM = null;

	private static readonly int Max_PlayHistoryCount = 10;

	private static Queue<string> s_playHistory = new Queue<string>(TsAudioBGM.Max_PlayHistoryCount);

	private static readonly float s_limitedUpdateTime = 0.1f;

	private static List<TsAudioBGM._PlayTimeInfo> s_playTimeInfos = new List<TsAudioBGM._PlayTimeInfo>();

	[SerializeField]
	private float _fadeTime = 0.1f;

	[SerializeField]
	private float _loopIntervalTime = 2f;

	private bool _wasPlaying;

	private float _loopIntervalEndTime = 3.40282347E+38f;

	private float _lastUpdateTime;

	private TsAudioBGM._Fade _fadeIn;

	private TsAudioBGM._Fade _fadeOut;

	private bool _useUnityLoop;

	public static TsAudioBGM CurrentBGM
	{
		get
		{
			return TsAudioBGM.s_currentBGM;
		}
	}

	public static TsAudioBGM NextBGM
	{
		get
		{
			return TsAudioBGM.s_nextBGM;
		}
	}

	public static string[] PlayHistory
	{
		get
		{
			return TsAudioBGM.s_playHistory.ToArray();
		}
	}

	public override bool isPlaying
	{
		get
		{
			bool flag = base.isPlaying;
			if (flag)
			{
				if (this._fadeIn._isFading)
				{
					flag = true;
				}
				if (this._fadeOut._isFading)
				{
					flag = true;
				}
			}
			return flag;
		}
	}

	public float FadeTime
	{
		get
		{
			return this._fadeTime;
		}
		set
		{
			this._fadeTime = Mathf.Max(0f, value);
		}
	}

	public float LoopIntervalTime
	{
		get
		{
			return this._loopIntervalTime;
		}
		set
		{
			this._loopIntervalTime = Mathf.Max(0f, value);
		}
	}

	public bool UseUnityLoop
	{
		get
		{
			return this._useUnityLoop;
		}
		set
		{
			this._useUnityLoop = value;
			base.Loop = value;
		}
	}

	public TsAudioBGM(EAudioType audioType) : base(audioType)
	{
	}

	public static void SaveCurrentBGMPlayTime()
	{
		if (TsAudioBGM.s_currentBGM == null || TsAudioBGM.s_currentBGM.RefAudioSource == null)
		{
			return;
		}
		if (TsAudioBGM.s_playTimeInfos.Count >= 5)
		{
			TsAudioBGM.s_playTimeInfos.RemoveAt(0);
		}
		TsAudioBGM._PlayTimeInfo item = null;
		foreach (TsAudioBGM._PlayTimeInfo current in TsAudioBGM.s_playTimeInfos)
		{
			if (TsAudioBGM.s_currentBGM.RefAudioSource.name.Equals(current.goName))
			{
				item = current;
			}
		}
		TsAudioBGM.s_playTimeInfos.Remove(item);
		TsAudioBGM._PlayTimeInfo playTimeInfo = new TsAudioBGM._PlayTimeInfo();
		playTimeInfo.goName = TsAudioBGM.s_currentBGM.RefAudioSource.name;
		playTimeInfo.playTime = TsAudioBGM.s_currentBGM.RefAudioSource.time;
		TsAudioBGM.s_playTimeInfos.Add(playTimeInfo);
	}

	public static bool RestoreBGMPlayTime(string goName)
	{
		TsAudioBGM._PlayTimeInfo playTimeInfo = null;
		foreach (TsAudioBGM._PlayTimeInfo current in TsAudioBGM.s_playTimeInfos)
		{
			if (current.goName.Equals(goName))
			{
				playTimeInfo = current;
			}
		}
		if (playTimeInfo == null)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find(goName);
		if (gameObject == null)
		{
			return false;
		}
		gameObject.audio.time = playTimeInfo.playTime;
		playTimeInfo.playTime = 0f;
		return true;
	}

	public static void InitBGMs()
	{
		if (TsAudioBGM.s_currentBGM != null)
		{
			TsAudioBGM.s_currentBGM = null;
		}
		if (TsAudioBGM.s_nextBGM != null)
		{
			TsAudioBGM.s_nextBGM = null;
		}
	}

	protected override void _OnAwake()
	{
		if (base.RefAudioSource == null)
		{
			TsLog.LogError("AudioSource Must need~!", new object[0]);
		}
		this._loopIntervalEndTime = 3.40282347E+38f;
	}

	protected override void _OnUpdate()
	{
		if (Time.time - this._lastUpdateTime < TsAudioBGM.s_limitedUpdateTime)
		{
			return;
		}
		this._lastUpdateTime = Time.time;
		this._Update_Loop();
		this._Update_Fade();
	}

	private void _Update_Loop()
	{
		if (base.IsForceStop)
		{
			return;
		}
		if (base.RefAudioSource == null)
		{
			return;
		}
		if (base.RefAudioClip == null)
		{
			return;
		}
		if (!this.isPlaying && base.RefAudioClip.isReadyToPlay)
		{
			if (this._wasPlaying)
			{
				this._loopIntervalEndTime = Time.time + this._loopIntervalTime;
			}
			if (this._loopIntervalEndTime <= Time.time)
			{
				if (!this.UseUnityLoop)
				{
					this._InitLoopInterval_N_Play();
				}
				else
				{
					base.Loop = true;
					base.RefAudioSource.Play();
				}
			}
		}
		this._wasPlaying = this.isPlaying;
	}

	private void _InitLoopInterval_N_Play()
	{
		this._Stop();
		base.Play();
	}

	protected override void _OnDestroy()
	{
		this._Stop();
	}

	private void _Update_Fade()
	{
		if (TsAudioBGM.s_currentBGM != null && !TsAudioBGM.s_currentBGM.RefAdapter)
		{
			TsAudioBGM.s_currentBGM = null;
		}
		if (TsAudioBGM.s_nextBGM != null && !TsAudioBGM.s_nextBGM.RefAdapter)
		{
			TsAudioBGM.s_nextBGM = null;
		}
		if (TsAudioBGM.s_nextBGM != null)
		{
			if (TsAudioBGM.s_currentBGM == null)
			{
				TsAudioBGM.s_nextBGM.FadeIn();
			}
			else
			{
				TsAudioBGM.s_currentBGM.FadeOut();
				TsAudioBGM.s_nextBGM.FadeIn();
			}
			TsAudioBGM.s_currentBGM = TsAudioBGM.s_nextBGM;
			TsAudioBGM.s_nextBGM = null;
		}
	}

	public void FadeOut()
	{
		if (base.RefAdapter != null && !base.RefAdapter.gameObject.activeInHierarchy)
		{
			return;
		}
		base.RefAdapter.StartCoroutine(this._FadeOut());
	}

	[DebuggerHidden]
	private IEnumerator _FadeOut()
	{
		TsAudioBGM.<_FadeOut>c__Iterator68 <_FadeOut>c__Iterator = new TsAudioBGM.<_FadeOut>c__Iterator68();
		<_FadeOut>c__Iterator.<>f__this = this;
		return <_FadeOut>c__Iterator;
	}

	public void FadeIn()
	{
		if (base.RefAdapter != null && !base.RefAdapter.gameObject.activeInHierarchy)
		{
			return;
		}
		base.RefAdapter.StartCoroutine(this._FadeIn());
	}

	[DebuggerHidden]
	private IEnumerator _FadeIn()
	{
		TsAudioBGM.<_FadeIn>c__Iterator69 <_FadeIn>c__Iterator = new TsAudioBGM.<_FadeIn>c__Iterator69();
		<_FadeIn>c__Iterator.<>f__this = this;
		return <_FadeIn>c__Iterator;
	}

	protected override void _Play()
	{
		if (!base._PreprocessPlayClip())
		{
			TsLog.LogWarning("Cannot Play~!! failed~! _PreprocessPlayClip()", new object[0]);
			return;
		}
		if (!this.isPlaying)
		{
			TsAudioBGM.RestoreBGMPlayTime(base.RefAudioSource.name);
			TsAudioBGM.s_nextBGM = this;
			if (TsAudioBGM.s_playHistory.Count >= TsAudioBGM.Max_PlayHistoryCount)
			{
				TsAudioBGM.s_playHistory.Dequeue();
			}
			TsAudioBGM.s_playHistory.Enqueue(this._audioClip.name);
		}
	}

	protected override void _PlayOneShot()
	{
		TsLog.LogWarning("TsAudioBGM is not Support PlayOneShot()", new object[0]);
	}

	protected override void _Stop()
	{
		base.RefAdapter.StopAllCoroutines();
		if (this._fadeIn._isFading)
		{
			this._fadeIn._isFading = false;
		}
		if (TsAudioBGM.s_currentBGM == this)
		{
			TsAudioBGM.s_currentBGM = null;
		}
		if (TsAudioBGM.s_nextBGM == this)
		{
			TsAudioBGM.s_nextBGM = null;
		}
		this._loopIntervalEndTime = 3.40282347E+38f;
		base.RefAudioClip = null;
		this._ePlayable = TsAudio.EPlayable.None;
	}

	public override void RestoreToPlay()
	{
		this._Stop();
		base.Play();
	}
}
