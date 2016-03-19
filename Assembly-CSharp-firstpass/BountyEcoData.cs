using System;
using TsLibs;

public class BountyEcoData : NrTableData
{
	public short i16EcoIndex;

	public short i16BattleCount;

	public short i16BattlePosUnique;

	public string[] strCharCode = new string[11];

	public int[] i32CharKind = new int[11];

	public short[] i16BattlePos = new short[11];

	public BountyEcoData()
	{
		this.Init();
	}

	public void Init()
	{
		this.i16EcoIndex = 0;
		this.i16BattleCount = 0;
		this.i16BattlePosUnique = 0;
		for (int i = 0; i < 11; i++)
		{
			this.strCharCode[i] = string.Empty;
			this.i32CharKind[i] = 0;
			this.i16BattlePos[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.i16EcoIndex);
		row.GetColumn(num++, out this.i16BattleCount);
		row.GetColumn(num++, out this.i16BattlePosUnique);
		for (int i = 0; i < 5; i++)
		{
			row.GetColumn(num++, out this.strCharCode[i]);
			row.GetColumn(num++, out this.i16BattlePos[i]);
		}
	}
}
