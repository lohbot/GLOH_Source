using GAME;
using System;
using UnityForms;

public class BabelTower_RepeatDlg : Form
{
	private Button m_btRepeat;

	private Label m_lbCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "BabelTower/Dlg_babel_repeat", G_ID.BABELTOWER_REPEAT_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_btRepeat = (base.GetControl("Button_battlerepeat") as Button);
		Button expr_1C = this.m_btRepeat;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnStopBabelRepeat));
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BATTLE_REPEAT", this.m_btRepeat, this.m_btRepeat.GetSize());
		this.m_lbCount = (base.GetControl("Label_Label1") as Label);
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
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width - base.GetSizeX(), 0f);
	}

	public void OnStopBabelRepeat(IUIObject obj)
	{
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(this.RequestBabelMacroStopAndAutoBattle), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("187"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("188"), eMsgType.MB_OK_CANCEL);
			return;
		}
	}

	public void RequestBabelMacroStopAndAutoBattle(object a_oObject)
	{
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			Battle.BATTLE.Send_GS_BATTLE_AUTO_REQ();
		}
	}
}
