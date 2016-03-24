using System;
using TsLibs;

public class TableMovieUrl : NrTableBase
{
	public TableMovieUrl(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			NrTSingleton<NrTableMovieUrlManager>.Instance.SetData(data);
		}
		return true;
	}
}
