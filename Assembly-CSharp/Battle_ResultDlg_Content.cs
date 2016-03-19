using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_ResultDlg_Content : Form
{
	protected static int BATTLE_RESULT_LAYER_1 = 1;

	protected static int BATTLE_SREWARD_LAYER_NUM = 2;

	protected static int BATTLE_BABEL_RESULT_LAYER = 3;

	protected static int BATTLE_SREWARD_BUTTON_MAX = 4;

	private Button m_btClose;

	private Label m_lbLoading;

	private ListBox m_ItemList;

	private Button m_btRepeat;

	private Button m_btDown;

	private Button m_btUp;

	private Label m_lbClose;

	private Label m_lbRepeat;

	private Label m_lbDown;

	private Label m_lbUp;

	private DrawTexture m_dtLock;

	private NewListBox m_nlSolInfo;

	private Label m_lbExpTotalGet;

	private Label m_RewardExplainLabel;

	private Button m_RewardOKButton;

	private Label m_RewardNotify;

	private DrawTexture m_dtRank;

	private DrawTexture m_dtWiinbg;

	private GameObject m_goRankEffectObject;

	private bool m_bRankUPgrade;

	private bool m_bSetUpgradeTexture;

	private float m_fRankEffectShow;

	private Label m_lbGetEXP;

	private int m_nPartyCount;

	public string m_strBattleTime;

	public string m_strWin;

	public bool m_bWin;

	public int m_BattleSRewardUnique;

	private eBATTLE_ROOMTYPE m_eRoomType;

	private float m_fCloseEnableTime;

	private int m_nInjurySolCount;

	private float m_fBattleTime;

	private bool m_bRankEffectSet;

	private List<GS_BATTLE_RESULT_SOLDIER> m_SolInfoList = new List<GS_BATTLE_RESULT_SOLDIER>();

	private List<GS_BATTLE_RESULT_ITEM> m_ItemInfoList = new List<GS_BATTLE_RESULT_ITEM>();

	private GS_BATTLE_RESULT_NFY m_BasicInfo = new GS_BATTLE_RESULT_NFY();

	private List<GS_BATTLE_RESULT_SOLDIER> m_SReward_SolInfoList = new List<GS_BATTLE_RESULT_SOLDIER>();

	private List<GS_BATTLE_RESULT_ITEM> m_SReward_ItemInfoList = new List<GS_BATTLE_RESULT_ITEM>();

	private GS_BATTLE_SREWARD_ACK m_SReward_BasicInfo = new GS_BATTLE_SREWARD_ACK();

	private SREWARD_PRODUCT m_SelectSRewardProduct = new SREWARD_PRODUCT();

	private List<long> m_lsInjurySolID = new List<long>();

	private GameObject m_SelectSRewardGameObject;

	private GameObject m_SRewardGetGameObject;

	private GameObject m_SRewardImageGameObject;

	private string sRewardImageKey = string.Empty;

	private Label[] m_lbItemNum;

	private Label[] m_lbItemNumGet;

	private int m_iSelectIndex;

	private GameObject m_CloseEffect;

	private float m_fAutoCloseTime;

	private bool m_bShowRank;

	public bool RankEffectSet
	{
		get
		{
			return this.m_bRankEffectSet;
		}
		set
		{
			this.m_bRankEffectSet = value;
		}
	}

	public override void InitializeComponent()
	{
		Main_UI_SystemMessage.CloseUI();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Battle/RESULT/DLG_Battle_Result_Content", G_ID.BATTLE_RESULT_CONTENT_DLG, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 8f);
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			this.m_eRoomType = Battle.BATTLE.BattleRoomtype;
			this.m_nPartyCount = Battle.BabelPartyCount;
		}
		else
		{
			this.m_eRoomType = eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NONE;
		}
		this.Show();
	}

	public override void SetComponent()
	{
		this._SetComponetSol();
		this._SetComponetReputeRate();
		this._SetComponetRootItem();
		this._SetComponetSReward();
		base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_SREWARD_LAYER_NUM, false);
		this.m_btClose.Visible = false;
		this.m_btDown.Visible = false;
		this.m_btRepeat.Visible = false;
		this.m_btUp.Visible = false;
		this.m_lbItemNum = new Label[Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX];
		this.m_lbItemNumGet = new Label[Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX];
		string text = string.Empty;
		for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
		{
			text = string.Format("ItemName{0}", i + 1);
			this.m_lbItemNum[i] = UICreateControl.Label(text, text, false, 257f, 28f, SpriteText.Font_Effect.Black_Shadow_Big, SpriteText.Anchor_Pos.Upper_Center, SpriteText.Alignment_Type.Center, Color.white);
			this.m_lbItemNum[i].Hide(true);
			text = string.Format("ItemNameGet{0}", i + 1);
			this.m_lbItemNumGet[i] = UICreateControl.Label(text, text, false, 257f, 28f, SpriteText.Font_Effect.Black_Shadow_Big, SpriteText.Anchor_Pos.Upper_Center, SpriteText.Alignment_Type.Center, Color.white);
			this.m_lbItemNumGet[i].Hide(true);
		}
		this.m_fCloseEnableTime = 0f;
	}

	public override void InitData()
	{
		base.InitData();
		this.ResizeDlg();
	}

	public override void Update()
	{
		if (0f < this.m_fAutoCloseTime && this.m_fAutoCloseTime <= Time.realtimeSinceStartup)
		{
			this.m_fAutoCloseTime = 0f;
			this.ClickRewardOKButton(null);
		}
		base.Update();
		if (this.m_bRankEffectSet && this.m_fRankEffectShow != 0f && this.m_fRankEffectShow < Time.realtimeSinceStartup && this.m_goRankEffectObject != null)
		{
			float num = 0f;
			this.m_goRankEffectObject.SetActive(true);
			Animation componentInChildren = this.m_goRankEffectObject.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				componentInChildren.Play();
				num = componentInChildren.clip.length;
			}
			this.m_fRankEffectShow = 0f;
			Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
			if (battle_ResultDlg != null)
			{
				battle_ResultDlg.ResultFxTime = Time.realtimeSinceStartup + num;
			}
		}
		if (!this.m_btClose.Visible && this.m_SRewardGetGameObject == null && Scene.CurScene != Scene.Type.BATTLE && CommonTasks.IsEndOfPrework)
		{
			if (this.m_fCloseEnableTime == 0f)
			{
				this.m_fCloseEnableTime = Time.realtimeSinceStartup + 0.3f;
			}
			else if (this.m_fCloseEnableTime < Time.realtimeSinceStartup)
			{
				bool flag = true;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					foreach (long current in this.m_lsInjurySolID)
					{
						NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(current);
						if (soldierInfoFromSolID != null && !soldierInfoFromSolID.IsInjuryStatus())
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.m_btClose.Visible = true;
					this.m_lbClose.Visible = true;
					this.m_lbLoading.Visible = false;
					if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
					{
						if (this.m_bShowRank)
						{
							this.m_dtRank.Visible = true;
							this.m_dtWiinbg.Visible = true;
						}
						if (!NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
						{
							this.BabelTowerButtonSet(true);
						}
					}
					if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
					{
						this.OnClickClose(this.m_btClose);
					}
				}
			}
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_SelectSRewardGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SelectSRewardGameObject);
			this.m_SelectSRewardGameObject = null;
		}
		if (this.m_SRewardGetGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SRewardGetGameObject);
			this.m_SRewardGetGameObject = null;
		}
		if (this.m_SRewardImageGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SRewardImageGameObject);
			this.m_SRewardImageGameObject = null;
		}
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_END, 0f);
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_REPEAT_MAIN_DLG) == null)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_REPEAT_MAIN_DLG);
			}
			return;
		}
		if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG) && !NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
			{
				BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
				if (babelTowerMainDlg != null)
				{
					babelTowerMainDlg.FloorType = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELTYPE, 1);
					babelTowerMainDlg.ShowList();
				}
			}
			UnityEngine.Object.Destroy(this.m_goRankEffectObject);
			this.m_goRankEffectObject = null;
		}
		else if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			UnityEngine.Object.Destroy(this.m_goRankEffectObject);
			this.m_goRankEffectObject = null;
		}
		else if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMMAIN_DLG))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COLOSSEUMMAIN_DLG);
			}
			int @int = PlayerPrefs.GetInt("Colosseum GradeRank", 0);
			if (myCharInfo.ColosseumGrade > 0 && (@int != myCharInfo.GetColosseumMyGradeRank() || @int == 0))
			{
				ColosseumChangeRankDlg colosseumChangeRankDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_CHANGERANK_DLG) as ColosseumChangeRankDlg;
				if (colosseumChangeRankDlg != null)
				{
					colosseumChangeRankDlg.ShowChangeRank(@int);
					int colosseumMyGradeRank = myCharInfo.GetColosseumMyGradeRank();
					PlayerPrefs.SetInt("Colosseum GradeRank", colosseumMyGradeRank);
				}
			}
		}
		else if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_DAILYDUNGEON)
		{
			DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
			if (dailyDungeon_Main_Dlg != null)
			{
				dailyDungeon_Main_Dlg.SetClearEffect((sbyte)this.m_BasicInfo.BeforeRank, (sbyte)this.m_BasicInfo.CurrentRank);
			}
		}
		else if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
		{
			BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
			if (babelGuildBossDlg != null)
			{
				babelGuildBossDlg.ShowList();
			}
		}
		NrTSingleton<FiveRocksEventManager>.Instance.BattleResult(this.m_eRoomType, this.m_fBattleTime, this.m_nInjurySolCount);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bNoMove)
		{
			GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
			gS_WARP_REQ.nMode = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
		}
		if (null != this.m_CloseEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.m_CloseEffect);
		}
	}

	public void _SetComponetSol()
	{
		this.m_nlSolInfo = (base.GetControl("NewListBox_SolGetEXP") as NewListBox);
		this.m_lbExpTotalGet = (base.GetControl("Label_GetEXPNUM") as Label);
	}

	public void _SetComponetReputeRate()
	{
	}

	public void _SetComponetRootItem()
	{
		this.m_ItemList = (base.GetControl("ListBox_RootItemList") as ListBox);
		this.m_ItemList.Reserve = false;
		this.m_ItemList.ColumnNum = 6;
		this.m_ItemList.LineHeight = 100f;
		this.m_ItemList.itemSpacing = 2f;
		this.m_ItemList.UseColumnRect = true;
		this.m_ItemList.SetColumnRect(0, 0, 0, 530, 100);
		this.m_ItemList.SetColumnRect(1, 13, 13, 74, 74);
		this.m_ItemList.SetColumnRect(2, 20, 20, 60, 60);
		this.m_ItemList.SetColumnRect(3, 101, 17, 300, 30, SpriteText.Anchor_Pos.Middle_Left, 28f);
		this.m_ItemList.SetColumnRect(4, 101, 53, 300, 30, SpriteText.Anchor_Pos.Middle_Left, 28f);
		this.m_ItemList.SetColumnRect(5, 423, 32, 80, 36, SpriteText.Anchor_Pos.Middle_Left, 32f);
		this.m_lbClose = (base.GetControl("Label_close") as Label);
		this.m_btClose = (base.GetControl("Button_ok") as Button);
		Button expr_120 = this.m_btClose;
		expr_120.Click = (EZValueChangedDelegate)Delegate.Combine(expr_120.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_btClose.Visible = false;
		this.m_btClose.EffectAni = false;
		this.m_lbClose.Visible = false;
		this.m_lbLoading = (base.GetControl("Label_Loading") as Label);
		this.m_dtRank = (base.GetControl("DT_Rank") as DrawTexture);
		this.m_lbGetEXP = (base.GetControl("Label_GetEXP") as Label);
		this.m_dtWiinbg = (base.GetControl("DT_winbg") as DrawTexture);
		this.m_dtWiinbg.Visible = false;
		this.m_btRepeat = (base.GetControl("Button_repeat") as Button);
		Button expr_1E5 = this.m_btRepeat;
		expr_1E5.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1E5.Click, new EZValueChangedDelegate(this.OnClickRepeat));
		this.m_btDown = (base.GetControl("Button_down") as Button);
		Button expr_222 = this.m_btDown;
		expr_222.Click = (EZValueChangedDelegate)Delegate.Combine(expr_222.Click, new EZValueChangedDelegate(this.OnClickDown));
		this.m_btUp = (base.GetControl("Button_up") as Button);
		Button expr_25F = this.m_btUp;
		expr_25F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_25F.Click, new EZValueChangedDelegate(this.OnClickUp));
		this.m_lbRepeat = (base.GetControl("LB_repeat") as Label);
		this.m_lbDown = (base.GetControl("LB_down") as Label);
		this.m_lbUp = (base.GetControl("LB_up") as Label);
		this.m_dtLock = (base.GetControl("DT_Lock") as DrawTexture);
		this.BabelTowerButtonSet(false);
	}

	public void _SetComponetSReward()
	{
		this.m_RewardExplainLabel = (base.GetControl("Label_RewardExplain") as Label);
		this.m_RewardExplainLabel.Visible = false;
		this.m_RewardOKButton = (base.GetControl("Button_ok2") as Button);
		this.m_RewardOKButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRewardOKButton));
		this.m_RewardOKButton.Visible = false;
		this.m_RewardNotify = (base.GetControl("Label_Notify") as Label);
		if (null != this.m_RewardNotify.spriteText)
		{
			this.m_RewardNotify.SetSize(this.m_RewardNotify.spriteText.TotalWidth, this.m_RewardNotify.height);
		}
		this.m_RewardNotify.Visible = false;
	}

	private void BabelTowerButtonSet(bool bShow)
	{
		this.m_lbRepeat.Visible = bShow;
		this.m_lbDown.Visible = bShow;
		this.m_lbUp.Visible = bShow;
		this.m_btDown.Visible = bShow;
		this.m_btRepeat.Visible = bShow;
		this.m_btUp.Visible = bShow;
		this.m_btClose.Visible = bShow;
		this.m_lbClose.Visible = bShow;
		if (bShow)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			int num = 0;
			if (instance != null)
			{
				num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_REPEAT);
			}
			if (myCharInfo.GetLevel() < num)
			{
				this.m_dtLock.Visible = true;
			}
			else
			{
				this.m_dtLock.Visible = false;
			}
		}
		else
		{
			this.m_dtLock.Visible = false;
		}
	}

	public void ClickRewardOKButton(IUIObject obj)
	{
		this.m_fAutoCloseTime = 0f;
		if (this.m_SelectSRewardGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SelectSRewardGameObject);
			this.m_SelectSRewardGameObject = null;
		}
		if (this.m_SRewardGetGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SRewardGetGameObject);
			this.m_SRewardGetGameObject = null;
		}
		if (this.m_SRewardImageGameObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_SRewardImageGameObject);
			this.m_SRewardImageGameObject = null;
		}
		base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_SREWARD_LAYER_NUM, false);
		Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
		if (battle_ResultDlg != null)
		{
			battle_ResultDlg.LinkData(-1);
			base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_RESULT_LAYER_1, true);
			if (this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
			{
				base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_BABEL_RESULT_LAYER, false);
			}
			else
			{
				base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_BABEL_RESULT_LAYER, true);
				this.BabelTowerButtonSet(false);
			}
			this.m_nlSolInfo.Visible = true;
			this.BabelTowerButtonSet(false);
			this.m_dtRank.Visible = false;
			this.m_dtWiinbg.Visible = false;
			this.m_lbLoading.Visible = true;
			this.m_RewardOKButton.Visible = false;
		}
		if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_ING, 0f);
		}
	}

	public void ClickRewardCardButton(int iSelectIndex)
	{
		if (this.m_BattleSRewardUnique > 0)
		{
			this.m_iSelectIndex = iSelectIndex;
			GS_BATTLE_SREWARD_REQ gS_BATTLE_SREWARD_REQ = new GS_BATTLE_SREWARD_REQ();
			gS_BATTLE_SREWARD_REQ.m_nRewardUnique = this.m_BattleSRewardUnique;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_SREWARD_REQ, gS_BATTLE_SREWARD_REQ);
			if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
			{
				NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_SELECT_SPECIAL_RESULT, 0f);
			}
		}
	}

	public void ResizeDlg()
	{
		float x = (GUICamera.width - base.GetSizeX()) / 2f;
		float num = (GUICamera.height - base.GetSizeY()) / 2f;
		if (num < 76f)
		{
			num = 76f;
		}
		if (num + base.GetSizeY() >= GUICamera.height)
		{
			num -= num + base.GetSizeY() - GUICamera.height;
		}
		base.SetLocation(x, num);
	}

	private bool IsEnableBattle(short _nFloor, short _nSubFloor, short _nFloorType)
	{
		if (!NrTSingleton<BabelTowerManager>.Instance.IsEnableBattleUseActivityPoint(_nFloor, _nSubFloor, _nFloorType))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
			return false;
		}
		return true;
	}

	public void OnClickClose(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_RESULT_DLG);
	}

	public void OnClickRepeat(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num = 0;
		int num2 = 0;
		string text = " ";
		if (myCharInfo.ColosseumMatching)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("615"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (instance != null)
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
			{
				num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT);
			}
			else
			{
				short vipLevelAddBattleRepeat = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelAddBattleRepeat();
				num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT) + (int)vipLevelAddBattleRepeat;
			}
			num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_REPEAT);
		}
		if (myCharInfo.GetLevel() < num)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("781"),
				"level",
				num
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		short num3 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELTYPE, 0);
		int @int;
		int num4;
		if (num3 == 2)
		{
			@int = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 0);
			num4 = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, -1);
		}
		else
		{
			@int = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 0);
			num4 = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, -1);
		}
		if (@int <= 0 || num4 < 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("614");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (!this.IsEnableBattle((short)@int, (short)num4, num3))
		{
			return;
		}
		num4++;
		MsgBoxTwoCheckUI msgBoxTwoCheckUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_TWOCHECK_DLG) as MsgBoxTwoCheckUI;
		if (num3 == 2)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2784");
		}
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("186"),
			"type",
			text,
			"floor",
			@int.ToString(),
			"subfloor",
			num4.ToString(),
			"count",
			num2.ToString()
		});
		msgBoxTwoCheckUI.SetCheckBoxState(1, false);
		msgBoxTwoCheckUI.SetCheckBoxState(2, false);
		msgBoxTwoCheckUI.SetMsg(new YesDelegate(BabelTowerMainDlg.RepeatBabelStart), msgBoxTwoCheckUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("185"), empty2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("196"), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("263"), new CheckBox2Delegate(BabelTowerMainDlg.CheckBattleSpeedCount), eMsgType.MB_CHECK12_OK_CANCEL);
	}

	public void OnClickUp(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		short num = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELTYPE, 0);
		short num2;
		short num3;
		if (num == 2)
		{
			num2 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 0);
			num3 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, -1);
		}
		else
		{
			num2 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 0);
			num3 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, -1);
		}
		short maxSubFloor = NrTSingleton<BabelTowerManager>.Instance.GetMaxSubFloor(num2, num);
		if (num3 < maxSubFloor)
		{
			num3 += 1;
		}
		else
		{
			if (num2 + 1 >= 100)
			{
				return;
			}
			num2 += 1;
			num3 = 0;
		}
		if (!this.IsEnableBattle(num2, num3, num))
		{
			return;
		}
		bool flag = NrTSingleton<BabelTowerManager>.Instance.IsBabelStart();
		if (flag)
		{
			GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
			gS_BABELTOWER_GOLOBBY_REQ.mode = 0;
			gS_BABELTOWER_GOLOBBY_REQ.babel_floor = num2;
			gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = num3;
			gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
			gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = num;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
		}
	}

	public void OnClickDown(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		short num = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELTYPE, 0);
		short num2;
		short num3;
		if (num == 2)
		{
			num2 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 0);
			num3 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, -1);
		}
		else
		{
			num2 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 0);
			num3 = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, -1);
		}
		if (num3 - 1 >= 0)
		{
			num3 -= 1;
		}
		else
		{
			if (num2 - 1 <= 0)
			{
				return;
			}
			num2 -= 1;
			num3 = NrTSingleton<BabelTowerManager>.Instance.GetMaxSubFloor(num2, num);
		}
		if (!this.IsEnableBattle(num2, num3, num))
		{
			return;
		}
		bool flag = NrTSingleton<BabelTowerManager>.Instance.IsBabelStart();
		if (flag)
		{
			GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
			gS_BABELTOWER_GOLOBBY_REQ.mode = 0;
			gS_BABELTOWER_GOLOBBY_REQ.babel_floor = num2;
			gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = num3;
			gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
			gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = num;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
		}
	}

	public void AddSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_SolInfoList.Add(solinfo);
	}

	public void AddItemData(GS_BATTLE_RESULT_ITEM iteminfo)
	{
		if (iteminfo.m_sItem.m_nItemUnique == COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASH_ITEMUNIQUE))
		{
			this.m_ItemInfoList.Insert(0, iteminfo);
		}
		else
		{
			this.m_ItemInfoList.Add(iteminfo);
		}
	}

	public void SetBasicData(GS_BATTLE_RESULT_NFY info)
	{
		this.m_BasicInfo = info;
	}

	public void ClearSolData()
	{
		this.m_SolInfoList.Clear();
	}

	public void ClearItemData()
	{
		this.m_ItemInfoList.Clear();
	}

	public void AddSRewardSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_SReward_SolInfoList.Add(solinfo);
	}

	public void AddSRewardItemData(GS_BATTLE_RESULT_ITEM iteminfo)
	{
		this.m_SReward_ItemInfoList.Add(iteminfo);
	}

	public void SetSRewardBasicData(GS_BATTLE_SREWARD_ACK info)
	{
		this.m_SReward_BasicInfo = info;
	}

	public void ClearSRewardSolData()
	{
		this.m_SReward_SolInfoList.Clear();
	}

	public void ClearSRewardItemData()
	{
		this.m_SReward_ItemInfoList.Clear();
	}

	public void LinkData()
	{
		this._LinkSolData();
		this._LinkItemData();
	}

	private void AddLinkItemDataList(long nGold)
	{
		ListItem listItem = new ListItem();
		listItem.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
		listItem.SetColumnGUIContent(1, string.Empty, "Win_I_CPortFrame");
		string empty = string.Empty;
		listItem.SetColumnGUIContent(2, string.Empty, "Main_I_ExtraI01");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
			nGold
		});
		listItem.SetColumnStr(3, empty);
		this.m_ItemList.Add(listItem);
	}

	private void AddLinkItemDataList(ITEM Battleitem, int itemNum)
	{
		ListItem listItem = new ListItem();
		listItem.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
		listItem.SetColumnGUIContent(1, string.Empty, "Win_I_CPortFrame");
		string str = string.Empty;
		if (Battleitem.m_nItemPos != -9999)
		{
			listItem.SetColumnGUIContent(2, Battleitem);
			str = NrTSingleton<ItemManager>.Instance.GetName(Battleitem);
			str = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Battleitem);
		}
		else
		{
			listItem.SetColumnGUIContent(2, string.Empty, "Win_I_LetterC05");
			str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1935");
		}
		listItem.SetColumnStr(3, str);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1804"),
			"itemnum",
			itemNum.ToString()
		});
		listItem.SetColumnStr(5, empty);
		this.m_ItemList.Add(listItem);
	}

	private void _LinkItemData()
	{
		this.m_ItemList.Clear();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
			"0"
		});
		if (this.m_SelectSRewardProduct.m_nRewardType == 1)
		{
			if (this.m_SReward_ItemInfoList.Count > 0)
			{
				if (this.m_SelectSRewardProduct.m_nRewardValue1 > 0)
				{
					int itemPosFromItemUnique = NkUserInventory.GetInstance().GetItemPosFromItemUnique(this.m_SelectSRewardProduct.m_nRewardValue1);
					int itemPosType = Protocol_Item.GetItemPosType(this.m_SelectSRewardProduct.m_nRewardValue1);
					ITEM item = NkUserInventory.GetInstance().GetItem(itemPosType, itemPosFromItemUnique);
					if (item != null)
					{
						this.AddLinkItemDataList(item, this.m_SelectSRewardProduct.m_nRewardValue2);
					}
				}
			}
			else
			{
				this.AddLinkItemDataList(0L);
			}
		}
		else if (this.m_SelectSRewardProduct.m_nRewardType == 2)
		{
			this.AddLinkItemDataList((long)this.m_SelectSRewardProduct.m_nRewardValue1);
		}
		foreach (GS_BATTLE_RESULT_ITEM current in this.m_ItemInfoList)
		{
			ITEM sItem = current.m_sItem;
			if (sItem != null)
			{
				this.AddLinkItemDataList(sItem, current.ItemNum);
				if (sItem.m_nItemUnique == 70000)
				{
					if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
					{
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.BATTLE_BABEL_REWARD, (long)current.ItemNum);
					}
					else if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
					{
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.BATTLE_COLOSSEUM_REWARD, (long)current.ItemNum);
					}
				}
			}
		}
		this.m_ItemList.RepositionItems();
	}

	private void _LinkSolData()
	{
		int num = 0;
		int num2 = 9;
		this.m_nlSolInfo.Clear();
		this.m_lsInjurySolID.Clear();
		int num3 = 0;
		GS_BATTLE_RESULT_SOLDIER gS_BATTLE_RESULT_SOLDIER = null;
		NewListItem newListItem = null;
		foreach (GS_BATTLE_RESULT_SOLDIER current in this.m_SolInfoList)
		{
			int num4 = num % 2 * num2;
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
			if (charKindInfo != null)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				if (num < 19)
				{
					short gradeMaxLevel = charKindInfo.GetGradeMaxLevel((short)((byte)current.SolGrade));
					if (num % 2 == 0)
					{
						newListItem = new NewListItem(this.m_nlSolInfo.ColumnNum, true);
					}
					NkListSolInfo nkListSolInfo = new NkListSolInfo();
					nkListSolInfo.SolCharKind = current.CharKind;
					nkListSolInfo.SolGrade = current.SolGrade;
					nkListSolInfo.SolInjuryStatus = current.bInjury;
					nkListSolInfo.SolLevel = current.i16Level;
					nkListSolInfo.ShowLevel = true;
					if (nkListSolInfo.SolInjuryStatus)
					{
						this.m_lsInjurySolID.Add(current.SolID);
						this.m_nInjurySolCount++;
					}
					string text = string.Empty;
					if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(current.CharKind))
					{
						text = charPersonInfo.GetCharName();
					}
					else
					{
						text = charKindInfo.GetName();
					}
					newListItem.SetListItemData(0 + num4, true);
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471"),
						"targetname",
						text,
						"count",
						current.i16Level
					});
					NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
					Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(charKindInfo2.GetCharKind());
					if (portraitLeaderSol != null)
					{
						newListItem.SetListItemData(1 + num4, portraitLeaderSol, null, null, null, null);
					}
					else if (charKindInfo2 != null)
					{
						EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(charKindInfo2.GetCharKind(), (byte)current.SolGrade);
						if (eventHeroCharCode != null)
						{
							newListItem.SetListItemData(1 + num4, "Win_I_EventSol", null, null, null);
							newListItem.SetListItemData(2 + num4, nkListSolInfo, true, null, null);
						}
						else
						{
							newListItem.SetListItemData(2 + num4, nkListSolInfo, false, null, null);
						}
					}
					string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(charKindInfo2.GetCharKind(), (int)((byte)current.SolGrade), text);
					newListItem.SetListItemData(3 + num4, legendName, null, null, null);
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(current.SolID);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
						"count1",
						nkListSolInfo.SolLevel.ToString(),
						"count2",
						soldierInfoFromSolID.GetSolMaxLevel().ToString()
					});
					newListItem.SetListItemData(8 + num4, empty, null, null, null);
					if (this.m_SReward_SolInfoList.Count > 0)
					{
						gS_BATTLE_RESULT_SOLDIER = this.m_SReward_SolInfoList[num];
					}
					bool flag = false;
					if ((this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NORMAL || this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER) && gradeMaxLevel <= current.i16Level && current.i32AddExp == 0)
					{
						flag = true;
					}
					if (flag)
					{
						newListItem.SetListItemData(7 + num4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1373"), null, null, null);
					}
					else if (gradeMaxLevel == current.i16Level)
					{
						newListItem.SetListItemData(7 + num4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286"), null, null, null);
					}
					else if (gS_BATTLE_RESULT_SOLDIER != null)
					{
						string text2 = string.Empty;
						string empty2 = string.Empty;
						string empty3 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1801"),
							"exp",
							current.i32AddExp.ToString()
						});
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1802"),
							"exp",
							gS_BATTLE_RESULT_SOLDIER.i32AddExp.ToString()
						});
						text2 = empty2 + empty3;
						num3 += current.i32AddExp + gS_BATTLE_RESULT_SOLDIER.i32AddExp;
						newListItem.SetListItemData(7 + num4, text2, null, null, null);
					}
					else
					{
						string empty4 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1801"),
							"exp",
							current.i32AddExp.ToString()
						});
						newListItem.SetListItemData(7 + num4, empty4, null, null, null);
						num3 += current.i32AddExp;
					}
					NkSoldierInfo soldierInfoFromSolID2 = charPersonInfo.GetSoldierInfoFromSolID(current.SolID);
					if (soldierInfoFromSolID2 != null)
					{
						newListItem.Data = current.SolID;
						float num5 = soldierInfoFromSolID2.GetExpPercent();
						string empty5 = string.Empty;
						if (num5 < 0f)
						{
							num5 = 0f;
						}
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty5, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
							"Count",
							((int)(num5 * 100f)).ToString()
						});
						newListItem.SetListItemData(5 + num4, "Com_T_GauWaPr4", 163f * num5, null, null);
						newListItem.SetListItemData(6 + num4, false);
					}
					else
					{
						newListItem.Data = 0;
					}
					if (num % 2 == 1)
					{
						this.m_nlSolInfo.Add(newListItem);
						newListItem = null;
					}
					num++;
				}
			}
		}
		if (newListItem != null)
		{
			int num4 = num % 2 * num2;
			newListItem.SetListItemData(1 + num4, false);
			newListItem.SetListItemData(2 + num4, false);
			newListItem.SetListItemData(3 + num4, false);
			newListItem.SetListItemData(4 + num4, false);
			newListItem.SetListItemData(5 + num4, false);
			newListItem.SetListItemData(6 + num4, false);
			newListItem.SetListItemData(7 + num4, false);
			this.m_nlSolInfo.Add(newListItem);
			newListItem = null;
		}
		this.m_nlSolInfo.RepositionItems();
		this.m_lbExpTotalGet.SetText(num3.ToString());
	}

	public void _LinkBasicData()
	{
		if (this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_BABEL_RESULT_LAYER, false);
		}
		else
		{
			base.SetShowLayer(Battle_ResultDlg_Content.BATTLE_BABEL_RESULT_LAYER, true);
			this.BabelTowerButtonSet(false);
			this.m_dtRank.Visible = false;
			this.m_dtWiinbg.Visible = false;
			if (this.m_nPartyCount > 1)
			{
				string empty = string.Empty;
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_ADDEXPRATE);
				int num = (this.m_nPartyCount - 1) * value;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1374"),
					"count",
					num.ToString()
				});
				this.m_lbGetEXP.SetText(empty);
			}
		}
		if (Battle.BATTLE == null)
		{
			Debug.LogError("Battle Result dialog : Battle.BATTLE Is NULL");
			return;
		}
		bool flag = (eBATTLE_ALLY)this.m_BasicInfo.i8WinAlly == Battle.BATTLE.MyAlly;
		if (flag)
		{
			this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("855");
			this.m_bWin = true;
		}
		else
		{
			this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("856");
			this.m_bWin = false;
		}
		NrTSingleton<GameGuideManager>.Instance.WinBattle = flag;
		if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			string text = BATTLE_DEFINE.RANK_STRING[this.m_BasicInfo.CurrentRank];
			string textureFromBundle = "ui/babeltower/rank_" + text;
			if (this.m_BasicInfo.CurrentRank != 0)
			{
				this.m_dtRank.SetTextureFromBundle(textureFromBundle);
				this.m_bShowRank = true;
			}
			this.m_dtRank.Visible = false;
			this.m_dtWiinbg.Visible = false;
			string text2 = string.Empty;
			if (this.m_BasicInfo.BeforeRank == 0)
			{
				if (this.m_BasicInfo.CurrentRank > 0)
				{
					text2 = "Effect/Instant/fx_result_grade_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				}
			}
			else if ((int)this.m_BasicInfo.BeforeRank < this.m_BasicInfo.CurrentRank)
			{
				text2 = "Effect/Instant/fx_result_upgrade_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_bRankUPgrade = true;
			}
			if (text2 != string.Empty)
			{
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(text2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.BattleRankEffect), text);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
				this.RankEffectSet = true;
			}
		}
		this.m_fBattleTime = this.m_BasicInfo.fBattleTime;
	}

	public void _LinkSRewardDataSelect()
	{
		this.m_fAutoCloseTime = Time.realtimeSinceStartup + 3f;
		if (this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			BATTLE_SREWARD battleSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleSRewardData(this.m_SReward_BasicInfo.m_nRewardUnique);
			if (battleSRewardData == null)
			{
				return;
			}
			if (this.m_SReward_BasicInfo.m_nRewardProductIndex >= 4)
			{
				return;
			}
			SREWARD_PRODUCT sREWARD_PRODUCT = battleSRewardData.m_sRewardProduct[this.m_SReward_BasicInfo.m_nRewardProductIndex];
			if (sREWARD_PRODUCT == null)
			{
				return;
			}
			this.m_SelectSRewardProduct = sREWARD_PRODUCT;
			Transform child = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, "fx_selecttext");
			if (child != null)
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
			this.m_RewardNotify.Visible = false;
			if (this.m_SelectSRewardGameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SelectSRewardGameObject);
				this.m_SelectSRewardGameObject = null;
			}
			WWWItem wWWItem = NkEffectManager.CreateWItem("FX_REWARDGET");
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.NewShowSRewardGet), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.sRewardImageKey = sREWARD_PRODUCT.m_stRewardTexture;
			if (TsPlatform.IsMobile)
			{
				this.sRewardImageKey += "_mobile";
			}
			switch (sREWARD_PRODUCT.m_nRewardType)
			{
			case 0:
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1826"),
					"count",
					sREWARD_PRODUCT.m_nRewardValue1.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty);
				break;
			}
			case 1:
			{
				string empty2 = string.Empty;
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(sREWARD_PRODUCT.m_nRewardValue1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1827"),
					"itemname",
					itemNameByItemUnique,
					"num",
					sREWARD_PRODUCT.m_nRewardValue2.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty2);
				break;
			}
			case 2:
			{
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1911"),
					"money",
					sREWARD_PRODUCT.m_nRewardValue1.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty3);
				break;
			}
			}
			this.m_RewardExplainLabel.SetCharacterSize(50f);
		}
		else
		{
			BATTLE_BABEL_SREWARD battleBabelSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleBabelSRewardData(this.m_SReward_BasicInfo.m_nRewardUnique);
			if (battleBabelSRewardData == null)
			{
				return;
			}
			if (this.m_SReward_BasicInfo.m_nRewardProductIndex >= 4)
			{
				return;
			}
			SREWARD_PRODUCT sREWARD_PRODUCT2 = battleBabelSRewardData.m_sRewardProduct[this.m_SReward_BasicInfo.m_nRewardProductIndex];
			if (sREWARD_PRODUCT2 == null)
			{
				return;
			}
			this.m_SelectSRewardProduct = sREWARD_PRODUCT2;
			Transform child2 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, "fx_selecttext");
			if (child2 != null)
			{
				UnityEngine.Object.DestroyImmediate(child2.gameObject);
			}
			this.m_RewardNotify.Visible = false;
			if (this.m_SelectSRewardGameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SelectSRewardGameObject);
				this.m_SelectSRewardGameObject = null;
			}
			WWWItem wWWItem2 = NkEffectManager.CreateWItem("FX_REWARDGET");
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.NewShowSRewardGet), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
			this.sRewardImageKey = sREWARD_PRODUCT2.m_stRewardTexture;
			if (TsPlatform.IsMobile)
			{
				this.sRewardImageKey += "_mobile";
			}
			switch (sREWARD_PRODUCT2.m_nRewardType)
			{
			case 0:
			{
				string empty4 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1826"),
					"count",
					sREWARD_PRODUCT2.m_nRewardValue1.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty4);
				break;
			}
			case 1:
			{
				string empty5 = string.Empty;
				string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(sREWARD_PRODUCT2.m_nRewardValue1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty5, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1827"),
					"itemname",
					itemNameByItemUnique2,
					"num",
					sREWARD_PRODUCT2.m_nRewardValue2.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty5);
				break;
			}
			case 2:
			{
				string empty6 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty6, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1911"),
					"money",
					sREWARD_PRODUCT2.m_nRewardValue1.ToString()
				});
				this.m_RewardExplainLabel.SetText(empty6);
				break;
			}
			}
			this.m_RewardExplainLabel.SetCharacterSize(50f);
			if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
			{
				NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_SELECT_SPECIAL_COMPLETE, Time.realtimeSinceStartup);
			}
		}
	}

	private void ShowSRewardGet(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_SRewardGetGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_SRewardGetGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_SRewardGetGameObject, GUICamera.UILayer);
				Texture2D texture = (Texture2D)Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/Battle_Result/" + this.sRewardImageKey);
				NrTSingleton<UIImageBundleManager>.Instance.AddTexture(this.sRewardImageKey, texture);
				if (null != this.m_SRewardGetGameObject)
				{
					this.m_SRewardImageGameObject = NkUtil.GetChild(this.m_SRewardGetGameObject.transform, "rewardcard").gameObject;
					if (null != this.m_SRewardImageGameObject)
					{
						Texture2D texture2 = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.sRewardImageKey);
						if (null != texture2)
						{
							MeshRenderer component = this.m_SRewardImageGameObject.GetComponent<MeshRenderer>();
							if (component != null)
							{
								Material material = component.material;
								if (null != material)
								{
									material.mainTexture = texture2;
									if (null != this.m_SRewardImageGameObject.renderer)
									{
										this.m_SRewardImageGameObject.renderer.sharedMaterial = material;
									}
								}
							}
						}
					}
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SRewardGetGameObject);
					}
					this.Show();
					this.ShowSRewardDlgNext(true);
				}
			}
		}
	}

	private void ShowSReward(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_SelectSRewardGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_SelectSRewardGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_SelectSRewardGameObject, GUICamera.UILayer);
				for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
				{
					string strName = string.Format("card{0}", (i + 1).ToString());
					Transform child = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName);
					if (child != null)
					{
						GameObject gameObject2 = child.gameObject;
						if (gameObject2 != null)
						{
							NmSpecialReward nmSpecialReward = gameObject2.GetComponent<NmSpecialReward>();
							if (nmSpecialReward == null)
							{
								nmSpecialReward = gameObject2.AddComponent<NmSpecialReward>();
							}
							else
							{
								Animation component = gameObject2.GetComponent<Animation>();
								if (component != null)
								{
									if (component.isPlaying)
									{
										component.Stop();
									}
									string animation = string.Format("card{0}_off", (i + 1).ToString());
									component.Play(animation);
								}
							}
							nmSpecialReward.SetData(this, i);
						}
					}
				}
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SelectSRewardGameObject);
				}
				this.Show();
			}
		}
	}

	private void _LinkSRewardData(int BattleSRewardUnique)
	{
		WWWItem wWWItem = NkEffectManager.CreateWItem("FX_REWARD");
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.NewShowSReward), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void ShowSRewardDlg(int BattleSRewardUnique)
	{
		base.AllHideLayer();
		this.m_BattleSRewardUnique = BattleSRewardUnique;
		base.ShowLayer(Battle_ResultDlg_Content.BATTLE_SREWARD_LAYER_NUM);
		this._LinkSRewardData(BattleSRewardUnique);
		this.ShowSRewardDlgNext(false);
	}

	public void ShowSRewardDlgNext(bool bShow)
	{
		this.m_RewardExplainLabel.Visible = bShow;
		this.m_RewardOKButton.Visible = bShow;
	}

	private void NewShowSReward(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				float x = (GUICamera.width - this.m_RewardNotify.width) / 2f;
				if (Screen.width == 1024)
				{
					this.m_RewardNotify.SetLocation(x, 760f);
				}
				else if (Screen.width == 960)
				{
					this.m_RewardNotify.SetLocation(x, 660f);
				}
				else
				{
					this.m_RewardNotify.SetLocation(x, 630f);
				}
				this.m_RewardNotify.Visible = true;
				if (this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
				{
					BATTLE_SREWARD battleSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleSRewardData(this.m_BasicInfo.BattleSRewardUnique);
					this.m_SelectSRewardGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
					Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
					Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
					this.m_SelectSRewardGameObject.transform.position = effectUIPos;
					NkUtil.SetAllChildLayer(this.m_SelectSRewardGameObject, GUICamera.UILayer);
					Vector3 localPosition = new Vector3(0.162f, 0f, 0f);
					Vector3 localEulerAngles = new Vector3(0f, 0f, 0f);
					Vector3 localScale = new Vector3(0f, 0f, 0f);
					string strName = string.Empty;
					for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
					{
						strName = string.Format("card{0}", (i + 1).ToString());
						Transform child = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName);
						if (child != null)
						{
							GameObject gameObject2 = child.gameObject;
							if (gameObject2 != null)
							{
								NmSpecialReward nmSpecialReward = gameObject2.GetComponent<NmSpecialReward>();
								if (nmSpecialReward == null)
								{
									nmSpecialReward = gameObject2.AddComponent<NmSpecialReward>();
								}
								else
								{
									Animation component = gameObject2.GetComponent<Animation>();
									if (component != null)
									{
										if (component.isPlaying)
										{
											component.Stop();
										}
										string animation = string.Format("card0{0}_off", (i + 1).ToString());
										component.Play(animation);
									}
								}
								nmSpecialReward.SetData(this, i);
							}
						}
						strName = string.Format("card0{0}up", (i + 1).ToString());
						Transform child2 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName);
						if (child2 != null)
						{
							this.SetRewardItemInfo(child2.gameObject, battleSRewardData.m_sRewardProduct[i], i, this.m_lbItemNum[i]);
						}
						strName = string.Format("card0{0}down", i + 1);
						Transform child3 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName);
						if (child3 != null)
						{
							this.m_lbItemNum[i].Hide(false);
							this.m_lbItemNum[i].gameObject.transform.parent = child3;
							if (Vector3.zero == localEulerAngles && Vector3.zero != this.m_lbItemNum[i].gameObject.transform.localEulerAngles)
							{
								localEulerAngles = this.m_lbItemNum[i].gameObject.transform.localEulerAngles;
							}
							if (Vector3.zero == localScale && Vector3.zero != this.m_lbItemNum[i].gameObject.transform.localScale)
							{
								localScale = this.m_lbItemNum[i].gameObject.transform.localScale;
							}
						}
					}
					for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
					{
						this.m_lbItemNum[i].gameObject.transform.localPosition = localPosition;
						this.m_lbItemNum[i].gameObject.transform.localEulerAngles = localEulerAngles;
						this.m_lbItemNum[i].gameObject.transform.localScale = localScale;
					}
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SelectSRewardGameObject);
					}
					this.Show();
				}
				else
				{
					BATTLE_BABEL_SREWARD battleBabelSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleBabelSRewardData(this.m_BasicInfo.BattleSRewardUnique);
					this.m_SelectSRewardGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
					Vector2 screenPos2 = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
					Vector3 effectUIPos2 = base.GetEffectUIPos(screenPos2);
					this.m_SelectSRewardGameObject.transform.position = effectUIPos2;
					NkUtil.SetAllChildLayer(this.m_SelectSRewardGameObject, GUICamera.UILayer);
					Vector3 localPosition2 = new Vector3(0.162f, 0f, 0f);
					Vector3 localEulerAngles2 = new Vector3(0f, 0f, 0f);
					Vector3 localScale2 = new Vector3(0f, 0f, 0f);
					string strName2 = string.Empty;
					for (int j = 0; j < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; j++)
					{
						strName2 = string.Format("card{0}", (j + 1).ToString());
						Transform child4 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName2);
						if (child4 != null)
						{
							GameObject gameObject3 = child4.gameObject;
							if (gameObject3 != null)
							{
								NmSpecialReward nmSpecialReward2 = gameObject3.GetComponent<NmSpecialReward>();
								if (nmSpecialReward2 == null)
								{
									nmSpecialReward2 = gameObject3.AddComponent<NmSpecialReward>();
								}
								else
								{
									Animation component2 = gameObject3.GetComponent<Animation>();
									if (component2 != null)
									{
										if (component2.isPlaying)
										{
											component2.Stop();
										}
										string animation2 = string.Format("card0{0}_off", (j + 1).ToString());
										component2.Play(animation2);
									}
								}
								if (!NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
								{
									nmSpecialReward2.SetData(this, j);
								}
							}
						}
						strName2 = string.Format("card0{0}up", (j + 1).ToString());
						Transform child5 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName2);
						if (child5 != null)
						{
							this.SetRewardItemInfo(child5.gameObject, battleBabelSRewardData.m_sRewardProduct[j], j, this.m_lbItemNum[j]);
						}
						strName2 = string.Format("card0{0}down", j + 1);
						Transform child6 = NkUtil.GetChild(this.m_SelectSRewardGameObject.transform, strName2);
						if (child6 != null)
						{
							this.m_lbItemNum[j].Hide(false);
							this.m_lbItemNum[j].gameObject.transform.parent = child6;
							if (Vector3.zero == localEulerAngles2 && Vector3.zero != this.m_lbItemNum[j].gameObject.transform.localEulerAngles)
							{
								localEulerAngles2 = this.m_lbItemNum[j].gameObject.transform.localEulerAngles;
							}
							if (Vector3.zero == localScale2 && Vector3.zero != this.m_lbItemNum[j].gameObject.transform.localScale)
							{
								localScale2 = this.m_lbItemNum[j].gameObject.transform.localScale;
							}
						}
					}
					for (int j = 0; j < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; j++)
					{
						this.m_lbItemNum[j].gameObject.transform.localPosition = localPosition2;
						this.m_lbItemNum[j].gameObject.transform.localEulerAngles = localEulerAngles2;
						this.m_lbItemNum[j].gameObject.transform.localScale = localScale2;
					}
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SelectSRewardGameObject);
					}
					this.Show();
					if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
					{
						NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_BATTLE_SELECT_SPECIAL_RESULT, Time.realtimeSinceStartup);
					}
				}
			}
		}
	}

	public void SetRewardItemInfo(GameObject goCard, SREWARD_PRODUCT SRewardProduct, int iIndex, Label lbItemNum)
	{
		if (SRewardProduct != null)
		{
			this.SetItemTexture(goCard, SRewardProduct.m_stRewardTexture);
			switch (SRewardProduct.m_nRewardType)
			{
			case 0:
				lbItemNum.SetText(string.Format("{0}{1} x{2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("232"), SRewardProduct.m_nRewardValue1));
				break;
			case 1:
				lbItemNum.SetText(string.Format("{0}{1} x{2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(SRewardProduct.m_nRewardValue1), SRewardProduct.m_nRewardValue2));
				break;
			case 2:
				lbItemNum.SetText(string.Format("{0}{1} +{2}", NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676"), SRewardProduct.m_nRewardValue1));
				break;
			}
		}
	}

	public void SetItemTexture(GameObject gbItem, string strItemTexture)
	{
		Texture2D texture2D;
		if (TsPlatform.IsMobile)
		{
			texture2D = (Texture2D)Resources.Load(string.Format("{0}{1}{2}_mobile", NrTSingleton<UIDataManager>.Instance.FilePath, "Texture/Battle_Result/", strItemTexture));
		}
		else
		{
			texture2D = (Texture2D)Resources.Load(string.Format("{0}{1}{2}", NrTSingleton<UIDataManager>.Instance.FilePath, "Texture/Battle_Result/", strItemTexture));
		}
		if (null == texture2D)
		{
			return;
		}
		NrTSingleton<UIImageBundleManager>.Instance.AddTexture(strItemTexture, texture2D);
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(strItemTexture);
		if (null != texture)
		{
			MeshRenderer component = gbItem.GetComponent<MeshRenderer>();
			if (component != null)
			{
				Material material = component.material;
				if (null != material)
				{
					material.mainTexture = texture;
					if (null != gbItem.renderer)
					{
						gbItem.renderer.sharedMaterial = material;
					}
				}
			}
		}
	}

	private void NewShowSRewardGet(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_SRewardGetGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_SRewardGetGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_SRewardGetGameObject, GUICamera.UILayer);
				if (null != this.m_SRewardGetGameObject)
				{
					if (this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && this.m_eRoomType != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
					{
						BATTLE_SREWARD battleSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleSRewardData(this.m_SReward_BasicInfo.m_nRewardUnique);
						List<SREWARD_PRODUCT> list = new List<SREWARD_PRODUCT>();
						for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
						{
							list.Add(battleSRewardData.m_sRewardProduct[i]);
						}
						if (this.m_iSelectIndex != this.m_SReward_BasicInfo.m_nRewardProductIndex)
						{
							list[this.m_iSelectIndex] = battleSRewardData.m_sRewardProduct[this.m_SReward_BasicInfo.m_nRewardProductIndex];
							list[this.m_SReward_BasicInfo.m_nRewardProductIndex] = battleSRewardData.m_sRewardProduct[this.m_iSelectIndex];
						}
						this.NewShowSRewardGetSetItem(list);
					}
					else
					{
						BATTLE_BABEL_SREWARD battleBabelSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleBabelSRewardData(this.m_SReward_BasicInfo.m_nRewardUnique);
						List<SREWARD_PRODUCT> list2 = new List<SREWARD_PRODUCT>();
						for (int j = 0; j < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; j++)
						{
							list2.Add(battleBabelSRewardData.m_sRewardProduct[j]);
						}
						if (this.m_iSelectIndex != this.m_SReward_BasicInfo.m_nRewardProductIndex)
						{
							list2[this.m_iSelectIndex] = battleBabelSRewardData.m_sRewardProduct[this.m_SReward_BasicInfo.m_nRewardProductIndex];
							list2[this.m_SReward_BasicInfo.m_nRewardProductIndex] = battleBabelSRewardData.m_sRewardProduct[this.m_iSelectIndex];
						}
						this.NewShowSRewardGetSetItem(list2);
					}
				}
			}
		}
	}

	private void NewShowSRewardGetSub(string strObjectName, GameObject gb, SREWARD_PRODUCT SRewardProduct, int iIndex, Label lbItemNum)
	{
		Transform child = NkUtil.GetChild(gb.transform, strObjectName);
		if (child != null)
		{
			this.SetRewardItemInfo(child.gameObject, SRewardProduct, iIndex, lbItemNum);
		}
	}

	private void BattleRankEffect(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle())
		{
			if (null != _item.GetSafeBundle().mainAsset)
			{
				GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
				if (null != gameObject)
				{
					string str = _param as string;
					this.m_goRankEffectObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
					UnityEngine.Object.DontDestroyOnLoad(this.m_goRankEffectObject);
					if (this == null)
					{
						UnityEngine.Object.DestroyImmediate(this.m_goRankEffectObject);
						return;
					}
					Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
					Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
					this.m_goRankEffectObject.transform.position = effectUIPos;
					NkUtil.SetAllChildLayer(this.m_goRankEffectObject, GUICamera.UILayer);
					this.m_goRankEffectObject.SetActive(false);
					Animation componentInChildren = this.m_goRankEffectObject.GetComponentInChildren<Animation>();
					if (componentInChildren != null)
					{
						componentInChildren.Stop();
					}
					string path = "UI/BabelTower/Rank_" + str + NrTSingleton<UIDataManager>.Instance.AddFilePath;
					NrTSingleton<FormsManager>.Instance.RequestUIBundleDownLoad(path, new PostProcPerItem(this.LoadCompleteRankTexture), this.m_goRankEffectObject);
					if (this.m_bRankUPgrade)
					{
						string str2 = BATTLE_DEFINE.RANK_STRING[(int)this.m_BasicInfo.BeforeRank];
						string path2 = "UI/BabelTower/Rank_" + str2 + NrTSingleton<UIDataManager>.Instance.AddFilePath;
						NrTSingleton<FormsManager>.Instance.RequestUIBundleDownLoad(path2, new PostProcPerItem(this.LoadCompleteBeforeRankTexture), this.m_goRankEffectObject);
					}
					if (TsPlatform.IsMobile && TsPlatform.IsEditor)
					{
						NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goRankEffectObject);
					}
				}
			}
			else
			{
				Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
				if (battle_ResultDlg != null)
				{
					battle_ResultDlg.ResultFxTime = Time.realtimeSinceStartup;
				}
			}
		}
		else
		{
			Battle_ResultDlg battle_ResultDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
			if (battle_ResultDlg2 != null)
			{
				battle_ResultDlg2.ResultFxTime = Time.realtimeSinceStartup;
			}
		}
	}

	private void LoadCompleteRankTexture(WWWItem _item, object _param)
	{
		if (_item != null && _item.canAccessAssetBundle)
		{
			Texture2D texture2D = _item.mainAsset as Texture2D;
			if (texture2D != null)
			{
				GameObject gameObject = NkUtil.GetChild(this.m_goRankEffectObject.transform, "fx_plan_grade").gameObject;
				if (gameObject != null)
				{
					gameObject.renderer.sharedMaterial.mainTexture = texture2D;
					if (!this.m_bRankUPgrade)
					{
						this.m_fRankEffectShow = Time.realtimeSinceStartup + 0.2f;
					}
					else if (this.m_bSetUpgradeTexture)
					{
						this.m_fRankEffectShow = Time.realtimeSinceStartup + 0.2f;
					}
					else
					{
						this.m_bSetUpgradeTexture = true;
					}
				}
			}
			else
			{
				Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
				if (battle_ResultDlg != null)
				{
					battle_ResultDlg.ResultFxTime = Time.realtimeSinceStartup;
				}
			}
		}
		else
		{
			Battle_ResultDlg battle_ResultDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
			if (battle_ResultDlg2 != null)
			{
				battle_ResultDlg2.ResultFxTime = Time.realtimeSinceStartup;
			}
		}
	}

	private void LoadCompleteBeforeRankTexture(WWWItem _item, object _param)
	{
		if (_item != null && _item.canAccessAssetBundle)
		{
			Texture2D texture2D = _item.mainAsset as Texture2D;
			if (texture2D != null)
			{
				GameObject gameObject = NkUtil.GetChild(this.m_goRankEffectObject.transform, "fx_plan01").gameObject;
				if (gameObject != null)
				{
					gameObject.renderer.sharedMaterial.mainTexture = texture2D;
					if (this.m_bSetUpgradeTexture)
					{
						this.m_fRankEffectShow = Time.realtimeSinceStartup + 0.2f;
					}
					else
					{
						this.m_bSetUpgradeTexture = true;
					}
				}
			}
		}
	}

	private Texture2D GetPortraitLeaderSol(int iCharKind)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && kMyCharInfo.UserPortrait)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser.GetCharKind() == iCharKind)
			{
				return kMyCharInfo.UserPortraitTexture;
			}
		}
		return null;
	}

	public void NewShowSRewardGetSetItem(List<SREWARD_PRODUCT> SRewardList)
	{
		string text = string.Empty;
		Vector3 localPosition = new Vector3(-130f, 0f, 0f);
		Vector3 localEulerAngles = new Vector3(0f, 0f, 0f);
		Vector3 localScale = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
		{
			text = string.Format("card0{0}up", (i + 1).ToString());
			this.NewShowSRewardGetSub(text, this.m_SRewardGetGameObject, SRewardList[i], i, this.m_lbItemNumGet[i]);
			text = string.Format("fx_dummy0{0}", (i + 1).ToString());
			Transform child = NkUtil.GetChild(this.m_SRewardGetGameObject.transform, text);
			if (child != null)
			{
				Vector3 localScale2 = child.transform.localScale;
				localScale2.z = 1f;
				child.transform.localScale = localScale2;
				this.m_lbItemNumGet[i].Hide(false);
				this.m_lbItemNumGet[i].gameObject.transform.parent = child;
				if (Vector3.zero == localEulerAngles && Vector3.zero != this.m_lbItemNumGet[i].gameObject.transform.localEulerAngles)
				{
					localEulerAngles = this.m_lbItemNumGet[i].gameObject.transform.localEulerAngles;
				}
				if (Vector3.zero == localScale && Vector3.zero != this.m_lbItemNumGet[i].gameObject.transform.localScale)
				{
					localScale = this.m_lbItemNumGet[i].gameObject.transform.localScale;
				}
			}
		}
		for (int i = 0; i < Battle_ResultDlg_Content.BATTLE_SREWARD_BUTTON_MAX; i++)
		{
			this.m_lbItemNumGet[i].gameObject.transform.localPosition = localPosition;
			this.m_lbItemNumGet[i].gameObject.transform.localEulerAngles = localEulerAngles;
			this.m_lbItemNumGet[i].gameObject.transform.localScale = localScale;
		}
		text = "reward";
		Transform child2 = NkUtil.GetChild(this.m_SRewardGetGameObject.transform, text);
		if (child2 != null)
		{
			Animation component = child2.gameObject.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				text = string.Format("fx_get_card{0}", this.m_iSelectIndex + 1);
				component.Play(text);
			}
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SRewardGetGameObject);
		}
		this.Show();
		this.ShowSRewardDlgNext(true);
	}

	public void effectDelete(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_CloseEffect = obj;
	}
}
