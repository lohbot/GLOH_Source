using GAME;
using System;
using System.Collections.Generic;

public class BabelTowerManager : NrTSingleton<BabelTowerManager>
{
	public class BabelTower_Floor
	{
		public List<BABELTOWER_DATA> m_listBabelTowerData_Type1;

		public List<BABELTOWER_DATA> m_listBabelTowerData_Type2;

		public short m_SubFloor_Type1;

		public short m_SubFloor_Type2;

		public byte m_Column;

		public long m_ClearInfo;

		public long m_FloorType;

		public short LastSubFloor_Type1
		{
			get
			{
				return this.m_SubFloor_Type1;
			}
			set
			{
				this.m_SubFloor_Type1 = value;
			}
		}

		public short LastSubFloor_Type2
		{
			get
			{
				return this.m_SubFloor_Type2;
			}
			set
			{
				this.m_SubFloor_Type2 = value;
			}
		}

		public short SubFloorCount_Type1
		{
			get
			{
				return this.m_SubFloor_Type1 + 1;
			}
		}

		public short SubFloorCount_Type2
		{
			get
			{
				return this.m_SubFloor_Type2 + 1;
			}
		}

		public void AddBabelDataInfo(BABELTOWER_DATA data)
		{
			if (data.m_nFloorType == 2)
			{
				if (data.m_nSubFloor > this.m_SubFloor_Type2)
				{
					this.m_SubFloor_Type2 = data.m_nSubFloor;
				}
			}
			else if (data.m_nSubFloor > this.m_SubFloor_Type1)
			{
				this.m_SubFloor_Type1 = data.m_nSubFloor;
			}
			List<BABELTOWER_DATA> list;
			if (data.m_nFloorType == 2)
			{
				if (this.m_listBabelTowerData_Type2 == null)
				{
					this.m_listBabelTowerData_Type2 = new List<BABELTOWER_DATA>();
				}
				list = this.m_listBabelTowerData_Type2;
			}
			else
			{
				if (this.m_listBabelTowerData_Type1 == null)
				{
					this.m_listBabelTowerData_Type1 = new List<BABELTOWER_DATA>();
				}
				list = this.m_listBabelTowerData_Type1;
			}
			this.m_Column = NrTSingleton<BabelTowerManager>.Instance.GetBabelColumnNumFromFloor(data.m_nFloor, data.m_nSubFloor);
			int num = (int)(((data.m_nFloor - 1) * 5 + data.m_nSubFloor) % 63);
			long num2 = 1L << num;
			this.m_ClearInfo += num2;
			if (list == null)
			{
				TsLog.LogError("!!!!!!!!!!!!!!!!!listBabelTowerData == null  FloorType = {0}", new object[]
				{
					data.m_nFloorType
				});
				return;
			}
			if (data == null)
			{
				TsLog.LogError("!!!!!!!!!!!!!!!!!data == null  FloorType = {0}", new object[]
				{
					data.m_nFloorType
				});
				return;
			}
			list.Add(data);
		}
	}

	private Dictionary<short, BabelTowerManager.BabelTower_Floor> m_DicBabelTowerFloorInfo_Type1 = new Dictionary<short, BabelTowerManager.BabelTower_Floor>();

	private Dictionary<short, BabelTowerManager.BabelTower_Floor> m_DicBabelTowerFloorInfo_Type2 = new Dictionary<short, BabelTowerManager.BabelTower_Floor>();

	private short m_LastFloor_Type1;

	private short m_LastFloor_Type2;

	private Dictionary<short, BABEL_GUILDBOSS> m_DicBabel_GuildBossInfo = new Dictionary<short, BABEL_GUILDBOSS>();

	private short m_LastGuildBossFloor;

	private BabelTowerManager()
	{
		this.m_LastFloor_Type1 = 0;
		this.m_LastFloor_Type2 = 0;
		this.m_LastGuildBossFloor = 0;
	}

	public BabelTowerManager.BabelTower_Floor GetBabelTowerFloorInfo(short floor, short FloorType)
	{
		BabelTowerManager.BabelTower_Floor babelTower_Floor;
		if (FloorType == 2)
		{
			if (!this.m_DicBabelTowerFloorInfo_Type2.ContainsKey(floor))
			{
				babelTower_Floor = new BabelTowerManager.BabelTower_Floor();
				this.m_DicBabelTowerFloorInfo_Type2.Add(floor, babelTower_Floor);
			}
			else
			{
				babelTower_Floor = this.m_DicBabelTowerFloorInfo_Type2[floor];
			}
		}
		else if (!this.m_DicBabelTowerFloorInfo_Type1.ContainsKey(floor))
		{
			babelTower_Floor = new BabelTowerManager.BabelTower_Floor();
			this.m_DicBabelTowerFloorInfo_Type1.Add(floor, babelTower_Floor);
		}
		else
		{
			babelTower_Floor = this.m_DicBabelTowerFloorInfo_Type1[floor];
		}
		return babelTower_Floor;
	}

	public short GetBabelTowerLastSubFloor(short floor, short floortype)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = this.GetBabelTowerFloorInfo(floor, floortype);
		if (babelTowerFloorInfo == null)
		{
			return 0;
		}
		if (floortype == 2)
		{
			return babelTowerFloorInfo.LastSubFloor_Type2;
		}
		return babelTowerFloorInfo.LastSubFloor_Type1;
	}

	public void AddBabelTowerData(BABELTOWER_DATA _data)
	{
		if (_data.m_nFloorType == 2 && this.m_LastFloor_Type2 < _data.m_nFloor)
		{
			this.m_LastFloor_Type2 = _data.m_nFloor;
		}
		else if (this.m_LastFloor_Type1 < _data.m_nFloor)
		{
			this.m_LastFloor_Type1 = _data.m_nFloor;
		}
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = this.GetBabelTowerFloorInfo(_data.m_nFloor, _data.m_nFloorType);
		babelTowerFloorInfo.AddBabelDataInfo(_data);
	}

	public BABELTOWER_DATA GetBabelTowerData(short _floor, short _subfloor, short _floortype = 1)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = this.GetBabelTowerFloorInfo(_floor, _floortype);
		if (babelTowerFloorInfo == null)
		{
			return null;
		}
		if (_floortype == 2)
		{
			foreach (BABELTOWER_DATA current in babelTowerFloorInfo.m_listBabelTowerData_Type2)
			{
				if (current.m_nFloor == _floor && current.m_nSubFloor == _subfloor && current.m_nFloorType == _floortype)
				{
					BABELTOWER_DATA result = current;
					return result;
				}
			}
		}
		else
		{
			foreach (BABELTOWER_DATA current2 in babelTowerFloorInfo.m_listBabelTowerData_Type1)
			{
				if (current2.m_nFloor == _floor && current2.m_nSubFloor == _subfloor && current2.m_nFloorType == _floortype)
				{
					BABELTOWER_DATA result = current2;
					return result;
				}
			}
		}
		return null;
	}

	public short GetLastFloor(short nFloorType)
	{
		if (nFloorType == 2)
		{
			return this.m_LastFloor_Type2;
		}
		return this.m_LastFloor_Type1;
	}

	public short GetMaxSubFloor(short _Floor, short _FloorType)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = this.GetBabelTowerFloorInfo(_Floor, _FloorType);
		if (babelTowerFloorInfo == null)
		{
			return 0;
		}
		short num = 0;
		if (_FloorType == 2)
		{
			foreach (BABELTOWER_DATA current in babelTowerFloorInfo.m_listBabelTowerData_Type2)
			{
				if (current.m_nFloor == _Floor && current.m_nFloorType == _FloorType && num < current.m_nSubFloor)
				{
					num = current.m_nSubFloor;
				}
			}
		}
		else
		{
			foreach (BABELTOWER_DATA current2 in babelTowerFloorInfo.m_listBabelTowerData_Type1)
			{
				if (current2.m_nFloor == _Floor && current2.m_nFloorType == _FloorType && num < current2.m_nSubFloor)
				{
					num = current2.m_nSubFloor;
				}
			}
		}
		return num;
	}

	public bool IsBabelStart()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		int level = kMyCharInfo.GetLevel();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_LIMITLEVEL);
		if (level < value)
		{
			string empty = string.Empty;
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"level",
				value.ToString()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public bool IsClearBabel(short floor, BABEL_CLEARINFO info, short floortype)
	{
		BabelTowerManager.BabelTower_Floor babelTowerFloorInfo = this.GetBabelTowerFloorInfo(floor, floortype);
		return babelTowerFloorInfo != null && babelTowerFloorInfo.m_Column == info.ColumnNum && (babelTowerFloorInfo.m_ClearInfo & info.ClearInfo) == babelTowerFloorInfo.m_ClearInfo && babelTowerFloorInfo.m_FloorType == (long)info.FloorType;
	}

	public bool IsClearBabel(short floor, short sub_floor, BABEL_CLEARINFO info)
	{
		if (info == null)
		{
			TsLog.LogError("@@@@@@@BABEL_CLEARINFO == NULL@@@@@@@@@@  Column={0}", new object[]
			{
				NrTSingleton<BabelTowerManager>.Instance.GetBabelColumnNumFromFloor(floor, sub_floor)
			});
			return false;
		}
		int num = (int)(((floor - 1) * 5 + sub_floor) % 63);
		long num2 = 1L;
		long num3 = num2 << num;
		return (num3 & info.ClearInfo) == num3;
	}

	public bool IsEnableBattleUseActivityPoint(short nFloor, short nSubFloor, short nFloorType)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		BABELTOWER_DATA babelTowerData = this.GetBabelTowerData(nFloor, nSubFloor, nFloorType);
		return babelTowerData != null && kMyCharInfo.IsEnableBattleUseActivityPoint(babelTowerData.m_nWillSpend);
	}

	public byte GetBabelColumnNumFromFloor(short floor, short sub_floor)
	{
		return (byte)(((floor - 1) * 5 + sub_floor) / 63);
	}

	public string GetBabelRankImgText(byte rank, bool treasure)
	{
		string text = string.Empty;
		switch (rank)
		{
		case 0:
			text = "Win_I_Rank";
			break;
		case 1:
			text = "Win_I_RankD";
			break;
		case 2:
			text = "Win_I_RankC";
			break;
		case 3:
			text = "Win_I_RankB";
			break;
		case 4:
			text = "Win_I_RankA";
			break;
		case 5:
			text = "Win_I_RankS";
			break;
		case 6:
			text = "Win_I_RankSS";
			break;
		}
		if (!treasure)
		{
			text += "_G";
		}
		return text;
	}

	public void AddBabelGuildBossData(BABEL_GUILDBOSS _data)
	{
		if (this.m_DicBabel_GuildBossInfo.ContainsKey(_data.m_nFloor))
		{
			return;
		}
		if (this.m_LastGuildBossFloor < _data.m_nFloor)
		{
			this.m_LastGuildBossFloor = _data.m_nFloor;
		}
		this.m_DicBabel_GuildBossInfo.Add(_data.m_nFloor, _data);
	}

	public bool IsBabelGuildBoss(short floor)
	{
		return this.m_DicBabel_GuildBossInfo.ContainsKey(floor);
	}

	public BABEL_GUILDBOSS GetBabelGuildBossinfo(short floor)
	{
		if (!this.m_DicBabel_GuildBossInfo.ContainsKey(floor))
		{
			return null;
		}
		return this.m_DicBabel_GuildBossInfo[floor];
	}

	public short GetLastGuildBossFloor()
	{
		return this.m_LastGuildBossFloor;
	}
}
