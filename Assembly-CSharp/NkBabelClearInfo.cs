using GAME;
using System;
using System.Collections.Generic;

public class NkBabelClearInfo
{
	private List<BABEL_CLEARINFO> BabelClearList = new List<BABEL_CLEARINFO>();

	private List<BABEL_SUBFLOOR_RANKINFO> BabelSubFloor_RankInfo = new List<BABEL_SUBFLOOR_RANKINFO>();

	public void Init()
	{
		this.BabelClearList.Clear();
		this.BabelSubFloor_RankInfo.Clear();
	}

	public void AddBabelClearInfo(BABEL_CLEARINFO solInfo)
	{
		this.BabelClearList.Add(solInfo);
	}

	public void AddBabelSubFloorRankInfo(BABEL_SUBFLOOR_RANKINFO solInfo)
	{
		this.BabelSubFloor_RankInfo.Add(solInfo);
	}

	public void SetBabelSubFloorRankInfo(short floor, byte subfloor, byte rank, bool treasure, short floortype)
	{
		bool flag = true;
		foreach (BABEL_SUBFLOOR_RANKINFO current in this.BabelSubFloor_RankInfo)
		{
			if (current.i16Floor == floor && current.bySubFloor == subfloor && current.i16FloorType == floortype)
			{
				current.byRank = rank;
				current.bTreasure = treasure;
				flag = false;
				break;
			}
		}
		if (flag)
		{
			this.AddBabelSubFloorRankInfo(new BABEL_SUBFLOOR_RANKINFO
			{
				i16Floor = floor,
				bySubFloor = subfloor,
				byRank = rank,
				bTreasure = treasure,
				i16FloorType = floortype
			});
		}
	}

	public byte GetBabelFloorRankInfo(short floor, short floortype)
	{
		byte b = 0;
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerFloorInfo(floor, floortype);
		if (babelTowerFloorInfo == null)
		{
			return b;
		}
		short num = 0;
		short num2 = babelTowerFloorInfo.SubFloorCount_Type1;
		if (floortype == 2)
		{
			num2 = babelTowerFloorInfo.SubFloorCount_Type2;
		}
		for (short num3 = 0; num3 < num2; num3 += 1)
		{
			foreach (BABEL_SUBFLOOR_RANKINFO current in this.BabelSubFloor_RankInfo)
			{
				if (current.i16Floor == floor && (short)current.bySubFloor == num3 && current.i16FloorType == floortype)
				{
					if (b == 0 || b > current.byRank)
					{
						b = current.byRank;
					}
					num += 1;
				}
			}
		}
		if (floortype == 2)
		{
			if (num < babelTowerFloorInfo.SubFloorCount_Type2)
			{
				return 0;
			}
		}
		else if (num < babelTowerFloorInfo.SubFloorCount_Type1)
		{
			return 0;
		}
		return b;
	}

	public byte GetBabelSubFloorRankInfo(short floor, byte subfoor, short floortype)
	{
		foreach (BABEL_SUBFLOOR_RANKINFO current in this.BabelSubFloor_RankInfo)
		{
			if (current.i16Floor == floor && current.bySubFloor == subfoor && current.i16FloorType == floortype)
			{
				return current.byRank;
			}
		}
		return 0;
	}

	private BABEL_CLEARINFO GetBabelClearInfo(byte _column_num, short floortype)
	{
		foreach (BABEL_CLEARINFO current in this.BabelClearList)
		{
			if (_column_num == current.ColumnNum && floortype == current.FloorType)
			{
				return current;
			}
		}
		TsLog.LogError("BABEL_CLEARINFO == NULL ColumnNum={0}   FloorType ={1}", new object[]
		{
			_column_num,
			floortype
		});
		foreach (BABEL_CLEARINFO current2 in this.BabelClearList)
		{
			TsLog.LogError("BABEL_CLEARINFO Data  ColumnNum={0}   FloorType ={1} ClearData = {2} ", new object[]
			{
				current2.ColumnNum,
				current2.FloorType,
				current2.ClearInfo
			});
		}
		return null;
	}

	public bool IsBabelClear(short _floor, short floortype)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerFloorInfo(_floor, floortype);
		for (short num = 0; num < babelTowerFloorInfo.SubFloorCount_Type1; num += 1)
		{
			if (!this.IsBabelClear(_floor, num, floortype))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsBabelClear(short _floor, short _subfloor, short floortype)
	{
		byte babelColumnNumFromFloor = NrTSingleton<BabelTowerManager>.Instance.GetBabelColumnNumFromFloor(_floor, _subfloor);
		BABEL_CLEARINFO babelClearInfo = this.GetBabelClearInfo(babelColumnNumFromFloor, floortype);
		return NrTSingleton<BabelTowerManager>.Instance.IsClearBabel(_floor, _subfloor, babelClearInfo);
	}

	public bool IsBabelTreasure(short _floor, short floortype)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerFloorInfo(_floor, floortype);
		for (short num = 0; num < babelTowerFloorInfo.SubFloorCount_Type1; num += 1)
		{
			if (!this.IsBabelTreasure(_floor, num, floortype))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsBabelTreasure(short _floor, short _subfloor, short floortype)
	{
		foreach (BABEL_SUBFLOOR_RANKINFO current in this.BabelSubFloor_RankInfo)
		{
			if (current.i16Floor == _floor && (short)current.bySubFloor == _subfloor && current.i16FloorType == floortype)
			{
				return current.bTreasure;
			}
		}
		return false;
	}

	public void SetBabelClearInfo(byte column, long clearinfo, short floortype)
	{
		foreach (BABEL_CLEARINFO current in this.BabelClearList)
		{
			if (current.ColumnNum == column && current.FloorType == floortype)
			{
				current.ClearInfo = clearinfo;
				break;
			}
		}
	}
}
