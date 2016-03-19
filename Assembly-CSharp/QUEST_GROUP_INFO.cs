using System;
using System.Collections.Generic;
using TsLibs;

public class QUEST_GROUP_INFO
{
	public int m_i32QuestGroupUniuque;

	public short m_i16QuestChapterUnique;

	public short m_i16QuestPageUnique;

	public short m_i16SubChapterUnique;

	public int m_i32PageSort;

	public string m_strTextKey = string.Empty;

	public byte m_byQuestCount;

	public int m_nQuestType;

	public bool bIsLimit;

	public Dictionary<string, QUEST_SORTID> m_QuestUnique = new Dictionary<string, QUEST_SORTID>();

	public Dictionary<int, QUEST_SORTID> m_QuestUniqueBit = new Dictionary<int, QUEST_SORTID>();

	public List<QUEST_SORTID> m_QuestList = new List<QUEST_SORTID>();

	public void SetData(TsDataReader.Row row)
	{
		int num = 0;
		row.GetColumn(num++, out this.m_i32QuestGroupUniuque);
		row.GetColumn(num++, out this.m_i16QuestChapterUnique);
		row.GetColumn(num++, out this.m_i16QuestPageUnique);
		row.GetColumn(num++, out this.m_i16SubChapterUnique);
		row.GetColumn(num++, out this.m_i32PageSort);
		this.m_strTextKey = "Title_Group_" + this.m_i32QuestGroupUniuque.ToString();
		string text = "0";
		int i32QuestSort = 999999;
		for (int i = 0; i < 200; i++)
		{
			row.GetColumn(num++, out text);
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(text) != null)
			{
				QUEST_SORTID qUEST_SORTID = new QUEST_SORTID();
				qUEST_SORTID.m_strQuestUnique = text;
				qUEST_SORTID.m_i32QuestSort = i32QuestSort;
				qUEST_SORTID.m_i16Bit = i;
				if (this.m_QuestUnique.ContainsKey(text))
				{
					string msg = string.Concat(new string[]
					{
						"그룹 유니크 : ",
						this.m_i32QuestGroupUniuque.ToString(),
						" 퀘스트",
						text,
						"중북입니다."
					});
					NrTSingleton<NrMainSystem>.Instance.Alert(msg);
				}
				else
				{
					this.m_QuestUnique.Add(qUEST_SORTID.m_strQuestUnique, qUEST_SORTID);
					if (!this.m_QuestUniqueBit.ContainsKey(i))
					{
						this.m_QuestUniqueBit.Add(i, qUEST_SORTID);
					}
					this.m_QuestList.Add(qUEST_SORTID);
					this.m_byQuestCount += 1;
				}
			}
		}
		row.GetColumn(num++, out this.m_nQuestType);
		bool flag = false;
		if (this.m_nQuestType == 100)
		{
			flag = true;
		}
		if (flag)
		{
			foreach (QUEST_SORTID current in this.m_QuestUnique.Values)
			{
				if (!current.m_strQuestUnique.Equals("0"))
				{
					this.m_byQuestCount += 1;
				}
			}
		}
	}
}
