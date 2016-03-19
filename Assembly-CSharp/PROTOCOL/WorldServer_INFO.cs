using System;

namespace PROTOCOL
{
	public class WorldServer_INFO
	{
		public int i32SVID;

		public int i32CHID;

		public char[] szName = new char[51];

		public char[] szIP = new char[16];

		public int i32_Port;

		public byte byIsUnionServer;

		public char[] szWorldType = new char[2];
	}
}
