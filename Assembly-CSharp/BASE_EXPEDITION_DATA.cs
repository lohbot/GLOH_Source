using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_EXPEDITION_DATA : NrTableBase
{
	public static List<EXPEDITION_DATA> m_listExpeditionData = new List<EXPEDITION_DATA>();

	public BASE_EXPEDITION_DATA() : base(CDefinePath.ExpeditionDataURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EXPEDITION_DATA eXPEDITION_DATA = new EXPEDITION_DATA();
			eXPEDITION_DATA.SetData(data);
			BASE_EXPEDITION_DATA.m_listExpeditionData.Add(eXPEDITION_DATA);
		}
		return true;
	}

	public static long GetExpeditionMoneyFromGrade(byte grade)
	{
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(grade);
		if (expeditionDataFromGrade != null)
		{
			return expeditionDataFromGrade.Expedition_SEARCH_MONEY;
		}
		return 0L;
	}

	public static int GetExpeditionMoneyFromSolPossibleLevel(byte grade)
	{
		EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(grade);
		if (expeditionDataFromGrade != null)
		{
			return (int)expeditionDataFromGrade.SolPossiblelevel;
		}
		return 0;
	}

	public static EXPEDITION_DATA GetExpeditionDataFromGrade(byte grade)
	{
		foreach (EXPEDITION_DATA current in BASE_EXPEDITION_DATA.m_listExpeditionData)
		{
			if (current.nExpedition_Grade == grade)
			{
				return current;
			}
		}
		return null;
	}
}
