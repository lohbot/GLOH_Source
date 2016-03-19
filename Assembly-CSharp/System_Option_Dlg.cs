using GAME;
using Ndoors.Framework.Stage;
using SERVICE;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class System_Option_Dlg : Form
{
	private static int MAX_OPTION_NUM = 3;

	private Toolbar m_Tap;

	private Toggle[] m_Quality = new Toggle[System_Option_Dlg.MAX_OPTION_NUM];

	private DrawTexture m_txRating1;

	private Label m_lbRating1;

	private Label m_lbRating2;

	private bool m_bQualityUpdate;

	private Button m_Save;

	private Button m_AutoQuality;

	private HorizontalSlider m_EffectSound;

	private HorizontalSlider m_BgmSound;

	private HorizontalSlider m_MobileRotate;

	private HorizontalSlider m_Fps;

	private CheckBox m_MuteEffect;

	private CheckBox m_MuteBgm;

	private CheckBox m_LocalPush_Activity;

	private CheckBox m_LocalPush_Injury;

	private CheckBox m_LocalPush_BattleMatchOpen;

	private CheckBox m_cbColosseumInvite;

	private CheckBox m_ReservedWord;

	private CheckBox m_BuffSkillText;

	private Button m_Credit;

	private CheckBox m_HideBookmark;

	private CheckBox m_ckEffect;

	private Label m_lbEffect1;

	private Label m_lbEffect2;

	private float m_fMaxCameraRotate = 0.6f;

	private float m_fMinCameraRotate = 0.15f;

	private Label tempLabel;

	private DrawTexture m_dtEffectTitle;

	private DrawTexture m_dtEffectBG;

	private float m_fOldBGM;

	private float m_fOldSFX;

	private bool m_bOldBGM;

	private bool m_bOldSFX;

	public static bool m_bSaveMode;

	private int tabIndex = 1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "System/DLG_System_Option", G_ID.SYSTEM_OPTION, true);
		base.ChangeSceneDestory = false;
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_Tap = (base.GetControl("ToolBar") as Toolbar);
		string[] array = new string[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("477"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("478"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("816"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("556")
		};
		for (int i = 0; i < array.Length; i++)
		{
			this.m_Tap.Control_Tab[i].Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), array[i]);
			UIPanelTab expr_A7 = this.m_Tap.Control_Tab[i];
			expr_A7.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_A7.ButtonClick, new EZValueChangedDelegate(this.ClickToolBar));
		}
		this.m_Tap.FirstSetting();
		this.m_Quality[0] = (base.GetControl("Toggle_RadioBtn1") as Toggle);
		this.m_Quality[0].SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickQuality));
		this.m_Quality[0].Data = TsQualityManager.Level.HIGHEST;
		this.m_Quality[1] = (base.GetControl("Toggle_RadioBtn3") as Toggle);
		this.m_Quality[1].SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickQuality));
		this.m_Quality[1].Data = TsQualityManager.Level.MEDIUM;
		this.m_Quality[2] = (base.GetControl("Toggle_RadioBtn5") as Toggle);
		this.m_Quality[2].SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickQuality));
		this.m_Quality[2].Data = TsQualityManager.Level.LOWEST;
		this.m_Save = (base.GetControl("ok_button") as Button);
		this.m_Save.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickSave));
		this.m_AutoQuality = (base.GetControl("reset_button") as Button);
		this.m_AutoQuality.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickAutoQuality));
		this.m_txRating1 = (base.GetControl("DrawTexture_Rating01") as DrawTexture);
		this.m_lbRating1 = (base.GetControl("Label_Rating01") as Label);
		this.m_lbRating2 = (base.GetControl("Label_Rating05") as Label);
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORNAVER)
		{
			this.m_txRating1.SetTexture("Win_I_Deliberation04");
			this.m_lbRating1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2863"));
			this.m_lbRating2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2864"));
		}
		else
		{
			this.m_txRating1.SetTexture("Win_I_Deliberation01");
			this.m_lbRating1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1560"));
			this.m_lbRating2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1611"));
		}
		this.m_Credit = (base.GetControl("Button_Credit") as Button);
		this.m_Credit.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickCredit));
		if (this.m_Credit != null)
		{
			this.m_Credit.Visible = false;
		}
		if (this.m_Tap.Control_Tab.Length >= 5 && this.m_Tap.Control_Tab[4] != null)
		{
			this.m_Tap.Control_Tab[4].Visible = false;
		}
		this.m_EffectSound = (base.GetControl("eftsound_HSlider") as HorizontalSlider);
		this.m_EffectSound.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickEffectSound));
		this.m_BgmSound = (base.GetControl("bgm_HSlider") as HorizontalSlider);
		this.m_BgmSound.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickBgmSound));
		this.m_MuteEffect = (base.GetControl("check_button1") as CheckBox);
		this.m_MuteEffect.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickMuteEffect));
		this.m_MuteBgm = (base.GetControl("check_button2") as CheckBox);
		this.m_MuteBgm.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickMuteBgm));
		this.m_LocalPush_Activity = (base.GetControl("CheckBox_Push01") as CheckBox);
		this.m_LocalPush_Activity.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickLocalPush_Activity));
		this.m_LocalPush_Injury = (base.GetControl("CheckBox_Push02") as CheckBox);
		this.m_LocalPush_Injury.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickLocalPush_Injury));
		this.m_LocalPush_BattleMatchOpen = (base.GetControl("CheckBox_Push03") as CheckBox);
		this.m_LocalPush_BattleMatchOpen.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickLocalPush_BattleMatch));
		this.m_cbColosseumInvite = (base.GetControl("CheckBox_Colosseum") as CheckBox);
		this.m_cbColosseumInvite.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickColosseumInvite));
		this.m_MobileRotate = (base.GetControl("Camera_HSlider") as HorizontalSlider);
		this.m_Fps = (base.GetControl("HSlider_FPS") as HorizontalSlider);
		if (PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_ACTIVITY, 1) == 0)
		{
			this.m_LocalPush_Activity.SetCheckState(0);
		}
		else
		{
			this.m_LocalPush_Activity.SetCheckState(1);
		}
		if (PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_INJURYTIME, 0) == 0)
		{
			this.m_LocalPush_Injury.SetCheckState(0);
		}
		else
		{
			this.m_LocalPush_Injury.SetCheckState(1);
		}
		if (PlayerPrefs.GetInt(NrPrefsKey.LOCALPUSH_MATCHOPEN, 0) == 0)
		{
			this.m_LocalPush_BattleMatchOpen.SetCheckState(0);
		}
		else
		{
			this.m_LocalPush_BattleMatchOpen.SetCheckState(1);
		}
		this.m_dtEffectTitle = (base.GetControl("DrawTexture_SubTitleBG03") as DrawTexture);
		this.m_dtEffectBG = (base.GetControl("DrawTexture_AreaBg03") as DrawTexture);
		this.SetCameraRotate();
		this.SetFps();
		this.m_ReservedWord = (base.GetControl("CheckBox_Slang") as CheckBox);
		if (!PlayerPrefs.HasKey(NrPrefsKey.RESERVED_WORD))
		{
			PlayerPrefs.SetInt(NrPrefsKey.RESERVED_WORD, 1);
			this.m_ReservedWord.SetCheckState(1);
			NrTSingleton<ReservedWordManager>.Instance.SetUse(true);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.RESERVED_WORD) == 0)
		{
			this.m_ReservedWord.SetCheckState(0);
			NrTSingleton<ReservedWordManager>.Instance.SetUse(false);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.RESERVED_WORD) == 1)
		{
			this.m_ReservedWord.SetCheckState(1);
			NrTSingleton<ReservedWordManager>.Instance.SetUse(true);
		}
		this.m_ReservedWord.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickReservedWord));
		this.m_BuffSkillText = (base.GetControl("CheckBox_SkillText") as CheckBox);
		if (!PlayerPrefs.HasKey(NrPrefsKey.SKILL_TEXT))
		{
			PlayerPrefs.SetInt(NrPrefsKey.SKILL_TEXT, 1);
			this.m_BuffSkillText.SetCheckState(1);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBuffSkillTextUse(true);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.SKILL_TEXT) == 0)
		{
			this.m_BuffSkillText.SetCheckState(0);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBuffSkillTextUse(false);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.SKILL_TEXT) == 1)
		{
			this.m_BuffSkillText.SetCheckState(1);
			NrTSingleton<BattleSkill_Manager>.Instance.SetBuffSkillTextUse(true);
		}
		this.m_BuffSkillText.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkillText));
		this.m_HideBookmark = (base.GetControl("CheckBox_HideBookmark") as CheckBox);
		if (!PlayerPrefs.HasKey(NrPrefsKey.HIDE_BOOKMARK))
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_BOOKMARK, 1);
			if (this.m_HideBookmark != null)
			{
				this.m_HideBookmark.SetCheckState(1);
			}
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.HIDE_BOOKMARK) == 0)
		{
			this.m_HideBookmark.SetCheckState(0);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.HIDE_BOOKMARK) == 1)
		{
			this.m_HideBookmark.SetCheckState(1);
		}
		this.m_HideBookmark.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHideBookmark));
		this.m_lbEffect1 = (base.GetControl("Lable_Effect1") as Label);
		this.m_lbEffect2 = (base.GetControl("Lable_Effect2") as Label);
		this.m_lbEffect1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2742"));
		this.m_lbEffect2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2743"));
		this.m_ckEffect = (base.GetControl("CheckBox_Effect") as CheckBox);
		this.m_ckEffect.SetCheckState(NrTSingleton<NrUserDeviceInfo>.Instance.GetHotKey());
		this.m_ckEffect.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEffect));
		base.ShowLayer(1);
		this.m_ckEffect.Visible = false;
		this.m_lbEffect1.Visible = false;
		this.m_lbEffect2.Visible = false;
		this.m_dtEffectTitle.Visible = false;
		this.m_dtEffectBG.Visible = false;
		this.LoadOption();
		base.SetScreenCenter();
		if (Scene.CurScene == Scene.Type.WORLD)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SYSTEM", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private void ClickHideBookmark(IUIObject obj)
	{
		if (this.m_HideBookmark.IsChecked())
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_BOOKMARK, 1);
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_BOOKMARK, 0);
		}
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.CheckHideBookmark();
		}
	}

	public void CheckColosseumInvite()
	{
		if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsAtbCommonFlag(1L))
		{
			this.m_cbColosseumInvite.SetCheckState(0);
		}
		else
		{
			this.m_cbColosseumInvite.SetCheckState(1);
		}
	}

	private void ClickCredit(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CREDIT_DLG);
	}

	private void ClickEffect(IUIObject obj)
	{
		if (this.m_ckEffect.IsChecked())
		{
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("251"), eMsgType.MB_OK, new YesDelegate(this.On_MessageBok_OK), null);
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_SOFT_KEY, 0);
		}
	}

	private void On_MessageBok_OK(object a_oObject)
	{
		if (this.m_ckEffect.IsChecked())
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_SOFT_KEY, 1);
		}
		else
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_SOFT_KEY, 0);
		}
	}

	private void On_MessageBok_Cancle(object a_oObject)
	{
		if (this.m_ckEffect.IsChecked())
		{
			this.m_ckEffect.SetCheckState(0);
		}
		else
		{
			this.m_ckEffect.SetCheckState(1);
		}
	}

	private void ClickReservedWord(IUIObject obj)
	{
		if (this.m_ReservedWord.IsChecked())
		{
			NrTSingleton<ReservedWordManager>.Instance.SetUse(true);
			PlayerPrefs.SetInt(NrPrefsKey.RESERVED_WORD, 1);
		}
		else
		{
			NrTSingleton<ReservedWordManager>.Instance.SetUse(false);
			PlayerPrefs.SetInt(NrPrefsKey.RESERVED_WORD, 0);
		}
	}

	private void ClickSkillText(IUIObject obj)
	{
		if (this.m_BuffSkillText.IsChecked())
		{
			NrTSingleton<BattleSkill_Manager>.Instance.SetBuffSkillTextUse(true);
			PlayerPrefs.SetInt(NrPrefsKey.SKILL_TEXT, 1);
		}
		else
		{
			NrTSingleton<BattleSkill_Manager>.Instance.SetBuffSkillTextUse(false);
			PlayerPrefs.SetInt(NrPrefsKey.SKILL_TEXT, 0);
		}
	}

	public void SetCameraRotate()
	{
		float num = 0f;
		if (PlayerPrefs.HasKey("CameraRotate"))
		{
			num = PlayerPrefs.GetFloat("CameraRotate");
			if (num <= this.m_fMinCameraRotate)
			{
				num = 0f;
			}
			if (num >= this.m_fMaxCameraRotate)
			{
				num = 1f;
			}
		}
		else
		{
			maxCamera component = Camera.main.GetComponent<maxCamera>();
			if (component != null)
			{
				num = component.MobileRotate;
			}
		}
		this.m_MobileRotate.defaultValue = num;
		this.m_MobileRotate.Value = num;
	}

	public void SetFps()
	{
		int num = NmMainFrameWork.MAX_FPS - NmMainFrameWork.MIN_FPS;
		if (PlayerPrefs.HasKey("SaveFps"))
		{
			int num2 = PlayerPrefs.GetInt("SaveFps");
			if (num2 <= NmMainFrameWork.MIN_FPS)
			{
				num2 = NmMainFrameWork.MIN_FPS;
			}
			if (num2 >= NmMainFrameWork.MAX_FPS)
			{
				num2 = NmMainFrameWork.MAX_FPS;
			}
			num2 -= NmMainFrameWork.MIN_FPS;
			this.m_Fps.defaultValue = (float)num2 / (float)num;
			this.m_Fps.Value = (float)num2 / (float)num;
		}
		else
		{
			this.m_Fps.defaultValue = 1f;
			this.m_Fps.Value = 1f;
			this.SaveFps();
		}
	}

	public void SaveCameraRotate()
	{
		float num = this.m_MobileRotate.Value;
		if (num < this.m_fMinCameraRotate)
		{
			num = this.m_fMinCameraRotate;
		}
		if (num > this.m_fMaxCameraRotate)
		{
			num = this.m_fMaxCameraRotate;
		}
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (component != null)
		{
			component.MobileRotate = num;
		}
		PlayerPrefs.SetFloat("CameraRotate", num);
	}

	public void SaveFps()
	{
		float value = this.m_Fps.Value;
		int num = NmMainFrameWork.MAX_FPS - NmMainFrameWork.MIN_FPS;
		int num2 = NmMainFrameWork.MIN_FPS + (int)((float)num * value);
		if (num2 < NmMainFrameWork.MIN_FPS)
		{
			num2 = NmMainFrameWork.MIN_FPS;
		}
		if (value > (float)NmMainFrameWork.MAX_FPS)
		{
			num2 = NmMainFrameWork.MAX_FPS;
		}
		PlayerPrefs.SetInt("SaveFps", num2);
		Application.targetFrameRate = num2;
	}

	public void CameraRotate_Reset()
	{
		float num = 0.37f;
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (component != null)
		{
			component.MobileRotate = num;
			this.m_MobileRotate.defaultValue = num;
			this.m_MobileRotate.Value = num;
		}
		PlayerPrefs.SetFloat("CameraRotate", num);
	}

	private void ClickLocalPush_Activity(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		if (checkBox.IsChecked())
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME, true);
		}
		else
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_ACTIVITYTIME, false);
		}
	}

	private void ClickLocalPush_Injury(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		if (checkBox.IsChecked())
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME, true);
		}
		else
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME, false);
		}
	}

	private void ClickLocalPush_BattleMatch(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		if (checkBox.IsChecked())
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME, true);
		}
		else
		{
			NrTSingleton<NkLocalPushManager>.Instance.SetPushSetting(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_BATTLEMATCHTIME, false);
		}
	}

	private void ClickColosseumInvite(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		if (checkBox.IsChecked())
		{
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetAtbCommonFlag(1L);
		}
		else
		{
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.DelAtbCommonFlag(1L);
		}
	}

	private void ClickEffectSound(IUIObject obj)
	{
	}

	private void ClickBgmSound(IUIObject obj)
	{
	}

	private void ClickMuteEffect(IUIObject obj)
	{
	}

	private void ClickMuteBgm(IUIObject obj)
	{
	}

	private void ClickCameraRotate(IUIObject obj)
	{
	}

	private void ClickSave(IUIObject obj)
	{
		TsQualityManager.Instance.SaveCustomSettings();
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				float value;
				bool flag;
				if (eAudioType == EAudioType.BGM)
				{
					value = this.m_BgmSound.Value;
					flag = this.m_MuteBgm.IsChecked();
					UIDataManager.MuteBGM = flag;
				}
				else
				{
					value = this.m_EffectSound.Value;
					flag = this.m_MuteEffect.IsChecked();
					UIDataManager.MuteEffect = flag;
				}
				TsAudio.SetVolumeOfAudioType(eAudioType, value);
				TsAudio.SetMuteAudioType(eAudioType, flag);
			}
		}
		TsAudio.SavePlayerPrefs();
		this.SaveCameraRotate();
		this.SaveFps();
		this.CloseForm(null);
	}

	private void ClickAutoQuality(IUIObject obj)
	{
		if (this.tabIndex == 1)
		{
			TsQualityManager.Instance.RecoveryCustomSettings();
			int optimizedQualityLevel = (int)NrHardwareIndex.GetOptimizedQualityLevel();
			int num = Mathf.Clamp(optimizedQualityLevel, 0, this.m_Quality.Length - 1);
			this.m_Quality[num].Value = true;
			this.Graphic_Reset();
		}
		else if (this.tabIndex == 2)
		{
			this.Sound_Reset();
		}
		else if (this.tabIndex == 4)
		{
			this.CameraRotate_Reset();
		}
	}

	private void ClickTexture(IUIObject obj)
	{
		if (this.m_bQualityUpdate)
		{
			return;
		}
		Toggle toggle = (Toggle)obj;
		if (null == toggle)
		{
			return;
		}
		if (toggle.Value)
		{
			TsQualityManager.Instance.CurrQuality.TextureQuality = (TsQualityManager.TextureQuality)((int)toggle.Data);
			TsQualityManager.Instance.Refresh();
		}
	}

	private void ClickQuality(IUIObject obj)
	{
		Toggle toggle = (Toggle)obj;
		if (null == toggle)
		{
			return;
		}
		if (toggle.Value)
		{
			TsQualityManager.Level qualitySettings = (TsQualityManager.Level)((int)toggle.Data);
			CustomQuality.GetInstance().SetQualitySettings(qualitySettings);
			this.Graphic_Reset();
		}
	}

	private void ClickKaKaoUnregister(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowMessageBox(string.Empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("205"), eMsgType.MB_OK_CANCEL, new YesDelegate(this.onKaKaoUnregister), null);
	}

	private void onKaKaoUnregister(object a_oObject)
	{
		NrTSingleton<NkClientLogic>.Instance.RequestOTPAuthKey(eOTPRequestType.OTPREQ_UNREGISTER);
	}

	private void ClickToolBar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.tabIndex = uIPanelTab.panel.index + 1;
		base.ShowLayer(this.tabIndex);
		this.m_ckEffect.Visible = false;
		this.m_lbEffect1.Visible = false;
		this.m_lbEffect2.Visible = false;
		this.m_dtEffectTitle.Visible = false;
		this.m_dtEffectBG.Visible = false;
	}

	private void LoadOption()
	{
		TsQualityManager.Level currLevel = TsQualityManager.Instance.CurrLevel;
		int num;
		if (currLevel == TsQualityManager.Level.HIGH || currLevel == TsQualityManager.Level.HIGHEST)
		{
			num = 0;
		}
		else if (currLevel == TsQualityManager.Level.LOW || currLevel == TsQualityManager.Level.LOWEST)
		{
			num = 2;
		}
		else
		{
			num = 1;
		}
		this.m_Quality[num].Value = true;
		try
		{
			float volumeOfAudio = TsAudio.GetVolumeOfAudio(EAudioType.SFX);
			this.m_EffectSound.defaultValue = volumeOfAudio;
			this.m_EffectSound.Value = volumeOfAudio;
			this.m_fOldSFX = volumeOfAudio;
		}
		catch (Exception)
		{
			this.m_EffectSound.Value = 1f;
		}
		try
		{
			bool flag = TsAudio.IsMuteAudio(EAudioType.SFX);
			this.m_MuteEffect.SetCheckState((!flag) ? 0 : 1);
			this.m_bOldSFX = flag;
			UIDataManager.MuteEffect = flag;
		}
		catch (Exception)
		{
			this.m_MuteEffect.SetCheckState(0);
		}
		try
		{
			float volumeOfAudio2 = TsAudio.GetVolumeOfAudio(EAudioType.BGM);
			this.m_BgmSound.defaultValue = volumeOfAudio2;
			this.m_BgmSound.Value = volumeOfAudio2;
			this.m_fOldBGM = volumeOfAudio2;
		}
		catch (Exception)
		{
			this.m_BgmSound.Value = 1f;
		}
		try
		{
			bool flag2 = TsAudio.IsMuteAudio(EAudioType.BGM);
			this.m_MuteBgm.SetCheckState((!flag2) ? 0 : 1);
			this.m_bOldBGM = flag2;
			UIDataManager.MuteBGM = flag2;
		}
		catch (Exception)
		{
			this.m_MuteBgm.SetCheckState(0);
		}
	}

	private void Graphic_Reset()
	{
		this.m_bQualityUpdate = true;
		System_Option_Dlg.CallTsQualityManagerRefresh();
		this.m_bQualityUpdate = false;
	}

	public static void CallTsQualityManagerRefresh()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			bool fakeShadowEnable = TsQualityManager.Instance.CurrLevel <= TsQualityManager.Level.LOW;
			if (TsPlatform.IsMobile)
			{
				fakeShadowEnable = true;
			}
			NrTSingleton<NkCharManager>.Instance.SetFakeShadowEnable(fakeShadowEnable);
		}
		TsQualityManager.Instance.Refresh();
	}

	private void Sound_Reset()
	{
		this.m_MuteEffect.SetCheckState(0);
		this.m_MuteBgm.SetCheckState(0);
		this.m_EffectSound.Value = 1f;
		this.m_BgmSound.Value = 1f;
		this.Set_Sound();
	}

	private void Set_Sound()
	{
		for (int i = 0; i < 8; i++)
		{
			if (i == 1)
			{
				if (this.m_MuteBgm.IsChecked())
				{
					TsAudio.SetMuteAudioType((EAudioType)i, true);
				}
				else
				{
					bool mute = false;
					if (this.m_BgmSound.Value <= 0.001f)
					{
						mute = true;
					}
					TsAudio.SetMuteAudioType((EAudioType)i, mute);
					TsAudio.SetVolumeOfAudioType((EAudioType)i, this.m_BgmSound.Value);
				}
			}
			else if (this.m_MuteEffect.IsChecked())
			{
				TsAudio.SetMuteAudioType((EAudioType)i, true);
			}
			else
			{
				bool mute2 = false;
				if (this.m_EffectSound.Value <= 0.001f)
				{
					mute2 = true;
				}
				TsAudio.SetMuteAudioType((EAudioType)i, mute2);
				TsAudio.SetVolumeOfAudioType((EAudioType)i, this.m_EffectSound.Value);
			}
		}
		TsAudio.RefreshAllAudioVolumes();
		TsAudio.RefreshAllMuteAudio();
	}

	public override void OnClose()
	{
		if (Scene.CurScene == Scene.Type.WORLD)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SYSTEM", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public override void Update()
	{
		bool flag = false;
		if (this.m_MuteBgm.IsChecked() != this.m_bOldBGM)
		{
			flag = true;
		}
		else if (this.m_MuteEffect.IsChecked() != this.m_bOldSFX)
		{
			flag = true;
		}
		else if (this.m_BgmSound.Value != this.m_fOldBGM)
		{
			flag = true;
		}
		else if (this.m_EffectSound.Value != this.m_fOldSFX)
		{
			flag = true;
		}
		if (flag)
		{
			this.m_bOldBGM = this.m_MuteBgm.IsChecked();
			this.m_bOldSFX = this.m_MuteEffect.IsChecked();
			this.m_fOldBGM = this.m_BgmSound.Value;
			this.m_fOldSFX = this.m_EffectSound.Value;
			this.Set_Sound();
		}
	}
}
