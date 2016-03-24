using System;
using TsLibs;

public class SETITEM_DATA : NrTableData
{
	public int m_Idx;

	public string[] m_strSetEffectCode;

	public int[] m_nValue;

	public byte[] m_byOrder;

	public int[] m_nSetEffectCode;

	public string m_strTextKey;

	public SETITEM_DATA()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_Idx = 0;
		this.m_strSetEffectCode = new string[6];
		this.m_nValue = new int[6];
		this.m_byOrder = new byte[6];
		this.m_nSetEffectCode = new int[6];
		this.m_strTextKey = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_Idx);
		row.GetColumn(num++, out this.m_strSetEffectCode[0]);
		row.GetColumn(num++, out this.m_nValue[0]);
		row.GetColumn(num++, out this.m_byOrder[0]);
		row.GetColumn(num++, out this.m_strSetEffectCode[1]);
		row.GetColumn(num++, out this.m_nValue[1]);
		row.GetColumn(num++, out this.m_byOrder[1]);
		row.GetColumn(num++, out this.m_strSetEffectCode[2]);
		row.GetColumn(num++, out this.m_nValue[2]);
		row.GetColumn(num++, out this.m_byOrder[2]);
		row.GetColumn(num++, out this.m_strSetEffectCode[3]);
		row.GetColumn(num++, out this.m_nValue[3]);
		row.GetColumn(num++, out this.m_byOrder[3]);
		row.GetColumn(num++, out this.m_strSetEffectCode[4]);
		row.GetColumn(num++, out this.m_nValue[4]);
		row.GetColumn(num++, out this.m_byOrder[4]);
		row.GetColumn(num++, out this.m_strSetEffectCode[5]);
		row.GetColumn(num++, out this.m_nValue[5]);
		row.GetColumn(num++, out this.m_byOrder[5]);
		row.GetColumn(num++, out this.m_strTextKey);
	}
}
