using System;

public class NkMineMilitaryInfo
{
	private long m_nLegionID;

	private long m_nLegionActionID;

	private byte m_nMilitaryUnique;

	private byte m_nMilitaryStatus;

	private int m_nSolCount;

	private NkSoldierInfo[] m_pkSolInfo;

	private string m_szMilitaryName = string.Empty;

	public NkMineMilitaryInfo()
	{
		this.m_pkSolInfo = new NkSoldierInfo[5];
		this.Init();
	}

	public void Init()
	{
		this.m_nLegionID = 0L;
		this.m_nLegionActionID = 0L;
		this.m_nMilitaryUnique = 0;
		this.m_nMilitaryStatus = 0;
		this.m_nSolCount = 0;
		for (int i = 0; i < 5; i++)
		{
			this.m_pkSolInfo[i] = null;
		}
		this.m_szMilitaryName = string.Empty;
	}

	public bool IsValid()
	{
		return this.m_nMilitaryUnique > 0 && this.m_nSolCount > 0;
	}

	public void Set(NkMineMilitaryInfo pkInfo)
	{
		this.m_nLegionID = pkInfo.m_nLegionID;
		this.m_nLegionActionID = pkInfo.m_nLegionActionID;
		this.m_nMilitaryUnique = pkInfo.GetMilitaryUnique();
		this.m_nMilitaryStatus = pkInfo.GetMilitaryStatus();
		for (int i = 0; i < 5; i++)
		{
			this.m_pkSolInfo[i] = pkInfo.GetSolInfo(i);
		}
		this.m_szMilitaryName = pkInfo.GetMilitaryName();
	}

	public void SetMilitaryLegionID(long LegionID)
	{
		this.m_nLegionID = LegionID;
	}

	public long GetMilitaryLegionID()
	{
		return this.m_nLegionID;
	}

	public void SetMilitaryLegionActionID(long LegionActionID)
	{
		this.m_nLegionActionID = LegionActionID;
	}

	public long GetMilitaryLegionActionID()
	{
		return this.m_nLegionActionID;
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
		for (int i = 0; i < 5; i++)
		{
			if (this.m_pkSolInfo[i] != null && this.m_pkSolInfo[i].IsValid())
			{
				this.m_nSolCount++;
			}
		}
	}

	public void SetSolInfo(byte posindex, ref NkSoldierInfo pkSolinfo)
	{
		if (posindex < 0 || posindex >= 5)
		{
			return;
		}
		if (this.m_pkSolInfo[(int)posindex] != null && this.m_pkSolInfo[(int)posindex].IsValid() && pkSolinfo != null && pkSolinfo.IsValid())
		{
			return;
		}
		this.m_pkSolInfo[(int)posindex] = pkSolinfo;
		this.SetSolCount();
	}

	public void DelSolInfo(byte posindex)
	{
		if (posindex < 0 || posindex >= 5)
		{
			return;
		}
		this.m_pkSolInfo[(int)posindex] = null;
		this.SetSolCount();
	}

	public NkSoldierInfo GetSolInfo(int index)
	{
		if (index < 0 || index >= 5)
		{
			return null;
		}
		return this.m_pkSolInfo[index];
	}

	public NkSoldierInfo GetSolInfoFromSolID(long solid)
	{
		for (int i = 0; i < 5; i++)
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

	public void SetMilitaryName(string name)
	{
		this.m_szMilitaryName = name;
	}

	public string GetMilitaryName()
	{
		return this.m_szMilitaryName;
	}
}
