using System;
using UnityForms;

public class NewExploration_FunctionDlg : Form
{
	private Button m_btAuto;

	private Button m_btInitiative;

	private Button m_btChat;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "NewExploration/DLG_NewExploration_function", G_ID.NEWEXPLORATION_FUNCTION_DLG, false, true);
	}

	public override void SetComponent()
	{
		this.m_btAuto = (base.GetControl("BT_AUTOBATCH") as Button);
		Button expr_1C = this.m_btAuto;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickAuto));
		this.m_btInitiative = (base.GetControl("BT_Initiative") as Button);
		Button expr_59 = this.m_btInitiative;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickInitiative));
		this.m_btChat = (base.GetControl("BT_Chat") as Button);
		Button expr_96 = this.m_btChat;
		expr_96.Click = (EZValueChangedDelegate)Delegate.Combine(expr_96.Click, new EZValueChangedDelegate(this.OnClickChat));
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), GUICamera.height - base.GetSizeY(), base.GetLocation().z);
		}
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), GUICamera.height - base.GetSizeY(), base.GetLocation().z);
		}
	}

	public void OnClickAuto(IUIObject obj)
	{
		SoldierBatch_AutoBatchTool.AutoBatch();
	}

	public void OnClickInitiative(IUIObject obj)
	{
		InitiativeSetDlg initiativeSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INITIATIVE_SET_DLG) as InitiativeSetDlg;
		if (initiativeSetDlg != null)
		{
			initiativeSetDlg.SetBatchSolList(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION);
		}
	}

	public void OnClickChat(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}
}
