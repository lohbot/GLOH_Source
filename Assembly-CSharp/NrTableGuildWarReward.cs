using System;
using TsLibs;

public class NrTableGuildWarReward : NrTableBase
{
	public NrTableGuildWarReward() : base(CDefinePath.GUILDWAR_REWARD)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			GUILDWAR_REWARD_DATA gUILDWAR_REWARD_DATA = new GUILDWAR_REWARD_DATA();
			gUILDWAR_REWARD_DATA.SetData(data);
			NrTSingleton<GuildWarManager>.Instance.Set_Value(gUILDWAR_REWARD_DATA);
		}
		return true;
	}
}
