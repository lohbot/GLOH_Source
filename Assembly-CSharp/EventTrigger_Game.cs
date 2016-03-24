using GAME;
using StageHelper;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger_Game : EventTrigger
{
	public int EventTriggerUnique;

	private EventTriggerItem_StateCondition _StateOperation;

	private List<EventTriggerItem_StateCondition> StatList = new List<EventTriggerItem_StateCondition>();

	public void OnEnable()
	{
		this.SetStateOperation();
		if (this.EventConditionList != null)
		{
			foreach (GameObject current in this.EventConditionList)
			{
				EventCondition component = current.GetComponent<EventCondition>();
				if (component != null)
				{
					component.Verify = false;
					component.RegisterEvent();
				}
			}
		}
	}

	public void OnDisable()
	{
		if (NrTSingleton<NrEventTriggerInfoManager>.Instance.IsATB(this.EventTriggerUnique, EventTriggerInfo.EventTriggerATB.Repeat))
		{
			base.Enable(true);
		}
		else if (this.EventConditionList != null)
		{
			foreach (GameObject current in this.EventConditionList)
			{
				if (!(current == null))
				{
					EventCondition component = current.GetComponent<EventCondition>();
					if (component != null)
					{
						component.CleanEvent();
					}
				}
			}
		}
		if (this.BehaviorList != null)
		{
			foreach (GameObject current2 in this.BehaviorList)
			{
				if (!(current2 == null))
				{
					EventTriggerItem_Behavior component2 = current2.GetComponent<EventTriggerItem_Behavior>();
					if (!(component2 == null))
					{
						component2.OnDisableTrigger();
					}
				}
			}
		}
	}

	public void Update()
	{
		if (!this.TriggerOn && CommonTasks.IsEndOfPrework)
		{
			base.TriggerUpdate();
		}
	}

	public override void FixedUpdate()
	{
		if (this.TriggerOn && CommonTasks.IsEndOfPrework)
		{
			base.TriggerUpdate();
		}
	}

	protected override void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.Start();
		this.OnEnable();
	}

	public void SetStateOperation()
	{
		this._StateOperation = this.GetStateOperation();
	}

	public override void OnTrigger()
	{
		if (this.TriggerOn)
		{
			return;
		}
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(999, (long)this.EventTriggerUnique, 1L);
		NrTSingleton<NrEventTriggerInfoManager>.Instance.SendOnEventTrigger(this.EventTriggerUnique);
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			@char.m_kCharMove.MoveStop(true, false);
		}
		EventTriggerMapManager.Instance.AddActionTrigger(this);
		base.OnTrigger();
	}

	public void OnApplicationPause()
	{
	}

	public override void OffTrigger()
	{
		EventTriggerMapManager.Instance.RemoveActionTrigger(this);
		NrTSingleton<NrEventTriggerInfoManager>.Instance.SendActionEventTrigger(this.EventTriggerUnique);
		NrTSingleton<NrEventTriggerInfoManager>.Instance.SendOffEventTrigger(this.EventTriggerUnique);
		base.OffTrigger();
	}

	public override bool IsVerifyState()
	{
		if (this._StateOperation != null)
		{
			if (!this._StateOperation.Verify())
			{
				return false;
			}
		}
		else if (this.StateConditionList.Count > 0)
		{
			TsLog.LogWarning("[{0}] StateCondition Operation Error - {1}", new object[]
			{
				base.GetType().Name,
				base.name
			});
		}
		return true;
	}

	private EventTriggerItem_StateCondition GetStateOperation()
	{
		this.StatList.Clear();
		foreach (GameObject current in this.StateConditionList)
		{
			if (!(current == null))
			{
				StateCondition component = current.GetComponent<StateCondition>();
				if (!(component == null))
				{
					if (component.IsOperation())
					{
						EventTriggerItem_StateConditionOperation component2 = current.GetComponent<EventTriggerItem_StateConditionOperation>();
						component2.Clear();
						this.StatList.Add(component2);
						EventTriggerItem_StateCondition[] components = current.GetComponents<EventTriggerItem_StateCondition>();
						if (components.Length > 1)
						{
							EventTriggerItem_StateCondition[] array = components;
							for (int i = 0; i < array.Length; i++)
							{
								EventTriggerItem_StateCondition item = array[i];
								if (!this.StatList.Contains(item))
								{
									this.StatList.Add(item);
								}
							}
						}
					}
					else
					{
						this.StatList.Add(current.GetComponent<EventTriggerItem_StateCondition>());
					}
				}
			}
		}
		Stack<EventTriggerItem_StateConditionOperation> stack = new Stack<EventTriggerItem_StateConditionOperation>();
		EventTriggerItem_StateConditionOperation eventTriggerItem_StateConditionOperation = null;
		EventTriggerItem_StateCondition eventTriggerItem_StateCondition = null;
		foreach (EventTriggerItem_StateCondition current2 in this.StatList)
		{
			if (!(current2 == null))
			{
				if (current2 is EventTriggerItem_StateConditionOperation)
				{
					EventTriggerItem_StateConditionOperation eventTriggerItem_StateConditionOperation2 = current2 as EventTriggerItem_StateConditionOperation;
					StateCondition_BracketOpen x = eventTriggerItem_StateConditionOperation2 as StateCondition_BracketOpen;
					StateCondition_BracketClose x2 = eventTriggerItem_StateConditionOperation2 as StateCondition_BracketClose;
					if (x != null)
					{
						if (eventTriggerItem_StateCondition != null && eventTriggerItem_StateConditionOperation == null)
						{
							eventTriggerItem_StateConditionOperation = current2.gameObject.AddComponent<StateCondition_AND>();
							eventTriggerItem_StateConditionOperation.lItem = eventTriggerItem_StateCondition;
						}
						if (eventTriggerItem_StateConditionOperation != null)
						{
							stack.Push(eventTriggerItem_StateConditionOperation);
							eventTriggerItem_StateConditionOperation = null;
							eventTriggerItem_StateCondition = null;
						}
					}
					else if (x2 != null)
					{
						if (stack.Count > 0)
						{
							eventTriggerItem_StateConditionOperation = stack.Pop();
							eventTriggerItem_StateConditionOperation.rItem = eventTriggerItem_StateCondition;
							eventTriggerItem_StateCondition = eventTriggerItem_StateConditionOperation;
						}
					}
					else
					{
						if (eventTriggerItem_StateCondition != null)
						{
							eventTriggerItem_StateConditionOperation2.lItem = eventTriggerItem_StateCondition;
						}
						eventTriggerItem_StateConditionOperation = eventTriggerItem_StateConditionOperation2;
					}
				}
				else
				{
					EventTriggerItem_StateCondition eventTriggerItem_StateCondition2 = current2;
					if (eventTriggerItem_StateCondition != null && eventTriggerItem_StateConditionOperation == null)
					{
						eventTriggerItem_StateConditionOperation = current2.gameObject.AddComponent<StateCondition_AND>();
						eventTriggerItem_StateConditionOperation.lItem = eventTriggerItem_StateCondition;
					}
					if (eventTriggerItem_StateConditionOperation != null)
					{
						if (eventTriggerItem_StateConditionOperation.lItem == null)
						{
							eventTriggerItem_StateCondition = (eventTriggerItem_StateConditionOperation.lItem = eventTriggerItem_StateCondition2);
						}
						else if (eventTriggerItem_StateConditionOperation.rItem == null)
						{
							eventTriggerItem_StateConditionOperation.rItem = eventTriggerItem_StateCondition2;
							eventTriggerItem_StateCondition = eventTriggerItem_StateConditionOperation;
						}
						else
						{
							eventTriggerItem_StateConditionOperation = current2.gameObject.AddComponent<StateCondition_AND>();
							eventTriggerItem_StateConditionOperation.lItem = eventTriggerItem_StateCondition;
							eventTriggerItem_StateConditionOperation.rItem = eventTriggerItem_StateCondition2;
							eventTriggerItem_StateCondition = eventTriggerItem_StateConditionOperation;
						}
					}
					else
					{
						eventTriggerItem_StateCondition = eventTriggerItem_StateCondition2;
					}
				}
			}
		}
		if (eventTriggerItem_StateCondition != null && eventTriggerItem_StateConditionOperation == null)
		{
			return eventTriggerItem_StateCondition;
		}
		return eventTriggerItem_StateConditionOperation;
	}

	public POS3D[] GetEventArea()
	{
		List<POS3D> list = new List<POS3D>();
		if (this.EventConditionList != null)
		{
			foreach (GameObject current in this.EventConditionList)
			{
				EventTriggerItem_EventCondition component = current.GetComponent<EventTriggerItem_EventCondition>();
				if (component != null)
				{
					POS3D areaEvent = component.GetAreaEvent();
					if (areaEvent != null)
					{
						list.Add(areaEvent);
					}
				}
			}
		}
		return list.ToArray();
	}

	public bool IsQuestCondition(string QuestUnique)
	{
		foreach (GameObject current in this.StateConditionList)
		{
			if (!(current == null))
			{
				EventTriggerItem_StateCondition component = current.GetComponent<EventTriggerItem_StateCondition>();
				if (!(component == null))
				{
					IEventTriggerItem_StateConditionQuest eventTriggerItem_StateConditionQuest = component as IEventTriggerItem_StateConditionQuest;
					if (eventTriggerItem_StateConditionQuest != null)
					{
						if (eventTriggerItem_StateConditionQuest.IsConditionQuest(QuestUnique))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}
