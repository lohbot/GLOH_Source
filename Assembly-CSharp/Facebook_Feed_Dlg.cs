using GAME;
using System;
using TsBundle;
using UnityForms;

public class Facebook_Feed_Dlg : Form
{
	private Button m_btnSend;

	private DrawTexture m_txImage;

	private Label m_lbTextTitle;

	private TextArea m_taMessage;

	private FacebookFeedData m_FeedData = new FacebookFeedData();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Facebook/dlg_facebook_feed", G_ID.FACEBOOK_FEED_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_btnSend = (base.GetControl("BT_Upload") as Button);
		this.m_btnSend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSend));
		this.m_txImage = (base.GetControl("DT_Image") as DrawTexture);
		this.m_lbTextTitle = (base.GetControl("LB_FaceBook") as Label);
		this.m_taMessage = (base.GetControl("TA_Text") as TextArea);
		this.m_taMessage.AddCommitDelegate(new EZKeyboardCommitDelegate(this.OnInputText));
		base.SetScreenCenter();
	}

	public void SetType(eFACEBOOK_FEED_TYPE _Type, object Data)
	{
		string text = string.Empty;
		FacebookFeedData facebookFeedData = NrTSingleton<Facebook_Feed_Manager>.Instance.Get_FeedData(_Type);
		if (facebookFeedData != null)
		{
			this.m_FeedData.Copy(facebookFeedData);
			switch (_Type)
			{
			case eFACEBOOK_FEED_TYPE.GET_SOL:
			{
				SOLDIER_INFO sOLDIER_INFO = Data as SOLDIER_INFO;
				if (sOLDIER_INFO != null)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(sOLDIER_INFO.CharKind);
					if (charKindInfo != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Title_Text_Key);
						this.m_lbTextTitle.Text = (this.m_FeedData.Title_Text_Key = text);
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Msg_Text_Key);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							text,
							"Rank",
							(int)(sOLDIER_INFO.Grade + 1),
							"solname",
							charKindInfo.GetName()
						});
						this.m_FeedData.Msg_Text_Key = text;
						TsLog.LogWarning("Title : {0}  Message : {1}", new object[]
						{
							this.m_FeedData.Title_Text_Key,
							this.m_FeedData.Msg_Text_Key
						});
					}
				}
				break;
			}
			case eFACEBOOK_FEED_TYPE.ENCHANT_SOL:
			{
				NkSoldierInfo nkSoldierInfo = Data as NkSoldierInfo;
				if (nkSoldierInfo != null)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Title_Text_Key);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"solname",
						nkSoldierInfo.GetName()
					});
					this.m_lbTextTitle.Text = text;
					this.m_FeedData.Title_Text_Key = text;
					text = string.Empty;
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Msg_Text_Key);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"solname",
						nkSoldierInfo.GetName()
					});
					this.m_FeedData.Msg_Text_Key = text;
					TsLog.LogWarning("Message :{0} Title : {1} ImageURL : {2} Msg_Text_Key = {3}", new object[]
					{
						this.m_taMessage.Text,
						this.m_FeedData.Title_Text_Key,
						this.m_FeedData.Web_ImgageURL,
						this.m_FeedData.Msg_Text_Key
					});
				}
				else
				{
					TsLog.LogError("Fecebook_Feed_Dlg NkSoldierInfo == NULL@@@@@@@@ ", new object[0]);
				}
				break;
			}
			case eFACEBOOK_FEED_TYPE.ENCHANT_ITEM:
			{
				ITEM iTEM = Data as ITEM;
				if (iTEM != null)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Title_Text_Key);
					this.m_lbTextTitle.Text = (this.m_FeedData.Title_Text_Key = text);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Msg_Text_Key);
					string name = NrTSingleton<ItemManager>.Instance.GetName(iTEM);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"item",
						name,
						"grade",
						this.ItemRankText(iTEM)
					});
					this.m_FeedData.Msg_Text_Key = text;
				}
				else
				{
					TsLog.LogError("Fecebook_Feed_Dlg ITEM == NULL@@@@@@@@ ", new object[0]);
				}
				break;
			}
			case eFACEBOOK_FEED_TYPE.PLUNDER_WIN:
			{
				char[] buffer = Data as char[];
				string text2 = TKString.NEWString(buffer);
				if (!string.IsNullOrEmpty(text2))
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Title_Text_Key);
					this.m_lbTextTitle.Text = (this.m_FeedData.Title_Text_Key = text);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Msg_Text_Key);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"username",
						text2
					});
					this.m_FeedData.Msg_Text_Key = text;
					TsLog.LogWarning("Message :{0} Title : {1} ImageURL : {2} Msg_Text_Key = {3}", new object[]
					{
						this.m_taMessage.Text,
						this.m_FeedData.Title_Text_Key,
						this.m_FeedData.Web_ImgageURL,
						this.m_FeedData.Msg_Text_Key
					});
				}
				else
				{
					TsLog.LogError("Fecebook_Feed_Dlg DeffenderName == NULL@@@@@@@@ ", new object[0]);
				}
				break;
			}
			case eFACEBOOK_FEED_TYPE.DKALCHE_SOL:
			{
				SOLDIER_INFO sOLDIER_INFO2 = Data as SOLDIER_INFO;
				if (sOLDIER_INFO2 != null)
				{
					NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(sOLDIER_INFO2.CharKind);
					if (charKindInfo2 != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Title_Text_Key);
						this.m_lbTextTitle.Text = (this.m_FeedData.Title_Text_Key = text);
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook(this.m_FeedData.Msg_Text_Key);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							text,
							"Rank",
							(int)(sOLDIER_INFO2.Grade + 1),
							"solname",
							charKindInfo2.GetName()
						});
						this.m_FeedData.Msg_Text_Key = text;
						TsLog.LogWarning("Title : {0}  Message : {1}", new object[]
						{
							this.m_FeedData.Title_Text_Key,
							this.m_FeedData.Msg_Text_Key
						});
					}
				}
				break;
			}
			}
			this.m_txImage.SetTextureKey(facebookFeedData.Game_ImageKey);
		}
		else
		{
			this.m_FeedData = null;
		}
	}

	private void OnInputText(IKeyFocusable obj)
	{
		if (this.m_FeedData != null)
		{
			this.m_FeedData.User_Message = this.m_taMessage.Text;
		}
	}

	public void OnClickSend(IUIObject obj)
	{
		if (!TsPlatform.IsMobile || TsPlatform.IsEditor)
		{
			this.Close();
			return;
		}
		if (this.m_FeedData != null)
		{
			if (string.IsNullOrEmpty(this.m_taMessage.Text))
			{
				this.m_taMessage.Text = "      ";
			}
			if (string.IsNullOrEmpty(this.m_FeedData.Web_ImgageURL))
			{
				this.m_FeedData.Web_ImgageURL = string.Format("{0}/facebookimg/200.png", Option.GetProtocolRootPath(Protocol.HTTP));
			}
			NmFacebookManager.instance.PostMessage(this.m_taMessage.Text, "http://yg.nexon.com/teaser/index.aspx", this.m_FeedData.Title_Text_Key, this.m_FeedData.Web_ImgageURL, string.Empty, this.m_FeedData.Msg_Text_Key);
		}
		this.Close();
	}

	private string ItemRankText(ITEM item)
	{
		eITEM_RANK_TYPE rank = item.GetRank();
		string result = string.Empty;
		switch (rank)
		{
		case eITEM_RANK_TYPE.ITEM_RANK_D:
			result = "D";
			break;
		case eITEM_RANK_TYPE.ITEM_RANK_C:
			result = "C";
			break;
		case eITEM_RANK_TYPE.ITEM_RANK_B:
			result = "B";
			break;
		case eITEM_RANK_TYPE.ITEM_RANK_A:
			result = "A";
			break;
		case eITEM_RANK_TYPE.ITEM_RANK_S:
			result = "S";
			break;
		case eITEM_RANK_TYPE.ITEM_RANK_SS:
			result = "SS";
			break;
		}
		return result;
	}
}
