using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class DailyDungeon_Main_Dlg : Form
{
	private Label m_lbTitle;

	private Label m_lbRewardName;

	private Label m_lbHeroInfo;

	private Label m_lbMonsterInfo;

	private ItemTexture m_itRewardItem;

	private Button m_btChangeDifficulty;

	private Button m_btReward;

	private Button m_btStart;

	private Button m_btExit;

	private Button m_btHeroInfo;

	private DrawTexture m_dtBG;

	private DrawTexture m_dtDifficulty;

	private DrawTexture m_dtRightBottomFrame;

	private DrawTexture m_dtMonsterImage;

	private sbyte m_nDifficult = -1;

	private sbyte m_nDayOfWeek = -1;

	private string m_szBackImage = string.Empty;

	private GameObject m_goPlayAni;

	private GameObject m_goAnimation;

	private GameObject m_goBackTexture;

	private bool m_bAniPlay;

	private bool m_bRestoreReserve;

	private GameObject m_goRewardEffect;

	private GameObject m_goGageEffect;

	private GameObject SlotEffect;

	public sbyte Difficult
	{
		get
		{
			return this.m_nDifficult;
		}
	}

	public sbyte DayOfWeek
	{
		get
		{
			return this.m_nDayOfWeek;
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
		this.m_lbRewardName = (base.GetControl("LB_RewardName") as Label);
		this.m_lbMonsterInfo = (base.GetControl("LB_MonsterInfo") as Label);
		this.m_dtBG = (base.GetControl("Main_BG") as DrawTexture);
		this.m_itRewardItem = (base.GetControl("ItemTexture_reward") as ItemTexture);
		this.m_btChangeDifficulty = (base.GetControl("Button_Difficulty") as Button);
		Button expr_A0 = this.m_btChangeDifficulty;
		expr_A0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A0.Click, new EZValueChangedDelegate(this.OnClickChangeDifficulty));
		this.m_btReward = (base.GetControl("Btn_GetReward") as Button);
		Button expr_DD = this.m_btReward;
		expr_DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DD.Click, new EZValueChangedDelegate(this.OnRewardReq));
		this.m_btReward.AlphaAni(1f, 0.5f, -0.5f);
		this.m_btStart = (base.GetControl("Start_Btn") as Button);
		Button expr_134 = this.m_btStart;
		expr_134.Click = (EZValueChangedDelegate)Delegate.Combine(expr_134.Click, new EZValueChangedDelegate(this.OnClickStart));
		this.m_btExit = (base.GetControl("Exit_Btn") as Button);
		Button expr_171 = this.m_btExit;
		expr_171.Click = (EZValueChangedDelegate)Delegate.Combine(expr_171.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_btHeroInfo = (base.GetControl("Button_HeroInfo") as Button);
		this.m_btHeroInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierInfo));
		this.m_dtRightBottomFrame = (base.GetControl("DrawTexture_RightBottom") as DrawTexture);
		this.m_lbHeroInfo = (base.GetControl("Label_HeroInfo") as Label);
		this.m_dtMonsterImage = (base.GetControl("DT_MonsterFace") as DrawTexture);
		this.m_nDayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		DAILYDUNGEON_INFO dailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)this.m_nDayOfWeek);
		if (dailyDungeonInfo == null)
		{
			this.m_nDifficult = 1;
		}
		else
		{
			this.m_nDifficult = dailyDungeonInfo.m_i8Diff;
		}
		this._SetDialogPos();
		this.SetBG();
		base.SetScreenCenter();
		sbyte dayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		if ((int)dayOfWeek <= 0)
		{
			this.OnClose();
		}
		else
		{
			this.SetBasicData(dayOfWeek, false);
		}
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
		if (this.m_bAniPlay && !this.m_goAnimation.animation.isPlaying)
		{
			SoldierBatch.DailyDungeonDifficulty = this.m_nDifficult;
			SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON;
			FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
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
		this.SetBasicData(this.m_nDayOfWeek, false);
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

	public void OnClickSoldierInfo(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickChangeDifficulty(IUIObject obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		sbyte dayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		DAILYDUNGEON_INFO dailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)dayOfWeek);
		int num;
		if (dailyDungeonInfo == null)
		{
			num = 0;
		}
		else
		{
			num = dailyDungeonInfo.m_i32IsClear;
		}
		if (num >= 1)
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
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		this.m_nDifficult = nDifficult;
		this.SetBasicData(this.m_nDayOfWeek, true);
	}

	public void SetBG()
	{
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(1, this.m_nDayOfWeek);
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

	public void SetBasicData(sbyte nDayOfWeek, bool bCheck)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			this.Close();
			return;
		}
		DAILYDUNGEON_INFO dailyDungeonInfo = NrTSingleton<DailyDungeonManager>.Instance.GetDailyDungeonInfo((int)nDayOfWeek);
		EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo2;
		if (dailyDungeonInfo != null)
		{
			this.m_nDayOfWeek = (sbyte)dailyDungeonInfo.m_i32DayOfWeek;
			if (!bCheck)
			{
				this.m_nDifficult = dailyDungeonInfo.m_i8Diff;
			}
			dailyDungeonInfo2 = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(this.m_nDifficult, this.m_nDayOfWeek);
			if (dailyDungeonInfo2 == null)
			{
				this.Close();
				return;
			}
			if (dailyDungeonInfo.m_i32IsClear <= 1)
			{
				if (dailyDungeonInfo.m_i32IsClear != 1)
				{
					this.m_btStart.Visible = true;
					this.m_btReward.Visible = false;
					if (this.m_goGageEffect != null)
					{
						this.m_goGageEffect.SetActive(false);
					}
				}
				else
				{
					this.m_btStart.Visible = false;
					this.m_btReward.Visible = true;
					if (this.m_goGageEffect != null)
					{
						this.m_goGageEffect.SetActive(true);
					}
				}
				if ((int)dailyDungeonInfo.m_i8IsReward == 1)
				{
					this.m_btStart.Visible = false;
					this.m_btReward.Visible = false;
					this.m_btHeroInfo.Visible = false;
					this.m_lbHeroInfo.Visible = false;
					this.m_dtRightBottomFrame.Visible = false;
					if (this.m_goGageEffect != null)
					{
						this.m_goGageEffect.SetActive(true);
					}
					MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
					if (myCharInfoDlg != null)
					{
						myCharInfoDlg.UpdateNoticeInfo();
					}
				}
			}
		}
		else
		{
			this.m_nDayOfWeek = nDayOfWeek;
			if ((int)this.m_nDifficult <= 0 || !bCheck)
			{
				this.m_nDifficult = 1;
			}
			dailyDungeonInfo2 = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(this.m_nDifficult, this.m_nDayOfWeek);
			if (dailyDungeonInfo2 == null)
			{
				this.Close();
				return;
			}
			this.m_btStart.Visible = true;
			this.m_btReward.Visible = false;
			if (this.m_goGageEffect != null)
			{
				this.m_goGageEffect.SetActive(false);
			}
		}
		if (dailyDungeonInfo2 == null)
		{
			this.Close();
			return;
		}
		string textFromMap = NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(dailyDungeonInfo2.i32TextKey.ToString());
		this.m_lbTitle.SetText(textFromMap);
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = dailyDungeonInfo2.i32RewardItemUnique;
		iTEM.m_nItemNum = dailyDungeonInfo2.i32RewardItemNum;
		this.m_itRewardItem.SetItemTexture(iTEM);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(dailyDungeonInfo2.i32RewardItemUnique),
			"count",
			dailyDungeonInfo2.i32RewardItemNum.ToString()
		});
		this.m_lbRewardName.SetText(empty);
		this.m_dtDifficulty.SetTexture("Win_I_WorrGradeS" + this.m_nDifficult.ToString());
		this.m_lbMonsterInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(dailyDungeonInfo2.i32ExplainText.ToString()));
		this.m_dtMonsterImage.SetTextureFromBundle("ui/soldier/512/" + dailyDungeonInfo2.szMonIMG);
	}

	public void OnRewardReq(IUIObject obj)
	{
		if ((int)this.m_nDayOfWeek < 0)
		{
			return;
		}
		this.m_btReward.enabled = true;
		GS_CHARACTER_DAILYDUNGEON_REWARD_REQ gS_CHARACTER_DAILYDUNGEON_REWARD_REQ = new GS_CHARACTER_DAILYDUNGEON_REWARD_REQ();
		gS_CHARACTER_DAILYDUNGEON_REWARD_REQ.i32DayOfWeek = (int)this.m_nDayOfWeek;
		SendPacket.GetInstance().SendObject(2546, gS_CHARACTER_DAILYDUNGEON_REWARD_REQ);
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
		if (nrCharUser.GetPersonInfoUser() == null)
		{
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
				if (this.m_goBackTexture != null && !string.IsNullOrEmpty(this.m_szBackImage))
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

	public void IsRewardCheck(bool bCheck)
	{
		this.m_btReward.enabled = bCheck;
	}
}
