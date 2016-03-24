using GAME;
using System;
using UnityEngine;
using UnityForms;

public class MainChatDlg : Form
{
	private const string CHAT_LANG_ENG = "Main_I_ChatEng";

	private const string CHAT_LANG_KOR = "Main_I_ChatHan";

	private const int SIZE_LEVEL_NORMAL_MAX = 2;

	private const int SIZE_LEVEL_WIDE_MAX = 3;

	private CHAT_TYPE m_SelectTab;

	private int m_SizeLevel;

	private Label lbChatType;

	private ChatLabel _clChat;

	private ChatLabel _clChatAll;

	private ChatLabel _clChatGuild;

	private ChatLabel _clChatParty;

	private ChatLabel _clChatShout;

	private TextField _tfInput;

	private Button _btSize;

	private Button _btOption;

	private Button _btAD;

	private Button _btEmoticon;

	private Toolbar _ToolBar;

	private DrawTexture _dtInputBG;

	private DrawTexture _dtChatBG;

	private DrawTexture _dtChatLag;

	private bool bLanguage;

	private float DEFAULT_CHAT_BG_HEIGHT;

	private float DEFAULT_CHAT_LABEL_HEIGHT;

	private bool bOldTextFieldFocus;

	private bool bNotTextKeypadEnter;

	private ITEM linkItem = new ITEM();

	private bool bIsSetLinkItem;

	private bool useItemLinkText;

	private bool bHideControl;

	public ChatTabDefine _ChatTabDefine = new ChatTabDefine();

	private ChatManager _Manager = NrTSingleton<ChatManager>.Instance;

	private int SCREEN_TYPE_MAX_SIZE
	{
		get
		{
			return (GUICamera.height >= 1280f) ? 3 : 2;
		}
	}

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
		base.AlwaysUpdate = true;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Chat/Dlg_Chat_web", G_ID.CHAT_MAIN_DLG, false);
		base.ChangeSceneDestory = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	private ChatLabel MakeChat(string strName, Vector2 vPos)
	{
		ChatLabel chatLabel = base.GetControl(strName) as ChatLabel;
		chatLabel.Visible = false;
		chatLabel.itemSpacing = 5f;
		return chatLabel;
	}

	private void ChangeChatLabel(ref ChatLabel target, float width, float BGwidth)
	{
		target.SetSize(target.GetSize().x, width);
		target.SetLocation(10f, 10f + this.DEFAULT_CHAT_LABEL_HEIGHT - width);
		target.clipWhenMoving = true;
	}

	public override void SetComponent()
	{
		this._tfInput = (base.GetControl("TextField_Textinput") as TextField);
		this._tfInput.AddCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		this._tfInput.maxLength = 50;
		this._tfInput.MaxWidth = 0f;
		this._ToolBar = (base.GetControl("ToolBar_ChatTab") as Toolbar);
		this.lbChatType = (base.GetControl("Label_Sort") as Label);
		this.GetToolBab();
		for (int i = 0; i < this._ToolBar.Count; i++)
		{
			UIPanelTab expr_90 = this._ToolBar.Control_Tab[i];
			expr_90.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_90.ButtonClick, new EZValueChangedDelegate(this.OnClickTab));
		}
		this._ToolBar.FirstSetting();
		this._ToolBar.SetSelectTabIndex(0);
		this._btSize = (base.GetControl("Button_variableBtn") as Button);
		Button expr_F9 = this._btSize;
		expr_F9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F9.Click, new EZValueChangedDelegate(this.OnClickSize));
		this._btOption = (base.GetControl("Button_ChatConf") as Button);
		Button expr_136 = this._btOption;
		expr_136.Click = (EZValueChangedDelegate)Delegate.Combine(expr_136.Click, new EZValueChangedDelegate(this.OnClickOption));
		this._btEmoticon = (base.GetControl("Button_ChatEmo") as Button);
		Button expr_173 = this._btEmoticon;
		expr_173.Click = (EZValueChangedDelegate)Delegate.Combine(expr_173.Click, new EZValueChangedDelegate(this.OnClickEmoticon));
		this._btAD = (base.GetControl("Button_ChatShout") as Button);
		Button expr_1B0 = this._btAD;
		expr_1B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1B0.Click, new EZValueChangedDelegate(this.OnClickAD));
		this._dtInputBG = (base.GetControl("DrawTexture_TextArea") as DrawTexture);
		this._dtChatBG = (base.GetControl("DrawTexture_ChatBG") as DrawTexture);
		this._dtChatLag = (base.GetControl("DrawTexture_Lag") as DrawTexture);
		this._dtInputBG.SetAlpha(0.5f);
		Vector2 vPos = new Vector2(this._dtChatBG.GetSize().x, 0f);
		this._clChat = this.MakeChat("ChatLabel_Allchat", vPos);
		this._clChatAll = this.MakeChat("ChatLabel_Allchat", vPos);
		this._clChatGuild = this.MakeChat("ChatLabel_Guildchat", vPos);
		this._clChatParty = this.MakeChat("ChatLabel_Partychat", vPos);
		this._clChatShout = this.MakeChat("ChatLabel_Fieldchat", vPos);
		this.DEFAULT_CHAT_BG_HEIGHT = this._dtChatBG.GetSize().y;
		this.DEFAULT_CHAT_LABEL_HEIGHT = this._clChatAll.GetSize().y;
		base.Draggable = false;
		base.ShowLayer((int)(this.m_SelectTab + 1), 5);
		this.SetChatSize(this.SCREEN_TYPE_MAX_SIZE / 2);
	}

	public void GetToolBab()
	{
		this._ChatTabDefine.UserLoadChatTab();
		this._ToolBar.Control_Tab[0].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(CHAT_TYPE.NORMAL)) + this._ChatTabDefine.TabName[0];
		this._ToolBar.Control_Tab[1].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(CHAT_TYPE.GUILD)) + this._ChatTabDefine.TabName[1];
		this._ToolBar.Control_Tab[2].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(CHAT_TYPE.PARTY)) + this._ChatTabDefine.TabName[2];
		this._ToolBar.Control_Tab[3].Text = NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(CHAT_TYPE.BATTLE)) + this._ChatTabDefine.TabName[3];
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

	public override void InitData()
	{
		this.ChangeSize(GUICamera.width, GUICamera.height);
	}

	private void OnLostInputText(IKeyFocusable obj)
	{
		if (this.bNotTextKeypadEnter && this.IsChatFocused())
		{
			this.SetChatFocus();
		}
	}

	private void OnInputText(IKeyFocusable obj)
	{
		if (this._tfInput.Text.Length <= 0)
		{
			this._tfInput.ClearText();
			this.bNotTextKeypadEnter = true;
			return;
		}
		if (!NrTSingleton<ChatManager>.Instance.ProcessClientCommand(this._tfInput.Text, ref this._clChat))
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			NrTSingleton<ChatManager>.Instance.SendMessage(this.m_SelectTab, this._tfInput.Text, this.useItemLinkText, this.linkItem, 0, 0L, 0);
			NrTSingleton<ChatManager>.Instance.MakeChatText(this, this.m_SelectTab, myCharInfo.ColosseumGrade, this._tfInput.Text, this.linkItem);
		}
		this._tfInput.ClearText();
		this._tfInput.SetFocus();
		this.useItemLinkText = false;
		this.bIsSetLinkItem = false;
		this.linkItem = null;
	}

	public CHAT_TYPE GetSelectTab()
	{
		return this.m_SelectTab;
	}

	private void OnClickTab(IUIObject obj)
	{
		UIPanelTab uIPanelTab = obj as UIPanelTab;
		CHAT_TYPE cHAT_TYPE = (CHAT_TYPE)uIPanelTab.panel.index;
		if (cHAT_TYPE == this.m_SelectTab)
		{
			return;
		}
		this.m_SelectTab = cHAT_TYPE;
		base.ShowLayer((int)(this.m_SelectTab + 1), 5);
		string arg = string.Empty;
		switch (cHAT_TYPE)
		{
		case CHAT_TYPE.NORMAL:
			arg = this._ChatTabDefine.TabName[0];
			break;
		case CHAT_TYPE.GUILD:
			arg = this._ChatTabDefine.TabName[1];
			break;
		case CHAT_TYPE.PARTY:
			arg = this._ChatTabDefine.TabName[2];
			break;
		case CHAT_TYPE.BATTLE:
			arg = this._ChatTabDefine.TabName[3];
			break;
		case CHAT_TYPE.WATCH:
			arg = this._ChatTabDefine.TabName[6];
			break;
		}
		this.lbChatType.SetText(string.Format("{0}[{1}]", NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(this.m_SelectTab)), arg));
		if (cHAT_TYPE != CHAT_TYPE.NORMAL && this._Manager.GetUniqueFromMegType(cHAT_TYPE) <= 0)
		{
			this._ToolBar.SetSelectTabIndex((int)this.m_SelectTab);
			return;
		}
		this.ChangeTab(cHAT_TYPE);
	}

	private void ChangeTab(CHAT_TYPE type)
	{
		this._ToolBar.SetSelectTabIndex((int)type);
		this.m_SelectTab = type;
	}

	private void OnClickOption(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_OPTION_DLG);
	}

	private void OnClickAD(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_AD_DLG);
	}

	private void OnClickSize(IUIObject obj)
	{
		this.SetChatSize(++this.m_SizeLevel);
	}

	private void SetChatSize(int SizeLevel)
	{
		this.m_SizeLevel = SizeLevel;
		if (this.m_SizeLevel > this.SCREEN_TYPE_MAX_SIZE)
		{
			this.m_SizeLevel = 0;
		}
		float num = this._clChatAll.FontSize + 3f;
		float num2 = num * 6f + num * 4f * (float)this.m_SizeLevel;
		float num3 = num2 + 26f;
		this._dtChatBG.SetSize(this._dtChatBG.GetSize().x, num3);
		this._dtChatBG.SetLocation(this._dtChatBG.GetLocation().x, this.DEFAULT_CHAT_BG_HEIGHT - num3);
		this.ChangeChatLabel(ref this._clChatAll, num2, num3);
		this.ChangeChatLabel(ref this._clChatGuild, num2, num3);
		this.ChangeChatLabel(ref this._clChatParty, num2, num3);
		this.ChangeChatLabel(ref this._clChatShout, num2, num3);
		this.ChangeSize(GUICamera.width, GUICamera.height);
		Main_UI_FollowChar main_UI_FollowChar = (Main_UI_FollowChar)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_FOLLOWCHAR);
		if (main_UI_FollowChar != null)
		{
			main_UI_FollowChar.ChangeSize(GUICamera.width, GUICamera.height);
		}
	}

	private void OnClickEmoticon(IUIObject obj)
	{
		base.SetChildForm(G_ID.EMOTICON_DLG);
		EmoticonDlg emoticonDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EMOTICON_DLG) as EmoticonDlg;
		if (emoticonDlg != null)
		{
			emoticonDlg.SetLocation(base.GetSizeX() + 1f, GUICamera.height - emoticonDlg.GetSizeY());
			emoticonDlg.SetCharType(CHAT_TYPE.NORMAL);
		}
	}

	public override void Update()
	{
		if (Input.imeIsSelected != this.bLanguage)
		{
			this.bLanguage = Input.imeIsSelected;
			this._dtChatLag.SetTexture((!this.bLanguage) ? "Main_I_ChatEng" : "Main_I_ChatHan");
		}
		if (this.IsChatFocused() != this.bOldTextFieldFocus)
		{
			this.bOldTextFieldFocus = this.IsChatFocused();
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
		case CHAT_TYPE.WATCH:
			return "1304";
		}
		return string.Empty;
	}

	public bool IsChatFocused()
	{
		return this._tfInput.IsFocus();
	}

	public void SetChatFocus()
	{
		IKeyFocusable keyFocusable = NrTSingleton<UIManager>.Instance.FocusObject as IKeyFocusable;
		if (keyFocusable != null)
		{
			TextField y = keyFocusable as TextField;
			if (this._tfInput != y)
			{
				return;
			}
		}
		if (this.HideControl)
		{
			return;
		}
		if (this.bNotTextKeypadEnter)
		{
			this._tfInput.ClearFocus();
		}
		else
		{
			this._tfInput.SetFocus();
		}
		this.bNotTextKeypadEnter = !this.bNotTextKeypadEnter;
		bool flag;
		if (!this._tfInput.Visible)
		{
			AutoSpriteControlBase arg_A3_0 = this._tfInput;
			flag = true;
			this._dtInputBG.Visible = flag;
			flag = flag;
			this.lbChatType.Visible = flag;
			arg_A3_0.Visible = flag;
		}
		this._tfInput.ColorText = NrTSingleton<CTextParser>.Instance.GetTextColor(MainChatDlg.GetChatColorKey(this.m_SelectTab));
		UIPanelManager arg_E5_0 = this._ToolBar;
		flag = !this.bNotTextKeypadEnter;
		this._btOption.Visible = flag;
		arg_E5_0.Visible = flag;
		if (0 < this._tfInput.Text.Length)
		{
			this.OnInputText(null);
		}
	}

	public override void ChangedResolution()
	{
		this.ChangeSize(GUICamera.width, GUICamera.height);
	}

	public void ChangeSize(float screenwidth, float screenheight)
	{
		base.SetLocation(0f, screenheight - 155f);
	}

	public ChatLabel GetChatLable(CHAT_TYPE msgtype)
	{
		switch (msgtype)
		{
		case CHAT_TYPE.GUILD:
			return this._clChatGuild;
		case CHAT_TYPE.PARTY:
			return this._clChatParty;
		case CHAT_TYPE.BATTLE:
			return this._clChatShout;
		default:
			if (msgtype != CHAT_TYPE.ALL)
			{
				return null;
			}
			return this._clChatAll;
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
		this.bNotTextKeypadEnter = false;
	}

	public void SetHideControl(bool _bHide)
	{
		this.bHideControl = _bHide;
		this.bNotTextKeypadEnter = false;
		this._clChat.slider.Hide(_bHide);
		this._clChat.slider.upButton.Hide(_bHide);
		this._clChat.slider.downButton.Hide(_bHide);
		this._btEmoticon.Hide(_bHide);
		this._btAD.Hide(_bHide);
		this._btSize.Hide(_bHide);
		for (int i = 0; i < this._ToolBar.Count; i++)
		{
			this._ToolBar.Control_Tab[i].Hide(_bHide);
		}
		this._dtChatBG.Hide(_bHide);
		this._btOption.Hide(_bHide);
		this._tfInput.Hide(_bHide);
	}

	public void NPCTellChat(string name)
	{
		this._clChat.SystemChatPushText(name, string.Empty, string.Empty, null);
	}
}
