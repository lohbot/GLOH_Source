using GameMessage.Private;
using PROTOCOL;
using PROTOCOL.WORLD;
using System;

public class CMovingServer : NrTSingleton<CMovingServer>
{
	public long m_UID;

	public int tickCount;

	public bool m_IsMovingWorld;

	public bool m_IsMovingChannel;

	private bool m_bReqMovingCharInit;

	public long m_nCHMoveTargetPersonID;

	public byte m_nCHMoveType;

	public byte m_i8AgitMove;

	public long m_i64MovingWorld_KEY;

	public bool m_bIsWaitingEncryptKey;

	public long m_CID;

	public string m_szMoveFrontServerIP;

	public short m_i16MoveFrontServerPort;

	public bool ReqMovingCharInit
	{
		get
		{
			return this.m_bReqMovingCharInit;
		}
		set
		{
			this.m_bReqMovingCharInit = value;
		}
	}

	private CMovingServer()
	{
	}

	public bool Initialize()
	{
		this.m_UID = 0L;
		this.m_CID = 0L;
		this.m_IsMovingWorld = false;
		this.m_IsMovingChannel = false;
		this.m_nCHMoveTargetPersonID = 0L;
		this.m_nCHMoveType = 0;
		this.m_i64MovingWorld_KEY = 0L;
		this.m_bIsWaitingEncryptKey = false;
		this.tickCount = 0;
		this.m_szMoveFrontServerIP = string.Empty;
		return true;
	}

	public void SetMovingWorldInfo(long UID, long i64MovingWorld_KEY, string MoveFrontServerIP, short i16Port, long nCHMoveTargetPersonID, byte nCHMoveType, byte i8AgitMove)
	{
		this.m_UID = UID;
		this.m_i64MovingWorld_KEY = i64MovingWorld_KEY;
		this.m_IsMovingWorld = true;
		this.m_IsMovingChannel = false;
		this.tickCount = Environment.TickCount;
		this.m_szMoveFrontServerIP = MoveFrontServerIP;
		this.m_i16MoveFrontServerPort = i16Port;
		this.m_nCHMoveTargetPersonID = nCHMoveTargetPersonID;
		this.m_nCHMoveType = nCHMoveType;
		this.m_i8AgitMove = i8AgitMove;
		SendPacket.GetInstance().SetBlockSendPacket(true);
	}

	public void SetMovingChannelInfo(long UID, long CID)
	{
		this.m_UID = UID;
		this.m_CID = CID;
		this.m_IsMovingWorld = false;
		this.m_IsMovingChannel = true;
		this.tickCount = Environment.TickCount;
	}

	public bool IsMovingWorld()
	{
		return this.m_IsMovingWorld;
	}

	public bool IsMovingChannel()
	{
		return this.m_IsMovingChannel;
	}

	public bool IsWaitintEncryptKey()
	{
		return this.m_bIsWaitingEncryptKey;
	}

	public bool Update_MovingWorld()
	{
		if (this.IsWaitintEncryptKey())
		{
			return false;
		}
		if (this.tickCount + 5000 > Environment.TickCount)
		{
			return false;
		}
		if (BaseNet_Game.GetInstance().ConnectGameServer(this.m_szMoveFrontServerIP, (int)this.m_i16MoveFrontServerPort))
		{
			this.m_bIsWaitingEncryptKey = true;
		}
		return true;
	}

	public bool Update_MovingChannel()
	{
		if (this.tickCount + 5000 > Environment.TickCount)
		{
			return false;
		}
		FacadeHandler.Req_GS_AUTH_SESSION_REQ(this.m_UID, 0, this.m_CID, 1, 404);
		TsLog.Log(string.Concat(new object[]
		{
			"Update_MovingChannel() [",
			this.m_UID,
			" / ",
			this.m_CID,
			"]"
		}), new object[0]);
		return true;
	}

	public void Update()
	{
		if (this.m_IsMovingWorld)
		{
			if (this.Update_MovingWorld())
			{
			}
		}
		else if (this.m_IsMovingChannel && this.Update_MovingChannel())
		{
			this.Initialize();
		}
	}

	public void OnReceivedEncryptKey()
	{
		SendPacket.GetInstance().SetBlockSendPacket(false);
		WS_USER_LOGIN_MOVING_WORLD_REQ wS_USER_LOGIN_MOVING_WORLD_REQ = new WS_USER_LOGIN_MOVING_WORLD_REQ();
		wS_USER_LOGIN_MOVING_WORLD_REQ.i64MovingWorld_KEY = this.m_i64MovingWorld_KEY;
		wS_USER_LOGIN_MOVING_WORLD_REQ.UID = this.m_UID;
		wS_USER_LOGIN_MOVING_WORLD_REQ.m_nCHMoveTargetPersonID = this.m_nCHMoveTargetPersonID;
		wS_USER_LOGIN_MOVING_WORLD_REQ.m_nCHMoveType = this.m_nCHMoveType;
		wS_USER_LOGIN_MOVING_WORLD_REQ.i8AgitMove = this.m_i8AgitMove;
		SendPacket.GetInstance().SendObject(16777235, wS_USER_LOGIN_MOVING_WORLD_REQ);
		this.Initialize();
	}
}
