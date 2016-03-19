using System;
using TsBundle;
using TsLibs;

public class GATE_INFO : NrTableData
{
	public int GATE_IDX;

	public int SRC_MAP_IDX;

	public float SRC_POSX;

	public float SRC_POSY;

	public float SRC_POSZ;

	public float SRC_ANGLE;

	public int DST_MAP_IDX;

	public float DST_POSX;

	public float DST_POSY;

	public float DST_POSZ;

	public float DST_ANGLE;

	public int INDUN_IDX = -1;

	public string QUEST_KEY = string.Empty;

	public string QUEST_NEED = string.Empty;

	public string TEXTKEY = string.Empty;

	public float GATE_SCALE;

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
		set
		{
			if (TsPlatform.IsMobile)
			{
				this.MOBILE_BUNDLE_PATH = value;
			}
			else
			{
				this.WEB_BUNDLE_PATH = value;
			}
		}
	}

	public GATE_INFO() : base(NrTableData.eResourceType.eRT_GATE_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.GATE_IDX = 0;
		this.SRC_MAP_IDX = 0;
		this.SRC_POSX = 0f;
		this.SRC_POSY = 0f;
		this.SRC_POSZ = 0f;
		this.DST_MAP_IDX = 0;
		this.DST_POSX = 0f;
		this.DST_POSY = 0f;
		this.DST_POSZ = 0f;
		this.DST_ANGLE = 0f;
		this.INDUN_IDX = -1;
		this.QUEST_KEY = string.Empty;
		this.QUEST_NEED = string.Empty;
		this.TEXTKEY = string.Empty;
		this.GATE_SCALE = 0f;
		this.WEB_BUNDLE_PATH = string.Empty;
		this.MOBILE_BUNDLE_PATH = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.GATE_IDX);
		row.GetColumn(num++, out this.SRC_MAP_IDX);
		row.GetColumn(num++, out this.SRC_POSX);
		row.GetColumn(num++, out this.SRC_POSY);
		row.GetColumn(num++, out this.SRC_POSZ);
		row.GetColumn(num++, out this.SRC_ANGLE);
		row.GetColumn(num++, out this.DST_MAP_IDX);
		row.GetColumn(num++, out this.DST_POSX);
		row.GetColumn(num++, out this.DST_POSY);
		row.GetColumn(num++, out this.DST_POSZ);
		row.GetColumn(num++, out this.DST_ANGLE);
		row.GetColumn(num++, out this.INDUN_IDX);
		row.GetColumn(num++, out this.QUEST_KEY);
		row.GetColumn(num++, out this.QUEST_NEED);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.GATE_SCALE);
		row.GetColumn(num++, out this.WEB_BUNDLE_PATH);
		row.GetColumn(num++, out this.MOBILE_BUNDLE_PATH);
	}

	public string GetBundlePath()
	{
		return string.Format("Map/Gate/{0}{1}", this.BUNDLE_PATH, Option.extAsset);
	}
}
