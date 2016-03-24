using GameMessage;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Myth_Evolution_Success_DLG : Form
{
	private enum eBUNDLEDOWNSTATE
	{
		eBUNDLE_NONE,
		eBUNDLE_DOWNING,
		eBUNDLE_DOWNCOMPLTE,
		eBUNDLE_OK
	}

	private Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE eBUNDLEDOWN;

	private GameObject rootEffectGameObject;

	private string FaceImageKey = string.Empty;

	private string BaseRankImageKey = string.Empty;

	private string ChangeRankImageKey = string.Empty;

	private NkSoldierInfo m_pkSolinfo;

	private byte m_bBaseGrade;

	private byte m_bChangeGrade;

	private bool bEffectUpdate;

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

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruit", G_ID.MYTH_EVOLUTION_SUCCESS_DLG, false);
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
		this.solMovietTrans.Visible = false;
		this.solMovie = (base.GetControl("BT_Movie") as Button);
		this.solMovie.Visible = false;
		this.solMovieText = (base.GetControl("LB_Movie") as Label);
		this.solMovieText.Visible = false;
		this.SetLocationData();
		base.DonotDepthChange(360f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
			base.BLACK_BG.gameObject.transform.localPosition = new Vector3(base.BLACK_BG.gameObject.transform.localPosition.x, base.BLACK_BG.gameObject.transform.localPosition.y, 300f);
		}
	}

	public void LoadMyth_Evolurion(NkSoldierInfo pkSolinfo, byte i8BaseGrade, byte i8ChangeGrade)
	{
		TsAudio.StoreMuteAllAudio();
		TsAudio.SetExceptMuteAllAudio(EAudioType.UI, true);
		TsAudio.RefreshAllMuteAudio();
		this.m_pkSolinfo = pkSolinfo;
		this.m_bBaseGrade = i8BaseGrade;
		this.m_bChangeGrade = i8ChangeGrade;
		string str = string.Format("{0}", "Effect/Instant/fx_ui_shinhwacard" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.EvolutionSuccess), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void EvolutionSuccess(WWWItem _item, object _param)
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
			this.rootEffectGameObject.SetActive(true);
			this.SetTextureBundle();
			this.bEffectUpdate = true;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
			}
		}
	}

	private void SetGui()
	{
		this.solMovie.Visible = false;
		this.solMovietTrans.Visible = false;
		this.solMovieText.Visible = false;
	}

	private void SetTextureBundle()
	{
		if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_pkSolinfo.GetCharKind()) == null)
		{
			return;
		}
		this.SetGui();
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.m_pkSolinfo);
		string text = MsgHandler.HandleReturn<string>("PortraitFileName", new object[]
		{
			this.m_pkSolinfo.GetCharKind(),
			(int)(this.m_bBaseGrade - 1)
		});
		if (string.Empty == text)
		{
			return;
		}
		if (!string.IsNullOrEmpty(costumePortraitPath))
		{
			string text2 = MsgHandler.HandleReturn<string>("PortraitCostumeFileName", new object[]
			{
				this.m_pkSolinfo.GetCharKind(),
				(int)(this.m_bBaseGrade - 1),
				costumePortraitPath
			});
			if (!string.IsNullOrEmpty(text2))
			{
				text = text2;
			}
		}
		if (UIDataManager.IsUse256Texture())
		{
			this.FaceImageKey = text + "_256";
		}
		else
		{
			this.FaceImageKey = text + "_512";
		}
		this.BaseRankImageKey = "rankl" + this.m_bBaseGrade.ToString();
		this.ChangeRankImageKey = "rankm" + this.m_bChangeGrade.ToString();
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.BaseRankImageKey))
		{
			string str = string.Format("{0}", "UI/Soldier/" + this.BaseRankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), this.BaseRankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.ChangeRankImageKey))
		{
			string str2 = string.Format("{0}", "UI/Soldier/" + this.ChangeRankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SetBundleImage), this.ChangeRankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
		}
		this.eBUNDLEDOWN = Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING;
	}

	public override void Update()
	{
		if (this.bEffectUpdate && null != this.rootEffectGameObject)
		{
			if (this.eBUNDLEDOWN == Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_DOWNING)
			{
				if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.BaseRankImageKey) != null && NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.ChangeRankImageKey) != null)
				{
					GameObject gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, "fx_card01").gameObject;
					if (null != gameObject)
					{
						GameObject gameObject2 = NkUtil.GetChild(gameObject.transform, "fx_plan_face").gameObject;
						if (null != gameObject2)
						{
							Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey);
							if (null != texture)
							{
								Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
								if (null != material)
								{
									material.mainTexture = texture;
									if (null != gameObject2.renderer)
									{
										gameObject2.renderer.sharedMaterial = material;
									}
								}
							}
						}
						GameObject gameObject3 = NkUtil.GetChild(gameObject.transform, "fx_card_rank").gameObject;
						if (null != gameObject3)
						{
							Texture2D texture2 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.BaseRankImageKey);
							if (null != texture2)
							{
								Material material2 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
								if (null != material2)
								{
									material2.mainTexture = texture2;
									if (null != gameObject3.renderer)
									{
										gameObject3.renderer.sharedMaterial = material2;
									}
								}
							}
						}
					}
					gameObject = NkUtil.GetChild(this.rootEffectGameObject.transform, "fx_card03").gameObject;
					if (null != gameObject)
					{
						GameObject gameObject2 = NkUtil.GetChild(gameObject.transform, "fx_plan_face").gameObject;
						if (null != gameObject2)
						{
							Texture2D texture3 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.FaceImageKey);
							if (null != texture3)
							{
								Material material3 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
								if (null != material3)
								{
									material3.mainTexture = texture3;
									if (null != gameObject2.renderer)
									{
										gameObject2.renderer.sharedMaterial = material3;
									}
								}
							}
						}
						GameObject gameObject3 = NkUtil.GetChild(gameObject.transform, "fx_card_rank").gameObject;
						if (null != gameObject3)
						{
							Texture2D texture4 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.ChangeRankImageKey);
							if (null != texture4)
							{
								Material material4 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
								if (null != material4)
								{
									material4.mainTexture = texture4;
									if (null != gameObject3.renderer)
									{
										gameObject3.renderer.sharedMaterial = material4;
									}
								}
							}
						}
					}
					this.eBUNDLEDOWN = Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE;
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootEffectGameObject);
					}
				}
			}
			else if (this.eBUNDLEDOWN == Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_DOWNCOMPLTE)
			{
				Animation componentInChildren = this.rootEffectGameObject.GetComponentInChildren<Animation>();
				if (componentInChildren != null && !componentInChildren.isPlaying)
				{
					this.bEffectUpdate = false;
					this.closeUIButton.Visible = true;
					this.eBUNDLEDOWN = Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_OK;
				}
			}
		}
	}

	private void SetLocationData()
	{
		this.closeUIButton.SetSize(GUICamera.width, GUICamera.height);
		this.closeUIButton.SetLocation(0, 0);
	}

	private void ClickCloseButton(IUIObject obj)
	{
		if (this.eBUNDLEDOWN == Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_OK)
		{
			this.eBUNDLEDOWN = Myth_Evolution_Success_DLG.eBUNDLEDOWNSTATE.eBUNDLE_NONE;
			this.CloseForm(null);
		}
	}

	public override void CloseForm(IUIObject obj)
	{
		if (null != this.rootEffectGameObject)
		{
			UnityEngine.Object.Destroy(this.rootEffectGameObject);
		}
		Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
		if (myth_Evolution_Main_DLG != null)
		{
			myth_Evolution_Main_DLG.SetEvolution();
		}
		NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
		base.CloseForm(obj);
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

	public override void OnClose()
	{
		TsAudio.RestoreMuteAllAudio();
		TsAudio.RefreshAllMuteAudio();
		if (null != this.rootEffectGameObject)
		{
			UnityEngine.Object.Destroy(this.rootEffectGameObject);
		}
		base.OnClose();
	}
}
