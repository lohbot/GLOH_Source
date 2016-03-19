using System;
using TsLibs;

public class CAutoQuest
{
	private int nMapIndex;

	private string strQuestUnique = string.Empty;

	private bool bAccept;

	private bool bComlete;

	private int nRewardMapIndex;

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.nMapIndex);
		row.GetColumn(num++, out this.strQuestUnique);
		row.GetColumn(num++, out this.bAccept);
		row.GetColumn(num++, out this.bComlete);
		row.GetColumn(num++, out this.nRewardMapIndex);
	}

	public int GetMapUnique()
	{
		return this.nMapIndex;
	}

	public string GetQuestUnique()
	{
		return this.strQuestUnique;
	}

	public bool GetAccept()
	{
		return this.bAccept;
	}

	public bool GetComplete()
	{
		return this.bComlete;
	}

	public int GetRewardMapIndex()
	{
		return this.nRewardMapIndex;
	}
}
