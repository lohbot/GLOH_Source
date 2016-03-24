using GAME;
using System;
using UnityEngine;
using UnityForms;

public class ChatMobileDlg : Form
{
	private ChatLabel _clChat;

	private ChatLabel _clChatAll;

	private ChatLabel _clChatGuild;

	private ChatLabel _clChatSystem;

	private TextField _tfInput;

	private DrawTexture _dtChatBG;

	private Button _btKeyboard;

	public ChatTabDefine _ChatTabDefine = new ChatTabDefine();

	public ChatManager _Manager = NrTSingleton<ChatManager>.Instance;

	private CHAT_TYPE m_SelectTab;

	private ITEM linkItem = new ITEM();

	private bool bIsSetLinkItem;

	private bool useItemLinkText;

	private bool bHideControl;

	public bool HideControl
	{
		get
		{
			return this.bHideControl;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "CHAT/DLG_Chat", G_ID.CHAT_MAIN_DLG, false);
		base.ChangeSceneDestory = false;
		base.AlwaysUpdate = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
	}

	public override void SetComponent()
	{
		this._btKeyboard = (base.GetControl("Button_Input") as Button);
		Button expr_1C = this._btKeyboard;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickKeyboard));
		this._dtChatBG = (base.GetControl("DrawTexture_ChatBG") as DrawTexture);
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
		Vector2 vPos = new Vector2(this._dtChatBG.GetSize().x, 0f);
		this._clChat = ChatManager.MakeChatLabel(this, "ChatLabel_Allchat", vPos);
		this._clChatAll = ChatManager.MakeChatLabel(this, "ChatLabel_Allchat", vPos);
		this._clChatGuild = ChatManager.MakeChatLabel(this, "ChatLabel_Guildchat", vPos);
		this._clChatSystem = ChatManager.MakeChatLabel(this, "ChatLabel_Systemchat", vPos);
		this._clChat.RemoveBoxCollider();
		base.ShowLayer(1);
		ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
		if (chatMobile_Sub_Dlg != null)
		{
			chatMobile_Sub_Dlg.Hide();
		}
	}

	public CHAT_TYPE GetSelectTab()
	{
		return this.m_SelectTab;
	}

	private void OnClickKeyboard(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}

	public ChatLabel GetChatLable(CHAT_TYPE msgtype)
	{
		switch (msgtype)
		{
		case CHAT_TYPE.SYSTEM:
			goto IL_3B;
		case CHAT_TYPE.NTGUILD:
		case CHAT_TYPE.STORYCHAT:
			IL_1B:
			switch (msgtype)
			{
			case CHAT_TYPE.GUILD:
				return this._clChatGuild;
			case CHAT_TYPE.BATTLE:
				goto IL_3B;
			}
			return null;
		case CHAT_TYPE.ALL:
			return this._clChatAll;
		}
		goto IL_1B;
		IL_3B:
		return this._clChatSystem;
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

	public override void Update()
	{
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) != null)
		{
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			if (0 < this._Manager.GetChatMsgCount())
			{
				MainChatMsg mainChatMsg = this._Manager.ChatMsgDequeue();
				if (string.Empty == mainChatMsg.color)
				{
					mainChatMsg.color = ChatManager.GetChatColorKey(mainChatMsg.type);
				}
				ChatLabel chatLable = this.GetChatLable(mainChatMsg.type);
				if (null != chatLable)
				{
					chatLable.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
				}
				this._clChatAll.PushText(mainChatMsg.name, mainChatMsg.msg, mainChatMsg.color, mainChatMsg.linkItem);
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
		TextField expr_06 = this._tfInput;
		expr_06.Text += msg;
		TextField expr_1D = this._tfInput;
		expr_1D.OriginalContent += msg;
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

	public void SetHideControl(bool _bHide)
	{
		this._clChat.slider.Hide(_bHide);
		this._clChat.slider.upButton.Hide(_bHide);
		this._clChat.slider.downButton.Hide(_bHide);
		this._dtChatBG.Hide(_bHide);
		this._btKeyboard.Hide(_bHide);
	}

	public override void CloseForm(IUIObject obj)
	{
		this.Hide();
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_EXPBOOSTER_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MAIN_EXPBOOSTER_DLG);
		}
	}

	public override void Show()
	{
		base.Show();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_EXPBOOSTER);
		if (charSubData > 0L)
		{
			ExpBoosterDlg expBoosterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_EXPBOOSTER_DLG) as ExpBoosterDlg;
			if (expBoosterDlg == null)
			{
				expBoosterDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_EXPBOOSTER_DLG) as ExpBoosterDlg);
			}
			if (expBoosterDlg != null)
			{
				expBoosterDlg.Show();
			}
		}
	}
}
