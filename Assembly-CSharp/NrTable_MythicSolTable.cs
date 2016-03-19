using System;
using TsLibs;

public class NrTable_MythicSolTable : NrTableBase
{
	public NrTable_MythicSolTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			MythicSolTable mythicSolTable = new MythicSolTable();
			mythicSolTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddMythicSolTable(mythicSolTable);
		}
		return true;
	}
}
