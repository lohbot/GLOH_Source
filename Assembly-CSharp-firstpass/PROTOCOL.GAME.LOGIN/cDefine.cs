using System;

namespace PROTOCOL.GAME.LOGIN
{
	internal class cDefine
	{
		public static int GET_PROTOCOL_ID(int type)
		{
			return (int)((long)type & (long)((ulong)-16777216));
		}

		public static int GET_PROTO_SUB_ID(int type)
		{
			return type & 16777215;
		}
	}
}
