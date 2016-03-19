using System;
using TsLibs;

public class NrGuild_EXPTable : NrTableBase
{
	public NrGuild_EXPTable(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		NrTSingleton<NkGuildExpManager>.Instance.Init();
		foreach (TsDataReader.Row data in dr)
		{
			GUILD_EXP gUILD_EXP = new GUILD_EXP();
			gUILD_EXP.SetData(data);
			NrTSingleton<NkGuildExpManager>.Instance.Set_Value(gUILD_EXP);
		}
		return true;
	}
}
