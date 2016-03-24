using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ColosseumDlg : Form
{
	private Label m_laBronzeExplain;

	private Label m_lbColosseumMatchPoint;

	private Label m_lbCoinCount;

	private Label m_lbColosseumRank;

	private Label m_lbLeague;

	private Button m_btRank;

	private Button m_btColosseumStart;

	private Button m_bAttackMakeUp;

	private Button m_btAiMatch;

	private Button m_btRewardInfo;

	private DrawTexture m_dBGImg;

	private DrawTexture m_dtLeagureIcon;

	private DrawTexture m_dtEffect;

	private List<COLOSSEUM_RECORDINFO> record_list = new List<COLOSSEUM_RECORDINFO>();

	private int[] m_arColosseumBatchCharKind = new int[3];

	private DrawTexture m_dtFormIcon;

	private DrawTexture m_dtRankIcon;

	private DrawTexture m_dtAiBattleIcon;

	private Button m_Help_Button;

	private bool m_bAiRequest;

	public bool bReceiveBatchList;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private GameObject m_gbEffect_Fight;

	private int m_nWinID;

	private int m_nStep;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/dlg_fight_main", G_ID.COLOSSEUMMAIN_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_laBronzeExplain = (base.GetControl("LB_BronzeUp") as Label);
		this.m_laBronzeExplain.Visible = false;
		this.m_lbColosseumMatchPoint = (base.GetControl("Label_matchpoint") as Label);
		this.m_lbColosseumRank = (base.GetControl("Label_rank") as Label);
		this.m_lbLeague = (base.GetControl("LB_League") as Label);
		this.m_btColosseumStart = (base.GetControl("Button_OK") as Button);
		Button expr_80 = this.m_btColosseumStart;
		expr_80.Click = (EZValueChangedDelegate)Delegate.Combine(expr_80.Click, new EZValueChangedDelegate(this.ClickStart));
		this.m_bAttackMakeUp = (base.GetControl("Button_form") as Button);
		Button expr_BD = this.m_bAttackMakeUp;
		expr_BD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_BD.Click, new EZValueChangedDelegate(this.ClickForm));
		this.m_btRank = (base.GetControl("Button_rank") as Button);
		Button expr_FA = this.m_btRank;
		expr_FA.Click = (EZValueChangedDelegate)Delegate.Combine(expr_FA.Click, new EZValueChangedDelegate(this.ClickOnRank));
		this.m_btAiMatch = (base.GetControl("Button_AIBattle") as Button);
		Button expr_137 = this.m_btAiMatch;
		expr_137.Click = (EZValueChangedDelegate)Delegate.Combine(expr_137.Click, new EZValueChangedDelegate(this.OnClickAIBattle));
		this.m_btRewardInfo = (base.GetControl("Button_Info") as Button);
		Button expr_174 = this.m_btRewardInfo;
		expr_174.Click = (EZValueChangedDelegate)Delegate.Combine(expr_174.Click, new EZValueChangedDelegate(this.OnClickShowRewardInfo));
		this.m_dBGImg = (base.GetControl("DT_Innerbg") as DrawTexture);
		this.m_dBGImg.SetTextureFromBundle("UI/PvP/PvP_BG");
		this.m_dtLeagureIcon = (base.GetControl("DT_LeagueIcon") as DrawTexture);
		this.m_dtFormIcon = (base.GetControl("DrawTexture_FormIcon") as DrawTexture);
		this.m_dtFormIcon.SetTextureFromBundle("ui/pvp/colo_icon03");
		this.m_dtRankIcon = (base.GetControl("DrawTexture_RankIcon") as DrawTexture);
		this.m_dtRankIcon.SetTextureFromBundle("ui/pvp/colo_icon04");
		this.m_dtAiBattleIcon = (base.GetControl("DrawTexture_AIBattleIcon") as DrawTexture);
		this.m_dtAiBattleIcon.SetTextureFromBundle("ui/pvp/colo_icon05");
		this.m_Help_Button = (base.GetControl("Help_Button") as Button);
		Button expr_25F = this.m_Help_Button;
		expr_25F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_25F.Click, new EZValueChangedDelegate(this.OnClickHelp));
		this.m_dtEffect = (base.GetControl("DT_effect_burning") as DrawTexture);
		this.m_lbCoinCount = (base.GetControl("Label_Coincount") as Label);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int value = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_ONEDAY_GIVEITEM_LIMITCOUNT);
		short charDetailFromUnion = kMyCharInfo.GetCharDetailFromUnion(eCHAR_DETAIL_INFO.eCHAR_DETAIL_INFO_LIMIT_COUNT, 3);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3235"),
			"count1",
			charDetailFromUnion,
			"count2",
			value
		});
		this.m_lbCoinCount.SetText(empty);
		GS_COLOSSEUM_BATCH_SOLDIERLIST_REQ gS_COLOSSEUM_BATCH_SOLDIERLIST_REQ = new GS_COLOSSEUM_BATCH_SOLDIERLIST_REQ();
		gS_COLOSSEUM_BATCH_SOLDIERLIST_REQ.i32BatchKindTotal = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nColosseumBatchKindTotal;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_BATCH_SOLDIERLIST_REQ, gS_COLOSSEUM_BATCH_SOLDIERLIST_REQ);
		this.bReceiveBatchList = false;
		this.m_bAiRequest = false;
		base.SetScreenCenter();
		this.Hide();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("colosseum_enter");
	}

	public override void Show()
	{
		if (!this.bReceiveBatchList)
		{
			return;
		}
		this.ShowInfo();
		base.Show();
		this.GetColosseumBatchKind();
		if (this._GuideItem != null)
		{
			this.GuidItemReposition();
		}
		this.LoadEffect();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		this.HideUIGuide();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("colosseum_out");
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public void AddRecordInfo(COLOSSEUM_RECORDINFO info)
	{
		this.record_list.Add(info);
	}

	public int GetPlunderAttackPosSolNum(eSOL_SUBDATA eSetMode)
	{
		int num = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return num;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return num;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolSubData(eSetMode) > 0L)
			{
				num++;
			}
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return num;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolSubData(eSetMode) > 0L)
			{
				num++;
			}
		}
		return num;
	}

	public void ShowInfo()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo.ColosseumGrade == 0)
		{
			this.m_laBronzeExplain.Visible = true;
		}
		else
		{
			this.m_laBronzeExplain.Visible = false;
		}
		string gradeTexture = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTexture(myCharInfo.ColosseumGrade);
		this.m_dtLeagureIcon.SetTexture(gradeTexture);
		string gradeTextKey = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTextKey(myCharInfo.ColosseumGrade);
		this.m_lbLeague.SetText(gradeTextKey);
		int num = 1000 + myCharInfo.ColosseumGradePoint;
		if (myCharInfo.ColosseumOldRank > 0)
		{
			base.SetShowLayer(1, true);
		}
		else
		{
			base.SetShowLayer(1, false);
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("446");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"point",
			num
		});
		this.m_lbColosseumMatchPoint.SetText(text2);
		if (myCharInfo.ColosseumGrade <= 0)
		{
			this.m_lbColosseumRank.Visible = false;
		}
		else
		{
			int colosseumMyGradeRank = myCharInfo.GetColosseumMyGradeRank();
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("447");
			if (colosseumMyGradeRank > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"rank",
					colosseumMyGradeRank
				});
			}
			else
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("292");
			}
			this.m_lbColosseumRank.SetText(text2);
		}
	}

	public void ClickStart(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo.ColosseumMatching && !this.m_bAiRequest)
		{
			return;
		}
		byte b = 0;
		if (!myCharInfo.ColosseumMatching)
		{
			b = 0;
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("58");
			string text = string.Empty;
			string empty = string.Empty;
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				if (this.m_arColosseumBatchCharKind[i] > 0)
				{
					num++;
				}
			}
			int num2 = 3;
			if (num == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("695"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (num < num2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("65");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"currentnum",
					num,
					"maxnum",
					num2
				});
				msgBoxUI.SetMsg(new YesDelegate(this.OnStartMatch), null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
				return;
			}
		}
		if (this.m_bAiRequest && b == 0)
		{
			b = 2;
		}
		GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
		gS_COLOSSEUM_START_REQ.byMode = b;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
	}

	private void OnStartMatch(object a_oObject)
	{
		GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
		gS_COLOSSEUM_START_REQ.byMode = 0;
		if (this.m_bAiRequest)
		{
			gS_COLOSSEUM_START_REQ.byMode = 2;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
	}

	private void OnClickForm(object a_oObject)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		this.Close();
	}

	public void ClickForm(object a_oObject)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2;
		FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		this.Close();
	}

	public void ClickOnRank(IUIObject obj)
	{
		ColosseumRankInfoDlg colosseumRankInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMRANKINFO_DLG) as ColosseumRankInfoDlg;
		if (colosseumRankInfoDlg != null)
		{
			colosseumRankInfoDlg.ShowInfo(eCOLOSSEUMRANK_SHOWTYPE.eCOLOSSEUMRANK_SHOWTYPE_MYLEAGUERANK, 0);
		}
	}

	public void ClickOnRewardExplain(IUIObject obj)
	{
		ColosseumRewardExplainDlg colosseumRewardExplainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMREWARD_EXPLAIN_DLG) as ColosseumRewardExplainDlg;
		if (colosseumRewardExplainDlg != null)
		{
			colosseumRewardExplainDlg.ShowColosseumRewardExplain();
		}
	}

	public void OnClickAIBattle(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (!myCharInfo.ColosseumMatching)
		{
			this.m_bAiRequest = true;
			this.ClickStart(null);
		}
		else
		{
			GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
			gS_COLOSSEUM_START_REQ.byMode = 2;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.Close();
		}
	}

	public void OnClickShowRewardInfo(IUIObject obj)
	{
		ColosseumRewardExplainDlg colosseumRewardExplainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMREWARD_EXPLAIN_DLG) as ColosseumRewardExplainDlg;
		if (colosseumRewardExplainDlg != null)
		{
			colosseumRewardExplainDlg.ShowColosseumRewardExplain();
		}
	}

	public void OnClickHelp(IUIObject obj)
	{
		ColosseumHelpDlg colosseumHelpDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_HELP) as ColosseumHelpDlg;
		if (colosseumHelpDlg != null)
		{
			colosseumHelpDlg.Show();
		}
	}

	public void GetColosseumBatchKind()
	{
		int num = 0;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUMBATCH1 + i);
			if (charSubData != 0L)
			{
				SUBDATA_UNION sUBDATA_UNION = default(SUBDATA_UNION);
				sUBDATA_UNION.nSubData = charSubData;
				int n32SubData_ = sUBDATA_UNION.n32SubData_0;
				int n32SubData_2 = sUBDATA_UNION.n32SubData_1;
				byte b = 0;
				byte b2 = 0;
				SoldierBatch.GetCalcBattlePos((long)n32SubData_, ref b, ref b2);
				if (b2 >= 0 && b2 < 9)
				{
					if (n32SubData_2 > 0 && myCharInfo.IsEnableBatchColosseumSoldier(n32SubData_2))
					{
						this.m_arColosseumBatchCharKind[num] = n32SubData_2;
						num++;
					}
				}
			}
		}
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		int num = 0;
		if (!int.TryParse(param1, out num))
		{
			return;
		}
		if (num == 1)
		{
			if (this.CheckColosseumSolBatch())
			{
				UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)winID) as UI_UIGuide;
				if (uI_UIGuide != null)
				{
					uI_UIGuide.CloseUI = true;
					uI_UIGuide.Close();
				}
				return;
			}
			this._GuideItem = this.m_bAttackMakeUp;
		}
		else if (num == 2)
		{
			if (!this.CheckColosseumSolBatch())
			{
				UI_UIGuide uI_UIGuide2 = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)winID) as UI_UIGuide;
				if (uI_UIGuide2 != null)
				{
					uI_UIGuide2.CloseUI = true;
					uI_UIGuide2.Close();
				}
				return;
			}
			this._GuideItem = this.m_btAiMatch;
		}
		this.m_nWinID = winID;
		this.m_nStep = num;
		if (null != this._GuideItem)
		{
			UI_UIGuide uI_UIGuide3 = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide3 != null)
			{
				this._GuideItem.EffectAni = false;
				this._ButtonZ = this._GuideItem.GetLocation().z;
				this.GuidItemReposition();
			}
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
			uI_UIGuide.Close();
		}
		this._GuideItem = null;
	}

	public void GuidItemReposition()
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			Vector2 vector = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 80f, base.GetLocationY() + this._GuideItem.GetLocationY() - 10f);
			uI_UIGuide.Move(vector, vector);
			this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
			if (this.m_nStep == 1)
			{
				this.m_dtFormIcon.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f - 0.1f);
				this.m_dtFormIcon.AlphaAni(1f, 0.5f, -0.5f);
			}
			else if (this.m_nStep == 2)
			{
				this.m_dtAiBattleIcon.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f - 0.1f);
				this.m_dtAiBattleIcon.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
	}

	private bool CheckColosseumSolBatch()
	{
		int num = 0;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		for (int i = 0; i < 3; i++)
		{
			if (kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUMBATCH1 + i) != 0L)
			{
				num++;
			}
		}
		return num > 0;
	}

	public void LoadEffect()
	{
		string str = string.Format("{0}{1}", "Effect/Instant/fx_ui_coloseum_infinity", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.Effect_Fight), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void Effect_Fight(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_gbEffect_Fight = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_gbEffect_Fight);
					return;
				}
				Vector2 size = this.m_dtEffect.GetSize();
				this.m_gbEffect_Fight.transform.parent = this.m_dtEffect.gameObject.transform;
				this.m_gbEffect_Fight.transform.localPosition = new Vector3(size.x / 2f, -size.y / 2f, 0f);
				this.m_gbEffect_Fight.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
				NkUtil.SetAllChildLayer(this.m_gbEffect_Fight, GUICamera.UILayer);
				this.m_gbEffect_Fight.SetActive(true);
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_gbEffect_Fight);
				}
			}
		}
	}
}
