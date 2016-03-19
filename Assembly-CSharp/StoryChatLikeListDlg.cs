using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class StoryChatLikeListDlg : Form
{
	private Label m_Title;

	private NewListBox m_StoryChatLikeList;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "StoryChat/DLG_StoryChatLikeList", G_ID.STORYCHAT_LIKELIST_DLG, false, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_Title = (base.GetControl("LB_Title") as Label);
		this.m_StoryChatLikeList = (base.GetControl("NLB_Like") as NewListBox);
		base.SetScreenCenter();
	}

	public void SetStoryChatLikeList(StoryChatLike_Info[] array)
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("302");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"likenum",
			array.Length.ToString()
		});
		this.m_Title.Text = empty;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		this.m_StoryChatLikeList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			if (array[i].nPersonID == myCharInfo.m_PersonID)
			{
				flag = true;
			}
			bool flag2 = false;
			foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in myCharInfo.m_kFriendInfo.GetFriendInfoValues())
			{
				if (uSER_FRIEND_INFO.nPersonID == array[i].nPersonID)
				{
					flag2 = true;
					break;
				}
			}
			NewListItem newListItem = new NewListItem(this.m_StoryChatLikeList.ColumnNum, true);
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
				newListItem.SetListItemData(1, array[i].nCharKind, null, null, null);
			}
			string text = TKString.NEWString(array[i].szName);
			newListItem.SetListItemData(2, text, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152") + array[i].nLevel.ToString(), null, null, null);
			if (!flag)
			{
				newListItem.SetListItemData(4, string.Empty, array[i].nPersonID, new EZValueChangedDelegate(this.ClickUserInfo), null);
			}
			else
			{
				newListItem.SetListItemEnable(4, false);
			}
			if (!flag2 && !flag)
			{
				newListItem.SetListItemData(5, string.Empty, text, new EZValueChangedDelegate(this.ClickAddFriend), null);
			}
			else
			{
				newListItem.SetListItemEnable(5, false);
			}
			newListItem.SetListItemData(12, false);
			newListItem.Data = array[i];
			this.m_StoryChatLikeList.Add(newListItem);
		}
		this.m_StoryChatLikeList.RepositionItems();
	}

	private void ClickUserInfo(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		long nPersonID = (long)obj.Data;
		GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
		gS_OTHERCHAR_INFO_PERMIT_REQ.nPersonID = nPersonID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
	}

	private void ClickAddFriend(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		string text = (string)obj.Data;
		GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
		gS_FRIEND_APPLY_REQ.i32WorldID = 0;
		TKString.StringChar(text, ref gS_FRIEND_APPLY_REQ.name);
		SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("23"),
			"Charname",
			text
		});
		Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
	}
}
