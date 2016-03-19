using System;

public class QUEST_PARAM_CHECK
{
	public string strQuestUnique;

	public QUEST_CONNFY_INFO[] kQuestUpdateInfo = new QUEST_CONNFY_INFO[3];

	public QUEST_PARAM_CHECK()
	{
		for (byte b = 0; b < 3; b += 1)
		{
			this.kQuestUpdateInfo[(int)b] = new QUEST_CONNFY_INFO();
		}
	}

	public void Init()
	{
		this.strQuestUnique = string.Empty;
		for (byte b = 0; b < 3; b += 1)
		{
			this.kQuestUpdateInfo[(int)b].Init();
		}
	}
}
