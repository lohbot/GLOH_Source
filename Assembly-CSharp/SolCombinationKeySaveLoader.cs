using System;
using UnityEngine;

public class SolCombinationKeySaveLoader
{
	private static int NOT_SELECTED = -1;

	public static void SaveSolCombinationUniqeKeyInOS(string osKey, int solCombinationUniqeKey)
	{
		PlayerPrefs.SetString(osKey, NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetSortNumByUniqeKey(solCombinationUniqeKey).ToString());
	}

	public static int GetSolCombinationUniqeKeyInOS(string osKey)
	{
		string @string = PlayerPrefs.GetString(osKey);
		if (string.IsNullOrEmpty(@string))
		{
			return SolCombinationKeySaveLoader.NOT_SELECTED;
		}
		string solCombinationUniqeKeyBySortNum = SolCombinationKeySaveLoader.GetSolCombinationUniqeKeyBySortNum(int.Parse(@string));
		if (string.IsNullOrEmpty(solCombinationUniqeKeyBySortNum))
		{
			return SolCombinationKeySaveLoader.NOT_SELECTED;
		}
		return int.Parse(solCombinationUniqeKeyBySortNum);
	}

	private static string GetSolCombinationUniqeKeyBySortNum(int solCombinationSortNum)
	{
		return NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetUniqeKeyBySortNum(solCombinationSortNum).ToString();
	}
}
