using System;

public class EventArgs_QuestClose : EventArgs
{
	public string m_QuestUnique = string.Empty;

	public void Set(string QuestUnique)
	{
		this.m_QuestUnique = QuestUnique;
	}
}
