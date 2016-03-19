using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class Battle_Emergency_SelectDlg : Form
{
	private Button m_btRequetEmergency;

	private NewListBox m_nlSelectSoldierList;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/EMERGENCY_CALL/dlg_emergencyselect", G_ID.BATTLE_EMERGENCT_SELECT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btRequetEmergency = (base.GetControl("Button_OK") as Button);
		Button expr_1C = this.m_btRequetEmergency;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickRequestEmergency));
		this.m_nlSelectSoldierList = (base.GetControl("NewListBox_soldierlist") as NewListBox);
		this.SetSolList();
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation((GUICamera.width - base.GetSizeX()) / 2f, (GUICamera.height - base.GetSizeY()) / 2f);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void OnClickRequestEmergency(IUIObject obj)
	{
		if (this.m_nlSelectSoldierList.SelectedItem == null)
		{
			return;
		}
		long num = (long)this.m_nlSelectSoldierList.SelectedItem.Data;
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo nkSoldierInfo = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(num);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetSolInfo(num);
		}
		if (nkSoldierInfo == null)
		{
			return;
		}
		this.OnRequestEmergency(null);
	}

	public void OnRequestEmergency(object a_oObject)
	{
		if (this.m_nlSelectSoldierList.SelectedItem == null)
		{
			return;
		}
		long nChangeSolID = (long)this.m_nlSelectSoldierList.SelectedItem.Data;
		GS_BATTLE_CHANGE_SOLDIER_REQ gS_BATTLE_CHANGE_SOLDIER_REQ = new GS_BATTLE_CHANGE_SOLDIER_REQ();
		gS_BATTLE_CHANGE_SOLDIER_REQ.nChangeSolID = nChangeSolID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_CHANGE_SOLDIER_REQ, gS_BATTLE_CHANGE_SOLDIER_REQ);
		this.Close();
	}

	public void SetSolList()
	{
		this.m_nlSelectSoldierList.Clear();
		this.m_kSolList.Clear();
		List<int> list = new List<int>();
		list.Clear();
		eSOL_SUBDATA eType = eSOL_SUBDATA.SOL_SUBDATA_NONE;
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
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			eType = eSOL_SUBDATA.SOL_SUBDATA_PVP_BATTLEPOS;
			NkSoldierInfo[] kSolInfo = soldierList.m_kSolInfo;
			for (int i = 0; i < kSolInfo.Length; i++)
			{
				NkSoldierInfo nkSoldierInfo = kSolInfo[i];
				if (nkSoldierInfo.GetSolID() > 0L)
				{
					if (nkSoldierInfo.GetSolSubData(eType) > 0L && !list.Contains(nkSoldierInfo.GetCharKind()))
					{
						list.Add(nkSoldierInfo.GetCharKind());
					}
				}
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (current.GetSolID() > 0L)
				{
					if (current.GetSolSubData(eType) > 0L && !list.Contains(current.GetCharKind()))
					{
						list.Add(current.GetCharKind());
					}
				}
			}
			NkSoldierInfo[] kSolInfo2 = soldierList.m_kSolInfo;
			for (int j = 0; j < kSolInfo2.Length; j++)
			{
				NkSoldierInfo nkSoldierInfo2 = kSolInfo2[j];
				if (nkSoldierInfo2.GetSolID() > 0L)
				{
					if (!list.Contains(nkSoldierInfo2.GetCharKind()))
					{
						if (nkSoldierInfo2.GetSolSubData(eType) <= 0L)
						{
							this.m_kSolList.Add(nkSoldierInfo2);
						}
					}
				}
			}
			foreach (NkSoldierInfo current2 in readySolList.GetList().Values)
			{
				if (current2.GetSolID() > 0L)
				{
					if (!list.Contains(current2.GetCharKind()))
					{
						if (current2.GetSolPosType() != 2)
						{
							if (current2.GetSolPosType() != 6)
							{
								if (current2.GetSolSubData(eType) <= 0L)
								{
									this.m_kSolList.Add(current2);
								}
							}
						}
					}
				}
			}
		}
		else if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			List<int> babelCharKind = Battle.BATTLE.BabelCharKind;
			if (babelCharKind == null)
			{
				return;
			}
			foreach (int current3 in babelCharKind)
			{
				list.Add(current3);
			}
			NkSoldierInfo[] kSolInfo3 = soldierList.m_kSolInfo;
			for (int k = 0; k < kSolInfo3.Length; k++)
			{
				NkSoldierInfo nkSoldierInfo3 = kSolInfo3[k];
				if (nkSoldierInfo3.GetSolID() > 0L)
				{
					if (!list.Contains(nkSoldierInfo3.GetCharKind()))
					{
						this.m_kSolList.Add(nkSoldierInfo3);
					}
				}
			}
			foreach (NkSoldierInfo current4 in readySolList.GetList().Values)
			{
				if (current4.GetSolID() > 0L)
				{
					if (!list.Contains(current4.GetCharKind()))
					{
						if (current4.GetSolPosType() != 2)
						{
							if (current4.GetSolPosType() != 6)
							{
								this.m_kSolList.Add(current4);
							}
						}
					}
				}
			}
		}
		else
		{
			NkSoldierInfo[] kSolInfo4 = soldierList.m_kSolInfo;
			for (int l = 0; l < kSolInfo4.Length; l++)
			{
				NkSoldierInfo nkSoldierInfo4 = kSolInfo4[l];
				if (nkSoldierInfo4.GetSolID() > 0L)
				{
					list.Add(nkSoldierInfo4.GetCharKind());
				}
			}
			foreach (NkSoldierInfo current5 in readySolList.GetList().Values)
			{
				if (!current5.IsInjuryStatus())
				{
					if (current5.GetSolID() > 0L)
					{
						if (current5.GetSolPosType() != 2)
						{
							if (current5.GetSolPosType() != 6)
							{
								if (!list.Contains(current5.GetCharKind()))
								{
									this.m_kSolList.Add(current5);
								}
							}
						}
					}
				}
			}
		}
		this.m_kSolList.Sort(new Comparison<NkSoldierInfo>(this.CompareCombatPower));
		foreach (NkSoldierInfo current6 in this.m_kSolList)
		{
			if (!current6.IsInjuryStatus())
			{
				if (current6.GetSolID() > 0L)
				{
					if (!current6.IsSolStatus(4))
					{
						NewListItem newListItem = new NewListItem(this.m_nlSelectSoldierList.ColumnNum, true);
						newListItem.Data = current6.GetSolID();
						NkListSolInfo nkListSolInfo = new NkListSolInfo();
						nkListSolInfo.SolCharKind = current6.GetCharKind();
						nkListSolInfo.SolGrade = (int)current6.GetGrade();
						nkListSolInfo.SolLevel = current6.GetLevel();
						nkListSolInfo.FightPower = current6.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER);
						nkListSolInfo.ShowLevel = false;
						if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current6.GetCharKind()) != null)
						{
							nkListSolInfo.ShowCombat = true;
							EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(current6.GetCharKind(), current6.GetGrade());
							if (eventHeroCharCode != null)
							{
								newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
							}
							else
							{
								UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(current6.GetCharKind(), (int)current6.GetGrade());
								if (legendFrame != null)
								{
									newListItem.SetListItemData(0, legendFrame, null, null, null);
								}
								else
								{
									newListItem.SetListItemData(0, true);
								}
							}
							newListItem.SetListItemData(1, nkListSolInfo, current6, null, null);
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
								"charname",
								current6.GetName()
							});
							newListItem.SetListItemData(2, empty, null, null, null);
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031"),
								"count",
								current6.GetLevel()
							});
							newListItem.SetListItemData(3, empty, null, null, null);
							this.m_nlSelectSoldierList.Add(newListItem);
						}
					}
				}
			}
		}
		this.m_nlSelectSoldierList.RepositionItems();
	}

	private int CompareExpDESC(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetExp().CompareTo(a.GetExp());
	}

	private int CompareCombatPower(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetCombatPower().CompareTo(a.GetCombatPower());
	}
}
