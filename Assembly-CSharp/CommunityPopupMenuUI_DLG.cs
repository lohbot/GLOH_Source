using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class CommunityPopupMenuUI_DLG : Form
{
	public enum eCOMMUNITYMENU
	{
		eCOMMUNITYMENU_NONE,
		eCOMMUNITYMENU_SHOW_DETAIL_INFO,
		eCOMMUNITYMENU_WHISPER,
		eCOMMUNITYMENU_INVITE_PARTY,
		eCOMMUNITYMENU_FOLLOW,
		eCOMMUNITYMENU_POSTSEND,
		eCOMMUNITYMENU_FRIEND_DEL,
		eCOMMUNITYMENU_WARP,
		eCOMMUNITYMENU_END
	}

	private const int MAX_POPUPMENU_ITEM_CONT = 10;

	private ListBox m_LBList;

	private Rect m_rcWindow = new Rect(0f, 0f, 0f, 0f);

	private int m_siCount;

	private long m_nPersonID = -1L;

	private string m_SelectFriendName = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Community/DLG_CommunityPopupMenu", G_ID.COMMUNITYPOPUPMENU_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_LBList = (base.GetControl("ListBox_Menu") as ListBox);
		this.m_LBList.ColumnNum = 1;
		this.m_LBList.SetColumnWidth((int)base.GetSize().x, 0, 0, 0, 0);
		ListBox expr_46 = this.m_LBList;
		expr_46.SelectionChange = (EZValueChangedDelegate)Delegate.Combine(expr_46.SelectionChange, new EZValueChangedDelegate(this.ListBoxSelect));
		if (this.m_LBList.slider)
		{
			UnityEngine.Object.Destroy(this.m_LBList.slider.gameObject);
		}
		float x = NkInputManager.mousePosition.x;
		float num = GUICamera.height - NkInputManager.mousePosition.y;
		base.SetLocation((float)((int)x), (float)((int)num));
		this.m_rcWindow = new Rect(base.GetLocation().x, base.GetLocationY(), base.GetSize().x, base.GetSize().y);
		this.Initialize();
		base.Draggable = false;
	}

	public void Initialize()
	{
		this.m_siCount = 0;
		this.m_LBList.Clear();
		this.m_LBList.SelectIndex = -1;
		this.m_nPersonID = -1L;
		this.m_SelectFriendName = string.Empty;
		this.Hide();
	}

	public void ShowMenu(int _type, int _index, long _personid, string SelectFriendName)
	{
		this.Initialize();
		this.m_nPersonID = _personid;
		this.m_SelectFriendName = SelectFriendName;
		if (_type == 0)
		{
			switch (_index)
			{
			case 0:
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("346"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FOLLOW);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("347"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WHISPER);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_POSTSEND);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("465"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_INVITE_PARTY);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FRIEND_DEL);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_SHOW_DETAIL_INFO);
				break;
			case 1:
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("346"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FOLLOW);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("347"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WHISPER);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("465"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_INVITE_PARTY);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FRIEND_DEL);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_SHOW_DETAIL_INFO);
				break;
			case 2:
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FRIEND_DEL);
				break;
			}
		}
		else if (_type == 1)
		{
			if (_index == 0)
			{
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("346"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FOLLOW);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("347"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WHISPER);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_POSTSEND);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("465"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_INVITE_PARTY);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_SHOW_DETAIL_INFO);
			}
		}
		else if (_type == 2)
		{
			if (_index == 0)
			{
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("346"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FOLLOW);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("347"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WHISPER);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_POSTSEND);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("465"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_INVITE_PARTY);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_SHOW_DETAIL_INFO);
			}
		}
		else if (_type == 3)
		{
			if (_index == 0)
			{
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("346"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_FOLLOW);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("347"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WHISPER);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_POSTSEND);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("465"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_INVITE_PARTY);
				this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_SHOW_DETAIL_INFO);
			}
		}
		this.ReSizeDialog();
		this.Show();
	}

	private void ReSizeDialog()
	{
		this.m_LBList.RepositionItems();
		int num = 20 * this.m_siCount;
		base.SetSize(float.Parse(this.m_rcWindow.width.ToString()), (float)num);
		this.m_LBList.SetSize(float.Parse(this.m_rcWindow.width.ToString()), (float)num);
		this.m_LBList.SetLocation(0, 0);
		this.m_LBList.ResizeViewableArea();
	}

	public void ControlMenu_Add(string _text, CommunityPopupMenuUI_DLG.eCOMMUNITYMENU menutype)
	{
		if (string.Empty == _text)
		{
			return;
		}
		if (this.m_siCount >= 10)
		{
			return;
		}
		ListItem listItem = new ListItem();
		string szColorNum = (menutype != CommunityPopupMenuUI_DLG.eCOMMUNITYMENU.eCOMMUNITYMENU_WARP) ? "1101" : "1102";
		listItem.SetColumnStr(0, _text, NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum));
		listItem.Key = menutype;
		this.m_LBList.Add(listItem);
		this.m_siCount++;
	}

	public int GetAddCount()
	{
		return this.m_siCount;
	}

	public void SetPosition(int _x, int _y)
	{
		base.SetLocation((float)_x, (float)_y);
	}

	private void FriendDelYes(object a_oObject)
	{
		GS_DEL_FRIEND_REQ gS_DEL_FRIEND_REQ = new GS_DEL_FRIEND_REQ();
		gS_DEL_FRIEND_REQ.i64FriendPersonID = this.m_nPersonID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_DEL_FRIEND_REQ, gS_DEL_FRIEND_REQ);
	}

	private void ListBoxSelect(IUIObject obj)
	{
		ListBox listBox = (ListBox)obj;
		UIListItemContainer selectedItem = listBox.SelectedItem;
		if (selectedItem == null)
		{
			Debug.LogError("CommunityPopupMenuUI_DLG.cs --ListBoxSelectUIListItemContainer l_listitem == null");
			return;
		}
		switch ((int)selectedItem.Data)
		{
		case 1:
		{
			GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
			gS_OTHERCHAR_INFO_PERMIT_REQ.nPersonID = this.m_nPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
			break;
		}
		case 2:
		{
			NrCharBase charByPersonID = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(this.m_nPersonID);
			Debug.Log("eCOMMUNITYMENU_WHISPER >> nPersonID : " + this.m_nPersonID.ToString() + " Name : " + charByPersonID.GetCharName());
			GS_WHISPER_REQ gS_WHISPER_REQ = new GS_WHISPER_REQ();
			gS_WHISPER_REQ.RoomUnique = 0;
			TKString.StringChar(charByPersonID.GetCharName(), ref gS_WHISPER_REQ.Name);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_REQ, gS_WHISPER_REQ);
			break;
		}
		case 4:
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null && this.m_nPersonID > 0L)
			{
				nrCharUser.SetFollowCharPersonID(this.m_nPersonID, this.m_SelectFriendName);
			}
			break;
		}
		case 5:
		{
			NrCharBase charByPersonID2 = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(this.m_nPersonID);
			if (charByPersonID2 != null)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG);
			}
			break;
		}
		case 6:
			if (this.m_SelectFriendName != null && this.m_nPersonID != -1L)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("8");
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromMessageBox,
						"Charname",
						this.m_SelectFriendName
					});
					msgBoxUI.SetMsg(new YesDelegate(this.FriendDelYes), null, textFromInterface, empty, eMsgType.MB_OK_CANCEL);
				}
			}
			break;
		case 7:
			return;
		}
		this.Close();
	}
}
