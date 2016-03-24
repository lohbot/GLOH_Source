using System;
using System.Collections.Generic;

public class ITEMSKILLREINFORCE_Manager : NrTSingleton<ITEMSKILLREINFORCE_Manager>
{
	private Dictionary<int, Dictionary<int, ITEMSKILLREINFORCE>> mHash_ItemSkillReinforceData;

	private ITEMSKILLREINFORCE_Manager()
	{
		this.mHash_ItemSkillReinforceData = new Dictionary<int, Dictionary<int, ITEMSKILLREINFORCE>>();
	}

	public void SetItemSkillReinforceData(ITEMSKILLREINFORCE skillReinforceData)
	{
		if (!this.mHash_ItemSkillReinforceData.ContainsKey(skillReinforceData.nGroupUnique))
		{
			this.mHash_ItemSkillReinforceData.Add(skillReinforceData.nGroupUnique, new Dictionary<int, ITEMSKILLREINFORCE>());
			if (!this.mHash_ItemSkillReinforceData[skillReinforceData.nGroupUnique].ContainsKey(skillReinforceData.nItemSkillLevel))
			{
				this.mHash_ItemSkillReinforceData[skillReinforceData.nGroupUnique].Add(skillReinforceData.nItemSkillLevel, skillReinforceData);
				return;
			}
		}
		for (int i = skillReinforceData.nItemSkillLevel; i < 30; i++)
		{
			ITEMSKILLREINFORCE itemskillReinforceData = this.GetItemskillReinforceData(skillReinforceData.nGroupUnique, i - 1);
			if (itemskillReinforceData != null)
			{
				this.AddItemSkillReinforceData(skillReinforceData, i);
			}
		}
	}

	public void AddItemSkillReinforceData(ITEMSKILLREINFORCE skillReinforceData, int SkillLevelIndex)
	{
		ITEMSKILLREINFORCE itemskillReinforceData = this.GetItemskillReinforceData(skillReinforceData.nGroupUnique, SkillLevelIndex - 1);
		if (itemskillReinforceData != null)
		{
			ITEMSKILLREINFORCE itemskillReinforceData2 = this.GetItemskillReinforceData(skillReinforceData.nGroupUnique, SkillLevelIndex);
			ITEMSKILLREINFORCE iTEMSKILLREINFORCE = new ITEMSKILLREINFORCE();
			iTEMSKILLREINFORCE.Set(itemskillReinforceData);
			iTEMSKILLREINFORCE.Add(SkillLevelIndex, skillReinforceData);
			if (itemskillReinforceData2 == null)
			{
				this.mHash_ItemSkillReinforceData[skillReinforceData.nGroupUnique].Add(SkillLevelIndex, iTEMSKILLREINFORCE);
			}
			else
			{
				itemskillReinforceData2.Set(iTEMSKILLREINFORCE);
			}
		}
	}

	public ITEMSKILLREINFORCE GetItemskillReinforceData(int GroupUnique, int ItemSkillLevel)
	{
		if (this.mHash_ItemSkillReinforceData.ContainsKey(GroupUnique))
		{
			foreach (KeyValuePair<int, ITEMSKILLREINFORCE> current in this.mHash_ItemSkillReinforceData[GroupUnique])
			{
				ITEMSKILLREINFORCE value = current.Value;
				if (value.nItemSkillLevel == ItemSkillLevel)
				{
					return value;
				}
			}
		}
		return null;
	}
}
