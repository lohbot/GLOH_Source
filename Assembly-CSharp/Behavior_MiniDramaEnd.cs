using System;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_MiniDramaEnd : EventTriggerItem_Behavior
{
	private List<GameObject> goBehavior;

	private bool _IsClose;

	public override void InitExcute()
	{
		this.m_Excute = false;
		this.goBehavior = null;
		this._IsClose = false;
	}

	public override void Init()
	{
		Behavior component = base.gameObject.GetComponent<Behavior>();
		if (component != null)
		{
			this.goBehavior = new List<GameObject>();
			EventTrigger parentTrigger = component.ParentTrigger;
			int num = parentTrigger.BehaviorList.IndexOf(base.gameObject);
			for (int i = 0; i < num; i++)
			{
				this.goBehavior.Add(parentTrigger.BehaviorList[i]);
			}
		}
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (!this._IsClose)
		{
			if (this.goBehavior != null)
			{
				foreach (GameObject current in this.goBehavior)
				{
					if (current.activeInHierarchy)
					{
						return true;
					}
				}
			}
			NrTSingleton<EventTriggerMiniDrama>.Instance.CloseMiniDrama();
			this._IsClose = true;
			return true;
		}
		return NrTSingleton<EventTriggerMiniDrama>.Instance.ShowTime;
	}

	public override bool IsPopNext()
	{
		return !NrTSingleton<EventTriggerMiniDrama>.Instance.ShowTime;
	}

	public override string GetComment()
	{
		return string.Format("미니 연출을 끝낸다.", new object[0]);
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.DRAMA;
	}

	public override bool IsVaildValue()
	{
		return true;
	}
}
