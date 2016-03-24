using System;
using System.Collections.Generic;

public class NrTableMyth_EvolutionManager : NrTSingleton<NrTableMyth_EvolutionManager>
{
	private List<MYTH_EVOLUTION> m_Myth_Evolution = new List<MYTH_EVOLUTION>();

	private NrTableMyth_EvolutionManager()
	{
	}

	public void AddMyth_Evolution(MYTH_EVOLUTION Myth_Evolution)
	{
		this.m_Myth_Evolution.Add(Myth_Evolution);
	}

	public List<MYTH_EVOLUTION> GetValue()
	{
		return this.m_Myth_Evolution;
	}

	public MYTH_EVOLUTION GetMyth_EvolutionSeason(byte bSeason)
	{
		if (this.m_Myth_Evolution == null)
		{
			return null;
		}
		for (int i = 0; i < this.m_Myth_Evolution.Count; i++)
		{
			if (this.m_Myth_Evolution[i].m_bSeason == bSeason)
			{
				return this.m_Myth_Evolution[i];
			}
		}
		return null;
	}
}
