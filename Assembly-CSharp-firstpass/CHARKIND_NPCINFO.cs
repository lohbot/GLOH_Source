using System;
using TsLibs;

public class CHARKIND_NPCINFO : NrTableData
{
	public string CHARCODE = string.Empty;

	public string TEXTKEY_GREETING = string.Empty;

	public string SOUND_GREETING = string.Empty;

	public string TEXTKEY_BYE = string.Empty;

	public CHARKIND_NPCINFO()
	{
		base.SetTypeIndex(NrTableData.eResourceType.eRT_CHARKIND_NPCINFO);
		this.Init();
	}

	public void Init()
	{
		this.CHARCODE = string.Empty;
		this.TEXTKEY_GREETING = string.Empty;
		this.SOUND_GREETING = string.Empty;
		this.TEXTKEY_BYE = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.CHARCODE);
		row.GetColumn(num++, out this.TEXTKEY_GREETING);
		row.GetColumn(num++, out this.SOUND_GREETING);
		row.GetColumn(num++, out this.TEXTKEY_BYE);
		row.GetColumn(num++, out empty);
	}

	public string GetTextGreeting()
	{
		return this.TEXTKEY_GREETING;
	}

	public string GetTextBye()
	{
		return this.TEXTKEY_BYE;
	}
}
