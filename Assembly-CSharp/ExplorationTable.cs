using System;
using TsLibs;

public class ExplorationTable
{
	public int m_nLimitLevel;

	public int m_nRate;

	public string[] m_szTexture = new string[3];

	public long[] m_nRewardMoney = new long[3];

	public int[] m_nRewardItemUnique = new int[3];

	public int[] m_nRewardItemNum = new int[3];

	public ExplorationTable()
	{
		this.m_nLimitLevel = 0;
		this.m_nRate = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_szTexture[i] = string.Empty;
			this.m_nRewardMoney[i] = 0L;
			this.m_nRewardItemUnique[i] = 0;
			this.m_nRewardItemNum[i] = 0;
		}
	}

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_nLimitLevel);
		row.GetColumn(num++, out this.m_nRate);
		for (int i = 0; i < 3; i++)
		{
			row.GetColumn(num++, out this.m_szTexture[i]);
			string empty = string.Empty;
			row.GetColumn(num++, out empty);
			if (empty == "MONEY")
			{
				row.GetColumn(num++, out this.m_nRewardMoney[i]);
				long num2 = 0L;
				row.GetColumn(num++, out num2);
			}
			else if (empty == "ITEM")
			{
				row.GetColumn(num++, out this.m_nRewardItemUnique[i]);
				row.GetColumn(num++, out this.m_nRewardItemNum[i]);
			}
			else
			{
				this.m_nRewardMoney[i] = 0L;
				this.m_nRewardItemUnique[i] = 0;
				this.m_nRewardItemNum[i] = 0;
			}
		}
	}
}
