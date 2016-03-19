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

	public bool GetCharKindLegend(int i32CharKind)
	{
		int i = 0;
		while (i < this.m_SolGuideList.Count)
		{
			if (this.m_SolGuideList[i].m_i32CharKind == i32CharKind)
			{
				if (this.m_SolGuideList[i].m_i8Legend == 0)
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
}
