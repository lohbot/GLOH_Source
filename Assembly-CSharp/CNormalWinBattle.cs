using System;

public class CNormalWinBattle : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			name,
			"count2",
			base.GetParamVal().ToString(),
			"count1",
			i64ParamVal.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
