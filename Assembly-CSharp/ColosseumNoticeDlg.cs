using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class ColosseumNoticeDlg : Form
{
	private Label m_laColosseumNotify;

	private Button m_btDummyMatch;

	private Button m_btCancel;

	private DrawTexture m_dtLoadingImg;

	private bool m_bTournamentReady;

	private float m_fRoateVal = 5f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/dlg_match_notify", G_ID.COLOSSEUMNOTICE_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_laColosseumNotify = (base.GetControl("LB_Text") as Label);
		this.m_btCancel = (base.GetControl("BT_Cancel") as Button);
		Button expr_32 = this.m_btCancel;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnBtnClickCancel));
		this.m_dtLoadingImg = (base.GetControl("DT_Loding") as DrawTexture);
		this.m_btDummyMatch = (base.GetControl("Button_AI_match") as Button);
		Button expr_85 = this.m_btDummyMatch;
		expr_85.Click = (EZValueChangedDelegate)Delegate.Combine(expr_85.Click, new EZValueChangedDelegate(this.OnBtnClickDummyMatch));
		string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("155");
		this.m_laColosseumNotify.SetText(textFromNotify);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			if (myCharInfo.Tournament)
			{
				this.m_btDummyMatch.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2405"));
				this.m_laColosseumNotify.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2406"));
			}
			else
			{
				this.m_btDummyMatch.Visible = false;
			}
		}
	}

	public override void Show()
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg != null)
		{
			base.SetLocation(myCharInfoDlg.GetLocationX(), myCharInfoDlg.GetLocationY() + myCharInfoDlg.GetSizeY());
		}
		else
		{
			base.SetLocation(0f, 0f);
		}
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

	public void OnBtnClickDummyMatch(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			if (myCharInfo.Tournament)
			{
				if (!this.m_bTournamentReady)
				{
					if (myCharInfo == null)
					{
						return;
					}
					int num = 0;
					for (int i = 0; i < 3; i++)
					{
						long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUMBATCH1 + i);
						if (charSubData != 0L)
						{
							SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
							sUBDATA_UNION.nSubData = charSubData;
							int n32SubData_ = sUBDATA_UNION.n32SubData_0;
							int n32SubData_2 = sUBDATA_UNION.n32SubData_1;
							byte b = 0;
							byte b2 = 0;
							SoldierBatch.GetCalcBattlePos((long)n32SubData_, ref b, ref b2);
							if (b2 >= 0 && b2 < 9)
							{
								if (n32SubData_2 > 0 && myCharInfo.IsEnableBatchColosseumSoldier(n32SubData_2))
								{
									num++;
								}
							}
						}
					}
					if (num < 3)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("695"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						return;
					}
					GS_TOURNAMENT_PLAYER_READY_REQ gS_TOURNAMENT_PLAYER_READY_REQ = new GS_TOURNAMENT_PLAYER_READY_REQ();
					gS_TOURNAMENT_PLAYER_READY_REQ.m_bReady = true;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_PLAYER_READY_REQ, gS_TOURNAMENT_PLAYER_READY_REQ);
				}
				else
				{
					GS_TOURNAMENT_PLAYER_READY_REQ gS_TOURNAMENT_PLAYER_READY_REQ2 = new GS_TOURNAMENT_PLAYER_READY_REQ();
					gS_TOURNAMENT_PLAYER_READY_REQ2.m_bReady = false;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_PLAYER_READY_REQ, gS_TOURNAMENT_PLAYER_READY_REQ2);
				}
			}
			else
			{
				GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
				gS_COLOSSEUM_START_REQ.byMode = 2;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
			}
		}
	}

	public void SetReady(GS_TOURNAMENT_PLAYER_STATE_NFY _NFY)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null && @char.GetCharName().Equals(TKString.NEWString(_NFY.szPlayerName)))
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null && myCharInfo.Tournament)
			{
				if (_NFY.nPlayerState == 3)
				{
					this.m_btDummyMatch.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2427"));
					this.m_bTournamentReady = true;
				}
				else if (_NFY.nPlayerState == 2)
				{
					this.m_btDummyMatch.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2405"));
					this.m_bTournamentReady = false;
				}
			}
		}
	}
}
