using System;
using UnityEngine;

[Serializable]
public class MapTriggerInfo
{
	public int m_MapIdx;

	public string m_Name = string.Empty;

	public GameObject m_goMap;

	public bool Loaded
	{
		get
		{
			return !(this.m_goMap == null) && this.m_goMap.transform.childCount > 0;
		}
	}

	public EventTrigger[] GetTrigger()
	{
		if (this.m_goMap == null)
		{
			return null;
		}
		return this.m_goMap.GetComponentsInChildren<EventTrigger>(true);
	}

	public EventTrigger_Game[] GetTrigger_Game()
	{
		if (this.m_goMap == null)
		{
			return null;
		}
		return this.m_goMap.GetComponentsInChildren<EventTrigger_Game>(true);
	}

	public void SetActive(bool bActive)
	{
		for (int i = 0; i < this.m_goMap.transform.childCount; i++)
		{
			Transform child = this.m_goMap.transform.GetChild(i);
			child.gameObject.SetActive(bActive);
		}
		this.m_goMap.SetActive(bActive);
	}

	public void SetActive(bool bActive, string TriggerName)
	{
		Transform transform = this.m_goMap.transform.Find(TriggerName);
		if (transform != null)
		{
			transform.gameObject.SetActive(bActive);
		}
	}

	public bool IsStandByEventTrigger()
	{
		bool result = false;
		for (int i = 0; i < this.m_goMap.transform.childCount; i++)
		{
			Transform child = this.m_goMap.transform.GetChild(i);
			GameObject gameObject = child.gameObject;
			EventTrigger component = gameObject.GetComponent<EventTrigger>();
			if (component != null && component.IsEnable())
			{
				return true;
			}
		}
		return result;
	}

	public void SetCallFunc()
	{
		for (int i = 0; i < this.m_goMap.transform.childCount; i++)
		{
			Transform child = this.m_goMap.transform.GetChild(i);
			GameObject gameObject = child.gameObject;
			EventTrigger component = gameObject.GetComponent<EventTrigger>();
			if (component != null && component.IsEnable() && component.TriggerOn)
			{
				TsLog.Log("[EventTrigger] Collect ChangeStage Func: " + this.m_goMap.name, new object[0]);
				component.CollectLoadStageFunc();
			}
		}
	}

	public bool IsVaild(string EventTriggerName)
	{
		EventTrigger[] trigger = this.GetTrigger();
		EventTrigger[] array = trigger;
		for (int i = 0; i < array.Length; i++)
		{
			EventTrigger eventTrigger = array[i];
			if (eventTrigger.name == EventTriggerName)
			{
				return true;
			}
		}
		return false;
	}

	public int EventTriggerCount()
	{
		EventTrigger[] trigger = this.GetTrigger();
		if (trigger == null)
		{
			return 0;
		}
		return trigger.Length;
	}

	public void DestoryTriggerObject()
	{
		if (this.Loaded)
		{
			EventTriggerHelper.DestoryChildObject(this.m_goMap.transform);
		}
	}

	public void AttachTriggerObject(GameObject[] gos, bool bEnable)
	{
		for (int i = 0; i < gos.Length; i++)
		{
			GameObject gameObject = gos[i];
			EventTriggerItem[] components = gameObject.GetComponents<EventTriggerItem>();
			EventTriggerItem[] array = components;
			for (int j = 0; j < array.Length; j++)
			{
				EventTriggerItem eventTriggerItem = array[j];
				TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.WorldScene, eventTriggerItem.gameObject);
			}
			gameObject.transform.parent = this.m_goMap.transform;
			gameObject.SetActive(false);
			EventTrigger_Game component = gameObject.GetComponent<EventTrigger_Game>();
			if (component != null)
			{
				EventTriggerInfo eventTriggerInfo = NrTSingleton<NrEventTriggerInfoManager>.Instance.GetEventTriggerInfo(gameObject.name);
				if (eventTriggerInfo != null)
				{
					eventTriggerInfo.EventTrigger = component;
					component.EventTriggerUnique = eventTriggerInfo.Unique;
				}
				component.Enable(bEnable);
			}
		}
	}
}
