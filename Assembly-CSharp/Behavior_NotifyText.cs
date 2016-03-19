using System;

public class Behavior_NotifyText : EventTriggerItem_Behavior, IEventTrigger_ActorText
{
	public string m_NotifyTextKey = string.Empty;

	public float m_NotifyTime;

	public override void Init()
	{
		string text = NrTSingleton<EventTriggerMiniDrama>.Instance.GetText(this.m_NotifyTextKey);
		Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return !this.IsPopNext();
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("{0} 텍스트를 공지로 표시한다.", this.m_NotifyTextKey);
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
		return !string.IsNullOrEmpty(this.m_NotifyTextKey);
	}

	public void SetTextKey(string TextKey)
	{
		this.m_NotifyTextKey = TextKey;
	}
}
