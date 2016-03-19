using System;
using TsLibs;

public class NrTablePatchLoading : NrTableBase
{
	public NrTablePatchLoading(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			PatchLoading_Data patchLoading_Data = new PatchLoading_Data();
			patchLoading_Data.SetData(data);
			NrTSingleton<PatchLoading_Data_Manager>.Instance.Add(patchLoading_Data);
		}
		return true;
	}

	public override void ParseRowData(TsDataReader.Row tsRow)
	{
		PatchLoading_Data patchLoading_Data = new PatchLoading_Data();
		patchLoading_Data.SetData(tsRow);
		NrTSingleton<PatchLoading_Data_Manager>.Instance.Add(patchLoading_Data);
	}
}
