using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class NkSoldierInfo
{
	public SOLDIER_INFO m_kBase;

	public SOLDIER_BATTLESKILL_INFO m_kBattleSkill;

	private NrEquipItemInfo m_kEquipItemInfo;

	public long[] m_nSolSubData = new long[14];

	private NrCharKindInfo m_pkCharKindInfo;

	private CHARKIND_ATTACKINFO m_pkCHARKIND_ATTACKINFO;

	private int m_nCharID;

	private long m_AddHelpExp;

	private bool m_HelpSolUse;

	private bool m_bEventHero;

	private int m_EventHeroHp;

	private bool m_bLoadAllInfo;

	protected NkSolStatInfo m_kSolStatInfo;

	private long m_nCombatPower;

	private int m_nSoldierUpdateCount;

	public long AddHelpExp
	{
		get
		{
			return this.m_AddHelpExp;
		}
		set
		{
			this.m_AddHelpExp = value;
		}
	}

	public bool HelpSolUse
	{
		get
		{
			return this.m_HelpSolUse;
		}
		set
		{
			this.m_HelpSolUse = value;
		}
	}

	public NkSoldierInfo()
	{
		this.m_kBase = new SOLDIER_INFO();
		this.m_kBattleSkill = new SOLDIER_BATTLESKILL_INFO();
		this.m_kEquipItemInfo = new NrEquipItemInfo();
		this.m_kSolStatInfo = new NkSolStatInfo();
		this.Init();
	}

	public void Init()
	{
		this.m_kBase.Init();
		this.m_kBattleSkill.Init();
		this.m_kEquipItemInfo.Init();
		for (int i = 0; i < 14; i++)
		{
			this.m_nSolSubData[i] = 0L;
		}
		this.m_pkCharKindInfo = null;
		this.m_pkCHARKIND_ATTACKINFO = null;
		this.m_kSolStatInfo.Init();
		this.m_nCharID = 0;
		this.m_AddHelpExp = 0L;
		this.m_bEventHero = false;
		this.m_EventHeroHp = 0;
	}

	public void SetBaseCharID(int charid)
	{
		this.m_nCharID = charid;
	}

	public int GetBaseCharID()
	{
		return this.m_nCharID;
	}

	public void Set(NkSoldierInfo pkInfo)
	{
		if (pkInfo == null)
		{
			this.Init();
			return;
		}
		this.m_kBase.Set(ref pkInfo.m_kBase);
		this.m_kBattleSkill.Set(pkInfo.m_kBattleSkill);
		this.m_kEquipItemInfo.Set(pkInfo.m_kEquipItemInfo);
		for (int i = 0; i < 14; i++)
		{
			this.m_nSolSubData[i] = pkInfo.m_nSolSubData[i];
		}
		this.SetCharKindInfo();
		this.SetBaseCharID(pkInfo.GetBaseCharID());
	}

	public void Set(SOLDIER_INFO pkSolinfoInfo)
	{
		this.m_kBase.Set(ref pkSolinfoInfo);
		this.SetCharKindInfo();
	}

	public void Set(BATTLE_SOLDIER_INFO BattleSolInfo)
	{
		this.m_kBase.CharKind = BattleSolInfo.CharKind;
		this.m_kBase.Grade = BattleSolInfo.Grade;
		this.m_kBase.Level = BattleSolInfo.Level;
		this.m_kBase.Exp = BattleSolInfo.Exp;
		this.m_kBase.HP = BattleSolInfo.HP;
		for (int i = 0; i < 6; i++)
		{
			this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique = BattleSolInfo.BattleSkillData[i].BattleSkillUnique;
			this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel = BattleSolInfo.BattleSkillData[i].BattleSkillLevel;
		}
		this.SetCharKindInfo();
	}

	public void Set(long SolID, int CharKind, short Level)
	{
		this.m_kBase.SolID = SolID;
		this.m_kBase.CharKind = CharKind;
		this.m_kBase.Level = Level;
		this.SetCharKindInfo();
	}

	public void Set(AUCTION_REGISTER_SOL_TOTAL SolTotal)
	{
		this.Set(SolTotal.SoldierInfo);
		for (int i = 0; i < 6; i++)
		{
			this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique = SolTotal.BattleSkillData[i].BattleSkillUnique;
			this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel = SolTotal.BattleSkillData[i].BattleSkillLevel;
		}
		for (int i = 0; i < 14; i++)
		{
			this.m_nSolSubData[i] = SolTotal.SolSubData[i];
		}
	}

	public void Set(SOL_WAREHOUSE_INFO SolWarehouseInfo)
	{
		this.m_kBase.SolID = SolWarehouseInfo.i64SolID;
		this.m_kBase.CharKind = SolWarehouseInfo.i32CharKind;
		this.m_kBase.Level = SolWarehouseInfo.i16level;
		this.m_kBase.Grade = SolWarehouseInfo.ui8grade;
		this.m_kBase.Exp = SolWarehouseInfo.i64exp;
		this.SetCharKindInfo();
	}

	public bool IsValid()
	{
		return this.m_kBase.CharKind != 0;
	}

	public bool IsLeader()
	{
		return this.m_kBase.SolPosType == 1 && this.m_kBase.SolPosIndex == 0;
	}

	public void SetSolID(long solid)
	{
		this.m_kBase.SolID = solid;
	}

	public long GetSolID()
	{
		return this.m_kBase.SolID;
	}

	public void SetSolPosType(byte postype)
	{
		this.m_kBase.SolPosType = postype;
	}

	public byte GetSolPosType()
	{
		return this.m_kBase.SolPosType;
	}

	public void SetSolPosIndex(byte posindex)
	{
		this.m_kBase.SolPosIndex = posindex;
	}

	public byte GetSolPosIndex()
	{
		return this.m_kBase.SolPosIndex;
	}

	public void SetSolStatus(byte status)
	{
		this.m_kBase.SolStatus = status;
		this.m_nSoldierUpdateCount++;
	}

	public void AddSolStatus(byte status)
	{
		if ((this.m_kBase.SolStatus & status) == 0)
		{
			SOLDIER_INFO expr_18 = this.m_kBase;
			expr_18.SolStatus |= status;
		}
		this.m_nSoldierUpdateCount++;
	}

	public void RemoveSolStatus(byte status)
	{
		if (this.IsSolStatus(status))
		{
			SOLDIER_INFO expr_12 = this.m_kBase;
			expr_12.SolStatus -= status;
		}
		this.m_nSoldierUpdateCount++;
	}

	public byte GetSolStatus()
	{
		return this.m_kBase.SolStatus;
	}

	public bool IsSolStatus(byte status)
	{
		return (this.m_kBase.SolStatus & status) > 0;
	}

	public void SetMilitaryUnique(byte militaryunique)
	{
		this.m_kBase.MilitaryUnique = militaryunique;
	}

	public byte GetMilitaryUnique()
	{
		return this.m_kBase.MilitaryUnique;
	}

	public void SetBattlePos(short battlepos)
	{
		this.m_kBase.BattlePos = battlepos;
	}

	public short GetBattlePos()
	{
		return this.m_kBase.BattlePos;
	}

	public long GetFriendPersonID()
	{
		return this.m_kBase.nFriendPersonID;
	}

	public void SetFriendPersonID(long val)
	{
		this.m_kBase.nFriendPersonID = val;
	}

	public int GetInitiativeValue()
	{
		return this.m_kBase.nInitiativeValue;
	}

	public void SetInitiativeValue(int InitiativeValue)
	{
		this.m_kBase.nInitiativeValue = InitiativeValue;
	}

	public string GetName()
	{
		if (this.m_pkCharKindInfo != null)
		{
			if (!this.m_pkCharKindInfo.IsATB(1L))
			{
				return this.m_pkCharKindInfo.GetName();
			}
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(this.m_nCharID);
			if (@char != null)
			{
				return @char.GetCharName();
			}
		}
		return string.Empty;
	}

	public string GetCharCode()
	{
		return this.m_pkCharKindInfo.GetCode();
	}

	public short GetLegendType()
	{
		BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = this.m_pkCharKindInfo.GetCHARKIND_SOLGRADEINFO((int)this.m_kBase.Grade);
		if (cHARKIND_SOLGRADEINFO == null)
		{
			return 0;
		}
		return cHARKIND_SOLGRADEINFO.LegendType;
	}

	public int GetSeason()
	{
		BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = this.m_pkCharKindInfo.GetCHARKIND_SOLGRADEINFO((int)this.m_kBase.Grade);
		if (cHARKIND_SOLGRADEINFO == null)
		{
			return 0;
		}
		return cHARKIND_SOLGRADEINFO.SolSeason;
	}

	public void SetGrade(byte grade)
	{
		this.m_kBase.Grade = grade;
		this.m_nSoldierUpdateCount++;
		this.UpdateSoldierStatInfo();
	}

	public byte GetGrade()
	{
		return this.m_kBase.Grade;
	}

	public string GetGradeCode()
	{
		return ((int)(this.GetGrade() + 1)).ToString();
	}

	public short GetSolMaxGrade()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return 14;
		}
		return this.m_pkCharKindInfo.GetSolMaxGrade();
	}

	public bool IsMaxGrade()
	{
		return 0 < this.GetLegendType() || (short)this.m_kBase.Grade >= this.GetSolMaxGrade();
	}

	public void SetLevel(short level)
	{
		this.m_kBase.Level = level;
		this.m_nSoldierUpdateCount++;
		if (this.m_pkCharKindInfo.IsATB(1L) || this.m_pkCharKindInfo.IsATB(2L))
		{
			EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_pkCharKindInfo.GetCharKind(), this.m_kBase.Grade);
			if (eventHeroCharCode != null)
			{
				this.m_bEventHero = true;
				this.m_EventHeroHp = eventHeroCharCode.i32Hp;
			}
		}
		this.UpdateSoldierStatInfo();
	}

	public short GetLevel()
	{
		return this.m_kBase.Level;
	}

	public void SetExp(long exp)
	{
		this.m_kBase.Exp = exp;
		this.m_nSoldierUpdateCount++;
	}

	public void AddExp(long exp)
	{
		if (this.IsMaxLevel())
		{
			return;
		}
		this.m_kBase.Exp += exp;
		this.m_nSoldierUpdateCount++;
	}

	public long GetExp()
	{
		return this.m_kBase.Exp;
	}

	public short GetSolMaxLevel()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return 200;
		}
		return this.m_pkCharKindInfo.GetGradeMaxLevel((short)this.m_kBase.Grade);
	}

	public bool IsMaxLevel()
	{
		return this.m_kBase.Level >= this.GetSolMaxLevel();
	}

	public bool GetMaxSkillLevel()
	{
		bool result = false;
		if (this.IsMaxLevel())
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique != 0)
				{
					result = ((int)this.GetLevel() == this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel);
				}
			}
		}
		return result;
	}

	public void SetBattleSkillInfo(SOLDIER_BATTLESKILL_INFO pkBattleSkill)
	{
		this.m_kBattleSkill.Set(pkBattleSkill);
	}

	public void SetBattleSkillData(int skillindex, ref BATTLESKILL_DATA pkSkillData)
	{
		if (skillindex < 0 || skillindex >= 6)
		{
			return;
		}
		this.m_kBattleSkill.BattleSkillData[skillindex].Set(ref pkSkillData);
	}

	public void SetBattleSkillData(int SkillIndex, int skillUnique, int skillLevel)
	{
		if (SkillIndex < 0 && SkillIndex >= 6)
		{
			return;
		}
		this.m_kBattleSkill.BattleSkillData[SkillIndex].BattleSkillUnique = skillUnique;
		this.m_kBattleSkill.BattleSkillData[SkillIndex].BattleSkillLevel = skillLevel;
	}

	public int SelectBattleSkillByWeapon(int skillIndex)
	{
		for (int i = 0; i < 6; i++)
		{
			int battleSkillUnique = this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique;
			int battleSkillLevel = this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel;
			if (battleSkillUnique > 0 && battleSkillLevel > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
				BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(battleSkillUnique, battleSkillLevel);
				if (battleSkillBase != null && battleSkillTraining != null)
				{
					if (battleSkillBase.m_nSkilNeedWeapon > 0)
					{
						if (this.CheckNeedWeaponType(battleSkillBase.m_nSkilNeedWeapon))
						{
							return battleSkillUnique;
						}
					}
					else if (this.CheckNeedWeaponType(this.GetWeaponType()))
					{
						return battleSkillUnique;
					}
				}
			}
		}
		return 0;
	}

	public int GetBattleSkillUnique(int skillIndex)
	{
		if (skillIndex < 0 || skillIndex > 6)
		{
			return 0;
		}
		return this.m_kBattleSkill.BattleSkillData[skillIndex].BattleSkillUnique;
	}

	public int GetBattleSkillLevelByIndex(int skillIndex)
	{
		if (skillIndex < 0 || skillIndex > 6)
		{
			return 0;
		}
		return this.m_kBattleSkill.BattleSkillData[skillIndex].BattleSkillLevel;
	}

	public int GetBattleSkillIndexByUnique(int skillUnique)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique == skillUnique)
			{
				return i;
			}
		}
		return 0;
	}

	public int GetBattleSkillLevel(int skillUnique)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique == skillUnique)
			{
				return this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel;
			}
		}
		for (int j = 0; j < 6; j++)
		{
			ITEM equipItem = this.GetEquipItem(j);
			if (equipItem != null)
			{
				int num = equipItem.m_nOption[4];
				if (skillUnique == num)
				{
					return equipItem.m_nOption[5];
				}
				num = equipItem.m_nOption[6];
				if (skillUnique == num)
				{
					return equipItem.m_nOption[9];
				}
			}
		}
		return 0;
	}

	public int GetUpgradeBattleSkillNum()
	{
		List<BATTLESKILL_TRAINING> battleSkillTrainingGroup = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTrainingGroup(this);
		if (battleSkillTrainingGroup == null)
		{
			return 0;
		}
		int num = 0;
		foreach (BATTLESKILL_TRAINING current in battleSkillTrainingGroup)
		{
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(current.m_nSkillUnique);
			if (battleSkillBase != null)
			{
				int battleSkillLevel = this.GetBattleSkillLevel(current.m_nSkillUnique);
				bool flag = battleSkillLevel < (int)this.GetLevel() && battleSkillLevel < battleSkillBase.m_nSkillMaxLevel;
				if (flag)
				{
					num++;
				}
			}
		}
		return num;
	}

	private void CalcBattleSkillStateInfo(int eStatType)
	{
		for (int i = 0; i < 6; i++)
		{
			int battleSkillUnique = this.m_kBattleSkill.BattleSkillData[i].BattleSkillUnique;
			int battleSkillLevel = this.m_kBattleSkill.BattleSkillData[i].BattleSkillLevel;
			if (battleSkillUnique > 0 && battleSkillLevel > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
				if (battleSkillBase != null)
				{
					BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, battleSkillLevel);
					if (battleSkillDetail != null)
					{
						if (battleSkillBase.m_nSkillType == 2)
						{
							if (battleSkillBase.m_nSkilNeedWeapon > 0)
							{
								if (!this.CheckNeedWeaponType(battleSkillBase.m_nSkilNeedWeapon))
								{
									goto IL_D0;
								}
							}
							else if (!this.CheckNeedWeaponType(this.GetWeaponType()))
							{
								goto IL_D0;
							}
							this.CheckBSkillDetailStateInfo(battleSkillDetail, eStatType);
						}
					}
				}
			}
			IL_D0:;
		}
		if (!this.m_pkCharKindInfo.IsATB(4L))
		{
			NrEquipItemInfo equipItemInfo = this.GetEquipItemInfo();
			if (equipItemInfo == null)
			{
				return;
			}
			for (int j = 0; j < 6; j++)
			{
				ITEM item = equipItemInfo.GetItem(j);
				if (item != null)
				{
					if (j != 5 || this.IsAtbCommonFlag(2L))
					{
						int num = item.m_nOption[4];
						int num2 = item.m_nOption[5];
						int num3 = item.m_nOption[6];
						int num4 = item.m_nOption[9];
						if (num > 0 && num2 > 0)
						{
							BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num);
							if (battleSkillBase == null)
							{
								goto IL_221;
							}
							if (battleSkillBase.m_nSkillType == 2)
							{
								BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num, num2);
								if (battleSkillDetail == null)
								{
									goto IL_221;
								}
								this.CheckBSkillDetailStateInfo(battleSkillDetail, eStatType);
							}
						}
						if (num3 > 0 && num4 > 0)
						{
							BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num3);
							if (battleSkillBase != null)
							{
								if (battleSkillBase.m_nSkillType == 2)
								{
									BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num3, num4);
									if (battleSkillDetail != null)
									{
										this.CheckBSkillDetailStateInfo(battleSkillDetail, eStatType);
									}
								}
							}
						}
					}
				}
				IL_221:;
			}
		}
	}

	public void CheckBSkillDetailStateInfo(BATTLESKILL_DETAIL BSkillDetail, int eStatType)
	{
		if (BSkillDetail == null)
		{
			return;
		}
		if (eStatType == 0 || eStatType == 2)
		{
			for (int i = 0; i < 10; i++)
			{
				if (BSkillDetail.m_nSkillDetalParamType[i] > 0)
				{
					switch (BSkillDetail.m_nSkillDetalParamType[i])
					{
					case 5:
						this.m_kSolStatInfo.m_nBS_SumSTR += BSkillDetail.m_nSkillDetalParamValue[i];
						break;
					case 6:
						this.m_kSolStatInfo.m_nBS_SumDEX += BSkillDetail.m_nSkillDetalParamValue[i];
						break;
					case 7:
						this.m_kSolStatInfo.m_nBS_SumINT += BSkillDetail.m_nSkillDetalParamValue[i];
						break;
					case 8:
						this.m_kSolStatInfo.m_nBS_SumVIT += BSkillDetail.m_nSkillDetalParamValue[i];
						break;
					}
				}
			}
		}
		if (eStatType == 1 || eStatType == 2)
		{
			for (int j = 0; j < 10; j++)
			{
				if (BSkillDetail.m_nSkillDetalParamType[j] > 0)
				{
					switch (BSkillDetail.m_nSkillDetalParamType[j])
					{
					case 9:
						this.m_kSolStatInfo.m_nBS_PhysicalDefense += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 10:
						this.m_kSolStatInfo.m_nBS_MagicDefense += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 11:
						this.m_kSolStatInfo.m_nBS_HitRate += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 12:
						this.m_kSolStatInfo.m_nBS_Evasion += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 13:
						this.m_kSolStatInfo.m_nBS_Critical += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 14:
						this.m_kSolStatInfo.m_nBS_MinDamage += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 15:
						this.m_kSolStatInfo.m_nBS_MaxDamage += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 16:
						this.m_kSolStatInfo.m_nBS_MaxHP += BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 17:
					{
						int num = this.m_kSolStatInfo.m_nPhysicalDefense * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_PhysicalDefense += num;
						break;
					}
					case 18:
					{
						int num = this.m_kSolStatInfo.m_nMagicDefense * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_MagicDefense += num;
						break;
					}
					case 19:
					{
						int num = this.m_kSolStatInfo.m_nHitRate * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_HitRate += num;
						break;
					}
					case 20:
					{
						int num = this.m_kSolStatInfo.m_nEvasion * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_Evasion += num;
						break;
					}
					case 21:
					{
						int num = this.m_kSolStatInfo.m_nCritical * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_Critical += num;
						break;
					}
					case 22:
					{
						int num = this.m_kSolStatInfo.m_nMinDamage * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_MinDamage += num;
						break;
					}
					case 23:
					{
						int num = this.m_kSolStatInfo.m_nMaxDamage * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_MaxDamage += num;
						break;
					}
					case 24:
					{
						int num = this.m_kSolStatInfo.m_nMaxHP * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_MaxHP += num;
						break;
					}
					case 33:
						this.m_kSolStatInfo.m_nBS_MaxHP -= BSkillDetail.m_nSkillDetalParamValue[j];
						break;
					case 34:
					{
						int num = this.m_kSolStatInfo.m_nMaxHP * BSkillDetail.m_nSkillDetalParamValue[j] / 10000;
						this.m_kSolStatInfo.m_nBS_MaxHP -= num;
						break;
					}
					}
				}
			}
		}
	}

	public bool CheckNeedWeaponType(int battleskillNeedWeapon)
	{
		ITEM equipItem = this.GetEquipItem(0);
		eITEM_TYPE itemTypeByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemTypeByItemUnique(equipItem.m_nItemUnique);
		if (itemTypeByItemUnique == eITEM_TYPE.ITEMTYPE_NONE)
		{
			if (this.GetWeaponType() == battleskillNeedWeapon)
			{
				return true;
			}
		}
		else
		{
			switch (itemTypeByItemUnique)
			{
			case eITEM_TYPE.ITEMTYPE_SWORD:
				if (battleskillNeedWeapon == 1)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_SPEAR:
				if (battleskillNeedWeapon == 2)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_AXE:
				if (battleskillNeedWeapon == 3)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_BOW:
				if (battleskillNeedWeapon == 4)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_GUN:
				if (battleskillNeedWeapon == 5)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_CANNON:
				if (battleskillNeedWeapon == 6)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_STAFF:
				if (battleskillNeedWeapon == 7)
				{
					return true;
				}
				break;
			case eITEM_TYPE.ITEMTYPE_BIBLE:
				if (battleskillNeedWeapon == 8)
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	public long GetCurBaseExp()
	{
		short level = this.GetLevel() - 1;
		return NrTSingleton<NkLevelManager>.Instance.GetNextExp(this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO().EXP_TYPE, level);
	}

	public long GetNextExp()
	{
		return NrTSingleton<NkLevelManager>.Instance.GetNextExp(this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO().EXP_TYPE, this.GetLevel());
	}

	public long GetRemainExp()
	{
		return NrTSingleton<NkLevelManager>.Instance.GetRemainExp(this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO().EXP_TYPE, this.GetLevel(), this.GetExp());
	}

	public float GetExpPercent()
	{
		if (this.IsMaxLevel())
		{
			return 1f;
		}
		long num = this.GetNextExp() - this.GetCurBaseExp();
		float num2 = ((float)num - (float)this.GetRemainExp()) / (float)num;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		return num2;
	}

	public long GetEvolutionExp()
	{
		return this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_EVOLUTION_EXP);
	}

	public long GetCurBaseEvolutionExp()
	{
		int grade = (int)this.GetGrade();
		return NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(this.GetCharKind(), grade);
	}

	public long GetNextEvolutionExp()
	{
		int num = (int)this.GetGrade();
		if (!this.IsMaxGrade())
		{
			num++;
		}
		return NrTSingleton<NrCharKindInfoManager>.Instance.GetSolEvolutionNeedEXP(this.GetCharKind(), num);
	}

	public long GetRemainEvolutionExp()
	{
		return this.GetNextEvolutionExp() - this.GetEvolutionExp();
	}

	public float GetEvolutionExpPercent()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation() && this.IsLeader())
		{
			return 1f;
		}
		if (this.IsMaxGrade())
		{
			return 1f;
		}
		long num = this.GetNextEvolutionExp() - this.GetCurBaseEvolutionExp();
		float num2 = ((float)num - (float)this.GetRemainEvolutionExp()) / (float)num;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		return num2;
	}

	public NrEquipItemInfo GetEquipItemInfo()
	{
		return this.m_kEquipItemInfo;
	}

	public void SetReceivedEquipItem(bool bReceived)
	{
		this.m_kEquipItemInfo.SetReceiveData(bReceived);
	}

	public void SetEquipItemInfo(NrCharEquipPart pkEquipPart)
	{
		this.m_kEquipItemInfo.SetItemUniqueAll(pkEquipPart);
		this.UpdateSoldierStatInfo();
	}

	public bool SetItem(ITEM pkItem)
	{
		bool flag = this.m_kEquipItemInfo.SetEquipItem(pkItem);
		if (flag && pkItem.m_nItemPos == 0)
		{
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSkillInfo(this);
			}
		}
		return flag;
	}

	public void RemoveItem(int nitemPos)
	{
		this.m_kEquipItemInfo.RemoveItem(nitemPos);
		this.UpdateSoldierStatInfo();
	}

	public NkItem GetValidEquipItem(int itempos)
	{
		NkItem equipItem = this.m_kEquipItemInfo.GetEquipItem(itempos);
		if (equipItem != null && equipItem.IsValid() && equipItem.GetDurability() > 0)
		{
			return equipItem;
		}
		return null;
	}

	public ITEM GetEquipItem(int itempos)
	{
		return this.m_kEquipItemInfo.GetItem(itempos);
	}

	public ITEM GetEquipItemByUnique(int itemunique)
	{
		int equipItemPos = (int)NrTSingleton<ItemManager>.Instance.GetEquipItemPos(itemunique);
		return this.GetEquipItem(equipItemPos);
	}

	public int GetItemUpdateCount()
	{
		return this.m_kEquipItemInfo.GetUpdateCount();
	}

	public bool IsItemReceiveData()
	{
		return this.m_kEquipItemInfo.IsReceiveData();
	}

	private void SetCharKindInfo()
	{
		this.m_pkCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.GetCharKind());
		if (this.m_pkCharKindInfo != null)
		{
			CHARKIND_ATTACKINFO cHARKIND_ATTACKINFO = this.m_pkCharKindInfo.GetCHARKIND_ATTACKINFO();
			this.SetAttackInfo(ref cHARKIND_ATTACKINFO);
			this.UpdateSoldierStatInfo();
		}
	}

	public void SetCharKind(int charkind)
	{
		this.m_kBase.CharKind = charkind;
		this.SetCharKindInfo();
	}

	public int GetCharKind()
	{
		return this.m_kBase.CharKind;
	}

	public NrCharKindInfo GetCharKindInfo()
	{
		return this.m_pkCharKindInfo;
	}

	public void ChangeAttackInfo()
	{
		CHARKIND_ATTACKINFO cHARKIND_ATTACKINFO = null;
		if (this.m_pkCharKindInfo != null && this.m_pkCharKindInfo.IsATB((long)((ulong)-2147483648)))
		{
			ITEM equipItem = this.GetEquipItem(0);
			if (equipItem != null && equipItem.IsValid())
			{
				ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(equipItem.m_nItemUnique);
				if (itemTypeInfo != null)
				{
					cHARKIND_ATTACKINFO = NrTSingleton<NrBaseTableManager>.Instance.GetCharAttackInfo(itemTypeInfo.ATTACKTYPE.ToString());
				}
			}
		}
		if (cHARKIND_ATTACKINFO == null && this.m_pkCharKindInfo != null)
		{
			cHARKIND_ATTACKINFO = this.m_pkCharKindInfo.GetCHARKIND_ATTACKINFO();
		}
		this.SetAttackInfo(ref cHARKIND_ATTACKINFO);
	}

	public void SetAttackInfo(ref CHARKIND_ATTACKINFO attackinfo)
	{
		if (attackinfo != null)
		{
			this.m_pkCHARKIND_ATTACKINFO = attackinfo;
		}
	}

	public CHARKIND_ATTACKINFO GetAttackInfo()
	{
		return this.m_pkCHARKIND_ATTACKINFO;
	}

	public string GetWeaponCode()
	{
		return this.m_pkCHARKIND_ATTACKINFO.WEAPONCODE;
	}

	public int GetItemPosTypeByWeaponType()
	{
		if (this.m_pkCHARKIND_ATTACKINFO == null)
		{
			return 0;
		}
		switch (this.m_pkCHARKIND_ATTACKINFO.nWeaponType)
		{
		case 1:
		case 2:
		case 3:
			return 2;
		case 4:
		case 5:
		case 6:
			return 3;
		case 7:
		case 8:
			return 4;
		default:
			return 1;
		}
	}

	public int GetWeaponType()
	{
		if (this.m_pkCHARKIND_ATTACKINFO == null)
		{
			return 1;
		}
		return this.m_pkCHARKIND_ATTACKINFO.nWeaponType;
	}

	public int GetEquipWeaponOrigin()
	{
		return this.m_pkCharKindInfo.GetWeaponType();
	}

	public int GetEquipWeaponExtention()
	{
		if (!this.m_pkCharKindInfo.IsATB((long)((ulong)-2147483648)))
		{
			return 0;
		}
		for (int i = 1; i <= 8; i++)
		{
			ITEMTYPE_INFO itemTypeInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(i.ToString());
			if (itemTypeInfo != null)
			{
				if (this.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
				{
					if (itemTypeInfo.WEAPONTYPE != this.GetEquipWeaponOrigin())
					{
						return itemTypeInfo.WEAPONTYPE;
					}
				}
			}
		}
		return 0;
	}

	public bool IsEquipClassType(int weapontype, long classtype)
	{
		if (this.m_pkCharKindInfo == null)
		{
			return false;
		}
		if (classtype >= 9223372036854775807L)
		{
			return true;
		}
		if (!this.m_pkCharKindInfo.IsATB((long)((ulong)-2147483648)))
		{
			if (this.m_pkCharKindInfo.GetCHARKIND_ATTACKINFO().nWeaponType == weapontype)
			{
				return true;
			}
		}
		else if ((this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO().CLASSTYPE & classtype) > 0L)
		{
			return true;
		}
		return false;
	}

	public bool CanEquipAttackType(int attacktype)
	{
		return this.GetAttackInfo().ATTACKTYPE == attacktype;
	}

	public void EquipmentItem(ITEM a_cItem)
	{
		ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(a_cItem.m_nItemUnique);
		if (itemTypeInfo == null)
		{
			return;
		}
		if (!this.IsEquipClassType(itemTypeInfo.WEAPONTYPE, itemTypeInfo.EQUIPCLASSTYPE))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("34");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.GetSolPosType() == 2 || this.GetSolPosType() == 6)
		{
			return;
		}
		NrEquipItemInfo equipItemInfo = this.GetEquipItemInfo();
		eEQUIP_ITEM equipItemPos = NrTSingleton<ItemManager>.Instance.GetEquipItemPos(a_cItem.m_nItemUnique);
		ITEM item = equipItemInfo.GetItem((int)equipItemPos);
		GS_ITEM_MOVE_REQ gS_ITEM_MOVE_REQ = new GS_ITEM_MOVE_REQ();
		long nSrcSolID = 0L;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				nSrcSolID = soldierInfo.GetSolID();
			}
		}
		gS_ITEM_MOVE_REQ.m_nMoveType = NrTSingleton<ItemManager>.Instance.GetItemMoveType_InvenToSol(a_cItem.m_nPosType);
		gS_ITEM_MOVE_REQ.m_nSrcItemID = a_cItem.m_nItemID;
		gS_ITEM_MOVE_REQ.m_nSrcItemPos = a_cItem.m_nItemPos;
		gS_ITEM_MOVE_REQ.m_nSrcSolID = nSrcSolID;
		if (item.m_nItemUnique > 0)
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = (int)equipItemPos;
			gS_ITEM_MOVE_REQ.m_nDestSolID = this.GetSolID();
		}
		else
		{
			gS_ITEM_MOVE_REQ.m_nDestItemID = 0L;
			gS_ITEM_MOVE_REQ.m_nDestItemPos = (int)equipItemPos;
			gS_ITEM_MOVE_REQ.m_nDestSolID = this.GetSolID();
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MOVE_REQ, gS_ITEM_MOVE_REQ);
	}

	public CHARKIND_CLASSINFO GetClassInfo()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return null;
		}
		return this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO();
	}

	public bool IsClassType(long classtype)
	{
		return this.m_pkCharKindInfo != null && (this.m_pkCharKindInfo.GetCHARKIND_CLASSINFO().CLASSTYPE & classtype) > 0L;
	}

	public byte GetJoinCount()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		return (byte)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_JOIN_COUNT);
	}

	public byte GetJobType()
	{
		return this.m_pkCHARKIND_ATTACKINFO.nJobType;
	}

	public void InitSolSubData()
	{
		for (int i = 0; i < 14; i++)
		{
			this.m_nSolSubData[i] = 0L;
		}
	}

	public void SetSolSubData(int type, long value)
	{
		if (type < 0 || type >= 14)
		{
			return;
		}
		this.m_nSolSubData[type] = value;
		bool flag = false;
		bool flag2 = false;
		switch (type)
		{
		case 0:
			flag2 = true;
			break;
		case 1:
		case 2:
		case 4:
			flag = true;
			break;
		case 3:
			flag2 = true;
			break;
		case 9:
		case 10:
			flag = true;
			break;
		}
		if (flag && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.UpdateSolList(this.GetSolID());
				int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
				plunderSolListDlg.SetSolNum(solBatchNum, false);
			}
		}
		if (flag2)
		{
			this.m_nSoldierUpdateCount++;
		}
	}

	public long GetSolSubData(eSOL_SUBDATA eType)
	{
		return this.GetSolSubData((int)eType);
	}

	public long GetSolSubData(int type)
	{
		if (type < 0 || type >= 14)
		{
			return 0L;
		}
		return this.m_nSolSubData[type];
	}

	public void SetInjuryStatus(bool bInjury)
	{
		if (!bInjury)
		{
			if (this.IsSolStatus(2))
			{
				this.RemoveSolStatus(2);
				this.SetSolSubData(0, 0L);
				NrTSingleton<NkBabelMacroManager>.Instance.SetRequestInjury(this.GetSolID());
				NrTSingleton<NkCharManager>.Instance.ResetInjuryCureSolID();
			}
			return;
		}
		this.AddSolStatus(2);
		long curTime = PublicMethod.GetCurTime();
		this.SetSolSubData(0, curTime);
	}

	public bool IsInjuryStatus()
	{
		if (!this.IsSolStatus(2))
		{
			return false;
		}
		long solSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_STATUSVALUE);
		return solSubData > 0L && this.GetRemainInjuryTime() > 0L;
	}

	public long GetInjuryCureMoney()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num = 0;
		if (instance != null)
		{
			num = Math.Max(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_INJURYGOLD), 0);
		}
		long remainInjuryTime = this.GetRemainInjuryTime();
		long num2 = (remainInjuryTime % 60L <= 0L) ? 0L : 1L;
		long num3 = remainInjuryTime / 60L + num2;
		return num3 * (long)num;
	}

	public long GetInjuryTime()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return 0L;
		}
		int num = Math.Max(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_INJURYBASETIME), 1);
		int num2 = Math.Max(instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_INJURYLEVELTIME), 1);
		return (long)((num + (int)(this.GetLevel() - 1) * num2) * 60);
	}

	public long GetRemainInjuryTime()
	{
		long solSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_STATUSVALUE);
		if (solSubData <= 0L)
		{
			return 0L;
		}
		long injuryTime = this.GetInjuryTime();
		long diffSecond = PublicMethod.GetDiffSecond(solSubData);
		if (diffSecond >= injuryTime)
		{
			return 0L;
		}
		return injuryTime - diffSecond;
	}

	public bool InjuryCureByItem()
	{
		if (!this.IsInjuryStatus())
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		ITEM firstFunctionItem = NkUserInventory.GetInstance().GetFirstFunctionItem(eITEM_SUPPLY_FUNCTION.SUPPLY_INJURYCURE);
		if (firstFunctionItem != null)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(firstFunctionItem.m_nItemUnique);
			if (itemInfo == null || itemInfo.m_nParam[0] <= 0)
			{
				TsLog.LogError("InjuryCure Error: Can not find ItemInfo", new object[0]);
				return false;
			}
			num = firstFunctionItem.m_nItemNum;
			num2 = itemInfo.m_nParam[0];
		}
		if (firstFunctionItem == null || num <= 0)
		{
			return false;
		}
		long remainInjuryTime = this.GetRemainInjuryTime();
		int num3 = (int)remainInjuryTime / 60;
		int num4 = num3 / num2;
		if (remainInjuryTime % 60L > 0L)
		{
			num4++;
		}
		GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = new GS_ITEM_SUPPLY_USE_REQ();
		gS_ITEM_SUPPLY_USE_REQ.m_nItemUnique = firstFunctionItem.m_nItemUnique;
		gS_ITEM_SUPPLY_USE_REQ.m_nDestSolID = this.GetSolID();
		gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = 1;
		gS_ITEM_SUPPLY_USE_REQ.m_byPosType = firstFunctionItem.m_nPosType;
		gS_ITEM_SUPPLY_USE_REQ.m_shPosItem = firstFunctionItem.m_nItemPos;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
		return true;
	}

	public bool InjuryCureByMoney()
	{
		if (!this.IsInjuryStatus())
		{
			return false;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return false;
		}
		long money = kMyCharInfo.m_Money;
		long injuryCureMoney = this.GetInjuryCureMoney();
		if (money < injuryCureMoney)
		{
			return false;
		}
		GS_SOLDIER_INJURYCURE_REQ gS_SOLDIER_INJURYCURE_REQ = new GS_SOLDIER_INJURYCURE_REQ();
		gS_SOLDIER_INJURYCURE_REQ.nSolID = this.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_INJURYCURE_REQ, gS_SOLDIER_INJURYCURE_REQ);
		return true;
	}

	public bool IsEquipItem()
	{
		for (int i = 0; i < 6; i++)
		{
			ITEM equipItem = this.GetEquipItem(i);
			if (equipItem != null && 0 < equipItem.m_nItemUnique)
			{
				return true;
			}
		}
		return false;
	}

	public void SetLoadAllInfo(bool bLoadAllInfo)
	{
		this.m_bLoadAllInfo = bLoadAllInfo;
	}

	public bool GetLoadAllInfo()
	{
		return this.m_bLoadAllInfo;
	}

	public bool IsSolWarehouse()
	{
		return this.GetSolPosType() == 5;
	}

	public NkListSolInfo GetListSolInfo(bool showLevel = false)
	{
		return new NkListSolInfo
		{
			SolCharKind = this.GetCharKind(),
			SolGrade = (int)this.GetGrade(),
			SolLevel = this.GetLevel(),
			SolInjuryStatus = this.IsInjuryStatus(),
			ShowCombat = false,
			ShowLevel = showLevel
		};
	}

	public int GetFightPower()
	{
		return (int)this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
	}

	public bool IsAwakening()
	{
		bool result = false;
		if (this.IsAtbCommonFlag(4L))
		{
			result = true;
		}
		long solSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_INFO);
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = solSubData;
		if (sUBDATA_UNION.n16SubData_0 != 0)
		{
			result = true;
		}
		long solSubData2 = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		long solSubData3 = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		if (solSubData2 != 0L && solSubData3 != 0L)
		{
			result = true;
		}
		return result;
	}

	public bool IsAtbCommonFlag(long iAtbCommonFlag)
	{
		long solSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COMMONFLAG);
		return (solSubData & iAtbCommonFlag) != 0L;
	}

	public void SetAtbCommonFlag(long iAtbCommonFlag)
	{
		long num = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COMMONFLAG);
		num |= iAtbCommonFlag;
		GS_SOLSUBDATA_COMMONFLAG_REQ gS_SOLSUBDATA_COMMONFLAG_REQ = new GS_SOLSUBDATA_COMMONFLAG_REQ();
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SolID = this.GetSolID();
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SubDataValue = num;
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SubDataCommonFlag = iAtbCommonFlag;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLSUBDATA_COMMONFLAG_REQ, gS_SOLSUBDATA_COMMONFLAG_REQ);
	}

	public void DelAtbCommonFlag(long iAtbCommonFlag)
	{
		long num = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COMMONFLAG);
		num &= ~iAtbCommonFlag;
		GS_SOLSUBDATA_COMMONFLAG_REQ gS_SOLSUBDATA_COMMONFLAG_REQ = new GS_SOLSUBDATA_COMMONFLAG_REQ();
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SolID = this.GetSolID();
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SubDataValue = num;
		gS_SOLSUBDATA_COMMONFLAG_REQ.i64SubDataCommonFlag = iAtbCommonFlag;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLSUBDATA_COMMONFLAG_REQ, gS_SOLSUBDATA_COMMONFLAG_REQ);
	}

	public byte GetGuildWarRaidUnique()
	{
		if (this.GetSolPosType() != 7)
		{
			return 0;
		}
		return this.GetMilitaryUnique() - 60;
	}

	public int GetStatSTR()
	{
		return this.m_kSolStatInfo.m_nSumSTR;
	}

	public int GetStatDEX()
	{
		return this.m_kSolStatInfo.m_nSumDEX;
	}

	public int GetStatINT()
	{
		return this.m_kSolStatInfo.m_nSumINT;
	}

	public int GetStatVIT()
	{
		return this.m_kSolStatInfo.m_nSumVIT;
	}

	public int GetAwakeningSTR()
	{
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		return sUBDATA_UNION.n32SubData_0;
	}

	public int GetAwakeningDEX()
	{
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		return sUBDATA_UNION.n32SubData_1;
	}

	public int GetAwakeningINT()
	{
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		return sUBDATA_UNION.n32SubData_1;
	}

	public int GetAwakeningVIT()
	{
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		return sUBDATA_UNION.n32SubData_0;
	}

	public int GetHitRate()
	{
		return this.m_kSolStatInfo.m_nHitRate + this.GetBS_HitRate();
	}

	public int GetEvasion()
	{
		return this.m_kSolStatInfo.m_nEvasion + this.GetBS_Evasion();
	}

	public int GetCritical()
	{
		return this.m_kSolStatInfo.m_nCritical + this.GetBS_Critical();
	}

	public int GetCriticalInfoUI()
	{
		return this.m_kSolStatInfo.m_nCriticalInfoUI;
	}

	public int GetAttackSpeed()
	{
		return this.m_kSolStatInfo.m_nAttackSpeed;
	}

	public int GetPhysicalDefense()
	{
		return this.m_kSolStatInfo.m_nPhysicalDefense + this.GetBS_PhysicalDefense();
	}

	public float GetPhysicalDefenseRate()
	{
		return this.m_kSolStatInfo.m_fPhysicalDefenseRate;
	}

	public int GetMagicDefense()
	{
		return this.m_kSolStatInfo.m_nMagicDefense + this.GetBS_MagicDefense();
	}

	public float GetMagicDefenseRate()
	{
		return this.m_kSolStatInfo.m_fMagicDefenseRate;
	}

	public int GetMinDamage()
	{
		return this.m_kSolStatInfo.m_nMinDamage + this.GetBS_MinDamage();
	}

	public int GetMaxDamage()
	{
		return this.m_kSolStatInfo.m_nMaxDamage + this.GetBS_MaxDamage();
	}

	public int GetMaxHP()
	{
		return this.m_kSolStatInfo.m_nMaxHP + this.GetBS_MaxHP();
	}

	public int GetStatBS_STR()
	{
		return this.m_kSolStatInfo.m_nBS_SumSTR;
	}

	public int GetStatBS_DEX()
	{
		return this.m_kSolStatInfo.m_nBS_SumDEX;
	}

	public int GetStatBS_INT()
	{
		return this.m_kSolStatInfo.m_nBS_SumINT;
	}

	public int GetStatBS_VIT()
	{
		return this.m_kSolStatInfo.m_nBS_SumVIT;
	}

	public int GetBS_HitRate()
	{
		return this.m_kSolStatInfo.m_nBS_HitRate;
	}

	public int GetBS_Evasion()
	{
		return this.m_kSolStatInfo.m_nBS_Evasion;
	}

	public int GetBS_Critical()
	{
		return this.m_kSolStatInfo.m_nBS_Critical;
	}

	public int GetBS_PhysicalDefense()
	{
		return this.m_kSolStatInfo.m_nBS_PhysicalDefense;
	}

	public int GetBS_MagicDefense()
	{
		return this.m_kSolStatInfo.m_nBS_MagicDefense;
	}

	public int GetBS_MinDamage()
	{
		return this.m_kSolStatInfo.m_nBS_MinDamage;
	}

	public int GetBS_MaxDamage()
	{
		return this.m_kSolStatInfo.m_nBS_MaxDamage;
	}

	public int GetBS_RecoveryHP()
	{
		return this.m_kSolStatInfo.m_nBS_RecoveryHP;
	}

	public int GetBS_MaxHP()
	{
		return this.m_kSolStatInfo.m_nBS_MaxHP;
	}

	public void CalcStatInfo()
	{
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		this.m_kSolStatInfo.Init_BS_Stat();
		bool flag = false;
		int solgrade = 0;
		if (this.m_pkCharKindInfo.IsATB(3L))
		{
			flag = true;
			solgrade = (int)this.GetGrade();
		}
		int num = this.m_pkCharKindInfo.GetGradePlusSTR(solgrade);
		int num2 = this.m_pkCharKindInfo.GetGradePlusDEX(solgrade);
		int num3 = this.m_pkCharKindInfo.GetGradePlusINT(solgrade);
		int num4 = this.m_pkCharKindInfo.GetGradePlusVIT(solgrade);
		num += this.m_pkCharKindInfo.GetIncSTR(solgrade, (int)this.GetLevel());
		num2 += this.m_pkCharKindInfo.GetIncDEX(solgrade, (int)this.GetLevel());
		num3 += this.m_pkCharKindInfo.GetIncINT(solgrade, (int)this.GetLevel());
		num4 += this.m_pkCharKindInfo.GetIncVIT(solgrade, (int)this.GetLevel());
		this.CalcBattleSkillStateInfo(0);
		num += this.m_kSolStatInfo.m_nBS_SumSTR;
		num2 += this.m_kSolStatInfo.m_nBS_SumDEX;
		num3 += this.m_kSolStatInfo.m_nBS_SumINT;
		num4 += this.m_kSolStatInfo.m_nBS_SumVIT;
		if (!flag)
		{
			this.m_kSolStatInfo.m_nSumSTR = num;
			this.m_kSolStatInfo.m_nSumDEX = num2;
			this.m_kSolStatInfo.m_nSumINT = num3;
			this.m_kSolStatInfo.m_nSumVIT = num4;
			this.m_kSolStatInfo.m_nPhysicalDefense = this.m_pkCharKindInfo.GetPhysicalDefense();
			this.m_kSolStatInfo.m_nMagicDefense = 0;
			this.m_kSolStatInfo.m_nHitRate = this.m_pkCharKindInfo.GetHitRate();
			this.m_kSolStatInfo.m_nEvasion = this.m_pkCharKindInfo.GetEvasion();
			this.m_kSolStatInfo.m_nCritical = (this.m_pkCharKindInfo.GetCritical() + this.m_kSolStatInfo.m_nBS_Critical) / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_CRITICALRATE) + (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASECRITICAL);
			this.m_kSolStatInfo.m_nAttackSpeed = 100;
			this.m_kSolStatInfo.m_nMinDamage = this.m_pkCharKindInfo.GetMinDamage();
			this.m_kSolStatInfo.m_nMaxDamage = this.m_pkCharKindInfo.GetMaxDamage();
			this.m_kSolStatInfo.m_nRecoveryHP = 0;
			this.m_kSolStatInfo.m_nMaxHP = this.m_pkCharKindInfo.GetHP();
			this.CalcBattleSkillStateInfo(1);
		}
		else
		{
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			for (int i = 0; i < 6; i++)
			{
				NkItem validEquipItem = this.GetValidEquipItem(i);
				if (validEquipItem != null)
				{
					if (validEquipItem.GetITEMINFO() != null)
					{
						if (i != 5 || this.IsAtbCommonFlag(2L))
						{
							num += (int)validEquipItem.GetSTR();
							num2 += (int)validEquipItem.GetDEX();
							num3 += (int)validEquipItem.GetINT();
							num4 += (int)validEquipItem.GetVIT();
							num9 += validEquipItem.GetPhysicalDefense();
							num10 += validEquipItem.GetMagicDefense();
							num5 += validEquipItem.GetHitRate();
							num6 += validEquipItem.GetEvasion();
							num7 += validEquipItem.GetCritical();
							num8 += validEquipItem.GetAttackSpeed();
							num11 += validEquipItem.GetMinDamage();
							num12 += validEquipItem.GetMaxDamage();
							num13 += validEquipItem.GetAddHP();
						}
					}
				}
			}
			num += this.GetAwakeningSTR();
			num2 += this.GetAwakeningDEX();
			num3 += this.GetAwakeningINT();
			num4 += this.GetAwakeningVIT();
			this.m_kSolStatInfo.m_nSumSTR = num;
			this.m_kSolStatInfo.m_nSumDEX = num2;
			this.m_kSolStatInfo.m_nSumINT = num3;
			this.m_kSolStatInfo.m_nSumVIT = num4;
			this.m_kSolStatInfo.m_nPhysicalDefense = (int)((float)num * instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATDEFENCE)) + num9 + this.m_pkCharKindInfo.GetGradePlusPhysicalDefense((int)this.GetGrade());
			this.m_kSolStatInfo.m_nMagicDefense = num10;
			this.m_kSolStatInfo.m_nMagicDefense += this.m_pkCharKindInfo.GetGradePlusMagicDefense((int)this.GetGrade());
			this.m_kSolStatInfo.m_nHitRate = num5 + this.m_pkCharKindInfo.GetGradePlusHitRate((int)this.GetGrade());
			this.m_kSolStatInfo.m_nEvasion = num2 + num6 + this.m_pkCharKindInfo.GetGradePlusEvasion((int)this.GetGrade());
			this.m_kSolStatInfo.m_nCritical = num7 + this.m_kSolStatInfo.m_nBS_Critical + num2 / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_ITEMCRITICAL) + this.m_pkCharKindInfo.GetGradePlusCritical((int)this.GetGrade());
			this.m_kSolStatInfo.m_nCriticalInfoUI = this.m_kSolStatInfo.m_nCritical;
			this.m_kSolStatInfo.m_nCritical = this.m_kSolStatInfo.m_nCritical / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_CRITICALRATE) + (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASECRITICAL);
			num8 = 100 + num8;
			this.m_kSolStatInfo.m_nAttackSpeed = num8;
			int num14 = num;
			switch (this.GetJobType())
			{
			case 1:
				num14 = num;
				break;
			case 2:
				num14 = num3;
				break;
			case 3:
				num14 = num2;
				break;
			case 4:
				num14 = num3;
				break;
			}
			this.m_kSolStatInfo.m_nMinDamage = num14 * num11 / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATDAMAGE) + num11 + (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEDAMAGE) + (int)this.GetLevel() * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELDAMAGE) + this.m_pkCharKindInfo.GetGradePlusMinDamage((int)this.GetGrade());
			this.m_kSolStatInfo.m_nMaxDamage = num14 * num12 / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATDAMAGE) + num12 + (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEDAMAGE) + (int)this.GetLevel() * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELDAMAGE) + this.m_pkCharKindInfo.GetGradePlusMaxDamage((int)this.GetGrade());
			this.m_kSolStatInfo.m_nRecoveryHP = this.GetStatVIT() * (int)this.GetLevel() / 100;
			this.m_kSolStatInfo.m_nMaxHP = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BASEHP) + (this.GetStatVIT() / (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_STATHP) - (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_HPCONSTANT)) * ((int)this.GetLevel() + (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_LEVELHP)) + num13 + this.m_pkCharKindInfo.GetGradePlusHP((int)this.GetGrade());
			this.CalcBattleSkillStateInfo(1);
		}
		this.CalcDefenseRate();
		this.CalcCombatPower();
	}

	public void CalcDefenseRate()
	{
		int physicalDefense = this.GetPhysicalDefense();
		int magicDefense = this.GetMagicDefense();
		this.m_kSolStatInfo.m_fPhysicalDefenseRate = (float)BATTLE_CONSTANT_Manager.GetInstance().GetDefense(physicalDefense);
		this.m_kSolStatInfo.m_fMagicDefenseRate = (((float)magicDefense / 10f <= 50f) ? ((float)magicDefense / 10f) : 50f);
		this.m_kSolStatInfo.m_fMagicDefenseRate /= 100f;
	}

	public void CalcCombatPower()
	{
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		long num = 0L;
		num += (long)(this.m_kSolStatInfo.m_nMaxDamage * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_ATK));
		num += (long)(this.m_kSolStatInfo.m_nPhysicalDefense * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_PDEF));
		num += (long)(this.m_kSolStatInfo.m_nHitRate * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_HIT));
		num += (long)(this.m_kSolStatInfo.m_nEvasion * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_DODGE));
		num += (long)(this.m_kSolStatInfo.m_nCritical * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CRITICAL));
		num += (long)(this.m_kSolStatInfo.m_nSumSTR * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_STR));
		num += (long)(this.m_kSolStatInfo.m_nSumDEX * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_DEX));
		num += (long)(this.m_kSolStatInfo.m_nSumINT * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_INT));
		num += (long)(this.m_kSolStatInfo.m_nSumVIT * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_VIT));
		num += (long)(this.GetMaxHP() * (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_HP));
		num = (long)((float)num * instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CORRECT1));
		num = num * num * num;
		num = (long)((float)num * instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BP_CORRECT2));
		this.SetCombatPower(num);
		if (this.m_nCharID == 1)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				nrCharUser.GetPersonInfo().m_kSoldierList.SetBattleSol_CombatPower();
			}
		}
	}

	public void CalcHPMP()
	{
		if (this.m_bEventHero)
		{
			this.m_kSolStatInfo.m_nMaxHP = (int)((float)this.m_kSolStatInfo.m_nMaxHP * ((float)this.m_EventHeroHp * 0.01f));
		}
		this.SetHP(this.GetHP(), 0);
	}

	public void UpdateSoldierStatInfo()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return;
		}
		if (!this.m_kEquipItemInfo.IsReceiveData())
		{
			return;
		}
		this.ChangeAttackInfo();
		this.CalcStatInfo();
		this.CalcHPMP();
	}

	public void Force_UpdateSoldierStatInfo()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return;
		}
		this.ChangeAttackInfo();
		this.CalcStatInfo();
		this.CalcHPMP();
	}

	public void SetHP(int hp, int nAddHP)
	{
		if (this.m_kBase.HP != hp)
		{
			this.m_kBase.HP = hp;
		}
		if (this.m_kBase.HP < 0)
		{
			this.m_kBase.HP = 0;
		}
		if (this.m_kBase.HP > this.GetMaxHP() + nAddHP)
		{
			this.m_kBase.HP = this.GetMaxHP() + nAddHP;
		}
	}

	public void AddHP(int hp, int nAddHP)
	{
		this.m_kBase.HP += hp;
		if (this.m_kBase.HP < 0)
		{
			this.m_kBase.HP = 0;
		}
		if (this.m_kBase.HP > this.GetMaxHP() + nAddHP)
		{
			this.m_kBase.HP = this.GetMaxHP() + nAddHP;
		}
	}

	public int GetHP()
	{
		return this.m_kBase.HP;
	}

	public int GetRecoveryHP()
	{
		return this.m_kSolStatInfo.m_nRecoveryHP;
	}

	public void SetCombatPower(long combatpower)
	{
		this.m_nCombatPower = combatpower;
	}

	public long GetCombatPower()
	{
		return this.m_nCombatPower;
	}

	public void UpdateSoldierInfo()
	{
		if (this.m_nSoldierUpdateCount <= 0)
		{
			return;
		}
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.SetSoldierUpdate(this);
			solMilitaryGroupDlg.OnForceCheckInjury();
		}
		NrTSingleton<ExplorationManager>.Instance.UpdateSolInfo(this);
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				plunderSolListDlg.UpdateSolList(this.GetSolID());
				int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
				plunderSolListDlg.SetSolNum(solBatchNum, false);
			}
		}
		this.m_nSoldierUpdateCount = 0;
	}
}
