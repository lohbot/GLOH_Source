using System;

public class QUEST_CONNFY_INFO
{
	public int i32QuestCode;

	public long i64Param;

	public long i64PreParamVal;

	public bool bComplete;

	public void Init()
	{
		this.i32QuestCode = 0;
		this.i64Param = 0L;
		this.i64PreParamVal = 0L;
		this.bComplete = false;
	}
}
