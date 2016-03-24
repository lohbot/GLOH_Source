using ICSharpCode.SharpZipLib.Zip;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TsBundle;
using UnityEngine;

public class NkBattleReplayManager : NrTSingleton<NkBattleReplayManager>
{
	public List<NkBattleReplay> m_ReplayList;

	public bool bGS_BATTLE_INFO_NFY;

	public bool bGS_BF_TURNINFO_NFY;

	private bool m_bReplay;

	private float m_fReplayStartTime;

	public float m_fLogStartTime;

	private bool m_bSaveReplay;

	public bool m_bHiddenEnemyName;

	public List<NkBattleReplay> m_SaveList;

	private string m_szLoadFile = string.Empty;

	private string m_szDirectory = string.Empty;

	private bool m_bRequestWebReplay;

	public bool IsReplay
	{
		get
		{
			return this.m_bReplay;
		}
	}

	public bool SaveReplay
	{
		get
		{
			return this.m_bSaveReplay;
		}
		set
		{
			this.m_bSaveReplay = value;
		}
	}

	public string LoadFile
	{
		get
		{
			return this.m_szLoadFile;
		}
		set
		{
			this.m_szLoadFile = value;
		}
	}

	public string Directory
	{
		get
		{
			return this.m_szDirectory;
		}
	}

	private NkBattleReplayManager()
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			if (this.m_szDirectory == string.Empty)
			{
				this.m_szDirectory = string.Format("{0}/BattleReplay", TsPlatform.Operator.GetFileDir());
			}
		}
		else
		{
			this.m_szDirectory = "c:\\BattleReplay";
		}
	}

	public void Init()
	{
	}

	public void ClearReplayFlag()
	{
		if (this.m_bReplay)
		{
			GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
			gS_CHAR_STATE_SET_REQ.nSet = 4;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
		}
		this.m_bReplay = false;
		Battle.Replay = false;
		if (this.m_ReplayList != null)
		{
			this.m_ReplayList.Clear();
		}
	}

	public void RequestReplayHttp(long nPlunderID)
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		this.m_bRequestWebReplay = true;
		string text = (nPlunderID / 1000L).ToString();
		string url = string.Format("http://{0}/replay/{1}/{2}/{3}_Plunder_replay.zip", new object[]
		{
			NrGlobalReference.strWebPageDomain,
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_szWorldType,
			text.ToString(),
			nPlunderID.ToString()
		});
		Helper.RequestDownloadWebFile(url, true, new PostProcPerItem(this.OnDownLoadReplayDataZip), null);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
	}

	public void RequestReplayColosseumHttp(long ColossumID)
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		this.m_bRequestWebReplay = true;
		string text = (ColossumID / 1000L).ToString();
		string url = string.Format("http://{0}/replay/{1}/{2}/{3}_Colosseum_replay.zip", new object[]
		{
			NrGlobalReference.strWebPageDomain,
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_szWorldType,
			text.ToString(),
			ColossumID.ToString()
		});
		Helper.RequestDownloadWebFile(url, true, new PostProcPerItem(this.OnDownLoadReplayDataZip), true);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
	}

	public void RequestReplayMineHttp(long nLegionActionID)
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		this.m_bRequestWebReplay = true;
		string text = (nLegionActionID / 1000L).ToString();
		string url = string.Format("http://{0}/replay/{1}/{2}/{3}_Mine_replay.zip", new object[]
		{
			NrGlobalReference.strWebPageDomain,
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_szWorldType,
			text.ToString(),
			nLegionActionID.ToString()
		});
		Helper.RequestDownloadWebFile(url, true, new PostProcPerItem(this.OnDownLoadReplayDataZip), null);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
	}

	public void RequestReplayinfiBattleHttp(long ninfiBattleID)
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		this.m_bRequestWebReplay = true;
		string text = (ninfiBattleID / 1000L).ToString();
		string text2 = string.Format("http://{0}/replay/{1}/{2}/{3}_Infi_replay.zip", new object[]
		{
			NrGlobalReference.strWebPageDomain,
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_szWorldType,
			text.ToString(),
			ninfiBattleID.ToString()
		});
		TsLog.LogWarning("!!!! replay {0}", new object[]
		{
			text2
		});
		Helper.RequestDownloadWebFile(text2, true, new PostProcPerItem(this.OnDownLoadReplayDataZip), null);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
	}

	public void RequestExpeditionReplay(long nExpeditionID)
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		this.m_bRequestWebReplay = true;
		string text = (nExpeditionID / 1000L).ToString();
		string text2 = string.Format("http://{0}/replay/{1}/{2}/{3}_Expedition_replay.zip", new object[]
		{
			NrGlobalReference.strWebPageDomain,
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_szWorldType,
			text.ToString(),
			nExpeditionID.ToString()
		});
		TsLog.LogWarning("!!!! replay {0}", new object[]
		{
			text2
		});
		Helper.RequestDownloadWebFile(text2, true, new PostProcPerItem(this.OnDownLoadReplayDataZip), null);
		GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
		gS_CHAR_STATE_SET_REQ.nSet = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
	}

	public void LoadReplayData()
	{
		if (this.m_bRequestWebReplay)
		{
			return;
		}
		if (this.m_szLoadFile == string.Empty)
		{
			this.m_szLoadFile = string.Format("{0}/battlereplay.dat", this.m_szDirectory);
		}
		else
		{
			this.m_szLoadFile = string.Format("{0}/{1}", this.m_szDirectory, this.m_szLoadFile);
		}
		string szLoadFile = this.m_szLoadFile;
		if (this.m_ReplayList == null)
		{
			this.m_ReplayList = new List<NkBattleReplay>();
		}
		else
		{
			this.m_ReplayList.Clear();
		}
		if (File.Exists(szLoadFile))
		{
			try
			{
				using (Stream stream = File.Open(szLoadFile, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream))
					{
						while (binaryReader.PeekChar() != -1)
						{
							NkBattleReplay nkBattleReplay = new NkBattleReplay();
							nkBattleReplay.fTime = binaryReader.ReadSingle();
							nkBattleReplay.nType = binaryReader.ReadInt32();
							nkBattleReplay.nSize = binaryReader.ReadInt32();
							nkBattleReplay.pData = binaryReader.ReadBytes(nkBattleReplay.nSize);
							this.m_ReplayList.Add(nkBattleReplay);
						}
					}
				}
			}
			catch (Exception obj)
			{
				TsLog.LogWarning(obj);
			}
			this.bGS_BATTLE_INFO_NFY = false;
			this.bGS_BF_TURNINFO_NFY = false;
		}
		this.m_szLoadFile = string.Empty;
	}

	public bool LoadFromBinary(byte[] bytes)
	{
		if (this.m_ReplayList == null)
		{
			this.m_ReplayList = new List<NkBattleReplay>();
		}
		else
		{
			this.m_ReplayList.Clear();
		}
		MemoryStream memoryStream = this.UnZipToStream(bytes);
		if (memoryStream == null)
		{
			return false;
		}
		memoryStream.Position = 0L;
		using (BinaryReader binaryReader = new BinaryReader(memoryStream))
		{
			while (binaryReader.PeekChar() != -1)
			{
				NkBattleReplay nkBattleReplay = new NkBattleReplay();
				nkBattleReplay.fTime = binaryReader.ReadSingle();
				nkBattleReplay.nType = binaryReader.ReadInt32();
				nkBattleReplay.nSize = binaryReader.ReadInt32();
				nkBattleReplay.pData = binaryReader.ReadBytes(nkBattleReplay.nSize);
				this.m_ReplayList.Add(nkBattleReplay);
			}
		}
		this.bGS_BATTLE_INFO_NFY = false;
		this.bGS_BF_TURNINFO_NFY = false;
		this.m_szLoadFile = string.Empty;
		return true;
	}

	public void ReplayStart()
	{
		Battle.Replay = true;
		Battle.PlayAddRate = 0f;
		this.m_bReplay = true;
		this.m_fReplayStartTime = Time.time;
		if (this.m_szLoadFile != string.Empty)
		{
			if (this.m_ReplayList == null)
			{
				this.LoadReplayData();
			}
			else
			{
				this.m_ReplayList.Clear();
				this.LoadReplayData();
			}
		}
		if (this.m_ReplayList.Count <= 0)
		{
			return;
		}
		byte[] pData = this.m_ReplayList[0].pData;
		if (this.m_ReplayList[0].pData.Length == BaseNet_Game.GetInstance().ReplayReceiveBuffer(pData, 0, pData.Length))
		{
			this.m_ReplayList.RemoveAt(0);
		}
	}

	public void ReplayUpdate()
	{
		if (this.m_ReplayList.Count <= 0)
		{
			return;
		}
		if (this.m_ReplayList[0].nType == 204 && !this.bGS_BATTLE_INFO_NFY)
		{
			return;
		}
		if (this.m_ReplayList[0].nType == 211 && !this.bGS_BF_TURNINFO_NFY)
		{
			this.m_fReplayStartTime = 0f;
			return;
		}
		float fTime = this.m_ReplayList[0].fTime;
		if (Time.time - this.m_fReplayStartTime > fTime)
		{
			byte[] pData = this.m_ReplayList[0].pData;
			if (this.m_ReplayList[0].pData.Length == BaseNet_Game.GetInstance().ReplayReceiveBuffer(pData, 0, pData.Length))
			{
				if (this.m_ReplayList[0].nType == 211 && this.m_fReplayStartTime == 0f)
				{
					this.m_fReplayStartTime = Time.time;
				}
				this.m_ReplayList.RemoveAt(0);
				if (this.m_ReplayList.Count == 0)
				{
					this.m_bReplay = false;
					GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
					gS_CHAR_STATE_SET_REQ.nSet = 4;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
				}
			}
		}
	}

	public void SavePacket(byte[] btBuffer, int nType, int index, int nSize)
	{
		if (this.m_SaveList == null)
		{
			this.m_SaveList = new List<NkBattleReplay>();
			this.m_fLogStartTime = 0f;
		}
		if (nType == 211 && this.m_fLogStartTime == 0f)
		{
			this.m_fLogStartTime = Time.time;
		}
		NkBattleReplay nkBattleReplay = new NkBattleReplay();
		if (this.m_fLogStartTime == 0f)
		{
			nkBattleReplay.fTime = 0f;
		}
		else
		{
			nkBattleReplay.fTime = Time.time - this.m_fLogStartTime;
		}
		nkBattleReplay.nType = nType;
		nkBattleReplay.nSize = nSize;
		nkBattleReplay.pData = new byte[nSize];
		Array.Copy(btBuffer, index, nkBattleReplay.pData, 0, nSize);
		this.m_SaveList.Add(nkBattleReplay);
	}

	public void Savefile()
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor && !System.IO.Directory.Exists(this.m_szDirectory))
		{
			System.IO.Directory.CreateDirectory(this.m_szDirectory);
			NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1230"), this.m_szDirectory);
		}
		string text = string.Format("{0}/{1}_{2}_{3}{4}{5}.dat", new object[]
		{
			this.m_szDirectory,
			DateTime.Now.Month.ToString(),
			DateTime.Now.Day.ToString(),
			DateTime.Now.Hour.ToString("00"),
			DateTime.Now.Minute.ToString("00"),
			DateTime.Now.Second.ToString("00")
		});
		NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1230"), text);
		Stream output;
		if (!File.Exists(text))
		{
			output = File.Open(text, FileMode.OpenOrCreate);
			this.m_fLogStartTime = Time.time;
		}
		else
		{
			output = File.Open(text, FileMode.OpenOrCreate);
		}
		using (BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8))
		{
			for (int i = 0; i < this.m_SaveList.Count; i++)
			{
				binaryWriter.Write(this.m_SaveList[i].fTime);
				binaryWriter.Write(this.m_SaveList[i].nType);
				binaryWriter.Write(this.m_SaveList[i].nSize);
				binaryWriter.Write(this.m_SaveList[i].pData);
			}
		}
		this.m_SaveList.Clear();
		this.m_SaveList = null;
	}

	public MemoryStream UnZipToStream(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		MemoryStream result;
		try
		{
			MemoryStream memoryStream2 = new MemoryStream();
			using (ZipInputStream zipInputStream = new ZipInputStream(memoryStream))
			{
				byte[] buffer = new byte[2048];
				while (zipInputStream.GetNextEntry() != null)
				{
					while (true)
					{
						int num = zipInputStream.Read(buffer, 0, 2048);
						if (num <= 0)
						{
							break;
						}
						memoryStream2.Write(buffer, 0, num);
					}
				}
			}
			memoryStream.Close();
			TsLog.Log("Unzip Size = {0}", new object[]
			{
				memoryStream2.Length
			});
			result = memoryStream2;
		}
		catch (Exception arg)
		{
			TsLog.LogError(string.Format("UnZipFiles failed: {0}", arg), new object[0]);
			result = null;
		}
		return result;
	}

	private void OnDownLoadReplayDataZip(IDownloadedItem wItem, object obj)
	{
		this.m_bRequestWebReplay = false;
		if (!wItem.isSuccess)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("134"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ = new GS_CHAR_STATE_SET_REQ();
			gS_CHAR_STATE_SET_REQ.nSet = 4;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ);
			return;
		}
		if (wItem.safeBytes != null && this.LoadFromBinary(wItem.safeBytes))
		{
			this.ReplayStart();
			GS_CHAR_STATE_SET_REQ gS_CHAR_STATE_SET_REQ2 = new GS_CHAR_STATE_SET_REQ();
			gS_CHAR_STATE_SET_REQ2.nSet = 5;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_STATE_SET_REQ, gS_CHAR_STATE_SET_REQ2);
		}
	}
}
