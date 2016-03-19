using System;
using TsLibs;

public class NrTableEcoTalk : NrTableBase
{
	public NrTableEcoTalk(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		return base.ParseDataFromNDT_ForHelper<ECO_TALK>(dr);
	}
}
