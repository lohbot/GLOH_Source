using NLibCs.ErrorCollector.PROTOCOL;
using NLibCs.Net.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace NLibCs.ErrorCollector
{
	internal class Receiver
	{
		private enum RecvState
		{
			Recv_Header,
			Recv_Body
		}

		private delegate void Func(byte[] packet);

		private Connector _parent;

		private Dictionary<COMMAND, Receiver.Func> packetMappingDic = new Dictionary<COMMAND, Receiver.Func>();

		private Receiver.RecvState _eRecvState;

		private HEADER _recvheader = default(HEADER);

		public Receiver(Connector parent)
		{
			this._parent = parent;
		}

		private void BIND(COMMAND command, Receiver.Func method)
		{
			this.packetMappingDic.Add(command, method);
		}

		public void Recv()
		{
			try
			{
				if (this._parent.Poll(SelectMode.SelectRead, 10))
				{
					if (this._eRecvState == Receiver.RecvState.Recv_Header && this._parent.Sock.Available >= 5)
					{
						int num = this.RecvHeader(ref this._recvheader);
						this._eRecvState = Receiver.RecvState.Recv_Body;
					}
					if (this._eRecvState == Receiver.RecvState.Recv_Body)
					{
						if (this._recvheader.Size == 0)
						{
							this._parent.Handle.Occur_PacketError(PACKETERRORTYPE.RECVERR);
						}
						else
						{
							int num2 = (int)(this._recvheader.Size - 5);
							if (this._parent.Sock.Available >= num2)
							{
								byte[] array = new byte[num2];
								int num3 = this._parent.Sock.Receive(array);
								this.OnDispatch((COMMAND)this._recvheader.Command, array);
								this._eRecvState = Receiver.RecvState.Recv_Header;
								this._parent.StopPacketRecvCheck();
								this._parent.StartTimeoutCheck();
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private int RecvHeader(ref HEADER header)
		{
			try
			{
				byte[] buffer = new byte[5];
				int result = this._parent.Sock.Receive(buffer);
				RawSerializer rawSerializer = new RawSerializer(header.GetType(), false);
				MemoryStream source = new MemoryStream(buffer);
				rawSerializer.Deserialize<HEADER>(ref header, source);
				return result;
			}
			catch (Exception)
			{
			}
			return 0;
		}

		private object DeserializePacket(byte[] buffer, object packet)
		{
			RawSerializer rawSerializer = new RawSerializer(packet.GetType(), false);
			MemoryStream source = new MemoryStream(buffer);
			rawSerializer.Deserialize(packet, source);
			return packet;
		}

		private int OnDispatch(COMMAND command, byte[] packet)
		{
			Receiver.Func func = null;
			if (this.packetMappingDic.TryGetValue(command, out func))
			{
				this._parent.WriteLog(Connector.LOGTYPE.RS, string.Format("{0}", command));
				func(packet);
				return 0;
			}
			return -1;
		}
	}
}
