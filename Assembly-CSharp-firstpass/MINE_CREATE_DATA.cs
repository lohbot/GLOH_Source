using System;
using TsLibs;

public class MINE_CREATE_DATA : NrTableData
{
	public short MINE_ID;

	public string MINE_GRADE = string.Empty;

	public byte nMine_Grade;

	public int MINE_ITEM_UNIQUE;

	public int MINE_TOTAL_NUM;

	public int MINE_GIVE_NUM;

	public int[] MINE_ECO = new int[9];

	public short MINE_MON_LEVEL;

	public string Mine_MiniIcon = string.Empty;

	public MINE_CREATE_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.MINE_ID = 0;
		this.MINE_GRADE = string.Empty;
		this.nMine_Grade = 0;
		this.MINE_ITEM_UNIQUE = 0;
		this.MINE_TOTAL_NUM = 0;
		this.MINE_GIVE_NUM = 0;
		for (int i = 0; i < 9; i++)
		{
			this.MINE_ECO[i] = 0;
		}
		this.MINE_MON_LEVEL = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MINE_ID);
		row.GetColumn(num++, out this.MINE_GRADE);
		row.GetColumn(num++, out this.MINE_ITEM_UNIQUE);
		row.GetColumn(num++, out this.MINE_TOTAL_NUM);
		row.GetColumn(num++, out this.MINE_GIVE_NUM);
		row.GetColumn(num++, out this.MINE_ECO[0]);
		row.GetColumn(num++, out this.MINE_ECO[1]);
		row.GetColumn(num++, out this.MINE_ECO[2]);
		row.GetColumn(num++, out this.MINE_ECO[3]);
		row.GetColumn(num++, out this.MINE_ECO[4]);
		row.GetColumn(num++, out this.MINE_ECO[5]);
		row.GetColumn(num++, out this.MINE_ECO[6]);
		row.GetColumn(num++, out this.MINE_ECO[7]);
		row.GetColumn(num++, out this.MINE_ECO[8]);
		row.GetColumn(num++, out this.MINE_MON_LEVEL);
		row.GetColumn(num++, out this.Mine_MiniIcon);
	}

	public byte GetGrade()
	{
		return this.nMine_Grade;
	}
}
