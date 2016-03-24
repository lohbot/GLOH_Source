using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class MineRecordDlg : Form
{
	private Button m_btBack;

	private Toolbar m_tbTab;

	private NewListBox m_nlbRecordList;

	private Box m_Box_Page;

	private Button m_btNextPage;

	private Button m_btPrevPage;

	private Button m_btAll;

	private short m_page = 1;

	public override void InitializeComponent()
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_LIST_REQ(false, 1, false);
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/dlg_MineRecord", G_ID.MINE_RECORD_DLG, false, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBack));
		this.m_tbTab = (base.GetControl("ToolBar_ToolBar") as Toolbar);
		this.m_tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2988");
		this.m_tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2989");
		UIPanelTab expr_92 = this.m_tbTab.Control_Tab[0];
		expr_92.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_92.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_C0 = this.m_tbTab.Control_Tab[1];
		expr_C0.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_C0.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_tbTab.SetSelectTabIndex(0);
		this.m_nlbRecordList = (base.GetControl("NLB_MineRecord") as NewListBox);
		this.m_nlbRecordList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBattleResult));
		this.m_btNextPage = (base.GetControl("Button_Pre") as Button);
		this.m_btNextPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPrev));
		this.m_btPrevPage = (base.GetControl("Button_Next") as Button);
		this.m_btPrevPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNext));
		this.m_btAll = (base.GetControl("Button_ALL") as Button);
		this.m_btAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickAll));
		this.m_Box_Page = (base.GetControl("Box_Page") as Box);
	}

	public void SetList(GS_MINE_BATTLE_RESULT_LIST_ACK _ACK, NkDeserializePacket kDeserializePacket)
	{
		if (_ACK.i16Page != 1 && _ACK.ui8Count == 0)
		{
			return;
		}
		if (_ACK.i16Page == 1 && _ACK.ui8Count == 0)
		{
			NoticeIconDlg.SetIcon(ICON_TYPE.MINE_RECORED, false);
		}
		string empty = string.Empty;
		this.m_Box_Page.Text = _ACK.i16Page.ToString();
		this.m_nlbRecordList.Clear();
		this.m_page = _ACK.i16Page;
		for (int i = 0; i < (int)_ACK.ui8Count; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbRecordList.ColumnNum, true, string.Empty);
			MINE_BATTLE_RESULT_INFO packet = kDeserializePacket.GetPacket<MINE_BATTLE_RESULT_INFO>();
			if (i == 0)
			{
				NrTSingleton<MineManager>.Instance.m_i64FirstLegionActionID_By_List = packet.i64LegionActionID;
			}
			NrTSingleton<MineManager>.Instance.m_i64LastLegionActionID_By_List = packet.i64LegionActionID;
			newListItem.Data = packet;
			if (packet.i64BattleTime >= 0L)
			{
				DateTime dueDate = PublicMethod.GetDueDate(packet.i64BattleTime);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602"),
					"year",
					dueDate.Year,
					"month",
					dueDate.Month,
					"day",
					dueDate.Day
				});
				newListItem.SetListItemData(1, empty, null, null, null);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
					"hour",
					string.Format("{0:00}", dueDate.Hour),
					"min",
					string.Format("{0:00}", dueDate.Minute),
					"sec",
					string.Format("{0:00}", dueDate.Second)
				});
				newListItem.SetListItemData(2, empty, null, null, null);
			}
			newListItem.SetListItemData(3, this.GetListText(packet), null, null, null);
			newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1088"), packet, new EZValueChangedDelegate(this.OnClickBattleResult), null);
			if (packet.i32ItemUnique > 0 && packet.i32ItemNum != 0)
			{
				newListItem.SetListItemData(6, new ITEM
				{
					m_nItemUnique = packet.i32ItemUnique,
					m_nItemNum = packet.i32ItemNum
				}, null, null, null);
			}
			else
			{
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Attack");
				newListItem.SetListItemData(6, loader, null, null, null);
			}
			this.m_nlbRecordList.Add(newListItem);
		}
		this.m_nlbRecordList.RepositionItems();
	}

	public void Refresh_If_NonCompleteList()
	{
		if (this.m_tbTab.CurrentPanel.index == 0)
		{
			NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_LIST_REQ(false, 1, false);
		}
	}

	public override void Update()
	{
	}

	public void OnClickBack(IUIObject obj)
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
		this.Close();
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_LIST_REQ(uIPanelTab.panel.index == 1, 1, false);
		if (uIPanelTab.panel.index == 1)
		{
			this.m_btAll.Hide(true);
		}
		else
		{
			this.m_btAll.Hide(false);
		}
	}

	public override void OnClose()
	{
	}

	private void OnClickNext(IUIObject obj)
	{
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_LIST_REQ(this.m_tbTab.CurrentPanel.index == 1, this.m_page + 1, true);
	}

	private void OnClickPrev(IUIObject obj)
	{
		if (this.m_page <= 1)
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_BATTLE_RESULT_LIST_REQ(this.m_tbTab.CurrentPanel.index == 1, this.m_page - 1, false);
	}

	private void OnClickAll(IUIObject obj)
	{
		GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ gS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ = new GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ();
		for (int i = 0; i < this.m_nlbRecordList.Count; i++)
		{
			UIListItemContainer item = this.m_nlbRecordList.GetItem(i);
			if (item == null)
			{
				break;
			}
			MINE_BATTLE_RESULT_INFO mINE_BATTLE_RESULT_INFO = item.Data as MINE_BATTLE_RESULT_INFO;
			if (mINE_BATTLE_RESULT_INFO == null)
			{
				break;
			}
			gS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ.i64LegionActionIDs[i] = mINE_BATTLE_RESULT_INFO.i64LegionActionID;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ, gS_MINE_BATTLE_RESULT_REWARD_GET_ALL_REQ);
	}

	private void OnClickBattleResult(IUIObject obj)
	{
		MINE_BATTLE_RESULT_INFO mINE_BATTLE_RESULT_INFO = obj.Data as MINE_BATTLE_RESULT_INFO;
		if (mINE_BATTLE_RESULT_INFO == null)
		{
			UIListItemContainer selectedItem = this.m_nlbRecordList.SelectedItem;
			if (selectedItem != null)
			{
				mINE_BATTLE_RESULT_INFO = (selectedItem.Data as MINE_BATTLE_RESULT_INFO);
			}
			if (mINE_BATTLE_RESULT_INFO == null)
			{
				return;
			}
		}
		GS_MINE_BATTLE_RESULT_REPORT_REQ gS_MINE_BATTLE_RESULT_REPORT_REQ = new GS_MINE_BATTLE_RESULT_REPORT_REQ();
		gS_MINE_BATTLE_RESULT_REPORT_REQ.i64LegionActionID = mINE_BATTLE_RESULT_INFO.i64LegionActionID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_BATTLE_RESULT_REPORT_REQ, gS_MINE_BATTLE_RESULT_REPORT_REQ);
	}

	public string GetListText(MINE_BATTLE_RESULT_INFO info)
	{
		string empty = string.Empty;
		byte grade = 0;
		MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(info.i16MineDataID);
		if (mineCreateDataFromID != null)
		{
			grade = mineCreateDataFromID.GetGrade();
		}
		string text = (!info.bIsHiddenName) ? TKString.NEWString(info.szEnemyGuildName) : "????";
		if (info.bMeveBack)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2993"),
				"targetname",
				BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
			});
		}
		else if (info.bAttack)
		{
			if (info.bWin)
			{
				if (text != string.Empty)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2990"),
						"targetname",
						text,
						"targetname2",
						BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
					});
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2991"),
						"targetname",
						BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
					});
				}
			}
			else if (text != string.Empty)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2994"),
					"targetname",
					text,
					"targetname2",
					BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3041"),
					"targetname",
					BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
				});
			}
		}
		else if (!info.bAttack)
		{
			if (info.bWin)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2996"),
					"targetname",
					text,
					"targetname2",
					BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2995"),
					"targetname",
					text,
					"targetname2",
					BASE_MINE_DATA.GetMineName(grade, info.i16MineDataID)
				});
			}
		}
		return empty;
	}
}
