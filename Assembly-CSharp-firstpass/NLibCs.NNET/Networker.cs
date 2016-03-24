using System;

namespace NLibCs.NNET
{
	public class Networker
	{
		protected Session m_session;

		protected NetworkerType m_eNetworkerType;

		public string Name
		{
			get;
			set;
		}

		internal Networker()
		{
			this.m_eNetworkerType = NetworkerType.NONE;
		}

		public bool StartNetworker(NetworkInfo info, int nPort)
		{
			if (nPort == 0)
			{
				this.m_eNetworkerType = NetworkerType.CONNECTOR;
			}
			else
			{
				this.m_eNetworkerType = NetworkerType.ACCEPTER;
			}
			return false;
		}

		public void StopNetworker()
		{
		}

		public bool __send_packet(Session session)
		{
			return session != null && session.SendPacket();
		}
	}
}
