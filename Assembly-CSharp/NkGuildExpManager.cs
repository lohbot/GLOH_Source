using System;
using System.Collections.Generic;

public class NkGuildExpManager : NrTSingleton<NkGuildExpManager>
{
	private Dictionary<short, GUILD_EXP> m_kGuildLevelUpInfoList = new Dictionary<short, GUILD_EXP>();

	private NkGuildExpManager()
	{
	}

	public void Init()
	{
		this.m_kGuildLevelUpInfoList.Clear();
	}

	public void Set_Value(GUILD_EXP a_cValue)
	{
		this.m_kGuildLevelUpInfoList.Add(a_cValue.m_nLevel, a_cValue);
	}

	public GUILD_EXP GetGuildLevelUpInfo(short nLevel)
	{
		if (this.m_kGuildLevelUpInfoList.ContainsKey(nLevel))
		{
			return this.m_kGuildLevelUpInfoList[nLevel];
		}
		return null;
	}
}
