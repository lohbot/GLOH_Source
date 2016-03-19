using System;

namespace PROTOCOL
{
	public class GS_COMMUNITY_MESSAGE_GUILD
	{
		public enum MESSAGE_TYPE
		{
			eMESSAGE_GUILDBOSS_START,
			eMESSAGE_GUILDBOSS_CLEAR,
			eMESSAGE_GUILDBOSS_CLEARFAIL
		}

		public int i32SubMessageType;

		public int i32Param1;

		public long i64Param2;
	}
}
