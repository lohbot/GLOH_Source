using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TsBundle;
using UnityEngine;

[Serializable]
public abstract class TsAudio
{
	public enum EPlayable
	{
		None,
		Downloading,
		Success,
		Failure
	}

	public enum _EPlayType
	{
		PLAY,
		PLAY_ONE_SHOT,
		PLAY_CLIP_AT_POINT
	}

	[Serializable]
	public class BaseData
	{
		[SerializeField]
		private EAudioType _eAudioType;

		[SerializeField]
		private TsAudio.RandomizePitch _pitchRandomize;

		[SerializeField]
		private TsAudio.RandomizeVolume _volumeRandomize;

		[SerializeField]
		private List<TsAudioBundleInfo> _bundleInfos;

		[SerializeField]
		protected bool _skipIfPlayingSame;

		private int _manualDecideIndex = -1;

		private bool _loop;

		public HashSet<int> RequestedIndexList = new HashSet<int>();

		public HashSet<int> DownloadedIndexList = new HashSet<int>();

		private readonly int _maxRandomCount = 100;

		public bool IsDontDestroyOnLoad
		{
			get;
			set;
		}

		public int CurrentBundleInfoIndex
		{
			get;
			private set;
		}

		public string CurrentBundleName
		{
			get;
			private set;
		}

		public EAudioType AudioType
		{
			get
			{
				return this._eAudioType;
			}
		}

		public bool SkipIfPlayingSame
		{
			get
			{
				return this._skipIfPlayingSame;
			}
			set
			{
				this._skipIfPlayingSame = value;
			}
		}

		public float DelayPlayTime
		{
			get;
			set;
		}

		public int Test_ManualDownloadIndex
		{
			get;
			set;
		}

		public string[] IndexStringArrayOfBundleInfo
		{
			get
			{
				List<string> list = new List<string>(this._bundleInfos.Count);
				for (int i = 0; i < this._bundleInfos.Count; i++)
				{
					list.Add(i.ToString());
				}
				return list.ToArray();
			}
		}

		public TsAudio.RandomizePitch RandPitch
		{
			get
			{
				return this._pitchRandomize;
			}
		}

		public TsAudio.RandomizeVolume RandVolume
		{
			get
			{
				return this._volumeRandomize;
			}
		}

		public TsAudioBundleInfo DefaultBundleInfo
		{
			get
			{
				return this._bundleInfos[0];
			}
		}

		public int BundleInfoCount
		{
			get
			{
				return this._bundleInfos.Count;
			}
		}

		public TsAudioBundleInfo[] BundleInfoArray
		{
			get
			{
				return this._bundleInfos.ToArray();
			}
		}

		public bool Loop
		{
			get
			{
				return this._loop;
			}
			set
			{
				this._loop = value;
			}
		}

		public bool IsManualDecideMode
		{
			get;
			set;
		}

		public int ManualDecideIndex
		{
			get
			{
				return this._manualDecideIndex;
			}
			set
			{
				if (value <= -1 || value >= this._bundleInfos.Count)
				{
					TsLog.LogWarning("Out of Length~! TsAudio.ManualDecideIndex TatalCount= {0}   try to set Index= {1}", new object[]
					{
						this._bundleInfos.Count,
						value
					});
					value = 0;
				}
				this._manualDecideIndex = value;
			}
		}

		public string Tag
		{
			get;
			set;
		}

		private BaseData()
		{
		}

		public bool IsDownloadedAudioBundle(int index)
		{
			foreach (int current in this.DownloadedIndexList)
			{
				if (current == index)
				{
					return true;
				}
			}
			return false;
		}

		public static TsAudio.BaseData Create(EAudioType eAudioType)
		{
			return TsAudio.BaseData.Create(eAudioType, new TsAudio.RandomizePitch(), new TsAudio.RandomizeVolume(), false, new TsAudioBundleInfo());
		}

		public static TsAudio.BaseData Create(EAudioType eAudioType, TsAudio.RandomizePitch randPitch, TsAudio.RandomizeVolume randVolume, bool skipIfPlayingSame, TsAudioBundleInfo bundleInfo)
		{
			if (randPitch == null || randVolume == null || bundleInfo == null)
			{
				TsLog.LogError("It must need~!", new object[0]);
				UnityEngine.Debug.DebugBreak();
				return null;
			}
			return new TsAudio.BaseData
			{
				_eAudioType = eAudioType,
				_pitchRandomize = randPitch,
				_volumeRandomize = randVolume,
				_bundleInfos = new List<TsAudioBundleInfo>(),
				_bundleInfos = 
				{
					bundleInfo
				},
				IsDontDestroyOnLoad = false,
				Loop = false,
				IsManualDecideMode = false,
				DelayPlayTime = 0f,
				SkipIfPlayingSame = skipIfPlayingSame
			};
		}

		public static TsAudio.BaseData Create(EAudioType eAudioType, TsAudio.RandomizePitch randPitch, TsAudio.RandomizeVolume randVolume, bool skipIfPlayingSame, List<TsAudioBundleInfo> bundleInfos)
		{
			if (randPitch == null || randVolume == null || bundleInfos == null || bundleInfos.Count <= 0)
			{
				TsLog.LogError("It must need~!", new object[0]);
				UnityEngine.Debug.DebugBreak();
				return null;
			}
			TsAudio.BaseData baseData = new TsAudio.BaseData();
			baseData._eAudioType = eAudioType;
			baseData._pitchRandomize = randPitch;
			baseData._volumeRandomize = randVolume;
			baseData._bundleInfos = new List<TsAudioBundleInfo>();
			baseData._bundleInfos.AddRange(bundleInfos);
			baseData.IsDontDestroyOnLoad = false;
			baseData.Loop = false;
			baseData.IsManualDecideMode = false;
			baseData.DelayPlayTime = 0f;
			baseData.SkipIfPlayingSame = skipIfPlayingSame;
			return baseData;
		}

		public override string ToString()
		{
			return string.Format("AudioType({0}) BundleInfos(COUNT={1}  DefBundle={2}) RandomPitch({3}) RandomVolume({4}) Skip({5})", new object[]
			{
				this._eAudioType,
				this._bundleInfos.Count,
				this.DefaultBundleInfo,
				this._pitchRandomize,
				this._volumeRandomize,
				this.SkipIfPlayingSame
			});
		}

		public bool FillBundleInfo(AudioClip audioClip)
		{
			int index = this._bundleInfos.Count - 1;
			bool result = false;
			try
			{
				result = this._bundleInfos[index].FillBundleInfo(audioClip);
			}
			catch (Exception ex)
			{
				TsLog.LogError(ex.ToString(), new object[0]);
			}
			return result;
		}

		public void AddBundleInfo()
		{
			this._bundleInfos.Add(new TsAudioBundleInfo());
		}

		public void AddBundleInfo(TsAudioBundleInfo newBundleInfo)
		{
			if (newBundleInfo != null)
			{
				this._bundleInfos.Add(newBundleInfo);
			}
		}

		public void RemoveBundleInfo(ref int nIndex)
		{
			if (this._bundleInfos.Count >= 2 && nIndex >= 0)
			{
				this._bundleInfos.RemoveAt(nIndex);
				nIndex = -1;
			}
		}

		public void UpBundleInfo(ref int nIndex)
		{
			if (this._bundleInfos.Count >= 2 && nIndex > 0)
			{
				TsAudioBundleInfo item = this._bundleInfos[nIndex];
				this._bundleInfos.RemoveAt(nIndex);
				this._bundleInfos.Insert(--nIndex, item);
			}
		}

		public void DownBundleInfo(ref int nIndex)
		{
			if (this._bundleInfos.Count >= 2 && nIndex < this._bundleInfos.Count - 1 && nIndex >= 0)
			{
				TsAudioBundleInfo item = this._bundleInfos[nIndex];
				this._bundleInfos.RemoveAt(nIndex);
				this._bundleInfos.Insert(++nIndex, item);
			}
		}

		public TsAudioBundleInfo DecideBundleInfo()
		{
			if (this._bundleInfos.Count <= 0)
			{
				return null;
			}
			int num = 0;
			if (this._bundleInfos.Count >= 2)
			{
				if (!this.IsManualDecideMode)
				{
					for (int i = 0; i < this._maxRandomCount; i++)
					{
						num = UnityEngine.Random.Range(0, this._bundleInfos.Count);
						if (!this._bundleInfos[num].IsIgnoreRandomMode)
						{
							break;
						}
					}
				}
				else
				{
					num = this.ManualDecideIndex;
				}
			}
			this.CurrentBundleInfoIndex = num;
			this.CurrentBundleName = this._bundleInfos[num].AudioClipName;
			return this._bundleInfos[num];
		}

		public void RemoveUsedWWWItem()
		{
			foreach (TsAudioBundleInfo current in this._bundleInfos)
			{
				current.RemoveUsedWWWItem();
			}
		}
	}

	public class RequestData
	{
		public int requestIndex
		{
			get;
			set;
		}

		public TsAudio.BaseData baseData
		{
			get;
			set;
		}

		public RequestData(int index, TsAudio.BaseData baseData)
		{
			this.requestIndex = index;
			this.baseData = baseData;
		}
	}

	[Serializable]
	public class RandomizePitch : ICloneable
	{
		public static readonly float MIN = -3f;

		public static readonly float MAX = 3f;

		[SerializeField]
		private float _pitch = 1f;

		[SerializeField]
		public bool enable;

		[SerializeField]
		private float _min = 0.7f;

		[SerializeField]
		private float _max = 1.25f;

		public float pitch
		{
			get
			{
				return this._pitch;
			}
			set
			{
				this._pitch = value;
			}
		}

		public float min
		{
			get
			{
				return this._min;
			}
			set
			{
				this._min = Mathf.Clamp(value, TsAudio.RandomizePitch.MIN, this._max);
			}
		}

		public float max
		{
			get
			{
				return this._max;
			}
			set
			{
				this._max = Mathf.Clamp(value, this._min, TsAudio.RandomizePitch.MAX);
			}
		}

		public float Do()
		{
			if (this.enable)
			{
				return UnityEngine.Random.Range(this.min, this.max);
			}
			return this.pitch;
		}

		public override string ToString()
		{
			return string.Format("Pitch[{0}] enable[{1}] Min[{2}] Max[{3}]", new object[]
			{
				this._pitch,
				this.enable,
				this._min,
				this._max
			});
		}

		public object Clone()
		{
			return new TsAudio.RandomizePitch
			{
				enable = this.enable,
				_pitch = this._pitch,
				_min = this._min,
				_max = this._max
			};
		}

		public override bool Equals(object obj)
		{
			TsAudio.RandomizePitch randomizePitch = obj as TsAudio.RandomizePitch;
			return randomizePitch != null && this.enable == randomizePitch.enable && this._pitch == randomizePitch._pitch && this._min == randomizePitch._min && this._max == randomizePitch._max;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	[Serializable]
	public class RandomizeVolume : ICloneable
	{
		public static readonly float MIN;

		public static readonly float MAX = 1f;

		[SerializeField]
		private float _volume = 1f;

		[SerializeField]
		public bool enable;

		[SerializeField]
		private float _min = 0.8f;

		[SerializeField]
		private float _max = 1f;

		public float volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
			}
		}

		public float min
		{
			get
			{
				return this._min;
			}
			set
			{
				this._min = Mathf.Clamp(value, TsAudio.RandomizeVolume.MIN, this._max);
			}
		}

		public float max
		{
			get
			{
				return this._max;
			}
			set
			{
				this._max = Mathf.Clamp(value, this._min, TsAudio.RandomizeVolume.MAX);
			}
		}

		public float Do()
		{
			if (this.enable)
			{
				return UnityEngine.Random.Range(this.min, this.max);
			}
			return this.volume;
		}

		public override string ToString()
		{
			return string.Format("Volume[{0}] enable[{1}] Min[{2}] Max[{3}]", new object[]
			{
				this._volume,
				this.enable,
				this._min,
				this._max
			});
		}

		public object Clone()
		{
			return new TsAudio.RandomizeVolume
			{
				enable = this.enable,
				_volume = this._volume,
				_min = this._min,
				_max = this._max
			};
		}

		public override bool Equals(object obj)
		{
			TsAudio.RandomizeVolume randomizeVolume = obj as TsAudio.RandomizeVolume;
			return randomizeVolume != null && this.enable == randomizeVolume.enable && this._volume == randomizeVolume._volume && this._min == randomizeVolume._min && this._max == randomizeVolume._max;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class PlayPointInfo
	{
		public enum EType
		{
			None,
			Destroy_if_SameName,
			Skip_if_SameName
		}

		public TsAudio.PlayPointInfo.EType type
		{
			get;
			private set;
		}

		public Vector3 playPoint
		{
			get;
			private set;
		}

		public string goName
		{
			get;
			private set;
		}

		public PlayPointInfo(TsAudio.PlayPointInfo.EType _type)
		{
			this.type = _type;
			this.playPoint = Vector3.zero;
			this.goName = string.Empty;
		}

		public PlayPointInfo(TsAudio.PlayPointInfo.EType _type, Vector3 _playPoint)
		{
			this.type = _type;
			this.playPoint = _playPoint;
			this.goName = string.Empty;
		}

		public PlayPointInfo(TsAudio.PlayPointInfo.EType _type, Vector3 _playPoint, string _goName)
		{
			this.type = _type;
			this.playPoint = _playPoint;
			this.goName = _goName;
		}

		public override string ToString()
		{
			return string.Format("EType[{0}] Point[{1}] goName[{2}]", this.type.ToString(), this.playPoint.ToString(), this.goName);
		}
	}

	public static readonly string AssetBundleStackName = "TsAudio";

	private static float s_masterVolume = 1f;

	private static float[] s_volumeOfAudioTypes = new float[]
	{
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f
	};

	private static float[] s_volumeScalings = new float[]
	{
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f
	};

	private static bool[] s_disableAudio = new bool[10];

	private static bool[] s_muteAudio = new bool[10];

	private static bool[] s_storedMuteAudio = new bool[10];

	protected static bool s_useReservePlay = false;

	protected static bool s_flushReservedPlay = false;

	protected static HashSet<string> s_disableTagList = new HashSet<string>();

	protected static readonly string[] s_disableTagSep = new string[]
	{
		"#"
	};

	private static bool isStore = false;

	protected TsAudio.EPlayable _ePlayable;

	protected TsAudio._EPlayType _playType;

	protected TsAudio.PlayPointInfo _playPointInfo;

	protected TsAudioAdapter _adpater;

	protected AudioClip _audioClip;

	protected AudioSource _audioSource;

	protected float _calculatedVolume = 1f;

	protected bool _isForceStop = true;

	[SerializeField]
	protected bool _playOnDownload = true;

	[SerializeField]
	private TsAudio.BaseData _baseData;

	private static bool audioDisabled = false;

	public TsAudio._EPlayType _TempSet_playType
	{
		set
		{
			this._playType = value;
		}
	}

	public TsAudio.EPlayable Playable
	{
		get
		{
			return this._ePlayable;
		}
	}

	public bool PlayOnDownload
	{
		get
		{
			return this._playOnDownload;
		}
		set
		{
			this._playOnDownload = value;
		}
	}

	public virtual bool isPlaying
	{
		get
		{
			return this.RefAudioSource != null && this.RefAudioSource.isPlaying;
		}
	}

	public TsAudioAdapter RefAdapter
	{
		get
		{
			return this._adpater;
		}
		set
		{
			this._adpater = value;
		}
	}

	public AudioClip RefAudioClip
	{
		get
		{
			return this._audioClip;
		}
		set
		{
			if (this._audioClip != value)
			{
				this.baseData.RemoveUsedWWWItem();
			}
			this._audioClip = value;
			if (this._playType != TsAudio._EPlayType.PLAY_ONE_SHOT && this._audioSource != null)
			{
				this._audioSource.clip = this._audioClip;
			}
		}
	}

	public AudioSource RefAudioSource
	{
		get
		{
			return this._audioSource;
		}
		set
		{
			this._audioSource = value;
			if (this._playType != TsAudio._EPlayType.PLAY_ONE_SHOT && this._audioClip != null)
			{
				this._audioSource.clip = this._audioClip;
			}
		}
	}

	public float CalculatedVolume
	{
		get
		{
			return this._calculatedVolume;
		}
		set
		{
			if (this.RefAudioSource != null)
			{
				this.RefAudioSource.volume = value;
			}
			this._calculatedVolume = value;
		}
	}

	public float Volume
	{
		get
		{
			return this._baseData.RandVolume.volume;
		}
		set
		{
			if (this.RefAudioSource != null)
			{
				this.RefAudioSource.volume = value;
			}
			this._baseData.RandVolume.volume = value;
		}
	}

	public float Pitch
	{
		get
		{
			return this._baseData.RandPitch.pitch;
		}
		set
		{
			if (this.RefAudioSource != null)
			{
				this.RefAudioSource.pitch = value;
			}
			this._baseData.RandPitch.pitch = value;
		}
	}

	public TsAudio.BaseData baseData
	{
		get
		{
			return this._baseData;
		}
	}

	public bool Loop
	{
		get
		{
			return this._baseData.Loop;
		}
		set
		{
			this._baseData.Loop = value;
			if (this.RefAudioSource != null)
			{
				this.RefAudioSource.loop = value;
			}
		}
	}

	public static float MasterVolume
	{
		get
		{
			return TsAudio.s_masterVolume;
		}
		set
		{
			TsAudio.s_masterVolume = value;
			TsAudio.RefreshAllAudioVolumes();
		}
	}

	public static bool[] DisableAudioStatus
	{
		get
		{
			return TsAudio.s_disableAudio;
		}
	}

	protected bool IsReservedPlay
	{
		get;
		set;
	}

	public static bool UseReservePlay
	{
		get
		{
			return TsAudio.s_useReservePlay;
		}
		set
		{
			TsAudio.s_useReservePlay = value;
			if (TsAudio.s_useReservePlay)
			{
				TsAudio.s_flushReservedPlay = false;
			}
		}
	}

	public bool IsForceStop
	{
		get
		{
			return this._isForceStop;
		}
		protected set
		{
			this._isForceStop = value;
		}
	}

	protected ulong DelayPlayTimeToUnity
	{
		get
		{
			ulong result = 0uL;
			if (this.RefAudioClip != null)
			{
				result = (ulong)((float)this.RefAudioClip.samples * this.baseData.DelayPlayTime);
			}
			return result;
		}
	}

	protected TsAudio(EAudioType audioType)
	{
		if (this._baseData != null)
		{
			TsLog.LogError("_baseData not NULL~!", new object[0]);
		}
		this._baseData = TsAudio.BaseData.Create(audioType);
	}

	public void _InitBaseData(TsAudio.BaseData baseData)
	{
		if (baseData == null)
		{
			return;
		}
		this._baseData = baseData;
	}

	public virtual bool IsValid()
	{
		return !string.IsNullOrEmpty(this.baseData.DefaultBundleInfo.AssetBundleName);
	}

	public void CheckAndSetNeedRefs(TsAudioAdapter audioAdapter)
	{
		if (this.RefAdapter == null)
		{
			this.RefAdapter = audioAdapter;
		}
		if (this.RefAudioSource == null)
		{
			this.RefAudioSource = audioAdapter.audio;
		}
		if (this.RefAudioClip == null)
		{
			this.RefAudioClip = audioAdapter.audio.clip;
		}
	}

	public void OnAwake(TsAudioAdapter adapter)
	{
		if (TsAudio.audioDisabled || TsAudio.IsMuteAudioType(adapter.AudioType))
		{
			AudioSource audio = adapter.audio;
			if (audio != null)
			{
				audio.playOnAwake = false;
				audio.Stop();
			}
		}
		this.CheckAndSetNeedRefs(adapter);
		if (!this.RefAudioSource.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.RefAudioSource != null)
		{
			this.baseData.Loop = this.RefAudioSource.loop;
			if (!this.PlayOnDownload)
			{
				this.PlayOnDownload = this.RefAudioSource.playOnAwake;
			}
			this.RefAudioSource.playOnAwake = false;
			this.RefAudioSource.mute = TsAudio.IsMuteAudioType(this._baseData.AudioType);
			this.CalculatedVolume = this.CalculateVolume();
			this.RefAudioSource.volume = this.CalculatedVolume;
			if (this.PlayOnDownload && !TsAudio.IsMuteAudioType(adapter.AudioType))
			{
				this.Play();
			}
		}
		this._OnAwake();
	}

	protected virtual void _OnAwake()
	{
	}

	private bool _ReservePlay()
	{
		if (!TsAudio.UseReservePlay)
		{
			return false;
		}
		EAudioType audioType = this.baseData.AudioType;
		bool result;
		switch (audioType)
		{
		case EAudioType.ENVIRONMENT:
		case EAudioType.BGM_STREAM:
			goto IL_41;
		case EAudioType.MOVIE:
			IL_2E:
			if (audioType != EAudioType.BGM && audioType != EAudioType.AMBIENT)
			{
				if (this.baseData.IsDontDestroyOnLoad)
				{
					result = false;
				}
				else
				{
					this.IsReservedPlay = false;
					result = true;
				}
				return result;
			}
			goto IL_41;
		}
		goto IL_2E;
		IL_41:
		this.IsReservedPlay = true;
		result = true;
		return result;
	}

	public static void FlushReservedPlayList()
	{
		TsAudio.s_flushReservedPlay = true;
	}

	private void _PlayProcess(TsAudio._EPlayType ePlayType, bool redownload)
	{
		this._PlayProcess(ePlayType, null, redownload);
	}

	private void _PlayProcess(TsAudio._EPlayType ePlayType, TsAudio.PlayPointInfo playPointInfo, bool redownload)
	{
		this._playType = ePlayType;
		this._playPointInfo = playPointInfo;
		if (this._ReservePlay())
		{
			return;
		}
		if (this.RefAudioClip != null)
		{
			this._ePlayable = TsAudio.EPlayable.Success;
		}
		if (redownload)
		{
			this._ePlayable = TsAudio.EPlayable.None;
		}
		switch (this._ePlayable)
		{
		case TsAudio.EPlayable.None:
			if (!this.IsValid())
			{
				TsLog.LogError("Sound AssetBundleName Must Need~!", new object[0]);
				return;
			}
			this._ePlayable = TsAudio.EPlayable.Downloading;
			TsAudioManager.Container._RequestDownload(this._baseData, new PostProcPerItem(this.OnEvent_Downloaded));
			break;
		case TsAudio.EPlayable.Success:
			if (this.RefAdapter != null && !this.RefAdapter.gameObject.activeInHierarchy)
			{
				return;
			}
			switch (this._playType)
			{
			case TsAudio._EPlayType.PLAY:
				this._Play();
				break;
			case TsAudio._EPlayType.PLAY_ONE_SHOT:
				this._PlayOneShot();
				break;
			case TsAudio._EPlayType.PLAY_CLIP_AT_POINT:
				this._PlayClipAtPoint();
				break;
			default:
				TsLog.LogError("ePlayType is Invalid~!", new object[0]);
				break;
			}
			break;
		case TsAudio.EPlayable.Failure:
			TsLog.LogWarning("TsAudio._PlayProcess() Failure~!", new object[0]);
			break;
		}
	}

	protected bool _PreprocessPlayClip()
	{
		if (this.RefAudioSource == null)
		{
			return false;
		}
		if (this.RefAudioClip == null)
		{
			return false;
		}
		TsAudioAdapter.TryToAddAdapter(this.RefAudioSource.gameObject, this);
		this.Randomize();
		this.RefAudioSource.loop = this.baseData.Loop;
		this.CalculatedVolume = this.CalculateVolume();
		this.RefAudioSource.mute = TsAudio.IsMuteAudioType(this._baseData.AudioType);
		this.IsReservedPlay = false;
		this.IsForceStop = false;
		return true;
	}

	public void Play()
	{
		this._PlayProcess(TsAudio._EPlayType.PLAY, false);
	}

	protected virtual void _Play()
	{
		if (this.RefAudioSource == null)
		{
			UnityEngine.Debug.Log("RefAudioSource Is NULL :  " + this.baseData.DefaultBundleInfo.AudioClipName);
			return;
		}
		if (!this.RefAudioSource.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this._PreprocessPlayClip())
		{
			if (this._baseData.SkipIfPlayingSame)
			{
				if (!TsAudioPlayingList.isPlaying(this.RefAudioClip.name))
				{
					TsAudioPlayingList.Add(this.RefAudioClip.name, null);
					this.RefAdapter.DestroyAfter(this.RefAudioClip.length);
					this.RefAudioSource.Play(this.DelayPlayTimeToUnity);
				}
			}
			else
			{
				this.RefAudioSource.Play(this.DelayPlayTimeToUnity);
			}
		}
	}

	public virtual void PlayByManual(int bundleIndex)
	{
		this.PlayByManual(bundleIndex, 1f);
	}

	public virtual void PlayByManual(int bundleIndex, float pitch)
	{
		this.Pitch = pitch;
		if (!this.isPlaying || this.baseData.ManualDecideIndex != bundleIndex)
		{
			this.baseData.IsManualDecideMode = true;
			this.baseData.ManualDecideIndex = bundleIndex;
			this._PlayProcess(TsAudio._EPlayType.PLAY, true);
		}
	}

	public void PlayOneShot()
	{
		this._PlayProcess(TsAudio._EPlayType.PLAY_ONE_SHOT, false);
	}

	protected virtual void _PlayOneShot()
	{
		bool flag = this.RefAudioClip.name.Contains("FOOTSTEP");
		if (flag || this.RefAudioClip.name.Contains("ATTACK_1"))
		{
			if (flag && this.RefAudioSource.gameObject.name.Contains("monster/"))
			{
				return;
			}
			this.RefAudioSource.clip = this.RefAudioClip;
		}
		else
		{
			GameObject gameObject = BugFixAudio.NewAutoDestroyAudioSource(this, this.RefAudioSource.transform);
			this.RefAudioSource = gameObject.audio;
		}
		if (this._PreprocessPlayClip())
		{
			if (this._baseData.SkipIfPlayingSame)
			{
				if (TsAudioPlayingList.isPlaying(this.RefAudioClip.name))
				{
					return;
				}
				TsAudioPlayingList.Add(this.RefAudioClip.name, null);
			}
			this.RefAudioSource.Play(this.DelayPlayTimeToUnity);
		}
	}

	public void PlayClipAtPoint(Vector3 playPoint)
	{
		TsAudio.PlayPointInfo playClipAtPoint_Info = new TsAudio.PlayPointInfo(TsAudio.PlayPointInfo.EType.None, playPoint);
		this.PlayClipAtPoint(playClipAtPoint_Info);
	}

	public void PlayClipAtPoint(TsAudio.PlayPointInfo playClipAtPoint_Info)
	{
		if (playClipAtPoint_Info == null)
		{
			return;
		}
		this._PlayProcess(TsAudio._EPlayType.PLAY_CLIP_AT_POINT, playClipAtPoint_Info, false);
	}

	protected virtual void _PlayClipAtPoint()
	{
		if (this.RefAudioClip == null)
		{
			TsLog.LogWarning("_PlayClipAtPoint() must need AudioClip~!", new object[0]);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (this._playPointInfo.type == TsAudio.PlayPointInfo.EType.Destroy_if_SameName)
		{
			if (TsAudioPlayingList.isPlaying(this.RefAudioClip.name))
			{
				if (!TsAudioPlayingList.isPlayed(this.RefAudioClip.name, 10))
				{
					GameObject playingGo = TsAudioPlayingList.GetPlayingGo(this.RefAudioClip.name, true);
					if (playingGo != null)
					{
						UnityEngine.Object.Destroy(playingGo);
					}
				}
			}
			else
			{
				flag = true;
				flag2 = true;
			}
		}
		else if (this._playPointInfo.type == TsAudio.PlayPointInfo.EType.Skip_if_SameName)
		{
			if (TsAudioPlayingList.isPlaying(this.RefAudioClip.name))
			{
				return;
			}
			flag = true;
		}
		GameObject gameObject = BugFixAudio.NewAutoDestroyAudioSource(this, this._playPointInfo.playPoint);
		if (gameObject != null)
		{
			if (flag)
			{
				if (flag2)
				{
					TsAudioPlayingList.Add(this.RefAudioClip.name, gameObject);
				}
				else
				{
					TsAudioPlayingList.Add(this.RefAudioClip.name, null);
				}
			}
			if (this._baseData.IsDontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			gameObject.audio.volume = this.CalculatedVolume;
			this.RefAudioSource = gameObject.audio;
			if (this._PreprocessPlayClip())
			{
				this.RefAudioSource.Play(this.DelayPlayTimeToUnity);
			}
		}
	}

	public void Update()
	{
		if (TsAudio.s_flushReservedPlay && this.IsReservedPlay)
		{
			if (!this.baseData.IsManualDecideMode)
			{
				this._PlayProcess(this._playType, this._playPointInfo, false);
			}
			this.IsReservedPlay = false;
		}
		this._OnUpdate();
	}

	protected virtual void _OnUpdate()
	{
	}

	public void OnDestroy()
	{
		if (this.RefAudioClip != null)
		{
			TsAudioPlayingList.Remove(this.RefAudioClip.name);
		}
		this._baseData.RemoveUsedWWWItem();
		this._OnDestroy();
	}

	protected virtual void _OnDestroy()
	{
	}

	public virtual void DrawGizmos(GameObject go)
	{
	}

	public void Stop()
	{
		if (!this.isPlaying)
		{
			return;
		}
		this.IsReservedPlay = false;
		this.IsForceStop = true;
		this._audioSource.Stop();
		this._Stop();
	}

	protected virtual void _Stop()
	{
	}

	public void Pause()
	{
		if (!this.isPlaying)
		{
			return;
		}
		this._audioSource.Pause();
	}

	public void OnEvent_Downloaded(IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		TsAudio.RequestData requestData = obj as TsAudio.RequestData;
		if (requestData.baseData != this._baseData)
		{
			TsLog.LogError("OnEvent_Downloaded() It Must Same~!", new object[0]);
			return;
		}
		if (wItem.canAccessAssetBundle)
		{
			TsImmortal.bundleService.RequestLoadAsync(new LoadAsyncCallback(this.OnEvent_DownloadedAsync), wItem, obj, this._baseData.CurrentBundleName, typeof(AudioClip));
		}
		else if (wItem.canAccessAudioClip)
		{
			this.RefAudioClip = wItem.safeAudioClip;
			this._ePlayable = TsAudio.EPlayable.Success;
			this._OnDownload_Success();
			this.baseData.DownloadedIndexList.Add(requestData.requestIndex);
		}
		else
		{
			this.RefAudioClip = null;
			this._ePlayable = TsAudio.EPlayable.Failure;
			this._OnDownload_Failue();
		}
	}

	[DebuggerHidden]
	private IEnumerator OnEvent_DownloadedAsync(IDownloadedItem wItem, object obj, string name, Type type)
	{
		TsAudio.<OnEvent_DownloadedAsync>c__Iterator67 <OnEvent_DownloadedAsync>c__Iterator = new TsAudio.<OnEvent_DownloadedAsync>c__Iterator67();
		<OnEvent_DownloadedAsync>c__Iterator.wItem = wItem;
		<OnEvent_DownloadedAsync>c__Iterator.name = name;
		<OnEvent_DownloadedAsync>c__Iterator.type = type;
		<OnEvent_DownloadedAsync>c__Iterator.obj = obj;
		<OnEvent_DownloadedAsync>c__Iterator.<$>wItem = wItem;
		<OnEvent_DownloadedAsync>c__Iterator.<$>name = name;
		<OnEvent_DownloadedAsync>c__Iterator.<$>type = type;
		<OnEvent_DownloadedAsync>c__Iterator.<$>obj = obj;
		<OnEvent_DownloadedAsync>c__Iterator.<>f__this = this;
		return <OnEvent_DownloadedAsync>c__Iterator;
	}

	protected virtual void _OnDownload_Success()
	{
		if (this._playOnDownload)
		{
			this._PlayProcess(this._playType, this._playPointInfo, false);
		}
	}

	protected virtual void _OnDownload_Failue()
	{
		TsLog.LogWarning("_OnDownload_Failue() AudioClip Failed Download~! URL= {0}", new object[]
		{
			this.baseData.DefaultBundleInfo.DownloadPath
		});
	}

	public bool FillBundleInfo()
	{
		return !(this.RefAudioSource == null) && !(this.RefAudioClip == null) && this.baseData.FillBundleInfo(this.RefAudioClip);
	}

	public void AddBundleInfo()
	{
		this.baseData.AddBundleInfo();
	}

	public void RemoveBundleInfo(ref int nIndex)
	{
		this.baseData.RemoveBundleInfo(ref nIndex);
	}

	public void UpBundleInfo(ref int nIndex)
	{
		this.baseData.UpBundleInfo(ref nIndex);
	}

	public void DownBundleInfo(ref int nIndex)
	{
		this.baseData.DownBundleInfo(ref nIndex);
	}

	public void Randomize()
	{
		this.Pitch = this.baseData.RandPitch.Do();
		this.Volume = this.baseData.RandVolume.Do();
	}

	public override string ToString()
	{
		return this.baseData.ToString();
	}

	public float CalculateVolume()
	{
		return TsAudio.MasterVolume * TsAudio._CalcVolumeOfAudioType(this._baseData.AudioType) * this.Volume;
	}

	public static void SetVolumeOfAudioType(EAudioType eAudioType, float volume)
	{
		try
		{
			volume = Mathf.Min(volume, 1f);
			volume = Mathf.Max(volume, 0f);
			TsAudio.s_volumeOfAudioTypes[(int)eAudioType] = volume;
		}
		catch (Exception ex)
		{
			TsLog.LogError("SetVolumeOfAudio({0})   Exception~! = {1}", new object[]
			{
				eAudioType,
				ex.ToString()
			});
		}
	}

	public static float GetVolumeOfAudio(EAudioType eAudioType)
	{
		float result = 1f;
		string key = TsAudio._GetPlayerPrefKey_Volume(eAudioType);
		if (PlayerPrefs.HasKey(key))
		{
			result = PlayerPrefs.GetFloat(key);
		}
		return result;
	}

	public static float GetVolumeOfAudioType(EAudioType eAudioType)
	{
		float result = 1f;
		try
		{
			result = TsAudio.s_volumeOfAudioTypes[(int)eAudioType];
		}
		catch (Exception ex)
		{
			TsLog.LogError("GetVolumeOfAudio({0})   Exception~! = {1}", new object[]
			{
				eAudioType,
				ex.ToString()
			});
			result = 0f;
		}
		return result;
	}

	protected static float _CalcVolumeOfAudioType(EAudioType eAudioType)
	{
		float volumeOfAudioType = TsAudio.GetVolumeOfAudioType(eAudioType);
		return volumeOfAudioType * TsAudio.GetVolumeScaling(eAudioType);
	}

	public static void RefreshAllAudioVolumes()
	{
		TsAudioAdapter[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapter)) as TsAudioAdapter[];
		TsAudioAdapter[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapter tsAudioAdapter = array2[i];
			if (!(tsAudioAdapter.GetAudioEx().RefAudioSource == null))
			{
				tsAudioAdapter.GetAudioEx().CalculatedVolume = tsAudioAdapter.GetAudioEx().CalculateVolume();
			}
		}
	}

	public static void RefreshAudioVolumes(EAudioType eAudioType)
	{
		uint num = 0u;
		TsAudioAdapter[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapter)) as TsAudioAdapter[];
		TsAudioAdapter[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapter tsAudioAdapter = array2[i];
			if (tsAudioAdapter.AudioType == eAudioType)
			{
				if (!(tsAudioAdapter.GetAudioEx().RefAudioSource == null))
				{
					tsAudioAdapter.GetAudioEx().CalculatedVolume = tsAudioAdapter.GetAudioEx().CalculateVolume();
					num += 1u;
				}
			}
		}
	}

	public static void SetDisableDownloadAllAudio(bool disable)
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				TsAudio.SetDisableDownloadAudio(eAudioType, disable);
			}
		}
		TsAudio.audioDisabled = disable;
	}

	public static void SetDisableDownloadAudio(EAudioType eAudioType, bool disable)
	{
		try
		{
			TsAudio.s_disableAudio[(int)eAudioType] = disable;
		}
		catch (Exception ex)
		{
			TsLog.LogError("SetDisableDownloadAudio({0})    Exception~! = {1}", new object[]
			{
				eAudioType,
				ex.ToString()
			});
		}
	}

	public static bool IsDisableDownloadAudio(EAudioType eAudioType)
	{
		bool result = false;
		try
		{
			result = TsAudio.s_disableAudio[(int)eAudioType];
		}
		catch (Exception ex)
		{
			TsLog.LogError("IsDisableDownloadAudio({0})   Exception~! = {1}", new object[]
			{
				eAudioType,
				ex.ToString()
			});
			result = false;
		}
		return result;
	}

	public static void StoreMuteAllAudio()
	{
		if (TsAudio.isStore)
		{
			return;
		}
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				TsAudio.s_storedMuteAudio[(int)eAudioType] = TsAudio.IsMuteAudioType(eAudioType);
			}
		}
		TsAudio.isStore = true;
	}

	public static void RestoreMuteAllAudio()
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				TsAudio.SetMuteAudioType(eAudioType, TsAudio.s_storedMuteAudio[(int)eAudioType]);
			}
		}
		TsAudio.isStore = false;
	}

	public static void SetMuteAllAudio(bool mute)
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				TsAudio.SetMuteAudioType(eAudioType, mute);
			}
		}
	}

	public static void SetExceptMuteAllAudio(EAudioType _Excepttype, bool mute)
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				if (eAudioType != _Excepttype)
				{
					TsAudio.SetMuteAudioType(eAudioType, mute);
				}
			}
		}
	}

	public static void SetMuteAudioType(EAudioType type, bool mute)
	{
		try
		{
			TsAudio.s_muteAudio[(int)type] = mute;
		}
		catch (Exception ex)
		{
			TsLog.LogError("SetMuteAudio({0})   Exception~! = {1}", new object[]
			{
				type,
				ex.ToString()
			});
		}
	}

	public static bool IsMuteAudio(EAudioType type)
	{
		bool result = false;
		string key = TsAudio._GetPlayerPrefKey_Mute(type);
		if (PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) == 1)
		{
			result = true;
		}
		return result;
	}

	public static bool IsMuteAudioType(EAudioType type)
	{
		bool result = false;
		try
		{
			result = TsAudio.s_muteAudio[(int)type];
		}
		catch (Exception ex)
		{
			TsLog.LogError("IsMuteAudioType({0})   Exception~! = {1}", new object[]
			{
				type,
				ex.ToString()
			});
			result = false;
		}
		return result;
	}

	public static void RefreshAllMuteAudio()
	{
		uint num = 0u;
		TsAudioAdapter[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapter)) as TsAudioAdapter[];
		TsAudioAdapter[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapter tsAudioAdapter = array2[i];
			if (!(tsAudioAdapter.GetAudioEx().RefAudioSource == null))
			{
				bool mute = TsAudio.IsMuteAudioType(tsAudioAdapter.AudioType);
				tsAudioAdapter.GetAudioEx().RefAudioSource.mute = mute;
				num += 1u;
			}
		}
	}

	public static void RefreshMuteAudio(EAudioType type)
	{
		bool mute = TsAudio.IsMuteAudioType(type);
		uint num = 0u;
		TsAudioAdapter[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapter)) as TsAudioAdapter[];
		TsAudioAdapter[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapter tsAudioAdapter = array2[i];
			if (tsAudioAdapter.AudioType == type)
			{
				if (!(tsAudioAdapter.GetAudioEx().RefAudioSource == null))
				{
					tsAudioAdapter.GetAudioEx().RefAudioSource.mute = mute;
					num += 1u;
				}
			}
		}
	}

	public static void BGMFadeOut()
	{
		TsAudioAdapterBGM[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterBGM)) as TsAudioAdapterBGM[];
		TsAudioAdapterBGM[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TsAudioAdapterBGM tsAudioAdapterBGM = array2[i];
			tsAudioAdapterBGM.FadeOut();
		}
	}

	protected static string _GetPlayerPrefKey_Mute(EAudioType type)
	{
		return string.Format("Audio_Mute[{0}]", type.ToString());
	}

	protected static string _GetPlayerPrefKey_Volume(EAudioType type)
	{
		return string.Format("Audio_Volume[{0}]", type.ToString());
	}

	protected static string _GetPlayerPrefKey_DisableTags()
	{
		return "Audio_DisableTags";
	}

	protected static string _GetPlayerPrefValue_DisableTags()
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		int num = 0;
		foreach (string current in TsAudio.s_disableTagList)
		{
			num++;
			stringBuilder.Append(current);
			if (num < TsAudio.s_disableTagList.Count)
			{
				stringBuilder.Append(TsAudio.s_disableTagSep[0]);
			}
		}
		return stringBuilder.ToString();
	}

	public static void ApplyVolumeScalings(float[] volumeScalings)
	{
		if (volumeScalings.Length != 10)
		{
			TsLog.Assert(false, "MissMatch~! check float[] Length", new object[0]);
			return;
		}
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				TsAudio.SetVolumeScaling(eAudioType, volumeScalings[(int)eAudioType]);
			}
		}
	}

	public static void SetVolumeScaling(EAudioType type, float volumeScaling)
	{
		if (volumeScaling <= 0f)
		{
			volumeScaling = 1E-06f;
		}
		TsAudio.s_volumeScalings[(int)type] = volumeScaling;
	}

	public static float GetVolumeScaling(EAudioType type)
	{
		return TsAudio.s_volumeScalings[(int)type];
	}

	public static void LoadPlayerPrefs()
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				bool flag = TsAudio.IsMuteAudioType(eAudioType);
				string key = TsAudio._GetPlayerPrefKey_Mute(eAudioType);
				if (PlayerPrefs.HasKey(key))
				{
					flag = (PlayerPrefs.GetInt(key) == 1);
				}
				else
				{
					PlayerPrefs.SetInt(key, (!flag) ? 0 : 1);
				}
				TsAudio.SetMuteAudioType(eAudioType, flag);
				float num = TsAudio.GetVolumeOfAudioType(eAudioType);
				key = TsAudio._GetPlayerPrefKey_Volume(eAudioType);
				if (PlayerPrefs.HasKey(key))
				{
					num = PlayerPrefs.GetFloat(key);
				}
				else
				{
					PlayerPrefs.SetFloat(key, num);
				}
				TsAudio.SetVolumeOfAudioType(eAudioType, num);
			}
		}
		string key2 = TsAudio._GetPlayerPrefKey_DisableTags();
		if (PlayerPrefs.HasKey(key2))
		{
			string @string = PlayerPrefs.GetString(key2);
			string[] array3 = @string.Split(TsAudio.s_disableTagSep, StringSplitOptions.RemoveEmptyEntries);
			string[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				string tag = array4[j];
				TsAudio.AddDisableDownloadTag(tag);
			}
		}
		else
		{
			string value = TsAudio._GetPlayerPrefValue_DisableTags();
			PlayerPrefs.SetString(key2, value);
		}
	}

	public static void SavePlayerPrefs()
	{
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				string key = TsAudio._GetPlayerPrefKey_Mute(eAudioType);
				PlayerPrefs.SetInt(key, (!TsAudio.IsMuteAudioType(eAudioType)) ? 0 : 1);
				key = TsAudio._GetPlayerPrefKey_Volume(eAudioType);
				float volumeOfAudioType = TsAudio.GetVolumeOfAudioType(eAudioType);
				PlayerPrefs.SetFloat(key, volumeOfAudioType);
			}
		}
		string key2 = TsAudio._GetPlayerPrefKey_DisableTags();
		string value = TsAudio._GetPlayerPrefValue_DisableTags();
		PlayerPrefs.SetString(key2, value);
	}

	public static void AddDisableDownloadTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return;
		}
		TsAudio.s_disableTagList.Add(tag);
	}

	public static void RemoveDisableDownloadTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return;
		}
		TsAudio.s_disableTagList.Remove(tag);
	}

	public static void RemoveAllDisableDownloadTag()
	{
		TsAudio.s_disableTagList.Clear();
	}

	public static bool IsDisableDownloadByTag(string tag)
	{
		return TsAudio.s_disableTagList.Contains(tag);
	}

	public bool RequestDownload_AllAudioBundles()
	{
		return TsAudioManager.Container.RequestDownload_AllAudioBundles(this._baseData, new PostProcPerItem(this.OnDownloaded_JustDownload));
	}

	public bool RequestDownload_SelectiveAudioBundles(int[] indexes)
	{
		return TsAudioManager.Container.RequestDownload_SelectiveAudioBundles(this._baseData, indexes, new PostProcPerItem(this.OnDownloaded_JustDownload));
	}

	public void OnDownloaded_JustDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		TsAudio.RequestData requestData = obj as TsAudio.RequestData;
		requestData.baseData.DownloadedIndexList.Add(requestData.requestIndex);
		TsLog.Log("OnDownloaded_JustDownload() isSuccess= {0}   url= {1}", new object[]
		{
			wItem.canAccessAssetBundle,
			wItem.assetPath
		});
	}

	public static void OnDownloaded_Predownload(List<WWWItem> wiList, object obj)
	{
		TsLog.Log("TsAudio Predownload completed!", new object[0]);
	}

	public bool IsDownloadedAudioBundle(int index)
	{
		return this.baseData.IsDownloadedAudioBundle(index);
	}

	public virtual void RestoreToPlay()
	{
		if (!this.isPlaying)
		{
			this.Play();
		}
	}
}
