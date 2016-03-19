using System;
using TsLibs;

public class TableData_GameGuideInfo : NrTableData
{
	public GameGuideInfo gameGuideInfo;

	public TableData_GameGuideInfo() : base(NrTableData.eResourceType.eRT_GAMEGUIDE)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.gameGuideInfo = new GameGuideInfo();
		int num = 0;
		row.GetColumn(num++, out this.gameGuideInfo.m_nUnique);
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		if (Enum.IsDefined(typeof(GameGuideType), empty))
		{
			this.gameGuideInfo.m_eType = (GameGuideType)((int)Enum.Parse(typeof(GameGuideType), empty));
		}
		empty = string.Empty;
		row.GetColumn(num++, out empty);
		this.gameGuideInfo.m_eCheck = (GameGuideCheck)((int)Enum.Parse(typeof(GameGuideCheck), empty));
		row.GetColumn(num++, out this.gameGuideInfo.m_nPriority);
		row.GetColumn(num++, out this.gameGuideInfo.m_nMinLevel);
		row.GetColumn(num++, out this.gameGuideInfo.m_nMaxLevel);
		row.GetColumn(num++, out this.gameGuideInfo.m_strBaloonTextKey);
		row.GetColumn(num++, out this.gameGuideInfo.m_strTalkKey);
		row.GetColumn(num++, out this.gameGuideInfo.m_nDelayTime);
		row.GetColumn(num++, out this.gameGuideInfo.m_strButtonKey);
	}
}
