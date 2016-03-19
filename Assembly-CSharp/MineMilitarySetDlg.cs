using PROTOCOL;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MineMilitarySetDlg : Form
{
	private Button m_btGoMilitarySet;

	private Button m_btClose;

	private Label m_lTitle;

	private DrawTexture m_dBGImage;

	private Button[] m_btOccMilitary = new Button[9];

	private ItemTexture[] m_itOccMilitary = new ItemTexture[9];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/DLG_MineMilitarySet", G_ID.MINE_MILITARY_SET_DLG, false, true);
		float x = GUICamera.width - base.GetSizeX();
		float y = 0f;
		base.SetLocation(x, y);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 9; i++)
		{
			this.m_btOccMilitary[i] = (base.GetControl(string.Format("BT_User0{0}", i + 1)) as Button);
			this.m_itOccMilitary[i] = (base.GetControl(string.Format("DT_User0{0}", i + 1)) as ItemTexture);
		}
		this.m_lTitle = (base.GetControl("LB_Title") as Label);
		this.m_dBGImage = (base.GetControl("DT_ImageBG") as DrawTexture);
		this.m_btClose = (base.GetControl("BT_Cancel") as Button);
		Button expr_A5 = this.m_btClose;
		expr_A5.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A5.Click, new EZValueChangedDelegate(this.OnBtnClickClose));
		this.m_btGoMilitarySet = (base.GetControl("BT_Start") as Button);
		Button expr_E2 = this.m_btGoMilitarySet;
		expr_E2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E2.Click, new EZValueChangedDelegate(this.OnBtnClickGoMilitarySet));
		this.Show();
		string str = string.Format("UI/Mine/{0}{1}", "bg_minemilitaryset", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SetBackImage), "bg_minemilitaryset");
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
	}

	private void SetBackImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.m_dBGImage.SetTexture(texture2D);
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}

	public void ShowMilitarySolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1410"),
			"targetname",
			charPersonInfo.GetCharName()
		});
		this.m_lTitle.SetText(empty);
		foreach (KeyValuePair<int, MINE_MILITARY_USER_SOLINFO> current in SoldierBatch.MINE_INFO.GetUser_SolList())
		{
			MINE_MILITARY_USER_SOLINFO value = current.Value;
			byte ui8BatchIndex = value.ui8BatchIndex;
			this.m_itOccMilitary[(int)ui8BatchIndex].Visible = true;
			this.m_btOccMilitary[(int)ui8BatchIndex].Visible = true;
			this.m_itOccMilitary[(int)ui8BatchIndex].SetSolImageTexure(eCharImageType.SMALL, value.mine_solinfo[0].i32Kind, (int)value.mine_solinfo[0].ui8Grade);
			this.m_btOccMilitary[(int)ui8BatchIndex].Data = current;
			Button expr_EF = this.m_btOccMilitary[(int)ui8BatchIndex];
			expr_EF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_EF.Click, new EZValueChangedDelegate(this.ClickOccupyDetailInfo));
		}
		this.Show();
	}

	public void SetOccupyDetailinfo(int Index)
	{
		if (SoldierBatch.MINE_INFO.GetUser_SolList().ContainsKey(Index) && SoldierBatch.MINE_INFO.GetUser_SolList() != null)
		{
			this.SetOccupyDetailinfo(SoldierBatch.MINE_INFO.GetUser_SolList()[Index]);
		}
	}

	public void SetOccupyDetailinfo(MINE_MILITARY_USER_SOLINFO info)
	{
	}

	public void OnBtnClickClose(IUIObject obj)
	{
	}

	public void OnBtnClickGoMilitarySet(IUIObject obj)
	{
	}

	private void OnCompleteBatch(object a_oObject)
	{
	}

	public void ClickOccupyDetailInfo(IUIObject obj)
	{
		MINE_MILITARY_USER_SOLINFO mINE_MILITARY_USER_SOLINFO = obj.Data as MINE_MILITARY_USER_SOLINFO;
		if (mINE_MILITARY_USER_SOLINFO != null)
		{
			this.SetOccupyDetailinfo(mINE_MILITARY_USER_SOLINFO);
		}
	}
}
