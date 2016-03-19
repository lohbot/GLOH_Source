using System;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerMapInfo : MonoBehaviour
{
	public int m_TotalSectionRecord;

	[SerializeField]
	public Dictionary<int, MAP_INFO> m_DungeonInfo = new Dictionary<int, MAP_INFO>();

	private static EventTriggerMapInfo _Instance;

	public static EventTriggerMapInfo Instance
	{
		get
		{
			if (EventTriggerMapInfo._Instance == null)
			{
				GameObject gameObject = GameObject.Find(typeof(EventTriggerMapInfo).Name);
				if (gameObject == null)
				{
					gameObject = new GameObject(typeof(EventTriggerMapInfo).Name);
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					EventTriggerMapInfo._Instance = gameObject.AddComponent<EventTriggerMapInfo>();
					UnityEngine.Object.DontDestroyOnLoad(EventTriggerMapInfo._Instance);
				}
				EventTriggerMapInfo._Instance = gameObject.GetComponent<EventTriggerMapInfo>();
			}
			return EventTriggerMapInfo._Instance;
		}
	}
}
