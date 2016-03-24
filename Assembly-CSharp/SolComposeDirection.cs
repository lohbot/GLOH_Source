using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolComposeDirection : Form
{
	private const float EVENT_ANI_ENDTIME = 6f;

	private DrawTexture bgImage;

	private GameObject rootGameObject;

	private GameObject BasefaceGameObject;

	private Animation aniGameObject;

	private GameObject FinishfaceGameObject;

	private string BaseSolImageKey = string.Empty;

	private string strObjectKey = string.Empty;

	private bool bUpdate;

	private bool bSetBaseFace;

	private int m_ComposeType;

	private float startTime;

	private Shader m_Shader;

	private SOLCOMPOSE_TYPE m_SolComposeMainType;

	public int[] m_ExtractItemNum = new int[10];

	public bool[] m_bGreat = new bool[10];

	private int m_ExtractCount;

	private GameObject ExtractResultrootGameObject;

	private Label lbExtractItemCount;

	private Button btnOk;

	private bool bPlayExtractResult;

	public OnCloseCallback _closeCallback;

	public Shader SHADER
	{
		get
		{
			if (this.m_Shader == null)
			{
				if (TsPlatform.IsWeb || TsPlatform.IsEditor)
				{
					this.m_Shader = Shader.Find("AT2/_Effect/Standard");
				}
				if (TsPlatform.IsMobile)
				{
					this.m_Shader = Shader.Find("AT2/Effect/Standard_mobile");
				}
			}
			return this.m_Shader;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolComposeDirection", G_ID.SOLCOMPOSE_DIRECTION_DLG, true);
		base.InteractivePanel.draggable = false;
	}

	public override void SetComponent()
	{
		this.bgImage = (base.GetControl("DrawTexture_BG01") as DrawTexture);
		Texture2D texture2D = CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/bg_solcompose") as Texture2D;
		if (null != texture2D)
		{
			this.bgImage.SetSize(GUICamera.width, GUICamera.height);
			this.bgImage.SetTexture(texture2D);
		}
		this.lbExtractItemCount = (base.GetControl("Label_Extract_Result") as Label);
		this.lbExtractItemCount.SetLocation(GUICamera.width / 2f - this.lbExtractItemCount.GetSize().x / 2f, this.lbExtractItemCount.GetLocationY(), -90f);
		this.lbExtractItemCount.Visible = false;
		this.btnOk = (base.GetControl("Button_Confirm") as Button);
		Button expr_E1 = this.btnOk;
		expr_E1.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E1.Click, new EZValueChangedDelegate(this.BtnClickOk));
		this.btnOk.SetLocation(GUICamera.width / 2f - this.btnOk.GetSize().x / 2f, this.btnOk.GetLocationY(), -90f);
		this.btnOk.Hide(true);
		SolComposeMainDlg solComposeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_DLG) as SolComposeMainDlg;
		if (solComposeMainDlg == null)
		{
			solComposeMainDlg = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG) as SolComposeMainDlg);
		}
		if (solComposeMainDlg != null)
		{
			this.m_SolComposeMainType = solComposeMainDlg.GetSolComposeType();
			if (this.m_SolComposeMainType == SOLCOMPOSE_TYPE.EXTRACT)
			{
				this.bgImage.SetTextureKey("Win_T_WH");
				string str = string.Empty;
				str = string.Format("effect/instant/fx_direct_extraction{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.SolComposeExtract), null);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
			else
			{
				string str2 = string.Empty;
				str2 = string.Format("UI/Soldier/fx_direct_solcompose{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem2.SetItemType(ItemType.USER_ASSETB);
				wWWItem2.SetCallback(new PostProcPerItem(this.SolComposeSuccess), null);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
			}
		}
		UIDataManager.MuteSound(true);
		this.SetSize();
		base.DonotDepthChange(360f);
		this.Hide();
	}

	public void SetBG(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.bgImage.SetSize(GUICamera.width, GUICamera.height);
				this.bgImage.SetTexture(texture2D);
			}
		}
	}

	public void SetExtractData(bool[] bGreat, int[] ExtractItemNum)
	{
		this.m_bGreat = bGreat;
		this.m_ExtractItemNum = ExtractItemNum;
		this.m_ExtractCount = 0;
	}

	public void SetExtractResultData()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2837"),
			"Count",
			this.m_ExtractItemNum[this.m_ExtractCount].ToString()
		});
		this.lbExtractItemCount.SetText(empty);
		TsLog.LogWarning("!!!! TEXT : {0} = {1} = ", new object[]
		{
			this.m_ExtractCount,
			empty,
			this.m_ExtractItemNum[this.m_ExtractCount]
		});
		this.m_ExtractCount++;
		this.lbExtractItemCount.Visible = true;
		this.btnOk.Hide(false);
	}

	private void BtnClickOk(IUIObject obj)
	{
		if (this.m_ExtractCount >= 10 || this.m_ExtractItemNum[this.m_ExtractCount] == 0)
		{
			this.Close();
		}
		else
		{
			this.lbExtractItemCount.Visible = false;
			this.btnOk.Hide(true);
			this.SetExtractBundlePlay();
			this.bPlayExtractResult = true;
			this.bUpdate = true;
		}
	}

	private void SolComposeExtractResult(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.ExtractResultrootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (null == this.ExtractResultrootGameObject)
				{
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.ExtractResultrootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.ExtractResultrootGameObject, GUICamera.UILayer);
				this.SetExtractBundlePlay();
			}
		}
	}

	private void SetExtractBundlePlay()
	{
		string strName = "fx_many";
		string strName2 = "fx_small";
		if (this.m_bGreat[this.m_ExtractCount])
		{
			Transform child = NkUtil.GetChild(this.ExtractResultrootGameObject.transform, strName2);
			if (child != null && child.gameObject.activeInHierarchy)
			{
				child.gameObject.SetActive(false);
			}
			Transform child2 = NkUtil.GetChild(this.ExtractResultrootGameObject.transform, strName);
			if (child2 != null)
			{
				if (child2.gameObject.activeInHierarchy)
				{
					child2.gameObject.SetActive(false);
				}
				child2.gameObject.SetActive(true);
			}
		}
		else
		{
			Transform child3 = NkUtil.GetChild(this.ExtractResultrootGameObject.transform, strName);
			if (child3 != null && child3.gameObject.activeInHierarchy)
			{
				child3.gameObject.SetActive(false);
			}
			Transform child4 = NkUtil.GetChild(this.ExtractResultrootGameObject.transform, strName2);
			if (child4 != null)
			{
				if (child4.gameObject.activeInHierarchy)
				{
					child4.gameObject.SetActive(false);
				}
				child4.gameObject.SetActive(true);
			}
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.ExtractResultrootGameObject);
		}
		this.Show();
	}

	private void SolComposeExtract(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (null == this.rootGameObject)
				{
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.aniGameObject = this.rootGameObject.GetComponentInChildren<Animation>();
				if (null == this.aniGameObject)
				{
					return;
				}
				this.bUpdate = true;
				this.startTime = Time.realtimeSinceStartup;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
				this.Show();
			}
		}
	}

	private void SolComposeSuccess(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (null == this.rootGameObject)
				{
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.aniGameObject = this.rootGameObject.GetComponentInChildren<Animation>();
				if (null == this.aniGameObject)
				{
					return;
				}
				this.bUpdate = true;
				this.startTime = Time.realtimeSinceStartup;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
				this.Show();
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
				}
			}
		}
	}

	private void SetBackImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.bgImage.SetSize(GUICamera.width, GUICamera.height);
				this.bgImage.SetTexture(texture2D);
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}

	public void SetImage(NkSoldierInfo kBaseSol, int Type)
	{
		this.SetImage(kBaseSol, null, Type);
	}

	public void SetImage(NkSoldierInfo kBaseSol, NkSoldierInfo kSubSol, int Type)
	{
		this.m_ComposeType = Type;
		this.BaseSolImageKey = this.SetFaceImageKey(kBaseSol);
	}

	private string SetFaceImageKey(NkSoldierInfo kSol)
	{
		string text = string.Empty;
		if (kSol == null)
		{
			return text;
		}
		NrCharKindInfo charKindInfo = kSol.GetCharKindInfo();
		if (charKindInfo == null)
		{
			TsLog.LogError("SetImage SolID ={0} Not Found Kind", new object[]
			{
				kSol.GetCharKind()
			});
			return text;
		}
		int costumeUnique = (int)kSol.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(costumeUnique);
		string portraitFile = charKindInfo.GetPortraitFile1((int)kSol.GetGrade(), costumePortraitPath);
		if (UIDataManager.IsUse256Texture())
		{
			text = portraitFile + "_256";
		}
		else
		{
			text = portraitFile + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(text))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(text, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		return text;
	}

	private bool SetMaterial(ref GameObject _obj, string ImageKey)
	{
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(ImageKey);
		if (null != texture)
		{
			Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
			if (null != material)
			{
				material.mainTexture = texture;
				if (null != this.BasefaceGameObject.renderer)
				{
					_obj.renderer.sharedMaterial = material;
				}
				return true;
			}
		}
		else
		{
			TsLog.LogError("Not Found ImageKey = {0}", new object[]
			{
				ImageKey
			});
		}
		return false;
	}

	private void PlayExtractResult()
	{
		string str = string.Empty;
		str = string.Format("effect/instant/fx_direct_extraction_result{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SolComposeExtractResult), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public override void OnClose()
	{
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.DestroyObject(this.rootGameObject.gameObject);
		}
		if (null != this.ExtractResultrootGameObject)
		{
			UnityEngine.Object.DestroyObject(this.ExtractResultrootGameObject.gameObject);
		}
		if (this.m_SolComposeMainType != SOLCOMPOSE_TYPE.EXTRACT)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLCOMPOSE_SUCCESS_DLG);
			if (this.m_ComposeType == 1)
			{
				SolComposeSuccessDlg solComposeSuccessDlg = (SolComposeSuccessDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_SUCCESS_DLG);
				if (solComposeSuccessDlg != null)
				{
					solComposeSuccessDlg.LoadSolComposeSuccessBundle();
				}
			}
			else if (this.m_ComposeType == 3)
			{
				SolComposeSuccessDlg solComposeSuccessDlg2 = (SolComposeSuccessDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_SUCCESS_DLG);
				if (solComposeSuccessDlg2 != null)
				{
					solComposeSuccessDlg2.LoadSolLevelSuccessBundle();
				}
			}
			NrSound.ImmedatePlay("UI_SFX", "MERCENARY-COMPOSE", "SUCCESS");
		}
		UIDataManager.MuteSound(false);
		if (this._closeCallback != null)
		{
			this._closeCallback();
		}
		base.OnClose();
	}

	public override void Update()
	{
		if (this.bUpdate)
		{
			if (this.m_SolComposeMainType != SOLCOMPOSE_TYPE.EXTRACT)
			{
				if (!this.bSetBaseFace && null != this.rootGameObject)
				{
					this.strObjectKey = "fx_face_base";
					this.BasefaceGameObject = NkUtil.GetChild(this.rootGameObject.transform, this.strObjectKey).gameObject;
					if (null != this.BasefaceGameObject)
					{
						this.bSetBaseFace = this.SetMaterial(ref this.BasefaceGameObject, this.BaseSolImageKey);
					}
					else
					{
						TsLog.LogError("BaseSolImageKey = {0}  BasefaceGameObject == NULL", new object[]
						{
							this.BaseSolImageKey
						});
					}
				}
				if (0f < this.startTime && Time.realtimeSinceStartup - this.startTime > 1.05f)
				{
					this.bgImage.Visible = false;
					this.startTime = 0f;
				}
			}
			if (null != this.aniGameObject && !this.aniGameObject.isPlaying)
			{
				if (this.m_SolComposeMainType == SOLCOMPOSE_TYPE.EXTRACT)
				{
					if (!this.bPlayExtractResult)
					{
						this.PlayExtractResult();
						this.bPlayExtractResult = true;
					}
					else if (0f < this.startTime && Time.realtimeSinceStartup - this.startTime > 10.6f)
					{
						this.bUpdate = false;
						this.SetExtractResultData();
					}
				}
				else
				{
					this.Close();
				}
			}
		}
	}

	public override void ChangedResolution()
	{
		this.SetSize();
	}

	private void SetSize()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		this.bgImage.SetSize(GUICamera.width, GUICamera.height);
	}

	private void ClickSkipButton(IUIObject obj)
	{
		this.Close();
	}

	public void AddCloseCallback(OnCloseCallback callback)
	{
		this._closeCallback = (OnCloseCallback)Delegate.Combine(this._closeCallback, callback);
	}
}
