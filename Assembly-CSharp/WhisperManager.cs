using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class WhisperManager : NrTSingleton<WhisperManager>
{
	private Dictionary<int, WhisperRoom> RoomList;

	private string m_SetChatColor = "1101";

	private byte m_byUserState;

	private bool m_bMySendRequest;

	private bool m_bWindowClose;

	public string ChatColor
	{
		get
		{
			return this.m_SetChatColor;
		}
		set
		{
			this.m_SetChatColor = value;
		}
	}

	public byte UserState
	{
		get
		{
			return this.m_byUserState;
		}
		set
		{
			this.m_byUserState = value;
		}
	}

	public bool MySendRequest
	{
		get
		{
			return this.m_bMySendRequest;
		}
		set
		{
			this.m_bMySendRequest = value;
		}
	}

	public bool WindowClose
	{
		get
		{
			return this.m_bWindowClose;
		}
		set
		{
			this.m_bWindowClose = value;
		}
	}

	private WhisperManager()
	{
		this.RoomList = new Dictionary<int, WhisperRoom>();
	}

	public bool AddRoom(int roomunique)
	{
		if (!this.RoomList.ContainsKey(roomunique))
		{
			this.RoomList.Add(roomunique, new WhisperRoom(roomunique));
			this.ChangeRoom(roomunique);
			Debug.Log("Add Private Chat Room - Unique : " + roomunique.ToString());
			this.PushText(roomunique, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94"), NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("66"), 1306, true);
			return true;
		}
		return false;
	}

	public void DelRoom(int roomunique)
	{
		if (!this.RoomList.ContainsKey(roomunique))
		{
			return;
		}
		this.RoomList.Remove(roomunique);
		if (this.RoomList.Count == 0)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_ROOMLIST_DLG);
			NoticeIconDlg.SetIcon(ICON_TYPE.WHISPER, false);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
		}
		else
		{
			using (Dictionary<int, WhisperRoom>.ValueCollection.Enumerator enumerator = this.RoomList.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					WhisperRoom current = enumerator.Current;
					this.ChangeRoom(current.Room);
				}
			}
		}
		GS_CHAT_EXIT_REQ gS_CHAT_EXIT_REQ = new GS_CHAT_EXIT_REQ();
		gS_CHAT_EXIT_REQ.nRoomUnique = roomunique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAT_EXIT_REQ, gS_CHAT_EXIT_REQ);
	}

	public bool AddUser(int roomunique, long personid, string name, byte PlayState, int FaceKind, bool request)
	{
		bool result = false;
		if (this.RoomList.ContainsKey(roomunique))
		{
			foreach (WhisperUser current in this.RoomList[roomunique].GetUsers())
			{
				if (current.PersonID == personid)
				{
					return false;
				}
			}
			this.RoomList[roomunique].GetUsers().Add(new WhisperUser(personid, name, PlayState, FaceKind));
			Debug.Log(string.Concat(new object[]
			{
				"Add Private Chat User - name : ",
				name,
				" PlayState = ",
				PlayState
			}));
			result = true;
		}
		else if (this.AddRoom(roomunique))
		{
			this.RoomList[roomunique].GetUsers().Add(new WhisperUser(personid, name, PlayState, FaceKind));
			Debug.Log(string.Concat(new object[]
			{
				"Add Private Chat User - name : ",
				name,
				" PlayState = ",
				PlayState
			}));
			this.UpdateDlg(roomunique);
			result = true;
		}
		if (this.MySendRequest && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID == personid)
		{
			this.RoomList[roomunique].SetActive();
			this.m_bWindowClose = false;
			this.ShowWhisperDlg();
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.WHISPER_DLG);
		}
		return result;
	}

	public void DelUser(int roomunique, long personid)
	{
		if (!this.RoomList.ContainsKey(roomunique))
		{
			return;
		}
		if (personid == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
		{
			this.DelRoom(roomunique);
			return;
		}
		foreach (WhisperUser current in this.RoomList[roomunique].GetUsers())
		{
			if (current.PersonID == personid)
			{
				if (this.RoomList[roomunique].GetUsers().Count == 2)
				{
					this.RoomList[roomunique].SetLastUser(current.Name);
				}
				this.RoomList[roomunique].GetUsers().Remove(current);
				this.UpdateDlg(roomunique);
				break;
			}
		}
	}

	public void Clear()
	{
		foreach (WhisperRoom current in this.RoomList.Values)
		{
			current.GetUsers().Clear();
		}
		this.RoomList.Clear();
	}

	public bool PushText(int unique, string name, string msg, int color, bool systemmsg)
	{
		if (!this.RoomList.ContainsKey(unique))
		{
			Debug.LogWarning("(귓말) 생성된 방이 없습니다. unique : " + unique.ToString());
			return false;
		}
		if (!this.RoomList[unique].IsActive() && !systemmsg)
		{
			this.RoomList[unique].SetActive();
			if (this.RoomList.Count <= 1)
			{
				this.m_bWindowClose = false;
				this.ShowWhisperDlg();
			}
		}
		if (color == 0)
		{
			color = 1101;
		}
		this.RoomList[unique].PushChat(name, msg, color);
		this.RoomList[unique].CheckMSG = false;
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null && new_Whisper_Dlg.GetCurRoomUnique() == unique)
		{
			new_Whisper_Dlg.PushMsg(unique, name, msg, color.ToString());
			if (new_Whisper_Dlg.visible)
			{
				this.RoomList[unique].CheckMSG = true;
			}
			else
			{
				this.m_bWindowClose = false;
				this.ShowWhisperDlg();
			}
		}
		Debug.LogWarning(string.Concat(new object[]
		{
			"Add Private Chat Msg - name : ",
			name,
			" ,msg : ",
			msg,
			", CheckMsg :",
			this.RoomList[unique].CheckMSG,
			" RoomUnique = ",
			unique
		}));
		return true;
	}

	public List<WhisperUser> GetRoomUsers(int unique)
	{
		if (!this.RoomList.ContainsKey(unique))
		{
			return null;
		}
		return this.RoomList[unique].GetUsers();
	}

	public void UpdateDlg(int roomunique)
	{
		if (this.RoomList.ContainsKey(roomunique))
		{
			this.RoomList[roomunique].SetRoomName();
			New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
			if (new_Whisper_Dlg != null)
			{
				Debug.Log("UpdateDlg SetTitle" + this.RoomList[roomunique].GetRoomName());
				new_Whisper_Dlg.AddRoom(roomunique);
				new_Whisper_Dlg.SetTitle(this.RoomList[roomunique]);
				new_Whisper_Dlg.UpdateList(roomunique);
			}
		}
	}

	public Dictionary<int, WhisperRoom> GetRoomList()
	{
		return this.RoomList;
	}

	public WhisperRoom GetRoom(int roomunique)
	{
		if (this.RoomList.ContainsKey(roomunique))
		{
			return this.RoomList[roomunique];
		}
		return null;
	}

	public void ChangeRoom(int roomunique)
	{
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null && new_Whisper_Dlg.ChangeChatRoom(roomunique))
		{
			Debug.Log("Change Chat Room - " + roomunique.ToString());
			this.RoomList[roomunique].CheckMSG = true;
		}
	}

	public void ChangeFirstRoom()
	{
		using (Dictionary<int, WhisperRoom>.ValueCollection.Enumerator enumerator = this.RoomList.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				WhisperRoom current = enumerator.Current;
				this.ChangeRoom(current.Room);
			}
		}
	}

	public void ShowWhisperDlg()
	{
		if (this.RoomList.Count > 0 && !this.m_bWindowClose)
		{
			NoticeIconDlg.SetIcon(ICON_TYPE.WHISPER, true);
		}
	}

	public void SetUserStateChange(long PersonID, int roomunique, byte State)
	{
		if (this.RoomList.ContainsKey(roomunique))
		{
			this.RoomList[roomunique].SetUserState(State, PersonID);
		}
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null && new_Whisper_Dlg.GetCurRoomUnique() == roomunique)
		{
			new_Whisper_Dlg.UpdateList(roomunique);
		}
	}

	public bool IsCheckMSG()
	{
		foreach (int current in this.RoomList.Keys)
		{
			WhisperRoom whisperRoom = this.RoomList[current];
			if (whisperRoom != null && !whisperRoom.CheckMSG)
			{
				return false;
			}
		}
		return true;
	}

	public void WhisperApplyYesDelegate(object a_oObject)
	{
		GS_WHISPER_INVITE_ACK gS_WHISPER_INVITE_ACK = (GS_WHISPER_INVITE_ACK)a_oObject;
		if (gS_WHISPER_INVITE_ACK == null)
		{
			return;
		}
		GS_WHISPER_INVITEYESNO_REQ gS_WHISPER_INVITEYESNO_REQ = new GS_WHISPER_INVITEYESNO_REQ();
		gS_WHISPER_INVITEYESNO_REQ.nRoomUnique = gS_WHISPER_INVITE_ACK.nRoomUnique;
		gS_WHISPER_INVITEYESNO_REQ.nSendInvitePersonID = gS_WHISPER_INVITE_ACK.nSendInvitePersonID;
		gS_WHISPER_INVITEYESNO_REQ.i8YesNo = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_INVITEYESNO_REQ, gS_WHISPER_INVITEYESNO_REQ);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
	}

	public void WhisperApplyNoDelegate(object a_oObject)
	{
		GS_WHISPER_INVITE_ACK gS_WHISPER_INVITE_ACK = (GS_WHISPER_INVITE_ACK)a_oObject;
		if (gS_WHISPER_INVITE_ACK == null)
		{
			return;
		}
		GS_WHISPER_INVITEYESNO_REQ gS_WHISPER_INVITEYESNO_REQ = new GS_WHISPER_INVITEYESNO_REQ();
		gS_WHISPER_INVITEYESNO_REQ.nRoomUnique = gS_WHISPER_INVITE_ACK.nRoomUnique;
		gS_WHISPER_INVITEYESNO_REQ.nSendInvitePersonID = gS_WHISPER_INVITE_ACK.nSendInvitePersonID;
		gS_WHISPER_INVITEYESNO_REQ.i8YesNo = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_INVITEYESNO_REQ, gS_WHISPER_INVITEYESNO_REQ);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
	}

	public void SendUserState(int State)
	{
		this.UserState = (byte)State;
		foreach (WhisperRoom current in this.RoomList.Values)
		{
			GS_WHISPER_STATE_CHANGE_REQ gS_WHISPER_STATE_CHANGE_REQ = new GS_WHISPER_STATE_CHANGE_REQ();
			gS_WHISPER_STATE_CHANGE_REQ.i8UserChatState = (byte)State;
			gS_WHISPER_STATE_CHANGE_REQ.nRoomUnique = current.Room;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_STATE_CHANGE_REQ, gS_WHISPER_STATE_CHANGE_REQ);
			current.SetUserState(this.UserState, 0L);
		}
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null)
		{
			new_Whisper_Dlg.SetUserStateText(this.UserState);
			new_Whisper_Dlg.UpdateList(new_Whisper_Dlg.GetCurRoomUnique());
		}
	}

	public static string GetStatText(byte State)
	{
		string strTextKey = string.Empty;
		switch (State)
		{
		case 0:
			strTextKey = "2142";
			break;
		case 1:
			strTextKey = "2143";
			break;
		case 2:
			strTextKey = "2144";
			break;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey);
	}
}
