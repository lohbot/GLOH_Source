using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class WhisperPopupMenu : Form
{
	private DrawTexture m_BG;

	private ListBox m_List;

	private int m_nCount;

	private eWHISPER_POPUP_TYPE m_ShowType;

	private WhisperUser m_WhisperUser;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Whisper/dlg_whisperclick", G_ID.WHISPER_WHISPERPOPUPMENU_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_List = (base.GetControl("ListBox_ListBox0") as ListBox);
		this.m_List.LineHeight = 80f;
		this.m_List.ColumnNum = 1;
		this.m_List.SetColumnWidth((int)base.GetSize().x, 0, 0, 0, 0, 26f);
		ListBox expr_5B = this.m_List;
		expr_5B.SelectionChange = (EZValueChangedDelegate)Delegate.Combine(expr_5B.SelectionChange, new EZValueChangedDelegate(this.OnClickList));
		this.m_BG = (base.GetControl("DrawTexture_ListBG1") as DrawTexture);
		base.Draggable = false;
	}

	public void SetClickType(eWHISPER_POPUP_TYPE _type, WhisperUser _user = null)
	{
		this.m_ShowType = _type;
		switch (_type)
		{
		case eWHISPER_POPUP_TYPE.ROOMLIST:
			this.RoomList();
			break;
		case eWHISPER_POPUP_TYPE.STATE:
			this.StateList();
			break;
		case eWHISPER_POPUP_TYPE.GROUPLIST:
			this.m_WhisperUser = _user;
			this.GroupList();
			break;
		}
		this.ReSizeDialog();
		if (_type != eWHISPER_POPUP_TYPE.ROOMLIST)
		{
			this.SetMousePosition();
		}
	}

	public void RoomList()
	{
		this.m_List.Clear();
		this.m_nCount = 0;
		Dictionary<int, WhisperRoom> roomList = NrTSingleton<WhisperManager>.Instance.GetRoomList();
		foreach (int current in roomList.Keys)
		{
			string roomName = roomList[current].GetRoomName();
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, roomName);
			listItem.Key = current;
			this.m_List.Add(listItem);
			this.m_nCount++;
		}
		this.m_List.RepositionItems();
	}

	private void StateList()
	{
		this.m_List.Clear();
		this.m_nCount = 0;
		int num = 2142;
		for (int i = 0; i < 3; i++)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface((num + i).ToString());
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, textFromInterface);
			listItem.Key = i;
			this.m_List.Add(listItem);
			this.m_nCount++;
		}
		this.m_List.RepositionItems();
	}

	private void GroupList()
	{
		this.m_List.Clear();
		this.m_nCount = 0;
		this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), eWHISPERMENU.eEWHISPERMENU_FRIEND_ADD, "1101");
		this.ControlMenu_Add(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("788"), eWHISPERMENU.eEWHISPERMENU_SHOW_DETAIL_INFO, "1101");
		this.m_List.RepositionItems();
	}

	public void ControlMenu_Add(string _text, eWHISPERMENU menutype, string Color = "1101")
	{
		if (string.Empty == _text)
		{
			return;
		}
		ListItem listItem = new ListItem();
		listItem.SetColumnStr(0, _text, NrTSingleton<CTextParser>.Instance.GetTextColor(Color));
		listItem.Key = menutype;
		this.m_List.Add(listItem);
		this.m_nCount++;
	}

	private void ReSizeDialog()
	{
		float num = 1f;
		if (TsPlatform.IsWeb)
		{
			num = 0.7f;
		}
		this.m_List.RepositionItems();
		int num2 = (int)(this.m_List.LineHeight * (float)this.m_nCount);
		base.SetSize(base.GetSize().x, (float)num2 * num);
		this.m_List.SetSize(base.GetSize().x, (float)num2);
		this.m_List.SetLocation(0, 0);
		this.m_List.ResizeViewableArea();
		this.m_BG.SetSize(base.GetSize().x, (float)num2);
	}

	private void OnClickList(IUIObject obj)
	{
		switch (this.m_ShowType)
		{
		case eWHISPER_POPUP_TYPE.ROOMLIST:
		{
			int roomUnique = (int)this.m_List.SelectedItem.Data;
			this.ClickRoomList(roomUnique);
			break;
		}
		case eWHISPER_POPUP_TYPE.STATE:
		{
			int state = (int)this.m_List.SelectedItem.Data;
			this.ClickStateList(state);
			break;
		}
		case eWHISPER_POPUP_TYPE.GROUPLIST:
		{
			eWHISPERMENU type = (eWHISPERMENU)((int)this.m_List.SelectedItem.Data);
			this.ClickGroupList(type);
			break;
		}
		}
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null)
		{
			new_Whisper_Dlg.InteractivePanel.twinFormID = G_ID.NONE;
		}
		this.Close();
	}

	private void ClickRoomList(int RoomUnique)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_DLG))
		{
			New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
			new_Whisper_Dlg.TogleChange(RoomUnique);
		}
	}

	private void ClickStateList(int State)
	{
		NrTSingleton<WhisperManager>.Instance.SendUserState(State);
	}

	private void ClickGroupList(eWHISPERMENU _type)
	{
		if (_type != eWHISPERMENU.eEWHISPERMENU_FRIEND_ADD)
		{
			if (_type != eWHISPERMENU.eWHISPERMENU_USER_KICK)
			{
				if (_type == eWHISPERMENU.eEWHISPERMENU_SHOW_DETAIL_INFO)
				{
					GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
					gS_OTHERCHAR_INFO_PERMIT_REQ.nPersonID = this.m_WhisperUser.PersonID;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
				}
			}
		}
		else
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser.GetCharName().CompareTo(this.m_WhisperUser.Name) != 0)
			{
				GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
				gS_FRIEND_APPLY_REQ.i32WorldID = 0;
				TKString.StringChar(this.m_WhisperUser.Name, ref gS_FRIEND_APPLY_REQ.name);
				SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
			}
		}
	}

	private void SetMousePosition()
	{
		float x = NkInputManager.mousePosition.x;
		float num = GUICamera.height - NkInputManager.mousePosition.y;
		if (num + base.GetSizeY() > GUICamera.height)
		{
			num = GUICamera.height - base.GetSizeY();
		}
		base.SetLocation((float)((int)x), (float)((int)num));
	}

	public override void AfterShow()
	{
	}
}
