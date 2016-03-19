using System;
using TsLibs;

public class WORLDMAP_INFO : NrTableData
{
	public int WORLDMAP_IDX;

	public string WORLDMAP_NAME = string.Empty;

	public string WORLDMAP_BUNDLE_PATH = string.Empty;

	public string WORLDMAP_MOBILE_BUNDLE_PATH = string.Empty;

	public string TEXTKEY = string.Empty;

	public string WORLDBUNDLE_PATH
	{
		get
		{
			return this.WORLDMAP_BUNDLE_PATH;
		}
	}

	public WORLDMAP_INFO() : base(NrTableData.eResourceType.eRT_WORLDMAP_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.WORLDMAP_IDX = 0;
		this.WORLDMAP_NAME = string.Empty;
		this.WORLDMAP_BUNDLE_PATH = string.Empty;
		this.WORLDMAP_MOBILE_BUNDLE_PATH = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.WORLDMAP_IDX);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.WORLDMAP_BUNDLE_PATH);
		row.GetColumn(num++, out this.WORLDMAP_MOBILE_BUNDLE_PATH);
	}

	public string GetBundlePath()
	{
		return string.Format("UI/WorldMap/{0}", this.WORLDBUNDLE_PATH).ToLower();
	}
}
