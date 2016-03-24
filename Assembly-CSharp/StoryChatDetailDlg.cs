using GAME;
using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class StoryChatDetailDlg : Form
{
	private Label m_Title;

	private Button m_Send;

	private Button m_Like;

	private Button m_Emoticon;

	private Button m_btClose;

	private TextField m_InputText;

	private NewListBox m_DetailList;

	private StoryChat_Info m_CurrentStoryChat;

	private long m_nLastCommentID;

	private bool m_bAddComment;

	private int m_nCommentCount;

	private StoryComment_Info m_CurrentCommentInfo;

	private bool m_bMyStoryChat;

	private bool m_bRequestComment;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "StoryChat/DLG_StoryChatDetail", G_ID.STORYCHATDETAIL_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_Title = (base.GetControl("LB_Title") as Label);
		this.m_Send = (base.GetControl("BT_Enter") as Button);
		this.m_Send.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSend));
		this.m_Like = (base.GetControl("BT_Like") as Button);
		this.m_Like.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickLike));
		this.m_Emoticon = (base.GetControl("BT_Emo") as Button);
		this.m_Emoticon.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEmoticon));
		this.m_InputText = (base.GetControl("Input_text") as TextField);
		this.m_InputText.maxLength = 51;
		this.m_InputText.Text = string.Empty;
		this.m_DetailList = (base.GetControl("NLB_News") as NewListBox);
		this.m_DetailList.Reserve = false;
		this.m_btClose = (base.GetControl("Button_Close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
	}

	private void ClickEmoticon(IUIObject obj)
	{
		EmoticonDlg emoticonDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EMOTICON_DLG) as EmoticonDlg;
		if (emoticonDlg != null)
		{
			emoticonDlg.SetScreenCenter();
			emoticonDlg.SetCharType(CHAT_TYPE.STORYCHAT);
		}
	}

	private void ClickLikeList(IUIObject obj)
	{
		GS_STORYCHATLIKE_GET_REQ gS_STORYCHATLIKE_GET_REQ = new GS_STORYCHATLIKE_GET_REQ();
		gS_STORYCHATLIKE_GET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHATLIKE_GET_REQ, gS_STORYCHATLIKE_GET_REQ);
	}

	private void ClickLike(IUIObject obj)
	{
		GS_STORYCHATLIKE_SET_REQ gS_STORYCHATLIKE_SET_REQ = new GS_STORYCHATLIKE_SET_REQ();
		gS_STORYCHATLIKE_SET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHATLIKE_SET_REQ, gS_STORYCHATLIKE_SET_REQ);
		this.m_Like.controlIsEnabled = false;
	}

	private void ClickSend(IUIObject obj)
	{
		if (0 >= this.m_InputText.Text.Length)
		{
			return;
		}
		if (string.Empty == this.m_InputText.Text)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		string text = this.m_InputText.Text;
		if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
		{
			text = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
			{
				text
			});
		}
		if (text.Contains("*"))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("797"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		if (this.m_DetailList.ChangeLineHeight)
		{
			this.m_DetailList.ChangeLineHeight = false;
			this.m_DetailList.LineHeight = 114f;
			this.m_DetailList.SetColumnData("Mobile/DLG/StoryChat/NLB_News01_ColumnData" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		this.m_CurrentCommentInfo = new StoryComment_Info();
		this.m_CurrentCommentInfo.nPersonID = myCharInfo.m_PersonID;
		this.m_CurrentCommentInfo.nCharKind = myCharInfo.GetImgFaceCharKind();
		this.m_CurrentCommentInfo.szName = TKString.StringChar(@char.GetCharName());
		this.m_CurrentCommentInfo.szMessage = TKString.StringChar(text);
		this.m_CurrentCommentInfo.nLevel = (short)myCharInfo.GetLevel();
		GS_STORYCOMMENT_SET_REQ gS_STORYCOMMENT_SET_REQ = new GS_STORYCOMMENT_SET_REQ();
		gS_STORYCOMMENT_SET_REQ.m_nStoryCommentID = 0L;
		gS_STORYCOMMENT_SET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
		TKString.StringChar(text, ref gS_STORYCOMMENT_SET_REQ.szMessage);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCOMMENT_SET_REQ, gS_STORYCOMMENT_SET_REQ);
	}

	public void SetCommentList(long _CommentID)
	{
		if (this.m_CurrentCommentInfo == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_DetailList.ColumnNum, true, string.Empty);
		Texture2D texture2D = null;
		if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.STORYCHAT_DLG))
		{
			StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
			if (storyChatDlg != null)
			{
				texture2D = storyChatDlg.GetFriendPortraitPersonID(this.m_CurrentCommentInfo.nPersonID);
			}
		}
		if (texture2D != null)
		{
			newListItem.SetListItemData(1, texture2D, null, null, null, null);
		}
		else
		{
			EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(this.m_CurrentCommentInfo.nCharKind);
			if (eventHeroCharFriendCode != null)
			{
				newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
				newListItem.EventMark = true;
			}
			newListItem.SetListItemData(1, new CostumeDrawTextureInfo
			{
				charKind = this.m_CurrentCommentInfo.nCharKind,
				grade = -1,
				costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.m_CurrentCommentInfo.nFaceCharCostumeUnique)
			}, null, null, null);
		}
		string text = TKString.NEWString(this.m_CurrentCommentInfo.szName);
		newListItem.SetListItemData(2, NrTSingleton<UIDataManager>.Instance.GetString(text, "(", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), this.m_CurrentCommentInfo.nLevel.ToString(), ")"), null, null, null);
		DateTime nowTime = PublicMethod.GetNowTime();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"month",
			nowTime.Month,
			"day",
			nowTime.Day,
			"hour",
			nowTime.Hour,
			"min",
			nowTime.Minute
		});
		newListItem.SetListItemData(3, empty, null, null, null);
		newListItem.SetListItemData(4, TKString.NEWString(this.m_CurrentCommentInfo.szMessage), null, null, null);
		newListItem.SetListItemData(5, string.Empty, text, new EZValueChangedDelegate(this.ClickUser), null);
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			if (text == @char.GetCharName())
			{
				newListItem.SetListItemData(6, string.Empty, _CommentID, new EZValueChangedDelegate(this.DeleteStoryComment), null);
			}
			else
			{
				newListItem.SetListItemData(6, false);
			}
		}
		else
		{
			newListItem.SetListItemData(6, false);
		}
		newListItem.Data = _CommentID;
		this.m_nCommentCount++;
		this.m_DetailList.InsertAdd(this.m_DetailList.Count, newListItem);
		this.m_DetailList.RepositionItems();
		this.m_DetailList.ScrollPosition = 1f;
		this.m_CurrentCommentInfo = null;
	}

	public void SetStoryChat(StoryChat_Info info, bool battleReplay)
	{
		GS_STORYCOMMENT_GET_REQ gS_STORYCOMMENT_GET_REQ = new GS_STORYCOMMENT_GET_REQ();
		gS_STORYCOMMENT_GET_REQ.nStoryChatID = info.nStoryChatID;
		gS_STORYCOMMENT_GET_REQ.nStoryCommentID = 0L;
		gS_STORYCOMMENT_GET_REQ.nLoadCount = 10;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCOMMENT_GET_REQ, gS_STORYCOMMENT_GET_REQ);
		this.m_CurrentStoryChat = info;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("322");
		string text = string.Empty;
		string text2 = TKString.NEWString(info.szName);
		if (!battleReplay)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface,
				"targetname",
				text2
			});
		}
		else if (info.nCharKind == 8)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1307");
		}
		else if (info.nCharKind == 6)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("436");
		}
		else if (info.nCharKind == 5)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("43");
		}
		this.m_Title.Text = text;
		this.m_DetailList.ChangeLineHeight = true;
		this.m_DetailList.LineHeight = 160f;
		this.m_DetailList.Clear();
		NewListItem newListItem = new NewListItem(this.m_DetailList.ColumnNum, true, string.Empty);
		if (!battleReplay)
		{
			Texture2D texture2D = null;
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.STORYCHAT_DLG))
			{
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg != null)
				{
					texture2D = storyChatDlg.GetFriendPortraitPersonID(info.nPersonID);
				}
			}
			if (texture2D != null)
			{
				newListItem.SetListItemData(1, texture2D, null, null, null, null);
			}
			else
			{
				EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(info.nCharKind);
				if (eventHeroCharFriendCode != null)
				{
					newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
					newListItem.EventMark = true;
				}
				newListItem.SetListItemData(1, new CostumeDrawTextureInfo
				{
					charKind = info.nCharKind,
					grade = -1,
					costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(info.nFaceCharCostumeUnique)
				}, null, null, null);
			}
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152") + info.nLevel.ToString(), null, null, null);
			string text3 = TKString.NEWString(info.szMessage);
			text3 = text3.Replace("\n", "\r\n");
			newListItem.SetListItemData(6, text3, null, null, null);
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				if (text2 == @char.GetCharName())
				{
					this.m_bMyStoryChat = true;
				}
				else
				{
					newListItem.SetListItemData(13, false);
					this.m_bMyStoryChat = false;
				}
			}
		}
		else
		{
			char[] separator = new char[]
			{
				'/'
			};
			string[] array = TKString.NEWString(info.szMessage).Split(separator);
			if (info.nCharKind == 8)
			{
				newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1956"),
					"targetname1",
					array[1],
					"targetname2",
					array[2]
				});
				newListItem.SetListItemData(6, empty, null, null, null);
			}
			else if (info.nCharKind == 6)
			{
				newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1957"),
					"targetname1",
					array[1],
					"targetname2",
					array[2]
				});
				newListItem.SetListItemData(6, empty2, null, null, null);
			}
			else if (info.nCharKind == 5)
			{
				newListItem.SetListItemData(1, NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_Challenge02"), null, null, null);
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1957"),
					"targetname1",
					array[1],
					"targetname2",
					array[2]
				});
				newListItem.SetListItemData(6, empty3, null, null, null);
			}
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(13, false);
			this.m_bMyStoryChat = false;
		}
		newListItem.SetListItemData(2, text2, null, null, null);
		DateTime dueDate = PublicMethod.GetDueDate(info.nTime);
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
		text = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			textFromInterface,
			"month",
			dueDate.Month,
			"day",
			dueDate.Day,
			"hour",
			dueDate.Hour,
			"min",
			dueDate.Minute
		});
		newListItem.SetListItemData(4, text, null, null, null);
		newListItem.SetListItemData(9, info.nLikeCount.ToString(), null, null, null);
		newListItem.SetListItemData(10, info.nCommentCount.ToString(), null, null, null);
		newListItem.SetListItemData(11, string.Empty, info.nStoryChatID, new EZValueChangedDelegate(this.ClickLikeList), null);
		newListItem.SetListItemData(12, string.Empty, text2, new EZValueChangedDelegate(this.ClickUser), null);
		this.m_DetailList.Add(newListItem);
		this.m_DetailList.RepositionItems();
	}

	public void SetStoryCommentList(byte canLike, StoryComment_Info[] array)
	{
		if (canLike == 0)
		{
			this.m_Like.controlIsEnabled = false;
		}
		if (array == null)
		{
			return;
		}
		if (this.m_DetailList.ChangeLineHeight)
		{
			this.m_DetailList.ChangeLineHeight = false;
			this.m_DetailList.LineHeight = 114f;
			this.m_DetailList.SetColumnData("Mobile/DLG/StoryChat/NLB_News01_ColumnData" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		string b = string.Empty;
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			b = @char.GetCharName();
		}
		int num = 0;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (array[i].nStoryCommentID != 0L)
			{
				NewListItem newListItem = new NewListItem(this.m_DetailList.ColumnNum, true, string.Empty);
				Texture2D texture2D = null;
				if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.STORYCHAT_DLG))
				{
					StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
					if (storyChatDlg != null)
					{
						texture2D = storyChatDlg.GetFriendPortraitPersonID(array[i].nPersonID);
					}
				}
				if (texture2D != null)
				{
					newListItem.SetListItemData(1, texture2D, null, null, null, null);
				}
				else
				{
					EVENT_HERODATA eventHeroCharFriendCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharFriendCode(array[i].nCharKind);
					if (eventHeroCharFriendCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.EventMark = true;
					}
					newListItem.SetListItemData(1, new CostumeDrawTextureInfo
					{
						charKind = array[i].nCharKind,
						grade = -1,
						costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(array[i].nFaceCharCostumeUnique)
					}, null, null, null);
				}
				string text = TKString.NEWString(array[i].szName);
				newListItem.SetListItemData(2, NrTSingleton<UIDataManager>.Instance.GetString(text, "(", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), array[i].nLevel.ToString(), ")"), null, null, null);
				DateTime dueDate = PublicMethod.GetDueDate(array[i].nTime);
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("301");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"month",
					dueDate.Month,
					"day",
					dueDate.Day,
					"hour",
					dueDate.Hour,
					"min",
					dueDate.Minute
				});
				newListItem.SetListItemData(3, empty, null, null, null);
				string text2 = TKString.NEWString(array[i].szMessage);
				newListItem.SetListItemData(4, text2, null, null, null);
				newListItem.SetListItemData(5, string.Empty, text, new EZValueChangedDelegate(this.ClickUser), null);
				if (this.m_bMyStoryChat || text == b)
				{
					newListItem.SetListItemData(6, string.Empty, array[i].nStoryCommentID, new EZValueChangedDelegate(this.DeleteStoryComment), null);
				}
				else
				{
					newListItem.SetListItemData(6, false);
				}
				newListItem.Data = array[i];
				if (!this.m_bAddComment)
				{
					this.m_DetailList.Add(newListItem);
					TsLog.LogError("m_bAddComment ={0} CommentID:{1} Message:{2}", new object[]
					{
						this.m_bAddComment,
						array[i].nStoryCommentID,
						text2
					});
				}
				else
				{
					this.m_DetailList.InsertAdd(1 + num++, newListItem);
					TsLog.LogError("InsertAdd m_bAddComment ={0} Count = {3} CommentID:{1} Message:{2}", new object[]
					{
						this.m_bAddComment,
						array[i].nStoryCommentID,
						text2,
						num
					});
				}
				this.m_nCommentCount++;
			}
		}
		this.m_DetailList.RepositionItems();
		this.m_bAddComment = true;
		this.m_nLastCommentID = array[array.Length - 1].nStoryCommentID;
		TsLog.LogError("m_nCommentCount ={0} m_nLastCommentID ={1}", new object[]
		{
			this.m_nCommentCount,
			this.m_nLastCommentID
		});
	}

	public void DeleteStoryComment(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long num = (long)obj.Data;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.RequestDeleteStoryComment), num, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("75"), eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
	}

	private void RequestDeleteStoryComment(object obj)
	{
		if (obj == null)
		{
			return;
		}
		long nStoryCommentID = (long)obj;
		GS_STORYCOMMENT_SET_REQ gS_STORYCOMMENT_SET_REQ = new GS_STORYCOMMENT_SET_REQ();
		gS_STORYCOMMENT_SET_REQ.m_nStoryCommentID = nStoryCommentID;
		gS_STORYCOMMENT_SET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
		TKString.StringChar(string.Empty, ref gS_STORYCOMMENT_SET_REQ.szMessage);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCOMMENT_SET_REQ, gS_STORYCOMMENT_SET_REQ);
	}

	public void ClickUser(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		string text = (string)obj.Data;
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		if (text == nrCharUser.GetCharName())
		{
			return;
		}
		NrTSingleton<CRightClickMenu>.Instance.CreateUI(0L, 0, text, CRightClickMenu.KIND.CHAT_USER_LINK_TEXT, CRightClickMenu.TYPE.NAME_SECTION_2, false);
	}

	public override void Update()
	{
		if (this.m_CurrentStoryChat != null && this.m_bAddComment && this.m_DetailList.listMoved && !this.m_bRequestComment)
		{
			GS_STORYCOMMENT_GET_REQ gS_STORYCOMMENT_GET_REQ = new GS_STORYCOMMENT_GET_REQ();
			gS_STORYCOMMENT_GET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
			gS_STORYCOMMENT_GET_REQ.nStoryCommentID = this.m_nLastCommentID;
			gS_STORYCOMMENT_GET_REQ.nLoadCount = 10;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCOMMENT_GET_REQ, gS_STORYCOMMENT_GET_REQ);
			this.m_bRequestComment = true;
		}
		if (!this.m_DetailList.listMoved && this.m_bRequestComment)
		{
			this.m_bRequestComment = false;
		}
	}

	public void UpdateCommentNumText(bool flag)
	{
		if (flag)
		{
			this.m_nCommentCount++;
		}
		else
		{
			int count = this.m_DetailList.Count;
			for (int i = 1; i < count; i++)
			{
				this.m_DetailList.RemoveItem(1, true);
			}
			this.m_DetailList.RepositionItems();
			this.m_nCommentCount--;
			GS_STORYCOMMENT_GET_REQ gS_STORYCOMMENT_GET_REQ = new GS_STORYCOMMENT_GET_REQ();
			gS_STORYCOMMENT_GET_REQ.nStoryChatID = this.m_CurrentStoryChat.nStoryChatID;
			gS_STORYCOMMENT_GET_REQ.nStoryCommentID = 0L;
			gS_STORYCOMMENT_GET_REQ.nLoadCount = 10;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCOMMENT_GET_REQ, gS_STORYCOMMENT_GET_REQ);
		}
		Label label = this.m_DetailList.GetItem(0).GetElement(10) as Label;
		if (null != label)
		{
			label.Text = this.m_CurrentStoryChat.nCommentCount.ToString();
		}
		this.m_InputText.Text = string.Empty;
	}

	public void UpdateLikeNumText()
	{
		Label label = this.m_DetailList.GetItem(0).GetElement(9) as Label;
		if (null != label)
		{
			label.Text = this.m_CurrentStoryChat.nLikeCount.ToString();
		}
	}

	public void AddComment(string msg)
	{
		TextField expr_06 = this.m_InputText;
		expr_06.Text += msg;
		TextField expr_1D = this.m_InputText;
		expr_1D.OriginalContent += msg;
	}

	public override void OnClose()
	{
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		base.OnClose();
	}
}
