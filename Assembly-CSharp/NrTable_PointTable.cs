using GAME;
using System;
using TsLibs;

public class NrTable_PointTable : NrTableBase
{
	public NrTable_PointTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			PointTable pointTable = new PointTable();
			pointTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddPointTable(pointTable);
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			NrTSingleton<PointManager>.Instance.SetItemBuyRate(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_GET_ITEMPOINT_VALUE));
		}
		return true;
	}
}
