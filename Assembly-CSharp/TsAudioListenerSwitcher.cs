using System;
using UnityEngine;

public class TsAudioListenerSwitcher
{
	public static readonly string s_attachedGOName = ".SOUND_AUDIOLISTENER";

	private GameObject _targetGO;

	private GameObject _attachedChildGO;

	private AudioListener _audioListener;

	public string TargetGameObjectName
	{
		get
		{
			return (!(this._targetGO != null)) ? "Invalid~!" : this._targetGO.name;
		}
	}

	public AudioListener TargetAudioListener
	{
		get
		{
			return this._audioListener;
		}
	}

	private string AttachedGameObjectName
	{
		get
		{
			if (this._targetGO == null)
			{
				return string.Empty;
			}
			return this._targetGO.name + TsAudioListenerSwitcher.s_attachedGOName;
		}
	}

	public TsAudioListenerSwitcher(string gameObjectName)
	{
		this._targetGO = GameObject.Find(gameObjectName);
	}

	public TsAudioListenerSwitcher(GameObject targetGO)
	{
		this._targetGO = targetGO;
	}

	public bool Switch()
	{
		if (this._targetGO == null)
		{
			TsLog.LogError("Error~! TsAudioListenerSwitcher.Switch() reason = Cannot Found targetGameObject~!", new object[0]);
			return false;
		}
		if (this._targetGO.name.Equals(this.AttachedGameObjectName))
		{
			if (this._targetGO.transform.parent == null)
			{
				return false;
			}
			this._targetGO = this._targetGO.transform.parent.gameObject;
		}
		AudioListener component = this._targetGO.GetComponent<AudioListener>();
		Transform transform = this._targetGO.transform.FindChild(this.AttachedGameObjectName);
		if (transform == null)
		{
			this._attachedChildGO = new GameObject(this.AttachedGameObjectName, new Type[]
			{
				typeof(AudioListener)
			});
			this._attachedChildGO.transform.position = this._targetGO.transform.position;
			this._attachedChildGO.transform.rotation = this._targetGO.transform.rotation;
			this._attachedChildGO.transform.parent = this._targetGO.transform;
		}
		else
		{
			this._attachedChildGO = transform.gameObject;
		}
		try
		{
			this._audioListener = this._attachedChildGO.GetComponent<AudioListener>();
			this._audioListener.enabled = true;
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.ToString(), new object[0]);
			return false;
		}
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		TsAudioManager.Instance.OnSuccess_AudioListenerSwitch(this);
		return true;
	}

	public void ApplyListenerRotator()
	{
		if (this._attachedChildGO != null)
		{
			this._attachedChildGO.AddComponent<TsAudioListenerRotator>();
		}
	}

	public void DetachSwitcher()
	{
		if (this._attachedChildGO != null)
		{
			UnityEngine.Object.Destroy(this._attachedChildGO);
		}
	}

	public override bool Equals(object obj)
	{
		TsAudioListenerSwitcher tsAudioListenerSwitcher = obj as TsAudioListenerSwitcher;
		return tsAudioListenerSwitcher != null && !(this.TargetAudioListener != tsAudioListenerSwitcher.TargetAudioListener);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
