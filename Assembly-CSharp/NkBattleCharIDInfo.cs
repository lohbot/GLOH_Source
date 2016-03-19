using System;

public class NkBattleCharIDInfo
{
	public int m_nClientID;

	public short m_nCharUnique;

	public short m_nBUID;

	public NkBattleCharIDInfo()
	{
		this.Init();
	}

	public NkBattleCharIDInfo(int id, short charunique, short buid)
	{
		this.m_nClientID = id;
		this.m_nCharUnique = charunique;
		this.m_nBUID = buid;
	}

	public void Init()
	{
		this.m_nClientID = -1;
		this.m_nCharUnique = -1;
		this.m_nBUID = -1;
	}
}
