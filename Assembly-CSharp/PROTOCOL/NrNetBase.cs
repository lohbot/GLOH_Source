using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace PROTOCOL
{
	internal class NrNetBase
	{
		private Socket m_Socket;

		private ReceiveBuffer m_Buffer;

		private IPAddress m_kServeripaddress;

		private string m_IPAddress_Relogin;

		private int m_Port_Relogin;

		protected string m_szServerIp = "20.0.1.18";

		protected int m_nServerPort = 4000;

		private List<byte> m_ReceiveBuffer;

		private int m_iBufferSize = 204800;

		private bool m_bReadAble;

		public long m_nTotalReceivePacketSize;

		private long m_nPacketSizeLog;

		public Socket GetSocket()
		{
			return this.m_Socket;
		}

		public bool IsSocketConnected()
		{
			return this.m_Socket != null && this.m_Socket.Connected;
		}

		public void SetServerIPandPort(string ip, int port)
		{
			this.m_szServerIp = ip;
			this.m_nServerPort = port;
		}

		public bool ConnectServer(string strIP, int strPort)
		{
			if (this.m_ReceiveBuffer == null)
			{
				this.m_ReceiveBuffer = new List<byte>();
			}
			strIP.Trim();
			if (this.m_Socket != null)
			{
				this.Quit();
			}
			this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, this.m_iBufferSize);
			IPAddress.TryParse(strIP, out this.m_kServeripaddress);
			Debug.Log("===========> m_kServeripaddress : " + this.m_kServeripaddress.ToString());
			Debug.Log("===========> strPort : " + strPort.ToString());
			try
			{
				this.m_Socket.Connect(this.m_kServeripaddress, strPort);
			}
			catch (Exception ex)
			{
				TsLog.Log(ex.Message, new object[0]);
				return false;
			}
			this.m_Buffer = new ReceiveBuffer(this.m_Socket.ReceiveBufferSize);
			NrTSingleton<NrUpdateProcessor>.Instance.AddUpdate(new UpdateFunc(this.NetUpdate));
			SendPacket.GetInstance().SetSocket(this.m_Socket);
			this.m_IPAddress_Relogin = strIP;
			this.m_Port_Relogin = strPort;
			return this.m_Socket.Connected;
		}

		public void NetUpdate()
		{
			if (this.m_Socket != null && this.m_Socket.Connected)
			{
				int time_us = 0;
				this.m_bReadAble = this.m_Socket.Poll(time_us, SelectMode.SelectRead);
				if (this.m_bReadAble)
				{
					if (this.IsDisconnected())
					{
						this.OnDisconnected();
					}
					else
					{
						this.UpdateBuffer();
					}
				}
			}
		}

		public bool IsDisconnected()
		{
			return this.m_Socket == null || (this.m_bReadAble && this.m_Socket.Available == 0);
		}

		private bool UpdateBuffer()
		{
			NrTSingleton<NrMainSystem>.Instance.CheckSpentTime(eMSTime.MS_None);
			bool flag = this.m_Buffer.Receive(this.m_Socket, SocketFlags.None);
			if (flag)
			{
				int num = this.ReceiveBuffer(this.m_Buffer.Buffer, this.m_Buffer.Index, this.m_Buffer.Index + this.m_Buffer.TotalLen);
				this.m_Buffer.ReceivedComplete(num);
				this.m_nTotalReceivePacketSize += (long)num;
				if (this.m_nTotalReceivePacketSize - this.m_nPacketSizeLog >= 1048576L)
				{
					TsPlatform.FileLog(string.Concat(new object[]
					{
						"RECEIVE ===> Total ReceivePacket Size : ",
						this.m_nTotalReceivePacketSize.ToString(),
						" (startup : ",
						Time.realtimeSinceStartup,
						" sec)"
					}));
					this.m_nPacketSizeLog = this.m_nTotalReceivePacketSize;
				}
			}
			NrTSingleton<NrMainSystem>.Instance.CheckSpentTime(eMSTime.MS_NetBase);
			return flag;
		}

		public void UpdateBuffer_Old()
		{
			if (this.m_Socket.Connected && this.m_Socket.Available != 0)
			{
				byte[] array = new byte[this.m_Socket.Available];
				Array.Clear(array, 0, array.Length);
				int num = 0;
				try
				{
					num = this.m_Socket.Receive(array, this.m_Socket.Available, SocketFlags.None);
				}
				catch (SocketException ex)
				{
					Debug.Log("GAMEerr:" + ex.Message);
				}
				if (num > 0)
				{
					this.m_ReceiveBuffer.AddRange(array);
				}
				while (this.m_ReceiveBuffer.Count != 0)
				{
					byte[] array2 = this.m_ReceiveBuffer.ToArray();
					int count = this.ReceiveBuffer(array2, 0, array2.Length);
					this.m_ReceiveBuffer.RemoveRange(0, count);
					if (this.m_ReceiveBuffer.Count > 0)
					{
						Debug.Log("After Process Packet : receiveBuffer Size(" + this.m_ReceiveBuffer.Count + ")");
						break;
					}
				}
			}
		}

		protected virtual void OnDisconnected()
		{
		}

		private void _OnMessageBoxOK_QuitGame(object a_oObject)
		{
			NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
		}

		private void _OnMessageBoxOK_Relogin(object a_oObject)
		{
			NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
		}

		protected virtual int ReceiveBuffer(byte[] btBuffer, int index, int totalSize)
		{
			return -1;
		}

		public void Quit()
		{
			if (this.m_Socket != null)
			{
				try
				{
					if (this.m_Socket.IsBound && this.m_Socket.Connected)
					{
						this.m_Socket.Shutdown(SocketShutdown.Both);
					}
				}
				catch (SocketException ex)
				{
					Debug.LogError(ex.ToString());
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("169"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				this.m_Socket = null;
				NrTSingleton<NrUpdateProcessor>.Instance.DellUpdate(new UpdateFunc(this.NetUpdate));
				Debug.Log("SocketShutdown : " + Time.time);
			}
		}

		public bool ReConnectServer()
		{
			return this.ConnectServer(this.m_IPAddress_Relogin, this.m_Port_Relogin);
		}
	}
}
