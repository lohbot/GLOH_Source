using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class DailyDungeon_Main_Dlg : Form
{
	private Label m_lbTitle;

	private Label m_lbProgress;

	private Label m_lbReward;

	private ItemTexture m_itRewardItem;

	private Button m_btChangeDifficulty;

	private Button m_btReward;

	private Button m_btStart;

	private Button m_btExit;

	private DrawTexture m_dtBG;

	private DrawTexture m_dtDifficulty;

	private DrawTexture m_dtCleraDifficulty;

	private DrawTexture m_dtClearGage;

	private DrawTexture m_dwActivity;

	private Label m_lb_WillNum;

	private Label m_lbActivityTime;

	private Button m_btActivityCharge;

	private float m_fActivityUpdateTime;

	private long m_nBeforeActivity = -1L;

	private int m_nBaseActivity;

	private sbyte m_nDifficult = -1;

	private string m_szBackImage = string.Empty;

	private GameObject m_goPlayAni;

	private GameObject m_goAnimation;

	private GameObject m_goBackTexture;

	private bool m_bAniPlay;

	private bool m_bRestoreReserve;

	private GameObject m_goRewardEffect;

	private float m_fGageMax;

	private GameObject m_goGageEffect;

	private GameObject SlotEffect;

	public sbyte Difficult
	{
		get
		{
			return this.m_nDifficult;
		}
	}

	public bool RestoreReserve
	{
		get
		{
			return this.m_bRestoreReserve;
		}
		set
		{
			this.m_bRestoreReserve = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DailyDungeon/DLG_Dungeon_Main", G_ID.DAILYDUNGEON_MAIN, false);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Title_Label") as Label);
		this.m_dtDifficulty = (base.GetControl("DrawTexture_Difficulty") as DrawTexture);
		this.m_lbProgress = (base.GetControl("Label_Progress2") as Label);
		this.m_dtCleraDifficulty = (base.GetControl("DrawTexture_ClearDifficult") as DrawTexture);
		this.m_lbReward = (base.GetControl("Label_Reward") as Label);
		this.m_dtBG = (base.GetControl("Main_BG") as DrawTexture);
		this.m_itRewardItem = (base.GetControl("ItemTexture_reward") as ItemTexture);
		this.m_btChangeDifficulty = (base.GetControl("Button_Difficulty") as Button);
		Button expr_B6 = this.m_btChangeDifficulty;
		expr_B6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B6.Click, new EZValueChangedDelegate(this.OnClickChangeDifficulty));
		this.m_btReward = (base.GetControl("Btn_GetReward") as Button);
		Button expr_F3 = this.m_btReward;
		expr_F3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F3.Click, new EZValueChangedDelegate(this.OnRewardReq));
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_STARTBUTTON_UI", this.m_btReward, this.m_btReward.GetSize());
		this.m_btReward.AddGameObjectDelegate(new EZGameObjectDelegate(this.RewardEffectDelegate));
		this.m_btStart = (base.GetControl("Start_Btn") as Button);
		Button expr_167 = this.m_btStart;
		expr_167.Click = (EZValueChangedDelegate)Delegate.Combine(expr_167.Click, new EZValueChangedDelegate(this.OnClickStart));
		this.m_btExit = (base.GetControl("Exit_Btn") as Button);
		Button expr_1A4 = this.m_btExit;
		expr_1A4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1A4.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_dtClearGage = (base.GetControl("DrawTexture_Prg") as DrawTexture);
		this.m_fGageMax = this.m_dtClearGage.GetSize().x;
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_STARTBUTTON_UI", this.m_dtClearGage, this.m_dtClearGage.GetSize());
		this.m_dtClearGage.AddGameObjectDelegate(new EZGameObjectDelegate(this.ProgressDrawTextureDelegate));
		this.m_btActivityCharge = (base.GetControl("Button_WillCharge1") as Button);
		Button expr_247 = this.m_btActivityCharge;
		expr_247.Click = (EZValueChangedDelegate)Delegate.Combine(expr_247.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		this.m_nBaseActivity = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BASE_ACTIVITY);
		this.m_dwActivity = (base.GetControl("DrawTexture_will1") as DrawTexture);
		this.m_lbActivityTime = (base.GetControl("Will_Time_Label") as Label);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		this.m_btActivityCharge = (base.GetControl("Button_WillCharge1") as Button);
		this._SetDialogPos();
		this.SetBG();
		this.SetBasicData();
		string str = string.Format("{0}", "effect/instant/fx_direct_daydungeon" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.PlayAni), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(0f, 0f);
		float width = GUICamera.width;
		float height = GUICamera.height;
		if (this.m_dtBG != null)
		{
			this.m_dtBG.SetSize(width, height);
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_goPlayAni != null)
		{
			this.m_goAnimation = null;
			this.m_goBackTexture = null;
			UnityEngine.Object.Destroy(this.m_goPlayAni);
		}
		if (this.m_goGageEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goGageEffect);
			this.m_goGageEffect = null;
		}
		if (this.m_goRewardEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goRewardEffect);
			this.m_goRewardEffect = null;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_DIFFICULTY) != null)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DAILYDUNGEON_DIFFICULTY);
		}
	}

	public override void Update()
	{
		base.Update();
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg != null)
		{
			myCharInfoDlg.Update();
		}
		if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
		{
			this.m_lbActivityTime.SetText(myCharInfoDlg.StrActivityTime);
			this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 0.5f;
			this.SetActivityPointUI();
		}
		if (this.m_bAniPlay && !this.m_goAnimation.animation.isPlaying)
		{
			GS_EVENT_DAILYDUNGEON_OPEN_REQ gS_EVENT_DAILYDUNGEON_OPEN_REQ = new GS_EVENT_DAILYDUNGEON_OPEN_REQ();
			gS_EVENT_DAILYDUNGEON_OPEN_REQ.nDifficulty = (byte)this.m_nDifficult;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EVENT_DAILYDUNGEON_OPEN_REQ, gS_EVENT_DAILYDUNGEON_OPEN_REQ);
			this.m_bAniPlay = false;
			this.m_bRestoreReserve = true;
			UIDataManager.MuteSound(false);
		}
	}

	public void RestoreDailyDungeonDlg()
	{
		if (this.m_goPlayAni != null)
		{
			this.m_goPlayAni.SetActive(false);
		}
		this.m_bRestoreReserve = false;
		base.SetShowLayer(1, true);
		this.SetBasicData();
		UIDataManager.MuteSound(false);
	}

	public void SetClearEffect(sbyte nClearInfo, sbyte nTotalInfo)
	{
		if ((int)nClearInfo == (int)nTotalInfo)
		{
			string str = string.Format("{0}{1}", "effect/instant/fx_dungeonclear_ui", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this._funcUIEffectDownloaded), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
	}

	public void SetActivityPointUI()
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg == null)
		{
			return;
		}
		if (this.m_nBeforeActivity == myCharInfoDlg.CurrentActivity)
		{
			return;
		}
		this.m_nBeforeActivity = myCharInfoDlg.CurrentActivity;
		string empty = string.Empty;
		if (myCharInfoDlg.CurrentActivity > myCharInfoDlg.MaxActivity)
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				textColor + myCharInfoDlg.CurrentActivity.ToString() + textColor2,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				myCharInfoDlg.CurrentActivity,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		this.m_lb_WillNum.SetText(empty);
	}

	public void OnClickWillCharge(IUIObject obj)
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg == null)
		{
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (myCharInfoDlg.CurrentActivity >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickChangeDifficulty(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		sbyte nDayOfWeek = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventWeek();
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_DAILY_DUNGEON);
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = charSubData;
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(this.m_nDifficult, nDayOfWeek);
		if ((int)sUBDATA_UNION.n8SubData_1 >= (int)dailyDungeonInfo.i8TotalCount)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("602"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_DIFFICULTY) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_DIFFICULTY);
		}
	}

	public void SetDifficuly(sbyte nDifficult)
	{
		if ((int)this.m_nDifficult == (int)nDifficult)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		this.m_nDifficult = nDifficult;
		kMyCharInfo.SetCharSubData(26, 0L);
		this.SetBasicData();
	}

	public void SetBG()
	{
		sbyte nDayOfWeek = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventWeek();
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(1, nDayOfWeek);
		if (dailyDungeonInfo == null)
		{
			this.Close();
			return;
		}
		this.m_szBackImage = "UI/DailyDungeon/" + dailyDungeonInfo.szBGIMG;
		this.m_dtBG.SetTextureFromBundle(this.m_szBackImage);
		if (this.m_goBackTexture != null)
		{
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_szBackImage);
			if (texture == null)
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(this.m_szBackImage, new PostProcPerItem(this.SetBundleImage));
			}
			else
			{
				Renderer component = this.m_goBackTexture.GetComponent<Renderer>();
				if (component != null)
				{
					component.material.mainTexture = texture;
				}
			}
		}
	}

	public void SetBasicData()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		sbyte b = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventDay();
		sbyte b2 = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventWeek();
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_DAILY_DUNGEON_CLEARINFO);
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = charSubData;
		sbyte b3 = 0;
		if ((int)b2 == 0)
		{
			b3 = sUBDATA_UNION.n8SubData_0;
		}
		else if ((int)b2 == 1)
		{
			b3 = sUBDATA_UNION.n8SubData_1;
		}
		else if ((int)b2 == 2)
		{
			b3 = sUBDATA_UNION.n8SubData_2;
		}
		else if ((int)b2 == 3)
		{
			b3 = sUBDATA_UNION.n8SubData_3;
		}
		else if ((int)b2 == 4)
		{
			b3 = sUBDATA_UNION.n8SubData_4;
		}
		else if ((int)b2 == 5)
		{
			b3 = sUBDATA_UNION.n8SubData_5;
		}
		else if ((int)b2 == 6)
		{
			b3 = sUBDATA_UNION.n8SubData_6;
		}
		if ((int)b3 > 0)
		{
			this.m_dtCleraDifficulty.SetTexture("Win_I_WorrGradeS" + b3.ToString());
		}
		long num = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_DAILY_DUNGEON);
		SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
		sUBDATA_UNION2.nSubData = num;
		if (num != 0L && ((int)sUBDATA_UNION2.n8SubData_2 != (int)b2 || (int)sUBDATA_UNION2.n8SubData_3 != (int)b))
		{
			kMyCharInfo.SetCharSubData(26, 0L);
			num = 0L;
			sUBDATA_UNION2.nSubData = 0L;
			sUBDATA_UNION2.n8SubData_2 = b2;
			sUBDATA_UNION2.n8SubData_3 = b;
		}
		if ((int)this.m_nDifficult < 0)
		{
			if ((int)sUBDATA_UNION2.n8SubData_0 == 0)
			{
				this.m_nDifficult = b3;
			}
			else
			{
				this.m_nDifficult = sUBDATA_UNION2.n8SubData_0;
			}
		}
		if ((int)this.m_nDifficult == 0)
		{
			this.m_nDifficult = 1;
		}
		sUBDATA_UNION2.n8SubData_0 = this.m_nDifficult;
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(this.m_nDifficult, b2);
		if (dailyDungeonInfo == null)
		{
			this.Close();
			return;
		}
		string textFromMap = NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(dailyDungeonInfo.i32TextKey.ToString());
		this.m_lbTitle.SetText(textFromMap);
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = dailyDungeonInfo.i32RewardItemUnique;
		iTEM.m_nItemNum = dailyDungeonInfo.i32RewardItemNum;
		this.m_itRewardItem.SetItemTexture(iTEM);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(dailyDungeonInfo.i32RewardItemUnique),
			"count",
			dailyDungeonInfo.i32RewardItemNum.ToString()
		});
		this.m_lbReward.SetText(empty);
		this.m_dtDifficulty.SetTexture("Win_I_WorrGradeS" + this.m_nDifficult.ToString());
		if ((int)sUBDATA_UNION2.n8SubData_1 <= (int)dailyDungeonInfo.i8TotalCount)
		{
			if ((int)sUBDATA_UNION2.n8SubData_1 != (int)dailyDungeonInfo.i8TotalCount)
			{
				this.m_btStart.Visible = true;
				this.m_btReward.Visible = false;
				float num2 = (float)sUBDATA_UNION2.n8SubData_1 / (float)dailyDungeonInfo.i8TotalCount;
				int num3 = (int)(num2 * 100f);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
					"Count",
					num3.ToString()
				});
				this.m_lbProgress.SetText(empty);
				this.m_dtClearGage.SetSize(this.m_fGageMax * num2, this.m_dtClearGage.height);
				if (this.m_goGageEffect != null)
				{
					this.m_goGageEffect.SetActive(false);
				}
			}
			else
			{
				this.m_btStart.Visible = false;
				this.m_btReward.Visible = true;
				this.m_dtClearGage.SetSize(this.m_fGageMax, this.m_dtClearGage.height);
				this.m_lbProgress.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2012"));
				if (this.m_goGageEffect != null)
				{
					this.m_goGageEffect.SetActive(true);
				}
			}
		}
		else
		{
			this.m_btStart.Visible = false;
			this.m_btReward.Visible = false;
			this.m_lbProgress.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2012"));
			this.m_dtClearGage.SetSize(this.m_fGageMax, this.m_dtClearGage.height);
			if (this.m_goGageEffect != null)
			{
				this.m_goGageEffect.SetActive(true);
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.MAINEVENT);
			}
		}
		if (sUBDATA_UNION2.nSubData != num)
		{
			kMyCharInfo.SetCharSubData(26, sUBDATA_UNION2.nSubData);
		}
	}

	public void OnRewardReq(IUIObject obj)
	{
		GS_GET_EVENT_REWARD_REQ gS_GET_EVENT_REWARD_REQ = new GS_GET_EVENT_REWARD_REQ();
		gS_GET_EVENT_REWARD_REQ.m_nEventType = 13;
		SendPacket.GetInstance().SendObject(1664, gS_GET_EVENT_REWARD_REQ);
	}

	public void OnClickStart(IUIObject obj)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (!kMyCharInfo.IsEnableBattleUseActivityPoint(1))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NrPersonInfoUser personInfoUser = nrCharUser.GetPersonInfoUser();
		if (personInfoUser == null)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 6; i++)
		{
			if (kMyCharInfo.IsAddBattleSoldier(i))
			{
				NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
				if (soldierInfo == null || !soldierInfo.IsValid())
				{
					if (!flag)
					{
						flag = true;
					}
				}
				else
				{
					num++;
				}
				num2++;
			}
		}
		if (flag)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("146"),
				"currentnum",
				num.ToString(),
				"maxnum",
				num2.ToString()
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnBattleOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), empty, eMsgType.MB_OK_CANCEL);
			return;
		}
		bool flag2 = false;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				if (soldierInfo.IsInjuryStatus())
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag2)
		{
			MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI2.SetMsg(new YesDelegate(this.OnBattleInjuryOk), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("20"), eMsgType.MB_OK_CANCEL);
			return;
		}
		this.OnBattleOK(null);
	}

	public void OnBattleOK(object a_oObject)
	{
		if (this.m_bAniPlay)
		{
			return;
		}
		if (this.m_goPlayAni == null)
		{
			return;
		}
		this.m_goPlayAni.SetActive(true);
		this.m_goAnimation.animation.Play();
		UIDataManager.MuteSound(true);
		this.m_bAniPlay = true;
		base.SetShowLayer(1, false);
	}

	public void OnBattleInjuryOk(object a_oObject)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLMILITARYGROUP_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
		}
	}

	private void PlayAni(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (this == null)
		{
			return;
		}
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_goPlayAni = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_goPlayAni);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.m_goPlayAni.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_goPlayAni, GUICamera.UILayer);
				this.m_goAnimation = NkUtil.GetChild(this.m_goPlayAni.transform, "fx_dungeon").gameObject;
				this.m_goBackTexture = NkUtil.GetChild(this.m_goPlayAni.transform, "fx_plan_background").gameObject;
				if (this.m_goBackTexture != null)
				{
					Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_szBackImage);
					if (texture == null)
					{
						NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(this.m_szBackImage, new PostProcPerItem(this.SetBundleImage));
					}
					else
					{
						Renderer component = this.m_goBackTexture.GetComponent<Renderer>();
						if (component != null)
						{
							component.material.mainTexture = texture;
						}
					}
				}
				this.m_goPlayAni.SetActive(false);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goPlayAni);
				}
			}
		}
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
					Renderer component = this.m_goBackTexture.GetComponent<Renderer>();
					if (component != null)
					{
						component.material.mainTexture = texture2D;
					}
				}
			}
		}
	}

	public void RewardEffectDelegate(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		obj.transform.localScale = new Vector3(2f, 1.3f, 1f);
		this.m_goRewardEffect = obj;
	}

	public void ProgressDrawTextureDelegate(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goGageEffect = obj;
		this.m_goGageEffect.transform.localScale = new Vector3(1f, 0.35f, 1f);
		if (this.m_dtClearGage.GetSize().x != this.m_fGageMax)
		{
			this.m_goGageEffect.SetActive(false);
		}
		else
		{
			this.m_goGageEffect.SetActive(true);
		}
	}

	private void _funcUIEffectDownloaded(IDownloadedItem wItem, object obj)
	{
		if (null == wItem.mainAsset)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
			return;
		}
		GameObject gameObject = wItem.mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		this.SlotEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		if (null == this.SlotEffect)
		{
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 300f;
		this.SlotEffect.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this.SlotEffect, GUICamera.UILayer);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.SlotEffect);
		}
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyObject(this.SlotEffect, 5f);
		}
	}
}
