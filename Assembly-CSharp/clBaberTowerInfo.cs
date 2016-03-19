using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class clBaberTowerInfo
{
	public int m_nBabelRoomIndex;

	public short m_nBabelFloor;

	public short m_nBabelSubFloor;

	public long m_nLeaderPersonID;

	public short m_nBabelMinlevel;

	public short m_nBabelMaxlevel;

	public byte m_nCount;

	public short m_nBountyHuntUnique;

	public short m_nBabelFloorType;

	public BABELTOWER_PERSON[] stBabelPersonInfo = new BABELTOWER_PERSON[4];

	public List<BABEL_RNDINVITE_PERSON> m_Babel_RndInvitePersonList = new List<BABEL_RNDINVITE_PERSON>();

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

	public short BountHuntUnique
	{
		get
		{
			return this.m_nBountyHuntUnique;
		}
		set
		{
			this.m_nBountyHuntUnique = value;
		}
	}

	public clBaberTowerInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			this.stBabelPersonInfo[i] = new BABELTOWER_PERSON();
		}
		this.m_Babel_RndInvitePersonList.Clear();
	}

	public void Init()
	{
		this.m_nBabelRoomIndex = 0;
		this.m_nBabelFloor = 0;
		this.m_nBabelSubFloor = 0;
		this.m_nLeaderPersonID = 0L;
		this.m_nBabelMinlevel = 0;
		this.m_nBabelMaxlevel = 0;
		this.m_nCount = 0;
		for (int i = 0; i < 4; i++)
		{
			this.stBabelPersonInfo[i].Init();
		}
		this.m_Babel_RndInvitePersonList.Clear();
		this.m_nBountyHuntUnique = 0;
		this.m_nBabelFloorType = 0;
	}

	public void DeletePartyPerson(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID == nPersonID)
			{
				this.stBabelPersonInfo[i].nPartyPersonID = 0L;
				this.stBabelPersonInfo[i].strCharName = string.Empty;
				this.stBabelPersonInfo[i].nLevel = 0;
				this.stBabelPersonInfo[i].bReady = false;
				break;
			}
		}
	}

	public string GetName(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID == nPersonID)
			{
				return this.stBabelPersonInfo[i].strCharName;
			}
		}
		return null;
	}

	public void SetReadyBattle(long nPersonID, bool bReadyBattle)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID == nPersonID)
			{
				this.stBabelPersonInfo[i].bReady = bReadyBattle;
				break;
			}
		}
	}

	public byte GetReadyPersonCount()
	{
		byte b = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].bReady)
			{
				b += 1;
			}
		}
		return b;
	}

	public bool IsReadyBattle(long nPersonID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID == nPersonID)
			{
				if (!SoldierBatch.BABELTOWER_INFO.IsBabelLeader(nPersonID))
				{
					return this.stBabelPersonInfo[i].bReady;
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
			if (this.stBabelPersonInfo[i].bReady)
			{
				num++;
			}
		}
		return (int)this.m_nCount == num;
	}

	public void SetBabelTowerInfo(int nBabelRoomIndex, short nBabelFloor, short nBabelSubFloor, long nLeaderPersonID, short MinLevel, short MaxLevel, short i16BountyHuntUnique, short i16BabelFloorType)
	{
		this.m_nBabelRoomIndex = nBabelRoomIndex;
		this.m_nBabelFloor = nBabelFloor;
		this.m_nBabelSubFloor = nBabelSubFloor;
		this.m_nLeaderPersonID = nLeaderPersonID;
		this.m_nBabelMinlevel = MinLevel;
		this.m_nBabelMaxlevel = MaxLevel;
		this.m_nBountyHuntUnique = i16BountyHuntUnique;
		this.m_nBabelFloorType = i16BabelFloorType;
	}

	public void SetPartyCount()
	{
		byte b = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID > 0L)
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

	public bool IsBabelLeader(long nPersonID)
	{
		return nPersonID == this.m_nLeaderPersonID;
	}

	public BABELTOWER_PERSON GetBabelLeaderInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID > 0L)
			{
				if (this.stBabelPersonInfo[i].nPartyPersonID == this.m_nLeaderPersonID)
				{
					return this.stBabelPersonInfo[i];
				}
			}
		}
		return null;
	}

	public void SetPossibleLevel(short min_level, short max_level)
	{
		this.m_nBabelMinlevel = min_level;
		this.m_nBabelMaxlevel = max_level;
	}

	public BABELTOWER_PERSON GetBabelPersonInfo(int count)
	{
		return this.stBabelPersonInfo[count];
	}

	public void InitReadyState()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.stBabelPersonInfo[i].nPartyPersonID > 0L)
			{
				if (this.m_nLeaderPersonID == this.stBabelPersonInfo[i].nPartyPersonID)
				{
					this.stBabelPersonInfo[i].bReady = true;
				}
				else
				{
					this.stBabelPersonInfo[i].bReady = false;
				}
			}
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERUSERLIST_DLG))
		{
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.UpdateBabelReadyState();
			}
		}
	}

	public void SetSlotType(int pos, byte slot_type)
	{
		if (pos < 0 || pos >= 4)
		{
			return;
		}
		this.stBabelPersonInfo[pos].nSlotType = slot_type;
	}

	public void AddRndInvitePerson(BABEL_RNDINVITE_PERSON info)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (!myCharInfo.m_kFriendInfo.IsFriend(info.i64PersonID))
		{
			this.m_Babel_RndInvitePersonList.Add(info);
		}
	}
}
