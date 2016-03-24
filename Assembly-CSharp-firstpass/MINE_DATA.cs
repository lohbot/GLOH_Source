using System;
using TsLibs;

public class MINE_DATA : NrTableData
{
	public string MINE_GRADE = string.Empty;

	public byte nMine_Grade;

	public int MINE_TOTALCOUNT;

	public short POSSIBLELEVEL;

	public long MINE_SEARCH_MONEY;

	public string MINE_INTERFACEKEY = string.Empty;

	public string MINE_GRADE_INTERFACEKEY = string.Empty;

	public string MINE_GRADECOUNT_INTERFACEKEY = string.Empty;

	public string MINE_BG_NAME = string.Empty;

	public string MINE_ICON_NAME = string.Empty;

	public string MINE_BG1_NAME = string.Empty;

	public short SOLPOSSIBLELEVEL;

	public short MINE_CREATE_START_ID;

	public int nItemUnique;

	public int nItemNum;

	public int nMine_min;

	public int nMine_max;

	public int nDivision_num;

	public string Mine_UI_Icon = string.Empty;

	public MINE_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.MINE_GRADE = string.Empty;
		this.nMine_Grade = 0;
		this.MINE_TOTALCOUNT = 0;
		this.POSSIBLELEVEL = 0;
		this.MINE_SEARCH_MONEY = 0L;
		this.MINE_INTERFACEKEY = string.Empty;
		this.MINE_GRADE_INTERFACEKEY = string.Empty;
		this.MINE_GRADECOUNT_INTERFACEKEY = string.Empty;
		this.MINE_BG_NAME = string.Empty;
		this.MINE_ICON_NAME = string.Empty;
		this.MINE_BG1_NAME = string.Empty;
		this.SOLPOSSIBLELEVEL = 0;
		this.MINE_CREATE_START_ID = 0;
		this.nItemUnique = 0;
		this.nItemNum = 0;
		this.nMine_min = 0;
		this.nMine_max = 0;
		this.nDivision_num = 0;
		this.Mine_UI_Icon = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MINE_GRADE);
		row.GetColumn(num++, out this.MINE_TOTALCOUNT);
		row.GetColumn(num++, out this.POSSIBLELEVEL);
		row.GetColumn(num++, out this.MINE_SEARCH_MONEY);
		row.GetColumn(num++, out this.MINE_INTERFACEKEY);
		row.GetColumn(num++, out this.MINE_GRADE_INTERFACEKEY);
		row.GetColumn(num++, out this.MINE_GRADECOUNT_INTERFACEKEY);
		row.GetColumn(num++, out this.MINE_BG_NAME);
		row.GetColumn(num++, out this.MINE_ICON_NAME);
		row.GetColumn(num++, out this.MINE_BG1_NAME);
		row.GetColumn(num++, out this.SOLPOSSIBLELEVEL);
		row.GetColumn(num++, out this.MINE_CREATE_START_ID);
		row.GetColumn(num++, out this.nItemUnique);
		row.GetColumn(num++, out this.nItemNum);
		row.GetColumn(num++, out this.nMine_min);
		row.GetColumn(num++, out this.nMine_max);
		row.GetColumn(num++, out this.nDivision_num);
		row.GetColumn(num++, out this.Mine_UI_Icon);
	}

	public byte GetGrade()
	{
		return this.nMine_Grade;
	}
}
