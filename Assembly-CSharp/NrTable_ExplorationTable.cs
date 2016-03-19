using System;
using TsLibs;

public class NrTable_ExplorationTable : NrTableBase
{
	public NrTable_ExplorationTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ExplorationTable explorationTable = new ExplorationTable();
			explorationTable.SetData(data);
			NrTSingleton<ExplorationManager>.Instance.AddExplorationTable(explorationTable);
		}
		return true;
	}
}
