using System;
using TsLibs;

public class ITEM_COMPOSE : NrTableData
{
	public int m_nComposeProductionID;

	public int m_nComposeNpcKind;

	public int m_nComposeItemUnique;

	public int m_nComposeItemNum;

	public int[] m_nMaterialItemUnique = new int[10];

	public int[] m_nMaterialItemNum = new int[10];

	public ITEM_COMPOSE()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nComposeProductionID = 0;
		this.m_nComposeNpcKind = 0;
		this.m_nComposeItemUnique = 0;
		this.m_nComposeItemNum = 0;
		for (int i = 0; i < 10; i++)
		{
			this.m_nMaterialItemUnique[i] = 0;
			this.m_nMaterialItemNum[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.m_nComposeProductionID);
		row.GetColumn(num++, out this.m_nComposeNpcKind);
		row.GetColumn(num++, out this.m_nComposeItemUnique);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.m_nComposeItemNum);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[0]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[0]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[1]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[1]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[2]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[2]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[3]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[3]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[4]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[4]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[5]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[5]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[6]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[6]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[7]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[7]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[8]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[8]);
		row.GetColumn(num++, out this.m_nMaterialItemUnique[9]);
		row.GetColumn(num++, out this.m_nMaterialItemNum[9]);
	}
}
