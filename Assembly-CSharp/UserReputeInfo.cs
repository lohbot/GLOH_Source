using System;

public class UserReputeInfo
{
	private ReputeInfo[] m_kReputeInfo = new ReputeInfo[20];

	public UserReputeInfo()
	{
		this.Init();
	}

	public void Init()
	{
		for (short num = 0; num < 20; num += 1)
		{
			this.m_kReputeInfo[(int)num] = new ReputeInfo();
			this.m_kReputeInfo[(int)num].Init();
		}
	}

	public void AddReputeInfo(short unique, int value, int rewardValue)
	{
		if (unique == 0)
		{
			return;
		}
		for (short num = 0; num < 20; num += 1)
		{
			if (this.m_kReputeInfo[(int)num].GetUnique() == 0)
			{
				this.m_kReputeInfo[(int)num].SetReputeInfo(unique, value, rewardValue);
				break;
			}
		}
	}

	public void SetReputeInfo(short unique, int value, int rewardValue)
	{
		if (unique == 0)
		{
			return;
		}
		for (short num = 0; num < 20; num += 1)
		{
			if (unique == this.m_kReputeInfo[(int)num].GetUnique())
			{
				this.m_kReputeInfo[(int)num].SetReputeInfo(unique, value, rewardValue);
				break;
			}
		}
	}

	public void SetReputeInfo(short[] unique, int[] value, int[] rewardValue)
	{
		for (short num = 0; num < 20; num += 1)
		{
			if (unique[(int)num] != 0)
			{
				this.m_kReputeInfo[(int)num].SetReputeInfo(unique[(int)num], value[(int)num], rewardValue[(int)num]);
			}
		}
	}

	public ReputeInfo GetReputeInfo(short unique)
	{
		if (unique == 0)
		{
			return null;
		}
		for (short num = 0; num < 20; num += 1)
		{
			if (unique == this.m_kReputeInfo[(int)num].GetUnique())
			{
				return this.m_kReputeInfo[(int)num];
			}
		}
		return null;
	}

	public ReputeInfo GetReputeInfoIndex(int index)
	{
		if (20 <= index)
		{
			return null;
		}
		return this.m_kReputeInfo[index];
	}
}
