using System;

public class CSolItemGradeCount : CQuestCondition
{
	private long m_i64MaxCount;

	public long I64MaxCount
	{
		get
		{
			return this.m_i64MaxCount;
		}
		set
		{
			this.m_i64MaxCount = value;
		}
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return string.Empty;
		}
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique((int)base.GetParam());
		if (NrTSingleton<ItemManager>.Instance.GetItemInfo((int)base.GetParam()) == null)
		{
			return string.Empty;
		}
		NrSoldierList kSoldierList = charPersonInfo.m_kSoldierList;
		if (kSoldierList == null)
		{
			return string.Empty;
		}
		int solItemRankCount = kSoldierList.GetSolItemRankCount((int)base.GetParam(), (int)base.GetParamVal());
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			itemNameByItemUnique,
			"count1",
			base.GetParamVal(),
			"count2",
			solItemRankCount,
			"count3",
			this.m_i64MaxCount
		});
		return empty;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		if (NrTSingleton<ItemManager>.Instance.GetItemInfo((int)base.GetParam()) == null)
		{
			return false;
		}
		NrSoldierList kSoldierList = charPersonInfo.m_kSoldierList;
		if (kSoldierList == null)
		{
			return false;
		}
		int solItemRankCount = kSoldierList.GetSolItemRankCount((int)base.GetParam(), (int)base.GetParamVal());
		i64ParamVal = (long)solItemRankCount;
		return (long)solItemRankCount >= this.m_i64MaxCount;
	}
}
