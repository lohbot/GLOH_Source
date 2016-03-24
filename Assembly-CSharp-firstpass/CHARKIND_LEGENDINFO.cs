using System;
using TsLibs;

public class CHARKIND_LEGENDINFO : NrTableData
{
	public string CharCode = string.Empty;

	public string CharName = string.Empty;

	public short i16SortNum;

	public int i32Element_LegendCharkind;

	public byte ui8Element_LegendGrade;

	public int[] i32Base_CharKind = new int[5];

	public string[] i32Base_LegendCharCode = new string[5];

	public byte[] ui8Base_LegendGrade = new byte[5];

	public long i64NeedMoney;

	public int i32EssenceUnique;

	public int i32NeedEssence;

	public CHARKIND_LEGENDINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_LEGENDINFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.CharCode = string.Empty;
		this.CharName = string.Empty;
		this.i16SortNum = 0;
		this.i32Element_LegendCharkind = 0;
		this.ui8Element_LegendGrade = 0;
		for (int i = 0; i < 5; i++)
		{
			this.i32Base_CharKind[i] = 0;
			this.ui8Base_LegendGrade[i] = 0;
			this.i32Base_LegendCharCode[i] = string.Empty;
		}
		this.i64NeedMoney = 0L;
		this.i32EssenceUnique = 0;
		this.i32NeedEssence = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.CharCode);
		row.GetColumn(num++, out this.CharName);
		row.GetColumn(num++, out this.i16SortNum);
		row.GetColumn(num++, out this.ui8Element_LegendGrade);
		row.GetColumn(num++, out this.i32Base_LegendCharCode[0]);
		row.GetColumn(num++, out this.ui8Base_LegendGrade[0]);
		row.GetColumn(num++, out this.i32Base_LegendCharCode[1]);
		row.GetColumn(num++, out this.ui8Base_LegendGrade[1]);
		row.GetColumn(num++, out this.i32Base_LegendCharCode[2]);
		row.GetColumn(num++, out this.ui8Base_LegendGrade[2]);
		row.GetColumn(num++, out this.i32Base_LegendCharCode[3]);
		row.GetColumn(num++, out this.ui8Base_LegendGrade[3]);
		row.GetColumn(num++, out this.i32Base_LegendCharCode[4]);
		row.GetColumn(num++, out this.ui8Base_LegendGrade[4]);
		row.GetColumn(num++, out this.i64NeedMoney);
		row.GetColumn(num++, out this.i32EssenceUnique);
		row.GetColumn(num++, out this.i32NeedEssence);
	}
}
