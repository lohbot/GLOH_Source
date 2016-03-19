using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CreditDlg : Form
{
	private bool check = true;

	private GameObject rootGameObject;

	private GameObject childGameObject;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "System/DLG_Dummy", G_ID.CREDIT_DLG, false);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		string str = string.Format("{0}", "UI/ETC/fx_direct_credit" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.ShowCredit), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		UIDataManager.MuteSound(true);
	}

	public override void Update()
	{
		if (null != this.childGameObject && this.check && !this.childGameObject.animation.isPlaying)
		{
			this.check = false;
			this.Close();
		}
	}

	private void ShowCredit(WWWItem _item, object _param)
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
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.rootGameObject);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 90f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.childGameObject = NkUtil.GetChild(this.rootGameObject.transform, "fx_ending").gameObject;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
			}
		}
	}

	public override void OnClose()
	{
		UIDataManager.MuteSound(false);
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.Destroy(this.rootGameObject);
		}
		Resources.UnloadUnusedAssets();
	}
}
