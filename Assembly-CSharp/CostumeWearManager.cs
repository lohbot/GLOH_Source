using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class CostumeWearManager : NrTSingleton<CostumeWearManager>
{
	private CostumeWearManager()
	{
	}

	public void RequestCostumeWear(NkSoldierInfo solInfo, int costumeUnique)
	{
		if (solInfo == null)
		{
			return;
		}
		if (solInfo.GetSolPosType() == 2)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("864"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return;
		}
		if (solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME) == (long)costumeUnique)
		{
			return;
		}
		COSTUME_INFO costumeInfo = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeInfo(costumeUnique);
		if (!costumeData.IsNormalCostume() && (costumeInfo == null || costumeInfo.i32CostumePossibleToUse <= 0))
		{
			return;
		}
		int num = costumeUnique;
		if (costumeData.IsNormalCostume())
		{
			num = 0;
		}
		GS_SOLDIER_SUBDATA_REQ gS_SOLDIER_SUBDATA_REQ = new GS_SOLDIER_SUBDATA_REQ();
		gS_SOLDIER_SUBDATA_REQ.kSolSubData.nSolID = solInfo.GetSolID();
		gS_SOLDIER_SUBDATA_REQ.kSolSubData.nSubDataType = 14;
		gS_SOLDIER_SUBDATA_REQ.kSolSubData.nSubDataValue = (long)num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_SUBDATA_REQ, gS_SOLDIER_SUBDATA_REQ);
	}

	public void Refresh(NkSoldierInfo soldierInfo, bool refreshMyCharList, bool refreshCostumeSaleList)
	{
		this.CostumeRoomRefresh(refreshMyCharList, refreshCostumeSaleList);
		this.LeaderCharChange(ref soldierInfo);
	}

	public void ShowCostumeChangeMsg(NkSoldierInfo soldierInfo)
	{
		this.ShowCostumeChangeMessage(ref soldierInfo);
	}

	private void CostumeRoomRefresh(bool refreshMyCharList, bool refreshCostumeSaleList)
	{
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null)
		{
			return;
		}
		costumeRoom_Dlg.Refresh(refreshMyCharList, refreshCostumeSaleList);
	}

	private void LeaderCharChange(ref NkSoldierInfo soldierInfo)
	{
		if (soldierInfo == null)
		{
			return;
		}
		if (soldierInfo.GetSolID() != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetFaceSolID())
		{
			return;
		}
		GS_CHARACTER_SUBDATA_REQ gS_CHARACTER_SUBDATA_REQ = new GS_CHARACTER_SUBDATA_REQ();
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataType = 0;
		gS_CHARACTER_SUBDATA_REQ.kCharSubData.nSubDataValue = soldierInfo.GetSolID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_SUBDATA_REQ, gS_CHARACTER_SUBDATA_REQ);
	}

	private void ShowCostumeChangeMessage(ref NkSoldierInfo soldierInfo)
	{
		if (soldierInfo == null)
		{
			return;
		}
		if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) is CostumeRoom_Dlg))
		{
			return;
		}
		int costumeUnique = (int)soldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(costumeUnique);
		if (costumeData == null || costumeData.IsNormalCostume())
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("863"),
			"itemname",
			NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(costumeUnique)
		});
		Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
	}
}
