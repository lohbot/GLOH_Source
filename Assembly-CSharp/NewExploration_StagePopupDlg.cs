using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class NewExploration_StagePopupDlg : Form
{
	private Button m_btClose;

	private Button m_btStart;

	private Label m_lbTitle;

	private DrawTexture m_dtBossFace;

	private DrawTexture m_dtBossHPBar;

	private float m_fBossHPBarWidth;

	private Label m_lbBossHP;

	private Label m_lbExpPoint;

	private Label m_lbDragonHeart;

	private sbyte m_bFloor;

	private sbyte m_bSubFloor;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewExploration/DLG_StagePopup", G_ID.NEWEXPLORATION_STAGEPOPUP_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btClose = (base.GetControl("Btn_Close") as Button);
		Button expr_1C = this.m_btClose;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClose));
		this.m_btStart = (base.GetControl("Btn_Start") as Button);
		Button expr_59 = this.m_btStart;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickStart));
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_dtBossFace = (base.GetControl("DT_BossFace") as DrawTexture);
		this.m_lbBossHP = (base.GetControl("LB_BossHP") as Label);
		this.m_dtBossHPBar = (base.GetControl("DT_BossHP02") as DrawTexture);
		this.m_fBossHPBarWidth = this.m_dtBossHPBar.width;
		this.m_lbExpPoint = (base.GetControl("LB_ExpPoint") as Label);
		this.m_lbDragonHeart = (base.GetControl("LB_DragonHeart") as Label);
	}

	public void SetFloor(sbyte bFloor, sbyte bSubFloor)
	{
		this.m_bFloor = bFloor;
		this.m_bSubFloor = bSubFloor;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3477"),
			"count1",
			bFloor,
			"count2",
			bSubFloor
		});
		this.m_lbTitle.SetText(empty);
		bool flag = NrTSingleton<NewExplorationManager>.Instance.IsClear(bFloor, bSubFloor);
		base.SetShowLayer(2, flag);
		NEWEXPLORATION_DATA data = NrTSingleton<NewExplorationManager>.Instance.GetData(bFloor, bSubFloor);
		if (data == null)
		{
			return;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3470"),
			"count",
			ANNUALIZED.Convert(data.i32RewardExp)
		});
		this.m_lbExpPoint.SetText(empty);
		this.m_lbDragonHeart.SetText(ANNUALIZED.Convert(data.i32RewardCount));
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			int num = data.i32BossHp;
			if (NrTSingleton<NewExplorationManager>.Instance.CanPlay(bFloor, bSubFloor))
			{
				num = data.i32BossHp - NrTSingleton<NewExplorationManager>.Instance.GetBossDamage();
				if (num <= 0)
				{
					num = 0;
				}
			}
			else if (flag)
			{
				num = 0;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1808"),
				"count1",
				ANNUALIZED.Convert(num),
				"count2",
				ANNUALIZED.Convert(data.i32BossHp)
			});
			this.m_lbBossHP.SetText(empty);
			float num2 = (float)num / (float)data.i32BossHp;
			this.m_dtBossHPBar.SetSize(this.m_fBossHPBarWidth * num2, this.m_dtBossHPBar.GetSize().y);
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(data.i32BossCharKind);
		this.m_dtBossFace.SetTexture(eCharImageType.LARGE, charKindInfo.GetCharKind(), -1, string.Empty);
	}

	public void OnClickStart(IUIObject obj)
	{
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("881"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<NewExplorationManager>.Instance.CanPlay(this.m_bFloor, this.m_bSubFloor))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("882"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		clTempBattlePos[] autoBatchPos = SoldierBatch_SolList.GetAutoBatchPos(5, eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION, null);
		if (autoBatchPos == null || autoBatchPos.Length <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("889"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		GS_NEWEXPLORATION_STAGE_CHALLENGE_REQ gS_NEWEXPLORATION_STAGE_CHALLENGE_REQ = new GS_NEWEXPLORATION_STAGE_CHALLENGE_REQ();
		gS_NEWEXPLORATION_STAGE_CHALLENGE_REQ.i8Floor = this.m_bFloor;
		gS_NEWEXPLORATION_STAGE_CHALLENGE_REQ.i8SubFloor = this.m_bSubFloor;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_STAGE_CHALLENGE_REQ, gS_NEWEXPLORATION_STAGE_CHALLENGE_REQ);
	}

	public void OnClose(IUIObject obj)
	{
		this.Close();
	}
}
