using System;
using System.Collections.Generic;

public class NrTable_PlunderObjectinfo_Manager : NrTSingleton<NrTable_PlunderObjectinfo_Manager>
{
	private SortedDictionary<byte, PLUNDER_OBJECT_INFO> m_sdCollection;

	private NrTable_PlunderObjectinfo_Manager()
	{
		this.m_sdCollection = new SortedDictionary<byte, PLUNDER_OBJECT_INFO>();
	}

	public SortedDictionary<byte, PLUNDER_OBJECT_INFO> Get_Collection()
	{
		return this.m_sdCollection;
	}

	public int Get_Count()
	{
		return this.m_sdCollection.Count;
	}

	public PLUNDER_OBJECT_INFO Get_Value(byte nObjectID)
	{
		if (this.m_sdCollection.ContainsKey(nObjectID))
		{
			return this.m_sdCollection[nObjectID];
		}
		return null;
	}

	public void Set_Value(PLUNDER_OBJECT_INFO a_cValue)
	{
		if (this.m_sdCollection.ContainsKey(a_cValue.nObjectID))
		{
			this.m_sdCollection[a_cValue.nObjectID] = a_cValue;
		}
		else
		{
			this.m_sdCollection.Add(a_cValue.nObjectID, a_cValue);
		}
	}
}
