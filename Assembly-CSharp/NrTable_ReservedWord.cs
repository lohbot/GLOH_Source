using System;
using TsLibs;

public class NrTable_ReservedWord : NrTableBase
{
	public NrTable_ReservedWord(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ReservedWord reservedWord = new ReservedWord();
			reservedWord.SetData(data);
			NrTSingleton<ReservedWordManager>.Instance.AddReservedWord(reservedWord);
		}
		return true;
	}
}
