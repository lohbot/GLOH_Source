using System;

public class CAmassItemUse : CQuestCondition
{
	private int m_nItemUnique;

	public override string GetConditionText(long i64ParamVal)
	{
		if (NrTSingleton<NkQuestManager>.Instance == null)
		{
			return string.Empty;
		}
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nItemUnique);
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname1",
			itemNameByItemUnique,
			"targetname2",
			name,
			"count1",
			i64ParamVal.ToString(),
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}

	public void SetItemInfo(int nItemUnique, int i32ItemNum)
	{
		this.m_nItemUnique = nItemUnique;
	}
}
