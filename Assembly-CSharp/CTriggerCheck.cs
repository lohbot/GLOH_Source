using System;

public class CTriggerCheck : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string questConditionText = NrTSingleton<NrEventTriggerInfoManager>.Instance.GetQuestConditionText((int)base.GetParam());
		if (questConditionText == string.Empty)
		{
			TsLog.LogWarning("strConText == string.Empty GetParam = {0}", new object[]
			{
				base.GetParam()
			});
		}
		return questConditionText;
	}
}
