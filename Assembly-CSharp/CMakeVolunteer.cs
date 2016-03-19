using System;

public class CMakeVolunteer : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64PramVal)
	{
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
	}
}
