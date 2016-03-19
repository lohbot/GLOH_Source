using System;
using TsLibs;
using UnityEngine;

public class EFFECT_INFO : NrTableData
{
	public string EFFECT_KIND = string.Empty;

	public eEFFECT_TYPE EFFECT_TYPE;

	public eEFFECT_POS EFFECT_POS;

	public float LIFE_TIME;

	public Vector3 DIFFPOS = Vector3.zero;

	public float SCALE = 1f;

	public string WEB_BUNDLE_PATH = string.Empty;

	public string MOBILE_BUNDLE_PATH = string.Empty;

	public string strEffectType = string.Empty;

	public string strEffectPos = string.Empty;

	public byte LIMITCANCEL;

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

	public EFFECT_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.EFFECT_KIND = string.Empty;
		this.EFFECT_TYPE = eEFFECT_TYPE.INSTANT;
		this.LIFE_TIME = 0f;
		this.DIFFPOS = Vector3.zero;
		this.SCALE = 1f;
		this.WEB_BUNDLE_PATH = string.Empty;
		this.MOBILE_BUNDLE_PATH = string.Empty;
		this.LIMITCANCEL = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		ushort num2 = 0;
		row.GetColumn(num++, out this.EFFECT_KIND);
		row.GetColumn(num++, out this.strEffectType);
		row.GetColumn(num++, out this.strEffectPos);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out this.DIFFPOS.x);
		row.GetColumn(num++, out this.DIFFPOS.y);
		row.GetColumn(num++, out this.DIFFPOS.z);
		row.GetColumn(num++, out this.SCALE);
		row.GetColumn(num++, out this.WEB_BUNDLE_PATH);
		row.GetColumn(num++, out this.MOBILE_BUNDLE_PATH);
		row.GetColumn(num++, out this.LIMITCANCEL);
		if (0 > num2)
		{
			this.LIFE_TIME = 3.40282347E+38f;
		}
		else
		{
			this.LIFE_TIME = (float)num2 / 1000f;
		}
	}
}
