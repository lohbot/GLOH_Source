using System;

public class clTargetCharInfo
{
	public string strCharName = string.Empty;

	public int m_nLevel;

	public long m_lMoney;

	public void Init()
	{
		this.strCharName = string.Empty;
		this.m_nLevel = 0;
		this.m_lMoney = 0L;
	}

	public void SetTargetCharInfo(string charname, int level, long money)
	{
		this.strCharName = charname;
		this.m_nLevel = level;
		this.m_lMoney = money;
	}
}
