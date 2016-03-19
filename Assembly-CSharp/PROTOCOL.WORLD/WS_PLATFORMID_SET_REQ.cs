using System;

namespace PROTOCOL.WORLD
{
	public class WS_PLATFORMID_SET_REQ
	{
		public char[] m_szPlatformID = new char[256];

		public int m_nPlatformType;
	}
}
