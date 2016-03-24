using NLibCs.ErrorCollector.PROTOCOL;
using NLibCs.Net.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace NLibCs.ErrorCollector
{
	internal class Sender
	{
		private class StatsInfo
		{
			private DateTime _last;

			public int inPackets;

			public int outPackets;

			public int dispatched;

			public int processed;

			public double Delta
			{
				get
				{
					return 10000000.0 / (double)(DateTime.Now - this._last).Ticks;
				}
			}

			public StatsInfo()
			{
				this._last = DateTime.Now;
				this.inPackets = 0;
				this.outPackets = 0;
				this.dispatched = 0;
				this.processed = 0;
			}
		}

		public struct PacketQueueInstance
		{
			public COMMAND command;

			public object packet;

			public PacketQueueInstance(COMMAND _command, object _packet)
			{
				this.command = _command;
				this.packet = _packet;
			}
		}

		private Connector _parent;

		private string _serverName;

		private string _serverType;

		private string _serverKey;

		private Sender.StatsInfo _lastStats;

		private ErrorCollectorHandler _handler;

		private object lastSendPacket;

		private COMMAND lastSendcommand;

		private Thread _thread;

		private bool _isRun;

		private bool _isBusy;

		private bool _isFinish = true;

		private Queue packetQueue = Queue.Synchronized(new Queue());

		private int retryCount;

		private readonly int retryCountMax = 5;

		private string errorMessage = string.Empty;

		private List<COMMAND> NotReturnPacketList = new List<COMMAND>();

		private Stopwatch _stopwatch_Restart = new Stopwatch();

		public string ServerKey
		{
			get
			{
				return this._serverKey;
			}
			set
			{
				this._serverKey = value;
			}
		}

		private int UsedMemory
		{
			get
			{
				return (int)(Process.GetCurrentProcess().PeakWorkingSet64 / 1024L);
			}
		}

		public Sender(Connector parent, string serverName, string serverType, ErrorCollectorHandler handler)
		{
			this._parent = parent;
			this._serverName = serverName.ToLower();
			this._serverType = serverType.ToLower();
			this._serverKey = string.Empty;
			this._lastStats = new Sender.StatsInfo();
			this._handler = handler;
			this._stopwatch_Restart.Stop();
			this._stopwatch_Restart.Reset();
			this.SetNotReturnPacketList();
		}

		public void Send()
		{
			if (this._parent.Established)
			{
				this._stopwatch_Restart.Stop();
				this._stopwatch_Restart.Reset();
				this.retryCount = 0;
				if (!this._isBusy)
				{
					if (this.packetQueue.Count > 0)
					{
						this._isBusy = true;
						this._isFinish = false;
						Sender.PacketQueueInstance packetQueueInstance = (Sender.PacketQueueInstance)this.packetQueue.Dequeue();
						this.__send(packetQueueInstance.command, packetQueueInstance.packet);
					}
				}
				else if (this._isBusy && !this._isFinish)
				{
					this.ReSend();
				}
			}
			else if (this.packetQueue.Count > 0 || !this._isFinish)
			{
				if (this._stopwatch_Restart.ElapsedMilliseconds == 0L && this.retryCount < this.retryCountMax)
				{
					this._stopwatch_Restart.Start();
					this.retryCount++;
					Connector.Instance.ReStart();
				}
				else if (this.retryCount == this.retryCountMax)
				{
					this._stopwatch_Restart.Stop();
					this._stopwatch_Restart.Reset();
					this._parent.WriteLog(Connector.LOGTYPE.SF, string.Format("{0}", (ushort)this.lastSendcommand));
					this._parent.WriteLog(Connector.LOGTYPE.SF, this.errorMessage);
					if (this._parent.ReConnectToAnotherServer())
					{
						Connector.Instance.ReStart();
					}
					else
					{
						this.packetQueue.Clear();
						this._isBusy = false;
						this._isFinish = true;
					}
					this.retryCount = 0;
				}
				else if (this._stopwatch_Restart.ElapsedMilliseconds / 1000L >= 1L)
				{
					this._stopwatch_Restart.Stop();
					this._stopwatch_Restart.Reset();
					this._stopwatch_Restart.Start();
					this.retryCount++;
					Connector.Instance.ReStart();
				}
			}
			if (this.packetQueue.Count == 0 && this._parent.NeedHeartbeat())
			{
				Heartbeat_NFY heartbeat_NFY;
				this.packetQueue.Enqueue(new Sender.PacketQueueInstance(COMMAND.Heartbeat_NFY, heartbeat_NFY));
			}
		}

		public void send_CE_Send_Error_NFY(long UserSN, int platformType, int authType, string bundleVersion, string message, string stackTrace, string deviceModel, string deviceOS)
		{
			CE_Send_Error_NFY cE_Send_Error_NFY = default(CE_Send_Error_NFY);
			cE_Send_Error_NFY.userSN = UserSN;
			cE_Send_Error_NFY.platformType = platformType;
			cE_Send_Error_NFY.authType = authType;
			cE_Send_Error_NFY.bundleVerson = bundleVersion;
			cE_Send_Error_NFY.message = message;
			cE_Send_Error_NFY.stackTrace = stackTrace;
			cE_Send_Error_NFY.deviceModel = deviceModel;
			cE_Send_Error_NFY.deviceOS = deviceOS;
			this.packetQueue.Enqueue(new Sender.PacketQueueInstance(COMMAND.CE_Send_Error_NFY, cE_Send_Error_NFY));
		}

		public void send_CE_OFF_NFY()
		{
			CE_OFF_NFY cE_OFF_NFY = default(CE_OFF_NFY);
			this.packetQueue.Enqueue(new Sender.PacketQueueInstance(COMMAND.CE_OFF_NFY, cE_OFF_NFY));
		}

		public void ReSend()
		{
			this.__send(this.lastSendcommand, this.lastSendPacket);
		}

		private void SetNotReturnPacketList()
		{
			this.NotReturnPacketList.Add(COMMAND.CE_OFF_NFY);
			this.NotReturnPacketList.Add(COMMAND.CE_Send_Error_NFY);
		}

		private void __send(COMMAND command, object packet)
		{
			this.lastSendPacket = packet;
			this.lastSendcommand = command;
			byte[] array = new byte[32768];
			if (COMMAND.BEGIN < command && COMMAND.PROTOCOL_END > command)
			{
				byte[] array2 = new byte[5];
				ushort num = this.SerializePacket(array, packet);
				this.SerializeHeader(array2, num, (ushort)command);
				try
				{
					this._parent.Sock.Send(array2);
					if (num > 0)
					{
						this._parent.Sock.Send(array, (int)num, SocketFlags.None);
					}
					this._parent.WriteLog(Connector.LOGTYPE.SS, string.Format("{0}", (ushort)command));
					if (this.NeedPacketRecvCheck(command))
					{
						this._parent.StartPacketRecvCheck();
					}
					else
					{
						this._parent.SockClose();
						this._parent.WriteLog(Connector.LOGTYPE.INFO, "SC");
					}
					this._isBusy = false;
					this._isFinish = true;
					this.retryCount = 0;
					this.errorMessage = string.Empty;
					this._parent.StartTimeoutCheck();
				}
				catch (Exception ex)
				{
					this.errorMessage = ex.Message;
				}
				return;
			}
		}

		private bool NeedPacketRecvCheck(COMMAND command)
		{
			return !this.NotReturnPacketList.Contains(command);
		}

		private void SerializeHeader(byte[] buffer, ushort size, ushort command)
		{
			HEADER hEADER = new HEADER(size + 5, command);
			RawSerializer rawSerializer = new RawSerializer(hEADER.GetType(), false);
			MemoryStream target = new MemoryStream(buffer);
			int num;
			rawSerializer.Serialize(hEADER, target, out num);
		}

		private ushort SerializeHeaderEx(byte[] buffer)
		{
			buffer[5] = 94;
			buffer[6] = 124;
			return 2;
		}

		private ushort SerializePacket(byte[] buffer, object packet)
		{
			RawSerializer rawSerializer = new RawSerializer(packet.GetType(), false);
			MemoryStream target = new MemoryStream(buffer);
			int num;
			rawSerializer.Serialize(packet, target, out num);
			return (ushort)num;
		}
	}
}
