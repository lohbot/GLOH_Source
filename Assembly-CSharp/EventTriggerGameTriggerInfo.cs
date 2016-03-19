using System;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerGameTriggerInfo : MonoBehaviour
{
	[Serializable]
	public class TriggerInfoFlag
	{
		public EventTriggerInfo info;

		public int Flag;
	}

	private static EventTriggerGameTriggerInfo _Instance;

	public List<EventTriggerInfo> m_GameTriggerInfoList = new List<EventTriggerInfo>();

	public List<EventTriggerGameTriggerInfo.TriggerInfoFlag> FlagList = new List<EventTriggerGameTriggerInfo.TriggerInfoFlag>();

	public static EventTriggerGameTriggerInfo Instance
	{
		get
		{
			if (EventTriggerGameTriggerInfo._Instance == null)
			{
				EventTriggerGameTriggerInfo._Instance = EventTriggerGameTriggerInfo.GetInstance();
			}
			EventTriggerGameTriggerInfo._Instance.CheckInfo();
			return EventTriggerGameTriggerInfo._Instance;
		}
	}

	public bool IsLoad
	{
		get
		{
			return this.Count > 0;
		}
	}

	public int Count
	{
		get
		{
			return NrTSingleton<NrEventTriggerInfoManager>.Instance.m_EventTriggerInfo.Count;
		}
	}

	private static EventTriggerGameTriggerInfo GetInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(EventTriggerGameTriggerInfo).Name);
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(EventTriggerGameTriggerInfo).Name, new Type[]
			{
				typeof(EventTriggerGameTriggerInfo)
			});
		}
		EventTriggerGameTriggerInfo component = gameObject.GetComponent<EventTriggerGameTriggerInfo>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UnityEngine.Object.DontDestroyOnLoad(component);
		return component;
	}

	public void Start()
	{
		EventTriggerGameTriggerInfo.GetInstance();
	}

	public void Reset()
	{
		EventTriggerGameTriggerInfo._Instance = EventTriggerGameTriggerInfo.GetInstance();
	}

	public void Clear()
	{
		this.m_GameTriggerInfoList.Clear();
		this.FlagList.Clear();
		NrTSingleton<NrEventTriggerInfoManager>.Instance.Clear();
	}

	public void CheckInfo()
	{
		if (NrTSingleton<NrEventTriggerInfoManager>.Instance.m_Count <= 0)
		{
			foreach (EventTriggerInfo info in this.m_GameTriggerInfoList)
			{
				int num = this.FlagList.FindIndex((EventTriggerGameTriggerInfo.TriggerInfoFlag value) => value.info.Name == info.Name);
				if (num >= 0)
				{
					info.Flag.Set((long)this.FlagList[num].Flag);
				}
				NrTSingleton<NrEventTriggerInfoManager>.Instance.AddTriggerInfo(info);
			}
		}
		else
		{
			foreach (EventTriggerInfo info in this.m_GameTriggerInfoList)
			{
				if (info.Flag.Get() <= 0L)
				{
					int num2 = this.FlagList.FindIndex((EventTriggerGameTriggerInfo.TriggerInfoFlag value) => value.info.Name == info.Name);
					if (num2 >= 0)
					{
						info.Flag.Set((long)this.FlagList[num2].Flag);
					}
				}
			}
		}
	}

	public void Load()
	{
		if (NrTSingleton<NrEventTriggerInfoManager>.Instance.m_Count > 0)
		{
			foreach (EventTriggerInfo current in NrTSingleton<NrEventTriggerInfoManager>.Instance.m_EventTriggerInfo.Values)
			{
				this.Add(current);
			}
		}
	}

	public void Set(string Name, EventTriggerInfo NewInfo)
	{
		EventTriggerInfo info = this.GetInfo(Name);
		if (info != null)
		{
			info.Unique = NewInfo.Unique;
			info.Name = NewInfo.Name;
			info.MapIdx = NewInfo.MapIdx;
			info.Flag.Set(NewInfo.Flag.Get());
			info.ATB = NewInfo.ATB;
			info.QuestTextIndex = NewInfo.QuestTextIndex;
			info.Comment = NewInfo.Comment;
			int num = this.FlagList.FindIndex((EventTriggerGameTriggerInfo.TriggerInfoFlag value) => value.info.Name == info.Name);
			if (num >= 0)
			{
				this.FlagList[num].Flag = (int)info.Flag.Get();
			}
			NrTSingleton<NrEventTriggerInfoManager>.Instance.SetTriggerInfo(Name, NewInfo);
		}
	}

	public void Set(int Unique, EventTriggerInfo NewInfo)
	{
		EventTriggerInfo info = this.GetInfo(Unique);
		if (info != null)
		{
			this.Set(info.Name, NewInfo);
		}
	}

	public void New(EventTriggerInfo info)
	{
		EventTriggerInfo eventTriggerInfo = new EventTriggerInfo();
		eventTriggerInfo.Unique = info.Unique;
		eventTriggerInfo.Name = info.Name;
		eventTriggerInfo.MapIdx = info.MapIdx;
		eventTriggerInfo.Flag.Set(info.Flag.Get());
		eventTriggerInfo.ATB = info.ATB;
		eventTriggerInfo.QuestTextIndex = info.QuestTextIndex;
		eventTriggerInfo.Comment = info.Comment;
		eventTriggerInfo.Flag.Set(info.Flag.Get());
		this.Add(eventTriggerInfo);
		NrTSingleton<NrEventTriggerInfoManager>.Instance.NewTriggerInfo(eventTriggerInfo);
	}

	public void Add(EventTriggerInfo info)
	{
		if (!this.IsInfo(info.Name))
		{
			this.m_GameTriggerInfoList.Add(info);
		}
	}

	public bool Del(string Name)
	{
		EventTriggerInfo eventTriggerInfo = null;
		foreach (EventTriggerInfo current in this.m_GameTriggerInfoList)
		{
			if (current.Name == Name)
			{
				eventTriggerInfo = current;
				break;
			}
		}
		if (eventTriggerInfo != null)
		{
			this.m_GameTriggerInfoList.Remove(eventTriggerInfo);
			int num = this.FlagList.FindIndex((EventTriggerGameTriggerInfo.TriggerInfoFlag value) => value.info.Name == Name);
			if (num >= 0)
			{
				this.FlagList.Remove(this.FlagList[num]);
			}
			return NrTSingleton<NrEventTriggerInfoManager>.Instance.DelEventTriggerInfo(Name);
		}
		return false;
	}

	public bool IsInfo(string Name)
	{
		foreach (EventTriggerInfo current in this.m_GameTriggerInfoList)
		{
			if (current.Name.Equals(Name))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInfo(int Unique)
	{
		foreach (EventTriggerInfo current in this.m_GameTriggerInfoList)
		{
			if (current.Unique == Unique)
			{
				return true;
			}
		}
		return false;
	}

	public EventTriggerInfo GetInfoIndex(int index)
	{
		if (this.m_GameTriggerInfoList.Count <= index)
		{
			return null;
		}
		return this.m_GameTriggerInfoList[index];
	}

	public EventTriggerInfo GetInfo(int Unique)
	{
		foreach (EventTriggerInfo current in this.m_GameTriggerInfoList)
		{
			if (current.Unique == Unique)
			{
				return current;
			}
		}
		return null;
	}

	public EventTriggerInfo GetInfo(string Name)
	{
		foreach (EventTriggerInfo current in this.m_GameTriggerInfoList)
		{
			if (current.Name.Equals(Name))
			{
				return current;
			}
		}
		return null;
	}

	public int GetUnique(string EventTriggerName)
	{
		return NrTSingleton<NrEventTriggerInfoManager>.Instance.GetUnique(EventTriggerName);
	}
}
