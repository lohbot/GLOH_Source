using CostumeRoomDlg;
using GAME;
using SERVICE;
using System;
using System.Collections.Generic;
using UnityForms;

public class CostumeRoom_Dlg : Form
{
	public const int MAX_STAT_COSTUME = 3;

	public const int SHOW_COMBATPOWER_LEVEL = 50;

	public MyCharListSetter _myCharListSetter;

	public CostumeListSetter _costumeListSetter;

	public CostumeViewerSetter _costumeViewerSetter;

	public CostumeMoneySetter _costumeMoneySetter;

	public ComponentVariables _variables;

	public int _settingCostumeKind = -1;

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_CostumeRoom", G_ID.COSTUMEROOM_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		base.SetDeleagteCloseButton(new EZValueChangedDelegate(this.OnCloseBtn));
		base.Draggable = false;
		this.Init();
	}

	public override void SetComponent()
	{
		this._variables.SetComponent();
	}

	public override void Update()
	{
		this._costumeViewerSetter.Update();
		base.Update();
	}

	public override void OnClose()
	{
		this._costumeViewerSetter.OnClose();
		base.OnClose();
	}

	public void InitCostumeRoom(int costumeKind, NkSoldierInfo solInfo)
	{
		this._settingCostumeKind = costumeKind;
		this.MyCharListSetting(solInfo);
		this._costumeListSetter.InitCostumeListBox(this._variables._costumeListBox, costumeKind);
		this.InitCostumeView(this.GetBaseCostumeData(costumeKind));
		this._costumeMoneySetter.Refresh();
	}

	public void InitCostumeView(CharCostumeInfo_Data costumeData)
	{
		if (costumeData == null)
		{
			return;
		}
		this._costumeViewerSetter.InitCostumeView(costumeData);
	}

	public void Refresh(bool refreshMyCharList, bool refreshCostumeSaleList)
	{
		if (refreshMyCharList)
		{
			this._myCharListSetter.RefreshMyCharList(ref this._variables._mySolKindListBox);
		}
		if (refreshCostumeSaleList)
		{
			this._costumeListSetter.RefreshCostumeListBox(ref this._variables._costumeListBox);
		}
		this._costumeMoneySetter.Refresh();
	}

	public void RefreshMoney()
	{
		this._costumeMoneySetter.Refresh();
	}

	public List<int> GetSettedCostumeKindList()
	{
		return new List<int>
		{
			this._settingCostumeKind
		};
	}

	public void OnCloseBtn(IUIObject obj)
	{
		this.CloseCostumeGuideDlg();
		this.Close();
	}

	public void OnClickBackBtn(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickImmediatelyBuy(IUIObject obj)
	{
		if (this._costumeViewerSetter._settdCostumeData == null)
		{
			return;
		}
		NrTSingleton<CostumeBuyManager>.Instance.BuyCostumeByMsgBox(this._costumeViewerSetter._settdCostumeData);
	}

	public void OnChangeUIHideCheckBox(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		this._costumeViewerSetter.UIHide(checkBox.IsChecked());
	}

	public void OnClickCostumeMaterialLink(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}

	public void OnClickHelpBtn(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg == null)
		{
			return;
		}
		this._costumeViewerSetter._costumeCharSetter.VisibleChar(false);
		gameHelpList_Dlg.SetViewType(eHELP_LIST.Costume.ToString());
	}

	private void Init()
	{
		this._myCharListSetter = new MyCharListSetter(this);
		this._costumeListSetter = new CostumeListSetter(this);
		this._costumeViewerSetter = new CostumeViewerSetter(this);
		this._variables = new ComponentVariables(this);
		this._costumeMoneySetter = new CostumeMoneySetter(this);
	}

	private bool BlockOverlapBuy()
	{
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		return currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE && NrTSingleton<NrMainSystem>.Instance.m_bIsBilling;
	}

	private CharCostumeInfo_Data GetBaseCostumeData(int costumeKind)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(costumeKind);
		if (charKindInfo == null)
		{
			return null;
		}
		CharCostumeInfo_Data normalCostumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetNormalCostumeData(charKindInfo.GetCode());
		if (this._costumeListSetter._SelectedCostumeData == null)
		{
			return normalCostumeData;
		}
		return this._costumeListSetter._SelectedCostumeData;
	}

	private void CloseCostumeGuideDlg()
	{
		CostumeGuide_Dlg costumeGuide_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEGUIDE_DLG) as CostumeGuide_Dlg;
		if (costumeGuide_Dlg == null)
		{
			return;
		}
		costumeGuide_Dlg.Close();
	}

	private void MyCharListSetting(NkSoldierInfo initSelectedSolInfo)
	{
		this._myCharListSetter.InitMyCharList(ref this._variables._mySolKindListBox, this.GetSettedCostumeKindList(), initSelectedSolInfo);
	}
}
