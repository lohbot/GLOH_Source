using System;

public class MACRO_SOLDIERBATCH
{
	public long m_nSolID;

	public long m_nPersonID;

	public byte m_nGridPos;

	public bool m_bInjury;

	public INJURY_CURE_LEVEL m_eRequestInjury;

	public MACRO_SOLDIERBATCH()
	{
		this.m_nSolID = 0L;
		this.m_nGridPos = 0;
		this.m_bInjury = false;
		this.m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE;
	}

	public void Init()
	{
		this.m_nSolID = 0L;
		this.m_nGridPos = 0;
		this.m_bInjury = false;
		this.m_eRequestInjury = INJURY_CURE_LEVEL.INJURY_CURE_LEVEL_NONE;
	}
}
