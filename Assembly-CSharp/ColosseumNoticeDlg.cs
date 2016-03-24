using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ColosseumNoticeDlg : Form
{
	private Label m_laColosseumNotify;

	private Button m_btCancel;

	private DrawTexture m_dtLoadingImg;

	private float m_fRoateVal = 5f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/dlg_match_notify", G_ID.COLOSSEUMNOTICE_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_laColosseumNotify = (base.GetControl("LB_Text") as Label);
		this.m_btCancel = (base.GetControl("BT_Cancel") as Button);
		Button expr_32 = this.m_btCancel;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnBtnClickCancel));
		this.m_dtLoadingImg = (base.GetControl("DT_Loding") as DrawTexture);
		string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("155");
		this.m_laColosseumNotify.SetText(textFromNotify);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void Show()
	{
		base.Show();
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo.Tournament)
		{
			GS_TOURNAMENT_PLAYER_READY_REQ gS_TOURNAMENT_PLAYER_READY_REQ = new GS_TOURNAMENT_PLAYER_READY_REQ();
			gS_TOURNAMENT_PLAYER_READY_REQ.m_SetMode = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_PLAYER_READY_REQ, gS_TOURNAMENT_PLAYER_READY_REQ);
		}
	}

	public override void Update()
	{
		this.m_dtLoadingImg.Rotate(this.m_fRoateVal);
		base.Update();
	}

	public override void OnClose()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null && myCharInfo.Tournament)
		{
			return;
		}
		GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
		gS_COLOSSEUM_START_REQ.byMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
	}

	public void OnBtnClickCancel(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null && myCharInfo.Tournament)
		{
			return;
		}
		GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
		gS_COLOSSEUM_START_REQ.byMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
	}

	public void SetReady(GS_TOURNAMENT_PLAYER_STATE_NFY _NFY)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null && @char.GetCharName().Equals(TKString.NEWString(_NFY.szPlayerName)))
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null && myCharInfo.Tournament)
			{
				if (_NFY.nPlayerState != 3)
				{
					if (_NFY.nPlayerState == 2)
					{
					}
				}
			}
		}
	}
}
