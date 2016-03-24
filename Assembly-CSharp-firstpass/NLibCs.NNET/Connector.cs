using System;

namespace NLibCs.NNET
{
	public class Connector : Networker
	{
		public bool Connect(ConnectorInfo info)
		{
			return false;
		}

		private bool Reconnect()
		{
			return false;
		}

		private void Disconnect()
		{
		}

		private void SetOnAutoConnect(bool bOn, int nPriodMS = 0)
		{
		}

		public virtual bool SendPacket(byte patcket)
		{
			return base.__send_packet(this.m_session);
		}
	}
}
