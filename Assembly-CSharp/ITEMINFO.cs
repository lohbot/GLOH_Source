using GAME;
using System;

public class ITEMINFO
{
	public int m_nItemUnique;

	public long m_nATB;

	public string m_strEnglishName = string.Empty;

	public int m_nUseDate;

	public int m_nSortOrder;

	public long m_nPrice;

	public string m_strMaterialCode = string.Empty;

	public string m_strTextKey = string.Empty;

	public string m_strIconPath = string.Empty;

	public string m_strToolTipTextKey = string.Empty;

	public string m_strIconFile = string.Empty;

	public short m_nIconIndex;

	public int m_nItemType;

	public byte m_nItemKind;

	public int m_nUseMinLevel;

	public int m_nUseMaxLevel;

	public int m_nMinDamage;

	public int m_nMaxDamage;

	public int m_nNextLevel;

	public int m_nAddHP;

	public short m_nSTR;

	public short m_nDEX;

	public short m_nINT;

	public short m_nVIT;

	public int m_nCriticalPlus;

	public int m_nAttackSpeed;

	public int m_nHitratePlus;

	public int m_nEvasionPlus;

	public int m_nDurability;

	public int m_nPruductIndex;

	public int m_nGroupIndex;

	public int m_nQualityLevel;

	public int m_nDefense;

	public int m_nMagicDefense;

	public int m_nEnmitySword;

	public int m_nEnmityScimitar;

	public int m_nEnmitySpear;

	public int m_nEnmityAx;

	public int m_nEnmityBow;

	public int m_nEnmityCrossbow;

	public int m_nEnmityFan;

	public int m_nEnmityCannon;

	public int m_nMoveSpeed;

	public string[] m_nQuestLink;

	public byte m_nIsUseQuest;

	public int m_nCallMonster;

	public int m_nCallMonsterArea;

	public int[] m_nBoxItemUnique;

	public int[] m_nBoxItemNumber;

	public int[] m_nBoxItemProbability;

	public byte m_nFunctions;

	public int[] m_nParam;

	public string m_strQuestItemFunc = string.Empty;

	public int m_nQuestFuncParam;

	public byte m_nQuestIsDrop;

	public byte m_nQuestIsDisappear;

	public string m_strModelPath = string.Empty;

	public byte m_nAttachPartChar;

	public string m_strTextColorCode = string.Empty;

	public int m_nBoxRank;

	public int m_nBoxSealInfo;

	public int m_nNeedOpenItemUnique;

	public int m_nNeedOpenItemNum;

	public int m_nCardType;

	public int m_nRecruitType;

	public int m_nSetUnique;

	public byte m_nStarGrade;

	public string m_strOnlyUse = string.Empty;

	public string[] m_strOnlyUseCharCode = new string[20];

	public ITEMINFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nItemUnique = 0;
		this.m_strTextKey = string.Empty;
		this.m_strEnglishName = string.Empty;
		this.m_strIconPath = string.Empty;
		this.m_nATB = 0L;
		this.m_strToolTipTextKey = string.Empty;
		this.m_strMaterialCode = string.Empty;
		this.m_strIconFile = string.Empty;
		this.m_nIconIndex = 0;
		this.m_strModelPath = string.Empty;
		this.m_nAttachPartChar = 0;
		this.m_nItemType = 0;
		this.m_nItemKind = 0;
		this.m_nIsUseQuest = 0;
		this.m_nFunctions = 0;
		this.m_nCallMonster = 0;
		this.m_nCallMonsterArea = 0;
		this.m_nMoveSpeed = 0;
		this.m_nGroupIndex = 0;
		this.m_nDurability = 0;
		this.m_nPruductIndex = 0;
		this.m_nQualityLevel = 0;
		this.m_nUseMinLevel = 0;
		this.m_nUseMaxLevel = 0;
		this.m_nMinDamage = 0;
		this.m_nMaxDamage = 0;
		this.m_nNextLevel = 0;
		this.m_nDefense = 0;
		this.m_nMagicDefense = 0;
		this.m_nAddHP = 0;
		this.m_nSTR = 0;
		this.m_nDEX = 0;
		this.m_nINT = 0;
		this.m_nVIT = 0;
		this.m_nCriticalPlus = 0;
		this.m_nAttackSpeed = 0;
		this.m_nHitratePlus = 0;
		this.m_nEvasionPlus = 0;
		this.m_nEnmitySword = 0;
		this.m_nEnmityScimitar = 0;
		this.m_nEnmitySpear = 0;
		this.m_nEnmityAx = 0;
		this.m_nEnmityBow = 0;
		this.m_nEnmityCrossbow = 0;
		this.m_nEnmityFan = 0;
		this.m_nEnmityCannon = 0;
		this.m_nParam = new int[2];
		this.m_nQuestLink = new string[3];
		this.m_nBoxItemUnique = new int[12];
		this.m_nBoxItemNumber = new int[12];
		this.m_nBoxItemProbability = new int[12];
		this.m_strTextColorCode = string.Empty;
		this.m_strQuestItemFunc = null;
		this.m_nQuestFuncParam = 0;
		this.m_nQuestIsDrop = 0;
		this.m_nQuestIsDisappear = 0;
		this.m_nCardType = 0;
		this.m_nRecruitType = 0;
		this.m_nSetUnique = 0;
		this.m_strOnlyUse = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			this.m_strOnlyUseCharCode[i] = string.Empty;
		}
	}

	public bool IsItemATB(long nFlag)
	{
		return (this.m_nATB & nFlag) != 0L;
	}

	public bool IsCharTribe(byte chartribe)
	{
		return (this.m_nAttachPartChar & chartribe) != 0;
	}

	public int GetUseMinLevel(ITEM Item)
	{
		int num = this.m_nUseMinLevel;
		if (Item != null)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsItemLevelCheckBlock())
			{
				num = this.m_nUseMinLevel - Item.m_nOption[8];
			}
			else
			{
				num = this.m_nUseMinLevel;
			}
		}
		if (0 > num)
		{
			num = 0;
		}
		return num;
	}
}
