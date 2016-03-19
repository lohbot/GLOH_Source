using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Event_Dlg : Form
{
	private Toolbar m_tbToolBar;

	private NewListBox m_nlbEvent;

	public eEVENT_TYPE m_eCurrentTapIndex;

	public bool DailyDungeonComplete;

	public bool DailyQuestComplete;

	public int nQuestNpcKind = 388;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "dlg_event", G_ID.EVENT_MAIN, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_tbToolBar = (base.GetControl("ToolBar") as Toolbar);
		this.m_tbToolBar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1761");
		this.m_tbToolBar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1762");
		UIPanelTab expr_65 = this.m_tbToolBar.Control_Tab[0];
		expr_65.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_65.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		UIPanelTab expr_93 = this.m_tbToolBar.Control_Tab[1];
		expr_93.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_93.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		this.m_nlbEvent = (base.GetControl("NLB_event") as NewListBox);
		this.m_nlbEvent.Clear();
		this.CurrentEventReq();
	}

	public void CurrentEventReq()
	{
		GS_GET_EVENT_INFO_REQ gS_GET_EVENT_INFO_REQ = new GS_GET_EVENT_INFO_REQ();
		gS_GET_EVENT_INFO_REQ.m_nEventMode = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_GET_EVENT_INFO_REQ, gS_GET_EVENT_INFO_REQ);
	}

	public void ScheduleEventReq()
	{
		GS_GET_EVENT_INFO_REQ gS_GET_EVENT_INFO_REQ = new GS_GET_EVENT_INFO_REQ();
		gS_GET_EVENT_INFO_REQ.m_nEventMode = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_GET_EVENT_INFO_REQ, gS_GET_EVENT_INFO_REQ);
	}

	public void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_eCurrentTapIndex = (eEVENT_TYPE)uIPanelTab.panel.index;
		this.SelectTab(this.m_eCurrentTapIndex);
	}

	public void SelectTab(eEVENT_TYPE eAuctionRegisterType)
	{
		this.m_nlbEvent.Clear();
		if (eAuctionRegisterType != eEVENT_TYPE.eBUNNING_CURRENT_EVENT)
		{
			if (eAuctionRegisterType == eEVENT_TYPE.eBUNNING_Schedule_EVENT)
			{
				this.ScheduleEventReq();
			}
		}
		else
		{
			this.CurrentEventReq();
		}
	}

	public void CurrentEventSetData()
	{
		this.m_nlbEvent.Clear();
		for (int i = 0; i < 6; i++)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			int level = kMyCharInfo.GetLevel();
			EVENT_INFO eventInfo = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventInfo(i);
			if (eventInfo != null)
			{
				if (eventInfo.m_nEventType != 0)
				{
					BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
					if (value != null)
					{
						NewListItem newListItem = new NewListItem(this.m_nlbEvent.ColumnNum, true);
						if (level > value.m_nLimitLevel)
						{
							this.DailyDungeonComplete = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.SetBasicData();
							if (eventInfo.m_nEventType != 13 || !this.DailyDungeonComplete)
							{
								this.DailyQuestComplete = this.IsDailyQuestClear();
								if (eventInfo.m_nEventType != 14 || !this.DailyQuestComplete)
								{
									if (eventInfo.m_nEventType != 15 || !this.DailyQuestComplete)
									{
										if (eventInfo.m_nEventType != 16 || !this.DailyQuestComplete)
										{
											if (eventInfo.m_nEventType != 17 || !NrTSingleton<ContentsLimitManager>.Instance.IsBountyHunt())
											{
												newListItem.Data = eventInfo;
												string empty = string.Empty;
												long num = eventInfo.m_nLeftEventTime - PublicMethod.GetCurTime();
												string text = string.Empty;
												if (num >= 0L)
												{
													text = PublicMethod.ConvertTime(num);
												}
												NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
												{
													NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
													"timestring",
													text
												});
												newListItem.SetListItemData(0, false);
												newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nTitleText.ToString()), null, null, null);
												newListItem.SetListItemData(2, false);
												string text2 = string.Format("{0}", "UI/etc/" + value.m_strImage);
												newListItem.SetListItemData(3, text2, true, null, null);
												bool flag = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.IsHaveReward((eBUNNING_EVENT)eventInfo.m_nEventType);
												BUNNING_EVENT_INFO value2 = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
												if (value2.m_strRewardType == "ITEM")
												{
													newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), value.m_eEventType, new EZValueChangedDelegate(this.RewardItem), null);
												}
												else
												{
													newListItem.SetListItemData(4, false);
												}
												if (!flag)
												{
													newListItem.SetListItemEnable(4, false);
												}
												if (eventInfo.m_nEventType == 13)
												{
													newListItem.SetListItemEnable(4, false);
													newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyDungeon), null);
												}
												else if (eventInfo.m_nEventType == 14 || eventInfo.m_nEventType == 15 || eventInfo.m_nEventType == 16)
												{
													newListItem.SetListItemEnable(4, false);
													newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("466"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyQuest), null);
												}
												else if (eventInfo.m_nEventType == 17)
												{
													newListItem.SetListItemEnable(4, false);
													newListItem.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickBountyHunt), null);
												}
												else
												{
													newListItem.SetListItemData(9, false);
												}
												newListItem.SetListItemData(5, false);
												newListItem.SetListItemData(6, empty, null, null, null);
												newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), eventInfo.m_nEventType, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
												string empty2 = string.Empty;
												int limitCount = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetLimitCount((eBUNNING_EVENT)eventInfo.m_nEventType);
												if (limitCount == -1)
												{
													newListItem.SetListItemData(8, false);
												}
												else
												{
													NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
													{
														NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1198"),
														"count",
														limitCount
													});
													newListItem.SetListItemData(8, empty2, null, null, null);
												}
												this.m_nlbEvent.Add(newListItem);
												BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
												if (bookmarkDlg != null)
												{
													bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.MAINEVENT);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		this.m_nlbEvent.RepositionItems();
	}

	public void ScheduleEventSetData()
	{
		this.m_nlbEvent.Clear();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		for (int i = 0; i < NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventCount(); i++)
		{
			BUNNING_EVENT_REFLASH_INFO bUNNING_EVENT_REFLASH_INFO = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.Get_Value(i + eBUNNING_EVENT.eBUNNING_EVENT_BABELPARTY);
			if (bUNNING_EVENT_REFLASH_INFO != null)
			{
				BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue(bUNNING_EVENT_REFLASH_INFO.m_eEventType);
				if (value != null)
				{
					if (level > value.m_nLimitLevel)
					{
						NewListItem newListItem = new NewListItem(this.m_nlbEvent.ColumnNum, true);
						newListItem.SetListItemData(0, false);
						newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bUNNING_EVENT_REFLASH_INFO.m_nTitleText.ToString()), null, null, null);
						newListItem.SetListItemData(2, false);
						string text = string.Format("{0}", "UI/etc/" + value.m_strImage);
						newListItem.SetListItemData(3, text, true, null, null);
						newListItem.SetListItemData(4, false);
						newListItem.SetListItemData(5, false);
						newListItem.SetListItemData(6, false);
						newListItem.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), bUNNING_EVENT_REFLASH_INFO.m_eEventType, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
						newListItem.SetListItemData(8, false);
						newListItem.SetListItemData(9, false);
						this.m_nlbEvent.Add(newListItem);
					}
				}
			}
		}
		for (int j = 0; j < 6; j++)
		{
			EVENT_INFO eventInfo = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventInfo(j);
			if (eventInfo != null)
			{
				if (eventInfo.m_nEventType != 0)
				{
					BUNNING_EVENT_INFO value2 = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
					if (value2 != null)
					{
						if (level > value2.m_nLimitLevel)
						{
							this.DailyDungeonComplete = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.SetBasicData();
							if (eventInfo.m_nEventType == 13 && this.DailyDungeonComplete)
							{
								NewListItem newListItem2 = new NewListItem(this.m_nlbEvent.ColumnNum, true);
								newListItem2.SetListItemData(0, false);
								newListItem2.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nTitleText.ToString()), null, null, null);
								newListItem2.SetListItemData(2, false);
								string text2 = string.Format("{0}", "UI/etc/" + value2.m_strImage);
								newListItem2.SetListItemData(3, text2, true, null, null);
								newListItem2.SetListItemData(4, false);
								newListItem2.SetListItemData(5, false);
								newListItem2.SetListItemData(6, false);
								newListItem2.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), eventInfo.m_nEventType, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
								newListItem2.SetListItemData(8, false);
								if (eventInfo.m_nEventType == 13)
								{
									newListItem2.SetListItemEnable(4, false);
									newListItem2.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value2.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyDungeon), null);
								}
								else
								{
									newListItem2.SetListItemData(9, false);
								}
								this.m_nlbEvent.Add(newListItem2);
							}
							this.DailyQuestComplete = this.IsDailyQuestClear();
							if ((eventInfo.m_nEventType == 14 && this.DailyQuestComplete) || (eventInfo.m_nEventType == 15 && this.DailyQuestComplete) || (eventInfo.m_nEventType == 16 && this.DailyQuestComplete))
							{
								NewListItem newListItem3 = new NewListItem(this.m_nlbEvent.ColumnNum, true);
								newListItem3.SetListItemData(0, false);
								newListItem3.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nTitleText.ToString()), null, null, null);
								newListItem3.SetListItemData(2, false);
								string text3 = string.Format("{0}", "UI/etc/" + value2.m_strImage);
								newListItem3.SetListItemData(3, text3, true, null, null);
								newListItem3.SetListItemData(4, false);
								newListItem3.SetListItemData(5, false);
								newListItem3.SetListItemData(6, false);
								newListItem3.SetListItemData(7, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), eventInfo.m_nEventType, new EZValueChangedDelegate(this.OnClickDetailInfo), null);
								newListItem3.SetListItemData(8, false);
								if (eventInfo.m_nEventType == 13)
								{
									newListItem3.SetListItemEnable(4, false);
									newListItem3.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value2.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyDungeon), null);
								}
								else
								{
									newListItem3.SetListItemData(9, false);
								}
								this.m_nlbEvent.Add(newListItem3);
							}
						}
					}
				}
			}
		}
		this.m_nlbEvent.RepositionItems();
	}

	private void OnClickDetailInfo(IUIObject obj)
	{
		UIButton uIButton = (UIButton)obj;
		if (uIButton == null)
		{
			return;
		}
		if (uIButton.data == null)
		{
			return;
		}
		bool bCurrentEvent = false;
		if (this.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_CURRENT_EVENT || (int)uIButton.data == 13 || (int)uIButton.data == 14 || (int)uIButton.data == 15 || (int)uIButton.data == 16)
		{
			bCurrentEvent = true;
		}
		Event_Detail_Dlg event_Detail_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_MAIN_EXPLAIN) as Event_Detail_Dlg;
		if (event_Detail_Dlg != null)
		{
			event_Detail_Dlg.SetSelectInfoItem((int)uIButton.data, bCurrentEvent);
		}
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Show()
	{
		base.Show();
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
		this.UpsateNewListBox();
	}

	public void UpsateNewListBox()
	{
		for (int i = 0; i < 6; i++)
		{
			EVENT_INFO eventInfo = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventInfo(i);
			if (eventInfo != null)
			{
				if (eventInfo.m_nEventType > 0)
				{
					string empty = string.Empty;
					long num = eventInfo.m_nLeftEventTime - PublicMethod.GetCurTime();
					string text = string.Empty;
					if (num >= 0L)
					{
						text = PublicMethod.ConvertTime(num);
					}
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
						"timestring",
						text
					});
					for (int j = 0; j < this.m_nlbEvent.Count; j++)
					{
						UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_nlbEvent.GetItem(j);
						if (uIListItemContainer != null)
						{
							EVENT_INFO eVENT_INFO = (EVENT_INFO)uIListItemContainer.Data;
							if (eVENT_INFO != null)
							{
								if (eVENT_INFO.m_nEventType == eventInfo.m_nEventType)
								{
									Label label = uIListItemContainer.GetElement(6) as Label;
									if (null != label)
									{
										label.Text = empty;
									}
									break;
								}
							}
						}
					}
				}
			}
		}
		this.m_nlbEvent.RepositionItems();
	}

	private void RewardItem(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		GS_GET_EVENT_REWARD_REQ gS_GET_EVENT_REWARD_REQ = new GS_GET_EVENT_REWARD_REQ();
		gS_GET_EVENT_REWARD_REQ.m_nEventType = (int)obj.Data;
		SendPacket.GetInstance().SendObject(1664, gS_GET_EVENT_REWARD_REQ);
	}

	private bool IsDailyQuestClear()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		int num = (int)myCharInfo.GetCharDetail(5);
		return 0 < num && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num);
	}

	private void OnClickDailyDungeon(IUIObject obj)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_MAIN);
		}
		this.Close();
	}

	private void OnClickDailyQuest(IUIObject obj)
	{
		NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(this.nQuestNpcKind);
		this.Close();
	}

	private void OnClickBountyHunt(IUIObject obj)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BOUNTYHUNTING_DLG))
		{
			BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
			if (bountyHuntingDlg != null)
			{
				bountyHuntingDlg.SetData();
			}
		}
		this.Close();
	}
}
