using System;
using TsLibs;

public class NrTable_Achivement_Google : NrTableBase
{
	public NrTable_Achivement_Google() : base(CDefinePath.ACHIVEMENT_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			Achivement_GoogleData achivement_GoogleData = new Achivement_GoogleData();
			achivement_GoogleData.SetData(data);
			NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.SetData(achivement_GoogleData);
		}
		return true;
	}
}
