using GAME;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrSolWarehouse
{
	private bool m_bLoadServerData;

	private List<NkSoldierInfo> m_SolWarehouseList = new List<NkSoldierInfo>();

	public void Clear()
	{
		this.m_SolWarehouseList.Clear();
	}

	public void AddSolWarehouseInfo(GS_SOLDIER_WAREHOUSE_MOVE_ACK ACK)
	{
		if (this.GetSolWarehouse(ACK.SoldierInfo.SolID) == null)
		{
			NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
			nkSoldierInfo.Set(ACK.SoldierInfo);
			for (int i = 0; i < 6; i++)
			{
				nkSoldierInfo.SetBattleSkillData(i, ref ACK.BattleSkillData[i]);
			}
			for (int i = 0; i < 16; i++)
			{
				nkSoldierInfo.SetSolSubData(i, ACK.SolSubData[i]);
			}
			this.m_SolWarehouseList.Add(nkSoldierInfo);
		}
	}

	public void AddSolWarehouseInfo(GS_SOLDIER_LOAD_GET_ACK ACK)
	{
		NkSoldierInfo nkSoldierInfo = this.GetSolWarehouse(ACK.SoldierInfo.SolID);
		if (nkSoldierInfo == null)
		{
			NkSoldierInfo nkSoldierInfo2 = new NkSoldierInfo();
			nkSoldierInfo2.Set(ACK.SoldierInfo);
			for (int i = 0; i < 6; i++)
			{
				nkSoldierInfo2.SetBattleSkillData(i, ref ACK.BattleSkillData[i]);
			}
			for (int i = 0; i < 16; i++)
			{
				nkSoldierInfo2.SetSolSubData(i, ACK.SolSubData[i]);
			}
			nkSoldierInfo2.SetLoadAllInfo(true);
			this.m_SolWarehouseList.Add(nkSoldierInfo2);
			nkSoldierInfo = nkSoldierInfo2;
		}
		else
		{
			nkSoldierInfo.Set(ACK.SoldierInfo);
			for (int i = 0; i < 6; i++)
			{
				nkSoldierInfo.SetBattleSkillData(i, ref ACK.BattleSkillData[i]);
			}
			for (int i = 0; i < 16; i++)
			{
				nkSoldierInfo.SetSolSubData(i, ACK.SolSubData[i]);
			}
			nkSoldierInfo.SetLoadAllInfo(true);
		}
		if (nkSoldierInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				if (0L < ACK.EquipItem[i].m_nItemID)
				{
					nkSoldierInfo.SetItem(ACK.EquipItem[i]);
				}
			}
			nkSoldierInfo.SetReceivedEquipItem(true);
			nkSoldierInfo.UpdateSoldierStatInfo();
		}
	}

	public void AddSolWarehouseInfo(NkSoldierInfo pkSolinfo)
	{
		pkSolinfo.SetSolPosType(5);
		for (int i = 0; i < this.m_SolWarehouseList.Count; i++)
		{
			if (pkSolinfo.GetSolID() == this.m_SolWarehouseList[i].GetSolID())
			{
				this.m_SolWarehouseList[i].Set(pkSolinfo);
				return;
			}
		}
		this.m_SolWarehouseList.Add(pkSolinfo);
	}

	public NkSoldierInfo AddSolInfo(SOLDIER_INFO solInfo, SOLDIER_BATTLESKILL_INFO pkBattleSkill, bool bReadyEquipItem)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(solInfo);
		if (pkBattleSkill != null)
		{
			nkSoldierInfo.SetBattleSkillInfo(pkBattleSkill);
		}
		if (this.IsSameSolExist(solInfo.SolID))
		{
			NkSoldierInfo soldierInfo = this.GetSoldierInfo(solInfo.SolID);
			if (soldierInfo != null)
			{
				NrEquipItemInfo equipItemInfo = nkSoldierInfo.GetEquipItemInfo();
				if (equipItemInfo != null)
				{
					equipItemInfo.Set(soldierInfo.GetEquipItemInfo());
				}
			}
			this.RemoveSol(nkSoldierInfo.GetSolID());
			bReadyEquipItem = true;
		}
		nkSoldierInfo.SetReceivedEquipItem(bReadyEquipItem);
		nkSoldierInfo.UpdateSoldierStatInfo();
		this.m_SolWarehouseList.Add(nkSoldierInfo);
		return nkSoldierInfo;
	}

	public NkSoldierInfo GetSolWarehouse(long lSolID)
	{
		for (int i = 0; i < this.m_SolWarehouseList.Count; i++)
		{
			if (lSolID == this.m_SolWarehouseList[i].GetSolID())
			{
				return this.m_SolWarehouseList[i];
			}
		}
		return null;
	}

	public bool GetSolWarehouseCharKind(int i32CharKind)
	{
		for (int i = 0; i < this.m_SolWarehouseList.Count; i++)
		{
			if (i32CharKind == this.m_SolWarehouseList[i].GetCharKind())
			{
				return true;
			}
		}
		return false;
	}

	public List<int> GetWarehouseSolKindList()
	{
		if (this.m_SolWarehouseList == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		foreach (NkSoldierInfo current in this.m_SolWarehouseList)
		{
			if (current == null)
			{
				Debug.LogError("ERROR, NrMyCharInfo.cs GetWarehouseSolKindList(), soliderInfo is Null");
			}
			else if (!list.Contains(current.GetCharKind()))
			{
				list.Add(current.GetCharKind());
			}
		}
		return list;
	}

	public List<NkSoldierInfo> GetSolWarehouseList()
	{
		return this.m_SolWarehouseList;
	}

	public void SetLoadServerData(bool bLoadServerData)
	{
		this.m_bLoadServerData = bLoadServerData;
	}

	public bool GetLoadServerData()
	{
		return this.m_bLoadServerData;
	}

	public void RemoveSolWarehouse(long lSolID)
	{
		for (int i = 0; i < this.m_SolWarehouseList.Count; i++)
		{
			if (lSolID == this.m_SolWarehouseList[i].GetSolID())
			{
				this.m_SolWarehouseList.RemoveAt(i);
				break;
			}
		}
	}

	public int GetSolWarehouseCount()
	{
		return this.m_SolWarehouseList.Count;
	}

	private bool IsSameSolExist(long solID)
	{
		if (this.m_SolWarehouseList == null)
		{
			return false;
		}
		foreach (NkSoldierInfo current in this.m_SolWarehouseList)
		{
			if (current != null)
			{
				if (current.GetSolID() == solID)
				{
					return true;
				}
			}
		}
		return false;
	}

	private NkSoldierInfo GetSoldierInfo(long solID)
	{
		if (this.m_SolWarehouseList == null)
		{
			return null;
		}
		foreach (NkSoldierInfo current in this.m_SolWarehouseList)
		{
			if (current != null)
			{
				if (current.GetSolID() == solID)
				{
					return current;
				}
			}
		}
		return null;
	}

	private void RemoveSol(long solID)
	{
		if (this.m_SolWarehouseList == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = null;
		foreach (NkSoldierInfo current in this.m_SolWarehouseList)
		{
			if (current != null)
			{
				if (current.GetSolID() == solID)
				{
					nkSoldierInfo = current;
					break;
				}
			}
		}
		if (nkSoldierInfo != null)
		{
			this.m_SolWarehouseList.Remove(nkSoldierInfo);
		}
	}
}
