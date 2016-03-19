using System;

public abstract class EventTriggerItem_Behavior : EventTriggerItem
{
	protected bool m_Excute;

	public bool IsExcute()
	{
		return this.m_Excute;
	}

	public virtual void InitExcute()
	{
		this.m_Excute = false;
	}

	public abstract void Init();

	public abstract bool Excute();

	public abstract bool IsPopNext();

	public abstract Behavior._BEHAVIORTYPE GetBehaviorType();

	public abstract float ExcuteTiemSecond();

	public virtual void Draw()
	{
	}

	public virtual void CollectLoadStageFunc()
	{
	}

	public virtual float PopTimeSecond()
	{
		return 0f;
	}
}
