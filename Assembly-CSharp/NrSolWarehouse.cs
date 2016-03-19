using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;

public class NrSolWarehouse
{
	private bool m_bLoadServerData;

	private List<NkSoldierInfo> m_SolWarehouseList = new List<NkSoldierInfo>();

	public void Clear()
	{
		this.m_SolWarehouseList.Clear();
	}

	public void AddSolWarehouseInfo(SOL_WAREHOUSE_INFO SolWarehouseInfo)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(SolWarehouseInfo);
		nkSoldierInfo.SetSolPosType(5);
		nkSoldierInfo.SetLoadAllInfo(false);
		this.m_SolWarehouseList.Add(nkSoldierInfo);
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
			for (int i = 0; i < 14; i++)
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
			for (int i = 0; i < 14; i++)
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
			for (int i = 0; i < 14; i++)
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

	public List<NkSoldierInfo> GetSolWarehouseList()
	{
		return this.m_SolWarehouseList;
	}

	public bool CheckSolWarehouseLoadServerData(byte eSolWarehouseLoadServerData, long lSolID)
	{
		if (this.m_bLoadServerData)
		{
			return true;
		}
		GS_SOLDIER_WAREHOUSE_GET_REQ gS_SOLDIER_WAREHOUSE_GET_REQ = new GS_SOLDIER_WAREHOUSE_GET_REQ();
		gS_SOLDIER_WAREHOUSE_GET_REQ.i8SolWarehouseLoadServerData = eSolWarehouseLoadServerData;
		gS_SOLDIER_WAREHOUSE_GET_REQ.i64SolID = lSolID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_WAREHOUSE_GET_REQ, gS_SOLDIER_WAREHOUSE_GET_REQ);
		return false;
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
}
