using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CLIENT_RELOGIN_ACK
	{
		public short WorldID = -1;

		public long PersonID;

		public long Money;

		public int MapUnique = -1;

		public int BFID = -1;

		public NrCharPartInfo kPartInfo = new NrCharPartInfo();

		public long CharState;
	}
}
