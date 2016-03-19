using System;
using UnityEngine;

public static class TsAudioCreator
{
	public static TsAudio Create(EAudioType eAudioType)
	{
		TsAudio.BaseData baseData = TsAudio.BaseData.Create(eAudioType);
		return TsAudioCreator.Create(baseData);
	}

	public static TsAudio Create(TsAudio.BaseData baseData)
	{
		if (baseData == null)
		{
			TsLog.LogError("TsAudioCreator.Create() baseData == null   !!!!!", new object[0]);
			return null;
		}
		TsAudio tsAudio;
		switch (baseData.AudioType)
		{
		case EAudioType.SFX:
			tsAudio = new TsAudioSFX(baseData.AudioType);
			break;
		case EAudioType.BGM:
			tsAudio = new TsAudioBGM(baseData.AudioType);
			break;
		case EAudioType.AMBIENT:
			tsAudio = new TsAudioAmbient(baseData.AudioType);
			break;
		case EAudioType.UI:
			tsAudio = new TsAudioUI(baseData.AudioType);
			break;
		case EAudioType.VOICE:
			tsAudio = new TsAudioVoice(baseData.AudioType);
			break;
		case EAudioType.SYSTEM:
			tsAudio = new TsAudioSystem(baseData.AudioType);
			break;
		case EAudioType.GAME_DRAMA:
			tsAudio = new TsAudioGameDrama(baseData.AudioType);
			break;
		case EAudioType.ENVIRONMENT:
			tsAudio = new TsAudioEnvironment(baseData.AudioType);
			break;
		default:
			TsLog.Log("Check the EAudioType~! is Invalid Value~! EAudioType= " + baseData.AudioType, new object[0]);
			return null;
		}
		tsAudio._InitBaseData(baseData);
		return tsAudio;
	}

	public static GameObject CreateGameObjectWithAudio(TsAudio.BaseData baseData, AudioClip clip)
	{
		return TsAudioCreator.CreateGameObjectWithAudio(null, baseData, clip, false, false);
	}

	public static GameObject CreateGameObjectWithAudio(string goName, TsAudio.BaseData baseData, AudioClip clip)
	{
		return TsAudioCreator.CreateGameObjectWithAudio(goName, baseData, clip, false, false);
	}

	public static GameObject CreateGameObjectWithAudio(string goName, TsAudio.BaseData baseData, AudioClip clip, bool loop, bool isDontDestroyOnLoad)
	{
		if (string.IsNullOrEmpty(goName))
		{
			goName = "_Audio: " + clip.name;
		}
		TsAudio tsAudio = TsAudioCreator.Create(baseData);
		if (tsAudio == null)
		{
			TsLog.LogError("CreateGameObjectWithAudio()~! Failed Create~! goName= " + goName, new object[0]);
			return null;
		}
		GameObject gameObject = new GameObject(goName, new Type[]
		{
			typeof(AudioSource)
		});
		tsAudio.RefAudioSource = gameObject.audio;
		tsAudio.RefAudioClip = clip;
		TsAudioAdapter tsAudioAdapter = TsAudioAdapter.TryToAddAdapter(gameObject, tsAudio);
		tsAudioAdapter.GetAudioEx().baseData.Loop = loop;
		tsAudioAdapter.GetAudioEx().baseData.IsDontDestroyOnLoad = isDontDestroyOnLoad;
		return gameObject;
	}
}
