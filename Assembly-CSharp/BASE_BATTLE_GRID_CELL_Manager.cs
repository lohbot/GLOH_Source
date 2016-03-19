using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_BATTLE_GRID_CELL_Manager : NrTableBase
{
	private static BASE_BATTLE_GRID_CELL_Manager Instance;

	private Dictionary<int, BATTLE_GRID_CELL[]> m_dicBattleGridCell = new Dictionary<int, BATTLE_GRID_CELL[]>();

	private List<List<BATTLE_GRID_CELL>> GridIndexerList = new List<List<BATTLE_GRID_CELL>>();

	private BASE_BATTLE_GRID_CELL_Manager(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_GRID_CELL bATTLE_GRID_CELL = new BATTLE_GRID_CELL();
			bATTLE_GRID_CELL.SetData(data);
			this.ADDCellList_List(bATTLE_GRID_CELL);
		}
		return true;
	}

	public override void Finish()
	{
		this.RegistList_InHashMap();
	}

	public static BASE_BATTLE_GRID_CELL_Manager GetInstance()
	{
		if (BASE_BATTLE_GRID_CELL_Manager.Instance == null)
		{
			BASE_BATTLE_GRID_CELL_Manager.Instance = new BASE_BATTLE_GRID_CELL_Manager(CDefinePath.GridCellInfoURL);
		}
		return BASE_BATTLE_GRID_CELL_Manager.Instance;
	}

	public BATTLE_GRID_CELL[] GetInfo(int _id)
	{
		return this.m_dicBattleGridCell[_id];
	}

	public int GetInfoLength()
	{
		return this.m_dicBattleGridCell.Count;
	}

	private void RegistList_InHashMap()
	{
		for (int i = 0; i < this.GridIndexerList.Count; i++)
		{
			List<BATTLE_GRID_CELL> cellId_List = this.GetCellId_List(i);
			if (cellId_List != null && !this.m_dicBattleGridCell.ContainsKey(i))
			{
				this.m_dicBattleGridCell.Add(i, cellId_List.ToArray());
			}
		}
	}

	private List<BATTLE_GRID_CELL> GetCellId_List(int _id)
	{
		if (0 < _id)
		{
			return this.GetRankList_List(_id);
		}
		return null;
	}

	private List<BATTLE_GRID_CELL> GetRankList_List(int _id)
	{
		if (this.GridIndexerList.Count > _id)
		{
			return this.GridIndexerList[_id];
		}
		List<BATTLE_GRID_CELL> item = new List<BATTLE_GRID_CELL>();
		this.GridIndexerList.Add(item);
		return this.GetRankList_List(_id);
	}

	private void ADDCellList_List(BATTLE_GRID_CELL Base)
	{
		if (Base != null)
		{
			List<BATTLE_GRID_CELL> cellId_List = this.GetCellId_List(Base.GRID_ID);
			if (cellId_List != null)
			{
				cellId_List.Add(Base);
			}
		}
	}
}
