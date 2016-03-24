using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ItemMallSolDetailDlg : Form
{
	private class SolMythSkillSlotControl
	{
		private Button m_btMythSkill;

		private Button m_btMythSkill_Off;

		private DrawTexture m_dtMythSkillIcon;

		private DrawTexture m_dtMythSkillLock;

		private DrawTexture m_dtMythBG;

		private DrawTexture m_dtMythSkillBg;

		private Label m_lbMythSkillName;

		private Label m_lbMythSkill;

		private int m_i32MythSkillUnique;

		public void SetComponent(Form parent)
		{
			this.m_btMythSkill = (parent.GetControl("Btn_MythSkill") as Button);
			this.m_btMythSkill_Off = (parent.GetControl("Btn_MythSkill_Off") as Button);
			this.m_dtMythSkillIcon = (parent.GetControl("DT_MythSkillIcon") as DrawTexture);
			this.m_dtMythSkillLock = (parent.GetControl("DT_MythSkillLock") as DrawTexture);
			this.m_dtMythBG = (parent.GetControl("DT_GraBG") as DrawTexture);
			this.m_dtMythSkillBg = (parent.GetControl("DT_MythSkillIconBG") as DrawTexture);
			this.m_lbMythSkillName = (parent.GetControl("LB_MythSkillName") as Label);
			this.m_lbMythSkill = (parent.GetControl("LB_MythSkill") as Label);
			this.m_btMythSkill.Click = new EZValueChangedDelegate(this.OnClickMyth);
			this.m_btMythSkill_Off.Click = new EZValueChangedDelegate(this.OnClickMyth);
			this.SetEmpty();
		}

		public void SetEmpty()
		{
			this.m_btMythSkill.Visible = false;
			this.m_btMythSkill_Off.Visible = false;
			this.m_dtMythSkillIcon.Visible = false;
			this.m_dtMythSkillLock.Visible = false;
			this.m_dtMythBG.Visible = false;
			this.m_dtMythSkillBg.Visible = false;
			this.m_lbMythSkillName.Visible = false;
			this.m_lbMythSkill.Visible = false;
		}

		public void ShowInfo(int i32CharKind, int i32Grade)
		{
			this.SetEmpty();
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
			BATTLESKILL_BASE mythBattleSkill = charKindInfo.GetMythBattleSkill();
			if (mythBattleSkill == null)
			{
				return;
			}
			this.m_i32MythSkillUnique = mythBattleSkill.m_nSkillUnique;
			if (i32Grade >= 10)
			{
				this.m_btMythSkill.Visible = true;
			}
			else
			{
				this.m_btMythSkill_Off.Visible = true;
				this.m_dtMythSkillLock.Visible = true;
			}
			this.m_dtMythSkillIcon.Visible = true;
			this.m_dtMythBG.Visible = true;
			this.m_dtMythSkillBg.Visible = true;
			this.m_lbMythSkillName.Visible = true;
			this.m_lbMythSkill.Visible = true;
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(mythBattleSkill.m_nSkillUnique);
			this.m_dtMythSkillIcon.SetTexture(battleSkillIconTexture);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
				"skillname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mythBattleSkill.m_strTextKey)
			});
			this.m_lbMythSkillName.Text = empty;
		}

		public void OnClickMyth(IUIObject obj)
		{
			if (this.m_i32MythSkillUnique <= 0)
			{
				return;
			}
			SolDetail_Skill_Dlg solDetail_Skill_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_SKILLICON_DLG) as SolDetail_Skill_Dlg;
			if (solDetail_Skill_Dlg != null)
			{
				solDetail_Skill_Dlg.TopMost = true;
				solDetail_Skill_Dlg.SetSkillData(this.m_i32MythSkillUnique, this.m_i32MythSkillUnique, true);
				solDetail_Skill_Dlg.SetFocus();
			}
		}
	}

	private FunDelegate m_BuyDelegate;

	private SolDetailDlgTool m_SolInterfaceTool = new SolDetailDlgTool();

	protected Label m_Label_Stats_str2;

	protected Label m_Label_Stats_dex2;

	protected Label m_Label_Stats_vit2;

	protected Label m_Label_Stats_int2;

	private Button m_Button_MovieBtn;

	private ScrollLabel m_ScrollLabel_SolInfo;

	private Label m_Label_Price;

	private Button m_Button_Price;

	private DrawTexture m_DrawTexture_Won;

	private G_ID m_eParentUI = G_ID.ITEMMALL_DLG;

	private ItemMallSolDetailDlg.SolMythSkillSlotControl m_cMythSkillControl;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Item/dlg_itemmall_soldetail", G_ID.ITEMMALL_SOL_DETAIL, true);
	}

	public override void SetComponent()
	{
		this.m_SolInterfaceTool.m_DrawTextureWeapon = (base.GetControl("DT_Weapon") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Character_Name = (base.GetControl("Label_character_name") as Label);
		this.m_SolInterfaceTool.m_Label_SeasonNum = (base.GetControl("Label_SeasonNum") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Rank2 = (base.GetControl("Label_rank2") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Character = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_Button_MovieBtn = (base.GetControl("Button_Movie") as Button);
		this.m_Button_MovieBtn.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_PreViewHero));
		this.m_Label_Stats_str2 = (base.GetControl("Label_stats_str2") as Label);
		this.m_Label_Stats_dex2 = (base.GetControl("Label_stats_dex2") as Label);
		this.m_Label_Stats_vit2 = (base.GetControl("Label_stats_vit2") as Label);
		this.m_Label_Stats_int2 = (base.GetControl("Label_stats_int2") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Event = (base.GetControl("DrawTexture_Event") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_EventDate = (base.GetControl("Label_EventDate") as Label);
		for (int i = 0; i < 2; i++)
		{
			this.m_SolInterfaceTool.m_Lebel_EventHero[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (i + 1).ToString())) as Label);
		}
		this.m_SolInterfaceTool.m_DrawTextureSkillIcon = (base.GetControl("DT_SkillIcon") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_SkillAnger = (base.GetControl("Label_Anger") as Label);
		this.m_SolInterfaceTool.m_Label_SkillName = (base.GetControl("Label_SkillName") as Label);
		this.m_SolInterfaceTool.m_ScrollLabel_SkillInfo = (base.GetControl("ScrollLabel_SkillInfo") as ScrollLabel);
		this.m_ScrollLabel_SolInfo = (base.GetControl("ScrollLabel_SolInfo") as ScrollLabel);
		this.m_DrawTexture_Won = (base.GetControl("DrawTexture_Won") as DrawTexture);
		if (TsPlatform.IsIPhone || NrGlobalReference.strLangType.Equals("eng"))
		{
			this.m_DrawTexture_Won.SetTexture("Win_I_Dolla");
		}
		else
		{
			this.m_DrawTexture_Won.SetTexture("Win_I_Won");
		}
		this.m_Label_Price = (base.GetControl("Label_price") as Label);
		this.m_Button_Price = (base.GetControl("Button_Buy") as Button);
		this.m_Button_Price.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedPriceBtn));
		this.m_cMythSkillControl = new ItemMallSolDetailDlg.SolMythSkillSlotControl();
		this.m_cMythSkillControl.SetComponent(this);
		this.m_Button_Price.Visible = false;
		this.m_Label_Price.Visible = false;
		this.m_DrawTexture_Won.Visible = false;
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	private void OnClickedPriceBtn(IUIObject obj)
	{
		if (this.m_BuyDelegate != null)
		{
			this.m_BuyDelegate(obj);
		}
		this.Close();
	}

	private void OnIntroMovieButton(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int num = (int)obj.Data;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + num + " !!");
			return;
		}
		string sOLINTRO = charKindInfo.GetCHARKIND_INFO().SOLINTRO;
		if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				string str = string.Format("{0}SOLINTRO/", Option.GetProtocolRootPath(Protocol.HTTP));
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false, true);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false, true);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false, true);
		}
	}

	public void SetButtonEvent(FunDelegate delBuy)
	{
		if (delBuy != null)
		{
			this.m_BuyDelegate = (FunDelegate)Delegate.Combine(this.m_BuyDelegate, new FunDelegate(delBuy.Invoke));
		}
	}

	public void SetMallItem(ITEM_MALL_ITEM MallItem, int i32CharKind)
	{
		this.m_DrawTexture_Won.SetTexture(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)MallItem.m_nMoneyType));
		this.m_Label_Price.Text = ItemMallItemManager.GetCashPrice(MallItem);
		this.m_Button_Price.Data = MallItem;
		this.m_Label_Price.Visible = true;
		this.m_Button_Price.Visible = true;
		this.m_DrawTexture_Won.Visible = true;
		byte b = 6;
		int level = 50;
		List<SOL_GUIDE> value = NrTSingleton<NrTableSolGuideManager>.Instance.GetValue();
		foreach (SOL_GUIDE current in value)
		{
			if (current.m_i32CharKind == i32CharKind)
			{
				b = (byte)current.m_iSolGrade;
			}
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (i32CharKind != 0)
		{
			level = (int)charKindInfo.GetGradeMaxLevel((short)(b - 1));
		}
		this.SetSolKind(i32CharKind, b, level);
		this.m_eParentUI = G_ID.ITEMMALL_DLG;
	}

	public void SetSolKind(int i32CharKind, byte bGrade, int level)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + i32CharKind + " !!");
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		this.m_SolInterfaceTool.SetCharImg(bGrade - 1, string.Empty);
		this.m_SolInterfaceTool.SetHeroEventLabel(bGrade);
		this.m_SolInterfaceTool.SetSkillInfo_MaxLevel();
		this.m_Button_MovieBtn.data = i32CharKind;
		int num = charKindInfo.GetGradePlusSTR((int)(bGrade - 1));
		int num2 = charKindInfo.GetGradePlusDEX((int)(bGrade - 1));
		int num3 = charKindInfo.GetGradePlusINT((int)(bGrade - 1));
		int num4 = charKindInfo.GetGradePlusVIT((int)(bGrade - 1));
		num += charKindInfo.GetIncSTR((int)(bGrade - 1), level);
		num2 += charKindInfo.GetIncDEX((int)(bGrade - 1), level);
		num3 += charKindInfo.GetIncINT((int)(bGrade - 1), level);
		num4 += charKindInfo.GetIncVIT((int)(bGrade - 1), level);
		this.m_Label_Stats_str2.SetText(num.ToString());
		this.m_Label_Stats_dex2.SetText(num2.ToString());
		this.m_Label_Stats_vit2.SetText(num4.ToString());
		this.m_Label_Stats_int2.SetText(num3.ToString());
		this.m_ScrollLabel_SolInfo.SetScrollLabel(charKindInfo.GetDesc());
		this.m_cMythSkillControl.ShowInfo(i32CharKind, (int)bGrade);
	}

	private void Click_PreViewHero(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (!(obj.Data is int))
		{
			return;
		}
		int num = (int)obj.Data;
		if (NrTSingleton<NkCharManager>.Instance.GetChar(1) == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor(" [Click_PreViewHero] == SOL CHARKIND ERROR {0}" + num + " !!");
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.MessageBox_PreviewHero), charKindInfo.GetCharKind(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3293"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("438"), eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
	}

	private void MessageBox_PreviewHero(object a_oObject)
	{
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		GS_PREVIEW_HERO_START_REQ gS_PREVIEW_HERO_START_REQ = new GS_PREVIEW_HERO_START_REQ();
		gS_PREVIEW_HERO_START_REQ.i32CharKind = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PREVIEW_HERO_START_REQ, gS_PREVIEW_HERO_START_REQ);
		NrTSingleton<NkClientLogic>.Instance.GidPrivewHero = (int)this.m_eParentUI;
	}

	public void SetTimeShopItem(TIMESHOP_DATA _pTimeShopItem, int _i32CharKind)
	{
		if (_pTimeShopItem == null)
		{
			return;
		}
		this.m_DrawTexture_Won.SetTexture(NrTSingleton<NrTableTimeShopManager>.Instance.Get_MoneyTypeTextureName((eTIMESHOP_MONEYTYPE)_pTimeShopItem.m_nMoneyType));
		this.m_Label_Price.Text = _pTimeShopItem.m_lPrice.ToString();
		this.m_Button_Price.Data = _pTimeShopItem;
		this.m_Label_Price.Visible = true;
		this.m_Button_Price.Visible = true;
		this.m_DrawTexture_Won.Visible = true;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBuy_TimeShopItemByIDX(_pTimeShopItem.m_lIdx))
		{
			this.m_Button_Price.SetEnabled(false);
			this.m_Label_Price.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3341");
			this.m_DrawTexture_Won.Visible = false;
		}
		byte b = 6;
		int level = 50;
		string strSolKind = string.Empty;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_i32CharKind);
		if (charKindInfo != null)
		{
			strSolKind = charKindInfo.GetCode();
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(_pTimeShopItem.m_nItemUnique);
		if (itemInfo != null)
		{
			b = (byte)(itemInfo.m_nParam[1] + 1);
			if (b <= 1)
			{
				b = NrTSingleton<ItemManager>.Instance.GetTopGrade_GroupSolTicket((long)_pTimeShopItem.m_nItemUnique, strSolKind);
			}
			if (b > 15)
			{
				b = 15;
			}
		}
		else
		{
			List<SOL_GUIDE> value = NrTSingleton<NrTableSolGuideManager>.Instance.GetValue();
			foreach (SOL_GUIDE current in value)
			{
				if (current.m_i32CharKind == _i32CharKind)
				{
					b = (byte)current.m_iSolGrade;
				}
			}
		}
		if (charKindInfo != null)
		{
			level = (int)charKindInfo.GetGradeMaxLevel((short)(b - 1));
		}
		this.SetSolKind(_i32CharKind, b, level);
		this.m_eParentUI = G_ID.TIMESHOP_DLG;
	}
}
