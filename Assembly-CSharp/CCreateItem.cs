using System;

public class CCreateItem : CQuestCondition
{
	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string empty = string.Empty;
		if (0L < base.GetParam())
		{
			string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code,
				"targetname",
				itemNameByItemUnique,
				"count1",
				i64ParamVal.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		else
		{
			string textFromQuest_Code2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromQuest_Code2,
				"count1",
				i64ParamVal.ToString(),
				"count2",
				base.GetParamVal().ToString()
			});
		}
		return empty;
	}
}
