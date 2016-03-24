using System;
using TsLibs;

public class NrTableCharCostume : NrTableBase
{
	public NrTableCharCostume(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		int num = 0;
		foreach (TsDataReader.Row dataRow in dr)
		{
			NrTSingleton<NrCharCostumeTableManager>.Instance.SetData(num, dataRow);
			num++;
		}
		return true;
	}
}
