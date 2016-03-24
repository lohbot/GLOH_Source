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

	private Dictionary<int, GuildWarExchangeTable> m_kGuildWarExchangeTable = new Dictionary<int, GuildWarExchangeTable>();

	private Dictionary<int, EventExchangeTable> m_kEventExchangeTable = new Dictionary<int, EventExchangeTable>();

	private Dictionary<int, ExchangeEvolutionTable> m_kExchangeEvolutionTable = new Dictionary<int, ExchangeEvolutionTable>();

	private List<int> m_kMythSolLimit = new List<int>();

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
		if (!this.m_kJewelryTable.ContainsKey(info.m_nIDX))
		{
			this.m_kJewelryTable.Add(info.m_nIDX, info);
		}
	}

	public JewelryTable GetJewelryTable(int _i32IDX)
	{
		if (this.m_kJewelryTable.ContainsKey(_i32IDX))
		{
			return this.m_kJewelryTable[_i32IDX];
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
		if (!this.m_kMythicSolTable.ContainsKey(info.m_nIDX))
		{
			this.m_kMythicSolTable.Add(info.m_nIDX, info);
		}
	}

	public MythicSolTable GetMythicSolTable(int _i32IDX)
	{
		if (this.m_kMythicSolTable.ContainsKey(_i32IDX))
		{
			return this.m_kMythicSolTable[_i32IDX];
		}
		return null;
	}

	public Dictionary<int, MythicSolTable> GetTotalMythicSolTable()
	{
		return this.m_kMythicSolTable;
	}

	public void AddMythicSolLimit(int Unique)
	{
		this.m_kMythSolLimit.Add(Unique);
	}

	public void ClearMythicSolLimit()
	{
		this.m_kMythSolLimit.Clear();
	}

	public bool IsMythicSolLimit(int Unique)
	{
		return this.m_kMythSolLimit.Contains(Unique);
	}

	public void AddGuildWarExchangeTable(GuildWarExchangeTable info)
	{
		if (!this.m_kGuildWarExchangeTable.ContainsKey(info.m_nItemUnique))
		{
			this.m_kGuildWarExchangeTable.Add(info.m_nItemUnique, info);
		}
	}

	public GuildWarExchangeTable GetGuildWarExchangeTable(int unique)
	{
		if (this.m_kGuildWarExchangeTable.ContainsKey(unique))
		{
			return this.m_kGuildWarExchangeTable[unique];
		}
		return null;
	}

	public Dictionary<int, GuildWarExchangeTable> GetTotalGuildWarExchangeTable()
	{
		return this.m_kGuildWarExchangeTable;
	}

	public void AddEvolutionExchangeTable(ExchangeEvolutionTable info)
	{
		if (!this.m_kExchangeEvolutionTable.ContainsKey(info.m_nIDX))
		{
			this.m_kExchangeEvolutionTable.Add(info.m_nIDX, info);
		}
	}

	public ExchangeEvolutionTable GetExchangeEvolutionTable(int _i32IDX)
	{
		if (this.m_kExchangeEvolutionTable.ContainsKey(_i32IDX))
		{
			return this.m_kExchangeEvolutionTable[_i32IDX];
		}
		return null;
	}

	public Dictionary<int, ExchangeEvolutionTable> GetTotalExchangeEvolutionTable()
	{
		return this.m_kExchangeEvolutionTable;
	}

	public void AddEventExchangeTable(EventExchangeTable info)
	{
		if (!this.m_kEventExchangeTable.ContainsKey(info.m_nIDX))
		{
			this.m_kEventExchangeTable.Add(info.m_nIDX, info);
		}
	}

	public EventExchangeTable GetEventExchangeTable(int index)
	{
		if (this.m_kEventExchangeTable.ContainsKey(index))
		{
			return this.m_kEventExchangeTable[index];
		}
		return null;
	}

	public Dictionary<int, EventExchangeTable> GetTotalEventExchangeTable()
	{
		return this.m_kEventExchangeTable;
	}
}
