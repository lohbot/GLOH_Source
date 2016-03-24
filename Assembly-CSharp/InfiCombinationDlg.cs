using PlunderSolNumDlgUI;
using System;
using UnityEngine;
using UnityForms;

public class InfiCombinationDlg : Form
{
	private Button[] m_InfiBattlesolCombinationButton;

	private DropDownList[] m_ddlInfiBattleSolCombination;

	private DrawTexture[] m_InfiBattlesolCombinationBG;

	private Label[] m_InfiBattlesolCombinationLabel;

	private DrawTexture m_InfiBattleHideDT;

	private Button m_InfiBattleHideButton;

	private DrawTexture m_InfiBattlesolCombinationBGBase;

	private SolCompleteCombinationDDLManager m_solCompleteCombination;

	private bool hide;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/DLG_InfiCombination", G_ID.INFICOMBINATION_DLG, false);
		base.bCloseAni = false;
		base.DonotDepthChange(1000f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this.m_solCompleteCombination = new SolCompleteCombinationDDLManager();
	}

	public override void SetComponent()
	{
		this.m_InfiBattlesolCombinationButton = new Button[3];
		this.m_ddlInfiBattleSolCombination = new DropDownList[3];
		this.m_InfiBattlesolCombinationBG = new DrawTexture[3];
		this.m_InfiBattlesolCombinationLabel = new Label[3];
		this.m_InfiBattleHideDT = (base.GetControl("DT_Combination_HideButton") as DrawTexture);
		this.m_InfiBattleHideDT.Visible = false;
		this.m_InfiBattleHideButton = (base.GetControl("BTN_Combination_HideButton") as Button);
		this.m_InfiBattleHideButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickInfiBattleHide));
		this.m_InfiBattleHideButton.Visible = false;
		this.m_InfiBattlesolCombinationBGBase = (base.GetControl("DT_Combination_BG") as DrawTexture);
		this.m_InfiBattlesolCombinationBGBase.Visible = false;
		for (int i = 0; i < 3; i++)
		{
			this.m_InfiBattlesolCombinationButton[i] = (base.GetControl("BTN_Combination_" + (i + 1).ToString()) as Button);
			this.m_InfiBattlesolCombinationButton[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickInfiBattleCombination));
			this.m_InfiBattlesolCombinationButton[i].TabIndex = i;
			this.m_InfiBattlesolCombinationButton[i].Visible = false;
			this.m_ddlInfiBattleSolCombination[i] = (base.GetControl("DropDownList_Combination_BG" + (i + 1).ToString()) as DropDownList);
			this.m_ddlInfiBattleSolCombination[i].Clear();
			this.m_ddlInfiBattleSolCombination[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_InfiBattleCombinationDDL));
			this.m_ddlInfiBattleSolCombination[i].data = i;
			this.m_ddlInfiBattleSolCombination[i].SetVisible(false);
			this.m_InfiBattlesolCombinationLabel[i] = (base.GetControl("LB_Combination" + (i + 1).ToString()) as Label);
			this.m_InfiBattlesolCombinationLabel[i].Visible = false;
			this.m_InfiBattlesolCombinationBG[i] = (base.GetControl("DT_Combination_BG" + (i + 1).ToString()) as DrawTexture);
			this.m_InfiBattlesolCombinationBG[i].Visible = false;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			this.m_InfiBattleHideDT.Visible = true;
			this.m_InfiBattleHideButton.Visible = true;
			this.m_InfiBattlesolCombinationBGBase.Visible = true;
			for (int j = 0; j < 3; j++)
			{
				this.m_InfiBattlesolCombinationButton[j].Visible = true;
				this.m_ddlInfiBattleSolCombination[j].SetVisible(true);
				this.m_InfiBattlesolCombinationLabel[j].Visible = true;
				this.m_InfiBattlesolCombinationBG[j].Visible = true;
				this.RenewCompleteCombinationDDL((short)j);
			}
		}
		PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
		if (plunderSolNumDlg != null)
		{
			float x = plunderSolNumDlg.GetSizeX() - base.GetSizeX();
			float num = plunderSolNumDlg.GetTitleBarLocationY();
			PlunderTargetInfoDlg plunderTargetInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERTARGETINFO_DLG) as PlunderTargetInfoDlg;
			if (plunderTargetInfoDlg != null && plunderTargetInfoDlg.Visible)
			{
				num += plunderTargetInfoDlg.GetSizeY();
			}
			base.SetLocation(x, num, base.GetLocation().z);
		}
	}

	public override void InitData()
	{
	}

	public void On_ClickInfiBattleHide(IUIObject a_cObject)
	{
		this.Move();
	}

	public void Move()
	{
		if (base.IsMove)
		{
			return;
		}
		float num = (!this.hide) ? 1f : -1f;
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INFICOMBINATION_DLG) == null)
		{
			return;
		}
		float value = this.m_InfiBattlesolCombinationBGBase.GetSize().x * num;
		base.Move(value);
		this.hide = !this.hide;
	}

	public void On_ClickInfiBattleCombination(IUIObject a_cObject)
	{
		Button button = a_cObject as Button;
		if (null == button)
		{
			return;
		}
		SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
		if (solCombination_Dlg == null)
		{
			Debug.LogError("ERROR, SolGuide_Dlg.cs, On_ClickCombination(), SolCombination_Dlg is Null");
			return;
		}
		solCombination_Dlg.MakeCombinationSolUI(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetOwnBattleReadyAndReadySolKindList(), button.TabIndex);
	}

	private void Change_InfiBattleCombinationDDL(IUIObject obj)
	{
		DropDownList dropDownList = obj as DropDownList;
		if (null == dropDownList)
		{
			return;
		}
		int num = (int)dropDownList.data;
		if (this.m_ddlInfiBattleSolCombination[num] == null)
		{
			Debug.LogError("ERROR, PlunderSolNumDlg.cs, Change_CombinationDDL(), m_ddlSolCombination is Null");
			return;
		}
		this.m_solCompleteCombination.ChangeIdx(this.m_ddlInfiBattleSolCombination[num], num);
	}

	public void RenewCompleteCombinationDDL(short STARTPOS_INDEX)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			this.m_solCompleteCombination.RenewDDL(this.m_ddlInfiBattleSolCombination[(int)STARTPOS_INDEX], (int)STARTPOS_INDEX);
		}
	}

	public void RenewCompleteCombinationLabel(int solCombinationUniqueKey, int STARTPOS_INDEX)
	{
		if (Battle.isLeader)
		{
			return;
		}
		Debug.Log("Party RenewCompleteCombinationLabel UniqueKey : " + solCombinationUniqueKey);
		NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.SetUserSelectedUniqeKey(solCombinationUniqueKey, STARTPOS_INDEX);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE)
		{
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg != null)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = 0;
				plunderStartAndReMatchDlg.Send_InfiBattleMatch(1);
			}
		}
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}
}
