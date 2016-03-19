using System;

public class CWorldMapWarp : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal()) || (base.GetParam() == 0L && i64ParamVal >= base.GetParamVal());
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName((int)base.GetParam());
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			mapName
		});
		return empty;
	}
}
