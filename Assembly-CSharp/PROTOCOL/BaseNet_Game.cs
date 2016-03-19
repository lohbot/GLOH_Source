using PROTOCOL.BASE;
using PROTOCOL.GAME.ID;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace PROTOCOL
{
	internal class BaseNet_Game : NrNetBase
	{
		private static BaseNet_Game instance;

		private TcpClient Client;

		public bool m_bSavePacket;

		private int mPrevPackType;

		public static BaseNet_Game GetInstance()
		{
			if (BaseNet_Game.instance == null)
			{
				BaseNet_Game.instance = new BaseNet_Game();
			}
			return BaseNet_Game.instance;
		}

		public bool ConnectGameServer(string strIP, int strPort)
		{
			base.SetServerIPandPort(strIP, strPort);
			return base.ConnectServer(this.m_szServerIp, this.m_nServerPort);
		}

		public int ReplayReceiveBuffer(byte[] btBuffer, int index, int totalSize)
		{
			return this.ReceiveBuffer(btBuffer, index, totalSize);
		}

		protected override int ReceiveBuffer(byte[] btBuffer, int index, int totalSize)
		{
			int num = 0;
			int num2 = 0;
			int num3;
			int num4;
			while (true)
			{
				num2++;
				PacketHeader packetHeader = (PacketHeader)ReceivePakcet.DeserializePacket(btBuffer, index, out num, typeof(PacketHeader));
				if (index + (int)packetHeader.size + num > totalSize)
				{
					break;
				}
				int index2 = index;
				index += num;
				num3 = NrReceiveGame.Receive_GamePacket(btBuffer, index, packetHeader);
				if (this.m_bSavePacket && packetHeader.type >= 200 && packetHeader.type <= 254)
				{
					NrTSingleton<NkBattleReplayManager>.Instance.SavePacket(btBuffer, packetHeader.type, index2, num3 + num);
				}
				if (packetHeader.type == 203 && this.m_bSavePacket)
				{
					this.m_bSavePacket = false;
					NrTSingleton<NkBattleReplayManager>.Instance.Savefile();
				}
				if ((int)packetHeader.size != num3)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Err: PrevePackType: Watch Out Protocol ID:",
						((eGAME_PACKET_ID)this.mPrevPackType).ToString(),
						":Total:",
						totalSize
					}));
					Debug.LogError(string.Concat(new object[]
					{
						"Err :Receive_GamePacket Not Match=(Type:",
						((eGAME_PACKET_ID)packetHeader.type).ToString(),
						",Size:",
						packetHeader.size,
						" != ",
						num3,
						")I:",
						index
					}));
					num3 = (int)packetHeader.size;
					if (0 > packetHeader.size)
					{
						goto Block_8;
					}
				}
				this.mPrevPackType = packetHeader.type;
				num4 = totalSize - (num3 + index);
				if (num3 + index >= totalSize)
				{
					goto Block_9;
				}
				index += num3;
			}
			return index;
			Block_8:
			Debug.LogWarning("Packet:ErrorSkip");
			return index;
			Block_9:
			if (num4 != 0)
			{
				Debug.Log("Packet End Remain:" + num4);
			}
			index += num3;
			return index;
		}

		public long GetTotalReceivePacketSize()
		{
			return this.m_nTotalReceivePacketSize;
		}
	}
}
