using System;

public class NkCoinInfo
{
	private long m_nRealHearts;

	private long m_nFreeHearts;

	public NkCoinInfo()
	{
		this.m_nRealHearts = 0L;
		this.m_nFreeHearts = 0L;
	}

	public void SetCoinInfo(long realHearts, long freeHearts)
	{
		this.m_nRealHearts = realHearts;
		this.m_nFreeHearts = freeHearts;
	}

	public long GetTotalCoin()
	{
		return this.m_nRealHearts + this.m_nFreeHearts;
	}

	public long GetRealCoin()
	{
		return this.m_nRealHearts;
	}

	public long GetFreeCoin()
	{
		return this.m_nFreeHearts;
	}
}
