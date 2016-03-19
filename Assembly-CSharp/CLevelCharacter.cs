using System;

public class CLevelCharacter : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64PramVal)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		return nrCharUser != null && (long)nrCharUser.GetPersonInfo().GetLevel(0L) >= base.GetParamVal();
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"count1",
			base.GetParamVal()
		});
		return empty;
	}
}
