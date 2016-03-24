using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SolCombinationDlg;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolCombination_Dlg : Form
{
	private DropDownList m_ddlCombinationGradeList;

	private CheckBox m_cbCompleteCombinationView;

	private NewListBox m_nlbCombinationList;

	private Button m_btHelp;

	private Button m_btSolGuide;

	private Dictionary<int, string> _gradeTextKeyDic;

	private Dictionary<int, string> _gradeTextureKeyDic;

	private List<SolCombinationInfo_Data> _combinationInfo;

	private List<int> _charOwnSoldierKindList;

	private int ENTIRE_VIEW = 100;

	private int _selectedViewGradeKey = -1;

	private bool _isCompleteCombinationClicked;

	public int _selectedInfiBattleIndex = -1;

	private Initializer _initializer;

	private SolCombinationRowItemMaker _solCombinationRowItemMaker;

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_SolCombination", G_ID.SOLCOMBINATION_DLG, false, true);
		this._solCombinationRowItemMaker = new SolCombinationRowItemMaker();
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		this.Initialize();
	}

	public override void SetComponent()
	{
		this.m_ddlCombinationGradeList = (base.GetControl("DDL_Grade") as DropDownList);
		this.m_ddlCombinationGradeList.Reserve = false;
		this.m_ddlCombinationGradeList.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_CombinationGrade));
		this.m_ddlCombinationGradeList.SetViewArea(this._gradeTextKeyDic.Count);
		foreach (KeyValuePair<int, string> current in this._gradeTextKeyDic)
		{
			ListItem listItem = new ListItem();
			listItem.Key = current.Key;
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(current.Value));
			this.m_ddlCombinationGradeList.Add(listItem);
		}
		this.m_ddlCombinationGradeList.RepositionItems();
		this.m_ddlCombinationGradeList.SetFirstItem();
		this.m_cbCompleteCombinationView = (base.GetControl("CheckBox_complet") as CheckBox);
		this.m_cbCompleteCombinationView.CheckedChanged = new EZValueChangedDelegate(this.CheckBoxChange_CompleteCombinationView);
		this.m_nlbCombinationList = (base.GetControl("NLB_Combination") as NewListBox);
		this.m_nlbCombinationList.Reserve = false;
		this.m_nlbCombinationList.AddScrollDelegate(new EZScrollDelegate(this.ChangeCobinationInfo));
		this.m_nlbCombinationList.ReUse = true;
		this.m_btHelp = (base.GetControl("Help_Button") as Button);
		this.m_btHelp.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_cbCompleteCombinationView.SetToggleState(1);
		this.m_btSolGuide = (base.GetControl("Button_SolGuide") as Button);
		this.m_btSolGuide.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolGuide));
		base.SetLayerZ(1, -0.2f);
		this.Hide();
		GS_SOLCOMBINATIONLIMIT_INFO_REQ gS_SOLCOMBINATIONLIMIT_INFO_REQ = default(GS_SOLCOMBINATIONLIMIT_INFO_REQ);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLCOMBINATIONLIMIT_INFO_REQ, gS_SOLCOMBINATIONLIMIT_INFO_REQ);
	}

	private void ChangeCobinationInfo(IUIObject obj, int index)
	{
		if (index >= this._combinationInfo.Count)
		{
			return;
		}
		SolCombinationInfo_Data solCombinationInfo_Data = this._combinationInfo[index];
		if (solCombinationInfo_Data != null)
		{
			NewListItem item = this._solCombinationRowItemMaker.CreateSolCombinationItem(this.m_nlbCombinationList, this._gradeTextureKeyDic, this._charOwnSoldierKindList, solCombinationInfo_Data);
			this.m_nlbCombinationList.UpdateContents(index, item);
		}
	}

	public void MakeCombinationSolUI(List<int> charOwnSoldierKindList, int InfiBattleIndex)
	{
		this.m_nlbCombinationList.Clear();
		this._selectedViewGradeKey = this.ENTIRE_VIEW;
		this._selectedInfiBattleIndex = InfiBattleIndex;
		this._isCompleteCombinationClicked = this.m_cbCompleteCombinationView.IsChecked();
		if (charOwnSoldierKindList == null)
		{
			this._charOwnSoldierKindList = new List<int>();
		}
		else
		{
			this._charOwnSoldierKindList = charOwnSoldierKindList;
		}
		this.SetCombinationList();
	}

	private void Change_CombinationGrade(IUIObject obj)
	{
		Debug.Log("Change_CombinationGrade Callback");
		ListItem listItem = this.m_ddlCombinationGradeList.SelectedItem.Data as ListItem;
		if (listItem == null)
		{
			Debug.LogError("ERROR, SolCombination_DLG.cs. Change_CombinationGrade(), selectitems is null");
			return;
		}
		int num = (int)listItem.Key;
		Debug.Log("SelectedViewKey : " + num);
		if (this._selectedViewGradeKey == num)
		{
			return;
		}
		this._selectedViewGradeKey = num;
		this.SetCombinationList();
	}

	private void CheckBoxChange_CompleteCombinationView(IUIObject obj)
	{
		Debug.Log("CheckBoxChange_CompleteCombinationViewe Callback");
		Debug.Log(this.m_cbCompleteCombinationView.IsChecked());
		if (this._isCompleteCombinationClicked == this.m_cbCompleteCombinationView.IsChecked())
		{
			return;
		}
		this._isCompleteCombinationClicked = this.m_cbCompleteCombinationView.IsChecked();
		this.SetCombinationList();
	}

	private void Initialize()
	{
		this._initializer = new Initializer();
		this._gradeTextKeyDic = this._initializer.InitGradeTextKeyDic(this.ENTIRE_VIEW);
		this._gradeTextureKeyDic = this._initializer.InitGradeTextureKeyDic();
		this._combinationInfo = new List<SolCombinationInfo_Data>();
	}

	private void SetCombinationList()
	{
		this._combinationInfo.Clear();
		this.m_nlbCombinationList.Clear();
		List<KeyValuePair<long, SolCombinationInfo_Data>> combinationInfoSortedSorList = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCombinationInfoSortedSorList();
		foreach (KeyValuePair<long, SolCombinationInfo_Data> current in combinationInfoSortedSorList)
		{
			if (this.IsSelectedViewGradeByChecked(current.Value))
			{
				if (this.IsCompleteCombinationByChecked(current.Value))
				{
					if (this.IsShow(current.Value))
					{
						NewListItem item = this._solCombinationRowItemMaker.CreateSolCombinationItem(this.m_nlbCombinationList, this._gradeTextureKeyDic, this._charOwnSoldierKindList, current.Value);
						this.m_nlbCombinationList.Add(item);
						this._combinationInfo.Add(current.Value);
					}
				}
			}
		}
		this.m_nlbCombinationList.RepositionItems();
	}

	private bool IsSelectedViewGradeByChecked(SolCombinationInfo_Data combinationData)
	{
		return this._selectedViewGradeKey == this.ENTIRE_VIEW || this._selectedViewGradeKey == combinationData.m_nCombinationGrade;
	}

	private bool IsCompleteCombinationByChecked(SolCombinationInfo_Data combinationData)
	{
		return !this._isCompleteCombinationClicked || NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.IsCompleteCombination(this._charOwnSoldierKindList, combinationData);
	}

	private bool IsShow(SolCombinationInfo_Data combinationData)
	{
		return combinationData != null && 0 < combinationData.m_nCombinationIsShow;
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Teamplay.ToString());
		}
	}

	private void ClickSolGuide(IUIObject obj)
	{
		GS_SOLGUIDE_INFO_REQ gS_SOLGUIDE_INFO_REQ = new GS_SOLGUIDE_INFO_REQ();
		gS_SOLGUIDE_INFO_REQ.bElementMark = false;
		gS_SOLGUIDE_INFO_REQ.i32CharKind = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLGUIDE_INFO_REQ, gS_SOLGUIDE_INFO_REQ);
	}
}
