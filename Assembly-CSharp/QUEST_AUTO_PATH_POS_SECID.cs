using System;
using System.Collections.Generic;

public class QUEST_AUTO_PATH_POS_SECID
{
	public int m_nMapIndex;

	private Dictionary<string, QUEST_AUTO_PATH_POS_CHARCODE> m_dicQuestAutoPathPosCharCode = new Dictionary<string, QUEST_AUTO_PATH_POS_CHARCODE>();

	public bool Add(QUEST_AUTO_PATH_POS_CHARCODE kData)
	{
		if (this.m_dicQuestAutoPathPosCharCode.ContainsKey(kData.strCharCode))
		{
			return false;
		}
		this.m_dicQuestAutoPathPosCharCode.Add(kData.strCharCode, kData);
		return true;
	}

	public QUEST_AUTO_PATH_POS_CHARCODE GetData(string CharCode)
	{
		if (this.m_dicQuestAutoPathPosCharCode.ContainsKey(CharCode))
		{
			return this.m_dicQuestAutoPathPosCharCode[CharCode];
		}
		return null;
	}
}
