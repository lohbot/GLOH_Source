using System;
using TsLibs;

public class NrFontColorTable : NrTableBase
{
	public NrFontColorTable(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row tsRow in dr)
		{
			FONT_COLOR data = new FONT_COLOR(tsRow);
			if (!NrTSingleton<CTextParser>.Instance.SetData(data))
			{
				TsLog.LogError("Error! Parsing - " + this.m_strFilePath, new object[0]);
			}
		}
		return true;
	}
}
