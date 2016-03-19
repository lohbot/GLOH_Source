using System;

public class COpenIventory : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string result = string.Empty;
		if (base.GetParam() == 0L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParam() == 1L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParam() == 2L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		else if (base.GetParam() == 3L)
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		}
		return result;
	}
}
