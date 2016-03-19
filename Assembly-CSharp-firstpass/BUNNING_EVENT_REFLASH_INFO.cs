using GAME;
using System;

public class BUNNING_EVENT_REFLASH_INFO
{
	public int m_nStartTime;

	public int m_nEndTime;

	public eBUNNING_EVENT m_eEventType;

	public int m_nLimitCount;

	public int m_nTitleText;

	public int m_nExplain;

	public BUNNING_EVENT_REFLASH_INFO()
	{
		this.InitEventReflashInfo();
	}

	public void InitEventReflashInfo()
	{
		this.m_nStartTime = 0;
		this.m_nEndTime = 0;
		this.m_eEventType = eBUNNING_EVENT.eBUNNING_EVENT_NONE;
		this.m_nLimitCount = 0;
		this.m_nTitleText = 0;
		this.m_nExplain = 0;
	}

	public void SetEventReflashInfo(int nStartTime, int nEndTime, eBUNNING_EVENT eEventType, int nLimitCount, int nTitleText, int nExplain)
	{
		this.m_nStartTime = nStartTime;
		this.m_nEndTime = nEndTime;
		this.m_eEventType = eEventType;
		this.m_nLimitCount = nLimitCount;
		this.m_nTitleText = nTitleText;
		this.m_nExplain = nExplain;
	}
}
