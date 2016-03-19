using PROTOCOL.BASE;
using PROTOCOL.GAME.ID;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PROTOCOL
{
	public class SendPacket
	{
		private static SendPacket instance;

		private ITZEncrypt mEncrypt;

		private Socket m_Socket;

		private SendBuffer mBuffer;

		private bool m_bBlockSendPacket;

		public long m_nTotalSendPacketSize;

		private long m_nPacketSizeLog;

		private SendPacket()
		{
			this.mBuffer = new SendBuffer(SendBuffer.SEND_BUFFER_SIZE);
			this.mEncrypt = new ITZEncrypt();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static SendPacket GetInstance()
		{
			if (SendPacket.instance == null)
			{
				SendPacket.instance = new SendPacket();
			}
			return SendPacket.instance;
		}

		public void SetEncryptKey(byte uckey1, byte uckey2, bool bKeyFixed)
		{
			this.mEncrypt.SetKey(uckey1, uckey2, bKeyFixed);
		}

		public void SetSocket(Socket _Socket)
		{
			this.m_Socket = _Socket;
			if (TsPlatform.IsMobile)
			{
				this.m_bBlockSendPacket = false;
			}
		}

		public void SendIDType(int _ID)
		{
			this.SendObject(_ID, null);
		}

		public void SendObject(int _ID)
		{
			this.SendIDType(_ID);
		}

		public void SendObject(eGAME_PACKET_ID _eType, object _Obj)
		{
			if (this.m_bBlockSendPacket)
			{
				return;
			}
			this.SendObject((int)_eType, _Obj);
		}

		public void SendObject(int _idType, object _Obj)
		{
			if (this.m_Socket == null)
			{
				return;
			}
			if (this.m_bBlockSendPacket)
			{
				return;
			}
			if (!this.m_Socket.Connected)
			{
				return;
			}
			try
			{
				this.SendSocket(_idType, _Obj);
			}
			catch (Exception ex)
			{
				TsLog.Log(ex.Message, new object[0]);
			}
		}

		private void SendSocket(int _idType, object _Obj)
		{
			byte[] array = null;
			PacketHeader packetHeader;
			if (_Obj != null)
			{
				array = this.RawSerialize(_Obj);
				packetHeader = new PacketHeader();
				packetHeader.size = (short)array.Length;
				packetHeader.type = _idType;
			}
			else
			{
				packetHeader = new PacketHeader(_idType);
			}
			byte[] array2 = this.RawSerialize(packetHeader);
			if (this.m_Socket != null)
			{
				this.mBuffer.InsertMerge(array2, array);
				this.mBuffer.Send(this.m_Socket, this.mEncrypt);
				this.m_nTotalSendPacketSize += (long)(array2.Length + (int)packetHeader.size);
				if (this.m_nTotalSendPacketSize - this.m_nPacketSizeLog >= 1048576L)
				{
					TsPlatform.FileLog(string.Concat(new object[]
					{
						"SEND ===> Total SendPacket Size : ",
						this.m_nTotalSendPacketSize.ToString(),
						" (startup : ",
						Time.realtimeSinceStartup,
						" sec)"
					}));
					this.m_nPacketSizeLog = this.m_nTotalSendPacketSize;
				}
			}
		}

		private byte[] RawSerialize(object anything)
		{
			return TKMarshal.Serialize(anything);
		}

		public void SetBlockSendPacket(bool bBlockSendPacket)
		{
			this.m_bBlockSendPacket = bBlockSendPacket;
		}

		public bool IsBlockSendPacket()
		{
			return this.m_bBlockSendPacket;
		}

		public long GetTotalSendPacketSize()
		{
			return this.m_nTotalSendPacketSize;
		}
	}
}
