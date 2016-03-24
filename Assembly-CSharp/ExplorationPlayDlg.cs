using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExplorationPlayDlg : Form
{
	private DrawTexture m_BG;

	private long m_nMaxActivity;

	private Button m_Continue;

	private Button m_Close;

	private Button m_Skip;

	private Label m_ResultText;

	private Label m_lbActivityTime;

	private Label m_lb_WillNum;

	private long m_nCurrentActivity;

	private long m_nBeforeActivity;

	private float m_fActivityUpdateTime;

	private string m_szPath = "PlayingImg_";

	private bool m_bContinue = true;

	private GameObject walkGameObject;

	private GameObject walkAniGameObject;

	private GameObject rootGameObject;

	private GameObject aniGameObject;

	private GameObject childGameObject;

	private bool m_bSkip;

	private bool request;

	private float resultTextTime;

	private bool imageSet;

	private bool sendPacket;

	private string resultText = string.Empty;

	private string textureKey = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Exploration/Exploration_Play", G_ID.EXPLORATION_PLAY_DLG, false, true);
		base.ShowBlackBG(1f);
		if (null != this.closeButton)
		{
			base.SetDeleagteCloseButton(new EZValueChangedDelegate(this.ClickClose));
		}
	}

	public override void SetComponent()
	{
		this.m_BG = (base.GetControl("Main_BG") as DrawTexture);
		this.m_BG.SetTextureFromBundle("UI/Exploration/MainBG");
		this.m_Continue = (base.GetControl("Continue_Btn") as Button);
		this.m_Continue.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickContinue));
		this.m_Continue.EffectAni = false;
		this.m_Close = (base.GetControl("End_Btn") as Button);
		this.m_Close.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.m_Skip = (base.GetControl("Skip_Btn") as Button);
		this.m_Skip.SetSize(GUICamera.width, GUICamera.height);
		this.m_Skip.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkip));
		this.m_Skip.Visible = false;
		this.m_BG = (base.GetControl("DrawTexture_will1") as DrawTexture);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		this.m_lbActivityTime = (base.GetControl("Will_Time_Label") as Label);
		this.m_ResultText = (base.GetControl("Result_Label") as Label);
		this.m_ResultText.Visible = false;
		base.ShowLayer(1);
		base.SetScreenCenter();
		string str = string.Format("{0}", "UI/Exploration/fx_direct_exploere" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.ExplorationPlay1), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		string str2 = string.Format("{0}", "UI/Exploration/fx_direct_treasurebox" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem2.SetItemType(ItemType.USER_ASSETB);
		wWWItem2.SetCallback(new PostProcPerItem(this.ExplorationPlay2), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
		UIDataManager.MuteSound(true);
	}

	private void ClickSkip(IUIObject obj)
	{
		if (this.m_bSkip || this.sendPacket)
		{
			return;
		}
		this.m_bSkip = true;
		if (null != this.walkAniGameObject && this.walkGameObject.activeInHierarchy)
		{
			this.walkGameObject.SetActive(false);
			this.rootGameObject.SetActive(true);
			if (null != this.childGameObject)
			{
				this.childGameObject.SetActive(false);
			}
			this.aniGameObject.animation.Play();
			this.resultTextTime = Time.realtimeSinceStartup;
			GS_EXPLORATION_REQ gS_EXPLORATION_REQ = new GS_EXPLORATION_REQ();
			int num = 0;
			foreach (NkSoldierInfo current in NrTSingleton<ExplorationManager>.Instance.GetSolInfo())
			{
				if (num >= 6)
				{
					break;
				}
				gS_EXPLORATION_REQ.m_nSolID[num] = current.GetSolID();
				num++;
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPLORATION_REQ, gS_EXPLORATION_REQ);
			this.sendPacket = true;
		}
	}

	private void ExplorationPlay1(WWWItem _item, object _param)
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
				this.walkGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.walkGameObject);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.walkGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.walkGameObject, GUICamera.UILayer);
				this.walkAniGameObject = NkUtil.GetChild(this.walkGameObject.transform, "fx_exploere").gameObject;
				this.m_Skip.Visible = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.walkGameObject);
				}
			}
		}
	}

	private void ExplorationPlay2(WWWItem _item, object _param)
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
				this.rootGameObject.SetActive(false);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.rootGameObject);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 296f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.aniGameObject = NkUtil.GetChild(this.rootGameObject.transform, "fx_direct_treasurebox").gameObject;
				this.aniGameObject.animation.Stop();
				this.childGameObject = NkUtil.GetChild(this.rootGameObject.transform, "fx_treasure").gameObject;
				if (null != this.childGameObject)
				{
					this.childGameObject.SetActive(false);
				}
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
			}
		}
	}

	public void ClickContinue(IUIObject obj)
	{
		if (this.sendPacket)
		{
			return;
		}
		this.request = false;
		this.m_ResultText.Text = " ";
		this.m_ResultText.Visible = false;
		this.imageSet = false;
		if (null != this.rootGameObject)
		{
			this.rootGameObject.SetActive(false);
		}
		if (this.m_nCurrentActivity != 0L)
		{
			if (null != this.walkGameObject)
			{
				this.walkGameObject.SetActive(true);
			}
			if (null != this.walkAniGameObject)
			{
				this.walkAniGameObject.animation.Play();
			}
			if (this.m_bContinue)
			{
				base.ShowLayer(1);
				this.m_bContinue = false;
				this.m_Continue.controlIsEnabled = false;
			}
			this.m_Skip.Visible = true;
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (this.m_nCurrentActivity >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
	}

	public override void OnClose()
	{
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.RefreshSolList();
		}
		UIDataManager.MuteSound(false);
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.Destroy(this.rootGameObject);
		}
		if (null != this.walkGameObject)
		{
			UnityEngine.Object.Destroy(this.walkGameObject);
		}
		Resources.UnloadUnusedAssets();
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.EXPLORATION_REWARD_DLG);
	}

	public void ClickClose(IUIObject obj)
	{
		this.Close();
	}

	public override void Update()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (this.m_nCurrentActivity != kMyCharInfo.m_nActivityPoint || this.m_nMaxActivity != kMyCharInfo.m_nMaxActivityPoint)
			{
				this.m_nBeforeActivity = this.m_nCurrentActivity;
				this.m_nCurrentActivity = kMyCharInfo.m_nActivityPoint;
				this.m_nMaxActivity = kMyCharInfo.m_nMaxActivityPoint;
			}
			if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
			{
				MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
				if (myCharInfoDlg == null)
				{
					return;
				}
				this.m_lbActivityTime.SetText(myCharInfoDlg.StrActivityTime);
				this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 1f;
				this.SetActivityPointUI();
			}
		}
		if (!this.m_bSkip && !this.sendPacket && null != this.walkAniGameObject && !this.walkAniGameObject.animation.isPlaying && this.walkGameObject.activeInHierarchy)
		{
			this.walkGameObject.SetActive(false);
			this.rootGameObject.SetActive(true);
			if (null != this.childGameObject)
			{
				this.childGameObject.SetActive(false);
			}
			this.aniGameObject.animation.Play();
			this.resultTextTime = Time.realtimeSinceStartup;
			GS_EXPLORATION_REQ gS_EXPLORATION_REQ = new GS_EXPLORATION_REQ();
			int num = 0;
			foreach (NkSoldierInfo current in NrTSingleton<ExplorationManager>.Instance.GetSolInfo())
			{
				if (num >= 6)
				{
					break;
				}
				gS_EXPLORATION_REQ.m_nSolID[num] = current.GetSolID();
				num++;
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPLORATION_REQ, gS_EXPLORATION_REQ);
			this.sendPacket = true;
		}
		if (null != this.childGameObject && !this.imageSet && this.request)
		{
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.textureKey);
			if (null != texture)
			{
				Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
				if (null != material)
				{
					material.mainTexture = texture;
					if (null != this.childGameObject.renderer)
					{
						this.childGameObject.renderer.sharedMaterial = material;
						this.childGameObject.SetActive(true);
					}
					this.imageSet = true;
				}
			}
		}
		if (0f < this.resultTextTime && Time.realtimeSinceStartup - this.resultTextTime >= 1.5f)
		{
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EXPLOERE", "ACQUIRE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			this.m_ResultText.Visible = true;
			this.m_ResultText.Text = this.resultText;
			this.resultTextTime = 0f;
		}
		if (null != this.aniGameObject && null != this.aniGameObject.animation && !this.aniGameObject.animation.isPlaying)
		{
			this.m_Continue.controlIsEnabled = true;
			this.sendPacket = false;
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

	public void PlayEnd(int tableIndex, int index, long money, ITEM item)
	{
		this.m_bSkip = false;
		this.m_Skip.Visible = false;
		this.resultTextTime = Time.realtimeSinceStartup;
		this.request = true;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
		{
			MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
			if (myCharInfoDlg == null)
			{
				return;
			}
			this.m_lbActivityTime.SetText(myCharInfoDlg.StrActivityTime);
			this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 1f;
			this.SetActivityPointUI();
		}
		this.textureKey = string.Empty;
		ExplorationTable explorationTable = NrTSingleton<ExplorationManager>.Instance.GetExplorationTable(kMyCharInfo.GetLevel(), tableIndex);
		if (explorationTable != null)
		{
			this.textureKey = this.m_szPath + explorationTable.m_szTexture[index];
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.textureKey))
			{
				string str = string.Format("{0}", "UI/Exploration/" + this.textureKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), this.textureKey);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
			if (0L < money)
			{
				this.resultText = string.Empty;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1928");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.resultText, new object[]
				{
					textFromInterface,
					"count",
					money
				});
			}
			else if (0 < item.m_nItemUnique)
			{
				this.resultText = string.Empty;
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1929");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.resultText, new object[]
				{
					textFromInterface2,
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique),
					"count",
					item.m_nItemNum
				});
			}
		}
		base.SetShowLayer(1, false);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			if (0L < kMyCharInfo.m_nActivityPoint)
			{
				base.SetShowLayer(2, true);
				this.m_bContinue = true;
				base.SetShowLayer(3, true);
			}
			else
			{
				base.SetShowLayer(2, true);
				base.SetShowLayer(3, true);
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
}
