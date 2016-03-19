using System;
using TsLibs;

public class TableData_AdventureInfo : NrTableData
{
	public Adventure adventure;

	public TableData_AdventureInfo() : base(NrTableData.eResourceType.eRT_ADVENTURE)
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.adventure = new Adventure();
		int num = 0;
		row.GetColumn(num++, out this.adventure.m_nAdventureUnique);
		row.GetColumn(num++, out this.adventure.m_szAdventureName);
		row.GetColumn(num++, out this.adventure.m_szBackImage);
		for (int i = 0; i < 10; i++)
		{
			int num2 = -1;
			bool column = row.GetColumn(num++, out num2);
			if (!column || num2 == 0)
			{
				break;
			}
			Adventure.AdventureInfo adventureInfo = new Adventure.AdventureInfo();
			adventureInfo.questGroupUnique = num2;
			row.GetColumn(num++, out adventureInfo.monsterCode);
			string empty = string.Empty;
			row.GetColumn(num++, out empty);
			char[] separator = new char[]
			{
				'_'
			};
			string[] array = empty.Split(separator);
			adventureInfo.mapIndex = int.Parse(array[0]);
			adventureInfo.destX = int.Parse(array[1]);
			adventureInfo.destY = int.Parse(array[2]);
			this.adventure.m_kAdventureInfo.Add(adventureInfo);
		}
	}
}
