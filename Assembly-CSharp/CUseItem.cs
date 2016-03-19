using System;

public class CUseItem : CQuestCondition
{
	public override string GetConditionText(long i64ParamVal)
	{
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
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
		return empty;
	}
}
