using System;
using TsLibs;

public class BULLET_INFO : NrTableData
{
	public string NAME = string.Empty;

	public string EFFECT_KIND = string.Empty;

	public eBULLET_MOVE_TYPE MOVE_TYPE;

	public eBULLET_HIT_TYPE HIT_TYPE;

	public float HIT_RANGE;

	public float SPEED;

	public string END_EFFECT_KIND = string.Empty;

	public string szMOVE_TYPE = string.Empty;

	public string szHIT_TYPE = string.Empty;

	public BULLET_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.NAME = string.Empty;
		this.EFFECT_KIND = string.Empty;
		this.MOVE_TYPE = eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_NONE;
		this.HIT_TYPE = eBULLET_HIT_TYPE.eBULLET_HIT_TYPE_NONE;
		this.HIT_RANGE = 0f;
		this.SPEED = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.NAME);
		row.GetColumn(num++, out this.EFFECT_KIND);
		row.GetColumn(num++, out this.szMOVE_TYPE);
		row.GetColumn(num++, out this.szHIT_TYPE);
		row.GetColumn(num++, out this.HIT_RANGE);
		row.GetColumn(num++, out this.SPEED);
		row.GetColumn(num++, out this.END_EFFECT_KIND);
	}
}
