using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class SolDetail_Info_Dlg : Form
{
	public enum eSOLTOOLBAR
	{
		eSOLGUILDE,
		eEIEMENTSOL,
		eLEGENDELEMENTSOL,
		eMAX_TOOLBAR
	}

	private enum eElement_MsgType
	{
		eElement_NONE,
		eElement_NOTSOL,
		eElement_NEEDEXP,
		eElement_NEEDGRADE,
		eElement_SOLHEIGHT,
		eElement_OK
	}

	private SolSlotData m_SelectSlotData;

	private MaterialSol m_MaterialSol = new MaterialSol();

	private bool m_bLayer3Show;

	private Button m_Button_MovieBtn;

	private Label m_Label_Movie;

	private Toolbar m_Toolbar;

	private Button m_ButtonSkillIcon;

	private SolDetailDlgTool m_SolInterfaceTool = new SolDetailDlgTool();

	private Label m_Label_Stats_str2;

	private Label m_Label_Stats_dex2;

	private Label m_Label_Stats_vit2;

	private Label m_Label_Stats_int2;

	private ScrollLabel m_ScrollLabel_info;

	private Button m_Button_ok;

	private Label m_Label_Reincarnate;

	private Button m_Button_Help;

	private NewListBox m_NewListBox_Reincarnate;

	private Button m_Button_Reincarnate;

	private Label m_Label_Gold;

	private Label m_Label_Notice;

	private DrawTexture m_DrawTexture_Item;

	private Label m_Label_Essence;

	private Button m_Button_Legend;

	private DrawTexture m_DrawTexture_Time;

	private Label m_Label_Time;

	private SolDetail_Info_Dlg.eElement_MsgType[] m_eElement_Msg = new SolDetail_Info_Dlg.eElement_MsgType[5];

	private ElementSol m_Element_SolID = new ElementSol();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "SolGuide/DLG_SolDetail_Info", G_ID.SOLDETAIL_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_Toolbar = (base.GetControl("ToolBar_ToolBar28") as Toolbar);
		this.m_Toolbar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1648");
		UIPanelTab expr_44 = this.m_Toolbar.Control_Tab[0];
		expr_44.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_44.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_Toolbar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1645");
		UIPanelTab expr_93 = this.m_Toolbar.Control_Tab[1];
		expr_93.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_93.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_Toolbar.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2852");
		UIPanelTab expr_E2 = this.m_Toolbar.Control_Tab[2];
		expr_E2.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_E2.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_SolInterfaceTool.m_Label_Character_Name = (base.GetControl("Label_character_name") as Label);
		this.m_SolInterfaceTool.m_Label_SeasonNum = (base.GetControl("Label_SeasonNum") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Rank2 = (base.GetControl("Label_rank2") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Character = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_Button_MovieBtn = (base.GetControl("Button_MovieBtn") as Button);
		this.m_Button_MovieBtn.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnIntroMovieButton));
		this.m_Button_MovieBtn.Visible = false;
		this.m_Label_Movie = (base.GetControl("LB_Movie") as Label);
		this.m_Label_Movie.Visible = false;
		this.m_SolInterfaceTool.m_DrawTexture_Event = (base.GetControl("DrawTexture_Event") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_EventDate = (base.GetControl("Label_EventDate") as Label);
		for (int i = 0; i < 2; i++)
		{
			this.m_SolInterfaceTool.m_Lebel_EventHero[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (i + 1).ToString())) as Label);
		}
		this.m_SolInterfaceTool.m_DrawTextureWeapon = (base.GetControl("DT_WEAPON") as DrawTexture);
		this.m_SolInterfaceTool.m_DrawTextureSkillIcon = (base.GetControl("DT_SKILLICON") as DrawTexture);
		this.m_ButtonSkillIcon = (base.GetControl("DT_SKILLBUTTON") as Button);
		this.m_ButtonSkillIcon.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSkillIcon));
		this.m_Label_Stats_str2 = (base.GetControl("Label_stats_str2") as Label);
		this.m_Label_Stats_dex2 = (base.GetControl("Label_stats_dex2") as Label);
		this.m_Label_Stats_vit2 = (base.GetControl("Label_stats_vit2") as Label);
		this.m_Label_Stats_int2 = (base.GetControl("Label_stats_int2") as Label);
		this.m_ScrollLabel_info = (base.GetControl("ScrollLabel_info") as ScrollLabel);
		this.m_Button_ok = (base.GetControl("btn_ok") as Button);
		this.m_Button_ok.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.m_Label_Reincarnate = (base.GetControl("Label_Reincarnate") as Label);
		this.m_Button_Help = (base.GetControl("Button_Help") as Button);
		this.m_Button_Help.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickHelp));
		this.m_NewListBox_Reincarnate = (base.GetControl("NewListBox_Reincarnate") as NewListBox);
		this.m_NewListBox_Reincarnate.touchScroll = false;
		this.m_Button_Reincarnate = (base.GetControl("btn_Reincarnate") as Button);
		this.m_Button_Reincarnate.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickReincarnate));
		this.m_Button_Reincarnate.SetEnabled(false);
		this.m_Label_Gold = (base.GetControl("Label_Gold2") as Label);
		this.m_Label_Notice = (base.GetControl("Label_Label38") as Label);
		this.m_Label_Essence = (base.GetControl("Label_Essence2") as Label);
		this.m_Button_Legend = (base.GetControl("btn_Legend") as Button);
		this.m_Button_Legend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickLegendReincarnate));
		this.m_Button_Legend.SetEnabled(false);
		this.m_DrawTexture_Item = (base.GetControl("DrawTexture_EssenceIcon") as DrawTexture);
		this.m_DrawTexture_Time = (base.GetControl("DT_LegendTimeBg") as DrawTexture);
		this.m_Label_Time = (base.GetControl("Label_LegendTime") as Label);
		this.m_Toolbar.SetSelectTabIndex(0);
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
		this.m_bLayer3Show = false;
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void SetSolKind(SolSlotData SlotData)
	{
		if (SlotData == null)
		{
			return;
		}
		this.m_SelectSlotData = SlotData;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_SelectSlotData.i32KindInfo);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		this.m_SolInterfaceTool.SetCharImg(this.m_SelectSlotData.bSolGrade - 1);
		this.m_SolInterfaceTool.SetHeroEventLabel(this.m_SelectSlotData.bSolGrade);
		this.m_SolInterfaceTool.SetSkillIcon();
		this.m_SolInterfaceTool.m_Label_Rank2.Visible = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
		{
			if (charKindInfo.IsATB(1L))
			{
				this.m_SolInterfaceTool.m_DrawTexture_rank.Visible = false;
			}
			else
			{
				this.m_SolInterfaceTool.m_DrawTexture_rank.Visible = true;
			}
		}
		else
		{
			this.m_SolInterfaceTool.m_DrawTexture_rank.Visible = true;
		}
		this.m_Button_MovieBtn.data = this.m_SelectSlotData.i32KindInfo;
		this.m_Button_MovieBtn.Visible = false;
		this.m_Label_Movie.Visible = false;
		if (charKindInfo.GetCHARKIND_INFO().SOLINTRO != "0")
		{
			this.m_Button_MovieBtn.Visible = true;
			this.m_Label_Movie.Visible = true;
		}
		BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = charKindInfo.GetCHARKIND_SOLGRADEINFO((int)(this.m_SelectSlotData.bSolGrade - 1));
		if (cHARKIND_SOLGRADEINFO != null)
		{
			int num = charKindInfo.GetGradePlusSTR((int)(this.m_SelectSlotData.bSolGrade - 1));
			int num2 = charKindInfo.GetGradePlusDEX((int)(this.m_SelectSlotData.bSolGrade - 1));
			int num3 = charKindInfo.GetGradePlusINT((int)(this.m_SelectSlotData.bSolGrade - 1));
			int num4 = charKindInfo.GetGradePlusVIT((int)(this.m_SelectSlotData.bSolGrade - 1));
			num += charKindInfo.GetIncSTR((int)(this.m_SelectSlotData.bSolGrade - 1), (int)charKindInfo.GetGradeMaxLevel((short)this.m_SelectSlotData.bSolGrade));
			num2 += charKindInfo.GetIncDEX((int)(this.m_SelectSlotData.bSolGrade - 1), (int)charKindInfo.GetGradeMaxLevel((short)this.m_SelectSlotData.bSolGrade));
			num3 += charKindInfo.GetIncINT((int)(this.m_SelectSlotData.bSolGrade - 1), (int)charKindInfo.GetGradeMaxLevel((short)this.m_SelectSlotData.bSolGrade));
			num4 += charKindInfo.GetIncVIT((int)(this.m_SelectSlotData.bSolGrade - 1), (int)charKindInfo.GetGradeMaxLevel((short)this.m_SelectSlotData.bSolGrade));
			this.m_Label_Stats_str2.SetText(num.ToString());
			this.m_Label_Stats_dex2.SetText(num2.ToString());
			this.m_Label_Stats_vit2.SetText(num4.ToString());
			this.m_Label_Stats_int2.SetText(num3.ToString());
		}
		else
		{
			this.m_Label_Stats_str2.SetText("0");
			this.m_Label_Stats_dex2.SetText("0");
			this.m_Label_Stats_vit2.SetText("0");
			this.m_Label_Stats_int2.SetText("0");
		}
		this.m_ScrollLabel_info.SetScrollLabel(charKindInfo.GetDesc());
		bool flag = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsElementKind(charKindInfo.GetCharKind()))
		{
			if (!NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindAlchemy(charKindInfo.GetCharKind()))
			{
				this.m_Toolbar.Control_Tab[1].Visible = false;
			}
			else
			{
				this.m_Toolbar.Control_Tab[1].Visible = true;
				flag = true;
			}
			if (!NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindLegend(charKindInfo.GetCharKind()))
			{
				this.m_Toolbar.Control_Tab[2].Visible = false;
			}
			else
			{
				this.m_Toolbar.Control_Tab[2].Visible = true;
				flag = true;
			}
			if (flag && this.m_MaterialSol.i32BaseCharKind != charKindInfo.GetCharKind())
			{
				GS_ELEMENTMATERIAL_REQ gS_ELEMENTMATERIAL_REQ = new GS_ELEMENTMATERIAL_REQ();
				gS_ELEMENTMATERIAL_REQ.i32BaseCharKind = SlotData.i32KindInfo;
				SendPacket.GetInstance().SendObject(1842, gS_ELEMENTMATERIAL_REQ);
			}
		}
	}

	public void SetElEmentMaterial(int i32BaseCharKind, byte i8BaseGrade, int[] i32MaterialCharKind, byte[] i8MaterialGrade, long i64Money, byte i8LegendBaseGrade, int[] i32LegendMaterialCharKind, byte[] i8LegendMaterialGrade, long i64LegendMoney, int i32LegendItemUnique, int i32LegendEssencs)
	{
		if (this.m_SelectSlotData.i32KindInfo == i32BaseCharKind)
		{
			this.m_MaterialSol.Set(i32BaseCharKind, i8BaseGrade, i32MaterialCharKind, i8MaterialGrade, i64Money, i8LegendBaseGrade, i32LegendMaterialCharKind, i8LegendMaterialGrade, i64LegendMoney, i32LegendItemUnique, i32LegendEssencs);
		}
	}

	private void OnClickOK(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		base.CloseForm(obj);
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
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false);
		}
	}

	private void OnClickSkillIcon(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.m_SelectSlotData == null || this.m_SelectSlotData.i32SkillUnique == 0)
		{
			return;
		}
		SolDetail_Skill_Dlg solDetail_Skill_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_SKILLICON_DLG) as SolDetail_Skill_Dlg;
		if (solDetail_Skill_Dlg != null)
		{
			solDetail_Skill_Dlg.SetSkillData(this.m_SelectSlotData.i32SkillUnique, this.m_SelectSlotData.i32SkillText);
		}
	}

	private void SetSolGuildeLayerShow()
	{
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
		this.m_bLayer3Show = false;
		this.SetSolKind(this.m_SelectSlotData);
	}

	private void SetElementLayerShow()
	{
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, false);
		this.m_bLayer3Show = false;
		this.SetElementGui();
	}

	private void SetLegendElementLayerShow()
	{
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, true);
		this.m_bLayer3Show = false;
		this.SetLegendElementGui();
	}

	private void ClickToolbar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		if (uIPanelTab.panel.index == 1)
		{
			this.m_Toolbar.SetSelectTabIndex(uIPanelTab.panel.index);
			this.SetElementLayerShow();
		}
		else if (uIPanelTab.panel.index == 2)
		{
			this.m_Toolbar.SetSelectTabIndex(uIPanelTab.panel.index);
			this.SetLegendElementLayerShow();
		}
		else
		{
			this.m_Toolbar.SetSelectTabIndex(0);
			this.SetSolGuildeLayerShow();
		}
	}

	public void SetTabButtonHide(SolDetail_Info_Dlg.eSOLTOOLBAR eSolToolBar)
	{
		if (SolDetail_Info_Dlg.eSOLTOOLBAR.eEIEMENTSOL > eSolToolBar || SolDetail_Info_Dlg.eSOLTOOLBAR.eMAX_TOOLBAR <= eSolToolBar)
		{
			return;
		}
		this.m_Toolbar.Control_Tab[(int)eSolToolBar].Visible = false;
	}

	public void SetElementGui()
	{
		this.m_Button_Reincarnate.SetEnabled(false);
		this.m_NewListBox_Reincarnate.Clear();
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_SelectSlotData.i32KindInfo);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (this.m_SolInterfaceTool == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		if (this.m_MaterialSol == null)
		{
			TsLog.LogOnlyEditor("!!!! m_MaterialSol ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (this.m_MaterialSol.i32BaseCharKind != this.m_SelectSlotData.i32KindInfo)
		{
			this.m_MaterialSol.Init();
		}
		byte b = 0;
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			b = this.m_MaterialSol.i8BaseGrade;
		}
		if (b < 0)
		{
			b = 0;
		}
		this.m_SolInterfaceTool.m_DrawTexture_rank.Visible = true;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.m_SelectSlotData.i32KindInfo, (int)(b - 1));
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.m_SelectSlotData.i32KindInfo, (int)(b - 1));
		if (0 < legendType)
		{
			this.m_SolInterfaceTool.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.m_SolInterfaceTool.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
		if (this.m_SelectSlotData.i32KindInfo == 0)
		{
			this.m_Toolbar.Control_Tab[1].Visible = false;
			this.m_Toolbar.Control_Tab[2].Visible = false;
			return;
		}
		this.m_SolInterfaceTool.SetSeason(b);
		this.m_SolInterfaceTool.SetHeroEventLabel(b);
		int num = 0;
		byte bCharRank = 0;
		for (int i = 0; i < 5; i++)
		{
			if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
			{
				this.m_MaterialSol.GetCharData((byte)i, ref num, ref bCharRank);
			}
			this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NONE;
			if (num != 0)
			{
				NewListItem item = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true);
				this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
				this.SetReincarnateListBox(ref item, i, num, bCharRank, false);
				this.m_NewListBox_Reincarnate.Add(item);
			}
		}
		this.m_NewListBox_Reincarnate.RepositionItems();
		bool flag = this.SetButtonReincarnate();
		this.m_Button_Reincarnate.SetEnabled(flag);
		if (flag)
		{
			num = 0;
			bCharRank = 0;
			this.m_NewListBox_Reincarnate.Clear();
			for (int j = 0; j < 5; j++)
			{
				if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
				{
					this.m_MaterialSol.GetCharData((byte)j, ref num, ref bCharRank);
				}
				this.m_eElement_Msg[j] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NONE;
				if (num != 0)
				{
					NewListItem item2 = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true);
					this.m_eElement_Msg[j] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
					this.SetReincarnateListBox(ref item2, j, num, bCharRank, true);
					this.m_NewListBox_Reincarnate.Add(item2);
				}
			}
			this.m_NewListBox_Reincarnate.RepositionItems();
		}
		long num2 = 0L;
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			num2 = this.m_MaterialSol.i64Money;
		}
		this.m_Label_Gold.SetText(ANNUALIZED.Convert(num2));
	}

	private void SetReincarnateListBox(ref NewListItem item, int i, int i32CharKind, byte bCharRank, bool bElement)
	{
		string text = string.Empty;
		NkSoldierInfo battleSolDataCheck = this.GetBattleSolDataCheck(ref this.m_eElement_Msg[i], i32CharKind, bCharRank);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!!!!!!!! SolGuild - Element CharKind " + i32CharKind + " Error");
			return;
		}
		item.SetListItemData(8, false);
		item.SetListItemData(9, false);
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = i32CharKind;
		nkListSolInfo.SolGrade = (int)(bCharRank - 1);
		item.SetListItemData(3, nkListSolInfo, null, null, null);
		item.SetListItemData(4, charKindInfo.GetName(), null, null, null);
		text = this.GetElementSolMsg(this.m_eElement_Msg[i]);
		item.SetListItemData(5, text, null, null, null);
		if (this.m_eElement_Msg[i] == SolDetail_Info_Dlg.eElement_MsgType.eElement_OK || this.m_eElement_Msg[i] == SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT)
		{
			item.SetListItemData(0, true);
			item.SetListItemData(1, false);
			item.SetListItemData(7, battleSolDataCheck.GetListSolInfo(false), null, null, null);
		}
		else
		{
			item.SetListItemData(0, false);
			item.SetListItemData(1, true);
			if (battleSolDataCheck == null)
			{
				item.SetListItemData(7, false);
				return;
			}
			item.SetListItemData(7, battleSolDataCheck.GetListSolInfo(false), null, null, null);
		}
		if (bElement)
		{
			bool flag = false;
			for (int j = 0; j < 6; j++)
			{
				if (battleSolDataCheck.GetEquipItemInfo() != null)
				{
					ITEM item2 = battleSolDataCheck.GetEquipItemInfo().m_kItem[j].GetItem();
					if (item2 == null)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
						{
							i32CharKind,
							j
						});
					}
					else if (item2.m_nItemUnique != 0)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				item.SetListItemData(5, false);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
				item.SetListItemData(8, true);
				item.SetListItemData(9, true);
				item.SetListItemData(9, text, battleSolDataCheck, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
			}
			else if (battleSolDataCheck.GetFriendPersonID() != 0L)
			{
				item.SetListItemData(5, false);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
				item.SetListItemData(8, true);
				item.SetListItemData(9, true);
				item.SetListItemData(9, text, battleSolDataCheck, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
			}
		}
		item.Data = battleSolDataCheck.GetSolID();
	}

	private void OnClickHelp(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		this.m_bLayer3Show = !this.m_bLayer3Show;
		base.SetShowLayer(3, this.m_bLayer3Show);
		if (this.m_bLayer3Show)
		{
			base.SetLayerZ(3, -1f);
		}
		if (this.m_Toolbar.SeletedToolIndex == 2)
		{
			this.m_Label_Notice.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2851"));
		}
		else
		{
			this.m_Label_Notice.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1816"));
		}
	}

	private NkSoldierInfo GetBattleSolDataCheck(ref SolDetail_Info_Dlg.eElement_MsgType eElement_Msg, int i32CharKind, byte bCharGrade)
	{
		NkSoldierInfo result = null;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return result;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null && current.GetSolPosType() == 0 && current.GetCharKind() == i32CharKind)
			{
				this.GetSolGradeCheck(ref result, ref eElement_Msg, current, bCharGrade - 1);
			}
		}
		return result;
	}

	private void GetSolGradeCheck(ref NkSoldierInfo pkSolinfo, ref SolDetail_Info_Dlg.eElement_MsgType eElement_Msg, NkSoldierInfo pkReadySolinfo, byte bCharGrade)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(pkReadySolinfo.GetCharKind()))
		{
			eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
			TsLog.LogOnlyEditor("!!!! CONTENTLIMIT SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (pkReadySolinfo.GetMaxSkillLevel())
		{
			if (pkReadySolinfo.GetGrade() >= bCharGrade)
			{
				if (pkReadySolinfo.GetGrade() == bCharGrade)
				{
					if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_OK)
					{
						if (pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
						{
							pkSolinfo = pkReadySolinfo;
						}
					}
					else
					{
						eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_OK;
						pkSolinfo = pkReadySolinfo;
					}
				}
				else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE)
				{
					eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT;
					pkSolinfo = pkReadySolinfo;
				}
				else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
				{
					pkSolinfo = pkReadySolinfo;
				}
			}
			else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE)
			{
				eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE;
				pkSolinfo = pkReadySolinfo;
			}
			else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
			{
				pkSolinfo = pkReadySolinfo;
			}
		}
		else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP)
		{
			eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP;
			pkSolinfo = pkReadySolinfo;
		}
		else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
		{
			pkSolinfo = pkReadySolinfo;
		}
	}

	private string GetElementSolMsg(SolDetail_Info_Dlg.eElement_MsgType eType)
	{
		string result = string.Empty;
		switch (eType)
		{
		case SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1649");
			break;
		case SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1650");
			break;
		case SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1651");
			break;
		case SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1691");
			break;
		case SolDetail_Info_Dlg.eElement_MsgType.eElement_OK:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1575");
			break;
		}
		return result;
	}

	private void OnClickReincarnate(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1706");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("164");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnReincarnateOK), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
	}

	public void OnReincarnateOK(object a_oObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int num = 0;
		byte bCharGrade = 0;
		long[] array = new long[5];
		for (int i = 0; i < 5; i++)
		{
			if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
			{
				this.m_MaterialSol.GetCharData((byte)i, ref num, ref bCharGrade);
			}
			this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
			if (num == 0)
			{
				this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_OK;
				array[i] = 0L;
			}
			else
			{
				NkSoldierInfo battleSolDataCheck = this.GetBattleSolDataCheck(ref this.m_eElement_Msg[i], num, bCharGrade);
				if (battleSolDataCheck == null)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1649"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
					return;
				}
				if (battleSolDataCheck.GetSolPosType() != 0)
				{
					if (battleSolDataCheck.GetSolPosType() == 2 || battleSolDataCheck.GetSolPosType() == 6)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("384"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						return;
					}
					TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : SOL Pos{1}  ==  Char Sol Pos Type", new object[]
					{
						num,
						battleSolDataCheck.GetSolPosType()
					});
					return;
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						ITEM item = battleSolDataCheck.GetEquipItemInfo().m_kItem[j].GetItem();
						if (item == null)
						{
							TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
							{
								num,
								j
							});
							return;
						}
						if (item.m_nItemUnique != 0)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("383"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
							return;
						}
					}
					if (battleSolDataCheck.GetFriendPersonID() != 0L)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Set FriendSOLHELP ", new object[]
						{
							battleSolDataCheck.GetName()
						});
						return;
					}
					array[i] = battleSolDataCheck.GetSolID();
				}
			}
		}
		long num2 = 0L;
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			num2 = this.m_MaterialSol.i64Money;
		}
		if (num2 > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		GS_ELEMENT_SOL_GET_REQ gS_ELEMENT_SOL_GET_REQ = new GS_ELEMENT_SOL_GET_REQ();
		gS_ELEMENT_SOL_GET_REQ.i64PersonID = kMyCharInfo.m_PersonID;
		gS_ELEMENT_SOL_GET_REQ.i32CharKind = this.m_SelectSlotData.i32KindInfo;
		gS_ELEMENT_SOL_GET_REQ.i64SolID = array;
		SendPacket.GetInstance().SendObject(1838, gS_ELEMENT_SOL_GET_REQ);
	}

	private bool SetButtonReincarnate()
	{
		bool result = true;
		for (int i = 0; i < 5; i++)
		{
			if (this.m_eElement_Msg[i] >= SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL && this.m_eElement_Msg[i] <= SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE)
			{
				result = false;
			}
		}
		return result;
	}

	private void OnClickUnsetSolHelp(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)obj.Data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = nkSoldierInfo.GetFriendPersonID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = nkSoldierInfo.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = nkSoldierInfo.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
	}

	private void OnClickReleaseEquip(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = uIButton.Data as NkSoldierInfo;
		if (nkSoldierInfo != null)
		{
			Protocol_Item.Send_EquipSol_InvenEquip_All(nkSoldierInfo);
		}
	}

	public void ElementSolSuccess(SOLDIER_INFO pkSolInfo, bool bLegend)
	{
		SolElementSuccessDlg solElementSuccessDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLELEMENTSUCCESS_DLG) as SolElementSuccessDlg;
		if (solElementSuccessDlg != null)
		{
			if (bLegend)
			{
				solElementSuccessDlg.LoadLegendSolCompleteElement(pkSolInfo);
			}
			else
			{
				solElementSuccessDlg.LoadSolCompleteElement(pkSolInfo);
			}
		}
	}

	public void SetLegendElementGui()
	{
		this.m_Button_Legend.SetEnabled(false);
		this.m_NewListBox_Reincarnate.Clear();
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_SelectSlotData.i32KindInfo);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (this.m_SolInterfaceTool == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		if (this.m_MaterialSol == null)
		{
			TsLog.LogOnlyEditor("!!!! m_MaterialSol ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (this.m_MaterialSol.i32BaseCharKind != this.m_SelectSlotData.i32KindInfo)
		{
			this.m_MaterialSol.Init();
		}
		this.m_Label_Reincarnate.Visible = false;
		this.m_Button_Reincarnate.Visible = false;
		byte b = 0;
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			b = this.m_MaterialSol.i8LegendBaseGrade;
		}
		if (b < 0)
		{
			b = 0;
		}
		this.m_SolInterfaceTool.m_DrawTexture_rank.Visible = true;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.m_SelectSlotData.i32KindInfo, (int)(b - 1));
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.m_SelectSlotData.i32KindInfo, (int)(b - 1));
		if (0 < legendType)
		{
			this.m_SolInterfaceTool.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.m_SolInterfaceTool.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
		if (this.m_SelectSlotData.i32KindInfo == 0)
		{
			this.m_Toolbar.Control_Tab[1].Visible = false;
			this.m_Toolbar.Control_Tab[2].Visible = false;
			return;
		}
		this.m_SolInterfaceTool.SetSeason(b);
		this.m_SolInterfaceTool.SetHeroEventLabel(b);
		int num = 0;
		byte bCharRank = 0;
		this.m_Element_SolID.Init();
		for (int i = 0; i < 5; i++)
		{
			if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
			{
				this.m_MaterialSol.GetLegendCharData((byte)i, ref num, ref bCharRank);
			}
			this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NONE;
			if (num != 0)
			{
				NewListItem item = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true);
				this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
				this.SetLegendReincarnateListBox(ref item, i, num, bCharRank, false);
				this.m_NewListBox_Reincarnate.Add(item);
			}
		}
		this.m_NewListBox_Reincarnate.RepositionItems();
		if (this.m_MaterialSol.i32LegendItemUnique > 0)
		{
			this.m_DrawTexture_Item.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_MaterialSol.i32LegendItemUnique);
		}
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			long i64LegendMoney = this.m_MaterialSol.i64LegendMoney;
			int i32LegendEssencs = this.m_MaterialSol.i32LegendEssencs;
			this.m_Label_Gold.SetText(ANNUALIZED.Convert(i64LegendMoney));
			this.m_Label_Essence.SetText(ANNUALIZED.Convert(i32LegendEssencs));
		}
		long charSubData = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
		long curTime = PublicMethod.GetCurTime();
		bool flag = this.SetButtonReincarnate();
		if (curTime < charSubData)
		{
			flag = false;
			string text = string.Empty;
			long i64Time = charSubData - PublicMethod.GetCurTime();
			text = this.GetTimeToString(i64Time);
			this.m_Label_Time.SetText(text);
		}
		else
		{
			this.m_DrawTexture_Time.Visible = false;
			this.m_Label_Time.Visible = false;
		}
		this.m_Button_Reincarnate.SetEnabled(flag);
		if (flag)
		{
			num = 0;
			bCharRank = 0;
			this.m_NewListBox_Reincarnate.Clear();
			this.m_Element_SolID.Init();
			for (int j = 0; j < 5; j++)
			{
				if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
				{
					this.m_MaterialSol.GetLegendCharData((byte)j, ref num, ref bCharRank);
				}
				this.m_eElement_Msg[j] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NONE;
				if (num != 0)
				{
					NewListItem item2 = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true);
					this.m_eElement_Msg[j] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
					this.SetLegendReincarnateListBox(ref item2, j, num, bCharRank, true);
					this.m_NewListBox_Reincarnate.Add(item2);
				}
			}
			this.m_NewListBox_Reincarnate.RepositionItems();
			this.m_Button_Legend.SetEnabled(true);
		}
	}

	private void SetLegendReincarnateListBox(ref NewListItem item, int i, int i32CharKind, byte bCharRank, bool bElement)
	{
		string text = string.Empty;
		NkSoldierInfo legendBattleSolDataCheck = this.GetLegendBattleSolDataCheck(ref this.m_eElement_Msg[i], i32CharKind, bCharRank);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!!!!!!!! SolGuild - Element CharKind " + i32CharKind + " Error");
			return;
		}
		item.SetListItemData(8, false);
		item.SetListItemData(9, false);
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = i32CharKind;
		nkListSolInfo.SolGrade = (int)(bCharRank - 1);
		item.SetListItemData(3, nkListSolInfo, null, null, null);
		item.SetListItemData(4, charKindInfo.GetName(), null, null, null);
		text = this.GetElementSolMsg(this.m_eElement_Msg[i]);
		item.SetListItemData(5, text, null, null, null);
		if (this.m_eElement_Msg[i] == SolDetail_Info_Dlg.eElement_MsgType.eElement_OK || this.m_eElement_Msg[i] == SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT)
		{
			item.SetListItemData(0, true);
			item.SetListItemData(1, false);
			if (legendBattleSolDataCheck == null)
			{
				item.SetListItemData(7, false);
				return;
			}
			item.SetListItemData(7, legendBattleSolDataCheck.GetListSolInfo(false), null, null, null);
			this.m_Element_SolID.SetLegendSol(i32CharKind, legendBattleSolDataCheck.m_kBase.SolID);
		}
		else
		{
			item.SetListItemData(0, false);
			item.SetListItemData(1, true);
			if (legendBattleSolDataCheck == null)
			{
				item.SetListItemData(7, false);
				return;
			}
			item.SetListItemData(7, legendBattleSolDataCheck.GetListSolInfo(false), null, null, null);
			this.m_Element_SolID.SetLegendSol(i32CharKind, legendBattleSolDataCheck.m_kBase.SolID);
		}
		if (bElement)
		{
			bool flag = false;
			for (int j = 0; j < 6; j++)
			{
				if (legendBattleSolDataCheck.GetEquipItemInfo() != null)
				{
					ITEM item2 = legendBattleSolDataCheck.GetEquipItemInfo().m_kItem[j].GetItem();
					if (item2 == null)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
						{
							i32CharKind,
							j
						});
					}
					else if (item2.m_nItemUnique != 0)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				item.SetListItemData(5, false);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
				item.SetListItemData(8, true);
				item.SetListItemData(9, true);
				item.SetListItemData(9, text, legendBattleSolDataCheck, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
			}
			else if (legendBattleSolDataCheck.GetFriendPersonID() != 0L)
			{
				item.SetListItemData(5, false);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
				item.SetListItemData(8, true);
				item.SetListItemData(9, true);
				item.SetListItemData(9, text, legendBattleSolDataCheck, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
			}
		}
		item.Data = legendBattleSolDataCheck.GetSolID();
	}

	private void OnClickLegendReincarnate(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1706");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("164");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnLegendReincarnateOK), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
	}

	public void OnLegendReincarnateOK(object a_oObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int num = 0;
		byte bCharGrade = 0;
		long[] array = new long[5];
		this.m_Element_SolID.Init();
		for (int i = 0; i < 5; i++)
		{
			if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
			{
				this.m_MaterialSol.GetLegendCharData((byte)i, ref num, ref bCharGrade);
			}
			this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
			if (num == 0)
			{
				this.m_eElement_Msg[i] = SolDetail_Info_Dlg.eElement_MsgType.eElement_OK;
				array[i] = 0L;
			}
			else
			{
				NkSoldierInfo legendBattleSolDataCheck = this.GetLegendBattleSolDataCheck(ref this.m_eElement_Msg[i], num, bCharGrade);
				if (legendBattleSolDataCheck == null)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1649"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
					return;
				}
				this.m_Element_SolID.SetLegendSol(num, legendBattleSolDataCheck.m_kBase.SolID);
				if (legendBattleSolDataCheck.GetSolPosType() != 0)
				{
					if (legendBattleSolDataCheck.GetSolPosType() == 2 || legendBattleSolDataCheck.GetSolPosType() == 6)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("384"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						return;
					}
					TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : SOL Pos{1}  ==  Char Sol Pos Type", new object[]
					{
						num,
						legendBattleSolDataCheck.GetSolPosType()
					});
					return;
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						ITEM item = legendBattleSolDataCheck.GetEquipItemInfo().m_kItem[j].GetItem();
						if (item == null)
						{
							TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
							{
								num,
								j
							});
							return;
						}
						if (item.m_nItemUnique != 0)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("383"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
							return;
						}
					}
					if (legendBattleSolDataCheck.GetFriendPersonID() != 0L)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Set FriendSOLHELP ", new object[]
						{
							legendBattleSolDataCheck.GetName()
						});
						return;
					}
					array[i] = legendBattleSolDataCheck.GetSolID();
				}
			}
		}
		long num2 = 0L;
		int num3 = 0;
		if (this.m_MaterialSol.i32BaseCharKind == this.m_SelectSlotData.i32KindInfo)
		{
			num2 = this.m_MaterialSol.i64LegendMoney;
			num3 = NkUserInventory.GetInstance().GetItemCnt(this.m_MaterialSol.i32LegendItemUnique);
		}
		if (num2 > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		if (num3 < this.m_MaterialSol.i32LegendEssencs)
		{
			string empty = string.Empty;
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_MaterialSol.i32LegendItemUnique);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
				"target",
				itemNameByItemUnique
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		GS_ELEMENT_LEGENDSOL_GET_REQ gS_ELEMENT_LEGENDSOL_GET_REQ = new GS_ELEMENT_LEGENDSOL_GET_REQ();
		gS_ELEMENT_LEGENDSOL_GET_REQ.i64PersonID = kMyCharInfo.m_PersonID;
		gS_ELEMENT_LEGENDSOL_GET_REQ.i32CharKind = this.m_SelectSlotData.i32KindInfo;
		gS_ELEMENT_LEGENDSOL_GET_REQ.i64SolID = array;
		SendPacket.GetInstance().SendObject(1840, gS_ELEMENT_LEGENDSOL_GET_REQ);
	}

	private NkSoldierInfo GetLegendBattleSolDataCheck(ref SolDetail_Info_Dlg.eElement_MsgType eElement_Msg, int i32CharKind, byte bCharGrade)
	{
		NkSoldierInfo result = null;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return result;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null && current.GetSolPosType() == 0 && current.GetCharKind() == i32CharKind && !this.m_Element_SolID.GetLegendSolCheck(i32CharKind, current.m_kBase.SolID))
			{
				this.GetLegendSolGradeCheck(ref result, ref eElement_Msg, current, bCharGrade - 1);
			}
		}
		return result;
	}

	private void GetLegendSolGradeCheck(ref NkSoldierInfo pkSolinfo, ref SolDetail_Info_Dlg.eElement_MsgType eElement_Msg, NkSoldierInfo pkReadySolinfo, byte bCharGrade)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(pkReadySolinfo.GetCharKind()))
		{
			eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NOTSOL;
			TsLog.LogOnlyEditor("!!!! CONTENTLIMIT SOL CHARKIND ERROR {0}" + this.m_SelectSlotData.i32KindInfo + " !!");
			return;
		}
		if (pkReadySolinfo.GetMaxSkillLevel())
		{
			if (pkReadySolinfo.GetGrade() >= bCharGrade)
			{
				if (pkReadySolinfo.GetGrade() == bCharGrade)
				{
					if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_OK)
					{
						if (pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
						{
							pkSolinfo = pkReadySolinfo;
						}
					}
					else
					{
						eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_OK;
						pkSolinfo = pkReadySolinfo;
					}
				}
				else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE)
				{
					eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT;
					pkSolinfo = pkReadySolinfo;
				}
				else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_SOLHEIGHT && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
				{
					pkSolinfo = pkReadySolinfo;
				}
			}
			else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE)
			{
				eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE;
				pkSolinfo = pkReadySolinfo;
			}
			else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDGRADE && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
			{
				pkSolinfo = pkReadySolinfo;
			}
		}
		else if (eElement_Msg < SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP)
		{
			eElement_Msg = SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP;
			pkSolinfo = pkReadySolinfo;
		}
		else if (eElement_Msg == SolDetail_Info_Dlg.eElement_MsgType.eElement_NEEDEXP && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
		{
			pkSolinfo = pkReadySolinfo;
		}
	}

	public string GetTimeToString(long i64Time)
	{
		string empty = string.Empty;
		if (i64Time > 0L)
		{
			long totalDayFromSec = PublicMethod.GetTotalDayFromSec(i64Time);
			long hourFromSec = PublicMethod.GetHourFromSec(i64Time);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2858"),
				"day",
				totalDayFromSec,
				"hour",
				hourFromSec
			});
		}
		return empty;
	}

	public void GetSelectToolBarRefresh()
	{
		int seletedToolIndex = this.m_Toolbar.SeletedToolIndex;
		if (seletedToolIndex == 1)
		{
			this.SetElementLayerShow();
		}
		else if (seletedToolIndex == 2)
		{
			this.SetLegendElementLayerShow();
		}
	}
}
