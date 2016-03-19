using System;

namespace NPatch
{
	internal class PCChecker : IHWChecker
	{
		public bool IsWifiConnection
		{
			get
			{
				return false;
			}
		}

		public bool IsConnectedInternet
		{
			get
			{
				return false;
			}
		}

		public uint FreeMemoryAsMegaByte
		{
			get
			{
				return 0u;
			}
		}

		public uint FreeSpaceAsMegaByte
		{
			get
			{
				return 0u;
			}
		}
	}
}
