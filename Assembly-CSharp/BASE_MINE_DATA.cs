using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_MINE_DATA : NrTableBase
{
	public static List<MINE_DATA> m_listMineData = new List<MINE_DATA>();

	public BASE_MINE_DATA() : base(CDefinePath.MineDataURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MINE_DATA mINE_DATA = new MINE_DATA();
			mINE_DATA.SetData(data);
			mINE_DATA.nMine_Grade = BASE_MINE_DATA.ParseGradeFromString(mINE_DATA.MINE_GRADE);
			BASE_MINE_DATA.m_listMineData.Add(mINE_DATA);
		}
		return true;
	}

	public static byte ParseGradeFromString(string szgrade)
	{
		if (szgrade == "SMALL")
		{
			return 1;
		}
		if (szgrade == "MEDIUM")
		{
			return 2;
		}
		if (szgrade == "LARGE")
		{
			return 3;
		}
		if (szgrade == "LARGEST")
		{
			return 4;
		}
		return 0;
	}

	public static long GetMineMoneyFromGrade(byte grade)
	{
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(grade);
		if (mineDataFromGrade != null)
		{
			return mineDataFromGrade.MINE_SEARCH_MONEY;
		}
		return 0L;
	}

	public static int GetMineMoneyFromSolPossibleLevel(byte grade)
	{
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(grade);
		if (mineDataFromGrade != null)
		{
			return (int)mineDataFromGrade.SOLPOSSIBLELEVEL;
		}
		return 0;
	}

	public static MINE_DATA GetMineDataFromGrade(byte grade)
	{
		foreach (MINE_DATA current in BASE_MINE_DATA.m_listMineData)
		{
			if (BASE_MINE_DATA.ParseGradeFromString(current.MINE_GRADE) == grade)
			{
				return current;
			}
		}
		return null;
	}

	public static string GetMineName(byte grade, short minedata_id)
	{
		string empty = string.Empty;
		MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(grade);
		if (mineDataFromGrade != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mineDataFromGrade.MINE_GRADECOUNT_INTERFACEKEY),
				"count",
				(int)(minedata_id - mineDataFromGrade.MINE_CREATE_START_ID + 1)
			});
		}
		return empty;
	}
}
