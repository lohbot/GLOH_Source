using System;
using System.Collections.Generic;
using TsBundle;
using TsLibs;

public class MAP_INFO : NrTableData
{
	public int MAP_INDEX;

	public string TEXTKEY = string.Empty;

	public string WEB_TILE_INFO = string.Empty;

	public int MAX_USER;

	public long MAP_ATB;

	public float MAP_X;

	public float MAP_Y;

	public int SCENARIOINDEX;

	public string WEB_BUNDLE_PATH = string.Empty;

	public string MOBILE_TILE_INFO = string.Empty;

	public string MOBILE_BUNDLE_PATH = string.Empty;

	public string MAP_ICON = string.Empty;

	public int MAP_NIGHTMODE;

	public int PARENTS_MAP_IDX;

	public float WARP_POS_X;

	public float WARP_POS_Y;

	public float WARP_POS_Z;

	public string OST_NAME = string.Empty;

	public string strMapATB = string.Empty;

	private List<GATE_INFO> kGateInfoList = new List<GATE_INFO>();

	private List<GATE_INFO> kDSTGateInfoList = new List<GATE_INFO>();

	public string TILE_INFO
	{
		get
		{
			if (TsPlatform.IsMobile)
			{
				return this.MOBILE_TILE_INFO;
			}
			return this.WEB_TILE_INFO;
		}
	}

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

	public MAP_INFO() : base(NrTableData.eResourceType.eRT_MAP_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.MAP_INDEX = 0;
		this.TEXTKEY = string.Empty;
		this.WEB_TILE_INFO = string.Empty;
		this.MAX_USER = 0;
		this.MAP_ATB = 0L;
		this.WEB_BUNDLE_PATH = string.Empty;
		this.MOBILE_TILE_INFO = string.Empty;
		this.MOBILE_BUNDLE_PATH = string.Empty;
		this.OST_NAME = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.MAP_INDEX);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.WEB_TILE_INFO);
		row.GetColumn(num++, out this.MAX_USER);
		row.GetColumn(num++, out this.strMapATB);
		row.GetColumn(num++, out this.MAP_X);
		row.GetColumn(num++, out this.MAP_Y);
		row.GetColumn(num++, out this.SCENARIOINDEX);
		row.GetColumn(num++, out this.WEB_BUNDLE_PATH);
		row.GetColumn(num++, out this.MOBILE_TILE_INFO);
		row.GetColumn(num++, out this.MOBILE_BUNDLE_PATH);
		row.GetColumn(num++, out this.MAP_ICON);
		row.GetColumn(num++, out this.MAP_NIGHTMODE);
		row.GetColumn(num++, out this.PARENTS_MAP_IDX);
		row.GetColumn(num++, out this.WARP_POS_X);
		row.GetColumn(num++, out this.WARP_POS_Y);
		row.GetColumn(num++, out this.WARP_POS_Z);
		row.GetColumn(num++, out this.OST_NAME);
	}

	public bool IsMapATB(long flag)
	{
		return (this.MAP_ATB & flag) != 0L;
	}

	public string GetBundlePath()
	{
		if (this.IsMapATB(2L))
		{
			return string.Format("Map/Territory/{0}{1}", this.BUNDLE_PATH, Option.extAsset).ToLower();
		}
		return string.Format("Map/World/{0}{1}", this.BUNDLE_PATH, Option.extAsset).ToLower();
	}

	public GATE_INFO[] GetGateInfo()
	{
		return this.kGateInfoList.ToArray();
	}

	public GATE_INFO[] GetDSTGateInfo()
	{
		return this.kDSTGateInfoList.ToArray();
	}

	public void AddGateInfo(GATE_INFO pkGateInfo)
	{
		this.kGateInfoList.Add(pkGateInfo);
	}

	public void AddDSTGateInfo(GATE_INFO pkGateInfo)
	{
		this.kDSTGateInfoList.Add(pkGateInfo);
	}

	public int GetGateInfoLenght()
	{
		return this.kGateInfoList.Count;
	}

	public int GetDSTGateInfoLenght()
	{
		return this.kDSTGateInfoList.Count;
	}
}
