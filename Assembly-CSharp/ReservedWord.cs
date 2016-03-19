using System;
using TsLibs;

public class ReservedWord
{
	public string text = string.Empty;

	public int value;

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.text);
		row.GetColumn(num++, out this.value);
	}
}
