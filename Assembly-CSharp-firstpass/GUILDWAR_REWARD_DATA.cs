using System;
using TsLibs;

public class GUILDWAR_REWARD_DATA : NrTableData
{
	public int Index;

	public int Min_Guild_Rank;

	public int Max_Guild_Rank;

	public int Win_Reward_Gold;

	public int Win_Reward_ItemUnique;

	public int Win_Reward_ItemCount;

	public int Win_Guild_Point;

	public int Lose_Reward_Gold;

	public int Lose_Reward_ItemUnique;

	public int Lose_Reward_ItemCount;

	public int Lose_Guild_Point;

	public GUILDWAR_REWARD_DATA() : base(NrTableData.eResourceType.eRT_GUILDWAR_REWARD)
	{
	}

	private void Init()
	{
		this.Index = 0;
		this.Min_Guild_Rank = 0;
		this.Max_Guild_Rank = 0;
		this.Win_Reward_Gold = 0;
		this.Win_Reward_ItemUnique = 0;
		this.Win_Reward_ItemCount = 0;
		this.Win_Guild_Point = 0;
		this.Lose_Reward_Gold = 0;
		this.Lose_Reward_ItemUnique = 0;
		this.Lose_Reward_ItemCount = 0;
		this.Lose_Guild_Point = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		row.GetColumn(0, out this.Index);
		row.GetColumn(1, out this.Min_Guild_Rank);
		row.GetColumn(2, out this.Max_Guild_Rank);
		row.GetColumn(3, out this.Win_Reward_Gold);
		row.GetColumn(4, out this.Win_Reward_ItemUnique);
		row.GetColumn(5, out this.Win_Reward_ItemCount);
		row.GetColumn(6, out this.Win_Guild_Point);
		row.GetColumn(7, out this.Lose_Reward_Gold);
		row.GetColumn(8, out this.Lose_Reward_ItemUnique);
		row.GetColumn(9, out this.Lose_Reward_ItemCount);
		row.GetColumn(10, out this.Lose_Guild_Point);
	}
}
