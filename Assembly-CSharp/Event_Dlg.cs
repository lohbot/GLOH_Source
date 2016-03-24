using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Globalization;
using UnityForms;

public class Event_Dlg : Form
{
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
		this.m_nlbEvent = (base.GetControl("NLB_event") as NewListBox);
		this.m_nlbEvent.Clear();
		this.CurrentEventReq();
		this.ScheduleEventReq();
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

	public void CurrentEventSetData()
	{
		for (int i = 0; i < 7; i++)
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
					if (eventInfo.m_nEventType != 14)
					{
						if (eventInfo.m_nEventType != 18)
						{
							if (eventInfo.m_nEventType != 1)
							{
								BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
								if (value != null)
								{
									string text = this.EventWeek((int)eventInfo.m_nEventInfoWeek);
									int num = value.m_nStartTime + value.m_nDurationTime / 60;
									if (num >= 0 && num > 24)
									{
										num -= 24;
									}
									NewListItem newListItem = new NewListItem(this.m_nlbEvent.ColumnNum, true, string.Empty);
									if (level > value.m_nLimitLevel)
									{
										this.DailyDungeonComplete = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.SetBasicData();
										if (eventInfo.m_nEventType != 14 || !this.DailyDungeonComplete)
										{
											this.DailyQuestComplete = this.IsDailyQuestClear();
											if (eventInfo.m_nEventType != 15 || !this.DailyQuestComplete)
											{
												if (eventInfo.m_nEventType != 16 || !this.DailyQuestComplete)
												{
													if (eventInfo.m_nEventType != 17 || !this.DailyQuestComplete)
													{
														if (eventInfo.m_nEventType != 18 || !NrTSingleton<ContentsLimitManager>.Instance.IsBountyHunt())
														{
															if (eventInfo.m_nEventType != 36 || NrTSingleton<ContentsLimitManager>.Instance.IsChallenge())
															{
																newListItem.Data = eventInfo;
																string text2 = string.Empty;
																long num2 = eventInfo.m_nLeftEventTime - PublicMethod.GetCurTime();
																string text3 = string.Empty;
																if (num2 >= 0L)
																{
																	text3 = PublicMethod.ConvertTime(num2);
																}
																NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																{
																	NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
																	"timestring",
																	text3
																});
																string text4 = string.Format("{0}", "UI/etc/" + value.m_strImage);
																newListItem.SetListItemData(0, text4, true, null, null);
																newListItem.SetListItemData(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nTitleText.ToString()), null, null, null);
																newListItem.SetListItemData(2, text2, null, null, null);
																string empty = string.Empty;
																int limitCount = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetLimitCount((eBUNNING_EVENT)eventInfo.m_nEventType);
																if (limitCount == -1)
																{
																	newListItem.SetListItemData(3, false);
																}
																else
																{
																	NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
																	{
																		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1198"),
																		"count",
																		limitCount
																	});
																	newListItem.SetListItemData(3, empty, null, null, null);
																}
																text = this.EventWeek(-1);
																if (eventInfo.m_nEventType == 15)
																{
																	text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2550");
																}
																else if (eventInfo.m_nEventType == 16)
																{
																	text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2560");
																}
																else if (eventInfo.m_nEventType == 17)
																{
																	text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2561");
																}
																else if (value.m_nYear > 0)
																{
																	NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																	{
																		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2307"),
																		"month",
																		value.m_nMonth.ToString(),
																		"day",
																		value.m_nDay.ToString(),
																		"starttime",
																		eventInfo.m_nStartTime.ToString(),
																		"endmonth",
																		value.m_nEndMonth.ToString(),
																		"endday",
																		value.m_nEndDay.ToString(),
																		"endtime",
																		num.ToString()
																	});
																}
																else if (eventInfo.m_nEventType == 4 || eventInfo.m_nEventType == 37 || eventInfo.m_nEventType == 38 || eventInfo.m_nEventType == 39)
																{
																	if (eventInfo.m_nMaxLimitCount > 0)
																	{
																		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																		{
																			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
																			"day",
																			text,
																			"starttime",
																			eventInfo.m_nStartTime.ToString(),
																			"endtime",
																			num.ToString(),
																			"count",
																			limitCount
																		});
																	}
																	else
																	{
																		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																		{
																			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
																			"day",
																			text,
																			"starttime",
																			eventInfo.m_nStartTime.ToString(),
																			"endtime",
																			num.ToString()
																		});
																	}
																}
																else if (eventInfo.m_nEventType == 19 || eventInfo.m_nEventType == 20 || eventInfo.m_nEventType == 21 || eventInfo.m_nEventType == 22 || eventInfo.m_nEventType == 23 || eventInfo.m_nEventType == 24)
																{
																	string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2543");
																	if (eventInfo.m_nMaxLimitCount > 0)
																	{
																		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																		{
																			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
																			"day",
																			text,
																			"starttime",
																			eventInfo.m_nStartTime.ToString(),
																			"endtime",
																			num.ToString(),
																			"count",
																			limitCount
																		});
																	}
																	else
																	{
																		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																		{
																			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
																			"day",
																			textFromInterface,
																			"starttime",
																			eventInfo.m_nStartTime.ToString(),
																			"endtime",
																			num.ToString()
																		});
																	}
																}
																else if (eventInfo.m_nMaxLimitCount > 0)
																{
																	NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																	{
																		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
																		"day",
																		text,
																		"starttime",
																		eventInfo.m_nStartTime.ToString(),
																		"endtime",
																		num.ToString(),
																		"count",
																		limitCount
																	});
																}
																else
																{
																	NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
																	{
																		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
																		"day",
																		text,
																		"starttime",
																		eventInfo.m_nStartTime.ToString(),
																		"endtime",
																		num.ToString()
																	});
																}
																newListItem.SetListItemData(4, text2, null, null, null);
																string text5 = string.Empty;
																if (eventInfo.m_nEventType == 37 || eventInfo.m_nEventType == 38 || eventInfo.m_nEventType == 39 || eventInfo.m_nEventType == 4 || eventInfo.m_nEventType == 5 || eventInfo.m_nEventType == 6 || eventInfo.m_nEventType == 7 || eventInfo.m_nEventType == 8 || eventInfo.m_nEventType == 9 || eventInfo.m_nEventType == 10 || eventInfo.m_nEventType == 11 || eventInfo.m_nEventType == 12 || eventInfo.m_nEventType == 13 || eventInfo.m_nEventType == 25 || eventInfo.m_nEventType == 26 || eventInfo.m_nEventType == 27 || eventInfo.m_nEventType == 28 || eventInfo.m_nEventType == 29 || eventInfo.m_nEventType == 30 || eventInfo.m_nEventType == 31 || eventInfo.m_nEventType == 32 || eventInfo.m_nEventType == 33 || eventInfo.m_nEventType == 34 || eventInfo.m_nEventType == 35)
																{
																	NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text5, new object[]
																	{
																		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nExplain.ToString()),
																		"rate",
																		value.m_nRewardCount.ToString()
																	});
																}
																else
																{
																	text5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfo.m_nExplain.ToString());
																}
																newListItem.SetListItemData(5, text5, null, null, null);
																bool flag = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.IsHaveReward((eBUNNING_EVENT)eventInfo.m_nEventType);
																BUNNING_EVENT_INFO value2 = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eventInfo.m_nEventType);
																if (value2.m_strRewardType == "ITEM")
																{
																	newListItem.SetListItemData(6, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), value.m_eEventType, new EZValueChangedDelegate(this.RewardItem), null);
																}
																else
																{
																	newListItem.SetListItemData(6, false);
																}
																if (!flag)
																{
																	newListItem.SetListItemEnable(6, false);
																}
																if (eventInfo.m_nEventType == 14)
																{
																	newListItem.SetListItemEnable(6, false);
																	newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyDungeon), null);
																}
																else if (eventInfo.m_nEventType == 15 || eventInfo.m_nEventType == 16 || eventInfo.m_nEventType == 17)
																{
																	newListItem.SetListItemEnable(6, false);
																	newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("466"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickDailyQuest), null);
																}
																else if (eventInfo.m_nEventType == 18)
																{
																	newListItem.SetListItemEnable(6, false);
																	newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1580"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickBountyHunt), null);
																}
																else if (eventInfo.m_nEventType == 36)
																{
																	if (NrTSingleton<ContentsLimitManager>.Instance.IsChallenge())
																	{
																		newListItem.SetListItemEnable(6, false);
																		newListItem.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"), value.m_eEventType, new EZValueChangedDelegate(this.OnClickChallenge), null);
																	}
																}
																else
																{
																	newListItem.SetListItemData(10, false);
																}
																newListItem.SetListItemData(11, false);
																newListItem.SetListItemData(12, false);
																newListItem.SetListItemData(13, false);
																this.m_nlbEvent.Add(newListItem);
																MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
																if (myCharInfoDlg != null)
																{
																	myCharInfoDlg.UpdateNoticeInfo();
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
					}
				}
			}
		}
		this.m_nlbEvent.RepositionItems();
	}

	public void ScheduleEventSetData()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		for (int i = 0; i < NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventCount(); i++)
		{
			BUNNING_EVENT_REFLASH_INFO bUNNING_EVENT_REFLASH_INFO = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.Get_Value(i + eBUNNING_EVENT.eBUNNING_EVENT_SHOWEVENT);
			if (bUNNING_EVENT_REFLASH_INFO != null)
			{
				if (bUNNING_EVENT_REFLASH_INFO.m_eEventType != eBUNNING_EVENT.eBUNNING_EVENT_DAILYDUNGEON)
				{
					if (bUNNING_EVENT_REFLASH_INFO.m_eEventType != eBUNNING_EVENT.eBUNNING_EVENT_BOUNTYHUNT)
					{
						BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue(bUNNING_EVENT_REFLASH_INFO.m_eEventType);
						if (value != null)
						{
							if (level > value.m_nLimitLevel)
							{
								NewListItem newListItem = new NewListItem(this.m_nlbEvent.ColumnNum, false, string.Empty);
								string text = string.Format("{0}", "UI/etc/" + value.m_strImage);
								newListItem.SetListItemData(0, text, true, null, null);
								newListItem.SetListItemData(1, false);
								newListItem.SetListItemData(2, false);
								newListItem.SetListItemData(3, false);
								string text2 = string.Empty;
								int num = bUNNING_EVENT_REFLASH_INFO.m_nStartTime + bUNNING_EVENT_REFLASH_INFO.m_nEndTime / 60;
								if (num >= 0 && num > 24)
								{
									num -= 24;
								}
								string text3 = this.EventWeek((int)value.m_nWeek);
								if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST)
								{
									text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2550");
								}
								else if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST1)
								{
									text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2560");
								}
								else if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_DAILYQUEST2)
								{
									text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2561");
								}
								else if (value.m_nYear > 0)
								{
									NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
									{
										NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2307"),
										"month",
										value.m_nMonth.ToString(),
										"day",
										value.m_nDay.ToString(),
										"starttime",
										value.m_nStartTime.ToString(),
										"endmonth",
										value.m_nEndMonth.ToString(),
										"endday",
										value.m_nEndDay.ToString(),
										"endtime",
										num.ToString()
									});
									if (bUNNING_EVENT_REFLASH_INFO.m_nLimitCount > 0)
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
											"day",
											text3,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString(),
											"count",
											bUNNING_EVENT_REFLASH_INFO.m_nLimitCount
										});
									}
									else
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
											"day",
											text3,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString()
										});
									}
								}
								else if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE)
								{
									text3 = this.EventWeek(-1);
									if (bUNNING_EVENT_REFLASH_INFO.m_nLimitCount > 0)
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
											"day",
											text3,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString(),
											"count",
											bUNNING_EVENT_REFLASH_INFO.m_nLimitCount
										});
									}
									else
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
											"day",
											text3,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString()
										});
									}
								}
								else if (value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF1 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF2 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF3 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF4 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF5 || value.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF6)
								{
									string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2543");
									if (bUNNING_EVENT_REFLASH_INFO.m_nLimitCount > 0)
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
											"day",
											text3,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString(),
											"count",
											bUNNING_EVENT_REFLASH_INFO.m_nLimitCount
										});
									}
									else
									{
										NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
										{
											NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
											"day",
											textFromInterface,
											"starttime",
											value.m_nStartTime.ToString(),
											"endtime",
											num.ToString()
										});
									}
								}
								else if (bUNNING_EVENT_REFLASH_INFO.m_nLimitCount > 0)
								{
									NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
									{
										NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3193"),
										"day",
										text3,
										"starttime",
										value.m_nStartTime.ToString(),
										"endtime",
										num.ToString(),
										"count",
										bUNNING_EVENT_REFLASH_INFO.m_nLimitCount
									});
								}
								else
								{
									NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
									{
										NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
										"day",
										text3,
										"starttime",
										value.m_nStartTime.ToString(),
										"endtime",
										num.ToString()
									});
								}
								newListItem.SetListItemData(4, text2, null, null, null);
								newListItem.SetListItemData(5, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bUNNING_EVENT_REFLASH_INFO.m_nExplain.ToString()), null, null, null);
								newListItem.SetListItemData(6, false);
								newListItem.SetListItemData(10, false);
								newListItem.Data = new EVENT_INFO
								{
									m_nEventType = (int)value.m_eEventType,
									m_nEventInfoWeek = value.m_nWeek,
									m_nStartTime = value.m_nStartTime,
									m_nTitleText = value.m_nTitleText,
									m_nExplain = value.m_nExplain,
									m_nEndDurationTime = value.m_nDurationTime,
									m_nLeftEventTime = 0L,
									m_nMaxLimitCount = 0
								};
								text2 = this.SetScheduleWeekEventTime(value);
								newListItem.SetListItemData(11, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bUNNING_EVENT_REFLASH_INFO.m_nTitleText.ToString()), null, null, null);
								newListItem.SetListItemData(12, true);
								newListItem.SetListItemData(13, text2, null, null, null);
								newListItem.SetListItemEnable(13, false);
								this.m_nlbEvent.Add(newListItem);
							}
						}
					}
				}
			}
		}
		this.m_nlbEvent.RepositionItems();
	}

	private string SetScheduleWeekEventTime(BUNNING_EVENT_INFO pEventInfo)
	{
		string result = string.Empty;
		DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
		Calendar calendar = CultureInfo.InvariantCulture.Calendar;
		int dayOfWeek = (int)calendar.GetDayOfWeek(dueDate);
		if ((int)pEventInfo.m_nWeek == -1 || (int)pEventInfo.m_nWeek == dayOfWeek)
		{
			if (dueDate.Hour < pEventInfo.m_nStartTime)
			{
				DateTime startTime = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, pEventInfo.m_nStartTime, 0, 0);
				result = this.ScheduleEventTime(dueDate, startTime);
			}
			else
			{
				DateTime startTime2 = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, pEventInfo.m_nStartTime, 0, 0);
				if ((int)pEventInfo.m_nWeek == -1)
				{
					startTime2 = startTime2.AddDays(1.0);
				}
				else
				{
					startTime2 = startTime2.AddDays(7.0);
				}
				result = this.ScheduleEventTime(dueDate, startTime2);
			}
		}
		else
		{
			int num = dayOfWeek - (int)pEventInfo.m_nWeek;
			if (num < 0)
			{
				num += 7;
			}
			num = 7 - num;
			DateTime startTime3 = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, pEventInfo.m_nStartTime, 0, 0);
			startTime3 = startTime3.AddDays((double)num);
			result = this.ScheduleEventTime(dueDate, startTime3);
		}
		return result;
	}

	private string ScheduleEventTime(DateTime ServerTimeNow, DateTime startTime)
	{
		string empty = string.Empty;
		int num = ServerTimeNow.CompareTo(startTime);
		if (0 > num)
		{
			TimeSpan timeSpan = startTime - ServerTimeNow;
			if (0 < timeSpan.Days)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3194"),
					"day",
					timeSpan.Days
				});
			}
			else if (0 < timeSpan.Hours)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3192"),
					"timestring",
					timeSpan
				});
			}
			else if (0 < timeSpan.Minutes)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3192"),
					"timestring",
					timeSpan
				});
			}
		}
		return empty;
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
		if (this.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_CURRENT_EVENT || (int)uIButton.data == 14 || (int)uIButton.data == 15 || (int)uIButton.data == 16 || (int)uIButton.data == 17)
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
		this.UpdateScheduleEventTime();
	}

	public void UpsateNewListBox()
	{
		for (int i = 0; i < 7; i++)
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
						UIListItemContainer item = this.m_nlbEvent.GetItem(j);
						if (item != null)
						{
							EVENT_INFO eVENT_INFO = (EVENT_INFO)item.Data;
							if (eVENT_INFO != null)
							{
								if (eVENT_INFO.m_nEventType == eventInfo.m_nEventType)
								{
									Label label = item.GetElement(2) as Label;
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

	public void UpdateScheduleEventTime()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		for (int i = 0; i < NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventCount(); i++)
		{
			BUNNING_EVENT_REFLASH_INFO bUNNING_EVENT_REFLASH_INFO = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.Get_Value(i + eBUNNING_EVENT.eBUNNING_EVENT_SHOWEVENT);
			if (bUNNING_EVENT_REFLASH_INFO != null)
			{
				string text = string.Empty;
				for (int j = 0; j < this.m_nlbEvent.Count; j++)
				{
					UIListItemContainer item = this.m_nlbEvent.GetItem(j);
					if (!(item == null))
					{
						EVENT_INFO eVENT_INFO = (EVENT_INFO)item.Data;
						if (eVENT_INFO != null)
						{
							if (bUNNING_EVENT_REFLASH_INFO.m_eEventType == (eBUNNING_EVENT)eVENT_INFO.m_nEventType)
							{
								DrawTexture drawTexture = item.GetElement(12) as DrawTexture;
								if (null != drawTexture)
								{
									drawTexture.SetLocationZ(-1f);
								}
								UIButton uIButton = item.GetElement(13) as UIButton;
								if (null != uIButton)
								{
									BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)eVENT_INFO.m_nEventType);
									if (value != null)
									{
										text = this.SetScheduleWeekEventTime(value);
										uIButton.SetLocationZ(-2f);
										uIButton.Text = text;
										break;
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
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_SELECT))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_SELECT);
		}
	}

	private void OnClickDailyQuest(IUIObject obj)
	{
		NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(this.nQuestNpcKind);
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

	private void OnClickChallenge(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsChallenge())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHALLENGE_DLG))
			{
				ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
				if (challengeDlg != null)
				{
					challengeDlg.SetEventTab();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
			}
		}
	}

	public string EventWeek(int nEventInfoWeek)
	{
		string result = string.Empty;
		switch (nEventInfoWeek + 1)
		{
		case 0:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2242");
			break;
		case 1:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2243");
			break;
		case 2:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2244");
			break;
		case 3:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2245");
			break;
		case 4:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2246");
			break;
		case 5:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2247");
			break;
		case 6:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2248");
			break;
		case 7:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2249");
			break;
		}
		return result;
	}
}
