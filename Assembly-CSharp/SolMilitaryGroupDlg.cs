using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using Ts;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolMilitaryGroupDlg : Form
{
	private enum eSolMilitaryGroupLayer
	{
		SOLMODE_MILITARYGROUP,
		SOLMODE_MILITARYLIST,
		SOLMODE_READYSOLLIST,
		SOLMODE_MILITARYINFO,
		SOLMODE_EQUIPITEM,
		SOLMODE_SKILLINFO,
		SOLMODE_CUREINFO,
		SOLMODE_WAREHOUSE,
		SOLMODE_MINE_MILITARY
	}

	private enum eSolMilitaryGroupTab
	{
		SOLTAB_SOLLIST_BATTLE,
		SOLTAB_SOLLIST_READY,
		SOLTAB_SOLLIST_WAREHOUSE,
		MAX_SOLMILITARY_TAB_NUM
	}

	private enum eSOLWAREHOUSESTATE
	{
		eSOLWAREHOUSESTATE_EMPTY = -1,
		eSOLWAREHOUSESTATE_EXPANSION = -2
	}

	private enum eSolSortOrder
	{
		SORTORDER_CPOWERDESC,
		SORTORDER_CPOWERASC,
		SORTORDER_LEVELDESC,
		SORTORDER_LEVELASC,
		SORTORDER_NAME,
		SORTORDER_GRADEDESC,
		SORTORDER_GRADEASC,
		SORTORDER_FIGHTINGPOWERDESC,
		SORTORDER_FIGHTINGPOWERASC
	}

	private class SolSkillSlotControl
	{
		private DrawTexture SkillSlotIcon;

		private DrawTexture SkillSlotEmpty;

		private Button SkillSlot;

		private Button SkillUpdate;

		private Label SkillName;

		private Label SkillLevel;

		private int nSlotIndex;

		private bool bEmptySlot;

		private BATTLESKILL_TRAINING pkSkillinfo;

		public void SetComponent(SolMilitaryGroupDlg parent, int index)
		{
			this.nSlotIndex = index;
			string str = (this.nSlotIndex + 1).ToString();
			this.SkillSlotIcon = (parent.GetControl("skillicon_0" + str) as DrawTexture);
			this.SkillSlotEmpty = (parent.GetControl("skill_slot_emty0" + str) as DrawTexture);
			this.SkillSlot = (parent.GetControl("skill_slot0" + str) as Button);
			this.SkillSlot.data = this.nSlotIndex;
			this.SkillSlot.AddValueChangedDelegate(new EZValueChangedDelegate(parent.OnClickSkillUpdate));
			this.SkillUpdate = (parent.GetControl("btn_skillup0" + str) as Button);
			this.SkillUpdate.data = this.nSlotIndex;
			this.SkillUpdate.AddValueChangedDelegate(new EZValueChangedDelegate(parent.OnClickSkillUpdate));
			this.SkillName = (parent.GetControl("skill_skillname0" + str) as Label);
			this.SkillLevel = (parent.GetControl("skill_skilllevel0" + str) as Label);
		}

		public void SetEmpty(bool bEmpty, bool bForce)
		{
			if (bForce || this.bEmptySlot != bEmpty)
			{
				this.bEmptySlot = bEmpty;
				this.SkillSlotIcon.Visible = !this.bEmptySlot;
				this.SkillSlotEmpty.Visible = !this.bEmptySlot;
				this.SkillSlot.Visible = !this.bEmptySlot;
				this.SkillUpdate.Visible = !this.bEmptySlot;
				this.SkillName.Visible = !this.bEmptySlot;
				this.SkillLevel.Visible = !this.bEmptySlot;
			}
		}

		public void SetSkillInfo(BATTLESKILL_BASE pkSkillBase, int currentlevel, int sollevel, int limitlevel, bool bEnableUpdate)
		{
			if (pkSkillBase == null)
			{
				return;
			}
			this.SetEmpty(false, true);
			string empty = string.Empty;
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(pkSkillBase.m_nSkillUnique);
			this.SkillSlotIcon.SetTexture(battleSkillIconTexture);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
				"skillname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(pkSkillBase.m_strTextKey)
			});
			this.SkillName.Text = empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1885"),
				"skilllevel",
				currentlevel.ToString()
			});
			this.SkillLevel.Text = empty;
			this.SkillUpdate.Visible = bEnableUpdate;
		}

		public void SetTrainingSkillInfo(BATTLESKILL_TRAINING skillinfo)
		{
			this.pkSkillinfo = skillinfo;
		}

		public BATTLESKILL_TRAINING GetTrainingSkillInfo()
		{
			return this.pkSkillinfo;
		}

		public bool IsEnableUpdate()
		{
			return this.SkillUpdate.Visible;
		}
	}

	public const int LIST_COUNT = 7;

	private DrawTexture m_dtEffect;

	private Toggle[] SolModeToggle;

	private Label[] m_lbTab = new Label[3];

	private DrawTexture[] SolModeSkillCountImage;

	private Label[] SolModeSkillCountNum;

	private Button RegistMilitaryButton;

	private NewListBox mBattleSolList;

	private NewListBox mReadySolList;

	private DropDownList m_dlReadySolSort;

	private ImageView[] SolEquipItem;

	private Label SoldierSpec;

	private Label ItemSolStatHP;

	private Label ItemSolStatDamage;

	private Label ItemSolStatDefence;

	private Label ItemSolStatMagicDefence;

	private Label ItemSolStatSeason;

	private Label m_lbInitiativeValue;

	private Label SelectSolName;

	private DrawTexture m_DrawTexture_rank;

	private DrawTexture SelectSolImage;

	private DrawTexture GradeExpBG;

	private DrawTexture GradeExpGage;

	private Label GradeExpText;

	private Button ChangeFaceChar;

	private Button ViewSolDetailInfo1;

	private SolDetailDlgTool m_SolInterfaceTool = new SolDetailDlgTool();

	private Button m_btReincarnation;

	private DrawTexture m_dtRank2;

	private DrawTexture m_dtReincarnationEffect;

	private SolMilitaryGroupDlg.SolSkillSlotControl kSolSkillSlotControl;

	private DrawTexture SolInjuryImage;

	private Label SolCureLabel;

	private Button SolCureButton;

	private Button SolAllCureButton;

	private NewListBox m_nlbWarehouseList;

	private DropDownList m_dlWarehouseSolSort;

	private DrawTexture m_dtRingslotlock;

	private Label m_lbMilitary;

	private int nCurrentSolPosType = 1;

	private NkSoldierInfo mSelectedSolinfo;

	private bool bShowSoldierEquip = true;

	private bool bRefreshSolinfo;

	private bool bRequest;

	private bool bSetFace;

	private bool bLoadChangeFaceChar;

	private GameObject rootGameObject;

	private float fStartTime;

	private float fSolCureTime;

	private int nInjurySoldierCount;

	private int nInjuryBattleSoldierCount;

	private int nInjuryReadySoldierCount;

	private bool bSetFirstItem;

	private long nCurrentAgreeState;

	private NkSoldierInfo mLeaderSolinfo;

	private float MAX_EXP_GAGUE;

	private float READY_LIST_MAX_EXP_GAGUE = 236f;

	private float BATTLE_LIST_MAX_EXP_GAGUE = 236f;

	private List<NkSoldierInfo> m_kBattleSolSortList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kReadySolSortList = new List<NkSoldierInfo>();

	private string[] m_strTab = new string[3];

	private int m_iSolWarehouseMax;

	private int m_iTabIndex;

	private bool m_bRefreshWarehouse = true;

	private float TEX_SIZE = 512f;

	private int m_iReadySortOrder;

	private int m_iWarehouseSortOrder = 2;

	private bool m_bRefreshList;

	private bool m_bChangeFaceChar;

	private int m_nChangeFaceCharKind;

	private int m_nFaceSolPosIndex;

	private string faceImageKey = string.Empty;

	private List<NkSoldierInfo> SORT_LIST
	{
		get
		{
			if (this.nCurrentSolPosType == 1)
			{
				return this.m_kBattleSolSortList;
			}
			return this.m_kReadySolSortList;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.ShowHide = true;
		if (TsPlatform.IsIPhone && TsPlatform.IsLowSystemMemory)
		{
			form.ShowHide = false;
		}
		instance.LoadFileAll(ref form, "Soldier/DLG_SolMilitaryGroup", G_ID.SOLMILITARYGROUP_DLG, true);
		form.AlwaysUpdate = true;
		base.ShowBlackBG(0.5f);
	}

	private void SetEquipItemImageView(string controlKey, eEQUIP_ITEM index)
	{
		this.SolEquipItem[(int)index] = (base.GetControl(controlKey) as ImageView);
		this.SolEquipItem[(int)index].SetImageView(0, 0, 80, 80, 0, 0, (int)this.SolEquipItem[(int)index].GetSize().y);
		this.SolEquipItem[(int)index].itemSpacing = 0f;
		this.SolEquipItem[(int)index].clipContents = false;
		this.SolEquipItem[(int)index].Data = (int)index;
	}

	public override void SetComponent()
	{
		string str = string.Empty;
		this.m_iReadySortOrder = PlayerPrefs.GetInt(NrPrefsKey.SOLMILITARYGROUP_READY_SORT, 0);
		this.m_iWarehouseSortOrder = PlayerPrefs.GetInt(NrPrefsKey.SOLMILITARYGROUP_WAREHOUSE_SORT, 2);
		this.m_dtEffect = (base.GetControl("DrawTexture_Effect") as DrawTexture);
		this.m_dtEffect.SetLocationZ(-1f);
		this.SolModeToggle = new Toggle[3];
		this.SolModeSkillCountImage = new DrawTexture[3];
		this.SolModeSkillCountNum = new Label[3];
		for (int i = 0; i < 3; i++)
		{
			str = (i + 1).ToString();
			this.SolModeToggle[i] = (base.GetControl("tab0" + str) as Toggle);
			this.SolModeToggle[i].data = i;
			this.SolModeToggle[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
			this.m_lbTab[i] = (base.GetControl("label_tab0" + str) as Label);
			if (i == 0 || i == 1)
			{
				this.SolModeSkillCountImage[i] = (base.GetControl("DrawTexture_Notice0" + str) as DrawTexture);
				this.SolModeSkillCountImage[i].SetLocation(this.SolModeSkillCountImage[i].GetLocationX(), this.SolModeSkillCountImage[i].GetLocationY(), this.SolModeToggle[i].GetLocation().z - 0.1f);
				this.SolModeSkillCountImage[i].Visible = false;
				this.SolModeSkillCountNum[i] = (base.GetControl("Label_Notice" + str) as Label);
				this.SolModeSkillCountNum[i].SetLocation(this.SolModeSkillCountNum[i].GetLocationX(), this.SolModeSkillCountNum[i].GetLocationY(), this.SolModeSkillCountImage[i].GetLocation().z - 0.1f);
				this.SolModeSkillCountNum[i].Visible = false;
			}
		}
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
		Label label = base.GetControl("label_tab02") as Label;
		label.Text = NrTSingleton<UIDataManager>.Instance.GetString(textColor, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1229"));
		this.RegistMilitaryButton = (base.GetControl("btn_position") as Button);
		this.RegistMilitaryButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMakeMilitary));
		this.mBattleSolList = (base.GetControl("ListBox_battle_group") as NewListBox);
		this.mBattleSolList.touchScroll = false;
		this.mBattleSolList.Reserve = false;
		this.mBattleSolList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierView));
		this.mReadySolList = (base.GetControl("ListBox_standby") as NewListBox);
		this.mReadySolList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierView));
		this.m_dlReadySolSort = (base.GetControl("DropDown_StanbySort") as DropDownList);
		this.m_dlReadySolSort.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeReadySortOrder));
		this.m_dlReadySolSort.SetVisible(false);
		this.SolEquipItem = new ImageView[6];
		this.SetEquipItemImageView("item_head", eEQUIP_ITEM.EQUIP_HELMET);
		this.SetEquipItemImageView("item_armor", eEQUIP_ITEM.EQUIP_ARMOR);
		this.SetEquipItemImageView("item_glove", eEQUIP_ITEM.EQUIP_GLOVE);
		this.SetEquipItemImageView("item_boots", eEQUIP_ITEM.EQUIP_BOOTS);
		this.SetEquipItemImageView("item_weapon", eEQUIP_ITEM.EQUIP_WEAPON1);
		this.SetEquipItemImageView("item_ring", eEQUIP_ITEM.EQUIP_RING);
		this.SoldierSpec = (base.GetControl("Label_soldierspec2") as Label);
		this.m_lbInitiativeValue = (base.GetControl("Label_InitiativeValue") as Label);
		this.ItemSolStatHP = (base.GetControl("Label_stats_HP2") as Label);
		this.ItemSolStatDamage = (base.GetControl("Label_stats_damage2") as Label);
		this.ItemSolStatDefence = (base.GetControl("Label_stats_defence2") as Label);
		this.ItemSolStatMagicDefence = (base.GetControl("Label_stats_magicdefence2") as Label);
		this.ItemSolStatSeason = (base.GetControl("Label_Season") as Label);
		this.SelectSolName = (base.GetControl("Label_ch_name") as Label);
		this.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank01") as DrawTexture);
		this.SelectSolImage = (base.GetControl("drawtexture_ch_img") as DrawTexture);
		this.GradeExpBG = (base.GetControl("DrawTexture_GradePRGBG") as DrawTexture);
		this.GradeExpGage = (base.GetControl("DrawTexture_GradePRG") as DrawTexture);
		this.MAX_EXP_GAGUE = this.GradeExpGage.GetSize().x;
		this.GradeExpText = (base.GetControl("Label_GradeText") as Label);
		this.ChangeFaceChar = (base.GetControl("btn_boss") as Button);
		this.ChangeFaceChar.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeFaceChar));
		this.ViewSolDetailInfo1 = (base.GetControl("btn_chainfo") as Button);
		this.ViewSolDetailInfo1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSolDetailView));
		this.m_SolInterfaceTool.m_Label_EventDate = (base.GetControl("Label_EventDate") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Event = (base.GetControl("DrawTexture_Event") as DrawTexture);
		for (int j = 0; j < 2; j++)
		{
			this.m_SolInterfaceTool.m_Lebel_EventHero[j] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (j + 1).ToString())) as Label);
		}
		this.m_btReincarnation = (base.GetControl("Button_Reincarnation") as Button);
		this.m_btReincarnation.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickReincarnation));
		this.m_btReincarnation.Hide(true);
		this.m_dtRank2 = (base.GetControl("DrawTexture_rank02") as DrawTexture);
		this.m_dtRank2.Hide(true);
		this.m_dtReincarnationEffect = (base.GetControl("DrawTexture_ReincarnationFX") as DrawTexture);
		this.m_dtReincarnationEffect.Hide(true);
		this.m_dtRingslotlock = (base.GetControl("DrawTexture_ringslotlock") as DrawTexture);
		this.m_dtRingslotlock.SetLocationZ(-1f);
		this.m_dtRingslotlock.Visible = false;
		this.kSolSkillSlotControl = new SolMilitaryGroupDlg.SolSkillSlotControl();
		this.kSolSkillSlotControl.SetComponent(this, 0);
		this.SolInjuryImage = (base.GetControl("drawtexture_injury") as DrawTexture);
		this.SolCureLabel = (base.GetControl("Label_Cure") as Label);
		this.SolCureButton = (base.GetControl("Button_Cure") as Button);
		this.SolCureButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSolCure));
		this.SolAllCureButton = (base.GetControl("btn_allcure") as Button);
		this.SolAllCureButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSolAllCure));
		this.m_nlbWarehouseList = (base.GetControl("ListBox_warehouse") as NewListBox);
		this.m_nlbWarehouseList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickWarehouseList));
		this.m_dlWarehouseSolSort = (base.GetControl("DropDown_WarehouseSort") as DropDownList);
		this.m_dlWarehouseSolSort.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeWarehouseSortOrder));
		this.m_dlWarehouseSolSort.SetVisible(false);
		this.m_lbMilitary = (base.GetControl("Label_Military") as Label);
		this.m_lbMilitary.Visible = false;
		base.SetScreenCenter();
		if (null != this.closeButton)
		{
			this.closeButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.PlayCloseSound));
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			this.m_iSolWarehouseMax = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOL_WAREHOUSE_MAX);
		}
		this.m_strTab[0] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121");
		this.m_strTab[1] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("123");
		this.m_strTab[2] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2179");
		this.SetSortList();
		this.MakeSolWarehouseList(true);
	}

	private void PlayCloseSound(IUIObject obj)
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-INFORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void InitSolList()
	{
		this.mBattleSolList.Clear();
		this.mReadySolList.Clear();
		this.m_kBattleSolSortList.Clear();
		this.m_kReadySolSortList.Clear();
	}

	public override void InitData()
	{
		this.bShowSoldierEquip = true;
		base.SetShowLayer(4, true);
		this.SetTrainingSkillInfo(this.mSelectedSolinfo, true);
		base.SetShowLayer(6, false);
		this.InitLayer();
		this.CheckSkillUpSolNum();
	}

	public void InitLayer()
	{
		this.SetLayerTabControl(this.m_iTabIndex);
	}

	public void CheckSkillUpSolNum()
	{
		int num = 0;
		int num2 = 0;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
			if (0L < soldierInfo.GetSolID())
			{
				if (this.SetTrainingSkillInfo(soldierInfo, false))
				{
					num++;
				}
			}
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (0L < current.GetSolID())
			{
				if (this.SetTrainingSkillInfo(current, false))
				{
					num2++;
				}
			}
		}
		this.SetSkillUpSolNum(num, num2);
	}

	private void SetSkillUpSolNum(int nBattleSolSkillUpCount, int nReadySolSkillUpCount)
	{
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				if (nBattleSolSkillUpCount > 0)
				{
					this.SolModeSkillCountImage[i].Visible = true;
					this.SolModeSkillCountNum[i].SetText(nBattleSolSkillUpCount.ToString());
					this.SolModeSkillCountNum[i].Visible = true;
				}
				else
				{
					this.SolModeSkillCountImage[i].Visible = false;
					this.SolModeSkillCountNum[i].Visible = false;
				}
				break;
			case 1:
				if (nReadySolSkillUpCount > 0)
				{
					this.SolModeSkillCountImage[i].Visible = true;
					this.SolModeSkillCountNum[i].SetText(nReadySolSkillUpCount.ToString());
					this.SolModeSkillCountNum[i].Visible = true;
				}
				else
				{
					this.SolModeSkillCountImage[i].Visible = false;
					this.SolModeSkillCountNum[i].Visible = false;
				}
				break;
			}
		}
	}

	private void OnClickTabControl(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		if (toggle == null || !toggle.Value)
		{
			return;
		}
		this.m_iTabIndex = (int)toggle.data;
		this.SetLayerTabControl(this.m_iTabIndex);
	}

	public override void Hide()
	{
		if (this.mSelectedSolinfo != null)
		{
			this.SelectSolImage.SetTexture("Com_I_Transparent");
			this.mSelectedSolinfo = null;
		}
		base.Hide();
	}

	public override void Show()
	{
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.Visible = true;
		}
		this.OnForceCheckInjury();
		if (this.mSelectedSolinfo != null)
		{
			this.SelectSolImage.SetUVMask(new Rect(4f / this.TEX_SIZE, 0f, 504f / this.TEX_SIZE, 448f / this.TEX_SIZE));
			this.SelectSolImage.SetTextureEffect(eCharImageType.LARGE, this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade());
		}
		base.Show();
		if (this.IsReincarnation())
		{
			this.SetShowReincarnation(true);
		}
		else
		{
			this.SetShowReincarnation(false);
		}
		if (this.mSelectedSolinfo != null)
		{
			this.m_SolInterfaceTool.SetHeroEventLabel(this.mSelectedSolinfo.GetGrade() + 1);
		}
		if (this.m_bRefreshList)
		{
			SolMilitaryGroupDlg.eSolMilitaryGroupTab iTabIndex = (SolMilitaryGroupDlg.eSolMilitaryGroupTab)this.m_iTabIndex;
			if (iTabIndex == SolMilitaryGroupDlg.eSolMilitaryGroupTab.SOLTAB_SOLLIST_BATTLE || iTabIndex == SolMilitaryGroupDlg.eSolMilitaryGroupTab.SOLTAB_SOLLIST_READY)
			{
				this.MakeSolListAndSort();
			}
		}
	}

	public void OnForceCheckInjury()
	{
		this.nInjurySoldierCount++;
		this.nInjuryBattleSoldierCount++;
		this.nInjuryReadySoldierCount++;
	}

	private void SetLayerTabControl(int tabindex)
	{
		if (!this.SolModeToggle[tabindex].Value)
		{
			this.SolModeToggle[tabindex].Value = true;
			return;
		}
		this.m_dlReadySolSort.SetVisible(false);
		this.m_dlReadySolSort.controlIsEnabled = false;
		this.m_dlWarehouseSolSort.SetVisible(false);
		this.m_dlWarehouseSolSort.controlIsEnabled = false;
		switch (tabindex)
		{
		case 0:
			this.nCurrentSolPosType = 1;
			this.nInjurySoldierCount = 0;
			base.SetShowLayer(2, false);
			base.SetShowLayer(3, false);
			base.SetShowLayer(1, true);
			base.SetShowLayer(6, false);
			base.SetShowLayer(7, false);
			base.SetShowLayer(8, false);
			this.m_dtEffect.Visible = false;
			this.ChangeFaceChar.SetEnabled(true);
			this.MakeSolListAndSort();
			this.bSetFirstItem = false;
			this.SetSelectFirstItem(this.mBattleSolList);
			this.nInjurySoldierCount = 1;
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
			{
				if (this.mSelectedSolinfo != null && this.mSelectedSolinfo.GetCharKindInfo().IsATB(1L))
				{
					this.m_DrawTexture_rank.Visible = false;
				}
				else
				{
					this.m_DrawTexture_rank.Visible = true;
				}
			}
			break;
		case 1:
			this.nCurrentSolPosType = 0;
			this.nInjurySoldierCount = 0;
			base.SetShowLayer(1, false);
			base.SetShowLayer(3, false);
			base.SetShowLayer(2, true);
			base.SetShowLayer(6, false);
			base.SetShowLayer(7, false);
			base.SetShowLayer(8, false);
			this.m_dtEffect.Visible = false;
			this.ChangeFaceChar.SetEnabled(false);
			this.MakeSolListAndSort();
			this.bSetFirstItem = false;
			this.SetSelectFirstItem(this.mReadySolList);
			this.nInjurySoldierCount = 1;
			this.mReadySolList.clipWhenMoving = true;
			this.m_dlReadySolSort.SetVisible(true);
			this.m_dlReadySolSort.controlIsEnabled = true;
			break;
		case 2:
			this.nCurrentSolPosType = 5;
			base.SetShowLayer(1, false);
			base.SetShowLayer(3, false);
			base.SetShowLayer(2, false);
			base.SetShowLayer(6, false);
			base.SetShowLayer(7, true);
			base.SetShowLayer(8, false);
			this.m_dtEffect.Visible = false;
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.CheckSolWarehouseLoadServerData(0, 0L))
			{
			}
			this.m_dlWarehouseSolSort.SetVisible(true);
			this.m_dlWarehouseSolSort.controlIsEnabled = true;
			this.bSetFirstItem = false;
			this.SetSelectFirstItem(this.m_nlbWarehouseList);
			break;
		}
		this.SetSolCount((SolMilitaryGroupDlg.eSolMilitaryGroupTab)tabindex);
	}

	private void InitSelectedSolinfo()
	{
		this.mSelectedSolinfo = null;
		this.SetEquipItemInfo();
		this.SetTrainingSkillInfo(this.mSelectedSolinfo, true);
		this.SetSoldierStatInfo();
		this.SetEventHeroTexture();
	}

	public NkSoldierInfo GetSelectedSolinfo()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return null;
		}
		return this.mSelectedSolinfo;
	}

	public void SetSelectedSolinfo(NkSoldierInfo selectSolinfo)
	{
		if (selectSolinfo != null && selectSolinfo.IsValid())
		{
			this.mSelectedSolinfo = selectSolinfo;
		}
		for (int i = 0; i < this.SORT_LIST.Count; i++)
		{
			if (this.SORT_LIST[i].GetSolID() == selectSolinfo.GetSolID())
			{
				this.SORT_LIST[i].Set(selectSolinfo);
				break;
			}
		}
		if (this.mSelectedSolinfo.IsAtbCommonFlag(2L))
		{
			this.m_dtRingslotlock.Visible = false;
		}
		else
		{
			this.m_dtRingslotlock.Visible = true;
		}
	}

	public void RefreshInit()
	{
		this.InitData();
		this.SetData();
	}

	public void RefreshBattleSolList()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		this.m_kBattleSolSortList.Clear();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
			this.AddSolList(soldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE, this.m_kBattleSolSortList);
		}
		this.m_kBattleSolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareSolPosIndex));
		this.SetBattleSoldierList();
	}

	public void FirstAction()
	{
		this.MakeSolListAndSort(true);
		this.Hide();
	}

	public void RefreshSolList()
	{
		this.m_bRefreshList = true;
	}

	public void RefreshEquipItem(NkSoldierInfo selectSolinfo)
	{
		this.SetSelectedSolinfo(selectSolinfo);
		this.SetDataForSoldier();
		if (selectSolinfo.IsLeader())
		{
			this.CheckSkillUpSolNum();
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.SOLINFO);
			}
		}
	}

	public void RefreshSkillInfo(NkSoldierInfo selectSolinfo)
	{
		this.SetSelectedSolinfo(selectSolinfo);
		this.SetDataForSoldier();
		if (selectSolinfo.IsLeader())
		{
			this.CheckSkillUpSolNum();
		}
	}

	public void SetSoldierSelectByBattle(int solindex)
	{
	}

	public void SetData()
	{
		this.MakeSolListAndSort();
	}

	private void SetDataForSoldier()
	{
		if (this.bShowSoldierEquip)
		{
			this.SetEquipItemInfo();
		}
		this.SetTrainingSkillInfo(this.mSelectedSolinfo, true);
		this.SetSoldierStatInfo();
		this.SetEventHeroTexture();
		this.CheckSolMineMilitaryStatus();
	}

	private void AddSolList(NkSoldierInfo pkSolinfo, eSOL_POSTYPE eAddPosType, List<NkSoldierInfo> list)
	{
		if (pkSolinfo == null || !pkSolinfo.IsValid())
		{
			return;
		}
		if (eAddPosType == eSOL_POSTYPE.SOLPOS_BATTLE)
		{
			if (pkSolinfo.GetSolPosType() != (byte)eAddPosType)
			{
				return;
			}
		}
		else if (pkSolinfo.GetSolPosType() != 0 && pkSolinfo.GetSolPosType() != 2 && pkSolinfo.GetSolPosType() != 6 && pkSolinfo.GetSolPosType() != 7)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.Set(pkSolinfo);
		list.Add(nkSoldierInfo);
	}

	public bool MakeSolListAndSort()
	{
		this.MakeSolListAndSort(this.m_bRefreshList);
		this.m_bRefreshList = false;
		return true;
	}

	private bool MakeSolListAndSort(bool bDirectUpdate)
	{
		bool flag = bDirectUpdate;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return flag;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (!flag)
		{
			if (this.nCurrentSolPosType == 1)
			{
				flag = (this.SORT_LIST.Count != (int)soldierList.GetBattleSoldierCount());
			}
			else
			{
				flag = (this.SORT_LIST.Count != readySolList.GetCount());
			}
		}
		if (flag)
		{
			this.m_kBattleSolSortList.Clear();
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
				this.AddSolList(soldierInfo, eSOL_POSTYPE.SOLPOS_BATTLE, this.m_kBattleSolSortList);
			}
			this.m_kBattleSolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareSolPosIndex));
			this.SetSoldierList(eSOL_POSTYPE.SOLPOS_BATTLE);
			this.m_kReadySolSortList.Clear();
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY, this.m_kReadySolSortList);
			}
			this.SetSolSort(this.m_iReadySortOrder, this.m_kReadySolSortList);
			this.SetSoldierList(eSOL_POSTYPE.SOLPOS_READY);
		}
		return flag;
	}

	private int CompareSolPosIndex(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetSolPosIndex().CompareTo(b.GetSolPosIndex());
	}

	private int CompareExp(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetExp().CompareTo(a.GetExp());
	}

	private NkSoldierInfo FindSimpleSolInfo(long SolID)
	{
		for (int i = 0; i < this.SORT_LIST.Count; i++)
		{
			if (this.SORT_LIST[i].GetSolID() == SolID)
			{
				return this.SORT_LIST[i];
			}
		}
		return null;
	}

	private NkSoldierInfo FindSimpleSolInfoIndex(int SolPosIndex)
	{
		for (int i = 0; i < this.SORT_LIST.Count; i++)
		{
			if ((int)this.SORT_LIST[i].GetSolPosIndex() == SolPosIndex)
			{
				return this.SORT_LIST[i];
			}
		}
		return null;
	}

	private void SetBattleSoldierList()
	{
		this.mBattleSolList.Clear();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo pkSolinfo = null;
			for (int j = 0; j < this.m_kBattleSolSortList.Count; j++)
			{
				if (i == (int)this.m_kBattleSolSortList[j].GetSolPosIndex())
				{
					pkSolinfo = this.m_kBattleSolSortList[j];
				}
			}
			this.AddBattleSolList(i, pkSolinfo);
		}
		this.mBattleSolList.RepositionItems();
	}

	private void SetSoldierList(eSOL_POSTYPE ePostType)
	{
		if (ePostType == eSOL_POSTYPE.SOLPOS_BATTLE)
		{
			this.mBattleSolList.Clear();
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo pkSolinfo = null;
				for (int j = 0; j < this.m_kBattleSolSortList.Count; j++)
				{
					if (i == (int)this.m_kBattleSolSortList[j].GetSolPosIndex())
					{
						pkSolinfo = this.m_kBattleSolSortList[j];
					}
				}
				this.AddBattleSolList(i, pkSolinfo);
			}
			this.mBattleSolList.RepositionItems();
		}
		else if (ePostType == eSOL_POSTYPE.SOLPOS_READY)
		{
			this.mReadySolList.Clear();
			using (new ScopeProfile("AddReadySolList"))
			{
				for (int k = 0; k < this.m_kReadySolSortList.Count; k++)
				{
					this.AddReadySolList(this.m_kReadySolSortList[k]);
				}
			}
			using (new ScopeProfile("mReadySolList.RepositionItems"))
			{
				this.mReadySolList.RepositionItems();
			}
		}
		else if (ePostType == eSOL_POSTYPE.SOLPOS_WAREHOUSE)
		{
			this.MakeSolWarehouseList(true);
		}
		this.SetSolCount((SolMilitaryGroupDlg.eSolMilitaryGroupTab)this.m_iTabIndex);
	}

	private void SetBattleSolListItem(int solindex, ref NewListItem item, ref NkSoldierInfo pkSolinfo)
	{
		string text = string.Empty;
		if (pkSolinfo != null)
		{
			EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(pkSolinfo.GetCharKind(), pkSolinfo.GetGrade());
			if (eventHeroCharCode != null)
			{
				item.SetListItemData(0, "Win_I_EventSol", null, null, null);
			}
			else
			{
				UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade());
				if (legendFrame != null)
				{
					item.SetListItemData(0, legendFrame, null, null, null);
				}
			}
			Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(pkSolinfo.GetCharKind());
			if (portraitLeaderSol == null)
			{
				item.SetListItemData(1, pkSolinfo.GetListSolInfo(false), null, null, null);
			}
			else
			{
				item.SetListItemData(1, portraitLeaderSol, null, null, null, null);
			}
			string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade(), pkSolinfo.GetName());
			item.SetListItemData(2, legendName, null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
				"count1",
				pkSolinfo.GetLevel().ToString(),
				"count2",
				pkSolinfo.GetSolMaxLevel().ToString()
			});
			item.SetListItemData(3, text, null, null, null);
			int num = pkSolinfo.GetEquipWeaponOrigin();
			if (num > 0)
			{
				item.SetListItemData(4, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
			}
			num = pkSolinfo.GetEquipWeaponExtention();
			if (num > 0)
			{
				item.SetListItemData(5, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
			}
			else
			{
				item.SetListItemData(5, false);
			}
			long exp = pkSolinfo.GetExp();
			long curBaseExp = pkSolinfo.GetCurBaseExp();
			long nextExp = pkSolinfo.GetNextExp();
			long num2 = exp - curBaseExp;
			long num3 = nextExp - curBaseExp;
			long remainExp = pkSolinfo.GetRemainExp();
			float num4 = 0f;
			if (pkSolinfo.IsMaxLevel())
			{
				num4 = 1f;
			}
			if (!pkSolinfo.IsMaxLevel() && 0L < num3 && remainExp < num3)
			{
				num4 = ((float)num3 - (float)remainExp) / (float)num3;
			}
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			if (0f > num4)
			{
				num4 = 0f;
			}
			item.SetListItemData(6, "Win_T_ReputelPrgBG", null, null, null);
			item.SetListItemData(7, "Com_T_GauWaPr4", this.BATTLE_LIST_MAX_EXP_GAGUE * num4, null, null);
			item.SetListItemData(8, string.Empty, pkSolinfo.GetSolID(), new EZValueChangedDelegate(this.OnClickSoldierDelete), null);
			if (pkSolinfo.IsMaxLevel())
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
					"exp",
					num2.ToString(),
					"maxexp",
					num3.ToString()
				});
			}
			item.SetListItemData(9, text, null, null, null);
			item.SetListItemData(10, false);
			item.Data = pkSolinfo.GetSolID();
			bool visibe = false;
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				int num5 = (int)myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FACE_SOLINDEX);
				if ((int)pkSolinfo.GetSolPosIndex() == num5)
				{
					this.m_nChangeFaceCharKind = pkSolinfo.GetCharKind();
					this.m_nFaceSolPosIndex = (int)pkSolinfo.GetSolPosIndex();
					visibe = true;
				}
			}
			item.SetListItemData(11, visibe);
			item.SetListItemData(12, false);
			item.SetListItemData(13, false);
			if (this.SetTrainingSkillInfo(pkSolinfo, false))
			{
				item.SetListItemData(14, true);
			}
			else
			{
				item.SetListItemData(14, false);
			}
			if (pkSolinfo.IsAtbCommonFlag(1L))
			{
				item.SetListItemData(15, true);
			}
			else
			{
				item.SetListItemData(15, false);
			}
		}
		else
		{
			for (int i = 0; i < this.mBattleSolList.ColumnNum; i++)
			{
				item.SetListItemData(i, false);
			}
			int num6 = 1;
			NrMyCharInfo myCharInfo2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo2 != null)
			{
				num6 = myCharInfo2.GetLevelForSolPosIndex(solindex);
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1872"),
				"level",
				num6.ToString()
			});
			item.SetListItemData(10, true);
			item.SetListItemData(10, text, null, null, null);
			item.Data = 0L;
		}
	}

	private void SetReadySolListItem(ref NewListItem item, ref NkSoldierInfo pkSolinfo)
	{
		string text = string.Empty;
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(pkSolinfo.GetCharKind(), pkSolinfo.GetGrade());
		if (eventHeroCharCode != null)
		{
			item.SetListItemData(0, "Win_I_EventSol", null, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(0, legendFrame, null, null, null);
			}
		}
		item.SetListItemData(1, pkSolinfo.GetListSolInfo(false), null, null, null);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade(), pkSolinfo.GetName());
		item.SetListItemData(2, legendName, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
			"count1",
			pkSolinfo.GetLevel().ToString(),
			"count2",
			pkSolinfo.GetSolMaxLevel().ToString()
		});
		item.SetListItemData(3, text, null, null, null);
		int num = pkSolinfo.GetEquipWeaponOrigin();
		if (num > 0)
		{
			item.SetListItemData(4, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
		}
		num = pkSolinfo.GetEquipWeaponExtention();
		if (num > 0)
		{
			item.SetListItemData(5, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
		}
		else
		{
			item.SetListItemData(5, false);
		}
		long num2 = pkSolinfo.GetExp() - pkSolinfo.GetCurBaseExp();
		long num3 = pkSolinfo.GetNextExp() - pkSolinfo.GetCurBaseExp();
		float num4 = 1f;
		if (!pkSolinfo.IsMaxLevel())
		{
			num4 = ((float)num3 - (float)pkSolinfo.GetRemainExp()) / (float)num3;
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			if (0f > num4)
			{
				num4 = 0f;
			}
		}
		item.SetListItemData(6, "Win_T_ReputelPrgBG", null, null, null);
		item.SetListItemData(7, "Com_T_GauWaPr4", this.READY_LIST_MAX_EXP_GAGUE * num4, null, null);
		if (pkSolinfo.IsMaxLevel())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
				"exp",
				num2.ToString(),
				"maxexp",
				num3.ToString()
			});
		}
		item.SetListItemData(8, text, null, null, null);
		item.SetListItemData(9, string.Empty, null, null, null);
		item.Data = pkSolinfo.GetSolID();
		item.SetListItemData(10, false);
		if (this.SetTrainingSkillInfo(pkSolinfo, false))
		{
			item.SetListItemData(11, true);
		}
		else
		{
			item.SetListItemData(11, false);
		}
		if (pkSolinfo.GetSolPosType() == 2 || pkSolinfo.GetSolPosType() == 6)
		{
			item.SetListItemData(12, true);
			item.SetListItemData(13, false);
			item.SetListItemData(14, true);
			if (pkSolinfo.GetSolPosType() == 6)
			{
				item.SetListItemData(12, "Win_I_PMine2", null, null, null);
				UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_PMineInfo");
				if (uIBaseInfoLoader != null)
				{
					item.SetListItemData(14, uIBaseInfoLoader, pkSolinfo, new EZValueChangedDelegate(this.ClickMilitaryInfo), null);
				}
			}
			else
			{
				item.SetListItemData(14, string.Empty, pkSolinfo, new EZValueChangedDelegate(this.ClickMilitaryInfo), null);
			}
		}
		else if (pkSolinfo.GetSolPosType() == 7)
		{
			item.SetListItemData(12, "Win_I_PMine", null, null, null);
			item.SetListItemData(13, false);
			item.SetListItemData(14, string.Empty, pkSolinfo, new EZValueChangedDelegate(this.ClickGuildWarApplyInfo), null);
		}
		else
		{
			item.SetListItemData(12, false);
			item.SetListItemData(13, true);
			item.SetListItemData(13, string.Empty, null, new EZValueChangedDelegate(this.ClickSolWarehouseMove), null);
			item.SetListItemData(14, false);
		}
		if (pkSolinfo.IsAtbCommonFlag(1L))
		{
			item.SetListItemData(15, true);
		}
		else
		{
			item.SetListItemData(15, false);
		}
	}

	private void AddBattleSolList(int solindex, NkSoldierInfo pkSolinfo)
	{
		NewListItem item = new NewListItem(this.mBattleSolList.ColumnNum, true);
		this.SetBattleSolListItem(solindex, ref item, ref pkSolinfo);
		this.mBattleSolList.Add(item);
		if (pkSolinfo != null && pkSolinfo.IsInjuryStatus())
		{
			this.nInjurySoldierCount++;
			this.nInjuryBattleSoldierCount++;
		}
	}

	private void RemoveAddBattleSolList(int solindex, NkSoldierInfo pkSolinfo)
	{
		NewListItem item = new NewListItem(this.mBattleSolList.ColumnNum, true);
		this.SetBattleSolListItem(solindex, ref item, ref pkSolinfo);
		this.mBattleSolList.RemoveAdd(solindex, item);
		if (pkSolinfo != null && pkSolinfo.IsInjuryStatus())
		{
			this.nInjurySoldierCount++;
			this.nInjuryBattleSoldierCount++;
		}
	}

	private void RemoveAddReadySolList(int index, NkSoldierInfo pkSolinfo)
	{
		NewListItem item = new NewListItem(this.mBattleSolList.ColumnNum, true);
		this.SetReadySolListItem(ref item, ref pkSolinfo);
		this.mReadySolList.RemoveAdd(index, item);
	}

	private void RemoveAddWarehouseSolList(int index, NkSoldierInfo pkSolinfo)
	{
		NewListItem item = new NewListItem(this.mBattleSolList.ColumnNum, true);
		this.SetSolWarehouseListItemSol(ref item, ref pkSolinfo);
		this.mReadySolList.RemoveAdd(index, item);
	}

	private void AddReadySolList(NkSoldierInfo pkSolinfo)
	{
		NewListItem item = new NewListItem(this.mReadySolList.ColumnNum, true);
		this.SetReadySolListItem(ref item, ref pkSolinfo);
		this.mReadySolList.Add(item);
		if (pkSolinfo != null && pkSolinfo.IsInjuryStatus())
		{
			this.nInjurySoldierCount++;
			this.nInjuryReadySoldierCount++;
		}
	}

	private void SetSolCount(SolMilitaryGroupDlg.eSolMilitaryGroupTab eTab)
	{
		int num = this.m_kReadySolSortList.Count;
		for (int i = 0; i < 3; i++)
		{
			if (i != (int)eTab)
			{
				this.m_lbTab[i].SetText(this.m_strTab[i]);
			}
		}
		int num2;
		switch (eTab)
		{
		case SolMilitaryGroupDlg.eSolMilitaryGroupTab.SOLTAB_SOLLIST_BATTLE:
			num = this.m_kBattleSolSortList.Count;
			num2 = 6;
			break;
		case SolMilitaryGroupDlg.eSolMilitaryGroupTab.SOLTAB_SOLLIST_READY:
			num = this.m_kReadySolSortList.Count;
			num2 = 50;
			break;
		case SolMilitaryGroupDlg.eSolMilitaryGroupTab.SOLTAB_SOLLIST_WAREHOUSE:
			num = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseCount();
			num2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWarehouseCount();
			break;
		default:
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1743"),
			"text",
			this.m_strTab[(int)eTab],
			"count1",
			num,
			"count2",
			num2
		});
		this.m_lbTab[(int)eTab].SetText(empty);
	}

	public void SetSoldierUpdate(NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo == null)
		{
			return;
		}
		for (int i = 0; i < this.mBattleSolList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.mBattleSolList.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null) && (long)uIListItemContainer.Data == pkSolinfo.GetSolID())
			{
				bool flag = false;
				for (int j = 0; j < this.m_kBattleSolSortList.Count; j++)
				{
					if (pkSolinfo.GetSolID() == this.m_kBattleSolSortList[j].GetSolID())
					{
						flag = true;
						this.m_kBattleSolSortList[j].Set(pkSolinfo);
						break;
					}
				}
				if (!flag)
				{
					if (i >= this.m_kBattleSolSortList.Count)
					{
						goto IL_13B;
					}
					if (this.m_kBattleSolSortList[i] == null)
					{
						this.m_kBattleSolSortList[i] = new NkSoldierInfo();
					}
					this.m_kBattleSolSortList[i].Set(pkSolinfo);
				}
				NewListItem item = new NewListItem(this.mBattleSolList.ColumnNum, true);
				this.SetBattleSolListItem((int)pkSolinfo.GetSolPosIndex(), ref item, ref pkSolinfo);
				this.mBattleSolList.RemoveAdd(i, item);
				this.mBattleSolList.RepositionItems();
				this.nInjuryBattleSoldierCount++;
				break;
			}
			IL_13B:;
		}
		for (int k = 0; k < this.m_kReadySolSortList.Count; k++)
		{
			if (this.m_kReadySolSortList[k].GetSolID() == pkSolinfo.GetSolID())
			{
				this.m_kReadySolSortList[k].Set(pkSolinfo);
				break;
			}
		}
		for (int l = 0; l < this.mReadySolList.Count; l++)
		{
			UIListItemContainer uIListItemContainer = this.mReadySolList.GetItem(l) as UIListItemContainer;
			if (!(uIListItemContainer == null) && (long)uIListItemContainer.Data == pkSolinfo.GetSolID())
			{
				NewListItem item2 = new NewListItem(this.mReadySolList.ColumnNum, true);
				this.SetReadySolListItem(ref item2, ref pkSolinfo);
				this.mReadySolList.UpdateAdd(l, item2);
				this.mReadySolList.RepositionItems();
				this.nInjuryReadySoldierCount++;
				break;
			}
		}
		this.nInjurySoldierCount = this.nInjuryBattleSoldierCount + this.nInjuryReadySoldierCount;
		this.SetSolCount((SolMilitaryGroupDlg.eSolMilitaryGroupTab)this.m_iTabIndex);
	}

	public void SetEquipItemInfo()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			for (int i = 0; i < 6; i++)
			{
				if (!(this.SolEquipItem[i] == null))
				{
					this.SolEquipItem[i].Clear();
				}
			}
			this.SelectSolName.Text = string.Empty;
			this.SelectSolImage.SetTexture(string.Empty);
			this.m_DrawTexture_rank.Visible = false;
			this.GradeExpBG.Visible = false;
			this.GradeExpGage.Visible = false;
			this.GradeExpText.Visible = false;
			this.m_dtReincarnationEffect.Visible = true;
			this.m_dtRingslotlock.Visible = false;
			return;
		}
		for (int j = 0; j < 6; j++)
		{
			if (j == 5)
			{
				if (this.mSelectedSolinfo.IsAtbCommonFlag(2L))
				{
					this.m_dtRingslotlock.Visible = false;
				}
				else
				{
					this.m_dtRingslotlock.Visible = true;
				}
			}
			if (!(this.SolEquipItem[j] == null))
			{
				this.SolEquipItem[j].Clear();
				ImageSlot imageSlot = new ImageSlot();
				ITEM equipItem = this.mSelectedSolinfo.GetEquipItem(j);
				if (equipItem != null && equipItem.m_nItemID != 0L)
				{
					imageSlot.c_oItem = equipItem;
					imageSlot.c_bEnable = true;
					imageSlot.Index = j;
					imageSlot.itemunique = equipItem.m_nItemUnique;
					imageSlot._solID = this.mSelectedSolinfo.GetSolID();
					imageSlot.WindowID = base.WindowID;
					if (equipItem.m_nItemNum > 1)
					{
						imageSlot.SlotInfo._visibleNum = true;
					}
					imageSlot.SlotInfo._visibleRank = true;
					imageSlot.SlotInfo.Set(string.Empty, "+ " + equipItem.m_nRank.ToString());
				}
				else
				{
					imageSlot.c_oItem = null;
					imageSlot.Index = j;
					imageSlot._solID = this.mSelectedSolinfo.GetSolID();
					imageSlot.WindowID = base.WindowID;
					imageSlot.SlotInfo.Set(string.Empty, string.Empty);
				}
				this.SolEquipItem[j].SetImageSlot(0, imageSlot, null, new EZValueChangedDelegate(this.OnClickEquipItem), null, null);
				this.SolEquipItem[j].RepositionItems();
			}
		}
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade(), this.mSelectedSolinfo.GetName());
		this.SelectSolName.Text = legendName;
		this.SelectSolImage.SetUVMask(new Rect(4f / this.TEX_SIZE, 0f, 504f / this.TEX_SIZE, 448f / this.TEX_SIZE));
		this.SelectSolImage.SetTextureEffect(eCharImageType.LARGE, this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade());
		this.GradeExpBG.Visible = true;
		this.GradeExpGage.Visible = true;
		this.GradeExpText.Visible = true;
		bool flag = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation() && this.mSelectedSolinfo.GetCharKindInfo().IsATB(1L))
		{
			this.m_DrawTexture_rank.Visible = false;
			float evolutionExpPercent = this.mSelectedSolinfo.GetEvolutionExpPercent();
			string text = string.Empty;
			this.GradeExpGage.SetSize(this.MAX_EXP_GAGUE * evolutionExpPercent, this.GradeExpGage.height);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("484");
			this.GradeExpText.SetText(text);
			flag = true;
		}
		if (!flag)
		{
			UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade());
			if (solLargeGradeImg != null)
			{
				this.m_DrawTexture_rank.Visible = true;
				if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade()))
				{
					this.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
				}
				this.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
			}
			else
			{
				this.m_DrawTexture_rank.Visible = false;
			}
			long num = this.mSelectedSolinfo.GetEvolutionExp() - this.mSelectedSolinfo.GetCurBaseEvolutionExp();
			long num2 = this.mSelectedSolinfo.GetNextEvolutionExp() - this.mSelectedSolinfo.GetCurBaseEvolutionExp();
			float evolutionExpPercent2 = this.mSelectedSolinfo.GetEvolutionExpPercent();
			string text2 = string.Empty;
			this.GradeExpGage.SetSize(this.MAX_EXP_GAGUE * evolutionExpPercent2, this.GradeExpGage.height);
			if (!this.mSelectedSolinfo.IsMaxGrade())
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
					"exp",
					num.ToString(),
					"maxexp",
					num2.ToString()
				});
			}
			else
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("484");
			}
			this.GradeExpText.SetText(text2);
		}
		this.CheckSolInjuryStatus();
		if (!this.SolInjuryImage.Visible)
		{
			this.fSolCureTime = 0f;
		}
		else
		{
			this.fSolCureTime = Time.time;
			this.nInjurySoldierCount++;
		}
		if (this.IsReincarnation())
		{
			UIBaseInfoLoader solGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolGradeImg(this.mSelectedSolinfo.GetCharKind(), (int)this.mSelectedSolinfo.GetGrade());
			if (solGradeImg != null)
			{
				this.m_dtRank2.SetTexture(solGradeImg);
			}
			Transform child = NkUtil.GetChild(this.m_dtReincarnationEffect.gameObject.transform, "child_effect");
			if (child == null)
			{
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BUTTON_REBIRTH", this.m_dtReincarnationEffect, this.m_dtReincarnationEffect.GetSize());
			}
			this.SetShowReincarnation(true);
		}
		else
		{
			this.SetShowReincarnation(false);
		}
	}

	private bool SetTrainingSkillInfo(NkSoldierInfo SkillSelectedSolinfo, bool bSetSkillSlotControl)
	{
		if (SkillSelectedSolinfo == null || !SkillSelectedSolinfo.IsValid())
		{
			this.kSolSkillSlotControl.SetEmpty(true, true);
			this.kSolSkillSlotControl.SetTrainingSkillInfo(null);
			return false;
		}
		if (bSetSkillSlotControl && !SkillSelectedSolinfo.IsValid())
		{
			this.kSolSkillSlotControl.SetEmpty(true, true);
			this.kSolSkillSlotControl.SetTrainingSkillInfo(null);
			return false;
		}
		List<BATTLESKILL_TRAINING> battleSkillTrainingGroup = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTrainingGroup(SkillSelectedSolinfo);
		if (battleSkillTrainingGroup == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		foreach (BATTLESKILL_TRAINING current in battleSkillTrainingGroup)
		{
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(current.m_nSkillUnique);
			if (battleSkillBase != null)
			{
				if (!SkillSelectedSolinfo.IsLeader() || SkillSelectedSolinfo.CheckNeedWeaponType(battleSkillBase.m_nSkilNeedWeapon))
				{
					int battleSkillLevel = SkillSelectedSolinfo.GetBattleSkillLevel(current.m_nSkillUnique);
					flag = (battleSkillLevel < (int)SkillSelectedSolinfo.GetLevel() && battleSkillLevel < battleSkillBase.m_nSkillMaxLevel);
					if (bSetSkillSlotControl)
					{
						this.kSolSkillSlotControl.SetSkillInfo(battleSkillBase, battleSkillLevel, (int)SkillSelectedSolinfo.GetLevel(), 0, flag);
						this.kSolSkillSlotControl.SetTrainingSkillInfo(current);
						if (flag)
						{
							num++;
						}
						if (battleSkillLevel == (int)SkillSelectedSolinfo.GetLevel() || battleSkillLevel == battleSkillBase.m_nSkillMaxLevel)
						{
							bSetSkillSlotControl = false;
						}
					}
					else if (flag)
					{
						num++;
					}
					break;
				}
			}
		}
		bool result;
		if (num == 0)
		{
			if (bSetSkillSlotControl)
			{
				this.kSolSkillSlotControl.SetEmpty(true, true);
			}
			result = false;
		}
		else
		{
			result = true;
		}
		if (this.mBattleSolList.Count > 0)
		{
			for (int i = 0; i < this.mBattleSolList.Count; i++)
			{
				UIListItemContainer uIListItemContainer = this.mBattleSolList.GetItem(i) as UIListItemContainer;
				if (!(uIListItemContainer == null) && (long)uIListItemContainer.Data == SkillSelectedSolinfo.GetSolID())
				{
					DrawTexture drawTexture = uIListItemContainer.GetElement(14) as DrawTexture;
					drawTexture.Visible = flag;
					break;
				}
			}
		}
		if (this.mReadySolList.Count > 0)
		{
			for (int j = 0; j < this.mReadySolList.Count; j++)
			{
				UIListItemContainer uIListItemContainer = this.mReadySolList.GetItem(j) as UIListItemContainer;
				if (!(uIListItemContainer == null) && (long)uIListItemContainer.Data == SkillSelectedSolinfo.GetSolID())
				{
					DrawTexture drawTexture2 = uIListItemContainer.GetElement(11) as DrawTexture;
					drawTexture2.Visible = flag;
					break;
				}
			}
		}
		return result;
	}

	public void SetSoldierStatInfo()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			this.SoldierSpec.Text = string.Empty;
			this.m_lbInitiativeValue.Text = string.Empty;
			this.ItemSolStatHP.Text = string.Empty;
			this.ItemSolStatDamage.Text = string.Empty;
			this.ItemSolStatDefence.Text = string.Empty;
			this.ItemSolStatMagicDefence.Text = string.Empty;
			this.ItemSolStatSeason.Text = string.Empty;
			return;
		}
		string text = string.Empty;
		NrCharKindInfo charKindInfo = this.mSelectedSolinfo.GetCharKindInfo();
		if (charKindInfo != null)
		{
			if (charKindInfo.GetCHARKIND_ATTACKINFO().ATTACKTYPE == this.mSelectedSolinfo.GetAttackInfo().ATTACKTYPE)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec1);
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec2);
			}
		}
		this.SoldierSpec.Text = text;
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.mSelectedSolinfo.GetCharKind(), this.mSelectedSolinfo.GetGrade());
		int num = this.mSelectedSolinfo.GetMaxHP();
		string text2 = this.mSelectedSolinfo.GetMaxHP().ToString();
		string text3 = this.mSelectedSolinfo.GetMinDamage().ToString();
		string text4 = this.mSelectedSolinfo.GetMaxDamage().ToString();
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
		string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
		string second = string.Empty;
		if (eventHeroCharCode != null)
		{
			if (eventHeroCharCode.i32Hp != 100)
			{
				num = (int)((float)this.mSelectedSolinfo.GetMaxHP() * ((float)eventHeroCharCode.i32Hp * 0.01f));
				second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", (num - this.mSelectedSolinfo.GetMaxHP()).ToString(), ")", textColor2);
				text2 = NrTSingleton<UIDataManager>.Instance.GetString(this.mSelectedSolinfo.GetMaxHP().ToString(), second);
			}
			if (eventHeroCharCode.i32Attack != 100)
			{
				float num2 = (float)(this.mSelectedSolinfo.GetMinDamage() * eventHeroCharCode.i32Attack) * 0.01f;
				float num3 = (float)(this.mSelectedSolinfo.GetMaxDamage() * eventHeroCharCode.i32Attack) * 0.01f;
				second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", ((int)(num2 - (float)this.mSelectedSolinfo.GetMinDamage())).ToString(), ")", textColor2);
				text3 = NrTSingleton<UIDataManager>.Instance.GetString(this.mSelectedSolinfo.GetMinDamage().ToString(), second);
				second = NrTSingleton<UIDataManager>.Instance.GetString(textColor, "(+", ((int)(num3 - (float)this.mSelectedSolinfo.GetMaxDamage())).ToString(), ")", textColor2);
				text4 = NrTSingleton<UIDataManager>.Instance.GetString(this.mSelectedSolinfo.GetMaxDamage().ToString(), second);
			}
		}
		this.m_lbInitiativeValue.Text = this.mSelectedSolinfo.GetInitiativeValue().ToString();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1879"),
			"maxhp",
			text2
		});
		this.ItemSolStatHP.Text = text;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1880"),
			"mindmg",
			text3,
			"maxdmg",
			text4
		});
		this.ItemSolStatDamage.Text = text;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1881"),
			"defance",
			this.mSelectedSolinfo.GetPhysicalDefense().ToString()
		});
		this.ItemSolStatDefence.Text = text;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1330"),
			"magicdefence",
			this.mSelectedSolinfo.GetMagicDefense().ToString()
		});
		this.ItemSolStatMagicDefence.Text = text;
		int num4 = this.mSelectedSolinfo.GetSeason() + 1;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			num4.ToString()
		});
		this.ItemSolStatSeason.Text = text;
		if (this.IsReincarnation())
		{
			this.SetShowReincarnation(true);
		}
		else
		{
			this.SetShowReincarnation(false);
		}
		if (this.mSelectedSolinfo.GetSolPosType() == 7)
		{
			this.m_lbMilitary.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2666"));
			this.m_lbMilitary.Visible = true;
		}
		else if (this.mSelectedSolinfo.GetSolPosType() == 6)
		{
			this.m_lbMilitary.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2478"));
			this.m_lbMilitary.Visible = true;
		}
		else if (this.mSelectedSolinfo.GetSolPosType() == 2)
		{
			this.m_lbMilitary.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1473"));
			this.m_lbMilitary.Visible = true;
		}
		else
		{
			this.m_lbMilitary.Visible = false;
		}
	}

	private void CheckSolInjuryStatus()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		if (this.mSelectedSolinfo.IsInjuryStatus())
		{
			if (!this.SolInjuryImage.Visible)
			{
				base.SetShowLayer(6, true);
				this.SetSoldierUpdate(this.mSelectedSolinfo);
				this.fSolCureTime = Time.time;
				this.nInjurySoldierCount++;
			}
			this.SetSolCureTimeInfo();
		}
		else if (this.SolInjuryImage.Visible)
		{
			base.SetShowLayer(6, false);
			this.SetSoldierUpdate(this.mSelectedSolinfo);
			this.fSolCureTime = 0f;
		}
	}

	private void CheckSolMineMilitaryStatus()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		bool bShow = false;
		if (this.mSelectedSolinfo.GetSolPosType() == 2 || this.mSelectedSolinfo.GetSolPosType() == 6)
		{
			bShow = true;
		}
		base.SetShowLayer(8, bShow);
		this.SetSoldierUpdate(this.mSelectedSolinfo);
	}

	private void SetSolCureTimeInfo()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		long remainInjuryTime = this.mSelectedSolinfo.GetRemainInjuryTime();
		string text = PublicMethod.ConvertTime(remainInjuryTime);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("65"),
			"timestring",
			text
		});
		this.SolCureLabel.SetText(empty);
	}

	private void CheckInjurySoldierList()
	{
		int num = 0;
		if (this.nInjuryBattleSoldierCount > 0)
		{
			for (int i = 0; i < this.m_kBattleSolSortList.Count; i++)
			{
				if (this.m_kBattleSolSortList[i] == null)
				{
					this.bRefreshSolinfo = true;
				}
				else if (this.m_kBattleSolSortList[i].IsSolStatus(2))
				{
					if (!this.m_kBattleSolSortList[i].IsInjuryStatus())
					{
						this.m_kBattleSolSortList[i].SetInjuryStatus(false);
						this.SetSoldierUpdate(this.m_kBattleSolSortList[i]);
					}
					else
					{
						num++;
					}
				}
			}
			this.nInjuryBattleSoldierCount = num;
		}
		num = 0;
		if (this.nInjuryReadySoldierCount > 0)
		{
			for (int j = 0; j < this.m_kReadySolSortList.Count; j++)
			{
				if (this.m_kReadySolSortList[j] == null)
				{
					this.bRefreshSolinfo = true;
				}
				else if (this.m_kReadySolSortList[j].IsSolStatus(2))
				{
					if (!this.m_kReadySolSortList[j].IsInjuryStatus())
					{
						this.m_kReadySolSortList[j].SetInjuryStatus(false);
						this.SetSoldierUpdate(this.m_kReadySolSortList[j]);
					}
					else
					{
						num++;
					}
				}
			}
			this.nInjuryReadySoldierCount = num;
		}
		this.nInjurySoldierCount = this.nInjuryBattleSoldierCount + this.nInjuryReadySoldierCount;
		if (this.bRefreshSolinfo)
		{
			this.RefreshSolList();
		}
		if (this.nInjurySoldierCount > 0)
		{
			this.SolAllCureButton.SetEnabled(true);
		}
		else
		{
			this.SolAllCureButton.SetEnabled(false);
		}
	}

	private void OnClickMakeMilitary(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYPOSITION_DLG);
	}

	private void SetSelectFirstItem(NewListBox pList)
	{
		if (this.bSetFirstItem)
		{
			return;
		}
		if (null == pList)
		{
			return;
		}
		UIListItemContainer uIListItemContainer = pList.GetItem(0) as UIListItemContainer;
		if (null == uIListItemContainer)
		{
			if (!(this.mBattleSolList != null))
			{
				return;
			}
			pList = this.mBattleSolList;
			uIListItemContainer = (pList.GetItem(0) as UIListItemContainer);
			if (uIListItemContainer == null)
			{
				return;
			}
		}
		if (uIListItemContainer.data != null)
		{
			long num = (long)uIListItemContainer.data;
			if (0L < num)
			{
				this.OnSoldierView(pList.GetItem(0));
				pList.SetSelectedItem(0);
				this.bSetFirstItem = true;
			}
		}
		if (!this.bSetFirstItem && this.m_iTabIndex == 2)
		{
			this.OnSoldierView(this.mBattleSolList.GetItem(0));
			this.mBattleSolList.SetSelectedItem(0);
			this.bSetFirstItem = true;
		}
	}

	private void OnClickSoldierView(IUIObject obj)
	{
		NewListBox newListBox = obj as NewListBox;
		if (obj == null || null == newListBox)
		{
			return;
		}
		this.OnSoldierView(newListBox.SelectedItem);
	}

	private void OnSoldierView(IUIListObject obj)
	{
		UIListItemContainer uIListItemContainer = (UIListItemContainer)obj;
		if (null == uIListItemContainer)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = null;
		if (uIListItemContainer.data != null)
		{
			long num = (long)uIListItemContainer.data;
			nkSoldierInfo = this.FindSimpleSolInfo(num);
			if (nkSoldierInfo == null)
			{
				nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouse(num);
			}
			if (nkSoldierInfo == null && 0L < num)
			{
				for (int i = 0; i < this.m_kBattleSolSortList.Count; i++)
				{
					if (this.m_kBattleSolSortList[i].GetSolID() == num)
					{
						nkSoldierInfo = this.m_kBattleSolSortList[i];
						break;
					}
				}
			}
		}
		if (nkSoldierInfo == null)
		{
			if (this.nCurrentSolPosType == 1)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo == null)
				{
					return;
				}
				if (!myCharInfo.IsAddBattleSoldier(uIListItemContainer.GetIndex()))
				{
					string message = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1501"), NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("28"));
					Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					return;
				}
				SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
				if (solMilitarySelectDlg != null)
				{
					solMilitarySelectDlg.SetLocationByForm(this);
					solMilitarySelectDlg.SetFocus();
				}
			}
			return;
		}
		if (nkSoldierInfo != null && nkSoldierInfo != this.mSelectedSolinfo)
		{
			this.mSelectedSolinfo = nkSoldierInfo;
			this.SetDataForSoldier();
		}
		else if (nkSoldierInfo == this.mSelectedSolinfo)
		{
			if (this.mSelectedSolinfo.IsAtbCommonFlag(2L))
			{
				this.m_dtRingslotlock.Visible = false;
			}
			else
			{
				this.m_dtRingslotlock.Visible = true;
			}
		}
	}

	public void ChangeShowSoldier(NkSoldierInfo pkChangeSoInfo)
	{
		if (pkChangeSoInfo != null)
		{
			this.mSelectedSolinfo = pkChangeSoInfo;
			this.SetDataForSoldier();
		}
	}

	private void OnClickSoldierDelete(IUIObject obj)
	{
		if (this.nCurrentSolPosType == 0)
		{
			return;
		}
		UIButton uIButton = (UIButton)obj;
		if (null == uIButton || uIButton.Data == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (num == 0L)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = this.FindSimpleSolInfo(num);
		short num2 = 0;
		while ((int)num2 < this.mBattleSolList.Count)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.mBattleSolList.GetItem((int)num2);
			if (!(null == uIListItemContainer))
			{
				if (num == (long)uIListItemContainer.Data)
				{
					break;
				}
			}
			num2 += 1;
		}
		this.mSelectedSolinfo = nkSoldierInfo;
		if (this.mSelectedSolinfo == null)
		{
			return;
		}
		this.ToReady(ref this.mSelectedSolinfo);
	}

	private void OnClickPlunderAgree(IUIObject obj)
	{
	}

	public void ToBattleOrMilitary(ref NkSoldierInfo pkSolinfo)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		NrCharKindInfo charKindInfo = pkSolinfo.GetCharKindInfo();
		if (charKindInfo == null)
		{
			return;
		}
		int num = 0;
		byte b = 0;
		if (charKindInfo.GetCHARKIND_CLASSINFO() == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo nkSoldierInfo = this.FindSimpleSolInfoIndex(i);
			if (nkSoldierInfo == null)
			{
				if (num == 0)
				{
					num = i;
				}
			}
			else if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetCharKind() == pkSolinfo.GetCharKind())
				{
					b += 1;
				}
			}
		}
		if (b >= pkSolinfo.GetJoinCount())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (this.nCurrentSolPosType == 1)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			if (!myCharInfo.IsAddBattleSoldier(num))
			{
				string message = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1501"), NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("28"));
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
			this.SendSolChangeToServer(ref pkSolinfo, 1, 1);
		}
	}

	public void ToReady(ref NkSoldierInfo pkSolinfo)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (pkSolinfo.IsLeader())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("415"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FACE_SOLINDEX);
		if ((int)pkSolinfo.GetSolPosIndex() == num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("415"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null || readySolList.GetCount() >= 50)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("118"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (this.nCurrentSolPosType == 1)
		{
			this.SendSolChangeToServer(ref pkSolinfo, 0, 0);
			this.bSetFirstItem = false;
			this.SetSelectFirstItem(this.mBattleSolList);
		}
		else
		{
			this.bSetFirstItem = false;
			this.SetSelectFirstItem(this.mReadySolList);
		}
	}

	private void SendSolChangeToServer(ref NkSoldierInfo pkSolinfo, byte solpostype, byte militaryunique)
	{
		GS_SOLDIER_CHANGE_POSTYPE_REQ gS_SOLDIER_CHANGE_POSTYPE_REQ = new GS_SOLDIER_CHANGE_POSTYPE_REQ();
		gS_SOLDIER_CHANGE_POSTYPE_REQ.SolID = pkSolinfo.GetSolID();
		gS_SOLDIER_CHANGE_POSTYPE_REQ.SolPosType = solpostype;
		gS_SOLDIER_CHANGE_POSTYPE_REQ.MilitaryUnique = militaryunique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_CHANGE_POSTYPE_REQ, gS_SOLDIER_CHANGE_POSTYPE_REQ);
	}

	private void OpenRightClickMenu(ITEM pkItem)
	{
		if (Protocol_Item.Is_EquipItem(pkItem.m_nItemUnique) && pkItem.m_nPosType == 10)
		{
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(pkItem, CRightClickMenu.KIND.ITEM_EQUIP_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_1);
		}
	}

	private void OnClickEquipItem(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null)
		{
			return;
		}
		if (this.mSelectedSolinfo.IsSolWarehouse())
		{
			return;
		}
		UIListItemContainer uIListItemContainer = (UIListItemContainer)obj;
		if (null == uIListItemContainer)
		{
			return;
		}
		ImageSlot imageSlot = uIListItemContainer.Data as ImageSlot;
		if (imageSlot == null)
		{
			return;
		}
		bool flag = false;
		ITEM equipItem = this.mSelectedSolinfo.GetEquipItem(imageSlot.Index);
		if (equipItem != null && equipItem.IsValid())
		{
			flag = true;
		}
		else
		{
			if (imageSlot.Index == 5 && !this.mSelectedSolinfo.IsAtbCommonFlag(2L))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("731"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			int itemPosType;
			if (imageSlot.Index == 0)
			{
				itemPosType = this.mSelectedSolinfo.GetItemPosTypeByWeaponType();
			}
			else
			{
				itemPosType = 1;
			}
			if (imageSlot.Index == 0 && this.mSelectedSolinfo.GetCharKindInfo().IsATB(1L) && this.mSelectedSolinfo.GetCharKindInfo().GetCharTribe() == 2)
			{
				for (int i = 0; i < ItemDefine.INVENTORY_ITEMSLOT_MAX; i++)
				{
					for (int j = 2; j < 4; j++)
					{
						ITEM item = NkUserInventory.GetInstance().GetItem(j, i);
						if (item != null && item.IsValid())
						{
							if (NrTSingleton<ItemManager>.Instance.GetEquipItemPos(item.m_nItemUnique) == (eEQUIP_ITEM)imageSlot.Index)
							{
								if (Protocol_Item.Is_Item_Equipment(item, this.mSelectedSolinfo, false))
								{
									flag = true;
									break;
								}
							}
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < ItemDefine.INVENTORY_ITEMSLOT_MAX; k++)
				{
					ITEM item = NkUserInventory.GetInstance().GetItem(itemPosType, k);
					if (item != null && item.IsValid())
					{
						if (NrTSingleton<ItemManager>.Instance.GetEquipItemPos(item.m_nItemUnique) == (eEQUIP_ITEM)imageSlot.Index)
						{
							if (Protocol_Item.Is_Item_Equipment(item, this.mSelectedSolinfo, false))
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
		}
		if (flag)
		{
			if (equipItem.m_nItemUnique == 0)
			{
				SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLEQUIPITEMSELECT_DLG) as SolEquipItemSelectDlg;
				if (solEquipItemSelectDlg != null)
				{
					solEquipItemSelectDlg.SetData(ref this.mSelectedSolinfo, (eEQUIP_ITEM)imageSlot.Index);
					solEquipItemSelectDlg.SetLocationByForm(this);
					solEquipItemSelectDlg.SetFocus();
				}
			}
			else
			{
				ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
				itemTooltipDlg.SolInfo = this.mSelectedSolinfo;
				itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, equipItem, null, true);
			}
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("557"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	private void OnDoubleClickEquipItem(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		UIScrollList uIScrollList = obj as UIScrollList;
		if (uIScrollList != null)
		{
			UIListItemContainer selectedItem = uIScrollList.SelectedItem;
			if (selectedItem != null)
			{
				ImageSlot imageSlot = selectedItem.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.IsValid())
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
						Protocol_Item.Send_EquipSol_InvenEquip(iTEM, this.mSelectedSolinfo.GetSolID());
					}
				}
			}
		}
	}

	private void OnRightClickEquipItem(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (this.mSelectedSolinfo.IsSolWarehouse())
		{
			return;
		}
		UIScrollList uIScrollList = obj as UIScrollList;
		if (uIScrollList != null)
		{
			UIListItemContainer selectedItem = uIScrollList.SelectedItem;
			if (selectedItem != null)
			{
				ImageSlot imageSlot = selectedItem.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.IsValid())
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
						this.OpenRightClickMenu(iTEM);
					}
				}
			}
		}
	}

	private void OnMouseOverEquipItem(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null)
		{
			return;
		}
		UIScrollList uIScrollList = obj as UIScrollList;
		if (uIScrollList != null)
		{
			UIListItemContainer uIListItemContainer = uIScrollList.MouseItem as UIListItemContainer;
			if (uIListItemContainer != null)
			{
				ImageSlot imageSlot = uIListItemContainer.Data as ImageSlot;
				if (imageSlot != null)
				{
					ITEM iTEM = imageSlot.c_oItem as ITEM;
					if (iTEM != null && iTEM.IsValid())
					{
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						itemTooltipDlg.SolInfo = this.mSelectedSolinfo;
						itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
					}
				}
			}
		}
	}

	private void OnMouseOutEquipItem(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}

	private void OnClickSkillUpdate(IUIObject obj)
	{
		Button button = (Button)obj;
		if (button == null)
		{
			return;
		}
		int num = (int)button.Data;
		if (num < 0 || num >= 3)
		{
			return;
		}
		BATTLESKILL_TRAINING trainingSkillInfo = this.kSolSkillSlotControl.GetTrainingSkillInfo();
		if (trainingSkillInfo == null)
		{
			return;
		}
		SolSkillUpdateDlg solSkillUpdateDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLSKILLUPDATE_DLG) as SolSkillUpdateDlg;
		if (solSkillUpdateDlg != null)
		{
			solSkillUpdateDlg.SetData(ref this.mSelectedSolinfo, trainingSkillInfo.m_nSkillUnique);
			solSkillUpdateDlg.SetLocationByForm(this);
			solSkillUpdateDlg.SetFocus();
		}
	}

	private void OnClickSolDetailView(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null)
		{
			return;
		}
		SolDetailinfoDlg solDetailinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAILINFO_DLG) as SolDetailinfoDlg;
		if (solDetailinfoDlg != null)
		{
			if (this.mSelectedSolinfo.IsSolWarehouse())
			{
				solDetailinfoDlg.SetDataNotMySol(ref this.mSelectedSolinfo);
			}
			else
			{
				solDetailinfoDlg.SetData(ref this.mSelectedSolinfo);
			}
			solDetailinfoDlg.SetLocationByForm(this);
			solDetailinfoDlg.SetFocus();
		}
	}

	private void OnClickChangeFaceChar(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FACE_SOLINDEX);
		if ((int)this.mSelectedSolinfo.GetSolPosIndex() == num)
		{
			return;
		}
		if (this.mSelectedSolinfo.GetSolPosType() == 1)
		{
			GS_CHARACTER_SUBDATA_REQ gS_CHARACTER_SUBDATA_REQ = new GS_CHARACTER_SUBDATA_REQ();
			gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataType = 0;
			gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataValue = (long)this.mSelectedSolinfo.GetSolPosIndex();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_SUBDATA_REQ, gS_CHARACTER_SUBDATA_REQ);
			this.m_nChangeFaceCharKind = this.mSelectedSolinfo.GetCharKind();
		}
		this.m_bChangeFaceChar = true;
	}

	private void OnClickSolCure(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		if (!this.mSelectedSolinfo.IsInjuryStatus())
		{
			return;
		}
		ITEM firstFunctionItem = NkUserInventory.GetInstance().GetFirstFunctionItem(eITEM_SUPPLY_FUNCTION.SUPPLY_INJURYCURE);
		if (firstFunctionItem != null)
		{
			this.OnClickSolCure2(obj);
			return;
		}
		long injuryCureMoney = this.mSelectedSolinfo.GetInjuryCureMoney();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && kMyCharInfo.m_Money < injuryCureMoney)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		GS_SOLDIER_INJURYCURE_REQ gS_SOLDIER_INJURYCURE_REQ = new GS_SOLDIER_INJURYCURE_REQ();
		gS_SOLDIER_INJURYCURE_REQ.nSolID = this.mSelectedSolinfo.GetSolID();
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("19"),
			"gold",
			injuryCureMoney
		});
		msgBoxUI.SetMsg(new YesDelegate(this.SendSolImmediatelyCure), gS_SOLDIER_INJURYCURE_REQ, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("64"), empty, eMsgType.MB_OK_CANCEL);
	}

	private void OnClickSolCure2(IUIObject obj)
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return;
		}
		if (!this.mSelectedSolinfo.IsInjuryStatus())
		{
			return;
		}
		ITEM firstFunctionItem = NkUserInventory.GetInstance().GetFirstFunctionItem(eITEM_SUPPLY_FUNCTION.SUPPLY_INJURYCURE);
		if (firstFunctionItem == null)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("154");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(firstFunctionItem.m_nItemUnique);
		if (itemInfo == null || itemInfo.m_nParam[0] <= 0)
		{
			return;
		}
		long remainInjuryTime = this.mSelectedSolinfo.GetRemainInjuryTime();
		string text = PublicMethod.ConvertTime(remainInjuryTime);
		int num = (int)remainInjuryTime / 60;
		int num2 = num / itemInfo.m_nParam[0];
		if (remainInjuryTime % 60L > 0L)
		{
			num2++;
		}
		int num3 = Math.Min(num2, firstFunctionItem.m_nItemNum);
		int num4 = itemInfo.m_nParam[0] * num3;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("65"),
			"timestring",
			text
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("69"),
			"count1",
			num3,
			"targetname",
			this.mSelectedSolinfo.GetName(),
			"count",
			num4,
			"timestring",
			empty
		});
		msgBoxUI.SetMsg(new YesDelegate(this.SendSolImmediatelyCure2), firstFunctionItem, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("513"), empty2, eMsgType.MB_OK_CANCEL);
	}

	public void SendSolImmediatelyCure(object obj)
	{
		GS_SOLDIER_INJURYCURE_REQ obj2 = obj as GS_SOLDIER_INJURYCURE_REQ;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_INJURYCURE_REQ, obj2);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "TREATMENT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SendSolImmediatelyCure2(object obj)
	{
		ITEM pkItem = obj as ITEM;
		Protocol_Item.Item_Use(pkItem);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "TREATMENT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void OnClickSolAllCure(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("230"),
				"count",
				this.nInjurySoldierCount
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnClickSolAllCureOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("229"), empty, eMsgType.MB_OK_CANCEL);
		}
	}

	private void OnClickSolAllCureOK(object _Object)
	{
		NrTSingleton<NkCharManager>.Instance.IsInjuryCureAllChar = true;
	}

	public override void Hide_End()
	{
		base.Hide_End();
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLEQUIPITEMSELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLEQUIPITEMSELECT_DLG);
		}
	}

	public override void OnClose()
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLEQUIPITEMSELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLEQUIPITEMSELECT_DLG);
		}
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.Destroy(this.rootGameObject);
		}
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
	}

	public void ActionChangeFaceChar(int tosolindex)
	{
		if (!this.m_bChangeFaceChar)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(this.m_nFaceSolPosIndex);
		if (soldierInfo != null && soldierInfo.IsValid())
		{
			this.SetSoldierUpdate(soldierInfo);
		}
		soldierInfo = charPersonInfo.GetSoldierInfo(tosolindex);
		if (soldierInfo != null && soldierInfo.IsValid())
		{
			this.SetSoldierUpdate(soldierInfo);
		}
		this.m_nFaceSolPosIndex = tosolindex;
		if (this.m_nChangeFaceCharKind != 0 && !this.bRequest)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_nChangeFaceCharKind);
			if (charKindInfo == null)
			{
				return;
			}
			if (UIDataManager.IsUse256Texture())
			{
				this.faceImageKey = charKindInfo.GetPortraitFile1((int)soldierInfo.GetGrade()) + "_256";
			}
			else
			{
				this.faceImageKey = charKindInfo.GetPortraitFile1((int)soldierInfo.GetGrade()) + "_512";
			}
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey))
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.faceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
			}
			string str = string.Format("{0}", "UI/Soldier/fx_leaderchange" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetChangeFaceCharImage), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.bRequest = true;
			if (!(null == base.BLACK_BG) || !TsPlatform.IsWeb)
			{
				base.BLACK_BG.Visible = true;
			}
			base.BLACK_BG.SetAlpha(1f);
			base.BLACK_BG.SetLocation(base.BLACK_BG.GetLocation().x, base.BLACK_BG.GetLocationY(), -5f);
			base.BLACK_BG.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSkip));
		}
	}

	public void ClickSkip(IUIObject obj)
	{
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.SetLocation(base.BLACK_BG.GetLocation().x, base.BLACK_BG.GetLocationY(), 0.1f);
			base.BLACK_BG.Visible = false;
		}
		this.InitChangeface();
	}

	private void SetChangeFaceCharImage(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 500f;
				this.rootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				this.bLoadChangeFaceChar = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
			}
		}
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}

	public override void Update()
	{
		if (Scene.CurScene != Scene.Type.WORLD)
		{
			return;
		}
		if (this.bLoadChangeFaceChar && this.m_bChangeFaceChar && null != this.rootGameObject && !this.bSetFace)
		{
			GameObject gameObject = NkUtil.GetChild(this.rootGameObject.transform, "face").gameObject;
			if (null != gameObject)
			{
				Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
				if (null != texture)
				{
					Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
					if (null != material)
					{
						material.mainTexture = texture;
						if (null != gameObject.renderer)
						{
							gameObject.renderer.sharedMaterial = material;
						}
						this.fStartTime = Time.time;
						this.bSetFace = true;
						this.bLoadChangeFaceChar = false;
						this.m_bChangeFaceChar = false;
					}
				}
			}
		}
		if (this.bSetFace && Time.time - this.fStartTime > 2.5f)
		{
			this.ClickSkip(null);
			this.CloseForm(null);
			this.fStartTime = Time.time;
		}
		if (this.fSolCureTime > 0f && Time.time - this.fSolCureTime > 1f)
		{
			this.CheckSolInjuryStatus();
		}
		if (this.nInjurySoldierCount > 0)
		{
			this.CheckInjurySoldierList();
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (this.nCurrentAgreeState != myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ENABLE_PLUNDER))
		{
			this.SetSoldierUpdate(this.mLeaderSolinfo);
		}
	}

	public void InitChangeface()
	{
		if (null != this.rootGameObject)
		{
			UnityEngine.Object.DestroyImmediate(this.rootGameObject);
			this.bSetFace = false;
			this.bRequest = false;
			this.m_nChangeFaceCharKind = 0;
			this.faceImageKey = string.Empty;
		}
	}

	public void SetPortraitRefresh()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo leaderSoldierInfo = charPersonInfo.GetLeaderSoldierInfo();
		if (leaderSoldierInfo == null)
		{
			return;
		}
		this.SetSoldierUpdate(leaderSoldierInfo);
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

	private void SetEventHeroTexture()
	{
		if (this.mSelectedSolinfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.mSelectedSolinfo.GetCharKind());
		if (charKindInfo == null)
		{
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		this.m_SolInterfaceTool.SetHeroEventLabel(this.mSelectedSolinfo.GetGrade() + 1);
	}

	private void OnClickWarehouseList(IUIObject obj)
	{
		UIListItemContainer selectedItem = this.m_nlbWarehouseList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		long num = (long)selectedItem.data;
		if (0L >= num)
		{
			SolMilitaryGroupDlg.eSOLWAREHOUSESTATE eSOLWAREHOUSESTATE = (SolMilitaryGroupDlg.eSOLWAREHOUSESTATE)num;
			SolMilitaryGroupDlg.eSOLWAREHOUSESTATE eSOLWAREHOUSESTATE2 = eSOLWAREHOUSESTATE;
			if (eSOLWAREHOUSESTATE2 != SolMilitaryGroupDlg.eSOLWAREHOUSESTATE.eSOLWAREHOUSESTATE_EXPANSION)
			{
				if (eSOLWAREHOUSESTATE2 != SolMilitaryGroupDlg.eSOLWAREHOUSESTATE.eSOLWAREHOUSESTATE_EMPTY)
				{
				}
			}
			else
			{
				int warehouseCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWarehouseCount();
				if (this.m_iSolWarehouseMax <= warehouseCount)
				{
					return;
				}
				SolWarehouseExpansionDlg solWarehouseExpansionDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOL_WAREHOUSE_EXPANSION_DLG) as SolWarehouseExpansionDlg;
				if (solWarehouseExpansionDlg != null)
				{
					solWarehouseExpansionDlg.SetSolWarehouseInfo();
				}
			}
		}
		else
		{
			NkSoldierInfo solWarehouse = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouse(num);
			if (solWarehouse != null)
			{
				if (!solWarehouse.GetLoadAllInfo())
				{
					GS_SOLDIER_LOAD_GET_REQ gS_SOLDIER_LOAD_GET_REQ = new GS_SOLDIER_LOAD_GET_REQ();
					gS_SOLDIER_LOAD_GET_REQ.i64SolID = num;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_LOAD_GET_REQ, gS_SOLDIER_LOAD_GET_REQ);
				}
				else
				{
					this.mSelectedSolinfo = solWarehouse;
					this.SetDataForSoldier();
				}
			}
		}
	}

	private void MakeSolWarehouseList()
	{
		this.MakeSolWarehouseList(this.m_bRefreshWarehouse);
		this.m_bRefreshWarehouse = false;
	}

	private void MakeSolWarehouseList(bool bRefreshWarehouse)
	{
		if (!bRefreshWarehouse)
		{
			return;
		}
		List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
		if (solWarehouseList == null)
		{
			return;
		}
		this.m_nlbWarehouseList.Clear();
		NkSoldierInfo nkSoldierInfo = null;
		for (int i = 0; i < solWarehouseList.Count; i++)
		{
			NewListItem item = new NewListItem(this.m_nlbWarehouseList.ColumnNum, true);
			nkSoldierInfo = solWarehouseList[i];
			this.SetSolWarehouseListItemSol(ref item, ref nkSoldierInfo);
			this.m_nlbWarehouseList.Add(item);
		}
		int warehouseCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWarehouseCount();
		if (solWarehouseList.Count < warehouseCount)
		{
			int num = warehouseCount - solWarehouseList.Count;
			long lState = -1L;
			for (int i = 0; i < num; i++)
			{
				NewListItem item2 = new NewListItem(this.m_nlbWarehouseList.ColumnNum, true);
				this.SetSolWarehouseListItemEmpty(ref item2, lState);
				this.m_nlbWarehouseList.Add(item2);
			}
		}
		if (this.m_iSolWarehouseMax > warehouseCount)
		{
			int num;
			if (warehouseCount < 7)
			{
				num = 7 - warehouseCount;
			}
			else
			{
				num = 1;
			}
			long lState = -2L;
			for (int i = 0; i < num; i++)
			{
				NewListItem item3 = new NewListItem(this.m_nlbWarehouseList.ColumnNum, true);
				this.SetSolWarehouseListItemExpansion(ref item3, lState);
				this.m_nlbWarehouseList.Add(item3);
			}
		}
		this.m_nlbWarehouseList.RepositionItems();
		this.bSetFirstItem = false;
		this.SetSelectFirstItem(this.m_nlbWarehouseList);
		if (0 < solWarehouseList.Count)
		{
			if (!solWarehouseList[0].GetLoadAllInfo())
			{
				GS_SOLDIER_LOAD_GET_REQ gS_SOLDIER_LOAD_GET_REQ = new GS_SOLDIER_LOAD_GET_REQ();
				gS_SOLDIER_LOAD_GET_REQ.i64SolID = solWarehouseList[0].GetSolID();
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_LOAD_GET_REQ, gS_SOLDIER_LOAD_GET_REQ);
			}
			else
			{
				this.mSelectedSolinfo = solWarehouseList[0];
				this.SetDataForSoldier();
				this.m_nlbWarehouseList.SetSelectedItem(0);
			}
		}
	}

	public void RefreshSolWarehouse()
	{
		if (this.m_iTabIndex == 1)
		{
			this.SetLayerTabControl(this.m_iTabIndex);
		}
		this.MakeSolWarehouseList(true);
	}

	public void RefreshSolWarehouseAdd()
	{
		this.SetLayerTabControl(this.m_iTabIndex);
		this.m_dtEffect.Visible = true;
		Transform child = NkUtil.GetChild(this.m_dtEffect.gameObject.transform, "child_effect");
		if (child == null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_DIRECT_UNLOCK", this.m_dtEffect, this.m_dtEffect.GetSize());
		}
		this.MakeSolWarehouseList(true);
	}

	public void RefreshSolWarehouse(GS_SOLDIER_WAREHOUSE_GET_ACK ACK)
	{
		if (ACK.i8SolWarehouseLoadServerData == 1)
		{
			this.SetSolWarehouseMove(ACK.i64SolID);
		}
		else
		{
			this.SetLayerTabControl(this.m_iTabIndex);
			this.MakeSolWarehouseList(true);
		}
	}

	public void RefreshSolWarehouseLoadAllInfo(long lSolID)
	{
		NkSoldierInfo solWarehouse = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouse(lSolID);
		if (solWarehouse != null)
		{
			this.mSelectedSolinfo = solWarehouse;
			this.SetDataForSoldier();
		}
	}

	public void ClickSolWarehouseMove(IUIObject obj)
	{
		UIListItemContainer uIListItemContainer = this.mReadySolList.GetSelectItem() as UIListItemContainer;
		if (null == uIListItemContainer)
		{
			return;
		}
		if (uIListItemContainer.Data == null)
		{
			return;
		}
		long solWarehouseMove = (long)uIListItemContainer.data;
		this.SetSolWarehouseMove(solWarehouseMove);
	}

	public void ClickMilitaryInfo(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = obj.Data as NkSoldierInfo;
		if (nkSoldierInfo != null)
		{
			if (nkSoldierInfo.GetSolPosType() == 2)
			{
				GS_MINE_SOLDIER_INFO_REQ gS_MINE_SOLDIER_INFO_REQ = new GS_MINE_SOLDIER_INFO_REQ();
				gS_MINE_SOLDIER_INFO_REQ.byMilitaryUnique = nkSoldierInfo.GetMilitaryUnique();
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SOLDIER_INFO_REQ, gS_MINE_SOLDIER_INFO_REQ);
			}
			else if (nkSoldierInfo.GetSolPosType() == 6)
			{
				GS_EXPEDITION_DETAILINFO_REQ gS_EXPEDITION_DETAILINFO_REQ = new GS_EXPEDITION_DETAILINFO_REQ();
				gS_EXPEDITION_DETAILINFO_REQ.ui8ExpeditionMilitaryUniq = nkSoldierInfo.GetMilitaryUnique();
				gS_EXPEDITION_DETAILINFO_REQ.i64ExpeditionID = 0L;
				gS_EXPEDITION_DETAILINFO_REQ.bUserInfo = true;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_EXPEDITION_DETAILINFO_REQ, gS_EXPEDITION_DETAILINFO_REQ);
			}
		}
	}

	public void SetSolWarehouseMove(long SolID)
	{
		if (0L >= SolID)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = this.FindSimpleSolInfo(SolID);
		if (nkSoldierInfo == null)
		{
			return;
		}
		if (nkSoldierInfo.GetSolPosType() != 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("371"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.CheckSolWarehouseLoadServerData(1, SolID))
		{
			return;
		}
		if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsSolWarehouseMove())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("590"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		if (nkSoldierInfo.GetSolPosType() != 0)
		{
			return;
		}
		if (0L >= nkSoldierInfo.GetFriendPersonID())
		{
			GS_SOLDIER_WAREHOUSE_MOVE_REQ gS_SOLDIER_WAREHOUSE_MOVE_REQ = new GS_SOLDIER_WAREHOUSE_MOVE_REQ();
			gS_SOLDIER_WAREHOUSE_MOVE_REQ.i64SolID = SolID;
			gS_SOLDIER_WAREHOUSE_MOVE_REQ.i8SolPosType = 5;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_WAREHOUSE_MOVE_REQ, gS_SOLDIER_WAREHOUSE_MOVE_REQ);
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKUnsetSolHelp), nkSoldierInfo, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("156"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("155"), eMsgType.MB_OK_CANCEL);
		msgBoxUI.Show();
	}

	private void SetSolWarehouseListItemSol(ref NewListItem item, ref NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo == null)
		{
			return;
		}
		string text = string.Empty;
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(pkSolinfo.GetCharKind(), pkSolinfo.GetGrade());
		if (eventHeroCharCode != null)
		{
			item.SetListItemData(0, "Win_I_EventSol", null, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(pkSolinfo.GetCharKind(), (int)pkSolinfo.GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(0, legendFrame, null, null, null);
			}
		}
		item.SetListItemData(1, pkSolinfo.GetListSolInfo(false), null, null, null);
		item.SetListItemData(2, pkSolinfo.GetName(), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
			"count1",
			pkSolinfo.GetLevel().ToString(),
			"count2",
			pkSolinfo.GetSolMaxLevel().ToString()
		});
		item.SetListItemData(3, text, null, null, null);
		int num = pkSolinfo.GetEquipWeaponOrigin();
		if (num > 0)
		{
			item.SetListItemData(4, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
		}
		num = pkSolinfo.GetEquipWeaponExtention();
		if (num > 0)
		{
			item.SetListItemData(5, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
		}
		else
		{
			item.SetListItemData(5, false);
		}
		long num2 = pkSolinfo.GetExp() - pkSolinfo.GetCurBaseExp();
		long num3 = pkSolinfo.GetNextExp() - pkSolinfo.GetCurBaseExp();
		float num4 = 1f;
		if (!pkSolinfo.IsMaxLevel())
		{
			num4 = ((float)num3 - (float)pkSolinfo.GetRemainExp()) / (float)num3;
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			if (0f > num4)
			{
				num4 = 0f;
			}
		}
		item.SetListItemData(6, "Win_T_ReputelPrgBG", null, null, null);
		item.SetListItemData(7, "Com_T_GauWaPr4", this.READY_LIST_MAX_EXP_GAGUE * num4, null, null);
		if (pkSolinfo.IsMaxLevel())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
				"exp",
				num2.ToString(),
				"maxexp",
				num3.ToString()
			});
		}
		item.SetListItemData(8, text, null, null, null);
		item.SetListItemData(9, string.Empty, null, null, null);
		item.Data = pkSolinfo.GetSolID();
		item.SetListItemData(10, false);
		item.SetListItemData(11, false);
		item.SetListItemData(12, string.Empty, null, new EZValueChangedDelegate(this.ClickSolReadyMove), null);
	}

	private void SetSolWarehouseListItemEmpty(ref NewListItem Item, long lState)
	{
		for (int i = 0; i < this.m_nlbWarehouseList.ColumnNum; i++)
		{
			Item.SetListItemData(i, false);
		}
		Item.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2188"), null, null, null);
		Item.SetListItemData(9, true);
		Item.Data = lState;
	}

	private void SetSolWarehouseListItemExpansion(ref NewListItem Item, long lState)
	{
		for (int i = 0; i < this.m_nlbWarehouseList.ColumnNum; i++)
		{
			if (1 < i)
			{
				Item.SetListItemData(i, false);
			}
		}
		Item.SetListItemData(1, "Win_I_Lock", null, null, null);
		Item.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2180"), null, null, null);
		Item.SetListItemData(9, true);
		Item.Data = lState;
	}

	public void ClickSolReadyMove(IUIObject obj)
	{
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null || readySolList.GetCount() >= 50)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("118"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		UIListItemContainer uIListItemContainer = this.m_nlbWarehouseList.GetSelectItem() as UIListItemContainer;
		if (null == uIListItemContainer)
		{
			return;
		}
		if (uIListItemContainer.Data == null)
		{
			return;
		}
		long num = (long)uIListItemContainer.data;
		if (0L >= num)
		{
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouse(num) == null)
		{
			return;
		}
		GS_SOLDIER_WAREHOUSE_MOVE_REQ gS_SOLDIER_WAREHOUSE_MOVE_REQ = new GS_SOLDIER_WAREHOUSE_MOVE_REQ();
		gS_SOLDIER_WAREHOUSE_MOVE_REQ.i64SolID = num;
		gS_SOLDIER_WAREHOUSE_MOVE_REQ.i8SolPosType = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_WAREHOUSE_MOVE_REQ, gS_SOLDIER_WAREHOUSE_MOVE_REQ);
	}

	public void MsgBoxOKUnsetSolHelp(object a_oObject)
	{
		NkSoldierInfo nkSoldierInfo = a_oObject as NkSoldierInfo;
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

	private void OnClickReincarnation(IUIObject obj)
	{
		this.Hide();
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REINCARNATION_DLG);
	}

	public void RefereshSelectSolInfo(NkSoldierInfo Soldier)
	{
		if (this.mSelectedSolinfo == null || Soldier == null)
		{
			return;
		}
		if (Soldier.GetSolID() != this.mSelectedSolinfo.GetSolID())
		{
			return;
		}
		this.SetSelectedSolinfo(Soldier);
		this.SetDataForSoldier();
	}

	public bool IsReincarnation()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
		{
			return false;
		}
		if (this.mSelectedSolinfo == null)
		{
			return false;
		}
		if (!this.mSelectedSolinfo.GetCharKindInfo().IsATB(1L))
		{
			return false;
		}
		if (!this.mSelectedSolinfo.IsLeader())
		{
			return false;
		}
		if (!this.mSelectedSolinfo.IsMaxGrade())
		{
			return false;
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_REINCARNATION_COUNT);
		int num2 = num + 1;
		ReincarnationInfo reincarnation = NrTSingleton<NrBaseTableManager>.Instance.GetReincarnation(num.ToString());
		ReincarnationInfo reincarnation2 = NrTSingleton<NrBaseTableManager>.Instance.GetReincarnation(num2.ToString());
		if (reincarnation == null || reincarnation2 == null)
		{
			return false;
		}
		int reincarnationCharKind = reincarnation2.GetReincarnationCharKind(reincarnation, this.mSelectedSolinfo.GetCharKind());
		return 0 < reincarnationCharKind && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel() >= reincarnation2.iNeedLevel;
	}

	public void SetShowReincarnation(bool bShow)
	{
		this.m_btReincarnation.Hide(!bShow);
		this.m_dtRank2.Hide(!bShow);
		this.m_dtReincarnationEffect.Visible = bShow;
	}

	public void SortReadySolList()
	{
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		this.m_kReadySolSortList.Clear();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			this.AddSolList(current, eSOL_POSTYPE.SOLPOS_READY, this.m_kReadySolSortList);
		}
		this.m_kReadySolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareExp));
	}

	public void SetSortList()
	{
		this.m_dlReadySolSort.Clear();
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1886"), 0);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1887"), 1);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1888"), 2);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1889"), 3);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"), 4);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"), 5);
		this.m_dlReadySolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"), 6);
		this.m_dlReadySolSort.SetViewArea(this.m_dlReadySolSort.Count);
		this.m_dlReadySolSort.RepositionItems();
		this.m_dlReadySolSort.SetFirstItem();
		this.m_dlWarehouseSolSort.Clear();
		this.m_dlWarehouseSolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1888"), 2);
		this.m_dlWarehouseSolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1889"), 3);
		this.m_dlWarehouseSolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"), 4);
		this.m_dlWarehouseSolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"), 5);
		this.m_dlWarehouseSolSort.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"), 6);
		this.m_dlWarehouseSolSort.SetViewArea(this.m_dlWarehouseSolSort.Count);
		this.m_dlWarehouseSolSort.RepositionItems();
		this.m_dlWarehouseSolSort.SetFirstItem();
	}

	private void OnChangeReadySortOrder(IUIObject obj)
	{
		this.m_iReadySortOrder = 0;
		if (this.m_dlReadySolSort.Count > 0 && this.m_dlReadySolSort.SelectedItem != null)
		{
			ListItem listItem = this.m_dlReadySolSort.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_iReadySortOrder = (int)listItem.Key;
			}
		}
		PlayerPrefs.SetInt(NrPrefsKey.SOLMILITARYGROUP_READY_SORT, this.m_iReadySortOrder);
		this.SetSolSort(this.m_iReadySortOrder, this.m_kReadySolSortList);
		this.SetSoldierList(eSOL_POSTYPE.SOLPOS_READY);
	}

	private void OnChangeWarehouseSortOrder(IUIObject obj)
	{
		this.m_iWarehouseSortOrder = 0;
		if (this.m_dlWarehouseSolSort.Count > 0 && this.m_dlWarehouseSolSort.SelectedItem != null)
		{
			ListItem listItem = this.m_dlWarehouseSolSort.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				this.m_iWarehouseSortOrder = (int)listItem.Key;
			}
		}
		PlayerPrefs.SetInt(NrPrefsKey.SOLMILITARYGROUP_WAREHOUSE_SORT, this.m_iWarehouseSortOrder);
		List<NkSoldierInfo> solWarehouseList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSolWarehouseList();
		if (solWarehouseList != null)
		{
			this.SetSolSort(this.m_iWarehouseSortOrder, solWarehouseList);
		}
		this.SetSoldierList(eSOL_POSTYPE.SOLPOS_WAREHOUSE);
	}

	private void SetSolSort(int iSolSortOrder, List<NkSoldierInfo> SolSortList)
	{
		if (SolSortList.Count > 0)
		{
			switch (iSolSortOrder)
			{
			case 0:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPowerDESC));
				break;
			case 1:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPowerASC));
				break;
			case 2:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelDESC));
				break;
			case 3:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareLevelASC));
				break;
			case 4:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareName));
				break;
			case 5:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeDESC));
				break;
			case 6:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareGradeASC));
				break;
			case 7:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareFightPowerDESC));
				break;
			case 8:
				SolSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareFightPowerASC));
				break;
			}
		}
	}

	private int CompareLevelDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private int CompareLevelASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetLevel().CompareTo(b.GetLevel());
	}

	private int CompareName(NkSoldierInfo a, NkSoldierInfo b)
	{
		if (a.GetName().Equals(b.GetName()))
		{
			return this.CompareLevelDESC(a, b);
		}
		return a.GetName().CompareTo(b.GetName());
	}

	private int CompareGradeDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_High(a, b);
	}

	private int CompareGradeASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return SolComposeListDlg.CompareGrade_Low(a, b);
	}

	private int CompareFightPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetFightPower().CompareTo(a.GetFightPower());
	}

	private int CompareFightPowerASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetFightPower().CompareTo(b.GetFightPower());
	}

	private int CompareCombatPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int CompareCombatPowerASC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return a.GetCombatPower().CompareTo((long)b.GetFightPower());
	}

	public void RefreshSolInfo(NkSoldierInfo pkSoldierInfo)
	{
		switch (this.m_iTabIndex)
		{
		case 0:
			for (int i = 0; i < this.mBattleSolList.Count; i++)
			{
				UIListItemContainer uIListItemContainer = this.mBattleSolList.GetItem(i) as UIListItemContainer;
				if (!(uIListItemContainer == null))
				{
					long num = (long)uIListItemContainer.data;
					if (num == pkSoldierInfo.GetSolID())
					{
						this.RemoveAddBattleSolList(i, pkSoldierInfo);
						return;
					}
				}
			}
			break;
		case 1:
			for (int i = 0; i < this.mReadySolList.Count; i++)
			{
				UIListItemContainer uIListItemContainer = this.mReadySolList.GetItem(i) as UIListItemContainer;
				if (!(uIListItemContainer == null))
				{
					long num = (long)uIListItemContainer.data;
					if (num == pkSoldierInfo.GetSolID())
					{
						this.RemoveAddReadySolList(i, pkSoldierInfo);
						return;
					}
				}
			}
			break;
		case 2:
			for (int i = 0; i < this.m_nlbWarehouseList.Count; i++)
			{
				UIListItemContainer uIListItemContainer = this.m_nlbWarehouseList.GetItem(i) as UIListItemContainer;
				if (!(uIListItemContainer == null))
				{
					long num = (long)uIListItemContainer.data;
					if (num == pkSoldierInfo.GetSolID())
					{
						this.RemoveAddWarehouseSolList(i, pkSoldierInfo);
						return;
					}
				}
			}
			break;
		}
	}

	public void ClickGuildWarApplyInfo(IUIObject obj)
	{
		if (obj.Data == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = obj.Data as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		if (nkSoldierInfo.GetSolPosType() != 7)
		{
			return;
		}
		byte guildWarRaidUnique = nkSoldierInfo.GetGuildWarRaidUnique();
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_MILITARY_INFO_REQ(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), guildWarRaidUnique);
		this.Close();
	}
}
