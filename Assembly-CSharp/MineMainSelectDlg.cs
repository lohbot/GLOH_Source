using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class MineMainSelectDlg : Form
{
	private DrawTexture m_dtSubBG;

	private Button m_btExpedition;

	private Button m_btMine;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Expedition/DLG_Mine_MainSelect", G_ID.MINE_MAINSELECT_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_dtSubBG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.m_dtSubBG.SetTextureFromBundle("UI/expedition/bg_personal_min001");
		this.m_btExpedition = (base.GetControl("BT_expedition") as Button);
		Button expr_42 = this.m_btExpedition;
		expr_42.Click = (EZValueChangedDelegate)Delegate.Combine(expr_42.Click, new EZValueChangedDelegate(this.OnClickExpedition));
		this.m_btMine = (base.GetControl("BT_Mine") as Button);
		Button expr_7F = this.m_btMine;
		expr_7F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7F.Click, new EZValueChangedDelegate(this.OnClickMine));
		base.SetScreenCenter();
	}

	public void OnClickMine(IUIObject obj)
	{
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID() || !NrTSingleton<ContentsLimitManager>.Instance.IsMineApply((short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("763");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MINE_TUTORIAL_STEP);
		if (charSubData == 1L)
		{
			MineTutorialStepDlg mineTutorialStepDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_TUTORIAL_STEP_DLG) as MineTutorialStepDlg;
			if (mineTutorialStepDlg != null)
			{
				mineTutorialStepDlg.SetStep(1L);
			}
		}
		else
		{
			NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
		}
	}

	public void OnClickExpedition(IUIObject obj)
	{
		string message = string.Empty;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsExpeditionLimit() || !NrTSingleton<ContentsLimitManager>.Instance.IsExpeditionLevel(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("727");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ = new GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ();
		gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ.i32Page = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ, gS_EXPEDITION_CURRENTSTATUS_INFO_GET_REQ);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void Update()
	{
		base.Update();
	}
}
