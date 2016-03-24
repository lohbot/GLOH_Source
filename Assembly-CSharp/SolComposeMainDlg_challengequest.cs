using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolComposeMainDlg_challengequest : SolComposeMainDlg
{
	private int _challengeQuestUnique = -1;

	public NkSoldierInfo _dummySolBase;

	private List<NkSoldierInfo> _selectedDummySoldierList;

	private NkSoldierInfo _dummyTranScendenceMaterialSol;

	private NkSoldierInfo _dummyComposeMaterialSol;

	public int _ChallengeQuestUnique
	{
		get
		{
			return this._challengeQuestUnique;
		}
		set
		{
			this._challengeQuestUnique = value;
		}
	}

	public void SetSolBase(NkSoldierInfo solInfo, bool baseSolSetting, List<NkSoldierInfo> selectedDummySoldierList)
	{
		this._dummySolBase = solInfo;
		this._selectedDummySoldierList = selectedDummySoldierList;
		if (this.m_ShowType == SOLCOMPOSE_TYPE.EXTRACT)
		{
			this.SetExtractSolBase(solInfo, baseSolSetting, selectedDummySoldierList);
		}
		else if (this.m_ShowType == SOLCOMPOSE_TYPE.TRANSCENDENCE)
		{
			this.SetTranscendenceSolBase(true);
		}
		else if (this.m_ShowType == SOLCOMPOSE_TYPE.COMPOSE)
		{
			this.SetComposeSolBase(true, solInfo);
		}
	}

	public void SetTranscendenceSolMaterial(NkSoldierInfo solInfo, bool baseSolSetting)
	{
		if (solInfo == null)
		{
			return;
		}
		this.SetTranscendenceMaterialSol(solInfo);
	}

	public void SetComposeSolMaterial(NkSoldierInfo solInfo, bool baseSolSetting)
	{
		if (solInfo == null)
		{
			return;
		}
		this.SetComposeMaterialSol(solInfo);
	}

	public void ClearRecommendChallenge()
	{
		this.ShowGameGuideDlg(GameGuideType.CHALLENGE_SOL_COMPOSE);
	}

	public new NewListBox GetListBox()
	{
		return base.GetListBox();
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/Compose/DLG_SolComposeMain_New", G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void OnClose()
	{
		this._challengeQuestUnique = -1;
		base.OnClose();
	}

	public override void SetComposeType(SOLCOMPOSE_TYPE eType)
	{
		base.SetComposeType(eType);
		for (int i = 0; i <= 3; i++)
		{
			if (i != (int)eType)
			{
				this.m_Toolbar.Control_Tab[i].controlIsEnabled = false;
			}
		}
		if (eType == SOLCOMPOSE_TYPE.COMPOSE)
		{
			this.btnRecommend.controlIsEnabled = false;
		}
	}

	protected override void ClickHelp(IUIObject obj)
	{
	}

	public static void OnExtractSuccessGuideEnd()
	{
		SolComposeMainDlg_challengequest.SendSuccessPacket(ChallengeManager.eCHALLENGECODE.CHALLENGECODE_EXTRACT);
		SolComposeMainDlg_challengequest.OnShowHelpDlg(eHELP_LIST.Soldier_Extract);
	}

	public static void OnTranscendenceSuccessGuideEnd()
	{
		SolComposeMainDlg_challengequest.SendSuccessPacket(ChallengeManager.eCHALLENGECODE.CHALLENGECODE_TRANSCENDENCE);
		SolComposeMainDlg_challengequest.OnShowHelpDlg(eHELP_LIST.Soldier_Transcend);
	}

	public static void OnComposeSuccessGuideEnd()
	{
		SolComposeMainDlg_challengequest.SendSuccessPacket(ChallengeManager.eCHALLENGECODE.CHALLENGECODE_COMPOSE);
		SolComposeMainDlg_challengequest.OnShowHelpDlg(eHELP_LIST.Soldier_Synthesis);
	}

	public void OnExtractDirectionEnd()
	{
		this.ShowGameGuideDlg(GameGuideType.CHALLENGE_SOL_EXTRACT);
	}

	public void OnTranscendenceDirectionEnd()
	{
		this.ShowGameGuideDlg(GameGuideType.CHALLENGE_SOL_TRANSCENDENCE);
	}

	protected override void OnClickExtractStart(IUIObject obj)
	{
		if (this.m_SolExtract.Count <= 0)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		if (this._dummySolBase == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("295"),
			"charname",
			this._dummySolBase.GetName()
		});
		msgBoxUI.SetMsg(new YesDelegate(this.OnClickExtractOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2866"), empty, eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
		base.HideTouch(false);
	}

	protected override void OnClickAddExtractSol(IUIObject obj)
	{
		base.HideTouch(false);
		SolComposeListDlg_challengequest solComposeListDlg_challengequest = (SolComposeListDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		if (solComposeListDlg_challengequest == null)
		{
			return;
		}
		solComposeListDlg_challengequest.InitData();
		solComposeListDlg_challengequest.ShowType = SOLCOMPOSE_TYPE.EXTRACT;
		solComposeListDlg_challengequest.InitDummyList();
		this.chHeartsUse.SetToggleState(0);
		this.chHeartsUse.SetEnabled(false);
		this.m_bUseHearts = false;
	}

	public override void OnClickTab(IUIObject obj)
	{
	}

	protected override void OnClickHeartsPurchase(IUIObject obj)
	{
	}

	protected override void OnClickTranscendenceBase(IUIObject obj)
	{
		if (this._dummySolBase != null)
		{
			return;
		}
		base.HideTouch(false);
		SolComposeListDlg_challengequest solComposeListDlg_challengequest = (SolComposeListDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		if (solComposeListDlg_challengequest == null)
		{
			return;
		}
		solComposeListDlg_challengequest.InitData();
		solComposeListDlg_challengequest.ShowType = SOLCOMPOSE_TYPE.TRANSCENDENCE;
		solComposeListDlg_challengequest.InitDummyList();
		this.chHeartsUse.SetToggleState(0);
		this.chHeartsUse.SetEnabled(false);
		this.m_bUseHearts = false;
	}

	private void OnClickRemove(IUIObject obj)
	{
		this.m_SolExtract.Clear();
		base.ShowExtractSol(false);
		NewListBox listBox = this.GetListBox();
		listBox.Clear();
	}

	private void OnClickExtractOK(object obj)
	{
		this.m_SolExtract.Clear();
		base.ShowExtractSol(false);
		if (this._selectedDummySoldierList == null)
		{
			return;
		}
		NewListBox listBox = this.GetListBox();
		listBox.Clear();
		SolComposeDirection solComposeDirection = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_DIRECTION_DLG) as SolComposeDirection;
		if (solComposeDirection == null)
		{
			return;
		}
		int[] array = new int[10];
		bool[] array2 = new bool[10];
		for (int i = 0; i < this._selectedDummySoldierList.Count; i++)
		{
			array[i] = (int)Mathf.Ceil((float)UnityEngine.Random.Range(10, 90));
			array[i] = array[0] - array[0] % 10;
			array2[i] = false;
		}
		solComposeDirection.SetExtractData(array2, array);
		solComposeDirection.AddCloseCallback(new OnCloseCallback(this.OnExtractDirectionEnd));
	}

	protected override void OnClickTranscendenceUseAdd(IUIObject obj)
	{
		base.HideTouch(false);
		SolComposeListDlg_challengequest solComposeListDlg_challengequest = (SolComposeListDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		if (solComposeListDlg_challengequest == null)
		{
			return;
		}
		solComposeListDlg_challengequest._OpenType = SOLCOMPOSELIST_DLG_OPENTYPE.TRANSCENDENCE_MATERIAL_SELECT;
		solComposeListDlg_challengequest.InitData();
		solComposeListDlg_challengequest.ShowType = SOLCOMPOSE_TYPE.TRANSCENDENCE;
		solComposeListDlg_challengequest.InitDummyList();
		this.chHeartsUse.SetToggleState(0);
		this.chHeartsUse.SetEnabled(false);
		this.m_bUseHearts = false;
	}

	public void OnClickTrnScendenceMaterialCancel(IUIObject obj)
	{
		if (this.GetListBox() == null)
		{
			return;
		}
		this._dummyTranScendenceMaterialSol = null;
		this.SetTranscendenceMaterialInfo(false);
		this.GetListBox().Clear();
	}

	protected override void OnClickTranscendenceStart(IUIObject obj)
	{
		if (this.GetListBox() == null || this.GetListBox().Count <= 0)
		{
			return;
		}
		if (this._dummySolBase == null)
		{
			Debug.LogError("ERROR, SolComposeMainDlg_challengequest.cs, OnClickTranscendenceStart(), _dummySolBase = null");
			return;
		}
		if (this._dummyTranScendenceMaterialSol == null)
		{
			Debug.LogError("ERROR, SolComposeMainDlg_challengequest.cs, OnClickTranscendenceStart(), _dummyTranScendenceMaterialSol = null");
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2874");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("294"),
			"charname",
			this._dummySolBase.GetCharKindInfo().GetName()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.OnTranscendenceMsgBoxOk), null, textFromInterface, empty, eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnTranscendenceMsgBoxOk(object obj)
	{
		SolTranscendenceSuccess solTranscendenceSuccess = (SolTranscendenceSuccess)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_TRANSCENDENCE_DLG);
		if (solTranscendenceSuccess == null)
		{
			return;
		}
		solTranscendenceSuccess.GetComposeTranscendence(true, this._dummySolBase.GetCharKind(), this._dummySolBase.GetGrade(), this._dummySolBase.GetGrade() + 1, this._dummyTranScendenceMaterialSol.GetCharKind(), this._dummyTranScendenceMaterialSol.GetGrade(), 0, 0);
		solTranscendenceSuccess.AddCloseCallback(new OnCloseCallback(this.OnTranscendenceDirectionEnd));
		this.SetTranscendenceSolBase(false);
		this.GetListBox().Clear();
		this.SetTranscendenceMaterialInfo(false);
		this._dummyTranScendenceMaterialSol = null;
		this._dummySolBase = null;
	}

	protected override void OnClickBase(IUIObject obj)
	{
		base.HideTouch(false);
		SolComposeListDlg_challengequest solComposeListDlg_challengequest = (SolComposeListDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		if (solComposeListDlg_challengequest == null)
		{
			return;
		}
		solComposeListDlg_challengequest._OpenType = SOLCOMPOSELIST_DLG_OPENTYPE.NONE;
		solComposeListDlg_challengequest.InitData();
		solComposeListDlg_challengequest.ShowType = SOLCOMPOSE_TYPE.COMPOSE;
		solComposeListDlg_challengequest.InitDummyList();
	}

	protected override void OnClickSub(IUIObject obj)
	{
		base.HideTouch(false);
		SolComposeListDlg_challengequest solComposeListDlg_challengequest = (SolComposeListDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		if (solComposeListDlg_challengequest == null)
		{
			return;
		}
		solComposeListDlg_challengequest._OpenType = SOLCOMPOSELIST_DLG_OPENTYPE.COMPOSE_MATERIAL_SELECT;
		solComposeListDlg_challengequest.InitData();
		solComposeListDlg_challengequest.ShowType = SOLCOMPOSE_TYPE.COMPOSE;
		solComposeListDlg_challengequest.InitDummyList();
	}

	protected override void OnClickStart(IUIObject obj)
	{
		base.HideTouch(false);
		if (this.GetListBox() == null || this.GetListBox().Count == 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("503");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_LIST_CHALLENGEQUEST_DLG);
		SolComposeCheckDlg_challengequest solComposeCheckDlg_challengequest = (SolComposeCheckDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_CHECK_CHALLENGEQUEST_DLG);
		if (solComposeCheckDlg_challengequest == null)
		{
			return;
		}
		solComposeCheckDlg_challengequest.SetData(this._dummySolBase, this._dummyComposeMaterialSol, SOLCOMPOSE_TYPE.COMPOSE);
	}

	public override void OnClickBuyGold(IUIObject obj)
	{
	}

	private NewListItem GetExtracteSolItem(NkSoldierInfo solInfo)
	{
		NewListItem newListItem = new NewListItem(this.GetListBox().ColumnNum, true, string.Empty);
		UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(solInfo.GetCharKind(), (int)solInfo.GetGrade());
		if (legendFrame != null)
		{
			newListItem.SetListItemData(0, legendFrame, null, null, null);
		}
		newListItem.SetListItemData(2, solInfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(solInfo.GetCharKind(), (int)solInfo.GetGrade(), solInfo.GetName());
		newListItem.SetListItemData(3, legendName, null, null, null);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			solInfo.GetLevel(),
			"count2",
			solInfo.GetSolMaxLevel()
		});
		newListItem.SetListItemData(4, textFromInterface, null, null, null);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
			"season",
			solInfo.GetSeason() + 1
		});
		newListItem.SetListItemData(5, empty, null, null, null);
		newListItem.SetListItemData(6, false);
		newListItem.SetListItemData(7, false);
		newListItem.SetListItemData(8, false);
		newListItem.SetListItemData(9, string.Empty, solInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickRemove), null);
		newListItem.Data = solInfo.GetSolID();
		return newListItem;
	}

	private NewListItem GetTranScendenceMaterialSolItem(NkSoldierInfo solInfo, bool visibleCloseButton)
	{
		if (solInfo == null || this.GetListBox() == null)
		{
			return null;
		}
		NewListBox listBox = this.GetListBox();
		NewListItem newListItem = new NewListItem(listBox.ColumnNum, true, string.Empty);
		UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(solInfo.GetCharKind(), (int)solInfo.GetGrade());
		if (legendFrame != null)
		{
			newListItem.SetListItemData(0, legendFrame, null, null, null);
		}
		newListItem.SetListItemData(1, solInfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(solInfo.GetCharKind(), (int)solInfo.GetGrade(), solInfo.GetName());
		newListItem.SetListItemData(2, legendName, null, null, null);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count1",
			solInfo.GetLevel(),
			"count2",
			solInfo.GetSolMaxLevel()
		});
		newListItem.SetListItemData(3, textFromInterface, null, null, null);
		newListItem.SetListItemData(4, false);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
			"season",
			solInfo.GetSeason() + 1
		});
		newListItem.SetListItemData(5, empty, null, null, null);
		newListItem.SetListItemData(6, false);
		newListItem.SetListItemData(7, false);
		newListItem.SetListItemData(8, true);
		newListItem.SetListItemData(9, false);
		newListItem.SetListItemData(10, "0/100", null, null, null);
		newListItem.SetListItemData(11, false);
		newListItem.SetListItemData(13, false);
		newListItem.SetListItemData(12, string.Empty, solInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickTrnScendenceMaterialCancel), null);
		newListItem.SetListItemData(12, visibleCloseButton);
		newListItem.Data = solInfo.GetSolID();
		return newListItem;
	}

	private void CalcDummyExtractData(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return;
		}
		int num = NrTSingleton<NrSolExtractRateManager>.Instance.GetSolExtractRateItemInfo(solInfo.GetSeason(), (int)solInfo.GetGrade(), false);
		num = (int)((float)num / 100f);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2857"),
			"count",
			string.Format("{0:F2}", num)
		});
		this.lbExtractPercentage.SetText(empty);
	}

	private void SetTranscendenceSolBase(bool show)
	{
		if (this._dummySolBase == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
			"targetname",
			this._dummySolBase.GetName(),
			"count1",
			this._dummySolBase.GetLevel().ToString(),
			"count2",
			this._dummySolBase.GetSolMaxLevel().ToString()
		});
		this.lb_TranscendenceBase_SolName.SetText(empty);
		this.lb_TranscendenceBase_SolName.Visible = show;
		this.dt_TranscendenceBase_SolImg.SetTexture(eCharImageType.LARGE, this._dummySolBase.GetCharKind(), (int)this._dummySolBase.GetGrade(), string.Empty);
		this.dt_TranscendenceBase_SolImg.Visible = show;
		string empty2 = string.Empty;
		int num = this._dummySolBase.GetSeason() + 1;
		if (num != 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
				"season",
				num
			});
		}
		this.lb_TarnscendenceBaseSeasont.SetText(empty2);
		this.lb_TarnscendenceBaseSeasont.Visible = show;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this._dummySolBase.GetCharKind(), (int)this._dummySolBase.GetGrade());
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this._dummySolBase.GetCharKind(), (int)this._dummySolBase.GetGrade());
		if (0 < legendType)
		{
			this.dt_TranscendenceBase_SolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.dt_TranscendenceBase_SolRank.SetTexture(solLargeGradeImg);
		this.dt_TranscendenceBase_SolRank.Visible = show;
		float num2 = 1f;
		this.dt_Transcendence_GaugeBar.SetSize(this.GAGE_TRANSCENDENCE_WIDTH * num2, this.dtGage.GetSize().y);
		this.dt_Transcendence_GaugeBar.Visible = show;
		string empty3 = string.Empty;
		float num3 = 2.5f;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3283"),
			"count",
			num3
		});
		this.lb_Label_AddPercentage.SetText(empty3);
		this.lb_Label_AddPercentage.Visible = show;
		this.btn_TranscendenceUse_Add1.Visible = show;
		this.btn_TranscendenceUse_Add2.Visible = show;
		this.btn_TranscendenceUse_Add3.Visible = show;
		this.lb_TranscendenceBase_Help.Visible = !show;
		this.lb_TranscendenceUseMoney.Visible = false;
		this.lb_Tarnscendence_ExpGet.Visible = false;
		this.lb_Transcendence_SuccessRate.Visible = false;
		this.lb_Transcendence_SuccessRate_Text.Visible = false;
		this.lb_Transcendence_Help2.Visible = false;
		this.btn_Transcendence_StartButton.Visible = false;
		this.lb_Transcendence_AddPercentage_Text.Visible = false;
		this.lb_Transcendence_AddPercentage.Visible = false;
		this.dt_DrawTexture_PercentagePlus.Visible = false;
		this.btnBaseSelect.Visible = !show;
		this.lblBaseGuideText.Visible = !show;
		this.lbBaseName.Visible = !show;
		if (this.lbBaseName.Visible)
		{
			this.lbBaseName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2031"));
		}
		this.lb_Transcendence_Help.Visible = true;
	}

	private void SetComposeSolBase(bool setting, NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return;
		}
		this.btnBaseSelect.SetButtonTextureKey("Win_B_Change");
		this.lblBaseGuideText.Visible = false;
		if (this.GetListBox() != null && this.GetListBox().Count == 0)
		{
			this.btnSubSelectMain.Visible = true;
			this.btnSubSelect.Visible = true;
			this.btnSubSelect.SetButtonTextureKey("Win_B_Addlist");
			this.dtExpBG.Visible = false;
			this.lblExp.Visible = false;
			this.lblGradeText.Visible = false;
			this.lblMaterialGuideText.Visible = true;
		}
		else
		{
			this.btnSubSelect.SetButtonTextureKey("Win_B_Change");
			this.btnSubSelectMain.Visible = false;
			this.dtExpBG.Visible = true;
			this.lblExp.Visible = true;
			this.lblGradeText.Visible = true;
			this.lblMaterialGuideText.Visible = false;
		}
		this.lblComposeText.Visible = true;
		if (solInfo != null)
		{
			this.dtBase.SetTexture(eCharImageType.LARGE, solInfo.GetCharKind(), (int)(solInfo.GetGrade() + 2), string.Empty);
		}
		this.dtBase.Visible = setting;
		if (solInfo != null)
		{
			UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(solInfo.GetCharKind(), (int)solInfo.GetGrade());
			if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(solInfo.GetCharKind(), (int)solInfo.GetGrade()))
			{
				this.SolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
			}
			this.SolRank.SetTexture(solLargeGradeImg);
		}
		this.SolRank.Visible = setting;
		if (solInfo != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3274"),
				"season",
				solInfo.GetSeason() + 1
			});
			this.lblBaseSeasonText.SetText(empty);
		}
		this.lblBaseSeasonText.Visible = setting;
		this.lblExp.Visible = setting;
		this.lblExp.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286"));
		this.lblComposeCost.SetText(string.Format("{0:###,###,###,##0}", 0));
		this.lblComposeCost.Visible = setting;
		if (solInfo != null)
		{
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("290"),
				"targetname",
				solInfo.GetName(),
				"count1",
				solInfo.GetLevel().ToString(),
				"count2",
				solInfo.GetSolMaxLevel().ToString()
			});
			this.lbBaseName.SetText(empty2);
		}
		this.lbBaseName.Visible = setting;
		float num = 1f;
		this.dtGage.SetSize(this.GAGE_SRC_WIDTH * num, this.dtGage.GetSize().y);
		this.dtGage.Visible = setting;
		this.GradeExpBG.Visible = setting;
		this.GradeExpGage.Visible = setting;
		this.GradeExpText.Visible = setting;
		this.lblGradeText.Visible = setting;
		float num2 = 0.8f;
		if (this.GetListBox() != null && 0 < this.GetListBox().Count)
		{
			this.GradeExpGage.SetSize(414f, this.GradeExpGage.height);
		}
		else
		{
			this.GradeExpGage.SetSize(414f * num2, this.GradeExpGage.height);
		}
		if (solInfo != null)
		{
			int num3 = 2000;
			long num4 = solInfo.GetNextEvolutionExp() - solInfo.GetCurBaseEvolutionExp();
			long num5 = num4 - (long)num3;
			if (this.GetListBox() != null && 0 < this.GetListBox().Count)
			{
				num5 = num4;
			}
			string empty3 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
				"exp",
				num5.ToString(),
				"maxexp",
				num4.ToString()
			});
			this.GradeExpText.SetText(empty3);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3273"),
				"evolexp",
				num3.ToString()
			});
			this.lblGradeText.SetText(empty3);
		}
		this.GradeExpText.Visible = setting;
		this.lblGradeText.Visible = setting;
		this.btnOk.Visible = (solInfo != null);
	}

	private void SetTranscendenceMaterialInfo(bool showInfo)
	{
		this.btn_Transcendence_StartButton.Visible = showInfo;
		this.lb_TranscendenceUseMoney.Visible = showInfo;
		this.lb_TranscendenceUseMoney.SetText(base.GetMoneyFormat(0L));
		string text = string.Empty;
		this.lb_Transcendence_SuccessRate.Visible = showInfo;
		text = string.Format(" {0} %", 100);
		this.lb_Transcendence_SuccessRate.SetText(text);
		this.lb_Transcendence_AddPercentage.Visible = showInfo;
		text = string.Format(" {0} %", 2.5f);
		this.lb_Transcendence_AddPercentage.SetText(text);
		this.lb_Tarnscendence_ExpGet.Visible = false;
		this.lb_Transcendence_SuccessRate_Text.Visible = showInfo;
		this.lb_Transcendence_AddPercentage_Text.Visible = showInfo;
		float num = 200f;
		float num2 = 2.5f;
		this.lb_Transcendence_Help.Visible = !showInfo;
		this.lb_Transcendence_Help2.Visible = showInfo;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2869"),
			"count",
			num,
			"count1",
			num2
		});
		this.lb_Transcendence_Help2.SetText(text);
		this.dt_DrawTexture_PercentagePlus.Visible = showInfo;
	}

	private void SetTranscendenceMaterialSol(NkSoldierInfo solInfo)
	{
		if (this.GetListBox() == null)
		{
			return;
		}
		NewListItem tranScendenceMaterialSolItem = this.GetTranScendenceMaterialSolItem(solInfo, true);
		if (tranScendenceMaterialSolItem == null)
		{
			return;
		}
		this.GetListBox().Clear();
		this._dummyTranScendenceMaterialSol = solInfo;
		this.SetTranscendenceMaterialInfo(true);
		if (this.GetListBox().Count <= 0)
		{
			this.GetListBox().Add(tranScendenceMaterialSolItem);
		}
		else
		{
			this.GetListBox().UpdateContents(0, tranScendenceMaterialSolItem);
		}
	}

	private void SetComposeMaterialSol(NkSoldierInfo solInfo)
	{
		if (this.GetListBox() == null)
		{
			return;
		}
		NewListItem tranScendenceMaterialSolItem = this.GetTranScendenceMaterialSolItem(solInfo, false);
		if (tranScendenceMaterialSolItem == null)
		{
			return;
		}
		this.GetListBox().Clear();
		this._dummyComposeMaterialSol = solInfo;
		if (this.GetListBox().Count <= 0)
		{
			this.GetListBox().Add(tranScendenceMaterialSolItem);
		}
		else
		{
			this.GetListBox().UpdateContents(0, tranScendenceMaterialSolItem);
		}
		this.SetComposeSolBase(true, this._dummySolBase);
	}

	private static void SendSuccessPacket(ChallengeManager.eCHALLENGECODE challengeCode)
	{
		GS_RECOMMEND_CHALLENGE_CLEAR_REQ gS_RECOMMEND_CHALLENGE_CLEAR_REQ = new GS_RECOMMEND_CHALLENGE_CLEAR_REQ();
		gS_RECOMMEND_CHALLENGE_CLEAR_REQ.i32RecommendChallengeUnique = (int)challengeCode;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_RECOMMEND_CHALLENGE_CLEAR_REQ, gS_RECOMMEND_CHALLENGE_CLEAR_REQ);
	}

	private void ShowGameGuideDlg(GameGuideType guideType)
	{
		NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.NONE, guideType);
		NrTSingleton<GameGuideManager>.Instance.Update();
		OnCloseCallback callback = null;
		if (this._challengeQuestUnique == 1502)
		{
			callback = new OnCloseCallback(SolComposeMainDlg_challengequest.OnExtractSuccessGuideEnd);
		}
		else if (this._challengeQuestUnique == 1505)
		{
			callback = new OnCloseCallback(SolComposeMainDlg_challengequest.OnTranscendenceSuccessGuideEnd);
		}
		else if (this._challengeQuestUnique == 1499)
		{
			callback = new OnCloseCallback(SolComposeMainDlg_challengequest.OnComposeSuccessGuideEnd);
		}
		GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
		gameGuideDlg.RegistCloseCallback(callback);
	}

	private void SetExtractSolBase(NkSoldierInfo solInfo, bool baseSolSetting, List<NkSoldierInfo> selectedDummySoldierList)
	{
		this.m_SolExtract.Clear();
		this.m_SolExtract.Add(solInfo.GetSolID());
		NewListBox listBox = this.GetListBox();
		listBox.Clear();
		if (baseSolSetting && selectedDummySoldierList != null)
		{
			foreach (NkSoldierInfo current in selectedDummySoldierList)
			{
				if (current != null)
				{
					listBox.Add(this.GetExtracteSolItem(current));
				}
			}
		}
		base.ShowExtractSol(baseSolSetting);
		this.CalcDummyExtractData(solInfo);
		listBox.RepositionItems();
	}

	public static void OnShowHelpDlg(eHELP_LIST helpInfo)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(helpInfo.ToString());
		}
	}
}
