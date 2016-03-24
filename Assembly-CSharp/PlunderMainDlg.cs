using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class PlunderMainDlg : Form
{
	private enum ePlunderState
	{
		eMode_None,
		eMode_ProtectTime,
		eMode_DelayTime
	}

	private float REFRESH_TIME = 2f;

	private DrawTexture m_DrawTexture_Main_BG;

	private Button m_Button_Attack;

	private Button m_Button_PvP_Report;

	private Button m_Button_Ranking;

	private Button m_Button_Reward;

	private Button m_Button_Pvp_FightIcon;

	private Label m_Label_Protect;

	private Label m_Label_LimitCount;

	private Label m_Label_RankTitle;

	private Label m_Label_Rank;

	private Label m_Label_chargetime;

	private ProgressBar m_ProgressBar;

	private DrawTexture m_DT_Notice;

	private DrawTexture m_DTEffect;

	private float m_fRankRefreshTime;

	private GameObject m_gbEffect_Fight;

	private GameObject m_gbEffect_Fade;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_main", G_ID.PLUNDERMAIN_DLG, false, true);
		base.ShowBlackBG(1f);
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo.GetLevel() > 20)
			{
				directionDLG.SetDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER);
			}
			directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER, 0);
		}
	}

	public override void SetComponent()
	{
		this.m_DrawTexture_Main_BG = (base.GetControl("DrawTexture_BGIMG") as DrawTexture);
		this.m_DrawTexture_Main_BG.SetTextureFromBundle("UI/PvP/Plunder_main");
		this.m_Button_Pvp_FightIcon = (base.GetControl("BT_pvp_fighticon") as Button);
		this.m_Button_Pvp_FightIcon.AddValueChangedDelegate(new EZValueChangedDelegate(this.BattleMatch));
		this.m_Button_Attack = (base.GetControl("BT_Attack") as Button);
		Button expr_6F = this.m_Button_Attack;
		expr_6F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6F.Click, new EZValueChangedDelegate(this.OnAttackMakeUp));
		this.m_Button_PvP_Report = (base.GetControl("BT_pvp_report") as Button);
		Button expr_AC = this.m_Button_PvP_Report;
		expr_AC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_AC.Click, new EZValueChangedDelegate(this.OnReport));
		this.m_Button_Ranking = (base.GetControl("BT_Ranking") as Button);
		Button expr_E9 = this.m_Button_Ranking;
		expr_E9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E9.Click, new EZValueChangedDelegate(this.OnRank));
		this.m_Button_Reward = (base.GetControl("Button_reward") as Button);
		Button expr_126 = this.m_Button_Reward;
		expr_126.Click = (EZValueChangedDelegate)Delegate.Combine(expr_126.Click, new EZValueChangedDelegate(this.OnProtectTime));
		this.m_Label_Protect = (base.GetControl("Label_Protect") as Label);
		this.m_Label_LimitCount = (base.GetControl("Label_LimitCount") as Label);
		this.m_Label_RankTitle = (base.GetControl("Label_RankingTitle") as Label);
		this.m_Label_Rank = (base.GetControl("Label_rank") as Label);
		this.m_Label_chargetime = (base.GetControl("Label_chargetime") as Label);
		this.m_ProgressBar = (base.GetControl("Progress") as ProgressBar);
		this.m_DT_Notice = (base.GetControl("DT_Notice") as DrawTexture);
		this.m_DT_Notice.Visible = false;
		this.m_DTEffect = (base.GetControl("DT_effect_burning") as DrawTexture);
		base.SetShowLayer(1, false);
		base.SetScreenCenter();
		this.LoadEffect();
	}

	public override void Show()
	{
		this.ShowMode();
	}

	public override void Update()
	{
		base.Update();
		this.CheckLimitTime();
		if (0f < this.m_fRankRefreshTime && this.m_fRankRefreshTime < Time.time)
		{
			this.m_fRankRefreshTime = 0f;
			this.m_Label_Rank.SetEnabled(true);
		}
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		if (null != this.m_gbEffect_Fight)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Fight);
		}
		if (null != this.m_gbEffect_Fade)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Fade);
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public void CheckLimitTime()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		string empty = string.Empty;
		int num = 0;
		long num2 = 0L;
		long num3 = 0L;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_CHARGEMAX);
			num2 = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLECOUNT);
			num3 = (long)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_CHARGETIME);
			if (num2 < 0L)
			{
				num2 = 0L;
			}
			if (num3 < 0L)
			{
				num3 = 0L;
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2235"),
			"count1",
			num2,
			"count2",
			num
		});
		this.m_Label_LimitCount.SetText(empty);
		long curTime = PublicMethod.GetCurTime();
		string text = string.Empty;
		if ((long)num == num2)
		{
			this.m_Label_chargetime.Visible = false;
			this.m_ProgressBar.Value = 0f;
			return;
		}
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLEADDCOUNT_TIME);
		float value;
		if (curTime <= charSubData)
		{
			this.m_Label_chargetime.Visible = true;
			text = PublicMethod.ConvertTime(charSubData - curTime);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2933"),
				"timestring",
				text
			});
			this.m_Label_chargetime.SetText(empty);
			value = 1f - (float)(charSubData - curTime) / ((float)num3 * 60f);
		}
		else
		{
			this.m_Label_chargetime.Visible = false;
			value = 1f;
		}
		this.m_ProgressBar.Value = value;
	}

	public void OnPlunderStart(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
		long curTime = PublicMethod.GetCurTime();
		long num = charSubData - curTime;
		if (num > 0L)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
				"timestring",
				PublicMethod.ConvertTime(num)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (myCharInfo != null)
		{
			int level = myCharInfo.GetLevel();
			long num2;
			if (level > 50)
			{
				num2 = (long)(level * (level - COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD2)));
			}
			else
			{
				num2 = (long)(level * COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD));
			}
			if (num2 > myCharInfo.m_Money)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "REMIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PLUNDER;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
	}

	private void BattleMatch(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
		long curTime = PublicMethod.GetCurTime();
		if (curTime < charSubData)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("862"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
			if (myCharInfo.GetLevel() < value)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129"),
					"level",
					value
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			long num = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLECOUNT);
			if (num < 0L)
			{
				num = 0L;
			}
			if (num > 0L)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "REMIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_INFIBATTLE;
				FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
	}

	public void OnAttackMakeUp(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
	}

	public void OnReport(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		GS_INFIBATTLE_RECORD_REQ gS_INFIBATTLE_RECORD_REQ = new GS_INFIBATTLE_RECORD_REQ();
		gS_INFIBATTLE_RECORD_REQ.i64PersonID = charPersonInfo.GetPersonID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_RECORD_REQ, gS_INFIBATTLE_RECORD_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "RECORD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OnRank(IUIObject obj)
	{
		this.m_Label_Rank.SetEnabled(false);
		this.m_fRankRefreshTime = Time.time + this.REFRESH_TIME;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		GS_INFIBATTLE_REWARDINFO_REQ gS_INFIBATTLE_REWARDINFO_REQ = new GS_INFIBATTLE_REWARDINFO_REQ();
		gS_INFIBATTLE_REWARDINFO_REQ.i64PersonID = myCharInfo.m_PersonID;
		SendPacket.GetInstance().SendObject(2011, gS_INFIBATTLE_REWARDINFO_REQ);
	}

	public void OnProtectTime(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			InfiBattleReward infiBattleReward = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward;
			if (infiBattleReward != null)
			{
				GS_INFIBATTLE_GET_REWARDINFO_REQ gS_INFIBATTLE_GET_REWARDINFO_REQ = new GS_INFIBATTLE_GET_REWARDINFO_REQ();
				gS_INFIBATTLE_GET_REWARDINFO_REQ.i64PersonID = myCharInfo.m_PersonID;
				SendPacket.GetInstance().SendObject(2013, gS_INFIBATTLE_GET_REWARDINFO_REQ);
			}
		}
	}

	public void ShowMode()
	{
		this.ShowInfiBattle();
	}

	public void ShowInfiBattle()
	{
		this.m_Button_Reward.Visible = true;
		this.m_Label_LimitCount.Visible = true;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2331");
		this.m_Label_Protect.SetText(textFromInterface);
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2217");
		this.m_Label_RankTitle.SetText(textFromInterface);
		if (0 < NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_Rank)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("447"),
				"rank",
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_Rank
			});
		}
		else
		{
			textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
		}
		this.m_Label_Rank.SetText(textFromInterface);
		this.SetRewardMark();
	}

	public void SetRewardMark()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleReward == 0)
		{
			this.m_DT_Notice.Visible = true;
		}
		else
		{
			this.m_DT_Notice.Visible = false;
		}
	}

	public void LoadEffect()
	{
		string str = string.Format("{0}{1}", "Effect/Instant/fx_ui_coloseum_infinity", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Fight), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		str = string.Format("{0}{1}", "Effect/Instant/fx_direct_fade", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Fade), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void Effect_Fight(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect_Fight = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Fight);
					return;
				}
				Vector2 size = this.m_DTEffect.GetSize();
				this.m_gbEffect_Fight.transform.parent = this.m_DTEffect.gameObject.transform;
				this.m_gbEffect_Fight.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, 0f);
				this.m_gbEffect_Fight.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
				NkUtil.SetAllChildLayer(this.m_gbEffect_Fight, GUICamera.UILayer);
				this.m_gbEffect_Fight.SetActive(true);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbEffect_Fight);
				}
			}
		}
	}

	private void Effect_Fade(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect_Fade = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Fade);
					return;
				}
				Vector2 size = this.m_DrawTexture_Main_BG.GetSize();
				this.m_gbEffect_Fade.transform.parent = this.m_DrawTexture_Main_BG.gameObject.transform;
				this.m_gbEffect_Fade.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_DrawTexture_Main_BG.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_gbEffect_Fade, GUICamera.UILayer);
				this.m_gbEffect_Fade.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbEffect_Fade);
				}
			}
		}
	}
}
