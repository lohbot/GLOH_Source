using System;
using UnityEngine;

public class CTimeAnimate
{
	private bool m_bStarted;

	public float m_UPDATE_INTERVAL = 1f;

	private float m_fNextUpdateTime;

	private float m_MAXVALUES = 1f;

	private float m_MINVALUES = 0.5f;

	private bool m_isSwitch;

	private int m_iMaxSwitchingValues;

	private int m_iAccumSwitch;

	private bool m_isSwitchComplete;

	public CTimeAnimate(float Update_Interval)
	{
		this.m_UPDATE_INTERVAL = Update_Interval;
	}

	public CTimeAnimate(float Update_Interval, int MaxSwitchingValues)
	{
		this.m_UPDATE_INTERVAL = Update_Interval;
		this.m_iMaxSwitchingValues = MaxSwitchingValues * 2;
	}

	public void SetAnimateStart(bool _Start)
	{
		if (this.m_bStarted != _Start)
		{
			this.m_bStarted = _Start;
			this.m_fNextUpdateTime = Time.time + this.m_UPDATE_INTERVAL;
		}
	}

	public void SetChangeTime(float _WantChangeTime)
	{
		this.m_UPDATE_INTERVAL = _WantChangeTime;
	}

	public bool AnimateUpdate()
	{
		if (this.m_iMaxSwitchingValues != 0)
		{
			if (this.m_isSwitchComplete)
			{
				return false;
			}
			if (this.m_iAccumSwitch >= this.m_iMaxSwitchingValues)
			{
				this.m_isSwitchComplete = true;
				TsLog.LogWarning("KKIComment2011/9/7/ : {0} ", new object[]
				{
					this.m_iAccumSwitch
				});
				return false;
			}
		}
		bool result = false;
		if (this.m_bStarted && this.m_fNextUpdateTime <= Time.time)
		{
			this.m_isSwitch = !this.m_isSwitch;
			this.m_iAccumSwitch++;
			result = true;
			this.m_fNextUpdateTime = Time.time + this.m_UPDATE_INTERVAL;
		}
		return result;
	}

	public int GetAccumValues()
	{
		return this.m_iAccumSwitch;
	}

	public float GetSwithValues()
	{
		float result = this.m_MINVALUES;
		if (!this.m_isSwitch)
		{
			result = this.m_MAXVALUES;
		}
		else
		{
			result = this.m_MINVALUES;
		}
		return result;
	}
}
