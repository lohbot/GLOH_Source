using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ChallengeBundleDlg : Form
{
	private Label m_Title1;

	private Label m_Title2;

	private float m_fStartTime;

	private GameObject m_RootObject;

	private GameObject m_AniObject;

	private bool m_bClose;

	private bool m_bEnd;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Item/DLG_Equipment_item", G_ID.CHALLENGE_BUNDLE_DLG, false, true);
		base.SetSize(GUICamera.width, GUICamera.height);
		base.ShowBlackBG(0.5f);
		base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public override void SetComponent()
	{
		this.m_Title1 = (base.GetControl("Label_Text01") as Label);
		this.m_Title2 = (base.GetControl("Label_Text02") as Label);
		this.m_Title1.Visible = false;
		this.m_Title2.Visible = false;
		UIDataManager.MuteSound(true);
		base.SetScreenCenter();
	}

	public void SetData(ChallengeManager.eCHALLENGECODE type)
	{
		string str = string.Format("{0}", "UI/Item/fx_direct_weapon" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.EquipItem), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		string empty = string.Empty;
		string empty2 = string.Empty;
		NrTSingleton<ChallengeManager>.Instance.GetEquipChallengeStrKey(type, out empty, out empty2);
		this.m_Title1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(empty);
		this.m_Title2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(empty2);
	}

	private void EquipItem(WWWItem _item, object _param)
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
				this.m_RootObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				this.m_RootObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
				if (this == null)
				{
					UnityEngine.Object.Destroy(this.m_RootObject);
					return;
				}
				this.m_AniObject = NkUtil.GetChild(this.m_RootObject.transform, "fx_direct_weapon").gameObject;
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 100f;
				this.m_RootObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_RootObject, GUICamera.UILayer);
				this.m_fStartTime = Time.realtimeSinceStartup;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_RootObject);
				}
			}
		}
	}

	public override void Update()
	{
		if (!this.m_bClose && 0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime > 3.8f)
		{
			this.m_Title1.Visible = true;
			this.m_Title2.Visible = true;
			this.m_bClose = true;
		}
		if (!this.m_bEnd && 0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime >= 6.8f)
		{
			base.AlphaAni(1f, 0f, 1f);
			this.m_bEnd = true;
		}
		if (this.m_bClose && null != this.m_AniObject && !this.m_AniObject.animation.isPlaying)
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
		UIDataManager.MuteSound(false);
		if (null != this.m_RootObject)
		{
			UnityEngine.Object.Destroy(this.m_RootObject);
		}
		Resources.UnloadUnusedAssets();
	}
}
