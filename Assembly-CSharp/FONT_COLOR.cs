using System;
using System.Globalization;
using TsLibs;

internal struct FONT_COLOR
{
	private string strCODE;

	private string strRed;

	private string strGreen;

	private string strBlue;

	private string strAlpha;

	public string ColorCode
	{
		get
		{
			return this.strCODE;
		}
	}

	public string ColorText
	{
		get
		{
			return string.Format("[#{0}{1}{2}{3}]", new object[]
			{
				int.Parse(this.strRed, NumberStyles.HexNumber).ToString("X2"),
				int.Parse(this.strGreen, NumberStyles.HexNumber).ToString("X2"),
				int.Parse(this.strBlue, NumberStyles.HexNumber).ToString("X2"),
				int.Parse(this.strAlpha, NumberStyles.HexNumber).ToString("X2")
			});
		}
	}

	public FONT_COLOR(TsDataReader.Row tsRow)
	{
		this.strCODE = string.Empty;
		this.strRed = string.Empty;
		this.strGreen = string.Empty;
		this.strBlue = string.Empty;
		this.strAlpha = string.Empty;
		this.SetData(tsRow);
	}

	private void SetData(TsDataReader.Row tsRow)
	{
		tsRow.GetColumn(0, out this.strCODE);
		tsRow.GetColumn(1, out this.strRed);
		tsRow.GetColumn(2, out this.strGreen);
		tsRow.GetColumn(3, out this.strBlue);
		tsRow.GetColumn(4, out this.strAlpha);
	}
}
