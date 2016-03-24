using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BattleSkill_Manager : NrTSingleton<BattleSkill_Manager>
{
	private Dictionary<int, BATTLESKILL_BASE> m_dicBattleSkillBase;

	private Dictionary<int, Dictionary<int, BATTLESKILL_DETAILINCLUDE>> mHash_BattleSkillDetailInclude;

	private Dictionary<int, Dictionary<int, BATTLESKILL_DETAIL>> mHash_BattleSkillDetail;

	private Dictionary<int, Dictionary<int, BATTLE_SKILL_TRAININGINCLUDE>> mHash_BattleSkillTrainingInclude;

	private Dictionary<int, Dictionary<int, BATTLESKILL_TRAINING>> mHash_BattleSkillTraining;

	private Dictionary<string, List<BATTLESKILL_TRAINING>> mHash_BattleSkillTraining_CharName;

	private Dictionary<int, Dictionary<int, MYTHSKILL_TRAINING>> m_hashMythSkillTraining;

	private Dictionary<string, BATTLESKILL_ICON> m_dicBattleSkillIcon;

	private Dictionary<int, UIBaseInfoLoader> m_dicUILoader = new Dictionary<int, UIBaseInfoLoader>();

	private bool m_BuffSkillTextUse = true;

	private BattleSkill_Manager()
	{
		this.m_dicBattleSkillBase = new Dictionary<int, BATTLESKILL_BASE>();
		this.mHash_BattleSkillDetailInclude = new Dictionary<int, Dictionary<int, BATTLESKILL_DETAILINCLUDE>>();
		this.mHash_BattleSkillDetail = new Dictionary<int, Dictionary<int, BATTLESKILL_DETAIL>>();
		this.mHash_BattleSkillTraining = new Dictionary<int, Dictionary<int, BATTLESKILL_TRAINING>>();
		this.mHash_BattleSkillTraining_CharName = new Dictionary<string, List<BATTLESKILL_TRAINING>>();
		this.mHash_BattleSkillTrainingInclude = new Dictionary<int, Dictionary<int, BATTLE_SKILL_TRAININGINCLUDE>>();
		this.m_hashMythSkillTraining = new Dictionary<int, Dictionary<int, MYTHSKILL_TRAINING>>();
		this.m_dicBattleSkillIcon = new Dictionary<string, BATTLESKILL_ICON>();
	}

	public void SetBattleSkillBase(BATTLESKILL_BASE BaseData)
	{
		if (!this.m_dicBattleSkillBase.ContainsKey(BaseData.m_nSkillUnique))
		{
			this.m_dicBattleSkillBase.Add(BaseData.m_nSkillUnique, BaseData);
		}
	}

	public void SetBattleSkillDetailInclude(BATTLESKILL_DETAILINCLUDE DetailIncludeData)
	{
		if (!this.mHash_BattleSkillDetailInclude.ContainsKey(DetailIncludeData.m_nSkillUnique))
		{
			this.mHash_BattleSkillDetailInclude.Add(DetailIncludeData.m_nSkillUnique, new Dictionary<int, BATTLESKILL_DETAILINCLUDE>());
		}
		if (!this.mHash_BattleSkillDetailInclude[DetailIncludeData.m_nSkillUnique].ContainsKey(DetailIncludeData.m_nSkillMinSkillLevel))
		{
			this.mHash_BattleSkillDetailInclude[DetailIncludeData.m_nSkillUnique].Add(DetailIncludeData.m_nSkillMinSkillLevel, DetailIncludeData);
		}
	}

	public void SetBattleSkillTrainingInclude(BATTLE_SKILL_TRAININGINCLUDE TrainingIncludeData)
	{
		if (!this.mHash_BattleSkillTrainingInclude.ContainsKey(TrainingIncludeData.m_nSkillUnique))
		{
			this.mHash_BattleSkillTrainingInclude.Add(TrainingIncludeData.m_nSkillUnique, new Dictionary<int, BATTLE_SKILL_TRAININGINCLUDE>());
		}
		if (!this.mHash_BattleSkillTrainingInclude[TrainingIncludeData.m_nSkillUnique].ContainsKey(TrainingIncludeData.m_nSkillMinSkillLevel))
		{
			this.mHash_BattleSkillTrainingInclude[TrainingIncludeData.m_nSkillUnique].Add(TrainingIncludeData.m_nSkillMinSkillLevel, TrainingIncludeData);
		}
	}

	public void SetBattleSkillDetail(BATTLESKILL_DETAIL DetailData)
	{
		if (!this.mHash_BattleSkillDetail.ContainsKey(DetailData.m_nSkillUnique))
		{
			this.mHash_BattleSkillDetail.Add(DetailData.m_nSkillUnique, new Dictionary<int, BATTLESKILL_DETAIL>());
		}
		if (!this.mHash_BattleSkillDetail[DetailData.m_nSkillUnique].ContainsKey(DetailData.m_nSkillLevel))
		{
			this.mHash_BattleSkillDetail[DetailData.m_nSkillUnique].Add(DetailData.m_nSkillLevel, DetailData);
		}
	}

	public void SetBattleSkillTraining(BATTLESKILL_TRAINING TrainingData)
	{
		if (!this.mHash_BattleSkillTraining.ContainsKey(TrainingData.m_nSkillUnique))
		{
			this.mHash_BattleSkillTraining.Add(TrainingData.m_nSkillUnique, new Dictionary<int, BATTLESKILL_TRAINING>());
		}
		if (!this.mHash_BattleSkillTraining[TrainingData.m_nSkillUnique].ContainsKey(TrainingData.m_nSkillLevel))
		{
			this.mHash_BattleSkillTraining[TrainingData.m_nSkillUnique].Add(TrainingData.m_nSkillLevel, TrainingData);
		}
		for (int i = 0; i < 20; i++)
		{
			if (this.mHash_BattleSkillTraining_CharName.ContainsKey(TrainingData.m_szCharCode[i]))
			{
				this.mHash_BattleSkillTraining_CharName[TrainingData.m_szCharCode[i]].Add(TrainingData);
			}
			else
			{
				this.mHash_BattleSkillTraining_CharName[TrainingData.m_szCharCode[i]] = new List<BATTLESKILL_TRAINING>();
				this.mHash_BattleSkillTraining_CharName[TrainingData.m_szCharCode[i]].Add(TrainingData);
			}
		}
	}

	public void SetMythSkillTraining(MYTHSKILL_TRAINING TrainingData)
	{
		if (!this.m_hashMythSkillTraining.ContainsKey(TrainingData.m_i32SkillUnique))
		{
			this.m_hashMythSkillTraining.Add(TrainingData.m_i32SkillUnique, new Dictionary<int, MYTHSKILL_TRAINING>());
		}
		if (!this.m_hashMythSkillTraining[TrainingData.m_i32SkillUnique].ContainsKey(TrainingData.m_i32SkillLevel))
		{
			this.m_hashMythSkillTraining[TrainingData.m_i32SkillUnique].Add(TrainingData.m_i32SkillLevel, TrainingData);
		}
	}

	public void SetBattleSkillIcon(BATTLESKILL_ICON IconData)
	{
		if (!this.m_dicBattleSkillIcon.ContainsKey(IconData.m_nSkillIconCode))
		{
			this.m_dicBattleSkillIcon.Add(IconData.m_nSkillIconCode, IconData);
		}
	}

	public bool AddMakeBattleSkillDetail(int SkillLevelIndex, BATTLESKILL_DETAILINCLUDE detailData_I, BATTLESKILL_DETAIL detailDataNext)
	{
		if (SkillLevelIndex <= 1 || detailData_I == null || detailDataNext == null)
		{
			return false;
		}
		if (SkillLevelIndex >= 101)
		{
			return false;
		}
		BATTLESKILL_DETAIL battleSkillDetail = this.GetBattleSkillDetail(detailData_I.m_nSkillUnique, SkillLevelIndex - 1);
		if (battleSkillDetail != null)
		{
			detailDataNext.Set(SkillLevelIndex, battleSkillDetail, detailData_I);
			detailDataNext.SetSkillDetailATB();
			this.SetBattleSkillDetail(detailDataNext);
			return true;
		}
		return false;
	}

	public bool AddMakeBattleSkillTraining(int SkillLevelIndex, BATTLE_SKILL_TRAININGINCLUDE trainingData_I, BATTLESKILL_TRAINING trainingDataNext)
	{
		if (SkillLevelIndex <= 1 || trainingData_I == null || trainingDataNext == null)
		{
			return false;
		}
		if (SkillLevelIndex >= 101)
		{
			return false;
		}
		BATTLESKILL_TRAINING battleSkillTraining = this.GetBattleSkillTraining(trainingData_I.m_nSkillUnique, SkillLevelIndex - 1);
		if (battleSkillTraining != null)
		{
			trainingDataNext.Set(SkillLevelIndex, battleSkillTraining, trainingData_I);
			this.SetBattleSkillTraining(trainingDataNext);
			return true;
		}
		return false;
	}

	public int GetNeedWeaponType(string datacode)
	{
		return NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponType(datacode);
	}

	public byte GetCharAniType(string datacode)
	{
		return (byte)NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo().GetCharAniTypeForString(datacode);
	}

	public byte GetBuffType(string datacode)
	{
		return (byte)NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo().GetBuffTypeForString(datacode);
	}

	public byte GetAiType(string datacode)
	{
		return (byte)NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo().GetAiTypeForString(datacode);
	}

	public BATTLESKILL_BASE GetBattleSkillBase(int skillUnique)
	{
		if (this.m_dicBattleSkillBase.ContainsKey(skillUnique))
		{
			return this.m_dicBattleSkillBase[skillUnique];
		}
		return null;
	}

	public BATTLESKILL_DETAILINCLUDE GetBattleSkillDetailIncludeData(int skillUnique, int skillLevel)
	{
		if (this.mHash_BattleSkillDetailInclude.ContainsKey(skillUnique))
		{
			foreach (KeyValuePair<int, BATTLESKILL_DETAILINCLUDE> current in this.mHash_BattleSkillDetailInclude[skillUnique])
			{
				int key = current.Key;
				BATTLESKILL_DETAILINCLUDE value = current.Value;
				if (key == skillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}

	public BATTLESKILL_DETAIL GetBattleSkillDetail(int skillUnique, int skillLevel)
	{
		if (this.mHash_BattleSkillDetail.ContainsKey(skillUnique))
		{
			foreach (KeyValuePair<int, BATTLESKILL_DETAIL> current in this.mHash_BattleSkillDetail[skillUnique])
			{
				int key = current.Key;
				BATTLESKILL_DETAIL value = current.Value;
				if (key == skillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}

	public BATTLE_SKILL_TRAININGINCLUDE GetBattleSkillTrainingIncludeData(int skillUnique, int skillLevel)
	{
		if (this.mHash_BattleSkillTrainingInclude.ContainsKey(skillUnique))
		{
			foreach (KeyValuePair<int, BATTLE_SKILL_TRAININGINCLUDE> current in this.mHash_BattleSkillTrainingInclude[skillUnique])
			{
				int key = current.Key;
				BATTLE_SKILL_TRAININGINCLUDE value = current.Value;
				if (key == skillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}

	public BATTLESKILL_ICON GetBattleSkillIcon(string skillIconCode)
	{
		if (this.m_dicBattleSkillIcon.ContainsKey(skillIconCode))
		{
			return this.m_dicBattleSkillIcon[skillIconCode];
		}
		return null;
	}

	public bool CheckBattleSkillDetailParamType(BATTLESKILL_DETAIL BSkillDetail)
	{
		if (BSkillDetail == null)
		{
			return false;
		}
		for (int i = 0; i < 10; i++)
		{
			switch (BSkillDetail.m_nSkillDetalParamType[i])
			{
			case 39:
			case 43:
			case 44:
				return true;
			}
		}
		return false;
	}

	public BATTLESKILL_TRAINING GetBattleSkillTraining(int skillUnique, int skillLevel)
	{
		if (this.mHash_BattleSkillTraining.ContainsKey(skillUnique))
		{
			foreach (KeyValuePair<int, BATTLESKILL_TRAINING> current in this.mHash_BattleSkillTraining[skillUnique])
			{
				int key = current.Key;
				BATTLESKILL_TRAINING value = current.Value;
				if (key == skillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}

	public List<BATTLESKILL_TRAINING> GetBattleSkillTrainingGroup(NkSoldierInfo SoldierData)
	{
		if (SoldierData == null)
		{
			return null;
		}
		List<BATTLESKILL_TRAINING> list = new List<BATTLESKILL_TRAINING>();
		string code = SoldierData.GetCharKindInfo().GetCode();
		if (!this.mHash_BattleSkillTraining_CharName.ContainsKey(code))
		{
			Debug.LogError("Cannot Find Skill : " + code);
			return null;
		}
		List<BATTLESKILL_TRAINING> list2 = this.mHash_BattleSkillTraining_CharName[code];
		if (list2 == null)
		{
			return null;
		}
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i].m_nSkillLevel == 1)
			{
				BATTLESKILL_BASE battleSkillBase = this.GetBattleSkillBase(list2[i].m_nSkillUnique);
				if (battleSkillBase != null)
				{
					if (battleSkillBase.m_nSkilNeedWeapon > 0)
					{
						if (SoldierData.CheckNeedWeaponType(battleSkillBase.m_nSkilNeedWeapon))
						{
							list.Add(list2[i]);
						}
					}
					else if (SoldierData.CheckNeedWeaponType(SoldierData.GetWeaponType()))
					{
						list.Add(list2[i]);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list;
	}

	public MYTHSKILL_TRAINING GetMythSkillTraining(int skillUnique, int skillLevel)
	{
		if (this.m_hashMythSkillTraining.ContainsKey(skillUnique))
		{
			foreach (KeyValuePair<int, MYTHSKILL_TRAINING> current in this.m_hashMythSkillTraining[skillUnique])
			{
				int key = current.Key;
				MYTHSKILL_TRAINING value = current.Value;
				if (key == skillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}

	public MYTHSKILL_TRAINING GetMythSkillTrainingData(NkSoldierInfo SoldierData)
	{
		if (SoldierData == null)
		{
			return null;
		}
		string code = SoldierData.GetCharKindInfo().GetCode();
		foreach (int current in this.m_hashMythSkillTraining.Keys)
		{
			foreach (KeyValuePair<int, MYTHSKILL_TRAINING> current2 in this.m_hashMythSkillTraining[current])
			{
				for (int i = 0; i < 20; i++)
				{
					if (!(code != current2.Value.m_strCharCode[i]))
					{
						if (current2.Value.m_i32SkillLevel == 1)
						{
							BATTLESKILL_BASE battleSkillBase = this.GetBattleSkillBase(current2.Value.m_i32SkillUnique);
							if (battleSkillBase != null)
							{
								if (battleSkillBase.m_nMythSkillType == 1)
								{
									int battleskillNeedWeapon;
									if (battleSkillBase.m_nSkilNeedWeapon > 0)
									{
										battleskillNeedWeapon = battleSkillBase.m_nSkilNeedWeapon;
									}
									else
									{
										battleskillNeedWeapon = SoldierData.GetWeaponType();
									}
									if (SoldierData.CheckNeedWeaponType(battleskillNeedWeapon))
									{
										return current2.Value;
									}
								}
							}
						}
					}
				}
			}
		}
		return null;
	}

	public UIBaseInfoLoader GetBattleSkillIconTexture(int skillUnique)
	{
		if (skillUnique <= 0)
		{
			return null;
		}
		BATTLESKILL_BASE battleSkillBase = this.GetBattleSkillBase(skillUnique);
		if (battleSkillBase == null)
		{
			return null;
		}
		BATTLESKILL_ICON battleSkillIcon = this.GetBattleSkillIcon(battleSkillBase.m_nSkillIconCode);
		if (battleSkillIcon == null)
		{
			return null;
		}
		if (this.m_dicUILoader.ContainsKey(battleSkillIcon.m_nSkillIconIndex))
		{
			return this.m_dicUILoader[battleSkillIcon.m_nSkillIconIndex];
		}
		string strSkillIconFile = battleSkillIcon.m_strSkillIconFile;
		int nSkillIconIndex = battleSkillIcon.m_nSkillIconIndex;
		UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
		uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		uIBaseInfoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/BattleSkill_Icon/", strSkillIconFile + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		float left = (float)(nSkillIconIndex % 20) * 51f;
		float top = (float)(nSkillIconIndex / 20) * 51f;
		uIBaseInfoLoader.UVs = new Rect(left, top, 50f, 50f);
		this.m_dicUILoader.Add(battleSkillIcon.m_nSkillIconIndex, uIBaseInfoLoader);
		return uIBaseInfoLoader;
	}

	public UIBaseInfoLoader GetBattleSkillBuffIconTexture(string BuffIconCode)
	{
		BATTLESKILL_ICON battleSkillIcon = this.GetBattleSkillIcon(BuffIconCode);
		if (battleSkillIcon == null)
		{
			return null;
		}
		if (this.m_dicUILoader.ContainsKey(battleSkillIcon.m_nSkillIconIndex))
		{
			return this.m_dicUILoader[battleSkillIcon.m_nSkillIconIndex];
		}
		string strSkillIconFile = battleSkillIcon.m_strSkillIconFile;
		int nSkillIconIndex = battleSkillIcon.m_nSkillIconIndex;
		UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
		uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		uIBaseInfoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/BattleSkill_Icon/", strSkillIconFile + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		float left = (float)(nSkillIconIndex % 20) * 51f;
		float top = (float)(nSkillIconIndex / 20) * 51f;
		uIBaseInfoLoader.UVs = new Rect(left, top, 50f, 50f);
		this.m_dicUILoader.Add(battleSkillIcon.m_nSkillIconIndex, uIBaseInfoLoader);
		return uIBaseInfoLoader;
	}

	public string GetBattleSkillDESC(int skillUnique)
	{
		if (skillUnique <= 0)
		{
			return null;
		}
		BATTLESKILL_BASE battleSkillBase = this.GetBattleSkillBase(skillUnique);
		if (battleSkillBase == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(battleSkillBase.m_nSkillDESC) || battleSkillBase.m_nSkillDESC == "0")
		{
			return string.Empty;
		}
		return battleSkillBase.m_nSkillDESC;
	}

	public string GetBattleSkillBuffIconDESC(string BuffIconCode)
	{
		BATTLESKILL_ICON battleSkillIcon = this.GetBattleSkillIcon(BuffIconCode);
		if (battleSkillIcon == null)
		{
			return null;
		}
		if (battleSkillIcon.m_nSkillIconDESC != string.Empty)
		{
			return battleSkillIcon.m_nSkillIconDESC;
		}
		return null;
	}

	public UIBaseInfoLoader GetBattleSkillBuffImmuneIconTexture(int nImmuneType)
	{
		string skillIconCode = "187";
		if (nImmuneType == 32768)
		{
			skillIconCode = "196";
		}
		BATTLESKILL_ICON battleSkillIcon = this.GetBattleSkillIcon(skillIconCode);
		if (battleSkillIcon == null)
		{
			return null;
		}
		if (this.m_dicUILoader.ContainsKey(battleSkillIcon.m_nSkillIconIndex))
		{
			return this.m_dicUILoader[battleSkillIcon.m_nSkillIconIndex];
		}
		string strSkillIconFile = battleSkillIcon.m_strSkillIconFile;
		int nSkillIconIndex = battleSkillIcon.m_nSkillIconIndex;
		UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
		uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		uIBaseInfoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/BattleSkill_Icon/", strSkillIconFile + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		float left = (float)(nSkillIconIndex % 20) * 51f;
		float top = (float)(nSkillIconIndex / 20) * 51f;
		uIBaseInfoLoader.UVs = new Rect(left, top, 50f, 50f);
		this.m_dicUILoader.Add(battleSkillIcon.m_nSkillIconIndex, uIBaseInfoLoader);
		return uIBaseInfoLoader;
	}

	public void SetBuffSkillTextUse(bool bCheck)
	{
		this.m_BuffSkillTextUse = bCheck;
	}

	public bool GetBuffSkillTextUse()
	{
		return this.m_BuffSkillTextUse;
	}

	public bool IsTargetWeaponTypeCheck(BATTLESKILL_BASE BSkillBase, NkBattleChar TargetBattleChar)
	{
		if (BSkillBase == null || TargetBattleChar == null)
		{
			return false;
		}
		if (BSkillBase.m_nSkillTargetWeaponType == 1L)
		{
			return true;
		}
		long targetWeaponType = 0L;
		NkItem validEquipItem = TargetBattleChar.GetValidEquipItem(0);
		if (validEquipItem == null)
		{
			switch (TargetBattleChar.GetWeaponType())
			{
			case 1:
				targetWeaponType = 2L;
				break;
			case 2:
				targetWeaponType = 4L;
				break;
			case 3:
				targetWeaponType = 8L;
				break;
			case 4:
				targetWeaponType = 16L;
				break;
			case 5:
				targetWeaponType = 32L;
				break;
			case 6:
				targetWeaponType = 64L;
				break;
			case 7:
				targetWeaponType = 128L;
				break;
			case 8:
				targetWeaponType = 256L;
				break;
			}
		}
		else
		{
			switch (validEquipItem.GetItemType())
			{
			case 1:
				targetWeaponType = 2L;
				break;
			case 2:
				targetWeaponType = 4L;
				break;
			case 3:
				targetWeaponType = 8L;
				break;
			case 4:
				targetWeaponType = 16L;
				break;
			case 5:
				targetWeaponType = 32L;
				break;
			case 6:
				targetWeaponType = 64L;
				break;
			case 7:
				targetWeaponType = 128L;
				break;
			case 8:
				targetWeaponType = 256L;
				break;
			}
		}
		return BSkillBase.CheckTargetWeaponType(targetWeaponType);
	}
}
