using GAME;
using Global;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BabelTowerInviteFriendListDlg : Form
{
	private NewListBox m_lbCommunityList;

	private Button m_btReset;

	private Dictionary<long, COMMUNITY_USER_INFO> m_dicCommunityList = new Dictionary<long, COMMUNITY_USER_INFO>();

	public List<long> m_RecentBabelPlayerList = new List<long>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_invite", G_ID.BABELTOWER_INVITEFRIENDLIST_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbCommunityList = (base.GetControl("nlb_friendlist") as NewListBox);
		this.m_btReset = (base.GetControl("BT_Reset") as Button);
		this.m_btReset.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickReset));
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		base.OnClose();
	}

	public void ShowInivteList()
	{
		this.ShowList();
		base.Show();
	}

	public void SetList()
	{
		this.m_RecentBabelPlayerList.Clear();
		for (int i = 0; i < 4; i++)
		{
			string s = string.Empty;
			s = PlayerPrefs.GetString("Babel JoinPlayer" + i, "0");
			long num = long.Parse(s);
			if (num > 0L)
			{
				this.m_RecentBabelPlayerList.Add(num);
			}
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		this.m_dicCommunityList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (!this.m_dicCommunityList.ContainsKey(uSER_FRIEND_INFO.nPersonID))
			{
				if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique || (int)uSER_FRIEND_INFO.i16Level >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BOUNTY_INVITE_LEVEL))
				{
					COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = new COMMUNITY_USER_INFO();
					cOMMUNITY_USER_INFO.Set(uSER_FRIEND_INFO);
					this.m_dicCommunityList.Add(uSER_FRIEND_INFO.nPersonID, cOMMUNITY_USER_INFO);
				}
			}
		}
		for (int j = 0; j < NrTSingleton<NewGuildManager>.Instance.GetMemberCount(); j++)
		{
			NewGuildMember memberInfoFromIndex = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromIndex(j);
			if (memberInfoFromIndex != null)
			{
				if (memberInfoFromIndex.GetPersonID() != charPersonInfo.GetPersonID())
				{
					if (!this.m_dicCommunityList.ContainsKey(memberInfoFromIndex.GetPersonID()))
					{
						if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique || (int)memberInfoFromIndex.GetLevel() >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BOUNTY_INVITE_LEVEL))
						{
							COMMUNITY_USER_INFO cOMMUNITY_USER_INFO2 = new COMMUNITY_USER_INFO();
							cOMMUNITY_USER_INFO2.Set(memberInfoFromIndex);
							this.m_dicCommunityList.Add(memberInfoFromIndex.GetPersonID(), cOMMUNITY_USER_INFO2);
						}
					}
				}
			}
		}
		foreach (BABEL_RNDINVITE_PERSON current in SoldierBatch.BABELTOWER_INFO.m_Babel_RndInvitePersonList)
		{
			if (!this.m_dicCommunityList.ContainsKey(current.i64PersonID))
			{
				if (0 >= SoldierBatch.BABELTOWER_INFO.BountHuntUnique || (int)current.i16Level >= COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BOUNTY_INVITE_LEVEL))
				{
					USER_FRIEND_INFO uSER_FRIEND_INFO2 = new USER_FRIEND_INFO();
					uSER_FRIEND_INFO2.nPersonID = current.i64PersonID;
					uSER_FRIEND_INFO2.szName = current.szCharName;
					uSER_FRIEND_INFO2.i16Level = current.i16Level;
					uSER_FRIEND_INFO2.i32WorldID_Connect = current.i32WorldID;
					uSER_FRIEND_INFO2.i32MapUnique = 1;
					COMMUNITY_USER_INFO cOMMUNITY_USER_INFO3 = new COMMUNITY_USER_INFO();
					cOMMUNITY_USER_INFO3.Set(uSER_FRIEND_INFO2);
					this.m_dicCommunityList.Add(cOMMUNITY_USER_INFO3.i64PersonID, cOMMUNITY_USER_INFO3);
				}
			}
		}
	}

	private static int RecentBabelPlayer(COMMUNITY_USER_INFO x, COMMUNITY_USER_INFO y)
	{
		BabelTowerInviteFriendListDlg babelTowerInviteFriendListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_INVITEFRIENDLIST_DLG) as BabelTowerInviteFriendListDlg;
		if (babelTowerInviteFriendListDlg == null)
		{
			return 0;
		}
		for (int i = 0; i < babelTowerInviteFriendListDlg.m_RecentBabelPlayerList.Count; i++)
		{
			if (x.i64PersonID == babelTowerInviteFriendListDlg.m_RecentBabelPlayerList[i])
			{
				return -1;
			}
		}
		return 1;
	}

	public void ShowList()
	{
		this.SetList();
		this.m_lbCommunityList.Clear();
		for (int i = 0; i < this.m_RecentBabelPlayerList.Count; i++)
		{
			long key = this.m_RecentBabelPlayerList[i];
			if (this.m_dicCommunityList.ContainsKey(key))
			{
				COMMUNITY_USER_INFO listItem = new COMMUNITY_USER_INFO();
				this.m_dicCommunityList.TryGetValue(key, out listItem);
				NewListItem item = this.SetListItem(listItem);
				this.m_lbCommunityList.Add(item);
			}
		}
		foreach (COMMUNITY_USER_INFO current in this.m_dicCommunityList.Values)
		{
			if (current.i32MapUnique > 0)
			{
				if (!this.m_RecentBabelPlayerList.Contains(current.i64PersonID))
				{
					NewListItem item2 = this.SetListItem(current);
					this.m_lbCommunityList.Add(item2);
				}
			}
		}
		this.m_lbCommunityList.RepositionItems();
	}

	private NewListItem SetListItem(COMMUNITY_USER_INFO info)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NewListItem newListItem = new NewListItem(this.m_lbCommunityList.ColumnNum, true);
		string text = string.Empty;
		string text2 = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1030");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"name",
			info.strName
		});
		newListItem.SetListItemData(0, text2, null, null, null);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count",
			info.i16Level
		});
		newListItem.SetListItemData(1, text2, null, null, null);
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575");
		newListItem.SetListItemData(2, text2, info, new EZValueChangedDelegate(this.BtnClickWhisper), null);
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("542");
		newListItem.SetListItemData(3, text2, info, new EZValueChangedDelegate(this.BtnInvite), null);
		text2 = Client.GetInstance().Get_WorldServerName_InfoFromID(info.i32WorldID_Connect);
		newListItem.SetListItemData(4, text2, null, null, null);
		if (kMyCharInfo.m_kFriendInfo.IsFriend(info.i64PersonID) || NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(info.i64PersonID) != null)
		{
			newListItem.SetListItemData(5, true);
			newListItem.SetListItemData(6, true);
		}
		else
		{
			newListItem.SetListItemData(5, false);
			newListItem.SetListItemData(6, false);
		}
		newListItem.Data = info;
		return newListItem;
	}

	public void BtnClickWhisper(IUIObject obj)
	{
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = obj.Data as COMMUNITY_USER_INFO;
		if (cOMMUNITY_USER_INFO != null)
		{
			GS_WHISPER_REQ gS_WHISPER_REQ = new GS_WHISPER_REQ();
			gS_WHISPER_REQ.RoomUnique = 0;
			TKString.StringChar(cOMMUNITY_USER_INFO.strName, ref gS_WHISPER_REQ.Name);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_REQ, gS_WHISPER_REQ);
			NrTSingleton<WhisperManager>.Instance.MySendRequest = true;
		}
	}

	public void BtnInvite(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = obj.Data as COMMUNITY_USER_INFO;
		if (cOMMUNITY_USER_INFO != null && SoldierBatch.BABELTOWER_INFO != null)
		{
			int index = this.ListBox_Index(cOMMUNITY_USER_INFO.i64PersonID);
			NewListItem newListItem = new NewListItem(this.m_lbCommunityList.ColumnNum, true);
			string text = string.Empty;
			string text2 = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1030");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"name",
				cOMMUNITY_USER_INFO.strName
			});
			newListItem.SetListItemData(0, text2, null, null, null);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				text,
				"count",
				cOMMUNITY_USER_INFO.i16Level
			});
			newListItem.SetListItemData(1, text2, null, null, null);
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575");
			newListItem.SetListItemData(2, text2, cOMMUNITY_USER_INFO, new EZValueChangedDelegate(this.BtnClickWhisper), null);
			newListItem.SetListItemEnable(3, false);
			text2 = Client.GetInstance().Get_WorldServerName_InfoFromID(cOMMUNITY_USER_INFO.i32WorldID_Connect);
			newListItem.SetListItemData(4, text2, null, null, null);
			if (kMyCharInfo.m_kFriendInfo.IsFriend(cOMMUNITY_USER_INFO.i64PersonID))
			{
				newListItem.SetListItemData(5, true);
				newListItem.SetListItemData(6, true);
			}
			else
			{
				newListItem.SetListItemData(5, false);
				newListItem.SetListItemData(6, false);
			}
			this.m_lbCommunityList.RemoveAdd(index, newListItem);
			this.m_lbCommunityList.RepositionItems();
			GS_BABELTOWER_INVITE_FRIEND_REQ gS_BABELTOWER_INVITE_FRIEND_REQ = new GS_BABELTOWER_INVITE_FRIEND_REQ();
			gS_BABELTOWER_INVITE_FRIEND_REQ.InvitePersonID = cOMMUNITY_USER_INFO.i64PersonID;
			gS_BABELTOWER_INVITE_FRIEND_REQ.floor = SoldierBatch.BABELTOWER_INFO.m_nBabelFloor;
			gS_BABELTOWER_INVITE_FRIEND_REQ.sub_floor = SoldierBatch.BABELTOWER_INFO.m_nBabelSubFloor;
			gS_BABELTOWER_INVITE_FRIEND_REQ.floortype = SoldierBatch.BABELTOWER_INFO.m_nBabelFloorType;
			gS_BABELTOWER_INVITE_FRIEND_REQ.i16BountyHuntUnique = SoldierBatch.BABELTOWER_INFO.BountHuntUnique;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_INVITE_FRIEND_REQ, gS_BABELTOWER_INVITE_FRIEND_REQ);
		}
	}

	private int ListBox_Index(long _friend_personid)
	{
		for (int i = 0; i < this.m_lbCommunityList.Count; i++)
		{
			IUIListObject item = this.m_lbCommunityList.GetItem(i);
			if (item != null)
			{
				COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)item.Data;
				if (cOMMUNITY_USER_INFO != null && cOMMUNITY_USER_INFO.i64PersonID == _friend_personid)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void BtnClickReset(IUIObject obj)
	{
		this.SetList();
	}
}
