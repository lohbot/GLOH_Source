using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EventTriggerMapManager : MonoBehaviour
{
	private class EventTriggerLoadingInfo
	{
		public int DungeonID;

		public int SectionID;
	}

	private class LoadingTriggerInfo
	{
		public MapTriggerInfo m_MapInfo;

		public int m_MapIdx;

		public LoadingTriggerInfo(MapTriggerInfo Info, int MapIdx)
		{
			this.m_MapInfo = Info;
			this.m_MapIdx = MapIdx;
		}
	}

	public const int GENERAL_MAPIDX = -1;

	public const int MAX_EVENTTRIGGER_NAME_LENGTH = 127;

	public bool m_TestMode;

	public bool m_DebugMode;

	[HideInInspector]
	public Dictionary<int, EventTrigger> _TriggerList = new Dictionary<int, EventTrigger>();

	public List<MapTriggerInfo> _ActiveMap = new List<MapTriggerInfo>();

	public List<EventTrigger_Game> _ActionEventTrigger = new List<EventTrigger_Game>();

	private static EventTriggerMapManager _instance;

	public List<MapTriggerInfo> m_MapTriggerList = new List<MapTriggerInfo>();

	private int m_CurrentMapIdx;

	private List<EventTriggerMapManager.LoadingTriggerInfo> LoadTriggerBuffer = new List<EventTriggerMapManager.LoadingTriggerInfo>();

	private EventTriggerMapManager.LoadingTriggerInfo LoadingSection;

	private bool LoadingTrigger;

	private bool m_bReload;

	public List<Action> m_PreLoadList = new List<Action>();

	public List<Func<bool>> m_StandbyWaitList = new List<Func<bool>>();

	public List<Action> m_PostLoadList = new List<Action>();

	private bool isActive;

	public static EventTriggerMapManager Instance
	{
		get
		{
			if (EventTriggerMapManager._instance == null)
			{
				EventTriggerMapManager._instance = EventTriggerMapManager.GetInstance();
			}
			return EventTriggerMapManager._instance;
		}
	}

	public bool IsLoad
	{
		get
		{
			return base.transform.childCount > 0;
		}
	}

	public void AddActive(MapTriggerInfo info)
	{
		if (!this._ActiveMap.Contains(info))
		{
			this._ActiveMap.Add(info);
		}
	}

	public void RemoveActive(MapTriggerInfo info)
	{
		if (this._ActiveMap.Contains(info))
		{
			this._ActiveMap.Remove(info);
		}
	}

	public void AddActionTrigger(EventTrigger_Game trigger)
	{
		if (!this._ActionEventTrigger.Contains(trigger))
		{
			this._ActionEventTrigger.Add(trigger);
		}
	}

	public void RemoveActionTrigger(EventTrigger_Game trigger)
	{
		if (this._ActionEventTrigger.Contains(trigger))
		{
			this._ActionEventTrigger.Remove(trigger);
		}
		EventTriggerInfo eventTriggerInfo = NrTSingleton<NrEventTriggerInfoManager>.Instance.GetEventTriggerInfo(trigger.EventTriggerUnique);
		if (eventTriggerInfo != null)
		{
			MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(eventTriggerInfo.MapIdx);
			if (mapTriggerInfo != null && !this._ActiveMap.Contains(mapTriggerInfo))
			{
				mapTriggerInfo.SetActive(false, trigger.name);
			}
		}
	}

	private static EventTriggerMapManager GetInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(EventTriggerMapManager).Name);
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(EventTriggerMapManager).Name, new Type[]
			{
				typeof(EventTriggerMapManager)
			});
		}
		EventTriggerMapManager component = gameObject.GetComponent<EventTriggerMapManager>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UnityEngine.Object.DontDestroyOnLoad(component);
		return component;
	}

	public void Claer()
	{
		UnityEngine.Debug.Log("EventTriggerMapManager Clear");
		EventTriggerHelper.DestoryChildObject(base.transform);
		base.transform.DetachChildren();
		this._ActiveMap.Clear();
		this._ActionEventTrigger.Clear();
		this.m_MapTriggerList.Clear();
	}

	public bool IsLoaded()
	{
		return this.m_MapTriggerList.Count > 0;
	}

	public void Start()
	{
		EventTriggerMapManager.GetInstance();
	}

	public void Reset()
	{
		EventTriggerMapManager._instance = EventTriggerMapManager.GetInstance();
	}

	public void LoadMapTrigger(int MapIdx)
	{
		if (this.m_TestMode)
		{
			return;
		}
		if (this.m_MapTriggerList.Count <= 0)
		{
			this.MakeMapInfo();
			this.LoadMapTrigger(-1);
		}
		if (this.m_CurrentMapIdx == MapIdx)
		{
			return;
		}
		if (this.m_CurrentMapIdx != -1)
		{
			MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(this.m_CurrentMapIdx);
			if (mapTriggerInfo != null)
			{
				mapTriggerInfo.SetActive(false);
				this.RemoveActive(mapTriggerInfo);
			}
		}
		MapTriggerInfo mapTriggerInfo2 = this.GetMapTriggerInfo(MapIdx);
		if (mapTriggerInfo2 == null)
		{
			return;
		}
		EventTriggerMapManager.LoadingTriggerInfo item = new EventTriggerMapManager.LoadingTriggerInfo(mapTriggerInfo2, MapIdx);
		this.LoadTriggerBuffer.Add(item);
		this.Load();
	}

	private void Load()
	{
		if (!this.LoadingTrigger)
		{
			if (this.LoadTriggerBuffer.Count <= 0)
			{
				return;
			}
			EventTriggerMapManager.LoadingTriggerInfo loadingTriggerInfo = this.LoadTriggerBuffer[0];
			if (loadingTriggerInfo == null)
			{
				return;
			}
			MapTriggerInfo mapInfo = loadingTriggerInfo.m_MapInfo;
			this.m_CurrentMapIdx = loadingTriggerInfo.m_MapIdx;
			this.AddActive(mapInfo);
			if (!mapInfo.Loaded && NrTSingleton<NrEventTriggerInfoManager>.Instance.IsVaildMapInfo(this.m_CurrentMapIdx))
			{
				string path = string.Format("EventTrigger_MAP_{0}", this.m_CurrentMapIdx.ToString());
				EventTriggerStageLoader.SetLoadingInfo(path, new EventTriggerStageLoader.LoadComplete(this.LoadTrigger));
				this.LoadingSection = loadingTriggerInfo;
				this.LoadingTrigger = true;
			}
			else
			{
				this.LoadNext(loadingTriggerInfo);
			}
		}
	}

	private void LoadTrigger(GameObject[] goTriggers)
	{
		MapTriggerInfo mapInfo = this.LoadingSection.m_MapInfo;
		if (mapInfo == null)
		{
			return;
		}
		mapInfo.AttachTriggerObject(goTriggers, true);
		this.LoadingTrigger = false;
		this.CheckActiveTrigger(mapInfo.m_MapIdx);
		this.LoadNext(this.LoadingSection);
		if (this.m_bReload && this.LoadTriggerBuffer != null && this.LoadTriggerBuffer.Count == 0)
		{
			this.ReloadPostWork();
		}
	}

	private void LoadNext(EventTriggerMapManager.LoadingTriggerInfo Info)
	{
		this.ActiveEventTriggerLinkToCurrentQuest();
		this.LoadTriggerBuffer.Remove(Info);
		if (this.LoadTriggerBuffer.Count > 0)
		{
			this.Load();
		}
	}

	private void CheckActiveTrigger(int MapIdx)
	{
		MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(MapIdx);
		if (mapTriggerInfo == null)
		{
			return;
		}
		EventTrigger_Game[] trigger_Game = mapTriggerInfo.GetTrigger_Game();
		if (trigger_Game != null)
		{
			EventTrigger_Game[] array = trigger_Game;
			for (int i = 0; i < array.Length; i++)
			{
				EventTrigger_Game eventTrigger_Game = array[i];
				if (eventTrigger_Game.EventConditionList.Count <= 0 && eventTrigger_Game.StateConditionList.Count <= 0)
				{
					eventTrigger_Game.Enable(false);
				}
			}
		}
	}

	public void OnShotDownTrigger()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long charDetail = kMyCharInfo.GetCharDetail(1);
		if (charDetail <= 0L)
		{
			return;
		}
		MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(this.m_CurrentMapIdx);
		if (mapTriggerInfo == null)
		{
			return;
		}
		EventTrigger_Game[] trigger_Game = mapTriggerInfo.GetTrigger_Game();
		if (trigger_Game == null)
		{
			return;
		}
		EventTrigger_Game[] array = trigger_Game;
		for (int i = 0; i < array.Length; i++)
		{
			EventTrigger_Game eventTrigger_Game = array[i];
			EventTrigger_Game component = eventTrigger_Game.GetComponent<EventTrigger_Game>();
			if (!(component == null))
			{
				if ((long)component.EventTriggerUnique == charDetail)
				{
					if (!component.TriggerOn)
					{
						component.OnTrigger();
					}
				}
			}
		}
	}

	public void ActiveEventTriggerLinkToCurrentQuest()
	{
		MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(this.m_CurrentMapIdx);
		if (mapTriggerInfo == null)
		{
			return;
		}
		EventTrigger_Game[] trigger_Game = mapTriggerInfo.GetTrigger_Game();
		if (trigger_Game == null && trigger_Game.Length <= 0)
		{
			return;
		}
		USER_CURRENT_QUEST_INFO[] userCurrentQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetUserCurrentQuestInfo();
		if (userCurrentQuestInfo != null)
		{
			USER_CURRENT_QUEST_INFO[] array = userCurrentQuestInfo;
			for (int i = 0; i < array.Length; i++)
			{
				USER_CURRENT_QUEST_INFO uSER_CURRENT_QUEST_INFO = array[i];
				EventTrigger_Game[] array2 = trigger_Game;
				for (int j = 0; j < array2.Length; j++)
				{
					EventTrigger_Game eventTrigger_Game = array2[j];
					if (eventTrigger_Game.IsQuestCondition(uSER_CURRENT_QUEST_INFO.strQuestUnique))
					{
						eventTrigger_Game.Enable(true);
					}
				}
			}
		}
	}

	public void MakeMapInfo()
	{
		ICollection mapInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo_Col();
		if (mapInfo_Col == null)
		{
			return;
		}
		MapTriggerInfo mapTriggerInfo = new MapTriggerInfo();
		mapTriggerInfo.m_MapIdx = -1;
		mapTriggerInfo.m_goMap = new GameObject(string.Format("[{0}]", -1.ToString()));
		mapTriggerInfo.m_goMap.SetActive(false);
		mapTriggerInfo.m_goMap.transform.parent = base.gameObject.transform;
		mapTriggerInfo.m_Name = "GENERAL";
		this.m_MapTriggerList.Add(mapTriggerInfo);
		foreach (MAP_INFO mAP_INFO in mapInfo_Col)
		{
			mapTriggerInfo = new MapTriggerInfo();
			mapTriggerInfo.m_MapIdx = mAP_INFO.MAP_INDEX;
			mapTriggerInfo.m_goMap = new GameObject(string.Format("[{0}] {1}", mAP_INFO.MAP_INDEX.ToString(), NrTSingleton<MapManager>.Instance.GetMapName(mAP_INFO.MAP_INDEX)));
			mapTriggerInfo.m_goMap.SetActive(false);
			mapTriggerInfo.m_goMap.transform.parent = base.gameObject.transform;
			mapTriggerInfo.m_Name = NrTSingleton<MapManager>.Instance.GetMapName(mAP_INFO.MAP_INDEX);
			this.m_MapTriggerList.Add(mapTriggerInfo);
		}
	}

	public MapTriggerInfo CurrentSectionTriggerInfo()
	{
		return this.GetMapTriggerInfo(this.m_CurrentMapIdx);
	}

	public EventTrigger GetEventTrigger(int MapIdx, string name)
	{
		MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(MapIdx);
		if (mapTriggerInfo == null)
		{
			return null;
		}
		if (mapTriggerInfo.m_goMap == null)
		{
			return null;
		}
		Transform transform = mapTriggerInfo.m_goMap.transform.Find(name);
		if (transform == null)
		{
			return null;
		}
		return transform.gameObject.GetComponent<EventTrigger>();
	}

	public MapTriggerInfo GetMapTriggerInfo(int MapIdx)
	{
		if (this.m_MapTriggerList == null)
		{
			return null;
		}
		return this.m_MapTriggerList.Find((MapTriggerInfo value) => value.m_MapIdx == MapIdx);
	}

	public MapTriggerInfo GetMapTriggerInfo(EventTrigger trigger)
	{
		if (!Application.isEditor)
		{
			return null;
		}
		foreach (MapTriggerInfo current in this.m_MapTriggerList)
		{
			EventTrigger[] trigger2 = current.GetTrigger();
			EventTrigger[] array = trigger2;
			for (int i = 0; i < array.Length; i++)
			{
				EventTrigger x = array[i];
				if (x == trigger)
				{
					return current;
				}
			}
		}
		return null;
	}

	public void ActiveTriggerInfo(EventTrigger trigger)
	{
		MapTriggerInfo mapTriggerInfo = this.GetMapTriggerInfo(trigger);
		if (mapTriggerInfo != null)
		{
			if (!mapTriggerInfo.IsStandByEventTrigger())
			{
				mapTriggerInfo.SetActive(false);
			}
			else
			{
				mapTriggerInfo.SetActive(true);
			}
		}
	}

	public void ReloadMapTrigger()
	{
		UnityEngine.Debug.Log("ReloadMapTrigger : " + this.isActive);
		EventTriggerMapManager.Instance.Claer();
		this.m_bReload = true;
		this.LoadMapTrigger(NrTSingleton<MapManager>.Instance.CurrentMapIndex);
		base.StartCoroutine(EventTriggerStageLoader.LoadEventTrigger());
	}

	private void ReloadPostWork()
	{
		base.StartCoroutine(EventTriggerMapManager.Instance.RunPostLoadWork());
		this.m_bReload = false;
	}

	public void SetRetuenStageFunc(Action preload, Func<bool> standbywait, Action postload)
	{
		if (TsPlatform.IsMobile)
		{
			if (preload != null)
			{
				preload();
			}
			if (standbywait != null)
			{
				standbywait();
			}
			if (postload != null)
			{
				postload();
			}
		}
		else
		{
			this.m_PreLoadList.Add(preload);
			this.m_StandbyWaitList.Add(standbywait);
			this.m_PostLoadList.Add(postload);
		}
	}

	public void SetTriggerActive(bool bActive)
	{
		this.isActive = bActive;
		foreach (EventTrigger_Game current in this._ActionEventTrigger)
		{
			if (!this.isActive)
			{
				current.CollectLoadStageFunc();
			}
			current.gameObject.SetActive(bActive);
		}
	}

	[DebuggerHidden]
	public IEnumerator RunStandByWork()
	{
		EventTriggerMapManager.<RunStandByWork>c__Iterator4 <RunStandByWork>c__Iterator = new EventTriggerMapManager.<RunStandByWork>c__Iterator4();
		<RunStandByWork>c__Iterator.<>f__this = this;
		return <RunStandByWork>c__Iterator;
	}

	[DebuggerHidden]
	public IEnumerator RunPostLoadWork()
	{
		EventTriggerMapManager.<RunPostLoadWork>c__Iterator5 <RunPostLoadWork>c__Iterator = new EventTriggerMapManager.<RunPostLoadWork>c__Iterator5();
		<RunPostLoadWork>c__Iterator.<>f__this = this;
		return <RunPostLoadWork>c__Iterator;
	}

	public void ActionTrigger_Pause()
	{
	}

	public void ActionTrigger_ReStart()
	{
		foreach (EventTrigger_Game current in this._ActionEventTrigger)
		{
			TsLog.LogWarning("[EventTrigger] ActionTrigger_ReStart {0}", new object[]
			{
				current.gameObject.name
			});
			current.OnTrigger();
		}
	}

	public void ActionTrigger_Reset(string _DlgID)
	{
		foreach (EventTrigger_Game current in this._ActionEventTrigger)
		{
			TsLog.LogWarning("[EventTrigger] ActionTrigger_Reset {0}", new object[]
			{
				current.gameObject.name
			});
			foreach (GameObject current2 in current.BehaviorList)
			{
				Behavior_UIGuide component = current2.GetComponent<Behavior_UIGuide>();
				if (component != null && string.Equals(component.m_DlgID, _DlgID))
				{
					current.TriggerOn = false;
				}
			}
		}
	}
}
