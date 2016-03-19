using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class StoryChatDlg : Form
{
	private static int MAX_STORYCHAT_LIST_NUM = 4;

	private Toolbar m_Toolbar;

	private NewListBox m_StoryChatList;

	private Button m_Write;

	private Box m_Page;

	private Button m_Prev;

	private Button m_Next;

	private Button m_Refresh;

	private int m_nCurrentPage = 1;

	private int m_nCurrentTabInex;

	private List<StoryChatPortrait> m_UserPortraitList = new List<StoryChatPortrait>();

	private int m_nOldTabIndex;

	private bool m_bBattleReplay;

	private StoryChat_Info[] m_CurrentStoryChatInfo = new StoryChat_Info[StoryChatDlg.MAX_STORYCHAT_LIST_NUM];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "StoryChat/DLG_StoryChat", G_ID.STORYCHAT_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_Toolbar = (base.GetControl("ToolBar") as Toolbar);
		this.m_Toolbar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("296");
		UIPanelTab expr_44 = this.m_Toolbar.Control_Tab[0];
		expr_44.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_44.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_Toolbar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1335");
		UIPanelTab expr_93 = this.m_Toolbar.Control_Tab[1];
		expr_93.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_93.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			this.m_Toolbar.Control_Tab[1].controlIsEnabled = false;
		}
		this.m_Toolbar.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("297");
		UIPanelTab expr_106 = this.m_Toolbar.Control_Tab[2];
		expr_106.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_106.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_Toolbar.Control_Tab[3].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1955");
		UIPanelTab expr_155 = this.m_Toolbar.Control_Tab[3];
		expr_155.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_155.ButtonClick, new EZValueChangedDelegate(this.ClickToolbar));
		this.m_Write = (base.GetControl("BT_writing") as Button);
		this.m_Write.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickWrite));
		this.m_Prev = (base.GetControl("BT_Back") as Button);
		this.m_Prev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_Next = (base.GetControl("BT_Next") as Button);
		this.m_Next.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_Refresh = (base.GetControl("BT_Refresh") as Button);
		this.m_Refresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRefresh));
		this.m_Page = (base.GetControl("Box_Page") as Box);
		this.m_Page.Text = this.m_nCurrentPage.ToString();
		this.m_StoryChatList = (base.GetControl("NLB_Talk") as NewListBox);
		this.m_StoryChatList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStoryChatList));
		this.m_StoryChatList.touchScroll = false;
		this.m_StoryChatList.Reserve = false;
		for (int i = 0; i < StoryChatDlg.MAX_STORYCHAT_LIST_NUM; i++)
		{
			this.m_CurrentStoryChatInfo[i] = new StoryChat_Info();
		}
		GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
		gS_STORYCHAT_GET_REQ.nPersonID = 0L;
		gS_STORYCHAT_GET_REQ.nType = 0;
		gS_STORYCHAT_GET_REQ.nPage = 1;
		gS_STORYCHAT_GET_REQ.nPageSize = StoryChatDlg.MAX_STORYCHAT_LIST_NUM;
		gS_STORYCHAT_GET_REQ.nFirstStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.nLastStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.bNextRequest = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
		base.SetScreenCenter();
	}

	public void SelectToolbar(int index)
	{
		this.m_Toolbar.SetSelectTabIndex(index);
	}

	private void ClickToolbar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		this.m_nOldTabIndex = this.m_nCurrentTabInex;
		this.m_nCurrentPage = 1;
		this.m_nCurrentTabInex = uIPanelTab.panel.index;
		if (this.m_nCurrentTabInex == 0 || this.m_nCurrentTabInex == 1)
		{
			this.m_Write.Visible = true;
		}
		else
		{
			this.m_Write.Visible = false;
		}
		if (this.m_nCurrentTabInex == 0 || this.m_nCurrentTabInex == 1 || this.m_nCurrentTabInex == 2)
		{
			this.m_bBattleReplay = false;
		}
		else if (this.m_nCurrentTabInex == 3)
		{
			this.m_bBattleReplay = true;
		}
		GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
		gS_STORYCHAT_GET_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
		gS_STORYCHAT_GET_REQ.nType = (byte)uIPanelTab.panel.index;
		gS_STORYCHAT_GET_REQ.nPage = 1;
		gS_STORYCHAT_GET_REQ.nPageSize = StoryChatDlg.MAX_STORYCHAT_LIST_NUM;
		gS_STORYCHAT_GET_REQ.nFirstStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.nLastStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.bNextRequest = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
	}

	private void refreshFriendPortrait()
	{
		this.m_UserPortraitList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			StoryChatPortrait storyChatPortrait = new StoryChatPortrait();
			storyChatPortrait.Set(uSER_FRIEND_INFO.nPersonID, false);
			this.m_UserPortraitList.Add(storyChatPortrait);
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		StoryChatPortrait storyChatPortrait2 = new StoryChatPortrait();
		storyChatPortrait2.Set(myCharInfo.m_PersonID, false);
		this.m_UserPortraitList.Add(storyChatPortrait2);
	}

	public StoryChatPortrait GetCommunity_User(long _PersonID)
	{
		StoryChatPortrait result = null;
		foreach (StoryChatPortrait current in this.m_UserPortraitList)
		{
			if (current.m_i64PersonID == _PersonID)
			{
				result = current;
				break;
			}
		}
		return result;
	}

	public Texture2D GetFriendPortraitPersonID(long i64PersonID)
	{
		Texture2D result = null;
		foreach (StoryChatPortrait current in this.m_UserPortraitList)
		{
			if (current.m_i64PersonID == i64PersonID)
			{
				result = current.m_PortraitTexutre;
				break;
			}
		}
		return result;
	}

	private void ClickPrev(IUIObject obj)
	{
		if (this.m_nCurrentPage <= 1)
		{
			return;
		}
		if (0 < this.m_StoryChatList.Count)
		{
			long nStoryChatID = this.m_CurrentStoryChatInfo[0].nStoryChatID;
			long nStoryChatID2 = this.m_CurrentStoryChatInfo[this.m_StoryChatList.Count - 1].nStoryChatID;
			GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
			gS_STORYCHAT_GET_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
			gS_STORYCHAT_GET_REQ.nType = (byte)this.m_nCurrentTabInex;
			gS_STORYCHAT_GET_REQ.nPage = this.m_nCurrentPage;
			gS_STORYCHAT_GET_REQ.nPageSize = StoryChatDlg.MAX_STORYCHAT_LIST_NUM;
			gS_STORYCHAT_GET_REQ.nFirstStoryChatID = nStoryChatID;
			gS_STORYCHAT_GET_REQ.nLastStoryChatID = nStoryChatID2;
			gS_STORYCHAT_GET_REQ.bNextRequest = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
			this.m_nCurrentPage--;
			this.m_Next.controlIsEnabled = true;
		}
	}

	private void ClickNext(IUIObject obj)
	{
		if (0 < this.m_StoryChatList.Count && StoryChatDlg.MAX_STORYCHAT_LIST_NUM == this.m_StoryChatList.Count)
		{
			this.m_nCurrentPage++;
			long nStoryChatID = this.m_CurrentStoryChatInfo[0].nStoryChatID;
			long nStoryChatID2 = this.m_CurrentStoryChatInfo[this.m_StoryChatList.Count - 1].nStoryChatID;
			GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
			gS_STORYCHAT_GET_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
			gS_STORYCHAT_GET_REQ.nType = (byte)this.m_nCurrentTabInex;
			gS_STORYCHAT_GET_REQ.nPage = this.m_nCurrentPage;
			gS_STORYCHAT_GET_REQ.nPageSize = 4;
			gS_STORYCHAT_GET_REQ.nFirstStoryChatID = nStoryChatID;
			gS_STORYCHAT_GET_REQ.nLastStoryChatID = nStoryChatID2;
			gS_STORYCHAT_GET_REQ.bNextRequest = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
			this.m_Prev.controlIsEnabled = true;
		}
	}

	private void ClickRefresh(IUIObject obj)
	{
		GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
		gS_STORYCHAT_GET_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
		gS_STORYCHAT_GET_REQ.nType = (byte)this.m_nCurrentTabInex;
		gS_STORYCHAT_GET_REQ.nPage = 1;
		gS_STORYCHAT_GET_REQ.nPageSize = 4;
		gS_STORYCHAT_GET_REQ.nFirstStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.nLastStoryChatID = 0L;
		gS_STORYCHAT_GET_REQ.bNextRequest = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
		this.m_nCurrentPage = 1;
	}

	private void ClickWrite(IUIObject obj)
	{
		StoryChatSetDlg storyChatSetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.STORYCHAT_SET_DLG) as StoryChatSetDlg;
		if (storyChatSetDlg != null)
		{
			storyChatSetDlg.SelectStory(this.m_nCurrentTabInex);
		}
	}

	private void ClickStoryChatList(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (null == this.m_StoryChatList.SelectedItem)
		{
			return;
		}
		StoryChat_Info storyChat_Info = this.m_CurrentStoryChatInfo[this.m_StoryChatList.SelectedItem.GetIndex()];
		if (storyChat_Info != null)
		{
			StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
			if (storyChatDetailDlg != null)
			{
				storyChatDetailDlg.SetStoryChat(storyChat_Info, this.m_bBattleReplay);
			}
		}
	}

	public void InitStoryChatList(int type)
	{
		if (type == this.m_nOldTabIndex)
		{
			if (this.m_nCurrentPage != 1)
			{
				this.m_nCurrentPage--;
				if (this.m_nCurrentPage == 1)
				{
					this.m_Prev.controlIsEnabled = false;
				}
				this.m_Next.controlIsEnabled = false;
			}
			else
			{
				this.m_StoryChatList.Clear();
			}
		}
		else
		{
			this.m_nCurrentPage = 1;
			this.m_Page.Text = this.m_nCurrentPage.ToString();
			this.m_Prev.controlIsEnabled = false;
			this.m_Next.controlIsEnabled = false;
			this.m_StoryChatList.Clear();
		}
	}

	public void SetBattleStoryChatList(StoryChat_Info[] array)
	{
		this.m_nOldTabIndex = this.m_nCurrentTabInex;
		this.m_StoryChatList.SetColumnData("Mobile/DLG/StoryChat/NLB_Scene01_ColumnData" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		for (int i = 0; i < StoryChatDlg.MAX_STORYCHAT_LIST_NUM; i++)
		{
			this.m_CurrentStoryChatInfo[i].Init();
		}
		for (int j = 0; j < array.Length; j++)
		{
			this.m_CurrentStoryChatInfo[j] = array[j];
		}
		if (array.Length != StoryChatDlg.MAX_STORYCHAT_LIST_NUM)
		{
			this.m_Next.controlIsEnabled = false;
		}
		else
		{
			this.m_Next.controlIsEnabled = true;
		}
		if (this.m_nCurrentPage == 1)
		{
			this.m_Prev.controlIsEnabled = false;
		}
		this.m_StoryChatList.Clear();
		string key = "ReplayStoryChatID";
		string s = string.Empty;
		long num = 0L;
		if (PlayerPrefs.HasKey(key))
		{
			s = PlayerPrefs.GetString(key);
			num = long.Parse(s);
		}
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k].nCharKind == 8 || array[k].nCharKind == 6 || array[k].nCharKind == 5)
			{
				NewListItem newListItem = new NewListItem(this.m_StoryChatList.ColumnNum, true);
				char[] separator = new char[]
				{
					'/'
				};
				string[] array2 = TKString.NEWString(array[k].szMessage).Split(separator);
				if (array[k].nCharKind == 8)
				{
					newListItem.SetListItemData(0, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
					newListItem.SetListItemData(1, NrTSingleton<CTextParser>.Instance.GetTextColor("1105") + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1307"), null, null, null);
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1956"),
						"targetname1",
						array2[1],
						"targetname2",
						array2[2]
					});
					newListItem.SetListItemData(9, empty, null, null, null);
					newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1513"), long.Parse(array2[0]), new EZValueChangedDelegate(this.ClickReplay1), null);
				}
				else if (array[k].nCharKind == 6)
				{
					newListItem.SetListItemData(0, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
					newListItem.SetListItemData(1, NrTSingleton<CTextParser>.Instance.GetTextColor("1105") + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("436"), null, null, null);
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1957"),
						"targetname1",
						array2[1],
						"targetname2",
						array2[2]
					});
					newListItem.SetListItemData(9, empty2, null, null, null);
					newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1513"), long.Parse(array2[0]), new EZValueChangedDelegate(this.ClickReplay2), null);
				}
				else if (array[k].nCharKind == 5)
				{
					newListItem.SetListItemData(0, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
					newListItem.SetListItemData(1, NrTSingleton<CTextParser>.Instance.GetTextColor("1105") + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("43"), null, null, null);
					string empty3 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1957"),
						"targetname1",
						array2[1],
						"targetname2",
						array2[2]
					});
					newListItem.SetListItemData(9, empty3, null, null, null);
					newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1513"), long.Parse(array2[0]), new EZValueChangedDelegate(this.ClickReplay3), null);
				}
				DateTime dueDate = PublicMethod.GetDueDate(array[k].nTime);
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
				string empty4 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
				{
					textFromInterface,
					"month",
					dueDate.Month,
					"day",
					dueDate.Day,
					"hour",
					dueDate.Hour,
					"min",
					dueDate.Minute
				});
				newListItem.SetListItemData(2, empty4, null, null, null);
				newListItem.SetListItemData(6, array[k].nCommentCount.ToString(), null, null, null);
				newListItem.SetListItemData(7, array[k].nLikeCount.ToString(), null, null, null);
				if (array[k].nStoryChatID > num)
				{
					newListItem.SetListItemData(10, true);
				}
				else
				{
					newListItem.SetListItemData(10, false);
				}
				this.m_StoryChatList.Add(newListItem);
			}
		}
		this.m_StoryChatList.RepositionItems();
		this.m_Page.Text = this.m_nCurrentPage.ToString();
	}

	private void ClickReplay1(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long nLegionActionID = (long)obj.Data;
		NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayMineHttp(nLegionActionID);
	}

	private void ClickReplay2(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long colossumID = (long)obj.Data;
		NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayColosseumHttp(colossumID);
	}

	private void ClickReplay3(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long nPlunderID = (long)obj.Data;
		NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayHttp(nPlunderID);
	}

	public void SetStoryChatList(int type, StoryChat_Info[] array)
	{
		this.m_nOldTabIndex = this.m_nCurrentTabInex;
		this.m_StoryChatList.SetColumnData("Mobile/DLG/StoryChat/NLB_Talk_ColumnData" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		for (int i = 0; i < StoryChatDlg.MAX_STORYCHAT_LIST_NUM; i++)
		{
			this.m_CurrentStoryChatInfo[i].Init();
		}
		for (int j = 0; j < array.Length; j++)
		{
			this.m_CurrentStoryChatInfo[j] = array[j];
		}
		this.refreshFriendPortrait();
		if (array.Length != StoryChatDlg.MAX_STORYCHAT_LIST_NUM)
		{
			this.m_Next.controlIsEnabled = false;
		}
		else
		{
			this.m_Next.controlIsEnabled = true;
		}
		if (this.m_nCurrentPage == 1)
		{
			this.m_Prev.controlIsEnabled = false;
		}
		this.m_StoryChatList.Clear();
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		string charName = charPersonInfo.GetCharName();
		string s = string.Empty;
		long num = 0L;
		if (type == 0)
		{
			string key = charName + "NewStoryChatID";
			if (PlayerPrefs.HasKey(key))
			{
				s = PlayerPrefs.GetString(key);
				num = long.Parse(s);
			}
		}
		else if (type == 1)
		{
			string key2 = charName + NrTSingleton<NewGuildManager>.Instance.GetGuildName();
			if (PlayerPrefs.HasKey(key2))
			{
				s = PlayerPrefs.GetString(key2);
				num = long.Parse(s);
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			NewListItem newListItem = new NewListItem(this.m_StoryChatList.ColumnNum, true);
			StoryChatPortrait community_User = this.GetCommunity_User(array[k].nPersonID);
			if (community_User != null)
			{
				Texture2D friendPortraitPersonID = this.GetFriendPortraitPersonID(array[k].nPersonID);
				if (friendPortraitPersonID != null)
				{
					newListItem.SetListItemData(1, friendPortraitPersonID, null, null, null, null);
				}
				else
				{
					EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(array[k].nCharKind);
					if (eventHeroCharFriendCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.EventMark = true;
					}
					newListItem.SetListItemData(1, array[k].nCharKind, null, null, null);
				}
			}
			else
			{
				StoryChatPortrait storyChatPortrait = new StoryChatPortrait();
				storyChatPortrait.Set(array[k].nPersonID, true);
				this.m_UserPortraitList.Add(storyChatPortrait);
			}
			string text = TKString.NEWString(array[k].szName);
			if (0L < array[k].nGuildID)
			{
				newListItem.SetListItemData(2, NrTSingleton<CTextParser>.Instance.GetTextColor("1105") + text, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(2, text, null, null, null);
			}
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152") + array[k].nLevel.ToString(), null, null, null);
			DateTime dueDate = PublicMethod.GetDueDate(array[k].nTime);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute
			});
			newListItem.SetListItemData(4, empty, null, null, null);
			string text2 = TKString.NEWString(array[k].szMessage);
			text2 = text2.Replace("\n", " ");
			if (100 < text2.Length)
			{
				text2 = text2.Substring(0, 100);
				text2 += "...";
			}
			newListItem.SetListItemData(6, text2, null, null, null);
			newListItem.SetListItemData(9, array[k].nCommentCount.ToString(), null, null, null);
			newListItem.SetListItemData(10, array[k].nLikeCount.ToString(), null, null, null);
			if (type == 0 || type == 1)
			{
				if (array[k].nStoryChatID > num)
				{
					newListItem.SetListItemData(12, true);
				}
				else
				{
					newListItem.SetListItemData(12, false);
				}
			}
			else
			{
				newListItem.SetListItemData(12, false);
			}
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				if (text == @char.GetCharName())
				{
					newListItem.SetListItemData(14, string.Empty, array[k].nStoryChatID, new EZValueChangedDelegate(this.DeleteStoryChat), null);
				}
				else
				{
					newListItem.SetListItemData(14, false);
				}
			}
			this.m_StoryChatList.Add(newListItem);
		}
		this.m_StoryChatList.RepositionItems();
		this.m_Page.Text = this.m_nCurrentPage.ToString();
	}

	public void DeleteStoryChat(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long num = (long)obj.Data;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.RequestDeleteStoryChat), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("75"), eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
	}

	private void RequestDeleteStoryChat(object obj)
	{
		if (obj == null)
		{
			return;
		}
		long nStoryChatID = (long)obj;
		GS_STORYCHAT_SET_REQ gS_STORYCHAT_SET_REQ = new GS_STORYCHAT_SET_REQ();
		gS_STORYCHAT_SET_REQ.m_nStoryChatID = nStoryChatID;
		TKString.StringChar(string.Empty, ref gS_STORYCHAT_SET_REQ.szMessage);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_SET_REQ, gS_STORYCHAT_SET_REQ);
		this.m_nCurrentPage = 1;
		this.m_Page.Text = this.m_nCurrentPage.ToString();
	}

	public void UpdateCommentNumText(bool flag)
	{
		if (null == this.m_StoryChatList.SelectedItem)
		{
			return;
		}
		StoryChat_Info storyChat_Info = this.m_CurrentStoryChatInfo[this.m_StoryChatList.SelectedItem.GetIndex()];
		if (storyChat_Info == null)
		{
			return;
		}
		Label label = this.m_StoryChatList.SelectedItem.GetElement(9) as Label;
		if (null != label)
		{
			if (flag)
			{
				StoryChat_Info expr_61 = storyChat_Info;
				expr_61.nCommentCount += 1;
			}
			else
			{
				StoryChat_Info expr_75 = storyChat_Info;
				expr_75.nCommentCount -= 1;
			}
			label.Text = storyChat_Info.nCommentCount.ToString();
		}
		else
		{
			Label label2 = this.m_StoryChatList.SelectedItem.GetElement(6) as Label;
			if (null != label2)
			{
				if (flag)
				{
					StoryChat_Info expr_C3 = storyChat_Info;
					expr_C3.nCommentCount += 1;
				}
				else
				{
					StoryChat_Info expr_D7 = storyChat_Info;
					expr_D7.nCommentCount -= 1;
				}
				label2.Text = storyChat_Info.nCommentCount.ToString();
			}
		}
	}

	public void UpdateLikeNumText()
	{
		if (null == this.m_StoryChatList.SelectedItem)
		{
			return;
		}
		StoryChat_Info storyChat_Info = this.m_CurrentStoryChatInfo[this.m_StoryChatList.SelectedItem.GetIndex()];
		if (storyChat_Info == null)
		{
			return;
		}
		Label label = this.m_StoryChatList.SelectedItem.GetElement(10) as Label;
		if (null != label)
		{
			StoryChat_Info expr_5B = storyChat_Info;
			expr_5B.nLikeCount += 1;
			label.Text = storyChat_Info.nLikeCount.ToString();
		}
		else
		{
			Label label2 = this.m_StoryChatList.SelectedItem.GetElement(7) as Label;
			if (null != label2)
			{
				StoryChat_Info expr_A3 = storyChat_Info;
				expr_A3.nLikeCount += 1;
				label2.Text = storyChat_Info.nLikeCount.ToString();
			}
		}
	}

	public void UpdateUserPersonID(long i64PersonID)
	{
		for (int i = 0; i < this.m_StoryChatList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.m_StoryChatList.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				StoryChat_Info storyChat_Info = (StoryChat_Info)uIListItemContainer.data;
				if (storyChat_Info != null)
				{
					if (storyChat_Info.nPersonID == i64PersonID)
					{
						NewListItem newListItem = new NewListItem(this.m_StoryChatList.ColumnNum, true);
						Texture2D friendPortraitPersonID = this.GetFriendPortraitPersonID(storyChat_Info.nPersonID);
						if (friendPortraitPersonID != null)
						{
							newListItem.SetListItemData(1, friendPortraitPersonID, null, null, null, null);
						}
						else
						{
							EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(storyChat_Info.nCharKind);
							if (eventHeroCharFriendCode != null)
							{
								newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
								newListItem.EventMark = true;
							}
							newListItem.SetListItemData(1, storyChat_Info.nCharKind, null, null, null);
						}
						string text = TKString.NEWString(storyChat_Info.szName);
						if (0L < storyChat_Info.nGuildID)
						{
							newListItem.SetListItemData(2, NrTSingleton<CTextParser>.Instance.GetTextColor("1105") + text, null, null, null);
						}
						else
						{
							newListItem.SetListItemData(2, text, null, null, null);
						}
						newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152") + storyChat_Info.nLevel.ToString(), null, null, null);
						DateTime dueDate = PublicMethod.GetDueDate(storyChat_Info.nTime);
						string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromInterface,
							"month",
							dueDate.Month,
							"day",
							dueDate.Day,
							"hour",
							dueDate.Hour,
							"min",
							dueDate.Minute
						});
						newListItem.SetListItemData(4, empty, null, null, null);
						string text2 = TKString.NEWString(storyChat_Info.szMessage);
						text2 = text2.Replace("\n", " ");
						if (100 < text2.Length)
						{
							text2 = text2.Substring(0, 100);
							text2 += "...";
						}
						newListItem.SetListItemData(6, text2, null, null, null);
						newListItem.SetListItemData(9, storyChat_Info.nCommentCount.ToString(), null, null, null);
						newListItem.SetListItemData(10, storyChat_Info.nLikeCount.ToString(), null, null, null);
						newListItem.SetListItemData(12, false);
						NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
						if (@char != null)
						{
							if (text == @char.GetCharName())
							{
								newListItem.SetListItemData(14, string.Empty, storyChat_Info.nStoryChatID, new EZValueChangedDelegate(this.DeleteStoryChat), null);
							}
							else
							{
								newListItem.SetListItemData(14, false);
							}
						}
						this.m_StoryChatList.RemoveAdd(i, newListItem);
					}
				}
			}
		}
		this.m_StoryChatList.RepositionItems();
	}

	public void UpdateUserCommunity(long _i64PersonID)
	{
		bool flag = false;
		for (int i = 0; i < this.m_UserPortraitList.Count; i++)
		{
			if (this.m_UserPortraitList[i].m_i64PersonID == _i64PersonID)
			{
				this.m_UserPortraitList[i].Set(_i64PersonID, true);
				flag = true;
			}
		}
		if (!flag)
		{
			StoryChatPortrait storyChatPortrait = new StoryChatPortrait();
			storyChatPortrait.Set(_i64PersonID, true);
			this.m_UserPortraitList.Add(storyChatPortrait);
		}
	}

	public List<StoryChatPortrait> GetStoryChatPortraitList()
	{
		return this.m_UserPortraitList;
	}
}
