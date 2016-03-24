using System;
using UnityForms;

public class NrCharKindInfo
{
	private CHARKIND_INFO m_pkCHARKIND_INFO;

	private CHARKIND_CLASSINFO m_pkCHARKIND_CLASSINFO;

	private CHARKIND_ATTACKINFO m_pkCHARKIND_ATTACKINFO;

	private CHARKIND_STATINFO m_pkCHARKIND_STATINFO;

	private CHARKIND_MONSTERINFO m_pkCHARKIND_MONSTERINFO;

	private CHARKIND_MONSTATINFO m_pkCHARKIND_MONSTATINFO;

	private CHARKIND_NPCINFO m_pkCHARKIND_NPCINFO;

	private NkCharAniInfo m_pkCharAniInfo;

	public CHARKIND_SOLDIERINFO m_pkCHARKIND_SOLDIERINFO;

	public CHARKIND_SOLGRADEINFO m_pkCHARKIND_SOLGRADEINFO;

	private int m_nCharKind;

	private byte m_nCharTribe;

	private string m_szName = string.Empty;

	private string m_szBundleName = string.Empty;

	private string m_szAniBundleName = string.Empty;

	private string m_szPosKey = string.Empty;

	private short m_nMaxSolGrade;

	public NrCharKindInfo(NrCharKindTempData pkTempData)
	{
		this.m_pkCHARKIND_INFO = pkTempData.m_pkCHARKIND_INFO;
		this.m_pkCHARKIND_CLASSINFO = pkTempData.m_pkCHARKIND_CLASSINFO;
		this.m_pkCHARKIND_ATTACKINFO = pkTempData.m_pkCHARKIND_ATTACKINFO;
		this.m_pkCHARKIND_STATINFO = pkTempData.m_pkCHARKIND_STATINFO;
		this.m_pkCHARKIND_MONSTERINFO = pkTempData.m_pkCHARKIND_MONSTERINFO;
		this.m_pkCHARKIND_MONSTATINFO = pkTempData.m_pkCHARKIND_MONSTATINFO;
		this.m_pkCHARKIND_NPCINFO = pkTempData.m_pkCHARKIND_NPCINFO;
		this.m_pkCHARKIND_SOLDIERINFO = pkTempData.m_pkCHARKIND_SOLDIERINFO;
		this.m_pkCHARKIND_SOLGRADEINFO = new CHARKIND_SOLGRADEINFO();
		this.m_nCharKind = 0;
		this.m_szName = string.Empty;
		this.m_szPosKey = string.Empty;
		this.m_nMaxSolGrade = 0;
	}

	public void SetCharKindInfo(ref CHARKIND_INFO charkindinfo, byte chartribe)
	{
		this.m_pkCHARKIND_INFO = charkindinfo;
		this.m_pkCHARKIND_CLASSINFO = NrTSingleton<NrBaseTableManager>.Instance.GetCharClassInfo(charkindinfo.nClassType.ToString());
		this.m_pkCHARKIND_ATTACKINFO = NrTSingleton<NrBaseTableManager>.Instance.GetCharAttackInfo(charkindinfo.nAttackType.ToString());
		this.m_nCharKind = this.m_pkCHARKIND_INFO.CHARKIND;
		this.m_nCharTribe = chartribe;
		if (string.IsNullOrEmpty(this.m_szName))
		{
			this.m_szName = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(this.m_pkCHARKIND_INFO.TEXTKEY);
		}
		if (charkindinfo.BUNDLE_PATH.Length > 0)
		{
			string[] array = charkindinfo.BUNDLE_PATH.Split(new char[]
			{
				'/'
			});
			this.m_szBundleName = array[array.Length - 1].Trim().ToLower();
		}
		if (charkindinfo.WEB_BUNDLE_PATH.Length > 0)
		{
			string[] array2 = charkindinfo.WEB_BUNDLE_PATH.Split(new char[]
			{
				'/'
			});
			this.m_szAniBundleName = array2[array2.Length - 1].Trim().ToLower();
		}
	}

	public void SetStatInfo(ref CHARKIND_STATINFO statinfo)
	{
		this.m_pkCHARKIND_STATINFO = statinfo;
	}

	public void SetMonsterInfo(ref CHARKIND_MONSTERINFO monsterinfo)
	{
		this.m_pkCHARKIND_MONSTERINFO = monsterinfo;
	}

	public void SetMonStatInfo(ref CHARKIND_MONSTATINFO monstatinfo)
	{
		this.m_pkCHARKIND_MONSTATINFO = monstatinfo;
	}

	public void SetNPCInfo(ref CHARKIND_NPCINFO npcinfo)
	{
		this.m_pkCHARKIND_NPCINFO = npcinfo;
	}

	public void SetAniInfo(ref NkCharAniInfo aniinfo)
	{
		this.m_pkCharAniInfo = aniinfo;
	}

	public void SetSoldierInfo(ref CHARKIND_SOLDIERINFO solinfo)
	{
		this.m_pkCHARKIND_SOLDIERINFO = solinfo;
	}

	public void SetSolGradeInfo(ref BASE_SOLGRADEINFO gradeinfo)
	{
		if (gradeinfo == null)
		{
			return;
		}
		if (gradeinfo.SolGrade < 0 || gradeinfo.SolGrade >= 15)
		{
			return;
		}
		this.m_pkCHARKIND_SOLGRADEINFO.SetSolGradeInfo(ref gradeinfo);
		if (gradeinfo.SolGrade > (int)this.m_nMaxSolGrade)
		{
			this.m_nMaxSolGrade = (short)gradeinfo.SolGrade;
		}
	}

	public CHARKIND_INFO GetCHARKIND_INFO()
	{
		return this.m_pkCHARKIND_INFO;
	}

	public CHARKIND_CLASSINFO GetCHARKIND_CLASSINFO()
	{
		return this.m_pkCHARKIND_CLASSINFO;
	}

	public CHARKIND_ATTACKINFO GetCHARKIND_ATTACKINFO()
	{
		return this.m_pkCHARKIND_ATTACKINFO;
	}

	public CHARKIND_STATINFO GetCHARKIND_STATINFO()
	{
		return this.m_pkCHARKIND_STATINFO;
	}

	public CHARKIND_MONSTERINFO GetCHARKIND_MONSTERINFO()
	{
		return this.m_pkCHARKIND_MONSTERINFO;
	}

	public CHARKIND_MONSTATINFO GetCHARKIND_MONSTATINFO()
	{
		return this.m_pkCHARKIND_MONSTATINFO;
	}

	public CHARKIND_NPCINFO GetCHARKIND_NPCINFO()
	{
		return this.m_pkCHARKIND_NPCINFO;
	}

	public CHARKIND_SOLDIERINFO GetCHARKIND_SOLDIERINFO()
	{
		return this.m_pkCHARKIND_SOLDIERINFO;
	}

	public BASE_SOLGRADEINFO GetCHARKIND_SOLGRADEINFO(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return null;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade];
	}

	public short GetLegendType(int solgrade)
	{
		return this.m_pkCHARKIND_SOLGRADEINFO.GetLegendType(solgrade);
	}

	public string GetCharEffectGrade(int solgrade)
	{
		if (this.GetLegendType(solgrade) == 1)
		{
			return this.m_pkCHARKIND_INFO.CharEffectGrade + "_SEVEN";
		}
		if (this.GetLegendType(solgrade) == 2)
		{
			return this.m_pkCHARKIND_INFO.CharEffectGrade + "_MYTH";
		}
		if (this.GetLegendType(solgrade) == 0)
		{
			return this.m_pkCHARKIND_INFO.CharEffectGrade;
		}
		return string.Empty;
	}

	public bool IsValid()
	{
		return this.m_nCharKind != 0;
	}

	public int GetCharKind()
	{
		return this.m_nCharKind;
	}

	public string GetCode()
	{
		return this.m_pkCHARKIND_INFO.CHARCODE;
	}

	public string GetName()
	{
		return this.m_szName;
	}

	public byte GetGender()
	{
		return this.m_pkCHARKIND_INFO.GENDER;
	}

	public byte GetCharTribe()
	{
		return this.m_nCharTribe;
	}

	public int GetWeaponType()
	{
		return this.m_pkCHARKIND_ATTACKINFO.nWeaponType;
	}

	public string GetWeaponCode()
	{
		return this.m_pkCHARKIND_ATTACKINFO.WEAPONCODE;
	}

	public int GetSeason(byte bGrade)
	{
		BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = this.GetCHARKIND_SOLGRADEINFO((int)bGrade);
		if (cHARKIND_SOLGRADEINFO == null)
		{
			return 0;
		}
		return cHARKIND_SOLGRADEINFO.SolSeason;
	}

	public string GetHitEffectCode()
	{
		if (this.m_pkCHARKIND_ATTACKINFO.HitEffectCode.Length == 0)
		{
			return "HIT";
		}
		return this.m_pkCHARKIND_ATTACKINFO.HitEffectCode;
	}

	public long GetATB()
	{
		return this.m_pkCHARKIND_INFO.nATB;
	}

	public bool IsATB(long flag)
	{
		return (this.m_pkCHARKIND_INFO.nATB & flag) != 0L;
	}

	public float GetBound()
	{
		return this.m_pkCHARKIND_INFO.fBound;
	}

	public byte GetBattleSizeX()
	{
		return this.m_pkCHARKIND_INFO.BattleSizeX;
	}

	public byte GetBattleSizeY()
	{
		return this.m_pkCHARKIND_INFO.BattleSizeY;
	}

	public byte GetAttackGrid()
	{
		return this.m_pkCHARKIND_ATTACKINFO.ATTACKGRID;
	}

	public string GetPortraitFile1(int solGrade = -1, string costumePortrait = "")
	{
		string text = string.Empty;
		if (string.IsNullOrEmpty(costumePortrait))
		{
			text = this.m_pkCHARKIND_INFO.PortraitFile1;
		}
		else
		{
			text = costumePortrait;
		}
		if (0 >= this.m_pkCHARKIND_INFO.PortraitRank)
		{
			return text;
		}
		if (solGrade == -1)
		{
			return NrTSingleton<UIDataManager>.Instance.GetString(text, "_03");
		}
		return NrTSingleton<UIDataManager>.Instance.GetString(text, "_0", (solGrade + 1).ToString());
	}

	public string GetDesc()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(this.m_pkCHARKIND_INFO.TEXTKEY_DESC);
	}

	public bool IsSlopeMode()
	{
		return this.m_pkCHARKIND_INFO.SLOPEMODE > 0;
	}

	public byte GetScale()
	{
		return this.m_pkCHARKIND_INFO.SCALE;
	}

	public string GetBundlePath()
	{
		return this.m_pkCHARKIND_INFO.BUNDLE_PATH;
	}

	public string GetBundleName()
	{
		return this.m_szBundleName;
	}

	public string GetAniBundleName()
	{
		return this.m_szAniBundleName;
	}

	public bool IsSameAniBundleName(string bundlename)
	{
		string text = this.m_szAniBundleName.Replace("_mobile", string.Empty);
		return text.Equals(bundlename);
	}

	public int GetSTR()
	{
		if (!this.IsATB(3L))
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.STR;
	}

	public int GetDEX()
	{
		if (!this.IsATB(3L))
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.DEX;
	}

	public int GetINT()
	{
		if (!this.IsATB(3L))
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.INT;
	}

	public int GetVIT()
	{
		if (!this.IsATB(3L))
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.VIT;
	}

	public int GetHitRate()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.HITRATE;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.HITRATE;
	}

	public int GetEvasion()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.EVASION;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.EVASION;
	}

	public int GetCritical()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.CRITICAL;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.CRITICAL;
	}

	public int GetMinDamage()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.MIN_DAMAGE;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.MIN_DAMAGE;
	}

	public int GetMaxDamage()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.MAX_DAMAGE;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.MAX_DAMAGE;
	}

	public int GetPhysicalDefense()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.DEFENSE;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.DEFENSE;
	}

	public int GetMagicDefense()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.MAGICDEFENSE;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.MAGICDEFENSE;
	}

	public int GetHP()
	{
		if (this.IsATB(3L))
		{
			return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[0].kSolStatInfo.HP;
		}
		if (this.m_pkCHARKIND_MONSTATINFO == null)
		{
			return 0;
		}
		if (this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo == null)
		{
			return 0;
		}
		return this.m_pkCHARKIND_MONSTATINFO.kSolStatInfo.HP;
	}

	public short GetBattleSkillUseRate()
	{
		return this.m_pkCHARKIND_STATINFO.BATTLESKILLUSERATE;
	}

	public short GetAngerlyPointMin()
	{
		return this.m_pkCHARKIND_STATINFO.ANGERLYPOINT_MIN;
	}

	public short GetAngerlyPointMax()
	{
		return this.m_pkCHARKIND_STATINFO.ANGERLYPOINT_MAX;
	}

	public short GetGradeMaxLevel(short solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 200;
		}
		if (!this.IsATB(3L))
		{
			return 200;
		}
		short maxLevel = this.m_pkCHARKIND_SOLGRADEINFO.GetMaxLevel((int)solgrade);
		short num;
		if (this.IsATB(1L))
		{
			num = NrTSingleton<ContentsLimitManager>.Instance.GetLimitLevel(1L);
		}
		else
		{
			if (!this.IsATB(2L))
			{
				return 0;
			}
			if (this.GetLegendType((int)solgrade) == 0)
			{
				num = NrTSingleton<ContentsLimitManager>.Instance.GetLimitLevel(2L);
			}
			else
			{
				num = maxLevel;
			}
		}
		if (num > 0 && maxLevel >= num)
		{
			return num;
		}
		return maxLevel;
	}

	public short GetSolMaxGrade()
	{
		short limitSolGrade = NrTSingleton<ContentsLimitManager>.Instance.GetLimitSolGrade();
		if (this.m_nMaxSolGrade > limitSolGrade)
		{
			this.m_nMaxSolGrade = limitSolGrade;
		}
		return this.m_nMaxSolGrade;
	}

	public int GetGradePlusSTR(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.STR;
	}

	public int GetGradePlusDEX(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.DEX;
	}

	public int GetGradePlusINT(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.INT;
	}

	public int GetGradePlusVIT(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.VIT;
	}

	public int GetGradePlusHitRate(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.HITRATE;
	}

	public int GetGradePlusEvasion(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.EVASION;
	}

	public int GetGradePlusCritical(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.CRITICAL;
	}

	public int GetGradePlusMinDamage(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.MIN_DAMAGE;
	}

	public int GetGradePlusMaxDamage(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.MAX_DAMAGE;
	}

	public int GetGradePlusPhysicalDefense(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.DEFENSE;
	}

	public int GetGradePlusMagicDefense(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.MAGICDEFENSE;
	}

	public int GetGradePlusHP(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolStatInfo.HP;
	}

	public int GetIncSTR(int solgrade, int sollevel)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return (int)this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolIncStatInfo.INC_STR * (sollevel - 1);
	}

	public int GetIncDEX(int solgrade, int sollevel)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return (int)this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolIncStatInfo.INC_DEX * (sollevel - 1);
	}

	public int GetIncINT(int solgrade, int sollevel)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return (int)this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolIncStatInfo.INC_INT * (sollevel - 1);
	}

	public int GetIncVIT(int solgrade, int sollevel)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return (int)this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].kSolIncStatInfo.INC_VIT * (sollevel - 1);
	}

	public int GetGainRate(int solgrade, int recruittype)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		if (recruittype < 0 || recruittype >= 23)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].nGainRate[recruittype];
	}

	public long GetComposeExp(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].ComposeExp;
	}

	public long GetEvolutionExp(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].EvolutionExp;
	}

	public long GetComposeCost(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].ComposeCost;
	}

	public long GetEvolutionCost(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].EvolutionCost;
	}

	public long GetSellPrice(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].SellPrice;
	}

	public byte GetTradeRank(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].TradeRank;
	}

	public long GetEvolutionNeedExp(int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return 0L;
		}
		return this.m_pkCHARKIND_SOLGRADEINFO.kBaseGradeInfo[solgrade].EvolutionNeedExp;
	}

	public void SetPosKey(string value)
	{
		this.m_szPosKey = value;
	}

	public string GetPosKey()
	{
		return this.m_szPosKey;
	}

	public int GetAniEventWeaponKey(int weapontype)
	{
		int result = 0;
		if (weapontype != this.GetWeaponType())
		{
			result = 1;
		}
		return result;
	}

	public float GetAnimationEvent(int weapontype, int anitype, int eventype, string costumeBundleName)
	{
		int aniEventWeaponKey = this.GetAniEventWeaponKey(weapontype);
		if (!string.IsNullOrEmpty(costumeBundleName))
		{
			return NrTSingleton<NrCharAniInfoManager>.Instance.GetAnimationEvent(costumeBundleName, aniEventWeaponKey, anitype, eventype);
		}
		if (this.m_pkCharAniInfo == null)
		{
			return 1f;
		}
		return this.m_pkCharAniInfo.GetAnimationEvent(aniEventWeaponKey, anitype, eventype);
	}

	public int GetHitAniCount(int weapontype, int anitype, string costumeBundleName)
	{
		int aniEventWeaponKey = this.GetAniEventWeaponKey(weapontype);
		if (!string.IsNullOrEmpty(costumeBundleName))
		{
			return NrTSingleton<NrCharAniInfoManager>.Instance.GetHitAniCount(costumeBundleName, aniEventWeaponKey, anitype);
		}
		if (this.m_pkCharAniInfo == null)
		{
			return -1;
		}
		return this.m_pkCharAniInfo.GetHitAniCount(aniEventWeaponKey, anitype);
	}

	public int GetBattleSkillLevel(int skillUnique)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique == skillUnique)
			{
				return this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillLevel;
			}
		}
		return 0;
	}

	public int GetBattleSkillUnique(int index)
	{
		if (index < 0 || index >= 6)
		{
			return 0;
		}
		return this.m_pkCHARKIND_STATINFO.kBattleSkillData[index].BattleSkillUnique;
	}

	public int GetBattleSkillLevelByIndex(int index)
	{
		if (index < 0 || index >= 6)
		{
			return 0;
		}
		return this.m_pkCHARKIND_STATINFO.kBattleSkillData[index].BattleSkillLevel;
	}

	public int GetBattleSkillIndexByUnique(int skillUnique)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique == skillUnique)
			{
				return i;
			}
		}
		return 0;
	}

	public int GetMythBattleSkillUniqueMaxCount()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique);
				if (battleSkillBase != null && battleSkillBase.m_nMythSkillType > 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetMythBattleSkillUniqueByIndex(int i32Idx)
	{
		int result = 0;
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique);
				if (battleSkillBase != null && battleSkillBase.m_nMythSkillType > 0)
				{
					if (num == i32Idx)
					{
						result = this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique;
					}
					num++;
				}
			}
		}
		return result;
	}

	public BATTLESKILL_BASE GetMythBattleSkill()
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_pkCHARKIND_STATINFO.kBattleSkillData[i].BattleSkillUnique);
				if (battleSkillBase != null && battleSkillBase.m_nMythSkillType > 0)
				{
					return battleSkillBase;
				}
			}
		}
		return null;
	}
}
