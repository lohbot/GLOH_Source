using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
	public enum _EVENTTRIGGER
	{
		EventCondition,
		StateCondition,
		Behavior,
		EndTrigger
	}

	public enum _MODE
	{
		Drama,
		Drama_TimeAfter,
		GAME
	}

	public bool TriggerOn;

	public List<GameObject> EventConditionList;

	public List<GameObject> StateConditionList;

	public List<GameObject> BehaviorList;

	public List<GameObject> EndTriggerList;

	public EventTrigger._MODE m_Mode;

	private EventTrigger._EVENTTRIGGER _CurrentExcute = EventTrigger._EVENTTRIGGER.Behavior;

	public static bool m_DebugMode;

	public EventTrigger()
	{
		this.Init();
	}

	public void Init()
	{
		this.EventConditionList = new List<GameObject>();
		this.StateConditionList = new List<GameObject>();
		this.BehaviorList = new List<GameObject>();
		this.EndTriggerList = new List<GameObject>();
	}

	public void Enable(bool enable)
	{
		base.enabled = enable;
	}

	public bool IsEnable()
	{
		return base.enabled;
	}

	private void Awake()
	{
		EventTriggerBundle eventTriggerBundle = UnityEngine.Object.FindObjectOfType(typeof(EventTriggerBundle)) as EventTriggerBundle;
		if (eventTriggerBundle != null)
		{
			this.SetAssetList(eventTriggerBundle);
		}
	}

	protected virtual void Start()
	{
		this.CheckErrorList();
		if (this.m_Mode == EventTrigger._MODE.GAME)
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	public List<GameObject> GetList(EventTrigger._EVENTTRIGGER eCategory)
	{
		switch (eCategory)
		{
		case EventTrigger._EVENTTRIGGER.EventCondition:
			return this.EventConditionList;
		case EventTrigger._EVENTTRIGGER.StateCondition:
			return this.StateConditionList;
		case EventTrigger._EVENTTRIGGER.Behavior:
			return this.BehaviorList;
		case EventTrigger._EVENTTRIGGER.EndTrigger:
			return this.EndTriggerList;
		default:
			return null;
		}
	}

	public void ChangeItem(EventTrigger._EVENTTRIGGER eCategory, int CurrentIndex, int ChangeIndex)
	{
		List<GameObject> list = this.GetList(eCategory);
		if (list == null)
		{
			return;
		}
		GameObject gameObject = list[CurrentIndex];
		GameObject gameObject2 = list[ChangeIndex];
		list[CurrentIndex] = gameObject2;
		list[ChangeIndex] = gameObject;
		this.RenameItemName(eCategory, gameObject);
		this.RenameItemName(eCategory, gameObject2);
	}

	public int AddItem(EventTrigger._EVENTTRIGGER eCategory, GameObject Item)
	{
		List<GameObject> list = this.GetList(eCategory);
		if (list == null)
		{
			return 0;
		}
		return this.InsertItem(list.Count, eCategory, Item);
	}

	public void RenameItemName(EventTrigger._EVENTTRIGGER eCategory, GameObject Item)
	{
		if (Item == null)
		{
			return;
		}
		List<GameObject> list = this.GetList(eCategory);
		if (list == null)
		{
			return;
		}
		int num = list.IndexOf(Item) + 1;
		EventTriggerItem component = Item.GetComponent<EventTriggerItem>();
		if (component != null)
		{
			Item.name = num.ToString() + ". " + component.GetType().ToString();
		}
		else
		{
			Item.name = num.ToString() + ". " + Item.name;
		}
	}

	public int InsertItem(int index, EventTrigger._EVENTTRIGGER eCategory, GameObject Item)
	{
		List<GameObject> list = this.GetList(eCategory);
		if (list == null)
		{
			return 0;
		}
		if (list.Count <= index)
		{
			list.Add(Item);
		}
		else
		{
			list.Insert(index, Item);
		}
		for (int i = index; i < list.Count; i++)
		{
			this.RenameItemName(eCategory, list[i]);
		}
		return list.Count;
	}

	public List<GameObject> DeleteItem(EventTrigger._EVENTTRIGGER eCategory, GameObject Item)
	{
		List<GameObject> list = this.GetList(eCategory);
		if (list == null)
		{
			return null;
		}
		int num = list.IndexOf(Item);
		if (list.Remove(Item))
		{
			for (int i = num; i < list.Count; i++)
			{
				this.RenameItemName(eCategory, list[i]);
			}
			return list;
		}
		return null;
	}

	public bool IsVerifyEvent()
	{
		bool flag = false;
		if (this.EventConditionList.Count > 0)
		{
			foreach (GameObject current in this.EventConditionList)
			{
				if (!(current == null))
				{
					EventCondition component = current.GetComponent<EventCondition>();
					if (component.Verify)
					{
						flag = true;
					}
					component.Verify = false;
				}
			}
		}
		else
		{
			flag = true;
		}
		if (flag && EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] Verify EventCondition: {1}", new object[]
			{
				base.GetType().Name,
				base.name
			});
		}
		return flag;
	}

	public virtual bool IsVerifyState()
	{
		foreach (GameObject current in this.StateConditionList)
		{
			if (!(current == null))
			{
				if (!current.GetComponent<StateCondition>().Verify())
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool ExcuteBehavior()
	{
		bool flag = false;
		bool flag2 = true;
		foreach (GameObject current in this.BehaviorList)
		{
			if (!(current == null))
			{
				Behavior component = current.GetComponent<Behavior>();
				if (flag)
				{
					current.SetActive(true);
					component.Init(this);
					if (EventTrigger.m_DebugMode)
					{
						TsLog.Log("[{0}] Start Behavior: {1}", new object[]
						{
							base.GetType().Name,
							current.name
						});
					}
				}
				if (current.activeInHierarchy)
				{
					if (!component.bPopNext)
					{
						flag = component.IsNextPop();
					}
					current.SetActive(component.Excute());
					if (!current.activeInHierarchy && EventTrigger.m_DebugMode)
					{
						TsLog.Log("[{0}] End Behavior: {1}", new object[]
						{
							base.GetType().Name,
							current.name
						});
					}
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.EndBehavior();
			return false;
		}
		return true;
	}

	private bool ExcuteEndTrigger()
	{
		bool flag = false;
		bool flag2 = true;
		foreach (GameObject current in this.EndTriggerList)
		{
			if (!(current == null))
			{
				EndTrigger component = current.GetComponent<EndTrigger>();
				if (flag)
				{
					current.SetActive(true);
					component.Init(this);
					if (EventTrigger.m_DebugMode)
					{
						TsLog.Log("[{0}] Start EndTrigger: {1}", new object[]
						{
							base.GetType().Name,
							current.name
						});
					}
				}
				if (current.activeInHierarchy)
				{
					if (!component.bPopNext)
					{
						flag = component.IsNextPop();
					}
					current.SetActive(component.Excute());
					if (!current.activeInHierarchy && EventTrigger.m_DebugMode)
					{
						TsLog.Log("[{0}] End EndTrigger: {1}", new object[]
						{
							base.GetType().Name,
							current.name
						});
					}
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.EndEndTrigger();
			return false;
		}
		return true;
	}

	public virtual void OffTrigger()
	{
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] End EventTrigger - {1}", new object[]
			{
				base.GetType().Name,
				base.name
			});
		}
		this.Enable(false);
		this.TriggerOn = false;
	}

	public virtual void OnTrigger()
	{
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] Start EventTrigger - {1}", new object[]
			{
				base.GetType().Name,
				base.name
			});
		}
		this.Enable(true);
		this.TriggerOn = true;
		if (this.BehaviorList != null && this.BehaviorList.Count > 0)
		{
			this.StartBehavior(this.BehaviorList[0]);
		}
	}

	private void StartEndTrigger()
	{
	}

	private void EndEndTrigger()
	{
		foreach (GameObject current in this.EndTriggerList)
		{
			EndTrigger component = current.GetComponent<EndTrigger>();
			component.bPopNext = false;
		}
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] End EndTriger", new object[]
			{
				base.GetType().Name
			});
		}
		this._CurrentExcute = EventTrigger._EVENTTRIGGER.Behavior;
	}

	public bool StartBehavior(GameObject goStartBehaivor)
	{
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] Start Behavior", new object[]
			{
				base.GetType().Name
			});
		}
		if (this.BehaviorList == null || this.BehaviorList.Count <= 0)
		{
			return false;
		}
		foreach (GameObject current in this.BehaviorList)
		{
			current.SetActive(false);
			Behavior component = current.GetComponent<Behavior>();
			if (component != null)
			{
				component.InitExcute();
			}
		}
		if (goStartBehaivor == null)
		{
			return false;
		}
		Behavior component2 = goStartBehaivor.GetComponent<Behavior>();
		goStartBehaivor.SetActive(true);
		component2.Init(this);
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] Start Behavior:{1}", new object[]
			{
				base.GetType().Name,
				component2.name
			});
		}
		this.ExcuteBehavior();
		return true;
	}

	private void EndBehavior()
	{
		foreach (GameObject current in this.BehaviorList)
		{
			Behavior component = current.GetComponent<Behavior>();
			component.bPopNext = false;
		}
		if (EventTrigger.m_DebugMode)
		{
			TsLog.Log("[{0}] End Behavior", new object[]
			{
				base.GetType().Name
			});
		}
		this._CurrentExcute = EventTrigger._EVENTTRIGGER.EndTrigger;
	}

	private bool Verify()
	{
		return this.IsVerifyEvent() && this.IsVerifyState();
	}

	private bool Excute()
	{
		EventTrigger._EVENTTRIGGER currentExcute = this._CurrentExcute;
		if (currentExcute != EventTrigger._EVENTTRIGGER.Behavior)
		{
			if (currentExcute == EventTrigger._EVENTTRIGGER.EndTrigger)
			{
				return this.ExcuteEndTrigger();
			}
		}
		else if (!this.ExcuteBehavior())
		{
			this.StartEndTrigger();
		}
		return true;
	}

	public virtual void FixedUpdate()
	{
		this.TriggerUpdate();
	}

	public void TriggerUpdate()
	{
		if (!this.TriggerOn)
		{
			if (this.Verify())
			{
				this.OnTrigger();
			}
		}
		else if (!this.Excute())
		{
			this.OffTrigger();
		}
	}

	public T GetItem<T>(EventTrigger._EVENTTRIGGER Category) where T : Component
	{
		List<GameObject> list = this.GetList(Category);
		if (list == null)
		{
			return (T)((object)null);
		}
		foreach (GameObject current in list)
		{
			T component = current.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		return (T)((object)null);
	}

	public GameObject GetItem(EventTrigger._EVENTTRIGGER Category, string name)
	{
		List<GameObject> list = this.GetList(Category);
		if (list == null)
		{
			return null;
		}
		foreach (GameObject current in list)
		{
			if (name.Equals(current.name))
			{
				return current;
			}
		}
		Transform transform = base.gameObject.transform.FindChild(Category.ToString());
		if (transform != null)
		{
			Transform transform2 = transform.FindChild(name);
			if (transform2 != null)
			{
				return transform2.gameObject;
			}
		}
		return null;
	}

	public bool GetAssetList(ref List<UnityEngine.Object> ObjectList)
	{
		Array values = Enum.GetValues(typeof(EventTrigger._EVENTTRIGGER));
		for (int i = 0; i < values.Length; i++)
		{
			List<GameObject> list = this.GetList((EventTrigger._EVENTTRIGGER)((int)values.GetValue(i)));
			if (list != null)
			{
				foreach (GameObject current in list)
				{
					EventTriggerItem component = current.GetComponent<EventTriggerItem>();
					if (!(component == null))
					{
						component.GetAssetList(ref ObjectList);
					}
				}
			}
		}
		return true;
	}

	public bool SetAssetList(EventTriggerBundle BundleList)
	{
		Array values = Enum.GetValues(typeof(EventTrigger._EVENTTRIGGER));
		for (int i = 0; i < values.Length; i++)
		{
			List<GameObject> list = this.GetList((EventTrigger._EVENTTRIGGER)((int)values.GetValue(i)));
			if (list != null)
			{
				foreach (GameObject current in list)
				{
					EventTriggerItem component = current.GetComponent<EventTriggerItem>();
					if (!(component == null))
					{
						component.SetAssetList(BundleList);
					}
				}
			}
		}
		return true;
	}

	public bool ChangeValue(UnityEngine.Object Org, UnityEngine.Object New)
	{
		Array values = Enum.GetValues(typeof(EventTrigger._EVENTTRIGGER));
		for (int i = 0; i < values.Length; i++)
		{
			List<GameObject> list = this.GetList((EventTrigger._EVENTTRIGGER)((int)values.GetValue(i)));
			if (list != null)
			{
				foreach (GameObject current in list)
				{
					EventTriggerItem component = current.GetComponent<EventTriggerItem>();
					if (!(component == null))
					{
						component.ChangeValue(Org, New);
					}
				}
			}
		}
		return true;
	}

	public Behavior._BEHAVIORTYPE GetBehaviorType(int SelectIndex)
	{
		if (this.BehaviorList.Count <= SelectIndex)
		{
			return Behavior._BEHAVIORTYPE.MAX_TYPE;
		}
		GameObject gameObject = this.BehaviorList[SelectIndex];
		if (gameObject != null)
		{
			EventTriggerItem_Behavior component = gameObject.GetComponent<EventTriggerItem_Behavior>();
			if (component != null)
			{
				return component.GetBehaviorType();
			}
		}
		return Behavior._BEHAVIORTYPE.MAX_TYPE;
	}

	public string GetComment(EventTrigger._EVENTTRIGGER SelectTrigger, int SelectIndex, out GameObject Item)
	{
		List<GameObject> list = this.GetList(SelectTrigger);
		if (list == null)
		{
			Item = null;
			return "none";
		}
		GameObject gameObject = list[SelectIndex];
		if (gameObject != null)
		{
			EventTriggerItem component = gameObject.GetComponent<EventTriggerItem>();
			if (component != null)
			{
				Item = gameObject;
				string text = component.GetComment();
				if (text == string.Empty)
				{
					text = component.GetType().ToString();
				}
				return text;
			}
		}
		Item = null;
		return "none";
	}

	public EventTriggerItem GetTriggerItem(EventTrigger._EVENTTRIGGER SelectTrigger, int SelectIndex)
	{
		List<GameObject> list = this.GetList(SelectTrigger);
		if (list == null)
		{
			return null;
		}
		if (SelectIndex < 0 || list.Count <= SelectIndex)
		{
			return null;
		}
		GameObject gameObject = list[SelectIndex];
		if (gameObject == null)
		{
			return null;
		}
		return gameObject.GetComponent<EventTriggerItem>();
	}

	public void ReadXML(XmlReader Reader)
	{
		try
		{
			string attribute = Reader.GetAttribute("Mode");
			this.m_Mode = (EventTrigger._MODE)((int)Enum.Parse(typeof(EventTrigger._MODE), attribute));
			Reader.Read();
			Array values = Enum.GetValues(typeof(EventTrigger._EVENTTRIGGER));
			for (int i = 0; i < values.Length; i++)
			{
				string text = values.GetValue(i).ToString();
				EventTrigger._EVENTTRIGGER category = (EventTrigger._EVENTTRIGGER)((int)Enum.Parse(typeof(EventTrigger._EVENTTRIGGER), text));
				if (Reader.ReadToNextSibling(text))
				{
					int num = int.Parse(Reader.GetAttribute(0));
					if (num > 0)
					{
						this.ReadListXML(Reader, category);
						Reader.ReadEndElement();
					}
				}
			}
			Reader.Read();
		}
		catch (Exception ex)
		{
			TsLog.Log(ex.Message, new object[0]);
		}
	}

	private void ReadItemXML(XmlReader Reader, ref GameObject go)
	{
		EventTriggerItem component = go.GetComponent<EventTriggerItem>();
		if (component == null)
		{
			return;
		}
		try
		{
			Reader.Read();
			while (Reader.ReadToNextSibling("Value"))
			{
				if (!Reader.IsEmptyElement)
				{
					if (Reader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					if (Reader.NodeType == XmlNodeType.Element)
					{
						component.ReadXML(Reader);
						Reader.ReadEndElement();
					}
				}
			}
		}
		catch (Exception ex)
		{
			TsLog.Log(ex.Message, new object[0]);
		}
	}

	private void ReadListXML(XmlReader Reader, EventTrigger._EVENTTRIGGER Category)
	{
		try
		{
			Reader.Read();
			while (Reader.ReadToNextSibling("Item"))
			{
				if (Reader.NodeType == XmlNodeType.EndElement)
				{
					break;
				}
				if (Reader.NodeType == XmlNodeType.Element)
				{
					Reader.MoveToNextAttribute();
					GameObject item = EventTriggerHelper.CreateEventTriggerItem(this, Category, Reader.Value.ToString());
					this.AddItem(Category, item);
					this.ReadItemXML(Reader, ref item);
					Reader.ReadEndElement();
				}
			}
		}
		catch (Exception ex)
		{
			TsLog.Log(ex.Message, new object[0]);
		}
	}

	public void WriteXML(XmlWriter Writer)
	{
		Writer.WriteStartElement("Object");
		Writer.WriteAttributeString("Name", base.gameObject.name);
		Writer.WriteAttributeString("Mode", this.m_Mode.ToString());
		Array values = Enum.GetValues(typeof(EventTrigger._EVENTTRIGGER));
		for (int i = 0; i < values.Length; i++)
		{
			List<GameObject> list = this.GetList((EventTrigger._EVENTTRIGGER)((int)values.GetValue(i)));
			if (list != null)
			{
				Writer.WriteStartElement(values.GetValue(i).ToString());
				Writer.WriteAttributeString("Count", list.Count.ToString());
				foreach (GameObject current in list)
				{
					EventTriggerItem component = current.GetComponent<EventTriggerItem>();
					if (!(component == null))
					{
						component.WriteXML(Writer);
					}
				}
				Writer.WriteEndElement();
			}
		}
		Writer.WriteEndElement();
	}

	public void CleanList(EventTrigger._EVENTTRIGGER eCategory)
	{
		List<GameObject> list = this.GetList(eCategory);
		list.RemoveAll(new Predicate<GameObject>(EventTrigger.CleanListFunc));
		for (int i = 0; i < list.Count; i++)
		{
			this.RenameItemName(eCategory, list[i]);
		}
		Transform transform = base.gameObject.transform.Find("Behavior");
		if (transform != null)
		{
			foreach (Transform transform2 in transform)
			{
				if (!(transform2 == null))
				{
					if (list.IndexOf(transform2.gameObject) < 0)
					{
						UnityEngine.Object.DestroyImmediate(transform2.gameObject);
					}
				}
			}
		}
	}

	public void ActiveItem(EventTrigger._EVENTTRIGGER Category, bool Active)
	{
		List<GameObject> list = this.GetList(Category);
		foreach (GameObject current in list)
		{
			current.SetActive(Active);
		}
	}

	private static bool CleanListFunc(GameObject go)
	{
		return go == null;
	}

	public bool CheckErrorList()
	{
		bool result = false;
		foreach (GameObject current in this.EventConditionList)
		{
			if (current == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null GameObject in EventCondition", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
			EventTriggerItem component = current.GetComponent<EventTriggerItem>();
			if (component == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in EventCondition", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
		}
		foreach (GameObject current2 in this.StateConditionList)
		{
			if (current2 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in StateCondition", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
			EventTriggerItem component2 = current2.GetComponent<EventTriggerItem>();
			if (component2 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in StateCondition", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
		}
		foreach (GameObject current3 in this.BehaviorList)
		{
			if (current3 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in Behavior", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
			EventTriggerItem component3 = current3.GetComponent<EventTriggerItem>();
			if (component3 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in Behavior", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
		}
		foreach (GameObject current4 in this.EndTriggerList)
		{
			if (current4 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in EndTrigger", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
			EventTriggerItem component4 = current4.GetComponent<EventTriggerItem>();
			if (component4 == null)
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogError("[{0}] Null Item in EndTrigger", new object[]
					{
						base.GetType().Name
					});
				}
				result = true;
				break;
			}
		}
		return result;
	}

	public void CollectLoadStageFunc()
	{
		foreach (GameObject current in this.BehaviorList)
		{
			if (current.activeInHierarchy)
			{
				EventTriggerItem_Behavior component = current.GetComponent<EventTriggerItem_Behavior>();
				if (component != null)
				{
					component.CollectLoadStageFunc();
				}
			}
		}
	}

	public bool IsVaildValue()
	{
		EventTriggerItem[] componentsInChildren = base.transform.GetComponentsInChildren<EventTriggerItem>();
		if (componentsInChildren == null)
		{
			return true;
		}
		bool result = true;
		EventTriggerItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			EventTriggerItem eventTriggerItem = array[i];
			if (!eventTriggerItem.IsVaildValue())
			{
				if (EventTrigger.m_DebugMode)
				{
					TsLog.LogWarning("[{0}] [{1}]의 [{2}] 값이 잘못 들어가 있습니다.", new object[]
					{
						base.GetType().Name,
						base.name,
						eventTriggerItem.name
					});
				}
				result = false;
			}
		}
		return result;
	}
}
