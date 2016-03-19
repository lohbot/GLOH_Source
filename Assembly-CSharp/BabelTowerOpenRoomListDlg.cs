using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class BabelTowerOpenRoomListDlg : Form
{
	private const int DEFAULT_PAGE = 1;

	private const short DROPDOWN_SHOWFLOOR_COUNT = 10;

	private const int SHOW_LIST_COUNT = 5;

	private List<BABELTOWER_OPENROOMLIST> m_listBabelOpenRoomList = new List<BABELTOWER_OPENROOMLIST>();

	private DropDownList m_ddlFloorList;

	private NewListBox m_lbOpenRoomList;

	private Button m_btJoinQuickRoom;

	private Button m_btSearch;

	private Button m_btRefresh;

	private Button m_btSearchFloor;

	private Label m_laPage;

	private Button m_btBackPage;

	private Button m_btNextPage;

	private Label m_laTitle;

	private int m_nPage;

	private int m_nMaxPage;

	private short m_nCurMinFloor = 1;

	private short m_nCurMaxFloor = 10;

	private short m_nSearchFloor = 1;

	private short m_nFloorType = 1;

	private bool m_bInitFloorList;

	public short FloorType
	{
		get
		{
			return this.m_nFloorType;
		}
		set
		{
			this.m_nFloorType = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/DLG_babel_roomlist", G_ID.BABELTOWER_OPENROOMLIST_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbOpenRoomList = (base.GetControl("NewListBox_roomlist") as NewListBox);
		this.m_lbOpenRoomList.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickJoinRoom));
		this.m_btJoinQuickRoom = (base.GetControl("BT_QuickJoin") as Button);
		this.m_btJoinQuickRoom.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickQuickJoinRoom));
		this.m_btSearch = (base.GetControl("BT_Search") as Button);
		this.m_btSearch.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickSearch));
		this.m_btRefresh = (base.GetControl("BT_Reset") as Button);
		this.m_btRefresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickRefreshList));
		this.m_btSearchFloor = (base.GetControl("Button_Floor") as Button);
		this.m_btSearchFloor.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickInputSearchFloor));
		this.m_laTitle = (base.GetControl("Label_title") as Label);
		this.m_laPage = (base.GetControl("LB_Page") as Label);
		this.m_laPage.SetText(string.Empty);
		this.m_btBackPage = (base.GetControl("BT_BackPage") as Button);
		this.m_btBackPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickBackPage));
		this.m_btNextPage = (base.GetControl("BT_NextPage") as Button);
		this.m_btNextPage.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickNextPage));
		this.m_ddlFloorList = (base.GetControl("DropDownList_FloorInfo") as DropDownList);
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.OnClose();
	}

	public void InitOpemRoomList()
	{
		if (!this.m_bInitFloorList)
		{
			if (null != this.m_ddlFloorList)
			{
				string text = string.Empty;
				string empty = string.Empty;
				int num = (int)(NrTSingleton<BabelTowerManager>.Instance.GetLastFloor(this.m_nFloorType) / 10);
				this.m_ddlFloorList.SetViewArea(num);
				this.m_ddlFloorList.Clear();
				for (int i = 0; i < num; i++)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("730");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						text,
						"floors",
						10 * i + 1,
						"floors2",
						10 * i + 10
					});
					this.m_ddlFloorList.AddItem(empty, (short)(10 * i));
				}
				this.m_ddlFloorList.SetFirstItem();
				this.m_ddlFloorList.RepositionItems();
				this.m_ddlFloorList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeList));
			}
			if (this.m_nFloorType == 2)
			{
				this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2785"));
			}
			this.m_bInitFloorList = true;
		}
		this.m_listBabelOpenRoomList.Clear();
	}

	public void AddOpenRoominfo(BABELTOWER_OPENROOMLIST info)
	{
		this.m_listBabelOpenRoomList.Add(info);
	}

	public void ShowInfo()
	{
		this.m_nMaxPage = this.m_listBabelOpenRoomList.Count / 5;
		if (this.m_listBabelOpenRoomList.Count % 5 == 0 && this.m_listBabelOpenRoomList.Count != 0)
		{
			this.m_nMaxPage--;
		}
		this.m_nPage = 0;
		this.m_btSearchFloor.Text = this.m_nSearchFloor.ToString();
		this.ShowList();
	}

	public void ShowList()
	{
		string text = string.Empty;
		string empty = string.Empty;
		this.m_lbOpenRoomList.Clear();
		for (int i = this.m_nPage * 5; i < this.m_nPage * 5 + 5; i++)
		{
			if (this.m_listBabelOpenRoomList.Count <= i)
			{
				break;
			}
			BABELTOWER_OPENROOMLIST bABELTOWER_OPENROOMLIST = this.m_listBabelOpenRoomList[i];
			if (bABELTOWER_OPENROOMLIST != null)
			{
				NewListItem newListItem = new NewListItem(this.m_lbOpenRoomList.ColumnNum, true);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"count",
					bABELTOWER_OPENROOMLIST.i16LeaderLevel,
					"targetname",
					TKString.NEWString(bABELTOWER_OPENROOMLIST.szLeaderName)
				});
				newListItem.SetListItemData(0, empty, null, null, null);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1600");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"usernum",
					bABELTOWER_OPENROOMLIST.byCurUserNum,
					"maxuser",
					bABELTOWER_OPENROOMLIST.byMaxUserNum
				});
				newListItem.SetListItemData(1, empty, null, null, null);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("833");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"floor",
					bABELTOWER_OPENROOMLIST.i16Floor,
					"subfloor",
					(int)(bABELTOWER_OPENROOMLIST.i16SubFloor + 1)
				});
				newListItem.SetListItemData(2, empty, null, null, null);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1285");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"min",
					bABELTOWER_OPENROOMLIST.MinLevel,
					"max",
					bABELTOWER_OPENROOMLIST.MaxLevel
				});
				newListItem.SetListItemData(3, empty, null, null, null);
				newListItem.Data = bABELTOWER_OPENROOMLIST;
				this.m_lbOpenRoomList.Add(newListItem);
			}
		}
		this.m_lbOpenRoomList.RepositionItems();
		this.m_laPage.SetText(string.Format("{0} / {1}", this.m_nPage + 1, this.m_nMaxPage + 1));
		base.Show();
	}

	public void BtClickRefreshList(IUIObject obj)
	{
		GS_BABELTOWER_OPENROOMLIST_GET_REQ gS_BABELTOWER_OPENROOMLIST_GET_REQ = new GS_BABELTOWER_OPENROOMLIST_GET_REQ();
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_startfloor = this.m_nCurMinFloor;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_finishfloor = this.m_nCurMaxFloor;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.FloorType = this.m_nFloorType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_OPENROOMLIST_GET_REQ, gS_BABELTOWER_OPENROOMLIST_GET_REQ);
	}

	public void OnChangeList(IUIObject obj)
	{
		ListItem listItem = this.m_ddlFloorList.SelectedItem.Data as ListItem;
		if (listItem != null)
		{
			short nCurMinFloor = (short)listItem.Key + 1;
			short nCurMaxFloor = (short)listItem.Key + 10;
			this.m_nCurMinFloor = nCurMinFloor;
			this.m_nCurMaxFloor = nCurMaxFloor;
			GS_BABELTOWER_OPENROOMLIST_GET_REQ gS_BABELTOWER_OPENROOMLIST_GET_REQ = new GS_BABELTOWER_OPENROOMLIST_GET_REQ();
			gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_startfloor = this.m_nCurMinFloor;
			gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_finishfloor = this.m_nCurMaxFloor;
			gS_BABELTOWER_OPENROOMLIST_GET_REQ.FloorType = this.m_nFloorType;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_OPENROOMLIST_GET_REQ, gS_BABELTOWER_OPENROOMLIST_GET_REQ);
		}
	}

	public void BtClickJoinRoom(IUIObject obj)
	{
		IUIListObject selectItem = this.m_lbOpenRoomList.GetSelectItem();
		if (selectItem == null)
		{
			return;
		}
		BABELTOWER_OPENROOMLIST bABELTOWER_OPENROOMLIST = selectItem.Data as BABELTOWER_OPENROOMLIST;
		if (bABELTOWER_OPENROOMLIST != null)
		{
			GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
			gS_BABELTOWER_GOLOBBY_REQ.mode = 2;
			gS_BABELTOWER_GOLOBBY_REQ.babel_floor = bABELTOWER_OPENROOMLIST.i16Floor;
			gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = bABELTOWER_OPENROOMLIST.i16SubFloor;
			gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = this.m_nFloorType;
			gS_BABELTOWER_GOLOBBY_REQ.nPersonID = bABELTOWER_OPENROOMLIST.i64LeaderPersonID;
			gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = bABELTOWER_OPENROOMLIST.i16FloorType;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
		}
		this.Close();
	}

	public void BtClickQuickJoinRoom(IUIObject obj)
	{
		GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
		gS_BABELTOWER_GOLOBBY_REQ.mode = 3;
		gS_BABELTOWER_GOLOBBY_REQ.babel_floor = 1;
		gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = 0;
		gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = this.m_nFloorType;
		gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
	}

	public void BtClickInputSearchFloor(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_Input_MinLevel), null, new Action<InputNumberDlg, object>(this.On_Close_InputNumber), null);
		inputNumberDlg.SetMinMax(1L, (long)NrTSingleton<BabelTowerManager>.Instance.GetLastFloor(this.m_nFloorType));
		inputNumberDlg.SetNum(0L);
	}

	private void On_Input_MinLevel(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		if (num < 1)
		{
			num = 1;
		}
		this.m_btSearchFloor.Text = num.ToString();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	private void On_Close_InputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void BtClickSearch(IUIObject obj)
	{
		string text = this.m_btSearchFloor.GetText();
		short num = short.Parse(text);
		if (num < 1 || num > NrTSingleton<BabelTowerManager>.Instance.GetLastFloor(this.m_nFloorType))
		{
			return;
		}
		this.m_nCurMinFloor = num;
		this.m_nCurMaxFloor = num;
		this.m_nSearchFloor = num;
		GS_BABELTOWER_OPENROOMLIST_GET_REQ gS_BABELTOWER_OPENROOMLIST_GET_REQ = new GS_BABELTOWER_OPENROOMLIST_GET_REQ();
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_startfloor = this.m_nCurMinFloor;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_finishfloor = this.m_nCurMaxFloor;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.FloorType = this.m_nFloorType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_OPENROOMLIST_GET_REQ, gS_BABELTOWER_OPENROOMLIST_GET_REQ);
	}

	public void BtClickBackPage(IUIObject obj)
	{
		if (this.m_nPage > 0)
		{
			this.m_nPage--;
			this.ShowList();
		}
	}

	public void BtClickNextPage(IUIObject obj)
	{
		if (this.m_nPage < this.m_nMaxPage)
		{
			this.m_nPage++;
			this.ShowList();
		}
	}
}
