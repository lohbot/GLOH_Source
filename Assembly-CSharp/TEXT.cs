using System;
using TsLibs;

public class TEXT
{
	public int LANG_TYPE;

	public string TEXTKEY = string.Empty;

	public string DATA = string.Empty;

	public void SetData(TsDataReader.Row row)
	{
		this.LANG_TYPE = 0;
		this.TEXTKEY = string.Empty;
		this.DATA = string.Empty;
		row.GetColumn(0, out this.LANG_TYPE);
		row.GetColumn(1, out this.TEXTKEY);
		row.GetColumn(2, out this.DATA);
	}
}
