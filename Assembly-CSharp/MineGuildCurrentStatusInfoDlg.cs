using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MineGuildCurrentStatusInfoDlg : Form
{
	private const int ONE_PAGE_COUNT = 4;

	private List<MINE_GUILD_CURRENTSTATUS_INFO> m_listMineGuild_CurrentStatus = new List<MINE_GUILD_CURRENTSTATUS_INFO>();

	private NewListBox m_lbCurrentStatus;

	private Button m_btMineSearch;

	private Box m_bxPage;

	private Button m_btPagePrev;

	private Button m_btPageNext;

	private Button m_btMineBattleRecord;

	private Button m_btGuildWar;

	private Label m_lagoMineJoinCount;

	private Label m_MineCountGetTime;

	private Label m_lbMineRecord;

	private DropDownList m_dlMineListStyle;

	private DrawTexture m_dtMineListBG;

	private int m_Page;

	private int m_MaxPage;

	private long m_GuildID;

	private float m_InfoSettingTime;

	private long m_RemainderCountGiveTime;

	private eMINE_LISTVIEW_STYLE m_Style = eMINE_LISTVIEW_STYLE.eMINE_LISTVIEW_STYLE_GUILD;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/dlg_minecondition", G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG, false, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbCurrentStatus = (base.GetControl("NLB_MineCondition") as NewListBox);
		this.m_lbCurrentStatus.Reserve = false;
		this.m_btMineSearch = (base.GetControl("BT_MineScarch") as Button);
		Button expr_3E = this.m_btMineSearch;
		expr_3E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3E.Click, new EZValueChangedDelegate(this.OnClickSearchMine));
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_btPagePrev = (base.GetControl("BT_Page01") as Button);
		Button expr_91 = this.m_btPagePrev;
		expr_91.Click = (EZValueChangedDelegate)Delegate.Combine(expr_91.Click, new EZValueChangedDelegate(this.OnClickPagePrev));
		this.m_btPageNext = (base.GetControl("BT_Page02") as Button);
		Button expr_CE = this.m_btPageNext;
		expr_CE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_CE.Click, new EZValueChangedDelegate(this.OnClickPageNext));
		this.m_btMineBattleRecord = (base.GetControl("BT_MineRecord") as Button);
		Button expr_10B = this.m_btMineBattleRecord;
		expr_10B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_10B.Click, new EZValueChangedDelegate(this.OnClickMineBattleRecord));
		this.m_btGuildWar = (base.GetControl("BT_GuildWarCondition") as Button);
		Button expr_148 = this.m_btGuildWar;
		expr_148.Click = (EZValueChangedDelegate)Delegate.Combine(expr_148.Click, new EZValueChangedDelegate(this.OnClickGuildWar));
		this.m_lbMineRecord = (base.GetControl("Label_MineRecord") as Label);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			this.m_btGuildWar.Hide(true);
			this.m_btMineBattleRecord.Hide(true);
			this.m_lbMineRecord.Visible = false;
		}
		this.m_lagoMineJoinCount = (base.GetControl("Label_Label18") as Label);
		this.m_dlMineListStyle = (base.GetControl("DropDownList_DropDownList19") as DropDownList);
		this.m_dlMineListStyle.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeMineListStyle));
		this.m_dtMineListBG = (base.GetControl("DT_DropDownBG") as DrawTexture);
		this.m_MineCountGetTime = (base.GetControl("LB_Time02") as Label);
		this.m_MineCountGetTime.SetText("00:00:00");
		this.SetDropDownList();
		this.m_GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		this.m_Style = eMINE_LISTVIEW_STYLE.eMINE_LISTVIEW_STYLE_GUILD;
		this.Hide();
	}

	public void SetGuildID(long Guild)
	{
		this.m_GuildID = Guild;
	}

	public void Show(int page, int totalcount)
	{
		this.m_Page = page;
		this.m_MaxPage = totalcount / 4;
		if (totalcount % 4 != 0)
		{
			this.m_MaxPage++;
		}
		this.SetJointCount();
		this.SetList();
		if (!base.ShowHide)
		{
			base.Show();
		}
	}

	public void SetJointCount()
	{
		string empty = string.Empty;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1842");
		int mineDayLimitCount = NrTSingleton<MineManager>.Instance.GetMineDayLimitCount();
		int mineJoinCount = NrTSingleton<MineManager>.Instance.GetMineJoinCount();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count1",
			mineDayLimitCount - mineJoinCount,
			"count2",
			mineDayLimitCount
		});
		this.m_lagoMineJoinCount.SetText(empty);
	}

	public void SetDropDownList()
	{
		this.m_dlMineListStyle.Clear();
		this.m_dlMineListStyle.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2888"), 1);
		this.m_dlMineListStyle.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2887"), 2);
		this.m_dlMineListStyle.SetViewArea(this.m_dlMineListStyle.Count);
		this.m_dlMineListStyle.RepositionItems();
		this.m_dlMineListStyle.SetFirstItem();
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			this.m_dlMineListStyle.SetVisible(false);
			this.m_dtMineListBG.Visible = false;
		}
		else
		{
			this.m_dlMineListStyle.SetVisible(true);
			this.m_dtMineListBG.Visible = true;
		}
	}

	public override void Update()
	{
		this.UpdateTime();
		base.Update();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
	}

	public void Clear()
	{
		this.m_lbCurrentStatus.Clear();
		this.m_listMineGuild_CurrentStatus.Clear();
	}

	public void SetRemainderCountGiveTime(long i64RemainderCounGiveTime)
	{
		this.m_RemainderCountGiveTime = i64RemainderCounGiveTime;
	}

	public void AddInfo(MINE_GUILD_CURRENTSTATUS_INFO[] info, byte bType)
	{
		this.Clear();
		for (int i = 0; i < info.Length; i++)
		{
			this.m_listMineGuild_CurrentStatus.Add(info[i]);
		}
		if (this.m_dlMineListStyle.IsVisible())
		{
			this.m_dlMineListStyle.SetIndex((int)(bType - 1));
			this.m_Style = (eMINE_LISTVIEW_STYLE)bType;
		}
	}

	public void SetList()
	{
		this.m_lbCurrentStatus.Clear();
		this.m_InfoSettingTime = Time.realtimeSinceStartup;
		string text = string.Empty;
		int num = 0;
		foreach (MINE_GUILD_CURRENTSTATUS_INFO current in this.m_listMineGuild_CurrentStatus)
		{
			NewListItem newListItem = new NewListItem(this.m_lbCurrentStatus.ColumnNum, true, string.Empty);
			MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(current.i16MineDataID);
			if (mineCreateDataFromID != null)
			{
				newListItem.SetListItemData(13, BASE_MINE_DATA.GetMineName(mineCreateDataFromID.GetGrade(), current.i16MineDataID), null, null, null);
				MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
				if (mineDataFromGrade != null)
				{
					UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(mineDataFromGrade.MINE_ICON_NAME);
					if (uIBaseInfoLoader != null)
					{
						newListItem.SetListItemData(6, uIBaseInfoLoader, current, new EZValueChangedDelegate(this.OnClickDefenceMilitaryInfo), null);
					}
				}
				int num2 = 9;
				string text2 = string.Empty;
				if (current.i64DefenceGuildID > 0L)
				{
					text2 = TKString.NEWString(current.szDefenceGuildName);
					if (current.bIsGuildWarDefenceGuild)
					{
						text2 = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), text2);
					}
					num2 = (int)current.byDefenceMilitaryCount;
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1325");
				}
				if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
				{
					text2 = text2 + " " + current.i64MineID.ToString();
				}
				newListItem.SetListItemData(2, text2, null, null, null);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1629"),
					"count",
					num2,
					"count2",
					9
				});
				newListItem.SetListItemData(10, text, null, null, null);
				newListItem.SetListItemData(12, string.Empty, null, null, null);
				if (current.byCurrentStatusType == 1 || current.byCurrentStatusType == 0)
				{
					if (current.i64AttackGuildID > 0L)
					{
						text = TKString.NEWString(current.szAttackGuildName);
						if (current.bIsGuildWarAttackGuild)
						{
							text = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1401"), text);
						}
						newListItem.SetListItemData(1, text, null, null, null);
						newListItem.SetListItemData(5, string.Empty, current, new EZValueChangedDelegate(this.OnClickAttackMilitaryInfo), null);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1629"),
							"count",
							current.byAttackMilitaryCount,
							"count2",
							9
						});
						newListItem.SetListItemData(9, text, null, null, null);
						newListItem.SetListItemData(11, string.Empty, null, null, null);
					}
					else
					{
						newListItem.SetListItemData(1, string.Empty, null, null, null);
						newListItem.SetListItemData(9, string.Empty, null, null, null);
						newListItem.SetListItemData(11, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1632"), null, null, null);
					}
					text = this.GetTextFromState(current);
					if (current.byMilitaryState == 2 || current.byMilitaryState == 3 || current.byMilitaryState == 6 || current.byMilitaryState == 7)
					{
						newListItem.SetListItemData(0, false);
						newListItem.SetListItemData(3, string.Empty, null, null, null);
						newListItem.SetListItemData(4, text, null, null, null);
					}
					else
					{
						if (current.byJoinMilitary == 0)
						{
							bool flag = true;
							if (current.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
							{
								flag = (current.byAttackMilitaryCount < 9);
							}
							else if (current.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
							{
								flag = (current.byDefenceMilitaryCount < 9);
							}
							newListItem.SetListItemData(0, flag);
							if (flag)
							{
								UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_NewTileBtnBlue");
								newListItem.SetListItemData(0, loader, current, new EZValueChangedDelegate(this.OnClickMilitaryBatch), null);
								newListItem.SetListItemData2(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323"));
							}
						}
						else if (current.byJoinMilitary == 1)
						{
							newListItem.SetListItemData(0, true);
							UIBaseInfoLoader loader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_BattleControl");
							newListItem.SetListItemData(0, loader2, current, new EZValueChangedDelegate(this.OnClickMilitaryReturn), null);
							newListItem.SetListItemData2(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615"));
						}
						newListItem.SetListItemData(3, text, null, null, null);
						newListItem.SetListItemData(4, string.Empty, null, null, null);
					}
				}
				else if (current.byCurrentStatusType == 2)
				{
					NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
					newListItem.SetListItemData(1, @char.GetCharName(), null, null, null);
					text = this.GetTextFromState(current);
					newListItem.SetListItemData(3, string.Empty, null, null, null);
					newListItem.SetListItemData(4, text, null, null, null);
				}
				newListItem.Data = current;
				this.m_lbCurrentStatus.Add(newListItem);
				num++;
			}
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1633"),
			"count1",
			this.m_Page,
			"count2",
			this.m_MaxPage
		});
		this.m_bxPage.SetText(text);
		this.SetListAttackGuildMark();
		this.m_lbCurrentStatus.RepositionItems();
	}

	private void SetMilitaryButtonInList(NewListItem item)
	{
		if (item == null)
		{
			return;
		}
	}

	public string GetTextFromState(MINE_GUILD_CURRENTSTATUS_INFO info)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string result = string.Empty;
		switch (info.byMilitaryState)
		{
		case 1:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525");
			break;
		case 2:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526");
			break;
		case 3:
		case 6:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1623");
			break;
		case 4:
		case 5:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1622");
			break;
		case 7:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1661");
			break;
		}
		result = text;
		if (info.byMilitaryState == 1 || info.byMilitaryState == 2)
		{
			long num = info.i64LeftTime - (long)(Time.realtimeSinceStartup - this.m_InfoSettingTime);
			if (num >= 0L)
			{
				long num2 = num / 60L / 60L % 24L;
				long num3 = num / 60L % 60L;
				long num4 = num % 60L;
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"hour",
					num2,
					"min",
					num3,
					"sec",
					num4
				});
				result = string.Format("{0} {1}", text, empty);
			}
			else if (info.byCurrentStatusType == 1 || info.byCurrentStatusType == 0)
			{
				if (info.byMilitaryState == 1)
				{
					info.byMilitaryState = 6;
				}
				else if (info.byMilitaryState == 2)
				{
					info.byMilitaryState = 7;
				}
			}
			else if (info.byCurrentStatusType == 2)
			{
				if (info.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
				{
					if (info.byMilitaryState == 1 || info.byMilitaryState == 2)
					{
						info.byMilitaryState = 7;
					}
				}
				else if (info.byMilitaryState == 1)
				{
					info.byMilitaryState = 6;
				}
				else if (info.byMilitaryState == 2)
				{
					info.byMilitaryState = 7;
				}
			}
		}
		return result;
	}

	public void UpdateTime()
	{
		long num = this.m_RemainderCountGiveTime - (long)(Time.realtimeSinceStartup - this.m_InfoSettingTime);
		if (num >= 0L)
		{
			long num2 = num / 60L / 60L % 24L;
			long num3 = num / 60L % 60L;
			long num4 = num % 60L;
			this.m_MineCountGetTime.SetText(string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4));
		}
		for (int i = 0; i < this.m_lbCurrentStatus.Count; i++)
		{
			IUIListObject item = this.m_lbCurrentStatus.GetItem(i);
			MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = item.Data as MINE_GUILD_CURRENTSTATUS_INFO;
			if (mINE_GUILD_CURRENTSTATUS_INFO != null)
			{
				if (mINE_GUILD_CURRENTSTATUS_INFO.i64LeftTime > 0L)
				{
					string text = string.Empty;
					string text2 = string.Empty;
					if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 1 || mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 0)
					{
						if (mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 2 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 3 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 6 || mINE_GUILD_CURRENTSTATUS_INFO.byMilitaryState == 7)
						{
							text = string.Empty;
							text2 = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
							UIButton uIButton = ((UIListItemContainer)item).GetElement(0) as UIButton;
							if (null != uIButton && uIButton.Visible)
							{
								uIButton.Visible = false;
							}
						}
						else
						{
							text = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
							text2 = string.Empty;
						}
					}
					else if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 2)
					{
						UIButton uIButton2 = ((UIListItemContainer)item).GetElement(0) as UIButton;
						if (null != uIButton2)
						{
							uIButton2.Visible = false;
							text = string.Empty;
							text2 = this.GetTextFromState(mINE_GUILD_CURRENTSTATUS_INFO);
						}
					}
					Label label = ((UIListItemContainer)item).GetElement(3) as Label;
					if (null != label)
					{
						label.Text = text;
					}
					Label label2 = ((UIListItemContainer)item).GetElement(4) as Label;
					if (null != label2)
					{
						label2.Text = text2;
					}
				}
			}
		}
	}

	public void SetListAttackGuildMark()
	{
		for (int i = 0; i < this.m_lbCurrentStatus.Count; i++)
		{
			IUIListObject item = this.m_lbCurrentStatus.GetItem(i);
			if (item != null)
			{
				MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = item.Data as MINE_GUILD_CURRENTSTATUS_INFO;
				if (mINE_GUILD_CURRENTSTATUS_INFO != null)
				{
					if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 1 || mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 0)
					{
						if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID > 0L)
						{
							DrawTexture drawTexture = ((UIListItemContainer)item).GetElement(8) as DrawTexture;
							if (drawTexture != null)
							{
								if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID() || !mINE_GUILD_CURRENTSTATUS_INFO.bIsAttackGuildNameHide)
								{
									string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID);
									WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebGuildImageCallback), drawTexture);
								}
								else
								{
									drawTexture.SetTexture("Win_BI_Nomark");
								}
							}
						}
					}
					else if (mINE_GUILD_CURRENTSTATUS_INFO.byCurrentStatusType == 2)
					{
						DrawTexture drawTexture2 = ((UIListItemContainer)item).GetElement(8) as DrawTexture;
						if (drawTexture2 != null)
						{
							NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
							drawTexture2.SetTexture(eCharImageType.SMALL, kMyCharInfo.GetImgFaceCharKind(), -1, string.Empty);
						}
					}
				}
			}
		}
	}

	public void RefreshList()
	{
		if (0L >= this.m_GuildID)
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(this.m_Page, (byte)this.m_Style, this.m_GuildID);
	}

	public void OnChangeMineListStyle(IUIObject obj)
	{
		eMINE_LISTVIEW_STYLE eMINE_LISTVIEW_STYLE = eMINE_LISTVIEW_STYLE.eMINE_LISTVIEW_STYLE_GUILD;
		if (this.m_dlMineListStyle.Count > 0 && this.m_dlMineListStyle.SelectedItem != null)
		{
			ListItem listItem = this.m_dlMineListStyle.SelectedItem.Data as ListItem;
			if (listItem != null)
			{
				eMINE_LISTVIEW_STYLE = (eMINE_LISTVIEW_STYLE)((int)listItem.Key);
			}
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, (byte)eMINE_LISTVIEW_STYLE, this.m_GuildID);
	}

	public void OnClickAttackMilitaryInfo(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			string message = string.Empty;
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(kMyCharInfo.m_PersonID);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			if (memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("532");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			byte nMode;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 4;
			}
			else
			{
				nMode = 5;
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bSearchMineGrade = 0;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickDefenceMilitaryInfo(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			string message = string.Empty;
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(kMyCharInfo.m_PersonID);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			if (memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("532");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			byte nMode;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 2;
			}
			else
			{
				nMode = 3;
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bSearchMineGrade = 0;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = 0L;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickMilitaryBatch(IUIObject obj)
	{
		string text = string.Empty;
		string message = string.Empty;
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
			{
				return;
			}
			if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(kMyCharInfo.m_PersonID);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			if (memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("532");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			MINE_CREATE_DATA mineCreateDataFromID = BASE_MINE_CREATE_DATA.GetMineCreateDataFromID(mINE_GUILD_CURRENTSTATUS_INFO.i16MineDataID);
			if (mineCreateDataFromID == null)
			{
				return;
			}
			MINE_DATA mineDataFromGrade = BASE_MINE_DATA.GetMineDataFromGrade(BASE_MINE_DATA.ParseGradeFromString(mineCreateDataFromID.MINE_GRADE));
			if (kMyCharInfo.GetLevel() < (int)mineDataFromGrade.POSSIBLELEVEL)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("408");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
				{
					text,
					"count",
					mineDataFromGrade.POSSIBLELEVEL
				});
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			byte nMode = 0;
			long nGuildID = 0L;
			if (mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				nMode = 2;
				nGuildID = mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID;
			}
			else if (mINE_GUILD_CURRENTSTATUS_INFO.i64AttackGuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID() || mINE_GUILD_CURRENTSTATUS_INFO.i64DefenceGuildID > 0L)
			{
				nMode = 4;
				nGuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
			}
			GS_MINE_SEARCH_REQ gS_MINE_SEARCH_REQ = new GS_MINE_SEARCH_REQ();
			gS_MINE_SEARCH_REQ.bSearchMineGrade = mineCreateDataFromID.nMine_Grade;
			gS_MINE_SEARCH_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			gS_MINE_SEARCH_REQ.m_nGuildID = nGuildID;
			gS_MINE_SEARCH_REQ.m_nMode = nMode;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_SEARCH_REQ, gS_MINE_SEARCH_REQ);
		}
	}

	public void OnClickMilitaryReturn(IUIObject obj)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = obj.Data as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO == null)
		{
			return;
		}
		if (mINE_GUILD_CURRENTSTATUS_INFO.i64MineID <= 0L)
		{
			return;
		}
		long num = 0L;
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			num = (long)instance.GetValue(eMINE_CONSTANT.eMINE_CONSTANT_MINE_RETURN_TIME);
		}
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("161");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromMessageBox2,
			"count",
			num.ToString()
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnStartBackMove), mINE_GUILD_CURRENTSTATUS_INFO, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
	}

	private void OnStartBackMove(object a_oObject)
	{
		MINE_GUILD_CURRENTSTATUS_INFO mINE_GUILD_CURRENTSTATUS_INFO = a_oObject as MINE_GUILD_CURRENTSTATUS_INFO;
		if (mINE_GUILD_CURRENTSTATUS_INFO != null)
		{
			GS_MINE_MILITARY_BACKMOVE_REQ gS_MINE_MILITARY_BACKMOVE_REQ = new GS_MINE_MILITARY_BACKMOVE_REQ();
			gS_MINE_MILITARY_BACKMOVE_REQ.m_nMineID = mINE_GUILD_CURRENTSTATUS_INFO.i64MineID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_MILITARY_BACKMOVE_REQ, gS_MINE_MILITARY_BACKMOVE_REQ);
		}
		this.RefreshList();
	}

	public void OnClickPagePrev(IUIObject obj)
	{
		if (this.m_Page <= 1)
		{
			return;
		}
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(--this.m_Page, (byte)this.m_Style, 0L);
	}

	public void OnClickPageNext(IUIObject obj)
	{
		if (this.m_Page >= this.m_MaxPage)
		{
			return;
		}
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			return;
		}
		NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(++this.m_Page, (byte)this.m_Style, 0L);
	}

	public void OnClickSearchMine(IUIObject obj)
	{
		MineSearchDlg mineSearchDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg;
		if (mineSearchDlg != null)
		{
			mineSearchDlg.Show();
		}
	}

	public void OnClickMineBattleRecord(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_RECORD_DLG);
	}

	public void OnClickGuildWar(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_INFO_REQ();
	}

	private void ReqWebGuildImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = _param as DrawTexture;
		if (drawTexture == null)
		{
			return;
		}
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}
}
