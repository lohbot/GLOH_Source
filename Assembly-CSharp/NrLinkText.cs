using System;
using UnityForms;

public class NrLinkText
{
	public static string NpcName(string strName)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("{@N", strName, "}");
	}

	public static string PlayerName(string strName)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("{@P", strName, "}");
	}

	public static string ItemName(string strName)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("{@I", strName, "}");
	}

	public static string PlunDerReplayName(long unique)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("[PB", unique.ToString(), "]");
	}

	public static string ColosseumReplayName(long unique)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("[CB", unique.ToString(), "]");
	}

	public static string MineReplayName(long unique)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("[MB", unique.ToString(), "]");
	}

	public static string CouponName(string text)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("[CP:", text.Trim(), "]");
	}

	public static string InfiBattleReplayName(long unique)
	{
		return NrTSingleton<UIDataManager>.Instance.GetString("[IB", unique.ToString(), "]");
	}
}
