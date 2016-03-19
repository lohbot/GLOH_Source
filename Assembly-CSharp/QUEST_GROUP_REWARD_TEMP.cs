using System;
using TsLibs;

public class QUEST_GROUP_REWARD_TEMP
{
	public int i32GroupUnique;

	public int i32Level;

	public int nItemUnique;

	public int i32LangType;

	public string strTextKey = string.Empty;

	public int i32ItemNum;

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.i32GroupUnique);
		row.GetColumn(num++, out this.i32Level);
		row.GetColumn(num++, out this.nItemUnique);
		row.GetColumn(num++, out this.i32LangType);
		row.GetColumn(num++, out this.strTextKey);
		row.GetColumn(num++, out this.i32ItemNum);
	}
}
