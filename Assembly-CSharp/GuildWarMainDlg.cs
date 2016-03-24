using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class GuildWarMainDlg : Form
{
	private Label m_lbGuildWarTimeState;

	private DrawTexture[] m_dtGuildBG = new DrawTexture[2];

	private Label[] m_lbGuildName = new Label[2];

	private DrawTexture[] m_dtGMark = new DrawTexture[2];

	private DrawTexture[] m_dtGMarkBG = new DrawTexture[2];

	private DrawTexture[] m_dtVictoryEffect = new DrawTexture[2];

	private Label[] m_lbMatchPoint = new Label[2];

	private Label[] m_lbGuildWarRank = new Label[2];

	private Label[] m_lbGuildLevel = new Label[2];

	private DrawTexture[] m_dtExpBar = new DrawTexture[2];

	private Label[] m_lbExpBarText = new Label[2];

	private Label[] m_lbGuildBirth = new Label[2];

	private Label[] m_lbAccrueHonor = new Label[2];

	private Label[] m_lbMemberCount = new Label[2];

	private Button[] m_btMine = new Button[2];

	private Button[] m_btGuild = new Button[2];

	private Label m_lbApplyNoti;

	private Button m_btWarRecord;

	private Button m_btRewardInfo;

	private Button m_btGuildWarList;

	private Button m_btApply;

	private Label m_lbApply;

	private Button m_btBack;

	private Box m_boxNotice;

	private float[] m_fExpWidth = new float[2];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_GuildWar_Main", G_ID.GUILDWAR_MAIN_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbGuildName[0] = (base.GetControl("LB_GuildName") as Label);
		this.m_lbGuildName[1] = (base.GetControl("LB_EnemyGuildName") as Label);
		this.m_dtGuildBG[0] = (base.GetControl("DT_MyGuildBG") as DrawTexture);
		this.m_dtGuildBG[1] = (base.GetControl("DT_EnemyGuildBG") as DrawTexture);
		this.m_dtGMark[0] = (base.GetControl("DT_GMark") as DrawTexture);
		this.m_dtGMark[1] = (base.GetControl("DT_EnemyGMark") as DrawTexture);
		this.m_btGuild[0] = (base.GetControl("BT_MyGuildInfo") as Button);
		Button expr_B0 = this.m_btGuild[0];
		expr_B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B0.Click, new EZValueChangedDelegate(this.OnClickGuild));
		this.m_btGuild[1] = (base.GetControl("BT_EnemyGuildInfo") as Button);
		Button expr_F1 = this.m_btGuild[1];
		expr_F1.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F1.Click, new EZValueChangedDelegate(this.OnClickGuild));
		string textureFromBundle = string.Format("UI/Etc/guildimg03", new object[0]);
		this.m_dtGMarkBG[0] = (base.GetControl("DT_MyGuildMarkBG") as DrawTexture);
		this.m_dtGMarkBG[0].SetTextureFromBundle(textureFromBundle);
		this.m_dtGMarkBG[1] = (base.GetControl("DT_EnemyGuildMarkBG") as DrawTexture);
		this.m_dtGMarkBG[1].SetTextureFromBundle(textureFromBundle);
		this.m_dtVictoryEffect[0] = (base.GetControl("DT_GuildWar_VictoryMark") as DrawTexture);
		this.m_dtVictoryEffect[1] = (base.GetControl("DT_EnemyGuildWar_VictoryMark") as DrawTexture);
		this.m_dtGuildBG[1].SetLocationZ(this.m_dtGuildBG[0].GetLocation().z);
		this.m_btGuild[1].SetLocationZ(this.m_btGuild[0].GetLocation().z);
		this.m_dtGMark[1].SetLocationZ(this.m_dtGMark[0].GetLocation().z);
		this.m_dtGMarkBG[1].SetLocationZ(this.m_dtGMarkBG[0].GetLocation().z);
		this.m_dtVictoryEffect[1].SetLocationZ(this.m_dtVictoryEffect[0].GetLocation().z);
		this.m_lbMatchPoint[0] = (base.GetControl("LB_MineWarPoint01") as Label);
		this.m_lbMatchPoint[1] = (base.GetControl("LB_MineWarPoint02") as Label);
		this.m_lbGuildWarTimeState = (base.GetControl("LB_GuildWarCondition") as Label);
		this.m_lbGuildWarRank[0] = (base.GetControl("LB_GuildWarRank") as Label);
		this.m_lbGuildWarRank[1] = (base.GetControl("LB_EnemyGuildWarRank") as Label);
		this.m_lbGuildLevel[0] = (base.GetControl("LB_LevelText") as Label);
		this.m_lbGuildLevel[1] = (base.GetControl("LB_EnemyLevelText") as Label);
		this.m_dtExpBar[0] = (base.GetControl("DrawTexture_ExpBar") as DrawTexture);
		this.m_dtExpBar[1] = (base.GetControl("DT_EnemyExpBar") as DrawTexture);
		this.m_lbExpBarText[0] = (base.GetControl("LB_ExpText2") as Label);
		this.m_lbExpBarText[1] = (base.GetControl("LB_EnemyExpText2") as Label);
		this.m_lbGuildBirth[0] = (base.GetControl("LB_GuildBirth") as Label);
		this.m_lbGuildBirth[1] = (base.GetControl("LB_EnemyGuildBirth") as Label);
		this.m_lbAccrueHonor[0] = (base.GetControl("LB_AccrueHonor") as Label);
		this.m_lbAccrueHonor[1] = (base.GetControl("LB_EnemyAccrueHonor") as Label);
		this.m_lbMemberCount[0] = (base.GetControl("LB_MyGuildMemberCount") as Label);
		this.m_lbMemberCount[1] = (base.GetControl("LB_EnemyGuildMemberText") as Label);
		this.m_lbApplyNoti = (base.GetControl("LB_GuildWarApplyNotice") as Label);
		this.m_btMine[0] = (base.GetControl("BT_MyMine") as Button);
		Button expr_417 = this.m_btMine[0];
		expr_417.Click = (EZValueChangedDelegate)Delegate.Combine(expr_417.Click, new EZValueChangedDelegate(this.OnClickMine));
		this.m_btMine[1] = (base.GetControl("BT_EnemyMine") as Button);
		Button expr_458 = this.m_btMine[1];
		expr_458.Click = (EZValueChangedDelegate)Delegate.Combine(expr_458.Click, new EZValueChangedDelegate(this.OnClickEnemyMine));
		this.m_btWarRecord = (base.GetControl("Button_WarRecord") as Button);
		Button expr_495 = this.m_btWarRecord;
		expr_495.Click = (EZValueChangedDelegate)Delegate.Combine(expr_495.Click, new EZValueChangedDelegate(this.OnClickWarRecord));
		this.m_btRewardInfo = (base.GetControl("Button_RewardInfo") as Button);
		Button expr_4D2 = this.m_btRewardInfo;
		expr_4D2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4D2.Click, new EZValueChangedDelegate(this.OnClickRewardInfo));
		this.m_btGuildWarList = (base.GetControl("Button_GuildWarList") as Button);
		Button expr_50F = this.m_btGuildWarList;
		expr_50F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_50F.Click, new EZValueChangedDelegate(this.OnClickWarList));
		this.m_btApply = (base.GetControl("button_apply") as Button);
		Button expr_54C = this.m_btApply;
		expr_54C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_54C.Click, new EZValueChangedDelegate(this.OnClickApply));
		this.m_lbApply = (base.GetControl("Label_apply") as Label);
		this.m_fExpWidth[0] = this.m_dtExpBar[0].GetSize().x;
		this.m_fExpWidth[1] = this.m_dtExpBar[1].GetSize().x;
		this.m_btBack = (base.GetControl("BT_Back") as Button);
		Button expr_5DB = this.m_btBack;
		expr_5DB.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5DB.Click, new EZValueChangedDelegate(this.OnClickBack));
		this.m_boxNotice = (base.GetControl("Box_Notice3") as Box);
		this.m_boxNotice.Hide(true);
	}

	public void SetInfo(GS_GUILDWAR_INFO_ACK ACK)
	{
		string text = string.Empty;
		if (NrTSingleton<GuildWarManager>.Instance.CanGetGuildWarReward())
		{
			this.m_boxNotice.Hide(false);
		}
		else
		{
			this.m_boxNotice.Hide(true);
		}
		if (ACK.i64GuildID[1] > 0L)
		{
			base.SetShowLayer(1, true);
			base.SetShowLayer(2, true);
			this.m_lbApplyNoti.Hide(true);
			if (ACK.bGuildWarTimeState == 2)
			{
				this.m_lbGuildWarTimeState.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2979"));
			}
			else if (ACK.bGuildWarTimeState == 3)
			{
				int num;
				if (ACK.i32WarMatchPoint[0] > ACK.i32WarMatchPoint[1])
				{
					num = 0;
				}
				else if (ACK.i32WarMatchPoint[0] < ACK.i32WarMatchPoint[1])
				{
					num = 1;
				}
				else
				{
					num = ((ACK.i32Rank[0] >= ACK.i32Rank[1]) ? 0 : 1);
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2980"),
					"targetname",
					TKString.NEWString(ACK.strGuildName[num])
				});
				this.m_lbGuildWarTimeState.SetText(text);
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_GUILDWAR_VICTORYMARK", this.m_dtVictoryEffect[num], this.m_dtVictoryEffect[num].GetSize());
				if (num == 0)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "GUILD_WIN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
				else
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "GUILD_LOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			else
			{
				base.SetShowLayer(2, false);
			}
		}
		else
		{
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, false);
			this.m_lbApplyNoti.Hide(false);
		}
		NrTSingleton<GuildWarManager>.Instance.bIsGuildWar = ACK.bIsGuildWar;
		NrTSingleton<GuildWarManager>.Instance.bIsGuildWarCancelReservation = ACK.bIsCancelReservation;
		this.SetApplyButton();
		for (int i = 0; i < 2; i++)
		{
			if (ACK.i64GuildID[i] > 0L)
			{
				this.m_btGuild[i].Data = ACK.i64GuildID[i];
				string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(ACK.i64GuildID[i]);
				WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), this.m_dtGMark[i]);
				text = TKString.NEWString(ACK.strGuildName[i]);
				if (NrTSingleton<GuildWarManager>.Instance.bIsGuildWar && i == 0)
				{
					text = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), TKString.NEWString(ACK.strGuildName[i]));
				}
				this.m_lbGuildName[i].SetText(text);
				this.m_lbMatchPoint[i].SetText(ACK.i32WarMatchPoint[i].ToString());
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1685"),
					"count",
					ACK.i32Rank[i]
				});
				this.m_lbGuildWarRank[i].SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1786"),
					"count",
					ACK.i16Level[i]
				});
				this.m_lbGuildLevel[i].SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1303"),
					"count1",
					ACK.i16CurMemberNum[i],
					"count2",
					ACK.i16MaxMemberNum[i]
				});
				this.m_lbMemberCount[i].SetText(text);
				long num2 = 0L;
				long num3 = 1L;
				GUILD_EXP guildLevelUpInfo = NrTSingleton<NkGuildExpManager>.Instance.GetGuildLevelUpInfo(ACK.i16Level[i]);
				if (guildLevelUpInfo != null)
				{
					num2 = ACK.i64Exp[i] - guildLevelUpInfo.m_nExp;
				}
				GUILD_EXP guildLevelUpInfo2 = NrTSingleton<NkGuildExpManager>.Instance.GetGuildLevelUpInfo(ACK.i16Level[i] + 1);
				if (guildLevelUpInfo2 != null && guildLevelUpInfo != null)
				{
					num3 = guildLevelUpInfo2.m_nExp - guildLevelUpInfo.m_nExp;
					if (0L > num3)
					{
						num3 = 0L;
					}
				}
				float num4 = (float)num2 / (float)num3;
				if (1f < num4)
				{
					num4 = 1f;
				}
				this.m_dtExpBar[i].SetSize(this.m_fExpWidth[i] * num4, this.m_dtExpBar[i].GetSize().y);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("603"),
					"count1",
					num2,
					"count2",
					num3
				});
				this.m_lbExpBarText[i].SetText(text);
				if (ACK.i64BirthDate[i] > 0L)
				{
					DateTime dueDate = PublicMethod.GetDueDate(ACK.i64BirthDate[i]);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602"),
						"year",
						dueDate.Year,
						"month",
						dueDate.Month,
						"day",
						dueDate.Day
					});
					this.m_lbGuildBirth[i].SetText(text);
				}
				else
				{
					this.m_lbGuildBirth[i].SetText(string.Empty);
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1041"),
					"count",
					ACK.i32WarPoint[i]
				});
				this.m_lbAccrueHonor[i].SetText(text);
			}
		}
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = (DrawTexture)_param;
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}

	public void SetApplyButton()
	{
		switch (NrTSingleton<GuildWarManager>.Instance.GetWarState())
		{
		case eGUILDWAR_STATE.eGUILDWAR_STATE_NONE:
			this.m_lbApply.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2976"));
			this.m_btApply.SetButtonTextureKey("Win_B_JoinGuildWar");
			break;
		case eGUILDWAR_STATE.eGUILDWAR_STATE_WAR:
			this.m_lbApply.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2977"));
			this.m_btApply.SetButtonTextureKey("Win_B_CancelGuildWar");
			break;
		case eGUILDWAR_STATE.eGUILDWAR_STATE_RESERVATION_CANCEL:
			this.m_lbApply.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2987"));
			this.m_btApply.SetButtonTextureKey("Win_B_JoinGuildWar");
			break;
		}
	}

	public void OnClickApply(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			switch (NrTSingleton<GuildWarManager>.Instance.GetWarState())
			{
			case eGUILDWAR_STATE.eGUILDWAR_STATE_NONE:
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2976");
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("289");
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKApply), eGUILDWAR_STATE.eGUILDWAR_STATE_WAR, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2976"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
				break;
			}
			case eGUILDWAR_STATE.eGUILDWAR_STATE_WAR:
			{
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2977");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("290");
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKApply), eGUILDWAR_STATE.eGUILDWAR_STATE_RESERVATION_CANCEL, null, null, textFromInterface2, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2977"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
				break;
			}
			case eGUILDWAR_STATE.eGUILDWAR_STATE_RESERVATION_CANCEL:
			{
				string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2987");
				string textFromMessageBox3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("291");
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKApply), eGUILDWAR_STATE.eGUILDWAR_STATE_WAR, null, null, textFromInterface3, textFromMessageBox3, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2987"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
				break;
			}
			}
		}
	}

	public void OnClickMine(IUIObject obj)
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
	}

	public void OnClickGuild(IUIObject obj)
	{
		long num = (long)obj.Data;
		if (num == 0L)
		{
			return;
		}
		GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
		gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
	}

	public void OnClickEnemyMine(IUIObject obj)
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 2, 0L);
	}

	public void MsgBoxOKApply(object EventObject)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_REQ((eGUILDWAR_STATE)((int)EventObject));
	}

	public void OnClickWarRecord(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_RECORD_GUILDWAR_DLG);
		this.Close();
	}

	public void OnClickRewardInfo(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUILDWAR_REWARDINFO_DLG);
	}

	public void OnClickWarList(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUILDWAR_LIST_DLG);
	}

	public void OnClickBack(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MEMBER_DLG);
		this.Close();
	}
}
