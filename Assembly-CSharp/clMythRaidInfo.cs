using GAME;
using System;
using UnityEngine;
using UnityForms;

public class clMythRaidInfo
{
	public int m_nMythRaidRoomIndex;

	public eMYTHRAID_DIFFICULTY m_nDifficulty;

	public long m_nLeaderPersonID;

	public short m_nMythRaidMinlevel;

	public short m_nMythRaidMaxlevel;

	public byte m_nCount;

	private bool m_bPartyBatch;

	public MYTHRAID_PERSON[] stMythRaidPersonInfo = new MYTHRAID_PERSON[4];

	public byte Count
	{
		get
		{
			return this.m_nCount;
		}
		set
		{
			this.m_nCount = value;
		}
	}

	public clMythRaidInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			this.stMythRaidPersonInfo[i] = new MYTHRAID_PERSON();
		}
	}

	public void Init()
	{
		this.m_nMythRaidRoomIndex = 0;
		this.m_nDifficulty = eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY;
		this.m_nLeaderPersonID = 0L;
		this.m_nMythRaidMinlevel = 0;
		this.m_nMythRaidMaxlevel = 0;
		this.m_nCount = 0;
		for (int i = 0; i < 4; i++)
		{
			this.stMythRaidPersonInfo[i].Init();
		}
	}

	public void DeletePartyPerson(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID == nPersonID)
			{
				this.stMythRaidPersonInfo[i].nPartyPersonID = 0L;
				this.stMythRaidPersonInfo[i].strCharName = string.Empty;
				this.stMythRaidPersonInfo[i].nLevel = 0;
				this.stMythRaidPersonInfo[i].bReady = false;
				this.stMythRaidPersonInfo[i].selectedGuardianUnique = -1;
				break;
			}
		}
	}

	public string GetName(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID == nPersonID)
			{
				return this.stMythRaidPersonInfo[i].strCharName;
			}
		}
		return null;
	}

	public void SetReadyBattle(long nPersonID, bool bReadyBattle)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID == nPersonID)
			{
				this.stMythRaidPersonInfo[i].bReady = bReadyBattle;
				break;
			}
		}
	}

	public byte GetReadyPersonCount()
	{
		byte b = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].bReady)
			{
				b += 1;
			}
		}
		return b;
	}

	public bool IsPartyBatch()
	{
		bool bPartyBatch = this.m_bPartyBatch;
		this.m_bPartyBatch = true;
		return bPartyBatch;
	}

	public bool IsReadyBattle(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID == nPersonID)
			{
				if (!SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(nPersonID))
				{
					return this.stMythRaidPersonInfo[i].bReady;
				}
			}
		}
		return false;
	}

	public bool IsCanBattle()
	{
		if (this.m_nCount <= 0)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].bReady)
			{
				num++;
			}
		}
		return (int)this.m_nCount == num;
	}

	public void SetMythRaidInfo(int nMythRaidRoomIndex, eMYTHRAID_DIFFICULTY eDifficulty, long nLeaderPersonID, short MinLevel, short MaxLevel)
	{
		this.m_nMythRaidRoomIndex = nMythRaidRoomIndex;
		this.m_nDifficulty = eDifficulty;
		this.m_nLeaderPersonID = nLeaderPersonID;
		this.m_nMythRaidMinlevel = MinLevel;
		this.m_nMythRaidMaxlevel = MaxLevel;
	}

	public void SetPartyCount()
	{
		byte b = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID > 0L)
			{
				b += 1;
			}
		}
		if (b == 0)
		{
			b = 1;
		}
		this.m_nCount = b;
	}

	public byte GetPartyCount()
	{
		return this.m_nCount;
	}

	public bool IsMythRaidLeader(long nPersonID)
	{
		return nPersonID == this.m_nLeaderPersonID;
	}

	public MYTHRAID_PERSON GetMythRaidLeaderInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID > 0L)
			{
				if (this.stMythRaidPersonInfo[i].nPartyPersonID == this.m_nLeaderPersonID)
				{
					return this.stMythRaidPersonInfo[i];
				}
			}
		}
		Debug.LogError("Can't not find LEADERINFO");
		return null;
	}

	public void SetPossibleLevel(short min_level, short max_level)
	{
		this.m_nMythRaidMinlevel = min_level;
		this.m_nMythRaidMaxlevel = max_level;
	}

	public MYTHRAID_PERSON GetMythRaidPersonInfo(int count)
	{
		return this.stMythRaidPersonInfo[count];
	}

	public void InitReadyState()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stMythRaidPersonInfo[i].nPartyPersonID > 0L)
			{
				if (this.m_nLeaderPersonID == this.stMythRaidPersonInfo[i].nPartyPersonID)
				{
					this.stMythRaidPersonInfo[i].bReady = true;
				}
				else
				{
					this.stMythRaidPersonInfo[i].bReady = false;
				}
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_USERLIST_DLG))
		{
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.UpdateMythRaidReadyState();
			}
		}
	}

	public void SetSlotType(int pos, byte slot_type)
	{
		if (pos < 0 || pos >= 4)
		{
			return;
		}
		this.stMythRaidPersonInfo[pos].nSlotType = slot_type;
		MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
		mythRaidLobbyUserListDlg.SetUserSlotType(pos, slot_type);
	}
}
