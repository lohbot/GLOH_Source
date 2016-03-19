using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class PlunderSolListDlg : Form
{
	private const int SHOW_COMBATPOWER_LEVEL = 50;

	private NewListBox m_nlSolList;

	private Label m_bSolNum;

	private Label m_bSolNum1;

	private Label m_lbBatchMode;

	private CheckBox m_cMode;

	private Button m_bCheck;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolList_Power = new List<NkSoldierInfo>();

	private List<NkSoldierInfo> m_kSolList_Level = new List<NkSoldierInfo>();

	private List<USER_FRIEND_INFO> m_kFriendSolList = new List<USER_FRIEND_INFO>();

	private NkSoldierInfo mSelectedSolinfo;

	public bool m_bMyBatchMode = true;

	private int m_nReadySolCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_sollist", G_ID.PLUNDERSOLLIST_DLG, false, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		float height = GUICamera.height;
		base.SetSize(base.GetSize().x, height);
		this.m_nlSolList = (base.GetControl("listbox_pvp_sollist") as NewListBox);
		this.m_nlSolList.viewableArea = new Vector2(this.m_nlSolList.GetSize().x, height - 112f);
		this.m_nlSolList.SetLocation(6, 112);
		this.m_bSolNum = (base.GetControl("Label_solnum") as Label);
		this.m_bSolNum1 = (base.GetControl("Label_solnum01") as Label);
		this.m_cMode = (base.GetControl("CheckBox_CheckBox8") as CheckBox);
		this.m_bCheck = (base.GetControl("Button_FriendSol") as Button);
		this.m_bCheck.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickChangeMode));
		this.m_bCheck.EffectAni = false;
		this.m_lbBatchMode = (base.GetControl("Label_Label6") as Label);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			this.m_lbBatchMode.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1348"));
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			this.SetSolList();
		}
		else
		{
			this.SetSolListPVPMakeUp();
		}
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), base.GetLocation().z);
	}

	public override void InitData()
	{
	}

	public override void Update()
	{
	}

	public void SetSolNum(int nSolNum, bool bObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			if (bObject && this.m_bMyBatchMode)
			{
				return;
			}
			if (!bObject && !this.m_bMyBatchMode)
			{
				return;
			}
		}
		int level = kMyCharInfo.GetLevel();
		int a = level / 3;
		int num = Mathf.Min(a, 5);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			num = 6;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			num = 3;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			if (SoldierBatch.BABELTOWER_INFO.Count <= 0)
			{
				SoldierBatch.BABELTOWER_INFO.Count = 1;
			}
			byte count = SoldierBatch.BABELTOWER_INFO.Count;
			if (count == 1)
			{
				base.ShowLayer(2);
				if (this.m_bMyBatchMode)
				{
					num = 9;
					nSolNum -= SoldierBatch.SOLDIERBATCH.GetBabelTowerFriendSolCount();
				}
				else
				{
					num = 3;
					nSolNum = SoldierBatch.SOLDIERBATCH.GetBabelTowerFriendSolCount();
				}
			}
			else
			{
				base.ShowLayer(1);
				num = (int)(12 / count + 1);
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			base.ShowLayer(2);
			if (!this.m_bMyBatchMode)
			{
				num = 3;
			}
			else
			{
				num *= 3;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
		{
			num = 5;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			num = 9;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
			if (expeditionDataFromGrade != null)
			{
				num = expeditionDataFromGrade.Expedition_SolBatch_Array * 3;
			}
			else
			{
				num = 15;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			num = 5;
		}
		else
		{
			num *= 3;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("73");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"num1",
			nSolNum.ToString(),
			"num2",
			num.ToString()
		});
		this.m_bSolNum.Text = empty;
		this.m_bSolNum1.Text = empty;
	}

	public void SetHelpFriendSolList()
	{
		byte count = SoldierBatch.BABELTOWER_INFO.Count;
		if (count != 1)
		{
			return;
		}
		this.m_nlSolList.Clear();
		this.m_kFriendSolList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID > 0L)
			{
				if (uSER_FRIEND_INFO.ui8HelpUse < 1)
				{
					this.m_kFriendSolList.Add(uSER_FRIEND_INFO);
				}
			}
		}
		this.m_kFriendSolList.Sort(new Comparison<USER_FRIEND_INFO>(this.CompareFriendSolLevel));
		int i = 0;
		while (i < this.m_kFriendSolList.Count)
		{
			USER_FRIEND_INFO uSER_FRIEND_INFO2 = this.m_kFriendSolList[i];
			NkListSolInfo nkListSolInfo = new NkListSolInfo();
			nkListSolInfo.SolCharKind = uSER_FRIEND_INFO2.FriendHelpSolInfo.i32SolKind;
			nkListSolInfo.SolGrade = (int)uSER_FRIEND_INFO2.FriendHelpSolInfo.bySolGrade;
			nkListSolInfo.SolLevel = uSER_FRIEND_INFO2.FriendHelpSolInfo.iSolLevel;
			nkListSolInfo.FightPower = (long)uSER_FRIEND_INFO2.FriendHelpSolInfo.i32SolFightPower;
			nkListSolInfo.ShowLevel = false;
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
			{
				goto IL_1A3;
			}
			if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(uSER_FRIEND_INFO2.FriendHelpSolInfo.i32SolKind) != null)
			{
				if (uSER_FRIEND_INFO2.FriendHelpSolInfo.iSolLevel >= 50)
				{
					nkListSolInfo.ShowCombat = true;
					nkListSolInfo.ShowLevel = false;
					goto IL_1A3;
				}
				nkListSolInfo.ShowCombat = false;
				nkListSolInfo.ShowLevel = true;
				goto IL_1A3;
			}
			IL_36B:
			i++;
			continue;
			IL_1A3:
			NewListItem newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
			newListItem.Data = uSER_FRIEND_INFO2.FriendHelpSolInfo.i64HelpSolID;
			newListItem.SetListItemData(6, false);
			newListItem.SetListItemData(7, false);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkListSolInfo.SolCharKind);
			if (charKindInfo != null)
			{
				EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(charKindInfo.GetCharKind());
				if (eventHeroCharFriendCode != null)
				{
					newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
					newListItem.SetListItemData(6, true);
				}
				else
				{
					UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo.SolCharKind, nkListSolInfo.SolGrade);
					if (legendFrame != null)
					{
						newListItem.SetListItemData(0, legendFrame, null, null, null);
					}
				}
			}
			newListItem.SetListItemData(1, nkListSolInfo, uSER_FRIEND_INFO2, null, new EZValueChangedDelegate(this.BtClickFriendListBox));
			newListItem.SetListItemData(2, string.Empty, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
			newListItem.SetListItemData(4, string.Empty, nkListSolInfo, null, null);
			newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
			bool flag = false;
			if (SoldierBatch.SOLDIERBATCH != null && SoldierBatch.SOLDIERBATCH.GetPositionBabel_Tower(uSER_FRIEND_INFO2.FriendHelpSolInfo.i64HelpSolID) > 0)
			{
				flag = true;
			}
			if (flag)
			{
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, true);
				newListItem.SetListItemData(3, true);
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(5, false);
			}
			else
			{
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, false);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(5, false);
			}
			this.m_nlSolList.Add(newListItem);
			goto IL_36B;
		}
		this.m_nlSolList.RepositionItems();
	}

	public void SetSolList()
	{
		base.ShowLayer(1);
		this.m_nlSolList.Clear();
		this.m_kSolList.Clear();
		this.m_kSolList_Power.Clear();
		this.m_kSolList_Level.Clear();
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
			base.ShowLayer(2);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			byte count = SoldierBatch.BABELTOWER_INFO.Count;
			if (count == 1)
			{
				base.ShowLayer(2);
			}
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		if (soldierList == null)
		{
			return;
		}
		NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
		for (int i = 0; i < kSolInfo.Length; i++)
		{
			NkSoldierInfo nkSoldierInfo = kSolInfo[i];
			if (nkSoldierInfo.GetSolID() > 0L)
			{
				if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
				{
					if (nkSoldierInfo.GetSolPosType() == 2)
					{
						goto IL_24F;
					}
					if (nkSoldierInfo.GetSolPosType() == 6)
					{
						goto IL_24F;
					}
				}
				if (nkSoldierInfo.GetSolPosType() != 7)
				{
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
					{
						break;
					}
					if (nkSoldierInfo.IsLeader())
					{
						this.m_kSolList.Add(nkSoldierInfo);
					}
					else
					{
						NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
						if (charKindInfo != null)
						{
							if (nkSoldierInfo.GetLevel() >= 50)
							{
								this.m_kSolList_Power.Add(nkSoldierInfo);
							}
							else
							{
								this.m_kSolList_Level.Add(nkSoldierInfo);
							}
						}
						else
						{
							this.m_kSolList_Level.Add(nkSoldierInfo);
						}
					}
				}
			}
			IL_24F:;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current.GetSolID() > 0L)
			{
				if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
				{
					if (current.GetSolPosType() == 2)
					{
						continue;
					}
					if (current.GetSolPosType() == 6)
					{
						continue;
					}
				}
				if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP)
				{
					int mineMoneyFromSolPossibleLevel = BASE_MINE_DATA.GetMineMoneyFromSolPossibleLevel(SoldierBatch.MINE_INFO.m_nMineGrade);
					if ((int)current.GetLevel() < mineMoneyFromSolPossibleLevel)
					{
						continue;
					}
				}
				if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP || current.GetLevel() >= NrTSingleton<GuildWarManager>.Instance.GuildWarApplySolLevel)
				{
					if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
					{
						EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade((byte)SoldierBatch.EXPEDITION_INFO.m_eExpeditionGrade);
						if (expeditionDataFromGrade != null && current.GetLevel() < expeditionDataFromGrade.SolPossiblelevel)
						{
							continue;
						}
					}
					if (current.IsLeader())
					{
						this.m_kSolList.Add(current);
					}
					else
					{
						NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.GetCharKind());
						if (charKindInfo2 != null)
						{
							if (current.GetLevel() >= 50)
							{
								this.m_kSolList_Power.Add(current);
							}
							else
							{
								this.m_kSolList_Level.Add(current);
							}
						}
						else
						{
							this.m_kSolList_Level.Add(current);
						}
					}
					this.m_nReadySolCount++;
				}
			}
		}
		this.m_kSolList_Power.Sort(new Comparison<NkSoldierInfo>(this.ComparePower));
		this.m_kSolList_Level.Sort(new Comparison<NkSoldierInfo>(this.CompareLevel));
		for (int j = 0; j < this.m_kSolList_Power.Count; j++)
		{
			this.m_kSolList.Add(this.m_kSolList_Power[j]);
		}
		for (int k = 0; k < this.m_kSolList_Level.Count; k++)
		{
			this.m_kSolList.Add(this.m_kSolList_Level[k]);
		}
		int l = 0;
		while (l < this.m_kSolList.Count)
		{
			NewListItem newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
			newListItem.Data = this.m_kSolList[l].GetSolID();
			newListItem.SetListItemData(6, false);
			newListItem.SetListItemData(7, false);
			EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_kSolList[l].GetCharKind(), this.m_kSolList[l].GetGrade());
			if (eventHeroCharCode != null)
			{
				newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
				newListItem.SetListItemData(6, true);
			}
			else
			{
				UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(this.m_kSolList[l].GetCharKind(), (int)this.m_kSolList[l].GetGrade());
				if (legendFrame != null)
				{
					newListItem.SetListItemData(0, legendFrame, null, null, null);
				}
			}
			Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(this.m_kSolList[l].GetCharKind());
			if (portraitLeaderSol != null)
			{
				newListItem.SetListItemData(1, portraitLeaderSol, this.m_kSolList[l], this.m_kSolList[l].GetLevel(), new EZValueChangedDelegate(this.BtClickUpListBox), new EZValueChangedDelegate(this.BtClickListBox));
				goto IL_728;
			}
			NkListSolInfo nkListSolInfo = new NkListSolInfo();
			nkListSolInfo.SolCharKind = this.m_kSolList[l].GetCharKind();
			nkListSolInfo.SolGrade = (int)this.m_kSolList[l].GetGrade();
			nkListSolInfo.SolLevel = this.m_kSolList[l].GetLevel();
			nkListSolInfo.FightPower = this.m_kSolList[l].GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
			nkListSolInfo.ShowLevel = false;
			if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
			{
				if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_kSolList[l].GetCharKind()) == null)
				{
					goto IL_8FD;
				}
				if (this.m_kSolList[l].GetLevel() >= 50)
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
			newListItem.SetListItemData(1, nkListSolInfo, this.m_kSolList[l], new EZValueChangedDelegate(this.BtClickUpListBox), new EZValueChangedDelegate(this.BtClickListBox));
			goto IL_728;
			IL_8FD:
			l++;
			continue;
			IL_728:
			newListItem.SetListItemData(2, string.Empty, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
			newListItem.SetListItemData(4, string.Empty, this.m_kSolList[l], null, null);
			newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
			bool flag = false;
			if (eSOL_SUBDATA != eSOL_SUBDATA.SOL_SUBDATA_NONE)
			{
				if (this.m_kSolList[l].GetSolSubData(eSOL_SUBDATA) > 0L)
				{
					flag = true;
				}
			}
			else if (SoldierBatch.SOLDIERBATCH != null)
			{
				if (SoldierBatch.SOLDIERBATCH.GetPositionBabel_Tower(this.m_kSolList[l].GetSolID()) > 0)
				{
					flag = true;
				}
				if ((SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP) && SoldierBatch.SOLDIERBATCH.GetTempBattlePos(this.m_kSolList[l].GetSolID()) > 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, true);
				newListItem.SetListItemData(3, true);
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(5, false);
			}
			else if (this.m_kSolList[l].IsInjuryStatus())
			{
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, false);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(4, true);
				newListItem.SetListItemData(5, true);
				if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
				{
					newListItem.SetListItemData(4, false);
				}
			}
			else
			{
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, false);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(5, false);
			}
			this.m_nlSolList.Add(newListItem);
			goto IL_8FD;
		}
		this.m_nlSolList.RepositionItems();
	}

	public NewListItem SetSolListFromSolID(long nSolID)
	{
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		NewListItem newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
		if (this.m_bMyBatchMode)
		{
			for (int i = 0; i < this.m_kSolList.Count; i++)
			{
				if (this.m_kSolList[i].GetSolID() == nSolID)
				{
					newListItem.SetListItemData(6, false);
					newListItem.SetListItemData(7, false);
					EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_kSolList[i].GetCharKind(), this.m_kSolList[i].GetGrade());
					if (eventHeroCharCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.SetListItemData(6, true);
					}
					else
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(this.m_kSolList[i].GetCharKind(), (int)this.m_kSolList[i].GetGrade());
						if (legendFrame != null)
						{
							newListItem.SetListItemData(0, legendFrame, null, null, null);
						}
					}
					newListItem.Data = nSolID;
					Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(this.m_kSolList[i].GetCharKind());
					if (portraitLeaderSol != null)
					{
						newListItem.SetListItemData(1, portraitLeaderSol, this.m_kSolList[i], this.m_kSolList[i].GetLevel(), new EZValueChangedDelegate(this.BtClickUpListBox), new EZValueChangedDelegate(this.BtClickListBox));
					}
					else
					{
						NkListSolInfo nkListSolInfo = new NkListSolInfo();
						nkListSolInfo.SolCharKind = this.m_kSolList[i].GetCharKind();
						nkListSolInfo.SolGrade = (int)this.m_kSolList[i].GetGrade();
						nkListSolInfo.SolLevel = this.m_kSolList[i].GetLevel();
						nkListSolInfo.FightPower = this.m_kSolList[i].GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
						nkListSolInfo.ShowLevel = false;
						if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
						{
							if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_kSolList[i].GetCharKind()) == null)
							{
								goto IL_4F7;
							}
							if (this.m_kSolList[i].GetLevel() >= 50)
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
						newListItem.SetListItemData(1, nkListSolInfo, this.m_kSolList[i], new EZValueChangedDelegate(this.BtClickUpListBox), new EZValueChangedDelegate(this.BtClickListBox));
					}
					newListItem.SetListItemData(2, string.Empty, null, null, null);
					newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
					newListItem.SetListItemData(4, string.Empty, this.m_kSolList[i], null, null);
					newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
					bool flag = false;
					if (eSOL_SUBDATA != eSOL_SUBDATA.SOL_SUBDATA_NONE)
					{
						if (this.m_kSolList[i].GetSolSubData(eSOL_SUBDATA) > 0L)
						{
							flag = true;
						}
					}
					else if (SoldierBatch.SOLDIERBATCH != null)
					{
						if (SoldierBatch.SOLDIERBATCH.GetPositionBabel_Tower(this.m_kSolList[i].GetSolID()) > 0)
						{
							flag = true;
						}
						if ((SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP) && SoldierBatch.SOLDIERBATCH.GetTempBattlePos(this.m_kSolList[i].GetSolID()) > 0)
						{
							flag = true;
						}
					}
					if (flag)
					{
						newListItem.SetListItemData(1, true);
						newListItem.SetListItemData(2, true);
						newListItem.SetListItemData(3, true);
						newListItem.SetListItemData(4, false);
						newListItem.SetListItemData(5, false);
					}
					else if (this.m_kSolList[i].IsInjuryStatus())
					{
						newListItem.SetListItemData(1, true);
						newListItem.SetListItemData(2, false);
						newListItem.SetListItemData(3, false);
						newListItem.SetListItemData(4, true);
						newListItem.SetListItemData(5, true);
						if (eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
						{
							newListItem.SetListItemData(4, false);
						}
					}
					else
					{
						newListItem.SetListItemData(1, true);
						newListItem.SetListItemData(2, false);
						newListItem.SetListItemData(3, false);
						newListItem.SetListItemData(4, false);
						newListItem.SetListItemData(5, false);
					}
					break;
				}
				IL_4F7:;
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			for (int j = 0; j < this.m_kFriendSolList.Count; j++)
			{
				USER_FRIEND_INFO uSER_FRIEND_INFO = this.m_kFriendSolList[j];
				if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolKind) != null)
				{
					NkListSolInfo nkListSolInfo2 = new NkListSolInfo();
					nkListSolInfo2.SolCharKind = uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolKind;
					nkListSolInfo2.SolGrade = (int)uSER_FRIEND_INFO.FriendHelpSolInfo.bySolGrade;
					nkListSolInfo2.SolLevel = uSER_FRIEND_INFO.FriendHelpSolInfo.iSolLevel;
					nkListSolInfo2.FightPower = (long)uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolFightPower;
					nkListSolInfo2.ShowLevel = true;
					if (uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID == nSolID)
					{
						newListItem.Data = uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID;
						newListItem.SetListItemData(6, false);
						newListItem.SetListItemData(1, nkListSolInfo2, uSER_FRIEND_INFO, null, new EZValueChangedDelegate(this.BtClickFriendListBox));
						newListItem.SetListItemData(2, string.Empty, null, null, null);
						newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
						newListItem.SetListItemData(4, string.Empty, nkListSolInfo2, null, null);
						newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
						bool flag2 = false;
						if (SoldierBatch.SOLDIERBATCH != null && SoldierBatch.SOLDIERBATCH.GetPositionBabel_Tower(uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID) > 0)
						{
							flag2 = true;
						}
						if (flag2)
						{
							newListItem.SetListItemData(1, true);
							newListItem.SetListItemData(2, true);
							newListItem.SetListItemData(3, true);
							newListItem.SetListItemData(4, false);
							newListItem.SetListItemData(5, false);
						}
						else
						{
							newListItem.SetListItemData(1, true);
							newListItem.SetListItemData(2, false);
							newListItem.SetListItemData(3, false);
							newListItem.SetListItemData(4, false);
							newListItem.SetListItemData(5, false);
						}
						break;
					}
				}
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
		}
		return newListItem;
	}

	public void UpdateSolInjury(long nSolID)
	{
		int num = this.ListBox_SolListIndex(nSolID);
		if (num < 0)
		{
			return;
		}
		NewListItem newListItem = this.SetSolListFromSolID(nSolID);
		if (newListItem != null)
		{
			this.m_nlSolList.UpdateAdd(num, newListItem);
		}
		this.m_nlSolList.RepositionItems();
		BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
		if (babelLobbyUserListDlg != null)
		{
		}
	}

	public void UpdateSolList(long nSolID)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE != eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			int num = this.ListBox_SolListIndex(nSolID);
			if (num < 0)
			{
				return;
			}
			NewListItem newListItem = this.SetSolListFromSolID(nSolID);
			if (newListItem != null)
			{
				this.m_nlSolList.UpdateAdd(num, newListItem);
			}
		}
		else
		{
			this.UpdatePVP2SoliderList((int)nSolID);
		}
		this.m_nlSolList.RepositionItems();
	}

	private int ListBox_SolListIndex(long nSolID)
	{
		if (nSolID <= 0L)
		{
			return -1;
		}
		int count;
		if (this.m_bMyBatchMode)
		{
			count = this.m_kSolList.Count;
		}
		else
		{
			count = this.m_kFriendSolList.Count;
		}
		for (int i = 0; i < count; i++)
		{
			long num;
			if (this.m_bMyBatchMode)
			{
				num = this.m_kSolList[i].GetSolID();
			}
			else
			{
				num = this.m_kFriendSolList[i].FriendHelpSolInfo.i64HelpSolID;
			}
			if (num > 0L && num == nSolID)
			{
				return i;
			}
		}
		return -1;
	}

	private int CompareCombatPowerDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}

	private int ComparePower(NkSoldierInfo a, NkSoldierInfo b)
	{
		long solSubData = a.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
		return b.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER).CompareTo(solSubData);
	}

	private int CompareLevel(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetLevel().CompareTo(a.GetLevel());
	}

	private int CompareFriendSolLevel(USER_FRIEND_INFO a, USER_FRIEND_INFO b)
	{
		return b.FriendHelpSolInfo.iSolLevel.CompareTo(a.FriendHelpSolInfo.iSolLevel);
	}

	private void BtClickFriendListBox(IUIObject obj)
	{
		USER_FRIEND_INFO uSER_FRIEND_INFO = obj.Data as USER_FRIEND_INFO;
		if (uSER_FRIEND_INFO == null)
		{
			return;
		}
		long i64HelpSolID = uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && SoldierBatch.SOLDIERBATCH.SolBatchLock)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("388");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		SoldierBatch.SOLDIERBATCH.MakePlunderCharFromUI(i64HelpSolID, uSER_FRIEND_INFO.nPersonID, uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolKind, (long)uSER_FRIEND_INFO.FriendHelpSolInfo.i32SolFightPower);
	}

	private void BtClickObjectListBox(IUIObject obj)
	{
		PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = obj.Data as PLUNDER_OBJECT_INFO;
		if (pLUNDER_OBJECT_INFO == null)
		{
			return;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && SoldierBatch.SOLDIERBATCH.SolBatchLock)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("388");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		SoldierBatch.SOLDIERBATCH.MakePlunderCharFromUIObject(pLUNDER_OBJECT_INFO);
	}

	private void BtClickUpListBox(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = obj.Data as NkSoldierInfo;
		this.mSelectedSolinfo = nkSoldierInfo;
		if (nkSoldierInfo == null || !nkSoldierInfo.IsValid())
		{
			return;
		}
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		if (!nkSoldierInfo.IsInjuryStatus() || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
		{
			return;
		}
		ITEM firstFunctionItem = NkUserInventory.GetInstance().GetFirstFunctionItem(eITEM_SUPPLY_FUNCTION.SUPPLY_INJURYCURE);
		if (firstFunctionItem != null)
		{
			this.SolCure2(nkSoldierInfo);
			return;
		}
		long injuryCureMoney = nkSoldierInfo.GetInjuryCureMoney();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && kMyCharInfo.m_Money < injuryCureMoney)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		GS_SOLDIER_INJURYCURE_REQ gS_SOLDIER_INJURYCURE_REQ = new GS_SOLDIER_INJURYCURE_REQ();
		gS_SOLDIER_INJURYCURE_REQ.nSolID = nkSoldierInfo.GetSolID();
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

	private void SolCure2(NkSoldierInfo kSolInfo)
	{
		if (kSolInfo == null || !kSolInfo.IsValid())
		{
			return;
		}
		if (!kSolInfo.IsInjuryStatus())
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
		long remainInjuryTime = kSolInfo.GetRemainInjuryTime();
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
			kSolInfo.GetName(),
			"count",
			num4,
			"timestring",
			empty
		});
		msgBoxUI.SetMsg(new YesDelegate(this.SendSolImmediatelyCure2), firstFunctionItem, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("513"), empty2, eMsgType.MB_OK_CANCEL);
	}

	public NkSoldierInfo GetSelectedSolinfo()
	{
		if (this.mSelectedSolinfo == null || !this.mSelectedSolinfo.IsValid())
		{
			return null;
		}
		return this.mSelectedSolinfo;
	}

	private void BtClickListBox(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = obj.Data as NkSoldierInfo;
		if (nkSoldierInfo == null || !nkSoldierInfo.IsValid())
		{
			return;
		}
		long solID = nkSoldierInfo.GetSolID();
		eSOL_SUBDATA eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDWAR_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_NONE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_ATTACK_INFIBATTLE;
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
		{
			eSOL_SUBDATA = eSOL_SUBDATA.SOL_SUBDATA_DEFENSE_INFIBATTLE;
		}
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER && SoldierBatch.SOLDIERBATCH.SolBatchLock)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("388");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (!nkSoldierInfo.IsInjuryStatus() || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_DEFENCE_BATTLEPOS || eSOL_SUBDATA == eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS)
		{
			SoldierBatch.SOLDIERBATCH.MakePlunderCharFromUI(solID, 0L, 0, 0L);
		}
	}

	public void SendSolImmediatelyCure(object obj)
	{
		GS_SOLDIER_INJURYCURE_REQ obj2 = obj as GS_SOLDIER_INJURYCURE_REQ;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_INJURYCURE_REQ, obj2);
	}

	public void SendSolImmediatelyCure2(object obj)
	{
		ITEM pkItem = obj as ITEM;
		Protocol_Item.Item_Use(pkItem);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "TREATMENT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void ClickChangeMode(IUIObject obj)
	{
		if (this.m_bMyBatchMode)
		{
			this.m_bMyBatchMode = false;
			this.m_cMode.SetCheckState(1);
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				this.SetHelpFriendSolList();
				this.SetSolNum(SoldierBatch.SOLDIERBATCH.GetBabelTowerFriendSolCount(), true);
			}
			else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
			{
				this.SetSolListFromObject();
				this.SetSolNum(SoldierBatch.SOLDIERBATCH.GetObjCount(), true);
			}
		}
		else
		{
			this.m_bMyBatchMode = true;
			this.m_cMode.SetCheckState(0);
			this.SetSolList();
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				this.SetSolNum(SoldierBatch.SOLDIERBATCH.GetSolBatchNum(), false);
			}
			else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP)
			{
				this.SetSolNum(SoldierBatch.SOLDIERBATCH.GetSolBatchNum(), false);
			}
		}
	}

	private void SetSolListFromObject()
	{
		this.m_nlSolList.Clear();
		NrTable_PlunderObjectinfo_Manager instance = NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance;
		if (instance == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		SortedDictionary<byte, PLUNDER_OBJECT_INFO> collection = instance.Get_Collection();
		foreach (PLUNDER_OBJECT_INFO current in collection.Values)
		{
			if (current.nNeedLevel <= level)
			{
				NkListSolInfo nkListSolInfo = new NkListSolInfo();
				nkListSolInfo.SolCharKind = current.nObject_Kind;
				nkListSolInfo.SolGrade = 5;
				nkListSolInfo.SolLevel = 0;
				nkListSolInfo.ShowCombat = true;
				nkListSolInfo.ShowLevel = false;
				NewListItem newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
				newListItem.Data = current.nObjectID;
				newListItem.SetListItemData(6, false);
				newListItem.SetListItemData(7, false);
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkListSolInfo.SolCharKind);
				if (charKindInfo != null)
				{
					EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(charKindInfo.GetCharKind());
					if (eventHeroCharFriendCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.SetListItemData(6, true);
					}
					else
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(nkListSolInfo.SolCharKind, nkListSolInfo.SolGrade);
						if (legendFrame != null)
						{
							newListItem.SetListItemData(0, legendFrame, null, null, null);
						}
					}
				}
				Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(charKindInfo.GetCharKind());
				if (portraitLeaderSol != null)
				{
					newListItem.SetListItemData(1, portraitLeaderSol, current, null, new EZValueChangedDelegate(this.BtClickObjectListBox), null);
				}
				else
				{
					newListItem.SetListItemData(1, nkListSolInfo, current, null, new EZValueChangedDelegate(this.BtClickObjectListBox));
				}
				newListItem.SetListItemData(2, string.Empty, null, null, null);
				newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
				newListItem.SetListItemData(4, string.Empty, nkListSolInfo, null, null);
				newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
				newListItem.SetListItemData(1, true);
				newListItem.SetListItemData(2, false);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(4, false);
				newListItem.SetListItemData(5, false);
				this.m_nlSolList.Add(newListItem);
			}
		}
		this.m_nlSolList.RepositionItems();
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

	public void SetSolListPVPMakeUp()
	{
		base.ShowLayer(1);
		this.m_nlSolList.Clear();
		this.m_kSolList.Clear();
		this.m_kSolList_Power.Clear();
		this.m_kSolList_Level.Clear();
		int colosseumEnableBatchSoldierKindCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKindCount();
		if (colosseumEnableBatchSoldierKindCount <= 0)
		{
			return;
		}
		for (int i = 0; i < colosseumEnableBatchSoldierKindCount; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
			int colosseumEnableBatchSoldierKind = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKind(i);
			if (colosseumEnableBatchSoldierKind > 0)
			{
				GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND = new GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND();
				gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind = colosseumEnableBatchSoldierKind;
				newListItem.SetListItemData(6, false);
				EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(colosseumEnableBatchSoldierKind, 0);
				if (eventHeroCharCode != null)
				{
					newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
					newListItem.SetListItemData(6, true);
				}
				newListItem.SetListItemData(1, new NkListSolInfo
				{
					SolCharKind = colosseumEnableBatchSoldierKind,
					SolGrade = 0,
					SolLevel = 1,
					ShowCombat = false,
					ShowLevel = false,
					ShowGrade = false
				}, gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND, null, new EZValueChangedDelegate(this.BtClickListBoxPVPMakeUp));
				newListItem.SetListItemData(2, string.Empty, null, null, null);
				newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
				newListItem.SetListItemData(4, string.Empty, null, null, null);
				newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
				newListItem.SetListItemData(7, string.Empty, gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND, null, new EZValueChangedDelegate(this.BtClickSolInfo));
				bool flag = false;
				if (SoldierBatch.SOLDIERBATCH != null && SoldierBatch.SOLDIERBATCH.GetTempBattlePos((long)colosseumEnableBatchSoldierKind) > 0)
				{
					flag = true;
				}
				if (flag)
				{
					newListItem.SetListItemData(1, true);
					newListItem.SetListItemData(2, true);
					newListItem.SetListItemData(3, true);
					newListItem.SetListItemData(4, false);
					newListItem.SetListItemData(5, false);
				}
				else
				{
					newListItem.SetListItemData(1, true);
					newListItem.SetListItemData(2, false);
					newListItem.SetListItemData(3, false);
					newListItem.SetListItemData(4, false);
					newListItem.SetListItemData(5, false);
				}
				this.m_nlSolList.Add(newListItem);
			}
		}
		this.m_nlSolList.RepositionItems();
	}

	private void BtClickListBoxPVPMakeUp(IUIObject obj)
	{
		GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND = obj.Data as GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND;
		if (gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND == null || gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind <= 0)
		{
			return;
		}
		SoldierBatch.SOLDIERBATCH.MakePVP2CharFromUI(gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind);
		ColosseumCardSettingDlg colosseumCardSettingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg;
		if (colosseumCardSettingDlg == null)
		{
			colosseumCardSettingDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg);
		}
		colosseumCardSettingDlg.SetSolInfo(gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind);
	}

	public void UpdatePVP2SoliderList(int nCharKind)
	{
		int colosseumEnableBatchSoldierKindCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKindCount();
		if (colosseumEnableBatchSoldierKindCount <= 0)
		{
			return;
		}
		int num = 0;
		NewListItem newListItem = null;
		for (int i = 0; i < colosseumEnableBatchSoldierKindCount; i++)
		{
			int colosseumEnableBatchSoldierKind = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKind(i);
			if (colosseumEnableBatchSoldierKind > 0)
			{
				if (colosseumEnableBatchSoldierKind == nCharKind)
				{
					GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND = new GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND();
					gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind = colosseumEnableBatchSoldierKind;
					newListItem = new NewListItem(this.m_nlSolList.ColumnNum, true);
					newListItem.SetListItemData(6, false);
					EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(colosseumEnableBatchSoldierKind, 0);
					if (eventHeroCharCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.SetListItemData(6, true);
					}
					newListItem.SetListItemData(1, new NkListSolInfo
					{
						SolCharKind = colosseumEnableBatchSoldierKind,
						SolGrade = -100,
						SolLevel = 1,
						ShowCombat = false,
						ShowLevel = false,
						ShowGrade = false
					}, gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND, null, new EZValueChangedDelegate(this.BtClickListBoxPVPMakeUp));
					newListItem.SetListItemData(2, string.Empty, null, null, null);
					newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("121"), null, null, null);
					newListItem.SetListItemData(4, string.Empty, null, null, null);
					newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("144"), null, null, null);
					newListItem.SetListItemData(7, string.Empty, gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND, null, new EZValueChangedDelegate(this.BtClickSolInfo));
					bool flag = false;
					if (SoldierBatch.SOLDIERBATCH != null && SoldierBatch.SOLDIERBATCH.GetTempBattlePos((long)colosseumEnableBatchSoldierKind) > 0)
					{
						flag = true;
					}
					if (flag)
					{
						newListItem.SetListItemData(1, true);
						newListItem.SetListItemData(2, true);
						newListItem.SetListItemData(3, true);
						newListItem.SetListItemData(4, false);
						newListItem.SetListItemData(5, false);
					}
					else
					{
						newListItem.SetListItemData(1, true);
						newListItem.SetListItemData(2, false);
						newListItem.SetListItemData(3, false);
						newListItem.SetListItemData(4, false);
						newListItem.SetListItemData(5, false);
					}
					break;
				}
				num++;
			}
		}
		if (newListItem != null)
		{
			this.m_nlSolList.UpdateAdd(num, newListItem);
		}
	}

	private void BtClickSolInfo(IUIObject obj)
	{
		GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND = obj.Data as GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND;
		if (gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND == null || gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind <= 0)
		{
			return;
		}
		ColosseumCardSettingDlg colosseumCardSettingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg;
		if (colosseumCardSettingDlg == null)
		{
			colosseumCardSettingDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg);
		}
		colosseumCardSettingDlg.SetSolInfo(gS_COLOSSEUM_BATCH_SOLDIERLIST_KIND.i32SolKind);
	}
}
