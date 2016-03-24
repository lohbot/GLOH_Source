using System;
using TsLibs;

public class NrTable_ExchangeEvolutionTable : NrTableBase
{
	public NrTable_ExchangeEvolutionTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ExchangeEvolutionTable exchangeEvolutionTable = new ExchangeEvolutionTable();
			exchangeEvolutionTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddEvolutionExchangeTable(exchangeEvolutionTable);
		}
		return true;
	}
}
