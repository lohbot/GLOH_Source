using System;

public class ReputeInfo
{
	private short m_nUnique;

	private int m_nValue;

	private int m_nRewardValue;

	public ReputeInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nUnique = 0;
		this.m_nValue = 0;
		this.m_nRewardValue = 0;
	}

	public short GetUnique()
	{
		return this.m_nUnique;
	}

	public int GetValue()
	{
		return this.m_nValue;
	}

	public int GetRewardValue()
	{
		return this.m_nRewardValue;
	}

	public void SetReputeInfo(short unique, int value, int rewardValue)
	{
		this.m_nUnique = unique;
		this.m_nValue = value;
		this.m_nRewardValue = rewardValue;
	}
}
