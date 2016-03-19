using System;
using System.Collections.Generic;

public class USER_QUEST_INFO
{
	public Dictionary<int, USER_QUEST_COMPLETE_INFO> m_dicCompleteInfo = new Dictionary<int, USER_QUEST_COMPLETE_INFO>();

	public USER_CURRENT_QUEST_INFO[] stUserCurrentQuestInfo = new USER_CURRENT_QUEST_INFO[10];

	public List<USER_CURRENT_QUEST_INFO> mainList = new List<USER_CURRENT_QUEST_INFO>();

	public List<USER_CURRENT_QUEST_INFO> subList = new List<USER_CURRENT_QUEST_INFO>();

	public USER_QUEST_INFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_dicCompleteInfo.Clear();
		for (byte b = 0; b < 10; b += 1)
		{
			this.stUserCurrentQuestInfo[(int)b] = new USER_CURRENT_QUEST_INFO();
		}
		this.mainList.Clear();
		this.subList.Clear();
	}
}
