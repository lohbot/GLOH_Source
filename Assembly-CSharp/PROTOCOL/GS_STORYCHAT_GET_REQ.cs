using System;

namespace PROTOCOL
{
	public class GS_STORYCHAT_GET_REQ
	{
		public long nPersonID;

		public byte nType;

		public int nPage;

		public int nPageSize;

		public long nFirstStoryChatID;

		public long nLastStoryChatID;

		public byte bNextRequest;
	}
}
