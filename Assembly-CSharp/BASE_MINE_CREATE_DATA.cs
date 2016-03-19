using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_MINE_CREATE_DATA : NrTableBase
{
	public static Dictionary<short, MINE_CREATE_DATA> m_dicMineCreateData = new Dictionary<short, MINE_CREATE_DATA>();

	public BASE_MINE_CREATE_DATA() : base(CDefinePath.MineCreateDataURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MINE_CREATE_DATA mINE_CREATE_DATA = new MINE_CREATE_DATA();
			mINE_CREATE_DATA.SetData(data);
			mINE_CREATE_DATA.nMine_Grade = BASE_MINE_DATA.ParseGradeFromString(mINE_CREATE_DATA.MINE_GRADE);
			if (BASE_MINE_CREATE_DATA.m_dicMineCreateData.ContainsKey(mINE_CREATE_DATA.MINE_ID))
			{
				return false;
			}
			BASE_MINE_CREATE_DATA.m_dicMineCreateData.Add(mINE_CREATE_DATA.MINE_ID, mINE_CREATE_DATA);
		}
		return true;
	}

	public static MINE_CREATE_DATA GetMineCreateDataFromID(short mine_createid)
	{
		if (BASE_MINE_CREATE_DATA.m_dicMineCreateData.ContainsKey(mine_createid))
		{
			return BASE_MINE_CREATE_DATA.m_dicMineCreateData[mine_createid];
		}
		return null;
	}
}
