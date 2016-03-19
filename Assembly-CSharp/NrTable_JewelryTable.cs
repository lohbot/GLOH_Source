using System;
using TsLibs;

public class NrTable_JewelryTable : NrTableBase
{
	public NrTable_JewelryTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			JewelryTable jewelryTable = new JewelryTable();
			jewelryTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddJewelryTable(jewelryTable);
		}
		return true;
	}
}
