using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrPersonInfoBase
{
	protected NrCharBasicInfo m_kCharBasicInfo;

	public NrSoldierList m_kSoldierList;

	private bool bBattleChar;

	public NrPersonInfoBase()
	{
		this.m_kCharBasicInfo = new NrCharBasicInfo();
		this.m_kSoldierList = new NrSoldierList();
		this.bBattleChar = false;
	}

	public virtual void Init()
	{
		this.m_kCharBasicInfo.Init();
		this.m_kSoldierList.Init();
	}

	public NrCharBasicInfo GetBasicInfo()
	{
		return this.m_kCharBasicInfo;
	}

	public void SetPersonID(long personid)
	{
		this.m_kCharBasicInfo.m_nPersonID = personid;
	}

	public long GetPersonID()
	{
		return this.m_kCharBasicInfo.m_nPersonID;
	}

	public void SetCharName(string name)
	{
		this.m_kCharBasicInfo.m_szCharName = name;
	}

	public string GetCharName()
	{
		return this.m_kCharBasicInfo.m_szCharName;
	}

	public void SetBattleChar(bool bBattle)
	{
		this.bBattleChar = bBattle;
	}

	public bool IsBattleChar()
	{
		return this.bBattleChar;
	}

	public virtual void SetPersonInfo(NrPersonInfoBase pkPersonInfo)
	{
		if (pkPersonInfo == null)
		{
			return;
		}
		this.m_kCharBasicInfo.Set(pkPersonInfo.GetBasicInfo());
		this.m_kSoldierList.Set(pkPersonInfo.m_kSoldierList);
		this.SetBattleChar(pkPersonInfo.IsBattleChar());
	}

	public virtual void SetUserData(NEW_MAKECHAR_INFO _CHARINFO)
	{
		this.m_kCharBasicInfo.m_nPersonID = _CHARINFO.PersonID;
		this.SetCharName(TKString.NEWString(_CHARINFO.CharName));
		this.m_kCharBasicInfo.m_nSolID = _CHARINFO.SolID;
		this.SetCharPos(_CHARINFO.CharPos);
		this.SetDirection(_CHARINFO.Direction);
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(_CHARINFO.SolID, _CHARINFO.CharKind, _CHARINFO.Level);
		this.SetSoldierInfo(0, nkSoldierInfo);
	}

	public virtual void SetUserData(BATTLE_SOLDIER_INFO _CHARINFO)
	{
		this.SetCharName(TKString.NEWString(_CHARINFO.CharName));
		this.SetCharPos(_CHARINFO.CharPos);
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(_CHARINFO);
		nkSoldierInfo.SetReceivedEquipItem(true);
		this.SetSoldierInfo(0, nkSoldierInfo);
	}

	public void SetCharPos(POS3D vCharPos)
	{
		this.SetCharPos(vCharPos.x, vCharPos.y, vCharPos.z);
	}

	public void SetCharPos(Vector3 vCharPos)
	{
		this.SetCharPos(vCharPos.x, vCharPos.y, vCharPos.z);
	}

	public void SetCharPos(float x, float y, float z)
	{
		this.m_kCharBasicInfo.SetCharPos(x, y, z);
	}

	public Vector3 GetCharPos()
	{
		return this.m_kCharBasicInfo.m_vCharPos;
	}

	public void SetDirection(POS3D vDirection)
	{
		this.SetDirection(vDirection.x, vDirection.y, vDirection.z);
	}

	public void SetDirection(Vector3 vDirection)
	{
		this.SetDirection(vDirection.x, vDirection.y, vDirection.z);
	}

	public void SetDirection(float x, float y, float z)
	{
		this.m_kCharBasicInfo.SetDirection(x, y, z);
	}

	public Vector3 GetDirection()
	{
		return this.m_kCharBasicInfo.m_vDirection;
	}

	public void SetMoveSpeed(float movespeed)
	{
		this.m_kCharBasicInfo.m_fMoveSpeed = movespeed;
	}

	public float GetMoveSpeed()
	{
		return this.m_kCharBasicInfo.m_fMoveSpeed;
	}

	public void SetSolID(long solid)
	{
		this.m_kCharBasicInfo.m_nSolID = solid;
	}

	public long GetSolID()
	{
		return this.m_kCharBasicInfo.m_nSolID;
	}

	public NrSoldierList GetSoldierList()
	{
		return this.m_kSoldierList;
	}

	public NkSoldierInfo SetSoldierInfo(int solindex, NkSoldierInfo kSoldierInfo)
	{
		return this.m_kSoldierList.SetSoldierInfo(solindex, kSoldierInfo);
	}

	public NkSoldierInfo SetSoldierInfo(int solindex, SOLDIER_INFO kSoldierInfo)
	{
		return this.m_kSoldierList.SetSoldierInfo(solindex, kSoldierInfo);
	}

	public NkSoldierInfo GetLeaderSoldierInfo()
	{
		return this.m_kSoldierList.GetSoldierInfo(0);
	}

	public NkSoldierInfo GetSoldierInfo(int solindex)
	{
		return this.m_kSoldierList.GetSoldierInfo(solindex);
	}

	public NkSoldierInfo GetSoldierInfoBySolID(long solID)
	{
		return this.m_kSoldierList.GetSoldierInfoBySolID(solID);
	}

	public int GetUpgradeBattleSkillNum()
	{
		return this.m_kSoldierList.GetUpgradeBattleSkillNum();
	}

	public NkSoldierInfo GetSoldierInfoFromSolID(long solid)
	{
		if (solid == 0L)
		{
			return null;
		}
		NkSoldierInfo nkSoldierInfo = this.m_kSoldierList.GetSoldierInfoBySolID(solid);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetSolInfo(solid);
			if (nkSoldierInfo == null)
			{
				nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouse(solid);
			}
		}
		return nkSoldierInfo;
	}

	public void SetCharKind(int solindex, int charkind)
	{
		NkSoldierInfo nkSoldierInfo = this.GetSoldierInfo(solindex);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = new NkSoldierInfo();
			nkSoldierInfo.Set(0L, charkind, 1);
			this.m_kSoldierList.SetSoldierInfo(solindex, nkSoldierInfo);
		}
		else
		{
			nkSoldierInfo.SetCharKind(charkind);
		}
	}

	public int GetKind(int solindex)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(solindex);
		if (soldierInfo != null)
		{
			return soldierInfo.GetCharKind();
		}
		return 0;
	}

	public void SetLevel(long solid, short level)
	{
		if (solid == 0L)
		{
			solid = this.m_kCharBasicInfo.m_nSolID;
		}
		NkSoldierInfo soldierInfoFromSolID = this.GetSoldierInfoFromSolID(solid);
		if (soldierInfoFromSolID != null)
		{
			soldierInfoFromSolID.SetLevel(level);
		}
	}

	public int GetLevel(long solid)
	{
		if (solid == 0L)
		{
			solid = this.m_kCharBasicInfo.m_nSolID;
		}
		NkSoldierInfo soldierInfoFromSolID = this.GetSoldierInfoFromSolID(solid);
		if (soldierInfoFromSolID != null)
		{
			return (int)soldierInfoFromSolID.GetLevel();
		}
		return 0;
	}

	public void UpdateSoldierInfo()
	{
		this.m_kSoldierList.UpdateSoldierInfo();
	}

	public NkSoldierInfo IsHelpSol(long FriendPersonID)
	{
		return this.m_kSoldierList.IsHelpSol(FriendPersonID);
	}

	public int GetBattleSolSlotCount()
	{
		int result = 6;
		int level = this.GetLevel(0L);
		if (level < 5)
		{
			result = 3;
		}
		else if (level < 10)
		{
			result = 4;
		}
		else if (level < 20)
		{
			result = 5;
		}
		else if (level < 30)
		{
			result = 6;
		}
		return result;
	}

	public int GetSoldierLevelAverage()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.GetLevel((long)i) != 0)
			{
				num += this.GetLevel((long)i);
				num2++;
			}
		}
		return num / num2;
	}

	public List<int> GetSolKindList()
	{
		if (this.m_kSoldierList == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		NkSoldierInfo[] kSolInfo = this.m_kSoldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo == null)
			{
				Debug.LogError("ERROR, NrPersonInfoBase.cs. GetSolKindList(), soliderInfo is Null");
			}
			else if (!list.Contains(nkSoldierInfo.GetCharKind()))
			{
				list.Add(nkSoldierInfo.GetCharKind());
			}
		}
		return list;
	}
}
