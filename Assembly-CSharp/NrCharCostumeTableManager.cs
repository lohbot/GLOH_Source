using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using TsLibs;
using UnityEngine;

public class NrCharCostumeTableManager : NrTSingleton<NrCharCostumeTableManager>
{
	private Dictionary<string, List<CharCostumeInfo_Data>> _costumeInfoDic;

	private Dictionary<int, CharCostumeInfo_Data> _costumeDicByUnique;

	private Dictionary<int, COSTUME_INFO> _ownCostumeDic;

	private NrCharCostumeTableManager()
	{
		this._costumeInfoDic = new Dictionary<string, List<CharCostumeInfo_Data>>();
		this._costumeDicByUnique = new Dictionary<int, CharCostumeInfo_Data>();
		this._ownCostumeDic = new Dictionary<int, COSTUME_INFO>();
	}

	public void UpdateCostumeCount(int costumeIdx, int updateCostumeCount, int updateCostumePossibleToUseCount)
	{
		if (costumeIdx <= 0)
		{
			return;
		}
		if (!this._ownCostumeDic.ContainsKey(costumeIdx))
		{
			this._ownCostumeDic.Add(costumeIdx, new COSTUME_INFO());
		}
		this._ownCostumeDic[costumeIdx].UpdateCostumeCount(updateCostumeCount, updateCostumePossibleToUseCount);
	}

	public COSTUME_INFO GetCostumeInfo(int costumeIdx)
	{
		if (costumeIdx <= 0)
		{
			return null;
		}
		if (!this._ownCostumeDic.ContainsKey(costumeIdx))
		{
			return null;
		}
		return this._ownCostumeDic[costumeIdx];
	}

	public void InsertCostumeData(COSTUME_INFO costumeInfo)
	{
		if (costumeInfo == null || costumeInfo.i32CostumeUnique <= 0)
		{
			return;
		}
		if (!this._ownCostumeDic.ContainsKey(costumeInfo.i32CostumeUnique))
		{
			this._ownCostumeDic.Add(costumeInfo.i32CostumeUnique, costumeInfo);
			return;
		}
		this._ownCostumeDic[costumeInfo.i32CostumeUnique] = costumeInfo;
	}

	public void ClearCostumeData()
	{
		this._ownCostumeDic.Clear();
	}

	public bool IsBuyCostume(int costumeIdx)
	{
		return this._ownCostumeDic != null && this._ownCostumeDic.ContainsKey(costumeIdx);
	}

	public int GetCostumeIdx(long shopIdx)
	{
		return -1;
	}

	public void SetData(int dataIdx, TsDataReader.Row dataRow)
	{
		CharCostumeInfo_Data charCostumeInfo_Data = new CharCostumeInfo_Data();
		charCostumeInfo_Data.SetData(dataRow);
		if (this._costumeInfoDic == null || this._costumeDicByUnique == null)
		{
			Debug.LogError("ERROR, NrCharCostumeTableManager.cs, SetData(), _costumeInfoDic is Null ");
			return;
		}
		if (!this._costumeInfoDic.ContainsKey(charCostumeInfo_Data.m_CharCode))
		{
			this._costumeInfoDic.Add(charCostumeInfo_Data.m_CharCode, new List<CharCostumeInfo_Data>());
		}
		this._costumeInfoDic[charCostumeInfo_Data.m_CharCode].Add(charCostumeInfo_Data);
		if (!this._costumeDicByUnique.ContainsKey(charCostumeInfo_Data.m_costumeUnique))
		{
			this._costumeDicByUnique.Add(charCostumeInfo_Data.m_costumeUnique, charCostumeInfo_Data);
		}
	}

	public List<int> GetCostumeKindList()
	{
		if (this._costumeInfoDic == null)
		{
			Debug.LogError("ERROR, NrCharCostumeTableManager.cs, GetCostumeKindList(), _costumeInfoDic is Null ");
			return null;
		}
		List<int> list = new List<int>();
		foreach (string current in this._costumeInfoDic.Keys)
		{
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(current);
			if (charKindInfoFromCode != null)
			{
				list.Add(charKindInfoFromCode.GetCharKind());
			}
		}
		return list;
	}

	public List<CharCostumeInfo_Data> GetCostumeDataList(string charCode)
	{
		if (this._costumeInfoDic == null || !this._costumeInfoDic.ContainsKey(charCode))
		{
			return null;
		}
		return this._costumeInfoDic[charCode];
	}

	public bool IsNewCostumeExist()
	{
		if (this._costumeDicByUnique == null)
		{
			return false;
		}
		foreach (CharCostumeInfo_Data current in this._costumeDicByUnique.Values)
		{
			if (current != null)
			{
				if (current.m_New)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsNewCostumeExistByCode(string charCode)
	{
		if (this._costumeInfoDic == null || !this._costumeInfoDic.ContainsKey(charCode))
		{
			return false;
		}
		foreach (CharCostumeInfo_Data current in this._costumeInfoDic[charCode])
		{
			if (current != null)
			{
				if (current.m_New)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsEventCostumeExistByCode(string charCode)
	{
		if (this._costumeInfoDic == null || !this._costumeInfoDic.ContainsKey(charCode))
		{
			return false;
		}
		foreach (CharCostumeInfo_Data current in this._costumeInfoDic[charCode])
		{
			if (current != null)
			{
				if (current.m_Event)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsEventCostume(CharCostumeInfo_Data costumeData)
	{
		return costumeData.m_Event;
	}

	public CharCostumeInfo_Data GetNormalCostumeData(string charCode)
	{
		if (this._costumeInfoDic == null)
		{
			return null;
		}
		if (!this._costumeInfoDic.ContainsKey(charCode))
		{
			return null;
		}
		foreach (CharCostumeInfo_Data current in this._costumeInfoDic[charCode])
		{
			if (current.IsNormalCostume())
			{
				return current;
			}
		}
		return null;
	}

	public CharCostumeInfo_Data GetCostumeData(int costumeUnique)
	{
		if (this._costumeDicByUnique == null)
		{
			return null;
		}
		if (!this._costumeDicByUnique.ContainsKey(costumeUnique))
		{
			return null;
		}
		return this._costumeDicByUnique[costumeUnique];
	}

	public int GetCostumeSolKind(int costumeUnique)
	{
		if (this._costumeDicByUnique == null)
		{
			return -1;
		}
		if (!this._costumeDicByUnique.ContainsKey(costumeUnique))
		{
			return -1;
		}
		return NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(this._costumeDicByUnique[costumeUnique].m_CharCode);
	}

	public string GetCostumePortraitPath(int costumeUnique)
	{
		if (this._costumeDicByUnique == null)
		{
			return string.Empty;
		}
		if (!this._costumeDicByUnique.ContainsKey(costumeUnique))
		{
			return string.Empty;
		}
		return this._costumeDicByUnique[costumeUnique].m_PortraitPath;
	}

	public string GetCostumeBundlePath(int costumeUnique)
	{
		if (this._costumeDicByUnique == null)
		{
			return string.Empty;
		}
		if (!this._costumeDicByUnique.ContainsKey(costumeUnique))
		{
			return string.Empty;
		}
		return this._costumeDicByUnique[costumeUnique].m_BundlePath;
	}

	public string GetCostumePortraitPath(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return string.Empty;
		}
		int costumeUnique = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		return this.GetCostumePortraitPath(costumeUnique);
	}

	public string GetCostumeBundlePath(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return string.Empty;
		}
		int costumeUnique = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		return this.GetCostumeBundlePath(costumeUnique);
	}

	public bool IsCostumeKind(int charKind)
	{
		List<int> costumeKindList = this.GetCostumeKindList();
		return costumeKindList != null && costumeKindList.Contains(charKind);
	}

	public CharCostumeInfo_Data GetSoldierCostumeData(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return null;
		}
		int num = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		if (num <= 0)
		{
			return this.GetNormalCostumeData(solInfo.GetCharCode());
		}
		return this.GetCostumeData(num);
	}

	public string GetCostumeName(int costumeUnique)
	{
		CharCostumeInfo_Data costumeData = this.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return string.Empty;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(costumeData.m_CostumeTextKey);
	}

	public bool IsCostumeUniqueEqualSolKind(int costumeUnique, int charKind)
	{
		CharCostumeInfo_Data costumeData = this.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return false;
		}
		int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(costumeData.m_CharCode);
		return charKindByCode != 0 && charKindByCode == charKind;
	}
}
