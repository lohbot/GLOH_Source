using System;
using System.Collections.Generic;

public class ComposeExpManager : NrTSingleton<ComposeExpManager>
{
	private SortedDictionary<short, ComposeExpData> m_sdCollection = new SortedDictionary<short, ComposeExpData>();

	private ComposeExpManager()
	{
	}

	public void Set_Value(ComposeExpData a_cValue)
	{
		if (a_cValue != null)
		{
			this.m_sdCollection.Add((short)a_cValue.SolLevel, a_cValue);
		}
	}

	public ComposeExpData GetFromBaseLevel(short Level)
	{
		if (this.m_sdCollection.ContainsKey(Level))
		{
			return this.m_sdCollection[Level];
		}
		return null;
	}

	public long GetExp(short Level, int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		if (this.m_sdCollection.ContainsKey(Level))
		{
			ComposeExpData composeExpData = this.m_sdCollection[Level];
			return composeExpData.GradeExp[solgrade];
		}
		return 0L;
	}
}
