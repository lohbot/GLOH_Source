using System;
using UnityForms;

public class DailyDungeon_Function_Dlg : Form
{
	private Button m_btInitiative;

	private Button m_btAutoBattle;

	private DrawTexture m_dtAutoBattle;

	private Button m_btAutoBatch;

	private bool m_bIsAuto;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "DailyDungeon/dlg_dailydungeon_function", G_ID.DAILYDUNGEON_FUNCTION_DLG, false, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btInitiative = (base.GetControl("BT_Initiative") as Button);
		this.m_btInitiative.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickInitialive));
		this.m_btAutoBattle = (base.GetControl("BT_AUTO") as Button);
		this.m_btAutoBattle.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickAutoBattle));
		this.m_dtAutoBattle = (base.GetControl("DT_AUTO") as DrawTexture);
		this.m_btAutoBatch = (base.GetControl("BT_AUTOBATCH") as Button);
		this.m_btAutoBatch.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickAutoBatch));
		this.Set_Init();
	}

	private void Set_Init()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
		{
			this.m_bIsAuto = true;
		}
		else
		{
			this.m_bIsAuto = false;
		}
		this.Set_AutoBattleTexture(this.m_bIsAuto);
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), GUICamera.height - base.GetSizeY(), base.GetLocation().z);
		}
	}

	public void Set_AutoBattle(bool bisAutoOn)
	{
		string message = string.Empty;
		if (bisAutoOn)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("523");
		}
		else
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("524");
		}
		Main_UI_SystemMessage.ADDMessage(message);
		this.m_bIsAuto = bisAutoOn;
		this.Set_AutoBattleTexture(this.m_bIsAuto);
	}

	private void Set_AutoBattleTexture(bool bisAutoOn)
	{
		if (bisAutoOn)
		{
			this.m_dtAutoBattle.SetTexture("Win_B_Aouto1");
		}
		else
		{
			this.m_dtAutoBattle.SetTexture("Win_B_Aouto2");
		}
	}

	private void OnClickInitialive(IUIObject Obj)
	{
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		if (tempCount <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("740"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		InitiativeSetDlg initiativeSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INITIATIVE_SET_DLG) as InitiativeSetDlg;
		if (initiativeSetDlg != null && SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_DAILYDUNGEON);
		}
	}

	private void OnClickAutoBattle(IUIObject Obj)
	{
		Battle.Send_GS_BATTLE_AUTO_REQ();
	}

	private void OnClickAutoBatch(IUIObject Obj)
	{
		SoldierBatch_AutoBatchTool.AutoBatch();
	}
}
