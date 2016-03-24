using GAME;
using System;
using UnityForms;

public class BabelTower_RepeatDlg : Form
{
	private Button m_btRepeat;

	private Label m_lbCount;

	private DrawTexture m_dtBg;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "BabelTower/Dlg_babel_repeat", G_ID.BABELTOWER_REPEAT_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btRepeat = (base.GetControl("Button_battlerepeat") as Button);
		Button expr_1C = this.m_btRepeat;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnStopBabelRepeat));
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BATTLE_REPEAT", this.m_btRepeat, this.m_btRepeat.GetSize());
		this.m_lbCount = (base.GetControl("Label_Label1") as Label);
		this.m_dtBg = (base.GetControl("DrawTexture_DrawTexture3") as DrawTexture);
		this._SetDialogPos();
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num = 0;
		if (instance != null)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
			{
				num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT);
			}
			else
			{
				int vipLevelAddBattleRepeat = (int)NrTSingleton<NrTableVipManager>.Instance.GetVipLevelAddBattleRepeat();
				num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT) + vipLevelAddBattleRepeat;
			}
		}
		int num2 = num - (NrTSingleton<NkBabelMacroManager>.Instance.MacroCount + 1);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1198"),
			"count",
			num2.ToString()
		});
		this.m_lbCount.SetText(empty);
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			this.m_lbCount.Visible = false;
			this.m_dtBg.Visible = false;
		}
	}

	public void _SetDialogPos()
	{
		eBATTLE_ROOMTYPE battleRoomtype = Battle.BATTLE.BattleRoomtype;
		if (battleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
		{
			if (battleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
			{
				base.SetLocation(GUICamera.width - base.GetSizeX(), base.GetLocationY() + NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDBOSS_BATTLEINFO_DLG).GetSizeY() / 3f);
			}
		}
		else
		{
			base.SetLocation(GUICamera.width - base.GetSizeX(), 0f);
		}
	}

	public void OnStopBabelRepeat(IUIObject obj)
	{
		eBATTLE_ROOMTYPE battleRoomtype = Battle.BATTLE.BattleRoomtype;
		if (battleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
		{
			if (battleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
			{
				if (NrTSingleton<NewExplorationManager>.Instance.AutoBattle)
				{
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					msgBoxUI.SetMsg(new YesDelegate(this.RequestNewExplorationStopAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3491"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("368"), eMsgType.MB_OK_CANCEL, 2);
					return;
				}
			}
		}
		else if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI2.SetMsg(new YesDelegate(this.RequestBabelMacroStopAndAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("187"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("188"), eMsgType.MB_OK_CANCEL, 2);
			return;
		}
	}

	public void RequestBabelMacroStopAndAutoBattle(object a_oObject)
	{
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			Battle.BATTLE.ChangeBattleAuto();
		}
	}

	public void RequestNewExplorationStopAutoBattle(object a_oObject)
	{
		if (NrTSingleton<NewExplorationManager>.Instance.AutoBattle)
		{
			NrTSingleton<NewExplorationManager>.Instance.SetAutoBattle(false, false, false);
			this.CloseForm(null);
		}
	}
}
