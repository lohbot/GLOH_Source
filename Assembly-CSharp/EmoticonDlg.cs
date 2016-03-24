using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class EmoticonDlg : Form
{
	private NewListBox _nlbEmo;

	private CHAT_TYPE m_eChatType;

	private float fTime = Time.realtimeSinceStartup + 0.5f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Chat/DLG_Emoticon", G_ID.EMOTICON_DLG, true);
		form.AlwaysUpdate = true;
	}

	public override void SetComponent()
	{
		Dictionary<string, UIEmoticonInfo> uIEmoticonDictionary = NrTSingleton<UIEmoticonManager>.Instance.UIEmoticonDictionary;
		this._nlbEmo = (base.GetControl("NLB_Emo") as NewListBox);
		NewListItem newListItem = new NewListItem(this._nlbEmo.ColumnNum, true, string.Empty);
		int num = 0;
		foreach (KeyValuePair<string, UIEmoticonInfo> current in uIEmoticonDictionary)
		{
			int num2 = num * 3;
			string text = current.Key;
			text = text.Replace("^", string.Empty);
			newListItem.SetListItemData(num2, string.Empty, current.Key, new EZValueChangedDelegate(this.OnClickEmoticon), null);
			newListItem.SetListItemData(num2 + 1, current.Key, null, null, null);
			newListItem.SetListItemData(num2 + 2, text, null, null, null);
			num++;
			if (num >= 3)
			{
				num = 0;
				this._nlbEmo.Add(newListItem);
				newListItem = new NewListItem(this._nlbEmo.ColumnNum, true, string.Empty);
			}
		}
		this._nlbEmo.RepositionItems();
		this.fTime = Time.realtimeSinceStartup + 0.5f;
	}

	public override void OnLoad()
	{
		base.OnLoad();
	}

	private void OnClickEmoticon(IUIObject obj)
	{
		if (this.m_eChatType == CHAT_TYPE.BABELPARTY || this.m_eChatType == CHAT_TYPE.MYTHRAID)
		{
			BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
			if (babelTower_ChatDlg != null)
			{
				babelTower_ChatDlg.AddChatText((string)obj.Data);
			}
		}
		else if (this.m_eChatType == CHAT_TYPE.NUM)
		{
			New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
			if (new_Whisper_Dlg != null)
			{
				new_Whisper_Dlg.AddChatText((string)obj.Data);
			}
		}
		else if (this.m_eChatType == CHAT_TYPE.STORYCHAT)
		{
			StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
			if (storyChatDetailDlg != null)
			{
				storyChatDetailDlg.AddComment((string)obj.Data);
			}
		}
		else
		{
			ChatManager.AddChatText((string)obj.Data);
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

	public override void Update()
	{
		if (this.fTime > Time.realtimeSinceStartup)
		{
			return;
		}
		if (TsPlatform.IsEditor)
		{
			if (NkInputManager.GetMouseButtonUp(0) || NkInputManager.GetMouseButtonUp(1))
			{
				G_ID g_ID = (G_ID)NrTSingleton<FormsManager>.Instance.MouseOverFormID();
				if (!NrTSingleton<FormsManager>.Instance.IsMouseOverForm() || g_ID != G_ID.EMOTICON_DLG)
				{
					this.Close();
				}
			}
		}
		else if (TsPlatform.IsMobile && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			G_ID g_ID2 = (G_ID)NrTSingleton<FormsManager>.Instance.MouseOverFormID();
			if (!NrTSingleton<FormsManager>.Instance.IsMouseOverForm() || g_ID2 != G_ID.EMOTICON_DLG)
			{
				this.Close();
			}
		}
	}
}
