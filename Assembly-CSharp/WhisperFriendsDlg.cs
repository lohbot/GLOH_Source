using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class WhisperFriendsDlg : Form
{
	private List<COMMUNITY_USER_INFO> m_CommunityUserList = new List<COMMUNITY_USER_INFO>();

	private NewListBox m_ListBox;

	private Button m_btnConfirm;

	private Button m_btClose;

	private int m_RoomUnique;

	public int RoomUnique
	{
		get
		{
			return this.m_RoomUnique;
		}
		set
		{
			this.m_RoomUnique = value;
			this.ShowListBox();
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Whisper/dlg_whisper_friend", G_ID.WHISPER_USERLIST_DLG, true);
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_ListBox = (base.GetControl("NewListBox_friend") as NewListBox);
		this.m_btnConfirm = (base.GetControl("Button_confirm") as Button);
		this.m_btnConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConfirm));
		foreach (USER_FRIEND_INFO userFriendInfo in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = new COMMUNITY_USER_INFO();
			cOMMUNITY_USER_INFO.Set(userFriendInfo);
			this.m_CommunityUserList.Add(cOMMUNITY_USER_INFO);
		}
	}

	private void ShowListBox()
	{
		this.m_ListBox.Clear();
		WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(this.m_RoomUnique);
		List<WhisperUser> users = room.GetUsers();
		List<long> list = new List<long>();
		foreach (WhisperUser current in users)
		{
			list.Add(current.PersonID);
		}
		for (int i = 0; i < this.m_CommunityUserList.Count; i++)
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = this.m_CommunityUserList[i];
			if (cOMMUNITY_USER_INFO != null && !list.Contains(cOMMUNITY_USER_INFO.i64PersonID) && cOMMUNITY_USER_INFO.bConnect)
			{
				NewListItem newListItem = new NewListItem(this.m_ListBox.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(0, cOMMUNITY_USER_INFO.strName, null, null, null);
				newListItem.SetListItemData(1, NrTSingleton<UIDataManager>.Instance.GetString("Lv ", cOMMUNITY_USER_INFO.i16Level.ToString()), null, null, null);
				newListItem.SetListItemData(2, CommunityUI_DLG.CommunityIcon(cOMMUNITY_USER_INFO), null, null, null);
				newListItem.Data = cOMMUNITY_USER_INFO;
				this.m_ListBox.Add(newListItem);
			}
		}
		this.m_ListBox.RepositionItems();
	}

	private void OnClickConfirm(IUIObject obj)
	{
		if (this.m_ListBox.SelectedItem != null)
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = this.m_ListBox.SelectedItem.Data as COMMUNITY_USER_INFO;
			if (cOMMUNITY_USER_INFO != null)
			{
				GS_WHISPER_INVITE_REQ gS_WHISPER_INVITE_REQ = new GS_WHISPER_INVITE_REQ();
				gS_WHISPER_INVITE_REQ.RoomUnique = this.m_RoomUnique;
				TKString.StringChar(cOMMUNITY_USER_INFO.strName, ref gS_WHISPER_INVITE_REQ.Name);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_INVITE_REQ, gS_WHISPER_INVITE_REQ);
				TsLog.LogWarning("GS_WHISPER_INVITE_REQ Room:{0} Name:{1}", new object[]
				{
					this.m_RoomUnique,
					cOMMUNITY_USER_INFO.strName
				});
			}
		}
		this.Close();
	}
}
