using GAME;
using PROTOCOL.WORLD;
using System;

public class NrPersonInfoUser : NrPersonInfoBase
{
	protected NrCharBasePart m_kCharBasicPart;

	public NrSubCharHelper m_kSubChar;

	public long m_i64LastLoginTime = -1L;

	private byte m_nDifficulty = 1;

	private long m_i64BattleSelectPersonID;

	private int m_i32BattleSelectRank;

	public byte Difficulty
	{
		get
		{
			return this.m_nDifficulty;
		}
		set
		{
			this.m_nDifficulty = value;
		}
	}

	public long InfiBattlePersonID
	{
		get
		{
			return this.m_i64BattleSelectPersonID;
		}
		set
		{
			this.m_i64BattleSelectPersonID = value;
		}
	}

	public int InfiBattleRank
	{
		get
		{
			return this.m_i32BattleSelectRank;
		}
		set
		{
			this.m_i32BattleSelectRank = value;
		}
	}

	public NrPersonInfoUser()
	{
		this.m_kCharBasicPart = new NrCharBasePart();
		this.m_kSubChar = new NrSubCharHelper();
		this.m_nDifficulty = 1;
		this.m_i64BattleSelectPersonID = 0L;
	}

	public override void Init()
	{
		base.Init();
		this.m_kCharBasicPart.Init();
		this.m_kSubChar.Init();
		this.m_nDifficulty = 1;
	}

	public void SetBasePart(NrCharBasePart pkBasicPart)
	{
		this.m_kCharBasicPart = pkBasicPart;
	}

	public NrCharBasePart GetBasePart()
	{
		return this.m_kCharBasicPart;
	}

	public void SetCharPartInfo(NrCharPartInfo pkPartInfo)
	{
		if (pkPartInfo != null)
		{
			this.SetBasePart(pkPartInfo.m_kBasePart);
			NkSoldierInfo soldierInfo = base.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				soldierInfo.SetEquipItemInfo(pkPartInfo.m_kEquipPart);
			}
		}
	}

	public override void SetPersonInfo(NrPersonInfoBase pkPersonInfo)
	{
		if (pkPersonInfo == null)
		{
			return;
		}
		base.SetPersonInfo(pkPersonInfo);
		NrPersonInfoUser nrPersonInfoUser = pkPersonInfo as NrPersonInfoUser;
		if (nrPersonInfoUser == null)
		{
			return;
		}
		if (this.m_kCharBasicPart == null)
		{
			return;
		}
		this.m_kCharBasicPart.SetData(nrPersonInfoUser.GetBasePart());
		this.m_kSubChar.Set(nrPersonInfoUser.m_kSubChar);
		this.m_i64LastLoginTime = nrPersonInfoUser.m_i64LastLoginTime;
	}

	public void SetBaseCharID(int charid)
	{
		this.m_kSoldierList.SetBaseCharID(charid);
	}

	public void SetUserData(WS_CHARLIST_ACK.NEW_CHARLIST_INFO charinfo)
	{
		this.m_i64LastLoginTime = charinfo.LastLoginTime;
		this.m_kCharBasicInfo.m_nPersonID = charinfo.PersonID;
		base.SetCharName(TKString.NEWString(charinfo.szCharName));
		this.m_kCharBasicInfo.m_nSolID = charinfo.SolID;
		this.m_kCharBasicPart.SetData(charinfo.kBasePart);
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(charinfo.SolID, charinfo.CharKind, charinfo.Level);
		base.SetSoldierInfo(0, nkSoldierInfo);
	}

	public override void SetUserData(NEW_MAKECHAR_INFO _CHARINFO)
	{
		base.SetUserData(_CHARINFO);
	}

	public NkSoldierInfo CleanSolPosType(byte prevpostype, NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return null;
		}
		NkSoldierInfo result = null;
		if (pkSolinfo.GetSolPosType() == 1)
		{
			if (prevpostype != 1)
			{
				NkSoldierInfo nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetSolInfo(pkSolinfo.GetSolID());
				if (nkSoldierInfo != null)
				{
					result = base.SetSoldierInfo((int)pkSolinfo.GetSolPosIndex(), pkSolinfo);
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().DelSol(nkSoldierInfo.GetSolID());
				}
			}
			else
			{
				result = pkSolinfo;
			}
		}
		else if (prevpostype == 1)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kSoldierList.GetSoldierInfoBySolID(pkSolinfo.GetSolID());
			if (nkSoldierInfo != null)
			{
				result = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().AddSolInfo(pkSolinfo);
				nkSoldierInfo.Init();
			}
		}
		else
		{
			result = pkSolinfo;
		}
		return result;
	}

	public NkSoldierInfo ChangeSolPosType(long solid, byte solpostype, byte solposindex, byte militaryunique, short battlepos)
	{
		NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
		if (militaryList == null)
		{
			return null;
		}
		NkSoldierInfo nkSoldierInfo = base.GetSoldierInfoFromSolID(solid);
		if (nkSoldierInfo == null)
		{
			return null;
		}
		byte solPosType = nkSoldierInfo.GetSolPosType();
		byte solPosIndex = nkSoldierInfo.GetSolPosIndex();
		byte militaryUnique = nkSoldierInfo.GetMilitaryUnique();
		nkSoldierInfo.SetSolPosType(solpostype);
		nkSoldierInfo.SetSolPosIndex(solposindex);
		nkSoldierInfo.SetMilitaryUnique(militaryunique);
		nkSoldierInfo.SetBattlePos(battlepos);
		nkSoldierInfo = this.CleanSolPosType(solPosType, nkSoldierInfo);
		if (nkSoldierInfo == null)
		{
			return null;
		}
		if (solPosType == 2)
		{
			if (solpostype != 2)
			{
				militaryList.DelMilitarySoldier(militaryUnique, solPosIndex);
			}
		}
		else if (solPosType == 6)
		{
			if (solpostype != 6)
			{
				militaryList.DelExpeditionMilitarySoldier(militaryUnique, solid);
			}
		}
		else if (solpostype == 2)
		{
			militaryList.AddMilitarySoldier(nkSoldierInfo.GetMilitaryUnique(), ref nkSoldierInfo);
		}
		else if (solpostype == 6)
		{
			militaryList.AddExpeditionMilitarySoldier(nkSoldierInfo.GetMilitaryUnique(), ref nkSoldierInfo);
		}
		return nkSoldierInfo;
	}
}
