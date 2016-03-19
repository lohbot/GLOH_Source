using System;
using System.Collections.Generic;

public class NkLevelManager : NrTSingleton<NkLevelManager>
{
	private Dictionary<string, NrLevelUpInfo> m_kLevelUpInfoList = new Dictionary<string, NrLevelUpInfo>();

	private NkLevelManager()
	{
	}

	public void Init()
	{
		this.m_kLevelUpInfoList.Clear();
	}

	public NrLevelUpInfo GetLevelUpInfo(string exptype)
	{
		NrLevelUpInfo result = null;
		if (!this.m_kLevelUpInfoList.TryGetValue(exptype, out result))
		{
			return null;
		}
		return result;
	}

	public bool Add(LEVEL_EXP pkLEVEL_EXP)
	{
		bool flag = false;
		NrLevelUpInfo nrLevelUpInfo = this.GetLevelUpInfo(pkLEVEL_EXP.EXP_TYPE);
		if (nrLevelUpInfo == null)
		{
			nrLevelUpInfo = new NrLevelUpInfo();
			flag = true;
		}
		nrLevelUpInfo.SetData(pkLEVEL_EXP);
		if (flag)
		{
			this.m_kLevelUpInfoList.Add(pkLEVEL_EXP.EXP_TYPE, nrLevelUpInfo);
		}
		return true;
	}

	public long GetExp(string exptype, short level)
	{
		NrLevelUpInfo levelUpInfo = this.GetLevelUpInfo(exptype);
		if (levelUpInfo == null)
		{
			return 0L;
		}
		return levelUpInfo.GetExp(level);
	}

	public long GetNextExp(string exptype, short level)
	{
		level += 1;
		return this.GetExp(exptype, level);
	}

	public long GetRemainExp(string exptype, short level, long curexp)
	{
		long nextExp = this.GetNextExp(exptype, level);
		if (nextExp <= 0L)
		{
			return 0L;
		}
		return nextExp - curexp;
	}
}
