using GAME;
using System;
using UnityForms;

public class NewExploration_AutoBattleDlg : Form
{
	private Button m_btStartBattle;

	private Button m_btCancel;

	private Label m_lbNote;

	private Label m_lbMoney;

	private Label m_lbSpeed;

	private CheckBox m_cbBox1;

	private CheckBox m_cbBox2;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "NewExploration/DLG_NewExploration_AutoBattle", G_ID.NEWEXPLORATION_AUTOBATTLE_DLG, false, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_btStartBattle = (base.GetControl("Button_ok") as Button);
		Button expr_1C = this.m_btStartBattle;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickStartBattle));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		Button expr_59 = this.m_btCancel;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickCancel));
		this.m_lbNote = (base.GetControl("Label_Note") as Label);
		this.m_lbMoney = (base.GetControl("LB_HEARTNUM") as Label);
		this.m_lbSpeed = (base.GetControl("LB_SPEEDUPNUM") as Label);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			this.m_lbMoney.SetText(ANNUALIZED.Convert(kMyCharInfo.m_Money));
			long num = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
			if (num < 0L)
			{
				num = 0L;
			}
			this.m_lbSpeed.SetText(num.ToString());
		}
		this.m_cbBox1 = (base.GetControl("CheckBox_1") as CheckBox);
		this.m_cbBox2 = (base.GetControl("CheckBox_2") as CheckBox);
		sbyte floor = NrTSingleton<NewExplorationManager>.Instance.GetFloor();
		sbyte subFloor = NrTSingleton<NewExplorationManager>.Instance.GetSubFloor();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("365"),
			"count1",
			(int)floor,
			"count2",
			(int)subFloor
		});
		this.m_lbNote.SetText(empty);
	}

	public void OnClickCancel(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickStartBattle(IUIObject obj)
	{
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("881"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.m_cbBox2.IsChecked() && !NewExploration_AutoBattleDlg.CheckBattleSpeedCount())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("775"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (this.m_cbBox2.IsChecked())
		{
			MsgBoxAutoSellUI msgBoxAutoSellUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_AUTOSELL_DLG) as MsgBoxAutoSellUI;
			msgBoxAutoSellUI.SetLoadData(this.m_cbBox1.IsChecked(), this.m_cbBox2.IsChecked(), MsgBoxAutoSellUI.eMODE.NEWEXPLORATION);
			return;
		}
		NrTSingleton<NewExplorationManager>.Instance.SetAutoBattle(true, this.m_cbBox1.IsChecked(), this.m_cbBox2.IsChecked());
		NrTSingleton<NewExplorationManager>.Instance.SetAutoBatch();
		if (!NrTSingleton<NewExplorationManager>.Instance.Send_GS_NEWEXPLORATION_START_REQ())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("889"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
	}

	public static bool CheckBattleSpeedCount()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		return kMyCharInfo != null && kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT) > 0L;
	}
}
