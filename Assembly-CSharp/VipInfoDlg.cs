using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class VipInfoDlg : Form
{
	private const int MIN_SLOT_COUNT = 0;

	private Label m_LabelTitle;

	private Button m_ButtonCancel;

	private NewListBox m_nlbVip;

	private DrawTexture m_dtMyVipMark;

	private Label m_lVipLevel;

	private DrawTexture m_DT_VipDrawTextureBg1;

	private DrawTexture m_DT_VipDrawTextureBg2;

	private Label m_lVipExp;

	private Label m_lVip;

	private Label m_lVipState;

	private Label m_lVipTile;

	private DrawTexture m_dtVIPMark1;

	private DrawTexture m_dtVIPMark2;

	private DrawTexture m_dtEffect;

	private Label m_lheart;

	private Label m_lSpeedUp;

	private Label m_lBabelTower;

	private List<VIP_INFODATA> m_listVipInfo = new List<VIP_INFODATA>();

	private VIP_INFODATA m_currVipInfo = new VIP_INFODATA();

	public bool m_bVipLevelUp;

	private long m_VipLevelUpdateTime;

	private int MAX_SLOT_COUNT = 5;

	private byte m_nCurrMyVipLevel;

	private int m_oldSelectIndex;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Message/DLG_VIPinfo", G_ID.VIPINFO_DLG, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_dtEffect = (base.GetControl("DrawTexture_popup_BG") as DrawTexture);
		this.m_LabelTitle = (base.GetControl("Label_title") as Label);
		this.m_ButtonCancel = (base.GetControl("Button_cancel") as Button);
		this.m_ButtonCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBtCancel));
		this.m_dtMyVipMark = (base.GetControl("DT_MyVIP_Mark") as DrawTexture);
		this.m_lVipLevel = (base.GetControl("LB_VIPlv_01") as Label);
		this.m_DT_VipDrawTextureBg1 = (base.GetControl("DT_VIPgage") as DrawTexture);
		this.m_DT_VipDrawTextureBg2 = (base.GetControl("DT_VIPgagein") as DrawTexture);
		this.m_lVipExp = (base.GetControl("LB_VIPexp") as Label);
		this.m_nlbVip = (base.GetControl("NLB_VIPBtn") as NewListBox);
		this.m_nlbVip.Clear();
		this.m_lVip = (base.GetControl("LB_VIP") as Label);
		this.m_lVipState = (base.GetControl("Label_VIPState") as Label);
		this.m_dtVIPMark1 = (base.GetControl("DT_VIP_Rank_Mark1") as DrawTexture);
		this.m_lVipTile = (base.GetControl("Label_VIPTitle") as Label);
		this.m_dtVIPMark2 = (base.GetControl("DT_VIP_Rank_Mark2") as DrawTexture);
		this.m_lheart = (base.GetControl("LB_Heart") as Label);
		this.m_lheart.Visible = false;
		this.m_lSpeedUp = (base.GetControl("LB_SpeedUP") as Label);
		this.m_lSpeedUp.Visible = false;
		this.m_lBabelTower = (base.GetControl("LB_BabelTower") as Label);
		this.m_lBabelTower.Visible = false;
		string text = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2849");
		this.m_LabelTitle.SetText(text);
		this.MAX_SLOT_COUNT = NrTSingleton<NrVipSubInfoManager>.Instance.GetSize();
		base.SetScreenCenter();
	}

	public void OnClickBtCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.VIPINFO_DLG);
	}

	public void SetLevel(byte i8Level, bool bVipLevelUpCheck)
	{
		if (bVipLevelUpCheck)
		{
			NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_firework_mobile", this.m_dtEffect, this.m_dtEffect.GetSize());
			this.m_bVipLevelUp = true;
		}
		if (i8Level <= 0 || (int)i8Level > this.MAX_SLOT_COUNT)
		{
			this.m_nCurrMyVipLevel = 0;
			base.ShowLayer(1, 3);
		}
		else
		{
			this.m_nCurrMyVipLevel = i8Level;
			base.ShowLayer(1, 4);
		}
		VipSubInfo vipSubInfo = NrTSingleton<NrVipSubInfoManager>.Instance.Get_VipSubInfo(i8Level);
		if (vipSubInfo == null)
		{
			return;
		}
		this.m_dtMyVipMark.SetTextureFromBundle(string.Format("UI/etc/{0}", vipSubInfo.strIconPath));
		this.m_lVipLevel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(vipSubInfo.strTitle.ToString());
		this.SetVipExp(i8Level);
		this.SetData(i8Level);
	}

	public void SetVipInfo(byte i8Level)
	{
		VipSubInfo vipSubInfo = NrTSingleton<NrVipSubInfoManager>.Instance.Get_VipSubInfo(i8Level);
		if (vipSubInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		this.m_listVipInfo = NrTSingleton<NrTableVipManager>.Instance.GetValue();
		for (int i = 0; i < this.m_listVipInfo.Count; i++)
		{
			if (this.m_listVipInfo[i].i8VipLevel == i8Level)
			{
				this.m_currVipInfo = this.m_listVipInfo[i];
			}
		}
		if (i8Level == 0)
		{
			this.m_dtVIPMark1.SetTextureFromBundle(string.Format("UI/etc/{0}", vipSubInfo.strIconPath));
		}
		else
		{
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			int num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT) + (int)this.m_currVipInfo.i8Battle_Repeat_Add;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3345"),
				"count",
				this.m_currVipInfo.i8FriendSupportNum
			});
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3345"),
				"count",
				this.m_currVipInfo.i16FastBattle
			});
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3346"),
				"count",
				num
			});
			this.m_dtVIPMark2.SetTextureFromBundle(string.Format("UI/etc/{0}", vipSubInfo.strIconPath));
			this.m_lheart.SetText(empty);
			this.m_lSpeedUp.SetText(empty2);
			this.m_lBabelTower.SetText(empty3);
		}
		this.m_lVip.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(vipSubInfo.strNote));
		this.m_lVipState.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(vipSubInfo.strState));
		this.m_lVipTile.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(vipSubInfo.strTitle));
	}

	public void SetData(byte i8Level)
	{
		for (int i = 0; i < 11; i++)
		{
			VipSubInfo vipSubInfo = NrTSingleton<NrVipSubInfoManager>.Instance.Get_VipSubInfo((byte)i);
			NewListItem newListItem = new NewListItem(this.m_nlbVip.ColumnNum, true, string.Empty);
			newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(vipSubInfo.strTitle.ToString()), vipSubInfo.byVipLevel, new EZValueChangedDelegate(this.ClickVip), null);
			newListItem.SetListItemData(1, false);
			if ((int)this.m_nCurrMyVipLevel == i)
			{
				newListItem.SetListItemData(2, true);
			}
			else
			{
				newListItem.SetListItemData(2, false);
			}
			this.m_nlbVip.Add(newListItem);
		}
		this.m_nlbVip.RepositionItems();
		this.SetVipInfo(i8Level);
	}

	public void ClickVip(IUIObject obj)
	{
		if (obj == null && obj.Data == null)
		{
			return;
		}
		byte b = (byte)obj.Data;
		UIListItemContainer selectItem = this.m_nlbVip.GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		if (this.m_oldSelectIndex != selectItem.index)
		{
			UIListItemContainer item = this.m_nlbVip.GetItem(this.m_oldSelectIndex);
			UIButton uIButton = item.GetElement(1) as UIButton;
			uIButton.Visible = false;
			this.m_oldSelectIndex = selectItem.index;
		}
		UIButton uIButton2 = selectItem.GetElement(1) as UIButton;
		if (uIButton2 != null)
		{
			uIButton2.Visible = true;
		}
		if (b == 0)
		{
			base.ShowLayer(1, 3);
		}
		else
		{
			base.ShowLayer(1, 4);
		}
		this.SetVipInfo(b);
	}

	public void SetVipExp(byte i8Level)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
			long num = NrTSingleton<NrTableVipManager>.Instance.GetLevelMaxExp(i8Level);
			long nextLevelMaxExp = NrTSingleton<NrTableVipManager>.Instance.GetNextLevelMaxExp(i8Level);
			string text = string.Empty;
			if (nextLevelMaxExp == 0L)
			{
				num = NrTSingleton<NrTableVipManager>.Instance.GetBeforLevelMaxExp(i8Level);
				text = num.ToString() + " / " + num.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize(this.m_DT_VipDrawTextureBg1.width, this.m_DT_VipDrawTextureBg2.GetSize().y);
			}
			else
			{
				long num2 = charSubData - num;
				long num3 = nextLevelMaxExp - num;
				float num4 = (float)num2 / (float)num3;
				text = num2.ToString() + " / " + num3.ToString();
				this.m_DT_VipDrawTextureBg2.SetSize((this.m_DT_VipDrawTextureBg1.GetSize().x - 2f) * num4, this.m_DT_VipDrawTextureBg2.GetSize().y);
			}
			this.m_DT_VipDrawTextureBg2.SetLocation(this.m_DT_VipDrawTextureBg1.GetLocationX(), this.m_DT_VipDrawTextureBg1.GetLocationY());
			this.m_lVipExp.SetText(text);
		}
		else
		{
			this.m_DT_VipDrawTextureBg2.Visible = false;
		}
	}

	public override void Update()
	{
		if (this.m_bVipLevelUp)
		{
			if (this.m_VipLevelUpdateTime == 0L)
			{
				this.m_VipLevelUpdateTime = PublicMethod.GetCurTime();
			}
			else if (PublicMethod.GetCurTime() > this.m_VipLevelUpdateTime + 5L)
			{
				GameObject obj = GameObject.Find("fx_firework");
				UnityEngine.Object.Destroy(obj);
				this.m_VipLevelUpdateTime = 0L;
				this.m_bVipLevelUp = false;
				return;
			}
		}
	}
}
