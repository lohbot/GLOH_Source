using System;
using TsLibs;

public class BASE_BATTLE_MAP_CELLINFO : NrTableBase
{
	public BATTLE_MAP_CELL_INFO m_CellInfo;

	public BASE_BATTLE_MAP_CELLINFO(string strFilePath) : base(strFilePath, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			this.m_CellInfo = new BATTLE_MAP_CELL_INFO();
			this.m_CellInfo.SetData(data);
		}
		return true;
	}
}
