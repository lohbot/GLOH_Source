using System;
using System.Collections.Generic;
using TsLibs;
using UnityEngine;

public class BASE_BATTLE_POS_Manager : NrTableBase
{
	private static BASE_BATTLE_POS_Manager Instance;

	private Dictionary<int, BATTLE_POS_GRID> m_dicBattlePos = new Dictionary<int, BATTLE_POS_GRID>();

	private BASE_BATTLE_POS_Manager(string strFilePath) : base(strFilePath)
	{
	}

	public static BASE_BATTLE_POS_Manager GetInstance()
	{
		if (BASE_BATTLE_POS_Manager.Instance == null)
		{
			BASE_BATTLE_POS_Manager.Instance = new BASE_BATTLE_POS_Manager(CDefinePath.BattlePosURL);
		}
		return BASE_BATTLE_POS_Manager.Instance;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BATTLE_POS bATTLE_POS = new BATTLE_POS();
			bATTLE_POS.SetData(data);
			this.Add(bATTLE_POS);
		}
		return true;
	}

	public override void Finish()
	{
	}

	public void Add(BATTLE_POS kData)
	{
		BATTLE_POS_GRID bATTLE_POS_GRID = this.GetInfo(kData.GRID_ID);
		if (bATTLE_POS_GRID == null)
		{
			bATTLE_POS_GRID = new BATTLE_POS_GRID();
			bATTLE_POS_GRID.Set(kData.GRID_ID, kData.POS_WIDTH, kData.POS_HEIGHT);
			this.m_dicBattlePos.Add(kData.GRID_ID, bATTLE_POS_GRID);
		}
		if (bATTLE_POS_GRID.mListPos == null)
		{
			Debug.LogError(string.Format("Add Fail:{0},{1}", kData.GRID_ID, kData.CELL));
		}
		bATTLE_POS_GRID.mListPos[kData.CELL] = new Vector3(kData.POS_X, 0f, kData.POS_Y);
	}

	public BATTLE_POS_GRID GetInfo(int _id)
	{
		if (this.m_dicBattlePos.ContainsKey(_id))
		{
			return this.m_dicBattlePos[_id];
		}
		return null;
	}

	public bool IsEnablePos(NrCharKindInfo pkCharKindInfo, short nGridPos, BATTLE_POS_GRID battle_Grid)
	{
		if (nGridPos < 0)
		{
			return false;
		}
		byte battleSizeX = pkCharKindInfo.GetBattleSizeX();
		byte battleSizeY = pkCharKindInfo.GetBattleSizeY();
		int num = (int)nGridPos % battle_Grid.m_nWidthCount;
		int num2 = (int)nGridPos / battle_Grid.m_nWidthCount;
		if (num + (int)battleSizeX > battle_Grid.m_nWidthCount)
		{
			return false;
		}
		if (num2 + (int)battleSizeY > battle_Grid.m_nHeightCount)
		{
			return false;
		}
		for (int i = 0; i < (int)battleSizeY; i++)
		{
			for (int j = 0; j < (int)battleSizeX; j++)
			{
				int num3 = num + j + (num2 + i) * battle_Grid.m_nWidthCount;
				if (num3 < 0 || num3 >= battle_Grid.m_nHeightCount * battle_Grid.m_nWidthCount)
				{
					return false;
				}
				if (battle_Grid.m_veBUID[num3] >= 0)
				{
					return false;
				}
				if (!battle_Grid.m_vebActive[num3])
				{
					return false;
				}
			}
		}
		return true;
	}
}
