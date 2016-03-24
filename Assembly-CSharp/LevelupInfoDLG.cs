using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class LevelupInfoDLG : Form
{
	private Label m_lbTitle;

	private Box m_boxExplain;

	private Button m_btClose;

	private Button m_btPrevSign;

	private Button m_btNextSign;

	private Label m_lbPageCounting;

	private Queue<LevelupInfo> levelupInfoQueue = new Queue<LevelupInfo>();

	private LevelupInfo showLevelupInfo;

	private int levelupInfoIndex;

	private Vector3 nextButtonPosition = Vector3.zero;

	private Vector3 prevButtonPosition = Vector3.zero;

	private string titleText = string.Empty;

	private string pageCountingText = string.Empty;

	private bool canPlayEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "system/dlg_levelupguide", G_ID.LEVELUP_GUIDE_DLG, false);
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetZ() + 50f);
	}

	public override void OnClose()
	{
	}

	public override void SetComponent()
	{
		this.m_boxExplain = (base.GetControl("LevelUpGuideExplain") as Box);
		this.m_btClose = (base.GetControl("Button_Close") as Button);
		Button expr_32 = this.m_btClose;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this._clickClose));
		this.m_btPrevSign = (base.GetControl("Button_Prev") as Button);
		Button expr_6F = this.m_btPrevSign;
		expr_6F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_6F.Click, new EZValueChangedDelegate(this._clickPrev));
		this.m_btNextSign = (base.GetControl("Button_Next") as Button);
		Button expr_AC = this.m_btNextSign;
		expr_AC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_AC.Click, new EZValueChangedDelegate(this._clickNext));
		this.m_lbTitle = (base.GetControl("Talk_LevelUpTitle") as Label);
		this.m_lbPageCounting = (base.GetControl("Label_pagecounting") as Label);
		this.titleText = this.m_lbTitle.GetText();
		this.pageCountingText = this.m_lbPageCounting.GetText();
		this.nextButtonPosition = this.m_btNextSign.transform.localPosition;
		this.prevButtonPosition = this.m_btPrevSign.transform.localPosition;
		base.SetScreenCenter();
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHAT_MOBILE_SUB_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG) && this.canPlayEffect)
		{
			this.canPlayEffect = false;
			this.EffectShow();
		}
	}

	public void Show(LevelupInfo levelupInfo)
	{
		if (this.showLevelupInfo == null)
		{
			this.showLevelupInfo = levelupInfo;
			this.SetInfo();
			base.Show();
			this.canPlayEffect = true;
		}
		else
		{
			this.levelupInfoQueue.Enqueue(levelupInfo);
		}
	}

	private void SetImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() == null)
		{
			return;
		}
		if (_item.GetSafeBundle().mainAsset == null)
		{
			return;
		}
		Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
		if (texture2D != null && _param is ItemTexture)
		{
			ItemTexture itemTexture = _param as ItemTexture;
			itemTexture.SetTexture(texture2D);
		}
	}

	private void _clickClose(IUIObject _obj)
	{
		if (this.levelupInfoQueue.Count > 0)
		{
			this.showLevelupInfo = this.levelupInfoQueue.Dequeue();
			this.levelupInfoIndex = 0;
			this.SetInfo();
		}
		else
		{
			this.Close();
		}
	}

	private void _clickPrev(IUIObject _obj)
	{
		if (this.levelupInfoIndex > 0)
		{
			this.levelupInfoIndex--;
			this.ChangeTextureExplain(this.levelupInfoIndex);
			this.SetPageCounting();
			this.SetButtonChange();
		}
	}

	private void _clickNext(IUIObject _obj)
	{
		if (this.levelupInfoIndex + 1 < this.showLevelupInfo.GetListCount())
		{
			this.levelupInfoIndex++;
			this.ChangeTextureExplain(this.levelupInfoIndex);
			this.SetPageCounting();
			this.SetButtonChange();
		}
	}

	private void SetInfo()
	{
		this.ChangeTextureExplain(this.levelupInfoIndex);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			this.titleText,
			"level",
			this.showLevelupInfo.GetLevel()
		});
		this.m_lbTitle.SetText(empty);
		this.SetPageCounting();
		this.SetButtonChange();
	}

	private void SetButtonChange()
	{
		this.m_btPrevSign.Visible = true;
		this.m_btNextSign.Visible = true;
		this.m_btPrevSign.transform.localPosition = this.prevButtonPosition;
		this.m_btNextSign.transform.localPosition = this.nextButtonPosition;
		if (this.levelupInfoIndex == 0 && this.showLevelupInfo.GetListCount() == 1)
		{
			this.m_btPrevSign.Visible = false;
			this.m_btNextSign.Visible = false;
		}
		else if (this.levelupInfoIndex <= 0)
		{
			this.m_btPrevSign.Visible = false;
		}
		else if (this.levelupInfoIndex + 1 == this.showLevelupInfo.GetListCount())
		{
			this.m_btNextSign.Visible = false;
		}
	}

	private void SetPageCounting()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			this.pageCountingText,
			"nowpagenum",
			this.levelupInfoIndex + 1,
			"maxpagenum",
			this.showLevelupInfo.GetListCount()
		});
		this.m_lbPageCounting.SetText(empty);
	}

	private void ChangeTextureExplain(int index)
	{
		string text;
		string strTextKey;
		this.showLevelupInfo.GetTexExplain(index, out text, out strTextKey);
		this.m_boxExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey));
	}

	private void SetOkButtonVisible(bool isVisible)
	{
		this.m_btNextSign.Visible = !isVisible;
	}

	private void EffectShow()
	{
		string str = "Effect/Instant/fx_levelup_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this._funcUIEffectDownloaded), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void _funcUIEffectDownloaded(IDownloadedItem wItem, object obj)
	{
		if (null == wItem.mainAsset)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
			return;
		}
		GameObject gameObject = wItem.mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		if (null == gameObject2)
		{
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 300f;
		gameObject2.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(gameObject2, GUICamera.UILayer);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject2);
		}
		if (null != gameObject2)
		{
			UnityEngine.Object.DestroyObject(gameObject2, 5f);
		}
	}

	private void SetDrawTexture(ItemTexture tex, string imageKey)
	{
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey);
		if (null != texture)
		{
			tex.SetTexture(texture);
		}
		else
		{
			string str = string.Format("{0}{1}", imageKey, NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetImage), tex);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
	}
}
