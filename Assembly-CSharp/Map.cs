using System;
using System.Collections.Generic;

public class Map
{
	public class Record
	{
		public int m_iUnique;

		public string m_szMapName;

		public Record(int iUnique, string szMapName)
		{
			this.m_iUnique = iUnique;
			this.m_szMapName = szMapName;
		}

		public Record(int iUnique)
		{
			this.m_iUnique = iUnique;
			this.m_szMapName = string.Empty;
		}

		public Record()
		{
			this.m_iUnique = 0;
			this.m_szMapName = string.Empty;
		}

		public Map.Record Setdata(int u, string sz)
		{
			this.m_iUnique = u;
			this.m_szMapName = sz;
			return this;
		}
	}

	protected Map.Record m_kRecord;

	protected List<Map> m_MapList;

	protected LinkedList<int> m_kLinkUniqueList = new LinkedList<int>();

	public Map.Record GetRecord()
	{
		return this.m_kRecord;
	}

	public void SetRecord(Map.Record kRecord)
	{
		this.m_kRecord = kRecord;
	}

	public LinkedList<int> GetLinkMapUniqueList()
	{
		return this.m_kLinkUniqueList;
	}

	public int GetIndex()
	{
		return this.m_kRecord.m_iUnique;
	}

	public void InsertLinkMapUnique(int iUnique)
	{
		if (this.m_kLinkUniqueList.Contains(iUnique))
		{
			return;
		}
		this.m_kLinkUniqueList.AddLast(iUnique);
	}

	public void RemoveLinkMapUnique(int iUnique)
	{
		if (!this.m_kLinkUniqueList.Contains(iUnique))
		{
			return;
		}
		this.m_kLinkUniqueList.Remove(iUnique);
	}

	public void RemoveLinkUniqueAll()
	{
		this.m_kLinkUniqueList.Clear();
	}
}
