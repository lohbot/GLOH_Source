using System;
using TsLibs;

public class NrTableAutoQuest : NrTableBase
{
	public NrTableAutoQuest(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			CAutoQuest cAutoQuest = new CAutoQuest();
			cAutoQuest.SetData(data);
			NrTSingleton<NkQuestManager>.Instance.AddAutoQuest(cAutoQuest);
		}
		return true;
	}
}
