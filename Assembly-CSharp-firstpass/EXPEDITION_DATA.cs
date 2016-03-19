using System;
using TsLibs;

public class EXPEDITION_DATA : NrTableData
{
	public byte nExpedition_Grade;

	public string Expedition_Name = string.Empty;

	public short Possiblelevel;

	public short SolPossiblelevel;

	public long Expedition_SEARCH_MONEY;

	public string Expedition_INTERFACEKEY = string.Empty;

	public string Expedition_GRADE_INTERFACEKEY = string.Empty;

	public string Expedition_GRADECOUNT_INTERFACEKEY = string.Empty;

	public string Expedition_BG_NAME = string.Empty;

	public string Expedition_ICON_NAME = string.Empty;

	public string Expedition_BG1_NAME = string.Empty;

	public int Expedition_SolBatch_Array;

	public string Expedition_UI_ICON = string.Empty;

	public EXPEDITION_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.nExpedition_Grade = 0;
		this.Expedition_Name = string.Empty;
		this.Possiblelevel = 0;
		this.SolPossiblelevel = 0;
		this.Expedition_SEARCH_MONEY = 0L;
		this.Expedition_INTERFACEKEY = string.Empty;
		this.Expedition_GRADE_INTERFACEKEY = string.Empty;
		this.Expedition_GRADECOUNT_INTERFACEKEY = string.Empty;
		this.Expedition_BG_NAME = string.Empty;
		this.Expedition_ICON_NAME = string.Empty;
		this.Expedition_BG1_NAME = string.Empty;
		this.Expedition_SolBatch_Array = 0;
		this.Expedition_UI_ICON = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nExpedition_Grade);
		row.GetColumn(num++, out this.Expedition_Name);
		row.GetColumn(num++, out this.Possiblelevel);
		row.GetColumn(num++, out this.SolPossiblelevel);
		row.GetColumn(num++, out this.Expedition_SEARCH_MONEY);
		row.GetColumn(num++, out this.Expedition_INTERFACEKEY);
		row.GetColumn(num++, out this.Expedition_GRADE_INTERFACEKEY);
		row.GetColumn(num++, out this.Expedition_GRADECOUNT_INTERFACEKEY);
		row.GetColumn(num++, out this.Expedition_BG_NAME);
		row.GetColumn(num++, out this.Expedition_ICON_NAME);
		row.GetColumn(num++, out this.Expedition_BG1_NAME);
		row.GetColumn(num++, out this.Expedition_SolBatch_Array);
		row.GetColumn(num++, out this.Expedition_UI_ICON);
	}

	public byte GetGrade()
	{
		return this.nExpedition_Grade;
	}
}
