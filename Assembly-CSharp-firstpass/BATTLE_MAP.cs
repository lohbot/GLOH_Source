using System;
using TsLibs;

public class BATTLE_MAP : NrTableData
{
	public short BATTLE_MAP_ID;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public short Type;

	public short SIZE_X;

	public short SIZE_Y;

	public short CELLCOUNT_X;

	public short CELLCOUNT_Y;

	public short CELL_SIZE;

	public string WEB_BUNDLE_PATH = string.Empty;

	public string MOBILE_BUNDLE_PATH = string.Empty;

	public string BUNDLE_PATH
	{
		get
		{
			if (TsPlatform.IsMobile)
			{
				return this.MOBILE_BUNDLE_PATH;
			}
			return this.WEB_BUNDLE_PATH;
		}
	}

	public void Init()
	{
		this.BATTLE_MAP_ID = 0;
		this.TEXTKEY = string.Empty;
		this.ENG_NAME = string.Empty;
		this.Type = 0;
		this.SIZE_X = 0;
		this.SIZE_Y = 0;
		this.CELLCOUNT_X = 0;
		this.CELLCOUNT_Y = 0;
		this.CELL_SIZE = 0;
		this.WEB_BUNDLE_PATH = string.Empty;
		this.MOBILE_BUNDLE_PATH = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.BATTLE_MAP_ID);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.ENG_NAME);
		row.GetColumn(num++, out this.Type);
		row.GetColumn(num++, out this.SIZE_X);
		row.GetColumn(num++, out this.SIZE_Y);
		row.GetColumn(num++, out this.CELLCOUNT_X);
		row.GetColumn(num++, out this.CELLCOUNT_Y);
		row.GetColumn(num++, out this.CELL_SIZE);
		row.GetColumn(num++, out this.WEB_BUNDLE_PATH);
		row.GetColumn(num++, out this.MOBILE_BUNDLE_PATH);
	}

	public void SetData(BATTLE_MAP pData)
	{
		if (pData == null)
		{
			return;
		}
		this.BATTLE_MAP_ID = pData.BATTLE_MAP_ID;
		this.TEXTKEY = string.Format("{0}", pData.TEXTKEY);
		this.ENG_NAME = string.Format("{0}", pData.ENG_NAME);
		this.Type = pData.Type;
		this.SIZE_X = pData.SIZE_X;
		this.SIZE_Y = pData.SIZE_Y;
		this.CELL_SIZE = pData.CELL_SIZE;
		this.CELLCOUNT_X = pData.CELLCOUNT_X;
		this.CELLCOUNT_Y = pData.CELLCOUNT_Y;
		this.WEB_BUNDLE_PATH = string.Format("{0}", pData.WEB_BUNDLE_PATH);
	}
}
