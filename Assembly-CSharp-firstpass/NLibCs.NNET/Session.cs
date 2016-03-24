using System;
using System.Net.Sockets;

namespace NLibCs.NNET
{
	public class Session
	{
		protected TcpClient m_client;

		public bool SendPacket()
		{
			return false;
		}
	}
}
