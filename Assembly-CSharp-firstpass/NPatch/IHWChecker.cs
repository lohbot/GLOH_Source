using System;

namespace NPatch
{
	public interface IHWChecker
	{
		bool IsWifiConnection
		{
			get;
		}

		bool IsConnectedInternet
		{
			get;
		}

		uint FreeMemoryAsMegaByte
		{
			get;
		}

		uint FreeSpaceAsMegaByte
		{
			get;
		}
	}
}
