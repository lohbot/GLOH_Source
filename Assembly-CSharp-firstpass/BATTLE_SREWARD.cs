using System;
using TsLibs;

public class BATTLE_SREWARD : NrTableData
{
	public int m_nRewardUnique;

	public int m_nRewardUserLevel;

	public SREWARD_PRODUCT[] m_sRewardProduct;

	public BATTLE_SREWARD()
	{
		this.m_sRewardProduct = new SREWARD_PRODUCT[4];
		for (int i = 0; i < 4; i++)
		{
			if (this.m_sRewardProduct[i] == null)
			{
				this.m_sRewardProduct[i] = new SREWARD_PRODUCT();
			}
		}
		this.Init();
	}

	public void Init()
	{
		this.m_nRewardUnique = 0;
		this.m_nRewardUserLevel = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.m_sRewardProduct[i] != null)
			{
				this.m_sRewardProduct[i].Init();
			}
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.m_nRewardUnique);
		row.GetColumn(num++, out this.m_nRewardUserLevel);
		row.GetColumn(num++, out this.m_sRewardProduct[0].m_strParserRewardType);
		row.GetColumn(num++, out this.m_sRewardProduct[0].m_nRewardRate);
		row.GetColumn(num++, out this.m_sRewardProduct[0].m_nRewardValue1);
		row.GetColumn(num++, out this.m_sRewardProduct[0].m_nRewardValue2);
		row.GetColumn(num++, out this.m_sRewardProduct[0].m_stRewardTexture);
		row.GetColumn(num++, out this.m_sRewardProduct[1].m_strParserRewardType);
		row.GetColumn(num++, out this.m_sRewardProduct[1].m_nRewardRate);
		row.GetColumn(num++, out this.m_sRewardProduct[1].m_nRewardValue1);
		row.GetColumn(num++, out this.m_sRewardProduct[1].m_nRewardValue2);
		row.GetColumn(num++, out this.m_sRewardProduct[1].m_stRewardTexture);
		row.GetColumn(num++, out this.m_sRewardProduct[2].m_strParserRewardType);
		row.GetColumn(num++, out this.m_sRewardProduct[2].m_nRewardRate);
		row.GetColumn(num++, out this.m_sRewardProduct[2].m_nRewardValue1);
		row.GetColumn(num++, out this.m_sRewardProduct[2].m_nRewardValue2);
		row.GetColumn(num++, out this.m_sRewardProduct[2].m_stRewardTexture);
		row.GetColumn(num++, out this.m_sRewardProduct[3].m_strParserRewardType);
		row.GetColumn(num++, out this.m_sRewardProduct[3].m_nRewardRate);
		row.GetColumn(num++, out this.m_sRewardProduct[3].m_nRewardValue1);
		row.GetColumn(num++, out this.m_sRewardProduct[3].m_nRewardValue2);
		row.GetColumn(num++, out this.m_sRewardProduct[3].m_stRewardTexture);
	}
}
