using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class ChallengeDlg : Form
{
	private Toolbar m_kToolbar;

	private NewListBox m_kListBox;

	private Box m_Notice1;

	private Box m_Notice2;

	private Box m_Notice3;

	private Box m_Percent;

	private float m_fOldSize;

	private ChallengeManager.TYPE m_nSelectType = ChallengeManager.TYPE.ONEDAY;

	private bool m_bRequest;

	private int count1;

	private int count2;

	private int count3;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_Challenge", G_ID.CHALLENGE_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_kToolbar = (base.GetControl("ToolBar_ToolBar1") as Toolbar);
		string[] array = new string[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("425"),
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("431")
		};
		for (int i = 0; i < this.m_kToolbar.Count; i++)
		{
			this.m_kToolbar.Control_Tab[i].Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), array[i]);
			UIPanelTab expr_83 = this.m_kToolbar.Control_Tab[i];
			expr_83.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_83.ButtonClick, new EZValueChangedDelegate(this.ClickToolBar));
		}
		this.m_kToolbar.Control_Tab[0].Data = ChallengeManager.TYPE.ONEDAY;
		this.m_kToolbar.Control_Tab[1].Data = ChallengeManager.TYPE.CONTINUE;
		this.m_kListBox = (base.GetControl("NLB_Mission") as NewListBox);
		this.m_Notice1 = (base.GetControl("DT_Notice1") as Box);
		this.m_Notice1.SetLocation(this.m_Notice1.GetLocationX(), this.m_Notice1.GetLocationY(), this.m_kToolbar.GetLocation().z - 0.1f);
		this.m_Notice1.Visible = false;
		this.m_Notice2 = (base.GetControl("DT_Notice2") as Box);
		this.m_Notice2.SetLocation(this.m_Notice2.GetLocationX(), this.m_Notice2.GetLocationY(), this.m_kToolbar.GetLocation().z - 0.1f);
		this.m_Notice2.Visible = false;
		this.m_Notice3 = (base.GetControl("DT_Notice3") as Box);
		this.m_Notice3.SetLocation(this.m_Notice3.GetLocationX(), this.m_Notice3.GetLocationY(), this.m_kToolbar.GetLocation().z - 0.1f);
		this.m_Notice3.Visible = false;
		this.m_Percent = (base.GetControl("DT_GageBar") as Box);
		this.m_fOldSize = this.m_Percent.GetSize().x;
		this.m_nSelectType = ChallengeManager.TYPE.ONEDAY;
		this.SetChallengeInfo(this.m_nSelectType);
		base.SetScreenCenter();
		base.ShowLayer((int)this.m_nSelectType);
	}

	private void ClickList(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.m_bRequest)
		{
			return;
		}
		GS_CHAR_CHALLENGE_REWARD_REQ gS_CHAR_CHALLENGE_REWARD_REQ = new GS_CHAR_CHALLENGE_REWARD_REQ();
		gS_CHAR_CHALLENGE_REWARD_REQ.m_nUnique = (int)((short)obj.Data);
		SendPacket.GetInstance().SendObject(1031, gS_CHAR_CHALLENGE_REWARD_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "REPUTATION", "REWARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_bRequest = true;
	}

	public void ClickToolBar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		ChallengeManager.TYPE nSelectType = (ChallengeManager.TYPE)((int)obj.Data);
		this.m_nSelectType = nSelectType;
		this.SetChallengeInfo(this.m_nSelectType);
		base.ShowLayer((int)this.m_nSelectType);
	}

	public void ChangeTab()
	{
		this.m_kToolbar.SetSelectTabIndex(1);
	}

	public override void Update()
	{
		int num = NrTSingleton<ChallengeManager>.Instance.GetDayRewardNoticeCount();
		if (0 < num)
		{
			if (this.count1 != num)
			{
				this.m_Notice1.Visible = true;
				this.m_Notice1.Text = num.ToString();
				this.count1 = num;
			}
		}
		else if (this.m_Notice1.Visible)
		{
			this.m_Notice1.Visible = false;
		}
		num = NrTSingleton<ChallengeManager>.Instance.GetContinueRewardNoticeCount();
		if (0 < num)
		{
			if (this.count2 != num)
			{
				this.m_Notice2.Visible = true;
				this.m_Notice2.Text = num.ToString();
				this.count2 = num;
			}
		}
		else if (this.m_Notice2.Visible)
		{
			this.m_Notice2.Visible = false;
		}
		num = NrTSingleton<ChallengeManager>.Instance.GetEventRewardNoticeCount();
		if (0 < num)
		{
			if (this.count3 != num)
			{
				this.m_Notice3.Visible = true;
				this.m_Notice3.Text = num.ToString();
				this.count3 = num;
			}
		}
		else if (this.m_Notice3.Visible)
		{
			this.m_Notice3.Visible = false;
		}
	}

	public void SetChallengeInfo()
	{
		this.SetChallengeInfo(this.m_nSelectType);
	}

	private void CheckDayChallenge(ref NewListItem item, NrMyCharInfo charInfo, ChallengeTable table, int index, ref string colorText, ref long value, ref bool complete)
	{
		long num = (long)charInfo.GetDayCharDetail((eCHAR_DAY_COUNT)table.m_nDetailInfoIndex);
		if (num >= (long)table.m_kRewardInfo[index].m_nConditionCount)
		{
			colorText = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
			value = (long)table.m_kRewardInfo[index].m_nConditionCount;
			complete = true;
		}
		else
		{
			value = num;
			item.SetListItemEnable(8, false);
			item.SetEnable(false);
			complete = false;
		}
		long charDetail = charInfo.GetCharDetail(12);
		if (1L <= (charDetail & table.m_nCheckRewardValue))
		{
			item.SetListItemEnable(8, false);
			item.SetEnable(false);
			value = (long)table.m_kRewardInfo[index].m_nConditionCount;
			complete = false;
		}
		else
		{
			item.SetListItemData(9, false);
		}
	}

	private void MakeOneDayList(NrMyCharInfo charInfo, UserChallengeInfo userChalengeInfo, Dictionary<short, ChallengeTable> challengeInfo)
	{
		this.m_kListBox.Clear();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		List<short> list = new List<short>();
		foreach (ChallengeTable current in challengeInfo.Values)
		{
			if ((int)current.m_nLevel <= charInfo.GetLevel())
			{
				string empty = string.Empty;
				long num = 0L;
				int num2 = -1;
				NewListItem newListItem = new NewListItem(this.m_kListBox.ColumnNum, true);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_szTitleTextKey), null, null, null);
				newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), current.m_nUnique, new EZValueChangedDelegate(this.ClickList), null);
				for (int i = 0; i < current.m_kRewardInfo.Count; i++)
				{
					if (charInfo.GetLevel() < current.m_kRewardInfo[i].m_nConditionLevel)
					{
						num2 = i;
						break;
					}
				}
				if (num2 != -1)
				{
					bool flag5 = false;
					long charDetail = charInfo.GetCharDetail(12);
					if (current.m_nUnique == 1011 || current.m_nUnique == 1012 || current.m_nUnique == 1013 || current.m_nUnique == 1014 || current.m_nUnique == 1015)
					{
						if (1L <= (charDetail & current.m_nCheckRewardValue) && current.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR5)
						{
							continue;
						}
						if (flag)
						{
							continue;
						}
						this.CheckDayChallenge(ref newListItem, charInfo, current, num2, ref empty, ref num, ref flag5);
						flag = true;
						if (!flag5)
						{
							continue;
						}
						list.Add(current.m_nUnique);
					}
					else if (current.m_nUnique == 1064 || current.m_nUnique == 1065 || current.m_nUnique == 1066)
					{
						if (1L <= (charDetail & current.m_nCheckRewardValue) && current.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER3)
						{
							continue;
						}
						if (flag2)
						{
							continue;
						}
						this.CheckDayChallenge(ref newListItem, charInfo, current, num2, ref empty, ref num, ref flag5);
						flag2 = true;
						if (!flag5)
						{
							continue;
						}
						list.Add(current.m_nUnique);
					}
					else if (current.m_nUnique == 1080 || current.m_nUnique == 1081 || current.m_nUnique == 1082)
					{
						if (1L <= (charDetail & current.m_nCheckRewardValue) && current.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO3)
						{
							continue;
						}
						if (flag3)
						{
							continue;
						}
						this.CheckDayChallenge(ref newListItem, charInfo, current, num2, ref empty, ref num, ref flag5);
						flag3 = true;
						if (!flag5)
						{
							continue;
						}
						list.Add(current.m_nUnique);
					}
					else if (current.m_nUnique == 1090 || current.m_nUnique == 1091 || current.m_nUnique == 1092)
					{
						if (1L <= (charDetail & current.m_nCheckRewardValue) && current.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER3)
						{
							continue;
						}
						if (flag4)
						{
							continue;
						}
						this.CheckDayChallenge(ref newListItem, charInfo, current, num2, ref empty, ref num, ref flag5);
						flag4 = true;
						if (!flag5)
						{
							continue;
						}
						list.Add(current.m_nUnique);
					}
					else
					{
						this.CheckDayChallenge(ref newListItem, charInfo, current, num2, ref empty, ref num, ref flag5);
						if (!flag5)
						{
							continue;
						}
						list.Add(current.m_nUnique);
					}
					newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(current.m_szIconKey), null, null, null);
					string str = string.Empty;
					string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_kRewardInfo[num2].m_szConditionTextKey);
					if (text.Contains("count"))
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
						{
							text,
							"count",
							current.m_kRewardInfo[num2].m_nConditionCount,
							"count1",
							num,
							"count2",
							current.m_kRewardInfo[num2].m_nConditionCount
						});
					}
					else
					{
						str = text;
					}
					newListItem.SetListItemData(3, empty + str, null, null, null);
					if (0L < current.m_kRewardInfo[num2].m_nMoney)
					{
						newListItem.SetListItemData(5, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"), null, null, null);
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
							"count",
							current.m_kRewardInfo[num2].m_nMoney
						});
						newListItem.SetListItemData(6, text, null, null, null);
					}
					else if (0 < current.m_kRewardInfo[num2].m_nItemUnique)
					{
						newListItem.SetListItemData(5, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_kRewardInfo[num2].m_nItemUnique), null, null, null);
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
							"itemname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_kRewardInfo[num2].m_nItemUnique),
							"count",
							current.m_kRewardInfo[num2].m_nItemNum
						});
						newListItem.SetListItemData(6, text, null, null, null);
					}
					newListItem.Data = current.m_nUnique;
					this.m_kListBox.Add(newListItem);
				}
			}
		}
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		foreach (ChallengeTable current2 in challengeInfo.Values)
		{
			if (!list.Contains(current2.m_nUnique))
			{
				if ((int)current2.m_nLevel <= charInfo.GetLevel())
				{
					string empty2 = string.Empty;
					long num3 = 0L;
					int num4 = -1;
					NewListItem newListItem2 = new NewListItem(this.m_kListBox.ColumnNum, true);
					newListItem2.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current2.m_szTitleTextKey), null, null, null);
					newListItem2.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), current2.m_nUnique, new EZValueChangedDelegate(this.ClickList), null);
					for (int j = 0; j < current2.m_kRewardInfo.Count; j++)
					{
						if (charInfo.GetLevel() < current2.m_kRewardInfo[j].m_nConditionLevel)
						{
							num4 = j;
							break;
						}
					}
					if (num4 != -1)
					{
						bool flag10 = false;
						long charDetail2 = charInfo.GetCharDetail(12);
						if (current2.m_nUnique == 1011 || current2.m_nUnique == 1012 || current2.m_nUnique == 1013 || current2.m_nUnique == 1014 || current2.m_nUnique == 1015)
						{
							if (1L <= (charDetail2 & current2.m_nCheckRewardValue) && current2.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_HEROWAR5)
							{
								continue;
							}
							if (flag6)
							{
								continue;
							}
							this.CheckDayChallenge(ref newListItem2, charInfo, current2, num4, ref empty2, ref num3, ref flag10);
							flag6 = true;
						}
						else if (current2.m_nUnique == 1064 || current2.m_nUnique == 1065 || current2.m_nUnique == 1066)
						{
							if (1L <= (charDetail2 & current2.m_nCheckRewardValue) && current2.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_COLOSSEUM_WITHPLAYER3)
							{
								continue;
							}
							if (flag7)
							{
								continue;
							}
							this.CheckDayChallenge(ref newListItem2, charInfo, current2, num4, ref empty2, ref num3, ref flag10);
							flag7 = true;
						}
						else if (current2.m_nUnique == 1080 || current2.m_nUnique == 1081 || current2.m_nUnique == 1082)
						{
							if (1L <= (charDetail2 & current2.m_nCheckRewardValue) && current2.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_INVITE_KAKAO3)
							{
								continue;
							}
							if (flag8)
							{
								continue;
							}
							this.CheckDayChallenge(ref newListItem2, charInfo, current2, num4, ref empty2, ref num3, ref flag10);
							flag8 = true;
						}
						else if (current2.m_nUnique == 1090 || current2.m_nUnique == 1091 || current2.m_nUnique == 1092)
						{
							if (1L <= (charDetail2 & current2.m_nCheckRewardValue) && current2.m_nCheckRewardValue != ChallengeManager.CHALLENGEREWARD_DAY_WIN_BABELTOWER3)
							{
								continue;
							}
							if (flag9)
							{
								continue;
							}
							this.CheckDayChallenge(ref newListItem2, charInfo, current2, num4, ref empty2, ref num3, ref flag10);
							flag9 = true;
						}
						else
						{
							this.CheckDayChallenge(ref newListItem2, charInfo, current2, num4, ref empty2, ref num3, ref flag10);
						}
						newListItem2.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(current2.m_szIconKey), null, null, null);
						string str2 = string.Empty;
						string text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current2.m_kRewardInfo[num4].m_szConditionTextKey);
						if (text2.Contains("count"))
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str2, new object[]
							{
								text2,
								"count",
								current2.m_kRewardInfo[num4].m_nConditionCount,
								"count1",
								num3,
								"count2",
								current2.m_kRewardInfo[num4].m_nConditionCount
							});
						}
						else
						{
							str2 = text2;
						}
						newListItem2.SetListItemData(3, empty2 + str2, null, null, null);
						if (0L < current2.m_kRewardInfo[num4].m_nMoney)
						{
							newListItem2.SetListItemData(5, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"), null, null, null);
							text2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
								"count",
								current2.m_kRewardInfo[num4].m_nMoney
							});
							newListItem2.SetListItemData(6, text2, null, null, null);
						}
						else if (0 < current2.m_kRewardInfo[num4].m_nItemUnique)
						{
							newListItem2.SetListItemData(5, NrTSingleton<ItemManager>.Instance.GetItemTexture(current2.m_kRewardInfo[num4].m_nItemUnique), null, null, null);
							text2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
								"itemname",
								NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current2.m_kRewardInfo[num4].m_nItemUnique),
								"count",
								current2.m_kRewardInfo[num4].m_nItemNum
							});
							newListItem2.SetListItemData(6, text2, null, null, null);
						}
						newListItem2.Data = current2.m_nUnique;
						this.m_kListBox.Add(newListItem2);
					}
				}
			}
		}
		this.m_kListBox.RepositionItems();
		list.Clear();
	}

	private void MakeWeekList(NrMyCharInfo charInfo, UserChallengeInfo userChalengeInfo, Dictionary<short, ChallengeTable> challengeInfo)
	{
		this.m_kListBox.Clear();
		foreach (ChallengeTable current in challengeInfo.Values)
		{
			if ((int)current.m_nLevel <= charInfo.GetLevel())
			{
				string empty = string.Empty;
				long num = 0L;
				int index = 0;
				NewListItem newListItem = new NewListItem(this.m_kListBox.ColumnNum, true);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_szTitleTextKey), null, null, null);
				for (int i = 0; i < current.m_kRewardInfo.Count; i++)
				{
					if (charInfo.GetLevel() < current.m_kRewardInfo[i].m_nConditionLevel)
					{
						index = i;
						break;
					}
				}
				newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(current.m_szIconKey), null, null, null);
				string str = string.Empty;
				string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_kRewardInfo[index].m_szConditionTextKey);
				if (text.Contains("count"))
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						text,
						"count",
						current.m_kRewardInfo[index].m_nConditionCount,
						"count1",
						num,
						"count2",
						current.m_kRewardInfo[index].m_nConditionCount
					});
				}
				else
				{
					str = text;
				}
				newListItem.SetListItemData(3, empty + str, null, null, null);
				if (0L < current.m_kRewardInfo[index].m_nMoney)
				{
					newListItem.SetListItemData(5, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"), null, null, null);
					text = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
						"count",
						current.m_kRewardInfo[index].m_nMoney
					});
					newListItem.SetListItemData(6, text, null, null, null);
				}
				else if (0 < current.m_kRewardInfo[index].m_nItemUnique)
				{
					newListItem.SetListItemData(5, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_kRewardInfo[index].m_nItemUnique), null, null, null);
					text = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
						"itemname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_kRewardInfo[index].m_nItemUnique),
						"count",
						current.m_kRewardInfo[index].m_nItemNum
					});
					newListItem.SetListItemData(6, text, null, null, null);
				}
				newListItem.Data = current.m_nUnique;
				this.m_kListBox.Add(newListItem);
			}
		}
		this.m_kListBox.RepositionItems();
	}

	private void MakeContinueList(NrMyCharInfo charInfo, UserChallengeInfo userChalengeInfo, Dictionary<short, ChallengeTable> challengeInfo)
	{
		this.m_kListBox.Clear();
		List<short> list = new List<short>();
		foreach (ChallengeTable current in challengeInfo.Values)
		{
			if ((int)current.m_nLevel <= charInfo.GetLevel())
			{
				string str = string.Empty;
				long num = 0L;
				bool flag = true;
				int num2 = -1;
				NewListItem newListItem = new NewListItem(this.m_kListBox.ColumnNum, true);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_szTitleTextKey), null, null, null);
				newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), current.m_nUnique, new EZValueChangedDelegate(this.ClickList), null);
				newListItem.SetListItemData(9, false);
				Challenge_Info userChallengeInfo = userChalengeInfo.GetUserChallengeInfo(current.m_nUnique);
				if (userChallengeInfo == null)
				{
					newListItem.SetListItemEnable(8, false);
					newListItem.SetEnable(false);
					num2 = 0;
					flag = false;
				}
				else
				{
					int i = 0;
					int num3 = 64;
					while (i < current.m_kRewardInfo.Count)
					{
						bool flag2 = false;
						if (i < num3)
						{
							long num4 = 1L << (i & 31);
							if ((userChallengeInfo.m_bGetReward1 & num4) == 0L)
							{
								flag2 = true;
							}
						}
						else
						{
							long num5 = 1L << (i - num3 & 31);
							if ((userChallengeInfo.m_bGetReward1 & num5) == 0L)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							if (current.m_kRewardInfo[i].m_nConditionCount == 0)
							{
								flag = false;
								break;
							}
							num2 = i;
							if (userChallengeInfo.m_nValue >= (long)current.m_kRewardInfo[i].m_nConditionCount)
							{
								str = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
								list.Add(userChallengeInfo.m_nUnique);
								num = userChallengeInfo.m_nValue;
								break;
							}
							flag = false;
							break;
						}
						else
						{
							i++;
						}
					}
					if (i == 64)
					{
						flag = false;
					}
				}
				if (flag && num2 != -1)
				{
					newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(current.m_szIconKey), null, null, null);
					string str2 = string.Empty;
					string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current.m_kRewardInfo[num2].m_szConditionTextKey);
					if (text.Contains("count"))
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str2, new object[]
						{
							text,
							"count",
							current.m_kRewardInfo[num2].m_nConditionCount,
							"count1",
							num,
							"count2",
							current.m_kRewardInfo[num2].m_nConditionCount
						});
					}
					else
					{
						str2 = text;
					}
					newListItem.SetListItemData(3, str + str2, null, null, null);
					if (0L < current.m_kRewardInfo[num2].m_nMoney)
					{
						newListItem.SetListItemData(5, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"), null, null, null);
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
							"count",
							current.m_kRewardInfo[num2].m_nMoney
						});
						newListItem.SetListItemData(6, text, null, null, null);
					}
					else if (0 < current.m_kRewardInfo[num2].m_nItemUnique)
					{
						newListItem.SetListItemData(5, NrTSingleton<ItemManager>.Instance.GetItemTexture(current.m_kRewardInfo[num2].m_nItemUnique), null, null, null);
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
							"itemname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.m_kRewardInfo[num2].m_nItemUnique),
							"count",
							current.m_kRewardInfo[num2].m_nItemNum
						});
						newListItem.SetListItemData(6, text, null, null, null);
					}
					newListItem.Data = current.m_nUnique;
					this.m_kListBox.Add(newListItem);
				}
			}
		}
		foreach (ChallengeTable current2 in challengeInfo.Values)
		{
			if (!list.Contains(current2.m_nUnique))
			{
				ChallengeTimeTable challengeTimeTable = NrTSingleton<ChallengeManager>.Instance.GetChallengeTimeTable(current2.m_nUnique);
				if (challengeTimeTable != null)
				{
					DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
					if (dueDate.Year < (int)challengeTimeTable.m_nStartYear || dueDate.Month < (int)challengeTimeTable.m_nStartMonth || dueDate.Hour < (int)challengeTimeTable.m_nStartHour || dueDate.Minute < (int)challengeTimeTable.m_nStartMinute)
					{
						continue;
					}
					if (dueDate.Year > (int)challengeTimeTable.m_nEndYear || dueDate.Month > (int)challengeTimeTable.m_nEndMonth || dueDate.Day > (int)challengeTimeTable.m_nEndDay || dueDate.Hour > (int)challengeTimeTable.m_nEndHour || dueDate.Minute > (int)challengeTimeTable.m_nEndMinute)
					{
						continue;
					}
				}
				if ((int)current2.m_nLevel <= charInfo.GetLevel())
				{
					string str3 = string.Empty;
					long num6 = 0L;
					bool flag3 = true;
					int num7 = -1;
					NewListItem newListItem2 = new NewListItem(this.m_kListBox.ColumnNum, true);
					newListItem2.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current2.m_szTitleTextKey), null, null, null);
					newListItem2.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493"), current2.m_nUnique, new EZValueChangedDelegate(this.ClickList), null);
					newListItem2.SetListItemData(9, false);
					Challenge_Info userChallengeInfo2 = userChalengeInfo.GetUserChallengeInfo(current2.m_nUnique);
					if (userChallengeInfo2 == null)
					{
						newListItem2.SetListItemEnable(8, false);
						newListItem2.SetEnable(false);
						num7 = 0;
					}
					else
					{
						int j = 0;
						int num8 = 64;
						while (j < current2.m_kRewardInfo.Count)
						{
							bool flag4 = false;
							if (j < num8)
							{
								long num9 = 1L << (j & 31);
								if ((userChallengeInfo2.m_bGetReward1 & num9) == 0L)
								{
									flag4 = true;
								}
							}
							else
							{
								long num10 = 1L << (j - num8 & 31);
								if ((userChallengeInfo2.m_bGetReward1 & num10) == 0L)
								{
									flag4 = true;
								}
							}
							if (flag4)
							{
								if (current2.m_kRewardInfo[j].m_nConditionCount == 0)
								{
									flag3 = false;
									break;
								}
								num7 = j;
								if (userChallengeInfo2.m_nValue >= (long)current2.m_kRewardInfo[j].m_nConditionCount)
								{
									str3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
								}
								else
								{
									newListItem2.SetListItemEnable(8, false);
									newListItem2.SetEnable(false);
								}
								num6 = userChallengeInfo2.m_nValue;
								break;
							}
							else
							{
								j++;
							}
						}
						if (j == 64)
						{
							flag3 = false;
						}
					}
					if (flag3 && num7 != -1)
					{
						newListItem2.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(current2.m_szIconKey), null, null, null);
						string str4 = string.Empty;
						string text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(current2.m_kRewardInfo[num7].m_szConditionTextKey);
						if (text2.Contains("count"))
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str4, new object[]
							{
								text2,
								"count",
								current2.m_kRewardInfo[num7].m_nConditionCount,
								"count1",
								num6,
								"count2",
								current2.m_kRewardInfo[num7].m_nConditionCount
							});
						}
						else
						{
							str4 = text2;
						}
						newListItem2.SetListItemData(3, str3 + str4, null, null, null);
						if (0L < current2.m_kRewardInfo[num7].m_nMoney)
						{
							newListItem2.SetListItemData(5, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"), null, null, null);
							text2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
								"count",
								current2.m_kRewardInfo[num7].m_nMoney
							});
							newListItem2.SetListItemData(6, text2, null, null, null);
						}
						else if (0 < current2.m_kRewardInfo[num7].m_nItemUnique)
						{
							newListItem2.SetListItemData(5, NrTSingleton<ItemManager>.Instance.GetItemTexture(current2.m_kRewardInfo[num7].m_nItemUnique), null, null, null);
							text2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
								"itemname",
								NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current2.m_kRewardInfo[num7].m_nItemUnique),
								"count",
								current2.m_kRewardInfo[num7].m_nItemNum
							});
							newListItem2.SetListItemData(6, text2, null, null, null);
						}
						newListItem2.Data = current2.m_nUnique;
						this.m_kListBox.Add(newListItem2);
					}
				}
			}
		}
		this.m_kListBox.RepositionItems();
		list.Clear();
		string empty = string.Empty;
		int num11 = NrTSingleton<ChallengeManager>.Instance.CalcTotalPercent();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
			"Count",
			NrTSingleton<ChallengeManager>.Instance.CalcTotalPercent()
		});
		this.m_Percent.Text = empty;
		this.m_Percent.SetSize(this.m_fOldSize * (float)num11 / 100f, this.m_Percent.GetSize().y);
	}

	public void SetChallengeInfo(ChallengeManager.TYPE type)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
		if (userChallengeInfo == null)
		{
			return;
		}
		Dictionary<short, ChallengeTable> challengeType = NrTSingleton<ChallengeManager>.Instance.GetChallengeType(type);
		if (challengeType == null)
		{
			this.m_kListBox.Clear();
			return;
		}
		if (type == ChallengeManager.TYPE.ONEDAY)
		{
			this.MakeOneDayList(kMyCharInfo, userChallengeInfo, challengeType);
		}
		else if (type == ChallengeManager.TYPE.WEEK)
		{
			this.MakeWeekList(kMyCharInfo, userChallengeInfo, challengeType);
		}
		else if (type == ChallengeManager.TYPE.CONTINUE)
		{
			this.MakeContinueList(kMyCharInfo, userChallengeInfo, challengeType);
		}
		else if (type == ChallengeManager.TYPE.EVENT)
		{
			this.MakeContinueList(kMyCharInfo, userChallengeInfo, challengeType);
		}
		this.m_bRequest = false;
	}
}
