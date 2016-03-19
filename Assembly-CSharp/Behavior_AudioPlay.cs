using System;
using TsBundle;

public class Behavior_AudioPlay : EventTriggerItem_Behavior
{
	public string m_DomainKey = "UI_SFX";

	public string m_CategoryKey = "MINI_DIRECTION";

	public string m_AudioKey = string.Empty;

	public string m_Repeat = "false";

	public bool m_bRepeat;

	public override void Init()
	{
		if (this.m_Repeat.Equals("true"))
		{
			this.m_bRepeat = true;
		}
		TsAudioManager.Container.RequestAudioClip(this.m_DomainKey, this.m_CategoryKey, this.m_AudioKey, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), TsAudioManager.MINI_SOUND, this.m_bRepeat);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return false;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		if (this.m_Repeat.Equals("true"))
		{
			this.m_bRepeat = true;
		}
		if (this.m_bRepeat)
		{
			return string.Format("{0}#{1}#{2} 소리를 반복적으로 출력한다.", this.m_DomainKey, this.m_CategoryKey, this.m_AudioKey);
		}
		return string.Format("{0}#{1}#{2} 소리를 출력한다.", this.m_DomainKey, this.m_CategoryKey, this.m_AudioKey);
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.SOUND;
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_DomainKey) && !string.IsNullOrEmpty(this.m_CategoryKey) && !string.IsNullOrEmpty(this.m_AudioKey);
	}
}
