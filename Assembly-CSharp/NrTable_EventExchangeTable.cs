using System;
using TsLibs;

public class NrTable_EventExchangeTable : NrTableBase
{
	public NrTable_EventExchangeTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EventExchangeTable eventExchangeTable = new EventExchangeTable();
			eventExchangeTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddEventExchangeTable(eventExchangeTable);
		}
		return true;
	}
}
