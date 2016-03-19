using System;
using UnityEngine;

namespace NPatch
{
	internal class UnityAndroidChecker : IHWChecker
	{
		public bool IsWifiConnection
		{
			get
			{
				bool result = false;
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.ndoors.plugintest.TestJava", new object[0]))
				{
					using (AndroidJavaObject @static = androidJavaObject.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaObject.Call<bool>("IsWifiConnect", new object[]
						{
							@static
						});
					}
				}
				return result;
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
				long num = 0L;
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.ndoors.plugintest.TestJava", new object[0]))
				{
					num = androidJavaObject.Call<long>("GetSDCardCapacity", new object[0]);
				}
				return (uint)num;
			}
		}
	}
}
