using System;
using System.IO;

namespace PROTOCOL
{
	public class ReceivePakcet
	{
		public static int DeserializePacket(byte[] rawdatas, int _Index, object _Object)
		{
			return TKMarshal.DeSerialize(rawdatas, _Index, _Object);
		}

		public static object DeserializePacket(byte[] rawdatas, int _Index, out int _Size, Type _Type)
		{
			return TKMarshal.DeSerializeType(rawdatas, _Index, out _Size, _Type);
		}

		public static int DesirialPacket(BinaryReader br, Type anytype)
		{
			return 0;
		}
	}
}
