using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ReincarnationSuccessDlg : Form
{
	private enum eBUNDLEDOWNSTATE
	{
		eBUNDLEDOWNSTATE_NONE,
		eBUNDLEDOWNSTATE_DOWNING,
		eBUNDLEDOWNSTATE_DOWNCOMPLTE,
		eBUNDLEDOWNSTATE_OK
	}

	private Button m_btEffectSkip;

	private GameObject m_goEffect;

	private int m_iBaseCharKind;

	private int m_iNewCharKind;

	private string m_strBaseFaceImageKey = string.Empty;

	private string m_strNewFaceImageKey = string.Empty;

	private ReincarnationSuccessDlg.eBUNDLEDOWNSTATE m_eBundleDownState;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DLG_Direction", G_ID.REINCARNATION_SUCCESS_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btEffectSkip = (base.GetControl("Button_Button0") as Button);
		this.m_btEffectSkip.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickEffectSkip));
	}

	private void OnClickEffectSkip(IUIObject obj)
	{
		this.SkipEffect();
		base.CloseNow();
	}

	public void SkipEffect()
	{
		if (null != this.m_goEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.m_goEffect);
		}
	}

	public override void OnClose()
	{
		this.SkipEffect();
		base.OnClose();
	}

	public void SetResult(int iBaseCharKind, GS_SOLDIER_REINCARNATION_SET_ACK ACK)
	{
		this.m_iBaseCharKind = iBaseCharKind;
		this.m_iNewCharKind = ACK.i32CharKind;
		string str = string.Format("{0}", "UI/Soldier/fx_direct_rebirth" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.LoadEffect), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void LoadEffect(WWWItem _item, object _param)
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
			if (null != this.m_goEffect)
			{
				UnityEngine.Object.DestroyImmediate(this.m_goEffect);
			}
			this.m_goEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
			if (null == this.m_goEffect)
			{
				return;
			}
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
			effectUIPos.z = 300f;
			this.m_goEffect.transform.position = effectUIPos;
			NkUtil.SetAllChildLayer(this.m_goEffect, GUICamera.UILayer);
			this.m_goEffect.SetActive(false);
			this.SetTextureBundle();
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goEffect);
			}
		}
	}

	public override void Update()
	{
		switch (this.m_eBundleDownState)
		{
		case ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_DOWNING:
			if (null != NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strBaseFaceImageKey) && null != NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strNewFaceImageKey))
			{
				this.SetFaceTexture("fx_face_base", this.m_strBaseFaceImageKey);
				this.SetFaceTexture("fx_face_new", this.m_strNewFaceImageKey);
				this.m_eBundleDownState = ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_DOWNCOMPLTE;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goEffect);
				}
				this.m_goEffect.SetActive(true);
			}
			break;
		case ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_DOWNCOMPLTE:
		{
			Animation componentInChildren = this.m_goEffect.GetComponentInChildren<Animation>();
			if (componentInChildren != null && !componentInChildren.isPlaying)
			{
				this.m_eBundleDownState = ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_OK;
			}
			break;
		}
		case ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_OK:
			this.SkipEffect();
			base.CloseNow();
			break;
		}
	}

	private void SetFaceTexture(string strDummyName, string strFaceImageKey)
	{
		GameObject gameObject = NkUtil.GetChild(this.m_goEffect.transform, strDummyName).gameObject;
		if (null != gameObject)
		{
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(strFaceImageKey);
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
	}

	private void SetTextureBundle()
	{
		this.SetTextureBundleSub(this.m_iBaseCharKind, out this.m_strBaseFaceImageKey);
		this.SetTextureBundleSub(this.m_iNewCharKind, out this.m_strNewFaceImageKey);
		this.m_eBundleDownState = ReincarnationSuccessDlg.eBUNDLEDOWNSTATE.eBUNDLEDOWNSTATE_DOWNING;
	}

	private void SetTextureBundleSub(int iCharKind, out string strFaceImageKey)
	{
		strFaceImageKey = string.Empty;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(iCharKind);
		if (charKindInfo != null)
		{
			if (UIDataManager.IsUse256Texture())
			{
				strFaceImageKey = charKindInfo.GetPortraitFile1(-1, string.Empty) + "_256";
			}
			else
			{
				strFaceImageKey = charKindInfo.GetPortraitFile1(-1, string.Empty) + "_512";
			}
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(strFaceImageKey))
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(strFaceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
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

	private void SetElementSuccessDlgGUI()
	{
		if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_iNewCharKind) == null)
		{
			return;
		}
	}
}
