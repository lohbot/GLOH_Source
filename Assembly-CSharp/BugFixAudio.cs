using System;
using UnityEngine;

public static class BugFixAudio
{
	private static GameObject BugFixAS;

	private static GameObject PlayAudioOnceRoot;

	public static GameObject PlayOnceRoot
	{
		get
		{
			return BugFixAudio.PlayAudioOnceRoot;
		}
	}

	static BugFixAudio()
	{
		BugFixAudio.BugFixAS = (Resources.Load("Common/BugFixAudioSource") as GameObject);
		if (BugFixAudio.BugFixAS == null)
		{
			Debug.LogError("BugFixAudio creation failed!");
		}
		else
		{
			BugFixAudio.BugFixAS.transform.position = Vector3.zero;
		}
		BugFixAudio.PlayAudioOnceRoot = new GameObject("@PlayAudioOnce");
		UnityEngine.Object.DontDestroyOnLoad(BugFixAudio.PlayAudioOnceRoot);
	}

	public static GameObject NewBugFixAudioSource(Transform parent)
	{
		if (parent == null)
		{
			return UnityEngine.Object.Instantiate(BugFixAudio.BugFixAS) as GameObject;
		}
		return UnityEngine.Object.Instantiate(BugFixAudio.BugFixAS, parent.transform.position, Quaternion.identity) as GameObject;
	}

	public static GameObject NewAutoDestroyAudioSource(TsAudio audioEx, Transform parent)
	{
		if (audioEx.baseData.Tag == TsAudioManager.MINI_SOUND)
		{
			return BugFixAudio.NewAudioSource(audioEx, TsAudioManager.Instance.CurrentAudioListener.transform, new Vector3(0f, 0f, 0f));
		}
		return BugFixAudio.NewAudioSource(audioEx, parent, parent.position);
	}

	public static GameObject NewAutoDestroyAudioSource(TsAudio audioEx, Vector3 position)
	{
		if (audioEx.baseData.Tag == TsAudioManager.MINI_SOUND)
		{
			return BugFixAudio.NewAudioSource(audioEx, TsAudioManager.Instance.CurrentAudioListener.transform, new Vector3(0f, 0f, 0f));
		}
		return BugFixAudio.NewAudioSource(audioEx, BugFixAudio.PlayAudioOnceRoot.transform, position);
	}

	private static GameObject NewAudioSource(TsAudio audioEx, Transform parent, Vector3 position)
	{
		GameObject gameObject = null;
		try
		{
			if (audioEx.RefAudioClip == null)
			{
				Debug.LogWarning("BugFixAudio RefAudioClip is null.");
				GameObject result = null;
				return result;
			}
			float t = audioEx.RefAudioClip.length + audioEx.baseData.DelayPlayTime;
			if (audioEx.RefAudioSource == null || audioEx.baseData.AudioType == EAudioType.UI)
			{
				gameObject = new GameObject(audioEx.RefAudioClip.name, new Type[]
				{
					typeof(AudioSource)
				});
			}
			else if (BugFixAudio.BugFixAS != null)
			{
				gameObject = (UnityEngine.Object.Instantiate(BugFixAudio.BugFixAS, position, Quaternion.identity) as GameObject);
			}
			if (gameObject == null)
			{
				Debug.LogError("New AduiSource failed!");
				GameObject result = null;
				return result;
			}
			gameObject.transform.parent = parent;
			if (audioEx.baseData.Tag == TsAudioManager.MINI_SOUND)
			{
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			gameObject.name = audioEx.RefAudioClip.name;
			gameObject.audio.clip = audioEx.RefAudioClip;
			gameObject.audio.loop = audioEx.baseData.Loop;
			if (!audioEx.baseData.Loop)
			{
				UnityEngine.Object.Destroy(gameObject, t);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return gameObject;
	}
}
