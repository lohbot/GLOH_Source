using System;

public class EVENT_INFO
{
	public sbyte m_nEventInfoWeek = -1;

	public int m_nEventType;

	public int m_nStartTime;

	public int m_nEndDurationTime;

	public int m_nMaxLimitCount;

	public long m_nLeftEventTime;

	public int m_nTitleText;

	public int m_nExplain;

	public EVENT_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nEventInfoWeek = -1;
		this.m_nEventType = 0;
		this.m_nStartTime = 0;
		this.m_nEndDurationTime = 0;
		this.m_nMaxLimitCount = 0;
		this.m_nLeftEventTime = 0L;
		this.m_nTitleText = 0;
		this.m_nExplain = 0;
	}

	public void SetEventInfo(sbyte nEventInfoWeek, int nEventType, int nStartTime, int nTime, int nMaxLimitCount, long nLeftEventTime, int nTitleText, int nExplain)
	{
		this.m_nEventInfoWeek = nEventInfoWeek;
		this.m_nEventType = nEventType;
		this.m_nStartTime = nStartTime;
		this.m_nEndDurationTime = nTime;
		this.m_nMaxLimitCount = nMaxLimitCount;
		this.m_nLeftEventTime = nLeftEventTime;
		this.m_nTitleText = nTitleText;
		this.m_nExplain = nExplain;
	}
}
