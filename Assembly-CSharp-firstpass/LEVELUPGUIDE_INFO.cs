using System;
using System.Collections.Generic;
using TsLibs;

public class LEVELUPGUIDE_INFO : NrTableData
{
	private readonly int MAX_TEXTUREEXPLAIN = 5;

	public int LEVEL;

	public string EXPLAIN = string.Empty;

	public List<string> explainList = new List<string>();

	public LEVELUPGUIDE_INFO() : base(NrTableData.eResourceType.eRT_LEVELUPGUIDE)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.LEVEL);
		for (int i = 0; i < this.MAX_TEXTUREEXPLAIN; i++)
		{
			row.GetColumn(num++, out this.EXPLAIN);
			this.explainList.Add(this.EXPLAIN);
		}
	}
}
