using System;
using UnityForms;

public class CQuest
{
	private QUEST_COMMON m_QuestCommon;

	private CQuestCondition[] m_QuestCondition = new CQuestCondition[3];

	public bool IsAutoMoveQuest()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCommon.cQuestCondition[i].i32QuestCode != 0)
			{
				if (this.m_QuestCommon.cQuestCondition[i].i32QuestCode != 133)
				{
					if (0 < this.m_QuestCommon.cQuestCondition[i].nMapUnique)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public string GetQuestUnique()
	{
		return this.m_QuestCommon.strQuestUnique;
	}

	public int GetQuestGroupUnique()
	{
		return this.m_QuestCommon.nQuestGroupUnique;
	}

	public bool IsDayQuest()
	{
		CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(this.m_QuestCommon.nQuestGroupUnique);
		return questGroupByGroupUnique != null && questGroupByGroupUnique.GetQuestType() == 100;
	}

	public void SetQuestInfo(QUEST_COMMON stQuestCommon)
	{
		this.m_QuestCommon = stQuestCommon;
		for (int i = 0; i < 3; i++)
		{
			this.m_QuestCondition[i] = null;
			int i32QuestCode = this.m_QuestCommon.cQuestCondition[i].i32QuestCode;
			switch (i32QuestCode)
			{
			case 1:
				this.m_QuestCondition[i] = new CAutoBattle();
				goto IL_9B3;
			case 2:
				this.m_QuestCondition[i] = new CAutoMove();
				goto IL_9B3;
			case 3:
				this.m_QuestCondition[i] = new CBackGenToWorld();
				goto IL_9B3;
			case 4:
				this.m_QuestCondition[i] = new CGoGenToWorld();
				goto IL_9B3;
			case 5:
			case 9:
			case 19:
			case 20:
			case 21:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 34:
			case 35:
			case 37:
			case 38:
			case 39:
			case 44:
			case 45:
			case 47:
			case 49:
			case 50:
			case 52:
			case 53:
			case 56:
			case 57:
			case 60:
			case 61:
			case 62:
			case 65:
			case 66:
			case 67:
			case 69:
			case 70:
			case 71:
			case 72:
			case 73:
			case 74:
			case 75:
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 81:
			case 82:
			case 83:
			case 84:
			case 86:
			case 87:
			case 88:
			case 92:
			case 93:
			case 94:
			case 97:
			case 110:
			case 111:
			case 116:
			case 117:
			case 121:
			case 124:
			case 129:
			case 130:
			case 131:
			case 132:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 141:
			case 142:
			case 143:
			case 145:
			case 146:
			case 147:
			case 148:
			case 151:
			case 153:
			case 154:
			case 156:
			case 157:
			case 162:
				IL_2CA:
				if (i32QuestCode != 999)
				{
					this.m_QuestCondition[i] = null;
					goto IL_9B3;
				}
				this.m_QuestCondition[i] = new CTriggerCheck();
				goto IL_9B3;
			case 6:
				this.m_QuestCondition[i] = new CGoSolToWorld();
				goto IL_9B3;
			case 7:
				this.m_QuestCondition[i] = new CGetItem();
				goto IL_9B3;
			case 8:
				this.m_QuestCondition[i] = new CBringItem();
				((CBringItem)this.m_QuestCondition[i]).SetItemInfo((int)stQuestCommon.cQuestCondition[1].i64Param, (int)stQuestCommon.cQuestCondition[1].i64ParamVal);
				goto IL_9B3;
			case 10:
				this.m_QuestCondition[i] = new CBuyMarket();
				goto IL_9B3;
			case 11:
				this.m_QuestCondition[i] = new CRepairItem();
				goto IL_9B3;
			case 12:
				this.m_QuestCondition[i] = new CBreakItem();
				goto IL_9B3;
			case 13:
				this.m_QuestCondition[i] = new CFailWar();
				goto IL_9B3;
			case 14:
				this.m_QuestCondition[i] = new CVictoryWar();
				goto IL_9B3;
			case 15:
				this.m_QuestCondition[i] = new CGetMoney();
				goto IL_9B3;
			case 16:
				this.m_QuestCondition[i] = new CCreateItem();
				goto IL_9B3;
			case 17:
				this.m_QuestCondition[i] = new CSignupItem();
				goto IL_9B3;
			case 18:
				this.m_QuestCondition[i] = new CEquipItem();
				goto IL_9B3;
			case 22:
				this.m_QuestCondition[i] = new CGoPlace();
				goto IL_9B3;
			case 29:
				this.m_QuestCondition[i] = new CGoDungeon();
				goto IL_9B3;
			case 30:
				this.m_QuestCondition[i] = new CGoNpc();
				goto IL_9B3;
			case 31:
				this.m_QuestCondition[i] = new COpenMilitary();
				goto IL_9B3;
			case 32:
				this.m_QuestCondition[i] = new CMilitaryFormation();
				goto IL_9B3;
			case 33:
				this.m_QuestCondition[i] = new CPillage();
				goto IL_9B3;
			case 36:
				this.m_QuestCondition[i] = new CKillMonster();
				goto IL_9B3;
			case 40:
				this.m_QuestCondition[i] = new CGoDungeon();
				goto IL_9B3;
			case 41:
				this.m_QuestCondition[i] = new CEquipment();
				goto IL_9B3;
			case 42:
				this.m_QuestCondition[i] = new CEquipSol();
				goto IL_9B3;
			case 43:
				this.m_QuestCondition[i] = new CCollectItem();
				goto IL_9B3;
			case 46:
				this.m_QuestCondition[i] = new CMakeVolunteer();
				goto IL_9B3;
			case 48:
				this.m_QuestCondition[i] = new CGiveitem();
				((CGiveitem)this.m_QuestCondition[i]).SetResultNpcID((int)this.m_QuestCommon.i64EndTypeVal);
				goto IL_9B3;
			case 51:
				this.m_QuestCondition[i] = new CLearnBattleSkill();
				goto IL_9B3;
			case 54:
				this.m_QuestCondition[i] = new CGetLetter();
				goto IL_9B3;
			case 55:
				this.m_QuestCondition[i] = new CSendLetter();
				goto IL_9B3;
			case 58:
				this.m_QuestCondition[i] = new CCombatPower();
				goto IL_9B3;
			case 59:
				this.m_QuestCondition[i] = new CGenItemGrade();
				goto IL_9B3;
			case 63:
				this.m_QuestCondition[i] = new COpenIventory();
				goto IL_9B3;
			case 64:
				this.m_QuestCondition[i] = new COpenMap();
				goto IL_9B3;
			case 68:
				this.m_QuestCondition[i] = new CUseItem();
				goto IL_9B3;
			case 85:
				this.m_QuestCondition[i] = new CUseMagic();
				goto IL_9B3;
			case 89:
				this.m_QuestCondition[i] = new CLevelSol();
				goto IL_9B3;
			case 90:
				this.m_QuestCondition[i] = new COpenTransMap();
				goto IL_9B3;
			case 91:
				this.m_QuestCondition[i] = new COpenSoldier();
				goto IL_9B3;
			case 95:
				this.m_QuestCondition[i] = new CLevelCharacter();
				goto IL_9B3;
			case 96:
				this.m_QuestCondition[i] = new CFollowChar();
				goto IL_9B3;
			case 98:
				this.m_QuestCondition[i] = new COpenMake();
				goto IL_9B3;
			case 99:
				this.m_QuestCondition[i] = new CAcceptBringChar();
				((CAcceptBringChar)this.m_QuestCondition[i]).SetSuChar((int)this.m_QuestCommon.cQuestCondition[1].i64Param);
				goto IL_9B3;
			case 100:
				this.m_QuestCondition[i] = new CCUpGradeItem();
				goto IL_9B3;
			case 101:
				this.m_QuestCondition[i] = new CTrainningMonster();
				goto IL_9B3;
			case 102:
				this.m_QuestCondition[i] = new COpenEquipWindow();
				goto IL_9B3;
			case 103:
				this.m_QuestCondition[i] = new CGuardChar();
				goto IL_9B3;
			case 104:
				this.m_QuestCondition[i] = new CAmassItem();
				goto IL_9B3;
			case 105:
				this.m_QuestCondition[i] = new CHuntChar();
				goto IL_9B3;
			case 106:
				this.m_QuestCondition[i] = new CEquipThisItem();
				goto IL_9B3;
			case 107:
				this.m_QuestCondition[i] = new CQuestMake();
				goto IL_9B3;
			case 108:
				this.m_QuestCondition[i] = new CGatherItem();
				goto IL_9B3;
			case 109:
				this.m_QuestCondition[i] = new CCheckDefense();
				goto IL_9B3;
			case 112:
				this.m_QuestCondition[i] = new COpenCollect();
				goto IL_9B3;
			case 113:
				this.m_QuestCondition[i] = new CClearBabel();
				goto IL_9B3;
			case 114:
				this.m_QuestCondition[i] = new CSampoomHelp();
				goto IL_9B3;
			case 115:
				this.m_QuestCondition[i] = new CSellItem();
				goto IL_9B3;
			case 118:
				this.m_QuestCondition[i] = new CPassTurn();
				goto IL_9B3;
			case 119:
				this.m_QuestCondition[i] = new CAutoBattleOn();
				goto IL_9B3;
			case 120:
				this.m_QuestCondition[i] = new CAutoBattleOff();
				goto IL_9B3;
			case 122:
				this.m_QuestCondition[i] = new CTakeChar();
				goto IL_9B3;
			case 123:
				this.m_QuestCondition[i] = new CJoinColosseum();
				goto IL_9B3;
			case 125:
				this.m_QuestCondition[i] = new CVoctoryBattleMatch();
				goto IL_9B3;
			case 126:
				this.m_QuestCondition[i] = new CBattleMatchList();
				goto IL_9B3;
			case 127:
				this.m_QuestCondition[i] = new CWatchBattleMatch();
				goto IL_9B3;
			case 128:
				this.m_QuestCondition[i] = new CExpedition();
				goto IL_9B3;
			case 133:
				this.m_QuestCondition[i] = new CMySoldier();
				goto IL_9B3;
			case 140:
				this.m_QuestCondition[i] = new COpenQuestList();
				goto IL_9B3;
			case 144:
				this.m_QuestCondition[i] = new CMakeSkillCon();
				goto IL_9B3;
			case 149:
				this.m_QuestCondition[i] = new CGoGenToNum();
				goto IL_9B3;
			case 150:
				this.m_QuestCondition[i] = new CSolItemGradeCount();
				((CSolItemGradeCount)this.m_QuestCondition[i]).I64MaxCount = this.m_QuestCommon.cQuestCondition[1].i64ParamVal;
				goto IL_9B3;
			case 152:
				this.m_QuestCondition[i] = new CGoSolToWorld2();
				goto IL_9B3;
			case 155:
				this.m_QuestCondition[i] = new CWinBattle();
				((CWinBattle)this.m_QuestCondition[i]).m_nCharKind = (int)stQuestCommon.cQuestCondition[1].i64Param;
				goto IL_9B3;
			case 158:
				this.m_QuestCondition[i] = new CExpandInventory();
				goto IL_9B3;
			case 159:
				this.m_QuestCondition[i] = new CAmassItemUse();
				((CAmassItemUse)this.m_QuestCondition[i]).SetItemInfo((int)stQuestCommon.cQuestCondition[1].i64Param, (int)stQuestCommon.cQuestCondition[1].i64ParamVal);
				goto IL_9B3;
			case 160:
				this.m_QuestCondition[i] = new CCareAnimal();
				goto IL_9B3;
			case 161:
				this.m_QuestCondition[i] = new CUseItem();
				goto IL_9B3;
			case 163:
				this.m_QuestCondition[i] = new CStorageFood();
				goto IL_9B3;
			case 164:
				this.m_QuestCondition[i] = new CSuccessRepute();
				goto IL_9B3;
			case 165:
				this.m_QuestCondition[i] = new CNormalWinBattle();
				goto IL_9B3;
			case 166:
				this.m_QuestCondition[i] = new CContinueBattle();
				goto IL_9B3;
			}
			goto IL_2CA;
			IL_9B3:
			if (this.m_QuestCondition[i] != null)
			{
				this.m_QuestCondition[i].SetConditionInfo(this.m_QuestCommon.cQuestCondition[i].i64Param, this.m_QuestCommon.cQuestCondition[i].i64ParamVal, this.m_QuestCommon.cQuestCondition[i].szCodeTextKey);
			}
		}
	}

	public QUEST_COMMON GetQuestCommon()
	{
		return this.m_QuestCommon;
	}

	public bool PreCheckQuestAccept()
	{
		return true;
	}

	public bool CheckQuestResult(USER_CURRENT_QUEST_INFO cUserCurrentQuestInfo)
	{
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(cUserCurrentQuestInfo.strQuestUnique);
		if (questByQuestUnique == null)
		{
			return false;
		}
		bool result = true;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCondition[i] != null && !this.m_QuestCondition[i].CheckCondition(questByQuestUnique.GetQuestCommon().cQuestCondition[i].i64Param, ref cUserCurrentQuestInfo.i64ParamVal[i]))
			{
				result = false;
			}
		}
		return result;
	}

	public bool IsCondition(QUEST_CONST.eQUESTCODE QuestCode)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCommon.cQuestCondition[i].i32QuestCode == (int)QuestCode)
			{
				return true;
			}
		}
		return false;
	}

	public string GetConditionText(long i64ParamVal, int bConditionNum)
	{
		if (this.m_QuestCondition[bConditionNum] == null)
		{
			return string.Empty;
		}
		return this.m_QuestCondition[bConditionNum].GetConditionText(i64ParamVal);
	}

	public bool CheckCondition(long i64Param, ref long i64ParamVal, int bConditionNum)
	{
		return this.m_QuestCondition[bConditionNum] != null && this.m_QuestCondition[bConditionNum].CheckCondition(i64Param, ref i64ParamVal);
	}

	public string GetQuestTitle()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(this.m_QuestCommon.strTextKey);
	}

	public string GetGiveQuestNpcName()
	{
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(this.m_QuestCommon.GiveQuestCharCode);
		if (charKindInfoFromCode == null)
		{
			return "NoName";
		}
		return charKindInfoFromCode.GetName();
	}

	public string GetQuestNpcName()
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_QuestCommon.i32QuestCharKind);
		if (charKindInfo == null)
		{
			return string.Empty;
		}
		return charKindInfo.GetName();
	}

	public NrCharKindInfo GetQuestNpc()
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_QuestCommon.i32QuestCharKind);
		if (charKindInfo == null)
		{
			return null;
		}
		return charKindInfo;
	}

	public string GetQuestSummary()
	{
		string strDlgID = this.GetQuestUnique().ToString() + "a";
		int num = 1;
		QUEST_DLG_INFO questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
		string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
		string charName = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
		NrTSingleton<UIDataManager>.Instance.InitStringBuilder();
		NrTSingleton<UIDataManager>.Instance.AppendString("{&20}");
		NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1637"));
		NrTSingleton<UIDataManager>.Instance.AppendString("{&15}");
		NrTSingleton<UIDataManager>.Instance.AppendString("\n");
		while (questDlgInfo != null)
		{
			string strLang_Idx = questDlgInfo.strLang_Idx;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				strLang_Idx
			});
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questDlgInfo.QuestDlgCharCode);
			if (charKindInfoFromCode == null)
			{
				num++;
				questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
			}
			else
			{
				if (questDlgInfo.bTalkUser)
				{
					NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<CTextParser>.Instance.GetTextColor("1104"));
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charName);
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(empty);
				}
				else
				{
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor);
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charKindInfoFromCode.GetName());
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(empty);
				}
				if (string.Empty != questDlgInfo.strUserAnswer)
				{
					NrTSingleton<UIDataManager>.Instance.AppendString("\n");
					NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<CTextParser>.Instance.GetTextColor("1104"));
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charName);
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(questDlgInfo.strUserAnswer);
				}
				NrTSingleton<UIDataManager>.Instance.AppendString("\n");
				num++;
				questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
			}
		}
		string @string = NrTSingleton<UIDataManager>.Instance.GetString();
		if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(this.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
		{
			return @string;
		}
		if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(this.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
		{
			return @string;
		}
		NrTSingleton<UIDataManager>.Instance.AppendString("\n\n");
		NrTSingleton<UIDataManager>.Instance.AppendString("{&20}");
		NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1638"));
		NrTSingleton<UIDataManager>.Instance.AppendString("{&15}");
		NrTSingleton<UIDataManager>.Instance.AppendString("\n");
		strDlgID = this.GetQuestUnique() + "p";
		num = 1;
		questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
		while (questDlgInfo != null)
		{
			string strLang_Idx2 = questDlgInfo.strLang_Idx;
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				strLang_Idx2
			});
			NrCharKindInfo charKindInfoFromCode2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questDlgInfo.QuestDlgCharCode);
			if (charKindInfoFromCode2 == null)
			{
				num++;
				questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
			}
			else
			{
				if (questDlgInfo.bTalkUser)
				{
					NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<CTextParser>.Instance.GetTextColor("1104"));
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charName);
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(empty2);
				}
				else
				{
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor);
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charKindInfoFromCode2.GetName());
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(empty2);
				}
				if (string.Empty != questDlgInfo.strUserAnswer)
				{
					NrTSingleton<UIDataManager>.Instance.AppendString("\n");
					NrTSingleton<UIDataManager>.Instance.AppendString(NrTSingleton<CTextParser>.Instance.GetTextColor("1104"));
					NrTSingleton<UIDataManager>.Instance.AppendString("[");
					NrTSingleton<UIDataManager>.Instance.AppendString(charName);
					NrTSingleton<UIDataManager>.Instance.AppendString("] ");
					NrTSingleton<UIDataManager>.Instance.AppendString(textColor2);
					NrTSingleton<UIDataManager>.Instance.AppendString(questDlgInfo.strUserAnswer);
				}
				NrTSingleton<UIDataManager>.Instance.AppendString("\n");
				num++;
				questDlgInfo = NrTSingleton<NkQuestManager>.Instance.GetQuestDlgInfo(strDlgID, num);
			}
		}
		return NrTSingleton<UIDataManager>.Instance.GetString();
	}

	public string GetQuestHint()
	{
		return string.Empty;
	}

	public short GetQuestLevel(int bGrade)
	{
		if (0 > bGrade || 5 < bGrade)
		{
			bGrade = 0;
		}
		return this.m_QuestCommon.i16RequireLevel[bGrade];
	}

	public bool IsServerCheck(int bNum)
	{
		return bNum < 0 || bNum >= 3 || this.m_QuestCondition[bNum] == null || this.m_QuestCondition[bNum].IsServerCheck();
	}

	public void AfterAccept()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCondition[i] != null)
			{
				this.m_QuestCondition[i].AfterAccept();
			}
		}
	}

	public void AfterOnGoing()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCondition[i] != null)
			{
				this.m_QuestCondition[i].AfterOnGoing();
			}
		}
	}

	public void AfterAutoPath()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_QuestCondition[i] != null)
			{
				this.m_QuestCondition[i].AfterAutoPath();
			}
		}
	}

	public void SetToolTipMsg(int Lang_Type_Tooltip, string Lang_Idx_Tooltip)
	{
	}

	public string GetToolTipMsg()
	{
		return string.Empty;
	}
}
