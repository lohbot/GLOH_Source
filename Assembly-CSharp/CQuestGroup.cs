using System;

public class CQuestGroup
{
	private QUEST_GROUP_INFO m_stQuestGroupInfo;

	~CQuestGroup()
	{
	}

	public QUEST_GROUP_INFO GetGroupInfo()
	{
		return this.m_stQuestGroupInfo;
	}

	public int GetGroupUnique()
	{
		return this.m_stQuestGroupInfo.m_i32QuestGroupUniuque;
	}

	public short GetPageUnique()
	{
		return this.m_stQuestGroupInfo.m_i16QuestPageUnique;
	}

	public short GetChapterUnique()
	{
		return this.m_stQuestGroupInfo.m_i16QuestChapterUnique;
	}

	public void SetQuestGroupInfo(QUEST_GROUP_INFO QuestGroupInfo)
	{
		this.m_stQuestGroupInfo = QuestGroupInfo;
	}

	public void SortingQuestInGroup()
	{
		CQuest cQuest = null;
		QUEST_SORTID qUEST_SORTID = null;
		for (int i = 0; i < 200; i++)
		{
			if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(i))
			{
				QUEST_SORTID qUEST_SORTID2 = this.m_stQuestGroupInfo.m_QuestUniqueBit[i];
				if (qUEST_SORTID2 != null)
				{
					cQuest = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID2.m_strQuestUnique);
					if (cQuest != null)
					{
						CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(cQuest.GetQuestCommon().strPreQuestUnique);
						if (questByQuestUnique != null)
						{
							CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(cQuest.GetQuestUnique());
							CQuestGroup questGroupByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(questByQuestUnique.GetQuestUnique());
							if (questGroupByQuestUnique != null && questGroupByQuestUnique2 != null && questGroupByQuestUnique.GetGroupUnique() != questGroupByQuestUnique2.GetGroupUnique() && questGroupByQuestUnique.GetGroupSort() > questGroupByQuestUnique2.GetGroupSort() && this.m_stQuestGroupInfo.m_QuestUnique.ContainsKey(qUEST_SORTID2.m_strQuestUnique))
							{
								qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUnique[qUEST_SORTID2.m_strQuestUnique];
								qUEST_SORTID.m_i32QuestSort = 1;
								break;
							}
						}
						else if (questByQuestUnique == null && this.m_stQuestGroupInfo.m_QuestUnique.ContainsKey(qUEST_SORTID2.m_strQuestUnique))
						{
							qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUnique[qUEST_SORTID2.m_strQuestUnique];
							qUEST_SORTID.m_i32QuestSort = 1;
							break;
						}
					}
				}
			}
		}
		if (qUEST_SORTID == null || cQuest == null)
		{
			return;
		}
		int num = 2;
		for (int j = 0; j < 200; j++)
		{
			CQuest questByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
			if (questByQuestUnique2 != null)
			{
				if (this.m_stQuestGroupInfo.m_QuestUnique.ContainsKey(questByQuestUnique2.GetQuestCommon().strNextQuestUnique))
				{
					QUEST_SORTID qUEST_SORTID3 = this.m_stQuestGroupInfo.m_QuestUnique[questByQuestUnique2.GetQuestCommon().strNextQuestUnique];
					if (qUEST_SORTID3 != null)
					{
						qUEST_SORTID3.m_i32QuestSort = num;
						num++;
						qUEST_SORTID = qUEST_SORTID3;
					}
				}
			}
		}
		this.m_stQuestGroupInfo.m_QuestList.Sort(new Comparison<QUEST_SORTID>(NkQuestManager.AscendingQuestNum));
	}

	public string GetQuestUniqueByBit(int bBit)
	{
		if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(bBit))
		{
			return this.m_stQuestGroupInfo.m_QuestUniqueBit[bBit].m_strQuestUnique;
		}
		return "0";
	}

	public int GetBitByQuestUnique(string strQuestUnique)
	{
		if (this.m_stQuestGroupInfo.m_QuestUnique.ContainsKey(strQuestUnique))
		{
			return this.m_stQuestGroupInfo.m_QuestUnique[strQuestUnique].m_i16Bit;
		}
		return -1;
	}

	public byte GetQuestCount()
	{
		return this.m_stQuestGroupInfo.m_byQuestCount;
	}

	public int GetGroupSortNum()
	{
		return this.m_stQuestGroupInfo.m_i32PageSort;
	}

	public string GetGroupTitle()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(this.m_stQuestGroupInfo.m_strTextKey);
	}

	public string GetPage()
	{
		return this.m_stQuestGroupInfo.m_i16QuestPageUnique.ToString();
	}

	public short GetPageNum()
	{
		return this.m_stQuestGroupInfo.m_i16QuestPageUnique;
	}

	public int GetGroupSort()
	{
		return this.m_stQuestGroupInfo.m_i32PageSort;
	}

	public long GetSubChapterUnique()
	{
		return (long)this.m_stQuestGroupInfo.m_i16SubChapterUnique;
	}

	public int GetGroupLevel()
	{
		if (!this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(0))
		{
			return 0;
		}
		QUEST_SORTID qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUniqueBit[0];
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
		if (questByQuestUnique != null)
		{
			return questByQuestUnique.GetQuestCommon().i32QuestLevel;
		}
		return 0;
	}

	public QUEST_SORTID GetQuestSortIDByBit(byte bBit)
	{
		if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey((int)bBit))
		{
			return this.m_stQuestGroupInfo.m_QuestUniqueBit[(int)bBit];
		}
		return null;
	}

	public QUEST_SORTID GetQuestSortIDByQuestUnique(string strQuestUnique)
	{
		if (this.m_stQuestGroupInfo.m_QuestUnique.ContainsKey(strQuestUnique))
		{
			return this.m_stQuestGroupInfo.m_QuestUnique[strQuestUnique];
		}
		return null;
	}

	public int GetQuestType()
	{
		return this.m_stQuestGroupInfo.m_nQuestType;
	}

	public bool IsFristQuest(string strQuestUnique)
	{
		for (int i = 0; i < 200; i++)
		{
			if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(i))
			{
				QUEST_SORTID qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUniqueBit[i];
				if (qUEST_SORTID != null)
				{
					if (qUEST_SORTID.m_strQuestUnique == strQuestUnique && qUEST_SORTID.m_i32QuestSort == 1)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public CQuest GetFirstQuest()
	{
		CQuest result = null;
		if (this.m_stQuestGroupInfo.m_QuestList.Count <= 0)
		{
			return null;
		}
		for (int i = 0; i < 200; i++)
		{
			QUEST_SORTID qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestList[i];
			if (qUEST_SORTID != null)
			{
				result = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
				break;
			}
		}
		return result;
	}

	public CQuest GetLastQuest()
	{
		if (this.m_stQuestGroupInfo.m_QuestList.Count <= 0)
		{
			return null;
		}
		for (int i = 0; i < 200; i++)
		{
			if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(i))
			{
				QUEST_SORTID qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUniqueBit[i];
				if (qUEST_SORTID != null)
				{
					CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
					CQuest questByQuestUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(questByQuestUnique.GetQuestCommon().strNextQuestUnique);
					if (questByQuestUnique.GetQuestGroupUnique() != questByQuestUnique2.GetQuestGroupUnique())
					{
						return questByQuestUnique2;
					}
				}
			}
		}
		return null;
	}

	public string GetCharpterPageString()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		return string.Concat(new string[]
		{
			this.GetChapterUnique().ToString(),
			empty3,
			" ",
			this.GetPage(),
			empty2
		});
	}

	public CQuest FindCurrentQuest()
	{
		CQuest result = null;
		for (int i = 0; i < 200; i++)
		{
			if (this.m_stQuestGroupInfo.m_QuestUniqueBit.ContainsKey(i))
			{
				QUEST_SORTID qUEST_SORTID = this.m_stQuestGroupInfo.m_QuestUniqueBit[i];
				if (qUEST_SORTID != null)
				{
					if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest(qUEST_SORTID.m_strQuestUnique))
					{
						return NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(qUEST_SORTID.m_strQuestUnique);
					}
				}
			}
		}
		return result;
	}
}
