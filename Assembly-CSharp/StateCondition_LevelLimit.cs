using System;

public class StateCondition_LevelLimit : EventTriggerItem_StateCondition
{
	public int m_MinLevel;

	public int m_MaxLevel;

	public override bool IsVaildValue()
	{
		return this.m_MinLevel != 0 || this.m_MaxLevel != 0;
	}

	public override bool Verify()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				int level = (int)soldierInfo.GetLevel();
				if (this.m_MinLevel <= level && level <= this.m_MaxLevel)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string GetComment()
	{
		return string.Concat(new object[]
		{
			"군주 레벨이 ",
			this.m_MinLevel,
			"이상 ",
			this.m_MaxLevel,
			"이하 면"
		});
	}
}
