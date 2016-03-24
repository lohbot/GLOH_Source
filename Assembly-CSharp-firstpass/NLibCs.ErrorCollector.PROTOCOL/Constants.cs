using System;

namespace NLibCs.ErrorCollector.PROTOCOL
{
	public static class Constants
	{
		public const ushort HEADER_SIZE = 5;

		public const ushort HEADER_EX_SIZE = 7;

		public const ushort PACKET_MAX_SIZE = 65535;

		public const byte CHK1 = 94;

		public const byte CHK2 = 124;

		public const int LENGTH_BUNDLEVERSION = 17;

		public const int LENGTH_MESSAGE = 129;

		public const int LENGTH_STACKTRACE = 2049;

		public const int LENGTH_DEVICEMODEL = 65;

		public const int LENGTH_DEVICEOS = 65;
	}
}
