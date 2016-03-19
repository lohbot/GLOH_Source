using System;
using TsLibs;

public class COLOSSEUM_GRADEINFO : NrTableData
{
	public short m_nGrade;

	public string m_Textkey_InterFace;

	public string m_GradeIcon_ImageKey;

	public COLOSSEUM_GRADEINFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nGrade = 0;
		this.m_Textkey_InterFace = null;
		this.m_GradeIcon_ImageKey = null;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nGrade);
		row.GetColumn(num++, out this.m_Textkey_InterFace);
		row.GetColumn(num++, out this.m_GradeIcon_ImageKey);
	}
}
