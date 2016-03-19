using System;
using TsLibs;

public class CHARKIND_INFO : NrTableData
{
	public int CHARKIND;

	public string CHARCODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string CharTribe = string.Empty;

	public byte GENDER;

	public byte SLOPEMODE;

	public byte BattleSizeX;

	public byte BattleSizeY;

	public string PortraitFile1 = string.Empty;

	public short PortraitRank;

	public string PortraitFile2 = string.Empty;

	public string TEXTKEY_DESC = string.Empty;

	public byte SCALE;

	public string WEB_BUNDLE_PATH = string.Empty;

	public string MOBILE_BUNDLE_PATH = string.Empty;

	public string SOLINTRO = string.Empty;

	public string SoldierSpec1 = string.Empty;

	public string SoldierSpec2 = string.Empty;

	public string CharEffectGrade = string.Empty;

	public long nClassType;

	public int nAttackType;

	public long nATB;

	public float fBound;

	public float fHalfBound;

	public string CLASSTYPE = string.Empty;

	public string ATTACKTYPE = string.Empty;

	public string ATB = string.Empty;

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

	public CHARKIND_INFO() : base(NrTableData.eResourceType.eRT_CHARKIND_INFO)
	{
		this.Init();
	}

	public void Init()
	{
		this.CHARKIND = 0;
		this.CHARCODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.CharTribe = string.Empty;
		this.GENDER = 0;
		this.SLOPEMODE = 0;
		this.BattleSizeX = 1;
		this.BattleSizeY = 1;
		this.PortraitFile1 = string.Empty;
		this.PortraitFile2 = string.Empty;
		this.TEXTKEY_DESC = string.Empty;
		this.SCALE = 0;
		this.WEB_BUNDLE_PATH = string.Empty;
		this.MOBILE_BUNDLE_PATH = string.Empty;
		this.SOLINTRO = string.Empty;
		this.SoldierSpec1 = string.Empty;
		this.SoldierSpec2 = string.Empty;
		this.CharEffectGrade = string.Empty;
		this.nClassType = 0L;
		this.nAttackType = 0;
		this.nATB = 0L;
		this.fBound = 0f;
		this.fHalfBound = 0f;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		byte b4 = 0;
		short num2 = 0;
		row.GetColumn(num++, out this.CHARKIND);
		row.GetColumn(num++, out this.CHARCODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.CharTribe);
		row.GetColumn(num++, out this.GENDER);
		row.GetColumn(num++, out b);
		row.GetColumn(num++, out this.CLASSTYPE);
		row.GetColumn(num++, out this.ATTACKTYPE);
		row.GetColumn(num++, out this.ATB);
		row.GetColumn(num++, out this.SLOPEMODE);
		row.GetColumn(num++, out b2);
		row.GetColumn(num++, out b3);
		row.GetColumn(num++, out b4);
		row.GetColumn(num++, out this.BattleSizeX);
		row.GetColumn(num++, out this.BattleSizeY);
		row.GetColumn(num++, out this.PortraitFile1);
		row.GetColumn(num++, out this.PortraitRank);
		row.GetColumn(num++, out this.PortraitFile2);
		row.GetColumn(num++, out num2);
		row.GetColumn(num++, out this.TEXTKEY_DESC);
		row.GetColumn(num++, out this.SCALE);
		row.GetColumn(num++, out this.WEB_BUNDLE_PATH);
		row.GetColumn(num++, out this.MOBILE_BUNDLE_PATH);
		row.GetColumn(num++, out this.SOLINTRO);
		row.GetColumn(num++, out this.SoldierSpec1);
		row.GetColumn(num++, out this.SoldierSpec2);
		row.GetColumn(num++, out this.CharEffectGrade);
		this.fBound = (float)b4 / 10f;
		this.fHalfBound = this.fBound / 2f;
	}
}
