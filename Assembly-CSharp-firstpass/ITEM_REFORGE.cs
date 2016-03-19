using System;
using TsLibs;

public class ITEM_REFORGE : NrTableData
{
	public int nGroupUnique;

	public int nItemMakeRank;

	public int nReforgeItemUnique;

	public int nReforgeItemNum;

	public int nReforgeGold;

	public int nUndoUnique;

	public int nUndoNum;

	public int nEnhanceUnique;

	public int nEnhancenum;

	public string stRank = string.Empty;

	public ITEM_REFORGE()
	{
		this.Init();
	}

	public void Init()
	{
		this.nGroupUnique = 0;
		this.nItemMakeRank = 0;
		this.nReforgeItemUnique = 0;
		this.nReforgeItemNum = 0;
		this.nReforgeGold = 0;
		this.nUndoUnique = 0;
		this.nUndoNum = 0;
		this.nEnhanceUnique = 0;
		this.nEnhancenum = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.nGroupUnique);
		row.GetColumn(num++, out this.stRank);
		row.GetColumn(num++, out this.nReforgeItemUnique);
		row.GetColumn(num++, out this.nReforgeItemNum);
		row.GetColumn(num++, out this.nReforgeGold);
		row.GetColumn(num++, out this.nUndoUnique);
		row.GetColumn(num++, out this.nUndoNum);
		row.GetColumn(num++, out this.nEnhanceUnique);
		row.GetColumn(num++, out this.nEnhancenum);
	}
}
