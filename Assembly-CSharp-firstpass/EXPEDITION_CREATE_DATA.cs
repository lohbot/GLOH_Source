using System;
using TsLibs;

public class EXPEDITION_CREATE_DATA : NrTableData
{
	public short EXPEDITION_CREATE_ID;

	public byte EXPEDITION_GRADE;

	public int EXPEDITION_ITEM_UNIQUE;

	public int EXPEDITION_GIVE_NUM;

	public int EXPEDITION_CREATE_MIN_ITEMNUM;

	public int EXPEDITION_CREATE_MAX_ITEMNUM;

	public short EXPEDITION_CREATE_MIN_MINLEVEL;

	public short EXPEDITION_CREATE_MIN_MAXLEVEL;

	public int[] EXPEDITION_ECO = new int[3];

	public EXPEDITION_CREATE_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.EXPEDITION_CREATE_ID = 0;
		this.EXPEDITION_GRADE = 0;
		this.EXPEDITION_ITEM_UNIQUE = 0;
		this.EXPEDITION_GIVE_NUM = 0;
		this.EXPEDITION_CREATE_MIN_ITEMNUM = 0;
		this.EXPEDITION_CREATE_MAX_ITEMNUM = 0;
		this.EXPEDITION_CREATE_MIN_MINLEVEL = 0;
		this.EXPEDITION_CREATE_MIN_MAXLEVEL = 0;
		for (int i = 0; i < 3; i++)
		{
			this.EXPEDITION_ECO[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.EXPEDITION_CREATE_ID);
		row.GetColumn(num++, out this.EXPEDITION_GRADE);
		row.GetColumn(num++, out this.EXPEDITION_ITEM_UNIQUE);
		row.GetColumn(num++, out this.EXPEDITION_GIVE_NUM);
		row.GetColumn(num++, out this.EXPEDITION_CREATE_MIN_ITEMNUM);
		row.GetColumn(num++, out this.EXPEDITION_CREATE_MAX_ITEMNUM);
		row.GetColumn(num++, out this.EXPEDITION_CREATE_MIN_MINLEVEL);
		row.GetColumn(num++, out this.EXPEDITION_CREATE_MIN_MAXLEVEL);
		row.GetColumn(num++, out this.EXPEDITION_ECO[0]);
		row.GetColumn(num++, out this.EXPEDITION_ECO[1]);
		row.GetColumn(num++, out this.EXPEDITION_ECO[2]);
	}

	public byte GetGrade()
	{
		return this.EXPEDITION_GRADE;
	}
}
