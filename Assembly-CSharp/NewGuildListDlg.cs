using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class NewGuildListDlg : Form
{
	public enum eSEARCHTYPE
	{
		eSEARCHTYPE_DEFAULT,
		eSEARCHTYPE_GUILDNAME,
		eSEARCHTYPE_MASTERNAME
	}

	private const int REFRESH_TIME_1 = 300;

	private const int REFRESH_TIME_2 = 2000;

	private Button m_btBack;

	private Button m_btReset;

	private DropDownList m_dlSearch;

	private Label m_lbDefaultText;

	private TextField m_tfSearchKeyword;

	private Button m_btSearch;

	private NewListBox m_nlbGuildList;

	private Button m_btPrev;

	private Box m_bxPage;

	private Button m_btNext;

	private Button m_btSortRank;

	private Button m_btSortLevel;

	private Button m_btWarList;

	private int m_iCurPageNum;

	private int m_iMaxPageNum = 1;

	private string m_strPageNum = string.Empty;

	private string m_strName = string.Empty;

	private string m_strMasterName = string.Empty;

	private string m_strMemberCnt = string.Empty;

	private string m_strText = string.Empty;

	private NewGuildListDlg.eSEARCHTYPE m_eSearchType;

	private NewGuildDefine.eNEWGUILD_SORT m_eNewGuildSort;

	private bool m_bSortRank = true;

	private bool m_bSortLevel = true;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_NewGuild_GuildList", G_ID.NEWGUILD_LIST_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_btBack = (base.GetControl("Button_Back") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickBack));
		this.m_btReset = (base.GetControl("Button_Reset") as Button);
		this.m_btReset.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickReset));
		this.m_dlSearch = (base.GetControl("DropDownList_Search") as DropDownList);
		this.m_dlSearch.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1790"), NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_GUILDNAME);
		this.m_dlSearch.AddItem(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1684"), NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_MASTERNAME);
		this.m_dlSearch.SetViewArea(this.m_dlSearch.Count);
		this.m_dlSearch.RepositionItems();
		this.m_dlSearch.SetFirstItem();
		this.m_dlSearch.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeSearchType));
		this.m_lbDefaultText = (base.GetControl("Label_KeywordDefaultText") as Label);
		this.m_tfSearchKeyword = (base.GetControl("TextField_Keyword") as TextField);
		this.m_tfSearchKeyword.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSearchKeyword));
		this.m_tfSearchKeyword.Clear();
		this.m_tfSearchKeyword.maxLength = 20;
		this.m_tfSearchKeyword.SetText(string.Empty);
		this.m_btSearch = (base.GetControl("Button_Search") as Button);
		this.m_btSearch.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSearch));
		this.m_nlbGuildList = (base.GetControl("NLB_GuildList") as NewListBox);
		this.m_nlbGuildList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickGuildInfo));
		this.m_nlbGuildList.touchScroll = false;
		this.m_btPrev = (base.GetControl("Button_Pre") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_bxPage = (base.GetControl("Box_Box19") as Box);
		this.m_btNext = (base.GetControl("Button_Next") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
		}
		this.m_btSortRank = (base.GetControl("Button_RankSorting") as Button);
		this.m_btSortRank.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSortRank));
		this.m_btSortLevel = (base.GetControl("Button_NameSorting") as Button);
		this.m_btSortLevel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSortLevel));
		this.m_btWarList = (base.GetControl("Button_WarList") as Button);
		this.m_btWarList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickWarList));
		this.m_btWarList.Hide(true);
		this.m_strMemberCnt = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1808");
		this.m_bSortRank = !this.m_bSortRank;
		this.Send_GuildList(this.m_iCurPageNum, this.m_eNewGuildSort);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD_LIST", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD_LIST", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.OnClose();
	}

	public void ClickPrev(IUIObject obj)
	{
		int num = this.m_iCurPageNum;
		if (-1 <= num - 1)
		{
			num--;
		}
		this.Send_GuildList(num, this.m_eNewGuildSort);
	}

	public void ClickNext(IUIObject obj)
	{
		int num = this.m_iCurPageNum;
		num++;
		this.Send_GuildList(num, this.m_eNewGuildSort);
	}

	public void Send_GuildList(int iCurPageNum, NewGuildDefine.eNEWGUILD_SORT eSort)
	{
		this.m_eSearchType = this.GetSearchType();
		this.m_eNewGuildSort = eSort;
		if (!this.IsSendGuildList())
		{
			return;
		}
		GS_NEWGUILD_LIST_REQ gS_NEWGUILD_LIST_REQ = new GS_NEWGUILD_LIST_REQ();
		gS_NEWGUILD_LIST_REQ.i16CurPageNum = (short)iCurPageNum;
		switch (this.m_eSearchType)
		{
		case NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_GUILDNAME:
			TKString.StringChar(this.m_tfSearchKeyword.GetText(), ref gS_NEWGUILD_LIST_REQ.strGuildName);
			break;
		case NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_MASTERNAME:
			TKString.StringChar(this.m_tfSearchKeyword.GetText(), ref gS_NEWGUILD_LIST_REQ.strMasterName);
			break;
		}
		gS_NEWGUILD_LIST_REQ.i8SortType = (byte)eSort;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_LIST_REQ, gS_NEWGUILD_LIST_REQ);
		this.SetEnableControl(false);
	}

	public void ClickGuildCreate(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_CREATE_DLG);
		base.CloseNow();
	}

	public void ClickSortRank(IUIObject obj)
	{
		this.m_bSortRank = !this.m_bSortRank;
		if (!this.m_bSortRank)
		{
			this.Send_GuildList(0, NewGuildDefine.eNEWGUILD_SORT.eNEWGUILD_SORT_RANK_MIN);
		}
		else
		{
			this.Send_GuildList(0, NewGuildDefine.eNEWGUILD_SORT.eNEWGUILD_SORT_RANK_MAX);
		}
	}

	public void ClickSortLevel(IUIObject obj)
	{
		this.m_bSortLevel = !this.m_bSortLevel;
		if (!this.m_bSortLevel)
		{
			this.Send_GuildList(0, NewGuildDefine.eNEWGUILD_SORT.eNEWGUILD_SORT_LEVEL_MIN);
		}
		else
		{
			this.Send_GuildList(0, NewGuildDefine.eNEWGUILD_SORT.eNEWGUILD_SORT_LEVEL_MAX);
		}
	}

	public void ClickWarList(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DECLAREWAR_GUILDLIST_DLG);
	}

	public void ClickGuildInfo(IUIObject obj)
	{
		IUIListObject selectItem = this.m_nlbGuildList.GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
		gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = (long)selectItem.Data;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
		this.SetEnableControl(false);
	}

	public void SetGuildList(GS_NEWGUILD_LIST_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_nlbGuildList.Clear();
		string text = string.Empty;
		for (int i = 0; i < (int)ACK.i16GuildListNum; i++)
		{
			NEWGUILD_LIST_INFO packet = kDeserializePacket.GetPacket<NEWGUILD_LIST_INFO>();
			NewListItem newListItem = new NewListItem(this.m_nlbGuildList.ColumnNum, true);
			this.m_strName = TKString.NEWString(packet.strGuildName);
			this.m_strMasterName = TKString.NEWString(packet.strMasterName);
			text = packet.i16Rank.ToString();
			if (0 >= packet.i16Rank)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			newListItem.SetListItemData(0, text, null, null, null);
			newListItem.SetListItemData(1, this.m_strName, null, null, null);
			newListItem.SetListItemData(2, ANNUALIZED.Convert(packet.i32Point), null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				this.m_strMemberCnt,
				"count1",
				packet.i16CurGuildNum,
				"count2",
				packet.i16MaxGuildNum
			});
			newListItem.SetListItemData(3, this.m_strText, null, null, null);
			newListItem.SetListItemData(4, this.m_strMasterName, null, null, null);
			switch (packet.i16Rank)
			{
			case 1:
				newListItem.SetListItemData(5, "Win_I_Rank03", null, null, null);
				break;
			case 2:
				newListItem.SetListItemData(5, "Win_I_Rank02", null, null, null);
				break;
			case 3:
				newListItem.SetListItemData(5, "Win_I_Rank01", null, null, null);
				break;
			default:
				newListItem.SetListItemData(5, false);
				break;
			}
			newListItem.SetListItemData(6, packet.i16Level.ToString(), null, null, null);
			string text2 = string.Empty;
			if (packet.i16AgitLevel == 0)
			{
				this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2774");
			}
			else
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1436");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
				{
					text2,
					"level",
					packet.i16AgitLevel
				});
			}
			newListItem.SetListItemData(7, this.m_strText, null, null, null);
			newListItem.Data = packet.i64GuildID;
			this.m_nlbGuildList.Add(newListItem);
		}
		this.m_nlbGuildList.RepositionItems();
		this.m_iCurPageNum = (int)ACK.i16CurPageNum;
		this.m_iMaxPageNum = (int)ACK.i16MaxPageNum;
		this.SetPageText();
	}

	public void SetPageText()
	{
		this.m_strPageNum = string.Format("{0}/{1}", (this.m_iCurPageNum + 1).ToString(), this.m_iMaxPageNum.ToString());
		this.m_bxPage.SetText(this.m_strPageNum);
	}

	public void ClickBack(IUIObject obj)
	{
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MAINSELECT_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MEMBER_DLG);
			NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
		}
		base.CloseNow();
	}

	public void ClickReset(IUIObject obj)
	{
		this.m_tfSearchKeyword.Clear();
		this.m_tfSearchKeyword.SetText(string.Empty);
		this.m_eSearchType = NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT;
		this.m_lbDefaultText.Hide(false);
		this.Send_GuildList(0, this.m_eNewGuildSort);
	}

	public void ClickSearch(IUIObject obj)
	{
		this.Send_GuildList(0, this.m_eNewGuildSort);
	}

	public void ClickSearchKeyword(IUIObject obj)
	{
		this.m_lbDefaultText.Hide(true);
	}

	public void OnChangeSearchType(IUIObject obj)
	{
		if (null == this.m_dlSearch.SelectedItem)
		{
			return;
		}
		ListItem listItem = this.m_dlSearch.SelectedItem.Data as ListItem;
		if (listItem == null)
		{
			return;
		}
		this.m_eSearchType = (NewGuildListDlg.eSEARCHTYPE)((int)listItem.Key);
		this.m_tfSearchKeyword.Clear();
	}

	public bool IsSendGuildList()
	{
		switch (this.m_eSearchType)
		{
		case NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_GUILDNAME:
		{
			string text = this.m_tfSearchKeyword.GetText();
			if (string.Empty == text)
			{
				return false;
			}
			if (10 <= text.Length)
			{
				return false;
			}
			break;
		}
		case NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_MASTERNAME:
		{
			string text2 = this.m_tfSearchKeyword.GetText();
			if (string.Empty == text2)
			{
				return false;
			}
			if (10 <= text2.Length)
			{
				return false;
			}
			break;
		}
		}
		return true;
	}

	public void SetEnableControl(bool bEnable)
	{
		if (this.m_eSearchType != NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT)
		{
			this.m_btReset.SetEnabled(bEnable);
			this.m_btSearch.SetEnabled(bEnable);
		}
		this.m_btBack.SetEnabled(bEnable);
		this.m_btPrev.SetEnabled(bEnable);
		this.m_btNext.SetEnabled(bEnable);
		this.m_btSortRank.SetEnabled(bEnable);
		this.m_btSortLevel.SetEnabled(bEnable);
		if (!bEnable)
		{
			if (this.m_eSearchType == NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT)
			{
				CallBackScheduler.Instance.RegFunc(300L, new Action(NewGuildListDlg.AutoEnableControl));
			}
			else
			{
				CallBackScheduler.Instance.RegFunc(2000L, new Action(NewGuildListDlg.AutoEnableControl));
			}
		}
	}

	public static void AutoEnableControl()
	{
		NewGuildListDlg newGuildListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_LIST_DLG) as NewGuildListDlg;
		if (newGuildListDlg != null)
		{
			newGuildListDlg.SetEnableControl(true);
		}
	}

	public NewGuildListDlg.eSEARCHTYPE GetSearchType()
	{
		string text = this.m_tfSearchKeyword.GetText();
		if (string.Empty == text)
		{
			return NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT;
		}
		if (null == this.m_dlSearch.SelectedItem)
		{
			return NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT;
		}
		ListItem listItem = this.m_dlSearch.SelectedItem.Data as ListItem;
		if (listItem == null)
		{
			return NewGuildListDlg.eSEARCHTYPE.eSEARCHTYPE_DEFAULT;
		}
		return (NewGuildListDlg.eSEARCHTYPE)((int)listItem.Key);
	}
}
