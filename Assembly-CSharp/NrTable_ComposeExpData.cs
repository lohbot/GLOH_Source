using System;
using TsLibs;

public class NrTable_ComposeExpData : NrTableBase
{
	public NrTable_ComposeExpData() : base(CDefinePath.SOLCOMPOSEEXP_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ComposeExpData composeExpData = new ComposeExpData();
			composeExpData.SetData(data);
			NrTSingleton<ComposeExpManager>.Instance.Set_Value(composeExpData);
		}
		return true;
	}
}
