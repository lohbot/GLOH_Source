using System;
using System.Collections.Generic;

public class BattleSReward_Manager : NrTSingleton<BattleSReward_Manager>
{
	private Dictionary<int, BATTLE_SREWARD> m_dicBattleSReward;

	private Dictionary<int, BATTLE_BABEL_SREWARD> m_dicBabelBattleSReward;

	private NkValueParse<int> m_kBattleSRewardTypeInfo;

	private BattleSReward_Manager()
	{
		this.m_dicBattleSReward = new Dictionary<int, BATTLE_SREWARD>();
		this.m_dicBabelBattleSReward = new Dictionary<int, BATTLE_BABEL_SREWARD>();
		this.m_kBattleSRewardTypeInfo = new NkValueParse<int>();
		this.SetParaDataCode();
	}

	public void SetParaDataCode()
	{
		this.m_kBattleSRewardTypeInfo.InsertCodeValue("EXP", 0);
		this.m_kBattleSRewardTypeInfo.InsertCodeValue("ITEM", 1);
		this.m_kBattleSRewardTypeInfo.InsertCodeValue("GOLD", 2);
	}

	public void SetBattleSRewardData(BATTLE_SREWARD SRewardData)
	{
		if (!this.m_dicBattleSReward.ContainsKey(SRewardData.m_nRewardUnique))
		{
			this.m_dicBattleSReward.Add(SRewardData.m_nRewardUnique, SRewardData);
		}
	}

	public void SetBabelSpecialReward(BATTLE_BABEL_SREWARD SRewardData)
	{
		if (!this.m_dicBabelBattleSReward.ContainsKey(SRewardData.m_nRewardUnique))
		{
			this.m_dicBabelBattleSReward.Add(SRewardData.m_nRewardUnique, SRewardData);
		}
	}

	public int GetSRewardType(string Type)
	{
		return this.m_kBattleSRewardTypeInfo.GetValue(Type);
	}

	public BATTLE_SREWARD GetBattleSRewardData(int RewardUnique)
	{
		if (this.m_dicBattleSReward.ContainsKey(RewardUnique))
		{
			return this.m_dicBattleSReward[RewardUnique];
		}
		return null;
	}

	public BATTLE_BABEL_SREWARD GetBattleBabelSRewardData(int RewardUnique)
	{
		if (this.m_dicBabelBattleSReward.ContainsKey(RewardUnique))
		{
			return this.m_dicBabelBattleSReward[RewardUnique];
		}
		return null;
	}
}
