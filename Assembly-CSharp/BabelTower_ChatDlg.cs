using System;
using UnityForms;

public class BabelTower_ChatDlg : Form
{
	private const int MAX_USER_COUNT = 4;

	private const int LEADER_INDEX = 0;

	private const float BUTTON_SIZE = 50f;

	public Label m_lTitle;

	public TextField m_tfChat;

	public Button m_btChatInPut;

	public Button m_btChatColorChange;

	public DrawTexture m_dtColorSwatchs;

	public Button m_btChatEmo;

	public ChatLabel m_clChat;

	public Button m_bMini;

	private BABELTOWER_USER_CHATINFO[] user_info = new BABELTOWER_USER_CHATINFO[4];

	public long m_nLeaderPersonID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_chat", G_ID.BABELTOWER_CHAT, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lTitle = (base.GetControl("Label_Title") as Label);
		this.m_tfChat = (base.GetControl("TextArea_InputText") as TextField);
		this.m_tfChat.CommitDelegate = new EZKeyboardCommitDelegate(this.OnInputText);
		this.m_tfChat.maxLength = 50;
		this.m_tfChat.MaxWidth = 0f;
		this.m_btChatInPut = (base.GetControl("Button_Enter") as Button);
		this.m_btChatInPut.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSend));
		this.m_btChatColorChange = (base.GetControl("Button_SelectColor") as Button);
		Button expr_A9 = this.m_btChatColorChange;
		expr_A9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A9.Click, new EZValueChangedDelegate(this.OnClickColor));
		this.m_dtColorSwatchs = (base.GetControl("DrawTexture_ColorSwatchs") as DrawTexture);
		this.m_btChatEmo = (base.GetControl("Button_ChatEmo") as Button);
		this.m_btChatEmo.Click = new EZValueChangedDelegate(this.OnClickEmoticon);
		this.m_btChatEmo.data = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("43");
		this.m_bMini = (base.GetControl("Button_Button24") as Button);
		this.m_bMini.Click = new EZValueChangedDelegate(this.OnClickMini);
		this.m_clChat = (base.GetControl("ChatLabel_Chat01") as ChatLabel);
		this.m_clChat.MakeBoxCollider();
		this.m_clChat.itemSpacing = 5f;
		for (int i = 0; i < 4; i++)
		{
			this.user_info[i] = new BABELTOWER_USER_CHATINFO();
			this.user_info[i].m_itUserImage0 = (base.GetControl("ItemTexture_User" + i) as ItemTexture);
			this.user_info[i].m_lCharName0 = (base.GetControl("Label_charname" + i) as Label);
		}
		base.SetScreenCenter();
		this.Hide();
	}

	public override void Show()
	{
		base.Show();
		this.SetLeaderPersonID(SoldierBatch.BABELTOWER_INFO.m_nLeaderPersonID);
		this.RefreshChatInfo();
		this.m_clChat.clipWhenMoving = true;
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	private void OnClickColor(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WHISPER_COLOR_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
			return;
		}
		WhisperColorDlg whisperColorDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WHISPER_COLOR_DLG) as WhisperColorDlg;
		if (whisperColorDlg == null)
		{
			return;
		}
		float num = 1f;
		if (TsPlatform.IsMobile)
		{
			whisperColorDlg.SetLocation(base.GetLocation().x + (this.m_btChatColorChange.GetLocation().x + 50f) * num - whisperColorDlg.GetSizeX(), base.GetLocationY() + this.m_btChatColorChange.GetLocationY() * num - 5f - whisperColorDlg.GetSizeY());
		}
		else
		{
			whisperColorDlg.SetLocation(base.GetLocation().x + this.m_btChatColorChange.GetLocation().x, base.GetLocationY() + base.GetSize().y);
		}
	}

	private void OnClickMini(IUIObject obj)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWER_CHAT))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EMOTICON_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_COLOR_DLG);
			this.Hide();
		}
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
				emoticonDlg.SetCharType(CHAT_TYPE.BABELPARTY);
				base.InteractivePanel.twinFormID = G_ID.EMOTICON_DLG;
			}
		}
	}

	public void OnClickSend(IUIObject obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		if (this.m_tfChat.Text.Length <= 0)
		{
			this.m_tfChat.ClearText();
			return;
		}
		if (this.m_tfChat.Text.Equals(this.m_tfChat.GetDefaultText()))
		{
			return;
		}
		NrTSingleton<ChatManager>.Instance.SendMessage(CHAT_TYPE.BABELPARTY, this.m_tfChat.Text, false, null, short.Parse(NrTSingleton<WhisperManager>.Instance.ChatColor), this.m_nLeaderPersonID, 0);
		this.m_tfChat.ClearText();
		this.m_tfChat.SetDefaultText(string.Empty);
	}

	private void OnInputText(IKeyFocusable obj)
	{
	}

	public void PushMsg(string name, string msg, string color)
	{
		this.m_clChat.PushText(name, msg, color, null);
	}

	public void SetLeaderPersonID(long nLeaderPersonID)
	{
		this.m_nLeaderPersonID = nLeaderPersonID;
	}

	public void SetLayer()
	{
		for (int i = 0; i < 5; i++)
		{
			base.SetShowLayer(i, false);
		}
		base.SetShowLayer(0, true);
		for (int j = 0; j < 4; j++)
		{
			BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(j);
			if (babelPersonInfo.nPartyPersonID > 0L)
			{
				base.SetShowLayer(j + 1, true);
			}
			else
			{
				base.SetShowLayer(j + 1, false);
			}
		}
	}

	public void RefreshChatInfo()
	{
		for (int i = 0; i < 4; i++)
		{
			this.user_info[i].Init();
		}
		this.SetLayer();
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("833");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"floor",
			SoldierBatch.BABELTOWER_INFO.m_nBabelFloor,
			"subfloor",
			(int)(SoldierBatch.BABELTOWER_INFO.m_nBabelSubFloor + 1)
		});
		this.m_lTitle.SetText(empty);
		for (int j = 0; j < 4; j++)
		{
			BABELTOWER_PERSON babelPersonInfo = SoldierBatch.BABELTOWER_INFO.GetBabelPersonInfo(j);
			if (babelPersonInfo != null && babelPersonInfo.nPartyPersonID != 0L)
			{
				this.user_info[j].Show(true);
				this.user_info[j].m_lCharName0.Text = babelPersonInfo.strCharName.ToString();
				this.user_info[j].m_lCharName0.Visible = true;
				this.user_info[j].m_itUserImage0.SetSolImageTexure(eCharImageType.SMALL, babelPersonInfo.nCharKind, -1);
				this.user_info[j].m_itUserImage0.Visible = true;
			}
		}
	}

	public void AddChatText(string msg)
	{
		TextField expr_06 = this.m_tfChat;
		expr_06.Text += msg;
		TextField expr_1D = this.m_tfChat;
		expr_1D.OriginalContent += msg;
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
		this.m_dtColorSwatchs.SetTexture(texture);
	}
}
