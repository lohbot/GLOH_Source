using System;
using UnityEngine;

public class SolCombination_RecentBattleStartInfoManager : NrTSingleton<SolCombination_RecentBattleStartInfoManager>
{
	private string _recentBattleSolCombinationUniqeKey = "_RECENT_SOL_COMBINATION_";

	private SolCombination_RecentBattleStartInfoManager()
	{
		this.InitBabelSolCombinationInfo();
	}

	public void SetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE battleRoomType, int solCombinationUniqeKey, int STARTPOS_INDEX)
	{
		Debug.Log(string.Concat(new object[]
		{
			"NORMAL, RecentBattleSolCombinationInfoManager.cs, SetRecentBattleSolCombinationInfo(), battleRoomType : ",
			battleRoomType.ToString(),
			", UniqeKey : ",
			solCombinationUniqeKey
		}));
		SolCombinationKeySaveLoader.SaveSolCombinationUniqeKeyInOS(this.GetOSKey(battleRoomType, STARTPOS_INDEX), solCombinationUniqeKey);
	}

	public int GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE battleRoomType, int STARTPOS_INDEX)
	{
		return SolCombinationKeySaveLoader.GetSolCombinationUniqeKeyInOS(this.GetOSKey(battleRoomType, STARTPOS_INDEX));
	}

	private void InitBabelSolCombinationInfo()
	{
		string key = eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER.ToString() + this._recentBattleSolCombinationUniqeKey;
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return;
		}
		string s = string.Empty;
		s = this.GetBabelBatchedSolCombination_TopUniqueKey();
		this.SetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER, int.Parse(s), 0);
	}

	private string GetBabelBatchedSolCombination_TopUniqueKey()
	{
		return NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCompleteCombinationTopUniqeKey(NrTSingleton<NkBabelMacroManager>.Instance.GetBatchedSolKindList()).ToString();
	}

	private string GetOSKey(eBATTLE_ROOMTYPE battleRoomType, int STARTPOS_INDEX)
	{
		string text = battleRoomType.ToString() + this._recentBattleSolCombinationUniqeKey + STARTPOS_INDEX.ToString();
		if (this.IsParty(battleRoomType))
		{
			text += "PARTY";
		}
		return text;
	}

	private bool IsParty(eBATTLE_ROOMTYPE battleRoomType)
	{
		return this.IsBabelParty(battleRoomType);
	}

	private bool IsBabelParty(eBATTLE_ROOMTYPE battleRoomType)
	{
		return battleRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && Battle.BATTLE != null && Battle.BabelPartyCount > 1;
	}
}
