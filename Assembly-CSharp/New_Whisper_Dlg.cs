using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class New_Whisper_Dlg : Form
{
	private const int SHOW_MAX_GROUP = 4;

	private const float BUTTON_SIZE = 50f;

	private Label[] m_lbGroupTitle = new Label[4];

	private Button[] m_btGroupClose = new Button[4];

	private Toggle[] m_tgGroup = new Toggle[4];

	private ListBox m_InviteList;

	private Button m_btInvite;

	private ChatLabel _ciWhisperChat;

	private TextField m_taInput;

	private Button m_btEmoticon;

	private Button m_btSendMessage;

	private Button m_btSelectColor;

	private Button m_btMinimum;

	private Button m_btGroupList;

	private Label m_lbLastTime;

	private DrawTexture m_txTextColor;

	private int m_CurChatUnique = -1;

	private List<int> m_RoomUniqueList = new List<int>();

	public int m_CurRoomUnique
	{
		get
		{
			return this.m_CurChatUnique;
		}
		set
		{
			this.m_CurChatUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Whisper/DLG_Whisper", G_ID.WHISPER_DLG, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		base.ChangeSceneDestory = false;
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 4; i++)
		{
			this.m_lbGroupTitle[i] = (base.GetControl(string.Format("Label_GroupTitle{0}", (i + 1).ToString("00"))) as Label);
			this.m_btGroupClose[i] = (base.GetControl(string.Format("Button_Close{0}", (i + 1).ToString("00"))) as Button);
			this.m_btGroupClose[i].Data = i;
			this.m_btGroupClose[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickDeleteRoom));
			this.m_tgGroup[i] = (base.GetControl(string.Format("Toggle_Group{0}", (i + 1).ToString("00"))) as Toggle);
			this.m_tgGroup[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickToggle));
		}
		this.m_InviteList = (base.GetControl("ListBox_InviteList") as ListBox);
		this.m_InviteList.LineHeight = 80f;
		this.m_InviteList.ColumnNum = 3;
		this.m_InviteList.UseColumnRect = true;
		this.m_InviteList.SetColumnRect(0, 15, 15, 50, 50);
		this.m_InviteList.SetColumnRect(1, 75, 11, 180, 28, SpriteText.Anchor_Pos.Middle_Left, 26f, false);
		this.m_InviteList.SetColumnRect(2, 75, 41, 180, 28, SpriteText.Anchor_Pos.Middle_Left, 26f, false);
		if (TsPlatform.IsMobile)
		{
			this.m_InviteList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickUserList));
		}
		else
		{
			this.m_InviteList.SetRightMouseDelegate(new EZValueChangedDelegate(this.OnClickUserList));
		}
		this.m_btInvite = (base.GetControl("Button_Invite") as Button);
		this.m_btInvite.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickInvite));
		this._ciWhisperChat = (base.GetControl("ChatLabel_Chat01") as ChatLabel);
		this._ciWhisperChat.MakeBoxCollider();
		this._ciWhisperChat.itemSpacing = 5f;
		this.m_taInput = (base.GetControl("TextArea_InputText") as TextField);
		if (NrGlobalReference.strLangType.Equals("eng"))
		{
			this.m_taInput.maxLength = 100;
		}
		else
		{
			this.m_taInput.maxLength = 50;
		}
		this.m_taInput.MaxWidth = 0f;
		this.m_btSendMessage = (base.GetControl("Button_Enter") as Button);
		this.m_btSendMessage.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnInputText));
		this.m_btSelectColor = (base.GetControl("Button_SelectColor") as Button);
		this.m_btSelectColor.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickColor));
		this.m_btMinimum = (base.GetControl("Button_Minimum") as Button);
		this.m_btMinimum.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMinium));
		this.m_btEmoticon = (base.GetControl("Button_ChatEmo") as Button);
		this.m_btEmoticon.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickEmoticon));
		this.m_btGroupList = (base.GetControl("Button_GroupList") as Button);
		this.m_btGroupList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickGroupList));
		this.m_lbLastTime = (base.GetControl("Label_TalkTime") as Label);
		this.m_txTextColor = (base.GetControl("DrawTexture_ColorSwatchs") as DrawTexture);
		this.SetUserStateText(NrTSingleton<WhisperManager>.Instance.UserState);
		this.SetChatColor();
		base.ShowLayer(1);
		base.SetScreenCenter();
		this.SetData();
	}

	private void SetData()
	{
		Dictionary<int, WhisperRoom> roomList = NrTSingleton<WhisperManager>.Instance.GetRoomList();
		foreach (int current in roomList.Keys)
		{
			WhisperRoom whisperRoom = roomList[current];
			if (whisperRoom != null)
			{
				this.AddRoom(current);
				this.SetTitle(whisperRoom);
				this.UpdateList(current);
			}
		}
	}

	private void OnInputText(IUIObject obj)
	{
		string text = this.m_taInput.Text;
		text = text.Trim();
		if (text.Length == 0)
		{
			TsLog.Log("OnInputText >>" + this.m_taInput.Text.Length.ToString(), new object[0]);
			this.m_taInput.ClearText();
			if (!TsPlatform.IsIPhone)
			{
				this.m_taInput.SetFocus();
			}
			return;
		}
		if (NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_CurChatUnique) == null)
		{
			TsLog.Log("OnInputText >>" + this.m_CurChatUnique.ToString(), new object[0]);
			this.m_taInput.ClearText();
			if (!TsPlatform.IsIPhone)
			{
				this.m_taInput.SetFocus();
			}
			return;
		}
		NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.NUM, this.m_taInput.Text, false, null, short.Parse(NrTSingleton<WhisperManager>.Instance.ChatColor), 0L, this.m_CurChatUnique);
		this.m_taInput.ClearText();
		this.m_taInput.Text = string.Empty;
	}

	private void OnClickColor(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_COLOR_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
			base.InteractivePanel.twinFormID = G_ID.NONE;
			return;
		}
		WhisperColorDlg whisperColorDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WHISPER_COLOR_DLG) as WhisperColorDlg;
		if (whisperColorDlg == null)
		{
			return;
		}
		float num = 1f;
		if (TsPlatform.IsWeb)
		{
			num = 0.7f;
		}
		whisperColorDlg.SetLocation(base.GetLocation().x + (this.m_btSelectColor.GetLocation().x + 50f) * num - whisperColorDlg.GetSizeX(), base.GetLocationY() + this.m_btSelectColor.GetLocationY() * num - 5f - whisperColorDlg.GetSizeY());
		base.InteractivePanel.twinFormID = G_ID.WHISPER_COLOR_DLG;
	}

	private void OnClickInvite(IUIObject obj)
	{
		WhisperFriendsDlg whisperFriendsDlg = base.SetChildForm(G_ID.WHISPER_USERLIST_DLG, Form.ChildLocation.CENTER) as WhisperFriendsDlg;
		whisperFriendsDlg.RoomUnique = this.m_CurChatUnique;
	}

	private void OnClickDeleteRoom(IUIObject obj)
	{
		Button button = obj as Button;
		int num = (int)button.Data;
		if (num < this.m_RoomUniqueList.Count)
		{
			int num2 = this.m_RoomUniqueList[num];
			NrTSingleton<WhisperManager>.Instance.DelRoom(num2);
			this.DelRoom(num2);
		}
	}

	private void OnClickToggle(IUIObject obj)
	{
		Toggle x = obj as Toggle;
		for (int i = 0; i < 4; i++)
		{
			if (this.m_tgGroup[i].Value && x == this.m_tgGroup[i])
			{
				this.m_CurChatUnique = this.m_RoomUniqueList[i];
				break;
			}
		}
		NrTSingleton<WhisperManager>.Instance.ChangeRoom(this.m_CurChatUnique);
	}

	private void OnClickMinium(IUIObject obj)
	{
		this.Hide();
	}

	private void OnClickClose(IUIObject obj)
	{
		this.CloseForm(null);
	}

	private void OnClickEmoticon(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EMOTICON_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EMOTICON_DLG);
			base.InteractivePanel.twinFormID = G_ID.NONE;
		}
		else
		{
			EmoticonDlg emoticonDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EMOTICON_DLG) as EmoticonDlg;
			if (emoticonDlg != null)
			{
				emoticonDlg.SetLocation(base.GetLocation().x + base.GetSizeX() - emoticonDlg.GetSizeX() - 1f, base.GetLocationY() + base.GetSizeY() - emoticonDlg.GetSizeY());
				emoticonDlg.SetCharType(CHAT_TYPE.NUM);
				base.InteractivePanel.twinFormID = G_ID.EMOTICON_DLG;
			}
		}
	}

	private void OnClickGroupList(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_WHISPERPOPUPMENU_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG);
			base.InteractivePanel.twinFormID = G_ID.NONE;
		}
		else
		{
			WhisperPopupMenu whisperPopupMenu = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG) as WhisperPopupMenu;
			if (whisperPopupMenu != null)
			{
				whisperPopupMenu.SetClickType(eWHISPER_POPUP_TYPE.ROOMLIST, null);
				float num = 1f;
				if (TsPlatform.IsWeb)
				{
					num = 0.7f;
				}
				whisperPopupMenu.SetLocation(base.GetLocation().x + base.GetSizeX() - whisperPopupMenu.GetSizeX(), base.GetLocationY() + (this.m_btGroupList.GetLocationY() + 50f) * num + 1f);
				base.InteractivePanel.twinFormID = G_ID.WHISPER_WHISPERPOPUPMENU_DLG;
			}
		}
	}

	private void OnClickUserState(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_WHISPERPOPUPMENU_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG);
			base.InteractivePanel.twinFormID = G_ID.NONE;
		}
		else
		{
			WhisperPopupMenu whisperPopupMenu = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG) as WhisperPopupMenu;
			if (whisperPopupMenu != null)
			{
				whisperPopupMenu.SetClickType(eWHISPER_POPUP_TYPE.STATE, null);
				base.InteractivePanel.twinFormID = G_ID.WHISPER_WHISPERPOPUPMENU_DLG;
			}
		}
	}

	private void OnClickUserList(IUIObject obj)
	{
		WhisperUser whisperUser = (WhisperUser)this.m_InviteList.SelectedItem.Data;
		if (whisperUser == null)
		{
			TsLog.LogWarning("User == null", new object[0]);
			return;
		}
		if (NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID() == whisperUser.PersonID)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_WHISPERPOPUPMENU_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG);
			base.InteractivePanel.twinFormID = G_ID.NONE;
		}
		else
		{
			WhisperPopupMenu whisperPopupMenu = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WHISPER_WHISPERPOPUPMENU_DLG) as WhisperPopupMenu;
			if (whisperPopupMenu != null)
			{
				whisperPopupMenu.SetClickType(eWHISPER_POPUP_TYPE.GROUPLIST, whisperUser);
				base.InteractivePanel.twinFormID = G_ID.WHISPER_WHISPERPOPUPMENU_DLG;
			}
		}
	}

	public int GetCurRoomUnique()
	{
		return this.m_CurRoomUnique;
	}

	public void AddRoom(int RoomUnique)
	{
		this.m_CurChatUnique = RoomUnique;
		if (!this.m_RoomUniqueList.Contains(RoomUnique))
		{
			if (this.m_RoomUniqueList.Count != 0)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < this.m_RoomUniqueList.Count; i++)
				{
					list.Add(this.m_RoomUniqueList[i]);
				}
				this.m_RoomUniqueList.Clear();
				this.m_RoomUniqueList.Add(RoomUnique);
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.m_RoomUniqueList.Contains(list[j]))
					{
						this.m_RoomUniqueList.Add(list[j]);
					}
					else
					{
						TsLog.Log("Duplication Unique = {0}", new object[]
						{
							list[j]
						});
					}
				}
			}
			else
			{
				this.m_RoomUniqueList.Add(RoomUnique);
			}
		}
		this.RangeRoom();
		this.m_tgGroup[0].Value = true;
		this.RefreshChat();
	}

	public void DelRoom(int RoomUnique)
	{
		this.m_RoomUniqueList.Remove(RoomUnique);
		if (this.m_RoomUniqueList.Count == 0)
		{
			this.Close();
			return;
		}
		this.RangeRoom();
		if (RoomUnique == this.m_CurChatUnique)
		{
			this.m_CurChatUnique = this.m_RoomUniqueList[0];
			this.m_tgGroup[0].controlIsEnabled = true;
			this.RefreshChat();
		}
	}

	public bool ChangeChatRoom(int chatunique)
	{
		if (NrTSingleton<WhisperManager>.Instance.GetRoom(chatunique) == null)
		{
			return false;
		}
		this.m_CurChatUnique = chatunique;
		this.RefreshChat();
		this.UpdateList(this.m_CurChatUnique);
		return true;
	}

	private void RangeRoom()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i < this.m_RoomUniqueList.Count)
			{
				WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_RoomUniqueList[i]);
				this.SetTitle(i, room);
				this.ShowHideToggle(i, true);
			}
			else
			{
				this.ShowHideToggle(i, false);
			}
		}
	}

	public void TogleChange(int RoomUnique)
	{
		this.m_CurChatUnique = RoomUnique;
		if (this.m_RoomUniqueList.Count != 0)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.m_RoomUniqueList.Count; i++)
			{
				list.Add(this.m_RoomUniqueList[i]);
			}
			this.m_RoomUniqueList.Clear();
			this.m_RoomUniqueList.Add(RoomUnique);
			for (int j = 0; j < list.Count; j++)
			{
				if (!this.m_RoomUniqueList.Contains(list[j]))
				{
					this.m_RoomUniqueList.Add(list[j]);
				}
				else
				{
					TsLog.Log("Duplication Unique = {0}", new object[]
					{
						list[j]
					});
				}
			}
		}
		else
		{
			this.m_RoomUniqueList.Add(RoomUnique);
		}
		this.RangeRoom();
		this.m_tgGroup[0].Value = true;
		this.RefreshChat();
	}

	public void UpdateList(int roomunique)
	{
		if (this.m_CurChatUnique == roomunique)
		{
			WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_CurChatUnique);
			this.SetList(room);
		}
	}

	public void SetTitle(WhisperRoom Room)
	{
		for (int i = 0; i < this.m_RoomUniqueList.Count; i++)
		{
			if (Room.Room == this.m_RoomUniqueList[i])
			{
				this.SetTitle(i, Room);
				break;
			}
		}
	}

	private void SetTitle(int RoomIndex, WhisperRoom Room)
	{
		if (RoomIndex >= 4)
		{
			return;
		}
		this.m_lbGroupTitle[RoomIndex].Text = NrTSingleton<CTextParser>.Instance.GetTextColor("1101") + Room.GetRoomName();
	}

	private void SetList(WhisperRoom Room)
	{
		List<WhisperUser> users = Room.GetUsers();
		if (users == null)
		{
			return;
		}
		this.m_InviteList.Clear();
		foreach (WhisperUser current in users)
		{
			ListItem listItem = new ListItem();
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.nFaceKind);
			if (charKindInfo == null)
			{
				TsLog.LogError("WhisperUser NrCharKindInfo == NULL  FaceKind = {0}", new object[]
				{
					current.nFaceKind
				});
			}
			listItem.SetColumnGUIContent(0, charKindInfo.GetCharKind(), false);
			listItem.SetColumnStr(1, current.Name, NrTSingleton<CTextParser>.Instance.GetTextColor("1101"));
			string text = string.Empty;
			string strTextKey = string.Empty;
			switch (current.byPlayState)
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
			default:
				TsLog.LogWarning("byPlayState = {0}", new object[]
				{
					current.byPlayState
				});
				strTextKey = "2142";
				break;
			}
			text = string.Format("({0})", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey));
			TsLog.LogWarning("UserState = {0}", new object[]
			{
				text
			});
			listItem.SetColumnStr(2, text, NrTSingleton<CTextParser>.Instance.GetTextColor("1101"));
			listItem.Key = current;
			this.m_InviteList.Add(listItem);
		}
		this.m_InviteList.RepositionItems();
	}

	public void SetUserStateText(byte state)
	{
	}

	private void ShowHideToggle(int index, bool bShow)
	{
		this.m_tgGroup[index].Visible = bShow;
		this.m_btGroupClose[index].Visible = bShow;
		this.m_lbGroupTitle[index].Visible = bShow;
	}

	private void RefreshChat()
	{
		this._ciWhisperChat.ClearList(true);
		WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_CurChatUnique);
		foreach (ChatMsg current in room.GetMsgQueue())
		{
			this.PushMsg(room.Room, current.Name, current.Msg, current.Color.ToString());
		}
	}

	private void LastMessageTime()
	{
		WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_CurChatUnique);
		DateTime dateTime = room.ReciveLastMessageTime();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2046");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"hour",
			dateTime.Hour.ToString("00"),
			"minute",
			dateTime.Minute.ToString("00"),
			"second",
			dateTime.Second.ToString("00")
		});
		this.m_lbLastTime.Text = textFromInterface;
	}

	public void AddChatText(string msg)
	{
		TextField expr_06 = this.m_taInput;
		expr_06.Text += msg;
		TextField expr_1D = this.m_taInput;
		expr_1D.OriginalContent += msg;
	}

	public void PushMsg(int chatunique, string name, string msg, string color)
	{
		if (this.m_CurChatUnique != chatunique)
		{
			return;
		}
		if (name.Equals(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94")))
		{
			this._ciWhisperChat.PushText(name, msg, color, null);
		}
		else
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2146");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				textFromInterface,
				"targetname",
				name
			});
			this._ciWhisperChat.PushText(textFromInterface, null, NrTSingleton<CTextParser>.Instance.GetTextColor("1102"));
			this._ciWhisperChat.PushText(msg, null, color);
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (!charPersonInfo.GetCharName().Equals(name))
		{
			this.LastMessageTime();
		}
	}

	public void PushMsg(int chatunique, string msg, string color)
	{
		if (this.m_CurChatUnique != chatunique)
		{
			return;
		}
		string msg2 = color + msg;
		this._ciWhisperChat.PushText(msg2, null);
	}

	public override void CloseForm(IUIObject obj)
	{
		this.Hide();
		NrTSingleton<WhisperManager>.Instance.WindowClose = true;
		NoticeIconDlg.SetIcon(ICON_TYPE.WHISPER, false);
	}

	public void SetChatColor()
	{
		string texture = string.Empty;
		string chatColor = NrTSingleton<WhisperManager>.Instance.ChatColor;
		switch (chatColor)
		{
		case "1107":
			texture = "Win_I_ChatColor01";
			break;
		case "1303":
			texture = "Win_I_ChatColor02";
			break;
		case "1110":
			texture = "Win_I_ChatColor03";
			break;
		case "1404":
			texture = "Win_I_ChatColor04";
			break;
		case "1304":
			texture = "Win_I_ChatColor05";
			break;
		case "1301":
			texture = "Win_I_ChatColor06";
			break;
		case "1113":
			texture = "Win_I_ChatColor07";
			break;
		case "1114":
			texture = "Win_I_ChatColor08";
			break;
		}
		this.m_txTextColor.SetTexture(texture);
	}
}
