using System;
using TsLibs;

public class ATTENDANCE : NrTableData
{
	public int m_i32Index;

	public short m_i16UserType;

	public int m_i32Group;

	public short m_i16Attend_Sequence;

	public int m_i32Item_Unique;

	public short m_i16Item_Num;

	public string m_strImageBundle;

	public byte m_i8RewardType;

	public override void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_i32Index);
		row.GetColumn(num++, out this.m_i16UserType);
		row.GetColumn(num++, out this.m_i32Group);
		row.GetColumn(num++, out this.m_i16Attend_Sequence);
		num++;
		row.GetColumn(num++, out this.m_i32Item_Unique);
		row.GetColumn(num++, out this.m_i16Item_Num);
		row.GetColumn(num++, out this.m_strImageBundle);
		row.GetColumn(num++, out this.m_i8RewardType);
	}
}
