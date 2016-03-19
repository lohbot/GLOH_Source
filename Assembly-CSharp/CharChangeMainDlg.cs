using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CharChangeMainDlg : Form
{
	public const int CLASSNUM_MAX = 4;

	private DrawTexture m_dtMaingBG;

	private DrawTexture[] m_dtCharBG;

	private Button[] m_btSelect;

	private Button[] m_btDetail;

	private DrawTexture[] m_dtShadowBG;

	private Label m_lbMoney;

	private Button m_btChange;

	private DrawTexture m_dtSelect;

	private E_CHAR_TRIBE m_eCharTribe;

	private GameObject m_gbEffect;

	private long m_lNeedMoney;

	private static StringBuilder m_strBuilder = new StringBuilder();

	private float m_fCloseTime;

	private int m_iChangeCharKind;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Char/dlg_charchangemain", G_ID.CHARCHANGEMAIN_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtCharBG = new DrawTexture[4];
		this.m_btSelect = new Button[4];
		this.m_btDetail = new Button[4];
		this.m_dtShadowBG = new DrawTexture[4];
		string text = string.Empty;
		this.m_dtMaingBG = (base.GetControl("BT_DrawTexture_DrawTexture22") as DrawTexture);
		Texture2D texture2D;
		for (int i = 0; i < 4; i++)
		{
			text = string.Format("BT_DrawTexture_CharBG{0}", (i + 1).ToString());
			this.m_dtCharBG[i] = (base.GetControl(text) as DrawTexture);
			text = string.Format("BT_Select0{0}", (i + 1).ToString());
			this.m_btSelect[i] = (base.GetControl(text) as Button);
			this.m_btSelect[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSelect));
			text = string.Format("BT_Detail0{0}", (i + 1).ToString());
			this.m_btDetail[i] = (base.GetControl(text) as Button);
			this.m_btDetail[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDetail));
			text = string.Format("BT_DrawTexture_CharShadow{0}", (i + 1).ToString());
			this.m_dtShadowBG[i] = (base.GetControl(text) as DrawTexture);
			text = string.Format("UI/charselect/ChShadow" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
			texture2D = (ResourceCache.GetResource(text) as Texture2D);
			if (null == texture2D)
			{
				CharChangeMainDlg.RequestDownload(text, new PostProcPerItem(CharChangeMainDlg._OnImageProcess), this.m_dtShadowBG[i]);
			}
			else
			{
				CharChangeMainDlg.SetTexture(this.m_dtShadowBG[i], texture2D);
			}
		}
		this.m_dtSelect = (base.GetControl("DT_Select01") as DrawTexture);
		this.m_dtSelect.Hide(true);
		this.m_btSelect[0].TabIndex = 2;
		this.m_btSelect[1].TabIndex = 3;
		this.m_btSelect[2].TabIndex = 1;
		this.m_btSelect[3].TabIndex = 4;
		this.m_btDetail[0].TabIndex = 2;
		this.m_btDetail[1].TabIndex = 3;
		this.m_btDetail[2].TabIndex = 1;
		this.m_btDetail[3].TabIndex = 4;
		this.m_lbMoney = (base.GetControl("LB_Gold") as Label);
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			this.m_lNeedMoney = charSpend.lCharChangeGold;
		}
		this.m_lbMoney.SetText(ANNUALIZED.Convert(this.m_lNeedMoney));
		this.m_btChange = (base.GetControl("BT_Change") as Button);
		this.m_btChange.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickChange));
		text = string.Format("UI/charselect/ChaSelect_BG" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
		texture2D = (ResourceCache.GetResource(text) as Texture2D);
		if (null == texture2D)
		{
			CharChangeMainDlg.RequestDownload(text, new PostProcPerItem(CharChangeMainDlg._OnImageProcess), this.m_dtMaingBG);
		}
		else
		{
			CharChangeMainDlg.SetTexture(this.m_dtMaingBG, texture2D);
		}
		CharChangeMainDlg.SetCharImage(this.m_dtCharBG[0], 3);
		CharChangeMainDlg.SetCharImage(this.m_dtCharBG[1], 6);
		CharChangeMainDlg.SetCharImage(this.m_dtCharBG[2], 1);
		CharChangeMainDlg.SetCharImage(this.m_dtCharBG[3], 2);
		base.SetScreenCenter();
	}

	public void ClickSelect(IUIObject obj)
	{
		Button button = obj as Button;
		if (null == button)
		{
			return;
		}
		this.SetSelect(button);
	}

	public void ClickDetail(IUIObject obj)
	{
		Button button = obj as Button;
		if (null == button)
		{
			return;
		}
		this.m_eCharTribe = (E_CHAR_TRIBE)button.TabIndex;
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHARCHANGE_DLG))
		{
			CharChangeDlg charChangeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHARCHANGE_DLG) as CharChangeDlg;
			if (charChangeDlg != null)
			{
				charChangeDlg.SetSelectCharKind(this.m_eCharTribe);
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHARCHANGE_DLG);
		}
	}

	public void ClickChange(IUIObject obj)
	{
		this.SetClassChange(this.m_eCharTribe);
	}

	public static int GetChangeCharKindFormIndex(E_CHAR_TRIBE eCharTribe)
	{
		switch (eCharTribe)
		{
		case E_CHAR_TRIBE.HUMAN:
			return 1;
		case E_CHAR_TRIBE.FURRY:
			return 3;
		case E_CHAR_TRIBE.ELF:
			return 6;
		case E_CHAR_TRIBE.HUMANF:
			return 2;
		default:
			return -1;
		}
	}

	public bool SetClassChange(E_CHAR_TRIBE eCharTribe)
	{
		if (!this.IsChangeChar(eCharTribe))
		{
			return false;
		}
		CharChangeMainDlg.ShowMessageBoxClassChange(eCharTribe);
		return true;
	}

	public static bool Send_GS_CHANGE_CLASS_REQ(E_CHAR_TRIBE eCharTribe)
	{
		if (0 >= CharChangeMainDlg.GetChangeCharKindFormIndex(eCharTribe))
		{
			return false;
		}
		GS_CHANGE_CLASS_REQ gS_CHANGE_CLASS_REQ = new GS_CHANGE_CLASS_REQ();
		gS_CHANGE_CLASS_REQ.i32CharTribe = (int)eCharTribe;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHANGE_CLASS_REQ, gS_CHANGE_CLASS_REQ);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHARCHANGE_DLG);
		return true;
	}

	private void EffectLoad(WWWItem _item, object _param)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsForm(G_ID.CHARCHANGEMAIN_DLG))
		{
			return;
		}
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			}
		}
	}

	public static void SetCharImage(DrawTexture _img, int CHARKIND)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(CHARKIND);
		if (charKindInfo == null)
		{
			return;
		}
		CharChangeMainDlg.m_strBuilder.Remove(0, CharChangeMainDlg.m_strBuilder.Length);
		if (CHARKIND % 2 == 1)
		{
			CharChangeMainDlg.m_strBuilder.Append(string.Format("UI/CharSelect/{0}male{1}", charKindInfo.GetCHARKIND_INFO().CharTribe, NrTSingleton<UIDataManager>.Instance.AddFilePath));
		}
		else
		{
			CharChangeMainDlg.m_strBuilder.Append(string.Format("UI/CharSelect/{0}female{1}", charKindInfo.GetCHARKIND_INFO().CharTribe, NrTSingleton<UIDataManager>.Instance.AddFilePath));
		}
		Texture2D texture2D = ResourceCache.GetResource(CharChangeMainDlg.m_strBuilder.ToString()) as Texture2D;
		if (null == texture2D)
		{
			CharChangeMainDlg.RequestDownload(CharChangeMainDlg.m_strBuilder.ToString(), new PostProcPerItem(CharChangeMainDlg._OnImageProcess), _img);
		}
		else
		{
			CharChangeMainDlg.SetTexture(_img, texture2D);
		}
	}

	public static Texture2D RequestDownload(string strAssetPath, PostProcPerItem callbackDelegate, object obj)
	{
		Texture2D texture2D = null;
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(strAssetPath + Option.extAsset, null);
		if (wWWItem != null && (wWWItem.isCanceled || !wWWItem.canAccessAssetBundle))
		{
			wWWItem.SetItemType(ItemType.TEXTURE2D);
			wWWItem.SetCallback(callbackDelegate, obj);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		if (!(wWWItem.mainAsset == null))
		{
			texture2D = (wWWItem.mainAsset as Texture2D);
			DrawTexture drawTexture = obj as DrawTexture;
			if (drawTexture != null)
			{
				CharChangeMainDlg.SetTexture(drawTexture, texture2D);
			}
			else
			{
				GameObject gameObject = obj as GameObject;
				if (gameObject != null)
				{
					CharChangeMainDlg.SetCharFaceTexture(gameObject, texture2D);
				}
			}
		}
		return texture2D;
	}

	public static void _OnImageProcess(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			Texture2D texture2D = wItem.mainAsset as Texture2D;
			DrawTexture drawTexture = obj as DrawTexture;
			if (drawTexture != null)
			{
				CharChangeMainDlg.SetTexture(drawTexture, texture2D);
			}
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
	}

	public static void ShowMessageBoxEquipItem()
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(CharChangeMainDlg.MessageBoxEquipItem), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("951"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("91"), eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("980"));
		msgBoxUI.Show();
	}

	public static void MessageBoxEquipItem(object a_oObject)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
		if (leaderSoldierInfo == null)
		{
			return;
		}
		Protocol_Item.Send_EquipSol_InvenEquip_All(leaderSoldierInfo);
	}

	public bool IsChangeChar(E_CHAR_TRIBE eCharTribe)
	{
		if (this.m_lNeedMoney > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
		if (leaderSoldierInfo == null)
		{
			return false;
		}
		int changeCharKindFormIndex = CharChangeMainDlg.GetChangeCharKindFormIndex(eCharTribe);
		if (changeCharKindFormIndex == leaderSoldierInfo.GetCharKind())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("212"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return false;
		}
		if (leaderSoldierInfo.IsEquipItem())
		{
			CharChangeMainDlg.ShowMessageBoxEquipItem();
			return false;
		}
		return true;
	}

	public static void SetTexture(DrawTexture dt, Texture2D texture2D)
	{
		dt.SetTexture(texture2D);
	}

	public void SetSelect(Button bt)
	{
		this.m_eCharTribe = (E_CHAR_TRIBE)bt.TabIndex;
		this.m_dtSelect.Hide(false);
		DrawTexture drawTexture = null;
		switch (this.m_eCharTribe)
		{
		case E_CHAR_TRIBE.HUMAN:
			drawTexture = this.m_dtCharBG[2];
			break;
		case E_CHAR_TRIBE.FURRY:
			drawTexture = this.m_dtCharBG[0];
			break;
		case E_CHAR_TRIBE.ELF:
			drawTexture = this.m_dtCharBG[1];
			break;
		case E_CHAR_TRIBE.HUMANF:
			drawTexture = this.m_dtCharBG[3];
			break;
		}
		if (null != drawTexture)
		{
			this.m_dtSelect.SetLocation((int)drawTexture.GetLocationX(), (int)drawTexture.GetLocationY());
		}
	}

	public static void ShowMessageBoxClassChange(E_CHAR_TRIBE eCharTribe)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("87"),
			"targetname",
			CharChangeMainDlg.GetClassName(eCharTribe)
		});
		msgBoxUI.SetMsg(new YesDelegate(CharChangeMainDlg.MessageBoxClassChangeOK), eCharTribe, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("951"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.Show();
	}

	public static void MessageBoxClassChangeOK(object a_oObject)
	{
		CharChangeMainDlg.Send_GS_CHANGE_CLASS_REQ((E_CHAR_TRIBE)((int)a_oObject));
	}

	public static string GetClassName(E_CHAR_TRIBE eCharTribe)
	{
		string result = string.Empty;
		switch (eCharTribe)
		{
		case E_CHAR_TRIBE.HUMAN:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("955");
			break;
		case E_CHAR_TRIBE.FURRY:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("952");
			break;
		case E_CHAR_TRIBE.ELF:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("954");
			break;
		case E_CHAR_TRIBE.HUMANF:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("956");
			break;
		}
		return result;
	}

	public void ShowResultEffect(int iChangeCharKind)
	{
		this.m_iChangeCharKind = iChangeCharKind;
		CharChangeMainDlg.m_strBuilder.Remove(0, CharChangeMainDlg.m_strBuilder.Length);
		CharChangeMainDlg.m_strBuilder.Append(string.Format("UI/Etc/{0}{1}{2}", "fx_characterchange", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset));
		GameObject y = ResourceCache.GetResource(CharChangeMainDlg.m_strBuilder.ToString()) as GameObject;
		if (null == y)
		{
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(CharChangeMainDlg.m_strBuilder.ToString(), NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this._OnEffectProcess), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
	}

	public void _OnEffectProcess(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			GameObject gameObject = wItem.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_gbEffect.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_gbEffect, GUICamera.UILayer);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbEffect);
				}
				this.m_fCloseTime = Time.time + 5f;
				Transform child = NkUtil.GetChild(this.m_gbEffect.transform, "fx_character");
				if (null != child)
				{
					this.SetCharFace(child, this.m_iChangeCharKind);
				}
			}
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
	}

	public override void Update()
	{
		if (0f < this.m_fCloseTime && this.m_fCloseTime < Time.time)
		{
			UnityEngine.Object.Destroy(this.m_gbEffect);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHARCHANGEMAIN_DLG);
		}
	}

	public void SetCharFace(Transform CharFace, int CHARKIND)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(CHARKIND);
		if (charKindInfo == null)
		{
			return;
		}
		CharChangeMainDlg.m_strBuilder.Remove(0, CharChangeMainDlg.m_strBuilder.Length);
		if (CHARKIND % 2 == 1)
		{
			CharChangeMainDlg.m_strBuilder.Append(string.Format("UI/CharSelect/{0}male{1}", charKindInfo.GetCHARKIND_INFO().CharTribe, NrTSingleton<UIDataManager>.Instance.AddFilePath));
		}
		else
		{
			CharChangeMainDlg.m_strBuilder.Append(string.Format("UI/CharSelect/{0}female{1}", charKindInfo.GetCHARKIND_INFO().CharTribe, NrTSingleton<UIDataManager>.Instance.AddFilePath));
		}
		Texture2D texture2D = ResourceCache.GetResource(CharChangeMainDlg.m_strBuilder.ToString()) as Texture2D;
		if (null == texture2D)
		{
			CharChangeMainDlg.RequestDownload(CharChangeMainDlg.m_strBuilder.ToString(), new PostProcPerItem(CharChangeMainDlg._OnImageProcessFace), CharFace.gameObject);
		}
		else
		{
			CharChangeMainDlg.SetCharFaceTexture(CharFace.gameObject, texture2D);
		}
	}

	public static void _OnImageProcessFace(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			Texture2D texture2D = wItem.mainAsset as Texture2D;
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				CharChangeMainDlg.SetCharFaceTexture(gameObject, texture2D);
			}
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
	}

	public static void SetCharFaceTexture(GameObject go, Texture2D texture2D)
	{
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component != null)
		{
			Material material = component.material;
			if (null != material)
			{
				material.mainTexture = texture2D;
				if (null != go.renderer)
				{
					go.renderer.sharedMaterial = material;
				}
			}
		}
	}

	public void SetEnableControl(bool bEnable)
	{
		for (int i = 0; i < 4; i++)
		{
			this.m_btSelect[i].SetEnabled(bEnable);
			this.m_btDetail[i].SetEnabled(bEnable);
		}
		this.m_btChange.SetEnabled(bEnable);
	}
}
