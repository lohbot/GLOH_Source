using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class Battle_SkilldescDlg : Form
{
	private int BUFFSKILLTEXT_LAYER_1 = 1;

	private int DEAD_SOLLIST_LAYER_2 = 2;

	private int MAX_BATTLE_SKILL_SHOWUP = 30;

	private Label m_lbBuffText;

	private ItemTexture m_dwBuffIcon;

	private NewListBox m_nlDeadSolList;

	private string[] m_nCheckBuffIconCode = new string[139];

	private int[] m_nCheckBattleSkillUnique;

	private float m_fBuffIconTime;

	private int BATTLE_BUFFICON_SHOWTIME;

	private int nReviveCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/dlg_battle_skilldesc", G_ID.BATTLE_SKILLDESC_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbBuffText = (base.GetControl("Label_buff_desc") as Label);
		this.m_dwBuffIcon = (base.GetControl("ItemTexture_buffIcon") as ItemTexture);
		this.m_nlDeadSolList = (base.GetControl("NLB_Resurrection") as NewListBox);
		this.m_nlDeadSolList.Reserve = false;
		this.m_nlDeadSolList.Clear();
		base.AllHideLayer();
		this.m_nCheckBattleSkillUnique = new int[this.MAX_BATTLE_SKILL_SHOWUP];
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		this.BATTLE_BUFFICON_SHOWTIME = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_SKILL_DESC_TIME);
		if (this.BATTLE_BUFFICON_SHOWTIME == 0)
		{
			this.BATTLE_BUFFICON_SHOWTIME = 3;
		}
		this._SetDialogPos();
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void InitData()
	{
		this.InitBuffIconUnique();
		this.InitSkillIconUnique();
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
	}

	public override void Update()
	{
		if (this.m_fBuffIconTime > 0f && this.m_fBuffIconTime + (float)this.BATTLE_BUFFICON_SHOWTIME < Time.time && this.m_dwBuffIcon.Visible)
		{
			base.AllHideLayer();
			this.m_fBuffIconTime = 0f;
		}
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void InitBuffIconUnique()
	{
		for (int i = 0; i < 139; i++)
		{
			this.m_nCheckBuffIconCode[i] = string.Empty;
		}
	}

	public void InitSkillIconUnique()
	{
		for (int i = 0; i < this.MAX_BATTLE_SKILL_SHOWUP; i++)
		{
			this.m_nCheckBattleSkillUnique[i] = 0;
		}
	}

	public bool CheckBuffIconUnique(string nBuffIconCode)
	{
		for (int i = 0; i < 139; i++)
		{
			if (this.m_nCheckBuffIconCode[i] == nBuffIconCode)
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckBattleSkillUnique(int nBattleSkillUnique)
	{
		for (int i = 0; i < this.MAX_BATTLE_SKILL_SHOWUP; i++)
		{
			if (this.m_nCheckBattleSkillUnique[i] == nBattleSkillUnique)
			{
				return false;
			}
		}
		return true;
	}

	public void SetBuffIconUnique(string nBuffIconCode)
	{
		for (int i = 0; i < 139; i++)
		{
			if (this.m_nCheckBuffIconCode[i] == string.Empty)
			{
				this.m_nCheckBuffIconCode[i] = nBuffIconCode;
				return;
			}
		}
	}

	public void SetBattleSkillUnique(int nBattleSkillUnique)
	{
		for (int i = 0; i < this.MAX_BATTLE_SKILL_SHOWUP; i++)
		{
			if (this.m_nCheckBattleSkillUnique[i] == 0)
			{
				this.m_nCheckBattleSkillUnique[i] = nBattleSkillUnique;
				return;
			}
		}
	}

	public void SetBuffTextShowUp(int skillunique, NkBattleChar ActionBattleChar)
	{
		if (this.m_nlDeadSolList.Count > 0)
		{
			return;
		}
		if (Battle.BATTLE.ColosseumObserver)
		{
			return;
		}
		if (this.m_dwBuffIcon.Visible)
		{
			return;
		}
		if (!NrTSingleton<BattleSkill_Manager>.Instance.GetBuffSkillTextUse())
		{
			return;
		}
		if ((Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT) && Battle.BabelPartyCount != 1)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillunique);
		if (battleSkillBase == null)
		{
			return;
		}
		if (ActionBattleChar != null)
		{
			if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDESC(skillunique) == string.Empty)
			{
				return;
			}
			if (!this.CheckBattleSkillUnique(battleSkillBase.m_nSkillUnique))
			{
				return;
			}
			this.m_dwBuffIcon.Clear();
			this.m_lbBuffText.Clear();
			this.SetBattleSkillUnique(battleSkillBase.m_nSkillUnique);
			this.m_dwBuffIcon.SetSolImageTexure(eCharImageType.SMALL, ActionBattleChar.GetCharKindInfo().GetCharKind(), -1);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_nSkillDESC),
				"targetname",
				ActionBattleChar.GetCharName()
			});
			this.m_lbBuffText.SetText(empty);
			base.AllHideLayer();
			base.SetShowLayer(this.BUFFSKILLTEXT_LAYER_1, true);
			this.m_fBuffIconTime = Time.time;
		}
		else
		{
			string text = string.Empty;
			for (int i = 0; i < 2; i++)
			{
				if (this.CheckBuffIconUnique(battleSkillBase.m_nBuffIconCode[i]))
				{
					text = battleSkillBase.m_nBuffIconCode[i];
					string battleSkillBuffIconDESC = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBuffIconDESC(text);
					UIBaseInfoLoader battleSkillBuffIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBuffIconTexture(text);
					this.m_dwBuffIcon.Clear();
					this.m_lbBuffText.Clear();
					if (battleSkillBuffIconDESC != string.Empty && battleSkillBuffIconTexture != null)
					{
						this.SetBuffIconUnique(text);
						this.m_dwBuffIcon.SetTexture(battleSkillBuffIconTexture);
						this.m_lbBuffText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBuffIconDESC));
						base.AllHideLayer();
						base.SetShowLayer(this.BUFFSKILLTEXT_LAYER_1, true);
						this.m_fBuffIconTime = Time.time;
					}
					else
					{
						base.AllHideLayer();
						base.SetShowLayer(this.BUFFSKILLTEXT_LAYER_1, false);
					}
					return;
				}
			}
		}
	}

	public void SetDeadSol(long nSolID)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			return;
		}
		if ((Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT) && Battle.BabelPartyCount != 1)
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			return;
		}
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		int num = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_RESURRECTION_COUNT);
		if (this.nReviveCount >= num)
		{
			if (this.m_nlDeadSolList.Count > 0)
			{
				this.m_nlDeadSolList.Clear();
			}
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(nSolID);
		if (soldierInfoFromSolID == null || !soldierInfoFromSolID.IsValid())
		{
			return;
		}
		if (this.m_nlDeadSolList.Count >= 3)
		{
			this.m_nlDeadSolList.RemoveItem(0, true);
		}
		NewListItem newListItem = new NewListItem(this.m_nlDeadSolList.ColumnNum, true, string.Empty);
		newListItem.Data = soldierInfoFromSolID.GetSolID();
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = soldierInfoFromSolID.GetCharKind();
		nkListSolInfo.SolGrade = (int)soldierInfoFromSolID.GetGrade();
		nkListSolInfo.SolLevel = soldierInfoFromSolID.GetLevel();
		nkListSolInfo.FightPower = soldierInfoFromSolID.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
		nkListSolInfo.ShowLevel = false;
		nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(soldierInfoFromSolID);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(soldierInfoFromSolID.GetCharKind());
		if (charKindInfo != null)
		{
			short gradeMaxLevel = charKindInfo.GetGradeMaxLevel((short)soldierInfoFromSolID.GetGrade());
			if (soldierInfoFromSolID.GetLevel() >= gradeMaxLevel)
			{
				nkListSolInfo.ShowCombat = true;
				nkListSolInfo.ShowLevel = false;
			}
			else
			{
				nkListSolInfo.ShowCombat = false;
				nkListSolInfo.ShowLevel = true;
			}
		}
		newListItem.SetListItemData(1, nkListSolInfo, soldierInfoFromSolID, new EZValueChangedDelegate(this.BtClickUpListBox), null);
		this.m_nlDeadSolList.Add(newListItem);
		this.m_nlDeadSolList.RepositionItems();
		base.AllHideLayer();
		base.ShowLayer(this.DEAD_SOLLIST_LAYER_2);
	}

	private void BtClickUpListBox(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = obj.Data as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		long num = 0L;
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(nkSoldierInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			num = charSpend.iResurrection_spend;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("175"),
			"targetname",
			nkSoldierInfo.GetName(),
			"count",
			num.ToString()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnReviveCharOk), nkSoldierInfo, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("174"), empty, eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnReviveCharOk(object a_oObject)
	{
		NkSoldierInfo nkSoldierInfo = a_oObject as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		long num = 0L;
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(nkSoldierInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			num = charSpend.iResurrection_spend;
		}
		if (num > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		long solID = nkSoldierInfo.GetSolID();
		GS_BATTLE_REVIVE_SOLDIER_REQ gS_BATTLE_REVIVE_SOLDIER_REQ = new GS_BATTLE_REVIVE_SOLDIER_REQ();
		gS_BATTLE_REVIVE_SOLDIER_REQ.nReviveSolID = solID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_REVIVE_SOLDIER_REQ, gS_BATTLE_REVIVE_SOLDIER_REQ);
	}

	public void AddReviveCount(long nSolID)
	{
		this.nReviveCount++;
		BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		int num = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_RESURRECTION_COUNT);
		if (this.nReviveCount >= num)
		{
			if (this.m_nlDeadSolList.Count > 0)
			{
				this.m_nlDeadSolList.Clear();
			}
			return;
		}
		for (int i = 0; i < this.m_nlDeadSolList.Count; i++)
		{
			IUIObject item = this.m_nlDeadSolList.GetItem(i);
			NkSoldierInfo nkSoldierInfo = item.Data as NkSoldierInfo;
			if (nkSoldierInfo != null)
			{
				if (nkSoldierInfo.GetSolID() == nSolID)
				{
					this.m_nlDeadSolList.RemoveItem(i, true);
					return;
				}
			}
		}
	}
}
