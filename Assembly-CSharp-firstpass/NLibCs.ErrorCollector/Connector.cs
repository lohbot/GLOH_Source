using NLibCs.ErrorCollector.PROTOCOL;
using NNetCs;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NLibCs.ErrorCollector
{
	public class Connector
	{
		public enum LOGTYPE
		{
			SS,
			SF,
			RS,
			RF,
			INFO
		}

		private Thread _recvThread;

		private bool _recvRun;

		private Thread _sendThread;

		private bool _sendRun;

		private Thread _coreThread;

		private bool _coreRun;

		private string _serverName = string.Empty;

		private string _serverType = string.Empty;

		private IPAddress _connectIP;

		private int _connectPort;

		private Socket _sock;

		private Sender _sender;

		private Receiver _receiver;

		private ErrorCollectorHandler _handler;

		private Stopwatch _stopwatch_connect = new Stopwatch();

		private Stopwatch _stopwatch_PacketRecv = new Stopwatch();

		private Stopwatch _stopwatch_Timeout = new Stopwatch();

		private object timeout_Lock = new object();

		private bool _useTimeout = true;

		private string _logPath = string.Empty;

		private int _notifySec;

		private bool isInitialized;

		private bool _useRecvThread = true;

		private bool _autoReconnect = true;

		private static ParallelServer m_parallelServer = new ParallelServer();

		private static Connector _connector;

		public Socket Sock
		{
			get
			{
				return this._sock;
			}
		}

		public bool Established
		{
			get
			{
				return this._sock != null && this._sock.Connected;
			}
		}

		public string ExecArgs
		{
			get;
			set;
		}

		public ErrorCollectorHandler Handle
		{
			get
			{
				return this._handler;
			}
		}

		public string LogPath
		{
			get
			{
				return this._logPath;
			}
		}

		public static Connector Instance
		{
			get
			{
				return Connector._connector;
			}
		}

		private Connector(string serverName, string serverType, string connectIP, int connectPort, ErrorCollectorHandler handler, bool useRecvThread, bool useTimeout)
		{
			this._recvRun = false;
			this._sendRun = false;
			this._serverName = serverName;
			this._serverType = serverType;
			this._connectIP = IPAddress.Parse(connectIP);
			this._connectPort = connectPort;
			this._handler = handler;
			this._sender = new Sender(this, serverName, serverType, this._handler);
			this._receiver = new Receiver(this);
			this._useRecvThread = useRecvThread;
			this._useTimeout = useTimeout;
		}

		public void Send_Error(long UserSN, int platformType, int authType, string bundleVersion, string message, string stackTrace, string deviceModel, string deviceOS)
		{
			this._sender.send_CE_Send_Error_NFY(UserSN, platformType, authType, bundleVersion, message, stackTrace, deviceModel, deviceOS);
		}

		public void Send_Off()
		{
			this._sender.send_CE_OFF_NFY();
		}

		public void SockClose()
		{
			if (this._sock != null)
			{
				this._sock.Close();
				this._sock = null;
			}
		}

		public void StartPacketRecvCheck()
		{
			this._stopwatch_PacketRecv.Reset();
			this._stopwatch_PacketRecv.Start();
		}

		public void StopPacketRecvCheck()
		{
			this._stopwatch_PacketRecv.Stop();
		}

		public void StartTimeoutCheck()
		{
			object obj = this.timeout_Lock;
			lock (obj)
			{
				while (!Monitor.TryEnter(this.timeout_Lock, 10))
				{
				}
				Monitor.Exit(this.timeout_Lock);
				this._stopwatch_Timeout.Stop();
				this._stopwatch_Timeout.Reset();
				this._stopwatch_Timeout.Start();
			}
		}

		public bool IsTimeout()
		{
			if (!this._useTimeout)
			{
				return false;
			}
			if (this._stopwatch_Timeout.ElapsedMilliseconds > 40000L)
			{
				this._stopwatch_Timeout.Stop();
				this._stopwatch_Timeout.Reset();
				return true;
			}
			return false;
		}

		public bool NeedHeartbeat()
		{
			return !this._useTimeout && this._stopwatch_Timeout.ElapsedMilliseconds / 5000L >= 1L;
		}

		private bool Open()
		{
			if (this._sock == null)
			{
				this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this._sock.ReceiveBufferSize = 32000;
			}
			this._stopwatch_connect.Stop();
			this._stopwatch_connect.Reset();
			if (this._useRecvThread)
			{
				if (this._recvThread == null)
				{
					this._recvThread = new Thread(new ThreadStart(this.RecvUpdateThread));
					this._recvThread.Name = this._serverType + "_connector_Recv";
					this._recvThread.Start();
				}
				if (this._sendThread == null)
				{
					this._sendThread = new Thread(new ThreadStart(this.SendUpdateThread));
					this._sendThread.Name = this._serverType + "_connector_Send";
					this._sendThread.Start();
				}
			}
			if (this._coreThread == null)
			{
				this._coreThread = new Thread(new ThreadStart(this.CoreUpdateThread));
				this._coreThread.Name = this._serverType + "_connector_Core";
				this._coreThread.Start();
			}
			else
			{
				this.Connect();
			}
			return true;
		}

		private void Close()
		{
			this._stopwatch_PacketRecv.Stop();
			this._stopwatch_PacketRecv.Reset();
			this._coreRun = false;
			if (this._coreThread != null)
			{
				this._coreThread.Join();
				this._coreThread = null;
			}
			this._recvRun = false;
			if (this._recvThread != null)
			{
				this._recvThread.Join();
				this._recvThread = null;
			}
			this._sendRun = false;
			if (this._sendThread != null)
			{
				this._sendThread.Join();
				this._sendThread = null;
			}
			if (this._sock != null)
			{
				this._sock.Close();
				this._sock = null;
			}
		}

		private void Connect()
		{
			try
			{
				if (this._sock == null)
				{
					this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					this._sock.ReceiveBufferSize = 32000;
				}
				else if (this._sock != null && this._sock.LocalEndPoint != null)
				{
					this._sock.Close();
					this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				}
				this._stopwatch_connect.Stop();
				this._stopwatch_connect.Reset();
				this._stopwatch_connect.Start();
				this._sock.Connect(this._connectIP, this._connectPort);
				this.isInitialized = true;
				this.StartTimeoutCheck();
			}
			catch (SocketException ex)
			{
				Console.WriteLine(" [{0}] >> {1}", this._serverName, ex.Message);
			}
		}

		private void CoreUpdateThread()
		{
			this._coreRun = true;
			while (this._coreRun)
			{
				this.CoreUpdate();
				Thread.Sleep(10);
			}
		}

		public void CoreUpdate()
		{
			if (this.IsTimeout())
			{
				this.SockClose();
				this.WriteLog(Connector.LOGTYPE.INFO, "TO");
			}
			long elapsedMilliseconds = this._stopwatch_connect.ElapsedMilliseconds;
			bool flag = elapsedMilliseconds == 0L || elapsedMilliseconds >= 5000L;
			if (!this.Established && flag && this._autoReconnect)
			{
				this.Connect();
			}
			else if (elapsedMilliseconds == 0L && !this._autoReconnect)
			{
				this.Connect();
			}
		}

		private void RecvUpdateThread()
		{
			this._recvRun = true;
			while (this._recvRun)
			{
				this.RecvUpdate();
				Thread.Sleep(10);
			}
		}

		public void RecvUpdate()
		{
			this._receiver.Recv();
		}

		private void SendUpdateThread()
		{
			this._sendRun = true;
			while (this._sendRun)
			{
				this.SendUpdate();
				Thread.Sleep(10);
			}
		}

		public void SendUpdate()
		{
			this._sender.Send();
		}

		public bool ReConnectToAnotherServer()
		{
			if (Connector._connector == null)
			{
				return false;
			}
			if (Connector.m_parallelServer.hasNext())
			{
				this.WriteLog(Connector.LOGTYPE.INFO, string.Format("Reconnect to {0}", Connector.m_parallelServer.GetIP().Substring(Connector.m_parallelServer.GetIP().LastIndexOf("."))));
				this._connectIP = IPAddress.Parse(Connector.m_parallelServer.GetIP());
				this._connectPort = Connector.m_parallelServer.GetPORT();
				return true;
			}
			this._handler.Occur_PacketError(PACKETERRORTYPE.RECONNECTERR);
			this.WriteLog(Connector.LOGTYPE.INFO, string.Format("All {0} is busy. try again later", this._serverName));
			return false;
		}

		public bool Poll(SelectMode mode, int microSeconds)
		{
			return this._sock != null && this._sock.Connected && this._sock.Poll(microSeconds, mode);
		}

		public bool ReStart()
		{
			if (!this.isInitialized)
			{
				return false;
			}
			if (Connector._connector == null)
			{
				return false;
			}
			if (Connector._connector._sock == null)
			{
				this._stopwatch_connect.Stop();
				this._stopwatch_connect.Reset();
				return true;
			}
			if (!Connector._connector._sock.Connected)
			{
				this._stopwatch_connect.Stop();
				this._stopwatch_connect.Reset();
				return true;
			}
			return true;
		}

		public bool WriteLog(Connector.LOGTYPE type, string log)
		{
			bool result;
			try
			{
				if (!Directory.Exists(Connector._connector.LogPath))
				{
					result = false;
				}
				else
				{
					using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Connector._connector.LogPath, string.Format("{0}log.txt", this._serverType)), true, Encoding.UTF8))
					{
						streamWriter.Write("{0}\t{1}\t{2}\r\n", DateTime.Now, type, log);
						streamWriter.Close();
					}
					result = true;
				}
			}
			catch (Exception var_1_86)
			{
				result = false;
			}
			return result;
		}

		public static bool Start(string serverName, string serverType, ErrorCollectorHandler handler, string local_location, string url_location, bool useUpdateThread = true, bool useAutoReconnect = false, bool useTimeout = true)
		{
			string empty = string.Empty;
			string url = string.Format("{0}{1}info.txt", url_location, serverType.ToLower());
			if (!Connector.m_parallelServer.LoadServerList(url, ref empty))
			{
				handler.Occur_SettingError(empty);
				return false;
			}
			if (Connector.m_parallelServer.ServerListCount == 0)
			{
				handler.Occur_SettingError(serverName + " info not exist");
				return false;
			}
			return Connector.Start(serverName, serverType, Connector.m_parallelServer.GetIP(), Connector.m_parallelServer.GetPORT(), handler, local_location, useUpdateThread, useAutoReconnect, useTimeout);
		}

		public static bool Start(string serverName, string serverType, string connectIP, int connectPort, ErrorCollectorHandler handler, string local_location, bool useUpdateThread = true, bool useAutoReconnect = false, bool useTimeout = true)
		{
			if (Connector._connector == null)
			{
				Connector._connector = new Connector(serverName, serverType.ToLower(), connectIP, connectPort, handler, useUpdateThread, useTimeout);
				Connector._connector._logPath = local_location;
				Connector._connector._autoReconnect = useAutoReconnect;
				return Connector._connector.Open();
			}
			if (Connector._connector._sock == null)
			{
				return Connector._connector.Open();
			}
			if (!Connector._connector._sock.Connected)
			{
				Connector._connector.Connect();
			}
			return true;
		}

		public static bool Stop()
		{
			if (Connector._connector != null)
			{
				Connector._connector.Close();
				return true;
			}
			return false;
		}
	}
}
