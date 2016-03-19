using Global;
using PROTOCOL.BASE;
using PROTOCOL.LOGIN.ID;
using System;
using System.Reflection;
using UnityEngine;

namespace PROTOCOL
{
	internal class BaseNet_Login : NrNetBase
	{
		private static BaseNet_Login instance;

		public static BaseNet_Login GetInstance()
		{
			if (BaseNet_Login.instance == null)
			{
				BaseNet_Login.instance = new BaseNet_Login();
			}
			return BaseNet_Login.instance;
		}

		public bool ConnectLoginServer()
		{
			int loginServerPort = Client.GetLoginServerPort();
			return base.ConnectServer(this.m_szServerIp, loginServerPort);
		}

		protected override int ReceiveBuffer(byte[] btBuffer, int index, int totalSize)
		{
			int num = 0;
			PacketHeader packetHeader;
			int num2;
			int num3;
			while (true)
			{
				packetHeader = (PacketHeader)ReceivePakcet.DeserializePacket(btBuffer, index, out num, typeof(PacketHeader));
				if (index + (int)packetHeader.size + num > totalSize)
				{
					break;
				}
				index += num;
				num2 = BaseNet_Login.Receive_GamePacket(btBuffer, index, packetHeader);
				if ((int)packetHeader.size != num2)
				{
					goto Block_2;
				}
				num3 = totalSize - (num2 + index);
				if (num2 + index >= totalSize)
				{
					goto Block_3;
				}
				index += num2;
			}
			return index;
			Block_2:
			Debug.LogWarning(string.Concat(new object[]
			{
				"Err :Receive_GamePacket Not Match  : ",
				packetHeader.type,
				"Detail-SizeInfo:",
				packetHeader.size,
				" != ",
				num2
			}));
			num2 = (int)packetHeader.size;
			return index;
			Block_3:
			if (num3 != 0)
			{
				Debug.Log("Packet End Remain:" + num3);
			}
			index += num2;
			return index;
		}

		public static int Receive_GamePacket(byte[] btBuffer, int index, PacketHeader header)
		{
			int result = 0;
			if (Enum.IsDefined(typeof(eLOGIN_PACKET_ID), header.type))
			{
				string text = "Recv_" + Enum.GetName(typeof(eLOGIN_PACKET_ID), header.type);
				MethodInfo method = typeof(NrReceiveLogin).GetMethod(text, new Type[]
				{
					typeof(NkDeserializePacket)
				});
				if (method != null)
				{
					NkDeserializePacket nkDeserializePacket = new NkDeserializePacket(btBuffer, index);
					object[] parameters = new object[]
					{
						nkDeserializePacket
					};
					method.Invoke(null, parameters);
					result = nkDeserializePacket.TotalReadByte;
					Debug.LogWarning(string.Concat(new object[]
					{
						"Pack Name : ",
						method.Name,
						" / Size : ",
						nkDeserializePacket.TotalReadByte
					}));
				}
				else
				{
					Debug.LogWarning("DONT FIND METHOD :(T)" + text + "(F)" + text);
				}
			}
			return result;
		}
	}
}
