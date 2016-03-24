using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class MineManager : NrTSingleton<MineManager>
{
	public long m_i64FirstLegionActionID_By_List;

	public long m_i64LastLegionActionID_By_List;

	private MineManager()
	{
	}

	public int GetMineDayLimitCount()
	{
		MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
		if (instance != null)
		{
			return instance.GetValue(eMINE_CONSTANT.eMINE_DAY_COUNT);
		}
		return 0;
	}

	public int GetMineJoinCount()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		return (int)kMyCharInfo.GetCharDetail(8);
	}

	public bool IsEnoughMineJoinCount()
	{
		return this.GetMineDayLimitCount() > this.GetMineJoinCount();
	}

	public void ClearDlg()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_MAINSELECT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_SEARCH_DLG);
	}

	public void Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(int page = 1, byte type = 1, long GuildID = 0L)
	{
		GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ = new GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ();
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i64GuildID = ((GuildID != 0L) ? GuildID : NrTSingleton<NewGuildManager>.Instance.GetGuildID());
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.i32Page = page;
		gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ.bType = type;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ, gS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ);
	}

	public void Send_GS_MINE_BATTLE_RESULT_LIST_REQ(bool bGiveComplete, short page, bool bNextRequest = false)
	{
		GS_MINE_BATTLE_RESULT_LIST_REQ gS_MINE_BATTLE_RESULT_LIST_REQ = new GS_MINE_BATTLE_RESULT_LIST_REQ();
		gS_MINE_BATTLE_RESULT_LIST_REQ.bGiveComplete = bGiveComplete;
		gS_MINE_BATTLE_RESULT_LIST_REQ.bNextRequest = bNextRequest;
		gS_MINE_BATTLE_RESULT_LIST_REQ.i16Page = page;
		gS_MINE_BATTLE_RESULT_LIST_REQ.i64FirstLegionActionID = this.m_i64FirstLegionActionID_By_List;
		gS_MINE_BATTLE_RESULT_LIST_REQ.i64LastLegionActionID = this.m_i64LastLegionActionID_By_List;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_BATTLE_RESULT_LIST_REQ, gS_MINE_BATTLE_RESULT_LIST_REQ);
	}

	public void Send_GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ(short page)
	{
		GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ gS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ = new GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ();
		gS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ.i16Page = page;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ, gS_MINE_BATTLE_RESULT_GUILDWAR_LIST_REQ);
	}
}
