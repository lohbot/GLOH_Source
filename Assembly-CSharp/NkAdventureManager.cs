using System;
using System.Collections.Generic;

public class NkAdventureManager : NrTSingleton<NkAdventureManager>
{
	private Dictionary<int, Adventure> m_kAdventureInfo = new Dictionary<int, Adventure>();

	private NkAdventureManager()
	{
	}

	public int TotalCount()
	{
		return this.m_kAdventureInfo.Count;
	}

	public bool AddAdventure(Adventure adventure)
	{
		if (this.m_kAdventureInfo.ContainsKey(adventure.GetAdventureUnique()))
		{
			return false;
		}
		this.m_kAdventureInfo.Add(adventure.GetAdventureUnique(), adventure);
		return true;
	}

	public Adventure GetAdventureFromUnique(int unique)
	{
		if (this.m_kAdventureInfo.ContainsKey(unique))
		{
			return this.m_kAdventureInfo[unique];
		}
		return null;
	}

	public Adventure GetCurrentAdventure(ref int index)
	{
		int num = 0;
		Adventure result = null;
		foreach (Adventure current in this.m_kAdventureInfo.Values)
		{
			foreach (Adventure.AdventureInfo current2 in current.m_kAdventureInfo)
			{
				if (NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(current2.questGroupUnique) == QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
				{
					index = num;
					result = current;
				}
				else
				{
					CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(current2.questGroupUnique);
					if (questGroupByGroupUnique != null)
					{
						CQuest cQuest = questGroupByGroupUnique.FindCurrentQuest();
						if (cQuest != null)
						{
							QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(cQuest.GetQuestUnique());
							if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE || questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING || questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
							{
								index = num;
								return current;
							}
						}
					}
				}
			}
			num++;
		}
		return result;
	}

	public Adventure GetAdventureFromIndex(int index)
	{
		int num = 0;
		foreach (Adventure current in this.m_kAdventureInfo.Values)
		{
			if (num == index)
			{
				return current;
			}
			num++;
		}
		return null;
	}

	public bool IsAcceptQuest()
	{
		foreach (Adventure current in this.m_kAdventureInfo.Values)
		{
			foreach (Adventure.AdventureInfo current2 in current.m_kAdventureInfo)
			{
				if (NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(current2.questGroupUnique) != QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
				{
					CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(current2.questGroupUnique);
					if (questGroupByGroupUnique != null)
					{
						CQuest cQuest = questGroupByGroupUnique.FindCurrentQuest();
						if (cQuest != null)
						{
							QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(cQuest.GetQuestUnique());
							if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE && NrTSingleton<NkQuestManager>.Instance.GetCurrentQuestCount() == 0)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}
}
