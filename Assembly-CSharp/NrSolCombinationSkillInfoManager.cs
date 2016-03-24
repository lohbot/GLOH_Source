using System;
using System.Collections.Generic;
using TsLibs;
using UnityEngine;

public class NrSolCombinationSkillInfoManager : NrTSingleton<NrSolCombinationSkillInfoManager>
{
	private readonly Dictionary<long, SolCombinationInfo_Data> _battleSolCombinationData;

	private readonly int NOT_EXIST = -1;

	private NrSolCombinationSkillInfoManager()
	{
		this._battleSolCombinationData = new Dictionary<long, SolCombinationInfo_Data>();
	}

	public void SetData(int dataIdx, TsDataReader.Row dataRow)
	{
		SolCombinationInfo_Data solCombinationInfo_Data = new SolCombinationInfo_Data();
		solCombinationInfo_Data.SetData(dataRow);
		this._battleSolCombinationData.Add((long)dataIdx, solCombinationInfo_Data);
	}

	public Dictionary<long, SolCombinationInfo_Data> GetCombinationInfo()
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetCombinationInfoCount(), _battleSolCombinationData is Null");
			return null;
		}
		return this._battleSolCombinationData;
	}

	public List<KeyValuePair<long, SolCombinationInfo_Data>> GetCombinationInfoSortedSorList()
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetCombinationInfoCount(), _battleSolCombinationData is Null");
			return null;
		}
		List<KeyValuePair<long, SolCombinationInfo_Data>> list = new List<KeyValuePair<long, SolCombinationInfo_Data>>(this._battleSolCombinationData);
		list.Sort((KeyValuePair<long, SolCombinationInfo_Data> first, KeyValuePair<long, SolCombinationInfo_Data> next) => first.Value.m_sortNum.CompareTo(next.Value.m_sortNum));
		return list;
	}

	public Dictionary<int, string> GetCompleteCombinationDic(List<int> charKindList)
	{
		if (charKindList == null)
		{
			return null;
		}
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		foreach (KeyValuePair<long, SolCombinationInfo_Data> current in this._battleSolCombinationData)
		{
			if (this.IsCompleteCombination(charKindList, current.Value))
			{
				dictionary.Add(current.Value.m_nCombinationUnique, current.Value.m_textTitleKey);
			}
		}
		return dictionary;
	}

	public bool IsCompleteCombination(List<int> charKindList, SolCombinationInfo_Data combinationData)
	{
		if (charKindList == null)
		{
			return false;
		}
		string[] szCombinationIsCharCode = combinationData.m_szCombinationIsCharCode;
		if (szCombinationIsCharCode == null)
		{
			Debug.LogError("ERROR, SolCombination_DLG.cs, CompleteCombinationCheck(), combinationCharCodeList is Null");
			return false;
		}
		string[] array = szCombinationIsCharCode;
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			if (!string.IsNullOrEmpty(text) && !(text == "0"))
			{
				int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(text);
				if (!charKindList.Contains(charKindByCode))
				{
					return false;
				}
			}
		}
		return true;
	}

	public int GetCompleteCombinationTopUniqeKey(List<int> charKindList)
	{
		Dictionary<int, string> completeCombinationDic = this.GetCompleteCombinationDic(charKindList);
		if (completeCombinationDic == null)
		{
			return this.NOT_EXIST;
		}
		if (completeCombinationDic.Count == 0)
		{
			return this.NOT_EXIST;
		}
		if (completeCombinationDic.Keys == null || completeCombinationDic.Keys.Count == 0)
		{
			return this.NOT_EXIST;
		}
		List<int> list = new List<int>(completeCombinationDic.Keys);
		list.Sort();
		return list[0];
	}

	public int GetSortNumByUniqeKey(int solCombinationUniqeKey)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetSortNumByUniqeKey(), _battleSolCombinationData is Null");
			return this.NOT_EXIST;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_nCombinationUnique == solCombinationUniqeKey)
			{
				return current.m_sortNum;
			}
		}
		return this.NOT_EXIST;
	}

	public int GetUniqeKeyBySortNum(int solCombinationSortNum)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetUniqeKeyBySortNum(), _battleSolCombinationData is Null");
			return this.NOT_EXIST;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_sortNum == solCombinationSortNum)
			{
				return current.m_nCombinationUnique;
			}
		}
		return this.NOT_EXIST;
	}

	public string GetTextTitleKeyByUniqeKey(int uniqeKey)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetTextTitleKeyByUniqeKey(), _battleSolCombinationData is Null");
			return string.Empty;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_nCombinationUnique == uniqeKey)
			{
				return current.m_textTitleKey;
			}
		}
		return string.Empty;
	}

	public string GetTextDetailKeyByUniqeKey(int uniqeKey)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetTextDetailKeyByUniqeKey(), _battleSolCombinationData is Null");
			return string.Empty;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_nCombinationUnique == uniqeKey)
			{
				return current.m_textDetailKey;
			}
		}
		return string.Empty;
	}

	public SolCombinationInfo_Data GetSolCombinationDataByUniqeKey(int uniqeKey)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetSolCombinationDataByUniqeKey(), _battleSolCombinationData is Null");
			return null;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_nCombinationUnique == uniqeKey)
			{
				return current;
			}
		}
		return null;
	}

	public void SetShowHide(int uniqueKey, int bShow)
	{
		if (this._battleSolCombinationData == null)
		{
			Debug.LogError("ERROR, TableSolCombinationSkill.cs, GetSolCombinationDataByUniqeKey(), _battleSolCombinationData is Null");
			return;
		}
		foreach (SolCombinationInfo_Data current in this._battleSolCombinationData.Values)
		{
			if (current.m_nCombinationUnique == uniqueKey)
			{
				current.m_nCombinationIsShow = bShow;
			}
		}
	}
}
