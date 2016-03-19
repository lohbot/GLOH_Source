using System;
using TsLibs;

public class NrTable_PointLimitTable : NrTableBase
{
	public NrTable_PointLimitTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			PointLimitTable pointLimitTable = new PointLimitTable();
			pointLimitTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddPointLimitTable(pointLimitTable);
		}
		return true;
	}
}
