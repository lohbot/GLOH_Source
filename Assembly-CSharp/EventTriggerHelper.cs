using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventTriggerHelper
{
	public delegate void AddEventTriggerHistoryLog(GameObject go, EventTriggerItem item, string log);

	public static EventTriggerHelper.AddEventTriggerHistoryLog AddLog;

	public static readonly string s_soundGameDrama = "_Sound_GameDrama";

	public static GameObject CreateGameDramaObject(string objectname, Type type)
	{
		GameObject gameObject = GameObject.Find("EventTriggerObject");
		if (gameObject == null)
		{
			gameObject = GameObject.Find("GameDrama");
			if (gameObject == null)
			{
				gameObject = new GameObject("GameDrama");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}
		return EventTriggerHelper.CreateEventTriggerObject(objectname, type, gameObject);
	}

	public static GameObject CreateEventTriggerObject(string objectname, Type type, GameObject ParentObject)
	{
		GameObject gameObject = new GameObject(objectname);
		if (type != null)
		{
			gameObject.AddComponent(type);
		}
		if (ParentObject != null)
		{
			gameObject.transform.parent = ParentObject.transform;
		}
		return gameObject;
	}

	public static GameObject CreateEventChildTriggerObject(EventTrigger _SelectTrigger, Type type)
	{
		if (_SelectTrigger.gameObject == null)
		{
			return null;
		}
		string name = "Child_" + _SelectTrigger.name;
		GameObject gameObject = new GameObject(name);
		if (type != null)
		{
			gameObject.AddComponent(type);
		}
		gameObject.transform.parent = _SelectTrigger.gameObject.transform;
		return gameObject;
	}

	public static GameObject CreateEventTriggerItem(EventTrigger EventTrigger, EventTrigger._EVENTTRIGGER eCategory, string ItemName)
	{
		Transform transform = EventTrigger.gameObject.transform.FindChild(eCategory.ToString());
		if (transform == null)
		{
			transform = new GameObject(eCategory.ToString())
			{
				transform = 
				{
					parent = EventTrigger.gameObject.transform
				}
			}.transform;
		}
		GameObject gameObject = new GameObject(ItemName);
		gameObject.SetActive(true);
		gameObject.transform.parent = transform.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.AddComponent(eCategory.ToString());
		if (ItemName != null)
		{
			gameObject.AddComponent(ItemName);
		}
		return gameObject;
	}

	public static void CameraMove(Camera MoveCamera, Transform Dest)
	{
		MoveCamera.transform.localPosition = Dest.transform.position;
		MoveCamera.transform.localRotation = Dest.transform.rotation;
	}

	public static GameObject SoundPlay(string goName, AudioClip audioClip, bool loop)
	{
		if (string.IsNullOrEmpty(goName) || audioClip == null)
		{
			return null;
		}
		GameObject gameObject = GameObject.Find(EventTriggerHelper.s_soundGameDrama);
		if (gameObject == null)
		{
			gameObject = new GameObject(EventTriggerHelper.s_soundGameDrama);
		}
		GameObject gameObject2 = new GameObject(goName, new Type[]
		{
			typeof(AudioSource),
			typeof(TsAudioAdapterGameDrama)
		});
		gameObject2.transform.position = TsAudioManager.Instance.CurrentAudioListener.transform.position;
		gameObject2.transform.parent = gameObject.transform;
		TsAudioAdapterGameDrama component = gameObject2.GetComponent<TsAudioAdapterGameDrama>();
		component.GetAudioEx().RefAudioClip = audioClip;
		component.GetAudioEx().RefAudioSource = gameObject2.GetComponent<AudioSource>();
		component.GetAudioEx().RefAudioSource.loop = loop;
		component.Play();
		TsLog.Log("### EventTriggerHelper.SoundPlay() audioClip[{0}]", new object[]
		{
			audioClip.name
		});
		return gameObject2;
	}

	private static bool HasAniClip(Animation Ani, AnimationClip clip)
	{
		if (Ani != null || clip != null)
		{
			AnimationClip clip2 = Ani.GetClip(clip.name);
			if (clip2 == null)
			{
				return false;
			}
			if (clip2.Equals(clip))
			{
				return true;
			}
		}
		return false;
	}

	public static void AddHistroyLog(GameObject go, EventTriggerItem item, string log)
	{
		if (EventTriggerHelper.AddLog != null)
		{
			EventTriggerHelper.AddLog(go, item, log);
		}
	}

	public static bool AddAniClip(Animation Ani, AnimationClip clip)
	{
		if (Ani == null || clip == null)
		{
			return false;
		}
		if (!EventTriggerHelper.HasAniClip(Ani, clip))
		{
			Ani.AddClip(clip, clip.name);
		}
		if (EventTriggerHelper.AddLog != null)
		{
			EventTriggerHelper.AddLog(Ani.gameObject, null, string.Format("Add AniClip : {0}", clip.name));
		}
		return true;
	}

	public static bool AddAniClip(GameObject obj, AnimationClip clip)
	{
		return !(obj == null) && !(clip == null) && EventTriggerHelper.AddAniClip(obj.animation, clip);
	}

	public static void DelAniClip(Animation Ani, AnimationClip clip)
	{
		if (Ani == null || clip == null)
		{
			return;
		}
		if (EventTriggerHelper.HasAniClip(Ani, clip))
		{
			Ani.RemoveClip(clip);
		}
	}

	public static void ActiveAllTreeChildren(GameObject obj, bool bActive)
	{
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			EventTriggerHelper.ActiveAllTreeChildren(child.gameObject, bActive);
			child.gameObject.SetActive(bActive);
			if (bActive && child.gameObject.animation != null)
			{
				child.gameObject.animation.Play();
			}
		}
	}

	public static Camera ActiveCamera()
	{
		Camera camera = null;
		if (Camera.main)
		{
			camera = Camera.main;
		}
		else if (Camera.main)
		{
			camera = Camera.main;
		}
		else if (Camera.current)
		{
			camera = Camera.current;
		}
		if (camera != null)
		{
			return camera;
		}
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Camera Error", new object[0]);
		}
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			Camera camera2 = allCameras[i];
			if (camera2.enabled)
			{
				return camera2;
			}
		}
		return null;
	}

	public static Transform GetChildTransform(Transform parent, string ObjectName)
	{
		Transform transform = parent.FindChild(ObjectName);
		if (transform == null)
		{
			int childCount = parent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				transform = EventTriggerHelper.GetChildTransform(child, ObjectName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return transform;
	}

	public static GameObject GameObjectCopy(GameObject Org, Transform Dest)
	{
		GameObject gameObject = new GameObject(Org.name);
		if (gameObject != null)
		{
			gameObject.transform.parent = Dest;
			Component[] components = Org.GetComponents(typeof(Component));
			Component[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				Component org = array[i];
				EventTriggerHelper.ComponentCopy(org, gameObject, true);
			}
			if (Org.transform.childCount > 0)
			{
				for (int j = 0; j < Org.transform.childCount; j++)
				{
					GameObject gameObject2 = Org.transform.GetChild(j).gameObject;
					if (gameObject2 != null)
					{
						EventTriggerHelper.GameObjectCopy(gameObject2, gameObject.transform);
					}
				}
			}
		}
		return gameObject;
	}

	public static bool ComponentCopy(Component Org, GameObject CopyObject, bool bValueCopy)
	{
		if (Org == null || CopyObject == null)
		{
			return false;
		}
		Component component = CopyObject.GetComponent(Org.GetType().Name);
		if (!bValueCopy && component != null)
		{
			return false;
		}
		if (component == null)
		{
			component = CopyObject.AddComponent(Org.GetType());
		}
		if (component == null)
		{
			return false;
		}
		FieldInfo[] fields = component.GetType().GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
			fieldInfo.SetValue(component, fieldInfo.GetValue(Org));
		}
		return true;
	}

	public static bool SortEventTriggerTimeAfter(ref List<GameObject> BehaviorList)
	{
		if (BehaviorList == null)
		{
			return false;
		}
		for (int i = 0; i < BehaviorList.Count - 1; i++)
		{
			for (int j = i + 1; j < BehaviorList.Count; j++)
			{
				GameObject gameObject = BehaviorList[i];
				EventTriggerItem component = gameObject.GetComponent<EventTriggerItem>();
				if (component == null)
				{
					return false;
				}
				EventTriggerItem_TimeAfterActive eventTriggerItem_TimeAfterActive = component as EventTriggerItem_TimeAfterActive;
				if (eventTriggerItem_TimeAfterActive == null)
				{
					return false;
				}
				GameObject gameObject2 = BehaviorList[j];
				EventTriggerItem component2 = gameObject2.GetComponent<EventTriggerItem>();
				if (component2 == null)
				{
					return false;
				}
				EventTriggerItem_TimeAfterActive eventTriggerItem_TimeAfterActive2 = component2 as EventTriggerItem_TimeAfterActive;
				if (eventTriggerItem_TimeAfterActive2 == null)
				{
					return false;
				}
				if (eventTriggerItem_TimeAfterActive.m_fWaitTime_Second > eventTriggerItem_TimeAfterActive2.m_fWaitTime_Second)
				{
					BehaviorList[i] = gameObject2;
					BehaviorList[j] = gameObject;
				}
			}
		}
		return true;
	}

	public static bool DisableEventTriggerItem(EventTrigger Trigger, EventTrigger._EVENTTRIGGER Category, EventTriggerItem PointItem, Type DisableItem)
	{
		List<GameObject> list = Trigger.GetList(Category);
		if (list == null)
		{
			return false;
		}
		if (PointItem == null)
		{
			return false;
		}
		int num = list.IndexOf(PointItem.gameObject);
		for (int i = num; i >= 0; i--)
		{
			GameObject gameObject = list[i];
			if (!(gameObject == null))
			{
				Component component = gameObject.GetComponent(DisableItem);
				if (!(component == null))
				{
					component.gameObject.SetActive(false);
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsCreateUI()
	{
		return UnityEngine.Object.FindObjectOfType(typeof(GUICamera)) != null;
	}

	public static void GameDramaOff()
	{
	}

	public static void DestoryChildObject(Transform Parent)
	{
		if (Parent == null)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		int childCount = Parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = Parent.GetChild(i).gameObject;
			if (!(gameObject == null))
			{
				list.Add(gameObject);
			}
		}
		foreach (GameObject current in list)
		{
			UnityEngine.Object.DestroyImmediate(current);
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(current);
			}
			else
			{
				UnityEngine.Object.Destroy(current);
			}
		}
	}
}
