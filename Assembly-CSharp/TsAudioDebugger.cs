using System;
using System.Collections.Generic;
using UnityEngine;

public class TsAudioDebugger : MonoBehaviour
{
	[SerializeField]
	private List<AudioListener> _currentaudioListeners = new List<AudioListener>();

	[SerializeField]
	private List<TsAudioAdapter> _currentPlayAudios = new List<TsAudioAdapter>();

	[SerializeField]
	private List<TsAudioAdapter> _currentStopAudios = new List<TsAudioAdapter>();

	[SerializeField]
	private bool _enableDebug;

	private float _lastUpdateTime;

	private float _updateIntervalTime = 1f;

	public AudioListener[] AudioListenerArray
	{
		get
		{
			return this._currentaudioListeners.ToArray();
		}
	}

	public TsAudioAdapter[] PlayAudioArray
	{
		get
		{
			return this._currentPlayAudios.ToArray();
		}
	}

	public TsAudioAdapter[] StopAudioArray
	{
		get
		{
			return this._currentStopAudios.ToArray();
		}
	}

	public bool EnableDebug
	{
		get
		{
			return this._enableDebug;
		}
		set
		{
			this._enableDebug = value;
			if (this._enableDebug)
			{
				this._lastUpdateTime = 0f;
				this._SearchAudioListeners();
				this._SearchAudioAdapter();
			}
			else
			{
				this._currentaudioListeners.Clear();
			}
		}
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (!Application.isEditor)
		{
			return;
		}
		if (!this._enableDebug)
		{
			return;
		}
		if (Time.time - this._lastUpdateTime < this._updateIntervalTime)
		{
			return;
		}
		this._lastUpdateTime = Time.time;
		this._SearchAudioListeners();
		this._SearchAudioAdapter();
	}

	private void _SearchAudioListeners()
	{
		this._currentaudioListeners.Clear();
		AudioListener[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
		if (array.Length >= 1)
		{
			this._currentaudioListeners.AddRange(array);
		}
	}

	private void _SearchAudioAdapter()
	{
		this._currentPlayAudios.Clear();
		this._currentStopAudios.Clear();
		TsAudioAdapter[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapter)) as TsAudioAdapter[];
		if (array.Length >= 1)
		{
			TsAudioAdapter[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TsAudioAdapter tsAudioAdapter = array2[i];
				if (tsAudioAdapter.GetAudioEx().isPlaying)
				{
					this._currentPlayAudios.Add(tsAudioAdapter);
				}
				if (tsAudioAdapter.GetAudioEx().IsForceStop)
				{
					this._currentStopAudios.Add(tsAudioAdapter);
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (Camera.main == null)
		{
			return;
		}
		Color color = Gizmos.color;
		Transform transform = Camera.main.transform;
		foreach (AudioListener current in this._currentaudioListeners)
		{
			if (!(current == null))
			{
				if (current.enabled)
				{
					Gizmos.color = Color.blue;
				}
				else
				{
					Gizmos.color = Color.red;
				}
				Gizmos.DrawLine(transform.position, current.transform.position);
			}
		}
		Gizmos.color = color;
	}
}
