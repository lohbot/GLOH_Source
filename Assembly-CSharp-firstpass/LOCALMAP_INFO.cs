using System;
using TsLibs;

public class LOCALMAP_INFO : NrTableData
{
	public int LOCALMAP_IDX;

	public string LOCALMAP_NAME_TEXT_INDEX = string.Empty;

	public string LOCALMAP_ICON = string.Empty;

	public float LOCALMAP_X;

	public float LOCALMAP_Y;

	public string LOCALMAP_BUNDLE_PATH = string.Empty;

	public string LOCALMAP_MOBILE_BUNDLE_PATH = string.Empty;

	public int[] MAP_INDEX = new int[20];

	public string LOCALBUNDLE_PATH
	{
		get
		{
			return this.LOCALMAP_BUNDLE_PATH;
		}
	}

	public LOCALMAP_INFO() : base(NrTableData.eResourceType.eRT_LOCALMAP_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.LOCALMAP_IDX = 0;
		this.LOCALMAP_NAME_TEXT_INDEX = string.Empty;
		this.LOCALMAP_X = 0f;
		this.LOCALMAP_Y = 0f;
		this.LOCALMAP_BUNDLE_PATH = string.Empty;
		this.LOCALMAP_MOBILE_BUNDLE_PATH = string.Empty;
		for (int i = 0; i < 20; i++)
		{
			this.MAP_INDEX[i] = 0;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.LOCALMAP_IDX);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.LOCALMAP_NAME_TEXT_INDEX);
		row.GetColumn(num++, out this.LOCALMAP_ICON);
		row.GetColumn(num++, out this.LOCALMAP_X);
		row.GetColumn(num++, out this.LOCALMAP_Y);
		row.GetColumn(num++, out this.LOCALMAP_BUNDLE_PATH);
		row.GetColumn(num++, out this.LOCALMAP_MOBILE_BUNDLE_PATH);
		row.GetColumn(num++, out this.MAP_INDEX[0]);
		row.GetColumn(num++, out this.MAP_INDEX[1]);
		row.GetColumn(num++, out this.MAP_INDEX[2]);
		row.GetColumn(num++, out this.MAP_INDEX[3]);
		row.GetColumn(num++, out this.MAP_INDEX[4]);
		row.GetColumn(num++, out this.MAP_INDEX[5]);
		row.GetColumn(num++, out this.MAP_INDEX[6]);
		row.GetColumn(num++, out this.MAP_INDEX[7]);
		row.GetColumn(num++, out this.MAP_INDEX[8]);
		row.GetColumn(num++, out this.MAP_INDEX[9]);
		row.GetColumn(num++, out this.MAP_INDEX[10]);
		row.GetColumn(num++, out this.MAP_INDEX[11]);
		row.GetColumn(num++, out this.MAP_INDEX[12]);
		row.GetColumn(num++, out this.MAP_INDEX[13]);
		row.GetColumn(num++, out this.MAP_INDEX[14]);
		row.GetColumn(num++, out this.MAP_INDEX[15]);
		row.GetColumn(num++, out this.MAP_INDEX[16]);
		row.GetColumn(num++, out this.MAP_INDEX[17]);
		row.GetColumn(num++, out this.MAP_INDEX[18]);
		row.GetColumn(num++, out this.MAP_INDEX[19]);
	}

	public string GetBundlePath()
	{
		return string.Format("UI/WorldMap/{0}", this.LOCALBUNDLE_PATH).ToLower();
	}
}
