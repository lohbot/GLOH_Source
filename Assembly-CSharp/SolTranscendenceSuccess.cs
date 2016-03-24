using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolTranscendenceSuccess : Form
{
	private enum eBUNDLEDOWNSTATE
	{
		eBUNDLE_NONE,
		eBUNDLE_DOWNING,
		eBUNDLE_DOWNCOMPLTE,
		eBUNDLE_OK
	}

	private GameObject rootEffectGameObject;

	private GameObject SubEffectGameObject;

	private SolTranscendenceSuccess.eBUNDLEDOWNSTATE eBUNDLEDOWN;

	private string m_strBaserankImageKey = string.Empty;

	private string m_strBasefaceImageKey = string.Empty;

	private string m_strUpgraderankImageKey = string.Empty;

	private string m_strUpgradefaceImageKey = string.Empty;

	private string m_strUpgraderankTextImageKey = string.Empty;

	private string m_strSubrankImageKey = string.Empty;

	private string m_strSubfaceImageKey = string.Empty;

	private DrawTexture bgImage;

	private DrawTexture nameBack;

	private DrawTexture closeBack;

	private FlashLabel solName;

	private FlashLabel closeText;

	private Button closeUIButton;

	private Button skipButton;

	private Button solMovie;

	private Button solMovietTrans;

	private Label solMovieText;

	private bool m_bComposeTranscendence;

	private int m_i32FailItemNum;

	private bool m_bEffectUpdate;

	private bool m_bLegendBaseType;

	private bool m_bLegendSubType;

	private bool m_bSetrank;

	private bool m_bSetface;

	private bool m_bSetrankText;

	private OnCloseCallback _closeCallback;

	public void InitComposeData()
	{
		this.m_bComposeTranscendence = false;
		this.m_i32FailItemNum = 0;
		this.m_bEffectUpdate = false;
		this.m_bLegendBaseType = false;
		this.m_bLegendSubType = false;
		this.m_bSetrank = false;
		this.m_bSetface = false;
		this.m_bSetrankText = false;
		this.m_strBaserankImageKey = string.Empty;
		this.m_strBasefaceImageKey = string.Empty;
		this.m_strUpgraderankImageKey = string.Empty;
		this.m_strUpgradefaceImageKey = string.Empty;
		this.m_strUpgraderankTextImageKey = string.Empty;
		this.m_strSubrankImageKey = string.Empty;
		this.m_strSubfaceImageKey = string.Empty;
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruit", G_ID.SOLCOMPOSE_TRANSCENDENCE_DLG, false);
	}

	public override void SetComponent()
	{
		this.bgImage = (base.GetControl("DrawTexture_BG") as DrawTexture);
		this.bgImage.Visible = false;
		this.nameBack = (base.GetControl("DrawTexture_SolNameBK") as DrawTexture);
		this.nameBack.Visible = false;
		this.closeBack = (base.GetControl("DrawTexture_CloseBK01") as DrawTexture);
		this.closeBack.Visible = false;
		this.solName = (base.GetControl("FlashLabel_SolName01") as FlashLabel);
		this.solName.Visible = false;
		this.closeText = (base.GetControl("FlashLabel_CloseText01") as FlashLabel);
		this.closeText.Visible = false;
		this.closeUIButton = (base.GetControl("Button_CloseButton01") as Button);
		this.closeUIButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCloseButton));
		this.skipButton = (base.GetControl("Button_Skip") as Button);
		this.skipButton.Visible = false;
		this.solMovietTrans = (base.GetControl("Button_MovieBtn") as Button);
		this.solMovietTrans.Visible = false;
		this.solMovie = (base.GetControl("BT_Movie") as Button);
		this.solMovie.Visible = false;
		this.solMovieText = (base.GetControl("LB_Movie") as Label);
		this.solMovieText.Visible = false;
		this.solMovieText.SetSize(800f, 64f);
		this.closeUIButton.SetSize(GUICamera.width, GUICamera.height);
		this.closeUIButton.SetLocation(0, 0);
		base.DonotDepthChange(360f);
	}

	public void GetComposeTranscendence(bool bCompose, int i32BaseKind, byte i8BaseRank, byte i8UpgradeRank, int i32SubKind, byte i8SubRank, int i32ItemNum, int i32CostumeUnique)
	{
		this.InitComposeData();
		this.m_bComposeTranscendence = bCompose;
		this.m_i32FailItemNum = i32ItemNum;
		if (bCompose)
		{
			this.m_bSetrankText = false;
		}
		else
		{
			this.m_bSetrankText = true;
		}
		this.m_bSetrank = false;
		this.m_bSetface = false;
		this.m_bEffectUpdate = false;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32BaseKind);
		if (charKindInfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32SubKind);
		if (charKindInfo2 == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32BaseKind);
		if (charKindInfo3 == null)
		{
			return;
		}
		this.m_strBaserankImageKey = this.GetLegendType(charKindInfo, (int)i8BaseRank) + ((int)(i8BaseRank + 1)).ToString();
		this.m_strUpgraderankImageKey = this.GetLegendType(charKindInfo3, (int)i8UpgradeRank) + ((int)(i8UpgradeRank + 1)).ToString();
		this.m_strSubrankImageKey = this.GetLegendType(charKindInfo2, (int)i8SubRank) + ((int)(i8SubRank + 1)).ToString();
		if (charKindInfo.GetLegendType((int)i8BaseRank) == 2)
		{
			this.m_bLegendBaseType = true;
		}
		else
		{
			this.m_bLegendBaseType = false;
		}
		if (charKindInfo2.GetLegendType((int)i8SubRank) == 2)
		{
			this.m_bLegendSubType = true;
		}
		else
		{
			this.m_bLegendSubType = false;
		}
		string str = string.Empty;
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strBaserankImageKey))
		{
			str = string.Format("{0}", "UI/Soldier/" + this.m_strBaserankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), this.m_strBaserankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgraderankImageKey))
		{
			str = string.Format("{0}", "UI/Soldier/" + this.m_strUpgraderankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SetBundleImage), this.m_strUpgraderankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strSubrankImageKey))
		{
			str = string.Format("{0}", "UI/Soldier/" + this.m_strSubrankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem3 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem3.SetItemType(ItemType.USER_ASSETB);
			wWWItem3.SetCallback(new PostProcPerItem(this.SetBundleImage), this.m_strSubrankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem3, DownGroup.RUNTIME, true);
		}
		if (!this.m_bSetrankText)
		{
			this.m_strUpgraderankTextImageKey = this.GetRankText((int)i8UpgradeRank);
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgraderankTextImageKey))
			{
				str = string.Format("{0}", "UI/Soldier/" + this.m_strUpgraderankTextImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem4 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem4.SetItemType(ItemType.USER_ASSETB);
				wWWItem4.SetCallback(new PostProcPerItem(this.SetBundleImage), this.m_strUpgraderankTextImageKey);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem4, DownGroup.RUNTIME, true);
			}
		}
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(i32CostumeUnique);
		if (UIDataManager.IsUse256Texture())
		{
			this.m_strBasefaceImageKey = charKindInfo.GetPortraitFile1((int)i8BaseRank, costumePortraitPath) + "_256";
		}
		else
		{
			this.m_strBasefaceImageKey = charKindInfo.GetPortraitFile1((int)i8BaseRank, costumePortraitPath) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strBasefaceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.m_strBasefaceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		if (UIDataManager.IsUse256Texture())
		{
			this.m_strUpgradefaceImageKey = charKindInfo3.GetPortraitFile1((int)i8UpgradeRank, string.Empty) + "_256";
		}
		else
		{
			this.m_strUpgradefaceImageKey = charKindInfo3.GetPortraitFile1((int)i8UpgradeRank, string.Empty) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgradefaceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.m_strUpgradefaceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		if (UIDataManager.IsUse256Texture())
		{
			this.m_strSubfaceImageKey = charKindInfo2.GetPortraitFile1((int)i8SubRank, string.Empty) + "_256";
		}
		else
		{
			this.m_strSubfaceImageKey = charKindInfo2.GetPortraitFile1((int)i8SubRank, string.Empty) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strSubfaceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.m_strSubfaceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		str = string.Format("{0}", "UI/Soldier/fx_legendcard_compose" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem5 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem5.SetItemType(ItemType.USER_ASSETB);
		wWWItem5.SetCallback(new PostProcPerItem(this.SolComposeSuccess), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem5, DownGroup.RUNTIME, true);
		this.eBUNDLEDOWN = SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING;
	}

	private void SolComposeSuccess(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (this == null)
		{
			return;
		}
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.mainAsset as GameObject;
			if (null == gameObject)
			{
				return;
			}
			if (null != this.rootEffectGameObject)
			{
				UnityEngine.Object.Destroy(this.rootEffectGameObject);
			}
			this.rootEffectGameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
			if (null == this.rootEffectGameObject)
			{
				return;
			}
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
			effectUIPos.z = 300f;
			this.rootEffectGameObject.transform.position = effectUIPos;
			NkUtil.SetAllChildLayer(this.rootEffectGameObject, GUICamera.UILayer);
			this.rootEffectGameObject.SetActive(false);
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
			}
			this.SetComposeUpgrade();
		}
	}

	public bool SetObjectTexture(string strfaceImage, string faceObjName)
	{
		if (this.SubEffectGameObject == null)
		{
			return false;
		}
		GameObject gameObject = NkUtil.GetChild(this.SubEffectGameObject.transform, faceObjName).gameObject;
		if (null != gameObject)
		{
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(strfaceImage);
			if (null != texture)
			{
				Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
				if (null != material)
				{
					material.mainTexture = texture;
					if (null != gameObject.renderer)
					{
						gameObject.renderer.sharedMaterial = material;
						return true;
					}
				}
			}
		}
		return true;
	}

	public bool SetRankTextTexture(string strfaceImage, string faceObjName)
	{
		if (this.SubEffectGameObject == null)
		{
			return false;
		}
		GameObject gameObject = NkUtil.GetChild(this.SubEffectGameObject.transform, faceObjName).gameObject;
		if (null != gameObject)
		{
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(strfaceImage);
			if (null != texture)
			{
				Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
				if (null != material)
				{
					material.mainTexture = texture;
					if (null != gameObject.renderer)
					{
						gameObject.renderer.sharedMaterial = material;
						return true;
					}
				}
			}
		}
		return true;
	}

	public override void Update()
	{
		if (this.m_bEffectUpdate)
		{
			if (this.eBUNDLEDOWN == SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING)
			{
				if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strBasefaceImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgradefaceImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strSubfaceImageKey) != null && this.SetObjectTexture(this.m_strBasefaceImageKey, "face01") && this.SetObjectTexture(this.m_strSubfaceImageKey, "face02"))
				{
					this.m_bSetface = true;
				}
				if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strBaserankImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgraderankImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strSubrankImageKey) != null && this.SetObjectTexture(this.m_strBaserankImageKey, "rank01") && this.SetObjectTexture(this.m_strSubrankImageKey, "rank02"))
				{
					if (this.m_bComposeTranscendence)
					{
						if (this.SetObjectTexture(this.m_strUpgraderankImageKey, "rank03"))
						{
							this.m_bSetrank = true;
						}
					}
					else
					{
						this.m_bSetrank = true;
					}
				}
				if (!this.m_bSetrankText && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strUpgraderankTextImageKey) != null && this.m_bComposeTranscendence)
				{
					this.SetObjectTexture(this.m_strUpgraderankTextImageKey, "card_grade");
					this.m_bSetrankText = true;
				}
				if (this.m_bSetface && this.m_bSetrank && this.m_bSetrankText)
				{
					this.eBUNDLEDOWN = SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE;
					this.SetPlay();
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
					}
				}
			}
			else if (this.eBUNDLEDOWN == SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE)
			{
				Animation componentInChildren = this.SubEffectGameObject.GetComponentInChildren<Animation>();
				this.SetItemText();
				if (componentInChildren != null && !componentInChildren.isPlaying)
				{
					this.m_bEffectUpdate = false;
					this.eBUNDLEDOWN = SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_OK;
					this.SetLoopPlay();
				}
			}
		}
	}

	public void SetPlay()
	{
		if (null == this.rootEffectGameObject)
		{
			return;
		}
		string strName = "fx_cut01";
		string strName2 = "fx_cut02";
		this.rootEffectGameObject.SetActive(true);
		GameObject gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, strName).gameObject;
		if (gameObject != null)
		{
			if (this.m_bComposeTranscendence)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		GameObject gameObject2 = NkUtil.GetChild(this.rootEffectGameObject.transform, strName2).gameObject;
		if (gameObject2 != null)
		{
			if (this.m_bComposeTranscendence)
			{
				gameObject2.SetActive(false);
			}
			else
			{
				gameObject2.SetActive(true);
			}
		}
	}

	public void SetLoopPlay()
	{
		if (null == this.rootEffectGameObject)
		{
			return;
		}
		if (!this.m_bComposeTranscendence)
		{
			return;
		}
		GameObject gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, "loops").gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		GameObject gameObject2 = NkUtil.GetChild(this.rootEffectGameObject.transform, "square_mesh_loop_2").gameObject;
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
		}
	}

	public void SetComposeUpgrade()
	{
		this.m_bEffectUpdate = true;
		if (null == this.rootEffectGameObject)
		{
			return;
		}
		this.SubEffectGameObject = null;
		string strName = "fx_cut01";
		string strName2 = "fx_cut02";
		this.rootEffectGameObject.SetActive(false);
		GameObject gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, strName).gameObject;
		if (gameObject != null && this.m_bComposeTranscendence)
		{
			this.SubEffectGameObject = gameObject;
			this.SetLegendCardActive(gameObject);
		}
		GameObject gameObject2 = NkUtil.GetChild(this.rootEffectGameObject.transform, strName2).gameObject;
		if (gameObject2 != null && !this.m_bComposeTranscendence)
		{
			this.SubEffectGameObject = gameObject2;
			this.SetLegendCardActive(gameObject2);
		}
	}

	private void SetLegendCardActive(GameObject obj)
	{
		GameObject gameObject = NkUtil.GetChild(obj.transform, "card01").gameObject;
		GameObject gameObject2 = NkUtil.GetChild(obj.transform, "card02").gameObject;
		if (gameObject == null || gameObject2 == null)
		{
			return;
		}
		GameObject gameObject3 = NkUtil.GetChild(gameObject.transform, "premiem_card_silver").gameObject;
		GameObject gameObject4 = NkUtil.GetChild(gameObject.transform, "premiem_card_gold").gameObject;
		if (gameObject3 == null || gameObject4 == null)
		{
			return;
		}
		if (this.m_bLegendBaseType)
		{
			gameObject3.SetActive(false);
			gameObject4.SetActive(true);
		}
		else
		{
			gameObject3.SetActive(true);
			gameObject4.SetActive(false);
		}
		gameObject3 = NkUtil.GetChild(gameObject2.transform, "premiem_card_silver").gameObject;
		gameObject4 = NkUtil.GetChild(gameObject2.transform, "premiem_card_gold").gameObject;
		if (gameObject3 == null || gameObject4 == null)
		{
			return;
		}
		if (this.m_bLegendSubType)
		{
			gameObject3.SetActive(false);
			gameObject4.SetActive(true);
		}
		else
		{
			gameObject3.SetActive(true);
			gameObject4.SetActive(false);
		}
	}

	private void SetItemText()
	{
		if (!this.m_bComposeTranscendence)
		{
			if (this.SubEffectGameObject == null)
			{
				return;
			}
			GameObject gameObject = NkUtil.GetChild(this.SubEffectGameObject.transform, "fx_text_area").gameObject;
			if (gameObject != null)
			{
				Vector3 position = default(Vector3);
				position.x = gameObject.transform.position.x - 140f;
				position.y = gameObject.transform.position.y + 30f;
				position.z = gameObject.transform.position.z;
				this.solMovieText.gameObject.transform.position = position;
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2837"),
					"Count",
					this.m_i32FailItemNum.ToString()
				});
				this.solMovieText.Visible = true;
				this.solMovieText.SetText(empty);
				this.solMovieText.SetCharacterSize(38f);
				this.solMovieText.SetMultiLine(false);
			}
		}
	}

	public string GetRankText(int solgrade)
	{
		string result = string.Empty;
		if (solgrade < 6 || solgrade > 13)
		{
			return result;
		}
		switch (solgrade)
		{
		case 6:
		case 10:
			result = "extractgrade_a";
			break;
		case 7:
		case 11:
			result = "extractgrade_s";
			break;
		case 8:
		case 12:
			result = "extractgrade_ss";
			break;
		case 9:
		case 13:
			result = "extractgrade_ex";
			break;
		}
		return result;
	}

	public string GetLegendType(NrCharKindInfo kindinfo, int solgrade)
	{
		string result = string.Empty;
		if (kindinfo == null)
		{
			return result;
		}
		if (kindinfo.GetLegendType(solgrade) == 2)
		{
			result = "rankm";
		}
		else
		{
			result = "rankl";
		}
		return result;
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
				}
			}
		}
	}

	private void ClickCloseButton(IUIObject obj)
	{
		if (this.eBUNDLEDOWN == SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_OK)
		{
			this.eBUNDLEDOWN = SolTranscendenceSuccess.eBUNDLEDOWNSTATE.eBUNDLE_NONE;
			if (null != this.rootEffectGameObject)
			{
				UnityEngine.Object.Destroy(this.rootEffectGameObject);
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
			{
				SolComposeMainDlg.Instance.ComposeTranscendence();
			}
			NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
		}
	}

	public override void OnClose()
	{
		UIDataManager.MuteSound(false);
		if (null != this.rootEffectGameObject)
		{
			UnityEngine.Object.Destroy(this.rootEffectGameObject);
		}
		Resources.UnloadUnusedAssets();
		if (this._closeCallback != null)
		{
			this._closeCallback();
		}
		base.OnClose();
	}

	public void AddCloseCallback(OnCloseCallback callback)
	{
		this._closeCallback = (OnCloseCallback)Delegate.Combine(this._closeCallback, callback);
	}
}
