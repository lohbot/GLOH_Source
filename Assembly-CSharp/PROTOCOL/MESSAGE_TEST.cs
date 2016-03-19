using System;
using System.Runtime.InteropServices;

namespace PROTOCOL
{
	public class MESSAGE_TEST
	{
		public int i32TestValue;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
		public char[] msg = new char[21];
	}
}
