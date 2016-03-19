using System;
using UnityEngine;

public class TsAudioManager : MonoBehaviour
{
	public static string UI_SOUND = "2D";

	public static string MINI_SOUND = "MINI_SOUND";

	private static TsAudioManager s_instance;

	public static readonly string Resource_UploadServer_IP = "\\\\10.1.50.56\\SamPoomSound\\";

	private TsAudioContainer _audioContainer = new TsAudioContainer();

	private TsAudioListenerSwitcher _defaultAudioListenerSwitcher;

	private TsAudioListenerSwitcher _currentAudioListenerSwitcher;

	private AudioClip _tempclip;

	public static bool IsDestroy;

	public static TsAudioManager Instance
	{
		get
		{
			if (TsAudioManager.s_instance == null)
			{
				GameObject gameObject = GameObject.Find(TsAudioManager.GameObjectName);
				if (gameObject == null)
				{
					gameObject = new GameObject(TsAudioManager.GameObjectName);
					gameObject.transform.position = new Vector3(0f, 50000f, 0f);
				}
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				TsAudioManager.s_instance = gameObject.GetComponent<TsAudioManager>();
				if (TsAudioManager.s_instance == null)
				{
					TsAudioManager.s_instance = gameObject.AddComponent<TsAudioManager>();
				}
				if (!TsAudioManager.s_instance._Init())
				{
					TsLog.LogError("Failed~! Cannot Initialize TsAudioManager~!", new object[0]);
				}
			}
			return TsAudioManager.s_instance;
		}
	}

	public static TsAudioContainer Container
	{
		get
		{
			return TsAudioManager.Instance._audioContainer;
		}
	}

	public TsAudioContainer AudioContainer
	{
		get
		{
			return this._audioContainer;
		}
	}

	public static string GameObjectName
	{
		get
		{
			return "_AudioManager";
		}
	}

	public AudioListener CurrentAudioListener
	{
		get
		{
			return this._currentAudioListenerSwitcher.TargetAudioListener;
		}
	}

	private TsAudioManager()
	{
	}

	public void FirstInit()
	{
	}

	public void OnApplicationQuit()
	{
		TsAudioManager.IsDestroy = true;
	}

	private bool _Init()
	{
		TsAudio.LoadPlayerPrefs();
		TsAudio.RefreshAllAudioVolumes();
		TsAudio.RefreshAllMuteAudio();
		this._defaultAudioListenerSwitcher = new TsAudioListenerSwitcher(base.gameObject);
		if (Application.isPlaying)
		{
			this._currentAudioListenerSwitcher = null;
			AudioListener audioListener = this.SearchAndEnable_CurrentAudioListener();
			if (audioListener != null)
			{
				this._currentAudioListenerSwitcher = new TsAudioListenerSwitcher(audioListener.gameObject);
				if (this._currentAudioListenerSwitcher == null)
				{
					TsLog.LogWarning("_currentAudioListenerSwitcher == null", new object[0]);
				}
			}
			else if (Camera.main != null)
			{
				this._currentAudioListenerSwitcher = new TsAudioListenerSwitcher(Camera.main.gameObject);
				if (this._currentAudioListenerSwitcher == null)
				{
					TsLog.LogWarning("_currentAudioListenerSwitcher == null", new object[0]);
				}
			}
			if (this._currentAudioListenerSwitcher == null)
			{
				this._currentAudioListenerSwitcher = this._defaultAudioListenerSwitcher;
			}
			this._currentAudioListenerSwitcher.Switch();
			if (this.CurrentAudioListener == null)
			{
				TsLog.LogError("Failed~! Set Current AudioListener~!", new object[0]);
			}
		}
		else
		{
			this._defaultAudioListenerSwitcher.Switch();
		}
		if (Application.isEditor)
		{
			if (base.gameObject.GetComponent<TsAudioDebugger>() == null)
			{
				base.gameObject.AddComponent<TsAudioDebugger>();
			}
			if (base.gameObject.GetComponent<TsTestDownloadAtAudioContainer>() == null)
			{
				base.gameObject.AddComponent<TsTestDownloadAtAudioContainer>();
			}
		}
		if (this._tempclip == null)
		{
			this._tempclip = AudioClip.Create("tempclip", 44100, 1, 44100, false, false);
		}
		return true;
	}

	public void InitAudioListenerSwitcher()
	{
		this._defaultAudioListenerSwitcher.Switch();
	}

	public AudioListener SearchAndEnable_CurrentAudioListener()
	{
		bool flag = false;
		AudioListener audioListener = null;
		AudioListener[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
		AudioListener[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			AudioListener audioListener2 = array2[i];
			if (this.IsAudioListenerOfAudioManager(audioListener2))
			{
				audioListener2.enabled = false;
			}
			else if (!flag)
			{
				if (audioListener2.enabled && audioListener2.gameObject.activeInHierarchy)
				{
					audioListener = audioListener2;
					flag = true;
				}
			}
			else if (audioListener2.enabled)
			{
				audioListener2.enabled = false;
				TsLog.LogWarning("audioListener.enabled == true..... It must be false~! GOName= " + audioListener2.name, new object[0]);
			}
		}
		if (audioListener == null)
		{
			TsLog.LogWarning("TsAudioManager.SearchCurrentAudioListener()~! Cannot Found Current AudioListener~!!!", new object[0]);
			return null;
		}
		return audioListener;
	}

	public bool IsAudioListenerOfAudioManager(AudioListener audioListener)
	{
		AudioListener componentInChildren = TsAudioManager.Instance.GetComponentInChildren<AudioListener>();
		return !(componentInChildren == null) && componentInChildren == audioListener;
	}

	public bool SwitchAudioListenerAndDisableAll(TsAudioListenerSwitcher switcher)
	{
		if (switcher == null)
		{
			return false;
		}
		this.DisableAllAudioListener();
		return switcher.Switch();
	}

	public void DisableAllAudioListener()
	{
		AudioListener[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
		AudioListener[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			AudioListener audioListener = array2[i];
			audioListener.enabled = false;
		}
	}

	public void OnSuccess_AudioListenerSwitch(TsAudioListenerSwitcher switcher)
	{
		if (switcher == null)
		{
			return;
		}
		if (this._currentAudioListenerSwitcher != null)
		{
			if (this._currentAudioListenerSwitcher.Equals(switcher))
			{
				return;
			}
			this._currentAudioListenerSwitcher.DetachSwitcher();
		}
		this._currentAudioListenerSwitcher = switcher;
		this._currentAudioListenerSwitcher.TargetAudioListener.enabled = true;
	}

	public static void RemoveAudioListener(GameObject go, bool bObject)
	{
		AudioListener component = go.GetComponent<AudioListener>();
		if (component != null)
		{
			TsLog.LogWarning("AudioListener Exist~!! name = " + component.name, new object[0]);
			UnityEngine.Object.DestroyImmediate(component, bObject);
		}
	}

	public float CalcDistanceToCurrentAudioListener(Vector3 fromPos)
	{
		return Vector3.Distance(this.CurrentAudioListener.transform.position, fromPos);
	}

	public AudioClip GetTempClip()
	{
		return this._tempclip;
	}
}
