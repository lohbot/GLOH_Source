using System;
using TsLibs;

public class NrTable_GuildWarable : NrTableBase
{
	public NrTable_GuildWarable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			GuildWarExchangeTable guildWarExchangeTable = new GuildWarExchangeTable();
			guildWarExchangeTable.SetData(data);
			NrTSingleton<PointManager>.Instance.AddGuildWarExchangeTable(guildWarExchangeTable);
		}
		return true;
	}
}
