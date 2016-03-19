using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolElementSuccessDlg : Form
{
	private enum eBUNDLEDOWNSTATE
	{
		eBUNDLE_NONE,
		eBUNDLE_DOWNING,
		eBUNDLE_DOWNCOMPLTE,
		eBUNDLE_OK
	}

	private GameObject rootEffectGameObject;

	private bool bEffectUpdate;

	private string FaceImageKey = string.Empty;

	private string RankImageKey = string.Empty;

	private string SeasonImageKey = string.Empty;

	private SolElementSuccessDlg.eBUNDLEDOWNSTATE eBUNDLEDOWN;

	private SOLDIER_INFO m_pSolInfo;

	private int m_i32SelectCharKind;

	private byte m_bSelectGrade;

	private byte m_bSelectSeason;

	private bool m_bShowSolMovie;

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

	private DrawTexture faceBookImg;

	private Label faceBookText;

	private Button facebookTrans;

	private bool bLegend;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruitSuccess", G_ID.SOLELEMENTSUCCESS_DLG, false);
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
		this.closeUIButton.Visible = false;
		this.skipButton = (base.GetControl("Button_Skip") as Button);
		this.skipButton.Visible = false;
		this.solMovietTrans = (base.GetControl("Button_MovieBtn") as Button);
		this.solMovietTrans.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolMovie));
		this.solMovietTrans.Visible = false;
		this.solMovie = (base.GetControl("BT_Movie") as Button);
		this.solMovie.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolMovie));
		this.solMovie.Visible = false;
		this.solMovieText = (base.GetControl("LB_Movie") as Label);
		this.solMovieText.Visible = false;
		this.faceBookImg = (base.GetControl("DT_Reuse") as DrawTexture);
		this.faceBookImg.Visible = false;
		this.facebookTrans = (base.GetControl("BT_Reuse") as Button);
		this.facebookTrans.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickFaceBookButton));
		this.facebookTrans.Visible = false;
		this.faceBookText = (base.GetControl("LB_Reuse") as Label);
		this.faceBookText.Visible = false;
		this.SetData();
		base.DonotDepthChange(360f);
	}

	private void SolRecruitSuccess(WWWItem _item, object _param)
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
			this.SetTextureBundle();
			this.SetElementSuccessDlgGUI();
			this.bEffectUpdate = true;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
			}
		}
	}

	public void LoadSolCompleteElement(SOLDIER_INFO pkSolInfo)
	{
		if (pkSolInfo == null)
		{
			return;
		}
		this.bLegend = false;
		this.m_pSolInfo = pkSolInfo;
		this.m_i32SelectCharKind = pkSolInfo.CharKind;
		this.m_bSelectGrade = pkSolInfo.Grade;
		this.m_bSelectGrade += 1;
		this.m_bSelectSeason = NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindSeason(pkSolInfo.CharKind);
		string str = string.Format("{0}", "UI/Soldier/fx_direct_engineering" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SolRecruitSuccess), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void LoadLegendSolCompleteElement(SOLDIER_INFO pkSolInfo)
	{
		if (pkSolInfo == null)
		{
			return;
		}
		this.bLegend = true;
		this.m_pSolInfo = pkSolInfo;
		this.m_i32SelectCharKind = pkSolInfo.CharKind;
		this.m_bSelectGrade = pkSolInfo.Grade;
		this.m_bSelectGrade += 1;
		this.m_bSelectSeason = NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindSeason(pkSolInfo.CharKind);
		string str = string.Format("{0}", "UI/Soldier/fx_direct_dragonhero" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SolRecruitSuccess), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public override void Update()
	{
		if (this.bEffectUpdate && null != this.rootEffectGameObject)
		{
			if (this.eBUNDLEDOWN == SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING)
			{
				if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.RankImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.SeasonImageKey) != null)
				{
					GameObject gameObject;
					if (this.bLegend)
					{
						gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, "face").gameObject;
					}
					else
					{
						gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, "fx_plan_face").gameObject;
					}
					if (null != gameObject)
					{
						Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey);
						if (null != texture)
						{
							Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material)
							{
								material.mainTexture = texture;
								if (null != gameObject.renderer)
								{
									gameObject.renderer.sharedMaterial = material;
								}
							}
						}
					}
					GameObject gameObject2;
					if (this.bLegend)
					{
						gameObject2 = NkUtil.GetChild(this.rootEffectGameObject.transform, "rank").gameObject;
					}
					else
					{
						gameObject2 = NkUtil.GetChild(this.rootEffectGameObject.transform, "fx_card_rank").gameObject;
					}
					if (null != gameObject2)
					{
						Texture2D texture2 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.RankImageKey);
						if (null != texture2)
						{
							Material material2 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material2)
							{
								material2.mainTexture = texture2;
								if (null != gameObject2.renderer)
								{
									gameObject2.renderer.sharedMaterial = material2;
								}
							}
						}
					}
					GameObject gameObject3 = NkUtil.GetChild(this.rootEffectGameObject.transform, "fx_font_number").gameObject;
					if (null != gameObject3)
					{
						Texture2D texture3 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.SeasonImageKey);
						if (null != texture3)
						{
							Material material3 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material3)
							{
								material3.mainTexture = texture3;
								if (null != gameObject3.renderer)
								{
									gameObject3.renderer.sharedMaterial = material3;
								}
							}
						}
					}
					this.eBUNDLEDOWN = SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE;
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
					}
					this.rootEffectGameObject.SetActive(true);
				}
			}
			else if (this.eBUNDLEDOWN == SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE)
			{
				Animation componentInChildren = this.rootEffectGameObject.GetComponentInChildren<Animation>();
				if (componentInChildren != null && !componentInChildren.isPlaying)
				{
					this.FacebookGui(true);
					this.IntroGui(true);
					this.SetSolName();
					this.bEffectUpdate = false;
					this.closeUIButton.Visible = true;
					this.eBUNDLEDOWN = SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_OK;
				}
			}
		}
	}

	private void SetTextureBundle()
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32SelectCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		if (UIDataManager.IsUse256Texture())
		{
			this.FaceImageKey = charKindInfo.GetPortraitFile1((int)(this.m_bSelectGrade - 1)) + "_256";
		}
		else
		{
			this.FaceImageKey = charKindInfo.GetPortraitFile1((int)(this.m_bSelectGrade - 1)) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.FaceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		if (this.bLegend)
		{
			this.RankImageKey = "rankl" + this.m_bSelectGrade.ToString();
		}
		else
		{
			this.RankImageKey = "rank" + this.m_bSelectGrade.ToString();
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.RankImageKey))
		{
			string str = string.Format("{0}", "UI/Soldier/" + this.RankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), this.RankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		this.SeasonImageKey = "font_number" + this.m_bSelectSeason.ToString();
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.SeasonImageKey))
		{
			string str2 = string.Format("{0}", "UI/Soldier/" + this.SeasonImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SetBundleImage), this.SeasonImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
		}
		this.eBUNDLEDOWN = SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING;
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

	private void ClickSolMovie(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int charkind = (int)obj.Data;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			return;
		}
		string sOLINTRO = charKindInfo.GetCHARKIND_INFO().SOLINTRO;
		if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				string str = string.Format("{0}SOLINTRO/", Option.GetProtocolRootPath(Protocol.HTTP));
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false);
		}
	}

	private void ClickFaceBookButton(IUIObject obj)
	{
		Facebook_Feed_Dlg facebook_Feed_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.FACEBOOK_FEED_DLG) as Facebook_Feed_Dlg;
		if (facebook_Feed_Dlg != null)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32SelectCharKind);
			if (charKindInfo != null)
			{
				facebook_Feed_Dlg.SetType(eFACEBOOK_FEED_TYPE.DKALCHE_SOL, this.m_pSolInfo);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.FACEBOOK_FEED_DLG);
			}
		}
	}

	private void ClickCloseButton(IUIObject obj)
	{
		if (this.eBUNDLEDOWN == SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_OK)
		{
			this.eBUNDLEDOWN = SolElementSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLE_NONE;
			if (null != this.rootEffectGameObject)
			{
				UnityEngine.Object.Destroy(this.rootEffectGameObject);
			}
			SolDetail_Info_Dlg solDetail_Info_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_DLG) as SolDetail_Info_Dlg;
			if (solDetail_Info_Dlg != null)
			{
				if (this.bLegend)
				{
					solDetail_Info_Dlg.SetLegendElementGui();
				}
				else
				{
					solDetail_Info_Dlg.SetElementGui();
				}
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
		base.OnClose();
	}

	private void SetElementSuccessDlgGUI()
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32SelectCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		this.FacebookGui(false);
		this.IntroGui(false);
		this.m_bShowSolMovie = false;
		if (charKindInfo.GetCHARKIND_INFO().SOLINTRO != "0")
		{
			this.m_bShowSolMovie = true;
			this.solMovie.Data = this.m_i32SelectCharKind;
			this.solMovietTrans.Data = this.m_i32SelectCharKind;
		}
	}

	private void SetData()
	{
		this.closeUIButton.SetSize(GUICamera.width, GUICamera.height);
		this.closeUIButton.SetLocation(0, 0);
		this.skipButton.SetSize(GUICamera.width, GUICamera.height);
		this.skipButton.SetLocation(0, 0);
		this.closeBack.SetSize(GUICamera.width, this.closeBack.height);
		this.closeBack.SetLocation(0f, GUICamera.height - this.closeBack.height);
		this.closeText.width = GUICamera.width;
		this.closeText.SetLocation(0f, this.closeBack.GetLocationY() + 6f, -12f);
		float num = GUICamera.width / 2f;
		if (this.solName.width >= num)
		{
			this.solName.SetLocation(0f, GUICamera.height / 2f + this.solName.height / 2f, -102f);
			this.nameBack.SetLocation(0f, GUICamera.height / 2f + this.nameBack.height / 2f + 13f, -102f);
		}
		else if (this.solName.width < num)
		{
			this.solName.SetLocation(num - this.solName.width + 50f, GUICamera.height / 2f + this.solName.height / 2f, -102f);
			this.nameBack.SetLocation(num - this.nameBack.width + 50f, GUICamera.height / 2f + this.nameBack.height / 2f + 13f, -102f);
		}
		this.solMovie.SetLocation(this.solMovie.GetLocationX(), this.closeBack.GetLocationY() - this.solMovie.GetSize().y - 10f, -102f);
		this.solMovieText.SetLocation(this.solMovieText.GetLocationX(), this.closeBack.GetLocationY() - this.solMovie.GetSize().y - 5f, -102f);
		this.solMovietTrans.SetLocation(this.solMovietTrans.GetLocationX(), this.closeBack.GetLocationY() - this.solMovietTrans.GetSize().y - 10f, -102f);
		this.faceBookImg.SetLocation(this.faceBookImg.GetLocation().x, this.closeBack.GetLocationY() - this.faceBookImg.GetSize().y - 10f, -102f);
		this.faceBookText.SetLocation(this.faceBookText.GetLocation().x, this.closeBack.GetLocationY() - this.faceBookImg.GetSize().y - 5f, -102f);
		this.facebookTrans.SetLocation(this.facebookTrans.GetLocation().x, this.closeBack.GetLocationY() - this.facebookTrans.GetSize().y - 10f, -102f);
	}

	public void FacebookGui(bool bShow)
	{
		if (TsPlatform.IsBand)
		{
			return;
		}
		this.faceBookImg.Visible = bShow;
		this.faceBookText.Visible = bShow;
		this.facebookTrans.Visible = bShow;
	}

	public void IntroGui(bool bShow)
	{
		if (this.m_bShowSolMovie)
		{
			this.solMovie.Visible = bShow;
			this.solMovietTrans.Visible = bShow;
			this.solMovieText.Visible = bShow;
		}
		else
		{
			this.solMovie.Visible = false;
			this.solMovietTrans.Visible = false;
			this.solMovieText.Visible = false;
		}
	}

	public void SetSolName()
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32SelectCharKind);
		if (charKindInfo != null)
		{
			this.nameBack.Visible = true;
			this.solName.Visible = true;
			this.solName.SetFlashLabel(charKindInfo.GetName());
		}
	}
}
