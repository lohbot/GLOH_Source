using System;
using UnityForms;

public class EmoticonDlg : Form
{
	private Button[] _btEmotion = new Button[20];

	private Label[] _lbEmoticon = new Label[20];

	private FlashLabel[] _flbEmoticon = new FlashLabel[20];

	private CHAT_TYPE m_eChatType;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Chat/DLG_Emoticon", G_ID.EMOTICON_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < this._btEmotion.Length; i++)
		{
			this._btEmotion[i] = (base.GetControl("Main_I_ChatE" + i.ToString()) as Button);
			Button expr_33 = this._btEmotion[i];
			expr_33.Click = (EZValueChangedDelegate)Delegate.Combine(expr_33.Click, new EZValueChangedDelegate(this.OnClickEmoticon));
			this._lbEmoticon[i] = (base.GetControl("Label_I_ChatE" + i.ToString()) as Label);
			this._btEmotion[i].data = "^" + this._lbEmoticon[i].Text;
			this._flbEmoticon[i] = (base.GetControl("FlashLabel_I_ChatE" + i.ToString()) as FlashLabel);
			this._flbEmoticon[i].SetFlashLabel((string)this._btEmotion[i].data);
		}
	}

	private void OnClickEmoticon(IUIObject obj)
	{
		Button button = obj as Button;
		if (this.m_eChatType == CHAT_TYPE.BABELPARTY)
		{
			BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
			if (babelTower_ChatDlg != null)
			{
				babelTower_ChatDlg.AddChatText((string)button.data);
			}
		}
		else if (this.m_eChatType == CHAT_TYPE.NUM)
		{
			New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
			if (new_Whisper_Dlg != null)
			{
				new_Whisper_Dlg.AddChatText((string)button.data);
			}
		}
		else if (this.m_eChatType == CHAT_TYPE.STORYCHAT)
		{
			StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
			if (storyChatDetailDlg != null)
			{
				storyChatDetailDlg.AddComment((string)button.data);
			}
		}
		else
		{
			ChatManager.AddChatText((string)button.data);
		}
	}

	public void SetCharType(CHAT_TYPE eChatType)
	{
		this.m_eChatType = eChatType;
	}

	public CHAT_TYPE GetCharType()
	{
		return this.m_eChatType;
	}

	public override void OnClose()
	{
		base.OnClose();
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null)
		{
			new_Whisper_Dlg.InteractivePanel.twinFormID = G_ID.NONE;
		}
	}
}
