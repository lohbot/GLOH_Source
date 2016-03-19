using System;

public class NkExpeditionMilitaryInfo
{
	private byte m_nMilitaryUnique;

	private byte m_nMilitaryStatus;

	private int m_nSolCount;

	private NkSoldierInfo[] m_pkSolInfo;

	public NkExpeditionMilitaryInfo()
	{
		this.m_pkSolInfo = new NkSoldierInfo[15];
		this.Init();
	}

	public NkSoldierInfo[] GetExpeditionSolInfo()
	{
		return this.m_pkSolInfo;
	}

	public void Init()
	{
		this.m_nMilitaryUnique = 0;
		this.m_nMilitaryStatus = 0;
		this.m_nSolCount = 0;
		for (int i = 0; i < 15; i++)
		{
			this.m_pkSolInfo[i] = null;
		}
	}

	public bool IsValid()
	{
		return this.m_nMilitaryUnique > 0 && this.m_nSolCount > 0;
	}

	public void Set(NkExpeditionMilitaryInfo pkInfo)
	{
		this.m_nMilitaryUnique = pkInfo.GetMilitaryUnique();
		this.m_nMilitaryStatus = pkInfo.GetMilitaryStatus();
		for (int i = 0; i < 15; i++)
		{
			this.m_pkSolInfo[i] = pkInfo.GetSolInfo(i);
		}
	}

	public void SetMilitaryUnique(byte militaryunique)
	{
		this.m_nMilitaryUnique = militaryunique;
	}

	public byte GetMilitaryUnique()
	{
		return this.m_nMilitaryUnique;
	}

	public void SetMilitaryStatus(byte militarystatus)
	{
		this.m_nMilitaryStatus = militarystatus;
	}

	public byte GetMilitaryStatus()
	{
		return this.m_nMilitaryStatus;
	}

	public void SetSolCount()
	{
		this.m_nSolCount = 0;
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pkSolInfo[i] != null && this.m_pkSolInfo[i].IsValid())
			{
				this.m_nSolCount++;
			}
		}
	}

	public int GetSolCount()
	{
		return this.m_nSolCount;
	}

	public void SetSolInfo(ref NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo.GetSolPosIndex() < 0 || pkSolinfo.GetSolPosIndex() >= 3)
		{
			return;
		}
		if (pkSolinfo.GetBattlePos() < 0)
		{
			return;
		}
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pkSolInfo[i] == null || !this.m_pkSolInfo[i].IsValid())
			{
				this.m_pkSolInfo[i] = pkSolinfo;
				this.SetSolCount();
				return;
			}
		}
	}

	public void DelSolInfo(long solid)
	{
		if (solid < 0L)
		{
			return;
		}
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pkSolInfo[i] != null)
			{
				if (this.m_pkSolInfo[i].GetSolID() == solid)
				{
					this.m_pkSolInfo[i] = null;
					break;
				}
			}
		}
		this.SetSolCount();
	}

	public NkSoldierInfo GetSolInfo(int index)
	{
		if (index < 0 || index >= 15)
		{
			return null;
		}
		return this.m_pkSolInfo[index];
	}

	public NkSoldierInfo GetSolInfoFromSolID(long solid)
	{
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pkSolInfo[i] != null && this.m_pkSolInfo[i].IsValid())
			{
				if (this.m_pkSolInfo[i].GetSolID() == solid)
				{
					return this.m_pkSolInfo[i];
				}
			}
		}
		return null;
	}

	public NkSoldierInfo GetLeaderSolInfo()
	{
		NkSoldierInfo nkSoldierInfo = null;
		for (int i = 0; i < 15; i++)
		{
			if (this.m_pkSolInfo[i] != null)
			{
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = this.m_pkSolInfo[i];
				}
				else if (this.m_pkSolInfo[i].m_kBase.SolPosIndex <= nkSoldierInfo.m_kBase.SolPosIndex && this.m_pkSolInfo[i].m_kBase.BattlePos < nkSoldierInfo.m_kBase.BattlePos)
				{
					nkSoldierInfo = this.m_pkSolInfo[i];
				}
			}
		}
		return nkSoldierInfo;
	}
}
