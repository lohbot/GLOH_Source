using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BabelTower_FriendList : Form
{
	private NewListBox m_ListBox;

	private Label m_lbTitle;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_friend", G_ID.BABELTOWER_FRIENDLIST, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_ListBox = (base.GetControl("NLB_List") as NewListBox);
	}

	public void SetData(short nFloorIndex, short nFloorType)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("649"),
			"floor",
			nFloorIndex.ToString()
		});
		this.m_lbTitle.Text = empty;
		this.m_ListBox.Clear();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		List<FRIEND_BABEL_CLEARINFO> babelFloor_FriendList = kMyCharInfo.m_kFriendInfo.GetBabelFloor_FriendList(nFloorIndex, nFloorType);
		if (babelFloor_FriendList != null)
		{
			for (int i = 0; i < babelFloor_FriendList.Count; i++)
			{
				USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(babelFloor_FriendList[i].i64FriendPersonID);
				if (friend != null)
				{
					NewListItem newListItem = new NewListItem(5, true);
					Texture2D friendTexture = kMyCharInfo.GetFriendTexture(babelFloor_FriendList[i].i64FriendPersonID);
					if (friendTexture == null)
					{
						newListItem.SetListItemData(1, new NkListSolInfo
						{
							SolCharKind = friend.i32FaceCharKind,
							SolGrade = -1,
							SolLevel = friend.i16Level
						}, null, null, null);
					}
					else
					{
						newListItem.SetListItemData(1, friendTexture, null, null, null, null);
					}
					newListItem.SetListItemData(2, TKString.NEWString(friend.szName), null, null, null);
					newListItem.SetListItemData(3, "Lv." + friend.i16Level.ToString(), null, null, null);
					newListItem.SetListItemData(4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), friend, new EZValueChangedDelegate(this.BtClickWhisper), null);
					this.m_ListBox.Add(newListItem);
				}
			}
		}
		this.m_ListBox.RepositionItems();
	}

	public void BtClickWhisper(IUIObject obj)
	{
		USER_FRIEND_INFO uSER_FRIEND_INFO = obj.Data as USER_FRIEND_INFO;
		GS_WHISPER_REQ gS_WHISPER_REQ = new GS_WHISPER_REQ();
		gS_WHISPER_REQ.RoomUnique = 0;
		TKString.StringChar(TKString.NEWString(uSER_FRIEND_INFO.szName), ref gS_WHISPER_REQ.Name);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_REQ, gS_WHISPER_REQ);
		NrTSingleton<WhisperManager>.Instance.MySendRequest = true;
	}
}
