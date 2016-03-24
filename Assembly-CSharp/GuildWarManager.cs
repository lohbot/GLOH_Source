using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class GuildWarManager : NrTSingleton<GuildWarManager>
{
	public List<GUILDWAR_REWARD_DATA> m_GuildWarRewardList = new List<GUILDWAR_REWARD_DATA>();

	public bool bIsGuildWar;

	public bool bIsGuildWarCancelReservation;

	private bool bCanGetGuildWarReward;

	private GuildWarManager()
	{
	}

	public void Clear()
	{
		this.m_GuildWarRewardList.Clear();
	}

	public void Set_Value(GUILDWAR_REWARD_DATA data)
	{
		this.m_GuildWarRewardList.Add(data);
	}

	public bool CanGetGuildWarReward()
	{
		return this.bCanGetGuildWarReward;
	}

	public void SetCanGetGuildWarReward(bool bCanGetGuildWarReward)
	{
		this.bCanGetGuildWarReward = bCanGetGuildWarReward;
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
		}
		GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
		if (guildCollect_DLG != null)
		{
			guildCollect_DLG.Update_Notice();
		}
	}

	public eGUILDWAR_STATE GetWarState()
	{
		if (!this.bIsGuildWar)
		{
			return eGUILDWAR_STATE.eGUILDWAR_STATE_NONE;
		}
		if (this.bIsGuildWarCancelReservation)
		{
			return eGUILDWAR_STATE.eGUILDWAR_STATE_RESERVATION_CANCEL;
		}
		return eGUILDWAR_STATE.eGUILDWAR_STATE_WAR;
	}

	public void ClearDlg()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GUILDWAR_LIST_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GUILDWAR_MAIN_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GUILDWAR_REWARDINFO_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_RECORD_GUILDWAR_DLG);
	}

	public void Send_GS_GUILDWAR_APPLY_REQ(eGUILDWAR_STATE requestState)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			return;
		}
		GS_GUILDWAR_APPLY_REQ gS_GUILDWAR_APPLY_REQ = new GS_GUILDWAR_APPLY_REQ();
		gS_GUILDWAR_APPLY_REQ.bRequestState = (byte)requestState;
		SendPacket.GetInstance().SendObject(2206, gS_GUILDWAR_APPLY_REQ);
	}

	public void Send_GS_GUILDWAR_MATCH_LIST_REQ(short iPageIndex)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			return;
		}
		GS_GUILDWAR_MATCH_LIST_REQ gS_GUILDWAR_MATCH_LIST_REQ = new GS_GUILDWAR_MATCH_LIST_REQ();
		gS_GUILDWAR_MATCH_LIST_REQ.i16CurPage = iPageIndex;
		SendPacket.GetInstance().SendObject(2200, gS_GUILDWAR_MATCH_LIST_REQ);
	}

	public void Send_GS_GUILDWAR_RANKINFO_REQ(short i16CurPage)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			return;
		}
		GS_GUILDWAR_RANKINFO_REQ gS_GUILDWAR_RANKINFO_REQ = new GS_GUILDWAR_RANKINFO_REQ();
		gS_GUILDWAR_RANKINFO_REQ.i16CurPage = i16CurPage;
		SendPacket.GetInstance().SendObject(2208, gS_GUILDWAR_RANKINFO_REQ);
	}

	public void Send_GS_GUILDWAR_IS_WAR_TIME_REQ()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_GUILDWAR_IS_WAR_TIME_REQ, new GS_GUILDWAR_IS_WAR_TIME_REQ());
	}

	public void Send_GS_GUILDWAR_INFO_REQ()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewGuildWarLimit())
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_GUILDWAR_INFO_REQ, new GS_GUILDWAR_INFO_REQ());
	}
}
