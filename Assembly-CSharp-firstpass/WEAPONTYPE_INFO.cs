using System;
using TsLibs;

public class WEAPONTYPE_INFO : NrTableData
{
	public string WEAPONCODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ATB = string.Empty;

	public byte EQUIPCOUNT;

	public byte SCALE;

	public string BATTLEDUMMY = string.Empty;

	public string BACKDUMMY = string.Empty;

	public WEAPONTYPE_INFO()
	{
		base.SetTypeIndex(NrTableData.eResourceType.eRT_WEAPONTYPE_INFO);
		this.Init();
	}

	public void Init()
	{
		this.WEAPONCODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.ATB = string.Empty;
		this.EQUIPCOUNT = 0;
		this.SCALE = 0;
		this.BATTLEDUMMY = string.Empty;
		this.BACKDUMMY = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.WEAPONCODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.ATB);
		row.GetColumn(num++, out this.EQUIPCOUNT);
		row.GetColumn(num++, out this.SCALE);
		row.GetColumn(num++, out this.BATTLEDUMMY);
		row.GetColumn(num++, out this.BACKDUMMY);
	}
}
