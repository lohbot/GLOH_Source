using System;

public class NkCharIDInfo
{
	public int m_nClientID;

	public short m_nCharUnique;

	public int m_nWorldID;

	public NkCharIDInfo()
	{
		this.Init();
	}

	public NkCharIDInfo(int id, short charunique)
	{
		this.m_nClientID = id;
		this.m_nCharUnique = charunique;
		this.m_nWorldID = 0;
	}

	public void Init()
	{
		this.m_nClientID = -1;
		this.m_nCharUnique = -1;
		this.m_nWorldID = -1;
	}
}
