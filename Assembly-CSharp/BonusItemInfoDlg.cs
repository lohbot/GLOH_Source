using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BonusItemInfoDlg : Form
{
	private const int MAX_ITEM_COUNT = 3;

	private YesDelegate m_OkDelegate;

	private ResultDelegate m_ResultDelegate;

	private object m_ResultObject;

	private bool m_bActionResultDelegate;

	private object m_oOkObject;

	private Button m_btClose;

	private DrawTexture m_dtInBox;

	private Label m_lbTitle;

	private Label m_lbNotExistButtonNoti;

	private Button m_btOk;

	private DrawTexture[] m_dtItemSlot_2 = new DrawTexture[2];

	private ItemTexture[] m_itItemImg_2 = new ItemTexture[2];

	private Button[] m_btItem_2 = new Button[2];

	private DrawTexture[] m_dtItemSlot_3 = new DrawTexture[3];

	private ItemTexture[] m_itItemImg_3 = new ItemTexture[3];

	private Button[] m_btItem_3 = new Button[3];

	private ITEM[] m_Items = new ITEM[3];

	private int m_ItemCount;

	private bool m_bIsPlayAni;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Message/DLG_BonusItem_Info", G_ID.BONUS_ITEM_INFO_DLG, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_OkDelegate = null;
		this.m_dtInBox = (base.GetControl("DT_InBox") as DrawTexture);
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_lbNotExistButtonNoti = (base.GetControl("LB_Notice") as Label);
		this.m_btClose = (base.GetControl("Btn_Close") as Button);
		Button expr_65 = this.m_btClose;
		expr_65.Click = (EZValueChangedDelegate)Delegate.Combine(expr_65.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_btOk = (base.GetControl("Button_Ok") as Button);
		Button expr_A2 = this.m_btOk;
		expr_A2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A2.Click, new EZValueChangedDelegate(this.OnClickOk));
		for (int i = 0; i < 2; i++)
		{
			this.m_dtItemSlot_2[i] = (base.GetControl(string.Format("DT_ItemSlot2_{0}", i + 1)) as DrawTexture);
			this.m_itItemImg_2[i] = (base.GetControl(string.Format("IT_ItemImg2_{0}", i + 1)) as ItemTexture);
			this.m_btItem_2[i] = (base.GetControl(string.Format("Btn_ItemInfo2_{0}", i + 1)) as Button);
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_dtItemSlot_3[j] = (base.GetControl(string.Format("DT_ItemSlot3_{0}", j + 1)) as DrawTexture);
			this.m_itItemImg_3[j] = (base.GetControl(string.Format("IT_ItemImg3_{0}", j + 1)) as ItemTexture);
			this.m_btItem_3[j] = (base.GetControl(string.Format("Btn_ItemInfo3_{0}", j + 1)) as Button);
		}
	}

	public void AddItem(ITEM item)
	{
		if (item == null || item.m_nItemUnique <= 0)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < this.m_Items.Length; i++)
		{
			if (this.m_Items[i] == null || this.m_Items[i].m_nItemUnique <= 0)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			this.m_Items[num] = item;
			this.m_ItemCount++;
		}
	}

	public void ShowItem()
	{
		if (this.m_ItemCount % 2 == 0)
		{
			base.SetShowLayer(1, true);
			base.SetShowLayer(2, false);
			base.SetShowLayer(3, false);
			for (int i = 0; i < this.m_ItemCount; i++)
			{
				this.m_itItemImg_2[i].SetItemTexture(this.m_Items[i]);
				this.m_btItem_2[i].data = this.m_Items[i];
				Button expr_5E = this.m_btItem_2[i];
				expr_5E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5E.Click, new EZValueChangedDelegate(this.OnClickItem));
			}
		}
		else
		{
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, true);
			base.SetShowLayer(3, this.m_ItemCount > 1);
			for (int j = 0; j < this.m_ItemCount; j++)
			{
				this.m_itItemImg_3[j].SetItemTexture(this.m_Items[j]);
				this.m_btItem_3[j].data = this.m_Items[j];
				Button expr_ED = this.m_btItem_3[j];
				expr_ED.Click = (EZValueChangedDelegate)Delegate.Combine(expr_ED.Click, new EZValueChangedDelegate(this.OnClickItem));
			}
		}
	}

	public void SetMsg(YesDelegate a_deOk, object a_oObject, bool bShowButton, string strTitle, string strNotButtonNotify)
	{
		if (a_deOk != null)
		{
			this.m_OkDelegate = (YesDelegate)Delegate.Combine(this.m_OkDelegate, new YesDelegate(a_deOk.Invoke));
		}
		if (a_oObject != null)
		{
			this.m_oOkObject = a_oObject;
		}
		this.m_lbNotExistButtonNoti.SetText(strNotButtonNotify);
		this.m_lbTitle.Text = strTitle;
		this.m_lbNotExistButtonNoti.SetText(strNotButtonNotify);
		this.m_btOk.Hide(!bShowButton);
		this.m_lbNotExistButtonNoti.Hide(bShowButton);
		this.Show();
	}

	public void SetOkButtonEnable(bool bEnable)
	{
		if (!this.m_btOk.IsHidden())
		{
			this.m_btOk.SetEnabled(bEnable);
		}
	}

	private void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickOk(IUIObject obj)
	{
		if (this.m_OkDelegate != null)
		{
			this.m_OkDelegate(this.m_oOkObject);
		}
		this.SetOkButtonEnable(false);
	}

	private void OnClickItem(IUIObject obj)
	{
		ITEM iTEM = obj.Data as ITEM;
		if (iTEM == null)
		{
			return;
		}
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		if (itemTooltipDlg != null)
		{
			itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
		}
	}

	public void ActiveRewardEffect(ResultDelegate obj, object data)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "EXPLOERE", "BOX_OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("ui/mythicraid/fx_myth_raid_treasure_chest_mobile", this.m_dtInBox, this.m_dtInBox.GetSize());
		this.m_bIsPlayAni = true;
		this.m_ResultDelegate = obj;
		this.m_ResultObject = data;
	}

	public override void Update()
	{
		if (this.m_bIsPlayAni)
		{
			Animation componentInChildren = this.m_dtInBox.transform.GetComponentInChildren<Animation>();
			if (componentInChildren != null && !componentInChildren.isPlaying)
			{
				this.m_bIsPlayAni = false;
				if (this.m_ResultDelegate != null)
				{
					this.m_ResultDelegate(this.m_ResultObject);
					this.m_bActionResultDelegate = true;
				}
				this.Close();
			}
		}
	}

	public override void OnClose()
	{
		if (!this.m_bActionResultDelegate && this.m_ResultDelegate != null)
		{
			this.m_ResultDelegate(this.m_ResultObject);
		}
	}
}
