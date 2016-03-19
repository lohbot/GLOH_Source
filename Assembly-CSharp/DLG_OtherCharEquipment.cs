using GAME;
using System;
using UnityEngine;
using UnityForms;

public class DLG_OtherCharEquipment : Form
{
	private const int EventSTATISTICS_NUM = 2;

	private Label m_lbCharName;

	private Label m_lbHP;

	private Label m_lbAtack;

	private Label m_lbDefence;

	private DrawTexture m_dtCharImg;

	private DrawTexture m_dtRank;

	private ImageView m_ivHelmet;

	private ImageView m_ivArmor;

	private ImageView m_ivGlove;

	private ImageView m_ivBoots;

	private ImageView m_ivWeapon;

	private ImageView m_ivRing;

	private DrawTexture m_dtEventTexture;

	private Label[] m_lbEventHeroStat = new Label[2];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/dlg_otherequipmentwindow", G_ID.DLG_OTHER_CHAR_EQUIPMENT, true);
	}

	public override void SetComponent()
	{
		this.m_lbCharName = (base.GetControl("Label_ChaName") as Label);
		this.m_lbHP = (base.GetControl("Label_stats_HP2") as Label);
		this.m_lbAtack = (base.GetControl("Label_stats_damage2") as Label);
		this.m_lbDefence = (base.GetControl("Label_stats_defence2") as Label);
		this.m_dtCharImg = (base.GetControl("drawtexture_ch_img") as DrawTexture);
		this.m_dtRank = (base.GetControl("DrawTexture_rank01") as DrawTexture);
		this.m_ivHelmet = (base.GetControl("item_head") as ImageView);
		this.InitImageView(this.m_ivHelmet);
		this.m_ivArmor = (base.GetControl("item_armor") as ImageView);
		this.InitImageView(this.m_ivArmor);
		this.m_ivGlove = (base.GetControl("item_glove") as ImageView);
		this.InitImageView(this.m_ivGlove);
		this.m_ivBoots = (base.GetControl("item_boots") as ImageView);
		this.InitImageView(this.m_ivBoots);
		this.m_ivWeapon = (base.GetControl("item_weapon") as ImageView);
		this.InitImageView(this.m_ivWeapon);
		this.m_ivRing = (base.GetControl("item_ring") as ImageView);
		this.InitImageView(this.m_ivRing);
		this.m_dtEventTexture = (base.GetControl("DrawTexture_Event") as DrawTexture);
		for (int i = 0; i < 2; i++)
		{
			this.m_lbEventHeroStat[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (i + 1).ToString())) as Label);
		}
		base.SetScreenCenter();
	}

	private string GetCharNameBySoldierInfo(NkSoldierInfo solInfo)
	{
		DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
		if (dLG_OtherCharDetailInfo != null)
		{
			return dLG_OtherCharDetailInfo.GetCharNameBySoldierInfo(solInfo);
		}
		return string.Empty;
	}

	public void SetSolStatInfo(NkSoldierInfo solInfo)
	{
		if (solInfo != null)
		{
			string charNameBySoldierInfo = this.GetCharNameBySoldierInfo(solInfo);
			this.m_lbCharName.Text = string.Format("Lv.{0} {1}", solInfo.GetLevel(), charNameBySoldierInfo);
			string text = string.Empty;
			string empty = string.Empty;
			EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(solInfo.GetCharKind(), solInfo.GetGrade());
			int num = solInfo.GetMaxHP();
			int num2 = solInfo.GetMinDamage();
			int num3 = solInfo.GetMaxDamage();
			string text2 = solInfo.GetMaxHP().ToString();
			string text3 = solInfo.GetMinDamage().ToString();
			string text4 = solInfo.GetMaxDamage().ToString();
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			string second = string.Empty;
			string empty2 = string.Empty;
			if (eventHeroCharCode != null)
			{
				int num4 = 0;
				if (eventHeroCharCode.i32Hp != 100)
				{
					num = (int)((float)solInfo.GetMaxHP() * ((float)eventHeroCharCode.i32Hp * 0.01f));
					second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", (num - solInfo.GetMaxHP()).ToString(), ")", textColor2);
					text2 = NrTSingleton<UIDataManager>.Instance.GetString(solInfo.GetMaxHP().ToString(), second);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2192"),
						"count",
						eventHeroCharCode.i32Hp.ToString()
					});
					this.m_lbEventHeroStat[num4].SetText(empty2);
					this.m_lbEventHeroStat[num4].SetAnchorText(SpriteText.Anchor_Pos.Middle_Right);
					this.m_lbEventHeroStat[num4].SetEnabled(true);
					this.m_lbEventHeroStat[num4].Hide(false);
					num4++;
				}
				if (eventHeroCharCode.i32Attack != 100)
				{
					num2 = (int)((float)solInfo.GetMinDamage() * ((float)eventHeroCharCode.i32Attack * 0.01f));
					num3 = (int)((float)solInfo.GetMaxDamage() * ((float)eventHeroCharCode.i32Attack * 0.01f));
					second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", (num2 - solInfo.GetMinDamage()).ToString(), ")", textColor2);
					text3 = NrTSingleton<UIDataManager>.Instance.GetString(solInfo.GetMinDamage().ToString(), second);
					second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", (num3 - solInfo.GetMaxDamage()).ToString(), ")", textColor2);
					text4 = NrTSingleton<UIDataManager>.Instance.GetString(solInfo.GetMaxDamage().ToString(), second);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2191"),
						"count",
						eventHeroCharCode.i32Attack.ToString()
					});
					this.m_lbEventHeroStat[num4].SetText(empty2);
					this.m_lbEventHeroStat[num4].SetAnchorText(SpriteText.Anchor_Pos.Middle_Right);
					this.m_lbEventHeroStat[num4].SetEnabled(true);
					this.m_lbEventHeroStat[num4].Hide(false);
				}
				this.m_dtEventTexture.Visible = true;
				Transform child = NkUtil.GetChild(this.m_dtEventTexture.gameObject.transform, "child_effect");
				if (child == null)
				{
					NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_EVENTFONT", this.m_dtEventTexture, this.m_dtEventTexture.GetSize());
				}
			}
			else
			{
				this.m_dtEventTexture.Visible = false;
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1879");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"maxhp",
				text2
			});
			this.m_lbHP.Text = empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1880");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"mindmg",
				text3,
				"maxdmg",
				text4
			});
			this.m_lbAtack.Text = empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1881");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"defance",
				solInfo.GetPhysicalDefense().ToString()
			});
			this.m_lbDefence.Text = empty;
			float num5 = 512f;
			this.m_dtCharImg.SetUVMask(new Rect(4f / num5, 0f, 504f / num5, 448f / num5));
			this.m_dtCharImg.SetTexture(eCharImageType.LARGE, solInfo.GetCharKind(), (int)solInfo.GetGrade());
			short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(solInfo.GetCharKind(), (int)solInfo.GetGrade());
			UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(solInfo.GetCharKind(), (int)solInfo.GetGrade());
			this.m_dtRank.Visible = (null != solLargeGradeImg);
			if (0 < legendType)
			{
				this.m_dtRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.m_dtRank.SetTexture(solLargeGradeImg);
			this.SetItem(this.m_ivHelmet, solInfo.GetEquipItem(1));
			this.SetItem(this.m_ivArmor, solInfo.GetEquipItem(2));
			this.SetItem(this.m_ivGlove, solInfo.GetEquipItem(3));
			this.SetItem(this.m_ivBoots, solInfo.GetEquipItem(4));
			this.SetItem(this.m_ivWeapon, solInfo.GetEquipItem(0));
			this.SetItem(this.m_ivRing, solInfo.GetEquipItem(5));
		}
		else
		{
			this.m_lbCharName.Text = string.Empty;
			this.m_lbHP.Text = string.Empty;
			this.m_lbAtack.Text = string.Empty;
			this.m_lbDefence.Text = string.Empty;
		}
	}

	private void SetDrawItem(DrawTexture control, ITEM kItem)
	{
		if (null != control)
		{
			control.InitSlotEffect();
			if (kItem != null)
			{
				if (null == control.collider)
				{
					control.UsedCollider(true);
				}
				int rank = kItem.m_nOption[2];
				if (string.Compare(ItemManager.RankStateString(rank), "best") == 0)
				{
					NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", control, control.GetSize());
				}
				control.SetTexture(NrTSingleton<ItemManager>.Instance.GetItemTexture(kItem.m_nItemUnique));
				control.c_cItemTooltip = kItem;
				control.nEquipItemPosition = 1;
				control.Visible = true;
			}
			else
			{
				control.BaseInfoLoderImage = null;
				control.Visible = false;
			}
		}
	}

	public void SetItem(ImageView IvItem, ITEM Item)
	{
		if (Item != null && 0 < Item.m_nItemUnique)
		{
			IvItem.Visible = true;
			ImageSlot slot = new ImageSlot();
			ReforgeMainDlg.SetImageSlotFromItem(ref slot, Item, 0);
			IvItem.Clear();
			IvItem.SetImageSlot(0, slot, null, null, null, null);
			IvItem.RepositionItems();
		}
		else
		{
			IvItem.Visible = false;
		}
	}

	public void DragDrop(EZDragDropParams a_sDragDropParams)
	{
	}

	public void On_ClickItem(IUIObject obj)
	{
		UIScrollList uIScrollList = obj as UIScrollList;
		if (uIScrollList != null)
		{
			UIListItemContainer selectedItem = uIScrollList.SelectedItem;
			if (selectedItem != null)
			{
				ImageSlot imageSlot = selectedItem.Data as ImageSlot;
				if (imageSlot != null && imageSlot.c_oItem != null)
				{
					ITEM iTEM = new ITEM();
					iTEM.Set(imageSlot.c_oItem as ITEM);
					if (iTEM != null && iTEM.m_nItemUnique > 0)
					{
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
					}
				}
			}
		}
	}

	public void On_Mouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}

	public void InitImageView(ImageView IvItem)
	{
		IvItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickItem));
		IvItem.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
		IvItem.SetImageView(0, 0, 80, 80, 0, 0, (int)IvItem.GetSize().y);
		IvItem.spacingAtEnds = false;
		IvItem.touchScroll = false;
		IvItem.clipContents = false;
		IvItem.ListDrag = false;
	}

	public static void ShowItemDetailInfo(ITEM Item, G_ID eGID)
	{
		ITEM iTEM = new ITEM();
		iTEM.Set(Item);
		if (iTEM != null && iTEM.m_nItemUnique > 0)
		{
			ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
			itemTooltipDlg.Set_Tooltip(eGID, iTEM, null, false);
		}
	}
}
