using GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolRecruitSuccessDlg : Form
{
	public enum eBundleIndexType
	{
		BI_NOMAL,
		BI_LEGEND,
		BI_MYTHOLOGY,
		BI_PREMIUM,
		MAX_BI
	}

	private SolRecruitSuccessDlg.eBundleIndexType m_BundleIndex;

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

	private Label m_lReUseTicket;

	private Button m_bReUseTicket;

	private Button m_bReUseTicket2;

	private bool bRcvedRemainSolPost;

	private bool skipMode;

	private float delayTime = 3f;

	private bool bVipInfoTextShow;

	private bool bRequest;

	private int m_RecruitType;

	private string m_LegendType = string.Empty;

	private Queue<GameObject> destroyGameObjects = new Queue<GameObject>();

	private GameObject rootGameObject;

	private GameObject normalGameObject;

	private GameObject skipGameObject;

	private string backImageKey = string.Empty;

	private string rankImageKey = string.Empty;

	private string faceImageKey = string.Empty;

	private string seasonImageKey = string.Empty;

	private bool bUpdate;

	private bool bSetCard;

	private bool bSetGrade;

	private bool bSetFace;

	private bool bSetSeasonFont;

	private SOLDIER_INFO kSolinfo;

	private Queue<SOLDIER_INFO> arraySolinfo = new Queue<SOLDIER_INFO>();

	private float startTime;

	private float skipTime;

	private float PremiumTime;

	private bool bSetName;

	private bool bReUseTicketView;

	private GameObject gradeObject;

	private bool bGrade;

	private bool bShowSolMovie;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruitSuccess", G_ID.SOLRECRUITSUCCESS_DLG, false);
	}

	public override void SetComponent()
	{
		this.bgImage = (base.GetControl("DrawTexture_BG") as DrawTexture);
		string text = "background";
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(text);
		if (null != texture)
		{
			this.bgImage.SetSize(GUICamera.width, GUICamera.height);
			this.bgImage.SetTexture(texture);
		}
		else
		{
			string str = string.Format("UI/Soldier/{0}{1}", text, NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBackImage), text);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
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
		this.skipButton.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkipButton1));
		this.solMovietTrans = (base.GetControl("Button_MovieBtn") as Button);
		this.solMovietTrans.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolMovie));
		this.solMovietTrans.Visible = false;
		this.solMovie = (base.GetControl("BT_Movie") as Button);
		this.solMovie.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSolMovie));
		this.solMovie.Visible = false;
		this.solMovieText = (base.GetControl("LB_Movie") as Label);
		this.solMovieText.Visible = false;
		this.m_lReUseTicket = (base.GetControl("LB_Reuse") as Label);
		this.m_lReUseTicket.Visible = false;
		this.m_bReUseTicket = (base.GetControl("BT_Reuse") as Button);
		this.m_bReUseTicket.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickReUseTicketButton));
		this.m_bReUseTicket.Visible = false;
		this.m_bReUseTicket2 = (base.GetControl("BT_Reuse2") as Button);
		this.m_bReUseTicket2.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickReUseTicketButton));
		this.m_bReUseTicket2.Visible = false;
		this.SetData();
		base.DonotDepthChange(360f);
		UIDataManager.MuteSound(true);
		this.Hide();
	}

	public SolRecruitSuccessDlg.eBundleIndexType CheckBundleIndex(int RecruitType)
	{
		SolRecruitSuccessDlg.eBundleIndexType result = SolRecruitSuccessDlg.eBundleIndexType.BI_NOMAL;
		switch (RecruitType)
		{
		case 10:
		case 11:
		case 12:
			break;
		case 13:
			result = SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND;
			goto IL_38;
		default:
			if (RecruitType != 21)
			{
				goto IL_38;
			}
			break;
		}
		result = SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM;
		IL_38:
		if (this.isTicketType())
		{
			SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
			if (solRecruitDlg != null)
			{
				int nowOpenTicketCardType = solRecruitDlg.GetNowOpenTicketCardType();
				result = (SolRecruitSuccessDlg.eBundleIndexType)nowOpenTicketCardType;
			}
		}
		return result;
	}

	public void StartSoldierGetBundle(int RecruitType, int iSolCount)
	{
		this.m_RecruitType = RecruitType;
		this.bReUseTicketView = false;
		SolRecruitSuccessDlg.eBundleIndexType bundleIndex = this.CheckBundleIndex(RecruitType);
		this.m_BundleIndex = bundleIndex;
		switch (bundleIndex)
		{
		case SolRecruitSuccessDlg.eBundleIndexType.BI_NOMAL:
		{
			string str = string.Format("{0}", "UI/Soldier/fx_soldierget" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SolRecruitSuccess), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			break;
		}
		case SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND:
		case SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY:
		{
			string str = string.Format("{0}", "UI/Soldier/fx_direct_dragonhero" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SolRecruitEssenceSuccess), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
			break;
		}
		case SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM:
		{
			string str = string.Format("{0}", "UI/Soldier/fx_direct_premium_get" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem3 = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem3.SetItemType(ItemType.USER_ASSETB);
			wWWItem3.SetCallback(new PostProcPerItem(this.SolRecruitPremiumSuccess), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem3, DownGroup.RUNTIME, true);
			break;
		}
		}
	}

	public void SetVipTextShow()
	{
		this.bVipInfoTextShow = true;
	}

	private void ClickSolMovie(IUIObject obj)
	{
		if (this.bRequest)
		{
			return;
		}
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
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false, true);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false, true);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false, true);
		}
		this.bRequest = true;
	}

	private void ClickFaceBookButton(IUIObject obj)
	{
		Facebook_Feed_Dlg facebook_Feed_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.FACEBOOK_FEED_DLG) as Facebook_Feed_Dlg;
		if (facebook_Feed_Dlg != null)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.kSolinfo.CharKind);
			if (charKindInfo != null)
			{
				facebook_Feed_Dlg.SetType(eFACEBOOK_FEED_TYPE.GET_SOL, this.kSolinfo);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.FACEBOOK_FEED_DLG);
			}
		}
	}

	private void ClickReUseTicketButton(IUIObject obj)
	{
		SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
		if (solRecruitDlg != null)
		{
			this.Close();
		}
	}

	private void ClickSkipButton1(IUIObject obj)
	{
		if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY)
		{
			return;
		}
		if (this.skipMode && this.arraySolinfo.Count == 0)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this.skipTime > 1.7f)
		{
			return;
		}
		if (null != this.rootGameObject)
		{
			this.skipMode = true;
			if (null != this.normalGameObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.normalGameObject, false);
			}
			if (null != this.gradeObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.gradeObject, false);
			}
			if (null != this.skipGameObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.skipGameObject, true);
			}
		}
	}

	private void ClickSkipButton2(IUIObject obj)
	{
		if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY)
		{
			return;
		}
		if (this.skipMode && !this.bSetName)
		{
			return;
		}
		if (null != this.rootGameObject)
		{
			this.skipMode = true;
			if (null != this.normalGameObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.normalGameObject, false);
			}
			if (null != this.gradeObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.gradeObject, false);
			}
			if (null != this.skipGameObject)
			{
				NkUtil.SetAllChildActiveRecursive(this.skipGameObject, false);
			}
		}
		if (0 < this.arraySolinfo.Count)
		{
			if (this.skipMode && this.bSetName && !this.bUpdate)
			{
				this.bSetName = false;
				this.startTime = Time.time;
				this.nameBack.Visible = false;
				this.solName.Visible = false;
				SOLDIER_INFO sOLDIER_INFO = this.arraySolinfo.Dequeue();
				this.bRequest = false;
				this.bUpdate = true;
				this.SetImage(sOLDIER_INFO, null);
				if (null != this.skipGameObject)
				{
					if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM)
					{
						short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(sOLDIER_INFO.CharKind, (int)sOLDIER_INFO.Grade);
						if (legendType == 1)
						{
							this.m_LegendType = "skip_legend";
						}
						else if (legendType == 2)
						{
							this.m_LegendType = "skip_myth";
						}
						else
						{
							this.m_LegendType = "skip_nomal";
						}
						this.skipGameObject = NkUtil.GetChild(this.rootGameObject.transform, this.m_LegendType).gameObject;
						if (this.skipGameObject != null)
						{
							NkUtil.SetAllChildActiveRecursive(this.skipGameObject, true);
							this.skipGameObject.animation.Stop();
							this.skipGameObject.animation.cullingType = AnimationCullingType.AlwaysAnimate;
							this.skipGameObject.animation.Play(this.m_LegendType);
						}
					}
					else
					{
						NkUtil.SetAllChildActiveRecursive(this.skipGameObject, true);
						this.skipGameObject.animation.Stop();
						this.skipGameObject.animation.cullingType = AnimationCullingType.AlwaysAnimate;
						this.skipGameObject.animation.Play("soldier_skip");
					}
				}
			}
			else
			{
				this.bSetName = false;
				this.startTime = Time.time;
				this.nameBack.Visible = false;
				this.solName.Visible = false;
				SOLDIER_INFO solinfo = this.arraySolinfo.Dequeue();
				this.bRequest = false;
				this.bUpdate = true;
				this.SetImage(solinfo, null);
				if (null != this.normalGameObject)
				{
					this.normalGameObject.animation.Stop();
					this.normalGameObject.animation.Play("soldier");
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

	public override void Update()
	{
		if (this.bUpdate)
		{
			if (null != this.normalGameObject && !this.bSetGrade)
			{
				if (this.m_BundleIndex != SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM)
				{
					GameObject gameObject = NkUtil.GetChild(this.normalGameObject.transform, "rank").gameObject;
					if (null != gameObject)
					{
						Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.rankImageKey);
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
								this.bSetGrade = true;
							}
						}
					}
				}
				if (null != this.skipGameObject)
				{
					GameObject gameObject2 = NkUtil.GetChild(this.skipGameObject.transform, "rank").gameObject;
					if (null != gameObject2)
					{
						Texture2D texture2 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.rankImageKey);
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
								this.bSetGrade = true;
							}
						}
					}
				}
			}
			if (null != this.normalGameObject && !this.bSetFace)
			{
				if (this.m_BundleIndex != SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM)
				{
					GameObject gameObject3 = NkUtil.GetChild(this.normalGameObject.transform, "face").gameObject;
					if (null != gameObject3)
					{
						Texture2D texture3 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
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
								this.bSetFace = true;
							}
						}
					}
				}
				if (null != this.skipGameObject)
				{
					GameObject gameObject4 = NkUtil.GetChild(this.skipGameObject.transform, "face").gameObject;
					if (null != gameObject4)
					{
						Texture2D texture4 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
						if (null != texture4)
						{
							Material material4 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material4)
							{
								material4.mainTexture = texture4;
								if (null != gameObject4.renderer)
								{
									gameObject4.renderer.sharedMaterial = material4;
								}
								this.bSetFace = true;
							}
						}
					}
				}
			}
			if (null != this.normalGameObject && !this.bSetSeasonFont)
			{
				if (this.m_BundleIndex != SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM)
				{
					GameObject gameObject5 = NkUtil.GetChild(this.normalGameObject.transform, "fx_font_number").gameObject;
					if (null != gameObject5)
					{
						Texture2D texture5 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.seasonImageKey);
						if (null != texture5)
						{
							Material material5 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material5)
							{
								material5.mainTexture = texture5;
								if (null != gameObject5.renderer)
								{
									gameObject5.renderer.sharedMaterial = material5;
								}
								this.bSetSeasonFont = true;
							}
						}
					}
				}
				if (null != this.skipGameObject)
				{
					GameObject gameObject6 = NkUtil.GetChild(this.skipGameObject.transform, "fx_font_number").gameObject;
					if (null != gameObject6)
					{
						Texture2D texture6 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.seasonImageKey);
						if (null != texture6)
						{
							Material material6 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
							if (null != material6)
							{
								material6.mainTexture = texture6;
								if (null != gameObject6.renderer)
								{
									gameObject6.renderer.sharedMaterial = material6;
								}
								this.bSetSeasonFont = true;
							}
						}
					}
				}
			}
			if ((this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY) && null != this.normalGameObject && !this.bSetCard)
			{
				GameObject gameObject7 = NkUtil.GetChild(this.normalGameObject.transform, "back").gameObject;
				if (null != gameObject7)
				{
					Texture2D texture7 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.backImageKey);
					if (null != texture7)
					{
						Material material7 = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
						if (null != material7)
						{
							material7.mainTexture = texture7;
							if (null != gameObject7.renderer)
							{
								gameObject7.renderer.sharedMaterial = material7;
							}
							this.bSetCard = true;
						}
					}
				}
			}
			if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY)
			{
				if (this.bSetGrade && this.bSetFace && this.bSetSeasonFont && this.bSetCard)
				{
					this.startTime = Time.realtimeSinceStartup;
					this.bUpdate = false;
				}
			}
			else if (this.bSetGrade && this.bSetFace && this.bSetSeasonFont)
			{
				this.startTime = Time.realtimeSinceStartup;
				this.bUpdate = false;
			}
		}
		else
		{
			bool flag = false;
			if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY)
			{
				if (!this.bSetName && this.skipTime > 0f && Time.realtimeSinceStartup > this.skipTime - 2f)
				{
					flag = true;
				}
			}
			else if ((!this.bSetName && Time.realtimeSinceStartup - this.startTime > this.delayTime) || (!this.bSetName && this.skipMode))
			{
				flag = true;
			}
			if (flag)
			{
				if (this.arraySolinfo.Count == 0)
				{
					this.skipButton.Visible = false;
					this.closeUIButton.Visible = true;
				}
				switch (this.m_BundleIndex)
				{
				case SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND:
				case SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY:
				case SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM:
					this.nameBack.SetLocation(this.nameBack.GetLocationX(), this.nameBack.GetLocationY(), -61f);
					this.solName.SetLocation(this.solName.GetLocationX(), this.solName.GetLocationY(), -62f);
					this.solMovie.SetLocation(this.solMovie.GetLocationX(), this.solMovie.GetLocationY(), -62f);
					this.solMovietTrans.SetLocation(this.solMovietTrans.GetLocationX(), this.solMovietTrans.GetLocationY(), -62f);
					this.solMovieText.SetLocation(this.solMovieText.GetLocationX(), this.solMovieText.GetLocationY(), -62f);
					break;
				}
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.kSolinfo.CharKind);
				if (charKindInfo != null)
				{
					this.nameBack.Visible = true;
					this.solName.Visible = true;
					string empty = string.Empty;
					if (1 < this.kSolinfo.Level)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2534"),
							"solname",
							charKindInfo.GetName(),
							"sollv",
							this.kSolinfo.Level
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2535"),
							"solname",
							charKindInfo.GetName()
						});
					}
					this.solName.SetFlashLabel(empty);
				}
				this.closeBack.Visible = true;
				this.closeText.Visible = true;
				this.closeText.SetFlashLabel(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1897"));
				this.bSetName = true;
				this.skipButton.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkipButton1));
				this.skipButton.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkipButton2));
				if (this.isTicketType())
				{
					SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
					if (solRecruitDlg != null)
					{
						if (solRecruitDlg.GetReUseTicket() && this.bReUseTicketView)
						{
							this.m_lReUseTicket.Visible = true;
							this.m_bReUseTicket.Visible = true;
							this.m_bReUseTicket2.Visible = true;
						}
						else
						{
							this.m_lReUseTicket.Visible = false;
							this.m_bReUseTicket.Visible = false;
							this.m_bReUseTicket2.Visible = false;
						}
					}
				}
				if (this.bShowSolMovie)
				{
					this.solMovie.Visible = true;
					this.solMovietTrans.Visible = true;
					this.solMovieText.Visible = true;
				}
			}
			else if (((this.bSetName && this.PremiumTime > 0f && Time.realtimeSinceStartup > this.PremiumTime) || (this.PremiumTime > 0f && this.bSetName && this.skipMode)) && this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_PREMIUM)
			{
				NkUtil.SetAllChildActiveRecursive(this.skipGameObject, true);
				this.PremiumTime = 0f;
			}
		}
	}

	private bool isTicketType()
	{
		switch (this.m_RecruitType)
		{
		case 2:
		case 3:
		case 4:
		case 5:
		case 7:
		case 8:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
			return true;
		}
		return false;
	}

	private void SolRecruitEssenceSuccess(WWWItem _item, object _param)
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
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.rootGameObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
				this.destroyGameObjects.Enqueue(this.rootGameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.rootGameObject);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = base.InteractivePanel.transform.position.z - 10f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.bUpdate = true;
				this.normalGameObject = NkUtil.GetChild(this.rootGameObject.transform, "fx_direct_dragonhero").gameObject;
				if (null != this.normalGameObject)
				{
					this.normalGameObject.SetActive(true);
				}
				AnimationClip clip = this.normalGameObject.animation.GetClip("fx_direct_dragonhero");
				this.skipTime = Time.realtimeSinceStartup + (clip.length + 1f);
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
				this.Show();
			}
		}
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
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.rootGameObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
				this.destroyGameObjects.Enqueue(this.rootGameObject);
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
				this.bUpdate = true;
				if (!this.bGrade)
				{
					this.gradeObject = NkUtil.GetChild(this.rootGameObject.transform, "grade").gameObject;
					if (null != this.gradeObject)
					{
						this.gradeObject.SetActive(false);
					}
				}
				this.normalGameObject = NkUtil.GetChild(this.rootGameObject.transform, "soldier").gameObject;
				if (null != this.normalGameObject)
				{
					this.normalGameObject.SetActive(true);
				}
				this.skipGameObject = NkUtil.GetChild(this.rootGameObject.transform, "skip").gameObject;
				if (null != this.skipGameObject)
				{
					this.skipGameObject.SetActive(false);
				}
				this.skipTime = Time.realtimeSinceStartup;
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
				this.Show();
			}
		}
	}

	private void SolRecruitPremiumSuccess(WWWItem _item, object _param)
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
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.rootGameObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
				this.destroyGameObjects.Enqueue(this.rootGameObject);
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
				this.bUpdate = true;
				if (this.kSolinfo != null)
				{
					short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.kSolinfo.CharKind, (int)this.kSolinfo.Grade);
					if (legendType == 1)
					{
						this.m_LegendType = "skip_legend";
					}
					else if (legendType == 2)
					{
						this.m_LegendType = "skip_myth";
					}
					else
					{
						this.m_LegendType = "skip_nomal";
					}
					this.normalGameObject = NkUtil.GetChild(this.rootGameObject.transform, "fx_direct_premium").gameObject;
					if (null != this.normalGameObject)
					{
						NkUtil.SetAllChildActiveRecursive(this.normalGameObject, true);
					}
					this.skipGameObject = NkUtil.GetChild(this.rootGameObject.transform, this.m_LegendType).gameObject;
					if (null != this.normalGameObject)
					{
						NkUtil.SetAllChildActiveRecursive(this.skipGameObject, false);
					}
					this.skipTime = Time.realtimeSinceStartup;
					this.PremiumTime = Time.realtimeSinceStartup + 2.2f;
				}
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

	private void ClickCloseButton(IUIObject obj)
	{
		ItemMallDlg_ChallengeQuest itemMallDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg_ChallengeQuest;
		if (itemMallDlg_ChallengeQuest != null)
		{
			itemMallDlg_ChallengeQuest.SuccessDirectionEnd();
		}
		if (this.bSetName)
		{
			if ((this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY) && (this.skipTime == 0f || Time.realtimeSinceStartup < this.skipTime))
			{
				return;
			}
			while (this.destroyGameObjects.Count > 0)
			{
				GameObject gameObject = this.destroyGameObjects.Dequeue();
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
		}
	}

	public override void OnClose()
	{
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
		SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
		if (solRecruitDlg != null)
		{
			solRecruitDlg.SetTicketList();
			solRecruitDlg.SetShowReUseTicket(true);
		}
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			itemMallDlg.SetShowData();
		}
		UIDataManager.MuteSound(false);
		while (this.destroyGameObjects.Count > 0)
		{
			GameObject gameObject = this.destroyGameObjects.Dequeue();
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		Resources.UnloadUnusedAssets();
		if (this.bRcvedRemainSolPost)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("685"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		NrTSingleton<EventConditionHandler>.Instance.CloseUI.OnTrigger();
		if (this.bVipInfoTextShow && NrTSingleton<FormsManager>.Instance.IsForm(G_ID.ITEMMALL_DLG))
		{
			ItemMallDlg itemMallDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
			if (itemMallDlg2 != null)
			{
				long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
				byte levelExp = NrTSingleton<NrTableVipManager>.Instance.GetLevelExp((long)((int)charSubData));
				itemMallDlg2.SetVipInfoShow(levelExp, false);
			}
		}
		base.OnClose();
	}

	public void SetNotifyByRemainSolPost(bool bRcvRemainSolPost)
	{
		this.bRcvedRemainSolPost = bRcvRemainSolPost;
	}

	public void SetImage(SOLDIER_INFO[] arrsolinfo)
	{
		this.SetImage(arrsolinfo[0], null);
		this.arraySolinfo.Clear();
		for (int i = 1; i < arrsolinfo.Length; i++)
		{
			this.arraySolinfo.Enqueue(arrsolinfo[i]);
		}
	}

	public void SetImage(SOLDIER_INFO solinfo, NkSoldierInfo paramSolInfo = null)
	{
		this.kSolinfo = null;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(solinfo.CharKind);
		if (charKindInfo == null)
		{
			return;
		}
		this.bShowSolMovie = false;
		this.solMovie.Visible = false;
		this.solMovietTrans.Visible = false;
		this.solMovieText.Visible = false;
		this.kSolinfo = solinfo;
		this.bSetCard = false;
		this.bSetFace = false;
		this.bSetGrade = false;
		this.bSetSeasonFont = false;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(solinfo.CharKind, (int)solinfo.Grade);
		if (this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_LEGEND || this.m_BundleIndex == SolRecruitSuccessDlg.eBundleIndexType.BI_MYTHOLOGY)
		{
			if (legendType == 1)
			{
				this.backImageKey = "card_legend";
			}
			else if (legendType == 2)
			{
				this.backImageKey = "card_myth";
			}
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.backImageKey))
			{
				string str = string.Format("{0}", "UI/Soldier/" + this.backImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), this.backImageKey);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
		}
		if (legendType == 1)
		{
			this.rankImageKey = "rankl" + ((int)(solinfo.Grade + 1)).ToString();
		}
		else if (legendType == 2)
		{
			this.rankImageKey = "rankm" + ((int)(solinfo.Grade + 1)).ToString();
		}
		else
		{
			this.rankImageKey = "rank" + ((int)(solinfo.Grade + 1)).ToString();
		}
		if (4 <= solinfo.Grade)
		{
			this.bGrade = true;
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.rankImageKey))
		{
			string str2 = string.Format("{0}", "UI/Soldier/" + this.rankImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SetBundleImage), this.rankImageKey);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
		}
		if (UIDataManager.IsUse256Texture())
		{
			this.faceImageKey = charKindInfo.GetPortraitFile1((int)solinfo.Grade, string.Empty) + "_256";
		}
		else
		{
			this.faceImageKey = charKindInfo.GetPortraitFile1((int)solinfo.Grade, string.Empty) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.faceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		NkSoldierInfo nkSoldierInfo = null;
		bool flag = false;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
				if (soldierInfo != null)
				{
					if (this.kSolinfo.SolID == soldierInfo.GetSolID())
					{
						nkSoldierInfo = soldierInfo;
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return;
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (this.kSolinfo.SolID == current.GetSolID())
				{
					nkSoldierInfo = current;
					break;
				}
			}
		}
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = paramSolInfo;
		}
		if (nkSoldierInfo != null)
		{
			this.seasonImageKey = "font_number" + (nkSoldierInfo.GetSeason() + 1).ToString();
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.seasonImageKey))
			{
				string str3 = string.Format("{0}", "UI/Soldier/" + this.seasonImageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem3 = Holder.TryGetOrCreateBundle(str3 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem3.SetItemType(ItemType.USER_ASSETB);
				wWWItem3.SetCallback(new PostProcPerItem(this.SetBundleImage), this.seasonImageKey);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem3, DownGroup.RUNTIME, true);
			}
		}
	}

	public override void ChangedResolution()
	{
		this.SetData();
	}

	private void SetData()
	{
		this.bgImage.SetSize(GUICamera.width, GUICamera.height);
		this.closeUIButton.SetSize(GUICamera.width, GUICamera.height);
		this.closeUIButton.SetLocation(0, 0);
		this.skipButton.SetSize(GUICamera.width, GUICamera.height);
		this.skipButton.SetLocation(0, 0);
		this.closeBack.SetSize(GUICamera.width, this.closeBack.height);
		this.closeBack.SetLocation(0f, GUICamera.height - this.closeBack.height);
		this.closeText.width = GUICamera.width;
		this.closeText.SetLocation(0f, this.closeBack.GetLocationY() + 6f);
		float num = GUICamera.width / 2f;
		if (this.solName.width >= num)
		{
			this.solName.SetLocation(0f, GUICamera.height / 2f + this.solName.height * 0.66f);
			this.nameBack.SetLocation(0f, GUICamera.height / 2f + this.nameBack.height / 2f + 12f);
		}
		else if (this.solName.width < num)
		{
			this.solName.SetLocation(num - this.solName.width + 50f, GUICamera.height / 2f + this.solName.height * 0.66f);
			this.nameBack.SetLocation(num - this.nameBack.width + 50f, GUICamera.height / 2f + this.nameBack.height / 2f + 12f);
		}
		this.solMovie.SetLocation(this.solMovie.GetLocationX(), this.closeBack.GetLocationY() - this.solMovie.GetSize().y - 10f);
		this.solMovieText.SetLocation(this.solMovieText.GetLocationX(), this.closeBack.GetLocationY() - this.solMovie.GetSize().y - 5f);
		this.solMovietTrans.SetLocation(this.solMovietTrans.GetLocationX(), this.closeBack.GetLocationY() - this.solMovietTrans.GetSize().y - 10f);
		this.m_lReUseTicket.SetLocation(this.m_lReUseTicket.GetLocationX() + 63f, this.closeBack.GetLocationY() - this.m_lReUseTicket.GetSize().y - 72f);
		this.m_bReUseTicket.SetLocation(this.m_bReUseTicket.GetLocationX() + 63f, this.closeBack.GetLocationY() - this.m_bReUseTicket.GetSize().y - 70f);
		this.m_bReUseTicket2.SetLocation(this.m_bReUseTicket2.GetLocationX() + 63f, this.closeBack.GetLocationY() - this.m_bReUseTicket2.GetSize().y - 70f);
	}
}
