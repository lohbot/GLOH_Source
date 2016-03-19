using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BountyHuntManager : NrTSingleton<BountyHuntManager>
{
	private List<BountyInfoData> m_BountyInfoDataList = new List<BountyInfoData>();

	private List<BountyEcoData> m_BountyEcoDataList = new List<BountyEcoData>();

	private List<BountyHuntNPCInfo> m_ClientNpcList = new List<BountyHuntNPCInfo>();

	private int m_nAutoMoveMapIndex = -1;

	private int m_nAutoMoveGateIndex = -1;

	private short m_iAutoMoveBountyHuntUnique;

	private short m_iWeek;

	public short AutoMoveBountyHuntUnique
	{
		get
		{
			return this.m_iAutoMoveBountyHuntUnique;
		}
		set
		{
			this.m_iAutoMoveBountyHuntUnique = value;
		}
	}

	public short Week
	{
		get
		{
			return this.m_iWeek;
		}
		set
		{
			this.m_iWeek = value;
		}
	}

	private BountyHuntManager()
	{
	}

	public bool AddBountyInfoData(BountyInfoData Data)
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (Data.i16Week == this.m_BountyInfoDataList[i].i16Week && Data.i16Page == this.m_BountyInfoDataList[i].i16Page && Data.i16Episode == this.m_BountyInfoDataList[i].i16Episode)
			{
				this.m_BountyInfoDataList[i] = Data;
				return true;
			}
		}
		this.m_BountyInfoDataList.Add(Data);
		return true;
	}

	public BountyInfoData GetBountyInfoDataFromUnique(short iUnique)
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (iUnique == this.m_BountyInfoDataList[i].i16Unique)
			{
				return this.m_BountyInfoDataList[i];
			}
		}
		return null;
	}

	public BountyInfoData GetBountyInfoData(short iWeek, short iPage, short iEpisode)
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (iWeek == this.m_BountyInfoDataList[i].i16Week && iPage == this.m_BountyInfoDataList[i].i16Page && iEpisode == this.m_BountyInfoDataList[i].i16Episode)
			{
				return this.m_BountyInfoDataList[i];
			}
		}
		return null;
	}

	public bool AddBountyEcoData(BountyEcoData Data)
	{
		for (int i = 0; i < this.m_BountyEcoDataList.Count; i++)
		{
			if (Data.i16EcoIndex == this.m_BountyEcoDataList[i].i16EcoIndex)
			{
				this.m_BountyEcoDataList[i] = Data;
				return true;
			}
		}
		this.m_BountyEcoDataList.Add(Data);
		return true;
	}

	public BountyEcoData GetBountyEcoData(short iEcoIndex)
	{
		for (int i = 0; i < this.m_BountyEcoDataList.Count; i++)
		{
			if (iEcoIndex == this.m_BountyEcoDataList[i].i16EcoIndex)
			{
				return this.m_BountyEcoDataList[i];
			}
		}
		return null;
	}

	public string GetBountyRankImgText(byte rank)
	{
		string result = string.Empty;
		switch (rank)
		{
		case 0:
			result = "Win_I_Rank";
			break;
		case 1:
			result = "Win_I_RankD";
			break;
		case 2:
			result = "Win_I_RankC";
			break;
		case 3:
			result = "Win_I_RankB";
			break;
		case 4:
			result = "Win_I_RankA";
			break;
		case 5:
			result = "Win_I_RankS";
			break;
		case 6:
			result = "Win_I_RankSS";
			break;
		}
		return result;
	}

	public BountyHuntNPCInfo UpdateClientNpc(int iMapIndex)
	{
		if (Scene.CurScene != Scene.Type.WORLD)
		{
			return null;
		}
		if (iMapIndex == 0)
		{
			iMapIndex = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex;
		}
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			BountyHuntNPCInfo bountyHuntNPCInfo = this.m_ClientNpcList[i];
			if (bountyHuntNPCInfo != null)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(bountyHuntNPCInfo.CharID);
			}
		}
		this.m_ClientNpcList.Clear();
		BountyInfoData bountyInfoDataFromUnique = this.GetBountyInfoDataFromUnique(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique);
		if (bountyInfoDataFromUnique != null)
		{
			if (iMapIndex != bountyInfoDataFromUnique.i32MapIndex)
			{
				return null;
			}
			if (this.Week != bountyInfoDataFromUnique.i16Week)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique = 0;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearBountyHuntClearInfo();
			}
			else
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(bountyInfoDataFromUnique.i32NPCCharKind);
				if (charKindInfo != null)
				{
					int num = UnityEngine.Random.Range(0, 5);
					NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
					nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfo.GetName());
					nEW_MAKECHAR_INFO.CharPos.x = bountyInfoDataFromUnique.fFixPosX[num];
					nEW_MAKECHAR_INFO.CharPos.y = 0f;
					nEW_MAKECHAR_INFO.CharPos.z = bountyInfoDataFromUnique.fFixPosZ[num];
					int num2 = bountyInfoDataFromUnique.iDirection[num];
					float f = (float)num2 * 0.0174532924f;
					nEW_MAKECHAR_INFO.Direction.x = 1f * Mathf.Sin(f);
					nEW_MAKECHAR_INFO.Direction.y = 0f;
					nEW_MAKECHAR_INFO.Direction.z = 1f * Mathf.Cos(f);
					nEW_MAKECHAR_INFO.CharKind = charKindInfo.GetCharKind();
					nEW_MAKECHAR_INFO.CharKindType = 3;
					nEW_MAKECHAR_INFO.CharUnique = NrTSingleton<NkCharManager>.Instance.GetClientNpcUnique();
					if (nEW_MAKECHAR_INFO.CharUnique == 0)
					{
						GS_BOUNTYHUNT_LOG_REQ gS_BOUNTYHUNT_LOG_REQ = new GS_BOUNTYHUNT_LOG_REQ();
						gS_BOUNTYHUNT_LOG_REQ.i16BountyHuntUnique = bountyInfoDataFromUnique.i16Unique;
						gS_BOUNTYHUNT_LOG_REQ.i32CharID = (int)nEW_MAKECHAR_INFO.CharUnique;
						gS_BOUNTYHUNT_LOG_REQ.i32MapIndex = iMapIndex;
						SendPacket.GetInstance().SendObject(1930, gS_BOUNTYHUNT_LOG_REQ);
					}
					int num3 = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, false);
					TsLog.LogOnlyEditor(string.Concat(new object[]
					{
						"BountyHunt UpdateClientNpc MapIndex : ",
						iMapIndex,
						" : ",
						num3
					}));
					BountyHuntNPCInfo bountyHuntNPCInfo2 = new BountyHuntNPCInfo();
					bountyHuntNPCInfo2.MapIndex = iMapIndex;
					bountyHuntNPCInfo2.CharID = num3;
					bountyHuntNPCInfo2.BountyHuntUnique = bountyInfoDataFromUnique.i16Unique;
					bountyHuntNPCInfo2.Pos = nEW_MAKECHAR_INFO.CharPos;
					bountyHuntNPCInfo2.RandIndex = num;
					this.m_ClientNpcList.Add(bountyHuntNPCInfo2);
					return bountyHuntNPCInfo2;
				}
			}
		}
		return null;
	}

	public void AutoMoveClientNPC(short iBountyHuntUnique)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		Vector3 lhs = this.FindFirstPath(iBountyHuntUnique);
		if (lhs != Vector3.zero)
		{
			GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
			gS_CHAR_FINDPATH_REQ.DestPos.x = lhs.x;
			gS_CHAR_FINDPATH_REQ.DestPos.y = lhs.y;
			gS_CHAR_FINDPATH_REQ.DestPos.z = lhs.z;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "AUTOMOVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public Vector3 FindFirstPath(short iBountyHuntUnique)
	{
		Vector3 result = Vector3.zero;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return result;
		}
		int mapIndex = myCharInfo.m_kCharMapInfo.MapIndex;
		BountyInfoData bountyInfoDataFromUnique = this.GetBountyInfoDataFromUnique(iBountyHuntUnique);
		if (bountyInfoDataFromUnique == null)
		{
			return result;
		}
		if (bountyInfoDataFromUnique.i32MapIndex != mapIndex)
		{
			this.IsWarp(iBountyHuntUnique, bountyInfoDataFromUnique.i32MapIndex);
		}
		else
		{
			BountyHuntNPCInfo bountyHuntNPCInfo = this.GetNPCInfo((int)bountyInfoDataFromUnique.i16Unique);
			if (bountyHuntNPCInfo == null)
			{
				bountyHuntNPCInfo = this.UpdateClientNpc(bountyInfoDataFromUnique.i32MapIndex);
			}
			if (bountyHuntNPCInfo != null)
			{
				int i32MapIndex = bountyInfoDataFromUnique.i32MapIndex;
				short destX = (short)bountyHuntNPCInfo.Pos.x;
				short destY = (short)bountyHuntNPCInfo.Pos.z;
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null && @char.m_kCharMove != null)
				{
					result = @char.m_kCharMove.FindFirstPath(i32MapIndex, destX, destY, false);
				}
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BOUNTYHUNTING_DLG);
		return result;
	}

	private bool IsWarp(short iBountyHuntUnique, int destMapIndex)
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName(destMapIndex);
		if (!(mapName != string.Empty))
		{
			return false;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return false;
		}
		ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
		if (gateInfo_Col == null)
		{
			return false;
		}
		int num = 0;
		foreach (GATE_INFO gATE_INFO in gateInfo_Col)
		{
			if (destMapIndex == gATE_INFO.DST_MAP_IDX)
			{
				num = gATE_INFO.GATE_IDX;
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
			"mapname",
			mapName
		});
		msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num, new NoDelegate(this.MsgCancel), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL);
		msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
		msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		this.m_nAutoMoveMapIndex = destMapIndex;
		this.m_nAutoMoveGateIndex = num;
		this.m_iAutoMoveBountyHuntUnique = iBountyHuntUnique;
		return true;
	}

	private void MapWarp(object obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetWarp(true, this.m_nAutoMoveGateIndex, this.m_nAutoMoveMapIndex);
	}

	private void MsgCancel(object obj)
	{
		this.m_iAutoMoveBountyHuntUnique = 0;
	}

	public BountyHuntNPCInfo GetNPCInfo(int iBountyHuntUnique)
	{
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			BountyHuntNPCInfo bountyHuntNPCInfo = this.m_ClientNpcList[i];
			if (bountyHuntNPCInfo != null)
			{
				if ((int)bountyHuntNPCInfo.BountyHuntUnique == iBountyHuntUnique)
				{
					if (NrTSingleton<NkCharManager>.Instance.GetChar(bountyHuntNPCInfo.CharID) != null)
					{
						return bountyHuntNPCInfo;
					}
				}
			}
		}
		return null;
	}

	public bool IsAllClear(short i16Week)
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (i16Week == this.m_BountyInfoDataList[i].i16Week)
			{
				if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBountyHuntClearUnique(this.m_BountyInfoDataList[i].i16Unique))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsBountyHuntNextEpisode(short iBountyHuntUnique)
	{
		BountyInfoData bountyInfoDataFromUnique = this.GetBountyInfoDataFromUnique(iBountyHuntUnique);
		if (bountyInfoDataFromUnique == null)
		{
			return false;
		}
		if (bountyInfoDataFromUnique.i16Page == 1 && bountyInfoDataFromUnique.i16Episode == 1)
		{
			return true;
		}
		short oldBountyHuntUnique = NrTSingleton<BountyHuntManager>.Instance.GetOldBountyHuntUnique(iBountyHuntUnique);
		return oldBountyHuntUnique != 0 && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBountyHuntClearUnique(oldBountyHuntUnique);
	}

	public void ClearUpdateNPC()
	{
		for (int i = 0; i < this.m_ClientNpcList.Count; i++)
		{
			BountyHuntNPCInfo bountyHuntNPCInfo = this.m_ClientNpcList[i];
			if (bountyHuntNPCInfo != null)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(bountyHuntNPCInfo.CharID);
			}
		}
		this.m_ClientNpcList.Clear();
	}

	public bool IsNextPage(short iWeek)
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (this.m_BountyInfoDataList[i].i16Week == iWeek)
			{
				if (1 < this.m_BountyInfoDataList[i].i16Page)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CheckBountyHuntInfoNPCCharKind()
	{
		for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
		{
			if (this.m_BountyInfoDataList[i].i32NPCCharKind > 0)
			{
				this.m_BountyInfoDataList[i].i32NPCCharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(this.m_BountyInfoDataList[i].strNPCCharCode);
			}
		}
	}

	public short GetOldBountyHuntUnique(short iBountyHuntUnique)
	{
		short result = 0;
		BountyInfoData bountyInfoDataFromUnique = this.GetBountyInfoDataFromUnique(iBountyHuntUnique);
		if (bountyInfoDataFromUnique != null)
		{
			for (int i = 0; i < this.m_BountyInfoDataList.Count; i++)
			{
				if (this.m_BountyInfoDataList[i].i16Unique < iBountyHuntUnique && this.m_BountyInfoDataList[i].i16Week == bountyInfoDataFromUnique.i16Week)
				{
					result = this.m_BountyInfoDataList[i].i16Unique;
				}
			}
		}
		return result;
	}
}
