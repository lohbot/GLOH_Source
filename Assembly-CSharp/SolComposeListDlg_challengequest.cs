using System;
using System.Collections.Generic;
using UnityForms;

public class SolComposeListDlg_challengequest : SolComposeListDlg
{
	private NkSoldierInfo _selectedDummySolInfo;

	private SOLCOMPOSELIST_DLG_OPENTYPE _openType;

	private List<NkSoldierInfo> _selectedDummySoldierList;

	public SOLCOMPOSELIST_DLG_OPENTYPE _OpenType
	{
		get
		{
			return this._openType;
		}
		set
		{
			this._openType = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_solcomposelist", G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG, true);
		this._selectedDummySoldierList = new List<NkSoldierInfo>();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public void InitDummyList()
	{
		SolComposeListDlg.SOL_LIST_INSERT_TYPE iNSERT_TYPE = base.INSERT_TYPE;
		if (null != this.ddList1)
		{
			this.ddList1.Clear();
			this.ddList1.SetIndex(-1);
			this.ddList1.SetViewArea(0);
			this.ddList1.controlIsEnabled = false;
		}
		if (null != this.ddList2)
		{
			this.ddList2.Clear();
			this.ddList2.SetIndex(-1);
			this.ddList2.SetViewArea(0);
			this.ddList2.controlIsEnabled = false;
		}
		this.mSortList.Add(this.CreateDummySoldier());
		if (base.ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			this.mSortList.Add(this.CreateDummySoldier());
		}
		base.ChangeSortDDL(null);
	}

	protected override void BtnClickListBox(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.ComposeNewListBox.SelectedItem;
		if (selectedItem == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)selectedItem.data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		if (this._selectedDummySoldierList.Contains(nkSoldierInfo))
		{
			this._selectedDummySoldierList.Remove(nkSoldierInfo);
		}
		else
		{
			this._selectedDummySoldierList.Add(nkSoldierInfo);
		}
		this._selectedDummySolInfo = nkSoldierInfo;
		NewListItem newListItem = base.UpdateSolList(nkSoldierInfo);
		if (this._selectedDummySoldierList.Contains(nkSoldierInfo))
		{
			newListItem.SetListItemData(7, true);
			newListItem.SetListItemData(7, "Com_I_Check", null, null, null);
		}
		else
		{
			newListItem.SetListItemData(7, false);
			newListItem.SetListItemData(7, string.Empty, null, null, null);
		}
		newListItem.SetListItemData(9, false);
		this.ComposeNewListBox.UpdateContents(selectedItem.GetIndex(), newListItem);
	}

	protected override void ClickOk(IUIObject obj)
	{
		if (this._selectedDummySoldierList == null)
		{
			return;
		}
		bool baseSolSetting = 0 < this._selectedDummySoldierList.Count;
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = (SolComposeMainDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG);
		if (solComposeMainDlg_challengequest == null)
		{
			return;
		}
		if (this._openType == SOLCOMPOSELIST_DLG_OPENTYPE.TRANSCENDENCE_MATERIAL_SELECT)
		{
			solComposeMainDlg_challengequest.SetTranscendenceSolMaterial(this._selectedDummySolInfo, baseSolSetting);
		}
		else if (this._openType == SOLCOMPOSELIST_DLG_OPENTYPE.COMPOSE_MATERIAL_SELECT)
		{
			solComposeMainDlg_challengequest.SetComposeSolMaterial(this._selectedDummySolInfo, baseSolSetting);
		}
		else
		{
			solComposeMainDlg_challengequest.SetSolBase(this._selectedDummySolInfo, baseSolSetting, this._selectedDummySoldierList);
		}
		this._openType = SOLCOMPOSELIST_DLG_OPENTYPE.NONE;
		this.Close();
	}

	protected override void ChangeInsertListDDL(IUIObject obj)
	{
	}

	private NkSoldierInfo CreateDummySoldier()
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		if (base.ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			nkSoldierInfo.SetCharKind(1053);
			nkSoldierInfo.SetGrade(6);
		}
		else if (base.ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			nkSoldierInfo.SetCharKind(1004);
			nkSoldierInfo.SetGrade(5);
		}
		else if (base.ShowType == SOLCOMPOSE_TYPE.COMPOSE)
		{
			nkSoldierInfo.SetCharKind(1004);
			nkSoldierInfo.SetGrade(4);
		}
		nkSoldierInfo.SetLevel(50);
		nkSoldierInfo.SetSolID(123456L);
		nkSoldierInfo.SetSolSubData(3, 0L);
		nkSoldierInfo.SetExp(nkSoldierInfo.GetCurBaseExp());
		return nkSoldierInfo;
	}
}
