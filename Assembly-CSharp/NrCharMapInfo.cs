using System;

public class NrCharMapInfo
{
	public int m_nMapIndex;

	private int m_nMapUnique;

	public short m_nBattleMapIdx = -1;

	public int MapUnique
	{
		get
		{
			return this.m_nMapUnique;
		}
		set
		{
			this.m_nMapIndex = NrTSingleton<MapManager>.Instance.GetMapIndexFromUnique(value);
			this.m_nMapUnique = value;
		}
	}

	public int MapIndex
	{
		get
		{
			return this.m_nMapIndex;
		}
	}

	public NrCharMapInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nMapIndex = 0;
		this.m_nMapUnique = 0;
		this.m_nBattleMapIdx = -1;
	}
}
