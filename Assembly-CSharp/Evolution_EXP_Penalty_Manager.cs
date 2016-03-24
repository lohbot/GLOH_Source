using System;
using System.Collections.Generic;

public class Evolution_EXP_Penalty_Manager : NrTSingleton<Evolution_EXP_Penalty_Manager>
{
	private Dictionary<byte, Evolution_EXP_Penalty> m_EvolutionExpPenalty;

	private Evolution_EXP_Penalty_Manager()
	{
		this.m_EvolutionExpPenalty = new Dictionary<byte, Evolution_EXP_Penalty>();
	}

	public void Add(Evolution_EXP_Penalty Data)
	{
		this.m_EvolutionExpPenalty.Add(Data.BaseSeason, Data);
	}

	public Evolution_EXP_Penalty GetSeaeonExpPenalty(byte Season)
	{
		if (this.m_EvolutionExpPenalty.ContainsKey(Season))
		{
			return this.m_EvolutionExpPenalty[Season];
		}
		return null;
	}

	public int GetSeasonExpPenalty(byte Base, byte Sub)
	{
		if (Sub >= 10)
		{
			return 10000;
		}
		Base += 1;
		if (this.m_EvolutionExpPenalty.ContainsKey(Base))
		{
			return this.m_EvolutionExpPenalty[Base].SeasonPenalty[(int)Sub];
		}
		return 10000;
	}
}
