using System;
using UnityEngine;
using UnityForms;

public class Battle_Emergency_CallDlg : Form
{
	private Button m_btShowSelectList;

	private Label m_lbEnergency;

	private float m_fEnableTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/EMERGENCY_CALL/dlg_emergency_call", G_ID.BATTLE_EMERGENCY_CALL_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_btShowSelectList = (base.GetControl("Button_emergency") as Button);
		Button expr_1C = this.m_btShowSelectList;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickShowSolList));
		this.m_lbEnergency = (base.GetControl("Label_emergency") as Label);
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			if (this.m_lbEnergency != null)
			{
				this.m_lbEnergency.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2341"));
			}
			this.OnClickShowSolList(null);
		}
		this._SetDialogPos();
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
		{
			this.Hide();
			this.Close();
		}
		if (this.CheckMythRaidUIOff())
		{
			this.Hide();
			this.Close();
		}
		this.m_btShowSelectList.AlphaAni(1f, 0.5f, -0.5f);
	}

	public override void OnClose()
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsNewColosseumSupport())
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG))
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG);
				}
			}
			else if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCT_SELECT_DLG))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCT_SELECT_DLG);
			}
		}
		else if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCT_SELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCT_SELECT_DLG);
		}
		base.OnClose();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
		if (battle_Control_Dlg == null)
		{
			base.SetLocation(0f, GUICamera.height - base.GetSizeY());
			return;
		}
		float x = battle_Control_Dlg.GetLocationX() - base.GetSizeX();
		float y = GUICamera.height - base.GetSizeY();
		base.SetLocation(x, y);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void OnClickShowSolList(IUIObject obj)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsNewColosseumSupport())
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG))
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG);
				}
			}
			else if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCT_SELECT_DLG))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_EMERGENCT_SELECT_DLG);
			}
		}
		else if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCT_SELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_EMERGENCT_SELECT_DLG);
		}
	}

	public void SetEnableControl(bool bEnable)
	{
		this.m_btShowSelectList.controlIsEnabled = bEnable;
	}

	public override void Update()
	{
		if (0f < this.m_fEnableTime && this.m_fEnableTime <= Time.time)
		{
			this.m_fEnableTime = 0f;
			this.SetEnableControl(true);
		}
	}

	private bool CheckMythRaidUIOff()
	{
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			return false;
		}
		int num = (int)BATTLE_CONSTANT_Manager.GetInstance().GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_MYTHRAID_EMERGENCY_COUNT);
		return num <= Battle.BATTLE.ChangeSolCount || Battle.BATTLE.DieSolCount <= Battle.BATTLE.ChangeSolCount || !NrTSingleton<NkBattleCharManager>.Instance.MyCharExist() || Battle.BATTLE.UseEmergencyHelpByThisTurn;
	}
}
