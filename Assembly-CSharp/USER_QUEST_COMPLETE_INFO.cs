using System;

public class USER_QUEST_COMPLETE_INFO
{
	public long i64QuestID;

	public int i32GroupUnique;

	public int i32LastGrade;

	public byte bCurrentGrade;

	public byte bCleared;

	public byte[] byCompleteQuest = new byte[25];

	public USER_QUEST_COMPLETE_INFO()
	{
		this.i64QuestID = 0L;
		this.i32GroupUnique = 0;
		this.i32LastGrade = 0;
		this.bCurrentGrade = 0;
	}
}
