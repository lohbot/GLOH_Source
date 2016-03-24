using System;
using System.Collections.Generic;

public class UIHelper
{
	public enum eSolSortOrder
	{
		SORTORDER_CPOWERDESC,
		SORTORDER_CPOWERASC,
		SORTORDER_LEVELDESC,
		SORTORDER_LEVELASC,
		SORTORDER_NAME,
		SORTORDER_GRADEDESC,
		SORTORDER_GRADEASC,
		SORTORDER_FIGHTINGPOWERDESC,
		SORTORDER_FIGHTINGPOWERASC
	}

	public static void SetSolSort(int iSolSortOrder, List<UIListItemContainer> solList)
	{
		if (solList.Count > 0)
		{
			switch (iSolSortOrder)
			{
			case 0:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareCombatPowerDESC2));
				break;
			case 1:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareCombatPowerASC2));
				break;
			case 2:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareLevelDESC2));
				break;
			case 3:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareLevelASC2));
				break;
			case 4:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareName2));
				break;
			case 5:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareGradeDESC2));
				break;
			case 6:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareGradeASC2));
				break;
			case 7:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareFightPowerDESC2));
				break;
			case 8:
				solList.Sort(new Comparison<UIListItemContainer>(UIHelper.CompareFightPowerASC2));
				break;
			}
		}
	}

	private static int CompareLevelDESC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo2.GetLevel().CompareTo(nkSoldierInfo.GetLevel());
	}

	private static int CompareLevelASC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo.GetLevel().CompareTo(nkSoldierInfo2.GetLevel());
	}

	private static int CompareName2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		if (nkSoldierInfo.GetName().Equals(nkSoldierInfo2.GetName()))
		{
			return UIHelper.CompareLevelDESC2(l, r);
		}
		return nkSoldierInfo.GetName().CompareTo(nkSoldierInfo2.GetName());
	}

	private static int CompareGradeDESC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo a = (NkSoldierInfo)l.Data;
		NkSoldierInfo b = (NkSoldierInfo)r.Data;
		return SolComposeListDlg.CompareGrade_High(a, b);
	}

	private static int CompareGradeASC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo a = (NkSoldierInfo)l.Data;
		NkSoldierInfo b = (NkSoldierInfo)r.Data;
		return SolComposeListDlg.CompareGrade_Low(a, b);
	}

	private static int CompareFightPowerDESC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo2.GetFightPower().CompareTo(nkSoldierInfo.GetFightPower());
	}

	private static int CompareFightPowerASC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo.GetFightPower().CompareTo(nkSoldierInfo2.GetFightPower());
	}

	private static int CompareCombatPowerDESC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo2.GetCombatPower().CompareTo(nkSoldierInfo.GetCombatPower());
	}

	private static int CompareCombatPowerASC2(UIListItemContainer l, UIListItemContainer r)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)l.Data;
		NkSoldierInfo nkSoldierInfo2 = (NkSoldierInfo)r.Data;
		return nkSoldierInfo.GetCombatPower().CompareTo((long)nkSoldierInfo2.GetFightPower());
	}
}
