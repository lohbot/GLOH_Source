using System;
using System.Collections.Generic;

public class QUEST_AUTO_PATH_POS
{
	public int m_nMapIndex;

	public Dictionary<int, QUEST_AUTO_PATH_POS_SECID> m_dicQuestAutoPathPosSecID = new Dictionary<int, QUEST_AUTO_PATH_POS_SECID>();

	public bool Add(QUEST_AUTO_PATH_POS_SECID kData)
	{
		if (this.m_dicQuestAutoPathPosSecID.ContainsKey(kData.m_nMapIndex))
		{
			return false;
		}
		this.m_dicQuestAutoPathPosSecID.Add(kData.m_nMapIndex, kData);
		return true;
	}

	public QUEST_AUTO_PATH_POS_SECID GetData(int mapindex)
	{
		if (this.m_dicQuestAutoPathPosSecID.ContainsKey(mapindex))
		{
			return this.m_dicQuestAutoPathPosSecID[mapindex];
		}
		return null;
	}
}
