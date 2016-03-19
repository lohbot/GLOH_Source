using System;
using TsLibs;

public class QUEST_DROP_ITEM
{
	public string strCharCode = string.Empty;

	public string strQuestUnique = string.Empty;

	public long nItemUnique;

	public QUEST_DROP_ITEM()
	{
		this.Init();
	}

	public void Init()
	{
		this.strCharCode = string.Empty;
		this.strQuestUnique = string.Empty;
		this.nItemUnique = 0L;
	}

	public void SetData(TsDataReader.Row row)
	{
		this.Init();
		long num = 0L;
		int num2 = 0;
		row.GetColumn(num2++, out num);
		row.GetColumn(num2++, out this.strCharCode);
		row.GetColumn(num2++, out this.strQuestUnique);
		row.GetColumn(num2++, out this.nItemUnique);
	}
}
