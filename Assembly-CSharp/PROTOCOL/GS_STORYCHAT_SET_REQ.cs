using System;

namespace PROTOCOL
{
	public class GS_STORYCHAT_SET_REQ
	{
		public int m_nType = 1;

		public long m_nStoryChatID;

		public char[] szMessage = new char[201];
	}
}
