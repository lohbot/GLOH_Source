using GAME;
using Ndoors.Memory;
using PROTOCOL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityForms;

public class NrFriendInfo
{
	private Dictionary<long, USER_FRIEND_INFO> m_dicFriendList;

	private Dictionary<long, FRIEND_BABEL_CLEARINFO> m_dcFriendsBabelData;

	private Dictionary<long, Texture2D> m_dicFriendTextureData;

	public NrFriendInfo()
	{
		this.m_dicFriendList = new Dictionary<long, USER_FRIEND_INFO>();
		this.m_dcFriendsBabelData = new Dictionary<long, FRIEND_BABEL_CLEARINFO>();
		this.m_dicFriendTextureData = new Dictionary<long, Texture2D>();
	}

	~NrFriendInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_dicFriendList.Clear();
		this.m_dcFriendsBabelData.Clear();
		this.m_dicFriendTextureData.Clear();
	}

	public void AddFriend(USER_FRIEND_INFO kFriendInfo)
	{
		if (this.m_dicFriendList.ContainsKey(kFriendInfo.nPersonID))
		{
			this.m_dicFriendList.Remove(kFriendInfo.nPersonID);
		}
		this.m_dicFriendList.Add(kFriendInfo.nPersonID, kFriendInfo);
		if (kFriendInfo.nPersonID > 10L && !this.m_dicFriendTextureData.ContainsKey(kFriendInfo.nPersonID))
		{
			this.m_dicFriendTextureData.Add(kFriendInfo.nPersonID, null);
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(kFriendInfo.nPersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserImageCallback), kFriendInfo.nPersonID);
		}
	}

	private void ReqWebUserImageCallback(Texture2D txtr, object _param)
	{
		long num = (long)_param;
		if (num == 0L)
		{
			return;
		}
		if (!this.m_dicFriendTextureData.ContainsKey(num))
		{
			return;
		}
		this.m_dicFriendTextureData[num] = txtr;
	}

	public Texture2D GetPersonIDTexture(long i64PersonID)
	{
		if (!this.m_dicFriendTextureData.ContainsKey(i64PersonID))
		{
			return null;
		}
		return this.m_dicFriendTextureData[i64PersonID];
	}

	public FRIEND_HELPSOLINFO GetHelpSolInfo(long FriendID)
	{
		if (this.m_dicFriendList.ContainsKey(FriendID))
		{
			return this.m_dicFriendList[FriendID].FriendHelpSolInfo;
		}
		return null;
	}

	public void DelFriend(long i64FriendPersonID)
	{
		if (!this.m_dicFriendTextureData.ContainsKey(i64FriendPersonID))
		{
			return;
		}
		this.m_dicFriendTextureData.Remove(i64FriendPersonID);
		if (!this.m_dicFriendList.ContainsKey(i64FriendPersonID))
		{
			return;
		}
		this.m_dicFriendList.Remove(i64FriendPersonID);
	}

	public void UpdateFriend(USER_FRIEND_INFO kFriendInfo)
	{
		if (!this.m_dicFriendList.ContainsKey(kFriendInfo.nPersonID))
		{
			return;
		}
		USER_FRIEND_INFO uSER_FRIEND_INFO = this.m_dicFriendList[kFriendInfo.nPersonID];
		uSER_FRIEND_INFO.Update(kFriendInfo);
	}

	public USER_FRIEND_INFO GetFriend(long i64FriendPersonID)
	{
		if (!this.m_dicFriendList.ContainsKey(i64FriendPersonID))
		{
			return null;
		}
		return this.m_dicFriendList[i64FriendPersonID];
	}

	public bool IsFriend(long i64FriendPersonID)
	{
		return this.m_dicFriendList.ContainsKey(i64FriendPersonID);
	}

	public int GetFriendCount()
	{
		int num = 0;
		foreach (USER_FRIEND_INFO current in this.m_dicFriendList.Values)
		{
			if (current.nPersonID > 11L)
			{
				num++;
			}
		}
		return num;
	}

	[DebuggerHidden]
	public IEnumerable GetFriendInfoValues()
	{
		NrFriendInfo.<GetFriendInfoValues>c__Iterator9 <GetFriendInfoValues>c__Iterator = new NrFriendInfo.<GetFriendInfoValues>c__Iterator9();
		<GetFriendInfoValues>c__Iterator.<>f__this = this;
		NrFriendInfo.<GetFriendInfoValues>c__Iterator9 expr_0E = <GetFriendInfoValues>c__Iterator;
		expr_0E.$PC = -2;
		return expr_0E;
	}

	public long GetFriendPersonIDByName(string Name)
	{
		foreach (USER_FRIEND_INFO current in this.m_dicFriendList.Values)
		{
			if (TKString.NEWString(current.szName).CompareTo(Name) == 0)
			{
				return current.nPersonID;
			}
		}
		return 0L;
	}

	public long GetFriendPersonIDByFacebookID(string FacebookID)
	{
		foreach (USER_FRIEND_INFO current in this.m_dicFriendList.Values)
		{
			if (TKString.NEWString(current.szFaceBookID).CompareTo(FacebookID) == 0)
			{
				return current.nPersonID;
			}
		}
		return 0L;
	}

	public string GetFriendPlatformNameByFacebookID(string FacebookID)
	{
		foreach (USER_FRIEND_INFO current in this.m_dicFriendList.Values)
		{
			if (TKString.NEWString(current.szFaceBookID).CompareTo(FacebookID) == 0)
			{
				return TKString.NEWString(current.szPlatformName);
			}
		}
		return string.Empty;
	}

	public int GetFriendsBaBelDataCount()
	{
		return this.m_dcFriendsBabelData.Count;
	}

	public void AddFriendBabel(FRIEND_BABEL_CLEARINFO kFriendBabelInfo)
	{
		if (this.m_dcFriendsBabelData.ContainsKey(kFriendBabelInfo.i64FriendPersonID))
		{
			this.m_dcFriendsBabelData.Remove(kFriendBabelInfo.i64FriendPersonID);
		}
		this.m_dcFriendsBabelData.Add(kFriendBabelInfo.i64FriendPersonID, kFriendBabelInfo);
	}

	public void DelFriendBabel(long nPersonID)
	{
		if (this.m_dcFriendsBabelData.ContainsKey(nPersonID))
		{
			this.m_dcFriendsBabelData.Remove(nPersonID);
		}
	}

	public void UpdateFriendBabelData(long nPersonID, short i16Floor, byte nSubfloor, short nFloorType, byte nRank = 0)
	{
		if (this.m_dcFriendsBabelData.ContainsKey(nPersonID))
		{
			if (this.m_dcFriendsBabelData[nPersonID].i16Floor < i16Floor || (this.m_dcFriendsBabelData[nPersonID].i16Floor == i16Floor && nSubfloor > this.m_dcFriendsBabelData[nPersonID].ui8SubFloor))
			{
				this.m_dcFriendsBabelData[nPersonID].i16Floor = i16Floor;
				this.m_dcFriendsBabelData[nPersonID].ui8SubFloor = nSubfloor;
				this.m_dcFriendsBabelData[nPersonID].i16FloorType = nFloorType;
				if (nRank != 0)
				{
					this.m_dcFriendsBabelData[nPersonID].ui8Rank = nRank;
				}
			}
		}
		else
		{
			FRIEND_BABEL_CLEARINFO fRIEND_BABEL_CLEARINFO = new FRIEND_BABEL_CLEARINFO();
			fRIEND_BABEL_CLEARINFO.i64FriendPersonID = nPersonID;
			fRIEND_BABEL_CLEARINFO.i16Floor = i16Floor;
			fRIEND_BABEL_CLEARINFO.ui8SubFloor = nSubfloor;
			fRIEND_BABEL_CLEARINFO.ui8Rank = nRank;
			fRIEND_BABEL_CLEARINFO.i16FloorType = nFloorType;
			if (this.m_dcFriendsBabelData != null)
			{
				this.m_dcFriendsBabelData.Add(fRIEND_BABEL_CLEARINFO.i64FriendPersonID, fRIEND_BABEL_CLEARINFO);
			}
		}
		BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
		if (babelTowerMainDlg != null && babelTowerMainDlg.visible)
		{
			babelTowerMainDlg.ShowList();
		}
	}

	public List<FRIEND_BABEL_CLEARINFO> GetBabelFloor_FriendList(short i16Floor, short FloorType)
	{
		List<FRIEND_BABEL_CLEARINFO> list = new List<FRIEND_BABEL_CLEARINFO>();
		foreach (FRIEND_BABEL_CLEARINFO current in this.m_dcFriendsBabelData.Values)
		{
			if (current.i16Floor == i16Floor && current.i16FloorType == FloorType)
			{
				list.Add(current);
				TsLog.Log("GetBabelFloor_FriendList add: {0}  firendid: {1}", new object[]
				{
					i16Floor,
					current.i64FriendPersonID
				});
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list;
	}
}
