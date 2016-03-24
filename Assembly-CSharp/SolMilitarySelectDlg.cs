using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolMilitarySelectDlg : Form
{
	public enum eSolSortType
	{
		SORTTYPE_MILITARY,
		SORTTYPE_READY,
		SORTTYPE_ALL
	}

	private enum eSolSortOrder
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

	private enum eSelectType
	{
		SELECTTYPE_NONE,
		SELECTTYPE_NORMAL,
		SELECTTYPE_ELEMENT,
		SELECTTYPE_MAX
	}

	public enum LoadType
	{
		NONE,
		EXPLORATION,
		SOLAWAKENING,
		SOLDETAIL,
		SOLMILITARYGROUP_HEROSETTING,
		SOLMILITARYGROUP_LEADERCHANGE,
		COMMUNITY
	}

	private SolMilitarySelectDlg.LoadType loadType;

	protected DropDownList SolSortTypeList;

	protected DropDownList SolSortOrderList;

	protected NewListBox SoldierList;

	private Button SolSelectConfirm;

	private Button m_btClose;

	private int m_nSearch_SolSortType = 1;

	private int m_nSearch_SolSortOrder;

	private int m_nSearch_MilitaryUnique;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolSortList = new List<NkSoldierInfo>();

	private Form m_pkParentDlg;

	private SolMilitarySelectDlg.eSelectType m_eSelectType = SolMilitarySelectDlg.eSelectType.SELECTTYPE_NORMAL;

	private int m_i32CharKind;

	private byte m_bCount;

	private long[] m_i64SelectSolID = new long[5];

	private bool m_SortwithoutHelpsol;

	public int SolSortType
	{
		set
		{
			this.m_nSearch_SolSortType = value;
		}
	}

	public bool SortwithoutHelpsol
	{
		get
		{
			return this.m_SortwithoutHelpsol;
		}
		set
		{
			this.m_SortwithoutHelpsol = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolMilitarySelect", G_ID.SOLMILITARYSELECT_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.SolSortTypeList = (base.GetControl("DropDownList_01") as DropDownList);
		this.SolSortTypeList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortType));
		this.SolSortOrderList = (base.GetControl("DropDownList_02") as DropDownList);
		this.SolSortOrderList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSortOrder));
		this.SoldierList = (base.GetControl("NewListBox_MilitarySelect") as NewListBox);
		this.SoldierList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierSelect));
		this.SoldierList.AddDoubleClickDelegate(new EZValueChangedDelegate(this.OnClickSoldierSelectConfirmOK));
		this.SoldierList.AutoListBox = false;
		this.SolSelectConfirm = (base.GetControl("btn_ok") as Button);
		this.SolSelectConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierSelectConfirmOK));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.InitData();
		base.SetScreenCenter();
	}

	public override void InitData()
	{
		this.m_nSearch_SolSortType = 1;
		this.m_nSearch_SolSortOrder = PlayerPrefs.GetInt(NrPrefsKey.SOLMILITARYSELECT_SORT, 0);
		this.m_nSearch_MilitaryUnique = 0;
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.OnClose();
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		if (TsPlatform.IsWeb)
		{
		}
		this.m_pkParentDlg = pkTargetDlg;
		base.SetScreenCenter();
		this.m_eSelectType = SolMilitarySelectDlg.eSelectType.SELECTTYPE_NORMAL;
	}

	public void SetLegendElement(int i32CharKind, long[] i64SolID, byte bCount)
	{
		this.m_eSelectType = SolMilitarySelectDlg.eSelectType.SELECTTYPE_ELEMENT;
		this.m_i32CharKind = i32CharKind;
		this.m_bCount = bCount;
		this.m_i64SelectSolID = i64SolID;
		this.SolSortOrderList.SetVisible(false);
		this.loadType = SolMilitarySelectDlg.LoadType.SOLDETAIL;
	}

	public void Refresh()
	{
		this.SetData();
	}

	private void SetData()
	{
		if (this.m_eSelectType == SolMilitarySelectDlg.eSelectType.SELECTTYPE_NORMAL)
		{
			this.MakeSolListAndSort();
			this.SetSoldierList();
		}
		else if (this.m_eSelectType == SolMilitarySelectDlg.eSelectType.SELECTTYPE_ELEMENT)
		{
			this.ElementMakeSolListAndSort();
			this.SetSoldierElementList();
		}
	}

	private void SetSoldierList()
	{
		this.SoldierList.Clear();
		int i = 0;
		while (i < this.m_kSolSortList.Count)
		{
			if (this.m_pkParentDlg == null || this.m_pkParentDlg.WindowID != 117)
			{
				goto IL_6F;
			}
			if (!this.m_kSolSortList[i].IsInjuryStatus())
			{
				if (!NrTSingleton<ExplorationManager>.Instance.IsSolInfo(this.m_kSolSortList[i].GetSolID()))
				{
					goto IL_6F;
				}
			}
			IL_11F:
			i++;
			continue;
			IL_6F:
			if (this.m_pkParentDlg != null && this.m_pkParentDlg.WindowID == 382)
			{
				if (this.m_kSolSortList[i].IsLeader() || (int)this.m_kSolSortList[i].GetLevel() < COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_AWAKENING_LIMIT_LEVEL))
				{
					goto IL_11F;
				}
			}
			else if (this.m_kSolSortList[i].GetSolPosType() == 2 && (this.m_pkParentDlg == null || this.m_pkParentDlg.WindowID != 219))
			{
				goto IL_11F;
			}
			this.SetSolListInfo(this.m_kSolSortList[i]);
			goto IL_11F;
		}
		this.SoldierList.RepositionItems();
		this.SoldierList.SetSelectedItem(0);
	}

	private void SetSoldierElementList()
	{
		this.SoldierList.Clear();
		for (int i = 0; i < this.m_kSolSortList.Count; i++)
		{
			if (this.m_kSolSortList[i] != null)
			{
				if (!this.m_kSolSortList[i].IsCostumeEquip())
				{
					this.SetSolListInfo(this.m_kSolSortList[i]);
				}
			}
		}
		this.SoldierList.RepositionItems();
		this.SoldierList.SetSelectedItem(0);
	}

	protected void SetSolListInfo(NkSoldierInfo pkSolinfo)
	{
		long num = pkSolinfo.GetExp() - pkSolinfo.GetCurBaseExp();
		long num2 = pkSolinfo.GetNextExp() - pkSolinfo.GetCurBaseExp();
		float num3 = ((float)num2 - (float)pkSolinfo.GetRemainExp()) / (float)num2;
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		if (0f > num3)
		{
			num3 = 0f;
		}
		if (pkSolinfo.IsMaxLevel())
		{
			num3 = 1f;
		}
		string text = string.Empty;
		NewListItem newListItem = new NewListItem(this.SoldierList.ColumnNum, true, string.Empty);
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(pkSolinfo.GetCharKind(), pkSolinfo.GetGrade());
		newListItem.SetListItemData(9, false);
		if (eventHeroCharCode != null)
		{
			newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
			newListItem.SetListItemData(9, true);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade());
			if (legendFrame != null)
			{
				newListItem.SetListItemData(0, legendFrame, null, null, null);
			}
		}
		newListItem.SetListItemData(1, pkSolinfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade(), pkSolinfo.GetName());
		newListItem.SetListItemData(2, legendName, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
			"count1",
			pkSolinfo.GetLevel().ToString(),
			"count2",
			pkSolinfo.GetSolMaxLevel().ToString()
		});
		newListItem.SetListItemData(3, text, null, null, null);
		int num4 = pkSolinfo.GetEquipWeaponOrigin();
		if (num4 > 0)
		{
			newListItem.SetListItemData(4, "Win_I_Weapon" + num4.ToString(), null, null, null);
		}
		num4 = pkSolinfo.GetEquipWeaponExtention();
		if (num4 > 0)
		{
			newListItem.SetListItemData(4, "Win_I_Weapon" + num4.ToString(), null, null, null);
		}
		if (pkSolinfo.IsAwakening())
		{
			newListItem.SetListItemData(5, "Win_I_DarkAlchemy", null, null, null);
		}
		else
		{
			newListItem.SetListItemData(5, false);
		}
		newListItem.SetListItemData(7, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_T_GauWaPr4"), 250f * num3, null, null);
		if (pkSolinfo.IsMaxLevel())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
				"exp",
				num.ToString(),
				"maxexp",
				num2.ToString()
			});
		}
		newListItem.SetListItemData(8, text, null, null, null);
		newListItem.Data = pkSolinfo;
		this.SoldierList.Add(newListItem);
	}

	public void SetSortList()
	{
		this.SolSortTypeList.Clear();
		this.SolSortOrderList.Clear();
		if (this.m_nSearch_SolSortType == 1)
		{
			this.SolSortTypeList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1229"), 0);
		}
		else
		{
			this.SolSortTypeList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1424"), 10);
		}
		this.SolSortTypeList.SetViewArea(this.SolSortTypeList.Count);
		this.SolSortTypeList.RepositionItems();
		this.SolSortTypeList.SetFirstItem();
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1886"), 7);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1887"), 8);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1888"), 2);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1889"), 3);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"), 4);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"), 5);
		this.SolSortOrderList.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"), 6);
		this.SolSortOrderList.SetViewArea(this.SolSortOrderList.Count);
		this.SolSortOrderList.RepositionItems();
		this.SolSortOrderList.SetFirstItem();
		this.OnChangeSortType(this.SolSortTypeList);
		this.OnChangeSortOrder(this.SolSortOrderList);
	}

	private void MakeAllSolList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			bool flag = true;
			if (this.m_pkParentDlg != null && this.m_pkParentDlg.WindowID == 382)
			{
				flag = false;
			}
			if (!flag || this.loadType == SolMilitarySelectDlg.LoadType.SOLMILITARYGROUP_LEADERCHANGE || nkSoldierInfo.GetFriendPersonID() <= 0L)
			{
				this.AddSolList(nkSoldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE);
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolPosType() != 6)
			{
				this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
			}
		}
	}

	private void MakeReadySolList()
	{
		if (this.SortwithoutHelpsol)
		{
			this.SolSortTypeList.SetVisible(false);
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int i = 0; i < kSolInfo.Length; i++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[i];
				if (!nkSoldierInfo.IsLeader())
				{
					if (nkSoldierInfo.GetFriendPersonID() <= 0L)
					{
						this.AddSolList(nkSoldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE);
					}
				}
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (!this.SortwithoutHelpsol || current.GetFriendPersonID() <= 0L)
			{
				if (current.GetSolPosType() != 6)
				{
					this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
				}
			}
		}
	}

	private void ElementMakeReadySolList()
	{
		this.SolSortTypeList.SetVisible(false);
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEGEND_ADVENT_HERO);
		if (num < 0)
		{
			num = 0;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(num))
		{
			num = 0;
		}
		if (NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(this.m_i32CharKind))
		{
			this.m_i32CharKind = 0;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolPosType() != 6)
			{
				if (!current.IsAtbCommonFlag(1L))
				{
					if (this.m_i32CharKind > 0 && this.m_i32CharKind == current.GetCharKind())
					{
						bool flag = true;
						for (int i = 0; i < 5; i++)
						{
							if (this.m_i64SelectSolID[i] == current.GetSolID())
							{
								flag = false;
								break;
							}
						}
						if (current.GetLegendType() > 0)
						{
							continue;
						}
						if (current.GetGrade() > 5)
						{
							continue;
						}
						if (flag)
						{
							this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
						}
					}
					if (num > 0 && num == current.GetCharKind())
					{
						bool flag = true;
						for (int j = 0; j < 5; j++)
						{
							if (this.m_i64SelectSolID[j] == current.GetSolID())
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY);
						}
					}
				}
			}
		}
	}

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLDETAIL_DLG) && pkSolinfo.GetSolID() == NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID())
		{
			return;
		}
		if (eAddPosType == eSOL_POSTYPE.SOLPOS_BATTLE)
		{
			if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
			{
				return;
			}
		}
		else if (pkSolinfo.GetSolPosType() != 0 && pkSolinfo.GetSolPosType() != 2)
		{
			return;
		}
		this.m_kSolList.Add(pkSolinfo);
	}

	private void ElementMakeSolListAndSort()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		this.m_kSolList.Clear();
		this.m_kSolSortList.Clear();
		this.ElementMakeReadySolList();
		if (this.m_kSolList.Count > 0)
		{
			this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
		}
		for (int i = 0; i < this.m_kSolList.Count; i++)
		{
			this.m_kSolSortList.Add(this.m_kSolList[i]);
		}
	}

	private void MakeSolListAndSort()
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		this.m_kSolList.Clear();
		this.m_kSolSortList.Clear();
		int nSearch_SolSortType = this.m_nSearch_SolSortType;
		if (nSearch_SolSortType != 1)
		{
			if (nSearch_SolSortType == 2)
			{
				this.MakeAllSolList();
			}
		}
		else
		{
			this.MakeReadySolList();
		}
		if (this.m_kSolList.Count > 0)
		{
			switch (this.m_nSearch_SolSortOrder)
			{
			case 0:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPowerDESC));
				break;
			case 1:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPowerASC));
				break;
			case 2:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelDESC));
				break;
			case 3:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelASC));
				break;
			case 4:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
				break;
			case 5:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeDESC));
				break;
			case 6:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeASC));
				break;
			case 7:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareFightPowerDESC));
				break;
			case 8:
				this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareFightPowerASC));
				break;
			}
			for (int i = 0; i < this.m_kSolList.Count; i++)
			{
				this.m_kSolSortList.Add(this.m_kSolList[i]);
			}
		}
	}

	private int CompareCombatPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int CompareCombatPowerASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetCombatPower().CompareTo(b.GetCombatPower());
	}

	private int CompareLevelDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private int CompareLevelASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (a.GetName().Equals(b.GetName()))
		{
			return this.CompareLevelDESC(a, b);
		}
		return a.GetName().CompareTo(b.GetName());
	}

	private int CompareGradeDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_High(a, b);
	}

	private int CompareGradeASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_Low(a, b);
	}

	private int CompareFightPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetFightPower().CompareTo(a.GetFightPower());
	}

	private int CompareFightPowerASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetFightPower().CompareTo(b.GetFightPower());
	}

	protected virtual void OnChangeSortType(IUIObject obj)
	{
		this.m_nSearch_SolSortType = 2;
		if (this.SolSortTypeList.Count > 0 && this.SolSortTypeList.SelectedItem != null)
		{
			ListItem listItem = this.SolSortTypeList.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_MilitaryUnique = (int)listItem.Key;
				if (this.m_nSearch_MilitaryUnique == 0)
				{
					this.m_nSearch_SolSortType = 1;
				}
			}
		}
		this.SetData();
	}

	private void OnChangeSortOrder(IUIObject obj)
	{
		this.m_nSearch_SolSortOrder = 0;
		if (this.SolSortOrderList.Count > 0 && this.SolSortOrderList.SelectedItem != null)
		{
			ListItem listItem = this.SolSortOrderList.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_nSearch_SolSortOrder = (int)listItem.Key;
			}
		}
		PlayerPrefs.SetInt(NrPrefsKey.SOLMILITARYSELECT_SORT, this.m_nSearch_SolSortOrder);
		this.SetData();
	}

	private void OnClickSoldierSelect(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
	}

	protected virtual void OnClickSoldierSelectConfirmOK(IUIObject obj)
	{
		if (!this.OnClickSoldierSelectConfirm())
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = this.SoldierList.SelectedItem.Data as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		switch (this.loadType)
		{
		case SolMilitarySelectDlg.LoadType.EXPLORATION:
			this.ClickConfirm_Exploration(nkSoldierInfo);
			break;
		case SolMilitarySelectDlg.LoadType.SOLAWAKENING:
			this.ClickConfirm_SolAwakening(nkSoldierInfo);
			break;
		case SolMilitarySelectDlg.LoadType.SOLDETAIL:
			this.ClickConfirm_SolDetail(nkSoldierInfo);
			break;
		case SolMilitarySelectDlg.LoadType.SOLMILITARYGROUP_HEROSETTING:
			this.ClickConfirm_SolmilitaryGroupHeroSetting(nkSoldierInfo);
			break;
		case SolMilitarySelectDlg.LoadType.SOLMILITARYGROUP_LEADERCHANGE:
			this.ClickConfirm_SolMilitaryGroup_LeaderChange(nkSoldierInfo);
			break;
		case SolMilitarySelectDlg.LoadType.COMMUNITY:
			this.ClickConfirm_Community(nkSoldierInfo);
			break;
		}
		base.CloseNow();
	}

	private bool OnClickSoldierSelectConfirm()
	{
		if (null == this.SolSortOrderList.SelectedItem || null == this.SoldierList.SelectedItem)
		{
			this.CloseForm(null);
			return false;
		}
		if (this.m_pkParentDlg == null || !this.m_pkParentDlg.Visible)
		{
			this.CloseForm(null);
			return false;
		}
		return true;
	}

	private void ClickConfirm_Exploration(NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo.IsInjuryStatus())
		{
			return;
		}
		ExplorationDlg explorationDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPLORATION_DLG) as ExplorationDlg;
		if (explorationDlg != null)
		{
			NrTSingleton<ExplorationManager>.Instance.AddCheckSolInfo(pkSolinfo);
			NrTSingleton<ExplorationManager>.Instance.SortSolInfo();
			explorationDlg.SetSolList();
		}
		this.Refresh();
	}

	private void ClickConfirm_SolAwakening(NkSoldierInfo pkSolinfo)
	{
		SolAwakeningDlg solAwakeningDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLAWAKENING_DLG) as SolAwakeningDlg;
		if (solAwakeningDlg != null)
		{
			solAwakeningDlg.SelelctSoldier(ref pkSolinfo);
		}
	}

	private void ClickConfirm_SolDetail(NkSoldierInfo pkSolinfo)
	{
		Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
		if (myth_Legend_Info_DLG != null)
		{
			myth_Legend_Info_DLG.AddLegendElement(pkSolinfo, (int)this.m_bCount);
			this.CloseForm(null);
		}
	}

	private void ClickConfirm_SolmilitaryGroupHeroSetting(NkSoldierInfo pkSolinfo)
	{
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.ToBattleOrMilitary(ref pkSolinfo);
		}
	}

	private void ClickConfirm_SolMilitaryGroup_LeaderChange(NkSoldierInfo pkSolinfo)
	{
		long num = (long)((int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FACE_SOLINDEX));
		if (num == pkSolinfo.GetSolID())
		{
			return;
		}
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg == null)
		{
			return;
		}
		GS_CHARACTER_SUBDATA_REQ gS_CHARACTER_SUBDATA_REQ = new GS_CHARACTER_SUBDATA_REQ();
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataType = 0;
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataValue = pkSolinfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_SUBDATA_REQ, gS_CHARACTER_SUBDATA_REQ);
		solMilitaryGroupDlg.SetLaderChangeInfo(pkSolinfo.GetCharKind());
	}

	private void ClickConfirm_Community(NkSoldierInfo pkSolinfo)
	{
		CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
		if (communityUI_DLG != null && communityUI_DLG.Visible)
		{
			communityUI_DLG.SetSelectHelpSol(pkSolinfo);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "SUPPORT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			this.CloseForm(null);
		}
	}

	private void ClickConfirm_None(IUIObject obj)
	{
		this.CloseForm(null);
	}

	public void SetLoadType(SolMilitarySelectDlg.LoadType _loadType)
	{
		this.loadType = _loadType;
	}

	public void CloseByParent(int pkParentDlgID)
	{
		if (this.m_pkParentDlg != null && this.m_pkParentDlg.WindowID == pkParentDlgID)
		{
			this.CloseForm(null);
		}
	}
}
