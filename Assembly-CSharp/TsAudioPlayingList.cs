using System;
using System.Collections.Generic;
using UnityEngine;

public static class TsAudioPlayingList
{
	private struct Item
	{
		public int frame;

		public GameObject go;

		public Item(int f, GameObject g)
		{
			this.frame = f;
			this.go = g;
		}
	}

	private static Dictionary<string, TsAudioPlayingList.Item> dic = new Dictionary<string, TsAudioPlayingList.Item>();

	public static bool isPlayed(string kName, int inFrames)
	{
		TsAudioPlayingList.Item item;
		return TsAudioPlayingList.dic.TryGetValue(kName, out item) && Time.frameCount - item.frame < inFrames;
	}

	public static bool isPlaying(string kName)
	{
		TsAudioPlayingList.Item value;
		if (TsAudioPlayingList.dic.TryGetValue(kName, out value))
		{
			if (Time.frameCount - value.frame < 5)
			{
				return true;
			}
			value.frame = Time.frameCount;
			TsAudioPlayingList.dic[kName] = value;
		}
		return false;
	}

	public static GameObject GetPlayingGo(string kName, bool updateFrame)
	{
		TsAudioPlayingList.Item item;
		if (TsAudioPlayingList.dic.TryGetValue(kName, out item))
		{
			if (updateFrame)
			{
				item.frame = Time.frameCount;
			}
			return item.go;
		}
		return null;
	}

	public static void Add(string kName, GameObject go)
	{
		if (TsAudioPlayingList.dic.ContainsKey(kName))
		{
			return;
		}
		TsAudioPlayingList.dic.Add(kName, new TsAudioPlayingList.Item(Time.frameCount, go));
	}

	public static void Remove(string kName)
	{
		TsAudioPlayingList.dic.Remove(kName);
	}

	public static void ClearList()
	{
		TsAudioPlayingList.dic.Clear();
	}
}
