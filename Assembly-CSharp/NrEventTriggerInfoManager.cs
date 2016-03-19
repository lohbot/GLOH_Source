using System;
using System.Collections.Generic;

public class NrEventTriggerInfoManager : NrTSingleton<NrEventTriggerInfoManager>
{
	public const int MAX_EVENTTRIGGER_NAME_LENGTH = 20;

	private List<long> _EventTriggerActionFlag = new List<long>();

	public Dictionary<int, EventTriggerInfo> _EventTriggerInfo = new Dictionary<int, EventTriggerInfo>();

	public Dictionary<string, EventTriggerInfo> _EventTriggerKeyInfo = new Dictionary<string, EventTriggerInfo>();

	private Dictionary<int, bool> _ActiveEventTrigger = new Dictionary<int, bool>();

	private List<EventTrigger_Game> _OnTriggers = new List<EventTrigger_Game>();

	public Dictionary<int, EventTriggerInfo> m_EventTriggerInfo
	{
		get
		{
			return this._EventTriggerInfo;
		}
	}

	public bool m_IsLoad
	{
		get
		{
			return this._EventTriggerInfo.Count > 0;
		}
	}

	public int m_Count
	{
		get
		{
			return this._EventTriggerInfo.Count;
		}
	}

	private NrEventTriggerInfoManager()
	{
	}

	public EventTriggerInfo GetInfo(int Index)
	{
		int num = 0;
		foreach (EventTriggerInfo current in this._EventTriggerInfo.Values)
		{
			if (num == Index)
			{
				return current;
			}
			num++;
		}
		return null;
	}

	public EventTriggerInfo GetInfo(string name)
	{
		foreach (EventTriggerInfo current in this._EventTriggerInfo.Values)
		{
			if (current.Name == name)
			{
				return current;
			}
		}
		return null;
	}

	public bool Initialize()
	{
		this.Clear();
		return true;
	}

	public void Clear()
	{
		if (this._EventTriggerInfo.Count > 0)
		{
			this._EventTriggerInfo.Clear();
		}
		if (this._EventTriggerKeyInfo.Count > 0)
		{
			this._EventTriggerKeyInfo.Clear();
		}
		if (this._ActiveEventTrigger.Count > 0)
		{
			this._ActiveEventTrigger.Clear();
		}
		if (this._OnTriggers.Count > 0)
		{
			this._OnTriggers.Clear();
		}
	}

	public void SendOffEventTrigger(int Unique)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(Unique);
		if (eventTriggerInfo == null)
		{
			return;
		}
		this._OnTriggers.Remove(eventTriggerInfo.EventTrigger);
	}

	public void SendOnEventTrigger(int Unique)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(Unique);
		if (eventTriggerInfo == null)
		{
			return;
		}
		if (!this._OnTriggers.Contains(eventTriggerInfo.EventTrigger))
		{
			this._OnTriggers.Add(eventTriggerInfo.EventTrigger);
		}
	}

	public void SendActionEventTrigger(int Unique)
	{
		if (this.GetEventTriggerInfo(Unique) == null)
		{
			return;
		}
	}

	public bool SetTriggerInfo(string Name, EventTriggerInfo NewInfo)
	{
		if (NewInfo == null)
		{
			return false;
		}
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(Name);
		if (eventTriggerInfo != null)
		{
			eventTriggerInfo.Unique = NewInfo.Unique;
			eventTriggerInfo.Name = NewInfo.Name;
			eventTriggerInfo.MapIdx = NewInfo.MapIdx;
			eventTriggerInfo.Flag.Set(NewInfo.Flag.Get());
			eventTriggerInfo.ATB = NewInfo.ATB;
			eventTriggerInfo.QuestTextIndex = NewInfo.QuestTextIndex;
			eventTriggerInfo.Comment = NewInfo.Comment;
		}
		return true;
	}

	public bool SetTriggerInfo(int Unique, EventTriggerInfo NewInfo)
	{
		if (NewInfo == null)
		{
			return false;
		}
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(Unique);
		if (eventTriggerInfo != null)
		{
			eventTriggerInfo.Unique = NewInfo.Unique;
			eventTriggerInfo.Name = NewInfo.Name;
			eventTriggerInfo.MapIdx = NewInfo.MapIdx;
			eventTriggerInfo.Flag.Set(NewInfo.Flag.Get());
			eventTriggerInfo.ATB = NewInfo.ATB;
			eventTriggerInfo.QuestTextIndex = NewInfo.QuestTextIndex;
			eventTriggerInfo.Comment = NewInfo.Comment;
		}
		return true;
	}

	public bool NewTriggerInfo(EventTriggerInfo Info)
	{
		this.AddTriggerInfo(Info);
		return true;
	}

	public bool AddTriggerInfo(EventTriggerInfo Info)
	{
		if (!this._EventTriggerInfo.ContainsKey(Info.Unique))
		{
			this._EventTriggerInfo.Add(Info.Unique, Info);
		}
		if (!this._EventTriggerKeyInfo.ContainsKey(Info.Name))
		{
			this._EventTriggerKeyInfo.Add(Info.Name, Info);
		}
		else if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.LogWarning("EventTriggerInfo Name Error: " + Info.Name, new object[0]);
		}
		return true;
	}

	public EventTriggerInfo GetEventTriggerInfo(string _EventTriggerName)
	{
		if (this._EventTriggerKeyInfo.ContainsKey(_EventTriggerName))
		{
			return this._EventTriggerKeyInfo[_EventTriggerName];
		}
		return null;
	}

	public EventTriggerInfo GetEventTriggerInfo(int Unique)
	{
		if (this._EventTriggerInfo.ContainsKey(Unique))
		{
			return this._EventTriggerInfo[Unique];
		}
		return null;
	}

	public string GetQuestConditionText(string EventTriggerName)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerName);
		return this.GetQuestConditionText(eventTriggerInfo);
	}

	public string GetQuestConditionText(int EventTriggerUnique)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerUnique);
		return this.GetQuestConditionText(eventTriggerInfo);
	}

	private string GetQuestConditionText(EventTriggerInfo Data)
	{
		if (Data == null)
		{
			return "Ʈ���� �˻�";
		}
		return "Ʈ���� �˻�";
	}

	public int GetUnique(string EventTriggerName)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerName);
		if (eventTriggerInfo != null)
		{
			return eventTriggerInfo.Unique;
		}
		return 0;
	}

	public void SetEventTriggerFlag(short Arrayindex, long Flag)
	{
		if (Arrayindex < 0)
		{
			return;
		}
		if (this._EventTriggerActionFlag.Count <= (int)Arrayindex)
		{
			int num = (int)Arrayindex - this._EventTriggerActionFlag.Count + 1;
			for (int i = 0; i < num; i++)
			{
				this._EventTriggerActionFlag.Add(0L);
			}
		}
		this._EventTriggerActionFlag[(int)Arrayindex] = Flag;
	}

	public bool IsEventTriggerFlag(int EventTriggerUnique)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerUnique);
		return eventTriggerInfo != null && eventTriggerInfo.Flag.IsFlag(this._EventTriggerActionFlag.ToArray());
	}

	public int GetInfoCount()
	{
		return this._EventTriggerInfo.Count;
	}

	public int[] GetDungeonEventTriggerUnique(int MapIdx)
	{
		List<int> list = new List<int>();
		foreach (EventTriggerInfo current in this._EventTriggerInfo.Values)
		{
			if (current.MapIdx == MapIdx)
			{
				list.Add(current.Unique);
			}
		}
		return list.ToArray();
	}

	public bool IsVaildMapInfo(int MapIdx)
	{
		foreach (EventTriggerInfo current in this._EventTriggerInfo.Values)
		{
			if (current.MapIdx == MapIdx)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsATB(int EventTriggerUnique, EventTriggerInfo.EventTriggerATB atb)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerUnique);
		return eventTriggerInfo != null && EventTriggerInfo.IsATB(eventTriggerInfo.ATB, atb);
	}

	public bool IsEventTriggerInfo(string EventTriggerName)
	{
		return this._EventTriggerKeyInfo.ContainsKey(EventTriggerName);
	}

	public bool DelEventTriggerInfo(string EventTriggerName)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerName);
		if (eventTriggerInfo == null)
		{
			return false;
		}
		this._EventTriggerInfo.Remove(eventTriggerInfo.Unique);
		this._EventTriggerKeyInfo.Remove(eventTriggerInfo.Name);
		return true;
	}

	public List<EventTrigger_Game> GetOnTrigger()
	{
		return this._OnTriggers;
	}

	public void OnTrigger(string EventTriggerName)
	{
		EventTriggerInfo eventTriggerInfo = this.GetEventTriggerInfo(EventTriggerName);
		if (eventTriggerInfo == null)
		{
			return;
		}
		if (!eventTriggerInfo.EventTrigger.TriggerOn)
		{
			eventTriggerInfo.EventTrigger.OnTrigger();
			EventTriggerMapManager.Instance.ActiveTriggerInfo(eventTriggerInfo.EventTrigger);
		}
	}
}
