using GAME;
using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class ChatMobile_Sub_Dlg : Form
{
	private Label _lChatChannel;

	private ChatLabel _clChat;

	private ChatLabel _clChatGuild;

	private Toolbar chatTab;

	private TextField _tfInput;

	private DrawTexture _dtChatBG;

	private Button _btSend;

	private Button _btEmoticon;

	private Button _btChangeChannel;

	private Label _lChatType;

	private Button _btChangeType1;

	private Button _btChangeType2;

	private Label _Default;

	public ChatManager _Manager = NrTSingleton<ChatManager>.Instance;

	private CHAT_TYPE m_SelectTab;

	private ITEM linkItem = new ITEM();

	private bool bIsSetLinkItem;

	private bool useItemLinkText;

	private bool bOldTextFieldFocus;

	private bool bHideControl;

	private bool bChangeChatMode;

	public ChatTabDefine _ChatTabDefine = new ChatTabDefine();

	private ChatMobileDlg m_MainChat;

	private int _nChangeChannel = 1;

	private int m_nChatTap;

	public bool HideControl
	{
		get
		{
			return this.bHideControl;
		}
	}

	public bool ChangeChatMode
	{
		get
		{
			return this.bChangeChatMode;
		}
		set
		{
			this.bChangeChatMode = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "CHAT/dlg_subchat", G_ID.CHAT_MOBILE_SUB_DLG, true);
		base.ChangeSceneDestory = false;
		base.AlwaysUpdate = true;
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this._lChatChannel = (base.GetControl("Label_channel1") as Label);
		this._tfInput = (base.GetControl("TextField_InputText") as TextField);
		this._tfInput.multiline = false;
		this._tfInput.AddCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		this._tfInput.AddFocusDelegate(new UITextField.FocusDelegate(this.OnFocusText));
		if (NrGlobalReference.strLangType.Equals("eng"))
		{
			this._tfInput.maxLength = 100;
		}
		else
		{
			this._tfInput.maxLength = 50;
		}
		this._tfInput.MaxWidth = 0f;
		this._tfInput.SetDefaultText(string.Empty);
		if (null != this._tfInput.spriteText)
		{
			this._tfInput.spriteText.parseColorTags = false;
			if (null != this._tfInput.spriteTextShadow)
			{
				this._tfInput.spriteTextShadow.parseColorTags = false;
			}
		}
		this.chatTab = (base.GetControl("TabBtn") as Toolbar);
		this.SetToolBarText();
		for (int i = 0; i < this.chatTab.Count; i++)
		{
			UIPanelTab expr_137 = this.chatTab.Control_Tab[i];
			expr_137.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_137.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		}
		this.chatTab.FirstSetting();
		this.chatTab.SetSelectTabIndex(0);
		this._btSend = (base.GetControl("Button_Enter") as Button);
		this._btSend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSend));
		this._btEmoticon = (base.GetControl("Button_ChatEmo") as Button);
		this._btEmoticon.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickEmoticon));
		this._btChangeChannel = (base.GetControl("Button_channel") as Button);
		this._btChangeChannel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeChannel));
		this._lChatType = (base.GetControl("Label_Chatkind") as Label);
		this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
		this._btChangeType1 = (base.GetControl("Button_ChatChange1") as Button);
		this._btChangeType2 = (base.GetControl("Button_ChatChange2") as Button);
		this._btChangeType1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeType1));
		this._btChangeType2.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeType2));
		this.SetChatType();
		this._dtChatBG = (base.GetControl("DrawTexture_ChatBG") as DrawTexture);
		base.SetLocation(0f, GUICamera.height - base.GetSize().y);
		Vector2 vPos = new Vector2(this._dtChatBG.GetSize().x, 0f);
		this._clChat = ChatManager.MakeChatLabel(this, "ChatLabel_AllChat", vPos);
		this._clChatGuild = ChatManager.MakeChatLabel(this, "ChatLabel_Guild", vPos);
		this._clChatGuild.Visible = false;
		this._Default = (base.GetControl("Label_chat") as Label);
		base.ShowLayer(1);
		this.m_MainChat = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) as ChatMobileDlg);
		base.SetScreenCenter();
		this.ChatStart();
		this.m_nChatTap = PlayerPrefs.GetInt(NrPrefsKey.CHAT_SUB_TAP, 0);
		CHAT_TYPE changeTap = (CHAT_TYPE)this.m_nChatTap;
		this.SetChangeTap(changeTap);
	}

	private void ChatStart()
	{
		for (int i = 0; i < this._Manager.GetChatMsgListCount(); i++)
		{
			MainChatMsg chatMsgData = this._Manager.GetChatMsgData(i);
			if (string.Empty == chatMsgData.color)
			{
				chatMsgData.color = ChatManager.GetChatColorKey(chatMsgData.type);
			}
			this._clChat.PushText(chatMsgData.name, chatMsgData.msg, chatMsgData.color, chatMsgData.linkItem);
			if (chatMsgData.type == CHAT_TYPE.GUILD)
			{
				this._clChatGuild.PushText(chatMsgData.name, chatMsgData.msg, chatMsgData.color, chatMsgData.linkItem);
			}
		}
	}

	private void OnInputText(IKeyFocusable obj)
	{
	}

	private void OnFocusText(IKeyFocusable obj)
	{
		this._Default.Visible = false;
	}

	private void OnClickEmoticon(IUIObject obj)
	{
		EmoticonDlg emoticonDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EMOTICON_DLG) as EmoticonDlg;
		if (emoticonDlg != null)
		{
			emoticonDlg.DonotDepthChange(base.GetLocation().z - 8f);
			emoticonDlg.SetLocation(base.GetLocationX() + base.GetSizeX() - emoticonDlg.GetSizeX() - 1f, base.GetLocationY() + base.GetSizeY() - emoticonDlg.GetSizeY() - 65f);
			emoticonDlg.SetCharType(CHAT_TYPE.NORMAL);
		}
	}

	public void OnClickSend(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long nChatBlockDate = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nChatBlockDate;
		if (0L < nChatBlockDate && PublicMethod.GetCurTime() <= nChatBlockDate)
		{
			DateTime dueDate = PublicMethod.GetDueDate(nChatBlockDate);
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("312");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"day",
				dueDate.Day,
				"hour",
				dueDate.Hour,
				"min",
				dueDate.Minute
			});
			CHAT_TYPE selectTab = this.m_SelectTab;
			NrTSingleton<ChatManager>.Instance.MakeChatText(this, selectTab, kMyCharInfo.ColosseumGrade, empty, null);
			this._tfInput.ClearText();
			return;
		}
		if (this._tfInput.Text.Length <= 0)
		{
			this._tfInput.ClearText();
			return;
		}
		if (this._tfInput.Text.Equals(this._tfInput.GetDefaultText()))
		{
			return;
		}
		string text = this.ConvertLinkText(this._tfInput.Text);
		if (text.Contains("[#"))
		{
			text = text.Replace("[#", " ");
		}
		if (text.Contains("RGBA("))
		{
			text = text.Replace("RGBA(", " ");
		}
		if (!NrTSingleton<ChatManager>.Instance.ProcessClientCommand(text, ref this._clChat))
		{
			CHAT_TYPE cHAT_TYPE = this.m_SelectTab;
			if (cHAT_TYPE == CHAT_TYPE.PARTY)
			{
				cHAT_TYPE = CHAT_TYPE.SYSTEM;
			}
			NrTSingleton<ChatManager>.Instance.SendMessage(cHAT_TYPE, text, this.useItemLinkText, this.linkItem, 0, 0L, 0);
			NrTSingleton<ChatManager>.Instance.MakeChatText(this, cHAT_TYPE, kMyCharInfo.ColosseumGrade, text, this.linkItem);
		}
		string text2 = string.Empty;
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			text2 = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				text
			});
		}
		if (text2.Contains("*"))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		this._tfInput.ClearText();
		this.useItemLinkText = false;
		this.bIsSetLinkItem = false;
		this.linkItem = null;
		this._tfInput.SetDefaultText(string.Empty);
		this._Default.Visible = true;
	}

	public void ChangeChannelMode(bool change)
	{
	}

	public void OnClickChangeType1(IUIObject obj)
	{
		if (this.m_SelectTab == CHAT_TYPE.NORMAL)
		{
			this.m_SelectTab = CHAT_TYPE.GUILD;
			this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2139");
		}
		else
		{
			this.m_SelectTab = CHAT_TYPE.NORMAL;
			this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
		}
	}

	public void OnClickChangeType2(IUIObject obj)
	{
		if (this.m_SelectTab == CHAT_TYPE.NORMAL)
		{
			this.m_SelectTab = CHAT_TYPE.GUILD;
			this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2139");
		}
		else
		{
			this.m_SelectTab = CHAT_TYPE.NORMAL;
			this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
		}
	}

	public void OnClickChangeChannel(IUIObject obj)
	{
		if (!this.ChangeChatMode)
		{
			string message = string.Empty;
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LIMIT_CHATTIME);
			if (charSubData > 0L)
			{
				return;
			}
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("190");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
			if (inputNumberDlg != null)
			{
				inputNumberDlg.DonotDepthChange(base.GetLocation().z - 16f);
				inputNumberDlg.SetScreenCenter();
				inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_Input_ChangeChannel), null, new Action<InputNumberDlg, object>(this.On_Input_CancelChannel), null);
				inputNumberDlg.SetMinMax(1L, 300L);
			}
			this.ChangeChannelMode(true);
		}
	}

	private void On_Input_ChangeChannel(InputNumberDlg a_cForm, object a_oObject)
	{
		int nChangeChannel = (int)a_cForm.GetNum();
		this._lChatChannel.Text = NrTSingleton<CTextParser>.Instance.GetTextColor("1101") + nChangeChannel.ToString();
		this._nChangeChannel = nChangeChannel;
		int nChangeChannel2 = this._nChangeChannel;
		if (nChangeChannel2 < 1 || nChangeChannel2 >= 302)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("190");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		GS_CHATCHANNEL_CHANGE_REQ gS_CHATCHANNEL_CHANGE_REQ = new GS_CHATCHANNEL_CHANGE_REQ();
		gS_CHATCHANNEL_CHANGE_REQ.i32Change_ChatChannel = nChangeChannel2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHATCHANNEL_CHANGE_REQ, gS_CHATCHANNEL_CHANGE_REQ);
		this.ChangeChannelMode(false);
	}

	private void On_Input_CancelChannel(InputNumberDlg a_cForm, object a_oObject)
	{
		this.ChangeChannelMode(false);
	}

	public CHAT_TYPE GetSelectTab()
	{
		return this.m_SelectTab;
	}

	private void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		if (uIPanelTab.panel.index == uIPanelTab.panelManager.CurrentPanel.index)
		{
			return;
		}
		CHAT_TYPE changeTap = (CHAT_TYPE)uIPanelTab.panel.index;
		this.SetChangeTap(changeTap);
	}

	private void ChangeTab(CHAT_TYPE type)
	{
		this.m_SelectTab = type;
		this.ChangeChannelMode(false);
	}

	private void SetChangeTap(CHAT_TYPE selectTab)
	{
		this.m_SelectTab = selectTab;
		base.ShowLayer((int)(this.m_SelectTab + 1));
		if (selectTab != CHAT_TYPE.NORMAL)
		{
			if (selectTab == CHAT_TYPE.GUILD)
			{
				this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2139");
				this._btChangeType1.controlIsEnabled = false;
				this._btChangeType2.controlIsEnabled = false;
				this._clChatGuild.clipWhenMoving = true;
			}
		}
		else
		{
			this._lChatType.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
			this.SetChatType();
		}
		PlayerPrefs.SetInt(NrPrefsKey.CHAT_SUB_TAP, (int)selectTab);
		if (selectTab != CHAT_TYPE.NORMAL && NrTSingleton<ChatManager>.Instance.GetUniqueFromMegType(selectTab) <= 0)
		{
			this.chatTab.SetSelectTabIndex((int)this.m_SelectTab);
			return;
		}
		this.ChangeTab(selectTab);
		this.SetChannelText();
	}

	private void OnClickOption(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_OPTION_DLG);
	}

	private void OnClickKeyboard(IUIObject obj)
	{
		this._tfInput.controlIsEnabled = true;
		this.SetChatFocus();
	}

	public ChatLabel GetChatLable(CHAT_TYPE msgtype)
	{
		if (msgtype == CHAT_TYPE.GUILD)
		{
			return this._clChatGuild;
		}
		if (msgtype != CHAT_TYPE.ALL)
		{
			return this._clChat;
		}
		return this._clChat;
	}

	public void SetToolBarText()
	{
		this.chatTab.Control_Tab[0].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450");
		this.chatTab.Control_Tab[1].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17");
	}

	public override void Set_Value(object obj)
	{
		ITEM iTEM = obj as ITEM;
		if (iTEM != null)
		{
			this.AddItemLinkText(iTEM);
		}
	}

	public void AddItemLinkText(ITEM item)
	{
		if (this.bIsSetLinkItem)
		{
			if (this.linkItem.m_nItemUnique == item.m_nItemUnique)
			{
				this.linkItem = item;
			}
			return;
		}
		string str = "<" + NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item) + ">";
		this.useItemLinkText = true;
		TextField expr_51 = this._tfInput;
		expr_51.Text += str;
		TextField expr_68 = this._tfInput;
		expr_68.OriginalContent += str;
		NrTSingleton<UIManager>.Instance.bLinkText = true;
		this._tfInput.SetFocus();
		this.linkItem = item;
		this.bIsSetLinkItem = true;
	}

	public bool DeleteLinkItem()
	{
		if (this.bIsSetLinkItem && this.useItemLinkText)
		{
			this.bIsSetLinkItem = false;
			this.useItemLinkText = false;
			return true;
		}
		return false;
	}

	public bool IsChatFocused()
	{
		return this._tfInput.IsFocus();
	}

	public void SetChatFocus()
	{
		if (this.HideControl)
		{
			return;
		}
		this._tfInput.Visible = true;
		this._tfInput.SetFocus();
		this._tfInput.ColorText = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(this.m_SelectTab));
	}

	public override void Update()
	{
		if (this.IsChatFocused() != this.bOldTextFieldFocus)
		{
			this.bOldTextFieldFocus = this.IsChatFocused();
		}
		if (0 < this._Manager.GetChatMsgCount())
		{
			MainChatMsg mainChatMsg = this._Manager.ChatMsgDequeue();
			if (string.Empty == mainChatMsg.color)
			{
				mainChatMsg.color = ChatManager.GetChatColorKey(mainChatMsg.type);
			}
			this._clChat.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
			if (mainChatMsg.type == CHAT_TYPE.GUILD)
			{
				this._clChatGuild.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
			}
			if (this.m_MainChat != null)
			{
				ChatLabel chatLable = this.m_MainChat.GetChatLable(CHAT_TYPE.ALL);
				if (chatLable != null)
				{
					chatLable.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
				}
			}
			TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
			if (tournamentLobbyDlg != null)
			{
				ChatLabel chatLable2 = tournamentLobbyDlg.GetChatLable(CHAT_TYPE.ALL);
				if (chatLable2 != null)
				{
					chatLable2.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
				}
			}
		}
		if (0 < this._Manager.GetCharChatCount())
		{
			CharChat charChat = this._Manager.CharChatDequeue();
			if (charChat.Character != null)
			{
				charChat.Character.MakeChatText(charChat.msg, true);
			}
		}
	}

	public void AddChatText(string msg)
	{
		this._Default.Visible = false;
		if (this._tfInput.Text.Equals(this._tfInput.GetDefaultText()))
		{
			this._tfInput.ClearText();
		}
		TextField expr_3D = this._tfInput;
		expr_3D.Text += msg;
		TextField expr_54 = this._tfInput;
		expr_54.OriginalContent += msg;
	}

	public void OnInputChatMacroText(int ChatMacroIndex)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string chatMacro = kMyCharInfo.m_ChatMacroInfo.GetChatMacro(ChatMacroIndex);
		if (chatMacro == string.Empty)
		{
			return;
		}
		this._Manager.SendMessage(this.m_SelectTab, chatMacro);
	}

	public void NPCTellChat(string name)
	{
		this._clChat.SystemChatPushText(name, string.Empty, string.Empty, null);
	}

	public void GetToolBab()
	{
		this._ChatTabDefine.UserLoadChatTab();
		this.chatTab.Control_Tab[0].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + this._ChatTabDefine.TabName[0];
		this.chatTab.Control_Tab[1].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + this._ChatTabDefine.TabName[1];
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EMOTICON_DLG);
	}

	public override void Hide()
	{
		this._tfInput.LostFocus();
		this._tfInput.ClearFocus();
		this._tfInput.ClearText();
		base.Hide();
		this.ChangeChannelMode(false);
	}

	private void SetChatType()
	{
		if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			this._btChangeType1.controlIsEnabled = true;
			this._btChangeType2.controlIsEnabled = true;
			this.chatTab.Control_Tab[1].controlIsEnabled = true;
		}
		else
		{
			this._btChangeType1.controlIsEnabled = false;
			this._btChangeType2.controlIsEnabled = false;
			this.chatTab.Control_Tab[1].controlIsEnabled = false;
		}
	}

	public override void Show()
	{
		this.SetChatType();
		this.SetChannelText();
		base.Show();
	}

	public void SetInputText(string text)
	{
		this._Default.Visible = false;
		this._tfInput.SetInputText(text, ref NrTSingleton<UIManager>.Instance.insert);
	}

	public string ConvertLinkText(string str)
	{
		string text = str;
		int num = 0;
		int num2 = 0;
		if (str.Contains("[PB"))
		{
			num = str.IndexOf("[PB", num);
			if (num != -1)
			{
				num2 = str.IndexOf("]", num2);
				if (num2 != -1 && num2 < num + 16)
				{
					text = str.Replace("[PB", "{@W[PB");
					text = text.Replace("]", "]}");
				}
			}
		}
		else if (str.Contains("[CB"))
		{
			num = str.IndexOf("[CB", num);
			if (num != -1)
			{
				num2 = str.IndexOf("]", num2);
				if (num2 != -1 && num2 < num + 13)
				{
					text = str.Replace("[CB", "{@C[CB");
					text = text.Replace("]", "]}");
				}
			}
		}
		else if (str.Contains("[IB"))
		{
			num = str.IndexOf("[IB", num);
			if (num != -1)
			{
				num2 = str.IndexOf("]", num2);
				if (num2 != -1 && num2 < num + 13)
				{
					text = str.Replace("[IB", "{@F[IB");
					text = text.Replace("]", "]}");
				}
			}
		}
		return text;
	}

	public void SetChannelText()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("327");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count",
			kMyCharInfo.ChatChannel
		});
		this._lChatChannel.Text = empty;
		this._nChangeChannel = 1;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LIMIT_CHATTIME);
		if (charSubData > 0L)
		{
			this._btChangeChannel.Visible = false;
		}
		else
		{
			this._btChangeChannel.Visible = true;
		}
	}
}
