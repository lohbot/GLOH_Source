using System;

public class NkMilitaryList
{
	private NkMineMilitaryInfo[] m_kMineMilitaryInfo;

	private NkExpeditionMilitaryInfo[] m_kExpeditionMilitaryInfo;

	public NkMilitaryList()
	{
		this.m_kMineMilitaryInfo = new NkMineMilitaryInfo[10];
		this.m_kExpeditionMilitaryInfo = new NkExpeditionMilitaryInfo[3];
		for (int i = 0; i < 10; i++)
		{
			this.m_kMineMilitaryInfo[i] = new NkMineMilitaryInfo();
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_kExpeditionMilitaryInfo[j] = new NkExpeditionMilitaryInfo();
		}
		this.Init();
	}

	public void Init()
	{
		for (int i = 0; i < 10; i++)
		{
			this.m_kMineMilitaryInfo[i].Init();
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_kExpeditionMilitaryInfo[j].Init();
		}
	}

	public int FindEmptyMineMilitaryIndex()
	{
		for (int i = 0; i < 10; i++)
		{
			if (!this.m_kMineMilitaryInfo[i].IsValid())
			{
				return i;
			}
		}
		return -1;
	}

	public NkMineMilitaryInfo GetEmptyMineMilitaryInfo()
	{
		int num = this.FindEmptyMineMilitaryIndex();
		if (num < 0)
		{
			return null;
		}
		byte b = (byte)(num + 2);
		NkMineMilitaryInfo mineMilitaryInfo = this.GetMineMilitaryInfo(b);
		if (mineMilitaryInfo != null)
		{
			mineMilitaryInfo.SetMilitaryUnique(b);
		}
		return mineMilitaryInfo;
	}

	public NkMineMilitaryInfo GetValidMineMilitaryInfo(byte militaryunique)
	{
		NkMineMilitaryInfo mineMilitaryInfo = this.GetMineMilitaryInfo(militaryunique);
		if (mineMilitaryInfo == null || !mineMilitaryInfo.IsValid())
		{
			return this.GetEmptyMineMilitaryInfo();
		}
		return mineMilitaryInfo;
	}

	public void InitMineMilitaryInfo(byte militaryunique)
	{
		NkMineMilitaryInfo mineMilitaryInfo = this.GetMineMilitaryInfo(militaryunique);
		if (mineMilitaryInfo != null)
		{
			for (byte b = 0; b < 5; b += 1)
			{
				this.DelMilitarySoldier(mineMilitaryInfo, b);
			}
			mineMilitaryInfo.Init();
		}
	}

	public NkMineMilitaryInfo GetMineMilitaryInfo(byte militaryunique)
	{
		int num = (int)(militaryunique - 2);
		if (num < 0 || num >= 10)
		{
			return null;
		}
		return this.m_kMineMilitaryInfo[num];
	}

	public bool AddMilitarySoldier(byte militaryunique, ref NkSoldierInfo pkSolinfo)
	{
		NkMineMilitaryInfo mineMilitaryInfo = this.GetMineMilitaryInfo(militaryunique);
		return this.AddMilitarySoldier(mineMilitaryInfo, ref pkSolinfo);
	}

	public bool AddMilitarySoldier(NkMineMilitaryInfo pkMilitaryInfo, ref NkSoldierInfo pkSolinfo)
	{
		if (pkMilitaryInfo == null || pkSolinfo == null)
		{
			return false;
		}
		if (pkSolinfo.GetSolPosIndex() == 0)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1616"),
				"solname",
				pkSolinfo.GetName()
			});
			pkMilitaryInfo.SetMilitaryName(empty);
			pkMilitaryInfo.SetMilitaryUnique(pkSolinfo.GetMilitaryUnique());
		}
		pkMilitaryInfo.SetSolInfo(pkSolinfo.GetSolPosIndex(), ref pkSolinfo);
		return true;
	}

	public bool DelMilitarySoldier(byte militaryunique, byte solposindex)
	{
		NkMineMilitaryInfo mineMilitaryInfo = this.GetMineMilitaryInfo(militaryunique);
		return this.DelMilitarySoldier(mineMilitaryInfo, solposindex);
	}

	public bool DelMilitarySoldier(NkMineMilitaryInfo pkMilitaryInfo, byte solposindex)
	{
		if (pkMilitaryInfo == null)
		{
			return false;
		}
		pkMilitaryInfo.DelSolInfo(solposindex);
		return true;
	}

	public int FindEmptyExpeditionMilitaryIndex()
	{
		for (int i = 0; i < 3; i++)
		{
			if (!this.m_kExpeditionMilitaryInfo[i].IsValid())
			{
				return i;
			}
		}
		return -1;
	}

	public NkExpeditionMilitaryInfo GetEmptyExpeditionMilitaryInfo()
	{
		int num = this.FindEmptyExpeditionMilitaryIndex();
		if (num < 0)
		{
			return null;
		}
		byte b = (byte)(num + 50);
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = this.GetExpeditionMilitaryInfo(b);
		if (expeditionMilitaryInfo != null)
		{
			expeditionMilitaryInfo.SetMilitaryUnique(b);
		}
		return expeditionMilitaryInfo;
	}

	public NkExpeditionMilitaryInfo GetValidExpeditionMilitaryInfo(byte militaryunique)
	{
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = this.GetExpeditionMilitaryInfo(militaryunique);
		if (expeditionMilitaryInfo == null || !expeditionMilitaryInfo.IsValid())
		{
			return this.GetEmptyExpeditionMilitaryInfo();
		}
		return expeditionMilitaryInfo;
	}

	public void InitExpeditionMilitaryInfo(byte militaryunique)
	{
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = this.GetExpeditionMilitaryInfo(militaryunique);
		if (expeditionMilitaryInfo != null)
		{
			for (byte b = 0; b < 15; b += 1)
			{
				this.DelExpeditionMilitarySoldier(expeditionMilitaryInfo, (long)b);
			}
			expeditionMilitaryInfo.Init();
		}
	}

	public NkExpeditionMilitaryInfo GetExpeditionMilitaryInfo(byte militaryunique)
	{
		int num = (int)(militaryunique - 50);
		if (num < 0 || num >= 3)
		{
			return null;
		}
		return this.m_kExpeditionMilitaryInfo[num];
	}

	public bool AddExpeditionMilitarySoldier(byte militaryunique, ref NkSoldierInfo pkSolinfo)
	{
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = this.GetExpeditionMilitaryInfo(militaryunique);
		return this.AddExpeditionMilitarySoldier(expeditionMilitaryInfo, ref pkSolinfo);
	}

	public bool AddExpeditionMilitarySoldier(NkExpeditionMilitaryInfo pkMilitaryInfo, ref NkSoldierInfo pkSolinfo)
	{
		if (pkMilitaryInfo == null || pkSolinfo == null)
		{
			return false;
		}
		if (pkSolinfo.GetSolPosIndex() == 0)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1616"),
				"solname",
				pkSolinfo.GetName()
			});
			pkMilitaryInfo.SetMilitaryUnique(pkSolinfo.GetMilitaryUnique());
		}
		pkMilitaryInfo.SetSolInfo(ref pkSolinfo);
		return true;
	}

	public bool DelExpeditionMilitarySoldier(byte militaryunique, long solid)
	{
		NkExpeditionMilitaryInfo expeditionMilitaryInfo = this.GetExpeditionMilitaryInfo(militaryunique);
		return this.DelExpeditionMilitarySoldier(expeditionMilitaryInfo, solid);
	}

	public bool DelExpeditionMilitarySoldier(NkExpeditionMilitaryInfo pkMilitaryInfo, long solid)
	{
		if (pkMilitaryInfo == null)
		{
			return false;
		}
		pkMilitaryInfo.DelSolInfo(solid);
		return true;
	}

	public bool DelExpeditionMilitarySoldier(NkExpeditionMilitaryInfo pkMilitaryInfo)
	{
		if (pkMilitaryInfo == null)
		{
			return false;
		}
		pkMilitaryInfo.Init();
		return true;
	}
}
