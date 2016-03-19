using System;

namespace PROTOCOL.BASE
{
	public class PacketHeader
	{
		public short size;

		public byte checksum;

		public int type;

		public short param;

		public PacketHeader()
		{
		}

		public PacketHeader(int _TypeID)
		{
			this.type = _TypeID;
			this.size = 0;
			this.param = 0;
		}

		public PacketHeader(Type _TypeOf, int _TypeID)
		{
			this.size = 0;
			this.type = _TypeID;
			this.param = 0;
		}
	}
}
