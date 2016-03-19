using System;

public class NkSimpleMilitaryInfo
{
	public byte m_nSolPosType;

	public byte m_nMilitaryUnique;

	public NkSimpleMilitaryInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nSolPosType = 0;
		this.m_nMilitaryUnique = 0;
	}

	public void Set(byte solpostype, byte militaryunique)
	{
		this.m_nSolPosType = solpostype;
		this.m_nMilitaryUnique = militaryunique;
	}
}
