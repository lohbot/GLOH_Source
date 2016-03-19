using GAME;
using System;
using System.Collections.Generic;
using TsLibs;
using UnityEngine;

public class BASE_BATTLE_GridData_Manager : NrTableBase
{
	private static BASE_BATTLE_GridData_Manager Instance;

	private Dictionary<int, NrGridData> m_dicBattleGrid = new Dictionary<int, NrGridData>();

	private BASE_BATTLE_GridData_Manager(string strFilePath) : base(strFilePath)
	{
	}

	public static BASE_BATTLE_GridData_Manager GetInstance()
	{
		if (BASE_BATTLE_GridData_Manager.Instance == null)
		{
			BASE_BATTLE_GridData_Manager.Instance = new BASE_BATTLE_GridData_Manager(CDefinePath.BattleGridDataURL);
		}
		return BASE_BATTLE_GridData_Manager.Instance;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_GRID_DATA bATTLE_GRID_DATA = new BATTLE_GRID_DATA();
			bATTLE_GRID_DATA.SetData(data);
			this.Add(bATTLE_GRID_DATA);
		}
		return true;
	}

	public override void Finish()
	{
	}

	public void Add(BATTLE_GRID_DATA kData)
	{
		NrGridData nrGridData = this.GetGrid(kData.GRID_ID);
		if (nrGridData == null)
		{
			nrGridData = new NrGridData();
			this.m_dicBattleGrid.Add(kData.GRID_ID, nrGridData);
		}
		nrGridData.InsertData(new Vector2((float)kData.POS_X, (float)kData.POS_Y));
	}

	public NrGridData GetGrid(int _id)
	{
		if (this.m_dicBattleGrid.ContainsKey(_id))
		{
			return this.m_dicBattleGrid[_id];
		}
		return null;
	}

	public int[] GetIndex(E_ATTACK_GRID_TYPE type, int Start, int xMax, int yMax)
	{
		Vector2 index = NrGridData.Convert(Start, xMax);
		NrGridData grid = this.GetGrid((int)type);
		if (grid != null)
		{
			return grid.GetIndex(index, xMax, yMax);
		}
		return null;
	}
}
