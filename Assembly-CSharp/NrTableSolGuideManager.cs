using System;
using System.Collections.Generic;

public class NrTableSolGuideManager : NrTSingleton<NrTableSolGuideManager>
{
	private List<SOL_GUIDE> m_SolGuideList = new List<SOL_GUIDE>();

	private NrTableSolGuideManager()
	{
	}

	public void AddSolGuide(SOL_GUIDE SolGuide)
	{
		CHARKIND_LEGENDINFO legendGuide_Col = NrTSingleton<NrBaseTableManager>.Instance.GetLegendGuide_Col(SolGuide.m_i32CharKind);
		if (legendGuide_Col != null)
		{
			SolGuide.m_i16LegendSort = legendGuide_Col.i16SortNum;
		}
		else
		{
			SolGuide.m_i16LegendSort = 1000;
		}
		this.m_SolGuideList.Add(SolGuide);
	}

	public List<SOL_GUIDE> GetValue()
	{
		return this.m_SolGuideList;
	}

	public List<SOL_GUIDE> GetValueAllSeason()
	{
		List<SOL_GUIDE> list = new List<SOL_GUIDE>();
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			bool flag = true;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].m_bSeason == this.m_SolGuideList[i].m_bSeason)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(this.m_SolGuideList[i]);
			}
		}
		return list;
	}

	public byte GetCharKindSeason(int i32CharKind)
	{
		byte result = 0;
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == i32CharKind)
			{
				result = this.m_SolGuideList[i].m_bSeason;
				break;
			}
		}
		return result;
	}

	public int GetCharKindGrade(int i32CharKind)
	{
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == i32CharKind)
			{
				return this.m_SolGuideList[i].m_iSolGrade;
			}
		}
		return -1;
	}

	public bool GetCharKindAlchemy(int i32CharKind)
	{
		int i = 0;
		while (i < this.m_SolGuideList.Count)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == i32CharKind)
			{
				if (this.m_SolGuideList[i].m_i8Alchemy == 0)
				{
					return true;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		return false;
	}

	public short GetCharKindLegendInfo(int i32CharKind)
	{
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == i32CharKind)
			{
				return (short)this.m_SolGuideList[i].m_i8Legend;
			}
		}
		return -1;
	}

	public SOL_GUIDE GetSolGuild(int iCharKind)
	{
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == iCharKind)
			{
				return this.m_SolGuideList[i];
			}
		}
		return null;
	}

	public bool FindSolInfo(int i32CharKind)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return false;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				if (soldierInfo.GetCharKind() == i32CharKind)
				{
					return true;
				}
			}
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null && current.IsValid())
			{
				if (current.GetCharKind() == i32CharKind)
				{
					return true;
				}
			}
		}
		return NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWarehouseSolList().GetSolWarehouseCharKind(i32CharKind);
	}

	public List<byte> GetSeasonList()
	{
		if (this.m_SolGuideList == null)
		{
			return null;
		}
		List<byte> list = new List<byte>();
		foreach (SOL_GUIDE current in this.m_SolGuideList)
		{
			if (!list.Contains(current.m_bSeason))
			{
				list.Add(current.m_bSeason);
			}
		}
		return list;
	}
}
