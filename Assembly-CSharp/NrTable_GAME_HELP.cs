using System;
using TsLibs;

public class NrTable_GAME_HELP : NrTableBase
{
	public NrTable_GAME_HELP() : base(CDefinePath.GAMEHELP_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			GameHelpInfo_Data gameHelpInfo_Data = new GameHelpInfo_Data();
			gameHelpInfo_Data.SetData(data);
			NrTSingleton<NrGameHelpInfoManager>.Instance.SetData(gameHelpInfo_Data);
		}
		return true;
	}
}
