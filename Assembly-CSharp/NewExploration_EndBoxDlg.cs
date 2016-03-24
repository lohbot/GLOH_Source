using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class NewExploration_EndBoxDlg : Form
{
	private Label m_lbDragonHeart;

	private Button m_btEnd;

	private Button m_btCancel;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewExploration/DLG_EndBox", G_ID.NEWEXPLORATION_ENDBOX_DLG, false);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbDragonHeart = (base.GetControl("LB_DragonHeart") as Label);
		this.m_btEnd = (base.GetControl("Button_End") as Button);
		Button expr_32 = this.m_btEnd;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickEnd));
		this.m_btCancel = (base.GetControl("Button_Cancel") as Button);
		Button expr_6F = this.m_btCancel;
		expr_6F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6F.Click, new EZValueChangedDelegate(this.OnClickCancel));
		int num = 0;
		NEWEXPLORATION_DATA endRewardData = NrTSingleton<NewExplorationManager>.Instance.GetEndRewardData();
		if (endRewardData != null)
		{
			num = endRewardData.i32RewardCount;
		}
		this.m_lbDragonHeart.SetText(ANNUALIZED.Convert(num));
	}

	public void OnClickEnd(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_NONE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("883"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("881"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_END_REQ, new GS_NEWEXPLORATION_END_REQ());
		this.Close();
	}

	public void OnClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
