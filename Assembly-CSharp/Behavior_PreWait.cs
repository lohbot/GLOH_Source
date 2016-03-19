using System;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_PreWait : EventTriggerItem_Behavior
{
	private List<GameObject> goBehavior;

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
		return !this.IsPopNext();
	}

	public override bool IsPopNext()
	{
		if (this.goBehavior != null)
		{
			foreach (GameObject current in this.goBehavior)
			{
				if (current.activeInHierarchy)
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	public override string GetComment()
	{
		return string.Format("이전 Behaivor들이 모두 실행 될때까지 기다린다.", new object[0]);
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
