using System;

public class CAmassItem : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		if (NrTSingleton<NkQuestManager>.Instance == null)
		{
			return string.Empty;
		}
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			name,
			"count1",
			i64ParamVal.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
