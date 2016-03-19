using PROTOCOL;
using System;
using System.Collections.Generic;

public class WhisperRoom
{
	private int RoomUnique;

	private List<WhisperUser> UserList = new List<WhisperUser>();

	private string RoomName = string.Empty;

	private string LastChatUser = string.Empty;

	private Queue<ChatMsg> ChatMsgQueue = new Queue<ChatMsg>();

	private bool m_bCheckMsg;

	private bool bActiveRoom;

	private string m_MainUserName = string.Empty;

	private DateTime m_LastMsgTime;

	public int Room
	{
		get
		{
			return this.RoomUnique;
		}
		private set
		{
		}
	}

	public bool CheckMSG
	{
		get
		{
			return this.m_bCheckMsg;
		}
		set
		{
			this.m_bCheckMsg = value;
		}
	}

	public WhisperRoom(int unique)
	{
		this.RoomUnique = unique;
	}

	public List<WhisperUser> GetUsers()
	{
		return this.UserList;
	}

	public void SetRoomName()
	{
		foreach (WhisperUser current in this.GetUsers())
		{
			if (current.PersonID != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
			{
				this.RoomName = current.Name;
				this.m_MainUserName = current.Name;
				break;
			}
		}
		if (this.UserList.Count > 2)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2034"),
				"count",
				(this.UserList.Count - 1).ToString()
			});
			if (this.RoomName.Length < 5)
			{
				this.RoomName += empty;
			}
			else
			{
				this.RoomName = this.RoomName.Substring(0, 5);
				this.RoomName = this.RoomName + ".." + empty;
			}
		}
	}

	public string GetRoomName()
	{
		return this.RoomName;
	}

	public void SetLastUser(string UserName)
	{
		this.LastChatUser = UserName;
	}

	public string GetLastUser()
	{
		return this.LastChatUser;
	}

	public void PushChat(string name, string msg, int color)
	{
		this.ChatMsgQueue.Enqueue(new ChatMsg(name, msg, color));
		if (this.ChatMsgQueue.Count > 50)
		{
			this.ChatMsgQueue.Dequeue();
		}
		if (name != string.Empty && !this.m_MainUserName.Equals(name))
		{
			this.m_LastMsgTime = PublicMethod.GetNowTime();
		}
	}

	public DateTime ReciveLastMessageTime()
	{
		return this.m_LastMsgTime;
	}

	public Queue<ChatMsg> GetMsgQueue()
	{
		return this.ChatMsgQueue;
	}

	public void SetActive()
	{
		this.bActiveRoom = true;
	}

	public bool IsActive()
	{
		return this.bActiveRoom;
	}

	public void SetUserState(byte _State, long _PersonID = 0L)
	{
		if (_PersonID == 0L)
		{
			_PersonID = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_PersonID;
		}
		foreach (WhisperUser current in this.UserList)
		{
			if (current.PersonID == _PersonID)
			{
				current.byPlayState = _State;
				break;
			}
		}
	}
}
