using GAME;
using GameMessage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class FacebookFriendInviteDlg : Form
{
	private const int MAX_VIEW_LIST = 4;

	private NewListBox m_ListBox;

	private Button m_btnBack;

	private Button m_btnNext;

	private Box m_bxPage;

	private int m_HistoryMaxView;

	private int m_nMaxPage;

	private int m_nCurPage;

	private List<FacebookUserData> m_InviteList;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Community/InviteFacebookFriend/dlg_facebook_friend", G_ID.FACEBOOK_FRIEND_INVITE, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_ListBox = (base.GetControl("NLB_FaceBooK") as NewListBox);
		this.m_btnBack = (base.GetControl("BT_Back") as Button);
		this.m_btnBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPrev));
		this.m_btnNext = (base.GetControl("BT_Next") as Button);
		this.m_btnNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNext));
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		MsgHandler.Handle("FacebookFriendDataArrage", new object[0]);
		this.m_InviteList = new List<FacebookUserData>(NmFacebookManager.instance.FriendsData.Values);
		if (this.m_InviteList != null && this.m_InviteList.Count > 0)
		{
			this.m_InviteList.Sort(new Comparison<FacebookUserData>(this.ComparerData));
		}
		TsLog.LogWarning("m_InviteList Count = {0} NmFacebookManager.instance.FriendsData.Count = {1}", new object[]
		{
			this.m_InviteList.Count,
			NmFacebookManager.instance.FriendsData.Count
		});
		this.m_nCurPage = 1;
		this.m_nMaxPage = this.m_InviteList.Count / 4 + 1;
		this.PageView();
		this.RequestData();
		this.UpdateList();
	}

	private void PageView()
	{
		this.m_bxPage.Text = string.Format("{0}/{1}", this.m_nCurPage, this.m_nMaxPage);
	}

	public void OnClickNext(IUIObject obj)
	{
		this.m_nCurPage++;
		if (this.m_nCurPage > this.m_nMaxPage)
		{
			this.m_nCurPage = this.m_nMaxPage;
			return;
		}
		if (this.m_nCurPage <= this.m_nMaxPage && this.m_HistoryMaxView < this.m_nCurPage)
		{
			this.m_HistoryMaxView = this.m_nCurPage;
			this.RequestData();
		}
		this.UpdateList();
		this.PageView();
	}

	public void OnClickPrev(IUIObject obj)
	{
		this.m_nCurPage--;
		if (this.m_nCurPage < 1)
		{
			this.m_nCurPage = 1;
			return;
		}
		this.UpdateList();
		this.PageView();
	}

	public void OnClickInvateGame(IUIObject obj)
	{
		FacebookUserData facebookUserData = (FacebookUserData)obj.Data;
		if (facebookUserData == null)
		{
			return;
		}
		NmFacebookManager.instance.FriendRequestMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromFacebook("15"), facebookUserData.m_ID);
		GS_CHAR_CHALLENGE_INVITESNG_REQ gS_CHAR_CHALLENGE_INVITESNG_REQ = new GS_CHAR_CHALLENGE_INVITESNG_REQ();
		gS_CHAR_CHALLENGE_INVITESNG_REQ.invite_sngtype = 1;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_CHALLENGE_INVITESNG_REQ, gS_CHAR_CHALLENGE_INVITESNG_REQ);
	}

	public void OnClickInvateFriend(IUIObject obj)
	{
		FacebookUserData facebookUserData = (FacebookUserData)obj.Data;
		if (facebookUserData == null)
		{
			return;
		}
		GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
		gS_FRIEND_APPLY_REQ.i32WorldID = 0;
		TKString.StringChar(facebookUserData.m_GameName, ref gS_FRIEND_APPLY_REQ.name);
		SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("23"),
			"Charname",
			facebookUserData.m_Name
		});
	}

	private int RequestData()
	{
		int num = 0;
		GS_FACEBOOK_FRIENDINFO_GET_REQ gS_FACEBOOK_FRIENDINFO_GET_REQ = new GS_FACEBOOK_FRIENDINFO_GET_REQ();
		for (int i = 0; i < 4; i++)
		{
			int num2 = 4 * (this.m_nCurPage - 1) + i;
			if (num2 < this.m_InviteList.Count && this.m_InviteList[num2].m_Installed && this.m_InviteList[num2].Level == 0)
			{
				char[] array = new char[65];
				TKString.StringChar(this.m_InviteList[num2].m_ID, ref array);
				for (int j = 0; j < 65; j++)
				{
					gS_FACEBOOK_FRIENDINFO_GET_REQ.FaceBookID[i, j] = array[j];
				}
				TsLog.LogWarning("RequestData SetData ID = {0}", new object[]
				{
					this.m_InviteList[num2].m_ID
				});
				num++;
			}
		}
		if (num > 0)
		{
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FACEBOOK_FRIENDINFO_GET_REQ, gS_FACEBOOK_FRIENDINFO_GET_REQ);
		}
		return num;
	}

	public void UpdateList()
	{
		this.m_ListBox.Clear();
		for (int i = 0; i < 4; i++)
		{
			int num = 4 * (this.m_nCurPage - 1) + i;
			if (num < this.m_InviteList.Count)
			{
				this.AddListItem(this.m_InviteList[num]);
			}
			else
			{
				TsLog.LogWarning("AddItemList Pos = {0}", new object[]
				{
					num
				});
			}
		}
		this.m_ListBox.RepositionItems();
	}

	private void AddListItem(FacebookUserData _ItemData)
	{
		TsLog.LogWarning("AddListItem _ItemData = {0} , GameName = {1}", new object[]
		{
			_ItemData.m_ID,
			_ItemData.m_GameName
		});
		NewListItem newListItem = new NewListItem(this.m_ListBox.ColumnNum, true);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(_ItemData.nFaceCharKind);
		if (charKindInfo != null)
		{
			newListItem.SetListItemData(3, charKindInfo.GetCharKind(), null, null, null);
			newListItem.SetListItemData(9, false);
		}
		else
		{
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(9, true);
		}
		if (string.IsNullOrEmpty(_ItemData.m_GameName))
		{
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(5, false);
			newListItem.SetListItemData(7, _ItemData.m_Name, null, null, null);
		}
		else
		{
			newListItem.SetListItemData(4, _ItemData.m_GameName, null, null, null);
			newListItem.SetListItemData(5, _ItemData.m_Name, null, null, null);
			newListItem.SetListItemData(7, false);
		}
		if (_ItemData.Level != 0)
		{
			newListItem.SetListItemData(6, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("178"), _ItemData, new EZValueChangedDelegate(this.OnClickInvateFriend), null);
			newListItem.SetListItemData(8, false);
		}
		else
		{
			newListItem.SetListItemData(6, false);
			newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("179"), _ItemData, new EZValueChangedDelegate(this.OnClickInvateGame), null);
		}
		newListItem.Data = _ItemData.m_ID;
		this.m_ListBox.Add(newListItem);
	}

	public void SetUserData(FACEBOOK_FRIEND_GAMEINFO Data)
	{
		FacebookUserData facebookUserData = this.FindFriendData(TKString.NEWString(Data.szFaceBookID));
		if (facebookUserData != null)
		{
			facebookUserData.m_GameName = TKString.NEWString(Data.szCharName);
			facebookUserData.nFaceCharKind = Data.nFaceCharKind;
			facebookUserData.Level = Data.nCharLevel;
			NmFacebookManager.instance.FriendsData[facebookUserData.m_ID] = facebookUserData;
		}
	}

	public FacebookUserData FindFriendData(string FacebookID)
	{
		for (int i = 0; i < this.m_InviteList.Count; i++)
		{
			if (this.m_InviteList[i].m_ID == FacebookID)
			{
				return this.m_InviteList[i];
			}
		}
		return null;
	}

	private int ComparerData(FacebookUserData a, FacebookUserData b)
	{
		if (a.m_Installed && !b.m_Installed)
		{
			return -1;
		}
		if (!a.m_Installed && b.m_Installed)
		{
			return 1;
		}
		return 1;
	}

	public static void FacebookFriendUpdate(FACEBOOK_FRIEND_GAMEINFO Data)
	{
		SortedDictionary<string, FacebookUserData> friendsData = NmFacebookManager.instance.FriendsData;
		string key = TKString.NEWString(Data.szFaceBookID);
		if (friendsData.ContainsKey(key))
		{
			friendsData[key].Level = Data.nCharLevel;
			friendsData[key].nFaceCharKind = Data.nFaceCharKind;
			friendsData[key].m_GameName = TKString.NEWString(Data.szCharName);
		}
	}

	private void RemoveData()
	{
		List<FacebookUserData> list = new List<FacebookUserData>();
		for (int i = 0; i < this.m_InviteList.Count; i++)
		{
			if (!this.m_InviteList[i].m_Installed)
			{
				list.Add(this.m_InviteList[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			this.m_InviteList.Remove(list[j]);
		}
		if (this.m_InviteList.Count == 0)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("204"));
			this.Close();
		}
	}
}
