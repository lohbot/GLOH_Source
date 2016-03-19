using System;
using TsLibs;

public class Evolution_EXP_Penalty : NrTableData
{
	public byte BaseSeason;

	public int[] SeasonPenalty = new int[6];

	public Evolution_EXP_Penalty()
	{
		this.Init();
	}

	public void Init()
	{
		this.BaseSeason = 0;
		for (int i = 0; i < 6; i++)
		{
			this.SeasonPenalty[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.BaseSeason);
		for (int i = 0; i < 6; i++)
		{
			row.GetColumn(num++, out this.SeasonPenalty[i]);
		}
	}
}
