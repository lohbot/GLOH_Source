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

	private Toggle[] m_tgMode = new Toggle[2];

	private DrawTexture m_DrawTexture_Main_BG;

	private Button m_Button_PvpMatch;

	private Button m_Button_Attack;

	private Button m_Button_Defense;

	private Button m_Button_PvP_Report;

	private Button m_Button_Ranking;

	private Button m_Button_Protect;

	private Button m_Button_Reward;

	private Label m_Label_Protect;

	private DrawTexture m_DrawTexture_Attack;

	private DrawTexture m_DrawTexture_Defense;

	private DrawTexture m_DrawTexture_PvpReport;

	private DrawTexture m_DrawTexture_Ranking;

	private DrawTexture m_DrawTexture_Protect;

	private Button m_btEffectShowHelp;

	private DrawTexture m_DrawTexture_InfibattleIMG;

	private DrawTexture m_DrawTexture_InfibattleIMG2;

	private DrawTexture m_DrawTexture_InfiEffect;

	private GameObject m_Effect_HeroGlow;

	private GameObject m_Effect_InfiBattle;

	private Label m_Label_infinite;

	private DrawTexture m_DrawTexture_plunderIMG;

	private DrawTexture m_DrawTexture_plunderIMG2;

	private DrawTexture m_DrawTexture_HeroEffect;

	private Label m_Label_Plunder;

	private Label m_Label_LimitCount;

	private Label m_Label_Gold;

	private Label m_Label_Protecttime;

	private Label m_Label_RankTitle;

	private Label m_Label_Rank;

	private DrawTexture m_DrawTexure_Active;

	private int m_nPlunderState;

	private DrawTexture m_DrawTexure_Active2;

	private eMODE m_eMode;

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
		this.m_Button_PvpMatch = (base.GetControl("BT_pvp_match") as Button);
		Button expr_42 = this.m_Button_PvpMatch;
		expr_42.Click = (EZValueChangedDelegate)Delegate.Combine(expr_42.Click, new EZValueChangedDelegate(this.BattleMatch));
		this.m_Button_Attack = (base.GetControl("BT_Attack") as Button);
		Button expr_7F = this.m_Button_Attack;
		expr_7F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7F.Click, new EZValueChangedDelegate(this.OnAttackMakeUp));
		this.m_Button_Defense = (base.GetControl("BT_Defense") as Button);
		Button expr_BC = this.m_Button_Defense;
		expr_BC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_BC.Click, new EZValueChangedDelegate(this.OnDefenseMakeUp));
		this.m_Button_PvP_Report = (base.GetControl("BT_pvp_report") as Button);
		Button expr_F9 = this.m_Button_PvP_Report;
		expr_F9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F9.Click, new EZValueChangedDelegate(this.OnReport));
		this.m_Button_Ranking = (base.GetControl("BT_Ranking") as Button);
		Button expr_136 = this.m_Button_Ranking;
		expr_136.Click = (EZValueChangedDelegate)Delegate.Combine(expr_136.Click, new EZValueChangedDelegate(this.OnRank));
		this.m_Button_Protect = (base.GetControl("Button_protect") as Button);
		Button expr_173 = this.m_Button_Protect;
		expr_173.Click = (EZValueChangedDelegate)Delegate.Combine(expr_173.Click, new EZValueChangedDelegate(this.OnProtect));
		this.m_Button_Reward = (base.GetControl("Button_reward") as Button);
		Button expr_1B0 = this.m_Button_Reward;
		expr_1B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1B0.Click, new EZValueChangedDelegate(this.OnProtectTime));
		this.m_Label_Protect = (base.GetControl("Label_Protect") as Label);
		this.m_DrawTexture_Attack = (base.GetControl("IMG_Attack") as DrawTexture);
		this.m_DrawTexture_Defense = (base.GetControl("IMG_Defense") as DrawTexture);
		this.m_DrawTexture_PvpReport = (base.GetControl("IMG_pvp_report") as DrawTexture);
		this.m_DrawTexture_Ranking = (base.GetControl("IMG_Ranking") as DrawTexture);
		this.m_DrawTexture_Protect = (base.GetControl("IMG_Protect") as DrawTexture);
		this.m_Label_LimitCount = (base.GetControl("Label_LimitCount") as Label);
		this.m_Label_Gold = (base.GetControl("Label_gold") as Label);
		this.m_Label_Protecttime = (base.GetControl("Label_protecttime") as Label);
		this.m_Label_RankTitle = (base.GetControl("Label_RankingTitle") as Label);
		this.m_Label_Rank = (base.GetControl("Label_rank") as Label);
		this.m_btEffectShowHelp = (base.GetControl("Help_Button") as Button);
		this.m_btEffectShowHelp.Click = new EZValueChangedDelegate(this.BtnShowEffectPlunder);
		this.m_DrawTexure_Active = (base.GetControl("DrawTexture_active") as DrawTexture);
		this.m_DrawTexture_plunderIMG = (base.GetControl("DrawTexture_plunderIMG") as DrawTexture);
		this.m_DrawTexture_plunderIMG2 = (base.GetControl("DrawTexture_PlunderIMG2") as DrawTexture);
		this.m_DrawTexture_HeroEffect = (base.GetControl("DrawTexture_HeroEffect") as DrawTexture);
		this.m_Label_Plunder = (base.GetControl("Label_Plunder") as Label);
		this.m_DrawTexure_Active2 = (base.GetControl("DrawTexture_active2") as DrawTexture);
		this.m_DrawTexture_InfibattleIMG = (base.GetControl("DrawTexture_InfibattleIMG") as DrawTexture);
		this.m_DrawTexture_InfibattleIMG2 = (base.GetControl("DrawTexture_InfibattleIMG2") as DrawTexture);
		this.m_DrawTexture_HeroEffect = (base.GetControl("DrawTexture_HeroEffect") as DrawTexture);
		this.m_DrawTexture_InfiEffect = (base.GetControl("DrawTexture_InfiEffect") as DrawTexture);
		this.m_Label_infinite = (base.GetControl("Label_infinite") as Label);
		base.SetShowLayer(1, false);
		this.m_tgMode[1] = (base.GetControl("Toggle_infinite") as Toggle);
		this.m_tgMode[1].data = eMODE.eMODE_INFIBATTLE;
		this.m_tgMode[1].Value = false;
		this.m_tgMode[0] = (base.GetControl("Toggle_Plunder") as Toggle);
		this.m_tgMode[0].data = eMODE.eMODE_PLUNDER;
		this.m_tgMode[0].Value = false;
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
		if (null != this.m_Effect_HeroGlow)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Effect_HeroGlow);
		}
		if (null != this.m_Effect_InfiBattle)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Effect_InfiBattle);
		}
	}

	public void CheckLimitTime()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (this.m_eMode == eMODE.eMODE_PLUNDER)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1775");
			string empty = string.Empty;
			int level = myCharInfo.GetLevel();
			long num;
			if (level > 50)
			{
				num = (long)(level * (level - COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD2)));
			}
			else
			{
				num = (long)(level * COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD));
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"gold",
				num.ToString()
			});
			this.m_Label_Gold.Text = empty;
			int num2 = 0;
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
			long curTime = PublicMethod.GetCurTime();
			long num3 = charSubData - curTime;
			if (num3 > 0L)
			{
				this.m_nPlunderState = 2;
			}
			else
			{
				num2++;
			}
			long charSubData2 = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_COOLTIME);
			long num4 = charSubData2 - curTime;
			if (num4 > 0L)
			{
				this.m_nPlunderState = 1;
			}
			else
			{
				num2++;
			}
			if (num2 >= 2)
			{
				this.m_nPlunderState = 0;
			}
			this.m_Label_Protecttime.Visible = true;
			if (this.m_nPlunderState == 0)
			{
				this.m_Label_Protecttime.Visible = false;
				return;
			}
			string empty2 = string.Empty;
			string text = string.Empty;
			string empty3 = string.Empty;
			uint num5 = 0u;
			uint num6 = 0u;
			uint num7 = 0u;
			uint num8 = 0u;
			if (this.m_nPlunderState == 1)
			{
				num5 = (uint)(num4 / 3600L / 24L);
				num6 = (uint)(num4 / 3600L - (long)((ulong)(num5 * 24u)));
				num7 = (uint)((num4 - (long)((ulong)(num5 * 24u * 3600u)) - (long)((ulong)(num6 * 3600u))) / 60L);
				num8 = (uint)(num4 - (long)((ulong)(num5 * 24u * 3600u)) - (long)((ulong)(num6 * 3600u)) - (long)((ulong)(num7 * 60u)));
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("96");
			}
			else if (this.m_nPlunderState == 2)
			{
				num5 = (uint)(num3 / 3600L / 24L);
				num6 = (uint)(num3 / 3600L - (long)((ulong)(num5 * 24u)));
				num7 = (uint)((num3 - (long)((ulong)(num5 * 24u * 3600u)) - (long)((ulong)(num6 * 3600u))) / 60L);
				num8 = (uint)(num3 - (long)((ulong)(num5 * 24u * 3600u)) - (long)((ulong)(num6 * 3600u)) - (long)((ulong)(num7 * 60u)));
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("403");
			}
			if (0u < num5)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1134"),
					"day",
					num5,
					"hour",
					num6,
					"min",
					num7
				});
			}
			else if (0u < num6)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1135"),
					"hour",
					num6,
					"min",
					num7
				});
			}
			else if (0u < num7)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1136"),
					"min",
					num7
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2336"),
					"sec",
					num8
				});
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				text,
				"timestring",
				empty2
			});
			this.m_Label_Protecttime.SetText(empty3);
		}
		if (this.m_eMode == eMODE.eMODE_INFIBATTLE)
		{
			string empty4 = string.Empty;
			int num9 = 0;
			int num10 = 0;
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (instance != null)
			{
				num9 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCount;
				num10 = num9 - (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(21);
				if (num10 < 0)
				{
					num10 = 0;
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2235"),
				"count1",
				num10,
				"count2",
				num9
			});
			this.m_Label_LimitCount.SetText(empty4);
			long charSubData3 = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
			long curTime2 = PublicMethod.GetCurTime();
			if (curTime2 < charSubData3)
			{
				this.m_Label_Protecttime.Visible = true;
				string text2 = PublicMethod.ConvertTime(charSubData3 - curTime2);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("403"),
					"timestring",
					text2
				});
				this.m_Label_Protecttime.SetText(empty4);
			}
			else
			{
				this.m_Label_Protecttime.Visible = false;
			}
		}
	}

	public void BtnShowEffectPlunder(IUIObject obj)
	{
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			directionDLG.ReviewDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER);
		}
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
		if (this.m_eMode == eMODE.eMODE_INFIBATTLE)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
			long curTime = PublicMethod.GetCurTime();
			long time = charSubData - curTime;
			if (curTime < charSubData)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
					"timestring",
					PublicMethod.ConvertTime(time)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (instance != null)
			{
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
				if (myCharInfo.GetLevel() < value)
				{
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129"),
						"level",
						value
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return;
				}
				int infiBattleCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCount;
				int num = infiBattleCount - (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(21);
				if (num < 0)
				{
					num = 0;
				}
				TsLog.LogWarning(" Match Count , {0} : {1}", new object[]
				{
					num,
					infiBattleCount
				});
				if (infiBattleCount > 0 && num > 0)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "REMIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_INFIBATTLE;
					FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		else if (this.m_eMode == eMODE.eMODE_PLUNDER)
		{
			long charSubData2 = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_PLUNDER_DELAYTIME);
			long curTime2 = PublicMethod.GetCurTime();
			long num2 = charSubData2 - curTime2;
			if (num2 > 0L)
			{
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("136"),
					"timestring",
					PublicMethod.ConvertTime(num2)
				});
				Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (myCharInfo != null)
			{
				int level = myCharInfo.GetLevel();
				long num3;
				if (level > 50)
				{
					num3 = (long)(level * (level - COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD2)));
				}
				else
				{
					num3 = (long)(level * COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_MATCHGOLD));
				}
				if (num3 > myCharInfo.m_Money)
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
	}

	public void OnAttackMakeUp(IUIObject obj)
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP;
				FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
			}
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP;
			FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		}
	}

	public void OnDefenseMakeUp(IUIObject obj)
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "DEFEND-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP;
				FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
			}
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "DEFEND-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP;
			FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		}
	}

	public void OnProtect(IUIObject obj)
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode != eMODE.eMODE_INFIBATTLE)
			{
			}
		}
		else
		{
			PlunderProtectDlg plunderProtectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_PROTECT_DLG) as PlunderProtectDlg;
			if (plunderProtectDlg != null)
			{
				plunderProtectDlg.Show();
			}
		}
	}

	public void OnReport(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				GS_INFIBATTLE_RECORD_REQ gS_INFIBATTLE_RECORD_REQ = new GS_INFIBATTLE_RECORD_REQ();
				gS_INFIBATTLE_RECORD_REQ.i64PersonID = charPersonInfo.GetPersonID();
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_RECORD_REQ, gS_INFIBATTLE_RECORD_REQ);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "RECORD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}
		else
		{
			GS_PLUNDER_RECORD_LIST_GET_REQ gS_PLUNDER_RECORD_LIST_GET_REQ = new GS_PLUNDER_RECORD_LIST_GET_REQ();
			gS_PLUNDER_RECORD_LIST_GET_REQ.i64PersonID = charPersonInfo.GetPersonID();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RECORD_LIST_GET_REQ, gS_PLUNDER_RECORD_LIST_GET_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "RECORD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void OnRank(IUIObject obj)
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				GS_INFIBATTLE_RANK_REQ gS_INFIBATTLE_RANK_REQ = new GS_INFIBATTLE_RANK_REQ();
				gS_INFIBATTLE_RANK_REQ.i8Type = 0;
				gS_INFIBATTLE_RANK_REQ.i32StartRank = 1;
				gS_INFIBATTLE_RANK_REQ.i32RankCount = 10;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INFIBATTLE_RANK_REQ, gS_INFIBATTLE_RANK_REQ);
			}
		}
		else
		{
			GS_PLUNDER_RANKINFO_GET_REQ gS_PLUNDER_RANKINFO_GET_REQ = new GS_PLUNDER_RANKINFO_GET_REQ();
			eRANKREQ_TYPE eRANKREQ_TYPE = eRANKREQ_TYPE.eRANKREQ_TYPE_TOPANDME;
			gS_PLUNDER_RANKINFO_GET_REQ.ui8Rank_GetType = (byte)eRANKREQ_TYPE;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PLUNDER_RANKINFO_GET_REQ, gS_PLUNDER_RANKINFO_GET_REQ);
		}
		this.m_Label_Rank.SetEnabled(false);
		this.m_fRankRefreshTime = Time.time + this.REFRESH_TIME;
	}

	public void OnProtectTime(IUIObject obj)
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo != null)
				{
					InfiBattleReward infiBattleReward = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward;
					if (infiBattleReward != null)
					{
						GS_INFIBATTLE_REWARDINFO_REQ gS_INFIBATTLE_REWARDINFO_REQ = new GS_INFIBATTLE_REWARDINFO_REQ();
						gS_INFIBATTLE_REWARDINFO_REQ.i64PersonID = myCharInfo.m_PersonID;
						SendPacket.GetInstance().SendObject(2011, gS_INFIBATTLE_REWARDINFO_REQ);
					}
				}
			}
		}
		else
		{
			PlunderProtectDlg plunderProtectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_PROTECT_DLG) as PlunderProtectDlg;
			if (plunderProtectDlg != null)
			{
				plunderProtectDlg.Show();
			}
		}
	}

	public void SetMode(eMODE eMode)
	{
		this.m_eMode = eMode;
		this.ShowMode();
	}

	public eMODE GetMode()
	{
		return this.m_eMode;
	}

	public void ShowMode()
	{
		eMODE eMode = this.m_eMode;
		if (eMode != eMODE.eMODE_PLUNDER)
		{
			if (eMode == eMODE.eMODE_INFIBATTLE)
			{
				this.ShowInfiBattle();
			}
		}
		else
		{
			this.ShowPlunder();
		}
	}

	public void ShowType()
	{
		bool flag = false;
		if (this.m_eMode == eMODE.eMODE_INFIBATTLE)
		{
			flag = true;
		}
		this.m_tgMode[1].Visible = flag;
		this.m_DrawTexure_Active2.Visible = flag;
		this.m_Label_infinite.Visible = flag;
		this.m_DrawTexture_InfibattleIMG.Visible = flag;
		this.m_DrawTexture_InfibattleIMG2.Visible = flag;
		this.m_tgMode[0].Visible = !flag;
		this.m_DrawTexure_Active.Visible = !flag;
		this.m_DrawTexture_plunderIMG.Visible = !flag;
		this.m_DrawTexture_plunderIMG2.Visible = !flag;
		this.m_Label_Plunder.Visible = !flag;
	}

	public void ShowPlunder()
	{
		this.m_eMode = eMODE.eMODE_PLUNDER;
		this.ShowType();
		this.m_Button_Reward.Visible = false;
		this.m_Button_Protect.Visible = true;
		this.m_DrawTexure_Active.SetTexture("Win_I_PvPTitleBG03");
		this.m_DrawTexure_Active2.SetTexture("Win_I_PvPTitleBG01");
		this.m_DrawTexture_Attack.SetTexture("Win_B_PvPButtonBG02");
		this.m_DrawTexture_Defense.SetTexture("Win_B_PvPButtonBG02");
		this.m_DrawTexture_PvpReport.SetTexture("Win_B_PvPButtonBG02");
		this.m_DrawTexture_Ranking.SetTexture("Win_B_PvPButtonBG02");
		this.m_DrawTexture_Protect.SetTexture("Win_B_PvPButtonBG02");
		this.m_Label_Gold.Visible = true;
		this.m_Label_LimitCount.Visible = false;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2266");
		this.m_Label_Protect.SetText(textFromInterface);
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("611");
		this.m_Label_RankTitle.SetText(textFromInterface);
		if (null != this.m_Effect_HeroGlow)
		{
			this.m_Effect_HeroGlow.SetActive(true);
		}
		if (null != this.m_Effect_InfiBattle)
		{
			this.m_Effect_InfiBattle.SetActive(false);
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		string text = string.Empty;
		string text2 = string.Empty;
		if (myCharInfo.PlunderRank > 0)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("447");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text2,
				"rank",
				myCharInfo.PlunderRank
			});
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("292");
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			text2,
			"rank",
			myCharInfo.PlunderRank
		});
		this.m_Label_Rank.Text = text;
	}

	public void ShowInfiBattle()
	{
		this.m_eMode = eMODE.eMODE_INFIBATTLE;
		this.ShowType();
		this.m_Button_Reward.Visible = true;
		this.m_Button_Protect.Visible = false;
		this.m_DrawTexure_Active.SetTexture("Win_I_PvPTitleBG01");
		this.m_DrawTexure_Active2.SetTexture("Win_I_PvPTitleBG02");
		this.m_DrawTexture_Attack.SetTexture("Win_B_PvPButtonBG01");
		this.m_DrawTexture_Defense.SetTexture("Win_B_PvPButtonBG01");
		this.m_DrawTexture_PvpReport.SetTexture("Win_B_PvPButtonBG01");
		this.m_DrawTexture_Ranking.SetTexture("Win_B_PvPButtonBG01");
		this.m_DrawTexture_Protect.SetTexture("Win_B_PvPButtonBG01");
		this.m_Label_Gold.Visible = false;
		this.m_Label_LimitCount.Visible = true;
		if (null != this.m_Effect_HeroGlow)
		{
			this.m_Effect_HeroGlow.SetActive(false);
		}
		if (null != this.m_Effect_InfiBattle)
		{
			this.m_Effect_InfiBattle.SetActive(true);
		}
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
		bool flag = true;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
		long curTime = PublicMethod.GetCurTime();
		long time = charSubData - curTime;
		if (curTime < charSubData)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2219"),
				"timestring",
				PublicMethod.ConvertTime(time)
			});
			flag = false;
		}
		if (flag)
		{
			this.m_Label_Protecttime.SetText(string.Empty);
		}
		else
		{
			this.m_Label_Protecttime.SetText(textFromInterface);
		}
	}

	public void ClickTabControl(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		eMODE mode = (eMODE)((int)obj.Data);
		this.SetMode(mode);
		if (null != this.m_gbEffect_Fade)
		{
			this.m_gbEffect_Fade.SetActive(true);
			Animation componentInChildren = this.m_gbEffect_Fade.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				componentInChildren.Play();
			}
		}
	}

	public void SetTgValue(eMODE eMode)
	{
		if (!this.m_tgMode[(int)eMode].Value)
		{
			this.m_tgMode[(int)eMode].Value = true;
		}
	}

	public void LoadEffect()
	{
		string str = string.Format("{0}{1}", "Effect/Instant/fx_button_fight", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Fight), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		str = string.Format("{0}{1}", "Effect/Instant/fx_direct_fade", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Fade), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		str = string.Format("{0}{1}", "Effect/Instant/fx_button_heroglow", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Hero), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		str = string.Format("{0}{1}", "Effect/Instant/fx_button_infiniteglow", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Infi), null);
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
				Vector2 size = this.m_Button_PvpMatch.GetSize();
				this.m_gbEffect_Fight.transform.parent = this.m_Button_PvpMatch.gameObject.transform;
				this.m_gbEffect_Fight.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_Button_PvpMatch.gameObject.transform.localPosition.z - 0.1f);
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

	private void Effect_Hero(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_Effect_HeroGlow = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_Effect_HeroGlow);
					return;
				}
				Vector2 size = this.m_DrawTexture_HeroEffect.GetSize();
				this.m_Effect_HeroGlow.transform.parent = this.m_DrawTexture_HeroEffect.gameObject.transform;
				this.m_Effect_HeroGlow.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_DrawTexture_HeroEffect.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_Effect_HeroGlow, GUICamera.UILayer);
				this.m_Effect_HeroGlow.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_Effect_HeroGlow);
				}
				if (this.m_eMode == eMODE.eMODE_PLUNDER)
				{
					this.m_Effect_HeroGlow.SetActive(true);
				}
			}
		}
	}

	private void Effect_Infi(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_Effect_InfiBattle = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_Effect_InfiBattle);
					return;
				}
				Vector2 size = this.m_DrawTexture_InfiEffect.GetSize();
				this.m_Effect_InfiBattle.transform.parent = this.m_DrawTexture_InfiEffect.gameObject.transform;
				this.m_Effect_InfiBattle.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, this.m_DrawTexture_InfiEffect.gameObject.transform.localPosition.z - 0.1f);
				NkUtil.SetAllChildLayer(this.m_Effect_InfiBattle, GUICamera.UILayer);
				this.m_Effect_InfiBattle.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_Effect_InfiBattle);
				}
				if (this.m_eMode == eMODE.eMODE_INFIBATTLE)
				{
					this.m_Effect_InfiBattle.SetActive(true);
				}
			}
		}
	}
}
