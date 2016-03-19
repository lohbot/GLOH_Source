using System;

public class CWinBattle : CQuestCondition
{
	public int m_nCharKind;

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		string text = string.Empty;
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(this.m_nCharKind);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"targetname",
			name
		});
		return empty;
	}
}
