using GAME;
using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class PostDlg : Form
{
	public enum eSEND_STATE
	{
		eSEND_STATE_NORMAL,
		eSEND_STATE_NEWGUILD
	}

	public enum eRECV_POSTTYPE
	{
		eRECV_POSTNORMAL,
		eRECV_POSTBATTLE,
		eRECV_POSTFRIENDGIFT,
		eRECV_POSTALL
	}

	private enum eTabMode
	{
		Send,
		RecvList,
		History
	}

	private class CMailRequireRange
	{
		public int i32MailType_Begin;

		public int i32MailType_End;
	}

	private class CMailRequireRangeForHistory : PostDlg.CMailRequireRange
	{
		public PostDlg.E_HISTORY_FILTERTYPE eFilterType;
	}

	private enum SORT_TABMENU_RECV
	{
		ALL,
		POST,
		SYSTEM,
		END
	}

	public enum E_HISTORY_FILTERTYPE
	{
		ALL,
		SEND,
		RECV
	}

	private enum SORT_TABMENU_HISTORY
	{
		ALL,
		RECV,
		SEND,
		SYSTEM,
		END
	}

	private enum E_SEND_HISTORY
	{
		SEND_HISTORY_0,
		SEND_HISTORY_1,
		SEND_HISTORY_2,
		SEND_HISTORY_3,
		SEND_HISTORY_4,
		SEND_HISTORY_5,
		SEND_HISTORY_6,
		SEND_HISTORY_7,
		END
	}

	private const int TOOLBAR_NUM = 3;

	private const short RECV_MAX = 30;

	public PostDlg.eRECV_POSTTYPE m_eMailType;

	private int m_nItemUnique_SEND;

	private int m_i32ItemSendNum_SEND;

	private int m_i32ItemCurNum_SEND;

	private int m_i32PosType_SEND;

	private int m_i32PosItem_SEND;

	private bool m_bToggleMailID;

	private object m_objSendItem;

	private int m_i32SelectedMailType;

	private int m_iRecvCurPage = 1;

	private long m_nFirstMailID;

	private long m_nLastMailID;

	private bool m_bHistoryNextRequest = true;

	private bool m_bNextRequest = true;

	private int m_nMaxMailNum = 1;

	private int m_iRecvTotalPage = 1;

	private int m_iRecvCount;

	private GS_MAILBOX_INFO[] m_RecvList = new GS_MAILBOX_INFO[30];

	private PostDlg.SORT_TABMENU_RECV _eLastSelectedType_send;

	private PostDlg.SORT_TABMENU_HISTORY _eLastSelectedType_history;

	private Toolbar _tbTab;

	private Button Button_GetAll;

	private TextField m_UserName;

	private TextArea m_Comment;

	private NewListBox m_NewListBox;

	private Button _btRecv_Prev;

	private Button _btRecv_Next;

	private Box _boxRecv_Page;

	private Button m_SendItem;

	private Button m_Cancel;

	private Button m_FriendList;

	private CheckBox m_cbPushMsg;

	private Label m_lbPushText;

	private Label m_lbPushText2;

	private Label m_lbRemain;

	private Label m_lbDailyMailCount;

	private NrMyCharInfo m_pkMyChar;

	private long m_nFriendPersonID;

	private bool requestMail;

	private Dictionary<int, long> m_mapPrevPageMailID = new Dictionary<int, long>();

	private DrawTexture m_LoadingImg;

	private Label m_LoadingTxt;

	private PostDlg.eSEND_STATE m_eSendState;

	public string recvUserName = string.Empty;

	private static short RECV_LISTNUM
	{
		get
		{
			return 4;
		}
	}

	private static short HISTORY_LISTNUM
	{
		get
		{
			return 4;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Post/DLG_NewPost", G_ID.POST_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this._tbTab = (base.GetControl("ToolBar_Tab") as Toolbar);
		this._tbTab.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("7");
		this._tbTab.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("5");
		this._tbTab.Control_Tab[2].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("6");
		for (int i = 0; i < 3; i++)
		{
			UIPanelTab expr_8D = this._tbTab.Control_Tab[i];
			expr_8D.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_8D.ButtonClick, new EZValueChangedDelegate(this.OnClickToolBar));
		}
		this._tbTab.FirstSetting();
		this.Button_GetAll = (base.GetControl("BT_GetAll") as Button);
		this.Button_GetAll.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickGetAll));
		this.m_UserName = (base.GetControl("TextField_InputName") as TextField);
		this.m_UserName.Text = string.Empty;
		TextField expr_11D = this.m_UserName;
		expr_11D.CommitDelegate = (EZKeyboardCommitDelegate)Delegate.Combine(expr_11D.CommitDelegate, new EZKeyboardCommitDelegate(this.OnInputText));
		this.m_Comment = (base.GetControl("TextArea_InputText") as TextArea);
		if (null != this.m_Comment.spriteText)
		{
			this.m_Comment.maxLength = 256;
			this.m_Comment.spriteText.parseColorTags = false;
			if (null != this.m_Comment.spriteTextShadow)
			{
				this.m_Comment.spriteTextShadow.parseColorTags = false;
			}
		}
		this.m_NewListBox = (base.GetControl("NewListBox_MailList") as NewListBox);
		this.m_NewListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickList));
		this.m_NewListBox.Reserve = false;
		this.m_SendItem = (base.GetControl("Button_Send") as Button);
		this.m_SendItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSendItem));
		this._btRecv_Prev = (base.GetControl("Button_ArrowBtn01") as Button);
		this._btRecv_Next = (base.GetControl("Button_ArrowBtn02") as Button);
		this._boxRecv_Page = (base.GetControl("Box_Page") as Box);
		Button expr_260 = this._btRecv_Prev;
		expr_260.Click = (EZValueChangedDelegate)Delegate.Combine(expr_260.Click, new EZValueChangedDelegate(this.OnClickRecvPrev));
		Button expr_287 = this._btRecv_Prev;
		expr_287.Click = (EZValueChangedDelegate)Delegate.Combine(expr_287.Click, new EZValueChangedDelegate(this.OnClickHistoryPrev));
		Button expr_2AE = this._btRecv_Next;
		expr_2AE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2AE.Click, new EZValueChangedDelegate(this.OnClickRecvNext));
		Button expr_2D5 = this._btRecv_Next;
		expr_2D5.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2D5.Click, new EZValueChangedDelegate(this.OnClickHistoryNext));
		this.m_Cancel = (base.GetControl("Button_Cancel") as Button);
		this.m_Cancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancel));
		this.m_FriendList = (base.GetControl("Button_friendlist") as Button);
		this.m_FriendList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickFriendList));
		this.m_cbPushMsg = (base.GetControl("CB_PushCheck01") as CheckBox);
		this.m_LoadingImg = (base.GetControl("DrawTexture_Loading") as DrawTexture);
		this.m_LoadingTxt = (base.GetControl("Label_Loading") as Label);
		this.m_LoadingImg.Visible = false;
		this.m_LoadingTxt.Visible = false;
		this.m_lbPushText = (base.GetControl("LB_PushText01") as Label);
		this.m_lbPushText2 = (base.GetControl("LB_PushText02") as Label);
		this.m_lbRemain = (base.GetControl("Label_Remain") as Label);
		this.m_lbDailyMailCount = (base.GetControl("Label_dailymailcount") as Label);
		this.SetDailyMailCount();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAIL", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_pkMyChar = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		this.ChangeTab_Send();
		base.SetScreenCenter();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("post_open");
		this.SetSendState(this.m_eSendState);
	}

	public void SetDailyMailCount()
	{
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD)
		{
			this.m_lbDailyMailCount.Text = string.Empty;
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FRIEND_MAIL_LIMIT);
		long charDetail = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(22);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2732"),
			"count",
			charDetail,
			"totalcount",
			num
		});
		this.m_lbDailyMailCount.Text = empty;
	}

	private void ClickFriendList(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POSTFRIEND_DLG) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POSTFRIEND_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POSTFRIEND_DLG);
		}
	}

	private void ClickSendItem(IUIObject obj)
	{
		this.OnPostSend();
	}

	private void ClickList(IUIObject obj)
	{
		UIListItemContainer selectItem = this.m_NewListBox.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		if (selectItem.Data == null)
		{
			return;
		}
		if (this._tbTab.CurrentPanel.index == 1)
		{
			long num = (long)selectItem.Data;
			if (num == 0L)
			{
				return;
			}
			this.RequestDetailInfo(num);
		}
		else if (this._tbTab.CurrentPanel.index == 2)
		{
			MAILBOXHISTORY_INFO mAILBOXHISTORY_INFO = (MAILBOXHISTORY_INFO)selectItem.Data;
			if (mAILBOXHISTORY_INFO == null)
			{
				return;
			}
			this.RequestHistoryInfo(mAILBOXHISTORY_INFO);
		}
	}

	public override void InitData()
	{
		if (TsPlatform.IsMobile)
		{
			base.Draggable = false;
			base.SetScreenCenter();
		}
		else
		{
			base.SetScreenCenter();
		}
	}

	public void ChangeTab_Send()
	{
		base.ShowLayer(1);
		this.ItemSlotClear();
		this._tbTab.SetSelectTabIndex(0);
	}

	public void ChangeTab_RecvList()
	{
		base.ShowLayer(2);
		this._tbTab.SetSelectTabIndex(1);
	}

	public void Close_PostRecvDlg()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POST_RECV_DLG);
	}

	public void ToggleMailID()
	{
		this.m_bToggleMailID = !this.m_bToggleMailID;
	}

	private void InitSendControl()
	{
		this.m_UserName.ClearDefaultText(this.m_UserName);
		this.m_UserName.Text = " ";
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD)
		{
			this.m_UserName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1939"));
		}
		this.m_Comment.ClearDefaultText(this.m_Comment);
		this.m_Comment.Text = " ";
	}

	public void ItemSlotClear()
	{
		this.m_nItemUnique_SEND = 0;
		this.m_i32ItemCurNum_SEND = 0;
		this.m_i32PosType_SEND = 0;
		this.m_i32PosItem_SEND = 0;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void DragDrop(EZDragDropParams a_sDragDropParams)
	{
		if (a_sDragDropParams.evt == EZDragDropEvent.Dropped && a_sDragDropParams.dragObj.DropTarget != null)
		{
			ImageSlot imageSlot = a_sDragDropParams.dragObj.Data as ImageSlot;
			if (imageSlot != null && !(imageSlot.c_oItem is ITEM))
			{
				return;
			}
			UIListItemContainer component = a_sDragDropParams.dragObj.DropTarget.GetComponent<UIListItemContainer>();
			ImageSlot imageSlot2 = component.Data as ImageSlot;
			if (imageSlot2 == null)
			{
				return;
			}
			if (imageSlot2.c_bDisable)
			{
				return;
			}
			G_ID windowID = (G_ID)imageSlot2.WindowID;
			if (windowID == G_ID.INVENTORY_DLG)
			{
				Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
				if (inventory_Dlg != null)
				{
				}
			}
		}
	}

	private void OnImageViewClick(IUIObject a_oObject)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INVENTORY_DLG))
		{
			Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
			inventory_Dlg.Show();
			inventory_Dlg.SetLocation((float)((int)(base.GetLocation().x - inventory_Dlg.GetSize().x)), base.GetLocationY());
		}
	}

	private void OnClickToolBar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		int layer = uIPanelTab.panel.index + 1;
		this.m_iRecvCurPage = 1;
		switch (uIPanelTab.panel.index)
		{
		case 0:
			this.requestMail = false;
			base.ShowLayer(layer);
			this.ItemSlotClear();
			this.InitSendControlTap_ForTapMove();
			break;
		case 1:
			base.ShowLayer(layer);
			this.m_nFirstMailID = 0L;
			this.m_nLastMailID = 0L;
			this.m_bNextRequest = false;
			this.RequestRecvList(this.m_eMailType);
			break;
		case 2:
			base.ShowLayer(2);
			this.RequestHistory(this.m_eMailType);
			this.Button_GetAll.Visible = false;
			break;
		}
		this.Close_PostRecvDlg();
	}

	private void OnClickGetAll(IUIObject obj)
	{
		GS_MAILBOX_TAKE_GETMAILALL_REQ gS_MAILBOX_TAKE_GETMAILALL_REQ = new GS_MAILBOX_TAKE_GETMAILALL_REQ();
		for (int i = 0; i < 5; i++)
		{
			GS_MAILBOX_INFO gS_MAILBOX_INFO = this.m_RecvList[i];
			if (gS_MAILBOX_INFO != null)
			{
				if (gS_MAILBOX_INFO.i64MailID != 0L)
				{
					gS_MAILBOX_TAKE_GETMAILALL_REQ.i8MailCount = (byte)i;
					gS_MAILBOX_TAKE_GETMAILALL_REQ.i64Idx[i] = gS_MAILBOX_INFO.i64MailID;
				}
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_TAKE_GETMAILALL_REQ, gS_MAILBOX_TAKE_GETMAILALL_REQ);
	}

	private void InitSendControlTap_ForTapMove()
	{
	}

	public void SetSendCharName(string _SendCharName)
	{
		this.m_UserName.ClearDefaultText(this.m_UserName);
		this.m_UserName.Text = _SendCharName;
	}

	private void CheckCharName()
	{
		if (this.m_UserName.Text.Length <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("98"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (this.m_UserName.Text == nrCharUser.GetCharName())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("313"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_MAILBOX_VERIFY_CHARNAME_REQ gS_MAILBOX_VERIFY_CHARNAME_REQ = new GS_MAILBOX_VERIFY_CHARNAME_REQ();
		TKString.StringChar(this.m_UserName.Text, ref gS_MAILBOX_VERIFY_CHARNAME_REQ.szCharName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_VERIFY_CHARNAME_REQ, gS_MAILBOX_VERIFY_CHARNAME_REQ);
	}

	private void OnInputText(IKeyFocusable obj)
	{
		this.CheckCharName();
	}

	private void OnClickSendNameX(IUIObject obj)
	{
	}

	private void OnClickSendMoney(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		if (inputNumberDlg != null)
		{
			inputNumberDlg.OnClickCancel(null);
		}
		inputNumberDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg);
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.InputNumberDlg_SendMoney_OnApply), null, new Action<InputNumberDlg, object>(this.InputNumberDlg_SendMoney_OnCancel), null);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		inputNumberDlg.SetMinMax(0L, kMyCharInfo.m_Money);
		inputNumberDlg.SetNum(0L);
		inputNumberDlg.Show();
		inputNumberDlg.SetScreenCenter();
	}

	private Vector2 GetInputNumberLocation(InputNumberDlg _inputDlg)
	{
		Vector2 result = Vector2.zero;
		if (TsPlatform.IsMobile)
		{
			result = base.GetLocation();
		}
		else
		{
			result = NrTSingleton<FormsManager>.Instance.Get_Show_Position(base.WindowID, _inputDlg.WindowID, FormsManager.E_FORM_POSITION.DOWN);
		}
		return result;
	}

	private void OnPostSend()
	{
		if (this.m_UserName.Text.Length <= 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("98"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (this.m_UserName.Text == nrCharUser.GetCharName())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("313"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NORMAL)
		{
			long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FRIEND_MAIL_LIMIT);
			long charDetail = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(22);
			if (num <= charDetail)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("744"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				return;
			}
		}
		GS_MAILBOX_SEND_REQ gS_MAILBOX_SEND_REQ = new GS_MAILBOX_SEND_REQ();
		gS_MAILBOX_SEND_REQ.nRecvType = (int)this.m_eSendState;
		TKString.StringChar(this.m_UserName.Text, ref gS_MAILBOX_SEND_REQ.szRecvCharName);
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			this.m_Comment.Text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				this.m_Comment.Text
			});
		}
		if (this.m_Comment.GetDefaultText() != string.Empty)
		{
			TKString.StringChar(string.Empty, ref gS_MAILBOX_SEND_REQ.szComment);
		}
		else
		{
			if (this.m_Comment.Text.Contains("[#"))
			{
				this.m_Comment.Text = this.m_Comment.Text.Replace("[#", string.Empty);
			}
			TKString.StringChar(this.m_Comment.Text, ref gS_MAILBOX_SEND_REQ.szComment);
		}
		gS_MAILBOX_SEND_REQ.nMoney = 0L;
		gS_MAILBOX_SEND_REQ.nFee = this.CalcFee(gS_MAILBOX_SEND_REQ.nMoney);
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD)
		{
			gS_MAILBOX_SEND_REQ.nFee = NrTSingleton<NewGuildManager>.Instance.GetPostTex() * (long)NrTSingleton<NewGuildManager>.Instance.GetMemberCount();
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (gS_MAILBOX_SEND_REQ.nMoney + gS_MAILBOX_SEND_REQ.nFee > kMyCharInfo.m_Money)
		{
			string text = string.Empty;
			string empty = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("100");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				gS_MAILBOX_SEND_REQ.nFee
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.m_nItemUnique_SEND > 0 && this.m_i32ItemSendNum_SEND > 0)
		{
			gS_MAILBOX_SEND_REQ.nPosType = this.m_i32PosType_SEND;
			gS_MAILBOX_SEND_REQ.sPosItem = this.m_i32PosItem_SEND;
			gS_MAILBOX_SEND_REQ.Cur_ItemNum = (int)((short)this.m_i32ItemCurNum_SEND);
			gS_MAILBOX_SEND_REQ.Send_ItemNum = (int)((short)this.m_i32ItemSendNum_SEND);
			gS_MAILBOX_SEND_REQ.nItemUnique = this.m_nItemUnique_SEND;
		}
		else
		{
			gS_MAILBOX_SEND_REQ.nPosType = -1;
			gS_MAILBOX_SEND_REQ.sPosItem = -1;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string empty2 = string.Empty;
		long num2 = 0L;
		if (this.m_nItemUnique_SEND != 0 && num2 != 0L)
		{
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nItemUnique_SEND);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("123"),
				"item",
				itemNameByItemUnique,
				"count",
				this.m_i32ItemSendNum_SEND,
				"sendmoney",
				num2,
				"money",
				gS_MAILBOX_SEND_REQ.nFee.ToString()
			});
		}
		else if (this.m_nItemUnique_SEND != 0)
		{
			string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_nItemUnique_SEND);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("122"),
				"item",
				itemNameByItemUnique2,
				"count",
				this.m_i32ItemSendNum_SEND,
				"money",
				gS_MAILBOX_SEND_REQ.nFee.ToString()
			});
		}
		else if (num2 != 0L)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("121"),
				"sendmoney",
				num2,
				"money",
				gS_MAILBOX_SEND_REQ.nFee.ToString()
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("62"),
				"price",
				gS_MAILBOX_SEND_REQ.nFee.ToString()
			});
		}
		msgBoxUI.SetMsg(new YesDelegate(this.OnSendOK), gS_MAILBOX_SEND_REQ, new NoDelegate(this.OnSendCancel), gS_MAILBOX_SEND_REQ, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("7"), empty2, eMsgType.MB_OK_CANCEL);
	}

	private void OnClickSend(IUIObject obj)
	{
		this.CheckCharName();
	}

	private void OnClickCancel(IUIObject obj)
	{
		this.Close();
	}

	private void OnClickRecvInfo(IUIObject obj)
	{
		Button button = obj as Button;
		long num = (long)button.Data;
		if (num == 0L)
		{
			return;
		}
		this.RequestDetailInfo(num);
	}

	private void OnClickHistoryinfo(IUIObject obj)
	{
		Button button = obj as Button;
		if (button.Data == null)
		{
			return;
		}
		MAILBOXHISTORY_INFO mAILBOXHISTORY_INFO = (MAILBOXHISTORY_INFO)button.Data;
		if (mAILBOXHISTORY_INFO == null)
		{
			return;
		}
		this.RequestHistoryInfo(mAILBOXHISTORY_INFO);
	}

	private void OnClickRecvPrev(IUIObject obj)
	{
		if (this._tbTab.CurrentPanel.index == 1)
		{
			if (this.requestMail)
			{
				return;
			}
			if (this.m_iRecvCurPage <= 1)
			{
				return;
			}
			this.m_iRecvCurPage--;
			this.m_bNextRequest = false;
			this.requestMail = true;
			this.RequestRecvList(this.m_eMailType);
		}
	}

	private void OnClickRecvNext(IUIObject obj)
	{
		if (this._tbTab.CurrentPanel.index == 1)
		{
			if (this.requestMail)
			{
				return;
			}
			if (this.m_iRecvCurPage >= this.m_iRecvTotalPage)
			{
				return;
			}
			this.m_iRecvCurPage++;
			this.m_bNextRequest = true;
			this.requestMail = true;
			this.RequestRecvList(this.m_eMailType);
		}
	}

	private void OnClickRecvRefresh(IUIObject obj)
	{
	}

	private void OnClickHistoryPrev(IUIObject obj)
	{
		if (this._tbTab.CurrentPanel.index == 2)
		{
			if (this.requestMail)
			{
				return;
			}
			if (this.m_iRecvCurPage <= 1)
			{
				return;
			}
			this.m_iRecvCurPage--;
			this.m_bNextRequest = false;
			this.requestMail = true;
			this.RequestHistory(this.m_eMailType);
		}
	}

	private void OnClickHistoryNext(IUIObject obj)
	{
		if (this._tbTab.CurrentPanel.index == 2)
		{
			if (this.requestMail)
			{
				return;
			}
			if (this.m_iRecvCurPage >= this.m_iRecvTotalPage)
			{
				return;
			}
			this.m_iRecvCurPage++;
			this.m_bNextRequest = true;
			this.requestMail = true;
			this.RequestHistory(this.m_eMailType);
		}
	}

	private void OnClickHistoryRefresh(IUIObject obj)
	{
		this.m_iRecvCurPage = 1;
		this.RequestHistory(this.m_eMailType);
	}

	public static void OnMessageEffect(string EffectKey)
	{
		if (EffectKey != null)
		{
			if (PostDlg.<>f__switch$map2 == null)
			{
				PostDlg.<>f__switch$map2 = new Dictionary<string, int>(2)
				{
					{
						"ef_sendmail_001",
						0
					},
					{
						"ef_getmail_001",
						1
					}
				};
			}
			int num;
			if (PostDlg.<>f__switch$map2.TryGetValue(EffectKey, out num))
			{
				if (num != 0)
				{
					if (num == 1)
					{
						if (TsPlatform.IsWeb)
						{
							TsAudioManager.Container.RequestAudioClip("UI_SFX", "MAIL", "RECEIVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
						}
						NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
					}
				}
				else if (TsPlatform.IsWeb)
				{
					TsAudioManager.Container.RequestAudioClip("UI_SFX", "MAIL", "SENDMAIL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
		}
	}

	public void OnSendOK(object a_oObject)
	{
		string text = this.m_Comment.Text;
		text = string.Format("{0}\n-{1}", text, this.m_pkMyChar.GetMyCharObject().name);
		GS_MAILBOX_SEND_REQ gS_MAILBOX_SEND_REQ = a_oObject as GS_MAILBOX_SEND_REQ;
		gS_MAILBOX_SEND_REQ.nRecvType = (int)this.m_eSendState;
		bool flag = false;
		if (this.m_cbPushMsg.StateNum == 1)
		{
			flag = this.CheckPushMsg();
		}
		if (this.m_cbPushMsg.StateNum == 1 && flag)
		{
			gS_MAILBOX_SEND_REQ.i8Push = 1;
			TKString.StringChar(text, ref gS_MAILBOX_SEND_REQ.strPushText);
		}
		else
		{
			gS_MAILBOX_SEND_REQ.i8Push = 0;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_SEND_REQ, gS_MAILBOX_SEND_REQ);
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NORMAL && this.m_cbPushMsg.StateNum == 1 && flag)
		{
			GS_FRIEND_PUSH_REQ gS_FRIEND_PUSH_REQ = new GS_FRIEND_PUSH_REQ();
			gS_FRIEND_PUSH_REQ.i64PersonID = this.m_pkMyChar.m_PersonID;
			gS_FRIEND_PUSH_REQ.i64FriendPersonID = this.m_nFriendPersonID;
			TKString.StringChar(text, ref gS_FRIEND_PUSH_REQ.szChatStr);
			gS_FRIEND_PUSH_REQ.i8RecvType = (byte)this.m_eSendState;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_PUSH_REQ, gS_FRIEND_PUSH_REQ);
		}
		this.InitSendControl();
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "MAIL", "SEND", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void OnSendCancel(object a_oObject)
	{
	}

	private void _SetCalcFeeSendMoney(long i64SendMoney)
	{
		this.CalcFee(i64SendMoney);
	}

	private void On_SendMoney_Change(IUIObject a_oObject)
	{
		this.RefreshFee();
	}

	private void RefreshFee()
	{
		this._SetCalcFeeSendMoney(0L);
	}

	public void InputNumberDlg_SendMoney_OnApply(InputNumberDlg dlg, object boject)
	{
		long num = dlg.GetNum();
		num = dlg.GetNum();
		this._SetCalcFeeSendMoney(num);
	}

	public void InputNumberDlg_SendMoney_OnCancel(InputNumberDlg dlg, object boject)
	{
		this._SetCalcFeeSendMoney(0L);
	}

	public void InputNumberDlg_OnApply(InputNumberDlg dlg, object obj)
	{
		ITEM iTEM = (ITEM)obj;
		if (iTEM != null)
		{
			long num = dlg.GetNum();
			string empty = string.Empty;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("889");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"count1",
				num
			});
			this.m_i32ItemSendNum_SEND = (int)num;
		}
	}

	public void InputNumberDlg_OnCancel(InputNumberDlg dlg, object boject)
	{
		this.ItemSlotClear();
	}

	public void SendItemSet(ITEM item)
	{
		ImageSlot srcSlot = new ImageSlot();
		PostDlg.SetImageSlotFromItem(ref srcSlot, item, 0);
		this.OnItemDragDrop(srcSlot);
	}

	public static bool IsEnableSend(ITEM item)
	{
		bool result = true;
		if (!NrTSingleton<ItemManager>.Instance.IsItemATB(item.m_nItemUnique, 2L))
		{
			string message = string.Empty;
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("418");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			result = false;
		}
		return result;
	}

	public void OnItemDragDrop(ImageSlot srcSlot)
	{
	}

	private long CalcFee(long money)
	{
		long num = 100L;
		long num2 = 0L;
		if (money != 0L)
		{
			num2 = money * 2L / 100L;
		}
		long num3 = num2 + num;
		return (num3 >= 100L) ? num3 : 100L;
	}

	public void SetSendItem(ImageSlot itemslot, int i32ItemNum)
	{
	}

	private void ClickReceiveMail(IUIObject obj)
	{
		UIButton uIButton = (UIButton)obj;
		if (null == uIButton)
		{
			return;
		}
		if (uIButton.Data == null)
		{
			return;
		}
		if (this._tbTab.CurrentPanel.index == 1)
		{
			long num = (long)uIButton.Data;
			if (num == 0L)
			{
				return;
			}
			GS_MAILBOX_TAKE_GETMAIL_REQ gS_MAILBOX_TAKE_GETMAIL_REQ = new GS_MAILBOX_TAKE_GETMAIL_REQ();
			gS_MAILBOX_TAKE_GETMAIL_REQ.i64Idx = num;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_TAKE_GETMAIL_REQ, gS_MAILBOX_TAKE_GETMAIL_REQ);
		}
	}

	private void ClickHistoryMail(IUIObject obj)
	{
	}

	public void ShowReceiveList()
	{
		this.requestMail = false;
		this.m_NewListBox.Clear();
		this.m_lbRemain.Visible = true;
		for (int i = 0; i < this.m_iRecvCount; i++)
		{
			GS_MAILBOX_INFO gS_MAILBOX_INFO = this.m_RecvList[i];
			if (gS_MAILBOX_INFO != null)
			{
				NewListItem newListItem = new NewListItem(this.m_NewListBox.ColumnNum, true, string.Empty);
				newListItem.Data = gS_MAILBOX_INFO.i64MailID;
				newListItem.SetListItemData(0, "Win_T_ItemEmpty", null, null, null);
				if (gS_MAILBOX_INFO.i32ItemUnique > 0 && gS_MAILBOX_INFO.i32ItemNum > 0)
				{
					newListItem.SetListItemData(1, new ITEM
					{
						m_nItemUnique = gS_MAILBOX_INFO.i32ItemUnique,
						m_nItemNum = gS_MAILBOX_INFO.i32ItemNum
					}, null, null, null);
				}
				else if (gS_MAILBOX_INFO.i64CharMoney > 0L)
				{
					UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01");
					newListItem.SetListItemData(1, loader, null, null, null);
				}
				else if (gS_MAILBOX_INFO.i32CharKind > 0)
				{
					newListItem.SetListItemData(1, new NkListSolInfo
					{
						SolCharKind = gS_MAILBOX_INFO.i32CharKind,
						SolGrade = (int)gS_MAILBOX_INFO.i8Grade,
						SolLevel = gS_MAILBOX_INFO.i16Level,
						ShowLevel = true
					}, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(1, NrTSingleton<PostUtil>.Instance.GetAttachIconTextureName(gS_MAILBOX_INFO.i64CharMoney, gS_MAILBOX_INFO.i64ItemID), null, null, null);
				}
				string text = string.Empty;
				if (this.m_bToggleMailID)
				{
					text = "(" + gS_MAILBOX_INFO.i64MailID.ToString() + ")" + NrTSingleton<PostUtil>.Instance.GetSendObjectName((eMAIL_TYPE)gS_MAILBOX_INFO.i32MailType, TKString.NEWString(gS_MAILBOX_INFO.szCharName_Send), false, false);
				}
				else
				{
					text = NrTSingleton<PostUtil>.Instance.GetSendObjectName((eMAIL_TYPE)gS_MAILBOX_INFO.i32MailType, TKString.NEWString(gS_MAILBOX_INFO.szCharName_Send), false, false);
				}
				newListItem.SetListItemData(2, text, null, null, null);
				string empty = string.Empty;
				DateTime dueDate = PublicMethod.GetDueDate(gS_MAILBOX_INFO.i64DateVary_Send);
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1541");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"year",
					dueDate.Year,
					"month",
					dueDate.Month,
					"day",
					dueDate.Day,
					"hour",
					dueDate.Hour,
					"min",
					dueDate.Minute,
					"sec",
					dueDate.Second
				});
				newListItem.SetListItemData(3, empty, null, null, null);
				dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
				DateTime dueDate2 = PublicMethod.GetDueDate(gS_MAILBOX_INFO.i64DateVary_End);
				TimeSpan timeSpan = dueDate2 - dueDate;
				string empty2 = string.Empty;
				textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("849");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromInterface,
					"day",
					timeSpan.Days,
					"hour",
					timeSpan.Hours,
					"min",
					timeSpan.Minutes
				});
				newListItem.SetListItemData(5, empty2, null, null, null);
				if (this.m_eMailType == PostDlg.eRECV_POSTTYPE.eRECV_POSTBATTLE)
				{
					newListItem.SetListItemData(4, false);
				}
				else if (this.m_RecvList[i].i32MailType >= 200 && this.m_RecvList[i].i32MailType <= 204)
				{
					newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("12"), null, null, null);
					newListItem.SetListItemEnable(4, false);
				}
				else if (this.m_RecvList[i].i32MailType == 111 || this.m_RecvList[i].i32MailType == 124)
				{
					newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("12"), null, null, null);
					newListItem.SetListItemEnable(4, false);
				}
				else
				{
					newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("12"), gS_MAILBOX_INFO.i64MailID, new EZValueChangedDelegate(this.ClickReceiveMail), null);
				}
				this.m_NewListBox.Add(newListItem);
			}
		}
		this.m_NewListBox.RepositionItems();
		this._boxRecv_Page.Text = this.m_iRecvCurPage + " / " + this.m_iRecvTotalPage;
	}

	public void SetRecvList(int totalCnt, int recvCnt, ref GS_MAILBOX_INFO[] nMailBoxInfo)
	{
		if (this._tbTab.CurrentPanel.index == 1)
		{
			this.m_iRecvCount = recvCnt;
			if (this.m_iRecvCount > 30)
			{
				this.m_iRecvCount = 30;
			}
			for (int i = 0; i < this.m_iRecvCount; i++)
			{
				this.m_RecvList[i] = nMailBoxInfo[i];
				if (i == 0)
				{
					this.m_nFirstMailID = nMailBoxInfo[i].i64MailID;
				}
				else if (this.m_iRecvCount - 1 == i)
				{
					this.m_nLastMailID = nMailBoxInfo[i].i64MailID;
				}
			}
			this.m_iRecvTotalPage = (totalCnt - 1) / (int)PostDlg.RECV_LISTNUM + 1;
			this.m_nMaxMailNum = totalCnt;
			if (!this.m_mapPrevPageMailID.ContainsKey(this.m_iRecvCurPage))
			{
				this.m_mapPrevPageMailID.Add(this.m_iRecvCurPage, this.m_nLastMailID);
			}
			else
			{
				this.m_mapPrevPageMailID[this.m_iRecvCurPage] = this.m_nLastMailID;
			}
			this.ShowReceiveList();
		}
	}

	public void ShowHistory(GS_MAILBOX_HISTORY_LIST_ACK History, int totalNum, int recvCnt, ref MAILBOXHISTORY_INFO[] nHistoryInfo)
	{
		this.requestMail = false;
		if (this._tbTab.CurrentPanel.index != 2)
		{
			return;
		}
		this.m_nFirstMailID = 0L;
		this.m_nLastMailID = 0L;
		this.m_NewListBox.Clear();
		this.m_lbRemain.Visible = false;
		if (this.m_iRecvCurPage == 1)
		{
			this.m_iRecvTotalPage = (totalNum - 1) / (int)PostDlg.HISTORY_LISTNUM + 1;
			this.m_nMaxMailNum = totalNum;
		}
		else
		{
			this.m_iRecvTotalPage = (this.m_nMaxMailNum - 1) / (int)PostDlg.HISTORY_LISTNUM + 1;
		}
		for (int i = 0; i < recvCnt; i++)
		{
			if (i == 0)
			{
				this.m_nFirstMailID = nHistoryInfo[i].i64MailID;
			}
			else if (recvCnt - 1 == i)
			{
				this.m_nLastMailID = nHistoryInfo[i].i64MailID;
			}
			NewListItem newListItem = new NewListItem(this.m_NewListBox.ColumnNum - 1, true, string.Empty);
			newListItem.Data = nHistoryInfo[i];
			eMAILBOX_ICONTYPE iconType = eMAILBOX_ICONTYPE.ICONTYPE_NONE;
			if (0L < nHistoryInfo[i].nMoney && 0 < nHistoryInfo[i].nItemUnique)
			{
				iconType = eMAILBOX_ICONTYPE.ICONTYPE_MONEY_ITEM;
			}
			else if (0L < nHistoryInfo[i].nMoney)
			{
				iconType = eMAILBOX_ICONTYPE.ICONTYPE_MONEY;
			}
			else if (0 < nHistoryInfo[i].nItemUnique)
			{
				iconType = eMAILBOX_ICONTYPE.ICONTYPE_MONEY_ITEM;
			}
			else if (0 < nHistoryInfo[i].nSolKind)
			{
				iconType = eMAILBOX_ICONTYPE.ICONTYPE_SOL;
			}
			newListItem.SetListItemData(0, false);
			newListItem.SetListItemData(1, NrTSingleton<PostUtil>.Instance.GetAttachIconTextureName(iconType), null, null, null);
			newListItem.SetListItemData(2, NrTSingleton<PostUtil>.Instance.GetSendObjectName((eMAIL_TYPE)nHistoryInfo[i].i32MailType, TKString.NEWString(nHistoryInfo[i].szCharName_Send), true, false), null, null, null);
			DateTime dueDate = PublicMethod.GetDueDate(nHistoryInfo[i].i64DateVary_Send);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1541");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"year",
				dueDate.Year,
				"month",
				dueDate.Month,
				"day",
				dueDate.Day,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute,
				"sec",
				dueDate.Second
			});
			newListItem.SetListItemData(3, empty, null, null, null);
			newListItem.SetListItemData(4, false);
			this.m_NewListBox.Add(newListItem);
		}
		this.m_NewListBox.RepositionItems();
		this._boxRecv_Page.Text = this.m_iRecvCurPage + " / " + this.m_iRecvTotalPage;
	}

	public void OnCharNameVerified()
	{
	}

	public bool CheckPushMsg()
	{
		if (this.m_eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD)
		{
			return true;
		}
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in this.m_pkMyChar.m_kFriendInfo.GetFriendInfoValues())
		{
			string text = TKString.NEWString(uSER_FRIEND_INFO.szName);
			if (text.Equals(this.m_UserName.Text))
			{
				if (uSER_FRIEND_INFO.i8Location != 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("290"));
					bool result = false;
					return result;
				}
				this.m_nFriendPersonID = uSER_FRIEND_INFO.nPersonID;
				break;
			}
		}
		if (this.m_nFriendPersonID == 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("290"));
			return false;
		}
		DateTime nowTime = PublicMethod.GetNowTime();
		return true;
	}

	private void RequestHistory(PostDlg.eRECV_POSTTYPE bType)
	{
		int eLastSelectedType_history = (int)this._eLastSelectedType_history;
		this.m_eMailType = bType;
		PostDlg.CMailRequireRangeForHistory cMailRequireRangeForHistory = this.JudgeMailType_ForHistory(eLastSelectedType_history);
		this.RequestHistory(cMailRequireRangeForHistory.i32MailType_Begin, cMailRequireRangeForHistory.i32MailType_End, PostDlg.E_HISTORY_FILTERTYPE.ALL);
	}

	public void RequestHistory(int i32MailType_Begin, int i32MailType_End, PostDlg.E_HISTORY_FILTERTYPE _Filtertype)
	{
		this.requestMail = true;
		GS_MAILBOX_HISTORY_LIST_REQ gS_MAILBOX_HISTORY_LIST_REQ = new GS_MAILBOX_HISTORY_LIST_REQ();
		gS_MAILBOX_HISTORY_LIST_REQ.i32Page = this.m_iRecvCurPage;
		gS_MAILBOX_HISTORY_LIST_REQ.i32PageSize = (int)PostDlg.HISTORY_LISTNUM;
		gS_MAILBOX_HISTORY_LIST_REQ.i32MailType_Begin = i32MailType_Begin;
		gS_MAILBOX_HISTORY_LIST_REQ.i32MailType_End = i32MailType_End;
		gS_MAILBOX_HISTORY_LIST_REQ.m_ui8FilterType = (byte)_Filtertype;
		gS_MAILBOX_HISTORY_LIST_REQ.nFirstMailID = this.m_nFirstMailID;
		gS_MAILBOX_HISTORY_LIST_REQ.nLastMailID = this.m_nLastMailID;
		gS_MAILBOX_HISTORY_LIST_REQ.bNextRequest = this.m_bHistoryNextRequest;
		gS_MAILBOX_HISTORY_LIST_REQ.i8MailType = (byte)this.m_eMailType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_HISTORY_LIST_REQ, gS_MAILBOX_HISTORY_LIST_REQ);
	}

	public void RequestRecvList(PostDlg.eRECV_POSTTYPE bType)
	{
		if (bType == PostDlg.eRECV_POSTTYPE.eRECV_POSTNORMAL)
		{
			this.Button_GetAll.Visible = true;
		}
		else if (bType == PostDlg.eRECV_POSTTYPE.eRECV_POSTBATTLE)
		{
			this.Button_GetAll.Visible = false;
		}
		PostDlg.CMailRequireRange cMailRequireRange = this.JudgeMailType(this._eLastSelectedType_send);
		this.m_eMailType = bType;
		this.RequestRecvList(cMailRequireRange.i32MailType_Begin, cMailRequireRange.i32MailType_End);
	}

	public void RequestRecvList(int i32MailType_Begin, int i32MailType_End)
	{
		this.requestMail = true;
		GS_MAILBOX_MINE_REQ gS_MAILBOX_MINE_REQ = new GS_MAILBOX_MINE_REQ();
		gS_MAILBOX_MINE_REQ.i32MailType_Begin = i32MailType_Begin;
		gS_MAILBOX_MINE_REQ.i32MailType_End = i32MailType_End;
		gS_MAILBOX_MINE_REQ.i32Page = this.m_iRecvCurPage;
		gS_MAILBOX_MINE_REQ.i32PageSize = (int)PostDlg.RECV_LISTNUM;
		gS_MAILBOX_MINE_REQ.nFirstMailID = this.m_nFirstMailID;
		gS_MAILBOX_MINE_REQ.nLastMailID = this.m_nLastMailID;
		gS_MAILBOX_MINE_REQ.i8MailType = (byte)this.m_eMailType;
		gS_MAILBOX_MINE_REQ.bNextRequest = this.m_bNextRequest;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_MINE_REQ, gS_MAILBOX_MINE_REQ);
	}

	public void RequestNextRecvList()
	{
		this.requestMail = true;
		PostDlg.CMailRequireRange cMailRequireRange = this.JudgeMailType(this._eLastSelectedType_send);
		GS_MAILBOX_MINE_REQ gS_MAILBOX_MINE_REQ = new GS_MAILBOX_MINE_REQ();
		gS_MAILBOX_MINE_REQ.i32MailType_Begin = cMailRequireRange.i32MailType_Begin;
		gS_MAILBOX_MINE_REQ.i32MailType_End = cMailRequireRange.i32MailType_End;
		gS_MAILBOX_MINE_REQ.i8MailType = (byte)this.m_eMailType;
		gS_MAILBOX_MINE_REQ.i32Page = this.m_iRecvCurPage;
		gS_MAILBOX_MINE_REQ.i32PageSize = (int)PostDlg.RECV_LISTNUM;
		if (this.m_iRecvCurPage == 1)
		{
			gS_MAILBOX_MINE_REQ.nFirstMailID = 0L;
			gS_MAILBOX_MINE_REQ.nLastMailID = 0L;
		}
		else if (this.m_iRecvCount == 1)
		{
			int num = this.m_iRecvCurPage - 2;
			if (0 >= num)
			{
				num = 1;
			}
			gS_MAILBOX_MINE_REQ.nFirstMailID = 0L;
			gS_MAILBOX_MINE_REQ.nLastMailID = this.m_mapPrevPageMailID[num];
			this.m_iRecvCurPage--;
		}
		else
		{
			gS_MAILBOX_MINE_REQ.nFirstMailID = 0L;
			gS_MAILBOX_MINE_REQ.nLastMailID = this.m_mapPrevPageMailID[this.m_iRecvCurPage - 1];
		}
		this.m_nMaxMailNum--;
		gS_MAILBOX_MINE_REQ.bNextRequest = true;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_MINE_REQ, gS_MAILBOX_MINE_REQ);
	}

	private void RequestDetailInfo(long i64MailIDX)
	{
		int num = 0;
		for (int i = 0; i < 30; i++)
		{
			GS_MAILBOX_INFO gS_MAILBOX_INFO = this.m_RecvList[i];
			if (gS_MAILBOX_INFO != null)
			{
				if (gS_MAILBOX_INFO.i64MailID == i64MailIDX)
				{
					num = gS_MAILBOX_INFO.i32MailType;
					this.m_i32SelectedMailType = num;
				}
			}
		}
		if (num > 0 && num < 205)
		{
			GS_MAILBOX_MAILINFO_REQ gS_MAILBOX_MAILINFO_REQ = new GS_MAILBOX_MAILINFO_REQ();
			gS_MAILBOX_MAILINFO_REQ.nMailBoxIdx = i64MailIDX;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_MAILINFO_REQ, gS_MAILBOX_MAILINFO_REQ);
		}
		else
		{
			GS_MAILBOX_MAILINFO_REPORT_REQ gS_MAILBOX_MAILINFO_REPORT_REQ = new GS_MAILBOX_MAILINFO_REPORT_REQ();
			gS_MAILBOX_MAILINFO_REPORT_REQ.i64MailIDX = i64MailIDX;
			gS_MAILBOX_MAILINFO_REPORT_REQ.i32MailType = num;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_MAILINFO_REPORT_REQ, gS_MAILBOX_MAILINFO_REPORT_REQ);
		}
	}

	private void RequestHistoryInfo(MAILBOXHISTORY_INFO MailInfo)
	{
		GS_MAILBOX_HISTORY_REQ gS_MAILBOX_HISTORY_REQ = new GS_MAILBOX_HISTORY_REQ();
		gS_MAILBOX_HISTORY_REQ.i64MailID = MailInfo.i64MailID;
		gS_MAILBOX_HISTORY_REQ.i32MailType = MailInfo.i32MailType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_HISTORY_REQ, gS_MAILBOX_HISTORY_REQ);
	}

	public override void OnClose()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
		this.Close_PostRecvDlg();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INVENTORY_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POSTFRIEND_DLG);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAIL", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void On_Mouse_Over(IUIObject a_oObject)
	{
	}

	private void On_Mouse_Out(IUIObject a_oObject)
	{
	}

	public void SetItem(ITEM item)
	{
		this.m_nItemUnique_SEND = item.m_nItemUnique;
		this.m_i32ItemCurNum_SEND = item.m_nItemNum;
		this.m_i32PosType_SEND = item.m_nPosType;
		this.m_i32PosItem_SEND = item.m_nItemPos;
		this.m_i32ItemSendNum_SEND = 1;
		string empty = string.Empty;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("889");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count1",
			1
		});
		if (NrTSingleton<ItemManager>.Instance.IsStack(item.m_nItemUnique))
		{
			InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.InputNumberDlg_OnApply), item, new Action<InputNumberDlg, object>(this.InputNumberDlg_OnCancel), item);
			inputNumberDlg.SetMinMax(1L, (long)item.m_nItemNum);
			inputNumberDlg.SetNum(1L);
			inputNumberDlg.SetScreenCenter();
			inputNumberDlg.Show();
		}
	}

	private void On_Mouse_Click(IUIObject a_oObject)
	{
		Inventory_Dlg inventory_Dlg = base.SetChildForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			inventory_Dlg.Show();
		}
	}

	private void On_Send_History_Select(IUIObject a_cUIObject)
	{
	}

	private void On_Send_History_Button(IUIObject a_cUIObject)
	{
	}

	private void Refresh_Send_History()
	{
	}

	private void Get_SendHistory()
	{
	}

	private void OnClickOpenCommunity_InPost(IUIObject obj)
	{
	}

	public void ConfirmRequestByName(string strName)
	{
		this.m_UserName.ClearDefaultText(this.m_UserName);
		this.m_UserName.Text = strName;
		this.ChangeTab_Send();
		this.CheckCharName();
	}

	private void BtnSort_Recv(IUIObject obj)
	{
	}

	private void BtnSort_History(IUIObject obj)
	{
	}

	private PostDlg.CMailRequireRangeForHistory JudgeMailType_ForHistory(int Type)
	{
		PostDlg.E_HISTORY_FILTERTYPE eFilterType = PostDlg.E_HISTORY_FILTERTYPE.ALL;
		PostDlg.SORT_TABMENU_HISTORY sORT_TABMENU_HISTORY = (PostDlg.SORT_TABMENU_HISTORY)Type;
		switch (sORT_TABMENU_HISTORY)
		{
		case PostDlg.SORT_TABMENU_HISTORY.ALL:
			break;
		case PostDlg.SORT_TABMENU_HISTORY.RECV:
		case PostDlg.SORT_TABMENU_HISTORY.SEND:
			Type = 0;
			if (sORT_TABMENU_HISTORY == PostDlg.SORT_TABMENU_HISTORY.RECV)
			{
				eFilterType = PostDlg.E_HISTORY_FILTERTYPE.RECV;
			}
			else
			{
				eFilterType = PostDlg.E_HISTORY_FILTERTYPE.SEND;
			}
			break;
		default:
			Type = sORT_TABMENU_HISTORY - PostDlg.SORT_TABMENU_HISTORY.RECV;
			break;
		}
		PostDlg.CMailRequireRange cMailRequireRange = this.JudgeMailType((PostDlg.SORT_TABMENU_RECV)Type);
		return new PostDlg.CMailRequireRangeForHistory
		{
			i32MailType_Begin = cMailRequireRange.i32MailType_Begin,
			i32MailType_End = cMailRequireRange.i32MailType_End,
			eFilterType = eFilterType
		};
	}

	private PostDlg.CMailRequireRange JudgeMailType(PostDlg.SORT_TABMENU_RECV _tapSortType)
	{
		PostDlg.CMailRequireRange cMailRequireRange = new PostDlg.CMailRequireRange();
		switch (_tapSortType)
		{
		case PostDlg.SORT_TABMENU_RECV.ALL:
			cMailRequireRange.i32MailType_Begin = 0;
			cMailRequireRange.i32MailType_End = 10001;
			break;
		case PostDlg.SORT_TABMENU_RECV.POST:
			cMailRequireRange.i32MailType_Begin = 200;
			cMailRequireRange.i32MailType_End = 204;
			break;
		case PostDlg.SORT_TABMENU_RECV.SYSTEM:
			cMailRequireRange.i32MailType_Begin = 100;
			cMailRequireRange.i32MailType_End = 163;
			break;
		}
		return cMailRequireRange;
	}

	public int GetRecvListSelectedMailtype()
	{
		return this.m_i32SelectedMailType;
	}

	public override void Set_Value(object a_oObject)
	{
		base.Set_Value(a_oObject);
		this.SendItemSet(a_oObject as ITEM);
		this.m_objSendItem = a_oObject;
	}

	public static void SetImageSlotFromItem(ref ImageSlot targetSlot, ITEM srcItem, short a_shPosItem)
	{
		if (srcItem != null)
		{
			targetSlot.c_oItem = srcItem;
			targetSlot.c_bEnable = true;
			targetSlot.Index = srcItem.m_nItemPos;
			targetSlot.imageStr = "Win_T_ItemEmpty";
			targetSlot.itemunique = srcItem.m_nItemUnique;
			if (srcItem.m_nItemNum > 1)
			{
				targetSlot.SlotInfo._visibleNum = true;
			}
			if (Protocol_Item.Is_EquipItem(srcItem.m_nItemUnique))
			{
				targetSlot.SlotInfo._visibleRank = true;
			}
			targetSlot.SlotInfo.Set(srcItem.m_nItemNum.ToString(), "+" + srcItem.m_nRank.ToString());
		}
		else
		{
			targetSlot.c_oItem = null;
			targetSlot.Index = (int)a_shPosItem;
			targetSlot.imageStr = "Win_T_ItemEmpty";
			targetSlot.SlotInfo.Set("0", "0");
		}
	}

	public override void Update()
	{
		if (this.requestMail)
		{
			this.m_LoadingImg.Visible = true;
			this.m_LoadingTxt.Visible = true;
			this.m_LoadingImg.Rotate(5f);
		}
		else
		{
			this.m_LoadingImg.Visible = false;
			this.m_LoadingTxt.Visible = false;
		}
	}

	public bool IsItemCheck(ITEM SourceItem)
	{
		bool result = false;
		if (SourceItem == this.m_objSendItem as ITEM)
		{
			result = true;
		}
		return result;
	}

	public void SetSendState(PostDlg.eSEND_STATE eSendState)
	{
		this.m_eSendState = eSendState;
		this.SetDailyMailCount();
		if (eSendState != PostDlg.eSEND_STATE.eSEND_STATE_NORMAL)
		{
			if (eSendState == PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD)
			{
				this.m_FriendList.SetEnabled(false);
				this._tbTab.Control_Tab[0].controlIsEnabled = true;
				this._tbTab.Control_Tab[1].controlIsEnabled = false;
				this._tbTab.Control_Tab[2].controlIsEnabled = false;
				this.m_lbPushText2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1940"));
				this.m_UserName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1939"));
				this.m_UserName.controlIsEnabled = false;
				this.m_cbPushMsg.SetCheckState(1);
			}
		}
		else
		{
			this.m_FriendList.SetEnabled(true);
			this._tbTab.Control_Tab[0].controlIsEnabled = true;
			this._tbTab.Control_Tab[1].controlIsEnabled = true;
			this._tbTab.Control_Tab[2].controlIsEnabled = true;
			this.m_lbPushText.Visible = true;
			this.m_lbPushText2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1351"));
			this.m_UserName.SetText(string.Empty);
			this.m_UserName.controlIsEnabled = true;
			this.m_cbPushMsg.SetCheckState(0);
		}
	}
}
