using GAME;
using PlunderSolNumDlgUI;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class PlunderSolNumDlg : Form
{
	public static int _syncSolCombinationUniqueKey = -1;

	private Label m_lHelpText;

	private Label m_lCharName;

	private Label m_lCharType;

	private Label m_lSkillInfo;

	private Button m_solCombinationButton;

	private DropDownList m_ddlSolCombination;

	private SolCompleteCombinationDDLManager m_solCompleteCombination;

	private DrawTexture m_solCombinationBG;

	private DrawTexture m_dtRightBG;

	private Label m_solCompleteCombinationLabel;

	private Label m_solCombinationBTN;

	private Label m_lbTitle;

	private Label m_lbAllFightPower;

	private Label m_lbInfiFightPower_user;

	private Label m_lbInfiFightPower_enemy;

	private Label m_lbInfiVS;

	private long SumFightPower_Ally0;

	private long SumFightPower_Ally1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_solnum", G_ID.PLUNDERSOLNUM_DLG, false);
		base.bCloseAni = false;
		base.DonotDepthChange(1000f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this.m_solCompleteCombination = new SolCompleteCombinationDDLManager();
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_title") as Label);
		this.m_lHelpText = (base.GetControl("Label_Help") as Label);
		this.m_lCharName = (base.GetControl("Label_Charname") as Label);
		this.m_lCharType = (base.GetControl("Label_Chartype") as Label);
		this.m_lSkillInfo = (base.GetControl("Label_Skill") as Label);
		this.m_solCombinationButton = (base.GetControl("BT_combination") as Button);
		this.m_solCombinationButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickCombination));
		this.m_ddlSolCombination = (base.GetControl("DropDownList_1") as DropDownList);
		this.m_ddlSolCombination.Clear();
		this.m_ddlSolCombination.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_CombinationDDL));
		this.m_solCompleteCombinationLabel = (base.GetControl("Label_combination") as Label);
		this.m_solCombinationBTN = (base.GetControl("Label_combinationBTN") as Label);
		this.m_dtRightBG = (base.GetControl("DrawTexture_TitleBGTopRight") as DrawTexture);
		this.m_solCombinationBG = (base.GetControl("DT_CombinationBG") as DrawTexture);
		this.m_lbAllFightPower = (base.GetControl("LB_FightPower") as Label);
		this.m_lbAllFightPower.Visible = false;
		this.m_lbInfiFightPower_user = (base.GetControl("LB_FightPower_user") as Label);
		this.m_lbInfiFightPower_user.Visible = false;
		this.m_lbInfiFightPower_enemy = (base.GetControl("LB_FightPower_enemy") as Label);
		this.m_lbInfiFightPower_enemy.Visible = false;
		this.m_lbInfiVS = (base.GetControl("Label_VS") as Label);
		this.m_lbInfiVS.Visible = false;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			this.m_ddlSolCombination.SetVisible(false);
			this.m_solCompleteCombinationLabel.gameObject.SetActive(true);
			this.RenewCompleteCombinationLabel(PlunderSolNumDlg._syncSolCombinationUniqueKey, 0);
			base.SetShowLayer(3, false);
		}
		else
		{
			if (this.m_solCompleteCombination.IsLeader())
			{
				this.m_ddlSolCombination.SetVisible(true);
				this.m_solCompleteCombinationLabel.gameObject.SetActive(false);
			}
			else
			{
				this.m_ddlSolCombination.SetVisible(false);
				this.m_solCompleteCombinationLabel.gameObject.SetActive(true);
				this.RenewCompleteCombinationLabel(PlunderSolNumDlg._syncSolCombinationUniqueKey, 0);
			}
			this.RenewCompleteCombinationDDL(0);
		}
		float x = 0f;
		float y = 0f;
		base.SetLocation(x, y, base.GetLocation().z);
		this.VisibleCheckSolCombinationUI();
	}

	public override void InitData()
	{
	}

	public void CalFightPower(long SolID, bool bAddFightPower)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		this.m_lbAllFightPower.Clear();
		string text = string.Empty;
		long num = 0L;
		NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(SolID);
		if (soldierInfoFromSolID != null)
		{
			num = soldierInfoFromSolID.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
			if (!bAddFightPower)
			{
				num *= -1L;
			}
		}
		switch (SoldierBatch.SOLDIER_BATCH_MODE)
		{
		case eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER:
		case eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP:
		case eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP:
		case eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP:
		case eSOLDIER_BATCH_MODE.MODE_MYTHRAID:
		case eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION:
		case eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON:
		{
			this.SumFightPower_Ally0 += num;
			string empty = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3381");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"charname",
				charPersonInfo.GetCharName(),
				"count",
				ANNUALIZED.Convert(this.SumFightPower_Ally0)
			});
			this.m_lbAllFightPower.SetText(empty);
			this.m_lbAllFightPower.Visible = true;
			break;
		}
		case eSOLDIER_BATCH_MODE.MODE_INFIBATTLE:
		case eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE:
		{
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			this.SumFightPower_Ally0 += num;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3382");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				text,
				"charname",
				charPersonInfo.GetCharName(),
				"count",
				ANNUALIZED.Convert(this.SumFightPower_Ally0)
			});
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3382");
			string enemyUserName = SoldierBatch.SOLDIERBATCH.GetEnemyUserName();
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				text,
				"charname",
				enemyUserName,
				"count",
				ANNUALIZED.Convert(this.SumFightPower_Ally1)
			});
			this.m_lbInfiFightPower_user.SetText(empty2);
			this.m_lbInfiFightPower_enemy.SetText(empty3);
			this.m_lbInfiFightPower_user.Visible = true;
			this.m_lbInfiFightPower_enemy.Visible = true;
			this.m_lbInfiVS.Visible = true;
			break;
		}
		}
	}

	public void SetSumFightPowerAlly0(long FightPowerAlly0, bool bReMath)
	{
		if (!bReMath)
		{
			this.SumFightPower_Ally0 = FightPowerAlly0;
		}
		this.SetInfiBattleFightPower();
	}

	public void ShowAllFightPower(eSOLDIER_BATCH_MODE SOLDIER_BATCH_MODE, long FightPowerAlly0)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		string text = string.Empty;
		switch (SOLDIER_BATCH_MODE)
		{
		case eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER:
		case eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP:
		case eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP:
		case eSOLDIER_BATCH_MODE.MODE_MYTHRAID:
		case eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION:
		case eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON:
			this.SumFightPower_Ally0 = FightPowerAlly0;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3381");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"charname",
				charPersonInfo.GetCharName(),
				"count",
				ANNUALIZED.Convert(this.SumFightPower_Ally0)
			});
			this.m_lbAllFightPower.SetText(empty);
			this.m_lbAllFightPower.Visible = true;
			break;
		}
	}

	public void SetInfiBattleFightPower()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text = string.Empty;
		string enemyUserName = SoldierBatch.SOLDIERBATCH.GetEnemyUserName();
		this.SumFightPower_Ally1 = 0L;
		for (int i = 0; i < 15; i++)
		{
			PLUNDER_TARGET_INFO infiDefenseSolInfo = SoldierBatch.SOLDIERBATCH.GetInfiDefenseSolInfo(i);
			if (infiDefenseSolInfo != null)
			{
				this.SumFightPower_Ally1 += (long)infiDefenseSolInfo.nFightPower;
			}
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3382");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"charname",
			charPersonInfo.GetCharName(),
			"count",
			ANNUALIZED.Convert(this.SumFightPower_Ally0)
		});
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3382");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			text,
			"charname",
			enemyUserName,
			"count",
			ANNUALIZED.Convert(this.SumFightPower_Ally1)
		});
		this.m_lbInfiFightPower_user.SetText(empty);
		this.m_lbInfiFightPower_enemy.SetText(empty2);
		this.m_lbInfiFightPower_user.Visible = true;
		this.m_lbInfiFightPower_enemy.Visible = true;
		this.m_lbInfiVS.Visible = true;
	}

	public float GetTitleBarLocationY()
	{
		return this.m_lHelpText.GetLocationY();
	}

	public void SetExplain()
	{
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
	}

	public void SetSeleteSol(long nSolID)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(nSolID);
		if (soldierInfoFromSolID == null)
		{
			base.SetShowLayer(1, true);
			base.SetShowLayer(2, false);
		}
		else
		{
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, true);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("567");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"targetname",
				soldierInfoFromSolID.GetName(),
				"count",
				soldierInfoFromSolID.GetLevel().ToString()
			});
			string text = string.Empty;
			NrCharKindInfo charKindInfo = soldierInfoFromSolID.GetCharKindInfo();
			if (charKindInfo != null)
			{
				if (charKindInfo.GetCHARKIND_ATTACKINFO().ATTACKTYPE == soldierInfoFromSolID.GetAttackInfo().ATTACKTYPE)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec1);
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec2);
				}
			}
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("992");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromInterface2,
				"type",
				text
			});
			int num = 0;
			string text2 = string.Empty;
			List<BATTLESKILL_TRAINING> battleSkillTrainingGroup = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTrainingGroup(soldierInfoFromSolID);
			if (battleSkillTrainingGroup != null)
			{
				foreach (BATTLESKILL_TRAINING current in battleSkillTrainingGroup)
				{
					int nSkillUnique = current.m_nSkillUnique;
					BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(nSkillUnique);
					if (battleSkillBase != null)
					{
						if (!soldierInfoFromSolID.IsCostumeEquip() || this.IsCostumeSkill(soldierInfoFromSolID, nSkillUnique))
						{
							num = soldierInfoFromSolID.GetBattleSkillLevel(current.m_nSkillUnique);
							text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
							break;
						}
					}
				}
			}
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292");
			string empty3 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				textFromInterface3,
				"skillname",
				text2,
				"skilllevel",
				num.ToString()
			});
			this.m_lCharName.Text = empty;
			this.m_lCharType.Text = empty2;
			this.m_lSkillInfo.Text = empty3;
		}
		this.GuildBossBattleUserName();
	}

	public void GuildBossBattleUserName()
	{
		string empty = string.Empty;
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(SoldierBatch.GUILDBOSS_INFO.m_i64CurPlayer);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP && memberInfoFromPersonID != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1963"),
				"targetname",
				memberInfoFromPersonID.GetCharName()
			});
			this.m_lHelpText.Visible = true;
			this.m_lHelpText.Text = empty;
		}
	}

	public void On_ClickCombination(IUIObject a_cObject)
	{
		SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
		if (solCombination_Dlg == null)
		{
			Debug.LogError("ERROR, SolGuide_Dlg.cs, On_ClickCombination(), SolCombination_Dlg is Null");
			return;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			solCombination_Dlg.MakeCombinationSolUI(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetOwnBattleMinePossibleKindList(), -1);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION)
		{
			PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
			if (plunderSolListDlg != null)
			{
				solCombination_Dlg.MakeCombinationSolUI(SoldierBatch_SolList.GetSolKindList(eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION), -1);
			}
		}
		else
		{
			solCombination_Dlg.MakeCombinationSolUI(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetOwnBattleReadyAndReadySolKindList(), -1);
		}
	}

	public void SetSolNum(int nSolNum)
	{
	}

	private void Change_CombinationDDL(IUIObject obj)
	{
		if (this.m_ddlSolCombination == null)
		{
			Debug.LogError("ERROR, PlunderSolNumDlg.cs, Change_CombinationDDL(), m_ddlSolCombination is Null");
			return;
		}
		this.m_solCompleteCombination.ChangeIdx(this.m_ddlSolCombination, 0);
	}

	public void RenewCompleteCombinationDDL(short STARTPOS_INDEX)
	{
		this.m_solCompleteCombination.RenewDDL(this.m_ddlSolCombination, 0);
	}

	public void RenewCompleteCombinationLabel(int solCombinationUniqueKey, int STARTPOS_INDEX)
	{
		if (Battle.isLeader)
		{
			return;
		}
		Debug.Log("Party RenewCompleteCombinationLabel UniqueKey : " + solCombinationUniqueKey);
		this.m_solCompleteCombinationLabel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetTextTitleKeyByUniqeKey(solCombinationUniqueKey));
		NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.SetUserSelectedUniqeKey(solCombinationUniqueKey, STARTPOS_INDEX);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			byte mode;
			if (SoldierBatch.BABELTOWER_INFO.IsBabelLeader(charPersonInfo.GetPersonID()))
			{
				mode = 0;
			}
			else
			{
				mode = 1;
			}
			GS_BABELTOWER_LEAVE_REQ gS_BABELTOWER_LEAVE_REQ = new GS_BABELTOWER_LEAVE_REQ();
			gS_BABELTOWER_LEAVE_REQ.mode = mode;
			gS_BABELTOWER_LEAVE_REQ.nLeavePersonID = charPersonInfo.GetPersonID();
			gS_BABELTOWER_LEAVE_REQ.nBabelRoomIndex = SoldierBatch.BABELTOWER_INFO.m_nBabelRoomIndex;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_LEAVE_REQ, gS_BABELTOWER_LEAVE_REQ);
			SoldierBatch.BABELTOWER_INFO.Init();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			byte mode2;
			if (SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(charPersonInfo2.GetPersonID()))
			{
				mode2 = 0;
			}
			else
			{
				mode2 = 1;
			}
			GS_MYTHRAID_LEAVE_REQ gS_MYTHRAID_LEAVE_REQ = new GS_MYTHRAID_LEAVE_REQ();
			gS_MYTHRAID_LEAVE_REQ.mode = mode2;
			gS_MYTHRAID_LEAVE_REQ.nLeavePersonID = charPersonInfo2.GetPersonID();
			gS_MYTHRAID_LEAVE_REQ.nMythRaidRoomIndex = SoldierBatch.MYTHRAID_INFO.m_nMythRaidRoomIndex;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_LEAVE_REQ, gS_MYTHRAID_LEAVE_REQ);
			SoldierBatch.MYTHRAID_INFO.Init();
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP;
			SoldierBatch.BABELTOWER_INFO.Init();
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE)
		{
			PlunderStartAndReMatchDlg plunderStartAndReMatchDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_STARTANDREMATCH_DLG) as PlunderStartAndReMatchDlg;
			if (plunderStartAndReMatchDlg != null)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = 0;
				plunderStartAndReMatchDlg.Send_InfiBattleMatch(1);
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			SoldierBatch.DailyDungeonDifficulty = 0;
		}
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
	}

	public void SetTitle(string strTitle)
	{
		this.m_lbTitle.SetText(strTitle);
	}

	private void VisibleCheckSolCombinationUI()
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_MYTHRAID && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_INFIBATTLE && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP && SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON && SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			this.m_ddlSolCombination.SetVisible(false);
			this.m_solCombinationButton.Visible = false;
			this.m_solCompleteCombinationLabel.Visible = false;
			this.m_solCombinationBG.Visible = false;
			this.m_solCombinationBTN.Visible = false;
			this.m_dtRightBG.Visible = false;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			this.m_ddlSolCombination.SetVisible(false);
			this.m_solCombinationButton.Visible = false;
			this.m_solCompleteCombinationLabel.Visible = false;
			this.m_solCombinationBG.Visible = false;
			this.m_solCombinationBTN.Visible = false;
			this.m_dtRightBG.Visible = false;
		}
	}

	private bool IsCostumeSkill(NkSoldierInfo solInfo, int checkSkillUnique)
	{
		if (solInfo == null)
		{
			return false;
		}
		int num = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		if (num <= 0)
		{
			return false;
		}
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(num);
		return costumeData != null && costumeData.m_SkillUnique == checkSkillUnique;
	}
}
