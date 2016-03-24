using GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolRecruitSuccess_RenewalDlg : Form
{
	private class SolSettingInfo
	{
		public GameObject faceObj;

		public GameObject gradeObj;

		public GameObject glowObj;

		public string faceImageKey = string.Empty;

		public string gradeImageKey = string.Empty;

		public string name = string.Empty;

		public bool isfaceTextureLoaded;

		public bool isgradeTextureLoaded;

		public SolRecruitSuccess_RenewalDlg.eCardType cardType;

		public bool TextureLoaded(string imageKey)
		{
			if (imageKey == this.faceImageKey)
			{
				this.isfaceTextureLoaded = true;
				return true;
			}
			if (imageKey == this.gradeImageKey)
			{
				this.isgradeTextureLoaded = true;
				return true;
			}
			return false;
		}

		public bool isTextureLoaded()
		{
			return this.isfaceTextureLoaded && this.isgradeTextureLoaded;
		}

		public bool SettingImage()
		{
			if (!this.isTextureLoaded())
			{
				return false;
			}
			if (this.faceObj == null)
			{
				return false;
			}
			if (this.gradeObj == null)
			{
				return false;
			}
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
			if (null != texture)
			{
				this.faceObj.transform.localRotation = Quaternion.identity;
				this.faceObj.renderer.sharedMaterial.mainTexture = texture;
			}
			texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.gradeImageKey);
			if (null != texture)
			{
				this.gradeObj.transform.localRotation = Quaternion.identity;
				this.gradeObj.renderer.sharedMaterial.mainTexture = texture;
			}
			return true;
		}

		public bool SettingGlow(string objName)
		{
			if (this.glowObj == null)
			{
				return false;
			}
			this.glowObj.transform.FindChild(objName).gameObject.SetActive(true);
			return true;
		}
	}

	public enum eCardType
	{
		NONE,
		HIGHSESSION,
		LEGEND,
		MYTH
	}

	private enum eCardOpenAniamtion
	{
		CARD_OPEN,
		CARD_END,
		CARD_CLOSE
	}

	private enum eCardOpen
	{
		ONE,
		Eleven
	}

	private const string EFFECT_NAME_ELEVEN = "fx_direct_hire_renewal";

	private const string EFFECT_NAME_ONE = "fx_direct_hire_renewal_one";

	private const string EFFECT_SPECIAL = "card_pick";

	private const string EFFECT_HIGHSESSION = "fx_hearts_card_ui";

	private const string EFFECT_LEGEND = "fx_legend_card_ui";

	private const string EFFECT_MYTH = "fx_myth_card_ui";

	private const string CARD_NAME = "card_dm";

	private const string CARD_PREFIX = "card_";

	private const string CARD_FACE = "fx_plan_face";

	private const string CARD_GRADE = "fx_card_rank";

	private const string CARD_GLOW = "card_glow_dm_";

	private const float SPECIALOBJECTTIME = 1.5f;

	private const int GLOWTIME_ONE = 84;

	private const int GLOWTIME_ELEVEN = 90;

	private const int NAMETIME_ONE = 130;

	private const int NAMETIME_ELEVEN = 12;

	private const int CARD_OPENSTART_ELEVEN = 130;

	private const int CARD_OPENSTART_ONE = 110;

	private Label lb_name;

	private Button bt_TouchArea;

	private SOLDIER_INFO[] solArray;

	private int[] EVENT_STARTTIME = new int[]
	{
		140,
		148,
		155,
		163,
		171,
		179,
		187,
		195,
		203,
		211,
		219
	};

	private int[] EVENT_ENDTIME = new int[]
	{
		145,
		153,
		160,
		168,
		176,
		184,
		192,
		200,
		208,
		216,
		224
	};

	private List<SolRecruitSuccess_RenewalDlg.SolSettingInfo> solSettingList = new List<SolRecruitSuccess_RenewalDlg.SolSettingInfo>();

	private bool isAllTextureLoaded;

	private GameObject specialCardObject;

	private GameObject specialCardFace;

	private GameObject specialCardGrade;

	private GameObject specialCardName;

	private GameObject specialCardGlow;

	private GameObject oneCardName;

	private GameObject rootGameObject;

	private Animation baseAnimation;

	private Animation specialCardAnimation;

	private float specialCardTime;

	private bool isUpdateGlow;

	private bool isFinish;

	private bool isUpdateName;

	private int nowOpenCardIndex;

	private int nowSpecialIndex = -1;

	private bool isCardOpenTime;

	private int recruitType = -1;

	private bool isSecondClick;

	private SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion cardOpenAnimation;

	private SolRecruitSuccess_RenewalDlg.eCardOpen cardOpen;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruitSuccess_Renewal", G_ID.SOLRECRUITSUCCESS_RENEWAL_DLG, false);
		base.bCloseAni = false;
	}

	public override void SetComponent()
	{
		this.lb_name = (base.GetControl("LB_Name01") as Label);
		this.lb_name.Visible = false;
		this.bt_TouchArea = (base.GetControl("BT_TouchArea") as Button);
		this.bt_TouchArea.Click = new EZValueChangedDelegate(this.OnClickClose);
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ());
		base.ShowBlackBG(1f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
			base.BLACK_BG.gameObject.transform.localPosition = new Vector3(base.BLACK_BG.gameObject.transform.localPosition.x, base.BLACK_BG.gameObject.transform.localPosition.y, 300f);
		}
		this.Hide();
	}

	public override void Show()
	{
		UIDataManager.MuteSound(false);
		TsAudio.StoreMuteAllAudio();
		TsAudio.SetExceptMuteAllAudio(EAudioType.SFX, true);
		TsAudio.RefreshAllMuteAudio();
		if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SOULCARD", "ONECARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, false);
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SOULCARD", "MANYCARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, false);
		}
		base.Show();
	}

	public override void CloseForm(IUIObject obj)
	{
		this.lb_name.Visible = false;
		this.DestroyLoadObject();
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			itemMallDlg.SetShowData();
		}
		if (null != BugFixAudio.PlayOnceRoot)
		{
			int childCount = BugFixAudio.PlayOnceRoot.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = BugFixAudio.PlayOnceRoot.transform.GetChild(i);
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
		TsAudio.RestoreMuteAllAudio();
		TsAudio.RefreshAllMuteAudio();
		base.CloseForm(obj);
	}

	public override void Update()
	{
		base.Update();
		this.CheckFinish();
		this.UpdateSound();
		this.UpdateSetting();
		this.UpdateName();
		this.UpdateGlow();
		this.UpdateAnimation();
	}

	private void DestroyLoadObject()
	{
		if (this.rootGameObject != null)
		{
			UnityEngine.Object.Destroy(this.rootGameObject);
		}
		if (this.specialCardObject != null)
		{
			UnityEngine.Object.Destroy(this.specialCardObject);
		}
	}

	private void UpdateSound()
	{
		if (this.nowOpenCardIndex == this.solSettingList.Count)
		{
			return;
		}
		if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			return;
		}
		if (this.baseAnimation == null)
		{
			return;
		}
		if ((float)this.EVENT_STARTTIME[this.nowOpenCardIndex] / this.baseAnimation.clip.frameRate <= this.baseAnimation[this.baseAnimation.clip.name].time && (float)this.EVENT_ENDTIME[this.nowOpenCardIndex] / this.baseAnimation.clip.frameRate >= this.baseAnimation[this.baseAnimation.clip.name].time)
		{
			if (this.nowOpenCardIndex == this.nowSpecialIndex)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SOULCARD", "CARDOPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, false);
			}
			else
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SOULCARD", "CARDSELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, false);
			}
			this.nowOpenCardIndex++;
		}
	}

	private bool CheckFinish()
	{
		if (this.isFinish)
		{
			return false;
		}
		if (this.baseAnimation == null)
		{
			return false;
		}
		if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			if (this.baseAnimation[this.baseAnimation.clip.name].time >= 130f / this.baseAnimation.clip.frameRate || (this.baseAnimation[this.baseAnimation.clip.name].time == 0f && this.isSecondClick))
			{
				if (this.recruitType == 1)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("685"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				this.isFinish = true;
			}
		}
		else
		{
			int num = this.solSettingList.Count - 1;
			if (this.baseAnimation[this.baseAnimation.clip.name].time >= (float)this.EVENT_ENDTIME[num] / this.baseAnimation.clip.frameRate || (this.baseAnimation[this.baseAnimation.clip.name].time == 0f && this.isSecondClick))
			{
				this.baseAnimation[this.baseAnimation.clip.name].speed = 0f;
				if (this.solSettingList.Count < 11)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("685"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				this.isFinish = true;
				return true;
			}
		}
		return false;
	}

	private void UpdateName()
	{
		if (this.isUpdateName)
		{
			return;
		}
		SolRecruitSuccess_RenewalDlg.eCardOpen eCardOpen = this.cardOpen;
		if (eCardOpen != SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			if (eCardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.Eleven)
			{
				if (this.specialCardAnimation.gameObject.activeInHierarchy && !this.specialCardAnimation.isPlaying)
				{
					if (this.specialCardName == null)
					{
						return;
					}
					this.lb_name.SetText(this.solSettingList[this.nowSpecialIndex].name);
					Vector3 position = this.specialCardName.transform.position;
					position.Set(position.x - this.lb_name.width / 2f, position.y + this.lb_name.height / 2f, position.z);
					this.lb_name.gameObject.transform.position = position;
					this.lb_name.Visible = true;
					this.isUpdateName = true;
				}
			}
		}
		else if (this.baseAnimation[this.baseAnimation.clip.name].time >= 130f / this.baseAnimation.clip.frameRate)
		{
			if (this.oneCardName == null)
			{
				return;
			}
			this.lb_name.SetText(this.solSettingList[0].name);
			Vector3 position2 = this.oneCardName.transform.position;
			position2.Set(position2.x - this.lb_name.width / 2f, position2.y + this.lb_name.height / 2f, position2.z);
			this.lb_name.gameObject.transform.position = position2;
			this.lb_name.Visible = true;
			this.isUpdateName = true;
		}
	}

	private void UpdateGlow()
	{
		SolRecruitSuccess_RenewalDlg.eCardOpen eCardOpen = this.cardOpen;
		if (eCardOpen != SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			if (eCardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.Eleven)
			{
				if (!this.isUpdateGlow && this.baseAnimation[this.baseAnimation.clip.name].time >= 90f / this.baseAnimation.clip.frameRate)
				{
					for (int i = 0; i < this.solSettingList.Count; i++)
					{
						switch (this.solSettingList[i].cardType)
						{
						case SolRecruitSuccess_RenewalDlg.eCardType.HIGHSESSION:
							if (!this.solSettingList[i].SettingGlow("fx_hearts_card_ui"))
							{
								return;
							}
							break;
						case SolRecruitSuccess_RenewalDlg.eCardType.LEGEND:
							if (!this.solSettingList[i].SettingGlow("fx_legend_card_ui"))
							{
								return;
							}
							break;
						case SolRecruitSuccess_RenewalDlg.eCardType.MYTH:
							if (!this.solSettingList[i].SettingGlow("fx_myth_card_ui"))
							{
								return;
							}
							break;
						}
					}
					this.isUpdateGlow = true;
				}
			}
		}
		else if (!this.isUpdateGlow && this.baseAnimation[this.baseAnimation.clip.name].time >= 84f / this.baseAnimation.clip.frameRate)
		{
			switch (this.solSettingList[0].cardType)
			{
			case SolRecruitSuccess_RenewalDlg.eCardType.HIGHSESSION:
				if (!this.solSettingList[0].SettingGlow("fx_hearts_card_ui"))
				{
					return;
				}
				break;
			case SolRecruitSuccess_RenewalDlg.eCardType.LEGEND:
				if (!this.solSettingList[0].SettingGlow("fx_legend_card_ui"))
				{
					return;
				}
				break;
			case SolRecruitSuccess_RenewalDlg.eCardType.MYTH:
				if (!this.solSettingList[0].SettingGlow("fx_myth_card_ui"))
				{
					return;
				}
				break;
			}
			this.isUpdateGlow = true;
		}
	}

	private void UpdateAnimation()
	{
		if (this.baseAnimation == null)
		{
			return;
		}
		if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE && !this.isCardOpenTime && this.baseAnimation[this.baseAnimation.clip.name].time >= 110f / this.baseAnimation.clip.frameRate)
		{
			if (this.baseAnimation == null)
			{
				return;
			}
			this.baseAnimation[this.baseAnimation.clip.name].speed = 0f;
			this.isCardOpenTime = true;
		}
		else if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.Eleven && !this.isCardOpenTime && this.baseAnimation[this.baseAnimation.clip.name].time >= 130f / this.baseAnimation.clip.frameRate)
		{
			if (this.baseAnimation == null)
			{
				return;
			}
			this.baseAnimation[this.baseAnimation.clip.name].speed = 0f;
			this.isCardOpenTime = true;
		}
		if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
		{
			return;
		}
		switch (this.cardOpenAnimation)
		{
		case SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_OPEN:
			if (this.nowSpecialIndex != -1 && this.baseAnimation[this.baseAnimation.clip.name].time >= (float)this.EVENT_STARTTIME[this.nowSpecialIndex] / this.baseAnimation.clip.frameRate)
			{
				if (this.specialCardObject == null)
				{
					return;
				}
				this.specialCardObject.SetActive(true);
				Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.solSettingList[this.nowSpecialIndex].faceImageKey);
				if (null != texture)
				{
					this.specialCardFace.renderer.sharedMaterial.mainTexture = texture;
				}
				texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.solSettingList[this.nowSpecialIndex].gradeImageKey);
				if (null != texture)
				{
					this.specialCardGrade.renderer.sharedMaterial.mainTexture = texture;
				}
				this.specialCardTime = Time.realtimeSinceStartup;
				this.cardOpenAnimation = SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_END;
				switch (this.solSettingList[this.nowSpecialIndex].cardType)
				{
				case SolRecruitSuccess_RenewalDlg.eCardType.HIGHSESSION:
					this.specialCardGlow.transform.FindChild("fx_hearts_card_ui").gameObject.SetActive(true);
					break;
				case SolRecruitSuccess_RenewalDlg.eCardType.LEGEND:
					this.specialCardGlow.transform.FindChild("fx_legend_card_ui").gameObject.SetActive(true);
					break;
				case SolRecruitSuccess_RenewalDlg.eCardType.MYTH:
					this.specialCardGlow.transform.FindChild("fx_myth_card_ui").gameObject.SetActive(true);
					break;
				}
			}
			break;
		case SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_END:
			if (this.nowSpecialIndex != -1 && this.baseAnimation[this.baseAnimation.clip.name].time >= (float)this.EVENT_ENDTIME[this.nowSpecialIndex] / this.baseAnimation.clip.frameRate)
			{
				this.baseAnimation[this.baseAnimation.clip.name].speed = 0f;
				this.cardOpenAnimation = SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_CLOSE;
			}
			break;
		case SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_CLOSE:
			if (this.specialCardObject == null)
			{
				return;
			}
			if (this.specialCardTime + 1.5f <= Time.realtimeSinceStartup)
			{
				this.specialCardObject.SetActive(false);
				this.lb_name.Visible = false;
				this.isUpdateName = false;
				if (!this.isFinish)
				{
					this.baseAnimation[this.baseAnimation.clip.name].speed = 1f;
				}
				this.nowSpecialIndex = this.GetNextSpecialIndex(this.nowSpecialIndex);
				this.cardOpenAnimation = SolRecruitSuccess_RenewalDlg.eCardOpenAniamtion.CARD_OPEN;
			}
			break;
		}
	}

	private int GetNextSpecialIndex(int _nowIndex)
	{
		if (_nowIndex + 1 == this.solSettingList.Count)
		{
			return -1;
		}
		for (int i = _nowIndex + 1; i < this.solSettingList.Count; i++)
		{
			if (this.solSettingList[i].cardType != SolRecruitSuccess_RenewalDlg.eCardType.NONE)
			{
				return i;
			}
		}
		return -1;
	}

	private void UpdateSetting()
	{
		if (this.solSettingList.Count <= 0)
		{
			return;
		}
		if (this.isAllTextureLoaded)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.solSettingList.Count; i++)
		{
			if (this.solSettingList[i].SettingImage())
			{
				num++;
			}
		}
		if (num == this.solSettingList.Count)
		{
			this.isAllTextureLoaded = true;
		}
	}

	public void SetList(SOLDIER_INFO _solInfo, int _recruitType)
	{
		this.recruitType = _recruitType;
		this.Hide();
		this.solArray = new SOLDIER_INFO[1];
		this.solArray[0] = _solInfo;
		this.EffectLoad("fx_direct_hire_renewal_one", new PostProcPerItem(this.SolRecruitSuccess), null);
		this.cardOpen = SolRecruitSuccess_RenewalDlg.eCardOpen.ONE;
	}

	public void SetList(SOLDIER_INFO[] _solArray)
	{
		this.Hide();
		this.cardOpen = SolRecruitSuccess_RenewalDlg.eCardOpen.Eleven;
		this.solArray = _solArray;
		this.EffectLoad("fx_direct_hire_renewal", new PostProcPerItem(this.SolRecruitSuccess), null);
		this.EffectLoad("card_pick", new PostProcPerItem(this.SpecialCardLoad), null);
	}

	private void EffectLoad(string effect, PostProcPerItem callback, object callbackParam)
	{
		string str = string.Format("{0}", "UI/Soldier/" + effect + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(callback, callbackParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void SolRecruitSuccess(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (this == null)
		{
			return;
		}
		if (null == _item.GetSafeBundle())
		{
			return;
		}
		if (null == _item.GetSafeBundle().mainAsset)
		{
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
		this.rootGameObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
		if (this == null)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 300f;
		this.rootGameObject.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
		}
		Animation[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Animation animation = array[i];
			if (animation != null && animation.cullingType != AnimationCullingType.AlwaysAnimate)
			{
				animation.cullingType = AnimationCullingType.AlwaysAnimate;
			}
		}
		this.baseAnimation = componentsInChildren[0];
		this.Show();
		this.SettingImages(this.rootGameObject.transform.FindChild("fx_direct_hire_renewal"));
	}

	private void SpecialCardLoad(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (this == null)
		{
			return;
		}
		if (null == _item.GetSafeBundle())
		{
			return;
		}
		if (null == _item.GetSafeBundle().mainAsset)
		{
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		gameObject2.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
		if (this == null)
		{
			UnityEngine.Object.DestroyImmediate(gameObject2);
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 290f;
		gameObject2.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(gameObject2, GUICamera.UILayer);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject2);
		}
		Animation[] componentsInChildren = gameObject2.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Animation animation = array[i];
			if (animation != null && animation.cullingType != AnimationCullingType.AlwaysAnimate)
			{
				animation.cullingType = AnimationCullingType.AlwaysAnimate;
			}
		}
		this.specialCardAnimation = componentsInChildren[0];
		this.specialCardObject = gameObject2;
		this.specialCardObject.SetActive(false);
		GameObject gameObject3 = this.specialCardObject.transform.GetChild(0).FindChild("card").gameObject;
		if (gameObject3 == null)
		{
			Debug.LogError("_cardObj is Null");
			return;
		}
		this.specialCardFace = gameObject3.transform.FindChild("fx_plan_face").gameObject;
		this.specialCardGrade = gameObject3.transform.FindChild("fx_card_rank01").gameObject;
		this.specialCardName = gameObject3.transform.FindChild("fx_card_rank02").gameObject;
		this.specialCardGlow = gameObject3.transform.FindChild("card_glow_dm_01").gameObject;
	}

	private void SettingImages(Transform rootTransform)
	{
		if (rootTransform == null)
		{
			Debug.LogError("There are no _rootObject");
			return;
		}
		Transform transform = rootTransform.FindChild("card_dm");
		if (transform == null)
		{
			Debug.LogError("There are no _cardObj");
			return;
		}
		for (int i = 0; i < this.solArray.Length; i++)
		{
			SolRecruitSuccess_RenewalDlg.SolSettingInfo solSettingInfo = new SolRecruitSuccess_RenewalDlg.SolSettingInfo();
			Transform transform2;
			if (i + 1 < 10)
			{
				transform2 = transform.FindChild("card_0" + (i + 1).ToString());
				solSettingInfo.glowObj = transform2.FindChild("card_glow_dm_0" + (i + 1).ToString()).gameObject;
			}
			else
			{
				transform2 = transform.FindChild("card_" + (i + 1).ToString());
				solSettingInfo.glowObj = transform2.FindChild("card_glow_dm_" + (i + 1).ToString()).gameObject;
			}
			if (transform2 == null)
			{
				Debug.LogError("There are no _cardTrans");
				return;
			}
			solSettingInfo.faceObj = transform2.FindChild("fx_plan_face").gameObject;
			if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.Eleven)
			{
				solSettingInfo.gradeObj = transform2.FindChild("fx_card_rank").gameObject;
			}
			else
			{
				solSettingInfo.gradeObj = transform2.FindChild("fx_card_rank01").gameObject;
				this.oneCardName = transform2.FindChild("fx_card_rank02").gameObject;
			}
			solSettingInfo.isfaceTextureLoaded = false;
			solSettingInfo.isgradeTextureLoaded = false;
			solSettingInfo.faceImageKey = this.GetFaceImageKey(this.solArray[i]);
			solSettingInfo.gradeImageKey = this.GetGradImageKey(this.solArray[i]);
			solSettingInfo.isfaceTextureLoaded = this.RequestImage(solSettingInfo.faceImageKey, true);
			solSettingInfo.isgradeTextureLoaded = this.RequestImage(solSettingInfo.gradeImageKey, false);
			solSettingInfo.cardType = this.GetCardType(this.solArray[i]);
			if (solSettingInfo.cardType != SolRecruitSuccess_RenewalDlg.eCardType.NONE && this.nowSpecialIndex == -1)
			{
				this.nowSpecialIndex = i;
			}
			solSettingInfo.name = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.solArray[i].CharKind).GetName();
			this.solSettingList.Add(solSettingInfo);
		}
	}

	private SolRecruitSuccess_RenewalDlg.eCardType GetCardType(SOLDIER_INFO _solInfo)
	{
		if (_solInfo.Grade >= 4)
		{
			return SolRecruitSuccess_RenewalDlg.eCardType.HIGHSESSION;
		}
		return SolRecruitSuccess_RenewalDlg.eCardType.NONE;
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() == null)
		{
			return;
		}
		if (_item.GetSafeBundle().mainAsset == null)
		{
			return;
		}
		Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
		if (texture2D == null)
		{
			return;
		}
		string imageKey = string.Empty;
		if (_param is string)
		{
			imageKey = (string)_param;
			NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
			for (int i = 0; i < this.solSettingList.Count; i++)
			{
				this.solSettingList[i].TextureLoaded(imageKey);
			}
		}
	}

	private string GetGradImageKey(SOLDIER_INFO _solInfo)
	{
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(_solInfo.CharKind, (int)_solInfo.Grade);
		string result;
		if (legendType == 1)
		{
			result = "rankl" + ((int)(_solInfo.Grade + 1)).ToString();
		}
		else if (legendType == 2)
		{
			result = "rankm" + ((int)(_solInfo.Grade + 1)).ToString();
		}
		else
		{
			result = "rank" + ((int)(_solInfo.Grade + 1)).ToString();
		}
		return result;
	}

	private string GetFaceImageKey(SOLDIER_INFO _solInfo)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_solInfo.CharKind);
		string result;
		if (UIDataManager.IsUse256Texture())
		{
			result = charKindInfo.GetPortraitFile1((int)_solInfo.Grade, string.Empty) + "_256";
		}
		else
		{
			result = charKindInfo.GetPortraitFile1((int)_solInfo.Grade, string.Empty) + "_512";
		}
		return result;
	}

	private bool RequestImage(string imageKey, bool isFaceImage)
	{
		if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey) != null)
		{
			return true;
		}
		if (isFaceImage)
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(imageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		else
		{
			string str = string.Format("{0}", "UI/Soldier/" + imageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), imageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		return false;
	}

	private void OnClickClose(IUIObject _obj)
	{
		if (!this.isFinish && this.isCardOpenTime && !this.isSecondClick)
		{
			this.baseAnimation[this.baseAnimation.clip.name].speed = 1f;
			if (this.cardOpen == SolRecruitSuccess_RenewalDlg.eCardOpen.ONE)
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SOULCARD", "CARDOPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, false);
			}
			this.isSecondClick = true;
		}
		else if (this.isFinish)
		{
			this.CloseForm(null);
		}
	}

	public void SetList_Debug(bool isOne)
	{
		int num = 11;
		SOLDIER_INFO[] array = new SOLDIER_INFO[num];
		int[] array2 = new int[]
		{
			1029,
			1121,
			1149,
			1069,
			1123,
			1017,
			1067,
			1094,
			1110,
			1026,
			1017
		};
		byte[] array3 = new byte[]
		{
			4,
			4,
			2,
			2,
			2,
			2,
			2,
			2,
			2,
			3,
			4
		};
		for (int i = 0; i < num; i++)
		{
			array[i] = new SOLDIER_INFO
			{
				SolID = (long)(637686 + i),
				BattlePos = -1,
				CharKind = array2[i],
				Grade = array3[i],
				Level = 1,
				HP = 1000000,
				nInitiativeValue = 50
			};
		}
		if (isOne)
		{
			this.SetList(array[0], 0);
		}
		else
		{
			this.SetList(array);
		}
	}
}
