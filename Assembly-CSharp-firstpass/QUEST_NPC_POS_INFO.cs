using System;
using TsLibs;

public class QUEST_NPC_POS_INFO : NrTableData
{
	public string strUnique = string.Empty;

	public string[] CHAR_CODE = new string[5];

	public QUEST_NPC_POS_INFO()
	{
		base.SetTypeIndex(NrTableData.eResourceType.eRT_QUEST_NPC_POS_INFO);
		this.Init();
	}

	public void Init()
	{
		this.strUnique = string.Empty;
		for (int i = 0; i < 5; i++)
		{
			this.CHAR_CODE[i] = string.Empty;
		}
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
	}
}
