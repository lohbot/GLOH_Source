using System;
using System.Collections.Generic;

public class PointManager : NrTSingleton<PointManager>
{
	public static int TEAR_TICKET = 77215;

	public static int TEAR_ELEMENT = 50701;

	public static int STAR_TICKET = 72273;

	public static int STAR_ELEMENT = 50302;

	public static int NECTAR_TICKET = 50307;

	public static int NECTAR_ELEMENT = 50305;

	public static int HERO_TICKET = 50501;

	public static int EQUIP_TICKET = 50502;

	private int m_nItemBuyRate = 1000;

	private Dictionary<int, PointTable> m_kPointTable = new Dictionary<int, PointTable>();

	private Dictionary<int, PointLimitTable> m_kPointLimitTable = new Dictionary<int, PointLimitTable>();

	private Dictionary<int, JewelryTable> m_kJewelryTable = new Dictionary<int, JewelryTable>();

	private Dictionary<int, MythicSolTable> m_kMythicSolTable = new Dictionary<int, MythicSolTable>();

	private PointManager()
	{
	}

	public bool Initialize()
	{
		this.m_nItemBuyRate = 1000;
		return true;
	}

	public void SetItemBuyRate(int rate)
	{
		this.m_nItemBuyRate = rate;
	}

	public int GetItemBuyRate()
	{
		return this.m_nItemBuyRate;
	}

	public Dictionary<int, PointTable> GetTotalPointTable()
	{
		return this.m_kPointTable;
	}

	public Dictionary<int, JewelryTable> GetTotalJewelryTable()
	{
		return this.m_kJewelryTable;
	}

	public void AddPointTable(PointTable info)
	{
		if (!this.m_kPointTable.ContainsKey(info.m_nItemUnique))
		{
			this.m_kPointTable.Add(info.m_nItemUnique, info);
		}
	}

	public PointTable GetPointTable(int unique)
	{
		if (this.m_kPointTable.ContainsKey(unique))
		{
			return this.m_kPointTable[unique];
		}
		return null;
	}

	public void AddJewelryTable(JewelryTable info)
	{
		if (!this.m_kJewelryTable.ContainsKey(info.m_nItemUnique))
		{
			this.m_kJewelryTable.Add(info.m_nItemUnique, info);
		}
	}

	public JewelryTable GetJewelryTable(int unique)
	{
		if (this.m_kJewelryTable.ContainsKey(unique))
		{
			return this.m_kJewelryTable[unique];
		}
		return null;
	}

	public void AddPointLimitTable(PointLimitTable info)
	{
		if (!this.m_kPointLimitTable.ContainsKey(info.m_nLevel))
		{
			this.m_kPointLimitTable.Add(info.m_nLevel, info);
		}
	}

	public PointLimitTable GetPointLimitTable(int level)
	{
		if (this.m_kPointLimitTable.ContainsKey(level))
		{
			return this.m_kPointLimitTable[level];
		}
		return null;
	}

	public void AddMythicSolTable(MythicSolTable info)
	{
		if (!this.m_kMythicSolTable.ContainsKey(info.m_nItemUnique))
		{
			this.m_kMythicSolTable.Add(info.m_nItemUnique, info);
		}
	}

	public MythicSolTable GetMythicSolTable(int unique)
	{
		if (this.m_kMythicSolTable.ContainsKey(unique))
		{
			return this.m_kMythicSolTable[unique];
		}
		return null;
	}

	public Dictionary<int, MythicSolTable> GetTotalMythicSolTable()
	{
		return this.m_kMythicSolTable;
	}
}
