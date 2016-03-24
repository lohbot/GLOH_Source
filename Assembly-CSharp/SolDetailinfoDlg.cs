using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using SERVICE;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolDetailinfoDlg : Form
{
	private SolDetailDlgTool m_SolInterfaceTool = new SolDetailDlgTool();

	private Label SolSTR;

	private Label SolDEX;

	private Label SolVIT;

	private Label SolINT;

	private Label TradeCount;

	private Label TradeCountName;

	private Label m_lbSoldierSpec;

	private ScrollLabel SolExplain;

	private Button IntroMovieButton;

	private Button m_btnClose;

	private Label IntroMoveieText;

	private Button UserPortraitRefresh;

	private Button UserPortraitChange;

	private Button m_HelpButton;

	private HorizontalSlider m_InitiativeValueSlider;

	private Button m_InitiativeValue_text;

	private Button m_InitiativeValue_Minus;

	private Button m_InitiativeValue_Add;

	private Label m_InitiativeValue_Value;

	private Button m_btLock;

	private DrawTexture m_dtLock;

	private CheckBox m_cOnlySkill;

	private Button m_btCostume;

	private Label m_lbCostume;

	private Button m_btnSolPreview;

	private float m_oldInitiativeValue_f;

	private int m_SaveInitiativeValue;

	private bool bLeaderSol;

	private float m_fRefreshTime;

	private NkSoldierInfo pkSolinfo;

	private string m_strText = string.Empty;

	private string m_strBaseTextColor = string.Empty;

	private string m_strIncTextColor = string.Empty;

	private float m_fLockDelayTime;

	private int m_selectedSolCostumeUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolDetailinfo", G_ID.SOLDETAILINFO_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_SolInterfaceTool.m_Label_Character_Name = (base.GetControl("Label_character_name") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Character = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_SolInterfaceTool.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Rank2 = (base.GetControl("Label_rank2") as Label);
		this.SolSTR = (base.GetControl("Label_stats_str2") as Label);
		this.SolDEX = (base.GetControl("Label_stats_dex2") as Label);
		this.SolVIT = (base.GetControl("Label_stats_vit2") as Label);
		this.SolINT = (base.GetControl("Label_stats_int2") as Label);
		this.TradeCount = (base.GetControl("Label_stats_TradeNum2") as Label);
		this.TradeCountName = (base.GetControl("Label_stats_TradeNum") as Label);
		this.TradeCountName.SetText(string.Empty);
		this.SolExplain = (base.GetControl("ScrollLabel_info") as ScrollLabel);
		this.m_lbSoldierSpec = (base.GetControl("Label_soldierspec2") as Label);
		this.IntroMovieButton = (base.GetControl("Button_MovieBtn") as Button);
		this.IntroMovieButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnIntroMovieButton));
		this.IntroMovieButton.Visible = false;
		this.IntroMoveieText = (base.GetControl("LB_Movie") as Label);
		this.IntroMoveieText.Visible = false;
		this.UserPortraitRefresh = (base.GetControl("Button_Change_Img") as Button);
		this.UserPortraitRefresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnUserPortraitRefresh));
		this.UserPortraitRefresh.Visible = false;
		this.UserPortraitChange = (base.GetControl("Button_Change") as Button);
		this.UserPortraitChange.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickUserPortraitChange));
		this.m_SolInterfaceTool.m_DrawTexture_Event = (base.GetControl("DrawTexture_Event") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_EventDate = (base.GetControl("Label_EventDate") as Label);
		for (int i = 0; i < 2; i++)
		{
			this.m_SolInterfaceTool.m_Lebel_EventHero[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (i + 1).ToString())) as Label);
		}
		this.m_SolInterfaceTool.m_Label_SeasonNum = (base.GetControl("Label_SeasonNum") as Label);
		this.m_InitiativeValueSlider = (base.GetControl("HSlider_HSlider34") as HorizontalSlider);
		this.m_InitiativeValue_text = (base.GetControl("btn_HELP") as Button);
		this.m_InitiativeValue_text.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnInitiativeValueText));
		this.m_InitiativeValue_Minus = (base.GetControl("btn_MINUS") as Button);
		this.m_InitiativeValue_Minus.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnInitiativeValueMinus));
		this.m_InitiativeValue_Add = (base.GetControl("btn_PLUS") as Button);
		this.m_InitiativeValue_Add.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnInitiativeValueAdd));
		this.m_InitiativeValue_Value = (base.GetControl("Label_NUMBER") as Label);
		this.m_cOnlySkill = (base.GetControl("CheckBox_button") as CheckBox);
		this.m_cOnlySkill.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDetailOnlySkillCheckBox));
		this.m_btLock = (base.GetControl("Button_Lock01") as Button);
		this.m_btLock.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLock));
		this.m_btLock.Visible = false;
		this.m_dtLock = (base.GetControl("DrawTexture_Lock01") as DrawTexture);
		this.m_dtLock.Visible = false;
		this.m_strBaseTextColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
		this.m_strIncTextColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1104");
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_HelpButton.Visible = false;
		this.m_btCostume = (base.GetControl("BT_Costume") as Button);
		this.m_btCostume.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCostume));
		this.m_btCostume.Visible = false;
		this.m_lbCostume = (base.GetControl("LB_CostumeBT") as Label);
		this.m_lbCostume.Visible = false;
		this.m_btnClose = (base.GetControl("Close_Button") as Button);
		this.m_btnClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.m_btnSolPreview = (base.GetControl("Button_SolPreview") as Button);
		this.m_btnSolPreview.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_PreViewHero));
		this.m_btnSolPreview.Visible = false;
		this.InitData();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INFORMATION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void UserPortraitRefreshData(bool bShow)
	{
		this.UserPortraitRefresh.Visible = bShow;
		this.UserPortraitChange.Visible = bShow;
		this.m_HelpButton.Visible = bShow;
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		base.SetScreenCenter();
	}

	public void SetCostumeUnique(int costumeUnique)
	{
		this.m_selectedSolCostumeUnique = costumeUnique;
	}

	public void SetData(ref NkSoldierInfo solinfo)
	{
		if (solinfo == null)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.pkSolinfo = charPersonInfo.GetSoldierInfoFromSolID(solinfo.GetSolID());
		if (this.pkSolinfo == null)
		{
			return;
		}
		this.SetSolder();
		this.SetSolDetailInfo();
		this.bLeaderSol = solinfo.IsLeader();
		this.UserPortraitRefreshData(this.bLeaderSol);
	}

	private void ClickDetailOnlySkillCheckBox(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (checkBox.IsChecked())
		{
			if (!this.pkSolinfo.IsAtbCommonFlag(8L))
			{
				this.pkSolinfo.SetAtbCommonFlag(8L);
			}
		}
		else if (this.pkSolinfo.IsAtbCommonFlag(8L))
		{
			this.pkSolinfo.DelAtbCommonFlag(8L);
		}
	}

	public void SetDataNotMySol(ref NkSoldierInfo solinfo)
	{
		if (solinfo == null)
		{
			return;
		}
		this.pkSolinfo = solinfo;
		if (this.pkSolinfo == null)
		{
			return;
		}
		this.SetSolder();
		this.SetSolDetailInfo();
		this.bLeaderSol = solinfo.IsLeader();
		this.UserPortraitRefreshData(this.bLeaderSol);
	}

	private void SetSolDetailInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		this.SetCostumeButton();
		this.pkSolinfo.UpdateSoldierStatInfo();
		NrCharKindInfo charKindInfo = this.pkSolinfo.GetCharKindInfo();
		if (charKindInfo == null)
		{
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		this.m_SolInterfaceTool.SetHeroEventLabel(this.pkSolinfo.GetGrade() + 1);
		string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.m_selectedSolCostumeUnique);
		this.m_SolInterfaceTool.SetCharImg(this.pkSolinfo.GetGrade(), costumePortraitPath);
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
		SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
		sUBDATA_UNION.nSubData = this.pkSolinfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_STRDEX);
		SUBDATA_UNION sUBDATA_UNION2 = default(SUBDATA_UNION);
		sUBDATA_UNION2.nSubData = this.pkSolinfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_AWAKENING_VITINT);
		int statSTR = this.pkSolinfo.GetStatSTR();
		int statDEX = this.pkSolinfo.GetStatDEX();
		int statVIT = this.pkSolinfo.GetStatVIT();
		int statINT = this.pkSolinfo.GetStatINT();
		this.ShowBaseSolStatAwakening(this.SolSTR, statSTR, sUBDATA_UNION.n32SubData_0);
		this.ShowBaseSolStatAwakening(this.SolDEX, statDEX, sUBDATA_UNION.n32SubData_1);
		this.ShowBaseSolStatAwakening(this.SolVIT, statVIT, sUBDATA_UNION2.n32SubData_0);
		this.ShowBaseSolStatAwakening(this.SolINT, statINT, sUBDATA_UNION2.n32SubData_1);
		string text = string.Empty;
		if (charKindInfo.GetCHARKIND_ATTACKINFO().ATTACKTYPE == this.pkSolinfo.GetAttackInfo().ATTACKTYPE)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec1);
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec2);
		}
		this.m_lbSoldierSpec.SetText(text);
		this.SolExplain.SetScrollLabel(charKindInfo.GetDesc());
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE);
		this.TradeCount.Hide(true);
		if (value == 1)
		{
			this.TradeCount.Hide(true);
			this.TradeCountName.Hide(false);
			byte tradeRank = this.pkSolinfo.GetCharKindInfo().GetTradeRank((int)this.pkSolinfo.GetGrade());
			if (tradeRank == 0 || this.pkSolinfo.GetGrade() < tradeRank - 1)
			{
				if (tradeRank == 0)
				{
					this.TradeCountName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1980"));
				}
				else if (!this.pkSolinfo.IsAwakening())
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1979"),
						"count",
						tradeRank
					});
					this.TradeCountName.SetText(empty);
				}
				else
				{
					this.TradeCountName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2346"));
				}
			}
			else if (!this.pkSolinfo.IsAwakening())
			{
				this.TradeCountName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1206"));
				long solSubData = this.pkSolinfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT);
				this.TradeCount.SetText(solSubData.ToString());
				this.TradeCount.Hide(false);
			}
			else
			{
				this.TradeCountName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2346"));
			}
		}
		else
		{
			this.TradeCountName.Hide(true);
		}
		this.IntroMovieButton.Visible = false;
		this.IntroMoveieText.Visible = false;
		this.m_btnSolPreview.Visible = false;
		if (charKindInfo.IsATB(1L))
		{
			this.IntroMovieButton.Visible = true;
			this.IntroMoveieText.Visible = true;
		}
		else
		{
			this.m_btnSolPreview.Visible = true;
		}
		this.SetInitiativeValue();
		if (this.pkSolinfo.IsAtbCommonFlag(8L))
		{
			this.m_cOnlySkill.SetCheckState(1);
		}
		else
		{
			this.m_cOnlySkill.SetCheckState(0);
		}
	}

	private void SetInitiativeValue()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		this.m_InitiativeValue_Value.SetText(this.pkSolinfo.GetInitiativeValue().ToString());
		float num = (float)this.pkSolinfo.GetInitiativeValue() / 100f;
		this.m_InitiativeValueSlider.defaultValue = num;
		this.m_InitiativeValueSlider.Value = num;
		this.m_oldInitiativeValue_f = num;
		this.m_SaveInitiativeValue = this.pkSolinfo.GetInitiativeValue();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_oldInitiativeValue_f != this.m_InitiativeValueSlider.Value)
		{
			int saveInitiativeValue = (int)(this.m_InitiativeValueSlider.Value * 100f);
			this.m_InitiativeValue_Value.SetText(saveInitiativeValue.ToString());
			this.m_oldInitiativeValue_f = this.m_InitiativeValueSlider.Value;
			this.m_SaveInitiativeValue = saveInitiativeValue;
		}
		if (this.m_fLockDelayTime > 0f && this.m_fLockDelayTime < Time.time)
		{
			this.m_btLock.controlIsEnabled = true;
			this.m_fLockDelayTime = 0f;
		}
	}

	private void OnClickUserPortraitChange(IUIObject obj)
	{
		if (this.bLeaderSol)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				NrTSingleton<NkClientLogic>.Instance.SetOTPRequestInfo(eOTPRequestType.OTPREQ_CHARPORTRAIT);
			}
		}
		else
		{
			this.CloseForm(null);
		}
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INFORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		GS_SET_SOLDIER_INITIATIVE_REQ gS_SET_SOLDIER_INITIATIVE_REQ = new GS_SET_SOLDIER_INITIATIVE_REQ();
		gS_SET_SOLDIER_INITIATIVE_REQ.nSolID = this.pkSolinfo.GetSolID();
		gS_SET_SOLDIER_INITIATIVE_REQ.nInitiativeValue = this.m_SaveInitiativeValue;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_INITIATIVE_REQ, gS_SET_SOLDIER_INITIATIVE_REQ);
	}

	private void OnIntroMovieButton(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = this.pkSolinfo.GetCharKindInfo();
		if (charKindInfo == null)
		{
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

	private void OnUserPortraitRefresh(IUIObject obj)
	{
		float num = Time.time - this.m_fRefreshTime;
		if (num > 3f)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				GS_GETPORTRAIT_INFO_REQ gS_GETPORTRAIT_INFO_REQ = new GS_GETPORTRAIT_INFO_REQ();
				gS_GETPORTRAIT_INFO_REQ.bDataType = 0;
				gS_GETPORTRAIT_INFO_REQ.i64DataID = @char.GetPersonID();
				SendPacket.GetInstance().SendObject(1726, gS_GETPORTRAIT_INFO_REQ);
				this.m_fRefreshTime = Time.time;
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"));
		}
	}

	private void OnInitiativeValueText(IUIObject obj)
	{
		ExplainTooltipDlg explainTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPLAIN_TOOLTIP_DLG) as ExplainTooltipDlg;
		if (explainTooltipDlg != null)
		{
			explainTooltipDlg.SetExplainType(ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_SOLDETAILINFO, this);
		}
	}

	private void OnInitiativeValueMinus(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_SaveInitiativeValue > 0)
		{
			this.m_SaveInitiativeValue--;
			this.m_InitiativeValue_Value.SetText(this.m_SaveInitiativeValue.ToString());
			float num = (float)this.m_SaveInitiativeValue / 100f;
			this.m_InitiativeValueSlider.defaultValue = num;
			this.m_InitiativeValueSlider.Value = num;
			this.m_oldInitiativeValue_f = num;
		}
	}

	private void OnInitiativeValueAdd(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_SaveInitiativeValue < 100)
		{
			this.m_SaveInitiativeValue++;
			this.m_InitiativeValue_Value.SetText(this.m_SaveInitiativeValue.ToString());
			float num = (float)this.m_SaveInitiativeValue / 100f;
			this.m_InitiativeValueSlider.defaultValue = num;
			this.m_InitiativeValueSlider.Value = num;
			this.m_oldInitiativeValue_f = num;
		}
	}

	public void ShowBaseSolStatAwakening(Label lbStat, int iStat, int iAwakeningStat)
	{
		if (iAwakeningStat == 0)
		{
			this.m_strText = string.Format(" {0}{1}", this.m_strBaseTextColor, iStat);
		}
		else if (0 < iAwakeningStat)
		{
			iStat -= iAwakeningStat;
			this.m_strText = string.Format(" {0}{1}({2}+{3})", new object[]
			{
				this.m_strBaseTextColor,
				iStat,
				this.m_strIncTextColor,
				iAwakeningStat
			});
		}
		else
		{
			iStat += Math.Abs(iAwakeningStat);
			this.m_strText = string.Format(" {0}{1}({2}{3})", new object[]
			{
				this.m_strBaseTextColor,
				iStat,
				this.m_strIncTextColor,
				iAwakeningStat
			});
		}
		lbStat.SetText(this.m_strText);
	}

	private void ClickLock(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.pkSolinfo.IsAtbCommonFlag(1L))
		{
			this.pkSolinfo.DelAtbCommonFlag(1L);
		}
		else
		{
			this.pkSolinfo.SetAtbCommonFlag(1L);
		}
		this.SetLockDelay();
	}

	private void SetLockDelay()
	{
		this.m_btLock.controlIsEnabled = false;
		this.m_fLockDelayTime = Time.time + 1f;
	}

	public void SetSolder()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.pkSolinfo.IsLeader() || this.pkSolinfo.IsSolWarehouse())
		{
			this.m_btLock.Visible = false;
			this.m_dtLock.Visible = false;
		}
		else
		{
			this.m_btLock.Visible = true;
			this.m_dtLock.Visible = true;
			if (this.pkSolinfo.IsAtbCommonFlag(1L))
			{
				this.m_dtLock.SetTexture("Win_I_Lock04");
			}
			else
			{
				this.m_dtLock.SetTexture("Win_I_Lock03");
			}
		}
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Hero_Reincarnate.ToString());
		}
	}

	private void SetCostumeButton()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		bool visible = NrTSingleton<NrCharCostumeTableManager>.Instance.IsCostumeKind(this.pkSolinfo.GetCharKind());
		this.m_btCostume.Visible = visible;
		this.m_lbCostume.Visible = visible;
		if (NrTSingleton<ContentsLimitManager>.Instance.IsCostumeLimit())
		{
			this.m_btCostume.Visible = false;
			this.m_lbCostume.Visible = false;
		}
	}

	private void OnClickCostume(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		List<int> costumeKindList = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeKindList();
		if (!costumeKindList.Contains(this.pkSolinfo.GetCharKind()))
		{
			return;
		}
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null)
		{
			return;
		}
		costumeRoom_Dlg.InitCostumeRoom(this.pkSolinfo.GetCharKind(), this.pkSolinfo);
		costumeRoom_Dlg.Show();
	}

	private void Click_PreViewHero(IUIObject obj)
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.pkSolinfo.GetCharKindInfo() == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.MessageBox_PreviewHero), this.pkSolinfo, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3293"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("438"), eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
	}

	private void MessageBox_PreviewHero(object a_oObject)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)a_oObject;
		if (nkSoldierInfo == null)
		{
			return;
		}
		if (nkSoldierInfo.GetCharKindInfo() == null)
		{
			return;
		}
		GS_PREVIEW_HERO_START_REQ gS_PREVIEW_HERO_START_REQ = new GS_PREVIEW_HERO_START_REQ();
		gS_PREVIEW_HERO_START_REQ.i32CharKind = nkSoldierInfo.GetCharKind();
		gS_PREVIEW_HERO_START_REQ.i32CostumeUnique = (int)nkSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PREVIEW_HERO_START_REQ, gS_PREVIEW_HERO_START_REQ);
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg != null)
		{
			solMilitaryGroupDlg.ChangeSceneDestory = false;
			solMilitaryGroupDlg.Hide();
			NrTSingleton<NkClientLogic>.Instance.GidPrivewHero = 82;
		}
	}
}
