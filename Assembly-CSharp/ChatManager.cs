using GAME;
using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class ChatManager : NrTSingleton<ChatManager>
{
	private const int MAX_CHAT_MSG_QUEUE = 80;

	private Queue<MainChatMsg> ChatMsgQueue = new Queue<MainChatMsg>();

	private Queue<MainChatMsg> ChatMsgList = new Queue<MainChatMsg>();

	private Queue<CharChat> CharChatQueue = new Queue<CharChat>();

	private int[] m_RoomUnique = new int[7];

	public static float SLIDER_WIDTH
	{
		get
		{
			return 12f;
		}
	}

	private ChatManager()
	{
	}

	public void InitChatMsg()
	{
		this.ChatMsgList.Clear();
	}

	public int GetChatMsgCount()
	{
		return this.ChatMsgQueue.Count;
	}

	public int GetCharChatCount()
	{
		return this.CharChatQueue.Count;
	}

	public int GetChatMsgListCount()
	{
		return this.ChatMsgList.Count;
	}

	public MainChatMsg ChatMsgDequeue()
	{
		return this.ChatMsgQueue.Dequeue();
	}

	public CharChat CharChatDequeue()
	{
		return this.CharChatQueue.Dequeue();
	}

	public MainChatMsg GetChatMsgData(int i)
	{
		MainChatMsg[] array = this.ChatMsgList.ToArray();
		if (i >= this.ChatMsgList.Count)
		{
			return null;
		}
		return array[i];
	}

	public void PushMsg(string name, string msg)
	{
		this.PushMsg(CHAT_TYPE.NORMAL, name, msg);
	}

	public void PushMsg(CHAT_TYPE type, string name, string msg)
	{
		MainChatMsg item = new MainChatMsg(type, name, msg, new ITEM(), string.Empty);
		this.ChatMsgQueue.Enqueue(item);
		if (this.ChatMsgList.Count >= 80)
		{
			this.ChatMsgList.Dequeue();
		}
		this.ChatMsgList.Enqueue(item);
	}

	public void PushMsg(int unique, string name, string msg)
	{
		this.PushMsg(this.GetMsgTypeFromUnique(unique), name, msg);
	}

	public void PushSystemMsg(string name, string msg)
	{
		this.PushSystemMsg(name, msg, string.Empty);
	}

	public void PushSystemMsg(string name, string msg, string color)
	{
		MainChatMsg item = new MainChatMsg(CHAT_TYPE.SYSTEM, name, msg, null, color);
		this.ChatMsgQueue.Enqueue(item);
		if (this.ChatMsgList.Count >= 80)
		{
			this.ChatMsgList.Dequeue();
		}
		this.ChatMsgList.Enqueue(item);
	}

	public void PushCharChat(NrCharBase charBase, string msg, CHAT_TYPE _ChatType)
	{
		this.CharChatQueue.Enqueue(new CharChat(charBase, msg, _ChatType));
		if (80 <= this.ChatMsgList.Count)
		{
			this.ChatMsgList.Dequeue();
		}
		if (charBase.GetCharName() == NrTSingleton<NkCharManager>.Instance.GetCharName())
		{
			MainChatMsg item = new MainChatMsg(_ChatType, charBase.GetCharName(), msg, new ITEM(), string.Empty);
			this.ChatMsgList.Enqueue(item);
		}
	}

	public void SendMessage(CHAT_TYPE type, string strText)
	{
		this.SendMessage(type, strText, false, null, 0, 0L, 0);
	}

	public void SendMessage(CHAT_TYPE type, string strText, bool useItemLinkText, ITEM linkItem, short color = 0, long babelLeaderPersonID = 0L, int roomUnique = 0)
	{
		if (type == CHAT_TYPE.SYSTEM)
		{
			type = CHAT_TYPE.NORMAL;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		GS_CHAT_REQ gS_CHAT_REQ = new GS_CHAT_REQ();
		gS_CHAT_REQ.ChatType = (byte)type;
		gS_CHAT_REQ.ColoseumGrade = myCharInfo.ColosseumGrade;
		gS_CHAT_REQ.Color = color;
		gS_CHAT_REQ.nBabelLeaderPersonID = babelLeaderPersonID;
		if (roomUnique == 0)
		{
			gS_CHAT_REQ.RoomUnique = this.m_RoomUnique[(int)type];
		}
		else
		{
			gS_CHAT_REQ.RoomUnique = roomUnique;
		}
		if (gS_CHAT_REQ.RoomUnique < 0)
		{
			return;
		}
		if (useItemLinkText)
		{
			if (linkItem == null)
			{
				return;
			}
			gS_CHAT_REQ.LinkItem = linkItem;
		}
		TKMarshal.StringChar(strText, ref gS_CHAT_REQ.szChatStr);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAT_REQ, gS_CHAT_REQ);
	}

	public void SendWhisperCommand(string strMsg, CHAT_TYPE type)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		GS_CHAT_REQ gS_CHAT_REQ = new GS_CHAT_REQ();
		gS_CHAT_REQ.ColoseumGrade = myCharInfo.ColosseumGrade;
		gS_CHAT_REQ.RoomUnique = this.GetUniqueFromMegType(type);
		if (gS_CHAT_REQ.RoomUnique < 0)
		{
			return;
		}
		TKMarshal.StringChar(strMsg, ref gS_CHAT_REQ.szChatStr);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAT_REQ, gS_CHAT_REQ);
	}

	public void MakeChatText(Form form, CHAT_TYPE type, short colosseumGrade, string strText, ITEM linkItem)
	{
		if (type == CHAT_TYPE.SYSTEM)
		{
			type = CHAT_TYPE.NORMAL;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null)
		{
			char c = TKString.StringChar("/")[0];
			if (!string.IsNullOrEmpty(strText))
			{
				char[] array = strText.ToCharArray(0, 1);
				if (c.CompareTo(array[0]) == 0)
				{
					return;
				}
			}
			ChatLabel chatLabel = null;
			ChatLabel chatLabel2 = null;
			if (TsPlatform.IsWeb)
			{
				MainChatDlg mainChatDlg = form as MainChatDlg;
				if (mainChatDlg != null)
				{
					chatLabel = mainChatDlg.GetChatLable(type);
					chatLabel2 = mainChatDlg.GetChatLable(CHAT_TYPE.ALL);
				}
			}
			else
			{
				ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = form as ChatMobile_Sub_Dlg;
				if (chatMobile_Sub_Dlg != null)
				{
					chatLabel = chatMobile_Sub_Dlg.GetChatLable(type);
					chatLabel2 = chatMobile_Sub_Dlg.GetChatLable(CHAT_TYPE.ALL);
				}
			}
			string name = string.Empty;
			if (chatLabel2 != null)
			{
				name = ChatManager.GetChatFrontString(nrCharUser.GetCharName(), colosseumGrade, type, false);
				chatLabel2.PushText(name, strText, ChatManager.GetChatColorKey(type), linkItem);
				if (null != chatLabel && type != CHAT_TYPE.NORMAL)
				{
					chatLabel.PushText(name, strText, ChatManager.GetChatColorKey(type), linkItem);
				}
			}
			if (TsPlatform.IsMobile)
			{
				ChatMobileDlg chatMobileDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg;
				if (chatMobileDlg != null)
				{
					chatLabel2 = chatMobileDlg.GetChatLable(CHAT_TYPE.ALL);
					if (chatLabel2 != null)
					{
						chatLabel2.PushText(name, strText, ChatManager.GetChatColorKey(type), linkItem);
					}
					if (80 <= this.ChatMsgList.Count)
					{
						this.ChatMsgList.Dequeue();
					}
					this.ChatMsgList.Enqueue(new MainChatMsg(type, ChatManager.GetChatNameStr(nrCharUser.GetCharName(), colosseumGrade, false), strText, linkItem, ChatManager.GetChatColorKey(type)));
				}
				TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
				if (tournamentLobbyDlg != null)
				{
					chatLabel2 = tournamentLobbyDlg.GetChatLable(CHAT_TYPE.ALL);
					if (chatLabel2 != null)
					{
						chatLabel2.PushText(name, strText, ChatManager.GetChatColorKey(type), linkItem);
					}
				}
			}
			nrCharUser.MakeChatText(strText, true);
		}
	}

	public void SetRoomUnique(int type, int unique)
	{
		if (type < 0 || type > this.m_RoomUnique.Length)
		{
			return;
		}
		this.m_RoomUnique[type] = unique;
	}

	public CHAT_TYPE GetMsgTypeFromUnique(int unique)
	{
		for (int i = 0; i < this.m_RoomUnique.Length; i++)
		{
			if (this.m_RoomUnique[i] == unique)
			{
				return (CHAT_TYPE)i;
			}
		}
		return CHAT_TYPE.NORMAL;
	}

	public int GetUniqueFromMegType(CHAT_TYPE type)
	{
		return this.m_RoomUnique[(int)type];
	}

	public static void NPCTellChat(string name)
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg != null)
			{
				mainChatDlg.NPCTellChat(name);
			}
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobileDlg chatMobileDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg;
			if (chatMobileDlg != null)
			{
				chatMobileDlg.NPCTellChat(name);
			}
			ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
			if (chatMobile_Sub_Dlg != null)
			{
				chatMobile_Sub_Dlg.NPCTellChat(name);
			}
			TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
			if (tournamentLobbyDlg != null)
			{
				tournamentLobbyDlg.NPCTellChat(name);
			}
		}
	}

	public static ChatLabel MakeChatLabel(Form form, string strName, Vector2 vPos)
	{
		ChatLabel chatLabel = form.GetControl(strName) as ChatLabel;
		if (null == chatLabel)
		{
			return null;
		}
		if (null != chatLabel.slider)
		{
			UnityEngine.Object.Destroy(chatLabel.slider.gameObject);
		}
		chatLabel.Visible = false;
		chatLabel.itemSpacing = 5f;
		return chatLabel;
	}

	public static string GetChatNameStr(string name, short nGrade, bool isLinkName = false)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		bool flag = true;
		switch (nGrade)
		{
		case 3:
			text2 = "2001";
			break;
		case 4:
			text2 = "1104";
			break;
		case 5:
			text2 = "1107";
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			text = NrTSingleton<UIDataManager>.Instance.GetString("{$", NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTexture(nGrade), "}");
		}
		if (text2 != string.Empty)
		{
			text2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				text2
			});
		}
		if (isLinkName)
		{
			name = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", new object[]
			{
				text2,
				"[",
				text,
				"{@P",
				text2,
				name,
				"}",
				text2,
				"]"
			});
		}
		else
		{
			name = NrTSingleton<UIDataManager>.Instance.GetString(text2, "[", text, text2, name, "]");
		}
		return name;
	}

	public static string GetChatFrontString(string name, CHAT_TYPE type)
	{
		string result = string.Empty;
		if (type == CHAT_TYPE.NORMAL)
		{
			result = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1336"), " ", name);
		}
		else if (type == CHAT_TYPE.GUILD)
		{
			result = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1337"), " ", name);
		}
		else
		{
			result = name;
		}
		return result;
	}

	public static string GetChatFrontString(string name, short colosseumGrade, CHAT_TYPE type, bool isLinkName = false)
	{
		string chatNameStr = ChatManager.GetChatNameStr(name, colosseumGrade, isLinkName);
		return ChatManager.GetChatFrontString(chatNameStr, type);
	}

	public static string GetChatColorKey(CHAT_TYPE type)
	{
		switch (type)
		{
		case CHAT_TYPE.NORMAL:
			return "1002";
		case CHAT_TYPE.GUILD:
			return "1105";
		case CHAT_TYPE.PARTY:
			return "1104";
		case CHAT_TYPE.BATTLE:
			return "1303";
		case CHAT_TYPE.BABELPARTY:
		case CHAT_TYPE.MYTHRAID:
			IL_24:
			if (type != CHAT_TYPE.SYSTEM)
			{
				return string.Empty;
			}
			return "1111";
		case CHAT_TYPE.WATCH:
			return "1304";
		}
		goto IL_24;
	}

	public static void SetHideControl(bool _bHide)
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg != null)
			{
				mainChatDlg.SetHideControl(_bHide);
			}
			if (!_bHide)
			{
				mainChatDlg.ChangeSize(GUICamera.width, GUICamera.height);
			}
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobileDlg chatMobileDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg;
			if (chatMobileDlg != null)
			{
				chatMobileDlg.SetHideControl(_bHide);
			}
		}
	}

	public static bool AddItemLink(ITEM _item)
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg == null)
			{
				return false;
			}
			mainChatDlg.AddItemLinkText(_item);
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
			if (chatMobile_Sub_Dlg != null)
			{
				chatMobile_Sub_Dlg.AddItemLinkText(_item);
			}
			ChatMobileDlg chatMobileDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg;
			if (chatMobileDlg == null)
			{
				return false;
			}
			chatMobileDlg.AddItemLinkText(_item);
		}
		return true;
	}

	public static int GetSelectTap()
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg != null)
			{
				return (int)mainChatDlg.GetSelectTab();
			}
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
			if (chatMobile_Sub_Dlg != null)
			{
				return (int)chatMobile_Sub_Dlg.GetSelectTab();
			}
		}
		return -1;
	}

	public static void GetToolBab()
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg != null)
			{
				mainChatDlg.GetToolBab();
			}
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobileDlg chatMobileDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg;
			if (chatMobileDlg != null)
			{
			}
		}
	}

	public static void AddChatText(string strdata)
	{
		if (TsPlatform.IsWeb)
		{
			MainChatDlg mainChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as MainChatDlg;
			if (mainChatDlg != null)
			{
				mainChatDlg.AddChatText(strdata);
			}
		}
		else if (TsPlatform.IsMobile)
		{
			ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
			if (chatMobile_Sub_Dlg != null)
			{
				chatMobile_Sub_Dlg.AddChatText(strdata);
			}
		}
	}

	public bool ProcessClientCommand(string chattext, ref ChatLabel pkTargetLable)
	{
		if (chattext.Contains("/ani") && chattext.Length == 10)
		{
			int id = int.Parse(chattext.Substring(5, 2));
			int animation = int.Parse(chattext.Substring(8, 2));
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(id);
			if (@char != null)
			{
				@char.SetAnimation((eCharAnimationType)animation);
				return true;
			}
		}
		if (chattext.Contains("/objcount"))
		{
			string text = string.Empty;
			text = "All = " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "Textures = " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "AudioClips = " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "Meshes = " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "Materials = " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "GameObjects = " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			text = "Components = " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length.ToString();
			pkTargetLable.PushText("SYSTEM", text, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
			return true;
		}
		if (chattext.Contains("/sf"))
		{
			NrTSingleton<FormsManager>.Instance.ShowOrClose(G_ID.DLG_FPS);
			return true;
		}
		if (chattext.Contains("/fm"))
		{
			NrTSingleton<FormsManager>.Instance.ShowOrClose(G_ID.DLG_MEMORYCHECK);
			return true;
		}
		if (chattext.Contains("/setfps"))
		{
			string[] array = chattext.Split(new char[]
			{
				'='
			});
			int num = 0;
			if (int.TryParse(array[1], out num))
			{
				if (num != 0)
				{
					Application.targetFrameRate = num;
				}
				return true;
			}
		}
		if (chattext.Contains("/delsolui"))
		{
			NrTSingleton<FormsManager>.Instance.ClearShowHideForms();
		}
		if (chattext.Contains("/sr"))
		{
			NrTSingleton<NkBattleReplayManager>.Instance.SaveReplay = !NrTSingleton<NkBattleReplayManager>.Instance.SaveReplay;
			NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1230"), "Save Replay : " + NrTSingleton<NkBattleReplayManager>.Instance.SaveReplay.ToString());
		}
		if (chattext.Contains("/rl") && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_REPLAY_LIST_DLG) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_REPLAY_LIST_DLG);
		}
		if (chattext.Contains("/tm") && 100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_MASTER_DLG) == null)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOURNAMENT_MASTER_DLG);
		}
		if (chattext.Contains("/logouttest") && 100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
		{
			NrMobileAuthSystem.Instance.RequestLogout = true;
			NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
		}
		if (chattext.Contains("/relogintest") && 100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
		{
			NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
		}
		return false;
	}
}
